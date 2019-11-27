<template>
  <div id="InboundOrder">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="1000px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-background="rgba(150, 150, 150, 0.6)"
        element-loading-spinner="el-icon-loading"
      >
        <div style="height:400px;overflow:auto;">
          <el-table
            :data="tableData"
            :height="380"
            :highlight-current-row="true"
            @sort-change="handelSortChange"
          >
            <el-table-column label="序号" width="70">
              <template slot-scope="scope">
                <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
              </template>
            </el-table-column>

            <el-table-column prop="inventoryType" label="类型">
              <template slot-scope="scope">
                <!-- {{scope.row.inventoryType}}
                {{scope.row.operateType}}-->
                <div v-if="scope.row.operateType==1">
                  <div v-if="scope.row.inventoryType==0">采购入库</div>
                  <div v-if="scope.row.inventoryType==1">生产入库</div>
                  <div v-if="scope.row.inventoryType==2">委外入库</div>
                  <div v-if="scope.row.inventoryType==3">其他入库</div>
                  <div v-if="scope.row.inventoryType==4">盘盈入库</div>
                </div>
                <div v-if="scope.row.operateType==2">
                  <div v-if="scope.row.inventoryType==0">采购出库</div>
                  <div v-if="scope.row.inventoryType==1">生产出库</div>
                  <div v-if="scope.row.inventoryType==2">委外出库</div>
                  <div v-if="scope.row.inventoryType==3">其他出库</div>
                  <div v-if="scope.row.inventoryType==4">盘亏出库</div>
                </div>
              </template>
            </el-table-column>

            <el-table-column prop="typeName" label="单据类型" />

            <el-table-column prop="orderNO" label="单据号" width="180" sortable="custom">
              <template slot-scope="scope">
                <div class="link" @click="goPage(scope.row)">{{scope.row.orderNO}}</div>
              </template>
            </el-table-column>

            <el-table-column prop="baseUnitName" label="基本单位名称" />

            <el-table-column prop="baseUnitNumber" label="基本单位数量" />

            <el-table-column prop="warehouseUnitName" label="单位">
              <template slot-scope="scope">
                <div v-if="scope.row.warehouseUnitName">{{scope.row.warehouseUnitName}}</div>
                <div v-else>{{scope.row.baseUnitName}}</div>
              </template>
            </el-table-column>
            <el-table-column prop="warehouseAmount" label="数量" />

            <el-table-column
              prop="opearateDate"
              label="日期"
              @sort-change="handelSortChange"
              width="100"
            >
              <template slot-scope="scope">{{scope.row.opearateDate|formatTDate}}</template>
            </el-table-column>
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
        <!-- <el-button type="primary" @click="OnBtnSaveSubmit">选择</el-button> -->
      </div>
    </el-dialog>
  </div>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import Pagination from "@/components/Pagination";
import { formatDate } from "@/utils/common.js";

export default {
  name: "viewsinventoryInventoryCountindexvue",
  data() {
    return {
      sortColumn: "",
      sortOrder: "",
      dialogVisible: false,
      fullscreenLoading: false,
      tableData: [],
      WarehouseId: -1,
      MaterialId: -1,
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1 // 页码
    };
  },
  props: {
    title: {
      default: "历史记录"
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
    // this.getMainList();
  },
  methods: {
    goPage(item) {
      var typeName = item.typeName;
      var name = "";
      var param = {
        item,
        type: 1
      };
      //采购入库
      if (typeName.indexOf("采购入库") != -1) {
        name = "CGRKD";
      }
      //生产入库
      if (typeName.indexOf("生产入库") != -1) {
        name = "HHSCRK";
      }
      //委外入库
      if (typeName.indexOf("委外入库") != -1) {
        return;
        name = "CGRKD";
      }
      //其他入库
      if (typeName.indexOf("其他入库") != -1) {
        name = "KCHQtk";
      }
      //盘盈入库
      if (typeName.indexOf("盘盈入库") != -1) {
        name = "KCHPyK";
      }
      //采购出库
      if (typeName.indexOf("采购出库") != -1) {
        return;
        name = "CGRKD";
      }
      //生产出库
      if (typeName.indexOf("生产出库") != -1) {
        name = "HLDYF";
      }
      //委外出库
      if (typeName.indexOf("委外出库") != -1) {
        return;
        name = "CGRKD";
      }
      //其他出库
      if (typeName.indexOf("其他出库") != -1) {
        name = "HHHFuM";
      }
      //盘亏出库
      if (typeName.indexOf("盘亏出库") != -1) {
        name = "YKCKD";
      }
      //销售出库
      if (typeName.indexOf("销售出库") != -1) {
        name = "HHHXSCK";
      }
      if (name == "") return;
      this.dialogVisible = false;
      // this.$router.push({
      //   name: name,
      //   params: param
      // });
      this.$router.push({
        path: name,
        query: {
          id: this.btoa(param.item.id)
        }
      });
      return;
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
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop;
      this.sortOrder = currSort.order == "ascending" ? "asc" : "desc";
      this.getMainList();
    },
    getMainList() {
      //角色列表
      var param = {};
      var orderConditions = [];
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        });
      } else {
        orderConditions = [
          {
            column: "opearateDate",
            condition: "desc"
          },
          {
            column: "orderNO",
            condition: "desc"
          }
        ];
      }
      param.IsPaging = true;
      param.PageSize = this.pageSize;
      param.PageIndex = this.pageIndex;
      param.WarehouseId = this.WarehouseId;
      param.MaterialId = this.MaterialId;
      param.orderByConditions = orderConditions;
      // console.log(param);

      this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "warehouse/api/InventoryReport/History",
        method: "post",
        data: param
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
#InboundOrder /deep/ {
  .el-dialog--center .el-dialog__body {
    padding: 20px 20px 0px;
  }
  .el-table--small td {
    padding: 8px !important;
  }
}
</style>

