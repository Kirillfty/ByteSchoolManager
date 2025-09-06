<template>
    <div class="card flex justify-center">
      <Drawer v-model:visible="visible" header="Меню">

        <Menu :model="items"></Menu>
      </Drawer>
      <Button icon="pi pi-arrow-right" @click="visible = true" />
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import Menu from 'primevue/menu';
import Button from 'primevue/button';
import Drawer from 'primevue/drawer';
import { useRouter } from 'vue-router';
let router = useRouter();

const items = ref([
  {
    label: 'Главная',
    icon: 'pi pi-home',
    command: () => router.push('/home')
  },
  {
    label: 'Курсы',
    icon: 'pi pi-book',
    command: () => router.push('/course')
  },
  {
    label: 'Занятия',
    icon: 'pi pi-book',
    command: () => router.push('/lesson')
  },
  {
    label: 'Студенты',
    icon: 'pi pi-user',
    command: () => router.push('/student')
  },
  {
    label: 'Тренера',
    icon: 'pi pi-user',
    command: () => router.push('/coach')
  }
]);
let visible = ref();
</script>