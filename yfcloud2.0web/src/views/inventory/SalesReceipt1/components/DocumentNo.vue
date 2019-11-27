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
        element-loading-background="rgba(1, 1, 1, 0)"
        element-loading-spinner="el-icon-loading"
      >
        <el-header id="elheader" class="header" height="auto">
          <el-form ref="dtData" label-width="76px" class="FormInput" inline label-position="left">
            <!-- <el-form-item label="订单单号：">
              <el-input
                v-model="dtData.warehousingOrder"
                placeholder="订单单号"
                clearable
                style="width:320px"
                @keyup.enter.native="getMainListIndex"
                @clear="getMainListIndex"
              />
            </el-form-item>-->

            <el-form-item label="订单单号：">
              <el-input
                v-model="dtData.warehousingOrder"
                placeholder="订单单号"
                clearable
                style="width:320px"
                @keyup.enter.native="getMainListIndex"
                @clear="getMainListIndex"
              >
                <i slot="suffix" class="el-input__icon el-icon-search" @click="getMainListIndex"></i>
              </el-input>
            </el-form-item>
            <el-form-item label="订单时间：">
              <el-date-picker
                style="width:400px"
                v-model="dtData.warehousingDate"
                type="daterange"
                align="left"
                unlink-panels
                range-separator="至"
                start-placeholder="开始日期"
                end-placeholder="结束日期"
                @change="getMainListIndex"
              />
            </el-form-item>
            <el-form-item class="formItem" label="客户名称：" prop="WarehousingType">
              <el-select
                v-model="dtData.WarehousingType"
                clearable
                filterable
                placeholder="请选择"
                class="headerIp"
                @change="getMainListIndex"
                style="width:320px"
              >
                <el-option
                  v-for="item in connectionData"
                  :key="item.id"
                  :label="item.customerName"
                  :value="item.id"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-form>
        </el-header>
        <div style="height:400px;overflow:auto;">
          <el-table
            :data="tableData"
            :height="380"
            :highlight-current-row="true"
            @row-click="rowClick"
            @cell-dblclick="cellDblclick"
            @sort-change="handelSortChange"
          >
            <!-- <el-table-column type="index" label="序号" /> -->
            <el-table-column label="序号" width="50">
              <template slot-scope="scope">
                <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
              </template>
            </el-table-column>
            <el-table-column prop="warehousingDate" label="订单日期" sortable="custom">
              <template slot-scope="scope">{{scope.row.orderDate|formatTDate}}</template>
            </el-table-column>
            <el-table-column prop="customerName" label="客户名称" width="120" />
            <el-table-column prop="salesmanName" label="业务员名称" />
            <el-table-column prop="orderNo" label="订单号" width="120" sortable="custom" />
            <el-table-column prop="orderTypeName" label="订单类型" />
            <el-table-column prop="settementTypeName" label="结算方式" />

            <el-table-column prop="auditStatus" label="审核状态">
              <template slot-scope="scope">
                <div v-if="scope.row.auditStatus==0">待审核</div>
                <div v-if="scope.row.auditStatus==1">未通过</div>
                <div v-if="scope.row.auditStatus==2">通过</div>
              </template>
            </el-table-column>

            <el-table-column prop="auditName" label="审核人" />

            <el-table-column prop="auditTime" label="审核时间">
              <template slot-scope="scope">{{scope.row.auditTime|formatTDate}}</template>
            </el-table-column>

            <el-table-column prop="operatorName" label="制单人" />
            <el-table-column prop="contactName" label="联系人" />
            <el-table-column prop="contactNumber" label="联系人电话" />
            <!-- <el-table-column prop="receiptAddress" label="收货地址" /> -->
            <el-table-column prop="receiptAddress" label="收货地址" width="280">
              <template slot-scope="scope">
                <!-- <el-tooltip
                  class="item"
                  effect="light"
                  :content="scope.row.receiptAddress"
                  :open-delay="300"
                  placement="top-end"
                  v-if="scope.row.receiptAddress.length>=20"
                >
                  <div class="ellipsis">{{scope.row.receiptAddress}}</div>
                </el-tooltip>-->
                <el-popover
                  v-if="scope.row.receiptAddress.length>=20"
                  placement="top-start"
                  trigger="hover"
                  :content="scope.row.receiptAddress"
                >
                  <div class="ellipsis" slot="reference">{{scope.row.receiptAddress}}</div>
                </el-popover>
                <div
                  class="ellipsis"
                  v-if="scope.row.receiptAddress.length<20"
                >{{scope.row.receiptAddress}}</div>
              </template>
            </el-table-column>
            <el-table-column
              label="操作"
              width="70"
              fixed="right"
              v-if="(btnAip.stopdeposit&&btnAip.stopdeposit.buttonCaption)"
            >
              <template slot-scope="scope">
                <el-tooltip
                  v-if="btnAip.stopdeposit&&btnAip.stopdeposit.buttonCaption"
                  class="item"
                  effect="light"
                  :content="btnAip.stopdeposit.buttonCaption"
                  placement="top-start"
                  :open-delay="200"
                >
                  <el-button
                    type="danger"
                    icon="el-icon-circle-close"
                    circle
                    @click="StopReceipt(scope.row)"
                    :disabled="scope.row.auditStatus!=2"
                  />
                </el-tooltip>
              </template>
            </el-table-column>
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
        <el-button type="primary" @click="OnBtnSaveSubmit">选择</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import Pagination from "@/components/Pagination";
import { formatDate, trim } from "@/utils/common.js";
import { setTimeout } from "timers";

export default {
  data() {
    return {
      sortColumn: "",
      sortOrder: "",
      dialogVisible: false,
      dtData: {
        WarehousingType: "",
        warehousingOrder: "",
        warehousingDate: "",
        auditStatus: -1
      },
      fullscreenLoading: false,
      tableData: [],
      clickRow: {},
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      connectionData: []
    };
  },
  props: {
    title: {
      default: "详情"
    },
    btnAip: {
      default: {}
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
    this.getCustomer();
  },
  methods: {
    open() {
      this.dialogVisible = true;
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
    StopReceipt(row, state) {
      this.$confirm("是否终止", {
        type: "warning"
      })
        .then(() => {
          var URL = "";
          URL = "/sales/api/TSSMSalesOrderMain/StopDeposit";
          this.fullscreenLoading = true;
          request({
            url: URL,
            method: "put",
            params: { requestObject: row.id }
          }).then(res => {
            if (res.code === 0) {
              this.$message({
                message: "操作成功",
                type: "success"
              });
              this.getMainList();
              this.$emit("reSet",1);
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
        })
        .catch(() => {});
    },
    getMainListIndex() {
      this.pageIndex = 1;
      this.getMainList();
    },
    rowClick(row) {
      this.clickRow = row;
    },
    OnBtnSaveSubmit() {
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
    getCustomer() {
      //获取客户名称
      var rqs = RequestObject.CreateGetObject(false, 0, 0, null);
      request({
        url: "/basicset/api/TBMCustomerFile/GetMainList",
        method: "get",
        params: {
          requestObject: JSON.stringify(rqs)
        }
      }).then(res => {
        this.loading = false;
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.connectionData = res.data;
        }
      });
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.getMainList();
    },
    getMainList() {
      //角色列表
      var orderConditions = [];
      var list = [{ column: "transferStatus", content: 1, condition: 0 }];
      if (trim(this.dtData.warehousingOrder) != "") {
        list.push({
          column: "orderNo",
          content: trim(this.dtData.warehousingOrder),
          condition: 6
        });
      }
      if (this.dtData.warehousingDate != null) {
        if (this.dtData.warehousingDate[0]) {
          var startDate = formatDate(this.dtData.warehousingDate[0]);
          var endDate = formatDate(this.dtData.warehousingDate[1]);
          list.push({
            column: "orderDate",
            content: this.dtData.warehousingDate[0]
              ? startDate + "," + endDate
              : "",
            condition: 10
          });
        }
      }
      if (this.dtData.WarehousingType !== "") {
        list.push({
          column: "customerId",
          content: this.dtData.WarehousingType,
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
            column: "orderDate",
            condition: "desc"
          },
          {
            column: "orderNo",
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
        url: "sales/api/TSSMSalesOrderMainNoPackage/GetMainList",
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
    padding: 20px 20px 0px;
  }
  .el-table--small td {
    padding: 8px !important;
  }
  .el-table--small th,
  .el-table--small td {
    padding: 8px 2px !important;
  }
}
</style>

