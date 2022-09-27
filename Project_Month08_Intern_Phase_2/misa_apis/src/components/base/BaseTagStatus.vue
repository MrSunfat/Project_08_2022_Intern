<template>
  <div
    class="tag-status d-f"
    :class="{ bg: dot === 'hide', 'no-bg': dot !== 'hide', [type]: type }"
  >
    <div class="dot" :class="dot"></div>
    <h1
      class="tag-status__content font-14"
      :class="{ 'mg-l-9': this.dot === 'show' }"
    >
      {{ this.content }}
    </h1>
  </div>
</template>

<script>
export default {
  name: "BaseTagStatus",
  props: {
    type: {
      type: String,
    },
    dot: {
      type: String,
    },
    content: {
      type: String,
    },
  },
};
</script>

<style scoped>
.tag-status {
  height: 30px;
  padding-right: 14px;
  align-items: center;
  border-radius: 25px;
}

.tag-status.bolder-text .tag-status__content {
  font-family: Open Sans Bold;
}

.tag-status.bg {
  background-color: var(--tag-bg);
}

.tag-status.no-bg {
  background-color: transparent !important;
}

.tag-status.on {
  background-color: var(--tag-bg);
}

.tag-status.off {
  background-color: rgb(255, 236, 230);
}

.tag-status .dot {
  background: var(--tag-color);
  width: 8px;
  height: 8px;
  border-radius: 50%;
}

.tag-status .dot.show {
  display: block;
}

.tag-status .dot.hide {
  display: none;
}

.tag-status.on .tag-status__content {
  color: var(--tag-color);
}

.tag-status.off .tag-status__content {
  color: rgb(246, 60, 60);
}

.off .dot {
  background-color: rgb(246, 60, 60);
}
</style>