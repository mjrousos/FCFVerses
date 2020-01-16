<template>
  <div class="container-fluid my-2">
    <div>
      <h1>{{ title }}</h1>
      <h2>Under construction</h2>
      <div class="row">
        <div class="col-12 my-2">
          <button class="btn btn-outline-info mr-2" @click="callApi(false)">
            Call Anonymous API
          </button>
          <button class="btn btn-outline-info" @click="callApi(true)">
            Call Authorized API
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Configuration from "../configuration";
import WebApiService from "../services/webApi.service";
import * as toastr from "toastr";

var apiClient = new WebApiService(Configuration.webApiUrl);

toastr.options = {
  closeButton: true,
  positionClass: "toast-bottom-center"
};

const data = {
  title: "FCF Verses"
};

export default {
  name: "home",
  data: () => data,
  methods: {
    callApi: async function(authorizedEndpoint) {
      var response = authorizedEndpoint
        ? await apiClient.authorizedApi()
        : await apiClient.anonymousApi();
      if (response) {
        toastr.info(response, "Message from web API");
      } else {
        toastr.error("Error calling web API");
      }
    }
  },
  computed: {
    signedIn() {
      return this.$store.getters.user != null;
    }
  }
};
</script>
