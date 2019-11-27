<template>
  <div id="Preview">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="1200px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-background="rgba(150, 150, 150, 0.6)"
        element-loading-spinner="el-icon-loading"
      >
        <el-header id="elheader" class="header" height="auto">
          <el-form
            ref="dtData"
            label-width="76px"
            class="FormInput pr"
            inline
            label-position="left"
          >
            <div class="bomTitle">{{bomData.title}}</div>
            <el-form-item label="出格编号：">
              <div style="width:200px;">{{bomData.pagerCode}}</div>
            </el-form-item>
            <el-form-item label="出格师傅：">
              <div style="width:200px;">{{bomData.realName}}</div>
            </el-form-item>
            <div class="imgMain">
              <el-form-item>
                <div class="imgVBox">
                  <img v-if="imageUrl1" :src="imgUrlName+imageUrl1" class="avatar" />
                  <i v-else class="el-icon-plus avatar-uploader-icon pr">
                    <font class="imgText text1">正</font>
                    <font class="imgText text2">面</font>
                  </i>
                </div>
              </el-form-item>
              <el-form-item>
                <div class="imgVBox">
                  <img v-if="imageUrl2" :src="imgUrlName+imageUrl2" class="avatar" />
                  <i v-else class="el-icon-plus avatar-uploader-icon pr">
                    <font class="imgText text1">侧</font>
                    <font class="imgText text2">面</font>
                  </i>
                </div>
              </el-form-item>
              <el-form-item style="margin-right:0px;">
                <div class="imgVBox">
                  <img v-if="imageUrl3" :src="imgUrlName+imageUrl3" class="avatar" />
                  <i v-else class="el-icon-plus avatar-uploader-icon pr">
                    <font class="imgText text1">背</font>
                    <font class="imgText text2">面</font>
                  </i>
                </div>
              </el-form-item>
            </div>
          </el-form>
        </el-header>
        <div style="height:500px;overflow:auto;" class="nosel">
          <el-table
            :data="tableData"
            :span-method="objectSpanMethod"
            :row-class-name="tableRowClassName"
            height="480"
            border
            style="width: 100%"
            :show-summary="true"
            :summary-method="getSummaries"
          >
            <el-table-column label="序号" width="80">
              <template slot-scope="scope">
                <div>{{scope.$index+1}}</div>
              </template>
            </el-table-column>
            <!-- <el-table-column prop="materialId" label="物料名称" width="150">
              <template slot-scope="scope">
                <div :class="{'redText':scope.row.state==1}">{{scope.row.materialId}}</div>
              </template>
            </el-table-column> -->
            <el-table-column prop="materialCode" label="物料代码" width="150">
              <template slot-scope="scope">
                <div :class="{'redText':scope.row.state==1}">{{scope.row.materialCode}}</div>
              </template>
            </el-table-column>
            <el-table-column prop="materialName" label="物料名称" width="150">
              <template slot-scope="scope">
                <div :class="{'redText':scope.row.state==1}">{{scope.row.materialName}}</div>
              </template>
            </el-table-column>
            <el-table-column prop="dicValue" label="部位" width="150">
              <template slot-scope="scope">
                <div :class="{'redText':scope.row.state==1}">{{scope.row.dicValue}}</div>
              </template>
            </el-table-column>
            <!-- <el-table-column prop="dicValue" label="部位名称" width="150" /> -->
            <el-table-column prop="lengthValue" label="长" />
            <el-table-column prop="widthValue" label="宽" />
            <el-table-column prop="numValue" label="件数" />
            <el-table-column prop="wideValue" label="封度（宽幅）" width="100" />
            <el-table-column prop="lossValue" label="损耗" />
            <el-table-column prop="singleValue" label="单用量" width="150">
              <template slot-scope="scope">
                <div :class="{'redText':scope.row.state==1}">{{scope.row.singleValue}}</div>
              </template>
            </el-table-column>
          </el-table>
        </div>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible=false">关闭</el-button>
        <!-- <el-button type="primary" @click="OnBtnSaveSubmit">选择</el-button> -->
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate } from "@/utils/common.js";
import { setTimeout } from "timers";
import BigNumber from "bignumber.js";

export default {
  data() {
    return {
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
      mergeArr: []
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
          if (index == 8) {
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
    tableRowClassName({ row, rowIndex }) {
      // if (row.state === -1) {
      //   return "color1";
      // }
      return "";
    },
    objectSpanMethod({ row, column, rowIndex, columnIndex }) {
      if (columnIndex === 1||columnIndex === 2) {
        let ind = row.len;
        for (var i = 0; i < this.mergeArr.length; i++) {
          if (row.materialId == this.mergeArr[i]["materialId"]) {
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
    open() {
      this.dialogVisible = true;
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
    setTableData(dt) {
      //分类
      // this.bomData.colorList.map(item => {
      //   item.children = [];
      //   dt.map(k => {
      //     if (item.id.split("-")[1] == k.itemId) item.children.push(k);
      //   });
      // });
      // //合计
      // this.bomData.colorList.map(item => {
      //   var name = "合计";
      //   var sum = 0;
      //   var itemName = "";
      //   item.children.forEach(j => {
      //     itemName = j.itemName;
      //     sum = sum + j.singleValue * 10000;
      //     j.len = item.children.length + 1;
      //   });
      //   if (item.children[0]) {
      //     item.children.push({
      //       materialName: "",
      //       dicValue: "",
      //       lengthValue: "",
      //       widthValue: "",
      //       numValue: "",
      //       wideValue: "",
      //       lossValue: "",
      //       itemName: itemName,
      //       itemId: item.children[0].itemId,
      //       materialName: name,
      //       singleValue: sum / 10000,
      //       len: item.children.length + 1,
      //       state: -1
      //     });
      //   }
      // });
      // //汇总
      // var ArrList = [];
      // this.bomData.colorList.map(item => {
      //   item.children.forEach((j, i) => {
      //     ArrList.push(j);
      //   });
      // });
      // this.mergeArr = [];
      // ArrList.forEach((item, index) => {
      //   if (item.state == -1) {
      //     this.mergeArr.push({
      //       itemId: item.itemId,
      //       rowIndex: index + 1 - item.len,
      //       len: item.len
      //     });
      //   }
      // });
      dt.sort((a, b) => {
        return a.materialId - b.materialId;
      }); //升序
      var ArrDt = [];
      var isHas = (ArrDt, id) => {
        var flag = false;
        for (var j = 0; j < ArrDt.length; j++) {
          if (ArrDt[j].materialId == id) {
            return true;
          }
        }
        return false;
      };
      for (var i = 0; i < dt.length; i++) {
        if (ArrDt.length == 0) {
          ArrDt.push({
            materialId: dt[i].materialId,
            childList: []
          });
        } else {
          if (!isHas(ArrDt, dt[i].materialId)) {
            ArrDt.push({
              materialId: dt[i].materialId,
              childList: []
            });
          }
        }
      }
      var childList = [];
      ArrDt.forEach(item => {
        dt.map(val => {
          if (item.materialId == val.materialId) {
            item.childList.push(val);
          }
        });
      });
      ArrDt.map(item => {
        var sum = 0;
        item.childList.forEach(j => {
          sum = BigNumber(sum)
            .plus(j.singleValue)
            .toNumber();
          j.len = item.childList.length + 1;
        });
        item.childList.push({
          materialId: item.materialId,
          materialName: "合计",
          dicValue:"合计",
          singleValue: sum,
          len: item.childList.length + 1,
          state: 1
        });
      });
      // console.log(ArrDt);
      var ArrList = [];
      ArrDt.map(item => {
        item.childList.forEach(j => {
          ArrList.push(j);
        });
      });
      this.mergeArr = [];
      ArrList.forEach((item, index) => {
        if (item.state == 1) {
          this.mergeArr.push({
            materialId: item.materialId,
            rowIndex: index + 1 - item.len,
            len: item.len
          });
        }
      });
      // console.log(JSON.stringify(dt))
      this.tableData = ArrList;
      this.$emit("closeLoading");
    },
    OnBtnSaveSubmit() {}
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
    color: red !important;
  }
}
</style>

