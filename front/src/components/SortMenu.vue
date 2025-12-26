<template>
  <div class="filters-container">
    <div class="filters-grid">
      <Select
        v-model="selectedCoach"
        :options="coachData.coachData"
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
const coachData = defineProps(['coachData']);
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


async function GetCourseData() {
  await axios.get('https://localhost:7273/api/Course')
    .then(function (res) {
      console.log(res.data);
       courseData.value = res.data.map(x => { return { code: x.id, label: x.title } });
    })
}
onMounted(async function () {

  await GetCourseData();
})
</script>

<style scoped>
  .filters-container{
    width:100%;
    display: flex;
    justify-content: center;
    align-items: center;
  }
.filters-grid{
  width:40%;
  display: flex;
  justify-content: center;
  flex-direction: column;
}
.sort-button{
  width:100%;
}

</style>
