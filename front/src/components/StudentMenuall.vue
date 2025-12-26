<template>
  <div class="card flex justify-content-center mt-5">
    <Button label="Управление списком студентов" icon="pi pi-users" @click="visible = true" />

    <Dialog v-model:visible="visible" modal header="Список студентов" :style="{ width: '50rem' }">

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
          />
          <Button label="Добавить" icon="pi pi-plus" @click="addStudent" :disabled="!selectedStudentToAdd" />
        </div>
      </div>

      <DataTable :value="students" responsiveLayout="scroll" class="p-datatable-sm">
        <Column field="name" header="Имя студента"></Column>

        <Column header="Статус">
          <template #body="slotProps">
            <Dropdown
              v-model="slotProps.data.status"
              :options="statuses"
              optionLabel="label"
              optionValue="value"
              placeholder="Выберите статус"
              class="w-full md:w-10rem"
            />
          </template>
        </Column>

        <Column header="Действия" :exportable="false" style="min-width:4rem">
          <template #body="slotProps">
            <Button
              icon="pi pi-trash"
              outlined
              rounded
              severity="danger"
              @click="confirmDelete(slotProps.data)"
            />
          </template>
        </Column>
      </DataTable>

      <template #footer>
        <Button label="Закрыть" icon="pi pi-check" @click="visible = false" autofocus />
      </template>
    </Dialog>
  </div>
</template>

<script setup>
import { ref } from 'vue';

// --- Состояние ---
const visible = ref(false);
const selectedStudentToAdd = ref(null);
const filteredStudents = ref([]);

// Статусы для Dropdown
const statuses = ref([
    { label: 'Активен', value: 'active' },
    { label: 'В академическом отпуске', value: 'academic' },
    { label: 'Отчислен', value: 'dropped' }
]);

// Список студентов в модальном окне
const students = ref([
    { id: 1, name: 'Анна Иванова', status: 'active' },
    { id: 2, name: 'Иван Петров', status: 'dropped' }
]);

// "Заданный список" для добавления
const databaseOfStudents = [
    { name: 'Елена Козлова' },
    { name: 'Дмитрий Смирнов' },
    { name: 'Ольга Николаева' },
    { name: 'Артем Соколов' }
];

// --- Методы ---

// Поиск по списку при вводе
const searchStudent = (event) => {
    setTimeout(() => {
        if (!event.query.trim().length) {
            filteredStudents.value = [...databaseOfStudents];
        } else {
            filteredStudents.value = databaseOfStudents.filter((s) => {
                return s.name.toLowerCase().includes(event.query.toLowerCase());
            });
        }
    }, 200);
};

// Добавление студента из списка
const addStudent = () => {
    if (selectedStudentToAdd.value) {
        const newStudent = {
            id: Date.now(),
            // Проверка, ввел ли пользователь имя вручную или выбрал объект
            name: typeof selectedStudentToAdd.value === 'string'
                  ? selectedStudentToAdd.value
                  : selectedStudentToAdd.value.name,
            status: 'active'
        };
        students.value.push(newStudent);
        selectedStudentToAdd.value = null; // Сброс поля
    }
};

// Удаление студента
const confirmDelete = (student) => {
    students.value = students.value.filter(s => s.id !== student.id);
};
</script>

<style scoped>
/* Дополнительные стили, если нужны */
</style>
