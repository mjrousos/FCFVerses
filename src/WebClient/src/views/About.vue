<template>
  <div class="about">
    <div class="row">
      <div class="jumbotron py-4">
        <h1>FCF Verses</h1>
        <p>
          FCF Verses is developed by Mike Rousos as a fun web development
          exercise, and as a tool for the FCF congregation to memorize Bible
          verses. To use the app, you will need to
          <a href="#" v-on:click.prevent="login">create a free account</a> using
          your email address so that your groups and personal verses can be
          associated with you.
        </p>
        <p>
          Once you're signed in, you can use the
          <a href="/groups">groups</a> page to join FCF groups or create and
          manage your own groups. You can use the <a href="/">verses</a> page to
          view the verses you're working on or add or remove verses for your own
          personal study (separate from the groups you're in).
        </p>
        <p>
          Please feel free to contact me if you have any questions or feedback.
        </p>
        <button class="btn btn-lg btn-primary" v-on:click="contact">
          <font-awesome-icon :icon="['fas', 'envelope']" class="mr-1" />
          Contact me
        </button>
      </div>
    </div>
    <div class="row">
      <div class="col-md-6 col-sm-12">
        <h3>Privacy and Security</h3>
        <p>
          FCF Verses does not use cookies and the only information that is
          stored is which groups your account is associated with and which
          personal verses you have picked to study. User accounts (and all
          associated information) will be deleted upon request. Account login
          details (email addresses and password hashes) are stored securely in
          the Microsoft Azure cloud (using Azure Active Directory B2C).
        </p>
      </div>
      <div class="col-md-6 col-sm-12">
        <h3>Contribute</h3>
        <p>
          If you're a developer interested in contributing to this project (or
          just interested in taking a look around the source code), everything
          is open-sourced
          <a href="https://github.com/mjrousos/fcfverses">on GitHub</a>. Feel
          free to log issues or make pull requests. The app is a Vue.js SPA
          front-end with an ASP.NET Core backend. Data is stored in Azure SQL
          and authentication is done via bearer tokens from Azure AD B2C. The
          app uses the
          <a href="https://scripture.api.bible/">API.Bible</a> service to look
          up verses, and is hosted in an Azure Kubernetes Service cluster.
        </p>
        <a href="https://github.com/mjrousos/fcfverses" class="btn btn-primary">
          <font-awesome-icon class="fa-lg mr-1" :icon="['fab', 'github']" />
          Source code
        </a>
      </div>
    </div>
  </div>
</template>

<script>
import AuthService from "../services/auth.service";
import Configuration from "../configuration";

function contact() {
  window.location.href = "mailto:VersesFeedback@MJRousos.com";
}

async function login() {
  await AuthService.loginAsync(Configuration.authSettings.popup);
}

export default {
  name: "about",
  methods: {
    contact,
    login
  }
};
</script>
