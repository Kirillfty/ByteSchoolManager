<template id="mar">
  <div class="container">
    <div class="header">
      <Navigation></Navigation>
      <InputText type="text" v-model="value" />
      <MenuCourse></MenuCourse>
    </div>

    <!-- Main Content -->
    <div class="flex-1 bg-gray-50 flex" v-for="item in courseData" :key="item" id="card-container">
      <Card>
        <template #title>{{ item.title }} {{ removeSeconds(item.time) }}</template>
        <template #content>
          <p class="m-0">Студенты:{{ item.studentCount }}</p>
          <SplitButton label="Изменить" @click="save" :model="items" />
        </template>
        
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import Navigation from '@/components/Navigation.vue';
import MenuCourse from '@/components/AddMenuCourse.vue';
import InputText from 'primevue/inputtext';
import SplitButton from 'primevue/splitbutton';
import { ref, onMounted } from 'vue';
import axios from 'axios';
import Card from 'primevue/card';

const save = () => {
  alert('first');
};


const items = ref([
  {
    label: 'Удалить',
    icon: 'pi pi-trash',
    command: () => {
     
      
    }
  },
  {
    separator: true
  }
]);
let courseData = ref('');
async function GetCourseData() {
  await axios.get('https://localhost:7273/api/Course')
    .then(function (res) {
      console.log(res.data);
      return courseData.value = res.data;
    })
}


function removeSeconds(timeStr) {
  return timeStr ? timeStr.split(':').slice(0, 2).join(':') : '';
};


onMounted(async () => {
  await GetCourseData();
})




</script>

<style scoped>
#card-container {
  display: flex;
  justify-content: center;
  align-items: center;
}

.header{
  display: flex;
  justify-content: center;
  align-items: center;
  width:100%;
}
</style>