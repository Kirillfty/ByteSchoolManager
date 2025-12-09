<template id="mar">
  <div class="container">
    <div class="header">
      <div class="nav">
        <Navigation></Navigation>
        <MenuCourse></MenuCourse>
      </div>
      <SortMenu v-model:sort="courseData"></SortMenu>
    </div>

    <!-- Main Content -->

      <div v-for="item in courseData" :key="item" id="card-container">
        <Card>
          <template #title>{{ item.id }} {{ removeSeconds(item.dateAndTime) }}</template>
          <template #content>
            <p class="m-0">Студенты:{{ item.students }}</p>
            <p class="m-0">id тренера:{{ item.coachId }}</p>
            <Button label="Изменить" @click="Edit" :model="item" />
            <Button label="Удалить" @click="Delete" :model="item" />
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


function Edit() {
  alert('edit');
}
function Delete() {
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

function removeSeconds(timeStr: string) {
  return timeStr ? timeStr.split(':').slice(0, 2).join(':') : '';
};


onMounted(async () => {
  await GetCourseData();
})




</script>

<style scoped>
#card-container {
  width: 100%;
  display: flex;
  justify-content: center;

}




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
