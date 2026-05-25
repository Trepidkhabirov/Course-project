<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed, onMounted, watch } from 'vue'
import order from '../assets/images/order.png'
import people from '../assets/images/people.png'
import transport from '../assets/images/transport.png'

const showTripModal = ref(false)
const currentOrder = ref(null)
const tripData = ref({
  vehicleId: null,
  from: '',
  to: '',
  distanceKm: 0, 
  price: 0,
  departureDate: '',
  arrivalDate: ''
})
const vehicleRates = {
  1: 50, 
  2: 80, 
}


const calculatePrice = () => {
  const distance = parseFloat(tripData.value.distanceKm)
  const vehicleId = tripData.value.vehicleId
  
  if (distance > 0 && vehicleId) {
    const selectedVehicle = vehicles.value.find(v => v.vehicleId === vehicleId)
    
    if (selectedVehicle) {
      const rate = selectedVehicle.vehicleType?.costPerKm || vehicleRates[selectedVehicle.vehicleTypeId] || 50
      
      tripData.value.price = distance * rate
      console.log(`Расчет: ${distance} км * тариф ${rate} руб = ${tripData.value.price}`)
    }
  } else {
    tripData.value.price = 0
  }
}

watch(() => tripData.value.distanceKm, calculatePrice)
watch(() => tripData.value.vehicleId, calculatePrice)



const errorMessage = ref('')
const saveTrip = async () =>
{
   if (!tripData.value.vehicleId || !tripData.value.departureDate || !tripData.value.arrivalDate || !tripData.value.distanceKm) {
    errorMessage.value = 'Заполните все поля!'
    setTimeout(() => {
      errorMessage.value = ''
}, 5000)
    return
  }
  const response = await fetch(
    `http://localhost:5095/api/Order/UpdateOrder?OrderId=${currentOrder.value.orderId}`,
    {
      method: "PUT",
      headers: { 'Content-Type': 'application/json'},
      body: JSON.stringify({
        Status: 'Ожидает оплаты',
        DepartureTime: tripData.value.departureDate,
        ArrivalTime: tripData.value.arrivalDate,
        vehicleId: tripData.value.vehicleId,
        Distance_km: tripData.value.distanceKm,
        Price: tripData.value.price 
      })
    }
  )
  const data = await response.json()
  console.log(data)
  showTripModal.value = false
  const res = await fetch('http://localhost:5095/api/Order/GetOrder')
orders.value = sortOrders(await res.json())
}

const openTripModal = (order) => {
  currentOrder.value = order
  tripData.value = {
    vehicleId: order.vehicleId || null,
    from: order.departurePoint,
    to: order.arrivalPoint,
    distanceKm: order.distanceKm  || 0,
     departureDate: order.departureTime || '',
    arrivalDate: order.arrivalTime || '',
    Price: order.price || 0,
  }
  console.log('Полный объект заказа:', order)
  console.log('distance_km:', order.distanceKm)
  console.log('DistanceKm:', order.distanceKm)
  showTripModal.value = true
   calculatePrice()
}
  const selectedDriverName = computed(() => {
    if (!tripData.value.vehicleId) return ''
    const vehicle = vehicles.value.find(v => v.vehicleId === tripData.value.vehicleId)
    if (!vehicle || !vehicle.drivers?.[0]?.user) return 'Водитель не привязан'
    return vehicle.drivers[0].user.fullName
  })

const departurepoint = ref('')
const arrivalpoint = ref('')
const weight = ref('')
const volumem3 = ref('')
const description = ref('')
const orders = ref([])
const drivers = ref([])
const vehicles = ref([])  
onMounted(async () => {
{
  const userID = parseInt(localStorage.getItem('userId'))
  const response = await fetch(
    `http://localhost:5095/api/Order/GetOrder`)
    const data = await response.json()
  orders.value = sortOrders(data)
  console.log(data)
  const response2 = await fetch('http://localhost:5095/api/Driver/GetDrivers')
  drivers.value = await response2.json()
console.log('drivers:', drivers.value)
const response3 = await fetch('http://localhost:5095/api/Vehicle/GetTransport')
  vehicles.value = await response3.json()
  console.log('vehicles:', vehicles.value)
  orders.value = sortOrders(data.filter(o => o.status !== 'Доставлено' && o.status !== 'Отменено'))
}
})
const logout = () => {
  localStorage.clear()
  router.push('/authorization')
}
const newStatus = ref('')
const showStatusModal = ref(false)
const openStatusModal = (order) => {
  currentOrder.value = order
  newStatus.value = order.status
  showStatusModal.value = true
}

const saveStatus = async () =>
{
  const response = await fetch(
     `http://localhost:5095/api/Order/UpdateOrder?OrderId=${currentOrder.value.orderId}`,
     {
      method: 'PUT',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({Status: newStatus.value})
     }
  )
  showStatusModal.value = false
  const res = await fetch('http://localhost:5095/api/Order/GetOrder')
  orders.value = sortOrders(await res.json())
}
const fullname = localStorage.getItem('fullname')


const totalOrders = computed( () => orders.value.length)
const totalwork = computed (() => orders.value.filter(o => o.status == 'Выполняется').length)
const totalend = computed (() => orders.value.filter(o => o.status == 'Доставлено').length)
const totalwait = computed (() => orders.value.filter(o => o.status == 'Ожидает').length)
const totalpaying = computed(() => orders.value.filter(o => o.status == 'Ожидает оплаты').length)
const initials = computed(() => {
  if (!fullname) return ''

  return fullname
    .split(' ')
    .map(word => word[0])
    .join('')
    .toUpperCase()
})

const statusOrder = { 
  'Ожидает': 0, 
  'Принято': 1, 
  'Ожидает оплаты': 2, 
  'Выполняется': 3, 
  'Доставлено': 4, 
  'Отменено': 5 
}

const sortOrders = (data) => {
  return data.sort((a, b) => statusOrder[a.status] - statusOrder[b.status])
}

const today = new Date().toISOString().split('T')[0]  
const availableVehicles = computed(() => 
  vehicles.value.filter(v => v.drivers && v.drivers.length > 0)
)

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
          <p class="user-role">Менеджер</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu_active">
            <img :src="order"> 
            <a class="menu-item" > Заявки</a>
        </div>
        <div class="podmenu" @click="$router.push('/trips')" >
            <img :src="transport">
            <a class="menu-item" >Рейсы</a>
        </div >
        <div class="podmenu" @click="$router.push('/drivers')">
            <img :src="people">
            <a class="menu-item">Водители </a>
        </div>
        </div>
<hr>
      <a class="logout" @click="logout">Выйти из системы</a>
    </div>

    <div class="content">
      
      <div class="topbar">Статус заявки</div>

      <div class="card">
        <div class="row">
          <div class="lab">
              <p class="lab_title">Всего заявок</p>
              <p style="color: black; font-size: 34px; margin-bottom: -50px; margin-left: 25px; ">{{ totalOrders }}</p>
              <p class="lab_other">За всё время</p>
          </div>
          <div class="lab">
            <p class="lab_title">Новых</p>
            <p style="color: black; font-size: 34px; margin-bottom: -50px; margin-left: 25px; ">{{ totalwait }}</p>
          <p class="lab_other">требуют обработки</p>
          </div>
          <div class="lab">
            <p class="lab_title">В работе</p>
            <p style="color: black; font-size: 34px; margin-bottom: -50px; margin-left: 25px; ">{{ totalwork }}</p>
            <p class="lab_other">активных</p>
          </div>
          <div class="lab">
            <p class="lab_title">Закрыто</p>
            <p style="color: black; font-size: 34px; margin-bottom: -50px; margin-left: 25px; ">{{ totalend }}</p>
            <p class="lab_other">За всё время</p>
          </div>
        </div>
        
        <h3 id="titleorder">Все заявки</h3>
        <div style="overflow-y: auto; max-height: 480px;" >
          <table>
            <thead>
              <tr>
                <td>№ ЗАЯВКИ</td>
                <td>ДАТА</td>
                <td>МАРШРУТ</td>
                <td>ГРУЗ (Т)</td>
                <td>СТАТУС</td>
              </tr>
            </thead>
            <tbody>
              <tr v-for="order in orders" :key="order.orderId">
                <td>#{{ order.orderId }}</td>
               <td>{{ new Date(order.receivedAt).toLocaleDateString('ru-RU') }}</td>
                 <td>{{ order.departurePoint }} → {{ order.arrivalPoint }}</td>
               <td>{{ order.weight }}</td>
                <td>
   <span 
    class="status"
    :class="{
      waiting: order.status === 'Ожидает',
      paying: order.status === 'Ожидает оплаты',
      accepted: order.status === 'Принято',
      progress: order.status === 'Выполняется',
      done: order.status === 'Доставлено',
      cancel: order.status === 'Отменено'
    }"
  >
    {{ order.status }}
  </span>

                </td>
                <td>
                  <div style="display: flex; flex-direction: row; gap: 5px;">
                    <button class="manbtn" @click="openStatusModal(order)">Статус</button>
                    <button class="manbtn" @click="openTripModal(order)">Назначить</button>  
                  </div>
                </td>
              </tr>
            </tbody>
            </table>
        </div>
        </div>
    </div>
  </div>
  <div v-if="showTripModal" class="simple-modal">
  <div class="simple-modal-content">
    <h2>Новый рейс</h2>
    <div class="simple-badge">Заявка #{{ currentOrder?.orderId }}</div>
    
    <div class="simple-row">
      <div class="simple-field">
        <label>Выберите транспорт</label>
        <select v-model="tripData.vehicleId" class="simple-select">
          <option value="Выберите транспорт"></option> 
          <option v-for="v in availableVehicles" :key="v.vehicleId" :value="v.vehicleId">
            {{ v.brand }} {{ v.model }} {{ v.licensePlate }}
          </option>
        </select>
      </div>
      <div class="simple-field">
        <label>Водитель</label>
        <input :value="selectedDriverName" disabled placeholder="Выберите транспорт" >
      </div>
    </div>

    <div class="simple-row">
      <div class="simple-field">
        <label>Откуда</label>
        <input v-model="tripData.from">
      </div>
      <div class="simple-field">
        <label>Куда</label>
        <input v-model="tripData.to">
      </div>
    </div>

    <div class="simple-row">
      <div class="simple-field">
        <label>Дата отправления</label>
        <input type="date" v-model="tripData.departureDate" :min="today">
      </div>
      <div class="simple-field">
        <label>Дата прибытия</label>
        <input type="date" v-model="tripData.arrivalDate" :min="today">
      </div>
    </div>
    
    <div class="simple-field">
      <label>Длина маршрута (км)</label>
      <input type="number" v-model.number="tripData.distanceKm">
         <label>Итоговая стоимость</label>
      <input type="number" v-model="tripData.price" readonly>
      <p style="color: red; font-size: 14px;" v-if="errorMessage"  > {{ errorMessage }}</p>
    </div>
    
    <div class="simple-buttons">
      <button class="simple-cancel" @click="showTripModal = false">Отмена</button>
      <button class="simple-save" @click="saveTrip">Сохранить</button>
    </div>
  </div>
</div>
<div v-if="showStatusModal" class="showStatusModal" >
  <div>

    <p>Изменить статус #1</p>
    <p>Новый статус</p>
    <select v-model="newStatus" class="simple-select">
      <option value="Ожидает">Ожидание</option>
      <option value="Ожидает оплаты">Ожидает оплаты</option>
      <option value="Принято">Принято</option>
      <option value="Выполняется">Выполняется</option>
      <option value="Доставлено">Доставлено</option>
      <option value="Отменено">Отменено</option>
    </select>
    <div class="simple-buttons">
      <button class="simple-cancel" @click="showStatusModal = false">Отмена</button>
      <button class="simple-save" @click="saveStatus">Сохранить</button>
    </div>
  </div>
</div>
</template>

<style>
#titleorder
{
  padding-top: 30px;
}
 hr{
    width: 340px;
 }

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 15px;
}
thead tr {
  border-bottom: 1px solid #d5dae3;
}
th {
  text-align: left;
  padding: 10px 15px;
  font-size: 16px;
  color: #7a8ba8;
  font-weight: 600;
}

.paying {
  background-color: #9b59b6;
  color: white;
}
.accepted {
  background-color: #3498db; 
  color: white;
}
td {
  padding: 15px;
  font-size: 16px;
  font-weight: bold;
  color: #1D2D50;
}
.status {
  padding: 6px 16px;
  border-radius: 20px;
  font-size: 13px;
  font-weight: bold;
}
.выполняется {
  background: #2ecc71;
  color: white;
}

.lab
{
  display: flex;
  width: 300px !important;
  height: 180px;
  border-radius: 15px;
  border: solid #C8D3E5 2px;
  background-color: white;
  flex-direction: column;
}
.lab .lab_title
{
font-family: Impact, Haettenschweiler, 'Arial Narrow Bold', sans-serif;
font-size: 22px;
font-weight: bold;
color: #7A8BA8;
padding-left: 15px;
padding-top: 10px;
}
.lab_other
{
  color: #7A8BA8;
  font-size: 18px;
  padding-top: 80px;
  padding-left: 15px;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box; 
}
#app { 
  max-width: none ;
  padding: 0 ;
  margin: 0 ;
  width: 100% ;
}

body, html {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

.content
{
    flex: 1;
    background: #f4f7fb; 
    display: flex;
    flex-direction: column;
}

.layout {
  display: flex;         
  min-height: 100vh;    
  font-family: Arial;
  width: 100%;
}


.sidebar {
  width: 340px;              
  background: #1D2D50;      
  color: white;             
  display: flex;
  flex-direction: column;    
   min-width: 340px;   
  flex-shrink: 0;  
  height: 100vh; 
  padding: 20px 0;
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

.podmenu_active
{
   display: flex;
    flex-direction: row;
    align-items: center;
    padding: 0 25px;
  background: rgba(0,0,0,0.25); ;
}
.podmenu_active img
{
    width: 25px;
    height: 25px;
    margin-right: 10px;
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
.user-name { font-size: 16px; }
.user-role { font-size: 17px; color: #a0aabf; }



.logout {
  padding: 15px 25px;
  font-size: 20px;
  color: #6b7590;
  cursor: pointer;
}

.menu {
  margin-top: 15px;
  flex: 1;                   
}
.menu-item {
  display: inline;            
  padding: 12px 25px;
  color: #c5cce0;
  font-size: 14px;
  cursor: pointer;          
  text-decoration: none;
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
  font-size: 12px;
  color: #777;
  margin-bottom: 6px;
}
.field input,
.field textarea {
  padding: 10px 14px;
  border: 1px solid #d5dae3;
  border-radius: 4px;
  background: #f4f7fb;      
  font-size: 14px;
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

.podmenu
{
    display: flex;
    flex-direction: row;
    align-items: center;
    padding: 0 25px;
}
.podmenu img
{
    width: 25px;
    height: 25px;
    margin-right: 10px;
}
.topbar {
  background: white;
  padding: 20px 40px;
  font-weight: bold;
  border-bottom: 1px solid #ddd;
}

.card {
  background: white;
  margin: 30px 40px;
  padding: 30px;
  border-radius: 8px;
}

a
{
  text-decoration: none;
}
.manbtn
{
  width: 200px;
  height: 40px;
}

.simple-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.simple-modal-content {
  background: white;
  border-radius: 12px;
  padding: 30px;
  width: 500px;
  box-shadow: 0 10px 40px rgba(0,0,0,0.2);
}

.simple-modal-content h3 {
  font-size: 20px;
  color: #1D2D50;
  margin-bottom: 15px;
}

.simple-badge {
  background: #f4f7fb;
  border: 1px solid #d5dae3;
  border-radius: 8px;
  padding: 10px 15px;
  color: #7a8ba8;
  font-size: 14px;
  margin-bottom: 20px;
}

.simple-field {
  display: flex;
  flex-direction: column;
  margin-bottom: 15px;
}

.simple-field label {
  font-size: 12px;
  color: #777;
  margin-bottom: 6px;
}

.simple-field input {
  padding: 10px 14px;
  border: 1px solid #d5dae3;
  border-radius: 8px;
  font-size: 14px;
  outline: none;
  width: 100%;
}

.simple-row {
  display: flex;
  gap: 15px;
}

.simple-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.simple-cancel {
  padding: 10px 25px;
  border: 1px solid #d5dae3;
  border-radius: 8px;
  background: white;
  color: #7a8ba8;
  cursor: pointer;
  font-size: 14px;
}

.simple-save {
  padding: 10px 25px;
  border: none;
  border-radius: 8px;
  background: #1A5FBB;
  color: white;
  cursor: pointer;
  font-size: 14px;
  font-weight: bold;
}

.simple-save:hover {
  background: #1f6fe0;
}
.simple-select {
  padding: 10px 14px;
  border: 1px solid #d5dae3;
  border-radius: 8px;
  font-size: 14px;
  outline: none;
  width: 100%;
  background: white;
}
.showStatusModal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.showStatusModal > div {
  background: white;
  border-radius: 12px;
  padding: 30px;
  width: 500px;
  box-shadow: 0 10px 40px rgba(0,0,0,0.2);
}

.showStatusModal p {
  margin-bottom: 15px;
  font-size: 18px;
  color: #1D2D50;
}

.showStatusModal select {
  width: 100% !important;
  padding: 10px;
  margin-bottom: 20px;
}
</style>