<template>
  <button class="base-btn d-f" :class="type">
    <div
      class="base-btn__icon icon-title-size"
      :style="{ maskPosition: `${positionIcon}` }"
      v-show="validateUrlIcon"
    ></div>
    <div class="base-btn__text" v-show="validateNameBtn">
      {{ nameBtn }}
    </div>
    <slot></slot>
  </button>
</template>

<script>
export default {
  name: "BaseButton",
  data() {
    return {};
  },
  props: {
    nameBtn: String,
    type: String,
    positionIcon: {
      type: String,
      default: "",
    },
  },
  methods: {},
  computed: {
    validateUrlIcon() {
      if (this.positionIcon === undefined || this.positionIcon.length === 0) {
        return false;
      }
      return true;
    },
    validateNameBtn() {
      if (this.nameBtn === undefined || this.nameBtn.length === 0) {
        return false;
      }
      return true;
    },
  },
};
</script>

<style scoped>
/* {
      'primary-btn': false,
      'second-btn': false,
      'icon-and-text-btn': true,
      'icon-btn': true,
    } */

/* base btn */
.base-btn {
  padding-left: 12px;
  padding-right: 16px;
  height: var(--base-heigth);
  border-radius: 4px;
  cursor: pointer;
  align-items: center;
}

.base-btn.primary-btn .base-btn__icon {
  background-color: #fff;
}

.base-btn__icon {
  mask-image: url("../../assets/Icons/ic_sprites_2.svg");
  mask-repeat: no-repeat;
  margin-right: 8px;
}
.base-btn__text {
  font-family: Open Sans SemiBold;
  font-size: 13px;
}

/* primary btn */
.primary-btn {
  background-color: var(--primary);
  color: #fff;
  border: none;
}

.primary-btn:hover {
  background-color: var(--btn-hover-bg);
}

.primary-btn.disable {
  background-color: var(--bg-btn-stye-disable);
  color: var(--text-disable-color);
  cursor: not-allowed;
}

/* second btn */
.second-btn {
  border: 1px solid var(--second-btn-bd);
  color: var(--style-two-normal-text-color);
  background-color: var(--second-bg-btn);
}

.second-btn:hover {
  background-color: var(--second-bg-hover-btn);
}

.second-btn.disable {
  background-color: var(--second-bg-btn);
  color: var(--text-disable-color);
  cursor: not-allowed;
}

/* icon & text btn */
.icon-and-text-btn {
  border: none;
  border: 1px solid var(--style-four-btn-bd);
  color: var(--style-four-text-color-btn);
  background-color: #fff;
}

.icon-and-text-btn:hover {
  background-color: var(--main-bg-btn);
  color: #fff;
  border: none;
}

.icon-and-text-btn.disable {
  background-color: var(--bg-btn-stye-disable);
  color: var(--text-disable-color);
  border: none;
  cursor: not-allowed;
}

/* icon btn */
.icon-btn,
.text-btn {
  border: 1px solid var(--border-input-color);
  background: #fff;
  min-width: 0 !important;
  padding: 0 4px;
}

.icon-btn > .base-btn__icon {
  margin-right: 0;
  background-color: var(--sidebar-icon-color);
}

.icon-btn:hover {
  background-color: var(--style-five-bg-hover);
}

.icon-btn.disable {
  cursor: not-allowed;
}

/* warn */
.warn-btn {
  border: 1px solid var(--warn-bg);
  background: transparent;
  padding: 0 16px;
}

.warn-btn > .base-btn__text {
  color: var(--warn-text-color);
}

.warn-btn:hover > .base-btn__text {
  color: var(--warn-bg);
}

/* text btn */
.text-btn > .base-btn__text {
  color: #1e1f1f;
  margin: 0 12px;
  padding: 0 12px;
}

.base-btn.text-btn:hover {
  background-color: var(--active-option-dropbox-bg);
}

/* icon-special */
.icon-special {
  border: none;
  background-color: transparent;
  min-width: 0 !important;
  padding: 0 4px;
  border-radius: 50%;
}

.icon-special > .base-btn__icon {
  margin-right: 0;
}

.icon-special:hover {
  background-color: var(--icon-s-btn-hover-bg);
}

.warn-btn.access-btn > .base-btn__text {
  color: #fff;
}

.warn-btn.access-btn {
  border-color: #e61d1d;
}

.warn-btn.access-btn:hover {
  background-color: #d81010;
}
</style>