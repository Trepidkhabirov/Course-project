<script setup>
import router from '@/router';
import logo from '../assets/images/logo.png'
import './register.css'
import { ref, computed, watch } from 'vue'

const surnametxt = ref('')
const nametxt = ref('')
const otchestvo = ref('')
const logintxt = ref('')
const passwordtxt = ref('')
const numberphone = ref('')
const error = ref('')
const messageError = ref('')
const fullname = computed(() => `${surnametxt.value} ${nametxt.value} ${otchestvo.value}`)
watch(numberphone, (newValue, oldValue) => {
  if (!newValue.startsWith('+7')) {
    numberphone.value = '+7' + newValue.replace(/[^0-9]/g, '')
  }
  
  let cleaned = '+7' + newValue.slice(2).replace(/[^0-9]/g, '')
  
  if (cleaned.length > 12) {
    cleaned = cleaned.slice(0, 12)
  }
  
  if (cleaned !== newValue) {
    numberphone.value = cleaned
  }
})
const register = async () => 
{
    if (!surnametxt.value || !nametxt.value || !logintxt.value || !passwordtxt.value || !numberphone.value) {
    messageError.value = 'Заполните все поля!'
    return
  }
      const phoneRegex = /^\+7\d{10}$/
    if (!phoneRegex.test(numberphone.value)) {
        messageError.value = 'Введите корректный номер телефона'
        setTimeout(() => {
            messageError.value = ''
        }, 5000)
        return
    }
    const response = await fetch(
        `http://localhost:5095/api/User/register`,
        {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                username: logintxt.value,
                password: passwordtxt.value,
                fullname: fullname.value,
                numberphone: numberphone.value,
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
        messageError.value = data.message
    }
    console.log(data)
}

const handlePhoneFocus = (event) => {
    if (!numberphone.value || numberphone.value === '') {
        numberphone.value = '+7'
    }
    setTimeout(() => {
        if (event.target) {
            event.target.setSelectionRange(event.target.value.length, event.target.value.length)
        }
    }, 0)
}

const handlePhoneInput = (event) => {
    let value = event.target.value
    
    if (!value.startsWith('+7')) {
        numberphone.value = '+7'
        return
    }
    
    let cleaned = '+7' + value.slice(2).replace(/[^0-9]/g, '')
    
    if (cleaned.length > 12) {
        cleaned = cleaned.slice(0, 12)
    }
    
    numberphone.value = cleaned
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
                    <input class="inputmini" v-model="surnametxt" placeholder="Иванов" type="text" >
                </div>
                <div class="div1">
                    <p class="textinput">Имя</p>
                    <input class="inputmini" v-model="nametxt"  placeholder="Иван" type="text" >
                </div>
            </div>
            <div class="divhorizontal">
                <div class="div1">
                    <p class="textinput">Отчество</p>
                    <input placeholder="Отчество" v-model="otchestvo" class="inputmini" type="text">
                </div> 
                    <div class="div1">
                        <p class="textinput">Номер телефона</p>
                       <input type="text" class="inputmini" v-model="numberphone" placeholder="+7 (900) 321-67-52" maxlength="12" @focus="handlePhoneFocus" @input="handlePhoneInput">
                    </div>

            </div>
              <p class="textinput">Данные для входа</p>
              <p class="textinput">Логин</p>
            <input placeholder="Придумайте логин" v-model="logintxt" type="text"  >
            <p class="textinput">Пароль</p>
            <input placeholder="***********" v-model="passwordtxt" type="password">
            <button type="submit">Регистрация</button>
            <p v-if="messageError" style="color: red; font-size: 14px; margin-bottom: 0px;">{{ messageError }}</p>
            <a id="noaccount" @click="$router.push('/authorization')" >Есть аккаунт? Войдите</a>
        </form>
    </div>
</div>
</template>
