<template>
  <div class="pagination d-f">
    <div class="footer__left d-f mg-l-16 font-14">
      Tổng số:&nbsp;
      <h1 class="no-mg total-record bold font-14">{{ totalRecord }}</h1>
      &nbsp;bản ghi
    </div>
    <div class="footer__right d-f">
      <div class="footer__right__container d-f">
        <span class="title mg-r-8">Số bản ghi/trang</span>
        <DxSelectBox
          class="page-size w-80"
          :items="currentRecord"
          :value="50"
          @valueChanged="handleSelectPageSize"
          placeholder="50"
        />
        <span class="current__record d-f font-14">
          <h1 class="title no-mg font-14 bold">
            {{ recordStart }} - {{ recordEnd || 1 }}
          </h1>
          &nbsp;bản ghi
        </span>
      </div>
      <div class="page-next-preview d-f mg-r-16">
        <BaseButton
          :type="buttomEnum.typeBtn.IconSpecial"
          positionIcon="-73px -24px"
          @click="handlePrevPage"
        />
        <BaseButton
          :type="buttomEnum.typeBtn.IconSpecial"
          positionIcon="-96px -24px"
          @click="handlNextPage"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { buttomEnum, currentRecord } from "@/scripts/enum";
import DxSelectBox from "devextreme-vue/select-box";
import BaseButton from "@/components/base/BaseButton.vue";
export default {
  name: "BasePagination",
  props: {
    pageNumber: {
      type: Number,
    },
    totalPage: {
      type: Number,
    },
    totalRecord: {
      type: Number,
      default: 0,
    },
    recordStart: {
      type: Number,
      default: 1,
    },
    recordEnd: {
      type: Number,
      default: 1,
    },
  },
  components: {
    DxSelectBox,
    BaseButton,
  },
  data() {
    return {
      buttomEnum,
      currentRecord,
    };
  },
  methods: {
    /**
     * Xử lý khi nhấn chuyển trang trước đó
     * Author: TNDanh (27/8/2022)
     */
    handlePrevPage() {
      if (this.pageNumber - 1 >= 1) {
        console.log("prev");
        this.$emit("prevPage", this.pageNumber - 1);
      } else {
        this.$emit("offPrevPage");
      }
    },
    /**
     * Xử lý khi nhấn chuyển về trang tiếp theo
     * Author: TNDanh (27/8/2022)
     */
    handlNextPage() {
      if (this.pageNumber + 1 <= this.totalPage) {
        console.log("next");
        this.$emit("nextPage", this.pageNumber + 1);
      } else {
        this.$emit("offNextPage");
      }
    },
    /**
     * Xử lý khi nhấn chọn pageSize
     * Author: TNDanh (7/9/2022)
     */
    handleSelectPageSize(event) {
      this.$emit("pageSize", event.value);
    },
  },
};
</script>

<style scoped>
.pagination {
  height: var(--footer-height);
  background: var(--primary-bg);
  border-radius: 8px;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}
/* Footer phải của user */
.footer__right__container {
  align-items: center;
}

.current__record {
  padding: 0 24px;
}

.page-next-preview {
  align-items: center;
}
</style>