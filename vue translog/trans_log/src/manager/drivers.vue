<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed, onMounted } from 'vue'
import order from '../assets/images/order.png'
import people from '../assets/images/people.png'
import transport from '../assets/images/transport.png'

const drivers = ref([])
const logout = () => {
  localStorage.clear()
  router.push('/authorization')
}


onMounted(async () => 
{
  const response = await fetch(
    `http://localhost:5095/api/Driver/GetDrivers`)
    const data = await response.json()
  drivers.value = data
  console.log(data)
})
const fullname = localStorage.getItem('fullname')


</script>

<template>
  <div class="layout">
        <div class="sidebar">
      
      <div class="logo">
        <h1>ТРАНС<span>ЛОГ</span></h1>
        <p>ГРУЗОПЕРЕВОЗКИ</p>
      </div>

      <div class="user">
        <div class="avatar">ИИ</div>
        <div>
          <p class="user-name"> {{ fullname }}</p>
          <p class="user-role">Менеджер</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu">
            <img :src="order"> 
            <a class="menu-item active" @click="$router.push('/addorder')">Заявки</a>
        </div>
        <div class="podmenu">
            <img :src="transport">
            <a class="menu-item" @click="$router.push('/trips')">Рейсы</a>
        </div >
        <div class="podmenu_active">
            <img :src="people">
            <a class="menu-item">Водители</a>
        </div>
        </div>
        <hr>
      <a class="logout" @click="logout">Выйти из системы</a>
    </div>

    <div class="content">
      
      <div class="topbar">Водители</div>
        <div class="card">
            <h2>Водители и транспорт</h2>
            <div style="overflow-y: auto; max-height: 700px;">
                <table>
                    <thead>
                        <tr>
                            <td>ФИО</td>
                            <td>Класс</td>
                            <td>Автомобиль</td>
                            <td>Гос. Номер</td>
                            <td>Груз. Т</td>
                            <td>Статус</td>
              </tr>
            </thead>
           <tbody>
  <tr v-for="d in drivers" :key="d.driverId">
    <td>{{ d.user?.fullName || '-' }}</td>
  <td>{{ d.vehicle?.vehicleType?.name || '-' }}</td>
    <td>{{ d.vehicle?.brand || '-'}} {{ d.vehicle.model || '-'}}</td>
    <td>{{ d.vehicle.licensePlate || '-'}} </td>
    <td>{{ d.vehicle?.payloadKg/1000 || '-' }}</td>
    <td><span class="status">{{ d.working || '-'}}</span></td>
  </tr>
</tbody>
        </table>
    </div>
</div>
</div>
</div>
</template>

<style>

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