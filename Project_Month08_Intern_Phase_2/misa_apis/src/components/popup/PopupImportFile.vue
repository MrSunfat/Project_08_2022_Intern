<template>
  <div class="popup-import-file">
    <div class="popup-import-file__container">
      <div class="icon-size close" @click="handleHidePopupImportFile">
        <div class="icon-title-size"></div>
      </div>
      <div class="popup-import-file__header">
        <h1 class="title bold">Nhập khẩu</h1>
      </div>
      <div class="popup-import-file__steps">
        <div
          class="step-one center__tag semi-bold"
          :class="{ active: step >= 1 }"
        >
          Tải tệp mẫu
        </div>
        <div
          class="step-two center__tag semi-bold"
          :class="{ active: step === 2 }"
        >
          Chọn tệp nhập khẩu
        </div>
      </div>
      <div class="popup-import-file__body">
        <div class="popup-import-file__body-one" v-show="step === 1">
          <h5 class="title">
            Mẫu file nhập khẩu phải có đầy đủ các thông tin sau: họ và tên, chức
            vụ, đơn vị, phòng ban, ...
          </h5>
          <BaseButton
            :nameBtn="buttomEnum.nameBtn.DownloadExcelTemplate"
            :type="buttomEnum.typeBtn.Primary"
            positionIcon="-216px -49px"
            @click="handleDownloadFileDefault"
          />
        </div>
        <div class="popup-import-file__body-two" v-show="step === 2">
          <div class="choose-file">
            <h5 class="title">Chọn tệp để nhập khẩu</h5>
            <input
              type="file"
              name=""
              ref="upload-excel"
              @change="handleChooseFile"
              class="d-none"
            />
            <div class="file-upload d-f">
              <BaseButton
                :nameBtn="buttomEnum.nameBtn.UploadExcel"
                :type="buttomEnum.typeBtn.Primary"
                positionIcon="-240px -49px"
                @click="handleUploadFile"
              />
              <span class="mg-t-15" v-show="nameFile">{{ nameFile }}</span>
              <span class="red-color" v-show="errorMessage"
                >({{ errorMessage }})</span
              >
            </div>
          </div>
          <div class="check" :class="{ import: nameFile }">
            <div class="check__record check-item">
              <h1 class="title">Số bản ghi</h1>
              <span class="content">{{ totalRecord }}</span>
            </div>
            <div class="check__valid check-item">
              <h1 class="title">Hợp lệ</h1>
              <span class="content">{{ validRecord }}</span>
            </div>
            <div class="check__illegal check-item">
              <h1 class="title">Không hợp lệ</h1>
              <span class="content">{{ illegalRecord }}</span>
            </div>
          </div>
          <div class="warn-invalid-users" v-show="invalidUsers?.length">
            Bạn có thể nhấn
            <span
              class="c-p warn-invalid-users__link semi-bold"
              @click="handleExportInvalidUsersFile"
              >tại đây</span
            >
            để xem thông tin người dùng không hợp lệ !
          </div>
        </div>
      </div>
      <div class="popup-import-file__footer">
        <BaseButton
          class="mg-r-12"
          :nameBtn="buttomEnum.nameBtn.Cancel"
          :type="buttomEnum.typeBtn.Text"
          @click="handleHidePopupImportFile"
        />
        <BaseButton
          v-show="step === 2"
          class="mg-r-12"
          :nameBtn="buttomEnum.nameBtn.Back"
          :type="buttomEnum.typeBtn.Text"
          @click="handleComback"
        />
        <BaseButton
          class="mg-r-12"
          :nameBtn="contextPrimaryBtn"
          :type="buttomEnum.typeBtn.Primary"
          @click="handlePrimaryBtn"
        />
      </div>
    </div>
  </div>
</template>

<script>
import BaseButton from "@/components/base/BaseButton.vue";
import { buttomEnum } from "@/scripts/enum";
import axios from "axios";
import { domain, user } from "@/scripts/constants";

export default {
  name: "PopupImportFile",
  components: {
    BaseButton,
  },
  data() {
    return {
      step: 1,
      contextPrimaryBtn: buttomEnum.nameBtn.Continue,
      buttomEnum,
      file: "",
      nameFile: "",
      totalRecord: 0,
      validRecord: 0,
      illegalRecord: 0,
      errorMessage: "",
      validUsers: [],
      invalidUsers: [],
    };
  },
  methods: {
    /**
     * Gửi sự kiện đóng popup file
     * Author: TNDanh (18/9/2022)
     */
    handleHidePopupImportFile() {
      this.$emit("hidePopupImportFile");
    },
    /**
     * Quay về trang đầu
     * Author: TNDanh (18/9/2022)
     */
    handleComback() {
      this.step = 1;
    },
    /**
     * Xử lý khi nhấn vào nút chính của form
     * Author: TNDanh (18/9/2022)
     */
    async handlePrimaryBtn() {
      switch (this.step) {
        case 1:
          this.step = 2;
          break;
        default:
          // 1. Thực hiện thêm các user thỏa mãn
          await this.$store.dispatch("addUsers", this.validUsers);
          this.$emit("addUsersSuccess");
          // 2. Thực hiện tải file excel chứa người dùng không hợp lệ (nếu có)
          // if (this.invalidUsers.length > 0) {
          //   await this.handleExportInvalidUsersFile();
          // }
          this.handleHidePopupImportFile();
      }
    },
    /**
     * Xử lý trả lại file excel chứa người dùng không hợp lệ
     * Author: TNDanh (21/9/2022)
     */
    async handleExportInvalidUsersFile() {
      await axios({
        url: `${domain}/${user}/ExportInvalidUsers`,
        method: "POST",
        responseType: "blob",
        data: this.invalidUsers,
      }).then((response) => {
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "Danh sách người dùng không hợp lệ.xlsx");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      });
    },
    /**
     * Tải file mẫu để nhập khẩu
     * Author: TNDanh (18/9/2022)
     */
    handleDownloadFileDefault() {
      axios({
        url: `${domain}/${user}/ExportDefault`,
        method: "GET",
        responseType: "blob",
      }).then((response) => {
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "Tệp mẫu nhập khẩu.xlsx");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      });
    },
    /**
     * Xử lý khi up file
     * Author: TNDanh (18/9/2022)
     */
    handleUploadFile() {
      this.$refs["upload-excel"].click();
    },
    /**
     * Xử lý khi nhấn chọn file
     * Author: TNDanh (18/9/2022)
     */
    async handleChooseFile(e) {
      this.file = e.target.files[0];
      this.nameFile = e.target.files[0]?.name;
      let form = new FormData();
      form.append("file", this.file);
      const res = await axios.post(`${domain}/${user}/Import`, form, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      // Kiểm tra file có hợp lệ không ?
      if (typeof res.data === "string" && res.data.includes("not a valid")) {
        this.errorMessage = "File không hợp lệ !";
        this.totalRecord = 0;
        this.illegalRecord = 0;
        this.validRecord = 0;
        this.validUsers = [];
        this.invalidUsers = [];
      } else {
        this.totalRecord = res.data?.Rows;
        this.illegalRecord = res.data?.IllegalRow;
        this.validRecord = res.data?.ValidRow;
        this.validUsers = res.data?.ValidUsers;
        this.invalidUsers = res.data?.InvalidUsers;
        this.errorMessage = "";
      }
    },
  },
  watch: {
    step(value) {
      switch (value) {
        case 1:
          this.contextPrimaryBtn = buttomEnum.nameBtn.Continue;
          break;

        default:
          this.contextPrimaryBtn = buttomEnum.nameBtn.Import;
          break;
      }
    },
  },
};
</script>

<style scoped>
.d-none {
  display: none;
}
.popup-import-file {
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

.popup-import-file__container {
  position: relative;
  background-color: #fff;
  width: 800px;
  height: 520px;
  border-radius: 8px;
}

.icon-size.close .icon-title-size {
  -webkit-mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -216px -24px;
  mask: url("../../assets/Icons/ic_sprites_2.svg") no-repeat -216px -24px;
  background-color: var(--icon-default-color);
}

.popup-import-file__container .icon-size:hover {
  background-color: var(--icon-default-hover-bg);
}

.popup-import-file__container .icon-size.close {
  top: 15px;
  right: 10px;
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.popup-import-file__header {
  padding: 24px 24px 0;
}

.popup-import-file__header .title {
  margin: 0;
  font-size: 24px;
}

.center__tag {
  display: flex;
  align-items: center;
  justify-content: center;
}

.popup-import-file__steps {
  padding: 0 24px;
  display: flex;
  align-items: center;
  height: 40px;
  color: #fff;
  width: 100%;
  margin-top: 12px;
}

.popup-import-file__steps .step-one,
.popup-import-file__steps .step-two {
  background-color: #afb4bb;
  height: 100%;
  width: 100%;
  position: relative;
}

.popup-import-file__steps .step-one::after {
  position: relative;
  content: "";
  right: -144px;
  border-left: 10px solid transparent;
  border-top: 20px solid #fff;
  border-bottom: 20px solid #fff;
  z-index: 2;
}

.popup-import-file__steps .step-two::before {
  position: relative;
  content: "";
  right: 113px;
  border-left: 10px solid #fff;
  border-top: 20px solid transparent;
  border-bottom: 20px solid transparent;
  z-index: 2;
}

.center__tag.active {
  background-color: var(--primary);
}

.popup-import-file__body {
  padding: 24px 24px;
  height: 331px;
}

.popup-import-file__body-one,
.popup-import-file__body-two {
  border-radius: 4px;
  border: 1px solid #ccc;
  padding: 24px 12px;
  height: 100%;
}

.popup-import-file__body-two {
  position: relative;
}

.warn-invalid-users {
  position: absolute;
  bottom: -36px;
  left: -2px;
}

.warn-invalid-users__link {
  color: var(--primary);
  cursor: pointer;
}

.popup-import-file__body-one {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.popup-import-file__body-two .choose-file {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.popup-import-file__body-two .choose-file .file-upload {
  align-items: center;
  flex-direction: column;
}

.popup-import-file__body-two .choose-file .file-upload span {
  margin-left: 10px;
  font-size: 15px;
  font-weight: 800;
  color: #212121;
}

.popup-import-file__body-one .title,
.popup-import-file__body-two .title {
  margin-top: 0;
  margin-bottom: 12px;
}

.popup-import-file__body-two .check {
  height: 165px;
  display: flex;
  padding-top: 15px;
  align-items: stretch;
  margin: 0 -12px;
  color: #fff;
}

.popup-import-file__body-two .check.import {
  height: 140px;
}

.popup-import-file__body-two .check-item {
  flex-grow: 1;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  margin: 0 12px;
  width: 240px;
}

.popup-import-file__body-two .check-item .title {
  font-size: 16px;
  margin-bottom: 0;
}

.popup-import-file__body-two .check-item .content {
  font-size: 44px;
}

.popup-import-file__body-two .check__record {
  background-image: url("../../assets/Background/Shape_SoBanGhi.png");
  background-repeat: no-repeat;
  background-size: 320px 178px;
}

.popup-import-file__body-two .check__valid {
  background-image: url("../../assets/Background/Shape_HopLe.png");
  background-repeat: no-repeat;
  background-size: 320px 178px;
}

.popup-import-file__body-two .check__illegal {
  background-image: url("../../assets/Background/Shape_KhongHopLe.png");
  background-repeat: no-repeat;
  background-size: 320px 178px;
}

.popup-import-file__footer {
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

.red-color {
  color: red !important;
}

.mg-t-15 {
  margin-top: 15px;
}
</style>