<template>
  <div class="card flex justify-center">
    <Button type="button" label="..." @click="toggle" aria-haspopup="true" aria-controls="overlay_tmenu" />
    <TieredMenu ref="menu" id="overlay_tmenu" :model="items" popup />
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import axios from 'axios'

const emit = defineEmits(['course-deleted'])

const props = defineProps(['Id'])
const menu = ref();
const items = ref([
  {
    label: 'Изменить',
    icon: 'pi pi-file-edit',
    items: [
      {
        label: 'Дату проведения занятия',
        icon: 'pi pi-file'
      },
      {
        label: 'Изменить тренера',
        icon: 'pi pi-file'
      },
      {
        label: 'Дату начала курса',
        icon: 'pi pi-file'
      },
      {
        label: 'Изменить дату конца курса',
        icon: 'pi pi-file'
      },
      {
        label: 'Время проведения занятия',
        icon: 'pi pi-file'
      }
    ]
  },
  {
    separator: true
  },
  {
    label: 'Удалить',
    icon: 'pi pi-trash',
    command:async()=>{
      await axios.delete('https://localhost:7273/api/Course/'+props.Id)
      .then((res)=>{
        console.log(props.Id);
        location.reload()
        return res;
      });
    }
  }
]);

const toggle = (event: Event) => {
  menu.value.toggle(event);
};
</script>