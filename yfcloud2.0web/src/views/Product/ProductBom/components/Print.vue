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
        <!-- <th style="width:105px">配色项目</th>
                <th style="width:95px">物料名称</th>
                <th style="width:95px">部位名称</th>
                <th style="width:60px">长</th>
                <th style="width:60px">宽</th>
                <th style="width:60px">件数</th>
                <th style="width:60px">封度</th>
                <th style="width:60px">损耗</th>
        <th style="width:70px">单用量</th>-->
        <div id="printMe" ref="print">
          <table
            frame="vsides"
            id="topTable"
            border="1"
            cellpadding="0"
            cellspacing="0"
            style="border-collapse:collapse;width:99.999%;border-top: 1px solid #000;"
            v-if="printState==2"
          >
            <tr>
              <td style="width:100px">
                <div class="span1_" style="width:105px">客户名称：</div>
              </td>
              <td colspan="2" style="width:185px">
                <div class="span2_"></div>
              </td>
              <td colspan="2" style="width:115px">
                <div class="span3_">纸格件数：</div>
              </td>
              <td colspan="2" style="width:115px">
                <div class="span3_">件</div>
              </td>
              <td colspan="2" style="width:125px">
                <div class="span3_">订单号：</div>
              </td>
              <td style="width:100px">
                <div class="span1_"></div>
              </td>
              <td colspan="2" style="width:185px">
                <div class="span2_">制单日期：</div>
              </td>
              <td colspan="2" style="width:115px">
                <div class="span3_"></div>
              </td>
              <td colspan="2" style="width:115px">
                <div class="span3_"></div>
              </td>
              <td colspan="2" style="width:125px">
                <div class="span3_"></div>
              </td>
            </tr>
            <tr>
              <td>
                <div class="span1_">包型名称：</div>
              </td>
              <td colspan="2">
                <div class="span2_">{{bomData.title}}</div>
              </td>
              <td colspan="2">
                <div class="span3_">出格师傅：</div>
              </td>
              <td colspan="2">
                <div class="span3_">{{bomData.realName}}</div>
              </td>
              <td colspan="2">
                <div class="span3_">数量</div>
              </td>
              <td>
                <div class="span1_"></div>
              </td>
              <td colspan="2">
                <div class="span2_">出货日期：</div>
              </td>
              <td colspan="2">
                <div class="span3_"></div>
              </td>
              <td colspan="2">
                <div class="span3_">制单人</div>
              </td>
              <td colspan="2" style="width:130px">
                <div class="span3_"></div>
              </td>
            </tr>
          </table>
          <table
            id="print-content1"
            border="1"
            cellpadding="0"
            cellspacing="0"
            style="border-collapse:collapse;width:50%;float:left;"
          >
            <thead>
              <tr v-if="printState==3">
                <td>
                  <div class="span1_" style="width:105px">客户名称：</div>
                </td>
                <td colspan="2">
                  <div class="span2_"></div>
                </td>
                <td colspan="2">
                  <div class="span3_">纸格件数：</div>
                </td>
                <td colspan="2">
                  <div class="span3_">件</div>
                </td>
                <td colspan="2" style="width:130px">
                  <div class="span3_">订单号：</div>
                </td>
              </tr>
              <tr v-if="printState==3">
                <td>
                  <div class="span1_">包型名称：</div>
                </td>
                <td colspan="2">
                  <div class="span2_">{{bomData.title}}</div>
                </td>
                <td colspan="2">
                  <div class="span3_">出格师傅：</div>
                </td>
                <td colspan="2">
                  <div class="span3_">{{bomData.realName}}</div>
                </td>
                <td colspan="2">
                  <div class="span3_">数量</div>
                </td>
              </tr>
              <tr>
                <th style="width:105px">配色项目</th>
                <th style="width:95px">物料名称</th>
                <th style="width:95px">部位名称</th>
                <th style="width:60px">长</th>
                <th style="width:60px">宽</th>
                <th style="width:60px">件数</th>
                <th style="width:60px">封度</th>
                <th style="width:60px">损耗</th>
                <th style="width:70px">单用量</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in tableData" :key="item.id">
                <td :rowspan="item.itemNameMix" v-if="item.itemNameMix!=0">
                  <div class="span1">{{item.itemName}}</div>
                </td>
                <!-- <td :rowspan="item.materialMix" v-if="item.materialMix!=0">
                  <div
                    class="span1"
                    :class="{'redText':item.materialState==-1,'sumSty':item.itemState==-1}"
                  >{{item.materialName}}</div>
                </td>-->
                <td
                  :rowspan="item.materialMix"
                  v-if="item.materialMix!=0&&item.materialState==-1"
                  style="color:#67c23a;font-weight: 800;"
                >
                  <div class="span1">{{item.materialName}}</div>
                </td>
                <td
                  :rowspan="item.materialMix"
                  v-else-if="item.materialMix!=0&&item.itemState==-1"
                  style="color:red;font-weight: 800;"
                >
                  <div class="span1">{{item.materialName}}</div>
                </td>
                <td :rowspan="item.materialMix" v-else-if="item.materialMix!=0">
                  <div class="span1">{{item.materialName}}</div>
                </td>

                <td>
                  <div class="span1">{{item.partName}}</div>
                </td>
                <td>
                  <div class="span9">{{item.lengthValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.widthValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.numValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.wideValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.lossValue}}</div>
                </td>
                <!-- <td>
                  <div
                    class="span9"
                    :class="{'redText':item.materialState==-1,'sumSty':item.itemState==-1}"
                  >{{item.singleValue}}</div>
                </td>-->
                <td v-if="item.materialState==-1" style="color:#67c23a;font-weight: 800;">
                  <div class="span1">{{item.singleValue}}</div>
                </td>
                <td v-else-if="item.itemState==-1" style="color:red;font-weight: 800;">
                  <div class="span1">{{item.singleValue}}</div>
                </td>
                <td v-else>
                  <div class="span9">{{item.singleValue}}</div>
                </td>
              </tr>
            </tbody>
          </table>
          <table
            id="print-content2"
            border="1"
            cellpadding="0"
            cellspacing="0"
            style="border-collapse:collapse;width:50%;float:left;"
          >
            <thead>
              <tr v-if="printState==3">
                <td style="width:105px">
                  <div class="span1_"></div>
                </td>
                <td colspan="2">
                  <div class="span2_">制单日期：</div>
                </td>
                <td colspan="2">
                  <div class="span3_"></div>
                </td>
                <td colspan="2">
                  <div class="span3_"></div>
                </td>
                <td colspan="2">
                  <div class="span3_"></div>
                </td>
              </tr>
              <tr v-if="printState==3">
                <td>
                  <div class="span1_"></div>
                </td>
                <td colspan="2">
                  <div class="span2_">出货日期：</div>
                </td>
                <td colspan="2">
                  <div class="span3_"></div>
                </td>
                <td colspan="2">
                  <div class="span3_">制单人</div>
                </td>
                <td colspan="2" style="width:130px">
                  <div class="span3_"></div>
                </td>
              </tr>
              <tr>
                <th style="width:105px">配色项目</th>
                <th style="width:95px">物料名称</th>
                <th style="width:95px">部位名称</th>
                <th style="width:60px">长</th>
                <th style="width:60px">宽</th>
                <th style="width:60px">件数</th>
                <th style="width:60px">封度</th>
                <th style="width:60px">损耗</th>
                <th style="width:70px">单用量</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in tableData2" :key="item.id">
                <td :rowspan="item.itemNameMix" v-if="item.itemNameMix!=0">
                  <div class="span1">{{item.itemName}}</div>
                </td>
                <!-- <td :rowspan="item.materialMix" v-if="item.materialMix!=0">
                  <div
                    class="span1"
                    :class="{'redText':item.materialState==-1,'sumSty':item.itemState==-1}"
                  >{{item.materialName}}</div>
                </td>-->
                <td
                  :rowspan="item.materialMix"
                  v-if="item.materialMix!=0&&item.materialState==-1"
                  style="color:#67c23a;font-weight: 800;"
                >
                  <div class="span1">{{item.materialName}}</div>
                </td>
                <td
                  :rowspan="item.materialMix"
                  v-else-if="item.materialMix!=0&&item.itemState==-1"
                  style="color:red;font-weight: 800;"
                >
                  <div class="span1">{{item.materialName}}</div>
                </td>
                <td :rowspan="item.materialMix" v-else-if="item.materialMix!=0">
                  <div class="span1">{{item.materialName}}</div>
                </td>

                <td>
                  <div class="span1">{{item.partName}}</div>
                </td>
                <td>
                  <div class="span9">{{item.lengthValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.widthValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.numValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.wideValue}}</div>
                </td>
                <td>
                  <div class="span9">{{item.lossValue}}</div>
                </td>
                <!-- <td>
                  <div
                    class="span9"
                    :class="{'redText':item.materialState==-1,'sumSty':item.itemState==-1}"
                  >{{item.singleValue}}</div>
                </td>-->
                <td v-if="item.materialState==-1" style="color:#67c23a;font-weight: 800;">
                  <div class="span1">{{item.singleValue}}</div>
                </td>
                <td v-else-if="item.itemState==-1" style="color:red;font-weight: 800;">
                  <div class="span1">{{item.singleValue}}</div>
                </td>
                <td v-else>
                  <div class="span9">{{item.singleValue}}</div>
                </td>
              </tr>
              <tr>
                <td colspan="9" :rowspan="imgBox">
                  <div style="width:100%;height:100%;text-align:center;font-size:18px;">
                    <img
                      :src="imgUrlName+imageUrl1"
                      style="width:150px;height:150px;"
                      width="150"
                      height="150"
                    />&nbsp;&nbsp;正面照
                    <br />&nbsp;
                    <br />
                    <img
                      :src="imgUrlName+imageUrl2"
                      style="width:150px;height:150px;"
                      width="150"
                      height="150"
                    />&nbsp;&nbsp;侧面照
                    <br />&nbsp;
                    <br />
                    <img
                      :src="imgUrlName+imageUrl3"
                      style="width:150px;height:150px;"
                      width="150"
                      height="150"
                    />&nbsp;&nbsp;反面照
                    <br />&nbsp;
                    <br />
                  </div>
                </td>
              </tr>
              <tr style="height:30px;" v-for="(item,index) in (imgBox-1)" :key="index"></tr>
            </tbody>
          </table>
        </div>
        <div style="clear:both;"></div>
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
import { newGuid } from "@/utils/guid";
import request from "@/utils/request";
import {
  formatDate,
  isEmpty,
  isRealNum,
  getMutipSort,
  getSort,
  deepClone
} from "@/utils/common.js";
import export2Excel from "@/utils/Export2Excel.js";
import { setTimeout } from "timers";
import BigNumber from "bignumber.js";
// import print from "@/utils/print.js";

export default {
  data() {
    return {
      rowspan: 40,
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
      tableData2: [],
      bomData: {},
      mergeArr: [],
      dialogWidth: "1225px",
      printHg: "auto",
      printState: -1,
      imgBox: 20,
      title:""
    };
  },
  props: {
    // title: {
    //   default: "详情"
    // }
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
      // var hg = 60; //图片宽高
      // var wh = 60; //图片宽高
      // var caption = `
      // <tr>
      //   <td colspan="9" style="text-align:center;font-size:18px;">${
      //     this.bomData.title
      //   }</td>
      // </tr>
      // <tr>
      //   <td colspan="9">出格编号：${this.bomData.pagerCode}</td>
      // </tr>
      // <tr>
      //   <td colspan="9">出格师傅：${this.bomData.pagerCode}</td>
      // </tr>
      // <tr>
      //   <td>正面：</td>
      //   <td style="min-width:80px;height:${hg}px; text-align: center; vertical-align: middle"><img  width="${wh}" height="${hg}" src="${this
      //   .imgUrlName + this.imageUrl1}" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
      //   <td>侧面：</td>
      //   <td style=min-width:80px;"height:${hg}px; text-align: center; vertical-align: middle"><img  width="${wh}" height="${hg}" src="${this
      //   .imgUrlName + this.imageUrl2}" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
      //   <td>背面：</td>
      //   <td style="min-width:80px;height:${hg}px; text-align: center; vertical-align: middle"><img  width="${wh}" height="${hg}" src="${this
      //   .imgUrlName + this.imageUrl3}" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
      // </tr>
      // `;
      // var thead = `<thead>
      // <tr>
      //   <th>配色项目</th>
      //   <th>物料名称</th>
      //   <th>部位名称</th>
      //   <th>长</th>
      //   <th>宽</th>
      //   <th>件数</th>
      //   <th>封度（宽幅）</th>
      //   <th>损耗</th>
      //   <th>单用量</th>
      // </tr>
      // </thead>`;
      // var tbody = "";
      // this.tableData.map((item, index) => {
      //   tbody += `<tr>`;

      //   for (var i = 0; i < this.mergeArr.length; i++) {
      //     if (item.itemId == this.mergeArr[i]["itemId"]) {
      //       if (index == this.mergeArr[i]["rowIndex"]) {
      //         // console.log(item.len)
      //         tbody += `<td rowspan="${item.len}">${this.isUndefined(
      //           item.itemName
      //         )}${item.len}</td>`;
      //       } else {
      //         // console.log(0)
      //       }
      //     }
      //   }

      //   //物料名称（添加有颜色）
      //   var materialName = this.isUndefined(item.materialName);
      //   if (item.total == -1) {
      //     tbody += `<td  style="color:#67c23a;font-weight: 800;">${materialName}</td>`;
      //   } else if (item.state == -1) {
      //     tbody += `<td  style="color:red;font-weight: 800;font-size:16px;">${materialName}</td>`;
      //   } else {
      //     tbody += `<td>${materialName}</td>`;
      //   }

      //   tbody += `<td>${this.isUndefined(item.dicValue)}</td>
      //   <td>${this.isUndefined(item.lengthValue)}</td>
      //   <td>${this.isUndefined(item.widthValue)}</td>
      //   <td>${this.isUndefined(item.numValue)}</th>
      //   <td>${this.isUndefined(item.wideValue)}</td>
      //   <td>${this.isUndefined(item.lossValue)}</td>`;
      //   //物料名称（添加有颜色）
      //   var singleValue = this.isUndefined(item.singleValue);
      //   if (item.total == -1) {
      //     tbody += `<td  style="color:#67c23a;font-weight: 800;">${singleValue}</td>`;
      //   } else if (item.state == -1) {
      //     tbody += `<td  style="color:red;font-weight: 800;font-size:16px;">${singleValue}</td>`;
      //   } else {
      //     tbody += `<td>${singleValue}</td>`;
      //   }

      //   tbody += `</tr>`;
      // });
      // var table = `<table border="1" cellspacing="0" cellpadding="0">${caption}${thead}<tbody>${tbody}</tbody></table>`;
      // table = `<div>${document.getElementById("printMe").innerHTML}</div>`;
      var table = `<table>
      <tr>
        <td>
          <table border="1"cellpadding="0"cellspacing="0">
          ${document.getElementById("print-content1").innerHTML}
          </table>
        </td>
        <td>
          <table border="1"cellpadding="0"cellspacing="0">
          ${document.getElementById("print-content2").innerHTML}
          </table>
        </td>
      </tr>
      </table>`;
      // console.log(table)
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
      if (columnIndex === 1) {
        let ind = row.len;
        for (var i = 0; i < this.mergeArr.length; i++) {
          if (row.itemId == this.mergeArr[i]["itemId"]) {
            if (rowIndex == this.mergeArr[i]["rowIndex"]) {
              return {
                rowspan: ind, //合并行数
                colspan: 1
              };
            } else {
              return {
                rowspan: 0,
                colspan: 0
              };
            }
          }
        }
      }
    },
    open(state) {
      this.tableFlag = false;
      this.printState = state;
      if (state == 2) {
        this.dialogWidth = "1225.56px";
        this.printBoxHeight = "auto";
        this.printHg = "600px";
        this.title="打印"
      } else {
        this.dialogWidth = "1225px";
        this.printBoxHeight = "auto";
        this.printHg = "600px";
        this.imgBox = 25;
        this.title="导出"
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
    async setTableData(data) {
      this.tableData = [];
      this.tableData2 = [];
      //排序
      var itemNameSort = getSort(function(a, b) {
        return a.itemName < b.itemName;
      });
      var materialNameSort = getSort(function(a, b) {
        return a.materialName < b.materialName;
      });
      var sortArr = [itemNameSort, materialNameSort];
      data.sort(getMutipSort(sortArr));
      data.map(item => {
        item.id = newGuid();
      });
      var dt = deepClone(data);
      var compList = await this.sortTable(dt);
      var imgBox = this.imgBox;
      if (compList.length > 60) {
        var df = parseInt(compList.length / 2); //获取中间条数
        var df1 =
          df + parseInt(imgBox / 2) <= data.length
            ? df + parseInt(imgBox / 2)
            : compList.length - 1;
        // var df2 = compList.length - df1 <= 0 ? 0 : compList.length - df1;

        //获取中间条数id（排除合计）
        function getId(dt, key) {
          if (dt[key]["itemState"] == -1 || dt[key]["materialState"] == -1) {
            return getId(dt, key - 1);
          } else {
            return dt[key]["id"];
          }
        }
        var id_ = getId(compList, df1);

        //根据id获取在原数组的中间条数
        var dl1 = 0;
        data.map((item, key) => {
          if (item.id == id_) {
            dl1 = key;
          }
        });
      } else {
        var dl1 = data.length;
      }

      var data1 = data.slice(0, dl1);
      var data2 = data.slice(dl1, data.length);
      var tableData = await this.sortTable(data1);
      var tableData2 = await this.sortTable(data2);
      if (tableData.length > tableData2.length + this.imgBox) {
        for (
          var i = 0;
          tableData.length - (tableData2.length + this.imgBox);
          i++
        ) {
          tableData2.push({});
        }
      } else {
        // console.log(tableData2.length + this.imgBox - tableData.length);
        for (
          var i = 0;
          tableData2.length + this.imgBox - tableData.length;
          i++
        ) {
          tableData.push({});
        }
      }
      this.tableData = tableData;
      this.tableData2 = tableData2;
      this.$emit("closeLoading");
    },
    sortTable(data) {
      return new Promise((resolve, reject) => {
        //合并
        var itemName = "";
        var materialName = "";
        var itemName2 = "";
        // data.map((item, key) => {
        for (var key = 0; key < data.length; key++) {
          // data[key].id = newGuid();
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
                id: newGuid(),
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
                id: newGuid(),
                materialName: "合计",
                itemState: -1,
                itemNameMix: 0,
                materialMix: 1,
                singleValue: dt.itemSum
              });
            }
            if (!data[key + 1]) {
              data.splice(parseInt(key) + 1, 0, {
                id: newGuid(),
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
              id: newGuid(),
              materialName: "合计",
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

        resolve(data);
      });
    }
  }
};
</script>

<style lang="scss" scoped>
#Print /deep/ {
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
    font-size: 12px;
    // font-weight: 600;
  }
  .sumSty {
    color: red !important;
    // font-weight: 800;
    font-size: 12px;
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
#topTable {
  table tr {
    border: dashed 1px #a59e9e;
    border-bottom: none;
  }
}
#printMe {
  padding: 0px 0px;
}
#printMe tr {
  height: 30px;
}
#printMe td {
  font-size: 12px;
  transform: scale(0.7);
  transform-origin: 0;
  position: relative;
}
#printMe th {
  font-weight: 200;
  font-size: 12px;
  transform: scale(0.7);
  position: relative;
}
#printMe th div,
#printMe td div {
  margin-left: 5px;
  height: 25px;
  line-height: 20px;
  position: absolute;
  top: 7px;
}
.span1_ {
  margin-right: -40px;
}
.span2_ {
  margin-right: -95px;
}
.span3_ {
  margin-right: -60px;
}
.span1 {
  margin-right: -40px;
}
.span4 {
  margin-right: -60px;
}
.span9 {
  margin-right: -30px;
}
</style>

