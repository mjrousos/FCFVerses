<template>
  <div>
    <div class="row m-0">
      <div class="col-10 p-0">
        <h5>{{ passage.reference }}</h5>
      </div>
      <div v-if="removable && !removing" class="col-2 p-0">
        <button
          class="btn btn-outline-light btn-circle-sm float-right deleteButton"
          @click="removeVerse"
        >
          <font-awesome-icon class="fa-lg" :icon="['fas', 'times']" />
        </button>
      </div>
    </div>
    <div class="row m-0">
      <p>{{ passage.text }}</p>
    </div>
  </div>
</template>

<script>
function removeVerse() {
  this.removing = true;
  this.$emit("remove-verse", this.passage.passageId);
}

export default {
  name: "verse",
  data: function() {
    return {
      removing: false
    };
  },
  methods: {
    removeVerse
  },
  props: {
    removable: Boolean,
    passage: Object
  }
};
</script>
