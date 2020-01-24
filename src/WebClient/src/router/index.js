import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import AuthService from "../services/auth.service";
import store from "../store";

Vue.use(VueRouter);

async function guardAuthorizedPath(_to, _from, next) {
  // Bizarrely, guard routes can run before the router-view is mounted,
  // (and, therefore, before app initialization is finished), so
  // double-check that the user and access token are populated
  // before making guard decisions based on them.
  await store.dispatch("refreshUser");
  if (store.getters.user != null) {
    next();
  } else {
    AuthService.loginAsync();
  }
}

const routes = [
  {
    path: "/",
    name: "home",
    component: Home
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue")
  },
  {
    path: "/quiz",
    beforeEnter: guardAuthorizedPath,
    name: "quiz",
    component: () => import(/* webpackChunkName: "quiz" */ "../views/Quiz.vue")
  },
  {
    path: "/groups",
    beforeEnter: guardAuthorizedPath,
    name: "groups",
    component: () =>
      import(/* webpackChunkName: "groups" */ "../views/Groups.vue")
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  // Apply Bootstrap class to active links
  linkExactActiveClass: "active",
  routes
});

export default router;
