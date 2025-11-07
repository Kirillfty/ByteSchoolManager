<template>
  <div>
    <!-- Кнопка для открытия меню -->
    <Button label="Меню" @click="toggleMenu" aria-haspopup="true" aria-controls="overlay_menu" />

    <!-- Меню -->
    <TieredMenu id="overlay_menu" ref="menu" :model="menuItems" :popup="true" />

    <!-- Диалоги -->
    <Dialog v-model:visible="displayStudentDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
      <p>Изменение студента</p>
      <template #footer>
        <div class="container">
          <div class="inputs">
            <input placeholder="Имя" v-model="studentDto.Name" />
            <input placeholder="Имя родителя" v-model="studentDto.parentName" />
            <input placeholder="Номер студента" v-model="studentDto.studentPhoneNumber" />
            <input placeholder="Номер родителя студента" v-model="studentDto.parentPhoneNumber" />
          </div>
          <div class="buttons">
            <Button label="✓" @click="UpdateStudent()" />
            <Button label="⨉" @click="displayStudentDialog = false" />
          </div>
        </div>
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
const props = defineProps(['Id'])
const menu = ref()
const displayStudentDialog = ref(false)
const studentDto = ref({
    Name: '',
    studentPhoneNumber: '',
    parentName: '',
    parentPhoneNumber: '',
});

function UpdateStudent(){
   axios.patch('https://localhost:7273/api/student/update',{
    id:props.Id,
    name:studentDto.value.Name,
    studentPhoneNumber:studentDto.value.studentPhoneNumber,
    parentName:studentDto.value.parentName,
    parentPhoneNumber:studentDto.value.parentPhoneNumber
  })
  .then(function(res){
    alert('updated'+res.status);
    location.reload();
  })
  .catch(function(ex){
    alert(ex)
  })
}

const toggleMenu = (event) => {
  menu.value.toggle(event)
}

const menuItems = ref([
  {
    label: 'Изменить',
    icon: 'pi pi-file-edit',
    items: [
      {
        label: 'Изменить данные студента',
        icon: 'pi pi-file',
        command: () => {
          displayStudentDialog.value = true
        }
      }
    ]
  },
  {
    separator: true,
  },
  {
    label: 'Удалить',
    icon: 'pi pi-trash',
    command: async () => {
      await axios.delete('https://localhost:7273/api/Student/delete/' + props.Id)
      .then((res) => {
        console.log(props.Id)
        location.reload()
        return res;
      })
    },
  },
])
</script>
<style scoped>
.inputs{
  display: flex;
  flex-direction: column;
}
.container{
  display:flex;
  flex-direction: column;
  align-items: center;
  padding:3%;
}
.buttons{
  margin-top:3%;
}
</style>
