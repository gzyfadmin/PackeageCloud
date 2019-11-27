<template>
  <div id="Details">
    <el-table
      id="tableData2"
      :data="dtData"
      ref="table2"
      :height="tableHeight3"
      style="width: 99.9%"
      row-key="id"
      border
      :show-summary="true"
      :summary-method="getSummaries"
      :span-method="objectSpanMethod"
      v-show="tableFlag"
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
</template>

<script>
import { newGuid } from "@/utils/guid";
import BigNumber from "bignumber.js";
import { unique, isEmpty } from "@/utils/common.js";
// import { resolve } from "dns";
// import { rejects } from "assert";

export default {
  data() {
    return {
      dtData: [],
      tableFlag: true
    };
  },
  props: {
    tableHeight3: {}
  },

  components: {},
  mounted() {
    // console.log(this.tableHeight3)
  },
  methods: {
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
    setDetailData(data) {
      return new Promise((resolve, reject) => {
        var packageIdArr = [];
        var materialIdArr = [];
        var colorNameArr = [];
        var materialId = "";
        var colorId = "";
        var materialId2 = "";
        data.map((item, key) => {
          item.id = newGuid();
          packageIdArr.push(item.packageId);
          if (materialId == "") {
            //计算你相同的第一个合并个数
            materialId = item.materialId;
            var sum_ = 0;
            function getSum(data, key, materialId, sum) {
              if (data[key] && data[key]["materialId"] == materialId) {
                sum = sum + 1;
                return getSum(data, key + 1, materialId, sum);
              } else {
                return sum;
              }
            }

            item.materialMix = getSum(data, key, materialId, sum_);
            //如果只有一条重新进行
            if (item.materialMix == 1) {
              materialId = "";
            }
          } else {
            //其他相同的合并个数为0
            item.materialMix = 0;
            if (data[key + 1] && data[key + 1]["materialId"] != materialId) {
              materialId = "";
            }
          }

          if (colorId == "" && materialId2 == "") {
            //计算你相同的第一个合并个数
            colorId = item.colorId;
            materialId2 = item.materialId;
            var sum_ = 0;
            function getSum(data, key, colorId, sum) {
              if (
                data[key] &&
                data[key]["colorId"] == colorId &&
                data[key]["materialId"] == materialId2
              ) {
                sum = sum + 1;
                return getSum(data, key + 1, colorId, sum);
              } else {
                return sum;
              }
            }

            item.colorMix = getSum(data, key, colorId, sum_);
            //如果只有一条重新进行
            if (item.colorMix == 1) {
              colorId = "";
              materialId2 = "";
            }
          } else {
            //其他相同的合并个数为0
            item.colorMix = 0;
            if (data[key + 1]) {
              if (
                (data[key + 1] && data[key + 1]["colorId"] != colorId) ||
                data[key + 1]["materialId"] != materialId2
              ) {
                colorId = "";
                materialId2 = "";
              }
            }
          }
        });

        // console.log(data);
        packageIdArr = unique(packageIdArr);
        // materialIdArr = unique(materialIdArr);
        // colorNameArr = unique(colorNameArr);
        packageIdArr.forEach(val => {
          //包型
          var filterValue = data.filter(v => {
            if (v["packageId"] == val) {
              return v;
            }
          });
          if (filterValue.length > 0) {
            //包型
            filterValue.forEach(k => {
              k["packageMix"] = 0;
            });
            filterValue[0]["packageMix"] = filterValue.length;
          }
        });

        this.tableFlag = false;
        this.dtData = data;
        setTimeout(() => {
          this.tableFlag = true;
        }, 100);
        resolve(data);
      });
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
    }
  }
};
</script>

<style lang="scss" scoped>
#Details /deep/ {
  .el-table {
    overflow: visible !important;
  }
}
</style>

