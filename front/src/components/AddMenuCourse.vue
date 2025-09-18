<template>
    <div class="card flex justify-center">
        <Drawer v-model:visible="visible" header="Меню">
            <InputText type="text" v-model="courseDto.dayOfWeak" placeholder="дни недели" />
            <InputText type="text" v-model="courseDto.timeOfLesson" placeholder="время проведения занятия" />
            <InputText type="text" v-model="courseDto.dateOfStartCourse" placeholder="дата начала курса" />
            <InputText type="text" v-model="courseDto.dateOfEndCourse" placeholder="дата конца курса" />
            <InputText type="text" v-model="courseDto.title" placeholder="название курса" />
            <InputText type="text" v-model="courseDto.id" placeholder="id тренера курса" />
            <Button label="Создать" @click="visible = true" />
        </Drawer>
        <Button icon="pi pi-plus" @click="visible = true" />
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import Button from 'primevue/button';
import Drawer from 'primevue/drawer';
import InputText from 'primevue/inputtext';


let courseDto = {
    dayOfWeak: '',
    timeOfLesson: '',
    dateOfStartCourse: '',
    dateOfEndCourse: '',
    title: '',
    id: ''
}
let visible = ref();
</script>