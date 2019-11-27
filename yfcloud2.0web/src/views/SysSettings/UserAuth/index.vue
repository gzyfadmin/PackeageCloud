<template>
  <el-container
    id="UserAuth"
    v-loading="fullscreenLoading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="76px" inline class="FormInput">
        <el-form-item label="申请账号：" prop="keyWords">
          <el-input
            v-model="dtData.keyWords"
            placeholder="请输入手机/邮箱"
            clearable
            style="width:200px"
            @keyup.enter.native="getMainListIndex"
            @clear="getMainListIndex"
          >
            <i slot="suffix" class="el-input__icon el-icon-search" @click="getMainListIndex"></i>
          </el-input>
        </el-form-item>
        <el-form-item label="申请时间：">
          <el-date-picker
            style="width:400px"
            v-model="dtData.warehousingDate"
            type="daterange"
            align="left"
            unlink-panels
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            @change="getMainListIndex"
          />
        </el-form-item>
        <el-form-item label="申请状态：">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainListIndex">
            <!-- <el-radio-button label="-1">全部</el-radio-button> -->
            <el-radio-button label="0">未审核</el-radio-button>
            <el-radio-button label="1">已通过</el-radio-button>
            <el-radio-button label="2">已拒绝</el-radio-button>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <el-button
        type="primary"
        @click="getMainListIndex"
        v-if="btnAip.query&&btnAip.query.buttonCaption"
      >{{ btnAip.query.buttonCaption }}</el-button>
    </el-header>
    <el-main id="elmain">
      <el-table
        v-if="tableFlag"
        ref="table"
        :height="tableHeight"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        @sort-change="handelSortChange"
      >
        <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="applyTime" label="申请时间" sortable="custom">
          <template slot-scope="scope">{{scope.row.applyTime|formatTDate}}</template>
        </el-table-column>
        <el-table-column prop="accountName" label="用户" />
        <el-table-column prop="telAccount" label="手机" />
        <el-table-column prop="emailAccount" label="邮箱" />
        <el-table-column prop="companyName" label="公司" />

        <el-table-column prop="applyStatus" label="申请状态" width="200%">
          <template slot-scope="scope">
            <div v-if="scope.row.applyStatus==0">未审核</div>
            <div v-if="scope.row.applyStatus==1">已审核</div>
            <div v-if="scope.row.applyStatus==2">已拒绝</div>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.agree&&btnAip.agree.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.agree.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-check"
                circle
                :disabled="scope.row.applyStatus!=0"
                @click="postData(scope.row,1)"
              />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.refuse&&btnAip.refuse.buttonCaption&&showtips"
              class="item"
              effect="light"
              :content="btnAip.refuse.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="danger"
                icon="el-icon-close"
                circle
                :disabled="scope.row.applyStatus!=0"
                @click="postData(scope.row,2)"
              />
            </el-tooltip>
            <el-button
              v-if="btnAip.refuse&&btnAip.refuse.buttonCaption&&!showtips"
              type="danger"
              icon="el-icon-close"
              circle
              :disabled="scope.row.applyStatus!=0"
              @click="postData(scope.row,2)"
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
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, trim } from "@/utils/common.js";
import { getStyle } from "@/utils/Dom.js";

import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";

// import { mapGetters } from "vuex";

export default {
  name: "viewsSysSettingsUserAuthindexvue",
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
      fullscreenLoading: false,
      showtips: true,
      tableFlag: true,
      btnAip: "",
      pathData: "", //路由信息
      pathDataView: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      sortColumn: '',
      sortOrder: '',
      tableData: [], // table数据模型
      dtData: {
        keyWords: "",
        warehousingType: -1,
        warehousingDate: "",
        auditStatus: 0
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
  mounted() {
    this.tableFlag = true;
    this.setStyle();
    this.pathData = getName("/HHHFKu", this.$store.getters.userPermission);
    // this.pathDataView = getName("/HHHFXu", this.$store.getters.userPermission);
  },
  activated() {
    this.tableFlag = false;
    setTimeout(() => {
      this.tableFlag = true;
    }, 10);
  },
  methods: {
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
    getMainListIndex() {
      this.pageIndex = 1;
      this.getMainList();
    },
    postData(item, state) {
      if(state==1){
        var text = "确定通过吗?"
      }
      if(state==2){
        var text = "确定拒绝吗?"
      }
      this.$confirm(text, {
        type: "warning"
      })
        .then(() => {
          var currData = {
            id: item.id,
            applyStatus: state
          };
          var currData2 = {
            id: item.id,
            applyStatus: 0
          };
          var data = RequestObject.CreatePostObject(currData, null, currData2);
          var method = "put";
          this.fullscreenLoading = true;
          request({
            url: "/system/api/TSMCompanyApply",
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
        })
        .catch(() => {});
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop
      this.sortOrder = currSort.order == 'ascending' ? 'asc' : 'desc'
      this.getMainList();
    },
    getMainList() {
      //角色列表
      var list = [];
      var orderConditions = [];
      // if (this.dtData.warehousingType !== -1) {
      //   list.push({
      //     column: "warehousingType",
      //     content: this.dtData.warehousingType,
      //     condition: 6
      //   });
      // }
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        })
      }else {
        orderConditions = [
          {
            column: "applyTime",
            condition: 'desc'
          }
        ]
      }
      if (this.dtData.warehousingDate != null) {
        if (this.dtData.warehousingDate[0]) {
          var startDate = formatDate(this.dtData.warehousingDate[0]);
          var endDate = formatDate(this.dtData.warehousingDate[1]);
          list.push({
            column: "applyTime",
            content: this.dtData.warehousingDate[0]
              ? startDate + "," + endDate
              : "",
            condition: 10
          });
        }
      }
      if (this.dtData.auditStatus != -1) {
        list.push({
          column: "applyStatus",
          content: this.dtData.auditStatus,
          condition: 0
        });
      }
      if (this.dtData.keyWords != "") {
        list.push({
          column: "telAccount",
          content: trim(this.dtData.keyWords),
          condition: 6
        });
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
        url: "system/api/TSMCompanyApply",
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
#UserAuth /deep/ {
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
