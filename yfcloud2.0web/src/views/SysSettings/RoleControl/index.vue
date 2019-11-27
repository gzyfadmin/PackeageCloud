<template>
  <el-container id="RoleControl" v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-main class="clr">
      <el-aside width="240px" class="tableTab" :style="{height:tableHeight+'px'}">
        <!-- <el-input v-model="filterText" placeholder="输入关键字进行过滤" suffix-icon="el-icon-search" /> -->
        <div class="roleTable roleHeader">所有角色</div>
        <el-tree
          ref="tree"
          class="filter-tree"
          :data="roleData"
          node-key="id"
          default-expand-all
          :expand-on-click-node="false"
          @node-click="nodeClick"
        >
          <!-- :filter-node-method="filterNode" -->
          <!-- :accordion="accordion" -->
          <!-- @node-drag-end="handleDragEnd" -->
          <!-- :props="defaultProps" -->
          <span slot-scope="{ node, data }" class="custom-tree-node pr">
            <span :id="'tree-'+data.id" :ref="'tree-'+data.id">{{ data.roleName }}</span>
          </span>
        </el-tree>
      </el-aside>
      <div class="tableBox">
        <div class="headerBox">
          <div v-if="treeClickItem.roleName" class="title">角色:{{ treeClickItem.roleName }}</div>
          <div v-if="!treeClickItem.roleName" class="title">角色</div>
          <el-header class="header" height="65px">
            <el-form label-width="100px" class="FormInput clr" inline @submit.native.prevent>
              <el-button
                v-if="btnAip.add&&btnAip.add.buttonCaption"
                type="primary"
                @click="handelAddClick"
              >{{ btnAip.add.buttonCaption }}</el-button>
              <el-button
                v-if="btnAip.batchdelete&&btnAip.batchdelete.buttonCaption"
                :type="typeColor"
                @click="handelDelClick"
                :disabled="delState"
              >{{ btnAip.batchdelete.buttonCaption }}</el-button>
              <!-- <el-button type="success" @click="handelAddClick">添加</el-button>
              <el-button type="danger" @click="handelDelClick">批量删除</el-button>-->
              <el-input
                v-model="search"
                placeholder="姓名/手机/邮箱"
                clearable
                style="width:200px;float:right;"
                @keyup.enter.native="doFilters"
                @clear="doFilters"
              >
                <i slot="suffix" class="el-input__icon el-icon-search" @click="doFilters"></i>
              </el-input>
            </el-form>
          </el-header>
        </div>

        <!-- <el-table ref="table" :height="tableHeight-88" :data="tableData" row-key="id" border> -->
        <el-table
          ref="table"
          :data="tableData"
          tooltip-effect="dark"
          style="width: 100%;"
          :height="tableHeight-94"
          @select="handleSelectionChange"
          @select-all="handleSelectionAllChange"
        >
          <el-table-column type="selection" width="55" />
          <!-- <el-table-column type="index" label="序号" /> -->
          <el-table-column prop="userAccount.accountName" label="账号姓名" class="ellipsis" />
          <el-table-column prop="userAccount.realName" label="真实姓名" class="ellipsis" />
          <el-table-column prop="userAccount.telAccount" label="手机号码" class="ellipsis" />
          <el-table-column prop="userAccount.emailAccount" label="邮箱" class="ellipsis" />
        </el-table>
      </div>
    </el-main>

    <Dialog ref="iframeDialog" :title="dialogTitle">
      <div slot="dialogBody" style="padding:20px">
        <Department ref="Department" />
      </div>
      <el-button slot="dialogFooter" size="small" type="primary" @click="doDialogBtn">确 定</el-button>
    </Dialog>
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { closest } from "@/utils/Dom";
import { getPageItem } from "@/utils/findObj";
import Department from "./components/Department";
import Dialog from "@/components/Dialog";
import { getBtnCtr } from "@/utils/BtnControl";
import { trim } from "@/utils/common.js";

export default {
  name: "viewsSysSettingsRoleControlindexvue",
  components: {
    Department,
    Dialog
  },
  data() {
    return {
      loading:false,
      btnAip: "",
      search: "", // 查询过滤字段
      headerHeight: "150px",
      mainHeight: "",
      footerHeight: "58px",
      tableHeight: "500",
      labelPosition: "left",
      filterText: "",
      accordion: false,
      defaultProps: {
        children: "children",
        label: "label"
      },
      tableData: [], // table数据模型
      tableDataHis: [], // table数据模型(历史版)
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 10, // 分页显示记录条数
      totalCount: 20, // 总记录数
      pageIndex: 1, // 页码
      sortColumn: "", // 排序字段
      sortOrder: "", // 排序规则
      menuName: "", // 菜单名称查询条件
      menuStatus: "全部", // 菜单状态
      Status: "",
      roleData: [],
      treeClickItem: {},
      selectList: [],
      dialogTitle: "", // 弹框标题
      delState: true,
      typeColor: "info",
      firstState: true
    };
  },
  watch: {
    filterText(val) {
      this.$refs.tree.filter(val);
    },
    selectList(val) {
      if (val.length > 0) {
        this.delState = false;
      } else {
        this.delState = true;
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.getTree();
  },
  mounted() {
    this.setStyle();
    var data = getPageItem(
      this.$route.path,
      this.$store.getters.userPermission
    );
    // console.log(data.component)
  },
  methods: {
    setStyle() {
      // 设置样式
      var b =
        document.documentElement.clientHeight -
        document.getElementById("navbar_yfkj").clientHeight;
      this.$nextTick(() => {
        this.tableHeight = b - 40;
      });
    },
    getTree() {
      this.loading = true;
      // 角色列表
      var rqs = RequestObject.CreateGetObject();
      request({
        url: "system/api/TSMRoles",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.roleData = res.data;
          //默认第一条
          if (this.firstState && this.roleData.length > 0) {
            this.firstState = false;
            this.treeClickItem = this.roleData[0];
            this.$nextTick(() => {
              this.setTreeStyle();
            });
          }
          this.loading = false;
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          this.loading = false;
        }
      });
    },
    handleSelectionAllChange(rows) {
      // 全选
      this.selectList = rows;
      if (rows.length == 0) {
        this.typeColor = "info";
      } else {
        this.typeColor = "danger";
      }
    },
    handleSelectionChange(rows) {
      // 单选
      if (rows.length == 0) {
        this.typeColor = "info";
      } else {
        this.typeColor = "danger";
      }
      this.selectList = rows;
    },
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    nodeClick(item) {
      this.treeClickItem = item;
      this.setTreeStyle();
    },
    setTreeStyle() {
      // 设置选中的样式
      var a = document.querySelectorAll("div.el-tree-node__content");
      a.forEach(item => {
        item.style.background = "#FFF";
      }); // 重置所有按钮样式
      closest(
        this.$refs["tree-" + this.treeClickItem.id],
        ".el-tree-node__content"
      ).style.background = "#F5F7FA"; // 点击按钮样式
      this.getUserList(); // 获取右侧table数据
    },
    getUserList() {
      // 获取列表数据
      var param = [
        { column: "roleid", content: this.treeClickItem.id, condition: 0 }
        // { column: "roleid", content: 42, condition: 0 }
      ];
      var robject = RequestObject.CreateGetObject(false, 0, 0, param);
      this.tableData = [];
      request({
        url: "/system/api/TSMRoles/GetDispathUserAsync",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
        if (res.code === 0) {
          this.tableData = res.data;
          this.tableDataHis = [...res.data];
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    handleDragEnd(draggingNode, dropNode, dropType, ev) {
      // 拖拽事件
      // console.log(draggingNode.key);
      // console.log(this.data);
    },
    handleDrop(draggingNode, dropNode, dropType, ev) {
      // 拖拽事件
      // console.log(draggingNode.key);
      // console.log(dropNode.key);
    },
    handelDelClick() {
      // 点击删除按钮
      if (this.selectList.length <= 0) {
        this.$message({
          message: "请选择删除的员工",
          type: "error"
        });
        return;
      }
      this.$confirm("是否删除用户？", "确认信息", {
        type: "warning"
      }).then(() => {
        var postDataList = [];
        this.selectList.map(item => {
          postDataList.push(item.id);
        });
        var reqObject = RequestObject.CreatePostObject(postDataList);
        request({
          url: "/system/api/TSMRoles/RemoveRoleForUser",
          method: "delete",
          data: reqObject
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            this.$message({
              message: "操作成功!",
              type: "success"
            });
            this.setTreeStyle(); // 设置tree样式和请求数据
          }
        });
      });
    },
    handelAddClick() {
      // 点击添加按钮
      if (!this.treeClickItem.id) {
        this.$message({
          message: "请选择角色",
          type: "error"
        });
        return;
      }
      this.dialogTitle = `角色:${this.treeClickItem.roleName}`;
      this.$refs.iframeDialog.dialog("open");
      this.$nextTick(() => {
        this.$refs.Department.tableHeight = this.tableHeight - 63 - 55;
      });
    },
    doDialogBtn() {
      // 点击保存按钮
      var selectList = this.$refs.Department.selectList;
      if (selectList.length <= 0) {
        this.$message({
          message: "请选择员工",
          type: "error"
        });
        return;
      }
      this.$confirm(
        `是否给用户添加“${this.treeClickItem.roleName}”的角色？`,
        "确认信息",
        {
          type: "warning"
        }
      ).then(() => {
        var postDataList = [];

        selectList.map(item => {
          postDataList.push({
            id: 0,
            roleId: this.treeClickItem.id,
            userId: item.id
          });
        });
        var reqObject = RequestObject.CreatePostObject(null, postDataList);
        request({
          url: "/system/api/TSMRoles/DispathDept",
          method: "post",
          data: reqObject
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            this.$message({
              message: "操作成功!",
              type: "success"
            });

            this.setTreeStyle(); // 设置tree样式和请求数据
            this.$refs.iframeDialog.dialog("close");
          }
        });
      });
    },
    doFilters() {
      const accountName = this.getFilter(
        this.tableDataHis,
        "accountName",
        trim(this.search)
      );
      const realName = this.getFilter(
        this.tableDataHis,
        "realName",
        trim(this.search)
      );

      const telAccount = this.getFilter(
        this.tableDataHis,
        "telAccount",
        trim(this.search)
      );
      const emailAccount = this.getFilter(
        this.tableDataHis,
        "emailAccount",
        trim(this.search)
      );
      const sumArr = [
        ...accountName,
        ...realName,
        ...telAccount,
        ...emailAccount
      ];
      const set = new Set(sumArr);
      const newArr = Array.from(set);
      this.tableData = newArr;
    },
    getFilter(data, res, search) {
      const list = [];
      data.map(item => {
        if (
          item["userAccount"][res] != null &&
          item["userAccount"][res].indexOf(search) != -1
        ) {
          list.push(item);
        }
      });
      return list;
    }
  }
};
</script>
<style lang="scss" scoped>
#RoleControl /deep/ {
  .pr {
    position: relative;
  }
  .header {
    padding-top: 10px;
  }
  .el-tag {
    cursor: pointer;
  }
  .ellipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    display: block;
    width: 100%;
    cursor: pointer;
  }
  .iconSty {
    width: 20px;
    height: 20px;
    vertical-align: middle;
  }
  .textCenter {
    text-align: center;
  }
  .dialog-footer {
    text-align: right;
  }
  .tableTab {
    float: left;
    margin: 0px;
    padding: 0px;
  }
  .tableBox {
    width: calc(100% - 260px);
    margin-left: 20px;
    float: left;
  }
  .title {
    padding: 10px 0px 0px;
  }
  aside {
    background: #fff;
  }
  .clr::after {
    content: "";
    display: block;
    height: 0;
    clear: both;
    visibility: hidden;
  }
  .roleTable {
    font-size: 12px;
    padding: 4px 10px 3px;
    border-bottom: 1px solid #dfe6ec;
    cursor: default;
  }
  .el-tree-node__content {
    padding: 20px 0px;
    font-size: 12px;
    border-bottom: 1px solid #dfe6ec;
    cursor: pointer;
  }
  .roleHeader {
    background: #f5f7fa;
    border-bottom: 1px solid #dfe6ec;
    text-align: center;
    color: #333333;
  }
  .el-tree {
    color: #333333;
  }
  .el-tree-node:focus > .el-tree-node__content {
    background-color: #fff;
  }
  aside {
    border: 1px solid #e6ebf5;
  }
  .el-table {
    border-right: 1px solid #e6ebf5;
    border-left: 1px solid #e6ebf5;
    border-top: 1px solid #e6ebf5;
  }
}
.el-header {
  // margin-bottom: 15px;
  padding-left: 0px;
  padding-right: 0px;
  // padding-bottom: 15px;
}
</style>
