<template>
  <div class="user loading-of-parent">
    <div class="user__header mg-b-16 d-f">
      <div class="user__header__left">
        <h1 class="font-20 bold user__title">{{ sidebarTitles.User.name }}</h1>
      </div>
      <div class="user__header__right center-vertical">
        <div class="dropbox">
          <DxSelectBox
            class="dropbox__container"
            :items="userGroupsTag"
            display-expr="UserGroupName"
            value-expr="UserGroupName"
            height="100%"
            @valueChanged="handleSelectUserGroup"
            :placeholder="placholderText.UserGroup"
            :noDataText="placholderText.UserGroupNoData"
          />
        </div>
        <div class="input-search">
          <BaseInputSearch
            class="w-305 mg-l-8"
            :placeholderText="placholderText.InputUser"
            :valueText="pageDetail.searchWord"
            @twoWayValue="modelValueSearch"
            @enterKey="handleEnterKeyWhenSearch"
          />
        </div>
        <div class="btn-add d-f">
          <BaseButton
            class="mg-l-8"
            :nameBtn="buttomEnum.nameBtn.Import"
            :type="buttomEnum.typeBtn.Primary"
            positionIcon="-237px 0px"
            @click="handleShowPopupImportFile"
          />
          <BaseButton
            class="mg-l-8"
            :nameBtn="buttomEnum.nameBtn.Export"
            :type="buttomEnum.typeBtn.Primary"
            positionIcon="-46px -48px"
            @click="handleExportExcelUser"
          />
          <BaseButton
            class="mg-l-8"
            :type="buttomEnum.typeBtn.Icon"
            positionIcon="-264px -144px"
            @click="handleToggleOption"
          />
        </div>
      </div>
    </div>
    <div class="user__main">
      <div class="user__content">
        <DxDataGrid
          :data-source="users"
          key-expr="UserID"
          :remote-operations="false"
          :allow-column-reordering="true"
          :allow-column-resizing="true"
          :show-borders="false"
          :focused-row-enabled="true"
          @row-click="handleShowDetailUser"
          :noDataText="noDataText.user"
          height="100%"
        >
          <DxColumn
            v-for="property in showPropertiesSelected"
            :key="property.ID"
            :data-field="property.Field"
            :caption="property.Name"
            :cell-template="property.Field === 'Status' && 'cellTemplateStatus'"
            :alignment="property.Field === 'Status' && 'center'"
            :width="property.Width"
          />
          <template #cellTemplateStatus="{ data }">
            <BaseTagStatus
              class="config-tag-status"
              :type="data.value?.includes('Đang') ? 'on' : 'off'"
              :content="data.value"
              :dot="statusTagEnum.Dot.Hide"
            />
          </template>
          <DxColumn
            data-field="UserID"
            caption=""
            cell-template="cellTemplateDelete"
            :allow-reordering="false"
            :allowResizing="false"
            width="50"
          />
          <template #cellTemplateDelete="{ data }">
            <div
              class="icon-size custom-column-delete"
              @click.stop="handleDeleteUser(data.value)"
            >
              <div class="icon-title-size"></div>
            </div>
          </template>
          <DxPaging :enabled="false" />
        </DxDataGrid>
      </div>
      <div class="user__footer d-f">
        <BasePagination
          :pageNumber="this.pageDetail.pageNumber"
          :totalPage="totalPage"
          :totalRecord="totalUsers"
          :recordStart="userStart"
          :recordEnd="userEnd"
          @pageSize="handleChangePageSize"
          @prevPage="handleLoadDataInPage"
          @nextPage="handleLoadDataInPage"
        />
      </div>
    </div>
    <div class="user__detail" :class="{ hide: !this.isShowDetailUser }">
      <BaseDetailUser
        :userDetail="user"
        @hiddenDetailUser="handleHideDetailUser"
        @showPopupUserGroup="handleShowPopupEditUserGroup"
      />
    </div>
    <div
      class="user__option"
      tabindex="-1"
      ref="userOption"
      v-if="isShowOption"
    >
      <BaseOption
        v-if="isShowOption"
        class="option"
        @closeOption="handleHideOption"
        @changePropertiesUser="handleChangePropertiesFollowOption"
        @refreshColumnOptions="handleRefreshColumnOptions"
      />
    </div>
    <PopupEditUserGroup
      v-if="isShowPopupEditUserGroup"
      :userGroupsTagW="userGroupsTagW"
      @hiddenPopupEditUserGroup="handleHidePopupEditUserGroup"
      @saveUserGroupInUser="handleSaveUserGroup"
    />
    <div class="loading" v-show="isLoading">
      <LoadingComp />
    </div>
    <PopupImportFile
      v-if="isPopupImportFile"
      @hidePopupImportFile="handleHidePopupImportFile"
      @addUsersSuccess="handleAddUsersSuccess"
    />
    <DialogWarn
      v-if="isDialog"
      :user="userDetail"
      type="user"
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
import axios from "axios";
import { mapGetters } from "vuex";
import { sidebarTitles, placholderText } from "@/scripts/constants";
import {
  userData,
  userGroupData,
  buttomEnum,
  currentRecord,
  statusTagEnum,
} from "@/scripts/enum";
import { domain, user, noDataText } from "@/scripts/constants";
import DxSelectBox from "devextreme-vue/select-box";
import { DxDataGrid, DxColumn, DxPaging } from "devextreme-vue/data-grid";
import BaseInputSearch from "@/components/base/BaseInputSearch.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import BasePagination from "@/components/base/BasePagination.vue";
import BaseOption from "@/components/base/BaseOption.vue";
import BaseDetailUser from "@/components/base/BaseDetailUser.vue";
import PopupEditUserGroup from "@/components/popup/PopupEditUserGroup.vue";
import BaseTagStatus from "@/components/base/BaseTagStatus.vue";
import LoadingComp from "@/components/Loading/LoadingComp.vue";
import PopupImportFile from "@/components/popup/PopupImportFile.vue";
import DialogWarn from "@/components/dialog/DialogWarn.vue";
import BaseToast from "@/components/base/BaseToast.vue";

export default {
  name: "UserPage",
  data() {
    return {
      sidebarTitles,
      placholderText,
      userData,
      userGroupData,
      userProperties: [],
      buttomEnum,
      statusTagEnum,
      currentRecord,
      // Biến để kiểm tra show/hide các popup và dialog
      isShowDetailUser: false,
      isShowPopupEditUserGroup: false,
      isShowOption: false,
      isLoading: false,
      isPopupImportFile: false,
      isDialog: false,
      pageDetail: {
        pageSize: 50,
        pageNumber: 1,
        searchWord: "",
        userGroupName: "",
      },
      event: null,
      noDataText,
      isToast: false,
      contentToast: "",
    };
  },
  components: {
    DxSelectBox,
    DxDataGrid,
    DxColumn,
    DxPaging,
    BaseInputSearch,
    BaseButton,
    BasePagination,
    BaseOption,
    BaseDetailUser,
    PopupEditUserGroup,
    BaseTagStatus,
    LoadingComp,
    PopupImportFile,
    DialogWarn,
    BaseToast,
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
     * Xử lý khi nhận sự kiện đóng DetailUser
     * Author: TNDanh (27/8/2022)
     */
    handleHideDetailUser() {
      this.isShowDetailUser = false;
    },
    /**
     * Xử lý khi mở DetailUser
     * Author: TNDanh (27/8/2022)
     */
    async handleShowDetailUser(user) {
      await this.$store.dispatch("getUserByID", user.data.UserID);
      this.isShowDetailUser = true;
    },
    /**
     * Nhận sự kiện mở popup edit userGroup
     * Author: TNDanh (28/8/2022)
     */
    handleShowPopupEditUserGroup() {
      this.isShowPopupEditUserGroup = true;
    },
    /**
     * Nhận sự kiên đóng popup edit userGroup
     * Author: TNDanh (28/8/2022)
     */
    handleHidePopupEditUserGroup() {
      this.isShowPopupEditUserGroup = false;
    },
    /**
     * Mở/ Đóng Otion
     * Author: TNDanh (28/8/2022)
     */
    handleToggleOption() {
      this.isShowOption = !this.isShowOption;
      this.$nextTick(() => {
        this.$refs.userOption.focus();
      });
    },
    /**
     * Đóng popup option
     * Author: TNDanh (28/8/2022)
     */
    handleHideOption() {
      this.isShowOption = false;
    },
    /**
     * Xử lý khi nhấn nút xóa 1 user
     * Author: TNDanh (30/8/2022)
     */
    async handleDeleteUser(userID) {
      // console.log("Xóa - ", userID);
      this.isDialog = true;
      this.$store.commit("setUserID", userID);
    },
    /**
     * Xét các thuộc tính của user nhận theo sự thay đổi của tùy chỉnh cột
     * Author: TNDanh (31/8/2022)
     */
    async handleChangePropertiesFollowOption(newProperties) {
      // this.userProperties = newProperties;
      await this.$store.dispatch("updateColumnsOption", newProperties);
      this.handleInitData();
    },
    /**
     * Xét bộ tùy chỉnh cột mặc định
     * Author: TNDanh (19/9/2022)
     */
    async handleRefreshColumnOptions() {
      await this.$store.dispatch("updateDefaultColumnsOption");
      this.handleInitData();
    },
    /**
     * Xét các pageSize khác nhau
     * Author: TNDanh (7/9/2022)
     */
    handleChangePageSize(pageSize) {
      this.pageDetail.pageSize = pageSize;
      this.pageDetail.pageNumber = 1;
      this.handleGetUsers();
    },
    /**
     *  Đi đến/quay về trang cũ
     *  Author: TNDanh (7/9/2022)
     */
    handleLoadDataInPage(idxPage) {
      this.pageDetail.pageNumber = idxPage;
      this.handleGetUsers();
    },
    /**
     * Lấy users mới
     * Author: TNDanh (7/9/2022)
     */
    async handleGetUsers() {
      this.isLoading = true;
      await this.$store.dispatch("getUsers", this.pageDetail);
    },
    /**
     * Nhấn enter ở ô tìm kiếm thì lấy users mới
     * Author: TNDanh (7/9/2022)
     */
    handleEnterKeyWhenSearch() {
      this.pageDetail.pageNumber = 1;
      this.handleGetUsers();
    },
    /**
     * Chọn userGroup
     * Author: TNDanh (7/9/2022)
     */
    handleSelectUserGroup(userGroup) {
      this.pageDetail.userGroupName = userGroup.value;
      this.pageDetail.pageNumber = 1;
      if (userGroup.value === "Tất cả") {
        this.pageDetail.userGroupName = "";
      }
      this.handleGetUsers();
    },
    /**
     * Gọi về các users cần hiển thị và tất cả userGroup
     * Author: TNDanh (8/9/2022)
     */
    async handleInitData() {
      this.isLoading = true;
      const res = await this.$store.dispatch("getColumnsOption");
      this.userProperties = res.data;
      localStorage.setItem(
        "userProperties",
        JSON.stringify(this.userProperties)
      );
      await this.$store.dispatch("getAllUserGroupTag");
      await this.handleGetUsers();
    },
    /**
     * Lưu nhóm người dùng
     * Author: TNDanh (9/9/2022)
     */
    async handleSaveUserGroup(userGroups) {
      this.isLoading = true;
      this.isShowPopupEditUserGroup = false;
      this.handleHideDetailUser();
      await this.$store.dispatch("editUserGroupInGroup", {
        userID: this.user.UserID,
        userGroups,
      });
      await this.handleGetUsers();
      this.contentToast = "Lưu thành công !";
      this.isToast = true;
    },
    /**
     * Xử lý xuất khẩu
     * Author: TNDanh (13/9/2022)
     */
    handleExportExcelUser() {
      axios({
        url: `${domain}/${user}/Export`,
        method: "GET",
        responseType: "blob",
      }).then((response) => {
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "Danh sách người dùng.xlsx");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      });
    },
    /**
     * Đóng PopupImportFile
     * Author: TNDanh (18/9/2022)
     */
    async handleHidePopupImportFile() {
      this.isPopupImportFile = false;
      await this.handleGetUsers();
    },
    /**
     * Mở PopupImportFile
     * Author: TNDanh (18/9/2022)
     */
    handleShowPopupImportFile() {
      this.isPopupImportFile = true;
    },
    /**
     * Đóng dialog
     * Author: TNDanh (19/9/2022)
     */
    handleCloseDialog() {
      this.isDialog = false;
      this.$store.commit("setUserID", "");
    },
    /**
     * Thực hiện đồng ý của dialog
     * Author: TNDanh (19/9/2022)
     */
    async handleAgreeDeleteMember() {
      await this.$store.dispatch("deleteUserByID", this.user?.userID);
      this.handleCloseDialog();
      this.handleGetUsers();
      this.contentToast = "Xóa thành công !";
      this.isToast = true;
    },
    /**
     * Hiện toast message
     * Author: TNDanh (23/9/2022)
     */
    handleOpenToast() {
      this.isToast = true;
    },
    /**
     * Ẩn toast message
     * Author: TNDanh (23/9/2022)
     */
    handleCloseToast() {
      this.isToast = false;
    },
    /**
     * Xử lý khi nhấn thêm người dùng mới thành công
     * Author: TNDanh (23/9/2022)
     */
    handleAddUsersSuccess() {
      this.contentToast = "Thêm mới người dùng thành công!";
      this.handleOpenToast();
    },
  },
  computed: {
    userDetail() {
      return this.users.find((us) => us.UserID === this.user.userID);
    },
    showPropertiesSelected() {
      return this.userProperties.filter((property) => property?.Selected);
    },
    ...mapGetters([
      "users",
      "totalUsers",
      "userStart",
      "userEnd",
      "totalPage",
      "user",
      "userGroupsTag",
      "userGroupsTagW",
      "userDetailForUserGroup",
      "columnsOption",
    ]),
  },
  created() {
    this.handleInitData();
  },
  mounted() {},
  watch: {
    users() {
      this.isLoading = false;
    },
  },
};
</script>

<style scoped>
.user {
  /* position: relative; */
  padding: 16px 20px;
  height: 100%;
}

.user__header {
  height: var(--base-heigth);
  align-items: center;
  justify-content: space-between;
}

/* header left của user */
.user__title {
  margin: 0;
}

/* header right của user */
.user__header__right {
  justify-content: space-between;
}

.user__main {
  position: relative;
  height: calc(100vh - var(--height-header) - var(--base-heigth) - 48px);
  overflow-y: hidden;
}

.user__content {
  height: calc(100% - var(--footer-height));
  /* overflow: auto; */
}

.user__content::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

.user__content::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px #e5e5e5;
  border-radius: 4px;
}

.user__content::-webkit-scrollbar-thumb {
  background: #bbb;
  border-radius: 4px;
}

.user__footer {
  position: absolute;
  content: "";
  left: 0;
  right: 0;
  bottom: 0;
}

/* Detail User */
.user__detail {
  position: fixed;
  content: "";
  top: 0;
  bottom: 0;
  right: 0;
  width: var(--detail-user-width);
  z-index: 1;
  box-shadow: 0 0 10px 0 rgb(0 0 0 / 10%);
  background-color: #fff;
  transition: all 0.25s ease-in;
}

.user__detail.hide {
  --hide: calc(var(--detail-user-width) + 100px);
  right: calc(0px - var(--hide));
}

/* Option user */
.user__option {
  position: fixed;
  top: 118px;
  right: 0;
  width: 330px;
  height: 570px;
  background: #fff;
  box-shadow: 0 0 10px 0 rgb(0 0 0 / 10%);
  border-radius: 8px;
  /* overflow: hidden; */
}

.user__option::before {
  position: absolute;
  top: 0;
  width: 0;
  height: 0;
  border-left: 5px solid transparent;
  border-right: 5px solid transparent;

  border-bottom: 5px solid #000;
}

/* config tag status */
.config-tag-status {
  padding: 0 !important;
  justify-content: center;
  width: 150px;
  margin: 0 auto;
}

.custom-column-delete {
  display: none;
  align-items: center;
  justify-content: center;
}

.custom-column-delete .icon-title-size {
  -webkit-mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -168px -24px;
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -168px -24px;
  background-color: var(--delete-icon-color);
}

.dx-datagrid-rowsview .dx-row:hover .custom-column-delete {
  display: flex;
}
</style>