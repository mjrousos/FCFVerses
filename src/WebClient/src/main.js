import Vue from "vue";
import App from "./App.vue";
import "./registerServiceWorker";
import router from "./router";
import store from "./store";
import "bootstrap";
import "./styles/style.scss";

Vue.config.productionTip = false;

async function initializeState() {
  // Populate initial user info from MSAL
  await store.dispatch("refreshUser");
  await store.dispatch("markAppInitialized");
}

new Vue({
  router,
  store,
  created() {
    initializeState();
  },
  render: h => h(App)
}).$mount("#app");
