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

export default {
  name: "home",
  data: function() {
    return {
      title: "FCF Verses"
    };
  },
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
