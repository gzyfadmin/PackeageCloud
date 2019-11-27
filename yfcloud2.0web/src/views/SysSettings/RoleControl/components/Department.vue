<template>
  <div id="RoleControl">
    <div class="clr">
      <el-aside width="240px" class="tableTab" :style="{height:tableHeight+'px'}">
        <div class="roleTable roleHeader">部门</div>
        <el-tree
          ref="tree"
          :data="roleData"
          default-expand-all
          :expand-on-click-node="false"
          node-key="id"
          @node-click="nodeClick"
        >
          <span slot-scope="{ node, data }" class="custom-tree-node pr">
            <span :ref="'tree-'+data.vitualId" :id="'tree-'+data.vitualId">{{ data.deptName }}</span>
          </span>
        </el-tree>
      </el-aside>
      <div class="tableBox">
        <div class="title" v-if="treeClickItem.deptName">{{treeClickItem.deptName}}</div>
        <div class="title" v-if="!treeClickItem.deptName">部门</div>
        <el-table
          ref="table"
          :data="tableData"
          tooltip-effect="dark"
          style="width: 100%"
          :height="tableHeight-26"
          @select="handleSelectionChange"
          @select-all="handleSelectionAllChange"
        >
          <el-table-column type="selection" width="55" />

          <!-- <el-table-column prop="accountName" label="全选" class="ellipsis">
            <template slot="header" slot-scope="scope">
              <el-checkbox @change="handelInnetTableSelectAll" v-model="selectAll"></el-checkbox>
            </template>
             <template slot-scope="scope">
                <el-checkbox @change="SelectionChange(scope.row)" v-model="scope.row.selected"></el-checkbox>
            </template>
          </el-table-column>-->
          <el-table-column prop="accountName" label="账号姓名" width="120" />
          <el-table-column prop="realName" label="真实姓名" />
          <el-table-column prop="deptName" label="部门" width="90" />
          <el-table-column prop="jobNumber" label="工号" />
          <el-table-column prop="telAccount" label="手机账号" width="100" />
          <el-table-column prop="emailAccount" label="邮箱账号" width="200" />
          <el-table-column prop="fixedNumber" label="固定电话" />
          <el-table-column prop="status" label="账号状态" width="80">
            <template slot-scope="scope">
              <div v-if="scope.row.status==0">无效</div>
              <div v-if="scope.row.status==1">有效</div>
              <div v-if="scope.row.status==2">过期</div>
            </template>
          </el-table-column>
          <el-table-column prop="expDate" label="有效期" width="90">
            <template slot-scope="scope">
              <div>{{ scope.row.expDate |formatTDate }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="address" label="联系地址" width="250">
            <template slot-scope="scope">
              <div class="ellipsis">{{scope.row.address}}</div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </div>
  </div>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import Dialog from "@/components/Dialog";
import { closest } from "@/utils/Dom";
import { ArrObj_unique } from "@/utils/dateUtil";

export default {
  data() {
    return {
      tableHeight: "500",
      tableData: [], // table数据模型
      roleData: [],
      treeClickItem: {},
      addData: [], //提交的数据
      selectAll: false, //是否全选
      selectList: [],
      forFlag: true
    };
  },
  watch: {
    selectAll(val) {}
  },
  filters: {
    formatTDate: value => {
      if (value != null) {
        var d = value.split("T");
      }
      return d ? d[0] : value;
    }
  },
  components: {},
  created() {
    // this.mainHeight =
    //   (height -
    //     126 -
    //     parseInt(this.headerHeight) -
    //     parseInt(this.footerHeight)).toString() +
    //   'px'
    this.getTree();
  },
  mounted() {
    // this.setStyle();
  },
  methods: {
    handleSelectionChange(rows) {
      //单选
      this.selectList = rows;
      this.tableData.map((item, index) => {
        rows.map(val => {
          if (item.id == val.id) {
            // this.tableData.unshift(this.tableData.splice(index, 1)[0]);
          }
        });
      });
    },
    handleSelectionAllChange(rows) {
      //全选
      this.selectList = rows;
    },
    getTree() {
      //b部门列表
      var rqs = RequestObject.CreateGetObject();
      request({
        url: "system/api/TSMDept/GetTree",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.roleData = res.data;
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    // handelInnetTableSelectAll(rows) {
    //   // 全选
    //   if(this.selectAll){
    //     this.tableData.map((item)=>{
    //       this.$set(item,"selected",true)
    //     })
    //     this.selectList=this.tableData;
    //   }else{
    //     this.tableData.map((item)=>{
    //       this.$set(item,"selected",false)
    //     })
    //   }
    // },
    // SelectionChange(val) {
    //   // 单选
    //   if(!this.forFlag){
    //     console.log(val.selected)
    //   }
    //   this.selectList=[]
    //   this.tableData.map((item)=>{
    //     if(item.selected==true){
    //       this.selectList.push(item)
    //     }
    //   })
    //   if(this.selectList.length==this.tableData.length){
    //     this.selectAll = true;
    //   }else{
    //     this.selectAll = false;
    //   }

    //   // this.tableData.map((item,index)=>{
    //   //   this.selectList.map((val)=>{
    //   //     if(item.id==val.id){
    //   //        this.tableData.unshift(this.tableData.splice(index,1)[0])
    //   //        this.forFlag = false;
    //   //     }
    //   //   })
    //   // })

    // },
    defaultdata(dt) {
      for (var i = 0; i < dt.length; i++) {
        dt[i].selected = false; // 按钮数组
      }
      return dt;
    },
    nodeClick(item) {
      //点击菜单
      if (item.actualId == null) {
        // this.tableData = [];
        return;
      }
      this.treeClickItem = item;
      var a = document.querySelectorAll("div.el-tree-node__content");
      a.forEach(item => {
        item.style.background = "#FFF";
      }); //重置所有按钮样式
      closest(
        this.$refs["tree-" + item.vitualId],
        ".el-tree-node__content"
      ).style.background = "#F5F7FA"; //点击按钮样式
      this.getUserList();
      // console.log(item.actualId);
    },
    getUserList() {
      // console.log()
      // 获取列表数据
      var param = [
        { column: "deptid", content: this.treeClickItem.actualId, condition: 0 }
      ];
      var robject = RequestObject.CreateGetObject(false, 0, 0, param);

      request({
        url: "/system/api/TSMDept/GetDispathUserAsync",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
        if (res.code === 0) {
          this.tableData = this.defaultdata(res.data);
          // console.log(this.tableData)
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    }
  }
};
</script>
<style lang="scss" scoped>
</style>
