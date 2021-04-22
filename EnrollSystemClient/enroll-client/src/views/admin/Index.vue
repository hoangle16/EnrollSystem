<template>
  <v-container fluid>
    <Navbar></Navbar>
    <v-main>
      <router-view />
    </v-main>
  </v-container>
</template>

<script>
import Navbar from "../../layouts/Navbar";
export default {
  name: "Admin",
  components: {
    Navbar,
  },
};
</script>