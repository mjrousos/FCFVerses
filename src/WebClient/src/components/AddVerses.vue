<template>
  <h1>Add new verses (from {{ verses["Genesis"].length }} books)</h1>
</template>

<script>
import VerseCounts from "../VerseCounts";

export default {
  name: "addVerses",
  data: function() {
    return {
      verses: VerseCounts
    };
  }
};
</script>
