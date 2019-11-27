<template>
  <el-container
    id="OtherPut"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="76px" inline class="FormInput">
        <el-form-item label="物料代码：" class="formItem">
          <el-input
            v-model="dtData.materialCode"
            placeholder="物料代码"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
            <i
              slot="suffix"
              class="el-input__icon el-icon-search"
              @click="getMainList(pageIndex=1)"
            ></i>
          </el-input>
        </el-form-item>
        <el-form-item label="物料名称：" class="formItem">
          <el-input
            v-model="dtData.materialName"
            placeholder="物料名称"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
            <i
              slot="suffix"
              class="el-input__icon el-icon-search"
              @click="getMainList(pageIndex=1)"
            ></i>
          </el-input>
        </el-form-item>
        <el-form-item label="仓库名称：" class="formItem">
          <el-select
            v-model="dtData.warehouseId"
            placeholder="请选择"
            @change="getMainList(pageIndex=1)"
          >
            <el-option label="所有" :value="-1"></el-option>
            <el-option
              v-for="item in warehouseData"
              :key="item.id"
              :label="item.warehouseName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <span v-if="!uploadState&&btnAip.upload&&btnAip.upload.buttonCaption">
        <upload-excel-component
          :on-success="handleSuccess"
          :before-upload="beforeUpload"
          :title="btnAip.upload.buttonCaption"
        />
      </span>

      <!-- <el-button
        type="primary"
        v-if="btnAip.queryexport&&btnAip.queryexport.buttonCaption"
        @click="exportExcel"
      >查询导出</el-button>-->
      <el-button
        type="primary"
        v-if="btnAip.allexport&&btnAip.allexport.buttonCaption"
        @click="exportAll"
      >{{btnAip.allexport.buttonCaption}}</el-button>
      <el-button
        type="primary"
        v-if="!uploadState&&btnAip.exporttemp&&btnAip.exporttemp.buttonCaption"
        @click="exportTemp"
      >{{btnAip.exporttemp.buttonCaption}}</el-button>
    </el-header>
    <el-main id="elmain">
      <el-table
        v-if="tableFlag"
        ref="table"
        :height="tableHeight"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        :summary-method="getSummaries"
        show-summary
        @sort-change="handelSortChange"
      >
        <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="materialCode" label="物料代码" width="150"></el-table-column>
        <el-table-column prop="materialName" label="物料名称" width="150"></el-table-column>
        <el-table-column prop="spec" label="规格型号"></el-table-column>
        <el-table-column prop="colorName" label="颜色"></el-table-column>
        <!-- <el-table-column prop="batchNo" label="批号"></el-table-column> -->
        <el-table-column prop="warehouseCode" label="仓库代码"></el-table-column>
        <el-table-column prop="warehouseName" label="仓库名称"></el-table-column>
        <el-table-column label="基本计量单位">
          <el-table-column prop="baseUnitName" label="单位" width="120"></el-table-column>
          <el-table-column prop="baseUnitNumber" label="数量" width="150"></el-table-column>
        </el-table-column>
        <el-table-column label="库存计量单位">
          <el-table-column prop="warehouseUnitName" label="单位" width="120"></el-table-column>
          <el-table-column prop="warehouseAmount" label="数量" width="150"></el-table-column>
        </el-table-column>
        <el-table-column prop="warehouseRate" label="与基本单位换算率"></el-table-column>

        <!-- <el-table-column prop="inDate" label="生产/采购日期"></el-table-column> -->
        <el-table-column prop="shelfLife" label="保质期(天)"></el-table-column>
        <el-table-column prop="id" label="操作" fixed="right">
          <template slot-scope="scope">
            <el-tooltip
              class="item"
              effect="light"
              content="历史记录"
              placement="top-start"
              :open-delay="300"
            >
              <el-button
                v-if="btnAip.history&&btnAip.history.buttonCaption"
                type="primary"
                icon="el-icon-search"
                circle
                @click="handelShowClick(scope.row)"
              />
            </el-tooltip>
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
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit"></InboundOrder>
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim, isEmpty,islegalNum } from "@/utils/common.js";

import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import InboundOrder from "./components/InboundOrder";
import UploadExcelComponent from "@/components/UploadExcel2/index.vue";
import { getStyle } from "@/utils/Dom.js";
import BigNumber from "bignumber.js";
// import { mapGetters } from "vuex";

export default {
  name: "viewsinventoryInventoryCountindexvue",
  components: {
    Pagination,
    InboundOrder,
    UploadExcelComponent
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
      btnAip: "",
      sortColumn: "",
      sortOrder: "",
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      dtData: {
        materialCode: "",
        materialName: "",
        warehouseId: -1
      },
      fullscreenLoading: false,
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      warehouseData: [], //仓库列表
      uploadState: true,
      tableFlag: true,
      imgsrc: ""
    };
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
  },
  async mounted() {
    this.setStyle();
    this.getMainList(); // 获取table数据
    this.imgsrc = this.imgUrlName;
    this.warehouseData = await this.getWarehouseData(); //仓库数据
    this.getIsImported();
  },
  activated() {
    this.tableFlag = false;
    setTimeout(() => {
      this.tableFlag = true;
    }, 10);
  },
  methods: {
    exportTemp() {
      //导出模板
      request({
        url: "/warehouse/api/InventoryReport/ExportOpeningTemplate",
        method: "get",
        params: {}
      }).then(res => {
        if (res.code == 0) {
          window.open(`${this.imgsrc}${res.data}`);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.getMainList();
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
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList();
    },
    handelShowClick(item) {
      //点击历史记录按钮
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.WarehouseId = item.warehouseId;
      this.$refs.InboundOrder.MaterialId = item.materialId;
      this.$refs.InboundOrder.getMainList();
    },
    OnBtnSaveSubmit() {},
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
    getMainList() {
      //角色列表
      var list = [];
      var orderConditions = [];
      if (this.dtData.materialCode !== "") {
        list.push({
          column: "materialCode",
          content: trim(this.dtData.materialCode),
          condition: 6
        });
      }
      if (this.dtData.materialName !== "") {
        list.push({
          column: "materialName",
          content: trim(this.dtData.materialName),
          condition: 6
        });
      }
      if (this.dtData.warehouseId != -1) {
        list.push({
          column: "warehouseId",
          content: this.dtData.warehouseId,
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
            column: "materialCode",
            condition: "desc"
          }
        ];
      }
      var rqs = {
        isPaging: true,
        pageSize: this.pageSize,
        pageIndex: this.pageIndex,
        postData: null,
        postDataList: null,
        queryConditions: list,
        orderByConditions: orderConditions
      };

      this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "warehouse/api/InventoryReport/LoadReport",
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
    exportAll() {
      var list = [];
      if (this.dtData.materialCode !== "") {
        list.push({
          column: "materialCode",
          content: this.dtData.materialCode,
          condition: 6
        });
      }
      if (this.dtData.materialName !== "") {
        list.push({
          column: "materialName",
          content: this.dtData.materialName,
          condition: 6
        });
      }
      if (this.dtData.warehouseId != -1) {
        list.push({
          column: "warehouseId",
          content: this.dtData.warehouseId,
          condition: 6
        });
      }
      var rqs = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        postData: null,
        postDataList: null,
        queryConditions: list,
        orderByConditions: null
      };
      this.fullscreenLoading = true;
      request({
        url: "warehouse/api/InventoryReport/ExportStaReport",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.fullscreenLoading = false;
          window.open(`${this.imgsrc}${res.data}`);
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
    exportCurrent() {
      //导出
      import("@/vendor/Export2Excel").then(excel => {
        const multiHeader = [
          // ["name", this.dtData.supplier, "amount1", "amount2", "amount3"]
          [
            "物料代码",
            "物料名称码",
            "规格型号",
            "颜色",
            "批号",
            "仓库代码",
            "仓库名称",
            "基本计量单位",
            "",
            "库存计量单位",
            "",
            "与基本单位换算率",
            "生产 / 采购日期",
            "保质期"
          ]
        ];
        // 物料代码;
        // 物料名称码;
        // 规格型号;
        // 颜色;
        // 批号;
        // 仓库代码;
        // 仓库名称;
        // 基本计量单位;
        // 单位;
        // 数量;
        // 库存计量单位;
        // 与基本单位换算率;
        // 生产 / 采购日期;
        // 保质期;
        // 操作;
        const header = [
          "物料代码",
          "物料名称码",
          "规格型号",
          "颜色",
          "批号",
          "仓库代码",
          "仓库名称",
          "单位",
          "数量",
          "单位",
          "数量",
          "与基本单位换算率",
          "生产 / 采购日期",
          "保质期"
        ];
        const filterVal = [
          "materialCode",
          "materialName",
          "spec",
          "colorName",
          "batchNo",
          "warehouseCode",
          "warehouseName",
          "baseUnitName",
          "baseUnitNumber",
          "warehouseUnitName",
          "warehouseAmount",
          "warehouseRate",
          "inDate",
          "shelfLife"
        ];
        const list = this.tableData;
        const data = this.formatJson(filterVal, list);
        const merges = [
          "A1:A2",
          "B1:B2",
          "C1:C2",
          "D1:D2",
          "E1:E2",
          "F1:F2",
          "G1:G2",
          "H1:I1",
          "J1:K1",
          "L1:L2",
          "M1:M2",
          "N1:N2"
        ];
        excel.export_json_to_excel({
          multiHeader,
          header,
          merges,
          data
        });
      });
    },
    formatJson(filterVal, jsonData) {
      //导出
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === "timestamp") {
            return parseTime(v[j]);
          } else {
            return v[j];
          }
        })
      );
    },
    async beforeUpload(file) {
      //导入前验证
      // const isLt1M = file.size / 1024 / 1024 < 1;
      // if (isLt1M) {
      //   return true;
      // }
      // this.$message({
      //   message: "Please do not upload files larger than 1m in size.",
      //   type: "warning"
      // });
      // return false;
    },
    getIsImported() {
      var param = {};
      this.fullscreenLoading = true;
      request({
        url: "warehouse/api/InventoryReport/IsImported",
        method: "post",
        params: param
      }).then(res => {
        this.fullscreenLoading = false;
        if (res.code === 0) {
          this.uploadState = res.data;
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    handleSuccess({ results, header }) {
      //导入数据
      var data = results;
      if (data.length > 10000) {
        this.$message({
          message: "不能大于10000条",
          type: "warning"
        });
        return;
      }
      this.$confirm(
        "是否带入数据(请录入所有数据，该操作只能操作一次)？",
        "提示",
        {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        }
      )
        .then(() => {
          this.ImportPrime(data);
        })
        .catch(() => {});
      return;
    },
    mergeArrRepeat2(arr, id, param1) {
      var newArr = [];
      var repeatArr = [];
      arr.forEach(item => {
        var dataItem = item;
        if (newArr.length > 0) {
          var filterValue = newArr.filter(v => {
            if (v[id] == dataItem[id] && v[param1] == dataItem[param1]) {
              return v;
            }
          });
          if (filterValue.length > 0) {
            newArr.forEach(n => {
              if (
                n[id] == filterValue[0][id] &&
                n[param1] == filterValue[0][param1]
              ) {
                repeatArr.push(n[id]);
              }
            });
          } else {
            newArr.push(dataItem);
          }
        } else {
          newArr.push(dataItem);
        }
      });
      return repeatArr;
    },
    ImportPrime(data) {
      // console.log(data);
      var postDataList = [];
      data.map(item => {
        postDataList.push({
          materialCode: item["物料代码（*）"],
          warehouseName: item["仓库名称（*）"],
          unitName: item["库存单位名称（*） "],
          num: item["期初数量（*）"]
        });
      });
      //判断物料代码不能为空
      let materialCodeArr = []; //重复数据
      let warehouseNameArr = []; //重复数据
      let unitNameArr = []; //重复数据
      let numArr = []; //重复数据
      let numIsNum= []; //重复数据
      postDataList.map(item => {
        if (isEmpty(item.materialCode)) {
          materialCodeArr.push(1);
        }
        if (isEmpty(item.warehouseName)) {
          warehouseNameArr.push(item.materialCode);
        }
        if (isEmpty(item.unitName)) {
          unitNameArr.push(item.materialCode);
        }
        if (isEmpty(item.num)) {
          numArr.push(item.materialCode);
        }
        // console.log(item.num+' '+islegalNum(item.num))
        if(!islegalNum(item.num)){
          numIsNum.push(item.materialCode);
        }
      });
      if (materialCodeArr.length > 0) {
        this.$confirm(`物料代码不能为空`, "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {})
          .catch(() => {});
        return;
      }
      if (warehouseNameArr.length > 0) {
        this.$confirm(
          `仓库名称不能为空(${warehouseNameArr.join(",")})`,
          "提示",
          {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning"
          }
        )
          .then(() => {})
          .catch(() => {});
        return;
      }
      if (unitNameArr.length > 0) {
        this.$confirm(
          `库存单位名称不能为空(${unitNameArr.join(",")})`,
          "提示",
          {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning"
          }
        )
          .then(() => {})
          .catch(() => {});
        return;
      }
      if (numArr.length > 0) {
        this.$confirm(`期初数量不能为空(${numArr.join(",")})`, "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {})
          .catch(() => {});
        return;
      }
      if (numIsNum.length > 0) {
        this.$confirm(`期初数量不合法(数字且最多保留四位小数)(${numIsNum.join(",")})`, "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {})
          .catch(() => {});
        return;
      }

      //判断物料代码仓库不能重复
      var rep = this.mergeArrRepeat2(postDataList, "materialCode", "warehouseName");
      if (rep.length > 0) {
        this.$confirm(`相同物料代码不能入库相同物料仓库(${rep.join(",")})`, "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {})
          .catch(() => {});
        return;
      }

      //判断物料代码仓库不能重复
      var rep = this.mergeArrRepeat2(
        postDataList,
        "materialCode",
        "warehouseName"
      );
      if (rep.length > 0) {
        this.$confirm(
          `相同物料代码不能入库相同物料仓库(${rep.join(",")})`,
          "提示",
          {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning"
          }
        )
          .then(() => {})
          .catch(() => {});
        return;
      }
      
      var reqObject = { postDataList: postDataList };
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/InventoryReport/ImportPrime",
        method: "post",
        data: reqObject
      }).then(res => {
        this.fullscreenLoading = false;
        if (res.code === 0) {
          this.$message({
            message: "操作成功",
            type: "success"
          });
          this.getIsImported();
          this.getMainList(); // 获取table数据
        } else {
          var msg = {};
          if (res.info === "") {
            msg = res.data;
          } else {
            msg = res.info;
          }
          this.$confirm(msg, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    exportExcel() {
      //导出Excel
      if (
        this.dtData.materialCode === "" &&
        this.dtData.materialName === "" &&
        this.dtData.warehouseId === -1
      ) {
        this.exportCurrent();
      } else {
        this.exportAll();
      }
    },
    getSummaries(param) {
      const { columns, data } = param;
      const sums = [];
      columns.forEach((column, index) => {
        if (index === 0) {
          sums[index] = "合计";
          return;
        }
        const values = data.map(item => Number(item[column.property]));
        if (!values.every(value => isNaN(value))) {
          sums[index] = values.reduce((prev, curr) => {
            const value = Number(curr);
            if (!isNaN(value)) {
              return BigNumber(prev).plus(curr);
            } else {
              return prev;
            }
          }, 0);

          if (index == 8 || index == 10) {
            sums[index] += "";
          } else {
            sums[index] = "";
          }
        } else {
          sums[index] = "";
        }
      });

      return sums;
    }
  }
};
</script>
<style lang="scss" scoped>
#OtherPut /deep/ {
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
