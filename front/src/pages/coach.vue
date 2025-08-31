<template>
  <div class="flex items-center justify-between max-w-2xl m-auto">
    <Button
      v-for="(date, index) in getWeekDates()"
      :key="index"
      fluid
      size="small"
      @click.prevent="GetLessonInDay(date.getDay())"
      severity="secondary">
      {{dayjs(date).format('D MMM')}}<Badge>2</Badge>
    </Button>
  </div>
  <div class="lessons" v-for="index in lessonData" :key="index.id">
    <div class="lesson" @click.prevent="goToLessonPage(index.id)">
      <h1 id="name-lesson">{{ index.title }}</h1>
      <h1 id="time-lesson">{{ getHoursMinutes(index.time) }}</h1>
      <div class="student-lesson-cont">
        <p id="student-lesson">{{ index.students }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import axios from 'axios'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

import dayjs from 'dayjs'
import 'dayjs/locale/ru' // Подключаем русскую локаль
import weekday from 'dayjs/plugin/weekday' // Для корректного определения недели

// Настраиваем day.js
dayjs.extend(weekday)
dayjs.locale('ru')

function getWeekDates(startFromMonday = true) {
  const today = new Date()
  const dayOfWeek = today.getDay() // 0 (воскресенье) - 6 (суббота)

  // Вычисляем смещение до понедельника
  const mondayOffset = startFromMonday ? (dayOfWeek === 0 ? -6 : 1 - dayOfWeek) : -dayOfWeek

  const monday = new Date(today)
  monday.setDate(today.getDate() + mondayOffset)

  const weekDates: Date[] = []
  for (let i = 0; i < 6; i++) {
    const date = new Date(monday)
    date.setDate(monday.getDate() + i)
    weekDates.push(date)
  }

  return weekDates
}

let lessonData = ref('')
let router = useRouter()

function goToLessonPage(id) {
  router.push('/lesson-' + id)
}
function GetLessonInDay(NumberOfDay) {
  lessonData.value = null
  const accessToken = localStorage.getItem('accessToken') as string
  console.log(accessToken)
  axios
    .get('https://localhost:7273/api/lesson/by-day/' + NumberOfDay, {
      headers: { Authorization: 'Bearer ' + accessToken },
    })
    .then(function (res) {
      lessonData.value = res.data
      return lessonData
    })
}
</script>

<style scoped></style>
