<template>
  <div class="container">
    <div class="navs">
      <Navigation></Navigation>
      <AddStudentMenu></AddStudentMenu>
    </div>
    <!-- Main Content -->
    <div class="flex-1 flex" v-for="item in studentsData" :key="item" id="card-container">
      <Card>
        <template #title>{{ item.name }}</template>
        <template #content>
          <p class="m-0">id:{{ item.id }}</p>

          <div class="buttons">
            <EditStudenttoggleMenu :Id="item.id"></EditStudenttoggleMenu>
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import EditStudenttoggleMenu from '@/components/EditStudenttoggleMenu.vue';
import Navigation from '@/components/Navigation.vue';
import Card from 'primevue/card';
import axios from 'axios';
import { ref, onMounted } from 'vue'
import AddStudentMenu from '@/components/AddStudentMenu.vue';
const studentsData = ref('');

async function GetAllStudets() {
  await axios.get('https://localhost:7273/api/Student')
    .then(function (res) {
      console.log(res.data);
      return studentsData.value = res.data;
    })
}
onMounted(async () => {
  await GetAllStudets();
})
</script>

<style scoped>
.container {
  width: 100%;
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}
.navs{
  display:flex;
  justify-content: center;
}
</style>
