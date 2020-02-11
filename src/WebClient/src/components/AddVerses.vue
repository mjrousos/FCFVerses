<template>
  <div class="card">
    <div class="card-body">
      <form>
        <div class="form-row">
          <h5 class="card-title">Add new verse</h5>
        </div>

        <!-- Book select -->
        <div class="form-row">
          <div class="form-group col-md-9 col-xs-12">
            <label>Initial verse</label>
            <div class="input-group">
              <select
                id="bookSelect"
                class="form-control"
                v-model="selectedBook"
              >
                <option selected hidden disabled value="">Book</option>
                <option
                  v-for="bookName in Object.keys(verses)"
                  :key="bookName"
                  :value="bookName"
                >
                  {{ bookName }}
                </option>
              </select>

              <!-- Chapter select -->
              <select
                id="chapterSelect"
                class="form-control"
                v-model="selectedChapter"
              >
                <option selected hidden disabled value="0">Chapter</option>
                <option
                  v-for="chapterNumber in chapterNumbers"
                  :key="chapterNumber"
                  :value="chapterNumber"
                >
                  {{ chapterNumber }}
                </option>
              </select>

              <!-- Verse select -->
              <select
                id="verseSelect"
                class="form-control"
                v-model="selectedVerse"
              >
                <option selected hidden disabled value="0">Verse</option>
                <option
                  v-for="verseNumber in verseNumbers"
                  :key="verseNumber"
                  :value="verseNumber"
                >
                  {{ verseNumber }}
                </option>
              </select>
            </div>
          </div>

          <!-- Verse count -->
          <div class="form-group col-md-2 col-xs-4">
            <label for="verseCount"># of verses</label>
            <input id="verseCount" class="form-control" v-model="verseCount" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group col-md-5 col-xs-12">
            <label for="startOffset">
              Words to skip at passage beginning
            </label>
            <input
              id="startOffset"
              class="form-control"
              v-model="beginningOffset"
            />
          </div>
          <div class="form-group col-md-5 col-xs-12">
            <label for="endOffset">
              Words to skip at passage end
            </label>
            <input id="endOffset" class="form-control" v-model="endingOffset" />
          </div>
        </div>

        <div class="form-row">
          <p>
            Preferred translation can be changed in
            <a href="/settings">user settings</a>.
          </p>
        </div>

        <div class="form-row">
          <button class="btn btn-primary" v-on:click.prevent="addVerses">
            Add
          </button>
        </div>
      </form>
    </div>
  </div>
  <!-- <h1>Add new verses (from {{ verses["Genesis"].length }} books)</h1> -->
</template>

<script>
import VerseCounts from "../VerseCounts";

async function addVerses() {
  // TODO
}

export default {
  name: "addVerses",
  computed: {
    chapterNumbers() {
      if (!this.selectedBook) {
        return [];
      } else {
        return Array.from(this.verses[this.selectedBook].keys()).map(
          (_, i) => i + 1
        );
      }
    },
    verseNumbers() {
      if (!this.selectedChapter) {
        return [];
      } else {
        return Array.from(
          new Array(
            this.verses[this.selectedBook][this.selectedChapter - 1]
          ).keys()
        ).map((_, i) => i + 1);
      }
    }
  },
  data: function() {
    return {
      verses: VerseCounts,
      selectedBook: "",
      selectedChapter: 0,
      selectedVerse: 0,
      verseCount: 1,
      beginningOffset: 0,
      endingOffset: 0
    };
  },
  methods: {
    addVerses
  }
};
</script>
