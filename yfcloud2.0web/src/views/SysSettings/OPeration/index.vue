<template>
  <el-container v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-header id="elheader" class="headerBd" style="height:auto">
      <el-form ref="form" label-widht="80px" inline>
        <el-form-item label="菜单：" label-width="48px">
          <el-input
            popper-class="my-autocomplete"
            v-model="form.state"
            placeholder="请输入菜单"
            @keyup.enter.native="handleIconClick1"
            clearable
            @clear="handleIconClick1"
            class="zdyBtn_1"
          >
            <i class="el-icon-search el-input__icon" slot="suffix" @click="handleIconClick1"></i>
          </el-input>
        </el-form-item>
        <el-form-item label="账号：" label-width="48px">
          <el-input
            popper-class="my-autocomplete"
            v-model="form.account"
            placeholder="请输入账号"
            @keyup.enter.native="handleIconClick1"
            clearable
            @clear="handleIconClick1"
            class="zdyBtn_1"
          >
            <i class="el-icon-search el-input__icon" slot="suffix" @click="handleIconClick1"></i>
          </el-input>
        </el-form-item>
        <el-form-item label="姓名：" label-width="48px">
          <el-input
            popper-class="my-autocomplete"
            v-model="form.name"
            placeholder="请输入姓名"
            @keyup.enter.native="handleIconClick1"
            clearable
            @clear="handleIconClick1"
            class="zdyBtn_1"
          >
            <i class="el-icon-search el-input__icon" slot="suffix" @click="handleIconClick1"></i>
          </el-input>
        </el-form-item>
        <el-form-item label="操作日期：" label-width="76px">
          <el-date-picker
            clearable
            v-model="value1"
            type="daterange"
            range-separator="至"
            start-placeholder="开始操作日期"
            end-placeholder="结束操作日期"
            @change="clickDate"
            class="zdyBtn_2"
          ></el-date-picker>
        </el-form-item>
      </el-form>
      <el-button v-if="btnAip.query&&btnAip.query.buttonCaption" type="primary" @click="handleIconClick1">{{ btnAip.query.buttonCaption }}</el-button>
    </el-header>
    <el-main class="wmltable" id="elmain">
      <el-table
        :data="tableData"
        style="width: 100%"
        border
        :height="mainHeight"
      >
        <el-table-column prop="path" label="菜单" width="180"></el-table-column>
        <el-table-column prop="account" label="账号" width="180"></el-table-column>
        <el-table-column prop="operateDeatail" label="详情">
          <template slot-scope="scope">
            <div class="ellipsis" :title="scope.row.operateDeatail">{{scope.row.operateDeatail}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="ipAddress" label="IP" width="200"></el-table-column>
        <el-table-column prop="realName" label="姓名" width="100"></el-table-column>
        <el-table-column
          prop="createTime"
          label="操作时间"
          width="135"
          sortable
          :formatter="dateFormat"
        ></el-table-column>
        <el-table-column prop="operateType" label="操作类型" width="100">
          <template
            slot-scope="scope"
          >{{ scope.row.operateType == 1 ? '新增':'' || scope.row.operateType == 2 ? '修改':'' || scope.row.operateType == 3 ? '删除':'' }}</template>
        </el-table-column>
      </el-table>
    </el-main>
    <el-footer id="elfooter">
      <Pagination
        :pageIndex="pageIndex"
        :totalCount="totalCount"
        :pageSize="pageSize"
        @pagination="sonZhi"
      />
    </el-footer>
  </el-container>
</template>
<script>
import Pagination from "@/components/Pagination";
import request from "@/utils/request";
import rquestObject from "@/utils/requestObject";
import { getBtnCtr } from '@/utils/BtnControl'
import { getStyle } from "@/utils/Dom.js";
import { trim } from "@/utils/common.js";

export default {
  name: "viewsSysSettingslogManagementoPerationindexvue",
  data() {
    return {
      btnAip: '',
      value1: "",
      input: "",
      loading:false,
      pageSize: 10, // 分页显示记录条数
      totalCount: 10, // 总记录数
      pageIndex: 1,
      mainHeight: "500",
      form: {
        state: "",
        name: "",
        account: "",
        strTime: "",
        endTime: ""
      },
      tableData: [],
      tableDataHight: []
    };
  },

  components: {
    Pagination
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )
    this.initPeration();
    this.setStyle();
  },
  mounted() {
    
  },
  methods: {
    dateFormat(row, column, cellValue, index) {
      var date = row[column.property];

      var date1 = new Date(+new Date(date) + 8 * 3600 * 1000)
        .toISOString()
        .replace(/T/g, " ")
        .replace(/\.[\d]{3}Z/, "");
      return date1;
    },
    setStyle() {
      // 设置页面样式
      // var b = document.documentElement.clientHeight - 84;
      this.$nextTick(() => {
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        // this.mainHeight = b - h - f - 40;
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.mainHeight = b - h - f - parseInt(pt) - parseInt(pb);
      });
    },
    setTimes(data) {
      let time = data; // timeTag为时间戳
      let newTime =
        time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate();
      return newTime;
    },
    clickDate() {
      if (this.value1 == null) {
        this.initPeration();
      } else {
        this.loading = true;
        var strTime = this.setTimes(this.value1[0]);
        var endTime = this.setTimes(this.value1[1]);
        this.form.strTime = strTime;
        this.form.endTime = endTime;
        var postData = {
          path: this.form.state,
          account: this.form.account,
          realName: this.form.name,
          createTimeBg: this.form.strTime,
          createTimeEd: this.form.endTime
        };
        let rquestO = rquestObject.CreateGetObjectFenye(
          true,
          this.pageSize,
          this.pageIndex,
          postData
        );
        request({
          url: "logstatus/api/Log",
          method: "GET",
          params: {
            requestObject: rquestO
          }
        }).then(res => {
          this.tableData = res.data;
          this.loading = false;
        });
      }
    },
    sonZhi(data) {
      this.pageSize = data.pageSize;
      this.pageIndex = data.pageIndex;
      var postdata = {
        path: this.form.state,
        account: this.form.account,
        realName: this.form.name,
        createTimeBg: this.form.strTime,
        createTimeEd: this.form.endTime
      };

      var requestObje = {
        isPaging: true,
        pageSize: data.pageSize,
        pageIndex: data.pageIndex,
        postData: postdata,
        postDataList: null,
        queryConditions: null,
        orderByConditions: null
      };
      request({
        url: "logstatus/api/Log",
        method: "GET",
        params: {
          requestObject: requestObje
        }
      }).then(res => {
        this.tableData = res.data;
      });
    },
    initPeration() {
      this.loading = true;
      var rquestO = rquestObject.CreateGetObject(true);
      request({
        url: "logstatus/api/Log",
        method: "GET",
        params: {
          requestObject: rquestO
        }
      }).then(res => {
        this.tableData = res.data;
        this.totalCount = res.totalNumber;
        this.pageIndex = res.pageIndex;
        this.pageSize = res.pageSize;
        this.loading = false;
      });
    },
    SelectFrom() {
      this.handleIconClick();
    },
    SelectFrom1() {
      this.handleIconClick1();
    },
    SelectFrom2() {
      this.handleIconClick2();
    },
    handleIconClick() {
      // 账号
      var postdata = {
        path: this.form.state,
        account: this.form.account,
        realName: this.form.name,
        createTimeBg: this.form.strTime,
        createTimeEd: this.form.endTime
      };
      let rquestO = rquestObject.CreateGetObjectFenye(
        true,
        this.pageSize,
        this.pageIndex,
        postdata
      );
      request({
        url: "logstatus/api/Log",
        method: "GET",
        params: {
          requestObject: rquestO
        }
      }).then(res => {
        this.tableData = res.data;
        this.totalCount = res.totalNumber;
        this.pageIndex = res.pageIndex;
        this.pageSize = res.pageSize;
      });
    },
    handleIconClick2() {
      var postdata = {
        path: this.form.state,
        account: this.form.account,
        realName: this.form.name,
        createTimeBg: this.form.strTime,
        createTimeEd: this.form.endTime
      };
      let rquestO = rquestObject.CreateGetObjectFenye(
        true,
        this.pageSize,
        this.pageIndex,
        postdata
      );
      request({
        url: "logstatus/api/Log",
        method: "GET",
        params: {
          requestObject: rquestO
        }
      }).then(res => {
        this.tableData = res.data;
        this.totalCount = res.totalNumber;
        this.pageIndex = res.pageIndex;
        this.pageSize = res.pageSize;
      });
    },
    handleIconClick1() {
      this.loading = true;
      var postdata = {
        path: trim(this.form.state),
        account: trim(this.form.account),
        realName: trim(this.form.name),
        createTimeBg: this.form.strTime,
        createTimeEd: this.form.endTime
      };
      let rquestO = rquestObject.CreateGetObjectFenye(
        true,
        this.pageSize,
        this.pageIndex,
        postdata
      );
      request({
        url: "logstatus/api/Log",
        method: "GET",
        params: {
          requestObject: rquestO
        }
      }).then(res => {
        this.tableData = res.data;
        this.totalCount = res.totalNumber;
        this.pageIndex = res.pageIndex;
        this.pageSize = res.pageSize;
        this.loading = false;
      });
    },
    tableRowClassName({ row, rowIndex }) {
      if (rowIndex === 1) {
        return "warning-row";
      } else if (rowIndex === 3) {
        return "success-row";
      }
      return "";
    },
    setTableHead({ row, rowIndex }) {
      if (rowIndex == 0) {
        return "success-row";
      }
      return "";
    },
    currentChange(val) {
      this.pageIndex = val;
      //分页改变页数
      var QueryCondition = [
        {
          column: "deptid",
          content: this.clickNodes,
          condition: 0
        }
      ];
      var requsets = rquestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        QueryCondition,
        null
      );

      request({
        url: "system/api/TSMDept/GetDispathUserAsync",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        this.tableData = res.data;
      });
    }
  }
};
</script>

<style lang="scss" scoped>
/deep/ .el-button--small {
  padding: 8px 15px;
}
.hederTableCs {
  background: yellow;
}
// .el-table .warning-row {
//   background: oldlace;
// }

// .el-table .success-row {
//   background-color: #f0f9eb;
// }
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
//  /deep/ .el-table--small th {
//     padding: 2px 0;
// }
</style>

