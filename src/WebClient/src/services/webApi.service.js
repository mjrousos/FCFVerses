import axios from "axios";
import AuthService from "./auth.service";

export default class WebApiService {
  constructor() {}

  // eslint-disable-next-line no-unused-vars
  async lookupPassage(chapter, firstVerse, verseCount, translation, startTrim, endTrim) {
    // TODO - Call backend to look up verse in cache or API
  }
  async getAllPassages() {
    // TODO - Call HTTP client service to retrieve verses from
    //        backend.
    return [
      {
        groupName: "Personal",
        admin: true,
        passages: [
          {
            reference: "John 3:16",
            translation: "NIV",
            text:
              "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."
          },
          {
            reference: "Matthew 6:10",
            translation: "NIV",
            text:
              "your kingdom come, your will be done, on earth as it is in heaven."
          },
          {
            reference: "Ephesians 1:15-19a",
            translation: "NIV",
            text:
              "For this reason, ever since I heard about your faith in the Lord Jesus and your love for all God’s people, I have not stopped giving thanks for you, remembering you in my prayers. I keep asking that the God of our Lord Jesus Christ, the glorious Father, may give you the Spirit[fn] of wisdom and revelation, so that you may know him better. I pray that the eyes of your heart may be enlightened in order that you may know the hope to which he has called you, the riches of his glorious inheritance in his holy people, and his incomparably great power for us who believe."
          },
          {
            reference: "Acts 15:11",
            translation: "NIV",
            text:
              "No! We believe it is through the grace of our Lord Jesus that we are saved, just as they are."
          }
        ]
      },
      {
        groupName: "Timothy Team (Sunday)",
        admin: false,
        passages: [
          {
            reference: "Luke 9:23",
            translation: "NIV",
            text:
              'Then he said to them all: "Whoever wants to be my disciple must deny themselves and take up their cross daily and follow me.'
          }
        ]
      },
      {
        groupName: "Liberty Through the Rock",
        admin: true,
        passages: []
      }
    ];
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
