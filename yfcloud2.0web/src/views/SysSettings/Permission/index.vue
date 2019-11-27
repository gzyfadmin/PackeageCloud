<template>
  <el-container
    id="platformPermissions"
    v-loading="loading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    style="margin-left:20px;"
  >
    <el-header id="elheader" class="headerBd" height="auto" style="margin:0px;padding:0px;padding-bottom:15px">
      <el-button
        v-if="btnAip.preserve&&btnAip.preserve.buttonCaption"
        type="primary"
        @click="handelSave"
      >{{ btnAip.preserve.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.delete&&btnAip.delete.buttonCaption"
        type="danger"
        @click="handelDelete"
      >{{ btnAip.delete.buttonCaption }}</el-button>
      <!-- <el-button type="primary" @click="handelSave">保存</el-button> -->
      <!-- <el-button type="danger" @click="handelDelete">删除</el-button> -->
    </el-header>
    <el-container style="margin:0px;padding:0px">
      <el-aside
        width="240px"
        style="margin:0px;padding:0px;"
        class="tableTab"
        :style="{height:mainHeight+'px'}"
      >
        <div class="roleTable roleHeader">所有角色</div>
        <el-tree
          ref="tree"
          class="filter-tree"
          :data="outTableData"
          node-key="id"
          default-expand-all
          :expand-on-click-node="false"
          @node-click="nodeClick"
        >
          <span slot-scope="{ node, data }" class="custom-tree-node pr">
            <span :id="'tree-'+data.id" :ref="'tree-'+data.id">{{ data.roleName }}</span>
          </span>
        </el-tree>
      </el-aside>

      <el-main style="margin:0px 10px 0px 10px;padding:0px">
        <el-container style="margin:0px;padding:0px 0px 0px 0px">
          <el-main class="menusTable" style="margin:0px 10px 0px;padding:0px">
            <el-table
              v-if="showtab"
              ref="innerTable"
              :height="mainHeight"
              :data="inTableData"
              style="width: 99.9%"
              row-key="id"
              :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
              border
              @select="handelInnerTableSelect"
              @select-all="handelInnetTableSelectAll"
            >
              <el-table-column type="selection" />
              <el-table-column v-if="false" prop="id" label="id" />
              <el-table-column v-if="false" prop="tempId" label="tempId" />
              <el-table-column prop="menuName" label="菜单名称" width="160px">
                <template slot-scope="scope">
                  <el-badge
                    v-if="scope.row.badgeNum>0"
                    :value="scope.row.badgeNum"
                    class="badgeItem"
                  >{{ scope.row.menuName }}</el-badge>
                  <div
                    v-if="scope.row.badgeNum<=0|| !scope.row.badgeNum"
                    class="badgeItem"
                  >{{ scope.row.menuName }}</div>
                </template>
              </el-table-column>
              <el-table-column prop="menuDesc" label="菜单描述" width="300px" />
              <el-table-column prop="buttons" label="按钮权限">
                <template slot-scope="scope">
                  <el-checkbox-group v-if="scope.row.isMenu" v-model="scope.row.buttons">
                    <el-checkbox
                      v-for="item in scope.row.onButtons"
                      :key="item.id"
                      :label="item.id"
                      @change="handleCheckGroup($event,scope.row)"
                    >{{ item.buttonCaption }}</el-checkbox>
                  </el-checkbox-group>
                </template>
              </el-table-column>
              <el-table-column label="操作" fixed="right" width="150px">
                <template v-if="scope.row.isMenu" slot-scope="scope">
                  <el-checkbox
                    v-model="scope.row.btnState"
                    @change="handelCheckAll($event,scope.row)"
                  >全选</el-checkbox>
                </template>
              </el-table-column>
            </el-table>
          </el-main>
        </el-container>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
// import { MessageBox } from 'element-ui'
// import DateUtil from '@/utils/dateUtil'
import height from "@/utils/height";
import AppTree from "@/utils/appTree";
// import { constants } from 'crypto'
import { closest } from "@/utils/Dom";
import { getBtnCtr } from "@/utils/BtnControl";

export default {
  name: "viewsSysSettingsPermissionindexvue",
  data() {
    return {
      showtab: true,
      loading: true,
      btnAip: "",
      treeClickItem: {},
      filterTenant: "",
      headerHeight: "50px",
      mainHeight: 0,
      treeDivStyle: "",
      treeData: null,
      outTableData: null,
      inTableData: [],
      allButton: null,
      currAppPermisson: null, // 当前正在操作的APP及菜单权限
      outSelectedRow: null, // 模板名称选中的行
      allPermisson: null, // 所有模板权限信息
      selectedNodeId: -1, // 被选中的节点id
      treeStyle: "",
      tableHeight: "500px",
      checkAll: false,
      checkAllState: false,
      isIndeterminate: true,
      roleList: {},
      btnIdList: [], // 权限按钮id list
      btnIsDefault: [],
      changeState: false,
      postDataEdit: [],
      defaultProps: {
        children: "children",
        label: "roleName"
      }
    };
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    // // 设置当前页面样式
    this.setStyle();

    // // 初始化所有模板信息
    this.InitTemplates();

    this.getAppTree().then(() => {
      // this.getPermissions(); //获取Menutree
    });
  },
  activated() {
    // this.showtab = false;
    // setTimeout(() => {
    //   this.showtab = true;
    // }, 10);
  },
  mounted() {
    // this.showtab = false;
    // setTimeout(() => {
    //   this.showtab = true;
    // }, 10);
  },
  methods: {
    nodeClick(item) {
      this.loading = true;
      this.treeClickItem = item;
      this.handelOutTableRowClick();
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
      // 获取右侧table数据
    },
    handleCheckedCitiesChange(value) {
      const checkedCount = value.length;
      this.checkAll = checkedCount === this.cities.length;
      this.isIndeterminate =
        checkedCount > 0 && checkedCount < this.cities.length;
    },
    getPermissions() {
      var reqObject = RequestObject.CreateGetObject(false, 0, 0, [
        { column: "roleId", content: this.roleList.id, condition: 0 }
      ]);
      request({
        url: "/system/api/TSMRolePermissions",
        method: "get",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      })
        .then(res => {
          this.loading = false;
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            this.allPermisson = res.data;
            this.postDataEdit = res.data;
            this.handlePermissionsData(this.inTableData, res.data);
          }
        })
        .catch(error => {
          this.loading = false;
        });
    },
    handlePermissionsData(parent, data) {
      // 将权限数据导入Apptree
      var This = this;
      this.menuTreeDefault(this.inTableData);
      function setData(dt) {
        for (var i = 0; i < dt.length; i++) {
          for (var j = 0; j < data.length; j++) {
            if (dt[i].id == data[j].menuId) {
              var b = data[j].buttonIds.split(",");
              var d = [];
              b.map(function(item) {
                d.push(parseInt(item));
              });
              if (dt[i].onButtons.length != 0) {
                const list = dt[i].onButtons;
                if (list.length == d.length) {
                  dt[i].btnState = true;
                }
              }
              This.$refs.innerTable.toggleRowSelection(dt[i], true);

              This.$set(dt[i], "buttons", d);
            }
          }
          if (dt[i].children) {
            setData(dt[i].children);
          }
        }
      }
      setData(parent);
      this.handelParent(parent);
    },
    handelParent(dt) {
      // 父级的菜单选择处理
      // 添加子元素选中的标志
      var setFlag = dt => {
        for (var i = 0; i < dt.length; i++) {
          if (dt[i].children) {
            for (var j = 0; j < dt[i].children.length; j++) {
              if (dt[i].children[j].buttons.length > 0) {
                dt[i].children[j].btnIf = "btnIfbtnIfbtnIfbtnIf_2";
              }
            }
            setFlag(dt[i].children);
          }
        }
      };
      setFlag(dt); // 添加子元素选中的标志
      this.setRowSelect(dt); // 设置选中
      for (var i = 0; i < dt.length; i++) {
        this.msgCount(dt[i]); // 统计消息
      }
    },
    msgCount(dt) {
      var l = 0;
      var setCount = dt => {
        for (var i = 0; i < dt.length; i++) {
          if (dt[i].buttons.length > 0 && dt[i].isMenu) {
            l = l + 1;
          }
          if (dt[i].children) {
            setCount(dt[i].children);
          }
        }
      };
      setCount(dt.children);
      dt.badgeNum = l;
    },
    setRowSelect(dt) {
      // 设置选中
      for (var i = 0; i < dt.length; i++) {
        if (dt[i].children) {
          // 不选父级按钮
          for (var j = 0; j < dt[i].children.length; j++) {
            if (dt[i].children[j].buttons.length <= 0) {
              var a = 0;
              for (var k = 0; k < dt[i].children.length; k++) {
                a = a + dt[i].children[k].buttons.length;
              }
              if (a == 0) {
                this.$refs.innerTable.toggleRowSelection(dt[i], false);
              }
            }
          }
          // 选中父级按钮
          var ste = JSON.stringify(dt[i].children);
          if (ste.indexOf("btnIfbtnIfbtnIfbtnIf_2") != -1) {
            this.$refs.innerTable.toggleRowSelection(dt[i], true);
          }

          this.setRowSelect(dt[i].children);
        }
      }
    },
    getAppTree() {
      // 获取table数据
      var robject = RequestObject.CreateGetObject(false, 0, 0, [
        { column: "ParentID", content: "-1", condition: 0 }
      ]);
      return request({
        url: "/system/api/TSMRolePermissions/GetMenuTree",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
        if (res.code === 0) {
          // this.inTableData = this.handleData(res.data)
          // console.log(res)
          this.inTableData = AppTree.GetTreeOnButtons(res.data);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    handleData(data) {
      this.menuTreeDefault(data);
      return data;
    },
    menuTreeDefault(dt) {
      // 重置table权限按钮
      for (var i = 0; i < this.inTableData.length; i++) {
        this.inTableData[i].badgeNum = 0; // 消息初始化
      }
      this.setTreeDefault(dt);
    },
    setTreeDefault(dt) {
      // Tree递归赋值
      for (var i = 0; i < dt.length; i++) {
        dt[i].buttons = []; // 按钮数组
        dt[i].btnState = false; // 全选flag
        dt[i].btnIf = "btnIfbtnIfbtnIfbtnIf_1"; // 选中flag
        this.$refs.innerTable.toggleRowSelection(dt[i], false);
        if (dt[i].children) {
          this.setTreeDefault(dt[i].children);
        }
      }
    },
    setStyle() {
      // 设置样式
      this.mainHeight = height - 116 - parseInt(this.headerHeight);
      this.treeStyle =
        "width:236px;height:" +
        (this.mainHeight - 4).toString() +
        "px;padding:0px;";
      this.treeDivStyle =
        "width:240px;height:" +
        this.mainHeight.toString() +
        "px;padding:2px;background:drgb(230,235,245);";
    },
    InitTemplates() {
      var reqObject = RequestObject.CreateGetObject();
      request({
        url: "/system/api/TSMRoles",
        method: "get",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      })
        .then(res => {
          this.loading = false;
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            this.outTableData = res.data;
          }
        })
        .catch(error => {
          this.loading = false;
        });
    },
    handelSave() {
      // alert(this.changeState)
      if (!this.roleList.id) {
        this.$message({
          message: "请先选择角色后再保存数据",
          type: "error"
        });
        return;
      }
      var postDataList = [];
      var getChange = dt => {
        for (var i = 0; i < dt.length; i++) {
          if (dt[i].buttons.length > 0) {
            var data = {
              id: 0,
              roleId: this.roleList.id,
              menuId: dt[i].id,
              buttonIds: dt[i].buttons.join(",")
            };
            if (dt[i].isMenu) {
              postDataList.push(data);
            }
          }
          if (dt[i].children) {
            getChange(dt[i].children);
          }
        }
      };
      getChange(this.inTableData);
      if (postDataList.length <= 0) {
        this.$message({
          message: "该角色下没有任何菜单，无法保存数据",
          type: "error"
        });
        return;
      }

      // if (JSON.stringify(AppTree.ToMenusIdButtonId(this.allPermisson)) === JSON.stringify(AppTree.ToMenusIdButtonId(postDataList))) {
      if (!this.changeState) {
        this.$message({
          message: "请修改角色权限后再保存数据",
          type: "error"
        });
        return;
      }
      this.loading = true;
      var reqObject = RequestObject.CreatePostObject(
        null,
        postDataList,
        null,
        this.postDataEdit
      );
      // 保存角色权限信息
      request({
        url: "/system/api/TSMRolePermissions",
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
          this.$store.dispatch("user/setUserPermission");

          // 重新获取数据
          this.changeState = false;
          this.getPermissions();
          // this.InitAllPermission();
        }
      });
    },
    handelOutTableRowClick() {
      var row = this.treeClickItem;
      // if (row.children.length == 0) {
      // 侧边栏点击
      if (!this.roleList.id) {
        // 未选菜单时候
        this.roleList = row;
        this.getPermissions(); // 获取权限
        this.changeState = false;
      }
      if (this.changeState == true) {
        this.$confirm(
          "是否删除角色名称为：" + this.roleList.roleName + "的权限？",
          "确认信息",
          {
            type: "warning"
          }
        ).then(() => {
          this.roleList = row;
          this.getPermissions(); // 获取权限
        });
        this.$confirm("是否保存数据？", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {
            this.handelSave(); // 保存数据
            // this.getPermissions() // 获取权限
            this.changeState = false;
          })
          .catch(() => {
            this.roleList = row;
            this.getPermissions(); // 获取权限
            this.changeState = false;
            this.setTreeStyle();
          });
      } else {
        this.roleList = row;
        this.getPermissions(); // 获取权限
        this.setTreeStyle();
      }
      // } else {
      //   // this.handelParent(this.inTableData)
      //   this.clearSelection()
      //   this.$confirm('此项不能查找和修改权限', '提示', {
      //     confirmButtonText: '确定',
      //     cancelButtonText: '取消',
      //     type: 'warning'
      //   })
      //     .then(() => {
      //       this.changeState = false
      //     })
      //     .catch(() => {
      //       this.changeState = false
      //     })
      // }
    },
    // 清空勾选项
    clearSelection() {
      this.$refs.innerTable.clearSelection();
      function data(dt) {
        for (var i = 0; i < dt.length; i++) {
          dt[i].buttons = [];
          dt[i].btnState = false;
          if (dt[i].children.length != 0) {
            for (var j = 0; j < dt[i].children.length; j++) {
              // data(dt[i].children[j])
              dt[i].children[j].buttons = [];
              dt[i].children[j].btnState = false;
            }
          }
        }
      }
      data(this.inTableData);
      for (var i = 0; i < this.inTableData.length; i++) {
        this.msgCount(this.inTableData[i]);
      }
    },
    handelInnerTableSelect(selection, row) {
      this.changeState = true; // 表示用户操作过数据
      // 如果row在selection表示是选中了
      // 如果row不在selection表示取消选中了
      let selectionFlag = false;
      selection.forEach((v, i) => {
        if (v.id == row.id) {
          selectionFlag = true;
          return;
        }
      });
      if (!selectionFlag) {
        // 取消当前菜单权限
        // row.buttons = [];
        this.menuTreeDefault([row]);
      } else {
        // const defaultButton = this.GetDefaultButton();
        this.menuTreeSelectAll([row]);
        // if (row.onButtons.length == row.buttons.length) {
        //   row.btnState = true
        // }
        // row.buttons = defaultButton;
      }
      this.handelParent(this.inTableData);
      // // 设置Tree的勾选
      // this.SetTreeCheck();
    },
    menuTreeSelectAll(dt, state) {
      var btn = [];
      for (var i = 0; i < dt.length; i++) {
        if (dt[i].onButtons != null) {
          var data = dt[i].onButtons;
          for (var l = 0; l < data.length; l++) {
            btn.push(data[l].id);
          }
        }
        this.$set(dt[i], "buttons", btn);
        btn = [];
        this.$refs.innerTable.toggleRowSelection(dt[i], true);
        if (dt[i].children) {
          this.menuTreeSelectAll(dt[i].children);
        }
        if (dt[i].onButtons != null) {
          if (dt[i].buttons.length == dt[i].onButtons.length) {
            dt[i].btnState = true;
          }
        }
      }
    },
    handelInnetTableSelectAll(selection) {
      this.changeState = true; // 表示用户操作过数据
      this.checkAllState = !this.checkAllState;
      if (this.checkAllState) {
        this.menuTreeSelectAll(this.inTableData); // 全选
      } else {
        this.menuTreeDefault(this.inTableData); // 全不选
      }
      this.handelParent(this.inTableData);
    },
    handelCheckAll(value, row) {
      this.changeState = true; // 表示用户操作过数据
      var btn = [];
      for (var i = 0; i < row.onButtons.length; i++) {
        // if(row.onButtons[i].id)
        btn.push(row.onButtons[i].id);
      }
      if (value) {
        this.$set(row, "buttons", btn);
        this.menuTreeSelectAll([row], 1);
      } else {
        this.menuTreeDefault([row]);
      }
      this.handelParent(this.inTableData);
    },
    handleCheckGroup(value, row) {
      this.changeState = true; // 表示用户操作过数据
      if (row.buttons.length == row.onButtons.length) {
        row.btnState = true;
      } else {
        row.btnState = false;
      }
      if (value) {
        this.$refs.innerTable.toggleRowSelection(row, true);
        if (this.btnIdList.length == row.buttons.length) {
          row.btnState = true;
        }
      } else {
        if (row.buttons.length < 1) {
          this.$refs.innerTable.toggleRowSelection(row, false);
        }
        if (this.btnIdList.length != row.buttons.length) {
          row.btnState = false;
        }
      }
      this.handelParent(this.inTableData); // 处理父级选中
    },
    handelDelete() {
      if (!this.roleList.id) {
        this.$message({
          message: "请选择需要删除的角色名称",
          type: "error"
        });
        return;
      }

      this.$confirm(
        "是否删除角色名称为：" + this.roleList.roleName + "的权限？",
        "确认信息",
        {
          type: "warning"
        }
      ).then(() => {
        this.loading = true;
        var postData = this.roleList.id;
        var reqObject = RequestObject.CreatePostObject(postData);
        request({
          url: "/system/api/TSMRolePermissions/ById",
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

            // 重新获取数据
            this.getPermissions();
          }
        });
      });
    }
  }
};
</script>

<style lang="scss" scoped>
#platformPermissions /deep/ {
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
    border-bottom: 2px solid #dfe6ec;
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
    background: #fff;
    border: 1px solid #e6ebf5;
  }
  .material-input__component[data-v-6bb35d14] {
    margin-top: 0px;
    position: relative;
  }
  .asideTable {
    background: #fff;
    border: 1px solid #dfe6ec;
  }
  .roleTable {
    font-size: 12px;
    padding: 4px 10px 3px;
    border-bottom: 1px solid #dfe6ec;
    cursor: pointer;
  }
  .active {
    background: #f5f7fa;
  }
  .roleTable:hover {
    background: #f5f7fa;
  }
  .badgeItem {
    display: inline-block;
  }
  .menusTitle {
    width: 100%;
    height: 50px;
    border-bottom: 1px solid #dfe6ec;
    text-align: center;
    line-height: 50px;
  }
  .menusTable {
    .el-table .cell {
      padding-top: 10px;
      padding-bottom: 10px;
    }
  }
  aside {
    background: #fff;
  }
  .el-table--border th, .el-table--border td{
    padding:0px;
  }
}
</style>
