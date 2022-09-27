<template>
  <div class="user-group loading-of-parent">
    <div class="user-group__header mg-b-16">
      <div class="header__left font-20 bold">Nhóm người dùng</div>
      <div class="header__right">
        <BaseInputSearch
          class="input"
          :placeholderText="placholderText.InputUserGroup"
          :valueText="pageDetail.searchWord"
          @twoWayValue="modelValueSearch"
          @enterKey="handleEnterKeyWhenSearch"
        />
      </div>
    </div>
    <div class="user-group__main">
      <div class="user-group__body">
        <DxDataGrid
          :data-source="userGroups"
          key-expr="UserGroupID"
          :remote-operations="false"
          :allow-column-reordering="true"
          :allow-column-resizing="true"
          :show-borders="false"
          :focused-row-enabled="true"
          column-resizing-mode="widget"
          @row-click="handleShowDetailUserGroup"
          :noDataText="noDataText.userGroup"
          height="100%"
        >
          <DxColumn
            data-field="UserGroupName"
            :caption="userGroupInfo.GroupName"
          />
          <DxColumn data-field="MemberCount" :caption="userGroupInfo.Members" />
          <DxColumn
            data-field="Description"
            :caption="userGroupInfo.Description"
          />
          <DxColumn
            data-field="Status"
            :caption="userGroupInfo.Status"
            :width="200"
            cell-template="cellTemplateStatus"
            alignment="center"
          />
          <template #cellTemplateStatus="{ data }">
            <BaseTagStatus
              class="config-tag-status"
              type="on"
              :content="data.value"
              :dot="statusTagEnum.Dot.Hide"
            />
          </template>
        </DxDataGrid>
      </div>
      <div class="user-group__footer">
        <BasePagination
          :pageNumber="pageDetail.pageNumber"
          :totalPage="totalPageUserGroups"
          :totalRecord="totalUserGroups"
          :recordStart="userGroupStart"
          :recordEnd="userGroupEnd"
          @pageSize="handleChangePageSize"
          @prevPage="handleLoadDataInPage"
          @nextPage="handleLoadDataInPage"
        />
      </div>
    </div>
    <div class="user-group__detail" v-show="isShowDetailUserGroup">
      <BaseDetailUserGroup
        @hideDetailUserGroup="handleHideDetailUserGroup"
        @showPopupAddMember="handleShowPopupAddMember"
        @searchKeyWord="handleSearchMemberInUserGroup"
        @deleteMember="deleteMemberInUserGroup"
        @deleteMembers="deleteMembersInUserGroup"
      />
    </div>
    <PopupAddMember
      v-if="isShowPopupAddMember"
      @hiddenPopupAddMember="handleHidePopupAddMember"
      @saveMemberForUserGroup="handleSaveMemberForUserGroup"
    />
    <div class="loading" v-show="isLoading">
      <LoadingComp />
    </div>
    <DialogWarn
      v-if="isDialog"
      :members="membersNeedRemove"
      type="user-group"
      @closeDialog="handleCloseDialog"
      @agreeDeleteMember="handleAgreeDeleteMember"
    />
    <BaseToast
      v-if="isToast"
      :contentToast="contentToast"
      @closeToast="handleCloseToast"
    />
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { placholderText, userGroupInfo, noDataText } from "@/scripts/constants";
import { userGroupData, statusTagEnum } from "@/scripts/enum";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import BaseInputSearch from "@/components/base/BaseInputSearch.vue";
import BasePagination from "@/components/base/BasePagination.vue";
import BaseDetailUserGroup from "@/components/base/BaseDetailUserGroup.vue";
import PopupAddMember from "@/components/popup/PopupAddMember.vue";
import BaseTagStatus from "@/components/base/BaseTagStatus.vue";
import LoadingComp from "@/components/Loading/LoadingComp.vue";
import DialogWarn from "@/components/dialog/DialogWarn.vue";
import BaseToast from "@/components/base/BaseToast.vue";
export default {
  name: "UserGroup",
  components: {
    BaseInputSearch,
    BasePagination,
    BaseDetailUserGroup,
    PopupAddMember,
    BaseTagStatus,
    DxDataGrid,
    DxColumn,
    LoadingComp,
    DialogWarn,
    BaseToast,
  },
  data() {
    return {
      userGroupInfo,
      userGroupData,
      placholderText,
      statusTagEnum,
      isShowDetailUserGroup: false,
      isShowPopupAddMember: false,
      isLoading: false,
      isDialog: false,
      isToast: false,
      pageDetail: {
        pageSize: 50,
        pageNumber: 1,
        searchWord: "",
      },
      userGroupIDCurrent: "",
      membersNeedRemove: [],
      event: null,
      noDataText,
      contentToast: "",
    };
  },
  methods: {
    /**
     * Nhận giá trị khi nhập giá trị trong BaseInputSearch
     * Author: TNDanh (27/8/2022)
     */
    modelValueSearch(value) {
      const me = this;
      this.pageDetail.searchWord = value;
      clearTimeout(me.event);
      me.event = setTimeout(() => {
        me.handleEnterKeyWhenSearch();
      }, 500);
    },
    /**
     * Mở DetailUserGroup
     * Author: TNDanh (28/8/2022)
     */
    async handleShowDetailUserGroup(userGroup) {
      // Gửi request, lấy thông tin của nhóm người dùng đó
      this.userGroupIDCurrent = userGroup.data.UserGroupID;
      await this.$store.dispatch("getUserGroupByID", {
        userGroupID: this.userGroupIDCurrent,
        searchWord: "",
      });

      // Lưu giá trị hiện tại của nhóm người dùng đó
      // let userGroupChoosing = this.userGroups?.filter(
      //   (u) => u.UserGroupID === this.userGroupIDCurrent
      // )[0];
      this.$store.commit("setUserGroupCurrent", this.userGroup);
      this.isShowDetailUserGroup = true;
    },
    /**
     * Xử lý giá trị liên quan đến thành viên trong nhóm người dùng
     * Author: TNDanh (11/9/2022)
     */
    async handleSearchMemberInUserGroup(value) {
      await this.$store.dispatch("getUserGroupByID", {
        userGroupID: this.userGroupIDCurrent,
        searchWord: value,
      });
    },
    /**
     * Xóa member trong nhóm người dùng
     * Author: TNDanh (11/9/2022)
     */
    deleteMemberInUserGroup(member) {
      this.isDialog = true;
      this.membersNeedRemove = member;
      //console.log(member);

      // console.log(member.MemberID);
    },
    /**
     * Xóa nhiều member trong nhóm người dùng
     * Author: TNDanh (11/9/2022)
     */
    deleteMembersInUserGroup(members) {
      this.isDialog = true;
      this.membersNeedRemove = [...members];
      console.log(members);
    },
    /**
     * Đóng DetailUserGroup
     * Author: TNDanh (28/8/2022)
     */
    handleHideDetailUserGroup() {
      this.isShowDetailUserGroup = false;
    },
    /**
     * Đóng Popup Add member
     * Author: TNDanh (29/8/2022)
     */
    handleHidePopupAddMember() {
      this.isShowPopupAddMember = false;
    },
    /**
     * Mở Popup Add Member
     * Author: TNDanh (29/8/2022)
     */
    handleShowPopupAddMember() {
      this.isShowPopupAddMember = true;
    },
    /**
     * Lấy userGroups mới
     * Author: TNDanh (8/9/2022)
     */
    async handleGetUserGroups() {
      this.isLoading = true;
      await this.$store.dispatch("getJobTitles");
      await this.$store.dispatch("getUserGroups", this.pageDetail);
    },
    /**
     * Xét các pageSize khác nhau
     * Author: TNDanh (8/9/2022)
     */
    handleChangePageSize(pageSize) {
      this.pageDetail.pageSize = pageSize;
      this.pageDetail.pageNumber = 1;
      this.handleGetUserGroups();
    },
    /**
     * Đi đến/quay về trang cũ
     * Author: TNDanh (8/9/2022)
     */
    handleLoadDataInPage(idxPage) {
      this.pageDetail.pageNumber = idxPage;
      this.handleGetUserGroups();
    },
    /**
     * Nhấn enter ở ô tìm  kiếm thì lấy userGroup mới
     * Author: TNDanh (9/9/2022)
     */
    handleEnterKeyWhenSearch() {
      this.pageDetail.pageNumber = 1;
      this.handleGetUserGroups();
    },
    /**
     * Đóng dialog
     * Author: TNDanh (11/9/2022)
     */
    handleCloseDialog() {
      this.isDialog = false;
    },
    /**
     * Thực hiện xóa thành viên và đóng dialog
     * Author: TNDanh (11/9/2022)
     */
    async handleAgreeDeleteMember(members) {
      await this.$store.dispatch("deleteMemberInUserGroup", {
        userGroupID: this.userGroupCurrent.UserGroupID,
        memberIDs: [...members],
      });
      this.$store.commit("setMembersInUserGroup", members);
      await this.handleGetUserGroups();
      this.handleCloseDialog();
      this.contentToast = "Xóa thành viên thành công !";
      this.isToast = true;
    },
    /**
     * Xử lý sự kiện tiếp theo sau khi lưu sự thay đổi thành viên trong nhóm người dùng
     * Author: TNDanh (23/9/2022)
     */
    async handleSaveMemberForUserGroup() {
      this.handleHidePopupAddMember();
      this.handleHideDetailUserGroup();
      this.isToast = true;
      await this.handleGetUserGroups();
      this.contentToast = "Lưu thành công !";
      this.isToast = true;
    },
    /**
     * Ẩn toast message
     * Author: TNDanh (23/9/2022)
     */
    handleCloseToast() {
      this.isToast = false;
    },
  },
  computed: {
    ...mapGetters([
      "userGroups",
      "userGroup",
      "totalPageUserGroups",
      "totalUserGroups",
      "userGroupStart",
      "userGroupEnd",
      "userGroupCurrent",
    ]),
  },
  created() {
    this.userGroupData = this.userGroupData.map((group) => ({
      ...group,
      Status: group.Status === 1 && statusTagEnum.Content.Use,
    }));
    this.handleGetUserGroups();
  },
  watch: {
    userGroups() {
      this.isLoading = false;
    },
  },
};
</script>

<style scoped>
.user-group {
  position: relative;
  height: 100%;
  padding: 16px 20px;
}

/* Header */
.user-group__header {
  display: flex;
  height: var(--base-heigth);
  justify-content: space-between;
  align-items: center;
}

.user-group__header .input {
  width: 238px;
}

/* --Main-- */
.user-group__main {
  position: relative;
  height: calc(100vh - var(--height-header) - var(--base-heigth) - 48px);
  overflow-y: hidden;
}

/* Body */
.user-group__body {
  height: calc(100% - var(--footer-height));
}

.user-group__body::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

.user-group__body::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px #e5e5e5;
  border-radius: 4px;
}

.user-group__body::-webkit-scrollbar-thumb {
  background: #bbb;
  border-radius: 4px;
}
/* Footer */
.user-group__footer {
  position: absolute;
  content: "";
  left: 0;
  right: 0;
  bottom: 0;
}

/* Detail */
.user-group__detail {
  position: fixed;
  content: "";
  top: 0;
  bottom: 0;
  right: 0;
  width: var(--detail-user-group-width);
  z-index: 1;
  box-shadow: 0 0 10px 0 rgb(0 0 0 / 10%);
  background-color: #fff;
  transition: all 0.25s ease-in;
}

/* config tag status */
.config-tag-status {
  padding: 0 !important;
  justify-content: center;
  width: 150px;
  margin: 0 auto;
}
</style>