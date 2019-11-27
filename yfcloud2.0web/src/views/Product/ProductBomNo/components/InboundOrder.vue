<template>
  <div id="InboundOrder">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="660px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-background="rgba(1, 1, 1, 0)"
        element-loading-spinner="el-icon-loading"
      >
        <el-header id="elheader" class="header" height="auto">
          <el-form ref="dtData" label-width="76px" class="FormInput" inline label-position="left">
            <el-form-item label="模板名称：">
              <el-input
                v-model="dtData.tempName"
                placeholder="模板名称："
                clearable
                style="width:200px"
                @keyup.enter.native="getMainListIndex"
                @clear="getMainListIndex"
                @submit.native.prevent
              >
                <i slot="suffix" class="el-input__icon el-icon-search" @click="getMainListIndex"></i>
              </el-input>
            </el-form-item>
            <el-form-item label="包型名称：">
              <el-select placeholder="请选择" v-model="dtData.packageId" @change="getMainListIndex"  style="width:200px">
                <el-option :value="-1" label="所有"></el-option>
                <el-option
                  v-for="item in BrandData"
                  :key="item.id"
                  :label="item.dicCode+item.dicValue"
                  :value="item.id"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-form>
        </el-header>
        <div style="height:400px;overflow:auto;">
          <el-table
            :data="tableData"
            height="380"
            border
            :highlight-current-row="true"
            @row-click="rowClick"
            @cell-dblclick="cellDblclick"
            v-if="tableFlag"
          >
            <el-table-column label="序号" width="150">
              <template slot-scope="scope">
                <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
              </template>
            </el-table-column>
            <el-table-column prop="tempName" label="模板名称" />
          </el-table>
        </div>
        <Pagination
          :pageIndex="pageIndex"
          :totalCount="totalCount"
          :pageSize="pageSize"
          @pagination="pagination"
        />
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible=false">关闭</el-button>
        <el-button type="primary" @click="OnBtnSaveSubmit">确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import Pagination from "@/components/Pagination";
import { formatDate } from "@/utils/common.js";

export default {
  data() {
    return {
      dialogVisible: false,
      dtData: {
        tempName: "",
        packageId:-1,
      },
      fullscreenLoading: false,
      tableData: [],
      clickRow: {},
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      tableFlag:false,
    };
  },
  props: {
    title: {
      default: "详情"
    },
    BrandData: {
      //部位信息
      type: Array
    }
  },
  filters: {
    formatTDate: value => {
      if (value == "" || value == null) {
        return "";
      }
      const d = value.split("T");
      return `${d[0]}`;
    }
  },
  components: {
    Pagination
  },
  mounted() {
    // this.getMainList();
  },
  methods: {
    open() {
      this.dialogVisible = true;
      this.tableFlag = false;
    },
    close() {
      this.dialogVisible = false;
    },
    pagination(val) {
      //改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList();
    },
    getMainListIndex() {
      this.pageIndex = 1;
      this.getMainList();
    },
    rowClick(row) {
      this.clickRow = row;
    },
    OnBtnSaveSubmit() {
      // console.log(this.BrandData)
      if (!this.clickRow.id) {
        this.$message.error("请选择数据");
        return;
      }
      this.close();
      this.$emit("OnBtnSaveSubmit", this.clickRow);
    },
    cellDblclick(row) {
      this.clickRow = row;
      this.OnBtnSaveSubmit();
    },
    getMainList() {
      setTimeout(()=>{
        this.tableFlag = true;
      },0) 
      //模板
      var list = [
        {
          column: "tempName",
          content: this.dtData.tempName,
          condition: 6
        }
      ];
      if(this.dtData.packageId !== -1){
        list.push({
          column: "packageId",
          content: this.dtData.packageId,
          condition: 0
        })
      }
      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        list
      );
      this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "manufacturing/api/TMMBOMNTempMain/GetMainList",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.tableData = res.data;
          this.totalCount = res.totalNumber;
          this.fullscreenLoading = false;
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          this.fullscreenLoading = false;
        }
      });
    },
  }
};
</script>

<style lang="scss" scoped>
#InboundOrder /deep/ {
  .el-dialog--center .el-dialog__body {
    padding: 20px 20px 0px;
  }
  .el-table--small td {
    padding: 8px !important;
  }
}
</style>

