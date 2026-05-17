<script setup>
import logo from '../assets/images/logo.png'
import './Authorization.css'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
const router = useRouter()
const logintxt = ref('')
const passwordtxt = ref('')
const error = ref('')
const auth = async () => 
{
    const response = await fetch(
        `http://localhost:5095/api/User/login?login=${logintxt.value}&password=${passwordtxt.value}`
    )
    const data = await response.json()
    if (response.ok)
    {
        localStorage.setItem('userId', data.userId)
        localStorage.setItem('roleId', data.roleId)
        localStorage.setItem('fullname', data.fullname)
        
        switch (data.roleId)
        {
            case 1: router.push('/admin');
            break
            case 2: router.push('/addorder');
            break
            case 3: router.push('/driver');
            break
            case 4: router.push('/neworder')
        }
    }
    else
    {
      error.value = data.message
      setTimeout(() => {
      error.value = ''
}, 5000)
    }
    console.log(data)
}

</script>
 
<template>
    <div class="auth">
        <div class="label">
            <form id="background" @submit.prevent="auth">
                <img :src="logo" class="logo">
                <h1 id="translog"><span>ТРАНС</span><span id="log">ЛОГ</span></h1>
                <p id="uchet">УЧЕТ ЗАЯВОК НА ГРУЗПЕРЕВОЗКИ</p>
                <hr>
                <p class="textinput">Логин</p>
                <input placeholder="Введите логин" v-model="logintxt" type="text"  required>
                <p class="textinput">Пароль</p>
                <input placeholder="*********" v-model="passwordtxt" type="password">
              <p v-if="error" style="color: red;">{{ error }}</p>
              <a id="noaccount" @click="$router.push('/register')">Нет аккаунта?</a>
            <button type="submit">Войти</button>
        </form>
    </div>
</div>
</template>