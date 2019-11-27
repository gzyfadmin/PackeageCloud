<template>
  <el-container id="SalesordersList"  >
    <!-- <el-header id="elheader" class="header" height="auto" >
      <el-form :label-position="labelPosition" label-width="90px" inline class="FormInput">
        <el-form-item label="客户名称：">
          <el-input
            v-model="dtData.customerName"
            placeholder="客户名称"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
            <i v-if="!dtData.customerName"  @click="getMainList(pageIndex=1)" style=" cursor: pointer;" slot="suffix" class="el-input__icon el-icon-search" />
        </el-input>
        </el-form-item>

        <el-form-item class="formItem" label="订单类型：" prop="orderTypeId">
          <el-select
            v-model="dtData.orderTypeId"
            placeholder="请选择"
            @change="getMainList(pageIndex=1)"
          >
            <el-option
              v-for="item in orderTypeoption"
              :key="item.id"
              :label="item.dicValue"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="联系人：">
          <el-input
            v-model="dtData.contactName"
            placeholder="联系人"
            clearable
            @keyup.enter.native="getMainList(pageIndex=1)"
            @clear="getMainList(pageIndex=1)"
          >
           <i v-if="!dtData.contactName"  @click="getMainList(pageIndex=1)" style=" cursor: pointer;" slot="suffix" class="el-input__icon el-icon-search" />
        </el-input>
        </el-form-item>
        <el-form-item label="审核状态：">
          <el-radio-group v-model="dtData.auditStatus" @change="getMainList(pageIndex=1)">
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
    </el-header> -->

    <el-main  v-if="showtab" v-loading="fullscreenLoading">
      <el-table
        @row-dblclick="dbtoproduct" 
      align="center"
        :header-cell-style="{'text-align':'center'}"
        ref="table"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
        
        :height="tableHeight"
        :summary-method="getSummaries"
        show-summary
      >
  
 <el-table-column label="序号" width="70">
          <template slot-scope="scope">
            <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
          </template>
        </el-table-column>
        <el-table-column label="订单日期" width="150px">
          <template slot-scope="scope">
            <div>{{scope.row.orderDate |formatTDate}}</div>
          </template>
        </el-table-column>
        <el-table-column label="销售订单号" width="150px">
          <template slot-scope="scope">
             <!-- @click="goPage(scope.row,1)"  -->
             <!-- class="link" -->
            <div >{{scope.row.orderNo}}</div>
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

        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
              <el-link style="color:skyblue" @click="toproduct(scope.row.orderNo,scope.row.id)" >
                选择该订单
              </el-link>
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

    <!-- <el-dialog :visible.sync="showdetialdialog" width="60%">
      <newdialog :dialogid="dialogid" v-if="showdetialdialog" />
    </el-dialog> -->
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
  // name: "SalesordersList",

  components: {
    Pagination,

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
      showtab:true,
      showtips:false,
      dialogid: null,
      showdetialdialog: false,
      btnAip: "",
      pathData: "", //路由信息
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      orderTypeoption: [],

      dtData: {
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
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.getMainList(); // 获取table数据
    
  },
  async activated() {
   this.showtab=false
    setTimeout(() => {
      this.showtab=true
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
     toproduct(code,id){
       
        this.$emit("salescode",[code,id])
    },
         dbtoproduct(row){
          //  scope.row.orderNo,scope.row.id
          //  console.log(row)
        this.$emit("salescode",[row.orderNo,row.id])
    },
      getSummaries(param) {
      //table总计
      const { columns, data } = param;
      const sums = [];

      const values0 = data.map(item => {
     var accountNum =0
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
        orderByConditions: null
      };

      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject:para }
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
          this.showtips=true
        })
        .catch(() => {
          this.showtips=true
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

    getMainList() {
      //角色列表
      var list = [];
      if (this.dtData.orderTypeId != -1) {
        list.push({
          column: "orderTypeId",
          content: this.dtData.orderTypeId,
          condition: 6
        });
      }
      if (this.dtData.customerName != "") {
        list.push({
          column: "customerName",
          content: this.dtData.customerName,
          condition: 6
        });
      }
      if (this.dtData.contactName != "") {
        list.push({
          column: "contactName",
          content: this.dtData.contactName,
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

      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        list
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
    }
  }
};
</script>

<style lang="scss" scoped>
#SalesordersList /deep/ {
  .cell{
    text-align: center;
  }
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
