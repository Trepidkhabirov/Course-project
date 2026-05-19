<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed } from 'vue'
import plus from '../assets/images/plus.png'
import time from '../assets/images/time.png'
import history from '../assets/images/history.png'

const messageAlert = ref('')

const departurepoint = ref('')
const arrivalpoint = ref('')
const weight = ref('')
const volumem3 = ref('')
const description = ref('')
const colormessage = ref('')

const initials = computed(() => {
  if (!fullname) return ''

  return fullname
    .split(' ')
    .map(word => word[0])
    .join('')
    .toUpperCase()
})

const order = async () =>
{
  if (!departurepoint.value || !arrivalpoint.value || !weight.value || !volumem3.value)
{ 
    colormessage.value = 'red'
    messageAlert.value = 'Заполните все поля'
      setTimeout(() => {
      messageAlert.value = ''
}, 5000)
return
}

  const response = await fetch(
    'http://localhost:5095/api/Order/CreateOrder',
    {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({
        userId: parseInt(localStorage.getItem('userId')),
        DeparturePoint: departurepoint.value,
        ArrivalPoint: arrivalpoint.value,
        Weight: parseFloat(weight.value),
        Volumem3: parseFloat(volumem3.value),
        Description: description.value,
        Status: 'В ожидании',
        ReceivedAt: new Date().toISOString()
      })
    }
  )
  const data = await response.json()
  console.log(data)
  if (response.ok)
  {
    departurepoint.value = ''
    arrivalpoint.value = ''
    weight.value = ''
    volumem3.value = ''
    description.value = ''
    colormessage.value = 'green'
    messageAlert.value = data.message
     setTimeout(() => {
      messageAlert.value = ''
}, 10000)

  }
  }
const fullname = localStorage.getItem('fullname')
const logout = () => {
  localStorage.clear()
  router.push('/authorization')
}


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
          <p class="user-name">{{ fullname || 'Нет'}}</p>
          <p class="user-role">Клиент</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu_active">
            <img :src="plus"> 
            <a class="menu-item active" >Новая заявка</a>
        </div>
        <div class="podmenu" @click="$router.push('/statusorder')">
            <img :src="time">
            <a class="menu-item" >Статус заявок</a>
        </div >
        <div class="podmenu" @click="$router.push('/historyorder')">
            <img :src="history">
            <a class="menu-item">История заявок</a>
        </div>
        </div>

        <hr>
      <a class="logout" @click="logout">Выйти из системы</a>
    </div>

    <form @submit.prevent="order">
    <div class="content">
      
      <div class="topbar">Новая заявка</div>

      <div class="card">
        <p class="card-title">Оформление заявки на грузоперевозку</p>

        <h2>МАРШРУТ</h2>
        
        <div class="row">
          <div class="field">
            <label style="font-size: 18px;">Пункт отправления</label>
            <input  type="text" v-model="departurepoint" placeholder="Москва">
          </div>
          <div class="field">
            <label style="font-size: 18px;" >Пункт прибытия</label>
            <input type="text" v-model="arrivalpoint" placeholder="Уфа">
          </div>
        </div>

        <h2>ПАРАМЕТРЫ ГРУЗА</h2>
        <div class="row">
          <div class="field">
            <label style="font-size: 18px;">Вес груза (тонн)</label>
            <input type="text" v-model="weight"  placeholder="0.5">
          </div>
          <div class="field">
            <label style="font-size: 18px;">Объем груза (м2)</label>
            <input type="text" v-model="volumem3" placeholder="1.0">
          </div>
        </div>
        
        <div class="field">
          <label style="font-size: 18px;">Описание груза (необязательно)</label>
          <textarea rows="5" v-model="description"></textarea>
        </div>
        <p v-if="messageAlert" :style="{color: colormessage, fontSize:   '20px' }"> {{ messageAlert }}</p>
        <button class="submit" style="font-size: 18px;">Подать заявку</button>
      </div>
    </div>
  </form>
  </div>
</template>

<style   >

hr{
    width: 340px;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

#app {
  max-width: none;
  padding: 0;
  margin: 0;
  width: 100%;
}

body, html {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

.layout {
  display: flex;
  min-height: 100vh;
  font-family: Arial;
  width: 100%;
}

.content {
  flex: 1;
  background: #f4f7fb;
  display: flex;
  flex-direction: column;
}

.sidebar {
  width: 340px;
  min-width: 340px;
  background: #1D2D50;
  color: white;
  display: flex;
  flex-direction: column;
  height: 100vh;
  padding: 20px 0;
  flex-shrink: 0;
}

.logo {
  padding: 0 25px 20px;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.logo h1 {
  font-family: Impact;
  font-size: 26px;
}

.logo h1 span {
  color: #4da6ff;
}

.logo p {
  font-size: 10px;
  letter-spacing: 3px;
  color: #a0aabf;
  margin-top: 3px;
}

.user {
  display: flex;
  align-items: center;
  padding: 20px 25px;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.avatar {
  width: 45px;
  height: 45px;
  background: #2a7fff;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 12px;
  font-weight: bold;
}

.user-name {
  font-size: 16px;
}

.user-role {
  font-size: 17px;
  color: #a0aabf;
}

.menu {
  margin-top: 15px;
  flex: 1;
}

.podmenu,
.podmenu_active {
  display: flex;
  flex-direction: row;
  align-items: center;
  padding: 0 25px;
}

.podmenu_active {
  background: rgba(0,0,0,0.25);
}

.podmenu img,
.podmenu_active img {
  width: 25px;
  height: 25px;
  margin-right: 10px;
}

.menu-item {
  display: inline;
  padding: 12px 25px;
  color: #c5cce0;
  font-size: 14px;
  cursor: pointer;
  text-decoration: none;
}

.logout {
  padding: 15px 25px;
  font-size: 20px;
  color: #6b7590;
  cursor: pointer;
}

.topbar {
  background: white;
  padding: 20px 40px;
  font-weight: bold;
  border-bottom: 1px solid #ddd;
  font-size: 16px;
}

.card {
  background: white;
  margin: 30px 40px;
  padding: 30px;
  border-radius: 8px;
}

.card-title {
  font-size: 22px;
  color: black;
  margin-bottom: 30px;
}

h2 {
  color: black;
  font-size: 20px;
  margin-bottom: 20px;
  margin-top: 25px;
}

.row {
  display: flex;
  gap: 20px;
}

.field {
  flex: 1;
  display: flex;
  flex-direction: column;
  margin-bottom: 15px;
}

.field label {
  font-size: 15px;
  color: #777;
  margin-bottom: 6px;
}

.field input,
.field textarea {
  padding: 10px 14px;
  border: 1px solid #d5dae3;
  border-radius: 4px;
  background: #f4f7fb;
  font-size: 16px;
  outline: none;
  font-family: inherit;
}

.submit {
  background: #2a7fff;
  color: white;
  border: none;
  padding: 12px 30px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  float: right;
}

.submit:hover {
  background: #1f6fe0;
}

form {
  flex: 1;
  display: flex;
  flex-direction: column;
}

a {
  text-decoration: none;
}

</style>