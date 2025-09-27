<template id="mar">
  <div class="container">
    <div class="header">
      <div class="menus">
        <Navigation></Navigation>
        <MenuCourse></MenuCourse>
      </div>
    </div>

    <!-- Main Content -->
    <div class="flex-1 bg-gray-50 flex" v-for="item in courseData" :key="item" id="card-container">
      <Card>
        <template #title>{{ item.title }} {{ removeSeconds(item.time) }}</template>
        <template #content>
          <p class="m-0">Студенты:{{ item.studentCount }}</p>
          <p class="m-0">Дата начала курса:{{ item.startDate }}</p>
          <p class="m-0">Дата конца курса:{{ item.endDate }}</p>
          <p class="m-0">Тренер:{{ item.coachId }}</p>
          <p class="m-0">Кол-во занятий:{{ item.lessonsCount }}</p>
          <div class="buttons">
            <ToggleMenu :Id="item.id"></ToggleMenu>
          </div>
        </template>
      </Card>
      
    </div>
  </div>
  
</template>

<script setup lang="ts">
import Navigation from '@/components/Navigation.vue';
import MenuCourse from '@/components/AddMenuCourse.vue';
import { ref, onMounted } from 'vue';
import axios from 'axios';
import Card from 'primevue/card';
import ToggleMenu from '@/components/ToggleMenu.vue';


let courseData = ref();
async function GetCourseData() {
  await axios.get('https://localhost:7273/api/Course')
    .then(function (res) {
      console.log(res.data);
      return courseData.value = res.data;
    })
}


function removeSeconds(timeStr: string) {
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
  flex-direction: column;
  padding: 1%;
}

.menus {
  width: 70%;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}
</style>