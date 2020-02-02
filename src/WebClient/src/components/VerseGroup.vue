<template>
  <div class="card">
    <div class="card-header container-fluid">
      <div class="row">
        <div class="col-10">
          <h5 class="card-title">{{ passages.groupName }} verses</h5>
        </div>
        <div v-if="passages.admin" class="col-2">
          <button
            class="btn btn-sm btn-outline-primary float-right"
            v-on:click="addVerses = !addVerses"
          >
            <font-awesome-icon :icon="['fas', 'plus']" class="mr-1" />
            Add Verses
          </button>
        </div>
      </div>
      <div class="row" v-if="addVerses">
        <AddVerses />
      </div>
    </div>
    <ul class="list-group list-group-flush" v-if="passages.passages.length > 0">
      <li
        class="list-group-item"
        v-for="passage in passages.passages"
        :key="passage.reference"
      >
        <Verse :passage="passage" />
      </li>
    </ul>
    <div class="card-body" v-else>
      <p class="card-text">
        No verses here yet. Add some to get started!
      </p>
    </div>
  </div>
</template>

<script>
import Verse from "./Verse";
import AddVerses from "./AddVerses";

export default {
  name: "verseGroup",
  components: {
    AddVerses,
    Verse
  },
  data: function() {
    return {
      addVerses: false
    };
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
