<template>
  <div class="popup-edit-user-group">
    <!-- <div class="popup-edit-user-group__layout"></div> -->
    <div class="popup-edit-user-group__container">
      <div class="icon-size close" @click="handleHidePopupEditUserGroup">
        <div class="icon-title-size"></div>
      </div>
      <div class="popup-edit-user-group__header">
        <h1 class="title bold">Chỉnh sửa nhóm</h1>
      </div>
      <div class="popup-edit-user-group__main">
        <div class="popup-edit-user-group__main_detail d-f">
          <div class="avatar mg-r-8">
            <img :src="avatarImg" alt="" class="avatar__img" />
          </div>
          <div class="detail__container">
            <div class="full-name__container font-14 d-f">
              <h1 class="full-name no-mg font-14 bold">{{ user?.FullName }}</h1>
              &nbsp;
              <span v-show="user?.Email">({{ user?.Email }})</span>
            </div>
            <div class="offical font-14">{{ user?.OrganizationName }}</div>
          </div>
        </div>
        <div class="popup-edit-user-group__main__group">
          <h1 class="title no-mg semil-bold font-16">
            Nhóm người dùng&nbsp;<span class="required">*</span>
          </h1>
          <div class="group__container">
            <div
              class="group__item d-f"
              v-for="userGroup in userGroupData"
              :key="userGroup.UserGroupID"
            >
              <DxCheckBox
                v-model:value="userGroup.Check"
                class="input-checkbox mg-r-16"
                :text="userGroup.UserGroupName"
              />
              <!-- <div class="content">
                {{ userGroup.UserGroupName }}
              </div> -->
            </div>
          </div>
        </div>
      </div>
      <div class="popup-edit-user-group__footer">
        <BaseButton
          class="mg-r-12"
          :nameBtn="buttomEnum.nameBtn.Cancel"
          :type="buttomEnum.typeBtn.Text"
          @click="handleHidePopupEditUserGroup"
        />
        <BaseButton
          class="mg-r-12"
          :nameBtn="buttomEnum.nameBtn.Save"
          :type="buttomEnum.typeBtn.Primary"
          @click="handleSaveUserGroup"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import BaseButton from "@/components/base/BaseButton.vue";
import { DxCheckBox } from "devextreme-vue/check-box";
import { buttomEnum } from "@/scripts/enum";
import avatarImg from "@/assets/Icons/avatar.png";
export default {
  name: "PopupEditUserGroup",
  components: {
    BaseButton,
    DxCheckBox,
  },
  props: {
    userGroupsTagW: {
      type: Array,
    },
  },
  data() {
    return {
      buttomEnum,
      userGroupData: [],
      avatarImg,
    };
  },
  methods: {
    /**
     * Tạo ra 1 sự kiện đóng popup
     * Author: TNDanh (28/8/2022)
     */
    handleHidePopupEditUserGroup() {
      this.$emit("hiddenPopupEditUserGroup");
    },
    /**
     * Gửi sự kiện lưu nhóm người dùng
     * Author: TNDanh (9/9/2022)
     */
    handleSaveUserGroup() {
      this.$emit(
        "saveUserGroupInUser",
        this.userGroupData.filter((userGroup) => userGroup.Check === true)
      );
      //this.$store.dispatch("editUserGroupInGroup", );
    },
    /**
     * Xử lý khi checkbox
     * Author: TNDanh (9/9/2022)
     */
    // handleChekbox(e) {
    //   this.$emit("checkboxUserGroup", e);
    //   console.log(this.userGroupsTagW);
    // },
  },
  computed: {
    ...mapGetters(["user", "userGroupsTagW"]),
  },
  mounted() {
    this.userGroupData = this.userGroupsTagW.map((userGroup) => {
      return {
        ...userGroup,
        Check: this.user.UserGroupName?.split(", ")?.includes(
          userGroup.UserGroupName
        )
          ? true
          : false,
      };
    });
  },
};
</script>

<style scoped>
.popup-edit-user-group {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.4);
  z-index: 3;
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
}

/* Container của popup edit user group */
.popup-edit-user-group__container {
  position: relative;
  background-color: #fff;
  height: var(--height-popup-edit-user-group);
  width: 500px;
  border-radius: 8px;
}

.popup-edit-user-group__container .close {
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  top: 24px;
  right: 18px;
  border-radius: 50%;
  cursor: pointer;
  z-index: 4;
}

.popup-edit-user-group__container .close:hover {
  background-color: var(--icon-default-hover-bg);
}

.popup-edit-user-group__container .icon-title-size {
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -216px -24px;
  background-color: var(--icon-default-color);
}

/* header */
.popup-edit-user-group__header {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  padding: 24px 24px 0;
  z-index: 1;
  height: var(--header-height-popup-edit-user-group);
}

.popup-edit-user-group__header .title {
  margin: 0;
  font-size: 24px;
  color: var(--title-color);
}

/* footer */
.popup-edit-user-group__footer {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  background-color: var(--popup-footer-bg);
  height: var(--footer-height-popup-edit-user-group);
  padding: 0 24px;
  border-top: 1px solid #dadce3;
  border-bottom-left-radius: 8px;
  border-bottom-right-radius: 8px;
}

/* main */
.popup-edit-user-group__main {
  margin-top: 58px;
  padding: 24px;
  max-height: calc(
    100% - var(--header-height-popup-edit-user-group) -
      var(--footer-height-popup-edit-user-group)
  );
  height: calc(
    100% - var(--header-height-popup-edit-user-group) -
      var(--footer-height-popup-edit-user-group)
  );
  overflow-y: scroll;
}

.popup-edit-user-group__main .detail__container {
  color: var(--title-color);
}

.popup-edit-user-group__main_detail {
  align-items: center;
}

.popup-edit-user-group__main .avatar {
  background-color: #000;
  width: 64px;
  height: 64px;
  overflow: hidden;
  border-radius: 50%;
}

.popup-edit-user-group__main .avatar .avatar__img {
  width: 100%;
  height: 100%;
}

.popup-edit-user-group__main__group .title {
  font-weight: 600;
  margin-top: 16px;
  margin-bottom: 10px;
}

.popup-edit-user-group__main__group .group__item {
  display: flex;
  align-items: center;
  height: var(--base-checkbox-height);
}
</style>