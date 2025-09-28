<template>
    <div>
        <!-- Кнопка для открытия меню -->
        <Button label="Меню" @click="toggleMenu" aria-haspopup="true" aria-controls="overlay_menu" />

        <!-- Меню -->
        <TieredMenu id="overlay_menu" ref="menu" :model="menuItems" :popup="true" />

        <!-- Диалоги -->
        <Dialog v-model:visible="displayCoachDialog" header="Пользователь" :style="{ width: '50vw' }" :modal="true">
            <p>Изменение тренера по id</p>
            <template #footer>
                <Input placeholder="id тренера" v-model="coachId"/>
                <Button label="Отправить" @click="ReplaceCoahInCourse(coachId)"></Button>
                <Button label="Закрыть" @click="displayCoachDialog = false" />
            </template>
        </Dialog>

        <Dialog v-model:visible="displayDateOfLessonDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
            <p>Изменить дату проведения занятия</p>
            <template #footer>
                <Button label="Закрыть" @click="displayDateOfLessonDialog = false" />
            </template>
        </Dialog>

        <Dialog v-model:visible="displayDateOfStartCourseDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
            <p>Изменить дату начала и конца курса</p>
            <template #footer>
                <Button label="Закрыть" @click="displayDateOfStartCourseDialog = false" />
            </template>
        </Dialog>

        <Dialog v-model:visible="displayDaysOfCourseDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
            <p>Изменить дни проведения занятий</p>
            <template #footer>
                <Button label="Закрыть" @click="displayDaysOfCourseDialog = false" />
            </template>
        </Dialog>

        <Dialog v-model:visible="displayTimeOfLessonDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
            <p>Изменить время проведения занятия</p>
            <template #footer>
                <Button label="Закрыть" @click="displayTimeOfLessonDialog = false" />
            </template>
        </Dialog>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios'
const props = defineProps(['Id'])
let coachId = ref();
const menu = ref();
const displayCoachDialog = ref(false);
const displayDateOfLessonDialog = ref(false);
const displayDateOfStartCourseDialog = ref(false);
const displayTimeOfLessonDialog = ref(false);
const displayDaysOfCourseDialog = ref(false);


let courseDto = ref({
    days: '',
    timeOfLesson: '',
    dateOfStartCourse: '',
    dateOfEndCourse: '',
    title: '',
    coachId: ''
});

const ReplaceCoahInCourse = (coachId) => {
    axios.patch('https://localhost:7273/api/Course/coach', {
        courseId: props.Id,
        coachId: coachId
    })
    .then((res) => {
        console.log(res);
        alert('ok');
    })
    .catch((error) => {
        console.error('Error:', error);
        alert('Error occurred');
    });
}

const toggleMenu = (event) => {
    menu.value.toggle(event);
};

const menuItems = ref([
    {
        label: 'Изменить',
        icon: 'pi pi-file-edit',
        items: [
            {
                label: 'Дату проведения занятия',
                icon: 'pi pi-file',
                command: () => {
                    displayDateOfLessonDialog.value = true;
                }
            },
            {
                label: 'Изменить тренера',
                icon: 'pi pi-file',
                command: () => {
                    displayCoachDialog.value = true;
                }
            },
            {
                label: 'Дату начала курса и конца курса',
                icon: 'pi pi-file',
                command: () => {
                    displayDateOfStartCourseDialog.value = true;
                }
            },
            {
                label: 'Время проведения занятия',
                icon: 'pi pi-file',
                command: () => {
                    displayTimeOfLessonDialog.value = true;
                }
            },
             {
                label: 'Изменить дни недели',
                icon: 'pi pi-file',
                command: () => {
                    displayTimeOfLessonDialog.value = true;
                }
            }
        ]
    },
    {
        separator: true
    },
    {
        label: 'Удалить',
        icon: 'pi pi-trash',
        command: async () => {
            await axios.delete('https://localhost:7273/api/Course/' + props.Id)
                .then((res) => {
                    console.log(props.Id);
                    location.reload()
                    return res;
                });
        }
    }
]);
</script>