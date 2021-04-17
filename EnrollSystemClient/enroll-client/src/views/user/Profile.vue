<template>
  <div class="container">
    <header class="jumbotron">
      <h3>
        <strong>{{ currentUser.userInfo.userName }}</strong> Profile
      </h3>
    </header>
    <p>
      <strong>Token:</strong>
      {{ currentUser.token.substring(0, 20) }} ...
      {{ currentUser.token.substr(currentUser.token.length - 20) }}
    </p>
    <p>
      <strong>Id:</strong>
      {{ currentUser.userInfo.id }}
    </p>
    <p>
      <strong>Name:</strong>
      {{ currentUser.userInfo.name }}
    </p>
    <strong>Authorities:</strong>
    <ul>
      <li v-for="(role, index) in currentUser.userInfo.role" :key="index">
        {{ role }}
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  name: "Profile",
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  mounted() {
    if (!this.currentUser) {
      this.$router.push("/login");
    }
  },
};
</script>