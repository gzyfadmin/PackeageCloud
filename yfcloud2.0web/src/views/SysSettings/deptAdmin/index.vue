<template>
  <el-container id="deptAdmin" v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-main>
      <el-row>
        <el-col :span="3">
          <div
            id="bordes"
            style="overflow-y:auto;"
            :style="treeHeight"
          >
            <el-tree
              :data="datas"
              ref="teees"
              default-expand-all
              :expand-on-click-node="false"
              @node-click="nodeClick"
              :highlight-current="true"
              
            ></el-tree>
          </div>
        </el-col>
        <!-- 新增第一value -->
        <el-col :span="21">
          <div
            id="e"
            class="rightBoxMain"
            style="overflow-y:auto;width: 100%;"
          >
            <el-table
              :data="tableData"
              border
              style="width: 100%;"
              :height="mainHeight"
            >
              <el-table-column
                prop="accountName"
                label="账号姓名"
                width="120"
              />
              <el-table-column
                prop="realName"
                label="真实姓名"
                width="90"
              />
              <el-table-column
                prop="jobNumber"
                label="工号"
                width="90"
              />
              <el-table-column
                prop="telAccount"
                label="手机账号"
                width="100"
              />
              <el-table-column
                prop="emailAccount"
                label="邮箱账号"
                width="200"
              />
              <el-table-column
                prop="fixedNumber"
                label="固定电话"
                width="120"
              />
              <el-table-column
                prop="status"
                label="账号状态"
                width="80"
              >
                <template slot-scope="scope">
                  <div v-if="scope.row.status==0">无效</div>
                  <div v-if="scope.row.status==1">有效</div>
                  <div v-if="scope.row.status==2">过期</div>
                </template>
              </el-table-column>
              <el-table-column
                prop="expDate"
                label="有效期"
                width="90"
              >
                <template slot-scope="scope">
                  <div>{{ scope.row.expDate |formatTDate }}</div>
                </template>
              </el-table-column>
              <el-table-column
                prop="address"
                label="联系地址"
              >
                <template slot-scope="scope">
                  <div
                    class="ellipsis"
                    :title="scope.row.address"
                  >{{scope.row.address}}</div>
                </template>
              </el-table-column>
              <el-table-column
                fixed="right"
                label="操作"
                width="50"
              >
                <template slot-scope="scope">
                  <!-- <el-button
                    size="mini"
                    @click="handleEdit(scope.$index, scope.row)"
                    type="primary"
                    icon="el-icon-edit"
                    circle
                  ></el-button>-->
                  <!-- :disabled="btnAip.addclass.buttonCaption" -->

                  <el-tooltip
                    class="item"
                    effect="light"
                    :content="btnAip.check.buttonCaption"
                    placement="top-start"
                    :open-delay="200"
                  >
                    <el-button
                      type="primary"
                      icon="el-icon-search"
                      circle
                      :disabled="!btnAip.check.buttonCaption"
                      @click="handleEdit(scope.$index, scope.row)"
                    />
                  </el-tooltip>
                </template>
              </el-table-column>

              <el-table-column
                prop="headPicPath"
                label="头像路径"
              >
                <template slot-scope="scope">
                  <!-- <div
                    v-if="scope.row.iconFlag&&scope.row.headPicPath"
                    class="icon-item avatarTable"
                    v-html="scope.row.headPicPath"
                  />
                  <svg-icon
                    v-if="!scope.row.iconFlag&&scope.row.headPicPath"
                    :icon-class="scope.row.headPicPath"
                    class="avatarTable2"
                  /> -->
                  <div class="headPicPath_">
                    <img
                      :src="scope.row.headPicPath"
                      alt=""
                    >
                  </div>
                </template>
              </el-table-column>
              <el-table-column
                prop="remarks"
                label="备注"
                v-if="setFalse1"
              >
                <template slot-scope="scope">
                  <div class="ellipsis">{{scope.row.remarks}}</div>
                </template>
              </el-table-column>
            </el-table>
          </div>
          <el-dialog
            title="查看详情"
            :visible.sync="dialogTableVisible"
            width="30%"
            center
            :before-close="diaLogClose"
            :close-on-click-modal="false"
          >
            <el-form
              ref="form"
              :model="form"
              label-width="90px"
              style="width:100%"
            >
              <el-form-item label="账号姓名：">
                <el-input
                  v-model="form.accountName"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="真实姓名：">
                <el-input
                  v-model="form.realName"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="工号：">
                <el-input
                  v-model="form.jobNumber"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="手机账号：">
                <el-input
                  v-model="form.telAccount"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="邮箱账号：">
                <el-input
                  v-model="form.emailAccount"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="固定电话：">
                <el-input
                  v-model="form.fixedNumber"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="账号状态：">
                <el-input
                  readonly
                  v-model="form.status==1?'有效':'' || form.status==0?'无效':'' || form.status==2?'过期':''"
                ></el-input>
              </el-form-item>
              <el-form-item label="有效期：">
                <el-input
                  v-model="form.expDate"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item label="联系地址：">
                <el-input
                 type="textarea"
                  v-model="form.address"
                  readonly
                ></el-input>
              </el-form-item>
              <el-form-item
                label="头像："
                prop="handiMG"
              >
                <div class="iconfont">
                  <!-- <div v-if="iconFlag" class="icon-item" v-html="form.headPicPath" />
                  <svg-icon v-if="!iconFlag" :icon-class="form.headPicPath" /> -->
                  <img
                    :src="form.headPicPath"
                    alt=""
                  >
                </div>
              </el-form-item>
              <el-form-item label="备注：">
                <el-input
                 type="textarea"
                  v-model="form.remarks"
                  readonly
                ></el-input>
              </el-form-item>
            </el-form>
          </el-dialog>

          <!-- <el-pagination
            background
            layout="prev, pager, next"
            :page-size="pageSize"
            :total="totalCount"
            :current-page="pageIndex"
            @current-change="currentChange"
          /> -->
          <div id="elfooter">
            <Pagination
              :page-index="pageIndex"
              :total-count="total"
              :page-size="pageSize"
              @pagination="pagination"
            />
          </div>
        </el-col>
      </el-row>
    </el-main>
  </el-container>
</template>
<script>
const id = 1000;
import Pagination from "@/components/Pagination";
import AppTree from "@/utils/appTree";
import request from "@/utils/request";
import rquestObject from "@/utils/requestObject";
import { getBtnCtr } from "@/utils/BtnControl";
// import height from "@/utils/height";
export default {
  name: "viewsSysSettingsdeptAdminindexvue",
  components: {
    Pagination
  },
  data() {
    return {
      btnAip:"",
      loading:"",
      firstState: true,
      iconFlag: true,
      setImgFt: false,
      setFalse1: false,
      dialogTableVisible: false,
      setFalse: false,
      imgsrc: this.imgUrlName,
      tableData: [],
      selectedRow: null,
      pageSize: 25, // 分页显示记录条数
      total: 0, // 总记录数
      pageIndex: 1, // 页码
      tableDatanew: null,
      headerHeight: "50px",
      mainHeight: 0,
      treeHeight:0,
      footerHeight: "50px",
      datas: [],
      clickNodes: "",
      form: {
        accountName: "",
        address: "",
        cId: "",
        companyId: "",
        createTime: "",
        deptId: "",
        deptName: "",
        emailAccount: "",
        expDate: "",
        fixedNumber: "",
        headPicPath: "",
        id: "",
        jobNumber: "",
        keyWords: "",
        passwd: "",
        realName: "",
        remarks: "",
        roleId: "",
        roleName: "",
        salt: "",
        status: "",
        telAccount: "",
        userInfoId: ""
      }
    };
  },
  created() {
    // this.loading = true;
    this.initApptree();
    // this.getTree();
    this.setPages(15);
    this.setStyle();
    // 获取按钮权限
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    // this.loading = false;
  },
  filters: {
    formatTDate: value => {
      if (value != null) {
        var d = value.split("T");
      }
      return d ? d[0] : value;
    }
  },
  methods: {
    formatTDates(value) {
      if (value != null) {
        var d = value.split("T");
      }
      return d ? d[0] : value;
    },
    diaLogClose() {
      this.setImgFt = false;
      this.dialogTableVisible = false;
    },
    offImg() {
      this.setImgFt = false;
    },
    handleImg() {
      this.setImgFt = true;
    },
    handleEdit(index, row) {
      // console.log(index);
      // console.log(row);
      this.dialogTableVisible = true;
      this.form = {
        accountName: row.accountName,
        address: row.address,
        cId: row.cId,
        companyId: row.companyId,
        createTime: row.createTime,
        deptId: row.deptId,
        deptName: row.deptName,
        emailAccount: row.emailAccount,
        expDate: this.formatTDates(row.expDate),
        fixedNumber: row.fixedNumber,
        headPicPath: this.setLog(row.headPicPath),
        id: row.id,
        jobNumber: row.jobNumber,
        keyWords: row.keyWords,
        passwd: row.passwd,
        realName: row.realName,
        remarks: row.remarks,
        roleId: row.roleId,
        roleName: row.roleName,
        salt: row.salt,
        status: row.status,
        telAccount: row.telAccount,
        userInfoId: row.userInfoId
      };
    },
     /**
     * pagination
     * 分页信息
     */
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.nodeClick();
    },
    // currentChange(val) {
    //   this.pageIndex = val;
    //   //分页改变页数
    //   var QueryCondition = [
    //     {
    //       column: "deptid",
    //       content: this.clickNodes,
    //       condition: 0
    //     }
    //   ];
    //   var requsets = rquestObject.CreateGetObject(
    //     true,
    //     this.pageSize,
    //     this.pageIndex,
    //     QueryCondition,
    //     null
    //   );

    //   request({
    //     url: "system/api/TSMDept/GetDispathUserAsync",
    //     method: "GET",
    //     params: { requestObject: JSON.stringify(requsets) }
    //   }).then(res => {
    //     this.tableData = res.data;
    //   });
    // },
    getTree() {
      // 获取列表数据
      var robject = rquestObject.CreateGetObject();
      request({
        url: "/system/api/TSMUser",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
        // console.log(res.data);
        if (res.code === 0) {
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    GetTreeDataDisabled(data, level) {
      var treeObj = function(data, level) {
        var resObject = [];
        if (!data || data.length < 1) {
          return null;
        }
        data.forEach((v, i) => {
          // console.log(v.actualId);
          var thisId = v.actualId;
          var dis = false;
          if (v.actualId === -1) {
            thisId = "Tree" + level + "" + i;
            dis = true;
          }
          if (v.children && v.children.length > 0) {
            resObject.push({
              id: thisId,
              actualId: v.actualId,
              vitualId: v.vitualId,
              label: v.deptName,
              parentId: v.parentId,
              disabled: true,
              seqNumber: v.seqNumber,
              pathLogic: v.pathLogic,
              deptDesc: v.deptDesc,
              companyId: v.companyId,
              createId: v.createId,
              createTime: v.createTime,
              children: treeObj(v.children, level + 1)
            });
          } else {
            resObject.push({
              id: thisId,
              actualId: v.actualId,
              vitualId: v.vitualId,
              label: v.deptName,
              parentId: v.parentId,
              disabled: true,
              seqNumber: v.seqNumber,
              pathLogic: v.pathLogic,
              deptDesc: v.deptDesc,
              companyId: v.companyId,
              createId: v.createId,
              createTime: v.createTime,
              children: treeObj(v.children, level + 1)
            });
          }
        });
        return resObject;
      };
      return treeObj(data, level);
    },
    initApptree() {
      var rqs = rquestObject.CreateGetObject();
      request({
        url: "system/api/TSMDept/GetTree",
        method: "GET",
        params: { requestObject: JSON.stringify(rqs) }
      }).then(res => {
        // console.log(res);
        // console.log(res.data);
        this.datas = this.GetTreeDataDisabled(res.data);
        if (this.firstState) {
          this.firstState = false;
          this.$nextTick(() => {
            var listDom = document.querySelectorAll(
              "div.el-tree-node__content"
            );
            listDom[0].click();
          });
        }
      });
    },
    clickNode(val) {
      // console.log(val);
      this.$refs.multipleTable.toggleRowSelection(val);
    },
    selectNode(val) {
      // console.log(val, "val");
      // console.log(this.rightData);
      var dataRight = this.rightData;
    },
    handleSizeChange(val) {
      // console.log(`每页 ${val} 条`);
    },
    handleCurrentChange(val) {
      // console.log(`当前页: ${val}`);
    },
    setStyle() {
     this.$nextTick(() => {
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elfooter = document.getElementById("elfooter");
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        this.mainHeight = b - f - 40;
        this.treeHeight = "height:"+this.mainHeight+"px;"
      });
    },
    setPages(dataNumber) {
      // var dataNumber=data.length;
      // console.log(dataNumber);

      var total = Math.ceil(dataNumber / 20);
      // console.log(total);
    },

    /**
     * 删除左侧表格树结构数据
     */

    /**
     * 初始化应用
     */
    initialize() {
      var getse = rquestObject.CreateGetObject();
      request({
        url: "/platform/api/TMenus/GetTree",
        method: "get",
        params: {
          requestObject: JSON.stringify(getse),
          withKey: 2
        }
      }).then(res => {
        this.data = AppTree.ToTableTreeDataDisabled(res.data);
      });
    },
    // 取消
    OnBtnClose(event) {
      this.dialogVisible = false;
      this.dialogVisibleValue = false;
    },

    /**
     * 查询接口
     * 查询第四层和第五层的数据
     * 改动后请求数据接口未改动请求更改
     */
    nodeClick(data) {
      // console.log(data)
      // console.log(1)
      // this.loading = true;
      // console.log(data.actualId)
      if(data){
        this.clickNodes = data.actualId;
      }
      var postaData = {
        deptID: this.clickNodes
      };
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
      // console.log(data.actualId);
      request({
        url: "system/api/TSMDept/GetDispathUserAsync",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        // console.log(res);
        // console.log(res.data);
        this.tableData = this.handelData(res.data);
        this.pageSize = res.pageSize;
        this.pageIndex = res.pageIndex;
        // this.totalCount = res.totalNumber;
        this.total = res.totalNumber;
        this.loading = false;
      });
    },
    handelData(data) {
      data.map(item => {
        //   if (
        //     item.headPicPath != null &&
        //     item.headPicPath.indexOf("el-icon") != -1
        //   ) {
        //     item.iconFlag = true;
        //   } else {
        //     item.iconFlag = false;
        //   }
        if (item.headPicPath != null) {
          item.headPicPath = this.imgUrlName + item.headPicPath;
        }
      });
      return data;
    },
    setLog(data) {
      if (data != null && data.indexOf("el-icon") != -1) {
        this.iconFlag = true;
      } else {
        this.iconFlag = false;
      }
      return data;
    },
    stringLink(data) {
      var dataString;
      if (data.length > 36) {
        dataString = data.substring(0, 36) + "。。。";
        return dataString;
      } else {
        return data;
      }
    }
  }
};
</script>
<style lang="scss" scoped>
#elfooter {
  padding-top: 10px;
  height: 30px;
}
#deptAdmin /deep/ {
  .el-col-3 {
    width: 240px;
  }
  .el-col-21 {
    width: calc(100% - 240px);
    .rightBoxMain{
      padding-left: 20px;
    }
  }
}
.headPicPath {
  width: 40px;
  height: 40px;
  overflow: hidden;
  border-radius: 50%;
  img {
    width: 100%;
    height: 100%;
  }
}
.headPicPath_ {
  width: 30px;
  height: 30px;
  overflow: hidden;
  border-radius: 50%;
  vertical-align: middle;
  img {
    width: 100%;
    height: 100%;
  }
}
.iconfont {
  // font-size: 36px;
  // color: #8c939d;
  width: 100px;
  height: 100px;
  overflow: hidden;
  border-radius: 50%;
  // display: flex;
  // line-height: 54px;
  // text-align: center;
  // display: flex;
  img {
    width: 100%;
    height: 100%;
  }
  .iconfontBox {
    flex: 1;
    text-align: right;
    line-height: 0px;
    height: 26px;
  }
}
/deep/.el-tree-node__content {
  position: relative;
  .el-tree-node__label {
    font-size: 12px;
    color: #606266;
  }
}
/* /deep/.el-button--text{
  position: absolute;
  top: -3px;
  right: 30px;
} */
.deleat {
  position: absolute !important;
  top: 6px;
  right: 0px;
}
.custom-tree-node {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 14px;
  padding-right: 8px;
}

#e {
  width: 100%;
  // border: 1px solid rgb(230, 235, 245);
}
#bordes {
  width: 100%;
  height: 650px;
  border: 1px solid rgb(230, 235, 245);
}
#bordes > div {
  padding-left: 0px !important;
}
.el-icon-delete {
  color: red;
  font-size: 18px;
}
.avatarTable {
  width: 40px;
  height: 40px;
  border: 1px solid #dcdfe6;
}
.avatarTables {
  width: 300px;
  height: 300px;
  border: 1px solid #dcdfe6;
  position: absolute;
  top: -400px;
  left: 30px;
}
#setImg {
  position: absolute;
  top: 0px;
  right: 0px;
  width: 20px;
  height: 20px;
  text-align: center;
  line-height: 20px;
  background-color: rgb(37, 175, 243);
  cursor: pointer;
}

/deep/ .el-tree-node__content {
  padding: 20px 0px;
  font-size: 12px;
  border-bottom: 1px solid #dfe6ec;
  cursor: pointer;
}
.el-tree-node:focus > .el-tree-node__content {
  background-color: #fff;
}
#deptAdmin
  /deep/
  .el-tree--highlight-current
  .el-tree-node.is-current
  > .el-tree-node__content {
  background-color: rgb(245, 247, 250) !important;
}
#deptAdmin /deep/ .el-tree-node__label {
  color: #333333;
}
.avatarTable2 {
  width: 30px;
  height: 30px;
}
.avatarTable {
  width: 40px;
  height: 40px;
  font-size: 30px;
}
</style>

