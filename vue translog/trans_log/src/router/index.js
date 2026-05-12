import { createRouter, createWebHistory } from 'vue-router'
import authorization from '../auth/authorization.vue'
import register from '@/auth/register.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: authorization,
    },
    {
      path: '/register',
      name: 'register',
      component: register,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
  ],
})

export default router
