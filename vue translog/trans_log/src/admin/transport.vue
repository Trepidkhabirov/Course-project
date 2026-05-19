<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed, onMounted } from 'vue'
import people from '../assets/images/users.png'
import spravka from '../assets/images/spavka.png'
import transport from '../assets/images/transport.png'

const showTripModal = ref(false)


const transports = ref([])


const vehicleTypes = ref([])
const drivers = ref([])
const fullname = localStorage.getItem('fullname')
const loadTransports = async () => {
  const r = await fetch(`http://localhost:5095/api/Vehicle/GetTransport`)
  transports.value = await r.json()
}
onMounted(async () => 
{
    await loadTransports()

  const r2 = await fetch(`http://localhost:5095/api/VehicleType/GetTypes`)
  vehicleTypes.value = await r2.json()

  const r3 = await fetch(`http://localhost:5095/api/User/GetUser`)
  const users = await r3.json()
 drivers.value = users.filter(u => u.roleId === 3 && !u.driver?.vehicleId)
})


const logout = () => {
  localStorage.clear() 
  router.push('/authorization')
}

const showAddTransport = ref(false)
const selectedTransport = ref(null)

const opentrannsportmodal = (vehicle) =>
{
  selectedTransport.value = vehicle
  showAddTransport.value = true
}
const newTransport = ref({
  licensePlate: '',
  brand: '',
  model: '',
  payloadKg: null,
  volumeM3: null,
  vehicleTypeId: null,
  userId: null
})
const addTransport = async () => {
  if (!newTransport.value.licensePlate || !newTransport.value.brand || !newTransport.value.model) {
    alert('Заполните обязательные поля!')
    return
  }
  await fetch(`http://localhost:5095/api/Vehicle/AddTransport`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(newTransport.value)
  })
  newTransport.value = 
{
    licensePlate: '',
  brand: '',
  model: '',
  payloadKg: null,
  volumeM3: null,
  vehicleTypeId: null,
  userId: null
}
  showAddTransport.value = false
  await loadTransports()

}


const showEditTransport = ref(false)
const editTransport = ref(null)

const openEditTransport = (vehicle) =>
{
  editTransport.value = {
      vehicleId: vehicle.vehicleId,
    licensePlate: vehicle.licensePlate,
    brand: vehicle.brand,
    model: vehicle.model,
    payloadKg: vehicle.payloadKg,
    volumeM3: vehicle.volumeM3,
    vehicleTypeId: vehicle.vehicleTypeId,
    userId: vehicle.drivers?.[0]?.userId ?? null
  }
  showEditTransport.value = true
}
const saveTransport = async () => 
{
   await fetch(`http://localhost:5095/api/Vehicle/UpdateTransport?vehicleId=${editTransport.value.vehicleId}`,
   {
     method: 'PUT',
     headers: { 'Content-Type': 'application/json' },
     body: JSON.stringify(editTransport.value)
    })
    showEditTransport.value = false
    await loadTransports()
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
        <div class="avatar">ИИ</div>
        <div>
          <p class="user-name">{{ fullname }}</p>
          <p class="user-role">Администратор</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu" @click="$router.push('/usersList')">
            <img :src="people"> 
            <a class="menu-item" > Пользователи</a>
        </div>
        <div class="podmenu" @click="$router.push('/spavki')" >
            <img :src="spravka">
            <a class="menu-item" >Справочники</a>
        </div >
        <div class="podmenu_active">
            <img :src="transport">
            <a class="menu-item">Транспорт</a>
        </div>
        </div>
<hr>
      <a class="logout" @click="logout">Выйти из системы</a>
    </div>

    <div class="content">
      
      <div class="topbar">Транспорт</div>

      <div class="card" >
        <div class="title_card">
          <h2 id="titleorder" style="margin-top: -30px;">Транспортные средства</h2>
          <button  class="simple-save" style="height: 40px; width: 200px; font-size: 18px;" @click="showAddTransport = true">Добавить</button>
        </div>
        <div style="overflow-y: auto; max-height: 680px;" >
          <table>
            <thead>
              <tr>
               <td>Гос. Номер</td>
                <td>Марка</td>
                <td>Грузоподъёмность (кг)</td>
                <td>Объём (м³)</td>
                <td>Тип</td>
                <td>Водитель</td>
                </tr>
            </thead>
            <tbody>
              <tr v-for="t in transports" :key="t.vehicleId">
                <td>{{ t.licensePlate }}</td>
               <td>{{ t.brand}} {{ t.model }}</td>
                 <td>{{ t.payloadKg }}</td>
                 <td> {{ t.volumeM3 }}</td>
                 <td>{{ t.vehicleType?.name?? 'Не указан' }}</td>
                 <td>{{ t.drivers?.[0]?.user?.fullName ?? 'Не назначен' }}</td>
                <td>
                  <div style="display: flex; flex-direction: row; gap: 5px;">
                    <button class="manbtn" @click="openEditTransport(t)">Изменить</button>
                  </div>
                </td>
              </tr>
            </tbody>
            </table>
        </div>
        </div>
    </div>
  </div>
  <div v-if="showAddTransport" class="simple-modal">
  <div class="simple-modal-content">
    <h2>Добавить транспорт</h2>

    <div class="simple-row">
      <div class="simple-field">
        <label>Гос. номер</label>
        <input v-model="newTransport.licensePlate" placeholder="А001АА 777">
      </div>
      <div class="simple-field">
        <label>Тип</label>
        <select v-model="newTransport.vehicleTypeId" class="simple-select">
          <option :value="null">Выберите тип</option>
          <option v-for="vt in vehicleTypes" :key="vt.typeId" :value="vt.typeId">{{ vt.name }}</option>
        </select>
      </div>
    </div>

    <div class="simple-row">
      <div class="simple-field">
        <label>Марка</label>
        <input v-model="newTransport.brand" placeholder="КАМАЗ">
      </div>
      <div class="simple-field">
        <label>Модель</label>
        <input v-model="newTransport.model" placeholder="5490">
      </div>
    </div>

    <div class="simple-row">
      <div class="simple-field">
        <label>Грузоподъёмность (кг)</label>
        <input type="number" v-model="newTransport.payloadKg">
      </div>
      <div class="simple-field">
        <label>Объём (м³)</label>
        <input type="number" v-model="newTransport.volumeM3">
      </div>
    </div>

    <div class="simple-field">
      <label>Водитель</label>
      <select v-model="newTransport.userId" class="simple-select">
        <option :value="null">Без водителя</option>
        <option v-for="d in drivers" :key="d.userId" :value="d.userId">{{ d.fullName }}</option>
      </select>
    </div>

    <div class="simple-buttons">
      <button class="simple-cancel" @click="showAddTransport = false">Отмена</button>
      <button class="simple-save" @click="addTransport">Сохранить</button>
    </div>
  </div>
</div>
<div v-if="showEditTransport" class="simple-modal">
  <div class="simple-modal-content">
    <h2>Изменить транспорт</h2>

    <div class="simple-row">
      <div class="simple-field">
        <label>Гос. номер</label>
        <input v-model="editTransport.licensePlate">
      </div>
      <div class="simple-field">
        <label>Тип</label>
        <select v-model="editTransport.vehicleTypeId" class="simple-select">
          <option :value="null">Выберите тип</option>
          <option v-for="vt in vehicleTypes" :key="vt.typeId" :value="vt.typeId">{{ vt.name }}</option>
        </select>
      </div>
    </div>

    <div class="simple-row">
      <div class="simple-field">
        <label>Марка</label>
        <input v-model="editTransport.brand">
      </div>
      <div class="simple-field">
        <label>Модель</label>
        <input v-model="editTransport.model">
      </div>
    </div>

    <div class="simple-row">
      <div class="simple-field">
        <label>Грузоподъёмность (кг)</label>
        <input type="number" v-model="editTransport.payloadKg">
      </div>
      <div class="simple-field">
        <label>Объём (м³)</label>
        <input type="number" v-model="editTransport.volumeM3">
      </div>
    </div>

    <div class="simple-field">
      <label>Водитель</label>
      <select v-model="editTransport.userId" class="simple-select">
        <option :value="null">Без водителя</option>
        <option v-for="d in drivers" :key="d.userId" :value="d.userId">{{ d.fullName }}</option>
      </select>
    </div>

    <div class="simple-buttons">
      <button class="simple-cancel" @click="showEditTransport = false">Отмена</button>
      <button class="simple-save" @click="saveTransport">Сохранить</button>
    </div>
  </div>
</div>
</template>

<style >
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
  display: flex;
  flex-direction: column;
  margin: 30px 40px;
  padding: 30px;
  height: 100%;
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