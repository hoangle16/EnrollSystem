<template>
  <v-app class="app-custom">
    <v-container fluid>
      <router-view />
    </v-container>
  </v-app>
</template>

<script>
export default {
  name: "App",
};
</script>

<style lang="scss">
body, html {
  padding: 0;
  margin: 0;
  width: 100%;
  min-height: 100vh;
}
.app-custom {
  background-color: #f7f7f7 !important;
}
</style>