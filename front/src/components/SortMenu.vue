<template>
  <div class="card flex justify-center ">
    <Select
      @change="(SelectedCourse) =>{Values.CourseId = SelectedCourse.value.code}"
      v-model="SelectedCourse"
      :options="coaches"
      showClear
      optionLabel="name"
      placeholder="Select a City"
      class="w-full md:w-56"
    />
     <Select
      @change="(SelectedCoach)=>{Values.CoachId = SelectedCoach.value.code}"
      :options="courses"
      v-model="SelectedCoach"
      showClear
      optionLabel="name"
      placeholder="Select a City"
      class="w-full md:w-56"
    />
    <DatePicker @value-change="(event)=>{handleDateChange(event)}" selectionMode="range" :manualInput="false" />

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
const Values = reactive({
  StartDate:'',
  EndDate:'',
  CourseId:'',
  CoachId:'',
  sort:''
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
const coaches = ref([{
  name:'1',
  code:'1'
}])
const courses = ref([{
  name:'1',
  code:'1'
}])

function handleDateChange(value:Date[]){
    if(Date.length == 2){
      Values.StartDate = value[0].toString();
      Values.EndDate = value[1].toString();
    }
}

watch(Values,(newValues)=>{
  emit('onChange',newValues)
  console.log(newValues);
})


</script>
