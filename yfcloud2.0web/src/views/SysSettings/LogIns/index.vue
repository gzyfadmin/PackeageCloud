<template>
  <el-container v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-header id="elheader" class="header headerBd" style="height:auto">
      <el-form inline label-width="76px" label-position="left">
        <el-form-item label="姓名：" label-width="48px">
          <el-input
            v-model="nameSearch"
            placeholder="请输入姓名"
            clearable
            @keyup.enter.native="getMakeReport"
            @clear="getMakeReport"
            class="zdyBtn_1"
          >
            <i slot="suffix" class="el-icon-search" @click="getMakeReport()" />
          </el-input>
        </el-form-item>
        <el-form-item label="账号：" label-width="48px">
          <el-input
            v-model="accountSearch"
            placeholder="请输入账号"
            clearable
            @keyup.enter.native="getMakeReport"
            @clear="getMakeReport"
            class="zdyBtn_1"
          >
            <i slot="suffix" class="el-icon-search" @click="getMakeReport()" />
          </el-input>
        </el-form-item>
        <el-form-item label="登录日期：">
          <div class="block">
            <el-date-picker
              clearable
              v-model="datePicker"
              type="daterange"
              range-separator="至"
              start-placeholder="登录开始日期"
              end-placeholder="登录结束日期"
              @change="getMakeReport"
              class="zdyBtn_2"
            />
          </div>
        </el-form-item>
      </el-form>
      <el-button
        v-if="btnAip.query&&btnAip.query.buttonCaption"
        type="primary"
        @click="getMakeReport()"
      >{{ btnAip.query.buttonCaption }}</el-button>
    </el-header>
    <el-main id="elmain" class="wmltable">
      <el-table :data="loginData" style="width: 100%" border :height="mainHeight">
        <el-table-column
          prop="loginTime"
          sortable
          :formatter="dateFormat"
          label="登录时间"
          width="180"
        />
        <el-table-column prop="account" label="账号" />
        <el-table-column prop="ipAddress" label="ip" />
        <el-table-column label="登录描述">
          <template slot-scope="scope">{{ ['登录成功','登录失败','登出'][scope.row.description] }}</template>
        </el-table-column>
        <el-table-column prop="realName" label="姓名" />
        <!-- <el-table-column
          prop="name"
          label="角色"
        />-->
        <el-table-column label="状态">
          <template slot-scope="scope">{{ scope.row.status==0? '在线':'离线' }}</template>
        </el-table-column>
        <el-table-column prop="timeSpan" label="在线时长" />
      </el-table>
    </el-main>
    <el-footer id="elfooter" :height="footHeight">
      <Pagination
        :page-index="pageIndex"
        :total-count="total"
        :page-size="pageSize"
        @pagination="pagination"
      />
      <!-- <Pagination :total="total" @pagination="pagination" /> -->
    </el-footer>
  </el-container>
</template>
<script>
import Pagination from "@/components/Pagination";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import DateUtil from "@/utils/dateUtil";
// import height from "@/utils/height";
import { getBtnCtr } from "@/utils/BtnControl";
import { getStyle } from "@/utils/Dom.js";
import { trim } from "@/utils/common.js";

export default {
  name: "viewsSysSettingslogManagementlogInsindexvue",
  components: {
    Pagination
  },
  data() {
    return {
      loading: true,
      btnAip: "",
      total: 0,
      headHeight: "50px",
      footHeight: "50px",
      mainHeight: 500,
      datePicker: "",
      nameSearch: "",
      accountSearch: "",
      pageSize: 25,
      pageIndex: 1,
      loginData: [],
      queryData: {
        id: "",
        loginTimeBg: "",
        loginTimeEd: "",
        account: "",
        ipAddress: "",
        description: "",
        realName: "",
        status: ""
      }
    };
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    // 高度
    // this.mainHeight =
    //   height -
    //   146 -
    //   parseInt(this.headHeight) -
    //   parseInt(this.footHeight).toString() +
    //   "px";
    // // 键盘返回键
    // var lett = this
    // document.onkeydown = function(e) {
    //   var key = window.event.keyCode
    //   if (key == 13) {
    //     lett.getMakeReport()
    //   }
    // }
    this.getLonginData();
    this.setStyle();
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
        this.mainHeight = b - h - f - parseInt(pt) - parseInt(pb);
      });
    },
    /**
     * dateFormat
     * 时间转换
     */
    dateFormat(row, column, cellValue, index) {
      var date = row[column.property];

      var date1 = new Date(+new Date(date) + 8 * 3600 * 1000)
        .toISOString()
        .replace(/T/g, " ")
        .replace(/\.[\d]{3}Z/, "");
      return date1;
    },
    /**
     * getLonginData
     * 登录信息
     */
    getLonginData() {
      var reqObject;
      if (this.datePicker && this.datePicker.length) {
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          this.queryData,
          null
        );
      } else {
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          null
        );
      }
      request({
        url: "/logstatus/api/Log/GetLoginLog",
        method: "get",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        this.loading = false;
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.loginData = res.data;
          this.total = res.totalNumber;
        }
      });
    },
    /**
     * getMakeReport
     * 搜索
     */
    getMakeReport() {
      this.loading = true;
      // 日期查询
      if (this.datePicker && this.datePicker.length > 0) {
        this.queryData.loginTimeBg = DateUtil.Format(
          this.datePicker[0],
          "yyyy-MM-dd"
        );
        this.queryData.loginTimeEd = DateUtil.Format(
          this.datePicker[1],
          "yyyy-MM-dd"
        );
      }
      // 条件查询（姓名，账号）
      if(trim(this.accountSearch)!=""){
        this.queryData.account = this.accountSearch;
      }
      if(trim(this.nameSearch)!="") {
        this.queryData.realName = this.nameSearch;
      }
      this.getLonginData();
    },
    /**
     * pagination
     * 分页信息
     */
    pagination(val) {
      this.loading = true;
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getLonginData();
    }
  }
};
</script>
<style lang="scss" scoped>
.el-header /deep/ {
  width: 100%;
  padding-top: 20px;
  .el-form {
    width: 100%;
    border-bottom: 1px solid #eee;
    // display: flex;
    margin-bottom: 20px;
    //  flex-direction: row !important;
    .el-form-item__content {
      // flex: 1;
      // padding-right: 60px;
    }
  }
}
// .has-gutter {
// /deep/ .el-table--small th {
//   padding: 2px 0;
// }
// }
</style>
