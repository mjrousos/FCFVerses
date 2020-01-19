import Vue from "vue";
import Vuex from "vuex";
import AuthService from "../services/auth.service";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    appInitialized: false,
    user: null
  },
  getters: {
    appInitialized: state => {
      return state.appInitialized;
    },
    user: state => {
      return state.user;
    },
    userName: state => {
      return AuthService.getUserName(state.user);
    }
  },
  mutations: {
    setAppInitialized: (state, newValue) => {
      state.appInitialized = newValue;
    },
    setUser: (state, newUser) => {
      state.user = newUser;
    }
  },
  actions: {
    markAppInitialized: ({ commit }) => {
      commit("setAppInitialized", true);
    },
    refreshUser: async ({ commit }) => {
      var user = AuthService.getUser();
      if (user) {
        // If an auth token is needed on initial landing page load,
        // it may be desirable to confirm that an access token is available
        // without redirecting since that will avoid a corner case where previous
        // account is known to MSAL but the auth token isn't available. Such a case
        // could cause a different user to be stuck in a loop where every time they
        // load the site, they will be redirected to log in as the previous user.
        // The problem with this solution is that it leads to a longer wait
        // time before the landing page is able to show with proper user information.
        var token = await AuthService.getAccessTokenAsync(null, false, true);
        if (token) {
          return commit("setUser", user);
        }
      }
      return commit("setUser", null);
    }
  },
  modules: {}
});
