<template>
  <div id="DocumentNo">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="1000px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-background="rgba(1, 1, 1, 0)"
        element-loading-spinner="el-icon-loading"
      >
        <el-header id="elheader" class="header" height="auto">
          <el-form ref="dtData" label-width="90px" class="FormInput" inline label-position="right">
            <el-form-item label="领料单号：" label-width="90px">
              <el-input
                v-model="dtData.pickNo"
                placeholder="领料单号"
                clearable
                style="width:300px"
                @keyup.enter.native="getMainListIndex"
                @clear="getMainListIndex"
              >
                <i slot="suffix" class="el-input__icon el-icon-search" @click="getMainListIndex"></i>
              </el-input>
            </el-form-item>

            <el-form-item label="订单时间：">
              <el-date-picker
                style="width:400px"
                v-model="dtData.applyDate"
                type="daterange"
                align="left"
                unlink-panels
                range-separator="至"
                start-placeholder="开始日期"
                end-placeholder="结束日期"
                @change="getMainListIndex"
              />
            </el-form-item>
            <!-- <el-form-item label="状态：" label-width="86px">
              <el-radio-group v-model="dtData.auditId" @change="getMainList(pageIndex=1)">
                <el-radio-button label="-1">全部</el-radio-button>
                <el-radio-button label="0">可算料</el-radio-button>
                <el-radio-button label="1">可转领料</el-radio-button>
                <el-radio-button label="2">可转采购</el-radio-button>
              </el-radio-group>
            </el-form-item>-->
          </el-form>
        </el-header>
        <div style="height:400px;overflow:auto;">
          <el-table
            :data="tableData"
            :height="380"
            :highlight-current-row="true"
            @row-click="rowClick"
            @cell-dblclick="cellDblclick"
            :span-method="objectSpanMethod"
            @sort-change="handelSortChange"
            v-if="tableFlag"
          >
            <!-- <el-table-column type="index" label="序号" /> -->
            <el-table-column label="序号" width="70">
              <template slot-scope="scope">
                <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
              </template>
            </el-table-column>
            <el-table-column label="领料单号">
              <template slot-scope="scope">
                <div @click="goPage(scope.row,1)">{{scope.row.pickNo}}</div>
              </template>
            </el-table-column>

            <el-table-column label="订单日期" sortable="custom">
              <template slot-scope="scope">
                <div>{{scope.row.applyDate |formatTDate}}</div>
              </template>
            </el-table-column>

            <el-table-column label="状态">
              <template slot-scope="scope">
                <div v-if="scope.row.auditStatus==0">待审核</div>
                <div v-if="scope.row.auditStatus==1">未通过</div>
                <div v-if="scope.row.auditStatus==2">通过</div>
              </template>
            </el-table-column>
            <el-table-column
              label="操作"
              width="70"
              fixed="right"
              v-if="(btnAip.stopwarepdo&&btnAip.stopwarepdo.buttonCaption)"
            >
              <template slot-scope="scope">
                <el-tooltip
                  v-if="btnAip.stopwarepdo&&btnAip.stopwarepdo.buttonCaption"
                  class="item"
                  effect="light"
                  :content="btnAip.stopwarepdo.buttonCaption"
                  placement="top-start"
                  :open-delay="200"
                >
                  <el-button
                    type="danger"
                    icon="el-icon-circle-close"
                    circle
                    @click="StopReceipt(scope.row)"
                  />
                </el-tooltip>
              </template>
            </el-table-column>

            <!-- applyDate: "2019-09-12T00:00:00"
auditId: 1
auditName: "云飞"
auditStatus: 2
auditTime: "2019-09-12T11:42:38"
childList: null
companyId: 1
deleteFlag: false
id: 15
isShowEdit: false
operatorId: 1
operatorName: "云飞"
operatorTime: "0001-01-01T00:00:00"
pickNo: "SO201909120010"
sourceId: 0
            transferFlag: false-->

            <!-- applyDate -->
            <!-- <el-table-column prop="packageCode" label="包型代码" width="150" />
            <el-table-column prop="packageName" label="包型名称" width="150" />
            <el-table-column prop="baseUnitName" label="基本单位" width="90" />
            <el-table-column prop="baseUnitNum" label="基本单位数量" width="120" />
            <el-table-column prop="produceUnitName" label="单位" width="90" />
            <el-table-column prop="productionNum" label="数量" width="80" />
            <el-table-column prop="workshopName" label="生产车间" width="100" />
            <el-table-column prop="principalName" label="负责人" width="100" />-->
          </el-table>
        </div>
        <Pagination
          :pageIndex="pageIndex"
          :totalCount="totalCount"
          :pageSize="pageSize"
          @pagination="pagination"
        />
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible=false">关闭</el-button>
        <el-button type="primary" @click="OnBtnSaveSubmit">选择</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import Pagination from "@/components/Pagination";
import { formatDate, trim } from "@/utils/common.js";

export default {
  data() {
    return {
      sortColumn: "",
      sortOrder: "",
      dialogVisible: false,
      dtData: {
        // tranSourceCode: "",
        pickNo: "",
        applyDate: "",
        auditId: -1
      },
      fullscreenLoading: false,
      tableData: [],
      clickRow: {},
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      connectionData: [],
      mergeArr: [],
      tableFlag: false
    };
  },
  props: {
    title: {
      default: "详情"
    },
    btnAip: {
      default: {}
    }
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
  components: {
    Pagination
  },
  mounted() {
    // this.getCustomer();
  },
  methods: {
    StopReceipt(row, state) {
      this.$confirm("是否终止", {
        type: "warning"
      })
        .then(() => {
          var URL = "";
          URL = "/manufacturing/api/TMMPickApplyMain/StopWare";
          this.fullscreenLoading = true;
          request({
            url: URL,
            method: "put",
            params: { requestObject: row.id }
          }).then(res => {
            if (res.code === 0) {
              this.$message({
                message: "操作成功",
                type: "success"
              });
              this.getMainList();
              this.$emit("reSet", 1);
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
        })
        .catch(() => {});
    },
    goPage(item, type) {
      // this.dialogVisible = false;
      // var name = "";
      // if (type == 1) {
      //   name = "HLDYE";
      //   this.$router.push({
      //     name: name,
      //     params: {
      //       item,
      //       type
      //     }
      //   });
      // }
    },
    open() {
      this.dialogVisible = true;
    },
    close() {
      this.dialogVisible = false;
    },
    pagination(val) {
      //改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList();
    },
    getMainListIndex() {
      this.pageIndex = 1;
      this.getMainList();
    },
    rowClick(row) {
      this.clickRow = row;
    },
    OnBtnSaveSubmit() {
      if (!this.clickRow.id) {
        this.$message.error("请选择数据");
        return;
      }
      this.close();
      this.$emit("OnBtnSaveSubmit", this.clickRow);
    },
    cellDblclick(row) {
      this.clickRow = row;

      this.OnBtnSaveSubmit();
    },
    objectSpanMethod({ row, column, rowIndex, columnIndex }) {
      // if (columnIndex === 0 ||columnIndex === 1 || columnIndex === 2) {
      //   let ind = row.len;
      //   for (var i = 0; i < this.mergeArr.length; i++) {
      //     if (row.pickNo == this.mergeArr[i]["pickNo"]) {
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
    },
    handleData(data) {
      //处理数据
      var ArrList = [];
      data.map((item, key) => {
        if (item.childList.length > 0) {
          item.childList.forEach((val, index) => {
            val["pickNo"] = item.pickNo;
            val["tranSourceCode"] = item.tranSourceCode; //入库申请单
            val["id_"] = val["id"];
            val["id"] = item.id;
            val["tranSourceId"] = item.tranSourceId;
            if (index + 1 === item.childList.length) {
              val["stateLen"] = -1;
              val["len"] = item.childList.length;
              val["rowIndex"] = key; //行号
            } else {
              val["stateLen"] = 0;
              val["len"] = item.childList.length;
              val["rowIndex"] = key; //行号
            }
            ArrList.push(val);
          });
        }
      });
      this.mergeArr = [];
      ArrList.forEach((item, index) => {
        if (item.stateLen == -1) {
          this.mergeArr.push({
            id: item.tranSourceId,
            id_: item.id_,
            pickNo: item.pickNo,
            tranSourceCode: item.tranSourceCode,
            rowIndex: index + 1 - item.len,
            len: item.len
          });
        }
      });
      return ArrList;
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.getMainList();
    },
    getMainList() {
      //列表
      var orderConditions = [];
      var list = [
        { column: "AuditStatus", content: 2, condition: 0 },
        { column: "transferFlag", content: 1, condition: 0 },
        // { column: "sourceId", content: 0, condition: 3 }
      ];
      // AuditStatus =2 和transferFlag=true

      if (this.dtData.applyDate != null) {
        if (this.dtData.applyDate[0]) {
          var startDate = formatDate(this.dtData.applyDate[0]);
          var endDate = formatDate(this.dtData.applyDate[1]);
          list.push({
            column: "applyDate",
            content: this.dtData.applyDate[0] ? startDate + "," + endDate : "",
            condition: 10
          });
        }
      }

      // if (this.dtData.tranSourceCode != "") {
      //   list.push({ column: "tranSourceCode", content: this.dtData.tranSourceCode, condition: 6 });
      // }
      if (this.dtData.pickNo != "") {
        list.push({
          column: "pickNo",
          content: trim(this.dtData.pickNo),
          condition: 6
        });
      }

      // if (this.dtData.auditId == 0) {
      //   list.push({ column: "mrpStatus", content: 0, condition: 0 });
      // }
      // if (this.dtData.auditId == 1) {
      //   list.push({ column: "isPick", content: 1, condition: 0 });
      // }
      // if (this.dtData.auditId == 2) {
      //   list.push({ column: "isPurchase", content: 1, condition: 0 });
      // }
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        });
      } else {
        orderConditions = [
          {
            column: "applyDate",
            condition: "desc"
          }
        ];
      }
      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        list,
        orderConditions
      );
      // this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "manufacturing/api/TMMPickApplyMain/GetMainList",
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
    }
  }
};
</script>

<style lang="scss" scoped>
#DocumentNo /deep/ {
  // .el-dialog--center .el-dialog__body {
  //   padding: 20px 20px 0px;
  // }
  .el-table--small td {
    padding: 8px !important;
  }
  .cell {
    padding: 0px 10px !important;
  }
  .link {
    color: #1890ff;
    cursor: pointer;
  }
}
</style>

