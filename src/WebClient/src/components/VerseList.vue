<template>
  <div>
    <div v-if="passagesGroups">
      <div
        class="row my-3"
        v-for="group in passagesGroups"
        :key="group.groupId"
      >
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
    passagesGroups() {
      return this.$store.getters.passages;
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
