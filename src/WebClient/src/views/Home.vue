<template>
  <div>
    <div v-if="signedIn">
      <VerseList />
    </div>
    <div v-else>
      <Welcome />
    </div>
  </div>
</template>

<script>
import VerseList from "@/components/VerseList.vue";
import Welcome from "@/components/Welcome.vue";

const data = {
  title: "FCF Verses"
};

export default {
  name: "home",
  data: () => data,
  components: {
    VerseList,
    Welcome
  },
  computed: {
    signedIn() {
      return this.$store.getters.user != null;
    }
  }
};
</script>
