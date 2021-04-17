import Vue from 'vue'
import VueRouter from 'vue-router'
import Login from '../views/auth/Login.vue'
import Profile from '../views/user/Profile.vue'

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
    component: () => import('../views/user/Profile.vue')
  },
  { path: '*', redirect: '/' }
]

// router.beforeEach((to, from, next) => {
//   const publicPages = ['/login'];
//   const authRequired = !publicPages.includes(to.path);
//   const loggedIn = localStorage.getItem('user');

//   if (authRequired && !loggedIn) {
//     next('/login');
//   } else {
//     next();
//   }
// })

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
      return next({ path: '/' });
    }
  }
  next();
})

export default router
