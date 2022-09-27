<template>
  <the-header />
  <router-view />
</template>

<script>
import TheHeader from "./components/layout/TheHeader.vue";
// import TheContent from "./components/layout/TheContent.vue";
// import TheSidebar from "./components/layout/TheSidebar.vue";

export default {
  name: "App",
  components: {
    TheHeader,
    // TheContent,
    // TheSidebar,
  },
};
</script>

<style>
@import url("./styles/main.css");
</style>
