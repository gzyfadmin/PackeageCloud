<template>
  <div id="Print">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      :width="dialogWidth"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-background="rgba(150, 150, 150, 0.6)"
        element-loading-spinner="el-icon-loading"
        :style="{ height: printHg, overflow:'auto' }"
      >
        <div id="printMe" ref="print">
          <el-table
            border
            style="width:100%"
            :data="headerTable"
            :show-header="false"
            :span-method="objectSpanMethod2"
          >
            <el-table-column prop="data1" width="140">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data1 }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data1Val">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data1Val }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data2" width="100">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data2 }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data2Val">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data2Val }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data3" width="100">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data3 }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data3Val">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data3Val }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data4" width="140">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data4 }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="data4Val" width="160">
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.data4Val }}</div>
              </template>
            </el-table-column>
          </el-table>
          <el-table
            id="tableData2"
            :data="tableData2"
            ref="table2"
            style="width:100%"
            row-key="id"
            :span-method="objectSpanMethod"
            :show-summary="true"
            :summary-method="getSummaries"
            border
          >
            <el-table-column label="序号" width="70">
              <template slot="header">
                <span class="tableHeader">
                  <span>序号</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="textAlign">
                  <span>{{ scope.$index+1 }}</span>
                </div>
              </template>
            </el-table-column>

            <el-table-column prop="materialCode" label="包型" width="200">
              <template slot="header">
                <span class="tableHeader">
                  <span>包型</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.packageName }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="materialName" label="物料" width="200">
              <template slot="header">
                <span class="tableHeader">
                  <span>物料</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.materialName }}</div>
              </template>
            </el-table-column>
            <!-- <el-table-column prop="materialName" label="物料" width="200">
        <template slot="header">
          <span class="tableHeader">
            <span>物料</span>
          </span>
        </template>
        <template slot-scope="scope">
          <div class="tableTd">{{ scope.row.materialName }}</div>
        </template>
            </el-table-column>-->

            <el-table-column prop="colorSolutionCode" label="颜色对应序号" width="200">
              <template slot="header">
                <span class="tableHeader">
                  <span>颜色对应序号</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div>
                  <div class="tableTd">{{ scope.row.colorSolutionCode }}</div>
                </div>
              </template>
            </el-table-column>

            <!-- <el-table-column prop="colorName" label="颜色" width="100">
        <template slot="header">
          <span class="tableHeader">
            <span>颜色</span>
          </span>
        </template>
        <template slot-scope="scope">
          <div class="tableTd">{{ scope.row.colorName }}</div>
        </template>
            </el-table-column>-->
            <el-table-column prop="colorName" label="颜色" width="100">
              <template slot="header">
                <span class="tableHeader">
                  <span>颜色</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.colorName }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="productionNum" label="单色数量">
              <template slot="header">
                <span class="tableHeader">
                  <span>单色数量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.productionNum }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="singleValue" label="单用量">
              <template slot="header">
                <span class="tableHeader">
                  <span>单用量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.singleValue }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="produceUnitName" label="单位">
              <template slot="header">
                <span class="tableHeader">
                  <span>单位</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div class="tableTd">{{ scope.row.produceUnitName}}</div>
              </template>
            </el-table-column>
          </el-table>
        </div>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="doPrint" v-if="printState==2">打印</el-button>
        <el-button type="primary" @click="exportTable" v-if="printState==3">导出</el-button>
        <el-button @click="dialogVisible=false">关闭</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, isEmpty, isRealNum } from "@/utils/common.js";
import export2Excel from "@/utils/Export2Excel.js";
import BigNumber from "bignumber.js";
// import print from "@/utils/print.js";

export default {
  data() {
    return {
      dialogVisible: false,
      fullscreenLoading: false,
      dialogWidth: "1320px",
      printHg: "600px",
      dtDataRules: {},
      tableFlag: true,
      printState: -1,
      headerTable: [],
      tableData2: []
    };
  },
  props: {
    title: {
      default: "详情"
    },
    headerDataObj: {},
    tableData: {
      default: []
    },
    dtData: {
      default: []
    }
  },
  filters: {
    formatTDate: value => {
      if (value == "") {
        return "";
      }
      return formatDate(value);
    },
    dataFormat(msg) {
      // return msg + "xxxxx";
      var text = "";
      if (msg) {
        for (var i = 0; i < msg.length; i++) {
          if (i != msg.length - 1) {
            text += msg[i].no + "，";
          } else {
            text += msg[i].no;
          }
        }
      }
      return text;
    }
  },
  components: {},
  mounted() {},
  methods: {
    setData(data) {
      this.tableData2 = [];
      if (data.length == 0) {
        for (var i = 0; i < 15; i++) {
          this.tableData2.push({
            packageName: "",
            materialName: "",
            colorSolutionCode: "",
            colorName: "",
            productionNum: "",
            singleValue: "",
            produceUnitName: "",
            packageMix: 1,
            materialMix: 1,
            colorMix: 1
          });
        }
      } else {
        this.tableData2 = data;
      }

      var auditStatusName = "";
      if (this.headerDataObj.auditStatus == 0) {
        auditStatusName = "待审核";
      }
      if (this.headerDataObj.auditStatus == 1) {
        auditStatusName = "未通过";
      }
      if (this.headerDataObj.auditStatus == 2) {
        auditStatusName = "通过";
      }
      if (this.headerDataObj.auditStatus == 3) {
        auditStatusName = "撤销审核";
      }

      this.headerTable = [];
      this.headerTable.push(
        {
          data1: "生产订单编号",
          data1Val: this.dtData.orderNo,
          data2: "订单日期",
          data2Val: this.$options.filters["formatTDate"](
            this.headerDataObj.orderDate
          ),
          data3: "订单单号",
          data3Val: this.headerDataObj.productionNo,
          data4: "审核状态",
          data4Val: auditStatusName
        },
        {
          data1: "操作员",
          data1Val: this.headerDataObj.operatorName,
          data2: "审核员",
          data2Val: this.headerDataObj.auditName,
          data3: "审核时间",
          data3Val: this.$options.filters["formatTDate"](
            this.headerDataObj.auditTime
          ),
          data4: "入库申请单号",
          data4Val: this.$options.filters["dataFormat"](this.dtData.whApplyMain)
        },
        {
          data1: "采购申请单号",
          data1Val: this.$options.filters["dataFormat"](this.dtData.purchase),
          data2: "领料单号",
          data2Val: this.$options.filters["dataFormat"](this.dtData.pickModel),
          pickModelcolspan: 5
        }
      );
    },
    objectSpanMethod2({ row, column, rowIndex, columnIndex }) {
      if (columnIndex === 3 && row.pickModelcolspan) {
        return {
          rowspan: 1, //合并行数
          colspan: row.pickModelcolspan
        };
      }
    },
    objectSpanMethod({ row, column, rowIndex, columnIndex }) {
      if (columnIndex === 1) {
        return {
          rowspan: row.packageMix, //合并行数
          colspan: 1
        };
      }
      if (columnIndex === 2) {
        return {
          rowspan: row.materialMix, //合并行数
          colspan: 1
        };
      }
      if (columnIndex === 4) {
        return {
          rowspan: row.colorMix, //合并行数
          colspan: 1
        };
      }
    },
    doPrint() {
      this.$print(this.$refs.print, {
        zoom: 0.6
      });
    },
    exportTable() {},
    getSummaries(param) {
      // table总计
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
              return BigNumber(prev)
                .plus(curr)
                .toNumber();
            } else {
              return prev;
            }
          }, 0);
          if (index == 5 || index == 6) {
            sums[index] = sums[index] + "";
            // sums[index] += ""
          } else {
            sums[index] = "";
          }
        } else {
          sums[index] = "";
        }
      });

      return sums;
    },
    open(state) {
      this.tableFlag = false;
      this.printState = state;
      if (state == 2) {
      } else {
      }
      this.$nextTick(() => {
        this.tableFlag = true;
        this.dialogVisible = true;
      });
    },
    close() {
      this.dialogVisible = false;
    }
  }
};
</script>

<style lang="scss" scoped>
/deep/ .el-table--border {
  border: 1px solid #dfe6ec !important;
}
.tableTd {
  font-size: 12px;
  transform: scale(0.7);
  transform-origin: 0;
  margin-right: -80px;
}
.tableHeader span {
  font-size: 12px;
  transform: scale(0.7);
  transform-origin: 0;
  margin-right: -80px;
  display: block;
}
/deep/ .el-table__footer-wrapper .cell {
  font-size: 12px;
  transform: scale(0.7);
  transform-origin: 0;
  display: block;
}
.textAlign span {
  font-size: 12px;
  transform: scale(0.7);
  transform-origin: 0;
  display: block;
}
</style>

