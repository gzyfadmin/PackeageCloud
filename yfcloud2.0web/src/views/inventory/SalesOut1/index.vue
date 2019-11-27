<template>
  <el-container
    id="SalesOut"
    v-loading="fullscreenLoading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="76px" inline class="FormInput">
        <el-form-item label="销售出库单号：" label-width="105px" prop="warehousingType">
          <el-input
            v-model="dtData.whSendOrder"
            @keyup.enter.native="getMainListIndex"
            clearable
            placeholder="销售出库单号"
            @clear="getMainListIndex"
          >
            <i slot="suffix" class="el-input__icon el-icon-search" @click="getMainList"></i>
          </el-input>
        </el-form-item>
        <el-form-item class="formItem" label="客户名称：" prop="WarehousingType">
          <el-select
            v-model="dtData.WarehousingType"
            clearable
            filterable
            placeholder="请选择"
            class="headerIp"
            @change="getMainListIndex"
          >
            <el-option
              v-for="item in connectionData"
              :key="item.id"
              :label="item.customerName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="审核状态：">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainListIndex">
            <el-radio-button label="-1">全部</el-radio-button>
            <el-radio-button label="0">待出库</el-radio-button>
            <el-radio-button label="1">未通过</el-radio-button>
            <el-radio-button label="2">通过</el-radio-button>
            <!-- <el-radio-button label="3">撤销审核</el-radio-button> -->
          </el-radio-group>
        </el-form-item>
      </el-form>
      <el-button
        type="primary"
        @click="getMainListIndex"
        v-if="btnAip.query&&btnAip.query.buttonCaption"
      >{{ btnAip.query.buttonCaption }}</el-button>
      <el-button
        type="primary"
        @click="goPage({},3)"
        v-if="btnAip.add&&btnAip.add.buttonCaption"
      >{{ btnAip.add.buttonCaption }}</el-button>
    </el-header>
    <el-main id="elmain" class="wmltable">
      <el-table
        ref="table"
        :height="tableHeight"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        :show-summary="true"
        :summary-method="getSummaries"
        @sort-change="handelSortChange"
      >
        <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="whSendDate" label="出库时间" width="100" sortable="custom">
          <template slot-scope="scope">{{scope.row.whSendDate|formatTDate}}</template>
        </el-table-column>
        <el-table-column prop="customerName" label="客户名称" width="200" />
        <el-table-column prop="whSendOrder" label="销售出库单号" width="200" sortable="custom">
          <template slot-scope="scope">
            <div @click="goPage(scope.row,1)" class="link">{{scope.row.whSendOrder}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="saleOrder" label="销售订单号" width="200">
          <!-- <template slot-scope="scope">
            <div @click="goPage(scope.row,4)" class="link">{{scope.row.saleOrder}}</div>
          </template>-->
        </el-table-column>
        <!-- <el-table-column prop="saleOrder" label="源销售订单号" width="200"/> -->
        <el-table-column prop="auditStatus" label="审核状态">
          <template slot-scope="scope">
            <div v-if="scope.row.auditStatus==0">待审核</div>
            <div v-if="scope.row.auditStatus==1">未通过</div>
            <div v-if="scope.row.auditStatus==2">通过</div>
          </template>
        </el-table-column>
        <el-table-column prop="number" label="出库数量" />
        <el-table-column prop="amount" label="出库金额" width="200px" />

        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <span v-if="btnAip.edit">
              <el-tooltip
                v-if="btnAip.edit.buttonCaption"
                class="item"
                effect="light"
                :content="btnAip.edit.buttonCaption"
                placement="top-start"
                :open-delay="200"
              >
                <el-button
                  type="primary"
                  icon="el-icon-edit"
                  circle
                  @click="goPage(scope.row,2)"
                  :disabled="!scope.row.isShowEdit||scope.row.auditStatus==2"
                />
              </el-tooltip>
              <el-tooltip
                v-if="btnAip.delete.buttonCaption&&showtips"
                class="item"
                effect="light"
                :content="btnAip.delete.buttonCaption"
                placement="top-start"
                :open-delay="200"
              >
                <el-button
                  type="danger"
                  icon="el-icon-delete"
                  circle
                  @click="addPutInStorage(scope.row)"
                  :disabled="scope.row.auditStatus==2"
                />
              </el-tooltip>
              <el-button
                v-if="btnAip.delete.buttonCaption&&!showtips"
                type="danger"
                icon="el-icon-delete"
                circle
                @click="addPutInStorage(scope.row)"
                :disabled="scope.row.auditStatus==2"
              />
            </span>
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <el-footer id="elfooter">
      <Pagination
        :page-index="pageIndex"
        :total-count="totalCount"
        :page-size="pageSize"
        @pagination="pagination"
      />
    </el-footer>
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim } from "@/utils/common.js";

import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import { getStyle } from "@/utils/Dom.js";
import BigNumber from "bignumber.js";
// import { mapGetters } from "vuex";

export default {
  name: "viewsinventorySalesOut1indexvue",
  components: {
    Pagination
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
  data() {
    return {
      sortColumn: "",
      sortOrder: "",
      showtips: true,
      btnAip: "",
      pathData: "", //路由信息
      pathDataView: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      dtData: {
        whSendOrder: "",
        warehousingType: -1,
        warehousingDate: "",
        auditStatus: -1
      },
      fullscreenLoading: false,
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      warehouseData: [], //仓库列表
      connectionData: [] //客户名称数据
    };
  },
  created() {
    // 动态头部按钮
    this.getMainList(); // 获取table数据
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.getCustomer(); //客户名称
  },
  mounted() {
    this.setStyle();
    this.pathData = getName(
      "viewsinventorySalesReceipt1indexvue",
      this.$store.getters.userPermission
    );
  },
  activated() {
    this.setStyle();
  },
  methods: {
    goPage(item, type) {
      // console.log(this.pathData.name)
      var name = "";
      var param = {};
      if (type == 1) {
        // name = "HHHXSCK1";
        // param = {
        //   item,
        //   type
        // };
        this.$router.push({
          path: "HHHXSCK1",
          query: {
            id: this.btoa(item.id)
          }
        });
        return;
      }
      if (type == 2) {
        name = "viewsinventorySalesReceipt1indexvue";
        param = {
          item,
          type
        };
      }
      if (type == 3) {
        name = "viewsinventorySalesReceipt1indexvue";
        param = {
          item: {},
          type
        };
      }
      if (type == 4) {
        // name = "HLDYC";
        // param = {
        //   item,
        //   type: 1
        // };
        this.$router.push({
          path: "HLDYC",
          query: {
            id: this.btoa(item.id)
          }
        });
        return;
      }
      this.$router.push({
        name: name,
        params: param
      });
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        // this.tableHeight = b - h - f - 40;
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.tableHeight = b - h - f - parseInt(pt) - parseInt(pb);
      });
    },
    getSummaries(param) {
      //table总计
      const { columns, data } = param;
      const sums = [];
      columns.forEach((column, index) => {
        if (index === 0) {
          sums[index] = "总计";
          return;
        }
        const values = data.map(item => {
          return Number(item[column.property]);
        });
        if (!values.every(value => isNaN(value))) {
          sums[index] = values.reduce((prev, curr) => {
            const value = Number(curr);
            if (!isNaN(value)) {
              return BigNumber(prev).plus(curr);
            } else {
              return prev;
            }
          }, 0);
          if (index == 6 || index == 7) {
            sums[index] += "";
          } else {
            sums[index] = "";
          }
        } else {
          sums[index] = "";
        }
      });

      return sums;
    },
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList();
    },
    getMainListIndex() {
      this.pageIndex = 1;
      this.getMainList();
    },
    getWarehouseData() {
      // 查看仓库档案信息
      var reqObject;
      reqObject = RequestObject.CreateGetObject(false, 0, 0, []);
      request({
        url: "/basicset/api/TBMWarehouseFile",
        method: "get",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.warehouseData = res.data;
        }
      });
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
    addPutInStorage(item) {
      this.showtips = false;
      //删除
      var currData = {
        id: item.id,
        _LogName: `删除ID：${item.id} 的销售出库单`
      };
      var data = { postData: currData };

      this.$confirm("是否删除出库单？", "确认信息", {
        type: "warning"
      })
        .then(() => {
          this.postData(data);
          this.showtips = true;
        })
        .catch(() => {
          this.showtips = true;
        });
    },
    postData(data) {
      var method = "DELETE";
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/TWMSalesMain",
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "删除成功",
            type: "success"
          });
          this.getMainList();
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
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
      var list = [
        {
          column: "customerId",
          content: this.dtData.WarehousingType,
          condition: 6
        },
        {
          column: "IsMaterial",
          content: 1,
          condition: 0
        }
      ];
      // if (this.dtData.warehousingDate != null) {
      //   if (this.dtData.warehousingDate[0]) {
      //     var startDate = formatDate(this.dtData.warehousingDate[0]);
      //     var endDate = formatDate(this.dtData.warehousingDate[1]);
      //     list.push({
      //       column: "whSendDate",
      //       content: this.dtData.warehousingDate[0]
      //         ? startDate + "," + endDate
      //         : "",
      //       condition: 10
      //     });
      //   }
      // }
      if (this.dtData.whSendOrder != "") {
        list.push({
          column: "whSendOrder",
          content: trim(this.dtData.whSendOrder),
          condition: 6
        });
      }
      if (this.dtData.auditStatus != -1) {
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
        url: "warehouse/api/TWMSalesMain/GetMainList",
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
#SalesOut /deep/ {
  .header {
    padding-top: 20px;
  }
  .link {
    color: #1890ff;
    cursor: pointer;
  }
  .el-header .el-form {
    border-bottom: 1px solid #eee;
    margin-bottom: 20px;
  }
  .el-table {
    overflow: visible !important;
  }
}
</style>
