<template>
  <div class="detail-preview-user-group">
    <div
      class="icon-size close icon-size-hover-bg d-f"
      @click="handleHideDetailUserGroup"
    >
      <div class="icon-title-size"></div>
    </div>
    <div class="detail-preview-user-group__header">
      <div class="header__top">
        <h1 class="user-group-name no-mg font-20 bold">
          {{ userGroupCurrent?.UserGroupName }}
        </h1>
        <BaseTagStatus
          class="tag no-mg bolder-text"
          :content="userGroupCurrent?.Status"
          :dot="statusTagEnum.Dot.Show"
          type="on"
        />
      </div>
      <div class="header__bottom mg-t-16">
        <span class="description">
          {{ userGroupCurrent?.Description }}
        </span>
      </div>
    </div>
    <div class="detail-preview-user-group__body">
      <div class="title bold mg-b-12">
        Thành viên&nbsp;<span class="bold"
          >({{ userGroup?.Members?.length || 0 }})</span
        >
      </div>
      <div class="d-f control mg-b-14">
        <BaseInputSearch
          :placeholderText="placholderText.InputMember"
          :valueText="userGroupDetail.searchWord"
          @twoWayValue="handleSearchMemberInUserGroup"
          @enterKey="handleEnterKeyWhenSearch"
        />
        <BaseButton
          :nameBtn="buttomEnum.nameBtn.AddMember"
          :type="buttomEnum.typeBtn.Primary"
          positionIcon="-237px 0px"
          @click="handleShowPopupAddMember"
        />
      </div>
      <div
        class="control-plus"
        v-show="
          this.membersInUserGroup?.filter((member) => member.Check === true)
            .length
        "
      >
        <span class="selected-mem mg-r-16">
          {{
            membersInUserGroup?.filter((member) => member.Check === true).length
          }}
          đang chọn
        </span>
        <p class="unchecked mg-r-16" @click="handleUnCheckAll">
          {{ buttomEnum.nameBtn.UnChecked }}
        </p>
        <BaseButton
          :nameBtn="buttomEnum.nameBtn.RemoveMember"
          :type="buttomEnum.typeBtn.Warn"
          @click="deleteMembers"
        />
      </div>
      <div class="container">
        <div
          class="member"
          v-for="member in membersInUserGroup"
          :key="member.MemberID"
        >
          <DxCheckBox
            v-model:value="member.Check"
            class="input-checkbox mg-r-14"
            @valueChanged="handleValueChanged"
          />
          <div class="member__content">
            <div class="avatar mg-r-14">
              <img :src="avatarImg" alt="avatar" class="avatar__img" />
            </div>
            <div class="member__detail">
              <div class="full-name">
                {{ member?.MemberName }}&nbsp;<span v-show="member?.Email"
                  >({{ member.Email }})</span
                >
              </div>
              <div class="position nowrap" :title="member.MemberDescription">
                {{ member?.MemberDescription }}
              </div>
            </div>
          </div>
          <div
            class="icon-size delete-member"
            @click="handleRemoveMemberInUserGroup(member)"
          >
            <div class="icon-title-size"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { statusTagEnum, buttomEnum } from "@/scripts/enum";
import { placholderText } from "@/scripts/constants";
import BaseTagStatus from "@/components/base/BaseTagStatus.vue";
import BaseInputSearch from "@/components/base/BaseInputSearch.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import { DxCheckBox } from "devextreme-vue/check-box";
import avatarImg from "@/assets/Icons/avatar.png";

export default {
  name: "BaseDetailUserGroup",
  components: {
    BaseTagStatus,
    BaseInputSearch,
    BaseButton,
    DxCheckBox,
  },
  data() {
    return {
      placholderText,
      statusTagEnum,
      buttomEnum,
      selectedMemberNumber: 0,
      avatarImg,
      userGroupDetail: {
        searchWord: "",
      },
      membersInUserGroup: [],
      membersChecked: [],
      event: null,
    };
  },
  methods: {
    /**
     * Tạo sự kiện đóng DetailUserGroup
     * Author: TNDanh (28/8/2022)
     */
    handleHideDetailUserGroup() {
      this.$emit("hideDetailUserGroup");
      this.$store.commit("setMemberCheckInUserGroup", []);
    },
    /**
     * Tạo sự kiện mở Popup Add Member
     * Author: TNDanh (29/8/2022)
     */
    handleShowPopupAddMember() {
      this.$emit("showPopupAddMember");
    },
    /**
     * Xử lý bỏ check toàn bộ member
     * Author: TNDanh (29/8/2022)
     */
    handleUnCheckAll() {
      // 1. Bỏ check toàn bộ member
      this.handleInitUserWithChekbox();
      this.$store.commit("setMemberCheckInUserGroup", []);
      // Xét số member đang chọn là 0
      this.selectedMemberNumber = 0;
    },
    /**
     * Khởi tạo user kèm checkbox
     * Author: TNDanh (29/8/2022)
     */
    handleInitUserWithChekbox() {
      this.$store.commit("setUserGroup", {
        ...this.userGroup,
        Members: this.userGroup?.Members?.map((member) => ({
          ...member,
          Check: false,
        })),
      });
    },
    /**
     * Nhận giá trị của searchWord
     * Author: TNDanh (11/9/2022)
     */
    handleSearchMemberInUserGroup(value) {
      const me = this;
      me.userGroupDetail.searchWord = value;
      clearTimeout(me.event);
      me.event = setTimeout(() => {
        me.handleEnterKeyWhenSearch(value);
      }, 500);
    },
    /**
     * Nhấn enter đẩy một sự kiện lên UserGroup xử lý
     * Author: TNDanh (11/9/2022)
     */
    handleEnterKeyWhenSearch(value) {
      this.$emit("searchKeyWord", value);
    },
    /**
     * Gửi sự kiện xóa Member trong userGroup
     * Author: TNDanh (11/9/2022)
     */
    handleRemoveMemberInUserGroup(memberID) {
      this.$emit("deleteMember", memberID);
    },
    /**
     * Gửi sự kiện xóa nhiều
     * Author: TNDanh (12/9/2022)
     */
    deleteMembers() {
      let membersCheck = this.membersInUserGroup.filter(
        (member) => member.Check
      );
      this.$emit("deleteMembers", membersCheck);
    },
    /**
     * Lưu các id của người dùng trong nhóm người dùng
     * Author: TNDanh (22/9/2022)
     */
    handleValueChanged() {
      let membersCheck = this.membersInUserGroup
        .filter((member) => member.Check)
        .map((member) => member.MemberID);
      this.$store.commit("setMemberCheckInUserGroup", membersCheck);
    },
  },
  computed: {
    ...mapGetters(["userGroup", "userGroupCurrent", "memberCheckInUserGroup"]),
  },
  created() {
    this.handleInitUserWithChekbox();
  },
  mounted() {
    // this.membersInUserGroup = this.userGroup.Members;
  },
  watch: {
    userGroup: {
      handler() {
        // 2. Đếm số lượng member đang chọn
        this.selectedMemberNumber = this.userGroup?.Members?.filter(
          (user) => user.Check
        ).length;

        this.membersInUserGroup = this.userGroup.Members?.map((member) => ({
          ...member,
          Check: this.memberCheckInUserGroup.includes(member.MemberID)
            ? true
            : false,
        }));
      },
      deep: true,
    },
  },
};
</script>

<style scoped>
.detail-preview-user-group {
  position: relative;
  padding: 24px;
  height: 100%;
}

.detail-preview-user-group .close {
  position: absolute;
  content: "";
  top: 24px;
  right: 24px;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.detail-preview-user-group .close > .icon-title-size {
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -216px -24px;
  background-color: var(--icon-default-color);
}

.detail-preview-user-group .tag {
  margin-left: 8px;
}

/* --header-- */
.detail-preview-user-group__header {
  border-bottom: 1px solid var(--detail-row-border-color);
}
.detail-preview-user-group__header .header__top {
  display: flex;
  align-items: center;
}

.detail-preview-user-group__header .header__bottom {
  margin-top: 16px;
  margin-bottom: 40px;
}

/* control */
.detail-preview-user-group__body .control {
  justify-content: space-between;
}

/* control plus */
.detail-preview-user-group__body .control-plus {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
  /* justify-content: space-between; */
}

.detail-preview-user-group__body .control-plus .unchecked {
  color: var(--primary);
  cursor: pointer;
}

/* --body-- */
.detail-preview-user-group__body {
  margin-top: 24px;
}

.detail-preview-user-group__body .container {
  height: 455px;
  overflow: auto;
}

.detail-preview-user-group__body .container .member {
  position: relative;
  height: 64px;
  display: flex;
  align-items: center;
}

.detail-preview-user-group__body .container .member > .delete-member {
  position: absolute;
  right: 0;
  height: 64px;
  display: none;
  align-items: center;
  cursor: pointer;
}

.detail-preview-user-group__body
  .container
  .member
  > .delete-member
  .icon-title-size {
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -171px -26px;
  background-color: var(--delete-icon-color);
}

.detail-preview-user-group__body .container .member:hover {
  background-color: var(--active-option-dropbox-bg);
}

.detail-preview-user-group__body .container .member:hover .delete-member {
  display: flex;
}

.detail-preview-user-group__body .container .member {
}

.detail-preview-user-group__body .container .member__content {
  display: flex;
}

.detail-preview-user-group__body .container .avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  overflow: hidden;
  background: var(--detail-row-border-color);
}

.detail-preview-user-group__body .container .avatar__img {
  width: 100%;
  height: 100%;
}
</style>