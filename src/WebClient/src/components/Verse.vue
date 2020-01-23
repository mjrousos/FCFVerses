<template>
  <div>
    <h5>{{ passage.reference }}</h5>
    <p>{{ passage.text }}</p>
  </div>
</template>

<script>
export default {
  name: "verse",
  props: {
    passage: Object
  }
};
</script>
