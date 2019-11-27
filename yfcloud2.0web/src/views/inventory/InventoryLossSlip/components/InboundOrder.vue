<template>
  <div id="InboundOrder">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="1000px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-spinner="el-icon-loading"
        element-loading-background="rgba(1, 1, 1, 0)"
        @click.native="listenClick"
      >
        <el-header id="elheader" class="header" height="auto">
          <el-form ref="dtData" label-width="76px" class="FormInput" inline label-position="left">
            <el-form-item label="出库单号：">
              <el-input
                v-model="dtData.warehousingOrder"
                placeholder="出库单号"
                clearable
                style="width:320px"
                @keyup.enter.native="getMainList"
                @clear="getMainList"
              />
            </el-form-item>
            <el-form-item label="出库时间：">
              <el-date-picker
                v-model="dtData.warehousingDate"
                style="width:400px"
                type="daterange"
                align="left"
                unlink-panels
                range-separator="至"
                start-placeholder="开始日期"
                end-placeholder="结束日期"
                @change="getMainList"
              />
            </el-form-item>
            <el-form-item label="审核状态：">
              <el-radio-group v-model="dtData.auditStatus" @change="getMainList">
                <el-radio-button label="-1">全部</el-radio-button>
                <el-radio-button label="0">待审核</el-radio-button>
                <el-radio-button label="1">未通过</el-radio-button>
                <el-radio-button label="2">通过</el-radio-button>
                <!-- <el-radio-button label="3">撤销审核</el-radio-button> -->
              </el-radio-group>
            </el-form-item>
          </el-form>
        </el-header>
        <div style="height:400px;overflow:auto;">
          <el-table
            :data="tableData"
            :height="380"
            :highlight-current-row="true"
            @row-click="rowClick"
            @row-dblclick="dblClick"
            @sort-change="handelSortChange"
          >
            <!-- <el-table-column type="index" label="序号" /> -->
            <el-table-column label="序号" width="50">
              <template slot-scope="scope">
                <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
              </template>
            </el-table-column>
            <!-- <el-table-column prop="warehousingType" label="来源部门ID" /> -->
            <el-table-column prop="whSendDate" label="出库日期" sortable="custom">
              <template slot-scope="scope">{{ scope.row.whSendDate|formatTDate }}</template>
            </el-table-column>
            <el-table-column prop="whSendOrder" label="出库单号" width="150" sortable="custom" />
            <el-table-column prop="auditStatus" label="审核状态">
              <template slot-scope="scope">
                <div v-if="scope.row.auditStatus==0">待审核</div>
                <div v-if="scope.row.auditStatus==1">未通过</div>
                <div v-if="scope.row.auditStatus==2">通过</div>
                <div v-if="scope.row.auditStatus==3">撤销审核</div>
              </template>
            </el-table-column>
            <!-- <el-table-column prop="operatorId" label="操作员id" /> -->
            <el-table-column prop="operatorName" label="账户姓名" />
            <el-table-column prop="receiptName" label="盘点员" />
            <el-table-column prop="auditName" label="审核员" />
            <el-table-column prop="auditTime" label="审核时间">
              <template slot-scope="scope">{{ scope.row.auditTime|formatTDate }}</template>
            </el-table-column>
          </el-table>
        </div>
        <Pagination
          :page-index="pageIndex"
          :total-count="totalCount"
          :page-size="pageSize"
          @pagination="pagination"
        />
        <div slot="footer" class="dialog-footer">
          <el-button @click="dialogVisible=false">关闭</el-button>
          <el-button type="primary" @click="OnBtnSaveSubmit">选择</el-button>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import Pagination from "@/components/Pagination";
import { formatDate, trim } from "@/utils/common.js";

export default {
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
  props: {
    title: {
      default: "详情"
    }
  },
  data() {
    return {
      sortColumn: "",
      sortOrder: "",
      dialogVisible: false,
      dtData: {
        warehousingOrder: "",
        warehousingDate: "",
        auditStatus: -1
      },
      fullscreenLoading: false,
      tableData: [],
      clickRow: {},
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1 // 页码
    };
  },
  mounted() {
    this.getMainList();
  },
  methods: {
    open() {
      this.dialogVisible = true;
    },
    openNo() {
      this.dialogVisible = false;
    },
    close() {
      this.dialogVisible = false;
    },
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList();
    },
    rowClick(row) {
      this.clickRow = row;
    },
    dblClick(row) {
      this.clickRow = row;
      this.OnBtnSaveSubmit();
    },
    OnBtnSaveSubmit() {
      if (!this.clickRow.id) {
        this.$message.error("请选择数据");
        return;
      }
      this.close();
      this.$emit("OnBtnSaveSubmit", this.clickRow);
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.getMainList();
    },
    getMainList() {
      // 角色列表
      var orderConditions = [];
      var list = [
        {
          column: "whSendOrder",
          content: trim(this.dtData.warehousingOrder),
          condition: 6
        }
      ];
      if (this.dtData.warehousingOrder) {
        this.pageIndex = 1;
      }
      // if (formatDate(this.dtData.warehousingDate) != "") {
      //   list.push({
      //     column: "warehousingDate",
      //     content: formatDate(this.dtData.warehousingDate),
      //     condition: 6
      //   });
      // }
      if (this.dtData.warehousingDate != null) {
        if (this.dtData.warehousingDate[0]) {
          this.pageIndex = 1;
          var startDate = formatDate(this.dtData.warehousingDate[0]);
          var endDate = formatDate(this.dtData.warehousingDate[1]);
          list.push({
            column: "whSendDate",
            content: this.dtData.warehousingDate[0]
              ? startDate + "," + endDate
              : "",
            condition: 10
          });
        }
      }
      if (this.dtData.auditStatus != -1) {
        this.pageIndex = 1;
        list.push({
          column: "auditStatus",
          content: this.dtData.auditStatus,
          condition: 6
        });
      }
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        });
      } else {
        orderConditions = [
          {
            column: "whSendDate",
            condition: "desc"
          },
          {
            column: "whSendOrder",
            condition: "desc"
          }
        ];
      }
      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        list,
        orderConditions
      );
      this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "warehouse/api/TWMDeficitMain/GetMainList",
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
    }
  }
};
</script>

<style lang="scss" scoped>
#InboundOrder /deep/ {
  .el-dialog--center .el-dialog__body {
    // padding: 20px 20px 0px;
  }
  .el-table--small td {
    padding: 8px !important;
  }
}
</style>

