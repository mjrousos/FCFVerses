<template>
  <div v-if="appInitialized">
    <NavBar />
    <!-- mb-5 to leave room for the fixed footer -->
    <div class="container mt-2 mb-5">
      <router-view />
    </div>
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
