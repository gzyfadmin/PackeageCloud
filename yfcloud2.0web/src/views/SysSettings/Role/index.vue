<template>
  <el-container id="menus" v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-header id="elheader" class="header" height="auto">
      <el-form :label-position="labelPosition" label-width="100px" class="FormInput">
        <!-- <el-button type="primary" @click="handelAddClicks">新增</el-button> -->
        <el-button v-if="btnAip.add&&btnAip.add.buttonCaption" type="primary" @click="handelAddClicks">{{ btnAip.add.buttonCaption }}</el-button>
      </el-form>
    </el-header>
    <el-main :height="mainHeight">
      <el-table
      v-if="showtab"
        :height="mainHeight"
        :data="tableData"
        v-model="tableData"
        row-key="id"
        border
        default-expand-all
        :tree-props="{children: 'children'}"
      >
        <el-table-column prop="roleName" label="角色名称" width="250" />
        <el-table-column label="序号" prop="seqNumber" />
        <el-table-column prop="roleDesc" label="角色描述" width="280">
          <template slot-scope="scope">
            <el-tooltip
              class="item"
              effect="light"
              :content="scope.row.roleDesc"
              :open-delay="300"
              placement="top-end"
              v-if="scope.row.roleDesc&&scope.row.roleDesc.length>=20"
            >
              <div class="ellipsis">{{scope.row.roleDesc}}</div>
            </el-tooltip>

            <!-- <el-popover
              v-if="scope.row.roleDesc&&scope.row.roleDesc.length>=20"
              placement="top-start"
              trigger="hover"
              :content="scope.row.roleDesc"
            >
              <div class="ellipsis" slot="reference">{{ scope.row.roleDesc }}</div>
            </el-popover> -->
            <div
              class="ellipsis"
              v-if="scope.row.roleDesc&&scope.row.roleDesc.length<20&&scope.row.roleDesc!=null"
            >{{scope.row.roleDesc}}</div>
          </template>
        </el-table-column>
        <el-table-column label="操作" fixed="right" width="245">
          <template slot-scope="scope">
            <el-tooltip
              class="item"
              effect="light"
              :content="btnAip.on.buttonCaption"
              placement="top-start"
              :open-delay="300"
            >
              <el-button
                v-if="btnAip.on&&btnAip.on.buttonCaption"
                type="primary"
                icon="el-icon-top"
                circle
                @click="handelMoveClick(scope.row.id,0)"
              />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.up&&btnAip.up.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.up.buttonCaption"
              placement="top-start"
              :open-delay="300"
            >
              <el-button
                type="primary"
                icon="el-icon-bottom"
                circle
                @click="handelMoveClick(scope.row.id,1)"
              />
            </el-tooltip>
            <el-tooltip
            v-if="btnAip.addclass&&btnAip.addclass.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.addclass.buttonCaption"
              placement="top-start"
              :open-delay="300"
            >
              <el-button
                type="primary"
                icon="el-icon-circle-plus-outline"
                circle
                @click="handelAddClick(scope.$index, scope.row)"
              />
            </el-tooltip>
            <el-tooltip
             v-if="btnAip.edit&&btnAip.edit.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.edit.buttonCaption"
              placement="top-start"
              :open-delay="300"
            >
              <el-button
                type="primary"
                icon="el-icon-edit"
                circle
                @click="redactClick(scope.$index, scope.row)"
              />
            </el-tooltip>
            <el-tooltip
            v-if="btnAip.delete&&btnAip.delete.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.delete.buttonCaption"
              placement="top-start"
              :open-delay="300"
            >
              <el-button
                type="danger"
                icon="el-icon-delete"
                circle
                @click="deleatTabel(scope.$index, scope.row)"
              />
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <el-dialog
      title="编辑角色信息"
      :visible.sync="dialogVisible"
      width="600px"
      :close-on-click-modal="false"
      :center="true"
    >
      <el-form ref="formData" label-width="100px" :model="formData" :rules="rules">
        <el-form-item label="角色名称：" prop="name">
          <el-input placeholder="请输入角色名称" v-model="formData.name" />
        </el-form-item>
        <el-form-item label="上级菜单：" prop="father">
          <el-cascader
            :options="options"
            :props="{ checkStrictly: true }"
            clearable
            style="width:100%"
            v-model="formData.father"
            :disabled="true"
          ></el-cascader>
        </el-form-item>
        <el-form-item label="排序：" prop="number">
          <el-input placeholder="请输入排序" v-model="formData.number" />
        </el-form-item>
        <el-form-item label="角色描述：" prop="names">
          <el-input type="textarea" placeholder="请输入角色描述" v-model="formData.names" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="handelCloseClick">关闭</el-button>
        <el-button type="primary" :loading="saveBtn" @click="setObts">保存</el-button>
      </div>
    </el-dialog>
    <el-dialog
      title="新增角色信息"
      :visible.sync="dialogVisibles"
      width="600px"
      :close-on-click-modal="false"
      :center="true"
    >
      <el-form ref="formData" label-width="100px" :rules="rules" :model="formData">
        <el-form-item label="角色名称：" prop="name">
          <el-input placeholder="请输入角色名称" v-model="formData.name" />
        </el-form-item>
        <el-form-item label="上级菜单：" prop="father">
          <el-cascader
            :options="options"
            :props="{ checkStrictly: true }"
            clearable
            style="width:100%"
            v-model="formData.father"
          ></el-cascader>
        </el-form-item>
        <el-form-item label="排序：" prop="number">
          <el-input placeholder="请输入排序" v-model="formData.number" />
        </el-form-item>
        <el-form-item label="角色描述：" prop="names">
          <el-input placeholder="请输入角色描述" type="textarea" v-model="formData.names" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="handelCloseClics">关闭</el-button>
        <el-button type="primary" @click="setObTs">保存</el-button>
      </div>
    </el-dialog>
  </el-container>
</template>
<script>
import rquestObjet from "@/utils/requestObject";
import height from "@/utils/height";
import { request } from "http";
import requests from "@/utils/request";
import { type } from "os";
import { getBtnCtr } from "@/utils/BtnControl";
export default {
  name: "viewsSysSettingsRoleindexvue",
  data() {
    /**
     * 序号验证
     * validatePass
     */
    var validatePass = (rule, value, callback) => {
      var test1 = /^[0-9]{1,5}$/;
      // console.log(test1.test(value));
      if (value === "") {
        callback(new Error("请输入序列号"));
      } else if (!test1.test(value)) {
        callback(new Error("请输入1-5位整数"));
      } else {
        callback();
      }
    };
    /**
     * setrolueName
     *
     *角色名称
     */
    var setrolueName = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入角色名称"));
      } else {
        callback();
      }
    };
    /**
     * 描述验证
     * setrouleDect
     */
    // var setrouleDect = (rule, value, callback) => {
    //   if (value === "") {
    //     callback(new Error("请输入角色描述"));
    //   } else {
    //     callback();
    //   }
    // };

    return {
      loading:false,
      saveBtn:false,
      showtab:false,
      formData: {
        name: "",
        names: "",
        number: "",
        father: ""
      },
      beforFormData: {
        name: "",
        names: "",
        number: "",
        father: ""
      },
      btnAip: "",
      beforData: "",
      options: "",
      sonId: "",
      selectedRow: null,
      headerHeight: "50px",
      mainHeight: "",
      footerHeight: "0",
      tableHeight: "600",
      labelPosition: "left",
      adds: false,
      edit: false,
      rules: {
        number: [{ validator: validatePass, trigger: "blur", required: true }],

        name: [
          { validator: setrolueName, trigger: "blur", required: true },
          { min: 0, max: 50, required: true ,message: "不能超过50个字符", trigger: "blur"},
        ],
        names:[{ min: 0, max: 500, required: false ,message: "不能超过500个字符", trigger: "blur"},]
      },
      tableData: [
        {
          id: 1,
          roleName: "总经办",
          roleDesc: "总经办",
          styl: true,
          children: [
            {
              id: 2,
              roleName: "总经办1",
              roleDesc: "总经办2",
              styl: false
            },
            {
              id: 3,
              roleName: "总经办1",
              roleDesc: "总经办2",
              styl: false
            }
          ]
        }
      ], // table数据模型
      dialogVisible: false, // 编辑窗口是否显示
      dialogVisibles: false,
      menuName: "", // 菜单名称查询条件
      Status: "全部", // 菜单状态
      storg1: "",
      storg2: "",
      add: "",
      rows: ""
    };
  },
  // watch: {
  //   tableData: function(newQuestion, oldQuestion) {
      // console.log(1);
      // console.log(newQuestion, "newQuestion");
      // console.log(oldQuestion, "oldQuestion");
  //   }
  // },
  watch: {
    dialogVisible(val) {
      if (!val) {
        this.$refs.formData.resetFields();
      }
    }
  },
  created() {
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.initTable();
    this.mainHeight =
      (
        height -
        126 -
        parseInt(this.headerHeight) -
        parseInt(this.footerHeight)
      ).toString() + "px";
  },
  activated() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
  },
  mounted() {
     this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
  },
  methods: {
    clearTable() {
      this.initTable();
    },
    handelInquire() {
      this.handelKeyDown();
    },
    /**
     * handelMoveClick
     * 上移或者下移
     */
    handelMoveClick(id, type) {
      let postData = {
        id: id,
        type: type
      };
      let postDatas = rquestObjet.CreatePostObject(postData);
      requests({
        url: "system/api/TSMRoles/MovePostion",
        method: "POST",
        data: postDatas
      }).then(res => {
        if (res.code == 0) {
          this.$message({
            type: "success",
            message: "操作成功!"
          });
          this.initTable();
        }
      });
    },
    /**
     * GetSelectOptionsData
     * 转化函数
     */
    GetSelectOptionsData(data, level) {
      var getSelect = function(data, level) {
        var resObject = [];
        if (!data || data.length < 1) {
          return null;
        }
        data.forEach((v, i) => {
          var thisId = v.id.toString();
          if (v.id === -1) {
            thisId = "Tree" + level + "" + i;
          }
          if (v.children && v.children.length > 0) {
            resObject.push({
              value: thisId,
              label: v.roleName,
              children: getSelect(v.children, level + 1)
            });
          } else {
            resObject.push({
              value: thisId,
              label: v.roleName,
              children: getSelect(v.children, level + 1)
            });
          }
        });
        return resObject;
      };
      return getSelect(data, level);
    },
    /**
     * 初始化table
     * initTable
     */
    initTable() {
      this.loading = true;
      var rqs = rquestObjet.CreateGetObject();
      requests({
        url: "system/api/TSMRoles",
        method: "GET",
        params: { requestObject: JSON.stringify(rqs) }
      }).then(res => {
        this.tableData = res.data;
        this.beforData = res.data;
        this.options = this.GetSelectOptionsData(res.data);
        this.loading = false;
      });
    },

    /**
     * fatherPaer
     * 查找父节点
     */
    fatherPaer(data, nodeid) {
      let currentId = nodeid; // 当前id
      let arr = [];
      function getParentsIds(data) {
        for (let i = 0; i < data.length; i++) {
          let temp = data[i];
          if (temp.id == currentId) {
            arr.push(temp.id);
            return 1;
          }
          if (temp && temp.children && temp.children.length > 0) {
            let t = getParentsIds(temp.children);
            if (t == 1) {
              arr.push(temp.id);
              return 1;
            }
          }
        }
      }
      getParentsIds(data);
      return arr;
    },
    // 父级新增关闭
    handelCloseClics() {
      this.dialogVisibles = false;
    },
    setObTs() {
      this.$refs.formData.validate(valid => {
        if (valid) {
          if (this.formData.father.length == 0) {
            var time = new Date(); // timeTag为时间戳
            var newTime =
              time.getFullYear() +
              "-" +
              (time.getMonth() + 1) +
              "-" +
              time.getDate();
            var postDatas = {
              roleName: this.formData.name,
              parentId: -1,
              seqNumber: this.formData.number,
              roleDesc: this.formData.names,
              createTime: newTime
            };
            var postdata = rquestObjet.CreatePostObject(postDatas);
            requests({
              url: "system/api/TSMRoles",
              method: "POST",
              data: postdata
            }).then(res => {
              // console.log(res);
              if (res.code == 0) {
                this.$message({
                  type: "success",
                  message: "操作成功!"
                });
                this.dialogVisibles = false;
                this.initTable();
              } else {
                this.$message.error("角色名称重复,请重新输入");
              }
            });
          } else {
            var sonArry = this.fatherPaer(this.tableData, this.sonId);
            let time = new Date(); // timeTag为时间戳
            let newTime =
              time.getFullYear() +
              "-" +
              (time.getMonth() + 1) +
              "-" +
              time.getDate();

            let postDatas = {
              seqNumber: this.formData.number,
              roleName: this.formData.name,
              parentId: this.formData.father[this.formData.father.length - 1],
              roleDesc: this.formData.names,
              createTime: newTime
            };
            var postdata = rquestObjet.CreatePostObject(postDatas);

            requests({
              url: "system/api/TSMRoles",
              method: "POST",
              data: postdata
            }).then(res => {
              if (res.code == 0) {
                this.$message({
                  type: "success",
                  message: "操作成功!"
                });
                this.dialogVisibles = false;
                this.initTable();
                this.adds = false;
              } else {
                this.$message.error("角色名称重复,请重新输入");
              }
            });
          }
        } else {
          return;
        }
      });
    },
    handelKeyDown() {
      let queryConditions = [
        {
          column: "roleName",
          content: this.menuName,
          condition: 6
        }
      ];
      var postObj = rquestObjet.CreateGetObject(
        false,
        0,
        0,
        queryConditions,
        null
      );
      requests({
        url: "/system/api/TSMRoles",
        method: "GET",
        params: {
          requestObject: JSON.stringify(postObj)
        }
      }).then(res => {
        this.tableData = res.data;
      });
    },
    /**
     * 子级新增
     * setObts
     */
    setObts() {
      this.$refs.formData.validate(valid => {
        if (valid) {
          this.saveBtn = true;
          if (this.edit) {
            //父节点更改数据
            let beforPostDatas = {
              id: this.rows.id,
              roleDesc: this.beforFormData.names,
              roleName: this.beforFormData.name,
              parentId: this.rows.parentId,
              seqNumber: this.beforFormData.number,
              companyId: this.rows.companyId,
              createTime: this.setTimes()
            };
            let postDatas = {
              id: this.rows.id,
              roleDesc: this.formData.names,
              roleName: this.formData.name,
              parentId: this.rows.parentId,
              seqNumber: this.formData.number,
              companyId: this.rows.companyId,
              createTime: this.setTimes()
            };
            let postdata = rquestObjet.CreatePostObject(
              postDatas,
              null,
              beforPostDatas
            );

            requests({
              url: "system/api/TSMRoles",
              method: "put",
              data: postdata
            }).then(res => {
              if (res.code == 0) {
                this.$message({
                  type: "success",
                  message: "操作成功!"
                });
                this.dialogVisible = false;
                this.initTable();
                this.edit = false;
              } else {
                this.$message.error("角色名称重复,请重新输入");
              }
            });
          } else if (this.adds) {
            //增加子菜单
            var sonArry = this.fatherPaer(this.tableData, this.sonId);
            let time = new Date(); // timeTag为时间戳
            let newTime =
              time.getFullYear() +
              "-" +
              (time.getMonth() + 1) +
              "-" +
              time.getDate();
            let postDatas = {
              seqNumber: this.formData.number,
              roleName: this.formData.name,
              parentId: this.sonId,
              roleDesc: this.formData.names,
              createTime: newTime
            };
            var postdata = rquestObjet.CreatePostObject(postDatas);

            requests({
              url: "system/api/TSMRoles",
              method: "POST",
              data: postdata
            }).then(res => {
              if (res.code == 0) {
                this.$message({
                  type: "success",
                  message: "操作成功!"
                });
                this.dialogVisible = false;
                this.initTable();
                this.adds = false;
              } else {
                this.$message.error("角色名称重复,请重新输入");
              }
            });
          }
          var setTime = setTimeout(()=>{
            this.saveBtn = false;
            clearTimeout(setTime)
          },50)
        } else {
          return;
        }
      });
    },
    setTimes() {
      let time = new Date(); // timeTag为时间戳
      let newTime =
        time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate();
      return newTime;
    },
    deleatTabel(index, row) {},
    //编辑
    redactClick(index, row) {
      this.edit = true;
      this.dialogVisible = true;
      this.formData.name = row.roleName;
      this.formData.names = row.roleDesc;
      this.formData.number = row.seqNumber;
      this.formData.father = [row.id.toString()];
      this.beforFormData.name = row.roleName;
      this.beforFormData.names = row.roleDesc;
      this.beforFormData.number = row.seqNumber;
      this.beforFormData.father = [row.id.toString()];
      this.rows = row;
    },
    //关闭
    handelCloseClick() {
      this.dialogVisible = false;
    },
    // 新增父级
    handelAddClicks(index, row) {
      this.formData.name = "";
      this.formData.names = "";
      this.formData.number = "";
      this.formData.father = "";
      this.dialogVisibles = true;
    },
    // 新增子集
    handelAddClick(index, row) {
      this.adds = true;
      this.formData.name = "";
      this.formData.names = "";
      this.formData.number = "";
      this.formData.father = [row.id.toString()];
      this.sonId = row.id;
      this.dialogVisible = true;
    },
    //删除
    deleatTabel(index, row) {
      this.$confirm("此操作将永久删除" + row.roleName + ", 是否继续？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          let postData = row.id;
          let postDatas = rquestObjet.CreatePostObject(postData);
          requests({
            url: "system/api/TSMRoles/ById",
            method: "DELETE",
            data: postDatas
          }).then(res => {
            if (res.code == 0) {
              this.$message({
                type: "success",
                message: "操作成功!"
              });
              this.initTable();
            }
          });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    }
  }
};
</script>
<style lang="scss" scoped>
#menus /deep/ {
  .header {
    padding-top: 15px;
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
  
}
</style>
