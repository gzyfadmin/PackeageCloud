<template>
  <div id="Preview">
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
          <table
            id="print-content1"
            border="1"
            cellpadding="0"
            cellspacing="0"
            style="border-collapse:collapse;width:100%;border:1px solid #dcdfe6;"
          >
            <thead>
              <tr v-for="(item,index) in headerTable" :key="index">
                <td>{{item.data1}}</td>
                <td>{{item.data1Val}}</td>
                <td>{{item.data2}}</td>
                <td :colspan="item.pickModelcolspan">{{item.data2Val}}</td>
                <td v-if="!item.pickModelcolspan">{{item.data3}}</td>
                <td v-if="!item.pickModelcolspan">{{item.data3Val}}</td>
                <td v-if="!item.pickModelcolspan">{{item.data4}}</td>
                <td v-if="!item.pickModelcolspan">{{item.data4Val}}</td>
              </tr>
              <tr>
                <td style="font-weight:bold;">序号</td>
                <td style="font-weight:bold;">包型</td>
                <td style="font-weight:bold;">物料</td>
                <td style="font-weight:bold;">颜色对应序号</td>
                <td style="font-weight:bold;">颜色</td>
                <td style="font-weight:bold;">单色数量</td>
                <td style="font-weight:bold;">单用量</td>
                <td style="font-weight:bold;">单位</td>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in tableData2" :key="item.id">
                <td>{{item.index}}</td>
                <td :rowspan="item.packageMix" v-if="item.packageMix!==0">{{item.packageName}}</td>
                <td :rowspan="item.materialMix" v-if="item.materialMix!==0">{{item.materialName}}</td>
                <td>{{item.colorSolutionCode}}</td>
                <td :rowspan="item.colorMix" v-if="item.colorMix!==0">{{item.colorName}}</td>
                <td>{{item.productionNum}}</td>
                <td>{{item.singleValue}}</td>
                <td>{{item.produceUnitName}}</td>
              </tr>
            </tbody>
          </table>
          <el-table
            id="tableData2"
            :data="tableData2"
            ref="table2"
            style="width: 99.9%;display:none;"
            row-key="id"
            :span-method="objectSpanMethod"
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
        <el-button type="primary" @click="exportTable">导出</el-button>
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
      dialogWidth: "1200px",
      printHg: "600px",
      dtDataRules: {},
      tableFlag: true,
      printState: -1,
      tableData2: [],
      headerTable: []
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
    setData(dt) {
      // console.log(dt)
      var data = JSON.parse(JSON.stringify(dt));
      // console.log(data)
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

      var productionNum = 0;
      var singleValue = 0;
      data.map((item, index) => {
        item.index = index + 1;
        productionNum = BigNumber(productionNum).plus(item.productionNum);
        singleValue = BigNumber(singleValue).plus(item.singleValue);
      });
      data.push({
        index: "合计",
        productionNum: productionNum,
        singleValue: singleValue
      });
      this.tableData2 = data;
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
      if (columnIndex === 5) {
        return {
          rowspan: row.colorMix, //合并行数
          colspan: 1
        };
      }
    },
    doPrint() {
      this.$print(this.$refs.print, {
        zoom: 1
      });
    },
    exportTable() {
      var table = `<table border="1"cellpadding="0"cellspacing="0">
          ${document.getElementById("print-content1").innerHTML}
          </table>`;
      // console.log(table)
      export2Excel(table, "BOM");
    },
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
          return Number(item[column.property].value);
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
          if (index == 5 || index == 6 || index == 7) {
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
</style>

