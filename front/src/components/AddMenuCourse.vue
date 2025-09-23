<template>
    <div class="card flex justify-center">
        <Drawer v-model:visible="visible" header="Меню">
            <div class="inputs">
                <InputText type="text" v-model="courseDto.days" placeholder="дни недели" />
                <InputText type="text" v-model="courseDto.timeOfLesson" placeholder="время проведения занятия" />
                <InputText type="text" v-model="courseDto.dateOfStartCourse" placeholder="дата начала курса" />
                <InputText type="text" v-model="courseDto.dateOfEndCourse" placeholder="дата конца курса" />
                <InputText type="text" v-model="courseDto.title" placeholder="название курса" />
                <InputText type="text" v-model="courseDto.coachId" placeholder="id тренера курса" />
            </div>
            <div class="buttons">
                <Button label="Добавить" @click="visible = true" />
                
            </div>
        </Drawer>
        <Button icon="pi pi-plus" @click="visible = true" />
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import Button from 'primevue/button';
import Drawer from 'primevue/drawer';
import InputText from 'primevue/inputtext';
import axios from 'axios'

async function AddCourse(){
    await axios.post('https://localhost:7273/api/Course',{days:Number(courseDto.value.days),
        timeOfLesson:courseDto.value.timeOfLesson,
        dateOfStartCourse:courseDto.value.dateOfStartCourse,
        dateOfEndCourse:courseDto.value.dateOfEndCourse,
        title:courseDto.value.title,
        coachId:courseDto.value.coachId
    })
    .then(()=>{
        alert('created');
    })
}

let courseDto = ref({
    days: '',
    timeOfLesson: '',
    dateOfStartCourse: '',
    dateOfEndCourse: '',
    title: '',
    coachId: ''
});
let visible = ref();
</script>