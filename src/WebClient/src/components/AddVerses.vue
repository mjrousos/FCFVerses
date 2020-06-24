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
                v-model="passage.book"
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
                v-model="passage.chapter"
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
                v-model="passage.verse"
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
            <input
              id="verseCount"
              class="form-control"
              v-model="passage.length"
            />
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
              v-model="passage.startOffset"
            />
          </div>
          <div class="form-group col-md-5 col-xs-12">
            <label for="endOffset">
              Words to skip at passage end
            </label>
            <input
              id="endOffset"
              class="form-control"
              v-model="passage.endOffset"
            />
          </div>
        </div>

        <div class="form-row">
          <p>
            Preferred translation can be changed in
            <a href="/settings">user settings</a>.
          </p>
        </div>

        <div class="form-row">
          <button
            :disabled="
              !(
                passage.book &&
                passage.chapter &&
                passage.verse &&
                passage.length > 0 &&
                enabled
              )
            "
            class="btn btn-primary"
            v-on:click.prevent="addVerses"
          >
            {{ buttonLabel }}
          </button>
        </div>
      </form>
    </div>
  </div>
  <!-- <h1>Add new verses (from {{ verses["Genesis"].length }} books)</h1> -->
</template>

<script>
import VerseCounts from "../VerseCounts";

function resetPassage() {
  this.passage.book = "";
  this.passage.chapter = 0;
  this.passage.verse = 0;
  this.passage.length = 1;
  this.passage.startOffset = 0;
  this.passage.endOffset = 0;
}

function addVerses() {
  // Copy the passage, both to change the book to an int and
  // to avoid the passage passed to add-verses being cleared by resetPassage.
  this.$emit("add-verses", {
    book: Object.keys(this.verses).indexOf(this.passage.book),
    chapter: this.passage.chapter,
    verse: this.passage.verse,
    length: parseInt(this.passage.length, 10),
    startOffset: parseInt(this.passage.startOffset, 10),
    endOffset: parseInt(this.passage.endOffset, 10)
  });
  this.resetPassage();
}

function disable() {
  this.enabled = false;
}

function enable() {
  this.enabled = true;
}

export default {
  name: "addVerses",
  computed: {
    buttonLabel() {
      return this.enabled ? "Add" : "Adding...";
    },
    chapterNumbers() {
      if (!this.passage.book) {
        return [];
      } else {
        return Array.from(this.verses[this.passage.book].keys()).map(
          (_, i) => i + 1
        );
      }
    },
    verseNumbers() {
      if (!this.passage.chapter) {
        return [];
      } else {
        return Array.from(
          new Array(
            this.verses[this.passage.book][this.passage.chapter - 1]
          ).keys()
        ).map((_, i) => i + 1);
      }
    }
  },
  data: function() {
    return {
      enabled: true,
      verses: VerseCounts,
      passage: {
        book: "",
        chapter: 0,
        verse: 0,
        length: 1,
        startOffset: 0,
        endOffset: 0
      }
    };
  },
  methods: {
    disable,
    enable,
    addVerses,
    resetPassage
  }
};
</script>
