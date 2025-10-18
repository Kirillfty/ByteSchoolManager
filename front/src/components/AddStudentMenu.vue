<template>
    <div class="card flex justify-center">
        <Drawer v-model:visible="visible" header="Меню">
            <div class="inputs">
                <InputText type="text" v-model="studentDto.name" placeholder="Имя студента" />
                <InputText type="text" v-model="studentDto.studentPhoneNumber" placeholder="Номер телефона студента" />
                <InputText type="text" v-model="studentDto.parentName" placeholder="Имя родителя" />
                <InputText type="text" v-model="studentDto.parentPhoneNumber" placeholder="Номер телефона родителя" />
            </div>
            <div class="buttons">
                <Button label="Добавить" @click="AddStudent()" />

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

async function AddStudent(){
   await axios.post('https://localhost:7273/api/Student',{name:studentDto.value.name,
    studentPhoneNumber:studentDto.value.studentPhoneNumber,
    parentName:studentDto.value.parentName,
    parentPhoneNumber:studentDto.value.studentPhoneNumber
   })
    .then(()=>{
        alert('created');
        location.reload();
    })
}

const studentDto = ref({
    name: '',
    studentPhoneNumber: '',
    parentName: '',
    parentPhoneNumber: '',
});
const visible = ref();
</script>
