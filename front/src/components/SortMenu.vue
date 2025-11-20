
<template>
    <div class="card flex justify-center">
        <Toast />
        <SplitButton @click="save" :model="items" />
    </div>
</template>

<script setup lang="ts">
import { useToast } from "primevue/usetoast";
import {defineModel} from 'vue'
import axios from 'axios';
const toast = useToast();
const model = defineModel('sortData');
const items = [
    {
        label: 'По дате(по убыванию)',
        command: () => {
            axios.get('https://localhost:7273/api/Lesson/get-all?Desc=true')
            .then(function(res){
              model.value = res.data;
            })
        }
    },
    {
        label: 'По id тренера',
        command: () => {
            toast.add({ severity: 'warn', summary: 'Delete', detail: 'Data Deleted', life: 3000 });
        }
    },
    {
        separator: true
    },
    {
        label: 'Quit',
        command: () => {
            window.location.href = 'https://vuejs.org/';
        }
    }
];




</script>
