import { createRouter, createWebHistory } from 'vue-router'
import authorization from '../auth/authorization.vue'
import register from '@/auth/register.vue'
import Neworder from '@/client/neworder.vue'
import Statusorder from '@/client/statusorder.vue'
import Historyorder from '@/client/historyorder.vue'
import Addorder from '@/manager/addorder.vue'
import Drivers from '@/manager/drivers.vue'
import Trips from '@/manager/trips.vue'
import UsersList from '@/admin/usersList.vue'
import Spavki from '@/admin/spavki.vue'
import Transport from '@/admin/transport.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/authorization',
      name: 'authorization',
      component: authorization,
    },
    {
      path: '/register',
      name: 'register',
      component: register,
    },
    {
      path: '/neworder',
      name: 'neworder',
      component: Neworder,
    },
    {
      path: '/statusorder',
      name: 'statusorder',
      component: Statusorder,
    },
    {
      path: '/historyorder',
      name: 'historyorder',
      component: Historyorder,
    },
    {
      path: '/addorder',
      name: 'addorder',
      component: Addorder
    },
    {
      path: '/drivers',
      name: 'drivers',
      component: Drivers
    },
    {
      path: '/trips',
      name: 'trips',
      component: Trips 
    },
    {
      path: '/usersList',
      name: 'usersList',
      component: UsersList
    },
    {
      path: '/spavki',
      name: 'spavki',
      component: Spavki
    },
    {
      path: '/transport',
      name: 'transport',
      component: Transport
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
