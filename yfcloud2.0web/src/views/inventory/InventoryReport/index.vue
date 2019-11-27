<template>
  <div id="bodys" :style="{height:tableHeight+'px'}">
    <el-container id="hights">
      <el-header height="auto" class="dashboard-editor-container">
        <el-row :gutter="32">
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>总入库量、出库量、库存分析图</span>
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
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>成品库存态势</span>
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
          <el-col :xs="24" :sm="24" :lg="8" class="chartSty">
            <div class="chartTitle">
              <span>库存量分析</span>
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
                  @change="loadingTableList(true)"
                />
              </el-form-item>
               <el-form-item label="物料名称：">
                <el-select
                  v-model="dtData.materialName"
                  placeholder="请输入物料名称"
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
                    :label="item.materialName"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>

              <el-form-item label="物料代码：">
                <el-select
                  v-model="dtData.materialCode"
                  placeholder="请输入物料代码"
                  @change="loadingTableList(true)"
                  filterable
                  clearable
                  :filter-method="filterMethod2"
                  :loading="loading"
                >
                  <!-- <el-option label="所有" :value="-1"></el-option> -->
                  <el-option
                    v-for="item in optionse"
                    :key="item.id"
                    :label="item.materialCode"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label="仓库名称：">
                <el-select
                  v-model="dtData.warehouseName"
                  placeholder="请选择"
                  @change="loadingTableList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in warehouseData"
                    :key="item.id"
                    :label="item.warehouseName"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
              <el-form-item label="物料分类：">
                <el-select
                  v-model="dtData.materialTypeName"
                  placeholder="请选择"
                  @change="loadingTableList(true)"
                >
                  <el-option label="所有" :value="-1"></el-option>
                  <el-option
                    v-for="item in classifyData"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
             
              <el-form-item label-width="0px">
                <el-button :loading="btnLoading" type="primary" @click="loadingTableList(true,true)">查询</el-button>
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
                <el-table-column prop="materialCode" label="物料代码" width="180" sortable="custom" />
                <el-table-column prop="materialName" label="物料名称" width="180" />
                <el-table-column prop="materialTypeName" label="物料分类" />
                <el-table-column prop="toDayStorageAmount" label="今日入库量" />
                <el-table-column prop="totalStorageAmount" label="累计入库量" />
                <el-table-column prop="toDayOutStorageAmount" label="今日出库量" />
                <el-table-column prop="totalOutStorageAmount" label="累计出库量" />
                <el-table-column prop="totalNum" label="库存量" />
                <el-table-column prop="totalAmount" label="金额" />
                <el-table-column prop="warehouseName" label="仓库名称" />
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
  name: "viewsinventoryInventoryReportindexvue",
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
      dataList:{},
      btnLoading: false,
      classifyData: "", //物料分类
      materialCode: "",
      materialName: "",
      materialCode_: "",
      materialName_: "",
      warehouseData: "", //仓库数据
      loading: false,
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
        materialCode: "", //物料代码
        materialName: "", //物料名称
        materialTypeName: -1, //物料分类
        warehouseName: -1, //仓库
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

    this.loadingSalesAmountCount(); //表一
    this.loadingGetSalesmanAmountCount(); //表二
    this.loadingGetDistributionChar(); //表三
    //this.loadingGetCustomers(); //获取物料名称物料代码
    this.loadingTableList(); //获取table数据
    this.getClassify(); //物料分类
    this.getWarehouseData().then(res => {
      this.warehouseData = res;
    });
  },
  mounted() {},
  methods: {
    async filterMethod1(query) {
      if (trim(query).length < 2) {
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
        this.materialName = "";
        this.optionse = [];
      }
    },
    async filterMethod2(query) {
      if (trim(query).length < 2) {
        return;
      }
      if (query !== "") {
        this.materialCode = query;
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
            index == 4 ||
            index == 5 ||
            index == 6 ||
            index == 7 ||
            index == 8 ||
            index == 9
          ) {
            // sums[index] += "";
            if(index==4){
              sums[index] = this.dataList["toDayinNum"]
            }
            if(index==5){
              sums[index] = this.dataList["totalinNum"]
            }
            if(index==6){
              sums[index] = this.dataList["toDayoutNum"]
            }
            if(index==7){
              sums[index] = this.dataList["totaloutNum"]
            }
            if(index==8){
              sums[index] = this.dataList["totalNum"]
            }
            if(index==9){
              sums[index] = this.dataList["totalAmount"]
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
      this.stop = true;
      this.PageIndex = 1;
      this.tableData = [];
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.loadingTableList();
    },
    loadingSalesAmountCount() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/InventoryReport/GetInventoryAmountCount",
        methods: "get",
        params: requestData
      }).then(res => {
        if (res.code == 0) {
          if (
            res.data.inAmountCount == 0 &&
            res.data.inventory == 0 &&
            res.data.outAmountCount == 0
          ) {
            this.noData1 = false;
          } else {
            this.noData1 = true;
            this.$refs["marneyChar"].createChart(res.data);
          }
        } else {
          this.noData1 = false;
        }
      });
    },
    loadingGetSalesmanAmountCount() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/InventoryReport/GetInventoryAmountCountDay",
        methods: "get",
        params: requestData
      }).then(res => {
        // console.log(res.data)
        if (res.code == 0) {
          // if (res.data.length > 0) {
          //   this.noData2 = true;
          if (res.data.seriesData.length > 0) {
            this.noData2 = true;
            this.$refs["SalesPerformancep"].lodingData(res.data);
          } else {
            this.noData2 = false;
          }
          // } else {
          //   this.noData2 = false;
          // }
        }
      });
    },
    loadingGetDistributionChar() {
      var requestData = RequestObject.CreateGetObject();
      request({
        url: "/report/api/InventoryReport/GetInventoryAnalysis",
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
            column: "materialCode",
            content: item.materialCode,
            condition: 6
          });
        }
        if (item.materialName != "") {
          queryData.push({
            column: "materialName",
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
          url: "/basicset/api/TBMMaterialFile",
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
    loadingTableList(isTure,isBtn) {
      if (isTure) {
        this.stop = true;
        this.PageIndex = 1;
        this.tableData = [];
        // this.stop = false;
      }
      if(isBtn) {
         this.btnLoading = true;
      }
      if (this.dtData.materialName == "") {
        this.materialName = "";
        this.materialName_ = "";
      }
      if (this.dtData.materialCode == "") {
        this.materialCode = "";
        this.materialCode_ = "";
      }
      this.optionse.map(item => {
        if (item.id == this.dtData.materialName) {
          this.materialName_ = item.materialName;
        }
        if (item.id == this.dtData.materialCode) {
          this.materialCode_ = item.materialCode;
        }
      });
      return new Promise((reslove, reject) => {
        this.fullscreenLoading = true;
        var list = [];
        var orderConditions = [];
        if (this.dtData.timeZone != null) {
          if (this.dtData.timeZone[0]) {
            var startDate = formatDate(this.dtData.timeZone[0]);
            var endDate = formatDate(this.dtData.timeZone[1]);
            list.push({
              column: "WarehousingDate",
              content: this.dtData.timeZone[0] ? startDate + "," + endDate : "",
              condition: 10
            });
          }
        }
        if (this.materialName_ != "") {
          list.push({
            column: "materialName",
            content: this.materialName_,
            condition: 0
          });
        }
        if (this.materialCode_ != "") {
          list.push({
            column: "materialCode",
            content: this.materialCode_,
            condition: 0
          });
        }
        if (this.dtData.warehouseName != -1) {
          list.push({
            column: "warehouseId",
            content: this.dtData.warehouseName,
            condition: 0
          });
        }
        if (this.dtData.materialTypeName != -1) {
          list.push({
            column: "materialTypeId",
            content: this.dtData.materialTypeName,
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
              column: "materialCode",
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
          url: "/report/api/InventoryReport/GetInventoryCountList",
          methods: "get",
          params: {
            requestObject: CreateGetObject
          }
        }).then(res => {
          if (res.code == 0) {
            this.fullscreenLoading = false;
            if (res.data.list.length < this.PageSize) {
              this.dataList = res.data;
              this.stop = true;
            } else {
              this.stop = false;
            }
            if (this.tableData.length != 0) {
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
              return;
            }
          }
        });
      });
    },
    getWarehouseData() {
      // 查看仓库档案信息
      return new Promise(function(reslove, reject) {
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
            // this.warehouseData = res.data;
            reslove(res.data);
          }
        });
      });
    },
    getClassify() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "物料分类",
          condition: 0
        }
      ];
      var requsets = {
        IsPaging: false,
        PageSize: this.pageSize,
        PageIndex: this.pageIndex,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
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
          this.classifyData = res.data;
        }
      });
    }
  }
};
</script>
<style  scoped lang="scss">
@import "@/styles/report.scss";
</style>
