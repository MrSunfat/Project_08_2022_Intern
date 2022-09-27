<template>
  <div class="input-main center-vertical" :class="{ active: this.isFocus }">
    <div class="search-icon icon-title-size"></div>
    <input
      type="text"
      class="input-search__container"
      :placeholder="this.placeholderText"
      v-model="value"
      @input="changeValueText"
      @focus="inputFocus"
      @blur="inputNoFocus"
      @keyup.enter="handleEnterInput"
    />
  </div>
</template>

<script>
export default {
  name: "BaseInputSearch",
  props: {
    placeholderText: {
      type: String,
    },
    valueText: {
      type: String,
    },
  },
  data() {
    return {
      value: this.valueText,
      isFocus: false,
    };
  },
  methods: {
    /**
     * Thay đổi giá trị của input ở component cha
     * Author: TNDanh (26/8/2022)
     */
    changeValueText() {
      this.$emit("twoWayValue", this.value);
    },
    /**
     * Active màu của component khi focus
     * Author: TNDanh (26/8/2022)
     */
    inputFocus() {
      this.isFocus = true;
    },
    /**
     * Tắt active màu của component khi không focus
     * Author: TNDanh (26/8/2022)
     */
    inputNoFocus() {
      this.isFocus = false;
    },
    /**
     * Nhấn enter -> gửi 1 sự kiện
     * Author: TNDanh (7/9/2022)
     */
    handleEnterInput() {
      this.$emit("enterKey", this.value);
    },
  },
  mounted() {},
};
</script>

<style scoped>
.input-main {
  border: 1px solid var(--border-input-color);
  height: var(--base-heigth);
  border-radius: 4px;
}

.input-main:hover {
  border-color: var(--primary);
}

.input-main.active {
  border-color: var(--primary) !important;
}

.input-search__container {
  outline: none;
  border: unset;
  padding: 0 12px;
  width: 100%;
}

.input-main .search-icon {
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -191px 0px;
  background-color: var(--icon-color);
  margin-left: 10px;
}
</style>