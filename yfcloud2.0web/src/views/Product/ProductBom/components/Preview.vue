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
          <el-header id="elheader" class="header" height="auto">
            <el-form
              ref="dtData"
              label-width="106px"
              class="FormInput pr"
              inline
              label-position="left"
            >
              <div
                class="bomTitle"
                style="height: 30px;font-size: 20px;font-weight: 600;"
              >{{bomData.title}}</div>
              <el-form-item label="出格编号：">
                <div style="width:200px;">{{bomData.pagerCode}}</div>
              </el-form-item>
              <el-form-item label="出格师傅：">
                <div style="width:200px;">{{bomData.realName}}</div>
              </el-form-item>
              <div
                class="imgMain"
                style="width: 216px;float: right; position: absolute;right: 0px;top: 0px;"
              >
                <el-form-item>
                  <div
                    class="imgVBox"
                    style="width: 60px;height: 60px;border: 1px dashed #d9d9d9;margin-bottom: 0px;"
                  >
                    <img
                      v-if="imageUrl1"
                      :src="imgUrlName+imageUrl1"
                      class="avatar"
                      style="width:58px;height:58px;"
                    />
                    <i
                      v-else
                      class="el-icon-plus avatar-uploader-icon"
                      style="position: relative;font-size: 28px;color: #000000;width: 60px;height: 60px;line-height: 60px;text-align: center;"
                    >
                      <font
                        style="position: absolute;font-size: 22px;left: 0px;width: 100%;text-align: center;opacity: 0.3;word-wrap: break-word;top:-12px"
                      >正</font>
                      <font
                        style="position: absolute;font-size: 22px;left: 0px;width: 100%;text-align: center;opacity: 0.3;word-wrap: break-word;top:12px"
                      >面</font>
                    </i>
                  </div>
                </el-form-item>
                <el-form-item>
                  <div
                    class="imgVBox"
                    style="width: 60px;height: 60px;border: 1px dashed #d9d9d9;margin-bottom: 0px;"
                  >
                    <img
                      v-if="imageUrl2"
                      :src="imgUrlName+imageUrl2"
                      class="avatar"
                      style="width:58px;height:58px;"
                    />
                    <i
                      v-else
                      class="el-icon-plus avatar-uploader-icon"
                      style="position: relative;font-size: 28px;color: #000000;width: 60px;height: 60px;line-height: 60px;text-align: center;"
                    >
                      <font
                        style="position: absolute;font-size: 22px;left: 0px;width: 100%;text-align: center;opacity: 0.3;word-wrap: break-word;top:-12px"
                      >侧</font>
                      <font
                        style="position: absolute;font-size: 22px;left: 0px;width: 100%;text-align: center;opacity: 0.3;word-wrap: break-word;top:12px"
                      >面</font>
                    </i>
                  </div>
                </el-form-item>
                <el-form-item style="margin-right:0px;">
                  <div
                    class="imgVBox"
                    style="width: 60px;height: 60px;border: 1px dashed #d9d9d9;margin-bottom: 0px;"
                  >
                    <img
                      v-if="imageUrl3"
                      :src="imgUrlName+imageUrl3"
                      class="avatar"
                      style="width:58px;height:58px;"
                    />
                    <i
                      v-else
                      class="el-icon-plus avatar-uploader-icon"
                      style="position: relative;font-size: 28px;color: #000000;width: 60px;height: 60px;line-height: 60px;text-align: center;"
                    >
                      <font
                        style="position: absolute;font-size: 22px;left: 0px;width: 100%;text-align: center;opacity: 0.3;word-wrap: break-word;top:-12px"
                      >背</font>
                      <font
                        style="position: absolute;font-size: 22px;left: 0px;width: 100%;text-align: center;opacity: 0.3;word-wrap: break-word;top:12px"
                      >面</font>
                    </i>
                  </div>
                </el-form-item>
              </div>
            </el-form>
          </el-header>
          <div :style="{ height: printBoxHeight , overflow:'auto' }" class="nosel">
            <el-table
              id="sumTable"
              :data="tableData"
              :span-method="objectSpanMethod"
              :row-class-name="tableRowClassName"
              :height="printBoxHeight"
              border
              style="width:99.99%; border: 1px solid #dfe6ec;"
              v-if="tableFlag"
            >
              <el-table-column label="序号" width="60">
                <template slot-scope="scope">
                  <div>{{scope.$index+1}}</div>
                </template>
              </el-table-column>
              <el-table-column prop="itemName" label="配色项目" width="150">
                <template slot-scope="scope">
                  <div>{{scope.row.itemName}}</div>
                </template>
              </el-table-column>
              <!-- <el-table-column prop="materialName" label="物料名称" width="150" /> -->
              <el-table-column prop="materialName" label="物料名称" width="150">
                <template slot-scope="scope">
                  <div
                    :class="{'redText':scope.row.materialState==-1,'sumSty':scope.row.itemState==-1}"
                  >{{scope.row.materialName}}</div>
                </template>
              </el-table-column>
              <el-table-column prop="partName" label="部位名称" width="150" />
              <el-table-column prop="lengthValue" label="长" />
              <el-table-column prop="widthValue" label="宽" />
              <el-table-column prop="numValue" label="件数" />
              <el-table-column prop="wideValue" label="封度（宽幅）" width="100" />
              <el-table-column prop="lossValue" label="损耗" />
              <!-- <el-table-column prop="singleValue" label="单用量" width="150" /> -->
              <el-table-column prop="singleValue" label="单用量" width="150">
                <template slot-scope="scope">
                  <div
                    :class="{'redText':scope.row.materialState==-1,'sumSty':scope.row.itemState==-1}"
                  >{{scope.row.singleValue}}</div>
                </template>
              </el-table-column>
            </el-table>
          </div>
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
import {
  formatDate,
  isEmpty,
  isRealNum,
  getMutipSort,
  getSort
} from "@/utils/common.js";
import export2Excel from "@/utils/Export2Excel.js";
import { setTimeout } from "timers";
import BigNumber from "bignumber.js";
// import print from "@/utils/print.js";

export default {
  data() {
    return {
      tableFlag: true,
      printBoxHeight: "480",
      imageUrl1: "",
      imageUrl2: "",
      imageUrl3: "",
      imgFlag1: false,
      imgFlag2: false,
      imgFlag3: false,
      dialogVisible: false,
      fullscreenLoading: false,
      tableData: [],
      bomData: {},
      mergeArr: [],
      dialogWidth: "1200px",
      printHg: "auto",
      printState: -1
    };
  },
  props: {
    title: {
      default: "详情"
    }
  },
  components: {},
  mounted() {},
  methods: {
    isUndefined(value) {
      if (isEmpty(value)) {
        return "";
      } else {
        return value;
      }
    },
    exportTable() {
      var hg = 60; //图片宽高
      var wh = 60; //图片宽高
      var caption = `
      <tr>
        <td colspan="9" style="text-align:center;font-size:18px;">${
          this.bomData.title
        }</td>
      </tr>
      <tr>
        <td colspan="9">出格编号：${this.bomData.pagerCode}</td>
      </tr>
      <tr>
        <td colspan="9">出格师傅：${this.bomData.pagerCode}</td>
      </tr>
      <tr>
        <td>正面：</td>
        <td style="min-width:80px;height:${hg}px; text-align: center; vertical-align: middle"><img  width="${wh}" height="${hg}" src="${this
        .imgUrlName + this.imageUrl1}" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td>侧面：</td>
        <td style=min-width:80px;"height:${hg}px; text-align: center; vertical-align: middle"><img  width="${wh}" height="${hg}" src="${this
        .imgUrlName + this.imageUrl2}" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
        <td>背面：</td>
        <td style="min-width:80px;height:${hg}px; text-align: center; vertical-align: middle"><img  width="${wh}" height="${hg}" src="${this
        .imgUrlName + this.imageUrl3}" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
      </tr>
      `;
      var thead = `<thead>
      <tr>
        <th>配色项目</th>
        <th>物料名称</th>
        <th>部位名称</th>
        <th>长</th>
        <th>宽</th>
        <th>件数</th>
        <th>封度（宽幅）</th>
        <th>损耗</th>
        <th>单用量</th>
      </tr>
      </thead>`;
      var tbody = "";
      this.tableData.map((item, index) => {
        tbody += `<tr>`;

        for (var i = 0; i < this.mergeArr.length; i++) {
          if (item.itemId == this.mergeArr[i]["itemId"]) {
            if (index == this.mergeArr[i]["rowIndex"]) {
              // console.log(item.len)
              tbody += `<td rowspan="${item.len}">${this.isUndefined(
                item.itemName
              )}${item.len}</td>`;
            } else {
              // console.log(0)
            }
          }
        }

        //物料名称（添加有颜色）
        var materialName = this.isUndefined(item.materialName);
        if (item.total == -1) {
          tbody += `<td  style="color:#67c23a;font-weight: 800;">${materialName}</td>`;
        } else if (item.state == -1) {
          tbody += `<td  style="color:red;font-weight: 800;font-size:16px;">${materialName}</td>`;
        } else {
          tbody += `<td>${materialName}</td>`;
        }

        tbody += `<td>${this.isUndefined(item.dicValue)}</td>
        <td>${this.isUndefined(item.lengthValue)}</td>
        <td>${this.isUndefined(item.widthValue)}</td>
        <td>${this.isUndefined(item.numValue)}</th>
        <td>${this.isUndefined(item.wideValue)}</td>
        <td>${this.isUndefined(item.lossValue)}</td>`;
        //物料名称（添加有颜色）
        var singleValue = this.isUndefined(item.singleValue);
        if (item.total == -1) {
          tbody += `<td  style="color:#67c23a;font-weight: 800;">${singleValue}</td>`;
        } else if (item.state == -1) {
          tbody += `<td  style="color:red;font-weight: 800;font-size:16px;">${singleValue}</td>`;
        } else {
          tbody += `<td>${singleValue}</td>`;
        }

        tbody += `</tr>`;
      });
      var table = `${caption}${thead}<tbody>${tbody}</tbody>`;
      export2Excel(table, "BOM");
    },
    doPrint() {
      this.$print(this.$refs.print, {
        zoom: 0.6
      });
    },
    setLink(URL) {
      var link = document.createElement("link");
      link.setAttribute("type", "text/css");
      link.setAttribute("rel", "stylesheet");
      link.setAttribute("href", URL);
      return link;
    },
    tableRowClassName({ row, rowIndex }) {
      // if (row.state === -1) {
      //   return "color1";
      // }
      return "";
    },
    objectSpanMethod({ row, column, rowIndex, columnIndex }) {
      // if (columnIndex === 1) {
      //   let ind = row.len;
      //   for (var i = 0; i < this.mergeArr.length; i++) {
      //     if (row.itemId == this.mergeArr[i]["itemId"]) {
      //       if (rowIndex == this.mergeArr[i]["rowIndex"]) {
      //         return {
      //           rowspan: ind, //合并行数
      //           colspan: 1
      //         };
      //       } else {
      //         return {
      //           rowspan: 0,
      //           colspan: 0
      //         };
      //       }
      //     }
      //   }
      // }
      if (columnIndex === 1) {
        return {
          rowspan: row.itemNameMix, //合并行数
          colspan: 1
        };
      }
      if (columnIndex === 2) {
        return {
          rowspan: row.materialMix, //合并行数
          colspan: 1
        };
      }
    },
    open(state) {
      this.tableFlag = false;
      this.printState = state;
      if (state == 2) {
        this.dialogWidth = "1220px";
        this.printBoxHeight = "auto";
        this.printHg = "600px";
      } else {
        this.dialogWidth = "1200px";
        this.printBoxHeight = "480";
        this.printHg = "auto";
      }
      this.$nextTick(() => {
        this.tableFlag = true;
        this.dialogVisible = true;
      });
    },
    close() {
      this.dialogVisible = false;
    },
    setData(dt) {
      this.bomData = dt;
      this.imageUrl1 = dt.frontImgPath;
      this.imageUrl2 = dt.sideImgPath;
      this.imageUrl3 = dt.backImgPath;
    },
    setTableData(data) {
      //排序
      var itemNameSort = getSort(function(a, b) {
        return a.itemName < b.itemName;
      });
      var materialNameSort = getSort(function(a, b) {
        return a.materialName < b.materialName;
      });
      var sortArr = [itemNameSort, materialNameSort];
      data.sort(getMutipSort(sortArr));

      //合并
      var itemName = "";
      var materialName = "";
      var itemName2 = "";
      // data.map((item, key) => {
      for (var key = 0; key < data.length; key++) {
        if (data[key].itemState == -1) {
          continue;
        }
        if (data[key].materialState == -1) {
          continue;
        }
        if (itemName == "") {
          //计算你相同的第一个合并个数
          itemName = data[key].itemName;
          var sum_ = 0;
          var itemSum = 0;
          function getSum(data, key, itemName, sum) {
            if (data[key] && data[key]["itemName"] == itemName) {
              sum = sum + 1;
              itemSum = BigNumber(itemSum)
                .plus(data[key]["singleValue"])
                .toNumber();
              return getSum(data, key + 1, itemName, sum);
            } else {
              return {
                sum: sum + 1,
                itemSum: itemSum
              };
            }
          }
          var dt = getSum(data, key, itemName, sum_, itemSum);
          data[key].itemNameMix = dt.sum;
          //如果只有一条重新进行
          if (data[key].itemNameMix == 2) {
            itemName = "";
            data.splice(parseInt(key) + 1, 0, {
              materialName: "合计",
              itemState: -1,
              itemNameMix: 0,
              materialMix: 1,
              singleValue: dt.itemSum
            });
          }
        } else {
          //其他相同的合并个数为0
          data[key].itemNameMix = 0;
          if (data[key + 1] && data[key + 1]["itemName"] != itemName) {
            itemName = "";
            data.splice(parseInt(key) + 1, 0, {
              materialName: "合计",
              itemState: -1,
              itemNameMix: 0,
              materialMix: 1,
              singleValue: dt.itemSum
            });
          }
          if (!data[key + 1]) {
            data.splice(parseInt(key) + 1, 0, {
              materialName: "合计",
              itemState: -1,
              itemNameMix: 0,
              materialMix: 1,
              singleValue: dt.itemSum
            });
          }
        }
      }

      for (var key = 0; key < data.length; key++) {
        if (data[key].itemState == -1) {
          continue;
        }
        if (data[key].materialState == -1) {
          continue;
        }
        if (materialName == "" && itemName2 == "") {
          //计算你相同的第一个合并个数
          materialName = data[key].materialName;
          itemName2 = data[key].itemName;
          var sum_ = 0;
          var materialSum = 0;
          function getSum(data, key, materialName, sum, materialSum) {
            if (
              data[key] &&
              data[key]["materialName"] == materialName &&
              data[key]["itemName"] == itemName2
            ) {
              sum = sum + 1;
              materialSum = BigNumber(materialSum)
                .plus(data[key]["singleValue"])
                .toNumber();
              return getSum(data, key + 1, materialName, sum, materialSum);
            } else {
              return {
                sum,
                materialSum
              };
            }
          }
          var dt = getSum(data, key, materialName, sum_, materialSum);
          data[key].materialMix = dt.sum;
          var addNum = key + data[key].materialMix;
          // console.log(addNum);
          data.splice(parseInt(addNum), 0, {
            materialName: "合计2",
            materialState: -1,
            itemNameMix: 0,
            materialMix: 1,
            singleValue: dt.materialSum
          });
          // console.log(key);
          // console.log(data[key]["itemNameMix"]);
          function findItemName(data, key) {
            if (data[key]["itemNameMix"] == 0) {
              return findItemName(data, key - 1);
            } else {
              return key;
            }
          }
          var itemNameFirst = findItemName(data, key);
          data[itemNameFirst]["itemNameMix"] =
            data[itemNameFirst]["itemNameMix"] + 1;

          //如果只有一条重新进行
          if (data[key].materialMix == 1) {
            materialName = "";
            itemName2 = "";
          }
        } else {
          //其他相同的合并个数为0
          data[key].materialMix = 0;
          if (data[key + 1]) {
            if (
              (data[key + 1] &&
                data[key + 1]["materialName"] != materialName) ||
              data[key + 1]["itemName"] != itemName2
            ) {
              materialName = "";
              itemName2 = "";
            }
          }
        }
      }

      // console.log(data);
      this.tableData = data;
      this.$emit("closeLoading");
    },
  }
};
</script>

<style lang="scss" scoped>
#Preview /deep/ {
  .el-table--striped .el-table__body tr.el-table__row--striped.current-row td,
  .el-table__body tr.current-row > td,
  .el-table__body tr.hover-row.current-row > td,
  .el-table__body tr.hover-row.el-table__row--striped.current-row > td,
  .el-table__body tr.hover-row.el-table__row--striped > td,
  .el-table__body tr.hover-row > td {
    background-color: transparent;
  }
  .el-dialog--center .el-dialog__body {
    padding: 20px 20px 0px;
  }
  .el-header {
    padding: 0px;
  }
  .pr {
    position: relative;
  }
  .el-table--small td {
    padding: 8px !important;
  }
  .bomTitle {
    height: 30px;
    font-size: 20px;
    font-weight: 600;
  }
  .imgMain {
    width: 208px;
    float: right;
    position: absolute;
    right: 0px;
    top: 0px;
  }
  .imgVBox {
    width: 60px;
    height: 60px;
    border: 1px dashed #d9d9d9;
    margin-bottom: 0px;
  }
  .imgVBox img {
    width: 58px;
    height: 58px;
  }
  .el-table .color1 {
    background: #f3d4d4;
    color: #606266;
  }
  .redText {
    color: #67c23a !important;
    font-weight: 600;
  }
  .sumSty {
    color: red !important;
    font-weight: 800;
    font-size: 16px;
  }
  .el-table--group::after,
  .el-table--border::after {
    width: 0px;
  }
  img {
    width: 58px;
    height: 58px;
  }
}
.el-table::before {
  height: 0px;
}
.el-table--group,
.el-table--border {
  border: 0px solid #dfe6ec;
}
.redText {
  color: #67c23a !important;
  font-weight: 600;
}
.sumSty {
  color: red !important;
  font-weight: 800;
  font-size: 16px;
}
// .el-table td div{
//     transform: scale(0.2);
//     transform-origin: 0;
// }
</style>

