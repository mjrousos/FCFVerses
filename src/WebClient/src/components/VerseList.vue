<template>
  <div>
    <div v-if="passageGroups">
      <div class="row my-3" v-for="group in passageGroups" :key="group.groupId">
        <VerseGroup :passages="group" />
      </div>
    </div>
    <div v-else>
      <LoadingSpinner title="Getting verses..." />
    </div>
  </div>
</template>

<script>
import VerseGroup from "./VerseGroup";
import LoadingSpinner from "./LoadingSpinner";

async function loadVerses() {
  this.$store.dispatch("refreshAllPassages");
}

export default {
  name: "verseList",
  components: {
    LoadingSpinner,
    VerseGroup
  },
  computed: {
    passageGroups() {
      return this.$store.getters.passages.passageGroups;
    }
  },
  created() {
    this.loadVerses();
  },
  methods: {
    loadVerses
  }
};
</script>
