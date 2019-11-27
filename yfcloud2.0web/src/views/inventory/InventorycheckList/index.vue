<template>
  <el-container
    id="InventorycheckList"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <el-form :label-position="labelPosition" label-width="76px" inline class="FormInput">
        <el-form-item label="盘点单号：">
          <el-input
            v-model="dtData.inventoryOrder"
            placeholder="盘点单号"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
            <i
              v-if="!dtData.inventoryOrder"
              @click="getMainList(pageIndex=1)"
              style=" cursor: pointer;"
              slot="suffix"
              class="el-input__icon el-icon-search"
            />
          </el-input>
        </el-form-item>
        <el-form-item label="盘点日期：">
          <el-date-picker
            style="width:400px"
            v-model="dtData.inventoryDate"
            type="daterange"
            align="left"
            unlink-panels
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            @change="getMainList(pageIndex=1)"
          />
        </el-form-item>

        <el-form-item class="formItem" label="盘点仓库：" prop="warehouseId">
          <el-select
            v-model="dtData.warehouseId"
            placeholder="请选择"
            @change="getMainList(pageIndex=1)"
          >
            <el-option
              v-for="item in warehouseData"
              :key="item.id"
              :label="item.warehouseName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="审核状态：">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainList(pageIndex=1)">
            <el-radio-button label="-1">全部</el-radio-button>
            <el-radio-button label="0">待审核</el-radio-button>
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

    <el-main class="wmltable" id="elmain">
      <el-table
        ref="table"
        :height="tableHeight"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        :summary-method="getSummaries"
        :header-cell-style="{background:'rgb(250, 250, 250)',color:'#333'}"
        show-summary
        @sort-change="handelSortChange"
      >
        <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
          </template>
        </el-table-column>

        <el-table-column prop="inventoryOrder" label="盘点单号" width="150px" sortable="custom">
          <template slot-scope="scope">
            <div @click="goPage(scope.row,1)" class="link">{{scope.row.inventoryOrder}}</div>
          </template>
        </el-table-column>

        <el-table-column prop="inventoryDate" label="盘点日期" width="120px" sortable="custom">
          <template slot-scope="scope">{{scope.row.inventoryDate|formatTDate}}</template>
        </el-table-column>

        <!-- operatorName -->

        <el-table-column label="盘点员" width="120px">
          <template slot-scope="scope">
            <div>{{scope.row.whAdminName}}</div>
          </template>
        </el-table-column>
        <el-table-column label="仓管员">
          <template slot-scope="scope">
            <div>{{scope.row.inventoryName}}</div>
          </template>
        </el-table-column>
        <el-table-column label="制单人">
          <template slot-scope="scope">
            <div>{{scope.row.operatorName}}</div>
          </template>
        </el-table-column>

        <el-table-column label="盘点仓库名称" width="120px">
          <template slot-scope="scope">
            <div>{{scope.row.warehouseName}}</div>
          </template>
        </el-table-column>

        <!-- warehouseName -->

        <el-table-column label="物料名称">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{scope.row.childList[index].materialName}}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="仓库单位">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <!-- baseUnitName -->
              <div
                v-if="scope.row.childList[index].warehouseUnitName"
              >{{scope.row.childList[index].warehouseUnitName}}</div>
              <div
                v-if="!scope.row.childList[index].warehouseUnitName"
              >{{scope.row.childList[index].baseUnitName}}</div>
              <div
                v-if="!scope.row.childList[index].warehouseUnitName&&!scope.row.childList[index].baseUnitName"
              >&nbsp;</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="账存数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].accountNum)}}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="实存数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].actualNum)}}</div>
            </div>
          </template>
        </el-table-column>

        <!-- profitUnitNum -->

        <el-table-column label="盘盈数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].profitNum)}}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="盘亏数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].deficitNum)}}</div>
            </div>
          </template>
        </el-table-column>

        <!-- actualUnitNum -->

        <!-- accountUnitNum -->
        <el-table-column label="基本单位账存数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].accountUnitNum)}}</div>
            </div>
          </template>
        </el-table-column>
        <el-table-column label="基本单位实存数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].actualUnitNum)}}</div>
            </div>
          </template>
        </el-table-column>
        <el-table-column label="基本单位盘盈数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].profitUnitNum)}}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="基本单位盘亏数量" width="150">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{Math.floor(scope.row.childList[index].deficitUnitNum)}}</div>
            </div>
          </template>
        </el-table-column>
        <el-table-column label="备注" width="140">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <el-popover
                class="item"
                trigger="hover"
                placement="top-start"
                :content="scope.row.childList[index].remark"
              >
                <div class="ellipsis" slot="reference">{{scope.row.childList[index].remark}}</div>
              </el-popover>

              <!-- <div>{{scope.row.childList[index].remark}}</div> -->
            </div>
          </template>
        </el-table-column>

        <el-table-column label="颜色">
          <template slot-scope="scope">
            <div v-for="(item,index) in scope.row.childList" :key="index">
              <div>{{scope.row.childList[index].colorName}}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column label="审核状态">
          <template slot-scope="scope">
            <!-- <div v-for="(item,index) in scope.row.childList" :key="index"> -->
            <!-- <div>{{scope.row.childList[index].auditStatus}}</div> -->

            <div v-if="scope.row.auditStatus==0">待审核</div>
            <div v-if="scope.row.auditStatus==1">未通过</div>
            <div v-if="scope.row.auditStatus==2">通过</div>
            <!-- </div> -->
          </template>
        </el-table-column>

        <el-table-column label="操作" width="100" fixed="right">
          <!-- <template slot-scope="scope">
            <span class="detialbtn" @click="showdetial(scope.row.id)">详情</span>
          </template>-->

          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.edit.buttonCaption&&scope.row.allowEdit"
              class="item"
              effect="light"
              :content="btnAip.edit.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button type="primary" icon="el-icon-edit" circle @click="goPage(scope.row,2)" />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.addclass.buttonCaption"
              class="item"
              effect="light"
              content="详情"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-search"
                circle
                @click="showdetial(scope.row.id)"
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

    <el-dialog :visible.sync="showdetialdialog" width="60%">
      <newdialog :dialogid="dialogid" v-if="showdetialdialog" />
    </el-dialog>
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim } from "@/utils/common.js";
//
import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import newdialog from "./components/dialog";
import { getStyle } from "@/utils/Dom.js";
import BigNumber from 'bignumber.js'
// import { mapGetters } from "vuex";

export default {
  name: "viewsinventoryInventorycheckListindexvue",
  components: {
    Pagination,
    newdialog
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
      dialogid: null,
      showdetialdialog: false,
      btnAip: "",
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      dtData: {
        inventoryDate: "",
        warehouseId: -1,
        inventoryOrder: "",
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
    // this.getMainList2(); // 获取table数据
    this.getWarehouseData(); //仓库数据
  },
  async activated() {
    this.setStyle();
    this.pathData = getName("/HHHFXz", this.$store.getters.userPermission);
  },
  deactivated() {
    this.showdetialdialog = false;
  },
  // showdetialdialog
  mounted() {
    this.setStyle();
    this.pathData = getName("/HHHFXz", this.$store.getters.userPermission);
  },
  methods: {
    showdetial(e) {
      this.dialogid = e;
      this.showdetialdialog = true;
    },

    getSummaries(param) {
      //table总计
      const { columns, data } = param;
      const sums = [];
      sums[0] = "总计";
      //账存数量
      const values0 = data.map(item => {
        var accountNum = 0;
        item.childList.map(newitem => {
          accountNum = newitem.accountNum + accountNum;
        });
        return Number(accountNum);
      });
      sums[9] = values0.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return BigNumber(prev).plus(curr).toNumber();
        } else {
          return prev
        }
      }, 0);
      //实存数量
      const values1 = data.map(item => {
        var actualNum = 0;
        item.childList.map(newitem => {
          actualNum = newitem.actualNum + actualNum;
        });
        return Number(actualNum);
      });
      sums[10] = values1.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return BigNumber(prev).plus(curr).toNumber();
        } else {
          return prev
        }
      }, 0);

      //盘盈数量
      const values2 = data.map(item => {
        var profitNum = 0;
        item.childList.map(newitem => {
          profitNum = newitem.profitNum + profitNum;
        });
        return Number(profitNum);
      });
      sums[11] = values2.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return BigNumber(prev).plus(curr).toNumber();
        } else {
          return prev
        }
      }, 0);

      //盘亏数量
      const values3 = data.map(item => {
        var deficitNum = 0;
        item.childList.map(newitem => {
          deficitNum = newitem.deficitNum + deficitNum;
        });
        return Number(deficitNum);
      });
      sums[12] = values3.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return BigNumber(prev).plus(curr).toNumber();
        } else {
          return prev
        }
      }, 0);

      return sums;
    },
    goPage(item, type) {
      var name = "";
      if (type == 1) {
        // name = "HLDYY";
        // this.$router.push({
        //   name: name,
        //   params: {
        //     item,
        //     type
        //   }
        // });
        this.$router.push({
          path: "HLDYY",
          query:{
            id:this.btoa(item.id) 
          }
        });
        return;
      }

      if (type == 2) {
        name = this.pathData.name;
        this.$router.push({
          name: name,
          params: {
            item,
            type
          }
        });
      }
      if (type == 3) {
        name = this.pathData.name;
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
          var origin = {
            id: -1,
            warehouseName: "所有"
          };
          this.warehouseData.unshift(origin);
        }
      });
    },
    addPutInStorage(item) {
      //删除
      var currData = {
        id: item.id,
        _LogName: `删除ID：${item.id} 的盘点单`
      };
      var data = RequestObject.CreatePostObject(currData);

      this.$confirm("是否删除盘点单？", "确认信息", {
        type: "warning"
      })
        .then(() => {
          this.postData(data);
        })
        .catch(() => {});
    },
    postData(data) {
      var method = "DELETE";
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/TWMOtherWhMain",
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
      if (this.dtData.warehouseId != -1) {
        list.push({
          column: "warehouseId",
          content: trim(this.dtData.warehouseId),
          condition: 6
        });
      }
      if (this.dtData.auditStatus != -1) {
        list.push({
          column: "auditStatus",
          content: trim(this.dtData.auditStatus),
          condition: 6
        });
      }
      if (this.dtData.inventoryOrder != "") {
        list.push({
          column: "inventoryOrder",
          content: trim(this.dtData.inventoryOrder),
          condition: 6
        });
      }
      // inventoryOrder
      if (this.dtData.inventoryDate != null) {
        if (this.dtData.inventoryDate[0]) {
          var startDate = formatDate(this.dtData.inventoryDate[0]);
          var endDate = formatDate(this.dtData.inventoryDate[1]);
          list.push({
            column: "inventoryDate",
            content: this.dtData.inventoryDate[0]
              ? startDate + "," + endDate
              : "",
            condition: 10
          });
        }
      }
      // if (this.dtData.auditStatus != -1) {
      //   list.push({
      //     column: "auditStatus",
      //     content: this.dtData.auditStatus,
      //     condition: 6
      //   });
      // }
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        });
        if (this.sortColumn == "inventoryDate") {
          orderConditions.push({
            column: "inventoryOrder",
            condition: "desc"
          });
        }
      } else {
        orderConditions = [
          {
            column: "inventoryDate",
            condition: "desc"
          },
          {
            column: "inventoryOrder",
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
        url: "warehouse/api/TWMInventoryMain/GetInventoryOrderList",
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
#InventorycheckList /deep/ {
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
.detialbtn {
  color: #1890ff;
  cursor: pointer;
}
</style>
