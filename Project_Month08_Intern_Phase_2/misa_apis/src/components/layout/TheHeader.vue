<template>
  <div class="header">
    <div class="header__left d-f">
      <div class="navbar icon-size"></div>
      <div class="logo"></div>
      <div class="title-container d-f">
        <router-link
          to="/business/overview"
          class="title-item w-100"
          :class="{ active: this.titleActive === headerTitles.Overview }"
          @click="activeTitle(headerTitles.Overview)"
        >
          <div class="title-item-name">
            {{ headerTitles.Overview }}
          </div>
          <div class="title-line"></div>
        </router-link>
        <router-link
          to="/business/execute"
          class="title-item w-80"
          :class="{ active: this.titleActive === headerTitles.Execute }"
          @click="activeTitle(headerTitles.Execute)"
        >
          <div class="title-item-name">
            {{ headerTitles.Execute }}
          </div>
          <div class="title-line"></div>
        </router-link>
        <router-link
          to="/business/vehicle"
          class="title-item w-115"
          :class="{ active: this.titleActive === headerTitles.Vehicle }"
          @click="activeTitle(headerTitles.Vehicle)"
        >
          <div class="title-item-name">
            {{ headerTitles.Vehicle }}
          </div>
          <div class="title-line"></div>
        </router-link>
        <router-link
          to="/business/advance"
          class="title-item w-80"
          :class="{ active: this.titleActive === headerTitles.Advance }"
          @click="activeTitle(headerTitles.Advance)"
        >
          <div class="title-item-name">
            {{ headerTitles.Advance }}
          </div>
          <div class="title-line"></div>
        </router-link>
        <router-link
          to="/business/payment"
          class="title-item w-80"
          :class="{ active: this.titleActive === headerTitles.Payment }"
          @click="activeTitle(headerTitles.Payment)"
        >
          <div class="title-item-name">
            {{ headerTitles.Payment }}
          </div>
          <div class="title-line"></div>
        </router-link>
        <router-link
          to="/business/setting/user"
          class="title-item w-80"
          :class="{ active: this.titleActive === headerTitles.Setting }"
          @click="activeTitle(headerTitles.Setting)"
        >
          <div class="title-item-name">
            {{ headerTitles.Setting }}
          </div>
          <div class="title-line"></div>
        </router-link>
      </div>
    </div>
    <div class="header__right d-f">
      <div class="notify icon-size">
        <img :src="notifyIcon" alt="Thông báo" class="notify-icon" />
      </div>
      <div class="help icon-size">
        <img :src="helpIcon" alt="Giúp đỡ" class="help-icon" />
      </div>
      <div class="user-avatar">
        <img :src="avatarIcon" alt="Avatar" class="icon-size avatar" />
      </div>
    </div>
  </div>
</template>

<script>
import { headerTitles } from "@/scripts/constants";
import notifyIcon from "@/assets/Icons/ic_notify.svg";
import helpIcon from "@/assets/Icons/ic_help.svg";
import avatarIcon from "@/assets/Icons/avatar.png";
export default {
  name: "TheHeader",
  data() {
    return {
      headerTitles,
      notifyIcon,
      avatarIcon,
      helpIcon,
      titleActive: headerTitles.Setting,
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
  },
};
</script>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  box-shadow: 0 0 4px rgb(0 0 0 / 16%);
  z-index: 1;
}

/*-- Header bên trái --*/
.header__left {
}

.header .navbar {
  width: 36px;
  height: 36px;
  margin: 10px 16px;
  background-image: url("../../assets/Icons/ic_navbar.svg");
  background-position: 50%;
  background-repeat: no-repeat;
  cursor: pointer;
}

.header .logo {
  background-image: url("../../assets/logo_BUSINESS.svg");
  height: 34px;
  width: 175px;
  margin-top: auto;
  margin-bottom: auto;
  margin-right: 40px;
}

/* Title của header */
.header .title-container {
  align-items: center;
}

.header .title-item {
  position: relative;
  text-decoration: none;
  color: var(--text-color);
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 14px;
  cursor: pointer;
}

.header .title-item:hover,
.header .title-item.active {
  color: var(--primary);
  transition: all 0.05s ease-in;
}

.header .title-line {
  position: absolute;
  bottom: 0;
  width: 0;
  height: 4px;
  background-color: var(--line-color);
}

.header .title-item:hover .title-line,
.header .title-item.active .title-line {
  width: 100%;
}

/*-- Header bên phải --*/
.header__right {
  align-items: center;
}

.header__right .notify {
  margin-right: 20px;
}

.header__right .help {
  margin-right: 20px;
}

.header__right .notify,
.header__right .help {
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 50%;
  cursor: pointer;
}

.icon-size:hover {
  background-color: var(--icon-bg-hover);
  border-radius: 50%;
}

.user-avatar {
  display: flex;
  align-items: center;
  height: 100%;
  width: 48px;
  padding: 0 6px;
  cursor: pointer;
  overflow: hidden;
  margin-right: 16px;
}

.user-avatar:hover {
  background-color: var(--icon-bg-hover);
  overflow: hidden;
}

.user-avatar > .avatar {
  border-radius: 50%;
}
</style>