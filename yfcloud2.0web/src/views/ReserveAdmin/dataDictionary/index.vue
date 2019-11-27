<template>
  <el-container v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-main>
      <el-row  class="clr">
        <div class="leftBox">
          <el-button
            type="primary"
            @click="addTableDatasClick"
            v-if="btnAip.menuadd&&btnAip.menuadd.buttonCaption"
          >{{btnAip.menuadd.buttonCaption}}</el-button>
          <div id="bordes" class="leftBoxCon">
            <el-table
              ref="singleTable"
              :data="tableData"
              :height="mainHeight"
              style="width: 100%;cursor:pointer"
              @cell-click="tableCell"
              highlight-current-row
              @header-click="clickHead"
              @current-change="handleCurrentChange"
              id="typeListTable"
            >
              <el-table-column prop="typeName" label="分类名称"></el-table-column>
              <el-table-column prop="companyId" label="企业ID" v-if="false"></el-table-column>
              <el-table-column prop="id" label="id" v-if="false"></el-table-column>
            </el-table>
          </div>
        </div>
        <!-- 新增第一value -->
        <div class="rightBox">
          <el-button
            type="primary"
            @click="clickAddTableDatas"
            v-if="btnAip.add&&btnAip.add.buttonCaption"
          >{{btnAip.add.buttonCaption}}</el-button>
          <el-button
            :type="infos"
            @click="clickDeleatAllTableDatas"
            v-if="btnAip.batchdelete&&btnAip.batchdelete.buttonCaption"
          >{{btnAip.batchdelete.buttonCaption}}</el-button>
          <!-- <el-input
            v-model="input"
            placeholder="请输入编号/名称"
            style="width:20%;float:right;"
            @keyup.enter.native="enterTableDatasSearch"
          >
            <i class="el-icon-zoom-in el-input__icon" slot="suffix" @click="tableDatasSearch"></i>
          </el-input>-->
          <div id="e" style="width: 100%;margin-top:15px">
            <el-table
              ref="multipleTable"
              :data="tableDatas"
              border
              style="width: 100%;cursor:pointer"
              :height="mainHeight"
              @selection-change="handleSelectionChange"
              id="tableDatas"
              @sort-change="sortChange"
            >
              <el-table-column type="selection" width="55"></el-table-column>
              <!-- <el-table-column
                prop="dicCode"
                label="编号"
                width="100"
              ></el-table-column>-->
              <!-- <el-table-column prop="dicCode" label="编号" width="100" v-else></el-table-column> -->
              <el-table-column prop="dicValue" label="名称" width="110"></el-table-column>
              <el-table-column
                prop="createTime"
                label="创建时间"
                width="260"
                sortable="custom"
                :formatter="dateFormat"
              ></el-table-column>
              <el-table-column prop="createName" label="创建人" width="110"></el-table-column>
              <el-table-column prop="updateTime" label="更新时间" width="260" :formatter="dateFormats"></el-table-column>
              <el-table-column prop="updateName" label="更新人" width="110"></el-table-column>
              <el-table-column prop="remark" label="备注" :formatter="valueLength"></el-table-column>

              <el-table-column label="操作" fixed="right" width="100">
                <template slot-scope="scope">
                  <el-tooltip
                   v-if="btnAip.edit&&btnAip.edit.buttonCaption"
                    class="item"
                    effect="light"
                    :content="btnAip.edit.buttonCaption"
                    placement="top-start"
                    :open-delay="200"
                  >
                    <el-button
                      type="primary"
                      icon="el-icon-edit"
                      circle
                      @click="handleTableEdit(scope.$index,scope.row)"
                    />
                  </el-tooltip>
                  <el-tooltip
                  v-if="btnAip.delete&&btnAip.delete.buttonCaption"
                    class="item"
                    effect="light"
                    :content="btnAip.delete.buttonCaption"
                    placement="top-start"
                    :open-delay="200"
                  >
                    <el-button
                      type="danger"
                      icon="el-icon-delete"
                      circle
                      @click="handleTableDelete(scope.$index,scope.row)"
                    />
                  </el-tooltip>
                </template>
              </el-table-column>
            </el-table>
          </div>
          <el-dialog
            :title="variable"
            :visible.sync="dialogTableVisible"
            width="30%"
            left
            :before-close="handleClose"
            :close-on-click-modal="false"
          >
            <el-form ref="form" :model="form" label-width="90px" style="width:100%" :rules="forms">
              <!-- <el-form-item
                label="编号："
                prop="accountName"
                v-if="fTName"
              >
                <el-input v-model="form.accountName"></el-input>
              </el-form-item>
              <el-form-item
                v-else
                label="编号："
                prop="accountNames"
              >
                <el-input
                  v-model="form.accountNames"
                  style="border:none !important"
                  id="inpt"
                ></el-input>
                <div style="width:446px;height:32px;text-align:center">{{form.accountName}}</div>
              </el-form-item>-->
              <el-form-item label="名称：" prop="realNames" v-if="tableColor">
                <el-input v-model="form.realNames"></el-input>
              </el-form-item>
              <el-form-item label="名称：" prop="realName" v-else>
                <el-input placeholder="请输入名称" v-model="form.realName"></el-input>
              </el-form-item>
              <el-form-item label="备注：" prop="textarea">
                <el-input type="textarea" :rows="2" placeholder="请输入内容" v-model="form.textarea"></el-input>
              </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
              <el-button @click="dialogTableVisibleCancel">关闭</el-button>
              <el-button type="primary" :loading="saveTableBtn" @click="dialogTableVisibleConfirm">保存</el-button>
            </div>
          </el-dialog>
         
        </div>
      </el-row>
      <div>
       <Pagination
            :pageIndex="pageIndex"
            :totalCount="totalCount"
            :pageSize="pageSize"
            @pagination="sonZhi"
          />
    </div>
    </el-main>
    
    

    <el-dialog title="新增分类" :visible.sync="dialogTableDatas" width="30%">
      <el-form
        ref="formTableDatas"
        :model="formTableDatas"
        label-width="100px"
        style="width:100%"
        :rules="formTableDatasRules"
        :close-on-click-modal="false"
      >
        <el-form-item label="分类名称：" prop="CompeName">
          <el-input placeholder="请输入内容" v-model="formTableDatas.CompeName"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="clickHandeladdTableDatasCancel">取消</el-button>
        <el-button type="primary" :loading="saveBtn" @click="clickHandeladdTableDatas">保存</el-button>
      </div>
    </el-dialog>
  </el-container>
</template>
<script>
const id = 1000;
import Pagination from "@/components/Pagination";
import request from "@/utils/request";
import rquestObject from "@/utils/requestObject";
import height from "@/utils/height";
import { getBtnCtr } from "@/utils/BtnControl";
export default {
  name: "viewsReserveAdmindataDictionaryindexvue",
  data() {
    var codeName = (rule, value, callback) => {
      const codeReg = /[\u4E00-\u9FA5]|[\uFE30-\uFFA0]/gi;
      if (!value) {
        return callback(new Error("请输入编号"));
      }
      if (codeReg.test(value)) {
        callback(new Error("不能输入汉字"));
      } else {
        callback();
      }
    };
    return {
      loading: false,
      saveBtn: false,
      saveTableBtn:false,
      orderArr: "desc",
      cellNume: 0,
      infos: "info",
      currentRow: null,
      btnAip: "",
      londing: false,
      showIs: false,
      deleatDataList: [],
      sonId: "",
      typeId: "",
      formTableDatas: {
        CompeName: ""
      },
      dialogTableDatas: false,
      input: "",
      tableDatas: [],
      tableData: [],
      variable: "",
      imgsrc: this.imgUrlName,
      dialogTableVisible: false,
      treeDivStyle: "",
      selectedRow: null,
      pageSize: 25, // 分页显示记录条数
      totalCount: 10, // 总记录数
      pageIndex: 1, // 页码
      tableDatanew: null,
      tableDataName: "",
      headerHeight: "50px",
      mainHeight: 0,
      footerHeight: "50px",
      fTName: false,
      tableColor: false,
      formNaems: "编号89757",
      form: {
        accountName: "编号89757",
        realName: "",
        realNames: "",
        textarea: "",
        accountNames: ""
      },
      formTableDatasRules: {
        CompeName: [
          { required: true, message: "请输入名称", trigger: "blur" },
          { min: 0, max: 6, message: "长度在 0 到 6 个字符", trigger: "blur" }
        ]
      },
      forms: {
        realName: [
          { required: true, message: "请输入名称", trigger: "blur" },
          { min: 0, max: 50, message: "长度在 0 到 50 个字符", trigger: "blur" }
        ],

        accountName: [
          { required: true, validator: codeName, trigger: "blur" },
          { min: 1, max: 10, message: "长度在 0 到 10 个字符", trigger: "blur" }
        ],
        realNames: [
          {
            required: true,
            message: "请输入字符在0到10个字符",
            trigger: "blur"
          },
          {
            min: 0,
            max: 10,
            message: "长度在 0 到 10 个字符",
            trigger: "blur"
          }
        ],
        textarea: [
          {
            min: 0,
            max: 200,
            message: "长度在 0 到 200 个字符",
            trigger: "blur"
          }
        ]
      }
    };
  },
  components: {
    Pagination
  },
  created() {
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.mainHeight = this.footerHeight - 150 - parseInt(this.headerHeight);
    // this.inittableDatas();
    this.initApptree();
    this.setStyle();
    // this.requestTBMDictionary(1);
  },
  mounted() {},
  filters: {
    formatTDate: value => {
      if (value != null) {
        var d = value.split("T");
      }
      return d ? d[0] : value;
    }
  },
 watch: {
    dialogTableDatas(val){
      if(!val) {
        this.saveBtn = false;
      }
    }
  },
  methods: {
    setCurrent() {
      this.$refs.singleTable.setCurrentRow();
    },
    handleCurrentChange(val) {
      this.currentRow = val;
    },
    clickHead() {
      this.cellNume = 0;
      this.infos = "info";
      this.setCurrent();
      this.inittableDatas();
      this.tableDataName = "";
    },
    valueLength(row, column, cellValue, index) {
      if (row.remark.length > 20) {
        return row.remark.slice(0, 20) + "...";
      } else {
        return row.remark;
      }
    },
    dateFormat(row, column, cellValue, index) {
      var date = row[column.property];
      var date1 = new Date(+new Date(date) + 8 * 3600 * 1000)
        .toISOString()
        .replace(/T/g, " ")
        .replace(/\.[\d]{3}Z/, "");
      return date1;
    },
    dateFormats(row, column, cellValue, index) {
      if (row.updateTime == null) {
        return;
      } else {
        var date = row[column.property];
        var date1 = new Date(+new Date(date) + 8 * 3600 * 1000)
          .toISOString()
          .replace(/T/g, " ")
          .replace(/\.[\d]{3}Z/, "");
        return date1;
      }
    },
    /**
     * clickDeleatAllTableDatas
     * 批量删除
     */
    clickDeleatAllTableDatas() {
      if (this.deleatDataList.length == 0) {
        this.$message.error("请选择要删除的选项");
        return;
      } else {
        this.$confirm("是否删除？", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {
            this.loading = true;
            // console.log(this.cellNume);
            if (this.cellNume == 0) {
              var datts = rquestObject.CreatePostObject(
                null,
                this.deleatDataList
              );
              request({
                url: "/basicset/api/TBMDictionary",
                method: "DELETE",
                data: datts
              }).then(res => {
                if (res.code == -1) {
                } else {
                  this.$message({
                    type: "success",
                    message: "操作成功!"
                  });
                  // this.inittableDatas();
                  this.requestTBMDictionary(this.typeId);
                  this.dialogTableVisible = false;
                  this.deleatDataList = [];
                }
              });
            } else {
              var datts = rquestObject.CreatePostObject(
                null,
                this.deleatDataList
              );
              request({
                url: "/basicset/api/TBMDictionary",
                method: "DELETE",
                data: datts
              }).then(res => {
                if (res.code == -1) {
                } else {
                  this.$message({
                    type: "success",
                    message: "操作成功!"
                  });

                  this.requestTBMDictionary(this.typeId);
                  this.dialogTableVisible = false;
                  this.deleatDataList = [];
                }
              });
            }
            this.loading = false;
            return;
          })
          .catch(() => {
            this.$message({
              type: "info",
              message: "已取消操作"
            });
          });
      }
    },
    /**
     * handleSelectionChange
     * 批量删除数据
     *
     */
    handleSelectionChange(val) {
      if (val.length > 0) {
        this.infos = "danger";
        var arrys = val;
        var pushArr = [];
        for (var i = 0; i < arrys.length; i++) {
          var obj = {
            id: arrys[i].id,
            _LogName: "删除数据" + arrys[i].dicValue
          };
          pushArr.push(obj);
        }
        this.deleatDataList = pushArr;
      } else {
        this.infos = "info";
        this.deleatDataList = [];
      }
    },
    /**
     * 初始化应用
     */
    inittableDatas() {
      var getse = rquestObject.CreateGetObjectFenye(true);
      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { requestObject: JSON.stringify(getse) }
      }).then(res => {
        if (res.code == -1) {
          this.$message({
            message: "请稍等",
            type: "success"
          });
          this.inittableDatas();
        } else {
          // this.pageSize = res.pageSize;
          this.totalCount = res.totalNumber;
          // this.pageIndex = res.pageIndex;
          this.tableDatas = res.data;
        }
        // this.data = AppTree.ToTableTreeDataDisabled(res.data);
      });
    },
    /**
     * 初始化分类数据
     * initApptree
     */
    initApptree() {
      this.loading = true
      var rqs = rquestObject.CreateGetObject();

      request({
        url: "/basicset/api/TBMDictionaryType",
        method: "GET",
        params: { requestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code == -1) {
           this.loading = false;
          this.$message({
            message: "请稍等",
            type: "success"
          });
          this.initApptree();
        } else {
          this.tableData = res.data;
          this.loading = false;
        }
      });
    },
    clickHandeladdTableDatasCancel() {
      this.loading = false;
      this.dialogTableDatas = false;
    },
    /**
     * clickHandeladdTableDatas
     * 点击缺点新增左侧分类名称
     */
    clickHandeladdTableDatas() {
      this.$refs.formTableDatas.validate(valid => {
        if (valid) {
          // this.loading = true;
          this.saveBtn = true;
          var compName = this.formTableDatas.CompeName;
          var postData = {
            id: 0,
            typeName: compName,
            companyId: 0
          };
          var datts = rquestObject.CreatePostObject(postData);
          request({
            url: "/basicset/api/TBMDictionaryType",
            method: "POST",
            data: datts
          }).then(res => {
            if (res.code == -1) {
              var setTime = setTimeout(()=>{
                // this.loading = false;
                // this.$message.error("分类名称重复请重新输入");
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
                this.saveBtn = false;
                clearTimeout(setTime);
              },50)
            } else {
              var setTime = setTimeout(()=>{
                // this.loading = false;
              this.dialogTableDatas = false;
              this.initApptree();
               clearTimeout(setTime);
              },50)
            }
          });
        } else {
          this.$message.error("数据不合法，无法保存");
          return false;
        }
      });
    },
    /**
     * addTableDatasClick
     * 新增分类名称
     *
     */
    addTableDatasClick() {
      this.formTableDatas.CompeName = "";
      this.dialogTableDatas = true;
    },
    enterTableDatasSearch() {
      this.tableDatasSearch();
    },
    sonZhi(data) {
      if (this.tableDataName == "") {
        this.pageSize = data.pageSize;
        this.pageIndex = data.pageIndex;
        var requestObje = {
          isPaging: true,
          pageSize: data.pageSize,
          pageIndex: data.pageIndex,
          postData: null,
          postDataList: null,
          queryConditions: null,
          orderByConditions: [
            { column: "createTime", condition: this.orderArr }
          ]
        };
        request({
          url: "/basicset/api/TBMDictionary",
          method: "GET",
          params: {
            requestObject: requestObje
          }
        }).then(res => {
          this.tableDatas = res.data;
        });
      } else {
        var QueryCondition = [
          {
            column: "typeId",
            content: this.typeId,
            condition: 0
          }
        ];
        this.pageSize = data.pageSize;
        this.pageIndex = data.pageIndex;
        var requestObje = {
          isPaging: true,
          pageSize: data.pageSize,
          pageIndex: data.pageIndex,
          postData: null,
          postDataList: null,
          queryConditions: QueryCondition,
          orderByConditions: [
            { column: "createTime", condition: this.orderArr }
          ]
        };
        request({
          url: "/basicset/api/TBMDictionary",
          method: "GET",
          params: {
            requestObject: requestObje
          }
        }).then(res => {
          this.tableDatas = res.data;
        });
      }
    },
    /**
     * tableDatasSearch
     * 表查询
     */
    tableDatasSearch() {},
    /**
     * handleTableDelete
     * 删除对应项的表格数据
     *
     */
    handleTableDelete(index, row) {
      if (this.tableDataName == "") {
        this.$message.error("请选择相应的分类名称");
      } else {
        this.$confirm("是否删除" + row.dicValue + "？", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        })
          .then(() => {
            var postDat = {
              id: row.id,
              _LogName: this.tableDataName
            };
            var datts = rquestObject.CreatePostObject(postDat);
            request({
              url: "/basicset/api/TBMDictionary",
              method: "DELETE",
              data: datts
            }).then(res => {
              if (res.code == -1) {
              } else {
                this.$message({
                  type: "success",
                  message: "操作成功!"
                });
                this.requestTBMDictionary(this.typeId);
                this.dialogTableVisible = false;
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
    },
    /**
     * dialogTableVisibleConfirm
     * 新增保存
     */
    dialogTableVisibleConfirm() {
        if (this.variable == "新增") {
          this.$refs.form.validate(valid => {
            if (valid) {
              this.saveTableBtn = true;
              var postDates = {
                id: 0,
                typeId: this.typeId,
                dicCode: this.form.accountName,
                dicValue: this.form.realName
                  ? this.form.realName
                  : this.form.realNames,
                remark: this.form.textarea,
                _LogName: this.tableDataName
              };
              var resq = {
                isPaging: true,
                pageSize: 25,
                pageIndex: 1,
                postData: postDates,
                postDataList: null,
                queryConditions: null,
                orderByConditions: null
              };
              var datts = rquestObject.CreatePostObject(postDates);
              request({
                url: "/basicset/api/TBMDictionary",
                method: "POST",
                data: resq
              }).then(res => {
                if (res.code == -1) {
                  // this.$message.error(res.info);
                  this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
                } else {
                  this.dialogTableVisible = false;
                  this.$message({
                    message: "操作成功",
                    type: "success"
                  });
                  this.requestTBMDictionary(this.typeId);
                }
              });
            } else {
              // this.$message.error("请填写好相关信息");
              this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
              return false;
            }
          });
        } else if (this.variable == "编辑") {
          this.$refs.form.validate(valid => {
            if (valid) {
              this.saveTableBtn = true;
              var postDates = {
                id: this.sonId,
                typeId: this.typeId,
                dicCode: this.form.accountName,
                dicValue: this.form.realName
                  ? this.form.realName
                  : this.form.realNames,
                remark: this.form.textarea,
                _LogName: this.tableDataName
              };
              var datts = rquestObject.CreatePostObject(postDates);
              request({
                url: "/basicset/api/TBMDictionary",
                method: "PUT",
                data: datts
              }).then(res => {
                if (res.code == -1) {
                  // this.$message.error(res.info);
                  this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
                  return;
                } else {
                  this.$message({
                    message: "操作成功",
                    type: "success"
                  });
                  this.requestTBMDictionary(this.typeId);
                  this.dialogTableVisible = false;
                }
              });
              // this.dialogTableVisible = false;
            } else {
              // this.$message.error("请填写好相关信息");
              this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
              return false;
            }
          });
        }
         var setTime = setTimeout(() => {
        this.saveTableBtn = false;
        clearTimeout(setTime)
      }, 50);
    },
    /**
     * dialogTableVisibleCancel
     * 新增取消
     *
     */
    handleClose() {
      this.dialogTableVisibleCancel();
    },
    dialogTableVisibleCancel() {
      this.$refs.form.resetFields();
      this.dialogTableVisible = false;
    },
    clickAddTableDatas() {
      this.variable = "新增";
      if (this.tableDataName == "品牌设置") {
        this.fTName = true;
        this.form = {
          accountName: "",
          realName: "",
          textarea: "",
          realNames: "",
          accountNames: ""
        };

        this.tableColor = false;
        this.dialogTableVisible = true;
      } else if (this.tableDataName == "") {
        this.$message.error("请先选择分类名称栏再添加");
        return;
      } else if (this.tableDataName == "颜色档案") {
        this.fTName = false;
        request({
          url: "/basicset/api/TBMDictionary/GerRandom",
          method: "Get"
        }).then(res => {
          this.form = {
            accountName: "",
            realName: "",
            realNames: "",
            textarea: "",
            accountNames: res
          };
        });
        this.tableColor = true;
        this.dialogTableVisible = true;
      } else {
        this.fTName = false;

        request({
          url: "/basicset/api/TBMDictionary/GerRandom",
          method: "Get"
        }).then(res => {
          this.form = {
            accountName: "",
            realName: "",
            realNames: "",
            textarea: "",
            accountNames: res
          };
        });
        this.dialogTableVisible = true;

        this.tableColor = false;
      }
    },
    handleTableEdit(index, row) {
      this.variable = "编辑";
      this.sonId = row.id;

      /**
       * row的值传递到 this.form对应的属性
       */
      if (this.tableDataName == "品牌设置") {
        this.form = {
          accountName: row.dicCode,
          realName: row.dicValue,
          textarea: row.remark,
          realNames: ""
        };
        this.fTName = true;
        this.tableColor = false;
        this.dialogTableVisible = true;
      } else if (this.tableDataName == "") {
        this.$message.error("请先选择分类名称栏再添加");
        return;
      } else if (this.tableDataName == "颜色档案") {
        this.form = {
          accountName: row.dicCode,
          realNames: row.dicValue,
          textarea: row.remark,
          realName: ""
        };
        this.fTName = false;
        this.tableColor = true;
        this.dialogTableVisible = true;
      } else {
        this.form = {
          accountName: row.dicCode,
          realName: row.dicValue,
          textarea: row.remark,
          realNames: ""
        };
        this.dialogTableVisible = true;
        this.fTName = false;
        this.tableColor = false;
      }
    },
    sortChange({ column, prop, order }) {
      this.orderArr = [];
      if (order === "ascending") {
        this.orderArr = "asc";
        this.requestTBMDictionary(this.typeId);
      }
      if (order === "descending") {
        this.orderArr = "desc";
        this.requestTBMDictionary(this.typeId);
      }
    },
    requestTBMDictionary(id) {
      var QueryCondition = [
        {
          column: "typeId",
          content: id,
          condition: 0
        }
      ];
      var requsets = rquestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        QueryCondition,
        [{ column: "createTime", condition: this.orderArr }]
      );
      request({
        url: "/basicset/api/TBMDictionary",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$message({
            message: "请稍等",
            type: "success"
          });
          // this.tableCell();
        } else {
          // console.log(res)
          // this.pageSize = res.pageSize;
          this.totalCount = res.totalNumber;
          // this.pageIndex = res.pageIndex;
          this.tableDatas = res.data;
        }
      });
    },
    tableCell(row, column, cell, event) {
      this.pageIndex = 1;
      this.cellNume = 0;
      this.infos = "info";
      // console.log(row.id);
      this.tableDataName = row.typeName;
      if (row.typeName == "品牌设置") {
        this.showIs = true;
      } else {
        this.showIs = false;
      }
      this.typeId = row.id;
      this.requestTBMDictionary(this.typeId);
    },
    setStyle() {
      this.mainHeight = height - 150 - parseInt(this.headerHeight);
      this.treeStyle =
        "width:236px;height:" +
        (this.mainHeight - 4).toString() +
        "px;padding:0px;";
      this.treeDivStyle =
        "width:221px;height:" +
        this.mainHeight.toString() +
        "px;padding:0px;background:drgb(230,235,245);";
    }
  }
};
</script>
<style lang="scss" scoped>
/deep/.el-dialog__title {
  color: #1890ff;
}
/deep/.el-table--group,
.el-table--border {
  border-bottom: 0px solid #ccc;
}
/deep/#inpt {
  border: none !important;
}
/deep/.el-table__fixed-body-wrapper {
  // top: 41px !important;
}
#typeListTable /deep/ .el-table__header-wrapper,
.el-table__footer-wrapper {
  // overflow-y: auto;
}
.leftBox {
  width: 240px;
  float: left;
}
.leftBoxCon {
  margin-top: 15px;
  width: 240px;
  height: 100%;
  padding: 0px;
}
.rightBox {
  width: calc(100% - 260px);
  margin-left: 20px;
  float: left;
}
.iconfont {
  font-size: 36px;
  color: #8c939d;
  width: 100%;
  height: 44px;
  display: flex;
  // line-height: 54px;
  text-align: center;
  display: flex;
  .iconfontBox {
    flex: 1;
    text-align: right;
    line-height: 0px;
    height: 26px;
  }
}
/deep/.el-tree-node__content {
  position: relative;
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
  // height: 650px;
  border: 1px solid rgb(230, 235, 245);
  border-bottom: 0px solid rgb(230, 235, 245);
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
.avatarTable2 {
  width: 30px;
  height: 30px;
}
.avatarTable {
  width: 40px;
  height: 40px;
  font-size: 30px;
}
/deep/ .el-pagination{
  margin-top:10px;
}
/deep/ .el-main{
  padding-bottom:0px;
}
/deep/ #tableDatas.el-table--small th {
    padding: 2px 0;
}
</style>

