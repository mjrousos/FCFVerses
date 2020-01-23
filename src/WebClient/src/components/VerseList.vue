<template>
  <div>
    <div v-if="passagesGroups">
      <div
        class="row my-3"
        v-for="group in passagesGroups"
        :key="group.groupName"
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
import WebApiService from "../services/webApi.service";
import VerseGroup from "./VerseGroup";
import LoadingSpinner from "./LoadingSpinner";

var apiClient = new WebApiService();

const data = {
  passagesGroups: []
};

async function loadVerses() {
  this.passagesGroups = await apiClient.getAllPassages();
}

export default {
  name: "verseList",
  components: {
    LoadingSpinner,
    VerseGroup
  },
  created() {
    this.loadVerses();
  },
  data: () => data,
  methods: {
    loadVerses
  }
};
</script>
