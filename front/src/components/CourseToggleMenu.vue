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
            <p>Изменить дату начала курса</p>
            <template #footer>
                <Button label="Закрыть" @click="displayDateOfStartCourseDialog = false" />
            </template>
        </Dialog>

        <Dialog v-model:visible="displayDateOfEndCourseDialog" header="Настройки" :style="{ width: '50vw' }" :modal="true">
            <p>Изменить дату конца курса</p>
            <template #footer>
                <Button label="Закрыть" @click="displayDateOfEndCourseDialog = false" />
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

const menu = ref();
const displayCoachDialog = ref(false);
const displayDateOfLessonDialog = ref(false);
const displayDateOfStartCourseDialog = ref(false);
const displayDateOfEndCourseDialog = ref(false);
const displayTimeOfLessonDialog = ref(false);

let courseDto = ref({
    days: '',
    timeOfLesson: '',
    dateOfStartCourse: '',
    dateOfEndCourse: '',
    title: '',
    coachId: ''
});

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
                label: 'Дату начала курса',
                icon: 'pi pi-file',
                command: () => {
                    displayDateOfStartCourseDialog.value = true;
                }
            },
            {
                label: 'Изменить дату конца курса',
                icon: 'pi pi-file',
                command: () => {
                    displayDateOfEndCourseDialog.value = true;
                }
            },
            {
                label: 'Время проведения занятия',
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