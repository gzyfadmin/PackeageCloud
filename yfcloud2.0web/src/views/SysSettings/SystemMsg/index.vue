<template>
  <el-container
    id="UserAuth"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="76px" inline class="FormInput">
        <el-form-item label="读取状态：">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainListIndex">
            <!-- <el-radio-button label="-1">全部</el-radio-button> -->
            <el-radio-button label="0">未读</el-radio-button>
            <el-radio-button label="1">已读</el-radio-button>
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
        <!-- <el-table-column type="expand">
          <template slot-scope="props">
            <el-form label-position="left" inline class="demo-table-expand">
              <el-form-item label="详情：">
                <span>{{ props.row.content }}</span>
              </el-form-item>
            </el-form>
          </template>
        </el-table-column>-->
        <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{(pageIndex-1)*pageSize+(scope.$index+1)}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="createDate" label="申请时间" width="100" sortable="custom">
          <template slot-scope="scope">{{scope.row.createDate|formatTDate}}</template>
        </el-table-column>
        <el-table-column prop="title" label="标题" width="180" />
        <el-table-column prop="content" label="详情">
          <template slot-scope="scope">
            <div class="ellipsis" :title="scope.row.content">{{scope.row.content}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="isRead" label="是否读取" width="80">
          <template slot-scope="scope">
            <div v-if="scope.row.isRead==0">未读</div>
            <div v-if="scope.row.isRead==1">已读</div>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.readmsg&&btnAip.readmsg.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.readmsg.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-check"
                circle
                :disabled="scope.row.isRead!=0"
                @click="postData(scope.row)"
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
  name: "viewsSysSettingsSystemMsgindexvue",
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
  },
  mounted() {
    this.tableFlag = true;
    this.setStyle();
    // this.pathData = getName("/HHHFKu", this.$store.getters.userPermission);
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
    postData(item) {
      this.fullscreenLoading = true;
      request({
        url: "/report/api/StaOnlineTime/Read",
        method: "put",
        params: { requestObject: item.toDoId }
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "操作成功",
            type: "success"
          });
          this.getMainList();
          this.$store.dispatch("user/getMsg");
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
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop
      this.sortOrder = currSort.order == 'ascending' ? 'asc' : 'desc'
      this.getMainList();
    },
    getMainList() {
      //获取列表
      var list = [];
      var orderConditions = [];
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        })
      }else {
        orderConditions = [
          {
            column: "createDate",
            condition: 'desc'
          }
        ]
      }
      if (this.dtData.auditStatus != -1) {
        list.push({
          column: "isRead",
          content: this.dtData.auditStatus,
          condition: 0
        });
      }
      var rqs = RequestObject.CreateGetObject(
        false,
        this.pageSize,
        this.pageIndex,
        list,
        orderConditions
      );
      this.fullscreenLoading = true;
      this.tableData = [];
      request({
        url: "/report/api/StaOnlineTime/GetToDoModel",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.tableData = res.data;
          // this.$store.dispatch("user/setMsgNum", res.data.length);
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
