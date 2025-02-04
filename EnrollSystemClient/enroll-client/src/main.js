import Vue from 'vue'
import App from './App.vue'
import router from './router/index'
import store from './store'
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import vuetify from './plugins/vuetify';
import VeeValidate from 'vee-validate';

Vue.config.productionTip = false

Vue.use(VeeValidate);

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
