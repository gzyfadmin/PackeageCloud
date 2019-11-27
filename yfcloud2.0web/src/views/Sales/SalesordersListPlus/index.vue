<template>
  <div id="bodys" :style="{height:tableHeight+'px'}">
    <el-container id="hights">
      <el-header height="auto" class="dashboard-editor-container">
        <el-row :gutter="32">
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>订单金额</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper">
              <MarneyChar ref="marneyChar" style="margin:0 auto" v-if="noData1" />
              <div v-else class="nolist">{{noDataMsg}}</div>
            </div>
          </el-col>
          <el-col :xs="12" :sm="12" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>业务员业绩</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper">
              <SalesPerformancep ref="SalesPerformancep" style="margin:0 auto" v-if="noData2" />
              <div v-else class="nolist">{{noDataMsg}}</div>
            </div>
          </el-col>
          <el-col :xs="12" :sm="12" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>款型分布</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper">
              <div class="distributionCharBox">
                <DistributionChar ref="DistributionChar" style="margin:0 auto" v-if="noData3" />
                <div v-else class="nolist">{{noDataMsg}}</div>
              </div>
            </div>
          </el-col>
        </el-row>
      </el-header>
      <el-main>
        <el-row>
          <el-col :span="24" class="headerBox">
            <el-form ref="form" :model="form" label-width="80px">
              <el-form-item label="时间区间：">
                <el-date-picker
                  v-model="dtData.timeZone"
                  style="width:400px;"
                  type="daterange"
                  range-separator="至"
                  start-placeholder="开始日期"
                  end-placeholder="结束日期"
                  @change="loadingGetSalesList(true)"
                />
              </el-form-item>
              <el-form-item label="客户名称：">
                <el-select
                  v-model="dtData.clientValue"
                  placeholder="请选择"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in options"
                    :key="item.id"
                    :label="item.customerName"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label="订单类型：">
                <el-select
                  v-model="dtData.orderFormValue"
                  placeholder="请选择"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in optionse"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label="业务员：">
                <el-select
                  v-model="dtData.salesManName"
                  placeholder="请选择"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in salesManData"
                    :key="item.value"
                    :label="item.name"
                    :value="item.value"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label-width="0px">
                <el-button :loading="btnLoading" type="primary" @click="loadingGetSalesList(true,true)">查询</el-button>
              </el-form-item>
              <el-form-item class="pr" label-width="0px">
                <div class="shareItem">
                  <!-- <i class="shareLg"></i> -->
                  <!-- <span>分享</span> -->
                </div>
              </el-form-item>
            </el-form>
          </el-col>
          <el-col>
            <template>
              <el-table
                :data="tableData"
                style="width: 99.99%"
                border
                height="400"
                :header-cell-style="{background:'rgb(236,240,249)'}"
                @sort-change="handelSortChange"
                v-loadmore="loadMore"
                :show-summary="true"
                :summary-method="getSummaries"
              >
                <el-table-column prop="orderNo" label="销售订单号" width="180" sortable="custom" />
                <el-table-column prop="packageName" label="包型名称" width="180" />
                <el-table-column prop="customerName" label="客户名称" />
                <el-table-column prop="salesManName" label="业务员" />
                <el-table-column prop="orderTypeName" label="订单类型" />
                <el-table-column prop="salesNum" label="订单数量" />
                <el-table-column prop="salesAmount" label="单价" />
                <el-table-column prop="salesAmountCount" label="订单金额" width="120" />
                <el-table-column prop="auditStatus" label="状态">
                  <template slot-scope="scope">
                    <div v-if="scope.row.auditStatus==0">待审核</div>
                    <div v-if="scope.row.auditStatus==1">未通过</div>
                    <div v-if="scope.row.auditStatus==2">通过</div>
                  </template>
                </el-table-column>
                <el-table-column prop="orderDate" label="日期" width="90" sortable="custom">
                  <template slot-scope="scope">
                    <div>{{scope.row.orderDate |formatTDate}}</div>
                  </template>
                </el-table-column>
              </el-table>
            </template>
          </el-col>
        </el-row>
      </el-main>
    </el-container>
  </div>
</template>
<script>
import MarneyChar from "./components/MarneyChar";
import SalesPerformancep from "./components/SalesPerformancep";
import DistributionChar from "./components/DistributionChar";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { formatDate } from "@/utils/common.js";
import BigNumber from 'bignumber.js'

export default {
  name: "viewsSalesSalesordersListPlusindexvue",
  components: {
    MarneyChar,
    SalesPerformancep,
    DistributionChar
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
      btnLoading:false,
      sortColumn: "",
      sortOrder: "",
      form: {},
      stop: false,
      fullscreenLoading: false,
      PageSize: 25,
      PageIndex: 1,
      Amount:0,
      Num:0,
      tableData: [],
      dtData: {
        timeZone: [],
        clientValue: -1,
        orderFormValue: -1,
        salesManName: -1
      },
      options: [],
      optionse: [],
      salesManData: [],
      noData1: true,
      noData2: true,
      noData3: true,
      noDataMsg: "暂时没有数据",
      tableHeight: 500
    };
  },
  created() {
    this.setStyle();
    var firstDate = new Date();
    var start = firstDate.setDate(1); //第一天
    var endDate = new Date(firstDate);
    endDate.setMonth(firstDate.getMonth() + 1);
    var end = endDate.setDate(0);
    this.dtData.timeZone.push(start);
    this.dtData.timeZone.push(end);

    this.loadingSalesAmountCount();
    this.loadingGetSalesmanAmountCount();
    this.loadingGetDistributionChar();
    this.loadingGetCustomers();
    this.loadingGetSalesList();
    this.getRuler();
    this.getType();
  },
  mounted() {
    // this.setHeight();
  },
  methods: {
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
          if (
            index == 5 
          ) {
            sums[index] = this.Num;
          }else if(index == 7) {
            sums[index] = this.Amount;
          } else {
            sums[index] = "";
          }
        } else {
          sums[index] = "";
        }
      });

      return sums;
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        this.tableHeight = b;
      });
    },
    async loadMore() {
      if (!this.stop) {
        this.stop = true;
        this.PageIndex = this.PageIndex + 1;
        await this.loadingGetSalesList();
        return;
      }
    },
    handelSortChange(currSort) {
      this.stop = true;
      this.PageIndex = 1;
      this.tableData = [];
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.loadingGetSalesList();
    },
    // setHeight() {
    //   var bodyHeight = document.getElementById("bodys");
    // },
    /**
     * 获取销售金额订单统计=订单金额
     * loadingSalesAmountCount()
     * API:/report/api/SalesReport/GetSalesAmountCount
     */
    loadingSalesAmountCount() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/SalesReport/GetSalesAmountCount",
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (res.data.seriesData[0] == "" && res.data.seriesData[1] == "") {
            this.noData1 = false;
          } else {
            this.noData1 = true;
            this.$refs["marneyChar"].createChart(res.data);
          }
        }
      });
    },
    /**
     * 获取当月分销售金额订单统计
     * loadingGetSalesmanAmountCount
     * API:/report/api/SalesReport/GetSalesmanAmountCount
     */
    loadingGetSalesmanAmountCount() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/SalesReport/GetSalesmanAmountCount",
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (res.data.length > 0) {
            this.noData2 = true;
            this.$refs["SalesPerformancep"].lodingData(res.data);
          } else {
            this.noData2 = false;
          }
        }
      });
    },
    /**
     * 获取当月款型分布统计
     * loadingGetDistributionChar
     * API:/report/api/SalesReport/GetSalePackageCount
     */
    loadingGetDistributionChar() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/SalesReport/GetSalePackageCount",
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (res.data.length > 0) {
            this.noData3 = true;
            this.$refs["DistributionChar"].packageData(res.data);
          } else {
            this.noData3 = false;
          }
        }
      });
    },
    /**
     * loadingGetCustomers
     * 获取所有客户已产生的订单
     * API:/report/api/SalesReport/GetCustomers
     *
     */
    loadingGetCustomers() {
      var requestData = RequestObject.CreateGetObject();
      // console.log(requestData)
      request({
        url: "/report/api/SalesReport/GetCustomers",
        methods: "get",
        params: requestData
      }).then(res => {
        this.options = res.data;
        // console.log(res, '所有客户的订单统计')
      });
    },
    /**
     * loadingGetSalesList
     * 获取销售一览表信息
     * API:/api/SalesReport/GetSalesList
     *
     */
    loadingGetSalesList(isTure,isBtn) {
      if (isTure) {
        this.stop = true;
        this.PageIndex = 1;
        this.tableData = [];
        // this.stop = false;
      }
      if(isBtn) {
        this.btnLoading = true;
      }
      return new Promise((reslove, reject) => {
        this.fullscreenLoading = true;
        var list = [];
        var orderConditions = [];
        if (this.dtData.timeZone != null) {
          if (this.dtData.timeZone[0]) {
            // console.log(this.dtData.timeZone);
            var startDate = formatDate(this.dtData.timeZone[0]);
            var endDate = formatDate(this.dtData.timeZone[1]);
            list.push({
              column: "orderDate",
              content: this.dtData.timeZone[0] ? startDate + "," + endDate : "",
              condition: 10
            });
          }
        }
        if (this.dtData.clientValue != -1) {
          list.push({
            column: "customerId",
            content: this.dtData.clientValue,
            condition: 0
          });
        }
        if (this.dtData.salesManName != -1) {
          list.push({
            column: "salesManId",
            content: this.dtData.salesManName,
            condition: 0
          });
        }
        if (this.dtData.orderFormValue != -1) {
          // alert(this.dtData.orderFormValue);
          list.push({
            column: "orderTypeId",
            content: this.dtData.orderFormValue,
            condition: 0
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
        var CreateGetObject = RequestObject.CreateGetObject(
          true,
          this.PageSize,
          this.PageIndex,
          list,
          orderConditions
        );
        request({
          url: "/report/api/SalesReport/GetSalesList",
          methods: "get",
          params: {
            requestObject: CreateGetObject
          }
        }).then(res => {
          if(res.code==0) {
            this.fullscreenLoading = false;
          this.Amount = res.data.total_Amount;
          this.Num = res.data.total_Num;
          // this.tableData.push(res.data);
          if (res.data.list.length < this.PageSize) {
            this.stop = true;
          } else {
            this.stop = false;
          }
          if (this.tableData.length != 0) {
            // this.tableData.push(res.data);
            if(!isBtn) {
              for (var i = 0; i < res.data.list.length; i++) {
              this.tableData.push(res.data.list[i]);
              reslove(res.data);
            }
            this.btnLoading = false; 
            }
          } else {
            this.tableData = res.data.list;
            this.btnLoading = false;
          }
          }
        });
      });
    },
    /**
     * 订单类型
     *  getType
     * API:/basicset/api/TBMDictionary
     */
    getType() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "订单类型",
          condition: 0
        }
      ];
      var requsets = {
        IsPaging: false,
        PageSize: 1,
        PageIndex: 25,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: requsets }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.optionse = res.data;
          //  dicValue
          // id
        }
      });
    },
    /**
     * 业务员
     *  getRuler
     * API:/report/api/SalesReport/GetSaleMan
     */
    getRuler() {
      var requsets = {
        IsPaging: false,
        PageSize: 1,
        PageIndex: 25,
        QueryConditions: null,
        OrderByConditions: null
      };
      request({
        url: "/report/api/SalesReport/GetSaleMan",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.salesManData = res.data;
        }
      });
    }
  }
};
</script>
<style  scoped lang="scss">
#bodys {
  background-color: rgb(243, 244, 248);
  overflow-y: scroll;
}
.el-main {
  padding-top: 0px;
}
.headerBox {
  border-top: 1px solid #dfe6ec;
  border-left: 1px solid #dfe6ec;
  border-right: 1px solid #dfe6ec;
  padding-top: 15px;
}
.dashboard-editor-container {
  padding: 20px;
  padding-bottom: 0px;
  background-color: rgb(240, 242, 245);
  position: relative;
}
.chart-wrapper {
  background: #fff;
  padding: 0px 16px 0;
  margin-bottom: 20px;
  min-height: 300px;
}
.nolist {
  background-image: url(./images/noData.png);
  background-repeat: no-repeat;
  background-position: center;
  height: 300px;
  width: 100%;
  font-size: 12px;
  color: #a2a2a2;
  padding-top: 200px;
  text-align: center;
}
.distributionCharBox {
  // max-width: 360px;
  margin: 0 auto;
}
.chartSty {
  /* background:#fff; */
  /* margin-bottom: 32px; */
}
.chartTitle {
  padding: 10px 0px 0px;
  position: relative;
  background: #fff;
  text-align: center;
  span {
    font-weight: 800;
    font-size: 14px;
  }
  .shareBox {
    position: absolute;
    right: 10px;
    top: 10px;
    color: #2cb3ff;
    font-size: 0px;
    cursor: pointer;
    span {
      font-size: 14px;
      font-weight: 200;
    }
  }
}
.shareItem {
  font-size: 0px;
  cursor: pointer;
  span {
    font-size: 14px;
    color: #2cb3ff;
  }
}
.shareLg {
  display: inline-block;
  vertical-align: -2px;
  width: 14px;
  height: 14px;
  background-image: url(./images/share.png);
  background-repeat: no-repeat;
}
.el-form-item {
  display: inline-block;
  margin-top: 18px;
  margin-left: 10px;
}
.centetFonts {
  position: absolute !important;
  top: 70%;
  left: 20%;
  z-index: 99999999999;
  width: 60%;
  height: 36px;
  line-height: 36px;
  background: rgb(149, 149, 149);
  color: white;
  font-size: 12px;
}
.block {
  display: inline-block;
  margin: 20px 20px;
}
.blocks {
  display: inline-block;
  margin: 20px 5px;
}
.el-form-item {
  margin-bottom: 15px;
  margin-top: 0px;
}
/deep/ .el-table__header-wrapper {
  overflow-y: scroll;
}
/deep/ .el-table__header-wrapper::-webkit-scrollbar {
  opacity: 0;
}
/deep/ .el-table__header-wrapper::-ms-scrollbar {
  opacity: 0;
}
/deep/ .el-table__header-wrapper::-moz-scrollbar {
  opacity: 0;
}
/deep/ .el-table__header-wrapper::-o-scrollbar {
  opacity: 0;
}
.el-table {
    overflow: visible !important;
  }
</style>
