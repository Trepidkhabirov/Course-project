<script setup>
import logo from '../assets/images/logo.png'
import './Authorization.css'
</script>
 
<template>
  <div class="auth">
    <div class="label">
        <form id="background">
            <img :src="logo" class="logo">
            <h1 id="translog"><span>ТРАНС</span><span id="log">ЛОГ</span></h1>
            <p id="uchet">УЧЕТ ЗАЯВОК НА ГРУЗПЕРЕВОЗКИ</p>
            <hr>
            <p class="textinput">Логин</p>
            <input placeholder="Введите логин" type="text"  required>
            <p class="textinput">Пароль</p>
            <input placeholder="********" type="password">
            <a id="noaccount" @click="$router.push('/register')">Нет аккаунта?</a>
            <button type="submit">Войти</button>
        </form>
    </div>
  </div>
</template>