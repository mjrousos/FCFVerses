<template>
  <div class="card">
    <div class="card-header">
      <h5 class="card-title">{{ passages.groupName }} verses</h5>
    </div>
    <ul class="list-group list-group-flush">
      <li
        class="list-group-item"
        v-for="passage in passages.passages"
        :key="passage.reference"
      >
        <Verse :passage="passage" />
      </li>
    </ul>
  </div>
</template>

<script>
import Verse from "./Verse";

export default {
  name: "verseGroup",
  components: {
    Verse
  },
  props: {
    passages: Object
  }
};
</script>

<style>
.card {
  width: 100%;
}
</style>
