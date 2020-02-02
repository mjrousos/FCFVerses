import Vue from "vue";
import App from "./App.vue";
import "./registerServiceWorker";
import router from "./router";
import store from "./store";
import "bootstrap";
import "./styles/style.scss";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faBible,
  faCog,
  faCross,
  faEnvelope,
  faPlus,
  faUser
} from "@fortawesome/free-solid-svg-icons";
import { faGithub } from "@fortawesome/free-brands-svg-icons";
import { faQuestionCircle } from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(
  faBible,
  faCog,
  faCross,
  faEnvelope,
  faGithub,
  faQuestionCircle,
  faPlus,
  faUser
);

Vue.component("font-awesome-icon", FontAwesomeIcon);

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
