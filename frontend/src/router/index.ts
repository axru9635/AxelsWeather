
import { createRouter, createWebHistory } from 'vue-router'

import WeatherOfTheDay from '../views/WeatherOfTheDay.vue' 
//import TomorrowsWeather from '../views/TomorrowsWeather.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: { path: "/today" }
    },
    {
      path: '/today',
      name: 'today',
      component: WeatherOfTheDay,
    },
    {
      path: '/tomorrow',
      name: 'tomorrow',
      component: WeatherOfTheDay,
    },
  ]
})

export default router
