<template>
  <div
    id="bodys"
    :style="{height:tableHeight+'px'}"
  >
    <el-container id="hights">
      <el-header
        height="auto"
        class="dashboard-editor-container"
      >
        <el-row :gutter="32">
          <el-col
            :xs="24"
            :sm="24"
            :lg="8"
            class="chartSty"
          >
            <div class="chartTitle">
              <span>供应商采购金额分析</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper pr">
              <div class="line">
                单位： 万元
              </div>
              <SalesPerformancep
                ref="SalesPerformancep"
                style="margin:0 auto"
                v-if="noData2"
              />
              <div
                v-else
                class="nolist"
              >{{noDataMsg}}</div>
            </div>
          </el-col>
          <el-col
            :xs="24"
            :sm="24"
            :lg="8"
            class="chartSty"
          >
            <div class="chartTitle">
              <span>采购订单金额态势</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper pr">
              <div class="lineTab">
                <span
                  :class="{active:activeTab==1}"
                  @click="loadingSalesAmountCount(1)"
                >天</span>
                <span
                  :class="{active:activeTab==2}"
                  @click="loadingSalesAmountCount(2)"
                >周</span>
                <span
                  :class="{active:activeTab==3}"
                  @click="loadingSalesAmountCount(3)"
                >月</span>
              </div>
              <MarneyChar
                ref="marneyChar"
                style="margin:0 auto"
                v-if="noData1"
              />
              <div
                v-else
                class="nolist"
              >{{noDataMsg}}</div>
            </div>
          </el-col>
          <el-col
            :xs="24"
            :sm="24"
            :lg="8"
            class="chartSty"
          >
            <div class="chartTitle">
              <span>采购员采购业绩</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper pr">
               <div class="line">
                单位： 万元
              </div>
              <div class="distributionCharBox">
                <DistributionChar
                  ref="DistributionChar"
                  style="margin:0 auto"
                  v-if="noData3"
                />
                <div
                  v-else
                  class="nolist"
                >{{noDataMsg}}</div>
              </div>
            </div>
          </el-col>
        </el-row>
      </el-header>
      <el-main>
        <el-row>
          <el-col
            :span="24"
            class="headerBox"
          >
            <el-form
              ref="form"
              :model="form"
              label-width="90px"
            >
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
              <el-form-item label="采购订单号：">
                <el-input
                  v-model="dtData.orderNo"
                  placeholder="采购订单号"
                  clearable
                  @keyup.enter.native="loadingGetSalesList(true)"
                  @clear="loadingGetSalesList(true)"
                >
                  <i
                    @click="loadingGetSalesList(true)"
                    style=" cursor: pointer;"
                    slot="suffix"
                    class="el-input__icon el-icon-search"
                  />
                </el-input>
              </el-form-item>
              <el-form-item label="供应商名称：">
                <el-select
                  v-model="dtData.clientValue"
                  placeholder="请选择"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option
                    label="所有"
                    :value="-1"
                  ></el-option>
                  <el-option
                    v-for="item in options"
                    :key="item.value"
                    :label="item.name"
                    :value="item.value"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label="采购员：">
                <el-select
                  v-model="dtData.salesManName"
                  placeholder="请选择"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option
                    label="所有"
                    :value="-1"
                  ></el-option>
                  <el-option
                    v-for="item in salesManData"
                    :key="item.value"
                    :label="item.name"
                    :value="item.value"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label="物料代码：">
                <el-input
                  v-model="dtData.materialCode"
                  placeholder="物料代码"
                  clearable
                  @keyup.enter.native="loadingGetSalesList(true)"
                  @clear="loadingGetSalesList(true)"
                >
                  <i
                    @click="loadingGetSalesList(true)"
                    style=" cursor: pointer;"
                    slot="suffix"
                    class="el-input__icon el-icon-search"
                  />
                </el-input>
              </el-form-item>
              <el-form-item label="物料名称：">
                <!-- <el-select
                  v-model="dtData.materialName"
                  placeholder="请选择"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option
                    label="所有"
                    :value="-1"
                  ></el-option>
                  <el-option
                    v-for="item in materialData"
                    :key="item.id"
                    :label="item.materialName"
                    :value="item.id"
                  />
                </el-select> -->
                <el-select
                  :loading="loading"
                  v-model="dtData.materialName"
                  filterable
                  clearable
                  placeholder="请输入物料名称"
                  :filter-method="remoteMethod"
                  @change="loadingGetSalesList(true)"
                >
                  <el-option
                    v-for="item in materialOptions"
                    :key="item.id"
                    :label="item.materialName"
                    :value="item.id"
                  >
                  </el-option>
                </el-select>
                <!-- <el-input
                  v-model="dtData.materialName"
                  placeholder="物料名称"
                  clearable
                  @keyup.enter.native="loadingGetSalesList(true)"
                  @clear="loadingGetSalesList(true)"
                >
                  <i
                    @click="loadingGetSalesList(true)"
                    style=" cursor: pointer;"
                    slot="suffix"
                    class="el-input__icon el-icon-search"
                  />
                </el-input> -->
              </el-form-item>
              <el-form-item label-width="0px">
                <el-button
                :loading="btnLoading"
                  type="primary"
                  @click="loadingGetSalesList(true,true)"
                >查询</el-button>
              </el-form-item>
              <el-form-item
                class="pr"
                label-width="0px"
              >
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
                <el-table-column
                  prop="orderNo"
                  label="采购订单号"
                  width="180"
                  sortable="custom"
                />
                <el-table-column
                  prop="materialCode"
                  label="物料代码"
                  width="100"
                />
                <el-table-column
                  prop="materialName"
                  label="物料名称"
                  width="180"
                />
                <el-table-column
                  prop="supplierName"
                  label="供应商名称"
                />
                <el-table-column
                  prop="buyerName"
                  label="采购员"
                />
                <el-table-column
                  prop="purchaseNum"
                  label="订单数量"
                />
                <el-table-column
                  prop="unitPrice"
                  label="单价"
                />
                <el-table-column
                  prop="purchaseAmount"
                  label="金额"
                  width="120"
                />
                <el-table-column prop="auditStatus" label="状态">
                  <template slot-scope="scope">
                    <div v-if="scope.row.auditStatus==0">待审核</div>
                    <div v-if="scope.row.auditStatus==1">未通过</div>
                    <div v-if="scope.row.auditStatus==2">通过</div>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="orderDate"
                  label="日期"
                  width="90"
                  sortable="custom"
                >
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
import BigNumber from "bignumber.js";
import { delcommafy } from "@/utils/common.js";

export default {
  name: "viewsPurchasePurchaseListPlusindexvue",
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
      loading: false,
      btnLoading:false,
      sortColumn: "",
      sortOrder: "",
      form: {},
      stop: false,
      fullscreenLoading: false,
      activeTab: 0,
      PageSize: 25,
      PageIndex: 1,
      Amount:0,
      Num:0,
      tableData: [],
      dtData: {
        timeZone: [],
        clientValue: -1,
        orderFormValue: -1,
        salesManName: -1,
        orderNo: "",
        materialCode: "",
        materialName: ""
      },
      options: [],
      materialData: [],
      materialOptions: [],
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

    this.loadingSalesAmountCount(1);
    this.loadingGetSalesmanAmountCount();
    this.loadingGetDistributionChar();
    this.loadingGetCustomers();
    this.loadingGetSalesList();
    this.getRuler();
    this.getMateria();
  },
  mounted() {},
  methods: {
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
        this.stop = false;
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
    /**
     * 获取采购订单金额态势
     * loadingSalesAmountCount()
     * API:/report/api/PurchaseReport/GetPurchaseAmountCountDay
     */
    loadingSalesAmountCount(state) {
      if (this.activeTab == state) {
        return;
      }
      this.activeTab = state;
      var url = "";
      if (state == 1) {
        url = "/report/api/PurchaseReport/GetPurchaseAmountCountDay";
      } else if (state == 2) {
        url = "/report/api/PurchaseReport/GetPurchaseAmountCountWeek";
      } else if (state == 3) {
        url = "/report/api/PurchaseReport/GetPurchaseAmountCountMonth";
      }
      var requestData = RequestObject.CreateGetObject();
      request({
        url: url,
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          var sum = 0;
          if (state == 1) {
            for (var i = 0; i < res.data.seriesData.length; i++) {
              if (res.data.seriesData[i] == "0.00") {
                sum++;
              }
            }
          } else {
            for (var i = 0; i < res.data.seriesData.length; i++) {
              if (res.data.seriesData[i] == "") {
                sum++;
              }
            }
          }
          if (sum == res.data.seriesData.length) {
            this.noData1 = false;
          } else {
            this.noData1 = true;
            var data = res.data;
            var seriesData = [];
            var num = "";
            for (var i = 0; i < data.seriesData.length; i++) {
              if(data.seriesData[i]!="") {
                num = BigNumber(delcommafy(data.seriesData[i]))
                .div(10000)
                .toNumber();
              }else {
                num = 0;
              }
              seriesData.push(num);
            }
            data.seriesData = seriesData;
            this.$nextTick(()=>{
              this.$refs["marneyChar"].createChart(data);
            })
          }
          // if (res.data.seriesData[0] == "" && res.data.seriesData[1] == "") {
          //   this.noData1 = false;
          // } else {
          //   this.noData1 = true;
          //   this.$refs["marneyChar"].createChart(res.data);
          // }
        }
      });
    },
    /**
     * 获取供应商采购金额分析统计
     * loadingGetSalesmanAmountCount
     * API:/report/api/PurchaseReport/GetPurchaseSupplierAmountCount
     */
    loadingGetSalesmanAmountCount() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/PurchaseReport/GetPurchaseSupplierAmountCount",
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
     * 获取采购员采购业绩分布统计
     * loadingGetDistributionChar
     * API:/report/api/PurchaseReport/GetPurchaseBuyerAmountCount
     */
    loadingGetDistributionChar() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/PurchaseReport/GetPurchaseBuyerAmountCount",
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
     * 获取所有供应商名称已产生的订单
     * API:/report/api/SalesReport/GetCustomers
     *
     */
    loadingGetCustomers() {
      var requestData = RequestObject.CreateGetObject();
      // console.log(requestData)
      request({
        url: "/report/api/PurchaseReport/GetSupplier",
        methods: "get",
        params: requestData
      }).then(res => {
        this.options = res.data;
        // console.log(res, '所有供应商名称的订单统计')
      });
    },
    /**
     * loadingGetSalesList
     * 获取采购一览表信息
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
            column: "supplierId",
            content: this.dtData.clientValue,
            condition: 6
          });
        }
        if (this.dtData.salesManName != -1) {
          list.push({
            column: "buyerId",
            content: this.dtData.salesManName,
            condition: 6
          });
        }
        if (this.dtData.materialName) {
          // alert(this.dtData.materialName);
          list.push({
            column: "materialId",
            content: this.dtData.materialName,
            condition: 6
          });
        }
        if (this.dtData.orderNo) {
          // alert(this.dtData.orderFormValue);
          list.push({
            column: "orderNo",
            content: this.dtData.orderNo,
            condition: 6
          });
        }
        if (this.dtData.materialCode) {
          // alert(this.dtData.orderFormValue);
          list.push({
            column: "materialCode",
            content: this.dtData.materialCode,
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
        var CreateGetObject = RequestObject.CreateGetObject(
          true,
          this.PageSize,
          this.PageIndex,
          list,
          orderConditions
        );
        request({
          url: "/report/api/PurchaseReport/GetPurchase",
          methods: "get",
          params: {
            requestObject: CreateGetObject
          }
        }).then(res => {
          if(res.code==0) {
            this.fullscreenLoading = false;
          // this.tableData.push(res.data.list);
          this.Amount = res.data.total_Amount;
          this.Num = res.data.total_Num;
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
              reslove(res.data.list);
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
    getSummaries(param) {
        const { columns, data } = param;
        const sums = [];
        columns.forEach((column, index) => {
          if (index === 0) {
            sums[index] = '总计';
            return;
          }
          const values = data.map(item => Number(item[column.property]));
          if (!values.every(value => isNaN(value))) {
            sums[index] = values.reduce((prev, curr) => {
              const value = Number(curr);
              if (!isNaN(value)) {
                return prev + curr;
              } else {
                return prev;
              }
            }, 0);
            if (index == 5) {
            sums[index] = this.Num
          }else if(index == 7) {
             sums[index] = this.Amount
          } else {
            sums[index] = ''
          }
          }
        });

        return sums;
    },
    /**
     * remoteMethod
     * 过滤物料名称
     */
    async remoteMethod(query) {
      if (query !== "") {
        this.loading = true;
        setTimeout(() => {
          this.materialOptions = this.materialData.filter(item => {
            return (
              item.materialName.toLowerCase().indexOf(query.toLowerCase()) > -1
            );
          });
        }, 200);
        this.loading = false;
      } else {
        this.materialOptions = [];
      }
    },
    /**
     * 物料名称
     *  getMateria
     * API:/basicset/api/TBMDictionary
     */
    getMateria() {
      var requsets = {
        IsPaging: false,
        PageSize: 1,
        PageIndex: 25,
        QueryConditions: null,
        OrderByConditions: null
      };
      request({
        url: "/basicset/api/TBMMaterialFile",
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
          this.materialData = res.data;
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
        url: "/report/api/PurchaseReport/GetSaleMan",
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
@import "@/styles/report.scss";
.line {
   position: absolute;
  font-size: 14px;
  left: 10%;
  top: 15px;
  z-index: 2;
}
.lineTab {
  position: absolute;
  font-size: 14px;
  right: 10%;
  top: 15px;
  z-index: 2;
  cursor: pointer;
  span {
    margin-left: 4px;
    color: #cccccc;
  }
  .active {
    color: #333333;
  }
}
</style>
