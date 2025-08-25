import {createApp} from 'vue'
import {createPinia} from 'pinia'
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import "./assets/css/main.css";
import { createRouter, createWebHistory } from 'vue-router'
import { routes, handleHotUpdate } from 'vue-router/auto-routes'

import App from './App.vue'

const app = createApp(App)

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

if (import.meta.hot) {
  handleHotUpdate(router)
}

app.use(router)
app.use(createPinia())
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      darkModeSelector: ".p-dark",
    }
  },
  ripple: true,
  locale: {
    accept: 'Russian'
  }
})

app.mount('#app')
