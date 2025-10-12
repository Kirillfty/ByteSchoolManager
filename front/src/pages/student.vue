<template>
  <div class="container">
    <Navigation></Navigation>
    <!-- Main Content -->
    <div class="flex-1 bg-gray-50 flex" v-for="item in studentsData" :key="item">
      <p>{{item.id}}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import Navigation from '@/components/Navigation.vue';
import axios from 'axios';
import {ref,Mounted, onMounted} from 'vue'
let studentsData = ref('');

async function GetAllStudets(){
    await axios.get('https://localhost:7273/api/Student')
    .then(function(res){
      console.log(res.data);
      return studentsData.value = res.data;
    })
}
onMounted(async()=>{
  await GetAllStudets();
})
</script>

<style scoped>
.container {
  width: 100%;
  display: flex;
  justify-content: space-between;
}


</style>
