import Vue from 'vue'
import VueRouter from 'vue-router'
import Login from '../views/auth/Login.vue'
import Profile from '../views/share/Profile.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    component: Profile,
    meta: {
      authorize: []
    }
  },
  {
    path: '/login',
    component: Login
  },
  {
    path: '/profile',
    name: 'profile',
    component: () => import('../views/share/Profile.vue'),
    meta: {
      authorize: []
    }
  },
  { path: '*', redirect: '/' }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})


router.beforeEach((to, from, next) => {
  const { authorize } = to.meta;
  const currentUser = localStorage.getItem('user');

  if (authorize){
    if (!currentUser) {
      return next({ path: '/login', query: { returnUrl: to.path } });
    }

    if (authorize.length && !authorize.includes(currentUser.userInfo.role)) {
      return next({ path: '/login' });
    }
  }
  next();
})

export default router
