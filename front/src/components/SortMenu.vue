
<template>
    <div class="card flex justify-center">
        <Select v-model="selectedCoach" :options="coachData" optionLabel="label" placeholder="Select a City" class="w-full md:w-56" />
        <Button @click="Sort()">Sort</Button>
    </div>
</template>

<script setup lang="ts">
import { ref ,onMounted,defineModel} from "vue";
import axios from 'axios'
const coachData = ref();
const sortData = defineModel('sort');
const selectedCoach = ref();
const API_BASE_URL = 'https://localhost:7273';

const Sort = async () => {

    console.log(selectedCoach);
    const params = new URLSearchParams();

    if (selectedCoach.value.code) {
      params.append("CoachId", selectedCoach.value.code);
    }



    params.append("CoachId", selectedCoach.value.code);

    const response = await fetch(
      `${API_BASE_URL}/api/lesson/get-all?${params.toString()}`
    );

    if (!response.ok) throw new Error("Ошибка загрузки");

    sortData.value = await response.json();



};




async function GetCoachData(){
  await axios.get('https://localhost:7273/api/Coach')
  .then(async function(res){
    coachData.value = res.data.map(x => { return { code: x.id, label: x.name }});
  })
}

onMounted(async function(){
  await GetCoachData();
})
</script>
