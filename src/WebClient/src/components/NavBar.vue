<template>
  <!-- https://getbootstrap.com/docs/4.4/components/navbar/ -->
  <header class="nav navbar-expand-sm navbar-dark bg-dark p-2">
    <a class="navbar-brand" v-on:click="route('')" href="/">
      <img
        class="logo-icon"
        alt="FCF Logo"
        src="./../assets/FCF_Logo_Icon.png"
      />
      FCF Verses
    </a>
    <button
      class="navbar-toggler ml-auto"
      type="button"
      data-toggle="collapse"
      data-target="#navbarSupportedContent"
      aria-controls="navbarSupportedContent"
      aria-expanded="false"
      aria-label="Toggle navigation"
    >
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarSupportedContent">
      <ul class="navbar-nav mr-auto">
        <li class="nav-item">
          <router-link class="nav-link" to="/">Verses</router-link>
        </li>
        <li class="nav-item">
          <router-link
            :class="{ disabled: !signedIn }"
            class="nav-link"
            to="/quiz"
          >
            Quiz
          </router-link>
        </li>
        <li class="nav-item">
          <router-link
            :class="{ disabled: !signedIn }"
            class="nav-link"
            to="/groups"
          >
            Groups
          </router-link>
        </li>
        <li class="nav-item">
          <router-link class="nav-link" to="/about">About</router-link>
        </li>
      </ul>
      <div class="ml-auto">
        <SignIn />
      </div>
    </div>
  </header>
</template>

<script>
import SignIn from "@/components/SignIn.vue";

export default {
  name: "navBar",
  components: {
    SignIn
  },
  computed: {
    signedIn() {
      return this.$store.getters.user != null;
    }
  }
};
</script>

<style>
.navbar-text {
  margin-right: 1rem;
}

.logo-icon {
  max-width: 50px;
  margin-right: 0.5rem;
}

.disabled {
  pointer-events: none;
  opacity: 0.8;
}
</style>
