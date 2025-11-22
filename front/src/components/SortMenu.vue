
<template>
    <div class="card flex justify-center">
        <Toast />
        <SplitButton :model="items" />
    </div>
    <Dialog v-model:visible="coachDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Сортировка по id тренера</p>
      <template #footer>
        <div class="container">
          <div class="inputs">
            <input placeholder="id тренера" v-model="id" />
          </div>
          <div class="buttons">
            <Button label="✓" @click="SortByCoachId()" />
            <Button label="⨉" @click="coachDialog = false" />
          </div>
        </div>
      </template>
    </Dialog>
    <Dialog v-model:visible="courseDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Сортировка по id тренера</p>
      <template #footer>
        <div class="container">
          <div class="inputs">
            <input placeholder="id курса" v-model="id" />
          </div>
          <div class="buttons">
            <Button label="✓" @click="SortByCourseId()" />
            <Button label="⨉" @click="courseDialog = false" />
          </div>
        </div>
      </template>
    </Dialog>
    <Dialog v-model:visible="startdialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Сортировка по начальной дате</p>
      <template #footer>
        <div class="container">
          <div class="inputs">
            <input placeholder="дата в формате(год-месяц-день)" v-model="date" />
          </div>
          <div class="buttons">
            <Button label="✓" @click="SortByStartDate()" />
            <Button label="⨉" @click="startdialog = false" />
          </div>
        </div>
      </template>
    </Dialog>
    <Dialog v-model:visible="endDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Сортировка по конечной дате</p>
      <template #footer>
        <div class="container">
          <div class="inputs">
            <input placeholder="дата в формате(год-месяц-день)" v-model="date" />
          </div>
          <div class="buttons">
            <Button label="✓" @click="SortByEndDate()" />
            <Button label="⨉" @click="endDialog = false" />
          </div>
        </div>
      </template>
    </Dialog>
</template>

<script setup lang="ts">
import { useToast } from "primevue/usetoast";
import {defineModel,ref} from 'vue'
import axios from 'axios';
let id = ref('');
let date = ref('');
const model = defineModel('sortData');
let coachDialog = ref(false);
let courseDialog = ref(false);
let startdialog = ref(false);
let endDialog = ref(false);
const items = [
    {
        label: 'По дате(по убыванию)',
        command: () => {
            axios.get('https://localhost:7273/api/Lesson/get-all?Desc=true')
            .then(function(res){
              model.value = res.data;
            })
        }
    },
    {
        label: 'По id тренера',
        command: () => {
            coachDialog.value = true;
        }
    },
    {
        label: 'По id курса',
        command: () => {
            courseDialog.value = true;
        }
    },
    {
        label: 'По дате начала проведения занятий',
        command: () => {
            startdialog.value = true;
        }
    },
    {
        label: 'По дате конца проведения занятий',
        command: () => {
            endDialog.value = true;
        }
    },
    {
        separator: true
    }
];

function SortByCoachId(){
    axios.get('https://localhost:7273/api/Lesson/get-all?CoachId='+Number(id.value))
            .then(function(res){
              model.value = res.data;
    })
}
function SortByCourseId(){
    axios.get('https://localhost:7273/api/Lesson/get-all?CourseId='+Number(id.value))
            .then(function(res){
              model.value = res.data;
    })
}
function SortByStartDate(){
    axios.get('https://localhost:7273/api/Lesson/get-all?StartDate='+date.value)
            .then(function(res){
              model.value = res.data;
    })
}
function SortByEndDate(){
    axios.get('https://localhost:7273/api/Lesson/get-all?EndDate='+date.value)
            .then(function(res){
              model.value = res.data;
    })
}

//https://localhost:7273/api/Lesson/get-all?StartDate=
</script>
