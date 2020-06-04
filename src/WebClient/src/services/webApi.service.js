import axios from "axios";
import AuthService from "./auth.service";

export default class WebApiService {
  constructor() {
    this.hostUrl = "";
  }

  async getAuthConfig() {
    return {
      headers: {
        authorization: `Bearer ${await AuthService.getAccessTokenAsync()}`
      }
    };
  }

  async addPassage(passage, groupId) {
    console.log(`Adding ${JSON.stringify(passage)}`);
    var endpoint = `${this.hostUrl}/Verses/${groupId}`;
    var response = await axios.put(
      endpoint,
      passage,
      await this.getAuthConfig()
    );
    if (!this.isSuccessStatusCode(response.status)) {
      this.displayError(endpoint, response);
      return false;
    }

    return true;
  }

  async removePassage(passageId, groupId) {
    var endpoint = `${this.hostUrl}/Verses/${groupId}/${passageId}`;
    var response = await axios.delete(endpoint, await this.getAuthConfig());
    if (!this.isSuccessStatusCode(response.status)) {
      this.displayError(endpoint, response);
      return false;
    }

    return true;
  }

  async getAllPassages() {
    var endpoint = `${this.hostUrl}/Verses`;
    var config = await this.getAuthConfig();
    var response = await axios.get(endpoint, config);
    try {
      if (response && this.isSuccessStatusCode(response.status)) {
        return response.data;
      } else {
        this.displayError(endpoint, response);
      }
    } catch (error) {
      console.error(`Error calling ${endpoint}: ${error.message}`);
    }

    return null;
  }

  async getPassages(groupId) {
    var endpoint = `${this.hostUrl}/Verses/${groupId}`;
    var response = await axios.get(endpoint, await this.getAuthConfig());
    try {
      if (response && this.isSuccessStatusCode(response.status)) {
        return response.data;
      } else {
        this.displayError(endpoint, response);
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

  displayError(endpoint, response) {
    // TODO : Alert w/ toast
    console.error(
      `Error response from ${endpoint}: ${response.status} ${
        response.statusText
      } ${JSON.stringify(response.data)}`
    );
  }

  isSuccessStatusCode = statusCode => statusCode / 100 == 2;
}
