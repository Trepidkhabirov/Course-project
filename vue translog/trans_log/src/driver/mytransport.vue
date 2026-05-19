<script setup>
import router from '@/router'
import { ref, onMounted, computed } from 'vue'
import order from '../assets/images/order.png'
import transport from '../assets/images/transport.png'

const fullname = localStorage.getItem('fullname')
const myVehicle = ref(null)

onMounted(async () => {
  const userId = localStorage.getItem('userId')
  const response = await fetch('http://localhost:5095/api/Driver/GetDrivers')
  const drivers = await response.json()
  const myDriver = drivers.find(d => d.userId == userId)
  if (myDriver) myVehicle.value = myDriver.vehicle
})

const logout = () => {
  localStorage.clear()
  router.push('/authorization')
}

const initials = computed(() => {
  if (!fullname) return ''

  return fullname
    .split(' ')
    .map(word => word[0])
    .join('')
    .toUpperCase()
})
</script>

<template>
  <div class="layout">
    <div class="sidebar">
      <div class="logo">
        <h1>ТРАНС<span>ЛОГ</span></h1>
        <p>ГРУЗОПЕРЕВОЗКИ</p>
      </div>
      <div class="user">
        <div class="avatar">{{ initials }}</div>
        <div>
          <p class="user-name">{{ fullname }}</p>
          <p class="user-role">Водитель</p>
        </div>
      </div>
      <div class="menu">
        <div class="podmenu" @click="$router.push('/mytrips')">
          <img :src="order">
          <a class="menu-item">Мои рейсы</a>
        </div>
        <div class="podmenu_active">
          <img :src="transport">
          <a class="menu-item">Транспорт</a>
        </div>
      </div>
      <hr>
      <a class="logout" @click="logout">Выйти из системы</a>
    </div>

    <div class="content">
      <div class="topbar">Мой транспорт</div>
      <div class="card">
        <h2>Мой автомобиль</h2>
        <div v-if="myVehicle" style="margin-top: 20px;">
          <table>
            <thead>
              <tr>
                <td>Гос. номер</td>
                <td>Марка</td>
                <td>Грузоподъёмность (кг)</td>
                <td>Объём (м³)</td>
                <td>Тип</td>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{{ myVehicle.licensePlate }}</td>
                <td>{{ myVehicle.brand }} {{ myVehicle.model }}</td>
                <td>{{ myVehicle.payloadKg }}</td>
                <td>{{ myVehicle.volumeM3 }}</td>
                <td>{{ myVehicle.vehicleType?.name ?? 'Не указан' }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else style="margin-top: 20px; color: #7a8ba8; font-size: 18px;">
          Транспорт не назначен
        </div>
      </div>
    </div>
  </div>
</template>