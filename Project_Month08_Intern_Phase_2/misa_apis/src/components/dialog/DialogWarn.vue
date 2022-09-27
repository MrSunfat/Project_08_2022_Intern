<template>
  <div class="dialog-warn">
    <div class="layout"></div>
    <div class="dialog-warn__container">
      <div class="icon-size close icon-size-hover-bg d-f" @click="closeDialog">
        <div class="icon-title-size"></div>
      </div>
      <div class="dialog-warn__container__header bold">Loại bỏ người dùng</div>
      <div class="dialog-warn__container__body">
        Bạn có chắc chắn muốn loại bỏ
        <span v-show="type === 'user-group'" v-html="contentMessage"></span>
        <span class="bold" v-show="user?.FullName">{{ user?.FullName }}</span>
        ra khỏi
        <span class="bold" v-show="type === 'user-group'"
          >nhóm {{ userGroupCurrent?.UserGroupName }}</span
        >

        <span class="bold" v-show="type === 'user'">trang Người dùng</span>
        không?
      </div>
      <div class="dialog-warn__container__footer">
        <BaseButton
          class="mg-r-12"
          :nameBtn="buttomEnum.nameBtn.No"
          :type="buttomEnum.typeBtn.Text"
          @click="closeDialog"
        />
        <BaseButton
          class="access-btn"
          :nameBtn="buttomEnum.nameBtn.YesAndRemove"
          :type="buttomEnum.typeBtn.Warn"
          @click="agreeDeleteMember"
        />
      </div>
    </div>
  </div>
</template>

<script>
import BaseButton from "@/components/base/BaseButton.vue";
import { buttomEnum } from "@/scripts/enum";
import { mapGetters } from "vuex";
export default {
  name: "DialogWarn",
  components: {
    BaseButton,
  },
  props: {
    members: {
      type: Array || Object,
    },
    type: {
      type: String,
    },
    user: {
      type: Object,
    },
  },
  data() {
    return {
      buttomEnum,
    };
  },
  methods: {
    /**
     * Gửi sự kiện đồng ý xóa
     * Author: TNDanh (11/9/2022)
     */
    agreeDeleteMember() {
      let memberIDs = [];
      if (this.members?.MemberName) {
        memberIDs.push(this.members.MemberID);
      }
      if (Array.isArray(this.members)) {
        memberIDs = this.members.map((member) => {
          return member.MemberID;
        });
        this.$store.commit("setMemberCheckInUserGroup", []);
      }
      this.$emit("agreeDeleteMember", memberIDs);
    },
    /**
     * Gửi sự kiện đóng dialog
     * Author: TNDanh (11/9/2022)
     */
    closeDialog() {
      this.$emit("closeDialog");
    },
  },
  computed: {
    contentMessage() {
      if (Array.isArray(this.members)) {
        return `<span class="bold">${this.members.length}</span> người dùng đã chọn`;
      }
      if (this.members?.MemberName) {
        return `<span class="bold">${this.members.MemberName}</span>`;
      }
      return "";
    },
    ...mapGetters(["userGroupCurrent"]),
  },
};
</script>

<style scoped>
.dialog-warn {
  position: fixed;
  display: flex;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 2;
}

.dialog-warn .layout {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.45);
}

.dialog-warn__container {
  position: relative;
  background: #fff;
  margin: auto;
  width: 500px;
  height: 195px;
  z-index: 3;
  border-radius: 6px;
  overflow: hidden;
}

.dialog-warn__container .close {
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  top: 18px;
  right: 14px;
}

.dialog-warn__container .close .icon-title-size {
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -216px -24px;
  background-color: var(--icon-default-color);
}

.dialog-warn__container__header {
  margin: 24px 24px 0;
  font-size: 20px;
}

.dialog-warn__container__body {
  padding: 24px 24px 0;
}

.dialog-warn__container__footer {
  position: absolute;
  left: 0;
  right: 0;
  bottom: 0;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0 24px;
  background: #f2f2f2;
}

.access-btn {
  background-color: #e61d1d;
  /* color: #fff; */
}

.access-btn .warn-btn:hover > .base-btn__text {
  color: #fff;
}
</style>