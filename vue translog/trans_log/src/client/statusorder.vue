<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import { ref, computed } from 'vue'
import plus from '../assets/images/plus.png'
import time from '../assets/images/time.png'
import history from '../assets/images/history.png'

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
          <p class="user-name">Иванов И. И.</p>
          <p class="user-role">Клиент</p>
        </div>
      </div>

      <div class="menu">
        <div class="podmenu">
            <img :src="plus"> 
            <a class="menu-item active"> Новая заявка</a>
        </div>
        <div class="podmenu">
            <img :src="time">
            <a class="menu-item">Статус заявок</a>
        </div >
        <div class="podmenu">
            <img :src="history">
            <a class="menu-item">История заявок</a>
        </div>
        </div>

      <a class="logout">Выйти из системы</a>
    </div>

    <div class="content">
      
      <div class="topbar">Новая заявка</div>

      <div class="card">
        <p class="card-title">Оформление заявки на грузоперевозку</p>

        <h3>Данные клиента</h3>
        <div class="row">
          <div class="field">
            <label>ФИО клиента</label>
            <input type="text" value="Иванов Иван Иванович">
          </div>
          <div class="field">
            <label>Номер телефона</label>
            <input type="text" value="+7 (900) 321-67-52">
          </div>
        </div>

        <h3>МАРШРУТ</h3>
        <div class="row">
          <div class="field">
            <label>Пункт отправления</label>
            <input type="text" value="Москва">
          </div>
          <div class="field">
            <label>Пункт прибытия</label>
            <input type="text" value="Уфа">
          </div>
        </div>

        <h3>ПАРАМЕТРЫ ГРУЗА</h3>
        <div class="row">
          <div class="field">
            <label>Вес груза (тонн)</label>
            <input type="text" value="0.5">
          </div>
          <div class="field">
            <label>Объем груза (м2)</label>
            <input type="text" value="1.0">
          </div>
        </div>

        <div class="field">
          <label>Описание груза (необязательно)</label>
          <textarea rows="5"></textarea>
        </div>

        <button class="submit">Подать заявку</button>
      </div>
    </div>
  </div>
</template>

<style>

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
.user-name { font-size: 13px; }
.user-role { font-size: 11px; color: #a0aabf; }

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
.menu-item:hover {
  background: rgba(255,255,255,0.05);
}
.menu-item.active {
  background: rgba(0,0,0,0.25); 
  color: white;
}

.logout {
  padding: 15px 25px;
  font-size: 12px;
  color: #6b7590;
  cursor: pointer;
}

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
.menu-item:hover {
  background: rgba(255,255,255,0.05);
}
.menu-item.active {
  background: rgba(0,0,0,0.25);
  color: white;
}

.logout {
  padding: 15px 25px;
  font-size: 12px;
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

</style>