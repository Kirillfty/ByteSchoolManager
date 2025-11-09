
<template>
    <div class="card flex justify-center">
        <Toast />
        <SplitButton label="Сортировать" @click="save" :model="items" />
    </div>
</template>

<script setup lang="ts">
import { useToast } from "primevue/usetoast";
const toast = useToast();

const items = [
    {
        label: 'По дате(по убыванию)',
        command: () => {
            toast.add({ severity: 'success', summary: 'Updated', detail: 'Data Updated', life: 3000 });
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

const save = () => {
    toast.add({ severity: 'success', summary: 'Success', detail: 'Data Saved', life: 3000 });
};
</script>
