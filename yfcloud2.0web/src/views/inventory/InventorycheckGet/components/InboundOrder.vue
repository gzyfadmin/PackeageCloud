<template>
  <div id="InboundOrder">
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="1000px"
      :close-on-click-modal="false"
      :center="true"
    >
      <el-header id="elheader" class="header" height="auto">
        <el-form ref="dtData" label-width="76px" class="FormInput" inline label-position="left">
          <el-form-item label="盘点单号：">
            <el-input
              v-model="dtData.warehousingOrder"
              placeholder="盘点单号"
              clearable
              style="width:320px"
              @keyup.enter.native="getMainList"
              @clear="getMainList"
            />
          </el-form-item>
          <el-form-item label="盘点时间：">
            <el-date-picker
              style="width:400px"
              v-model="dtData.warehousingDate"
              type="daterange"
              align="left"
              unlink-panels
              range-separator="至"
              start-placeholder="开始日期"
              end-placeholder="结束日期"
              @change="getMainList"
            />
          </el-form-item>
          <el-form-item label="审核状态：">
            <el-radio-group v-model="dtData.auditStatus" @change="getMainList">
              <el-radio-button label="-1">全部</el-radio-button>
              <el-radio-button label="0">待审核</el-radio-button>
              <el-radio-button label="1">未通过</el-radio-button>
              <el-radio-button label="2">通过</el-radio-button>
              <!-- <el-radio-button label="3">撤销审核</el-radio-button> -->
            </el-radio-group>
          </el-form-item>
        </el-form>
      </el-header>
      <div style="height:400px;overflow:auto;">
        <!-- allowEdit: false
auditId: null
auditName: null
auditStatus: 0
auditTime: null
childList: null
companyId: 1
deleteFlag: false
id: 4
inventoryDate: "2019-08-14T00:00:00"
inventoryOrder: "string"
operatorId: 1
operatorName: "云飞"
warehouseId: 0
warehouseName: null
whAdminId: 0
        whAdminName: null-->

        <el-table
          :data="tableData"
          :height="380"
          :highlight-current-row="true"
          @row-dblclick="rowClick"
          v-loading="fullscreenLoading"
          element-loading-background="rgba(150, 150, 150, 0.6)"
        >
          <!-- <el-table-column type="index" label="序号" /> -->
          <el-table-column label="序号" width="70">
            <template slot-scope="scope">
              <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="warehousingDate" label="盘点日期">
            <template slot-scope="scope">{{scope.row.inventoryDate|formatTDate}}</template>
          </el-table-column>

          <el-table-column prop="inventoryOrder" label="盘点单号" width="150" />

          <el-table-column prop="auditStatus" label="审核状态">
            <template slot-scope="scope">
              <div v-if="scope.row.auditStatus==0">待审核</div>
              <div v-if="scope.row.auditStatus==1">未通过</div>
              <div v-if="scope.row.auditStatus==2">通过</div>
              <div v-if="scope.row.auditStatus==3">撤销审核</div>
            </template>
          </el-table-column>
          <el-table-column prop="operatorName" label="操作员姓名" />
          <!-- <el-table-column prop="receiptName" label="收货员" /> -->
          <el-table-column prop="auditName" label="审核员" />
          <el-table-column prop="auditTime" label="审核时间">
            <template slot-scope="scope">{{scope.row.auditTime|formatTDate}}</template>
          </el-table-column>

          <!-- <el-table-column prop="warehousingDate" label="盘点日期">
            <template slot-scope="scope">{{scope.row.warehousingDate|formatTDate}}</template>
          </el-table-column>
          <el-table-column prop="warehousingOrder" label="盘点单号" width="150" />
          <el-table-column prop="auditStatus" label="审核状态">
            <template slot-scope="scope">
              <div v-if="scope.row.auditStatus==0">待审核</div>
              <div v-if="scope.row.auditStatus==1">未通过</div>
              <div v-if="scope.row.auditStatus==2">通过</div>
              <div v-if="scope.row.auditStatus==3">撤销审核</div>
            </template>
          </el-table-column>
          <el-table-column prop="operatorName" label="账户姓名" />
          <el-table-column prop="receiptName" label="收货员" />
          <el-table-column prop="auditName" label="审核员" />
          <el-table-column prop="auditTime" label="审核时间">
            <template slot-scope="scope">{{scope.row.auditTime|formatTDate}}</template>
          </el-table-column>-->
        </el-table>
      </div>
      <Pagination
        :pageIndex="pageIndex"
        :totalCount="totalCount"
        :pageSize="pageSize"
        @pagination="pagination"
      />
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
import { formatDate,trim } from "@/utils/common.js";

export default {
  data() {
    return {
      dialogVisible: false,
      dtData: {
        warehousingOrder: "",
        warehousingDate: "",
        auditStatus: -1
      },
      fullscreenLoading: false,
      tableData: [],
      clickRow: {},
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1 // 页码
    };
  },
  props: {
    title: {
      default: "详情"
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
    this.getMainList();
  },
  methods: {
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
    rowClick(row) {
      //  if(row!=this.clickRow){
     
      //  }else{
         
      //  }
      this.clickRow = row;
        if (!this.clickRow.id) {
        this.$message.error("请选择数据");
        return;
      }
      this.close();
      this.$emit("OnBtnSaveSubmit", this.clickRow);


 
    
    },
    OnBtnSaveSubmit() {
      if (!this.clickRow.id) {
        this.$message.error("请选择数据");
        return;
      }
      this.close();
      this.$emit("OnBtnSaveSubmit", this.clickRow);
    },
    getMainList() {
      //角色列表
    
    
    var list=[]
    if(this.dtData.warehousingOrder){
      list.push({
          column: "inventoryOrder",
          content: trim(this.dtData.warehousingOrder),
          condition: 6
        })
    }
      // if (formatDate(this.dtData.warehousingDate) != "") {
      //   list.push({
      //     column: "warehousingDate",
      //     content: formatDate(this.dtData.warehousingDate),
      //     condition: 6
      //   });
      // }
      if (this.dtData.warehousingDate != null) {
        if (this.dtData.warehousingDate[0]) {
          var startDate = formatDate(this.dtData.warehousingDate[0]);
          var endDate = formatDate(this.dtData.warehousingDate[1]);
          list.push({
            column: "inventoryDate",
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
      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        list
      );
      this.fullscreenLoading = true;
      this.tableData = [];
      // request({
      //   url: "warehouse/api/TWMOtherWhMain/GetMainList",
      //   method: "GET",
      //   params: { RequestObject: JSON.stringify(rqs) }
      // }).then(res => {
      //   if (res.code === 0) {
      //     this.tableData = res.data;
      //     this.totalCount = res.totalNumber;
      //     this.fullscreenLoading = false;
      //   } else {
      //     this.$confirm(res.info, "错误信息", {
      //       confirmButtonText: "确定",
      //       type: "error",
      //       showCancelButton: false
      //     });
      //     this.fullscreenLoading = false;
      //   }
      // });


      request({
        url: "warehouse/api/TWMInventoryMain/GetMainList",
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
#InboundOrder /deep/ {
  .el-dialog--center .el-dialog__body {
    padding: 20px 20px 0px;
  }
  .el-table--small td {
    padding: 8px !important;
  }
}
</style>

