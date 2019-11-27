<template>
  <el-container
    v-loading="loading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header id="elheader" class="headerBd" style="height:auto">
      <el-form inline>
        <el-form-item label="仓库编码：" label-width="76px">
          <el-input
            v-model="numSearch"
            placeholder="请输入仓库编码"
            clearable
            @clear="getMakeReport()"
            @keyup.enter.native="getMakeReport()"
          >
            <i slot="suffix" class="el-icon-search" @click="getMakeReport()" />
          </el-input>
        </el-form-item>
        <el-form-item label="仓库名称：" label-width="76px">
          <el-input
            v-model="nameSearch"
            placeholder="请输入仓库名称"
            clearable
            @clear="getMakeReport()"
            @keyup.enter.native="getMakeReport()"
          >
            <i slot="suffix" class="el-icon-search" @click="getMakeReport()" />
          </el-input>
        </el-form-item>
      </el-form>
      <el-button
        v-if="btnAip.query&&btnAip.query.buttonCaption"
        type="primary"
        @click="getMakeReport"
      >{{ btnAip.query.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.add&&btnAip.add.buttonCaption"
        type="primary"
        @click="handelAddClick"
      >{{ btnAip.add.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.batchdelete&&btnAip.batchdelete.buttonCaption"
        :type="typeColor"
        @click="handelBatchDelete"
      >{{ btnAip.batchdelete.buttonCaption }}</el-button>
      <!-- <el-button type="primary" @click="handelAddClick">新增</el-button>
      <el-button type="info" @click="handelBatchDelete">批量删除</el-button>-->
    </el-header>
    <el-main id="elmain">
      <el-table
        v-if="showtab"
        align="center"
        :data="warehouseData"
        style="width: 100%"
        border
        :height="mainHeight + 'px'"
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="50" />
        <el-table-column prop="code" label="仓库编号" width="100" />
        <!-- <el-table-column
          prop="warehouseName"
          label="仓库名称"
        />-->
        <el-table-column prop="warehouseName" label="仓库名称">
          <!-- <template slot-scope="scope">
            <el-popover
              placement="top-start"
              width="200"
              trigger="hover"
              :content="scope.row.warehouseName"
              v-if="scope.row.warehouseName.length>=20"
            >
              <div slot="reference" class="ellipsis">{{ scope.row.warehouseName }}</div>
            </el-popover>
            <div
              v-if="scope.row.warehouseName.length<20"
              class="ellipsis"
            >{{ scope.row.warehouseName }}</div>
          </template>-->
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.warehouseName&&scope.row.warehouseName.length>=20"
              class="item"
              effect="light"
              :content="scope.row.warehouseName"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.warehouseName }}</div>
            </el-tooltip>
            <div
              v-if="scope.row.warehouseName&&scope.row.warehouseName.length<20"
              class="ellipsis"
            >{{ scope.row.warehouseName }}</div>
          </template>
        </el-table-column>
        <!-- <el-table-column
          prop="warehouseAddress"
          label="仓库地址"
        />-->
        <el-table-column prop="warehouseAddress" label="仓库地址">
          <!-- <template slot-scope="scope"> -->
          <!-- <el-popover
              placement="top-start"
              width="200"
              trigger="hover"
              :content="scope.row.warehouseAddress"
              v-if="scope.row.warehouseAddress.length>=20"
            >
              <div slot="reference" class="ellipsis">{{ scope.row.warehouseAddress }}</div>
            </el-popover>
            <div
              v-if="scope.row.warehouseAddress.length<20"
              class="ellipsis"
            >{{ scope.row.warehouseAddress }}</div>
          </template>-->
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.warehouseAddress&&scope.row.warehouseAddress.length>=20"
              class="item"
              effect="light"
              :content="scope.row.warehouseAddress"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.warehouseAddress }}</div>
            </el-tooltip>
            <div
              v-if="scope.row.warehouseAddress&&scope.row.warehouseAddress.length<20"
              class="ellipsis"
            >{{ scope.row.warehouseAddress }}</div>
          </template>
        </el-table-column>

        <el-table-column prop="realName" label="负责人" width="90" />
        <el-table-column label="状态" width="50">
          <template slot-scope="scope">
            <i v-if="scope.row.status" class="el-icon-circle-check" />
            <!-- <i v-if="!scope.row.status" class="el-icon-circle-close" style="color:#909399" /> -->
            <span v-if="!scope.row.status" class="iconfont icon-Group-" style="font-size: 20px;" />
          </template>
        </el-table-column>
        <!-- <el-table-column
          prop="remark"
          label="备注"
        />-->
        <el-table-column prop="remark" label="备注" width="280">
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.remark.length>=20"
              class="item"
              effect="light"
              :content="scope.row.remark"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.remark }}</div>
            </el-tooltip>
            <div v-if="scope.row.remark.length<20" class="ellipsis">{{ scope.row.remark }}</div>
          </template>
        </el-table-column>

        <el-table-column label="操作" fixed="right" width="105">
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.edit.buttonCaption"
              class="item"
              effect="light"
              content="编辑"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-edit"
                circle
                @click="handelEditClick(scope.row)"
              />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.delete.buttonCaption&&showtips"
              class="item"
              effect="light"
              content="删除"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="danger"
                icon="el-icon-delete"
                circle
                @click="handelDelete(scope.row)"
              />
            </el-tooltip>
            <el-button
              v-if="btnAip.delete&&btnAip.delete.buttonCaption&&!showtips"
              type="danger"
              icon="el-icon-delete"
              circle
              @click="handelDelete(scope.row)"
            />
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <!-- 新增和编辑弹窗 -->
    <el-dialog
      :title="title"
      :visible.sync="warehouseVisible"
      width="40%"
      :close-on-click-modal="false"
    >
      <el-form ref="warehouseForm" :model="warehouseForm" :rules="warehouseRules">
        <el-form-item label="仓库编号：" prop="code" :label-width="formLabelWidth">
          <el-input placeholder="请输入仓库编号" v-model="warehouseForm.code" readonly />
        </el-form-item>
        <el-form-item label="仓库名称：" prop="warehouseName" :label-width="formLabelWidth">
          <el-input placeholder="请输入仓库名称" v-model="warehouseForm.warehouseName" />
        </el-form-item>
        <el-form-item label="仓库地址：" prop="warehouseAddress" :label-width="formLabelWidth">
          <el-input placeholder="请输入仓库地址" v-model="warehouseForm.warehouseAddress" />
        </el-form-item>
        <el-form-item label="负责人：" prop="principalId" :label-width="formLabelWidth">
          <el-select
            v-model="warehouseForm.principalId"
            filterable
            clearable
            placeholder="请选择"
            style="width:100%"
          >
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="状态：" prop="status" :label-width="formLabelWidth">
          <el-radio v-model="warehouseForm.status" label="true">启用</el-radio>
          <el-radio v-model="warehouseForm.status" label="false">停用</el-radio>
        </el-form-item>
        <el-form-item label="备注：" prop="remark" :label-width="formLabelWidth">
          <el-input placeholder="请输入备注" v-model="warehouseForm.remark" type="textarea" :rows="3" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="warehouseVisible = false">关闭</el-button>
        <el-button type="primary" :loading="saveBtn" @click="handelSave">保存</el-button>
      </div>
    </el-dialog>
    <!-- 分页 -->
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
import { getBtnCtr } from "@/utils/BtnControl";
import { formatDate, trim } from "@/utils/common.js";
import { getStyle } from "@/utils/Dom.js";

export default {
  name: "viewsReserveAdminWarehouseindexvue",
  components: {
    Pagination
  },
  data() {
    return {
      showtab: false,
      loading: true,
      saveBtn:false,
      btnAip: "",
      typeColor: "info",
      total: 0,
      headHeight: "50px",
      footHeight: "50px",
      mainHeight: "",
      datePicker: "",
      nameSearch: "",
      numSearch: "",
      pageSize: 25,
      pageIndex: 1,
      warehouseVisible: false,
      title: "新增仓库档案",
      method: "post",
      showtips: true,
      formLabelWidth: "100px",
      allSelectionChange: [],
      warehouseData: [],
      realNameOptions: [],
      postDataEdit: {},
      warehouseForm: {
        code: "",
        warehouseName: "",
        warehouseAddress: "",
        principalId: "",
        realName: "",
        status: "true",
        remark: "",
        id: ""
      },
      queryData: {
        code: "",
        warehouseName: ""
      },
      warehouseRules: {
        warehouseName: [
          { required: true, message: "请输入仓库名称", trigger: "blur" },
          {
            min: 1,
            max: 50,
            message: "最大允许输入50个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        warehouseAddress: [
          {
            required: false,
            message: "请输入仓库地址",
            trigger: ["blur", "change"]
          },
          {
            min: 1,
            max: 150,
            message: "最大允许输入150个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ],
        // principalId: [
        //   { required: true, message: '请选择负责人', trigger: ['blur', 'change'] }
        // ],
        remark: [
          {
            required: false,
            message: "请输入备注",
            trigger: ["blur", "change"]
          },
          {
            min: 1,
            max: 500,
            message: "最大允许输入500个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ]
      }
    };
  },
  watch: {
    warehouseData() {
      if (this.pageIndex > 1 && this.warehouseData.length == []) {
        this.pageIndex--;
        // 初始化表格数据
        this.getWarehouseData();
      }
    },
    warehouseVisible(val) {
      if (val) {
        // setTimeout(() => {
          // this.pswOnly = false;
        // }, 500);
         this.getUserCompany(); // 获取当前用户公司员工
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.warehouseForm.resetFields();
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );

    // 键盘返回键
    var lett = this;
    // document.onkeydown = function(e) {
    //   var key = window.event.keyCode;
    //   if (key == 13) {
    //     lett.getMakeReport();
    //   }
    // };
    this.getWarehouseData(); // 查看仓库档案
    this.setStyle(); // 设置页面样式
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
    /**
     * setStyle
     * 设置页面样式
     */
    setStyle() {
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
    /**
     * getUserCompany
     * 获取当前用户公司员工
     */
    getUserCompany() {
      var reqObject = RequestObject.LonginBookObject(
        false,
        this.pageSize,
        this.pageIndex,
        null,
        null
      );
      request({
        url: "/system/api/TSMUser/GetUserInCurentCompany",
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
          this.realNameOptions = res.data;
        }
      });
    },
    /**
     * getWarehouseData
     * 查看仓库档案
     */
    getWarehouseData() {
      var reqObject;
      var queryConditions = [];
      if (
        trim(this.queryData.code) != "" ||
        trim(this.queryData.warehouseName) != ""
      ) {
        if (trim(this.queryData.code) != "") {
          queryConditions.push({
            column: "code",
            content: this.queryData.code,
            condition: 6
          });
        }
        if (trim(this.queryData.warehouseName) != "") {
          queryConditions.push({
            column: "warehouseName",
            content: this.queryData.warehouseName,
            condition: 6
          });
        }
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          queryConditions
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
        url: "/basicset/api/TBMWarehouseFile/GetAll",
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
          this.warehouseData = res.data;
          this.total = res.totalNumber;
          // this.loading = false
        }
      });
    },
    /**
     * getMakeReport
     * 搜索
     */
    getMakeReport() {
      this.loading = true;
      // 条件查询（仓库编码，仓库名称）
      this.queryData.code = this.numSearch;
      this.queryData.warehouseName = this.nameSearch;
      this.getWarehouseData();
    },
    /**
     * GetCode
     * 获取仓库编码
     */
    GetCode() {
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        null,
        null
      );
      request({
        url: "/basicset/api/TBMWarehouseFile/GetCode",
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
          this.warehouseForm.code = res;
        }
      });
    },
    /**
     * handelEditClick
     * 编辑仓库档案
     */
    handelEditClick(row) {
      this.method = "put";
      this.postDataEdit = row;
      this.warehouseForm.id = row.id;
      this.warehouseForm.companyId = row.companyId;
      this.warehouseForm.code = row.code;
      this.warehouseForm.warehouseName = row.warehouseName;
      this.warehouseForm.warehouseAddress = row.warehouseAddress;
      this.warehouseForm.principalId = row.principalId;
      this.warehouseForm.realName = row.realName;
      this.warehouseForm.status = JSON.stringify(row.status);
      this.warehouseForm.remark = row.remark;
      this.warehouseVisible = true;
      this.title = "编辑仓库档案";
    },
    /**
     * handelAddClick
     * 新增仓库档案
     */
    handelAddClick() {
      this.method = "post";
      this.postDataEdit = null;
      this.warehouseForm.code = "";
      this.warehouseForm.warehouseName = "";
      this.warehouseForm.warehouseAddress = "";
      this.warehouseForm.principalId = "";
      this.warehouseForm.realName = "";
      this.warehouseForm.status = "true";
      this.warehouseForm.remark = "";
      this.warehouseForm.id = 0;
      this.GetCode();
      this.title = "新增仓库档案";
      this.warehouseVisible = true;
    },

    /**
     * handelSave
     * 保存仓库档案
     */
    handelSave() {
      this.$refs.warehouseForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
        } else {
          // this.loading = true;
          this.saveBtn = true;
          this.warehouseForm.status = JSON.parse(this.warehouseForm.status);
          var data = RequestObject.GetObject(
            this.warehouseForm,
            null,
            null,
            this.postDataEdit
          );
          request({
            url: "/basicset/api/TBMWarehouseFile",
            method: this.method,
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                if (res.info.indexOf("编号:") != -1) {
                  this.warehouseForm.status = JSON.stringify(
                    this.warehouseForm.status
                  );
                  this.GetCode();
                  this.handelSave();
                } else {
                 var setTime = setTimeout(()=>{
                    // this.loading = false;
                  this.warehouseForm.status = JSON.stringify(
                    this.warehouseForm.status
                  );
                  this.$confirm(res.info, "错误信息", {
                    confirmButtonText: "确定",
                    type: "error",
                    showCancelButton: false
                  });
                   this.saveBtn = false;
                   clearTimeout(setTime)
                 },50)
                }
              } else {
                var setTime = setTimeout(()=>{
                  this.warehouseVisible = false;
                this.getWarehouseData();
                this.$message({
                  message: "操作成功",
                  type: "success"
                });
                clearTimeout(setTime)
                },50)
              }
            })
            .catch(error => {
              // this.loading = false;
            });
        }
      });
    },
    /**
     * handleSelectionChange
     * 被勾选的数组
     */
    handleSelectionChange(val) {
      this.allSelectionChange = val;
      if (this.allSelectionChange.length == 0) {
        this.typeColor = "info";
      } else {
        this.typeColor = "danger";
      }
    },
    /**
     * handelBatchDelete
     * 批量删除
     */
    handelData() {
      return new Promise((resolve, reject) => {
        var data = [];
        for (var i = 0; i < this.allSelectionChange.length; i++) {
          var Object = {
            id: this.allSelectionChange[i].id,
            _LogName: this.allSelectionChange[i].warehouseName
          };
          data.push(Object);
        }
        resolve(data);
      });
    },
    async handelBatchDelete() {
      if (this.allSelectionChange.length == 0) {
        this.$message({
          message: "请选择要删除的仓",
          type: "error"
        });
        return;
      }
      var data = await this.handelData();
      var reqObject = RequestObject.GetObject(null, data, null, null);
      this.$confirm("是否删除？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.loading = true;
          // console.log(1)
          request({
            url: "/basicset/api/TBMWarehouseFile",
            method: "delete",
            data: reqObject
          })
            .then(res => {
              // console.log(3)
              if (res.code === -1) {
                this.loading = false;
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.getWarehouseData();
                this.$message({
                  message: "操作成功",
                  type: "success"
                });
              }
              this.loading = false;
              return;
            })
            .catch(error => {
              this.loading = false;
            });
        })
        .catch(() => {
          // console.log(2)
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    },
    /**
     * handelDelete
     * 删除仓库档案
     */
    handelDelete(row) {
      this.showtips = false;
      var data = {
        id: row.id,
        _LogName: row.warehouseName
      };
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        data,
        null
      );
      this.$confirm("是否删除" + row.warehouseName + "？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.showtips = true;
          this.loading = true;
          request({
            url: "/basicset/api/TBMWarehouseFile",
            method: "delete",
            data: reqObject
          })
            .then(res => {
              if (res.code === -1) {
                this.loading = false;
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.getWarehouseData();
                this.$message({
                  message: "删除成功",
                  type: "success"
                });
              }
            })
            .catch(error => {
              this.loading = false;
            });
        })
        .catch(() => {
          this.showtips = true;
          this.$message({
            type: "info",
            message: "已取消删除"
          });
        });
    },
    /**
     * pagination
     * 分页信息
     */
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getWarehouseData();
    }
  }
};
</script>

<style lang="scss" scoped>
@import "../../../styles/font/iconfont.css";
.el-header /deep/ {
  width: 100%;
  padding-top: 20px;
  // display: flex;
  .el-form {
    width: 100%;
    // display: flex;
    border-bottom: 1px solid #eee;
    // margin-bottom: 20px;
    .el-form-item {
      // flex: 1;
      // margin-right: 20px;
    }
  }
}
.el-main {
  i {
    font-size: 20px;
    text-align: center;
    color: #67c23a;
  }
}
/deep/ .el-dialog__title {
  color: #1890ff;
}
</style>
