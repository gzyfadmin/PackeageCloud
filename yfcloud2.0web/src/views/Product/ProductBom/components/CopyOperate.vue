<template>
  <div id="Print">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      :close-on-click-modal="false"
      :center="true"
    >
      <div>
        <el-checkbox-group v-model="checkedList">
          <el-checkbox v-for="item in titleOPt" :label="item.id" :key="item.id">{{item.value}}</el-checkbox>
        </el-checkbox-group>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="copyOpt">确定</el-button>
        <el-button @click="close">取消</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import {
  formatDate,
  isEmpty,
  isRealNum,
  getMutipSort,
  getSort
} from "@/utils/common.js";
import export2Excel from "@/utils/Export2Excel.js";
import { setTimeout } from "timers";
import BigNumber from "bignumber.js";
// import print from "@/utils/print.js";

export default {
  data() {
    return {
      dialogVisible: false,
      titleOPt: [
        { id: "itemName", value: "配色项目" },
        { id: "materialCode", value: "物料名称" },
        { id: "materialName", value: "部位名称" },
        { id: "formula", value: "单用量计算公式" },
        { id: "lengthValue", value: "长" },
        { id: "widthValue", value: "宽" },
        { id: "wideValue", value: "封度（宽幅）" },
        { id: "lossValue", value: "损耗" }
      ],
      checkedList: []
    };
  },
  props: {
    title: {
      default: "详情"
    }
  },
  components: {},
  mounted() {},
  methods: {
    open(list) {
      this.dialogVisible = true;
      if(list){
         this.checkedList= list; 
      }
    },
    close() {
      this.dialogVisible = false;
    },
    copyOpt() {
      this.close();
      this.$emit("copyOpt", this.checkedList);
    }
  }
};
</script>

<style lang="scss" scoped>
</style>

