<template>
  <div class="bg-surface-50 dark:bg-surface-950 min-h-screen py-12 sm:px-6 lg:px-8">
    <Toast />
    <div>
      <h2 class="mt-6 text-center text-3xl font-extrabold text-color-emphasis">
        Зарегистрируйте свой аккаунт
      </h2>
      <div class="mt-2 text-center max-w">
        <span class="text-muted-color">Или </span>
        <RouterLink
          class="text-primary hover:underline hover:text-shadow-2xs underline-offset-4"
          to="/"
          >войдите в существующий</RouterLink
        >
      </div>
    </div>
    <div class="mt-8 bg-white py-8 px-4 shadow rounded-lg sm:mx-auto sm:max-w-md">
      <Form
        class="space-y-6"
        v-slot="$form"
        :resolver="valibotResolver(LoginSchema)"
        @submit="onFormSubmit"
      >
        <div>
          <InputText name="username" type="text" placeholder="Имя пользователя" fluid />
          <Message v-if="$form.username?.invalid" severity="error" size="small" variant="simple">{{
            $form.username.error.message
          }}</Message>
        </div>
        <div>
          <Password name="password" placeholder="Пароль" fluid toggle-mask :feedback="false" />
          <Message v-if="$form.password?.invalid" severity="error" size="small" variant="simple">{{
            $form.password.error.message
          }}</Message>
        </div>
        <div>
          <Password
            name="passwordSubmit"
            placeholder="Повторите пароль"
            fluid
            toggle-mask
            :feedback="false"
          />
          <Message
            v-if="$form.passwordSubmit?.invalid"
            severity="error"
            size="small"
            variant="simple"
            >{{ $form.passwordSubmit.error.message }}</Message
          >
        </div>
        <Button
          :disabled="!$form.valid"
          fluid
          type="submit"
          severity="primary"
          label="Регистрация"
        />
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { valibotResolver } from '@primevue/forms/resolvers/valibot'
import * as v from 'valibot'
import { useToast } from 'primevue/usetoast'
import type { FormSubmitEvent } from '@primevue/forms'
import axios from 'axios'
import { router } from '@/main.ts'

const toast = useToast()

const LoginSchema = v.pipe(
  v.object({
    username: v.pipe(
      v.string('Введите имя пользователя'),
      v.trim(),
      v.nonEmpty('Введите имя пользователя'),
      v.minLength(5, 'Минимальная длинна имени пользователя - 5 символов'),
    ),
    password: v.pipe(
      v.string('Введите пароль'),
      v.trim(),
      v.nonEmpty('Введите пароль'),
      v.minLength(6, 'Минимальная длинна пароля - 6 символов'),
    ),
    passwordSubmit: v.pipe(
      v.string('Подтвердите пароль'),
      v.trim(),
      v.nonEmpty('Подтвердите пароль'),
    ),
  }),
  v.forward(
    v.partialCheck(
      [['password'], ['passwordSubmit']],
      (input) => input.password === input.passwordSubmit,
      'Пароли не совпадают.',
    ),
    ['passwordSubmit'],
  ),
)

function onFormSubmit(formData: FormSubmitEvent) {
  if (!formData.valid) return

  axios
    .post('/api/Auth/register', {
      login: formData.values.username,
      password: formData.values.password,
    })
    .then(function (res) {
      toast.add({
        severity: 'success',
        summary: `Вы зарегистрированы`,
        detail: `Добро пожаловать, ${formData.values.username}!`,
        life: 3000,
      })

      localStorage.clear()
      localStorage.setItem('accessToken', res.data.accessToken)
      localStorage.setItem('refreshToken', res.data.refreshToken)
      console.log('ok')
      //router.push('/coach');
    })
    .catch((res) => {
      if (res.response.status === 400) {
        toast.add({
          severity: 'error',
          summary: `Ошибка`,
          detail: `Такой пользователь уже создан!`,
          life: 3000,
        })
      } else if (res.response.status === 500) {
        toast.add({
          severity: 'error',
          summary: `Внутренняя ошибка`,
          detail: `Обратитесь с разработчику: t.me/fat_alien`,
          life: 3000,
        })
      } else {
        toast.add({
          severity: 'error',
          summary: `Непредвиденная ошибка`,
          detail: `Попробуйте еще раз или обратитесь с разработчику: t.me/fat_alien`,
          life: 3000,
        })
      }
    })
}
</script>
