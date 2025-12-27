<template>
  <div class="card flex justify-content-center mt-5">
    <Button label="Управление списком студентов" icon="pi pi-users" @click="openModal" />

    <Dialog v-model:visible="visible" modal header="Управление студентами курса" :style="{ width: '50rem' }"
            :closable="!loading" @hide="resetForm">

      <!-- Блок загрузки -->
      <ProgressBar v-if="loadingStudents" mode="indeterminate" class="mb-4" />

      <div class="flex flex-column gap-2 mb-4">
        <label for="student-search" class="font-bold">Добавить студента из базы:</label>
        <div class="flex gap-2">
          <AutoComplete
            v-model="selectedStudentToAdd"
            auto-highlight
            :suggestions="filteredStudents"
            @complete="searchStudent"
            field="name"
            placeholder="Введите имя для поиска..."
            class="w-full"
            :disabled="loading || loadingStudents"
            dropdown
          />
          <Button
            label="Добавить"
            icon="pi pi-plus"
            @click="addStudentFromSearch"
            :disabled="!selectedStudentToAdd || loading || loadingStudents"
            severity="success"
          />
        </div>
        <small class="text-color-secondary">Выберите студента из базы данных</small>
      </div>

      <!-- Список добавленных студентов -->
      <div class="mb-4">
        <div class="flex align-items-center justify-content-between mb-2">
          <h4 class="m-0">Студенты курса ({{ students.length }})</h4>
          <Button
            v-if="students.length > 0"
            label="Очистить список"
            icon="pi pi-trash"
            @click="clearAllStudents"
            severity="danger"
            outlined
            :disabled="loading"
          />
        </div>

        <div v-if="students.length === 0" class="p-4 text-center border-round border-1 surface-border">
          <i class="pi pi-users text-4xl text-color-secondary mb-2"></i>
          <p class="text-color-secondary">Нет добавленных студентов</p>
        </div>

        <DataTable v-else :value="students" responsiveLayout="scroll" class="p-datatable-sm"
                   :loading="loading" stripedRows>
          <Column field="name" header="Имя студента" style="min-width: 200px">
            <template #body="slotProps">
              <div class="flex align-items-center gap-2">
                <i class="pi pi-user"></i>
                <span>{{ slotProps.data.name }}</span>
              </div>
            </template>
          </Column>

          <Column header="Статус" style="min-width: 250px">
            <template #body="slotProps">
              <Dropdown
                v-model="slotProps.data.courseStatus"
                :options="statusOptions"
                optionLabel="label"
                optionValue="value"
                placeholder="Выберите статус"
                class="w-full"
                :disabled="loading"
                @change="onStatusChange(slotProps.data)"
              />
            </template>
          </Column>

          <Column header="Действия" style="min-width: 100px">
            <template #body="slotProps">
              <Button
                icon="pi pi-trash"
                outlined
                rounded
                severity="danger"
                @click="removeStudent(slotProps.data)"
                :disabled="loading"
                v-tooltip.top="'Удалить из курса'"
              />
            </template>
          </Column>
        </DataTable>
      </div>

      <Divider />

      <template #footer>
        <div class="flex justify-content-between w-full">
          <div>
            <small class="text-color-secondary">
              Статусы: 0 - Активен, 1 - Академотпуск, 2 - Отчислен
            </small>
          </div>
          <div class="flex gap-2">
            <Button
              label="Отмена"
              icon="pi pi-times"
              @click="visible = false"
              :disabled="loading"
              severity="secondary"
              outlined
            />
            <Button
              label="Сохранить изменения"
              icon="pi pi-save"
              @click="addStudentsToCourse"
              :loading="loading"
              :disabled="loading || students.length === 0"
              severity="primary"
            />
          </div>
        </div>
      </template>
    </Dialog>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import axios from 'axios';
import { useToast } from 'primevue/usetoast';

const toast = useToast();

// --- Props ---
const props = defineProps({
  id: {
    type: Number,
    required: true
  },
  initialStudents: {
    type: Array,
    default: () => []
  }
});

// --- Emits ---
const emit = defineEmits(['students-updated']);

// --- Состояние ---
const visible = ref(false);
const selectedStudentToAdd = ref(null);
const filteredStudents = ref([]);
const loading = ref(false);
const loadingStudents = ref(false);

// Статусы для Dropdown (числовые значения)
const statusOptions = ref([
  { label: 'Активен', value: 0 },
  { label: 'Не активен', value: 1 },
  { label: 'Пауза', value: 2 },
  { label: 'Онлайн', value: 3 }
]);

// Список студентов в модальном окне
const students = ref([]);

// База данных студентов для поиска
const databaseOfStudents = ref([]);

// --- Вычисляемые свойства ---
const existingStudentIds = computed(() => {
  return students.value.map(s => s.studentId);
});

// --- Методы ---

// Поиск студентов
const searchStudent = (event) => {
  if (!event.query.trim().length) {
    filteredStudents.value = [];
  } else {
    const query = event.query.toLowerCase();
    filteredStudents.value = databaseOfStudents.value.filter((student) => {
      return student.name.toLowerCase().includes(query) ||
             (student.email && student.email.toLowerCase().includes(query));
    });
  }
};

// Добавление студента из поиска
const addStudentFromSearch = () => {
  if (!selectedStudentToAdd.value || typeof selectedStudentToAdd.value !== 'object') {
    showError('Выберите студента из списка');
    return;
  }

  const studentId = selectedStudentToAdd.value.id;

  // Проверяем, не добавлен ли студент уже
  if (existingStudentIds.value.includes(studentId)) {
    showWarn('Этот студент уже добавлен в курс');
    selectedStudentToAdd.value = null;
    filteredStudents.value = [];
    return;
  }

  // Добавляем студента с дефолтным статусом "Активен" (0)
  const newStudent = {
    studentId: studentId,
    courseStatus: 0, // По умолчанию активен
    name: selectedStudentToAdd.value.name || selectedStudentToAdd.value.fullName || 'Неизвестный студент'
  };

  students.value.push(newStudent);
  selectedStudentToAdd.value = null;
  filteredStudents.value = [];

  showSuccess('Студент добавлен в список');
};

// Удаление студента из списка
const removeStudent = (student) => {
  students.value = students.value.filter(s => s.studentId !== student.studentId);
  showSuccess('Студент удален из списка');
};

// Очистить всех студентов
const clearAllStudents = () => {
  students.value = [];
  showSuccess('Список студентов очищен');
};

// Изменение статуса
const onStatusChange = (student) => {
  console.log(`Статус студента ${student.name} изменен на: ${student.courseStatus}`);
};

// Загрузка всех студентов из базы данных
const getAllStudents = async () => {
  try {
    loadingStudents.value = true;
    const response = await axios.get('https://localhost:7273/api/Student');

    if (response.data && Array.isArray(response.data)) {
      databaseOfStudents.value = response.data.map(student => ({
        id: student.id || student.studentId,
        name: student.name || student.fullName || `${student.firstName} ${student.lastName}`,
        email: student.email
      }));
    } else {
      databaseOfStudents.value = [];
      console.warn('Некорректный формат данных студентов:', response.data);
    }
  } catch (error) {
    console.error('Ошибка при загрузке студентов:', error);
    showError('Не удалось загрузить список студентов');
  } finally {
    loadingStudents.value = false;
  }
};

// Загрузка текущих студентов курса
const loadCourseStudents = async () => {
  try {
    loading.value = true;
    const response = await axios.get(`https://localhost:7273/api/Course/${props.id}/students`);

    if (response.data && Array.isArray(response.data)) {
      students.value = response.data.map(student => ({
        studentId: student.studentId || student.id,
        courseStatus: student.courseStatus || 0,
        name: student.name || student.fullName || 'Неизвестный студент'
      }));
    }
  } catch (error) {
    console.error('Ошибка при загрузке студентов курса:', error);
    // Продолжаем работу с пустым списком
    students.value = [];
  } finally {
    loading.value = false;
  }
};

// Основной метод - добавление студентов на курс через PATCH
const addStudentsToCourse = async () => {
  if (students.value.length === 0) {
    showError('Добавьте хотя бы одного студента');
    return;
  }

  try {
    loading.value = true;

    // Подготовка данных в ТОЧНОМ формате, который ожидает API
    const requestData = {
      courseId: Number(props.id),
      students: students.value.map(student => ({
        studentId: Number(student.studentId),
        courseStatus: Number(student.courseStatus)
      }))
    };

    console.log('Отправляемые данные:', JSON.stringify(requestData, null, 2));

    const response = await axios.patch(
      'https://localhost:7273/api/Course/students',
      requestData,
      {
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        },
        timeout: 10000
      }
    );

    console.log('Ответ от сервера:', response);

    if (response.status === 200 || response.status === 204 || response.status === 201) {
      showSuccess('Студенты успешно добавлены на курс!');

      // Эмитим событие для обновления родительского компонента
      emit('students-updated', students.value);

      // Закрываем модальное окно через небольшую задержку
      setTimeout(() => {
        visible.value = false;
      }, 1500);
    } else {
      showError(`Неизвестный статус ответа: ${response.status}`);
    }
  } catch (error) {
    console.error('Ошибка при добавлении студентов:', error);

    let errorMessage = 'Произошла ошибка при сохранении';

    if (error.response) {
      // Сервер вернул ошибку
      console.error('Данные ошибки:', error.response.data);
      console.error('Статус ошибки:', error.response.status);

      if (error.response.data) {
        if (typeof error.response.data === 'string') {
          errorMessage = error.response.data;
        } else if (error.response.data.message) {
          errorMessage = error.response.data.message;
        } else if (error.response.data.title) {
          errorMessage = error.response.data.title;
        } else if (error.response.data.errors) {
          errorMessage = Object.values(error.response.data.errors).flat().join(', ');
        }
      }

      if (error.response.status === 400) {
        errorMessage = `Ошибка валидации: ${errorMessage}`;
      } else if (error.response.status === 404) {
        errorMessage = 'Курс не найден';
      } else if (error.response.status === 409) {
        errorMessage = 'Конфликт данных: ' + (errorMessage || 'Студент уже добавлен на курс');
      }
    } else if (error.request) {
      // Запрос был сделан, но ответ не получен
      errorMessage = 'Не удалось подключиться к серверу. Проверьте подключение к интернету.';
    } else {
      // Ошибка при настройке запроса
      errorMessage = `Ошибка при отправке запроса: ${error.message}`;
    }

    showError(errorMessage);
  } finally {
    loading.value = false;
  }
};

// Сброс формы
const resetForm = () => {
  selectedStudentToAdd.value = null;
  filteredStudents.value = [];
};

// Открытие модального окна
const openModal = async () => {
  visible.value = true;

  // Загружаем данные только если еще не загружали
  if (databaseOfStudents.value.length === 0) {
    await getAllStudents();
  }

  // Загружаем текущих студентов курса
  await loadCourseStudents();
};

// Вспомогательные методы для уведомлений
const showSuccess = (message) => {
  toast.add({
    severity: 'success',
    summary: 'Успех',
    detail: message,
    life: 3000
  });
};

const showError = (message) => {
  toast.add({
    severity: 'error',
    summary: 'Ошибка',
    detail: message,
    life: 5000
  });
};

const showWarn = (message) => {
  toast.add({
    severity: 'warn',
    summary: 'Внимание',
    detail: message,
    life: 3000
  });
};

// --- Наблюдатели ---
watch(visible, (newVal) => {
  if (newVal) {
    // При открытии окна проверяем, есть ли начальные студенты
    if (props.initialStudents && props.initialStudents.length > 0) {
      students.value = props.initialStudents.map(s => ({
        studentId: s.id,
        courseStatus: 0,
        name: s.name
      }));
    }
  }
});

// --- Хуки ---
onMounted(async () => {
  // Предварительная загрузка студентов (можно отключить для ленивой загрузки)
  // await getAllStudents();
});

// Экспортируем метод для открытия модального окна
defineExpose({
  openModal,
  refreshStudents: getAllStudents
});
</script>

<style scoped>
:deep(.p-datatable .p-datatable-tbody > tr > td) {
  padding: 0.5rem 1rem;
}

:deep(.p-button.p-button-success:disabled) {
  opacity: 0.6;
  cursor: not-allowed;
}

:deep(.p-autocomplete) {
  width: 100%;
}

:deep(.p-dialog-footer) {
  border-top: 1px solid #e5e7eb;
  padding-top: 1rem;
}
</style>
