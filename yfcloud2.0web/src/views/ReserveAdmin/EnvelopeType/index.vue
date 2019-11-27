<template>
  <el-container
    v-loading="loading"
     element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header id="elheader" class="headerBd" style="height:auto">
      <div class="headerbar">
        <el-button
          v-if="btnAip.add&&btnAip.add.buttonCaption"
          type="primary"
          @click="handelAddClick"
        >{{ btnAip.add.buttonCaption }}</el-button>
        <el-button
          v-if="btnAip.batchdelete&&btnAip.batchdelete.buttonCaption&&iconType==1"
          :type="typeColor"
          @click="handelBatchDelete"
        >{{ btnAip.batchdelete.buttonCaption }}</el-button>
        <div style="flex: 1;"></div>
        <div class="icon">
          <i
            @click="onIconType(1)"
            class="el-icon-s-operation"
            :style="iconType==1?'color:#1890ff':''"
          ></i>
          <i @click="onIconType(2)" class="el-icon-menu" :style="iconType==2?'color:#1890ff':''"></i>
        </div>
      </div>
    </el-header>
    <el-main id="elmain">
      <div class="tableBox" v-if="iconType==1">
        <el-table
          align="center"
          :data="packageData"
          style="width: 100%;"
          border
          :height="mainHeight + 'px'"
          @selection-change="handleSelectionChange"
          @sort-change="handelSortChange"
        >
          <el-table-column type="selection" width="50" />
          <el-table-column prop="dicCode" label="包型编号" width="100" />
          <el-table-column prop="dicValue" label="包型名称" width="260" />
          <el-table-column prop="createTime" label="创建时间" width="140" sortable>
            <template slot-scope="scope">{{scope.row.createTime|formatTDate}}</template>
          </el-table-column>
          <el-table-column prop="createName" label="创建人" />
          <el-table-column prop="updateTime" label="更新时间" width="140">
            <template slot-scope="scope">{{scope.row.updateTime|formatTDate}}</template>
          </el-table-column>
          <el-table-column prop="updateName" label="更新人" />
          <el-table-column prop="remark" label="备注" width="280">
            <!-- <template slot-scope="scope">
              <el-popover
                placement="top-start"
                width="200"
                trigger="hover"
                :content="scope.row.remark"
                v-if="scope.row.remark&&scope.row.remark.length>=20"
              >
                <div
                  slot="reference"
                  class="ellipsis"
                >{{ scope.row.remark }}</div>
              </el-popover>
              <div
                v-if="scope.row.remark&&scope.row.remark.length<20"
                class="ellipsis"
              >{{ scope.row.remark }}</div>
            </template>-->
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

          <el-table-column prop="imgPath" label="图片" width="70">
            <template slot-scope="scope">
              <!-- <div style="height: 50px;"> -->
              <el-tooltip placement="top" effect="light">
                <div slot="content">
                  <img class="imgPath" :src="scope.row.imgPath" alt />
                </div>
                <img class="imgPath" :src="scope.row.imgPath" alt />
              </el-tooltip>
              <!-- </div> -->
            </template>
          </el-table-column>

          <el-table-column label="操作" fixed="right" width="105">
            <template slot-scope="scope">
              <el-tooltip
                v-if="btnAip.edit&&btnAip.edit.buttonCaption"
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
                v-if="btnAip.delete&&btnAip.delete.buttonCaption&&showtips"
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
      </div>
      <div class="typeImg" v-if="iconType==2" :style="this.imgMainHeight">
        <div v-for="(imet,index) in packageData" :key="index" class="imgType">
          <el-card style="height: 300px" :body-style="{ padding: '0px' }">
            <!-- <div @click.stop="handelDelete(imet)" class="del">
              <i class="el-icon-error"></i>
            </div>-->
            <div class="wu">
              <i v-if="imet.imgPath==''" class="el-icon-goods"></i>
            </div>
            <div v-if="imet.imgPath!=''" class="image">
              <img :src="imet.imgPath" />
            </div>
            <div style="padding: 14px;">
              <div class="overflow-h" style="text-align: center;" v-if="imet.dicValue.length+imet.dicCode.length<20">
                {{imet.dicCode}}
                <span style="font-weight: 600; font-size: 18px;">{{imet.dicValue}}</span>
              </div>
              <el-tooltip
              v-if="imet.dicValue.length+imet.dicCode.length>=20"
              class="item"
              effect="light"
              :content="imet.dicCode + '  ' + imet.dicValue"
              :open-delay="300"
              placement="top-end"
            >
            <div class="overflow-h" style="text-align: center;">
               {{imet.dicCode}}
                <span style="font-weight: 600; font-size: 18px;">{{imet.dicValue}}</span>
            </div>
            </el-tooltip>
              <div class="bottom clearfix">
                <time class="time">{{imet.remark}}</time>
              </div>
            </div>
          </el-card>
        </div>
      </div>
    </el-main>
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
    <!-- 新增和编辑弹窗 -->
    <el-dialog :title="title" :visible.sync="packageVisible" width="40%" :close-on-click-modal="false">
      <el-form ref="packageForm" :model="packageForm" :rules="packageRules">
        <el-form-item label="包型编号：" prop="dicCode" :label-width="formLabelWidth">
          <el-input placeholder="请输入包型编号" v-model="packageForm.dicCode" />
        </el-form-item>
        <el-form-item label="包型名称：" prop="dicValue" :label-width="formLabelWidth">
          <el-input placeholder="请输入包型名称" v-model="packageForm.dicValue" />
        </el-form-item>
        <el-form-item label="图片：" prop="imgPath" :label-width="formLabelWidth">
          <el-upload
            class="avatar-uploader"
            :action="$ajaxUrl+'/fileupload/api/files/upload'"
            :show-file-list="false"
            :on-success="handleAvatarSuccess"
            :before-upload="beforeAvatarUpload"
          >
            <div class="img-box" v-if="packageForm.imgPath">
              <i
                @click.stop="deletedImg"
                class="el-icon-error"
                style="font-size: 30px; color: #ff4949;position: absolute;top: -13px;left: 155px;"
              ></i>
              <img :src="packageForm.imgPath" class="avatar" />
            </div>
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
        <el-form-item label="备注：" prop="remark" :label-width="formLabelWidth">
          <el-input placeholder="请输入备注" v-model="packageForm.remark" type="textarea" :rows="3" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="packageVisible = false">关闭</el-button>
        <el-button type="primary" :loading="saveBtn" @click="handelSave">保存</el-button>
      </div>
    </el-dialog>
  </el-container>
</template>
<script>
import Pagination from "@/components/Pagination";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { getBtnCtr } from "@/utils/BtnControl";
import { getStyle } from "@/utils/Dom.js";

export default {
  name: "viewsReserveAdminEnvelopeTypeindexvue",
  components: {
    Pagination
  },
  filters: {
    formatTDate: value => {
      if (value == "" || value == null) {
        return "";
      }
      const d = value.split("T");
      return `${d[0]} ${d[1]}`;
    }
  },
  data() {
    var codeName = (rule, value, callback) => {
      const codeReg = /[\u4E00-\u9FA5]|[\uFE30-\uFFA0]/gi;
      if (!value) {
        return callback(new Error("请输入包型编码"));
      }
      if (codeReg.test(value)) {
        callback(new Error("不能输入汉字"));
      } else {
        callback();
      }
    };
    return {
       sortColumn: '',
      loading: true,
      saveBtn:false,
      btnAip: "",
      typeColor: "info",
      total: 0,
      headHeight: "50px",
      footHeight: "50px",
      mainHeight: "",
      imgMainHeight: "",
      datePicker: "",
      nameSearch: "",
      numSearch: "",
      pageSize: 25,
      pageIndex: 1,
      iconType: 1,
      packageVisible: false,
      title: "新增包型",
      method: "post",
      showtips: true,
      formLabelWidth: "100px",
      allSelectionChange: [],
      packageData: [],
      realNameOptions: [],
      postDataEdit: {},
      packageForm: {
        dicCode: "",
        dicValue: "",
        remark: "",
        imgPath: "",
        _LogName: "",
        id: ""
      },
      packageRules: {
        dicCode: [
          { required: true, validator: codeName, trigger: "blur" },
          {
            min: 1,
            max: 20,
            message: "最大允许输入20个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        dicValue: [
          {
            required: true,
            message: "请输入包型名称",
            trigger: "blur"
          },
          {
            min: 1,
            max: 20,
            message: "最大允许输入20个字符，请重新输入！",
            trigger: "blur"
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
    packageData() {
      if (this.pageIndex > 1 && this.packageData.length == []) {
        this.pageIndex--;
        // 初始化表格数据
        this.getPackageData();
      }
    },
    packageVisible(val) {
      if (val) {
        setTimeout(() => {
          // this.pswOnly = false;
        }, 500);
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.packageForm.resetFields();
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.getPackageData(); // 查看包型
    this.setStyle(); // 设置页面样式
  },
  mounter() {
    // this.setStyle(); // 设置页面样式
  },
  methods: {
    /**
     * 上传图片
     */
     handelSortChange(currSort) {
      this.sortColumn = currSort.prop
      this.sortOrder = currSort.order == 'ascending' ? 'asc' : 'desc'
      this.getPackageData();
    },
    handleAvatarSuccess(res, file) {
      this.packageForm.imgPath = this.imgUrlName + res;
    },
    beforeAvatarUpload(file) {
      const isJPG = file.type === "image/jpeg";
      const isLt2M = file.size / 1024 / 1024 < 2;

      if (!isJPG) {
        this.$message.error("上传头像图片只能是 JPG 格式!");
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 2MB!");
      }
      return isJPG && isLt2M;
    },
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
        this.imgMainHeight =
          "height:" + this.mainHeight + "px;overflow-y:scroll;";
      });
    },
    /**
     * onIconType
     * 视图类型
     */
    onIconType(id) {
      this.iconType = id;
      (this.pageSize = 25), (this.pageIndex = 1), this.getPackageData();
    },
    /**
     * getPackageData
     * 查看包型
     */
    getPackageData() {
      this.loading = true;
     var orderConditions = [];
if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        })
      }else {
        orderConditions = [
          {
            column: "createTime",
            condition: 'desc'
          }
        ]
      }
      var reqObject = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        null,
        orderConditions
      );
      request({
        url: "/basicset/api/TBMPackage",
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
          this.packageData = res.data;
          this.total = res.totalNumber;
          // this.loading = false
        }
      });
    },
    /**
     * handelEditClick
     * 编辑包型
     */
    handelEditClick(row) {
      this.method = "put";
      this.postDataEdit = row;
      this.packageForm.id = row.id;
      this.packageForm.dicCode = row.dicCode;
      this.packageForm.dicValue = row.dicValue;
      this.packageForm.imgPath = row.imgPath;
      this.packageForm._LogName = row._LogName;
      this.packageForm.remark = row.remark;
      this.packageVisible = true;
      this.title = "编辑包型";
    },
    /**
     * handelAddClick
     * 新增包型
     */
    handelAddClick() {
      this.method = "post";
      this.postDataEdit = null;
      this.packageForm.dicCode = "";
      this.packageForm.dicValue = "";
      this.packageForm.imgPath = "";
      this.packageForm._LogName = "";
      this.packageForm.remark = "";
      this.packageForm.id = 0;
      this.title = "新增包型";
      this.packageVisible = true;
    },
    /**
     * deletedImg
     * 删除图片
     */
    deletedImg() {
      this.packageForm.imgPath = "";
    },
    /**
     * handelSave
     * 保存包型
     */
    handelSave() {
      this.$refs.packageForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
        } else {
          // this.loading = true;
          this.saveBtn = true;
          var data = RequestObject.GetObject(
            this.packageForm,
            null,
            null,
            this.postDataEdit
          );
          request({
            url: "/basicset/api/TBMPackage",
            method: this.method,
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                var setTime = setTimeout(()=>{
                  // this.loading = false;
                this.packageForm.status = JSON.stringify(
                  this.packageForm.status
                );
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
                this.saveBtn = false;
                clearTimeout(setTime)
                },50)
              } else {
               var setTime = setTimeout(()=>{
                  this.packageVisible = false;
                this.getPackageData();
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
            _LogName: this.allSelectionChange[i].dicValue
          };
          data.push(Object);
        }
        resolve(data);
      });
    },
    async handelBatchDelete() {
      if (this.allSelectionChange.length == 0) {
        this.$message({
          message: "请选择要删除的包型",
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
          request({
            url: "/basicset/api/TBMPackage",
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
                this.getPackageData();
                this.$message({
                  message: "操作成功",
                  type: "success"
                });
              }
            })
            .catch(error => {
              this.loading = false;
            });
            this.loading = false;
            return;
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    },
    /**
     * handelDelete
     * 删除包型
     */
    handelDelete(row) {
      this.showtips = false;
      var data = {
        id: row.id,
        _LogName: row.dicValue
      };
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        data,
        null
      );
      this.$confirm("是否删除" + row.dicValue + "？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.showtips = true;
          this.loading = true;
          request({
            url: "/basicset/api/TBMPackage",
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
                this.getPackageData();
                this.$message({
                  message: "操作成功",
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
            message: "已取消操作"
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
      this.getPackageData();
    }
  }
};
</script>

<style lang="scss" scoped>
.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178px;
  height: 178px;
  line-height: 178px;
  text-align: center;
  border: 1px dashed #d9d9d9;
}
.avatar {
  width: 178px;
  height: 178px;
  display: block;
}

.time {
  font-size: 13px;
  color: #999;
}

.bottom {
  margin-top: 13px;
  line-height: 12px;
}

.button {
  padding: 0;
  float: right;
}
.image {
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}
.el-header /deep/ {
  width: 100%;
  padding-top: 20px;
  .headerbar {
    display: flex;
    height: 28px;
    .icon {
      text-align: right;
      font-size: 30px;
      height: 40px;
      margin-bottom: 20px;
      i {
        width: 40px;
      }
    }
  }
}
.el-main {
  .tableBox {
    .img-box {
      width: 100px;
      height: 100px;
      position: relative;
      .el-icon-error {
        position: absolute;
        top: 0px;
        left: 100px;
      }
      img {
        width: auto;
        height: auto;
      }
    }
    .imgPath {
      width: 30px;
      height: 30px;
      vertical-align: middle;
    }
  }
  .imgBox {
    width: 100px;
    height: 100px;
    border: 1px solid #dcdfe6;
    text-align: center;
    line-height: 100px;
    i {
      font-size: 30px;
    }
  }
  .typeImg {
    display: flex;
    flex-wrap: wrap;
    max-width: 1720px;
    margin: 0 auto;

    .imgType {
      // text-align: center;
      width: 300px;
      height: 300px;
      margin: 0 15px;
      margin-bottom: 40px;
      .el-card {
        // position: relative;
        .wu {
          i {
            width: 300px;
            height: 200px;
            text-align: center;
            line-height: 200px;
            font-size: 67px;
            border-bottom: 1px solid #eee;
          }
        }
        .image {
          width: 300px;
          height: 200px;
          border-bottom: 1px solid #eee;
          img {
            width: 300px;
            max-height: 200px;
          }
        }
      }
      .time {
        text-overflow: -o-ellipsis-lastline;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        line-clamp: 2;
        -webkit-box-orient: vertical;
        line-height: 16px;
      }
      .el-card__body {
        height: 300px !important;
      }
    }
  }
}
/deep/ .el-dialog__title {
  color: #1890ff;
}
.overflow-h {
  overflow:hidden; //超出的文本隐藏
  text-overflow:ellipsis; //溢出用省略号显示
  white-space:nowrap; //溢出不换行
}
</style>
