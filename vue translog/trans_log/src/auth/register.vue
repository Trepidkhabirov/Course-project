<script setup>
import logo from '../assets/images/logo.png'
import './register.css'
</script>

<template>
  <div class="auth">
    <div class="label">
        <form id="background">
            <img :src="logo" class="logo">
            <h1 id="translog"><span>ТРАНС</span><span id="log">ЛОГ</span></h1>
            <p id="uchet">РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ</p>
            <hr>
            <p class="textinput">ЛИЧНЫЕ ДАННЫЕ</p>
            <div class="divhorizontal">
                <div>
                    <p class="textinput">Фамилия</p>
                    <input class="inputmini" placeholder="Иванов" type="text" required>
                </div>
                <div>
                    <p class="textinput">Имя</p>
                    <input class="inputmini" placeholder="Иван" type="text" required>
                </div>
            </div>
            <input placeholder="Отчество" type="text">
            <p class="textinput">Данные для входа</p>
              <p class="textinput">Логин</p>
            <input placeholder="Придумайте логин" type="text"  required>
            <p class="textinput">Пароль</p>
            <input placeholder="********" type="password">
            <button type="submit">Регистрация</button>
            <a id="noaccount" >Есть аккаунт?</a>
        </form>
    </div>
  </div>
</template>