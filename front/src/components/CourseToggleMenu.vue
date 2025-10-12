<template>
  <div>
    <!-- Кнопка для открытия меню -->
    <Button label="Меню" @click="toggleMenu" aria-haspopup="true" aria-controls="overlay_menu" />

    <!-- Меню -->
    <TieredMenu id="overlay_menu" ref="menu" :model="menuItems" :popup="true" />

    <!-- Диалоги -->
    <Dialog v-model:visible="displayCoachDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Изменение тренера по id</p>
      <template #footer>
        <input placeholder="id тренера" v-model="coachId" />
        <Button label="Отправить" @click="ReplaceCoahInCourse()"></Button>
        <Button label="Закрыть" @click="displayCoachDialog = false" />
      </template>
    </Dialog>

    <Dialog v-model:visible="displayDateOfLessonDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
      <p>Изменить дату проведения занятия</p>
      <template #footer>
        <Button label="Закрыть" @click="displayDateOfLessonDialog = false" />
      </template>
    </Dialog>

    <Dialog v-model:visible="displayDateOfStartCourseDialog" header="Настройки" :style="{ width: '50vw' }"
      :modal="true">
      <p>Изменить дату начала и конца курса</p>
      <input type="text" placeholder="Дата начала курса" v-model="courseDto.dateOfStartCourse">
      <input type="text" placeholder="Дата конца курса" v-model="courseDto.dateOfEndCourse">
       <Button label="Отправить" @click="ReplaceStartAndEndCourse()" />
        <Button label="Закрыть" @click="displayDateOfStartCourseDialog = false" />
      <template #footer>

      </template>
    </Dialog>

    <Dialog v-model:visible="displayDaysOfCourseDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
      <p>Изменить дни проведения занятий</p>
      <template #footer>
        <input type="text" placeholder="Дни недели" v-model="courseDto.days"/>
        <Button label="Отправить" @click="ReplaceDaysCourse()" />
        <Button label="Закрыть" @click="displayDaysOfCourseDialog = false" />
      </template>
    </Dialog>

    <Dialog v-model:visible="displayTimeOfLessonDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
      <p>Изменить время проведения занятия</p>
      <template #footer>
        <input type="text" placeholder="время проведения занятия" v-model="courseDto.timeOfLesson">
        <Button label="Отправить"  @click="ReplaceTimeOfLesson()"/>
        <Button label="Закрыть" @click="displayTimeOfLessonDialog = false" />
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
const props = defineProps(['Id'])
const coachId = ref('')
const menu = ref()
const displayCoachDialog = ref(false)
const displayDateOfLessonDialog = ref(false)
const displayDateOfStartCourseDialog = ref(false)
const displayTimeOfLessonDialog = ref(false)
const displayDaysOfCourseDialog = ref(false)
let courseDto = ref({
     days: '',
     timeOfLesson: '',
     dateOfStartCourse: '',
     dateOfEndCourse: '',
     title: '',
     coachId: ''
});
const ReplaceStartAndEndCourse=()=>{
  axios.patch('https://localhost:7273/api/Course/dates',{courseId:props.Id,
    startDate:courseDto.value.dateOfStartCourse,
    endDate\:courseDto.value.dateOfEndCourse
  })
  .catch(function(err){
    console.log(err);
    alert(err);
  })
  .then(function(res){
    console.log(res)
    alert('ok');
  })
}
const ReplaceTimeOfLesson=()=>{
  axios.patch('https://localhost:7273/api/Course/time',{courseId:props.Id,time:courseDto.value.timeOfLesson})
  .then(function(res){
    console.log(res)
    alert('ok');
  })
  .catch(function(err){
    if(err.status === 400){
      alert('неправильный формат даты надо(часы:минуты:секунды)\n или же это время совпадает с текугим временем занятия')
    }
  })

}
const ReplaceDaysCourse = ()=>{
  axios.patch('https://localhost:7273/api/Course/days',{courseId:props.Id,days:stringToNumbers(courseDto.value.days)})
   .then(function(res){
    console.log(res)
    alert('ok');
  })
  .catch(function(err){
    console.error('Error:', err)
    alert('Error occurred')
  })
}
function stringToNumbers(str:string) {
    return str.split(',').map(Number);
}
const ReplaceCoahInCourse = () => {
  axios
    .patch('https://localhost:7273/api/Course/coach', {
      courseId: props.Id,
      coachId: coachId.value,
    })
    .then((res) => {
      console.log(res)
      alert('ok')
    })
    .catch((error) => {
      console.error('Error:', error)
      alert('Error occurred')
    })
}

const toggleMenu = (event) => {
  menu.value.toggle(event)
}

const menuItems = ref([
  {
    label: 'Изменить',
    icon: 'pi pi-file-edit',
    items: [
      {
        label: 'Дни проведения курса',
        icon: 'pi pi-file',
        command: () => {
          displayDaysOfCourseDialog.value = true
        },
      },
      {
        label: 'Изменить тренера',
        icon: 'pi pi-file',
        command: () => {
          displayCoachDialog.value = true
        },
      },
      {
        label: 'Дату начала курса и конца курса',
        icon: 'pi pi-file',
        command: () => {//////////////////////////
          displayDateOfStartCourseDialog.value = true
        },
      },
      {
        label: 'Время проведения занятия',
        icon: 'pi pi-file',
        command: () => {
          displayTimeOfLessonDialog.value = true
        },
      },
      {
        label: 'Изменить дни недели',
        icon: 'pi pi-file',
        command: () => {
          displayTimeOfLessonDialog.value = true
        },
      },
    ],
  },
  {
    separator: true,
  },
  {
    label: 'Удалить',
    icon: 'pi pi-trash',
    command: async () => {
      await axios.delete('https://localhost:7273/api/Course/' + props.Id).then((res) => {
        console.log(props.Id)
        location.reload()
        return res
      })
    },
  },
])
</script>
