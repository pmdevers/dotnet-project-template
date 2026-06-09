import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import './assets/main.css'

// Pages
import HomePage from './pages/HomePage.vue'
import CarsPage from './pages/CarsPage.vue'
import CarDetailPage from './pages/CarDetailPage.vue'
import ReservationPage from './pages/ReservationPage.vue'
import ConfirmationPage from './pages/ConfirmationPage.vue'

const routes = [
  { path: '/', component: HomePage, name: 'home' },
  { path: '/cars', component: CarsPage, name: 'cars' },
  { path: '/cars/:id', component: CarDetailPage, name: 'car-detail' },
  { path: '/reservation/:carId', component: ReservationPage, name: 'reservation' },
  { path: '/confirmation/:reservationId', component: ConfirmationPage, name: 'confirmation' },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

createApp(App).use(router).mount('#app')
