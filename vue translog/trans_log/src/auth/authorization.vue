<script setup>
import logo from '../assets/images/logo.png'
import './Authorization.css'
</script>

<template>
  <div class="auth">
    <div class="label">
        <form id="background">
            <img :src="logo" class="logo">
            
            <h1 id="translog">ТРАНСЛОГ</h1>
            <p>УЧЕТ ЗАЯВОК НА ГРУЗПЕРЕВОЗКИ</p>
            
            <p>Логин</p>
            <input placeholder="Введите логин" type="text" required>
            
            <p>Пароль</p>
            <input placeholder="********" type="password">
            
            <button type="submit">Войти</button>
            <a id="noaccount" href="#">Нет аккаунта?</a>
        </form>
    </div>
  </div>
</template>