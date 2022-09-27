<template>
  <div class="base-option">
    <div class="base-option__header">
      <div class="header__top">
        <h1 class="title no-mg font-18 bold">Tủy chỉnh cột</h1>
        <div class="icon-size refresh" @click="handleRefreshOption">
          <div class="icon-title-size"></div>
        </div>
      </div>
    </div>
    <div id="base-option__container">
      <div class="widget-container">
        <DxList
          :data-source="userProperties"
          :repaint-changes-only="true"
          :search-enabled="true"
          search-mode="contains"
          search-expr="Name"
          :searchEditorOptions="{ placeholder: 'Tìm kiếm' }"
          key-expr="Field"
          noDataText="Không có dữ liệu"
        >
          <template #item="{ data: item }">
            <DxCheckBox v-model="item.Selected" :text="item.Name" />
          </template>
          <DxItemDragging
            data="item"
            :allow-reordering="true"
            :on-drag-end="onDragEnd"
            group="tasks"
          />
        </DxList>
      </div>
    </div>
    <div class="base-option__footer">
      <BaseButton
        class="save"
        :type="buttomEnum.typeBtn.Primary"
        :nameBtn="buttomEnum.nameBtn.Save"
        @click="handleSaveOption"
      />
    </div>
  </div>
</template>
<script>
import DxList, { DxItemDragging } from "devextreme-vue/list";
import { buttomEnum } from "@/scripts/enum";
import { placholderText } from "@/scripts/constants";
// import BaseInputSearch from "@/components/base/BaseInputSearch.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import { DxCheckBox } from "devextreme-vue/check-box";

export default {
  components: {
    DxList,
    DxItemDragging,
    // BaseInputSearch,
    BaseButton,
    DxCheckBox,
  },
  data() {
    return {
      userProperties: [],
      buttomEnum,
      placholderText,
    };
  },
  methods: {
    /**
     * Xử lý khi kéo đến vị trí khác và hạ xuống
     * Author: TNDanh (13/9/2022)
     */
    onDragEnd(e) {
      let fromIndex = e.fromIndex;
      let toIndex = e.toIndex;
      if (e.fromIndex <= e.toIndex) {
        toIndex += 1;
      } else {
        fromIndex += 1;
      }
      this.userProperties.splice(toIndex, 0, this.userProperties[e.fromIndex]);
      this.userProperties.splice(fromIndex, 1);
    },
    /**
     * Lưu các thuộc tính của các cột vào localStorage
     * Author: TNDanh (30/8/2022)
     */
    handleSaveOption() {
      // localStorage.setItem(
      //   "userProperties",
      //   JSON.stringify(this.userProperties)
      // );
      // Gửi sự kiện đóng Option
      this.$emit("closeOption");
      // Gửi sự kiện thay đổi thuộc tính của user
      this.$emit("changePropertiesUser", this.userProperties);
    },
    /**
     * Xét lại về các thuộc tính mặc định muốn hiển thị
     * Author: TNDanh (30/8/2022)
     */
    handleRefreshOption() {
      this.$emit("closeOption");
      this.$emit("refreshColumnOptions");
    },
    /**
     * Nhập chữ trong lọc tùy chọn cột
     * Author: TNDanh (13/9/2022)
     */
    handleFilterColumnOption(value) {
      if (value) {
        console.log(value, this.userProperties);
        this.userProperties = this.userProperties.filter((property) =>
          property.Name.toLowerCase()?.includes(value.toLowerCase())
        );
      } else {
        this.handleInitColumnOption();
      }
    },
    /**
     * Xét giá trị cho tùy chọn cột
     * Author: TNDanh (13/9/2022)
     */
    handleInitColumnOption() {
      this.userProperties =
        JSON.parse(localStorage.getItem("userProperties")) || [];
    },
  },
  watch: {},
  computed: {},
  mounted() {
    this.handleInitColumnOption();
  },
};
</script>
<style scoped>
.base-option {
  position: relative;
  height: 100%;
  padding: 24px 24px 0;
}

.base-option::before {
  position: absolute;
  content: "";
  top: -10px;
  right: 26px;
  width: 0;
  height: 0;
  border-left: 10px solid transparent;
  border-right: 10px solid transparent;
  border-bottom: 10px solid #fff;
}

.base-option__container {
  width: 100%;
}

.widget-container {
  display: flex;
  width: 100%;
}

.widget-container > * {
  height: 400px;
  width: 50%;
  padding: 10px;
}

.dx-scrollview-content {
  min-height: 380px;
}

/* --Base Option-- */
/* Header */
.base-option__header {
  margin-bottom: 8px;
}

.base-option__header .header__top {
  display: flex;
  justify-content: space-between;
  align-items: center;
  /* margin-top: 24px; */
  margin-bottom: 12px;
}

.base-option .refresh {
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.base-option .icon-size.refresh:hover {
  background-color: var(--icon-bg-hover);
}

.base-option .icon-title-size {
  mask: url("../../assets/Icons/ic_sprites.svg") no-repeat -144px -72px;
  background: #000;
}

/* footer */
.base-option__footer {
  position: absolute;
  content: "";
  bottom: 0;
  left: 0;
  right: 0;
  height: var(--footer-height-popup-edit-user-group);
  background-color: var(--popup-footer-bg);
  display: flex;
  justify-content: flex-end;
  align-items: center;
  border-bottom-left-radius: 8px;
  border-bottom-right-radius: 8px;
}

.base-option__footer .save {
  justify-content: center;
  width: 66px;
  padding: 0 16px;
  margin-right: 24px;
}
</style>
