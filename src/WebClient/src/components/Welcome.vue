<template>
  <div class="jumbotron pt-4">
    <h1>FCF Verses</h1>
    <p class="lead">Memorize Verses in Community</p>
    <p>
      Welcome to FCF Verses, the web app for tracking and memorizing Bible
      passages at Faith Community Fellowship. Log in to get started tracking
      verses for groups you're part of or for personal study.
    </p>
    <a class="btn btn-lg btn-primary" href="#" v-on:click="login" role="button">
      <font-awesome-icon :icon="['fas', 'bible']" class="mr-1" /> Get Started
    </a>
    <a class="btn btn-lg btn-primary" href="/about" role="button">
      <font-awesome-icon :icon="['far', 'question-circle']" class="mr-1" />
      About FCF Verses
    </a>
    <a
      class="btn btn-lg btn-primary"
      href="https://FCFellowship.org"
      role="button"
    >
      <font-awesome-icon :icon="['fas', 'cross']" class="mr-1" />
      FCF Home Page
    </a>
  </div>
</template>

<script>
import Configuration from "../configuration";
import AuthService from "../services/auth.service";

async function login() {
  await AuthService.loginAsync(Configuration.authSettings.popup);
  this.refreshUserInfo();
}

export default {
  name: "welcome",
  methods: {
    login
  }
};
</script>

<style>
.btn {
  margin-right: 0.5rem;
  margin-bottom: 0.5rem;
}
</style>
