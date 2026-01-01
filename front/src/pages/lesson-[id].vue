<template>
  <Button label="Отметить"></Button>
  <Button label="Назад" @click="BackToCoachPage()"></Button>
  <h1>{{ id }}</h1>
</template>
<script setup lang="ts">
import { useRoute ,useRouter} from 'vue-router';
const route = useRoute();
const router = useRouter();
const id = route.params.id;

function BackToCoachPage(){
  router.push('/coach');
}
</script>
