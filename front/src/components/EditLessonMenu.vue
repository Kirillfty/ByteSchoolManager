<template>
  <div>
    <!-- Кнопка для открытия меню -->
    <Button label="Меню" @click="toggleMenu" aria-haspopup="true" aria-controls="overlay_menu" />

    <!-- Меню -->
    <TieredMenu id="overlay_menu" ref="menu" :model="menuItems" :popup="true" />

    <!-- Диалоги -->
    <Dialog v-model:visible="displayCoachDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Назначить замену</p>
      <template #footer>
        <Select
        v-model="selectedCoach"
        :options="coachData"
        optionLabel="label"
        placeholder="Выберите тренера"
        class="filter-select"
        />
        <Button label="Отправить" @click="EditCoachInLesson()"></Button>
        <Button label="Закрыть" @click="displayCoachDialog = false" />
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref,onMounted } from 'vue'
import axios from 'axios'
const selectedCoach = ref();
const menu = ref();
const coachData = ref();
const displayCoachDialog = ref(false)
const id = defineProps(['Id']);


const toggleMenu = (event) => {
  menu.value.toggle(event)
}
function EditCoachInLesson(){
    console.log(Number(id));
    console.log(selectedCoach.value);
    axios.patch('https://localhost:7273/api/Lesson/replace-coach/',{lessonId:id.Id,coachId:selectedCoach.value.code})
    .then(function(res){
        console.log('procces--'+res.status);
    })
}
const menuItems = ref([
  {
    label: 'Изменить',
    icon: 'pi pi-file-edit',
    items: [
      {
        label: 'Изменить тренера',
        icon: 'pi pi-file',
        command: () => {
          displayCoachDialog.value = true
        },
      }
    ],
  },
  {
    separator: true,
  }
])
async function GetCoachData() {
   await axios.get('https://localhost:7273/api/Coach')
     .then(async function (res) {
       coachData.value = res.data.map(x => { return { code: x.id, label: x.name } });
})

}
 onMounted(async()=>{
   await GetCoachData();
 })
</script>
