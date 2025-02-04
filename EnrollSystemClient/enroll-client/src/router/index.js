import Vue from 'vue'
import VueRouter from 'vue-router'
import Login from '../views/auth/Login.vue'
import Profile from '../views/share/Profile.vue'
import * as ROLE from "../constant/role";

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
    name: 'login',
    component: Login
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import('../views/admin/Index.vue'),
    meta: {
      authorize: [ROLE.Admin]
    },
    children: [
      {
        path: 'profile',
        name: 'admin_profile',
        component: () => import('../views/share/Profile.vue'),
        meta: {
          authorize: [ROLE.Admin]
        },
      },
      {
        path: 'users',
        name: 'admin_users',
        component: () => import('../views/admin/users-manager/UsersManager.vue'),
        meta: {
          authorize: [ROLE.Admin],
        },
      },
      {
        path: 'users/:userId',
        name: 'admin_users_details',
        component: () => import('../views/share/UserDetail.vue'),
        props: true,
        meta: {
          authorize: [ROLE.Admin],
        },
      },
      {
        path: 'sections',
        name: 'admin_sections',
        component: () => import('../views/admin/section-manager/SectionsManager.vue'),
        meta: {
          authorize: [ROLE.Admin],
        },
      },
      {
        path: 'rooms',
        name: 'admin_rooms',
        component: () => import('../views/admin/rooms-manager/RoomsManager.vue'),
        meta: {
          authorize: [ROLE.Admin],
        },
      },
      {
        path: 'section-register',
        name: 'admin_section_register',
        component: () => import('../views/admin/section-register/SectionRegister.vue'),
        meta: {
          authorize: [ROLE.Admin],
        },
      },
    ]
  },
  {
    path: '/teacher',
    name: "teacher",
    component: () => import('../views/teacher/Index.vue'),
    meta: {
      authorize: [ROLE.Teacher],
    },
    children: [
      {
        path: 'profile',
        name: 'teacher_profile',
        component: () => import('../views/share/Profile.vue'),
        meta: {
          authorize: [ROLE.Teacher]
        },
      },
      {
        path: 'schedule',
        name: 'teacher_schedule',
        component: () => import('../views/teacher/schedule/Schedule.vue'),
        meta: {
          authorize: [ROLE.Teacher]
        },
      },
      {
        path: 'calendar',
        name: 'teacher_calendar',
        component: () => import('../views/teacher/calendar/Calendar.vue'),
        meta: {
          authorize: [ROLE.Teacher]
        },
      },
      {
        path: 'attendance',
        name: 'teacher_attendance',
        component: () => import('../views/teacher/attendance/Attendance.vue'),
        meta: {
          authorize: [ROLE.Teacher]
        },
      },
      {
        path: 'attendance/:sectionId',
        name: 'teacher_attendance_details',
        component: () => import('../views/teacher/attendance/AttendanceDetail.vue'),
        props: true,
        meta: {
          authorize: [ROLE.Teacher],
        },
      },
    ],
  },
  {
    path: '/student',
    name: 'user',
    component: () => import('../views/user/Index.vue'),
    meta: {
      authorize: [ROLE.Student],
    },
    children: [
      {
        path: 'profile',
        name: 'student_profile',
        component: () => import('../views/share/Profile.vue'),
        meta: {
          authorize: [ROLE.Student]
        },
      },
      {
        path: 'schedule',
        name: 'student_schedule',
        component: () => import('../views/user/sections/SectionList.vue'),
        meta: {
          authorize: [ROLE.Student]
        },
      },
      {
        path: 'attendance/:sectionId',
        name: 'student_attendance',
        component: () => import('../views/user/sections/attendance/Attendance.vue'),
        props: true,
        meta: {
          authorize: [ROLE.Student],
        },
      },
      {
        path: 'register',
        name: 'student_register',
        component: () => import('../views/user/section-register/SectionRegister.vue'),
        meta: {
          authorize: [ROLE.Student]
        },
      },
    ]
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
  const currentUser = JSON.parse(localStorage.getItem('user'));
  if (currentUser && to.path == '/') {
    return next({ path: `${currentUser.userInfo.role}/profile` })
  }
  if (authorize) {
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
