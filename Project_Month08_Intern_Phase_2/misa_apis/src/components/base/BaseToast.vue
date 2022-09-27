<template>
  <div class="toast-message">
    <div class="toast-message__cover-icon">
      <div class="toast-message__icon"></div>
    </div>
    <div class="toast-message__content">{{ contentToast }}</div>
  </div>
</template>

<script>
export default {
  name: "BaseToast",
  props: {
    contentToast: String,
  },
  methods: {
    /**
     * Gửi sự kiện tắt toast message
     * Author: TNDanh (23/9/2022)
     */
    handleCloseToast() {
      this.$emit("closeToast");
    },
  },
  mounted() {
    setTimeout(() => {
      this.handleCloseToast();
    }, 3000);
  },
};
</script>

<style scoped>
.toast-message {
  position: fixed;
  display: flex;
  align-items: center;
  top: 70px;
  left: 50%;
  background-color: #25b973;
  background-color: #66d181;
  height: 40px;
  border: 2px solid #25b973;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 0 5px #b1b1b1;
  z-index: 10;
}

.toast-message__cover-icon {
  background-color: #25b973;
  width: 40px;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  border-top-right-radius: 8px;
  border-bottom-right-radius: 8px;
}

.toast-message__icon {
  width: 24px;
  height: 24px;
  mask: url("../../assets/Icons/ic_sprites.svg") no-repeat -666px -82px;
  background: white;
}

.toast-message__content {
  color: white;
  margin: 0 10px;
}
</style>