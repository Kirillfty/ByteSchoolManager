<template id="mar">
  <div class="container">
    <div class="header">
      <div class="nav">
        <Navigation></Navigation>
        <MenuCourse></MenuCourse>
      </div>
      <SortMenu v-model:sort="courseData" :coachData="coachData"></SortMenu>
    </div>

    <!-- Main Content -->
      <div id="message" v-if="courseData == ''">
        <p style="color:red;text-align: center;">loading...</p>
      </div>
      <div v-for="item in courseData" :key="item" id="card-container">
        <Card id="card">
          <template #title>{{ item.id }} {{ removeSeconds(item.dateAndTime) }}</template>
          <template #content>
            <p class="m-0">Студенты:{{ item.students }}</p>
            <p class="m-0">id тренера:{{ item.coachId }}</p>
            <div class="content-button">
              <EditLessonMenu :Id="item.id" :coachData="coachData"></EditLessonMenu>
              <Button label="Удалить" @click="Delete" :model="item" />
            </div>
          </template>

        </Card>
      </div>


  </div>
</template>

<script setup lang="ts">
import Navigation from '@/components/Navigation.vue';
import MenuCourse from '@/components/AddMenuCourse.vue';
import Button from 'primevue/button';
import { ref, onMounted } from 'vue';
import axios from 'axios';
import Card from 'primevue/card';
import SortMenu from '@/components/SortMenu.vue';

const coachData = ref();
async function GetCoachData() {
  await axios.get('https://localhost:7273/api/Coach')
    .then(async function (res) {
      coachData.value = res.data.map(x => { return { code: x.id, label: x.name } });
    })
}
onMounted(async()=>{
  await GetCoachData();
})

function Delete() {
  alert('delete');
}
const courseData = ref('');

async function GetCourseData() {
  await axios.get('https://localhost:7273/api/Lesson/get-all')
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




.header {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  width: 100%;
}

.nav {
  width: 55%;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
