<template>
  <div class="main">
    <the-sidebar />
    <the-content>
      <router-view />
    </the-content>
  </div>
</template>

<script>
import TheContent from "@/components/layout/TheContent.vue";
import TheSidebar from "@/components/layout/TheSidebar.vue";
export default {
  name: "TheMain",
  components: {
    TheContent,
    TheSidebar,
  },
};
</script>

<style>
</style>