<template>
  <div>
    <div v-if="!signedIn">
      <button
        class="btn btn-outline-light"
        type="submit"
        v-on:click.prevent="login"
      >
        <font-awesome-icon :icon="['fas', 'user']" class="mr-1" />
        Sign in
      </button>
    </div>
    <div class="navbar-collapse navbar-nav" v-if="signedIn">
      <span class="navbar-item mr-1">
        <span class="navbar-text m-0">Welcome, {{ userName }}</span>
        <router-link class="nav-item nav-link" to="/settings">
          <font-awesome-icon class="fa-lg" :icon="['fas', 'cog']" />
        </router-link>
      </span>
      <button
        class="btn btn-outline-light m-0"
        type="submit"
        v-on:click.prevent="logout"
      >
        Sign out
      </button>
    </div>
  </div>
</template>

<script>
import Configuration from "../configuration";
import AuthService from "../services/auth.service";

async function login() {
  await AuthService.loginAsync(Configuration.authSettings.popup);
  this.refreshUserInfo();
}

function logout() {
  AuthService.logout();
  this.refreshUserInfo();
}

function refreshUserInfo() {
  return this.$store.dispatch("refreshUser");
}

export default {
  name: "signIn",
  computed: {
    signedIn() {
      return this.$store.getters.user != null;
    },
    userName() {
      return this.$store.getters.userName;
    }
  },
  methods: {
    login,
    logout,
    refreshUserInfo
  }
};
</script>

<style>
span {
  display: flex;
}
</style>
