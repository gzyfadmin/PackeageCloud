<template>
  <div id="bodys" :style="{height:tableHeight+'px'}">
    <el-container id="hights">
      <el-header height="auto" class="dashboard-editor-container">
        <el-row :gutter="32">
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>生产车间入库数量</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper">
              <DistributionChar ref="DistributionChar" style="margin:0 auto" v-if="noData3" />
              <div v-else class="nolist">{{noDataMsg}}</div>
            </div>
          </el-col>
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>生产入库态势</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper pr">
              <div class="lineTab">
                <span :class="{active:activeTab==1}" @click="loadingGetSalesmanAmountCount(1)">天</span>
                <span :class="{active:activeTab==2}" @click="loadingGetSalesmanAmountCount(2)">周</span>
                <span :class="{active:activeTab==3}" @click="loadingGetSalesmanAmountCount(3)">月</span>
              </div>
              <SalesPerformancep ref="SalesPerformancep" style="margin:0 auto" v-if="noData2" />
              <div v-else class="nolist">{{noDataMsg}}</div>
            </div>
          </el-col>
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>生产订单数量分析</span>
              <div class="shareBox">
                <!-- <i class="shareLg"></i> -->
                <!-- <span>分享</span> -->
              </div>
            </div>
            <div class="chart-wrapper">
              <div class="distributionCharBox">
                <MarneyChar ref="marneyChar" style="margin:0 auto" v-if="noData1" />
                <div v-else class="nolist">{{noDataMsg}}</div>
              </div>
            </div>
          </el-col>
        </el-row>
      </el-header>
      <el-main>
        <el-row>
          <el-col :span="24" class="headerBox">
            <el-form ref="form" :model="form" label-width="76px">
              <el-form-item label="时间区间：">
                <el-date-picker
                  v-model="dtData.timeZone"
                  style="width:400px;"
                  type="daterange"
                  range-separator="至"
                  start-placeholder="开始日期"
                  end-placeholder="结束日期"
                  @change="loadingTableList(true)"
                />
              </el-form-item>

              <el-form-item label="生产订单号：" label-width="90px">
                <el-input
                  v-model="dtData.productionNo"
                  placeholder="生产订单号"
                  clearable
                  @keyup.enter.native="loadingTableList(true)"
                  @clear="loadingTableList(true)"
                >
                  <i
                    @click="loadingTableList(true)"
                    style=" cursor: pointer;"
                    slot="suffix"
                    class="el-input__icon el-icon-search"
                  />
                </el-input>
              </el-form-item>

              <el-form-item label="生产车间：">
                <el-select
                  v-model="dtData.workshopName"
                  placeholder="请选择"
                  @change="loadingTableList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in workshopOption"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>

              <el-form-item label="包型名称：">
                <el-select
                  v-model="dtData.materialName"
                  placeholder="请输入关键词"
                  @change="loadingTableList(true)"
                  filterable
                  clearable
                  :filter-method="filterMethod1"
                  :loading="loading"
                >
                  <!-- <el-option label="所有" :value="-1"></el-option> -->
                  <el-option
                    v-for="item in optionse"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>

              <el-form-item label="客户名称：">
                <el-select
                  v-model="dtData.customerName"
                  placeholder="请选择"
                  @change="loadingTableList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in connectionData"
                    :key="item.id"
                    :label="item.customerName"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>

              <el-form-item label="订单类型：">
                <el-select
                  v-model="dtData.productionTypeName"
                  placeholder="请选择"
                  @change="loadingTableList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in orderTypeoption"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label-width="0px">
                <el-button
                  :loading="btnLoading"
                  type="primary"
                  @click="loadingTableList(true,true)"
                >查询</el-button>
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
                <el-table-column label="序号" width="70">
                  <template slot-scope="scope">
                    <div>{{scope.$index+1}}</div>
                  </template>
                </el-table-column>
                <el-table-column prop="productionNo" label="生产订单号" width="180" />
                <el-table-column prop="packageName" label="包型名称" width="180" />
                <el-table-column prop="customerName" label="客户名称" width="120" />
                <el-table-column prop="productionTypeName" label="订单类型" />
                <el-table-column prop="productionNum" label="订单数量" />
                <el-table-column prop="workshopName" label="生产车间" />
                <el-table-column prop="toDayNum" label="今日入库数量" />
                <el-table-column prop="totalNum" label="累计入库数量" />
                <el-table-column prop="orderDate" label="日期" width="90">
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
import { formatDate, trim } from "@/utils/common.js";
import BigNumber from "bignumber.js";

export default {
  name: "viewsProductProductionReportindexvue",
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
      btnLoading: false,
      dataList: "",
      materialCode: "", //物料代码
      materialName: "", //物料名称
      connectionData: [], //客户名称列表
      orderTypeoption: [
        {
          dicValue: "库存生产",
          id: 0
        },
        {
          dicValue: "订单生产",
          id: 1
        }
      ], //订单类型列表
      loading: false,
      workshopOption: [], //车间数据
      ChartData1: [],
      ChartData2: [],
      ChartData3: [],
      activeTab: 0,
      sortColumn: "",
      sortOrder: "",
      form: {},
      stop: false,
      fullscreenLoading: false,
      PageSize: 25,
      PageIndex: 1,
      tableData: [],
      dtData: {
        timeZone: [],
        productionNo: "",
        workshopName: -1, //车间
        materialCode: "", //物料代码
        materialName: "", //物料名称
        customerName: -1, //客户名称
        productionTypeName: -1, //订单类型
        orderFormValue: -1,
        salesManName: -1
      },
      materialData: [],
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

    this.loadingSalesAmountCount(); //表三
    this.getChartData(1); //获取表二数据
    this.getChartData(2); //获取表二数据
    this.getChartData(3); //获取表二数据
    // this.loadingGetSalesmanAmountCount(1); //表二
    this.loadingGetDistributionChar(); //表一
    // this.loadingGetCustomers(); //获取物料名称物料代码
    this.loadingTableList(); //获取table数据
    this.getWorkshopOption(); //车间数据
    this.getCustomer(); //客户名称
    // this.getorderTypeoption(); //订单类型
  },
  mounted() {},
  methods: {
    async filterMethod1(query) {
      if (trim(query).length < 1) {
        return;
      }
      if (query !== "") {
        this.materialName = query;
        this.loading = true;
        this.optionse = await this.loadingGetCustomers({
          materialCode: trim(this.materialCode),
          materialName: trim(this.materialName)
        });
        this.loading = false;
      } else {
        this.materialCode = "";
        this.optionse = [];
      }
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
        await this.loadingTableList();
        return;
      }
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.loadingGetSalesList();
    },
    getChartData(state) {
      var requestData = RequestObject.CreateGetObject();
      var URL = "";
      if (state == 1) {
        URL = "/report/api/ProductionOrderReport/GetProductionWarehousingDay";
      }
      if (state == 2) {
        URL = "/report/api/ProductionOrderReport/GetProductionWarehousingWeek";
      }
      if (state == 3) {
        URL = "/report/api/ProductionOrderReport/GetProductionWarehousingMonth";
      }
      request({
        url: URL,
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (state == 1) {
            this.ChartData1 = res;
            if (this.activeTab == 0) {
              this.loadingGetSalesmanAmountCount(1);
            }
          }
          if (state == 2) {
            this.ChartData2 = res;
          }
          if (state == 3) {
            this.ChartData3 = res;
          }
        }
      });
    },
    loadingSalesAmountCount() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url:
          "/report/api/ProductionOrderReport/GetProductionOrderQuantityAnalysis",
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (res.data.seriesData[0] == "0" && res.data.seriesData[1] == "0") {
            this.noData1 = false;
          } else {
            this.noData1 = true;
            this.$refs["marneyChar"].createChart(res.data);
          }
          // this.noData3 = true;
          // this.$refs["marneyChar"].createChart(res.data);
        } else {
          this.noData1 = false;
        }
      });
    },
    getDataLength(data) {
      var sum = 0;
      for (var i = 0; i < data.seriesData.length; i++) {
        if (data.seriesData[i] == "0.00") {
          sum++;
        }
      }
      if (sum == data.seriesData.length) {
        // this.noData1 = false;
        // console.log()
        return false;
      } else {
        // this.noData1 = true;
        return true;
      }
    },
    loadingGetSalesmanAmountCount(state) {
      if (state == this.activeTab) {
        return;
      }
      this.activeTab = state;
      if (state == 1) {
        if (this.getDataLength(this.ChartData1.data)) {
          this.noData2 = true;
          this.$nextTick(() => {
            this.$refs["SalesPerformancep"].lodingData(this.ChartData1.data);
          });
        } else {
          this.noData2 = false;
        }
      }
      if (state == 2) {
        if (this.getDataLength(this.ChartData2.data)) {
          this.noData2 = true;
          this.$nextTick(() => {
            this.$refs["SalesPerformancep"].lodingData(this.ChartData2.data);
          });
        } else {
          this.noData2 = false;
        }
        // this.$refs["SalesPerformancep"].lodingData(this.ChartData2.data);
      }
      if (state == 3) {
        if (this.getDataLength(this.ChartData3.data)) {
          this.noData2 = true;
          this.$nextTick(() => {
            this.$refs["SalesPerformancep"].lodingData(this.ChartData3.data);
          });
        } else {
          this.noData2 = false;
        }
        // this.$refs["SalesPerformancep"].lodingData(this.ChartData3.data);
      }
    },
    loadingGetDistributionChar() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/ProductionOrderReport/GetInventoryAmountCount",
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (res.data.length > 0) {
            this.noData3 = true;
            this.$refs["DistributionChar"].lodingData(res.data);
          } else {
            this.noData3 = false;
          }
        }
      });
    },
    loadingGetCustomers(item) {
      //获取物料档案信息
      return new Promise((resolve, reject) => {
        this.materielData = [];
        var queryData = [];
        var queryData = [];
        if (item.materialCode != "") {
          queryData.push({
            column: "dicCode",
            content: item.materialCode,
            condition: 6
          });
        }
        if (item.materialName != "") {
          queryData.push({
            column: "dicValue",
            content: item.materialName,
            condition: 6
          });
        }

        var reqObject;
        reqObject = RequestObject.LonginBookObject(
          false,
          0,
          0,
          null,
          queryData
        );
        request({
          url: "/basicset/api/TBMPackage",
          method: "get",
          params: {
            requestObject: JSON.stringify(reqObject)
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
            // this.materielData = res.data;
            resolve(res.data);
          }
        });
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
          if (index == 5 || index == 7 || index == 8) {
            if (index == 5) {
              sums[index] = this.dataList["total_Num"];
            }
            if (index == 7) {
              sums[index] = this.dataList["total_DayNum"];
            }
            if (index == 8) {
              sums[index] = this.dataList["total_TotalNum"];
            }
          } else {
            sums[index] = "";
          }
        } else {
          sums[index] = "";
        }
      });

      return sums;
    },
    loadingTableList(isTure, isBtn) {
      if (isTure) {
        this.stop = true;
        this.PageIndex = 1;
        this.tableData = [];
        // this.stop = false;
      }
      if (isBtn) {
        this.btnLoading = true;
      }
      return new Promise((reslove, reject) => {
        this.fullscreenLoading = true;
        var list = [];
        var orderConditions = [];
        if (this.dtData.timeZone != null) {
          if (this.dtData.timeZone[0]) {
            var startDate = formatDate(this.dtData.timeZone[0]);
            var endDate = formatDate(this.dtData.timeZone[1]);
            list.push({
              column: "orderDate",
              content: this.dtData.timeZone[0] ? startDate + "," + endDate : "",
              condition: 10
            });
          }
        }
        //生产订单号
        if (this.dtData.productionNo != "") {
          list.push({
            column: "productionNo",
            content: trim(this.dtData.productionNo),
            condition: 6
          });
        }
        //车间
        if (this.dtData.workshopName != -1) {
          list.push({
            column: "workshopId",
            content: this.dtData.workshopName,
            condition: 0
          });
        }
        //包型
        if (this.dtData.materialName != "") {
          list.push({
            column: "packageId",
            content: this.dtData.materialName,
            condition: 0
          });
        }
        //客户名称
        if (this.dtData.customerName != -1) {
          list.push({
            column: "customerId",
            content: this.dtData.customerName,
            condition: 0
          });
        }
        //订单类型
        if (this.dtData.productionTypeName != -1) {
          list.push({
            column: "productionType",
            content: this.dtData.productionTypeName,
            condition: 0
          });
        }

        // if (this.dtData.orderFormValue != -1) {
        //   alert(this.dtData.orderFormValue);
        //   list.push({
        //     column: "orderTypeId",
        //     content: this.dtData.orderFormValue,
        //     condition: 0
        //   });
        // }
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
              column: "productionNo",
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
          url: "/report/api/ProductionOrderReport/GetProdutionCountList",
          methods: "get",
          params: {
            requestObject: CreateGetObject
          }
        }).then(res => {
          this.fullscreenLoading = false;
          // this.tableData.push(res.data);
          if (res.code == 0) {
            this.dataList = res.data;
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
    getWorkshopOption() {
      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "生产车间", condition: 0 }
        ],
        orderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.workshopOption = res.data;
        }
      });
    },
    getCustomer() {
      // 获取客户名称
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
    getorderTypeoption() {
      // var rqs = RequestObject.CreateGetObject(false, 0, 0, [
      //   { column: "typeId", content: 18, condition: 0 }
      // ]);
      //订单类型
      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "订单类型", condition: 0 }
        ],
        orderByConditions: [{ column: "orderDate", condition: "desc" }]
      };

      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.orderTypeoption = res.data;
          // this.orderTypeoption.unshift({
          //   dicValue: "所有",
          //   id: -1
          // });
        }
      });
    }
  }
};
</script>
<style  scoped lang="scss">
@import "@/styles/report.scss";
.lineTab {
  position: absolute;
  font-size: 14px;
  right: 10%;
  top: 15px;
  z-index: 1;
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
