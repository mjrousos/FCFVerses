import axios from "axios";
import AuthService from "./auth.service";

export default class WebApiService {
  constructor() {}

  async addPassage(passage, groupId) {
    console.log(`Adding ${JSON.stringify(passage)} to group ${groupId} for ${AuthService.getUserId(AuthService.getUser())}`);
  }

  async removePassage(passageId, groupId) {
    console.log(`Removing passage ${passageId} from group ${groupId} for ${AuthService.getUserId(AuthService.getUser())}`);
  }

  async getAllPassages() {
    var config = {
      headers: {
        authorization: `Bearer ${await AuthService.getAccessTokenAsync()}`
      }
    };

    var endpoint = `${this.hostUrl}/Verses`;
    var response = await axios.get(endpoint, config);
    try {
      if (response && this.isSuccessStatusCode(response.status)) {
        return response.data;
      } else {
        // TODO : Alert w/ toast
        console.error(
          `Error response from ${endpoint}: ${response.status} ${
            response.statusText
          } ${JSON.stringify(response.data)}`
        );
      }
    } catch (error) {
      console.error(`Error calling ${endpoint}: ${error.message}`);
    }

    return null;
  }

  async getPassages(groupId) {
    var config = {
      headers: {
        authorization: `Bearer ${await AuthService.getAccessTokenAsync()}`
      }
    };

    var endpoint = `${this.hostUrl}/Verses/${groupId}`;
    var response = await axios.get(endpoint, config);
    try {
      if (response && this.isSuccessStatusCode(response.status)) {
        return response.data;
      } else {
        // TODO : Alert w/ toast
        console.error(
          `Error response from ${endpoint}: ${response.status} ${
            response.statusText
          } ${JSON.stringify(response.data)}`
        );
      }
    } catch (error) {
      console.error(`Error calling ${endpoint}: ${error.message}`);
    }

    return null;
  }

  async getUserSettings() {
    // TODO
  }

  // eslint-disable-next-line no-unused-vars
  async updateUserSettings(settings) {
    // TODO
  }

  async anonymousApi() {
    var endpoint = `${this.hostUrl}/HelloWorld`;
    var response = await axios.get(endpoint);
    try {
      if (response && this.isSuccessStatusCode(response.status)) {
        return response.data;
      } else {
        console.error(
          `Error response from ${endpoint}: ${response.status} ${
            response.statusText
          } ${JSON.stringify(response.data)}`
        );
      }
    } catch (error) {
      console.error(`Error calling ${endpoint}: ${error.message}`);
    }

    return null;
  }

  async authorizedApi() {
    var config = {
      headers: {
        authorization: `Bearer ${await AuthService.getAccessTokenAsync()}`
      }
    };

    var endpoint = `${this.hostUrl}/HelloWorld/Secure`;
    var response = await axios.get(endpoint, config);
    try {
      if (response && this.isSuccessStatusCode(response.status)) {
        return response.data;
      } else {
        console.error(
          `Error response from ${endpoint}: ${response.status} ${
            response.statusText
          } ${JSON.stringify(response.data)}`
        );
      }
    } catch (error) {
      console.error(`Error calling ${endpoint}: ${error.message}`);
    }

    return null;
  }

  isSuccessStatusCode = statusCode => statusCode / 100 == 2;
}
