<template>
  <el-container
    id="OtherPut"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" inline class="FormInput">
        <el-form-item label="入库单号：" label-width="76px">
          <el-input
            v-model="dtData.warehousingOrder"
            placeholder="入库单号"
            clearable
            @keyup.enter.native="getMainListIndex"
            @clear="getMainListIndex"
          >
            <i slot="suffix" class="el-input__icon el-icon-search" @click="getMainListIndex"></i>
          </el-input>
        </el-form-item>
        <el-form-item label="入库时间：" label-width="76px">
          <el-date-picker
            v-model="dtData.warehousingDate"
            style="width:400px"
            type="daterange"
            align="left"
            unlink-panels
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            @change="getMainListIndex"
          />
        </el-form-item>
        <el-form-item label="审核状态：" label-width="76px">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainListIndex">
            <el-radio-button label="-1">全部</el-radio-button>
            <el-radio-button label="0">待审核</el-radio-button>
            <el-radio-button label="1">未通过</el-radio-button>
            <el-radio-button label="2">通过</el-radio-button>
            <!-- <el-radio-button label="3">撤销审核</el-radio-button> -->
          </el-radio-group>
        </el-form-item>
      </el-form>
      <el-button
        v-if="btnAip.query&&btnAip.query.buttonCaption"
        type="primary"
        @click="getMainListIndex"
      >{{ btnAip.query.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.add&&btnAip.add.buttonCaption"
        type="primary"
        @click="goPage({},3)"
      >{{ btnAip.add.buttonCaption }}</el-button>
    </el-header>
    <el-main id="elmain" class="wmltable">
      <el-table
        v-if="showtab"
        ref="table"
        :height="tableHeight"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        :show-summary="true"
        :summary-method="getSummaries"
        @sort-change="handelSortChange"
      >
        <el-table-column prop="warehousingDate" label="入库日期" width="100" sortable="custom">
          <template slot-scope="scope">{{scope.row.warehousingDate|formatTDate}}</template>
        </el-table-column>
        <el-table-column prop="warehousingOrderNo" label="入库单号" width="150" sortable="custom">
          <template slot-scope="scope">
            <div class="link" @click="goPage(scope.row,1)">{{ scope.row.warehousingOrderNo }}</div>
          </template>
        </el-table-column>

        <el-table-column prop="productionNo" label="源生产订单号" width="150">
          <template slot-scope="scope">
            <div class="link" @click="goPage(scope.row,4)">{{ scope.row.productionNo }}</div>
          </template>
        </el-table-column>

        <el-table-column prop="auditStatus" label="审核状态">
          <template slot-scope="scope">
            <div v-if="scope.row.auditStatus==0">待审核</div>
            <div v-if="scope.row.auditStatus==1">未通过</div>
            <div v-if="scope.row.auditStatus==2">通过</div>
          </template>
        </el-table-column>

        <el-table-column prop="number" label="数量" />
        <el-table-column prop="amount" label="金额" />

        <el-table-column prop="operatorName" label="制单人" />

        <el-table-column prop="receiptName" label="收货员" />

        <el-table-column prop="auditTime" label="审核时间">
          <template slot-scope="scope">{{scope.row.auditTime|formatTDate}}</template>
        </el-table-column>
        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <span v-if="btnAip.edit">
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
                :open-delay="200"
              >
                <el-button
                  :disabled="scope.row.auditStatus==2"
                  type="danger"
                  icon="el-icon-delete"
                  circle
                  @click="addPutInStorage(scope.row)"
                />
              </el-tooltip>

              <el-button
                v-if="btnAip.delete.buttonCaption&&!showtips"
                :disabled="scope.row.auditStatus==2"
                type="danger"
                icon="el-icon-delete"
                circle
                @click="addPutInStorage(scope.row)"
              />
            </span>
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
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim } from "@/utils/common.js";

import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import { getStyle } from "@/utils/Dom.js";
import BigNumber from 'bignumber.js'
// import { mapGetters } from "vuex";

export default {
  name: "viewsinventoryProductionPutindexvue",
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
      btnAip: "",
      pathData: "", // 路由信息
      pathDataView: "", // 路由信息
      tableHeight: "500",
      labelPosition: "left",
      showtips: true,
      tableData: [], // table数据模型
      dtData: {
        warehousingOrder: "",
        warehousingDate: "",
        auditStatus: -1
      },
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      warehouseData: [] // 仓库列表
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
    this.pathData = "viewsinventoryProductionReceiptindexvue";
  },
  mounted() {
    this.setStyle();
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
    this.pathData = "viewsinventoryProductionReceiptindexvue";
  },
  methods: {
    goPage(item, type) {
      var name = "";
      var param = {};
      if (type == 3) {
        //新增
        name = this.pathData;
        param = {
          item: {},
          type
        };
      }
      if (type == 1) {
        //查看入库单
        // name = "HHSCRK";
        // param = {
        //   item,
        //   type
        // };
        this.$router.push({
          path: "HHSCRK",
          query:{
            id:this.btoa(item.id) 
          }
        });
        return;
      }
      if (type == 2) {
        //编辑
        name = this.pathData;
        param = {
          item,
          type
        };
      }
      if (type == 4) {
        // name = "HLDYG";
        // param = {
        //   item: { id: item.productionID },
        //   type: 1
        // };
        this.$router.push({
          path: "HLDYG",
          query:{
            id:this.btoa(item.productionID) 
          }
        });
        return;
      }

      this.$router.push({
        name: name,
        params: param
      });
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
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList();
    },
    getMainListIndex() {
      this.pageIndex = 1;
      this.getMainList();
    },
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
    addPutInStorage(item) {
      this.showtips = false;
      // 删除
      var currData = {
        id: item.id,
        _LogName: `删除ID：${item.id} 的入库单`
      };
      var data = RequestObject.CreatePostObject(currData);

      this.$confirm("是否删除入库单？", "确认信息", {
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
        url: "/warehouse/api/TWMProductionWhMain",
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
      var orderConditions = [];
      var list = [
        {
          column: "warehousingOrderNo",
          content: trim(this.dtData.warehousingOrder),
          condition: 6
        }
      ];
      if (this.dtData.warehousingDate != null) {
        if (this.dtData.warehousingDate[0]) {
          var startDate = formatDate(this.dtData.warehousingDate[0]);
          var endDate = formatDate(this.dtData.warehousingDate[1]);
          list.push({
            column: "warehousingDate",
            content: this.dtData.warehousingDate[0]
              ? startDate + "," + endDate
              : "",
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
      } else {
        orderConditions = [
          {
            column: "warehousingDate",
            condition: "desc"
          },
          {
            column: "warehousingOrderNo",
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
        url: "warehouse/api/TWMProductionWhMain/GetMainList",
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
