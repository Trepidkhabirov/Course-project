<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import './register.css'
import { ref, computed } from 'vue'

const surnametxt = ref('')
const nametxt = ref('')
const otchestvo = ref('')
const logintxt = ref('')
const passwordtxt = ref('')
const error = ref('')
const fullname = computed(() => `${surnametxt.value} ${nametxt.value} ${otchestvo.value}`)
const register = async () => 
{
    const response = await fetch(
        `http://localhost:5095/api/User/register`,
        {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                username: logintxt.value,
                password: passwordtxt.value,
                fullname: fullname.value,
                roleId: 4,
                isActive: 1
            })  
        }
    )
    const data = await response.json()
    if (response.ok)
    {
        router.push('/authorization')
    }
    else
    {
        error.value = data.message
    }
    console.log(fullname.value, logintxt.value, passwordtxt.value)
}
</script>

<template>
    <div class="auth">
        <div class="label">
            <form id="background"  @submit.prevent="register">
                <img :src="logo" class="logo">
            <h1 id="translog"><span>ТРАНС</span><span id="log">ЛОГ</span></h1>
            <p id="uchet">РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ</p>
            <hr>
            <p class="textinput">ЛИЧНЫЕ ДАННЫЕ</p>
            <div class="divhorizontal">
                <div class="div1">
                    <p class="textinput">Фамилия</p>
                    <input class="inputmini" v-model="surnametxt" placeholder="Иванов" type="text" required>
                </div>
                <div class="div1">
                    <p class="textinput">Имя</p>
                    <input class="inputmini" v-model="nametxt"  placeholder="Иван" type="text" required>
                </div>
            </div>
              <p class="textinput">Отчество</p>
              <input placeholder="Отчество" v-model="otchestvo" type="text">
              <p class="textinput">Данные для входа</p>
              <p class="textinput">Логин</p>
            <input placeholder="Придумайте логин" v-model="logintxt" type="text"  required>
            <p class="textinput">Пароль</p>
            <input placeholder="***********" v-model="passwordtxt" type="password">
            <button type="submit">Регистрация</button>
            <a id="noaccount" @click="$router.push('/authorization')" >Есть аккаунт? Войдите</a>
        </form>
    </div>
</div>
</template>
