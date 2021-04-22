<template>
  <v-container>
    <v-app-bar height="56px" app color="primary" elevation="0 dark">
      <v-app-bar-nav-icon
        @click.stop="sidebarMenu = !sidebarMenu"
      ></v-app-bar-nav-icon>
      <v-spacer></v-spacer>
      <v-icon>mdi-account</v-icon>
      <v-btn x-small @click.prevent="logout()" color="normal" class="mr-2 ml-2"
        ><v-icon class="mr-1" small>mdi-logout</v-icon>Đăng xuất</v-btn>
    </v-app-bar>
    <v-navigation-drawer
      v-model="sidebarMenu"
      app
      floating
      :permanent="sidebarMenu"
      :mini-variant.sync="mini"
    >
      <v-list
        height="56px"
        class="custom-list-header"
        dense
        color="primary"
        dark
      >
        <v-list-item class="align-middle h-100">
          <v-list-item-action>
            <v-icon @click.stop="sidebarMenu = !sidebarMenu"
              >mdi-chevron-left</v-icon
            >
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>
              <h5 class="font-weight-thin mb-0">Dashboard</h5>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <v-list-item class="px-2" @click="toggleMini = !toggleMini">
        <v-list-item-avatar>
          <v-icon>mdi-account-outline</v-icon>
        </v-list-item-avatar>
        <v-list-item-content class="text-truncate">
          {{ currentUser.userInfo.name }}
        </v-list-item-content>
        <v-btn icon small>
          <v-icon>mdi-chevron-left</v-icon>
        </v-btn>
      </v-list-item>
      <v-divider style="margin-top: 0"></v-divider>
      <v-list>
        <template v-for="item in items">
          <v-list-item
            v-if="currentUser.userInfo.role === item.authorize"
            :key="item.title"
            link
            :to="item.href"
            class="custom-list-item"
            :title="item.title"
          >
            <v-list-item-icon>
              <v-icon color="primary">{{ item.icon }}</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title class="primary--text">{{
                item.title
              }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list>
    </v-navigation-drawer>
  </v-container>
</template>

<script>
export default {
  data() {
    return {
      sidebarMenu: true,
      toggleMini: false,
      items: [
        {
          title: "Home",
          href: "#",
          icon: "mdi-home-outline",
          authorize: "admin",
        },
        {
          title: "Detections",
          href: "#",
          icon: "mdi-shield-account",
          authorize: "teacher",
        },
        {
          title: "Components",
          href: "#",
          icon: "mdi-palette-swatch",
          authorize: "admin",
        },
        {
          title: "Customers",
          href: "#",
          icon: "mdi-account-search-outline",
          authorize: "admin",
        },
        {
          title: "Orders",
          href: "#",
          icon: "mdi-bus-clock",
          authorize: "student",
        },
      ],
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    mini() {
      return this.$vuetify.breakpoint.smAndDown || this.toggleMini;
    },
  },
  methods: {
    logout() {
      this.$store.dispatch("auth/logout");
      this.$router.push("/login");
    },
  },
};
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition-property: opacity;
  transition-duration: 0.25s;
}

.fade-enter-active {
  transition-delay: 0.25s;
}

.fade-enter,
.fade-leave-active {
  opacity: 0;
}
.theme--light.v-application ::-webkit-scrollbar {
  width: 13px;
}

.theme--light.v-application ::-webkit-scrollbar-track {
  background: #e6e6e6;
  border-left: 1px solid #dadada;
}

.theme--light.v-application ::-webkit-scrollbar-thumb {
  background: #b0b0b0;
  border: solid 3px #e6e6e6;
  border-radius: 7px;
}

.theme--light.v-application ::-webkit-scrollbar-thumb:hover {
  background: black;
}

.theme--dark.v-application ::-webkit-scrollbar {
  width: 13px;
}

.theme--dark.v-application ::-webkit-scrollbar-track {
  background: #202020;
  border-left: 1px solid #2c2c2c;
}

.theme--dark.v-application ::-webkit-scrollbar-thumb {
  background: #3e3e3e;
  border: solid 3px #202020;
  border-radius: 7px;
}

.theme--dark.v-application ::-webkit-scrollbar-thumb:hover {
  background: white;
}
.custom-list-item:hover {
  text-decoration: none;
}
</style>