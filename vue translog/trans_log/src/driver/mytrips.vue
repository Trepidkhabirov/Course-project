<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed, onMounted } from 'vue'
import order from '../assets/images/order.png'
import people from '../assets/images/people.png'
import transport from '../assets/images/transport.png'

const logout = () => {
    localStorage.clear()
    router.push('/authorization')
}

const orders = ref([])

onMounted(async () => 
{
    const response = await fetch(
        `http://localhost:5095/api/Order/GetOrder`)
        const data = await response.json()
        const userId = localStorage.getItem('userId')
        orders.value = data.filter(o => 
            o.vehicle?.drivers?.some(d => d.userId == userId)
        )

  
  console.log(data)
})

const fullname = localStorage.getItem('fullname')
const showStatusModal = ref(false)
const newStatus = ref('')
const currentOrder = ref(null)
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
      body: JSON.stringify({status: newStatus.value})
     }
  )
  showStatusModal.value = false
   const res = await fetch('http://localhost:5095/api/Order/GetOrder')
  const data = await res.json()
  const userId = localStorage.getItem('userId')
  orders.value = data.filter(o => 
    o.vehicle?.drivers?.some(d => d.userId == userId)
  )
}
const showDescModal = ref(false)
const currentDesc = ref('')
const openDescModal = (order) =>
{
  currentDesc.value = order.description || 'Описание не указано'
  showDescModal.value = true
}
const initials = computed(() => {
  if (!fullname) return ''

  return fullname
    .split(' ')
    .map(word => word[0])
    .join('')
    .toUpperCase()
})
const showCheckModal = ref(false)
const currentCheck = ref(null)

const openCheck = (order) => {
  currentCheck.value = order
  showCheckModal.value = true
}

const printCheck = () => {
  window.print()
}

async function downloadReceipt(orderId) {
  try {
    const response = await fetch(`http://localhost:5095/api/Order/GetNakladnya?orderId=${orderId}`);
    if (!response.ok) {
      alert('Ошибка при скачивании чека');
      return;
    }
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `nakladnya_${orderId}.pdf`);
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
  } catch (e) {
    alert('Ошибка при скачивании чека');
  }
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
          <p class="user-name"> {{ fullname }}</p>
          <p class="user-role">Водитель</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu_active">
            <img :src="order"> 
            <a class="menu-item active" >Мои рейсы</a>
        </div>
        <div class="podmenu" @click="$router.push('/mytransport')">
            <img :src="transport">
            <a class="menu-item">Транспорт</a>
        </div >
        </div>
        <hr>
      <a class="logout" @click="logout">Выйти из системы</a>
    </div>

    <div class="content">
      
      <div class="topbar">Мои рейсы</div>
        <div class="card">
            <h2>Мои рейсы</h2>
            <div style="overflow-x: auto; max-width: 100%;">

              <div style="overflow-y: auto; max-height: 640px; text-wrap: nowrap;">
                <table>
                  <thead>
                    <tr>
                      <td>Маршрут</td>
                      <td>Отправление → Прибытие</td>
                      <td>Расстояние</td>
                      <td>Груз (Т)</td>
                      <td>Контакты</td>
                      <td>Статус</td>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="o in orders" :key="o.orderId">
                      <td style="text-wrap: nowrap;">{{ o.departurePoint ?? '-'}}  → {{ o.arrivalPoint }}</td>
                      <td style="text-wrap: nowrap">{{ o.departureTime ? new Date(o.departureTime).toLocaleDateString('ru-RU') : 'Ожидайте'}} → {{ o.arrivalTime ? new Date(o.arrivalTime).toLocaleDateString('ru-RU') : 'Ожидайте'}} </td>
    <td>{{ o.distanceKm || '-' }}</td>
    <td>{{ o.weight }}</td>
    <td>{{ o.user?.numberphone ?? '-' }}</td>
    <td> <span 
      class="status"
      :class="{
        waiting: o.status === 'Ожидает',
        progress: o.status === 'Выполняется',
      done: o.status === 'Доставлено',
      cancel: o.status === 'Отменено'
    }"
  >
    {{ o.status }}
  </span></td>
  <td>
    <div style="display: flex; flex-direction: column; gap: 5px;">
      <button class="manbtn" @click="openStatusModal(o)" >Статус</button>
      <button class="manbtn" @click="openDescModal(o)" >Описание</button>
    <button class="manbtn" @click="downloadReceipt(o.orderId)" :disabled="o.status !== 'Доставлено'" 
  :style="o.status !== 'Доставлено' ? 'opacity: 0.4; cursor: not-allowed;' : ''">
  Накладная
</button>
        </div>
      </td>
    </tr>
  </tbody>
</table>
</div>
</div>
</div>
</div>
</div>
<div v-if="showStatusModal" class="showStatusModal" >
  <div>

    <p>Изменить статус #1</p>
    <p>Новый статус</p>
   <select v-model="newStatus" class="simple-select">
  <option value="Выполняется">Выполняется</option>
  <option value="Доставлено">Доставлено</option>
</select>
    <div class="simple-buttons">
      <button class="simple-cancel" @click="showStatusModal = false">Отмена</button>
      <button class="simple-save" @click="saveStatus">Сохранить</button>
    </div> 
  </div>
</div>
<div v-if="showDescModal" class="showStatusModal">
    <div>
      <p>Описание заказа</p>
      <p style="font-size: 16px; color: black;">{{ currentDesc }}</p>
      <div class="simple-buttons">
          <button class="simple-save" @click="showDescModal = false">Закрыть</button>
      </div>
    </div>
</div>
</template>

<style>

.waiting {
  background-color: gray;
  color: white;
}

.progress {
  background-color: orange;
  color: white;
}

.done {
  background-color: green;
  color: white;
}

.cancel {
  background-color: red;
  color: white;
}
 hr{
    width: 340px;
 }

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box; 
}
#app { 
  max-width: none ;
  padding: 0;
  margin: 0;
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
  font-size: 32px;
}
.logo h1 span {
  color: #4da6ff;          
}
.logo p {
  font-size: 12px;
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
.user-name { font-size: 16px; }
.user-role { font-size: 17px; color: #a0aabf; }

.menu {
  margin-top: 15px;
  flex: 1;                
}
.menu-item {
  display: block;           
  padding: 12px 25px;
  color: #c5cce0;
  font-size: 14px;
  cursor: pointer;          
  text-decoration: none;
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
.logout {
  padding: 15px 25px;
  font-size: 12px;
  color: #6b7590;
  cursor: pointer;
}

.logout {
  padding: 15px 25px;
  font-size: 16px;
  color: #6b7590;
  cursor: pointer;
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
  color: black;      
  padding: 20px 40px;
  font-weight: bold;
  font-size: 18px;    
  border-bottom: 1px solid #d5dae3;
}

.card {
  background: white;
  margin: 30px 40px;
  padding: 30px;
  border-radius: 8px;
}
h3 
{
    color: black;
}

</style>