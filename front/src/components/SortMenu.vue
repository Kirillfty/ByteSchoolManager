<template>
  <div class="card">
    <Select
      @change="(SelectedCourse) =>{Values.CourseId = SelectedCourse.value.code}"
      v-model="SelectedCourse"
      :options="coaches.course"
      showClear
      optionLabel="name"
      placeholder="Select a City"
      class="w-full md:w-56"
    />
     <Select
      @change="(SelectedCoach)=>{Values.CoachId = SelectedCoach.value.code}"
      :options="coaches.coach"
      v-model="SelectedCoach"
      showClear
      optionLabel="name"
      placeholder="Select a City"
      class="w-full md:w-56"
    />
    <DatePicker v-model="dates" @value-change="(dates)=>{saveDates(dates)}" selectionMode="range" :manualInput="false" />

    <Select
      @change="(SelectedSort)=>{Values.sort = SelectedSort.value.code}"
      :options="sorts"
      v-model="SelectedSort"
      showClear
      optionLabel="name"
      placeholder="Select a City"
      class="w-full md:w-56"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive, ref , watch} from 'vue'
const emit = defineEmits(['onChange']);
const SelectedCourse = ref('');
const SelectedCoach = ref('');
const SelectedSort = ref('');
const dates = ref();
const Values = reactive({
  StartDate:'',
  EndDate:'',
  CourseId:null,
  CoachId:null,
  sort:null
});
const sorts = ref([{
  name:'По Тренеру',
  code: 'coach'
},
{
  name:'По Курсу',
  code:'course'
},
{
  name: 'По Дате',
  code:'date'
}
])

const coaches = defineProps(['coach','course']);

const saveDates = () => {
  if (dates.value && dates.value.length === 2) {
    Values.StartDate = dates.value[0].toISOString().split('T')[0]; // Год-месяц-день
    Values.EndDate = dates.value[1].toISOString().split('T')[0];
    console.log(Values);
    // Теперь startDate и endDate в формате 'YYYY-MM-DD'
  }
};
watch(Values,(newValues)=>{
  emit('onChange',newValues)
  console.log(newValues);
})


</script>
<style scoped>
.card{
  display: flex;
  justify-content: center;
  flex-direction: column;
}
</style>
