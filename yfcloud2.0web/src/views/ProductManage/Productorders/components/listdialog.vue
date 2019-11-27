<template>
  <div id="listdialog">
    <el-dialog
      :visible.sync="showdetialdialog"
      width="1000px"
      :center="true"
      title="选择源单号"
      @close="showdetialdialog=false"
    >
      <el-main
        v-if="showtab"
        style="height:520px;overflow:auto;"
        v-loading="fullscreenLoading"
        element-loading-background="rgba(1, 1, 1, 0)"
        element-loading-spinner="el-icon-loading"
      >
        <el-header id="elheader" class="header" height="auto">
          <el-form ref="dtData" label-width="90px" class="FormInput" inline label-position="right">
            <el-form-item label="订单单号：">
              <el-input
                v-model="dtData.orderNo"
                placeholder="订单单号"
                clearable
                style="width:220px"
                @keyup.enter.native="getMainList(pageIndex=1)"
                @clear="getMainList(pageIndex=1)"
              >
                <i
                  slot="suffix"
                  class="el-input__icon el-icon-search"
                  @click="getMainList(pageIndex=1)"
                ></i>
              </el-input>
            </el-form-item>
            <el-form-item label="订单时间：">
              <el-date-picker
                style="width:400px"
                v-model="dtData.orderDate"
                type="daterange"
                align="left"
                unlink-panels
                range-separator="至"
                start-placeholder="开始日期"
                end-placeholder="结束日期"
                clearable
                @change="getMainList(pageIndex=1)"
                @clear="getMainList(pageIndex=1)"
              />
            </el-form-item>
          </el-form>
        </el-header>
        <el-table
          :highlight-current-row="true"
          @row-dblclick="dbtoproduct"
          @row-click="rowClick1"
          align="center"
          :header-cell-style="{'text-align':'center'}"
          ref="table"
          :data="tableData"
          style="width: 99.9%"
          row-key="id"
          :height="380"
          @sort-change="handelSortChange"
        >
          <!-- :summary-method="getSummaries"
          show-summary-->
          <el-table-column label="序号" width="70">
            <template slot-scope="scope">
              <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="orderDate" label="订单日期" width="150px" sortable="custom">
            <template slot-scope="scope">
              <div>{{scope.row.orderDate |formatTDate}}</div>
            </template>
          </el-table-column>
          <el-table-column prop="orderNo" label="销售订单号" width="150px" sortable="custom">
            <template slot-scope="scope">
              <!-- @click="goPage(scope.row,1)"  -->
              <!-- class="link" -->
              <div>{{scope.row.orderNo}}</div>
            </template>
          </el-table-column>
          <el-table-column label="客户" width="150px">
            <template slot-scope="scope">
              <div>{{scope.row.customerName}}</div>
            </template>
          </el-table-column>

          <el-table-column label="业务员" width="150px">
            <template slot-scope="scope">
              <div>{{scope.row.salesmanName}}</div>
            </template>
          </el-table-column>
          <el-table-column label="金额" width="150px">
            <template slot-scope="scope">
              <div>{{scope.row.salesAmount}}</div>
            </template>
          </el-table-column>

          <el-table-column label="联系人" width="150px">
            <template slot-scope="scope">
              <div>{{scope.row.contactName}}</div>
            </template>
          </el-table-column>

          <el-table-column label="联系人电话" width="150px">
            <template slot-scope="scope">
              <div>{{scope.row.contactNumber}}</div>
            </template>
          </el-table-column>

          <el-table-column label="订单类型" width="150px">
            <template slot-scope="scope">
              <div>{{scope.row.orderTypeName}}</div>
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
            width="70"
            fixed="right"
            v-if="(btnAip.stopproduct&&btnAip.stopproduct.buttonCaption)"
          >
            <template slot-scope="scope">
              <el-tooltip
                v-if="btnAip.stopproduct&&btnAip.stopproduct.buttonCaption"
                class="item"
                effect="light"
                :content="btnAip.stopproduct.buttonCaption"
                placement="top-start"
                :open-delay="200"
              >
                <el-button
                  type="danger"
                  icon="el-icon-circle-close"
                  circle
                  @click="StopReceipt(scope.row)"
                  :disabled="scope.row.auditStatus!=2"
                />
              </el-tooltip>
            </template>
          </el-table-column>
          <!-- 
        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
              <el-link style="color:skyblue" @click="toproduct(scope.row.orderNo,scope.row.id)" >
                选择该订单
              </el-link>
          </template>
          </el-table-column>-->
        </el-table>
      </el-main>

      <Pagination
        :page-index="pageIndex"
        :total-count="totalCount"
        :page-size="pageSize"
        @pagination="pagination"
      />

      <div slot="footer" class="dialog-footer">
        <el-button @click="showdetialdialog=false">关闭</el-button>
        <el-button type="primary" @click="dbtoproduct2">选择</el-button>
      </div>
    </el-dialog>

    <!-- <el-dialog :visible.sync="showdetialdialog" width="60%">
      <newdialog :dialogid="dialogid" v-if="showdetialdialog" />
    </el-dialog>-->
  </div>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim } from "@/utils/common.js";
import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";

// import { mapGetters } from "vuex";

export default {
  // name: "listdialog",

  components: {
    Pagination
  },
  props: {
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
  data() {
    return {
      fullscreenLoading:false,
      sortColumn: "",
      sortOrder: "",
      clickRow: {},
      showtab: true,
      showtips: false,
      dialogid: null,
      showdetialdialog: false,
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      orderTypeoption: [],

      dtData: {
        orderNo: "",
        orderDate: "",
        orderTypeId: -1,
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
    // this.btnAip = getBtnCtr(
    //   this.$route.path,
    //   this.$store.getters.userPermission
    // );
    this.getMainList(); // 获取table数据
  },
  async activated() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
    this.setStyle();
    this.pathData = getName("/HHHFuz", this.$store.getters.userPermission);
    this.getorderTypeoption();
  },
  mounted() {
    this.setStyle();
    this.pathData = getName("/HHHFuz", this.$store.getters.userPermission);
    this.getorderTypeoption();

    // setTimeout(() => {
    //   this.showtab=false
    // setTimeout(() => {
    //   this.showtab=true
    // }, 2);
    // }, 100);
  },
  methods: {
    StopReceipt(row, state) {
      this.$confirm("是否终止？", {
        type: "warning"
      })
        .then(() => {
          var URL = "";
          URL = "/sales/api/TSSMSalesOrderMain/StopProduct";
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
    rowClick1(row) {
      this.clickRow = row;
    },
    open() {
      this.showdetialdialog = true;
    },
    close() {
      this.showdetialdialog = false;
    },
    toproduct(code, id) {
      this.$emit("salescode", [code, id]);
      this.close();
    },
    dbtoproduct(row) {
      this.$emit("salescode", [row.orderNo, row.id]);
      this.close();
    },
    dbtoproduct2() {
      this.$emit("salescode", [this.clickRow.orderNo, this.clickRow.id]);
      this.close();
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
    getorderTypeoption() {
      // var rqs = RequestObject.CreateGetObject(false, 0, 0, [
      //   { column: "typeId", content: 18, condition: 0 }
      // ]);
      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [{ column: "TypeName", content: "订单类型", condition: 0 }],
        orderByConditions: [{ column: "orderDate", condition: "desc" }]
      };

      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.orderTypeoption = res.data;
          this.orderTypeoption.unshift({
            dicValue: "所有",
            id: -1
          });
        }
      });
    },

    showdetial(e) {
      this.dialogid = e;
      this.showdetialdialog = true;
    },

    goPage(item, type) {
      var name = "";
      if (type == 1) {
        name = "HLDYC";
        this.$router.push({
          name: name,
          params: {
            item,
            type
          }
        });
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
        this.tableHeight = b - h - f - 40;
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
        _LogName: `删除ID：${item.id} 的销售订单`
      };
      var data = RequestObject.CreatePostObject(currData);

      this.$confirm("是否删除销售订单？", "确认信息", {
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
        url: "/sales/api/TSSMSalesOrderMain",
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "保存数据成功",
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
      if (this.dtData.orderTypeId != -1) {
        list.push({
          column: "orderTypeId",
          content: this.dtData.orderTypeId,
          condition: 6
        });
      }
      //  orderNo:"",
      // orderDate:'',
      if (this.dtData.orderNo != "") {
        list.push({
          column: "orderNo",
          content: trim(this.dtData.orderNo),
          condition: 6
        });
      }

      if (this.dtData.orderDate != null) {
        if (this.dtData.orderDate[0]) {
          var startDate = formatDate(this.dtData.orderDate[0]);
          var endDate = formatDate(this.dtData.orderDate[1]);
          list.push({
            column: "orderDate",
            content: this.dtData.orderDate[0] ? startDate + "," + endDate : "",
            condition: 10
          });
        }
      }
      // if (this.dtData.contactName != "") {
      //   list.push({
      //     column: "contactName",
      //     content: trim(this.dtData.contactName),
      //     condition: 6
      //   });
      // }

      if (this.dtData.auditStatus != -1) {
        list.push({
          column: "auditStatus",
          content: this.dtData.auditStatus,
          condition: 6
        });
      }
      list.push({ column: "transProdStatus", content: 1, condition: 0 });
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        });
      } else {
        orderConditions = [
          {
            column: "orderDate",
            condition: "desc"
          },
          {
            column: "orderNo",
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
        url: "sales/api/TSSMSalesOrderMain/GetMainList",
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

      // var rqs = RequestObject.CreateGetObject(false, 0, 0, {"column":"transProdStatus","content":1,"condition":0});
      //   request({
      //     url: "/manufacturing/api/TSSMSalesOrderMain/GetMainList",
      //     method: "get",
      //     params: {
      //       requestObject: JSON.stringify(rqs)
      //     }
      //   }).then(res => {
      //     this.loading = false;
      //     if (res.code == -1) {
      //       this.$confirm(res.info, "错误信息", {
      //         confirmButtonText: "确定",
      //         type: "error",
      //         showCancelButton: false
      //       });
      //     } else {
      //       this.connectionData = res.data;
      //     }
      //   });
    }
  }
};
</script>
<style>
#listdialog .el-dialog__body {
  padding: 20px 20px 0px !important;
}
</style>
<style lang="scss" scoped>
#listdialog /deep/ {
  .el-table--small td {
    padding: 8px !important;
  }
  .cell {
    text-align: center;
  }
  .header {
    // padding-top: 20px;
  }
  .link {
    color: #1890ff;
    cursor: pointer;
  }
  .el-header .el-form {
    // border-bottom: 1px solid #eee;
    // margin-bottom: 20px;
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
