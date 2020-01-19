<template>
  <div v-if="appInitialized">
    <NavBar />
    <router-view />
    <Footer />
  </div>
  <LoadingSpinner v-else></LoadingSpinner>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
import Footer from "@/components/Footer.vue";
import LoadingSpinner from "@/components/LoadingSpinner.vue";

export default {
  name: "app",
  components: {
    NavBar,
    Footer,
    LoadingSpinner
  },
  computed: {
    appInitialized() {
      return this.$store.getters.appInitialized;
    }
  }
};
</script>
