<template>
  <div class="sidebar">
    <router-link
      v-for="title in Object.values(sidebarTitles)"
      :key="title.path"
      :to="pathPage(title.path)"
      class="sidebar__title"
      :class="{ [title.path]: true, active: this.titleActive === title.path }"
      @click="activeTitle(title.path)"
    >
      <div class="sidebar__icon icon-title-size"></div>
      <div class="title-name">
        {{ title.name }}
      </div>
    </router-link>
  </div>
</template>

<script>
import { sidebarTitles } from "@/scripts/constants";
import spriteIcon from "@/assets/Icons/ic_sprites.svg";
export default {
  name: "TheSidebar",
  data() {
    return {
      sidebarTitles,
      spriteIcon,
      titleActive: sidebarTitles.User.path,
    };
  },
  methods: {
    /**
     * Lưu giá trị title đang active
     * Author: TNDanh (25/8/2022)
     */
    activeTitle(title) {
      this.titleActive = title;
    },
    /**
     * Path của pages
     * Author: TNDanh (26/8/2022)
     */
    pathPage(path) {
      return `/business/setting/${path}`;
    },
  },
  computed: {},
};
</script>

<style scoped>
.sidebar {
  background-color: var(--primary-bg);
  padding: 16px 12px 0 12px;
  height: 100%;
}

.sidebar__title {
  display: flex;
  text-decoration: none;
  align-items: center;
  padding-left: 12px;
  height: var(--sidebar-item-height);
  border-radius: 8px;
  cursor: pointer;
}

.sidebar__icon {
  background-color: var(--sidebar-icon-color);
  margin-right: 8px;
}

.sidebar__title.user .sidebar__icon {
  /* background-image: var(spriteIcon); */
  mask: url("../../assets/Icons/ic_sprites.svg") no-repeat -336px -51px;
}

.sidebar__title.user-group .sidebar__icon {
  mask: url("../../assets/Icons/ic_sprites.svg") no-repeat -72px -26px;
}

.sidebar__title.authori .sidebar__icon {
  mask: url("../../assets/Icons/ic_authori.svg") no-repeat 2px -2px;
}

.sidebar__title.signature .sidebar__icon {
  mask: url("../../assets/Icons/ic_sign.svg") no-repeat 2px -2px;
}

.sidebar .title-name {
  font-size: 13px;
  color: var(--text-color);
}

.sidebar__title.active,
.sidebar__title:hover {
  background-color: var(--sidebar-item-active-bg);
}

.sidebar__title.active .sidebar__icon {
  background-color: var(--sidebar-icon-active-color);
}

.sidebar__title.active .title-name {
  color: var(--text-active-color);
  font-family: Open Sans Bold;
}
</style>