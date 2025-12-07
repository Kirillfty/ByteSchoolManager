<template>
  <div class="filters-container">
    <div class="filters-grid">
      <Select
        v-model="selectedCoach"
        :options="coachData"
        optionLabel="label"
        placeholder="Выберите тренера"
        class="filter-select"
      />

      <Select
        v-model="selectedCourse"
        :options="courseData"
        optionLabel="label"
        placeholder="Выберите курс"
        class="filter-select"
      />

      <div class="date-inputs">
        <span class="p-float-label">
          <Calendar
            v-model="startDate"
            id="startDate"
            dateFormat="dd.mm.yy"
            placeholder="Дата начала"
            :showIcon="true"
            class="date-input"
          />
          <label for="startDate">Дата начала</label>
        </span>

        <span class="p-float-label">
          <Calendar
            v-model="endDate"
            id="endDate"
            dateFormat="dd.mm.yy"
            placeholder="Дата окончания"
            :showIcon="true"
            class="date-input"
          />
          <label for="endDate">Дата окончания</label>
        </span>
      </div>

      <div class="checkbox-container">
        <Checkbox
          v-model="checkbox"
          :binary="true"
          inputId="sortDesc"
        />
        <label for="sortDesc" class="checkbox-label">По убыванию</label>
      </div>

      <Button
        @click="Sort()"
        icon="pi pi-sort-alt"
        class="sort-button"
        :disabled="isSorting"
      >
        <span v-if="isSorting">
          <i class="pi pi-spinner pi-spin" style="margin-right: 0.5rem"></i>
          Сортировка...
        </span>
        <span v-else>Сортировать</span>
      </Button>
    </div>

    <!-- Сообщения об ошибках -->
    <div v-if="errorMessage" class="error-message">
      <i class="pi pi-exclamation-triangle" style="margin-right: 0.5rem"></i>
      {{ errorMessage }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import axios from 'axios'
import Select from 'primevue/select';
import Button from 'primevue/button';
import Calendar from 'primevue/calendar';
import Checkbox from 'primevue/checkbox';

const checkbox = ref(false);
const startDate = ref();
const endDate = ref();
const coachData = ref();
const courseData = ref();
const sortData = defineModel('sort');
const selectedCoach = ref();
const selectedCourse = ref();
const isSorting = ref(false);
const errorMessage = ref('');

const API_BASE_URL = 'https://localhost:7273';

const Sort = async () => {
  // Сбрасываем сообщение об ошибке
  errorMessage.value = '';
  isSorting.value = true;

  try {
    const params = new URLSearchParams();

    // Проверка выбора курса
    if (!selectedCourse.value?.code) {
      errorMessage.value = 'Пожалуйста, выберите курс';
      isSorting.value = false;
      return;
    }

    // Проверка и добавление параметров дат
    if (startDate.value && endDate.value) {
      // Проверяем, что начальная дата раньше конечной
      const start = new Date(startDate.value);
      const end = new Date(endDate.value);

      if (start > end) {
        errorMessage.value = 'Дата начала не может быть позже даты окончания';
        isSorting.value = false;
        return;
      }

      params.append("StartDate", start.toISOString().split('T')[0]);
      params.append("EndDate", end.toISOString().split('T')[0]);
    } else if (startDate.value || endDate.value) {
      // Если указана только одна дата
      if (startDate.value) {
        params.append("StartDate", new Date(startDate.value).toISOString().split('T')[0]);
      }
      if (endDate.value) {
        params.append("EndDate", new Date(endDate.value).toISOString().split('T')[0]);
      }
    }

    // Добавляем ID тренера если выбран
    if (selectedCoach.value?.code) {
      params.append("CoachId", selectedCoach.value.code);
    }

    // Добавляем ID курса (обязательный параметр)
    params.append("CourseId", selectedCourse.value.code);

    // Сортировка: если checkbox true - по убыванию, если false - по возрастанию
    params.append("IsDescending", checkbox.value.toString());

    const response = await fetch(
      `${API_BASE_URL}/api/lesson/get-all?${params.toString()}`
    );

    if (!response.ok) {
      throw new Error("Ошибка загрузки данных");
    }

    const data = await response.json();

    // Если API не сортирует, делаем сортировку на клиенте
    if (data && Array.isArray(data)) {
      // Ищем поле с датой для сортировки
      const hasDateField = data.some(item => item.date || item.startDate || item.createdDate);

      if (hasDateField) {
        data.sort((a, b) => {
          // Пытаемся найти поле с датой
          const getDate = (item) => {
            return item.date || item.startDate || item.createdDate || item.Date;
          };

          const dateA = new Date(getDate(a)).getTime();
          const dateB = new Date(getDate(b)).getTime();

          // checkbox true = по убыванию, false = по возрастанию
          return checkbox.value ? dateB - dateA : dateA - dateB;
        });
      }
    }

    sortData.value = data;

  } catch (error) {
    console.error('Ошибка при сортировке:', error);
    errorMessage.value = 'Произошла ошибка при загрузке данных';

    // Скрываем сообщение об ошибке через 5 секунд
    setTimeout(() => {
      errorMessage.value = '';
    }, 5000);

  } finally {
    isSorting.value = false;
  }
};

async function GetCoachData() {
  await axios.get('https://localhost:7273/api/Coach')
    .then(async function (res) {
      coachData.value = res.data.map(x => { return { code: x.id, label: x.name } });
    })
}
async function GetCourseData() {
  await axios.get('https://localhost:7273/api/Course')
    .then(function (res) {
      console.log(res.data);
       courseData.value = res.data.map(x => { return { code: x.id, label: x.title } });
    })
}
onMounted(async function () {
  await GetCoachData();
  await GetCourseData();
})
</script>

<style scoped>
.filters-container {
  padding: 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 12px;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.filters-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  align-items: end;
}

.filter-select {
  width: 100%;
}

.filter-select :deep(.p-dropdown) {
  width: 100%;
  background: rgba(255, 255, 255, 0.95);
  border: 2px solid transparent;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.filter-select :deep(.p-dropdown:hover) {
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.filter-select :deep(.p-dropdown:not(.p-disabled).p-focus) {
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.2);
}

.date-inputs {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.date-input {
  width: 100%;
}

.date-input :deep(.p-calendar) {
  width: 100%;
}

.date-input :deep(.p-inputtext) {
  width: 100%;
  background: rgba(255, 255, 255, 0.95);
  border: 2px solid transparent;
  border-radius: 8px;
  padding: 0.75rem;
  transition: all 0.3s ease;
}

.date-input :deep(.p-inputtext:hover) {
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.date-input :deep(.p-inputtext:focus) {
  outline: none;
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.2);
}

.p-float-label {
  position: relative;
}

.p-float-label label {
  position: absolute;
  left: 0.75rem;
  top: 50%;
  transform: translateY(-50%);
  color: #6b7280;
  transition: all 0.3s ease;
  pointer-events: none;
  background: white;
  padding: 0 0.25rem;
  border-radius: 4px;
  font-size: 0.875rem;
}

.p-float-label :deep(.p-calendar.p-focus) ~ label,
.p-float-label :deep(.p-inputtext.p-filled) ~ label {
  top: -0.5rem;
  font-size: 0.75rem;
  color: #6366f1;
}

.checkbox-container {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem;
  background: rgba(255, 255, 255, 0.95);
  border-radius: 8px;
  height: fit-content;
}

.checkbox-container :deep(.p-checkbox .p-checkbox-box) {
  border: 2px solid #d1d5db;
  border-radius: 6px;
  transition: all 0.3s ease;
}

.checkbox-container :deep(.p-checkbox .p-checkbox-box.p-highlight) {
  background: #6366f1;
  border-color: #6366f1;
}

.checkbox-container :deep(.p-checkbox:not(.p-checkbox-disabled) .p-checkbox-box:hover) {
  border-color: #6366f1;
}

.checkbox-label {
  font-weight: 500;
  color: #374151;
  cursor: pointer;
  user-select: none;
}

.sort-button {
  background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
  border: none;
  border-radius: 8px;
  padding: 0.875rem 1.5rem;
  font-weight: 600;
  color: white;
  transition: all 0.3s ease;
  height: fit-content;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  min-width: 120px;
}

.sort-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(99, 102, 241, 0.3);
}

.sort-button:active:not(:disabled) {
  transform: translateY(0);
}

.sort-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.error-message {
  margin-top: 1rem;
  padding: 0.75rem 1rem;
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.3);
  border-radius: 8px;
  color: #dc2626;
  font-size: 0.875rem;
  display: flex;
  align-items: center;
  animation: slideIn 0.3s ease;
}

/* Адаптивность */
@media (max-width: 768px) {
  .filters-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .date-inputs {
    grid-template-columns: 1fr;
  }

  .filters-container {
    padding: 1rem;
  }
}

/* Анимации */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.filters-grid > * {
  animation: fadeIn 0.5s ease-out forwards;
}

.filters-grid > *:nth-child(1) { animation-delay: 0.1s; }
.filters-grid > *:nth-child(2) { animation-delay: 0.2s; }
.filters-grid > *:nth-child(3) { animation-delay: 0.3s; }
.filters-grid > *:nth-child(4) { animation-delay: 0.4s; }
.filters-grid > *:nth-child(5) { animation-delay: 0.5s; }
</style>
