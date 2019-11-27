<template>
  <el-container
    id="productList"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-header
      id="elheader"
      class="header headerBd"
      height="auto"
    >
      <el-form
        :label-position="labelPosition"
        label-width="76px"
        inline
        class="FormInput"
      >
        <el-form-item label="生产单号：">
          <el-input
            v-model="dtData.productionNo"
            placeholder="生产单号"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
            <i
              v-if="!dtData.customerName"
              @click="getMainList(pageIndex=1)"
              style=" cursor: pointer;"
              slot="suffix"
              class="el-input__icon el-icon-search"
            />
          </el-input>
        </el-form-item>

        <el-form-item label="客户名称：">
          <el-input
            v-model="dtData.customerName"
            placeholder="客户名称"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
            <i
              @click="getMainList(pageIndex=1)"
              style=" cursor: pointer;"
              slot="suffix"
              class="el-input__icon el-icon-search"
            />
          </el-input>
        </el-form-item>

        <el-form-item
          class="formItem"
          label="订单类型："
          prop="orderTypeId"
        >
          <el-select
            v-model="dtData.productionType"
            placeholder="请选择"
            @change="getMainList(pageIndex=1)"
          >
            <el-option
              v-for="item in orderTypeoption"
              :key="item.id"
              :label="item.label"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item
          class="formItem"
          label="类型："
          prop="orderTypeId"
          label-width="48px"
        >
          <el-select
            v-model="dtData.productionStatus"
            placeholder="请选择"
            @change="getMainList(pageIndex=1)"
          >
            <el-option
              v-for="item in Typeoption"
              :key="item.id"
              :label="item.label"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="审核状态：">
          <el-radio-group
            v-model="dtData.auditStatus"
            @change="getMainList(pageIndex=1)"
          >
            <el-radio-button label="-1">全部</el-radio-button>
            <el-radio-button label="0">待审核</el-radio-button>
            <el-radio-button label="1">未通过</el-radio-button>
            <el-radio-button label="2">通过</el-radio-button>
          </el-radio-group>
        </el-form-item>
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

    <el-main
      v-if="showtab"
      class="wmltable"
      id="elmain"
    >
      <el-table
        ref="table"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        :height="tableHeight"
        @sort-change="handelSortChange"
        :show-summary="true"
        :summary-method="getSummaries"
      >
        <el-table-column
          label="序号"
          width="70"
        >
          <template slot-scope="scope">
            <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
          </template>
        </el-table-column>
        <el-table-column
          prop="orderDate"
          label="订单日期"
          width="150px"
          sortable="custom"
        >
          <template slot-scope="scope">
            <div>{{scope.row.orderDate |formatTDate}}</div>
          </template>
        </el-table-column>
        <el-table-column
          prop="productionNo"
          label="生产单号"
          width="150px"
          sortable="custom"
        >
          <template slot-scope="scope">
            <div
              @click="goPage(scope.row,1)"
              class="link"
            >{{scope.row.productionNo}}</div>
          </template>
        </el-table-column>

        <el-table-column
          prop="whApplyMain"
          label="入库申请单号"
          width="150px"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.whApplyMain[0]" v-html="dataFormat(scope.row.whApplyMain)"></div>
            <div v-else></div>
          </template>
        </el-table-column>
        <el-table-column
          prop="pickModel"
          label="领料单号"
          width="150px"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.pickModel.length!=0" v-html="dataFormat(scope.row.pickModel)"></div>
            <div v-else></div>
          </template>
        </el-table-column>
        <el-table-column
          prop="purchase"
          label="采购申请单号"
          width="150px"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.purchase[0]" v-html="dataFormat(scope.row.purchase)"></div>
            <div v-else></div>
          </template>
        </el-table-column>

        <el-table-column
          label="客户名称"
          width="150px"
        >
          <template slot-scope="scope">
            <div>{{scope.row.customerName}}</div>
          </template>
        </el-table-column>
        <el-table-column
          label="类型"
          width="150px"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.productionStatus==0">待生产</div>
            <div v-if="scope.row.productionStatus==1">生产中</div>
            <div v-if="scope.row.productionStatus==2">生产完成</div>
          </template>
        </el-table-column>

        <el-table-column
          label="生产类型"
          width="150px"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.productionType==0">库存生产</div>
            <div v-if="scope.row.productionType==1">订单生产</div>
            <!-- <div>{{scope.row.productionType}}</div> -->
          </template>
        </el-table-column>

        <el-table-column label="审核状态">
          <template slot-scope="scope">
            <div v-if="scope.row.auditStatus==0">待审核</div>
            <div v-if="scope.row.auditStatus==1">未通过</div>
            <div v-if="scope.row.auditStatus==2">通过</div>
          </template>
        </el-table-column>

        <el-table-column
          label="操作"
          width="100"
          fixed="right"
        >
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
                :disabled="!scope.row.allowEdit||scope.row.auditStatus==2"
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
                icon="el-icon-delete"
                circle
                :disabled="scope.row.auditStatus==2"
                @click="addPutInStorage(scope.row,showtips=false)"
              />
            </el-tooltip>

            <el-button
              v-if="btnAip.delete.buttonCaption&&!showtips"
              type="danger"
              icon="el-icon-delete"
              circle
              :disabled="scope.row.auditStatus==2"
              @click="addPutInStorage(scope.row,showtips=false)"
            />
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

    <el-dialog
      :visible.sync="showdetialdialog"
      width="60%"
    >
      <newdialog
        :dialogid="dialogid"
        v-if="showdetialdialog"
      />
    </el-dialog>
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim } from "@/utils/common.js";
import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import newdialog from "./components/dialog";
// import { mapGetters } from "vuex";
import { getStyle } from "@/utils/Dom.js";
export default {
  name: "viewsProductManageProductordersListindexvue",

  components: {
    Pagination,
    newdialog
  },
  data() {
    return {
      sortColumn: "",
      sortOrder: "",
      showtab: true,
      showtips: false,
      dialogid: null,
      showdetialdialog: false,
      btnAip: "",
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      // 0库存生产 1订单生产
      // 生产状态 0待生产 1生产中 2生产完成
      orderTypeoption: [
        {
          id: -1,
          label: "所有"
        },
        {
          id: 0,
          label: "库存生产"
        },
        {
          id: 1,
          label: "订单生产"
        }
      ],
      Typeoption: [
        {
          id: -1,
          label: "所有"
        },
        {
          id: 0,
          label: "待生产"
        },
        {
          id: 1,
          label: "生产中"
        },
        {
          id: 2,
          label: "生产完成"
        }
      ],

      dtData: {
        productionNo: "",
        productionType: -1,
        productionStatus: -1,
        customerName: "",
        auditStatus: -1,
        contactName: ""
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
  },
  async activated() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
      this.setStyle();
    }, 10);
    // this.setStyle();
    this.pathData = "viewsProductManageProductordersindexvue";
    // this.getorderTypeoption();
  },
  mounted() {
    this.setStyle();
    this.pathData = "viewsProductManageProductordersindexvue";
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
  methods: {
    dataFormat(msg) {
      // return msg + "xxxxx";
      var text = "";
     if(msg) {
        for(var i=0;i<msg.length;i++) {
        if(i!=msg.length-1) {
          text+= msg[i].no + "</br>"
        }else {
          text+= msg[i].no
        }
      }
     }
       return text
    },
    getSummaries(param) {
      //table总计
      const { columns, data } = param;
      const sums = [];

      const values0 = data.map(item => {
        var accountNum = 0;
        accountNum = item.salesAmount + accountNum;

        return Number(accountNum);
      });
      sums[0] = "总计";
      sums[5] = values0.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return prev + curr;
        } else {
          return prev;
        }
      }, 0);

      return sums;
    },

    showdetial(e) {
      this.dialogid = e;
      this.showdetialdialog = true;
    },

    goPage(item, type) {
      var name = "";
      if (type == 1) {
        // name = "HLDYG";
        // this.$router.push({
        //   name: name,
        //   params: {
        //     item,
        //     type
        //   }
        // });
        this.$router.push({
          path: "HLDYG",
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
      // if (type == 4) {
      //   name = "HLDYE";
      //   this.$router.push({
      //     name: name,
      //     params: {
      //       item,
      //       type
      //     }
      //   });
      // }
      // if (type == 5) {
      //   name = this.pathData;
      //   this.$router.push({
      //     name: name,
      //     params: {
      //       item,
      //       type
      //     }
      //   });
      // }
      // if (type == 6) {
      //   name = this.pathData;
      //   this.$router.push({
      //     name: name,
      //     params: {
      //       item,
      //       type
      //     }
      //   });
      // }
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
        _LogName: `删除ID：${item.id} 的生产单`
      };
      var data = RequestObject.CreatePostObject(currData);

      this.$confirm("是否删除生产单？", "确认信息", {
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
        url: "/manufacturing/api/TMMProductionOrderMain",
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
      //  productionType: -1,
      // productionStatus:-1,
      var list = [];
      var orderConditions = [];
      if (this.dtData.productionType != -1) {
        list.push({
          column: "productionType",
          content: this.dtData.productionType,
          condition: 6
        });
      }

      if (this.dtData.productionNo != "") {
        list.push({
          column: "productionNo",
          content: trim(this.dtData.productionNo),
          condition: 6
        });
      }
      if (this.dtData.customerName != "") {
        list.push({
          column: "customerName",
          content: trim(this.dtData.customerName),
          condition: 6
        });
      }
      if (this.dtData.productionStatus != -1) {
        list.push({
          column: "productionStatus",
          content: this.dtData.productionStatus,
          condition: 6
        });
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
        if (this.sortColumn == "orderDate") {
          orderConditions.push({
            column: "productionNo",
            condition: "desc"
          });
        }
      } else {
        orderConditions = [
          {
            column: "orderDate",
            condition: "desc"
          },
          {
            column: "productionNo",
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
        url: "/manufacturing/api/TMMProductionOrderMain/GetMainList",
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
#productList /deep/ {
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
