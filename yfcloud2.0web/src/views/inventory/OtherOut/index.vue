<template>
  <el-container
    id="OtherOut"
    v-loading="fullscreenLoading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header
      id="elheader"
      class="header headerBd"
      height="auto"
      v-if="btnAip.query.buttonCaption"
    >
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="76px" inline class="FormInput">
        <el-form-item label="出库类型：" prop="whSendType">
          <el-select
            v-model="dtData.whSendType"
            placeholder="请选择"
            @change="getMainList(pageIndex=1)"
            style="width:200px"
          >
            <el-option label="所有" :value="-1"></el-option>
            <el-option label="销售出库" :value="0"></el-option>
            <el-option label="生产出库" :value="1"></el-option>
            <el-option label="裁片出库" :value="2"></el-option>
            <el-option label="委外出库" :value="3"></el-option>
            <!-- <el-option label="盘亏出库" :value="4"></el-option> -->
            <el-option label="其他出库" :value="5"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="出库时间：">
          <el-date-picker
            style="width:400px"
            v-model="dtData.whSendDate"
            type="daterange"
            align="left"
            unlink-panels
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            @change="getMainList(pageIndex=1)"
          />
        </el-form-item>
        <el-form-item label="审核状态：">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainList(pageIndex=1)">
            <el-radio-button label="-1">全部</el-radio-button>
            <el-radio-button label="0">待出库</el-radio-button>
            <el-radio-button label="1">未通过</el-radio-button>
            <el-radio-button label="2">通过</el-radio-button>
            <!-- <el-radio-button label="3">撤销审核</el-radio-button> -->
          </el-radio-group>
        </el-form-item>
        <!-- <el-button type="primary" @click="getMainList">查询</el-button> -->
      </el-form>
      <el-button
        type="primary"
        @click="getMainList(pageIndex=1)"
        v-if="btnAip.query&&btnAip.query.buttonCaption"
      >查询</el-button>
      <el-button
        type="primary"
        @click="goPage([],3)"
        v-if="btnAip.add&&btnAip.add.buttonCaption"
      >{{ btnAip.add.buttonCaption }}</el-button>
    </el-header>

    <el-main  id="elmain" class="wmltable">
      <el-table
        v-if="showtab"
        v-cloak
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

        <el-table-column prop="whSendDate" label="出库时间" width="200%" sortable="custom">
          <template slot-scope="scope">{{scope.row.whSendDate|formatTDate}}</template>
        </el-table-column>
        <el-table-column prop="whSendType" label="出库类型" width="200%">
          <template slot-scope="scope">
            <div v-if="scope.row.whSendType==0">销售出库</div>
            <div v-if="scope.row.whSendType==1">生产出库</div>
            <div v-if="scope.row.whSendType==2">裁片出库</div>
            <div v-if="scope.row.whSendType==3">委外出库</div>
            <div v-if="scope.row.whSendType==4">盘亏出库</div>
            <div v-if="scope.row.whSendType==5">其他出库</div>
          </template>
        </el-table-column>
        <el-table-column prop="whSendOrder" label="其它出库单号" width="200%" sortable="custom">
          <template slot-scope="scope">
            <div @click="goPage(scope.row,1)" class="link">{{scope.row.whSendOrder}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="auditStatus" label="审核状态">
          <template slot-scope="scope">
            <div v-if="scope.row.auditStatus==0">待审核</div>
            <div v-if="scope.row.auditStatus==1">未通过</div>
            <div v-if="scope.row.auditStatus==2">通过</div>
          </template>
        </el-table-column>
        <el-table-column prop="number" label="出库数量" />
        <el-table-column prop="amount" label="出库金额" />

        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.edit.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.edit.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-edit"
                circle
                @click="goPage(scope.row,2)"
                :disabled="!scope.row.isShowEdit||scope.row.auditStatus==2"
              />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.delete.buttonCaption&&showtips"
              class="item"
              effect="light"
              :content="btnAip.delete.buttonCaption"
              placement="top-start"
            >
              <el-button
                type="danger"
                :disabled="scope.row.auditStatus==2"
                icon="el-icon-delete"
                circle
                @click="addPutInStorage(scope.row,showtips=false)"
              />
            </el-tooltip>

            <el-button
              v-if="btnAip.delete.buttonCaption&&!showtips"
              :disabled="scope.row.auditStatus==2"
              type="danger"
              icon="el-icon-delete"
              circle
              @click="addPutInStorage(scope.row,showtips=false)"
            />

            <!-- &&scope.row.deleteFlag -->
          </template>
        </el-table-column>
      </el-table>
    </el-main>

    <el-footer id="elfooter">
      <Pagination
        :page-sizes="[25,50,100,200]"
        :page-index="pageIndex"
        :total-count="totalCount"
        :page-size="pageSize"
        @pagination="pagination"
      />
    </el-footer>
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate } from "@/utils/common.js";

import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import { constants } from "crypto";
import { getStyle } from "@/utils/Dom.js";
import BigNumber from 'bignumber.js'
// import { mapGetters } from "vuex";

export default {
  name: "viewsinventoryOtherOutindexvue",
  components: {
    Pagination
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
      sortColumn: "",
      sortOrder: "",
      showtab: true,
      showtips: true,
      btnAip: "",
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      dtData: {
        whSendType: -1,
        whSendDate: "",
        auditStatus: -1
      },
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      warehouseData: [] //仓库列表
    };
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.getMainList(); // 获取table数据
    // this.getWarehouseData(); //仓库数据
  },
  async activated() {
    this.setStyle();
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);

    this.pathData = "viewsinventoryCheckoutindexvue";
  },
  mounted() {
    this.setStyle();
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
    this.pathData = "viewsinventoryCheckoutindexvue";
  },
  methods: {
    // getSummaries(param) {
    //   //table总计
    //   const { columns, data } = param;
    //   const sums = [];

    //   const values0 = data.map(item => {
    //     var accountNum = 0;
    //     accountNum = item.number + accountNum;
    //     return Number(accountNum);
    //   });
    //   sums[0] = "总计";
    //   sums[5] = values0.reduce((prev, curr) => {
    //     const value = Number(curr);
    //     if (!isNaN(value)) {
    //       return prev + curr;
    //     } else {
    //       return prev;
    //     }
    //   }, 0);

    //   const values1 = data.map(item => {
    //     var accountNum = 0;
    //     accountNum = item.amount + accountNum;

    //     return Number(accountNum);
    //   });
    //   sums[6] = values1.reduce((prev, curr) => {
    //     const value = Number(curr);
    //     if (!isNaN(value)) {
    //       return prev + curr;
    //     } else {
    //       return prev;
    //     }
    //   }, 0);

    //   return sums;
    // },
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
          if (index == 5 || index == 6) {
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
    // HHHFuM
    // goPage(item, type) {
    //   this.$router.push({
    //     name: this.pathData.name,
    //     params: {
    //       item,
    //       type
    //     }
    //   });
    // },

    goPage(item, type) {
      var name = "";
      if (type == 1) {
        // name = "HHHFuM";
        // this.$router.push({
        //   name: name,
        //   params: {
        //     item,
        //     type
        //   }
        // });
        this.$router.push({
          path: "HHHFuM",
          query:{
            id:this.btoa(item.id) 
          }
        });
        return;
      }
      if (type == 2) {
        name = this.pathData;
        this.$router.push({
          name: name,
          params: {
            item,
            type
          }
        });
      }

      if (type == 3) {
        name = this.pathData;
        this.$router.push({
          name: name,
          params: {
            item,
            type
          }
        });
      }
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

    addPutInStorage(item) {
      //删除
      var currData = {
        id: item.id,
        _LogName: `删除ID：${item.id} 的出库单`
      };
      var data = RequestObject.CreatePostObject(currData);

      this.$confirm("是否删除出库单？", "确认信息", {
        type: "warning"
      })
        .then(() => {
          this.postData(data);
          this.showtips = true;
        })
        .catch(() => {
          this.showtips = true;
        });
    },
    postData(data) {
      var method = "DELETE";
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/TWMOtherWhSendMain",
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "操作成功",
            type: "success"
          });
          this.getMainList();
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        }
      });
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.getMainList();
    },
    getMainList() {
      //角色列表
      var list = [];
      var orderConditions = [];
      if (this.dtData.whSendType !== -1) {
        list.push({
          column: "whSendType",
          content: this.dtData.whSendType,
          condition: 6
        });
      }
      if (this.dtData.whSendDate != null) {
        if (this.dtData.whSendDate[0]) {
          var startDate = formatDate(this.dtData.whSendDate[0]);
          var endDate = formatDate(this.dtData.whSendDate[1]);
          list.push({
            column: "whSendDate",
            content: this.dtData.whSendDate[0] ? startDate + "," + endDate : "",
            condition: 10
          });
        }
      }
      if (this.dtData.auditStatus != -1) {
        list.push({
          column: "auditStatus",
          content: this.dtData.auditStatus,
          condition: 6
        });
      }
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        });
        if (this.sortColumn == "whSendDate") {
          orderConditions.push({
            column: "whSendOrder",
            condition: "desc"
          });
        }
      } else {
        orderConditions = [
          {
            column: "whSendDate",
            condition: "desc"
          },
          {
            column: "whSendOrder",
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
      this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "warehouse/api/TWMOtherWhSendMain/GetMainList",
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
#OtherOut /deep/ {
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
