<template>
  <el-container
    id="OtherPut"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(1, 1, 1, 0)"
  >
    <el-header id="elheader" class="header" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="90px" inline class="FormInput">
        <el-form-item label="物料名称：" prop="materialname">
          <el-input v-model="dtData.materialname"></el-input>
        </el-form-item>
        <el-button type="primary" @click="getMainList(dialogid,pageIndex=1)">查询</el-button>
      </el-form>
    </el-header>
    <el-main>
      <el-table
        ref="table"
        :data="tableData"
        style="width: 99.9%;"
        row-key="id"
        border
        :show-summary="true"
        :summary-method="getSummaries"
        :header-cell-style="{background:'rgb(250, 250, 250)',color:'#333'}"
        :max-height="300"
      >
        <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
          </template>
        </el-table-column>
        <el-table-column label="主表id">
          <template slot-scope="scope">
            <div>{{scope.row.mainId}}</div>
          </template>
        </el-table-column>

        <el-table-column label="物料名称">
          <template slot-scope="scope">
            <div>{{scope.row.materialName}}</div>
          </template>
        </el-table-column>

        <el-table-column label="盘点仓库">
          <template slot-scope="scope">
            <div>{{scope.row.warehouseName}}</div>
          </template>
        </el-table-column>

        <el-table-column label="单位">
          <template slot-scope="scope">
            <div>{{scope.row.warehouseUnitName}}</div>
          </template>
        </el-table-column>

        <el-table-column label="账存数量">
          <template slot-scope="scope">
            <div>{{scope.row.accountNum}}</div>
          </template>
        </el-table-column>

        <el-table-column label="实存数量">
          <template slot-scope="scope">
            <div>{{scope.row.actualNum}}</div>
          </template>
        </el-table-column>

    

        <!-- profitUnitNum -->

        <el-table-column label="盘盈数量">
          <template slot-scope="scope">
            <div>{{scope.row.profitNum}}</div>
          </template>
        </el-table-column>
             <el-table-column label="盘亏数量">
          <template slot-scope="scope">
            <div>{{scope.row.deficitNum}}</div>
          </template>
        </el-table-column>


<el-table-column label="基本单位账存数量" width="120px">
          <template slot-scope="scope">
            <div>{{scope.row.accountUnitNum}}</div>
          </template>
        </el-table-column>
<el-table-column label="基本单位实存数量" width="120px">
          <template slot-scope="scope">
            <div>{{scope.row.actualUnitNum}}</div>
          </template>
        </el-table-column>
    <el-table-column label="基本单位盘盈数量" width="120px">
          <template slot-scope="scope">
            <div>{{scope.row.profitUnitNum}}</div>
          </template>
        </el-table-column>
        <el-table-column label="基本单位盘亏数量" width="120px">
          <template slot-scope="scope">
            <div>{{scope.row.deficitUnitNum}}</div>
          </template>
        </el-table-column>

   
        <el-table-column label="备注">
          <template slot-scope="scope">
            <div>{{scope.row.remark}}</div>
          </template>
        </el-table-column>

        <el-table-column label="颜色">
          <template slot-scope="scope">
            <div>{{scope.row.colorName}}</div>
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <el-footer id="elfooter">
      <!-- layout="total, prev, pager, next, jumper" -->
      <Pagination
        layout="total, prev, pager, next, jumper"
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
import { formatDate,trim } from "@/utils/common.js";

import Pagination from "@/components/Pagination";
import { getBtnCtr, getName } from "@/utils/BtnControl";

// import { mapGetters } from "vuex";

export default {
  props: ["dialogid"],
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
      showdetialdialog: false,
      btnAip: "",
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      dtData: {
        materialname: "",
        warehousingDate: "",
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
    this.getMainList(this.dialogid); // 获取table数据
    // this.getWarehouseData(); //仓库数据
  },

  mounted() {
    // this.setStyle();
    this.pathData = getName("/HHHFXz", this.$store.getters.userPermission);
  },
  methods: {
    showdetial() {
      this.showdetialdialog = true;
    },

    getSummaries(param) {
      //table总计
      const { columns, data } = param;
      const sums = [];
      sums[0] = "总计";
      //账存数量
      const values0 = data.map(item => {
        return Number(item.accountNum);
      });
      sums[5] = values0.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return prev + curr;
        } else {
          return prev;
        }
      }, 0);

      const values1 = data.map(item => {
        return Number(item.actualNum);
      });
      sums[6] = values1.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return prev + curr;
        } else {
          return prev;
        }
      }, 0);

      const values2 = data.map(item => {
        return Number(item.profitNum);
      });
      sums[7] = values2.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return prev + curr;
        } else {
          return prev;
        }
      }, 0);

      const values3 = data.map(item => {
        return Number(item.deficitNum);
      });
      sums[8] = values3.reduce((prev, curr) => {
        const value = Number(curr);
        if (!isNaN(value)) {
          return prev + curr;
        } else {
          return prev;
        }
      }, 0);

      return sums;
    },
    goPage(item, type) {
      this.$router.push({
        name: this.pathData.name,
        params: {
          item,
          type
        }
      });
    },
    
  
    // setStyle() {
    //   // 设置页面样式
    //   this.$nextTick(() => {
    //     var navbar = document.getElementById("navbar_yfkj");
    //     var nv = navbar.clientHeight || navbar.offsetHeight;
    //     var b = document.documentElement.clientHeight - nv;
    //     var elheader = document.getElementById("elheader");
    //     var elfooter = document.getElementById("elfooter");
    //     var h = elheader.clientHeight || elheader.offsetHeight;
    //     var f = elfooter.clientHeight || elfooter.offsetHeight;
    //     this.tableHeight = b - h - f - 40;
    //   });
    // },
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMainList(this.dialogid);
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
      //删除
      var currData = {
        id: item.id,
        _LogName: `删除ID：${item.id} 的入库单`
      };
      var data = RequestObject.CreatePostObject(currData);

      this.$confirm("是否删除入库单", "确认信息", {
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
            message: "保存数据成功",
            type: "success"
          });
          this.getMainList(this.dialogid);
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

    getMainList(e) {
      //列表
      var list = [];
      this.fullscreenLoading = true;
      this.tableData = [];
      // this.dialogid
      request({
        url: "warehouse/api/TWMInventoryMain/GetDetailList?id=" + e,
        method: "GET"
      }).then(res => {

        if (res.code === 0) {
          console.log("res")
console.log(res)
          //先过滤后分页
          var filterdata = []; //过滤
          for (var i = 0; i < res.data.length; i++) {
            if (
              res.data[i].materialName == this.dtData.materialname ||
              this.dtData.materialname == ""
            ) {
              filterdata.push(res.data[i]);
            }
          }


          var result = []; //分页
          for (var i = 0; i < filterdata.length; i += this.pageSize) {
            result.push(filterdata.slice(i, i + this.pageSize));
          }

          this.tableData = result[this.pageIndex - 1];
          this.totalCount = filterdata.length;
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
    padding-top: 10px;
  }
  .link {
    color: #1890ff;
    cursor: pointer;
  }
}
.detialbtn {
  color: #1890ff;
  cursor: pointer;
}
</style>
