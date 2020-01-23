<template>
  <div>
    <h2 class="loaderTitle">{{ title }}</h2>
    <div class="loader" />
    <div class="loaderBackground">
      <img src="../assets/FCF_Logo_Icon.png" />
    </div>
  </div>
</template>

<script>
export default {
  name: "loaderSpinner",
  props: ["title"]
};
</script>

<style>
.loaderTitle {
  text-align: center;
  position: absolute;
  left: 50%;
  top: 50%;
  margin-top: -120px;
  margin-left: -200px;
  width: 400px;
}

.loader {
  position: absolute;
  left: 50%;
  top: 50%;
  border: 16px solid #d3d3d3; /* Light grey */
  border-top: 16px solid #3498db;
  border-bottom: 16px solid #3498db;
  border-radius: 50%;
  margin: -60px 0 0 -60px;
  width: 120px;
  height: 120px;
  animation: spin 2s linear infinite;
  z-index: 0;
}

.loaderBackground {
  position: absolute;
  left: 50%;
  top: 50%;
  border-radius: 50%;
  margin: -50px 0 0 -50px;
  width: 100px;
  height: 100px;
  z-index: 1;
}

.loaderBackground img {
  height: 100%;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
