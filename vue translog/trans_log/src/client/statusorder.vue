<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed, onMounted } from 'vue'
import plus from '../assets/images/plus.png'
import time from '../assets/images/time.png'
import history from '../assets/images/history.png'

const departurepoint = ref('')
const arrivalpoint = ref('')
const weight = ref('')
const volumem3 = ref('')
const description = ref('')
const orders = ref([])

const statusOrder = {
  'Ожидает оплаты': 0,
  'Ожидает': 1,
  'Принято': 2,
  'Выполняется': 3,
}


const sortOrders = (data) => {
  return data.sort((a, b) => statusOrder[a.status] - statusOrder[b.status])
}

onMounted(async () => 
{
  const userID = parseInt(localStorage.getItem('userId'))
  const response = await fetch(
    `http://localhost:5095/api/Order/GetHistory?Userid=${userID}`)
    const data = await response.json()
     orders.value = data.filter(o => o.status !== 'Доставлено' && o.status !== 'Отменено')
  console.log(data)
  orders.value = sortOrders(data.filter(o => o.status !== 'Доставлено' && o.status !== 'Отменено'))
})

const logout = () => {
  localStorage.clear()
  router.push('/authorization')
}
const fullname = localStorage.getItem('fullname')

const orderID = ref('')
const cancel = async (orderId) =>
{
  const response = await fetch(`http://localhost:5095/api/Order/UpdateOrder?OrderId=${orderId}`,
  {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json'},
    body: JSON.stringify( { Status: 'Отменено'} )
})
  const userID = parseInt(localStorage.getItem('userId'))
  const res = await fetch(`http://localhost:5095/api/Order/GetHistory?Userid=${userID}`)
  const data = await res.json()
  orders.value = sortOrders(data.filter(o => o.status !== 'Доставлено' && o.status !== 'Отменено'))
 
  
}


const initials = computed(() => {
  if (!fullname) return ''

  return fullname
    .split(' ')
    .map(word => word[0])
    .join('')
    .toUpperCase()
})

const showPaymentModal = ref(false)
const selectedOrder = ref(null)

function payOrder(orderId) {
  selectedOrder.value = orders.value.find(o => o.orderId === orderId);
  showPaymentModal.value = true;
}

async function confirmPay() {
  if (!selectedOrder.value) return;
  await fetch(`http://localhost:5095/api/Order/UpdateOrder?OrderId=${selectedOrder.value.orderId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ Status: 'Принято' })
  });
  const userID = parseInt(localStorage.getItem('userId'));
  const res = await fetch(`http://localhost:5095/api/Order/GetHistory?Userid=${userID}`);
  const data = await res.json();
  orders.value = sortOrders(data.filter(o => o.status !== 'Доставлено' && o.status !== 'Отменено'));
  showPaymentModal.value = false;
  selectedOrder.value = null;
}

function cancelPayModal() {
  showPaymentModal.value = false
  selectedOrder.value = null
}

async function downloadReceipt(orderId) {
  try {
    const response = await fetch(`http://localhost:5095/api/Order/GetReceipt?orderId=${orderId}`);
    if (!response.ok) {
      alert('Ошибка при скачивании чека');
      return;
    }
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `check_${orderId}.pdf`);
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
          <p class="user-role">Клиент</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu" @click="$router.push('/neworder')">
            <img :src="plus"> 
            <a class="menu-item" > Новая заявка</a>
        </div>
        <div class="podmenu_active" >
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

    <div class="content">
      
      <div class="topbar">Статус заявки</div>

      <div class="card">
        <h2 id="titleorder" style="margin-top: -30px;" >Текущие заявки</h2>
        <div style="overflow-y: auto; max-height: 600px;">
          <table>
            <thead>
              <tr>
                <td>№ ЗАЯВКИ</td>
                <td>ДАТА</td>
                <td>МАРШРУТ</td>
                <td>Отправление → Прибытие</td>
                <td>ГРУЗ (Т)</td>
                <td>СТАТУС</td>
              </tr>
            </thead>
            <tbody>
              <tr v-for="order in orders" :key="order.orderId">
                <td>#{{ order.orderId }}</td>
               <td>{{ new Date(order.receivedAt).toLocaleDateString('ru-RU') }}</td>
                 <td>{{ order.departurePoint }} → {{ order.arrivalPoint }}</td>
                 <td>{{ order.departureTime ? new Date(order.departureTime).toLocaleDateString('ru-RU') : 'Ожидайте' }} → {{ order.arrivalTime ? new Date(order.arrivalTime).toLocaleDateString('ru-RU') : 'Ожидайте'}} </td>
               <td>{{ order.weight }}</td>
                <td>
                    <span 
    class="status"
    :class="{
      waiting: order.status === 'Ожидание',
         accepted: order.status === 'Принято',
        paying: order.status === 'Ожидает оплаты',
      progress: order.status === 'Выполняется',
      done: order.status === 'Доставлено',
      cancel: order.status === 'Отменено'
    }"
  >
    {{ order.status }}
  </span>
                </td>
                <td>
                <button
                v-if="order.status === 'Принято'"
                @click="downloadReceipt(order.orderId)"
                class="btn-receipt"
                style="width: 150px; font-size: 14px; height: 40px; background: #2196f3; color: white; border: none; padding: 8px 16px; border-radius: 25px; cursor: pointer; margin-top: 6px; margin-right: 5px">
                Скачать чек
                </button>
                <button 
                v-if="order.status === 'Ожидает оплаты'" 
                @click="payOrder(order.orderId)"
                class="btn-pay"  style="width: 150px; font-size: 14px; height: 40px; background: #2ecc71; color: white; border: none; padding: 8px 16px; border-radius: 25px; cursor: pointer;">
                Оплатить
              </button>
              <button 
 v-if="order.status === 'Ожидание' || order.status === 'Принято' || order.status === 'Ожидает оплаты'" 
 @click="cancel(order.orderId)"
 class="btn-cancel"
style="width: 150px; font-size: 14px; height: 40px; background: #e74c3c; color: white; border: none; padding: 8px 16px; border-radius: 25px; cursor: pointer; margin-left: 5px;">
Отменить
</button>
</td>
              </tr>
            </tbody>
            </table>
        </div>
        </div>
    </div>
  </div>
<div v-if="showPaymentModal && selectedOrder" class="showStatusModal">
  <div class="modal">
    <h2 style="font-size:22px;margin-bottom:16px;">Оплата заказа</h2>
    <div class="simple-field">
      <label>
        Откуда:
        <input type="text" readonly :value="selectedOrder.departurePoint" />
      </label>
      <label>
        Куда:
        <input type="text" readonly :value="selectedOrder.arrivalPoint" />
      </label>
      <label>
        Объем (м³):
        <input type="text" readonly :value="selectedOrder.volumeM3 || selectedOrder.volumem3 || ''" />
      </label>
        <label>
        Вес (кг):
        <input type="text" readonly :value="selectedOrder.weight || selectedOrder.volumem3 || ''" />
      </label>
      <label>
        Дистанция (км):
        <input type="text" readonly :value="selectedOrder.distanceKm || ''" />
      </label>
      <label>
        Цена (₽):
        <input type="text" readonly :value="selectedOrder.price || ''" />
      </label>
    </div>
    <div style="margin-top:28px;display:flex;gap:16px;justify-content:flex-end;">
      <button @click="confirmPay" style="background:#27ae60;color:white;padding:9px 28px;border:none;border-radius:7px;font-size:17px;cursor:pointer;">Оплатить</button>
      <button @click="cancelPayModal" style="background:#eee;color:#222;padding:9px 20px;border:none;border-radius:7px;font-size:16px;cursor:pointer;">Отменить</button>
    </div>
  </div>
</div>
</template>

<style >

.modal-backdrop {
  position: fixed;
  inset: 0; 
  background: rgba(0,0,0,0.45);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999; 
}

.modal {
  background: #fff;
  border-radius: 10px;
  padding: 24px 28px;
  box-shadow: 0 6px 32px rgba(0,0,0,0.18);
  min-width: 320px;
}

.status {
  padding: 8px 16px;
  border-radius: 20px;
  color: white;
  font-size: 13px;
  font-weight: bold;
  display: inline-block;
  min-width: 120px;
  text-align: center;
}

.waiting {
  background-color: gray;
}

.progress {
  background-color: orange;
}

.done {
  background-color: green;
}

.cancel {
  background-color: red;
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
td {
  padding: 15px;
  font-size: 16px ;
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
  width: 330px;
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


</style>