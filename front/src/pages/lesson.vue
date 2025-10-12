<template id="mar">
  <div class="container">
    <div class="header">
      <Navigation></Navigation>

      <MenuCourse></MenuCourse>
    </div>

    <!-- Main Content -->
    <div class="flex-1 bg-gray-50 flex" v-for="item in courseData" :key="item" id="card-container">
      <Card>
        <template #title>{{ item.id }} {{ removeSeconds(item.dateAndTime) }}</template>
        <template #content>
          <p class="m-0">Студенты:{{ item.students }}</p>
          <p class="m-0">id тренера:{{ item.coachId }}</p>
          <Button label="Изменить" @click="Edit" :model="items" />
          <Button label="Удалить" @click="Delete" :model="items" />
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

function Edit(){
  alert('edit');
}
function Delete(){
  alert('delete');
}
let courseData = ref('');
async function GetCourseData() {
  await axios.get('https://localhost:7273/api/Lesson/get-all')
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
