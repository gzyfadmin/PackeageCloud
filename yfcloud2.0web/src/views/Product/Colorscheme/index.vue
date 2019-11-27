<template>
  <el-container
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-main>
      <el-row>
        <el-col :span="4" class="header headerBd">
          <div style=" border-bottom: 1px solid #eee; margin-bottom: 15px;">
            <el-input
              v-model="dicValue0"
              :disabled="inputdisable"
              style="margin-bottom:15px;"
              placeholder="包型名称"
              clearable
              @clear="getorderTypeoption(dicValue0='')"
            >
              <!-- @keyup.enter.native="getorderTypeoption()"
              @clear="getorderTypeoption(dicValue0='')"-->

              <!-- @click="getorderTypeoption()" -->
              <i slot="suffix" style=" cursor: pointer;" class="el-input__icon el-icon-search" />
            </el-input>
          </div>
          <!-- <div> -->
            <!-- <el-button v-if="nowclick.actualId" @click="left_add()">新增</el-button> -->
            <!-- <el-popover
              :open-delay="1"
              :close-delay="1"
              class="item"
              trigger="hover"
              placement="top-start"
              content="编辑包型中含有的配色项目"
            >
              <el-button
                v-if="btnAip.edit.buttonCaption"
                slot="reference"
                style="margin:10px;margin-left:0px;margin-top:0px;"
                type="primary"
                :disabled="!nowclick.actualId"
                @click="left_add(dicValue0)"
              >{{ btnAip.edit.buttonCaption }}</el-button>
            </el-popover>

            <el-popover
              :open-delay="1"
              :close-delay="1"
              class="item"
              trigger="hover"
              placement="top-start"
              content="删除包型中含有的配色项目"
            >
              <el-button
                v-if="btnAip.delete&&btnAip.delete.buttonCaption"
                slot="reference"
                style="margin:10px;margin-left:0px;margin-top:0px;"
                type="danger"
                :disabled="!nowclick.actualId"
                @click="left_del(dicValue0)"
              >{{ btnAip.delete.buttonCaption }}</el-button>
            </el-popover>
          </div> -->
          <div id="bordes" style="overflow-y:auto;" :style="treeDivStyle">
            <el-tree
              v-show="showtree"
              ref="trees"
              :data="datas"
              default-expand-all
              :filter-node-method="filterNode"
              :expand-on-click-node="false"
              :highlight-current="true"
              @node-click="nodeClick"
            >
              <span slot-scope="{ node, data }" class="custom-tree-node pr">
                <span
                  :id="'tree-'+data.id"
                  :ref="'tree-'+data.id"
                  :title="data.label"
                >{{ data.label }}</span>
              </span>
            </el-tree>
          </div>
        </el-col>
        <!-- 新增第一value -->
        <el-col :span="20">
          <el-header id="elheader" class="header headerBd" height="auto">
            <el-form
              label-position="left"
              label-width="140px"
              inline
              class="FormInput"
              :height="mainHeight"
              @submit.native.prevent
            >
              <el-form-item label="方案编号：">
                <el-input
                  v-model="dtData.solutionCode"
                  style="width:16vw;"
                  :disabled="!nowclick.id"
                  placeholder="方案编号"
                  clearable
                  @keyup.enter.native="right_list(pageIndex=1)"
                  @clear="right_list(pageIndex=1)"
                >
                  <i
                    slot="suffix"
                    style=" cursor: pointer;"
                    class="el-input__icon el-icon-search"
                    @click="right_list(pageIndex=1)"
                  />
                </el-input>
              </el-form-item>

              <el-form-item label="方案编号：" style="display:none;">
                <el-input
                  v-model="dtData.solutionCode"
                  style="width:16vw;"
                  :disabled="!nowclick.id"
                  placeholder="方案编号"
                  clearable
                  @keyup.enter.native="right_list(pageIndex=1)"
                  @clear="right_list(pageIndex=1)"
                  @submit.native.prevent
                >
                  <i
                    slot="suffix"
                    style=" cursor: pointer;"
                    class="el-input__icon el-icon-search"
                    @click="right_list(pageIndex=1)"
                  />
                </el-input>
              </el-form-item>
            </el-form>

            <!-- v-if="btnAip.add.buttonCaption" -->
            <div v-if="btnAip">
              <el-button
                v-if="btnAip.query&&btnAip.query.buttonCaption"
                type="primary"
                :disabled="!nowclick.id||!colorAddFlag"
                @click="right_list(pageIndex=1)"
              >{{ btnAip.query.buttonCaption }}</el-button>
              <el-button
                v-if="btnAip.add&&btnAip.add.buttonCaption"
                type="primary"
                :disabled="!nowclick.id||!colorAddFlag"
                style="margin-bottom:10px"
                @click="openright_add()"
              >{{ btnAip.add.buttonCaption }}</el-button>
            </div>
          </el-header>
          <!-- :height="mainHeight" -->
          <div id="e" style="overflow-y:auto;width: 100%;" :style="{'height':mainHeight+'px'}">
            <!-- :max-height="mainHeight" -->
            <el-table
              id="tablePs"
              ref="tablePs"
              :data="tableData"
              border
              style="width: 100%;"
              :height="mainHeight-3"
            >
              <el-table-column prop="solutionCode" label="方案编号" width="120" />
              <el-table-column prop="colorName" label="颜色" width="120" />
              <!-- min-width="120" -->

              <!-- <div v-for="(item,index) in tableChildren" :key="index">
                <el-table-column
                 fixed="left"
                  v-if="tableData.length==0"
                  min-width="120"
                  :label="item.label"
                  prop=""
                />
                <el-table-column
                 fixed="left"
                  v-if="tableData.length!=0"
                  min-width="120"
                  :label="item.label"
                  :prop="'itemcaption'+item.id"
                />
              </div> -->
              <div v-for="(item,index) in tableChildren" :key="index">
                <el-table-column
                  filterable
                  v-if="!tableChildren.length"
                  :label="item.label"
                  :prop="'itemcaption'+item.id"
                />
                <el-table-column
                  v-if="tableChildren.length"
                  min-width="120"
                  :label="item.label"
                  :prop="'itemcaption'+item.id"
                />
              </div>
              
              <!-- <el-table-column :label="newitem.label[index]" v-for="(item,index) in newitem.id" :key="index">
                <template>
                  <div style="width:120px;" v-if="index!=newitem.id.length-1">{{newitem.label[index]}}</div>
                  <div style="min-width:120px;" v-if="index==newitem.id.length-1">{{newitem.label[index]}}</div>
                </template>
              </el-table-column>-->
              <el-table-column v-if="!tableChildren" label />
              <!-- <el-table-column v-if="false" fixed="left" label /> -->

              <!-- align="right" -->
              <el-table-column prop="imagePath" label="图片" width="120" fixed="right">
                <template slot-scope="scope">
                  <el-tooltip v-if="scope.row.imagePath" placement="top" effect="light">
                    <div slot="content">
                      <img class="avatar2" :src="imgUrlName+scope.row.imagePath" alt />
                    </div>
                    <img :src="imgUrlName+scope.row.imagePath" class="avatar" />
                  </el-tooltip>

                  <div v-if="!scope.row.imagePath" class="avatar" />
                </template>
              </el-table-column>

              <el-table-column label="操作" width="120" fixed="right">
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
                      size="mini"
                      type="primary"
                      icon="el-icon-edit"
                      circle
                      @click="handleEdit(scope.$index, scope.row)"
                    />
                  </el-tooltip>

                  <el-tooltip
                    v-if="btnAip.delete&&btnAip.delete.buttonCaption&&showtips"
                    class="item"
                    effect="light"
                    :content="btnAip.delete.buttonCaption"
                    placement="top-start"
                  >
                    <el-button
                      type="danger"
                      icon="el-icon-delete"
                      circle
                      @click="addPutInStorage(scope.row,showtips=false)"
                    />
                  </el-tooltip>
                  <el-button
                    v-if="btnAip.delete&&btnAip.delete.buttonCaption&&!showtips"
                    type="danger"
                    icon="el-icon-delete"
                    circle
                    @click="addPutInStorage(scope.row,showtips=false)"
                  />
                </template>
              </el-table-column>
            </el-table>
          </div>
          <!-- showleft_add -->
          <el-dialog
            :visible.sync="showleft_add"
            title="新增"
            close="showleft_add=false"
            width="550px"
            center
            :close-on-click-modal="false"
          >
            <el-form label-width="140px" style="width:100%">
              <el-form-item label="包型名称：" :required="true">{{ form0.ParentName }}</el-form-item>
              <el-form-item label="配色项目：" :required="true">
                <!-- @change="change" -->
                <el-select
                  v-model="form0.children"
                  style="width:90%;"
                  collapse-tags
                  multiple
                  placeholder="请选择"
                >
                  <el-option
                    v-for="item in projectobject"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
              <el-button @click="showleft_add = false">关闭</el-button>
              <el-button type="primary" @click="left_add_confirm()">确定</el-button>
            </span>
          </el-dialog>
          <!-- showright_add -->
          <el-dialog
            v-if="showright_add"
            :visible.sync="showright_add"
            title="新增"
            close="showright_add=false"
            width="550px"
            center
            :close-on-click-modal="false"
          >
            <el-form
              ref="form1"
              label-width="140px"
              style="width:100%"
              :model="form1"
              :rules="form1Rules"
            >
              <el-form-item label="方案编号：" prop="solutionCode">
                <el-input v-model="form1.solutionCode" style="width:90%;" />
              </el-form-item>
              <el-form-item label="颜色：" prop="colorId">
                <el-select
                    v-model="form1.colorId"
                    style="width:90%;"
                    placeholder="请选择"
                    filterable
                  >
                    <el-option
                      v-for="item in colorlist"
                      :key="item.id"
                      :label="item.dicValue"
                      :value="item.id"
                    />
                  </el-select>
              </el-form-item>
              <div v-for="(item,index) in form1.childList" :key="index">
                <el-form-item :label="item.label+'：'" :required="true">
                  <el-select
                    filterable
                    v-model="form1.childList[index].children"
                    style="width:90%;"
                    placeholder="请选择"
                    :class="item.error? 'item__error':''"
                  >
                    <el-option
                      v-for="item in colorlist"
                      :key="item.id"
                      :label="item.dicValue"
                      :value="item.id"
                    />
                  </el-select>
                  <div v-if="item.error" class="el-form-item__error" v-text="'请选择'+item.label" />
                </el-form-item>
              </div>

              <el-form-item label="图片：" class="imgBox">
                <el-upload
                  class="avatar-uploader"
                  :action="$ajaxUrl+'/fileupload/api/files/upload'"
                  :show-file-list="false"
                  :on-success="handleAvatarSuccess1"
                  :before-upload="beforeAvatarUpload"
                >
                  <img
                    v-if="form1.imagePath"
                    :src="imgUrlName+form1.imagePath"
                    class="avatar"
                    @mouseover="imgOver(1)"
                  />
                  <i v-else class="el-icon-plus avatar-uploader-icon pr">
                    <font class="imgText text1">正</font>
                    <font class="imgText text2">面</font>
                  </i>
                  <span v-if="form1.imagePath" class="removeBox">
                    <el-button
                      type="danger"
                      icon="el-icon-minus"
                      circle
                      @click.stop="handleRemove(1)"
                    />
                  </span>
                  <!-- <span class="removeBox" @click.stop="handleRemove(1)" v-if="imgFlag==1">
                    <i class="el-icon-delete" @mouseout="imgOut()"></i>
                  </span>-->
                </el-upload>
              </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
              <el-button @click="showright_add = false">关闭</el-button>
              <el-button type="primary" @click="right_add_confirm()">确定</el-button>
            </span>
          </el-dialog>
          <!-- dialogTableVisible -->

          <el-dialog
            v-if="dialogTableVisible"
            :visible.sync="dialogTableVisible"
            title="编辑"
            close="dialogTableVisible=false"
            width="550px"
            center
            :close-on-click-modal="false"
          >
            <el-form
              ref="form2"
              label-width="140px"
              style="width:100%"
              :model="form2"
              :rules="form1Rules"
            >
              <el-form-item label="方案编号：" prop="solutionCode">
                <el-input v-model="form2.solutionCode" style="width:90%;" />
              </el-form-item>

              <el-form-item label="颜色：">
                <el-select
                    v-model="form2.colorId"
                    style="width:90%;"
                    placeholder="请选择"
                    filterable
                  >
                    <el-option
                      v-for="item in colorlist"
                      :key="item.id"
                      :label="item.dicValue"
                      :value="item.id"
                    />
                  </el-select>
              </el-form-item>

              <!-- hihi -->
              <div v-for="(item,index) in form2.childList" :key="index">
                <el-form-item :label="item.label+'：'" :required="true">
                  <el-select
                    v-model="form2.childList[index].children"
                    style="width:90%;"
                    placeholder="请选择"
                    filterable
                    :class="item.error? 'item__error':''"
                  >
                    <el-option
                      v-for="item in colorlist"
                      :key="item.id"
                      :label="item.dicValue"
                      :value="item.id"
                    />
                  </el-select>
                  <div v-if="item.error" class="el-form-item__error" v-text="'请选择'+item.label" />
                </el-form-item>
              </div>

              <el-form-item label="图片：" class="imgBox">
                <el-upload
                  class="avatar-uploader"
                  :action="$ajaxUrl+'/fileupload/api/files/upload'"
                  :show-file-list="false"
                  :on-success="handleAvatarSuccess2"
                  :before-upload="beforeAvatarUpload"
                >
                  <img
                    v-if="form2.imagePath"
                    :src="imgUrlName+form2.imagePath"
                    class="avatar"
                    @mouseover="imgOver(1)"
                  />

                  <i v-else class="el-icon-plus avatar-uploader-icon pr">
                    <font class="imgText text1">正</font>
                    <font class="imgText text2">面</font>
                  </i>
                  <!-- <span class="removeBox" @click.stop="handleRemove(2)" v-if="imgFlag==1">
                    <i class="el-icon-delete" @mouseout="imgOut()"></i>
                  </span>-->

                  <span v-if="form2.imagePath" class="removeBox">
                    <el-button
                      type="danger"
                      icon="el-icon-minus"
                      circle
                      @click.stop="handleRemove(2)"
                    />
                  </span>
                </el-upload>
              </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
              <el-button @click="dialogTableVisible = false">关闭</el-button>
              <el-button type="primary" @click="right_edit_confirm()">确 定</el-button>
            </span>
          </el-dialog>

          <!-- <el-dialog
            :visible.sync="uploadshow"
            title="选择图片"
            close="uploadshow=false"
            width="550px"
            center
          >
          <UploadPic
            ref="Upload"
            @upLoadClose="upLoadClose"
          />
          </el-dialog>-->

          <el-footer id="elfooter">
            <!-- layout="prev, pager, next" -->
            <!-- <el-pagination
              style="text-align:center"

              :page-size="pageSize"
              :total="totalCount"
              :current-page="pageIndex"

              @current-change="currentChange"
            />-->
            <!-- :page-sizes="[25,11,2]" -->
            <Pagination
              :page-index="pageIndex"
              :total-count="totalCount"
              :page-size="pageSize"
              @pagination="pagination"
            />
          </el-footer>
        </el-col>
      </el-row>
    </el-main>
  </el-container>
</template>
<script>
const id = 1000;
import AppTree from "@/utils/appTree";
import UploadPic from "./components/upload";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { getBtnCtr, getName } from "@/utils/BtnControl";
import height from "@/utils/height";
import { async } from "q";
import Pagination from "@/components/Pagination";
import { trim } from "@/utils/common.js";
import { closest } from "@/utils/Dom";

export default {
  // name: "DataDictionary",
  name: "ViewsProductColorschemeindexvue",
  components: {
    UploadPic,
    Pagination
  },

  filters: {
    formatTDate: value => {
      if (value != null) {
        var d = value.split("T");
      }
      return d ? d[0] : value;
    }
  },
  data() {
    return {
      listId:"",
      tableChildren:null,
      colorAddFlag: true,
      showtree: true,
      fullscreenLoading: false,
      inputdisable: false,
      imgFlag1: false,
      dtData: {
        solutionCode: ""
      },
      form1Rules: {
        solutionCode: [
          { required: true, message: "请填写编号", trigger: "blur" }
        ],
        colorId: [
          { required: true, message: "请选择颜色", trigger: "blur" }
        ]
      },
      dialogImageUrl: "",
      dialogVisible: false, // 图片标记
      Visible: false, // 弹框标记
      changeTreeState: true,
      imgFlag: 0,

      uploadshow: true,
      test: "",
      btnAip: "",
      showtips: false,
      colorlist: [],
      showright_add: false,
      form0: {
        ParentName: null,
        ParentId: null,
        actualId: null,
        children: "",
        children2: "",
        id: null,
        label: null
      },
      form1: {
        colorId:"",
        imagePath: "",
        solutionCode: "",
        packageId: "",
        childList: []
      },
      form2: {
        colorId:"",
        solutionCode: "",
        packageId: "",
        imagePath: "",
        childList: []
      },
      form: {
        ParentId: null,
        actualId: null,
        children: "",
        id: null,
        label: null
      },
      label: "",
      showleft_add: false,
      nowclick: [],
      projectobject: [],
      dicValue0: "",
      newitem: [],
      newarr: [],
      lastarr: [],
      orderTypeoption: [],
      iconFlag: true,
      setImgFt: false,
      setFalse1: false,
      dialogTableVisible: false,
      setFalse: false,
      imgsrc: this.imgUrlName,
      tableData: [],
      treeDivStyle: "",
      selectedRow: null,
      pageSize: 25, // 分页显示记录条数
      totalCount: 10, // 总记录数
      pageIndex: 1, // 页码
      tableDatanew: null,
      headerHeight: "117px",
      mainHeight: 0,
      footerHeight: "50px",
      datas: [],
      clickNodes: "",
      coloritem: ""
    };
  },
  watch: {
    lastarr(val, oldVal) {
      // 普通的watch监听
      this.datas = this.GetTreeDataDisabled(val);
      // console.log(this.datas);
      //  this.initApptree();
      // this.getTree();
    },
    dicValue0(val) {
      this.$refs.trees.filter(trim(val));
    }
  },
  deactivated() {},
  mounted() {
    this.setStyle();
  },
  deactivated() {
    this.showleft_add = false;
    this.showright_add = false;
    this.dialogTableVisible = false;
  },
  async created() {
    var that = this;

    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    //  that.coloritem =await that.getallcolor()

    this.getorderTypeoption();
    // this.mainHeight = this.footerHeight - 256 - parseInt(this.headerHeight);
    // this.initApptree();
    // this.getTree();
    this.setPages(15);
  },
  methods: {
    getallcolor() {
      return new Promise(function(resolve, reject) {
        var that = this;
        var para = {
          isPaging: false,
          pageSize: 0,
          pageIndex: 0,
          queryConditions: [],
          orderByConditions: null
        };

        request({
          url: "/manufacturing/api/TMMColorItem",
          method: "get",
          params: { RequestObject: para }
        }).then(res => {
          resolve(res.data);
        });
      });
    },

    filterNode(value, data, node) {
      // if (!value) return true;
      // var dt = data.label;
      // return dt.indexOf(value) !== -1;

      if (!value) {
        return true;
      }
      const level = node.level;
      const _array = []; // 这里使用数组存储 只是为了存储值。
      this.getReturnNode(node, _array, value, data);
      let result = false;
      _array.forEach(item => {
        result = result || item;
      });
      return result;
    },

    getReturnNode(node, _array, value, data) {
      const isPass =
        node.data &&
        node.data.label &&
        node.data.label.indexOf(value) !== -1 &&
        node.level == 1; // node.level==1控制只能搜索第一级的数据
      // && node.parent.id==0;
      isPass ? _array.push(isPass) : "";
      this.index++;
      if (!isPass && node.level != 1 && node.parent) {
        //! isPass控制上面没通过的数据

        this.getReturnNode(node.parent, _array, value);
      }
    },
    beforeAvatarUpload(file) {
      const isJPG = file.type === "image/jpeg";
      const isPNG = file.type === "image/png";
      const isLt2M = file.size / 1024 / 1024 < 2;

      if (!isJPG && !isPNG) {
        this.$message.error("上传头像图片只能是 JPG,PNG 格式!");
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 2MB!");
      }
      return (isJPG || isPNG) && isLt2M;
    },
    handleAvatarSuccess1(res, file) {
      this.imgFlag1 = true;
      this.form1.imagePath = res;
    },
    handleAvatarSuccess2(res, file) {
      this.imgFlag1 = true;
      this.form2.imagePath = res;
    },
    handleRemove(state) {
      if (state == 1) {
        this.form1.imagePath = "";
        this.imgFlag1 = false;
      }
      if (state == 2) {
        this.form2.imagePath = "";
        this.imgFlag1 = false;
      }
    },

    imgOver(state) {
      // this.imgFlag1 = state;
    },
    imgOut() {
      this.imgFlag1 = -1;
    },

    upLoadClose(data) {
      this.iconfontImg = this.imgUrlName + data;
    },
    addPutInStorage(item) {
      // 删除

      this.$confirm("是否删除？", "确认信息", {
        type: "warning"
      })
        .then(() => {
          var para = {
            postData: {
              id: parseInt(item.id),
              _LogName: "string"
            },
            postDataList: null
          };
          request({
            url: "/manufacturing/api/TMMColorSolutionMainNew",
            method: "DELETE",
            data: para
          }).then(res => {
            if (res.code == 0) {
              this.$message.success("操作成功");
            } else {
              this.$message.error("操作失败");
            }

            this.right_list();
          });

          this.showtips = true;
        })
        .catch(() => {
          this.showtips = true;
        });
    },

    handleEdit(index, row) {
      //  this.dtData.solutionCode=""
      this.dialogTableVisible = true;
      var nowchildren = this.form0.children2;
      var arr = [];

      this.form2 = {
        id: row.id,
        solutionCode: row.solutionCode,
        imagePath: row.imagePath,
        packageId: row.packageId,
        colorId: parseInt(row.colorId),
        childList: []
      };
      // hihi
      this.tableChildren.map(item => {
        var str = "itemid" + item.id;
        var obj = new Object({
          ParentId: item.ParentId,
          ParentName: item.ParentName,
          mainId: row.id,
          children: parseInt(row[str]) ? parseInt(row[str]) : "",
          chooseid: item.id,
          id: item.actualId,
          label: item.label,
          error: false
        });
        this.form2.childList.unshift(obj);
      });

      // this.form2.childList
    },

    right_edit_confirm() {
      this.$refs.form2.validate(valid => {
        var error = false;
        for (var i = 0; i < this.form2.childList.length; i++) {
          if (this.form2.childList[i].children == "") {
            this.form2.childList[i].error = true;
            error = true;
          } else {
            this.form2.childList[i].error = false;
          }
        }
        if (error == true) {
          return;
        }
        if (!valid) {
        } else {
          if (this.form2.childList.length == 0) {
            this.$message.error("配色方案至少包含一条数据");
            return;
          }
          var flag = true;
          var arr = [];
          var falsearr = [];
          this.form2.childList.map(item => {
            var obj = new Object({
              id: item.id,
              mainId: this.form2.id,
              itemId: item.chooseid,
              colorId: item.children,
              _LogName: ""
            });
            arr.push(obj);
            if (!obj.colorId) {
              flag = false;
              falsearr.push(item.label);
            }
          });
          var str = falsearr.join("，");
          var para = {
            postData: {
              id: this.form2.id,
              solutionCode: this.form2.solutionCode,
              imagePath: this.form2.imagePath,
              packageId: this.form2.packageId,
              childList: arr,
              colorId: this.form2.colorId,
              _LogName: ""
            }
          };
          // console.log(JSON.stringify(para))
          // return;

          if (flag) {
            request({
              url: "/manufacturing/api/TMMColorSolutionMainNew",
              method: "put",
              data: para
            }).then(res => {
              if (res.code == 0) {
                this.$message.success("操作成功");
                this.dialogTableVisible = false;
                this.right_list();
              } else {
                this.$message.error(res.info);
              }
            });
          } else {
            this.$message.error("以下配色项目：" + str + "，还没选择颜色！");
          }
        }
      });
    },

    openright_add() {
      this.dtData.solutionCode = "";
      this.form1.imagePath = "";
      this.showright_add = true;
      var nowchildren = this.form0.children2;
      this.form1 = {
        solutionCode: "",
        imagePath: this.form1.imagePath,
        packageId: this.nowclick.parentId,
        childList: []
      };
      // console.log(this.tableChildren,'给值')
      this.tableChildren.map(item => {
        var obj = new Object({
          ParentId: item.ParentId,
          ParentName: item.ParentName,
          actualId: item.actualId,
          // children: item.children,
          chooseid: item.id,
          id: "",
          label: item.label,
          error: false
        });
        this.form1.childList.unshift(obj);
      });
    },

    right_add_confirm() {
      this.$refs.form1.validate(valid => {
        var error = false;
        for (var i = 0; i < this.form1.childList.length; i++) {
          if (this.form1.childList[i].children == undefined) {
            this.form1.childList[i].error = true;
            error = true;
          } else {
            this.form1.childList[i].error = false;
          }
        }
        if (error == true) {
          return;
        }
        if (!valid) {
        } else {
          var arr = [];
          var flag = true;
          var falsearr = [];
          this.form1.childList.map(item => {
            var obj = new Object({
              id: 0,
              mainId: 0,
              itemId: item.chooseid,
              colorId: item.children,
              _LogName: "新增配色方案"+this.form1.solutionCode
            });
            if (!obj.colorId) {
              flag = false;
              falsearr.push(item.label);
            }
            arr.push(obj);
          });
          var str = falsearr.join("，");
          var para = {
            postData: {
              id: 0,
              solutionCode: this.form1.solutionCode,
              imagePath: this.form1.imagePath,
              packageId: this.listId,
              childList: arr,
              colorId: this.form1.colorId,
              _LogName: ""
            }
          };

          if (flag) {
            request({
              url: "/manufacturing/api/TMMColorSolutionMainNew",
              method: "post",
              data: para
            }).then(res => {
              if (res.code == 0) {
                this.$message.success("操作成功");
                this.showright_add = false;
                this.right_list();
              } else {
                this.$message.error(res.info);
              }
            });
          } else {
            this.$message.error("以下配色项目：" + str + "，还没选择颜色！");
          }
        }
      });
    },
    left_del_all(nowid) {
      // /api/TMMColorItem/DeleteAll

      var para = {
        postData: {
          id: nowid,
          _LogName: "string"
        }
      };
      request({
        url: "/manufacturing/api/TMMColorItem/DeleteAll",
        method: "delete",
        data: para
      }).then(res => {
        if (res.code == 0) {
          this.$message.success("操作成功");
        } else {
          this.$message.error("操作失败");
        }

        this.getorderTypeoption();
      });
    },
    left_del(e) {
      if (this.nowclick.ParentId != this.nowclick.actualId) {
        this.$confirm("是否删除？", "确认信息", {
          type: "warning"
        })
          .then(() => {
            var para = {
              postData: {
                id: this.form0.actualId,
                _LogName: "string"
              }
            };
            request({
              url: "/manufacturing/api/TMMColorItem",
              method: "delete",
              data: para
            }).then(res => {
              if (res.code == 0) {
                this.$message.success("操作成功");
              } else {
                this.$message.error("操作失败");
              }

              this.getorderTypeoption(e);
              this.right_list();
            });
          })
          .catch(() => {});
      } else if (this.nowclick.ParentId == this.nowclick.actualId) {
        this.$confirm(
          "是否删除" + this.label + "下的所有配色项目？",
          "确认信息",
          {
            type: "warning"
          }
        )
          .then(res => {
            // if (res.code == 0) { } else {
            //   this.$message.error("数据请求失败");
            // }
            this.left_del_all(this.nowclick.ParentId);
            this.getorderTypeoption(e);
            this.right_list();
          })
          .catch(() => {});
      }
    },
    left_add_confirm() {
      this.showleft_add = false;

      // this.left_del_all(this.form0.ParentId);

      var that = this;

      var arr = [];
      that.form0.children.map(item => {
        var obj = new Object({
          id: 0,
          packageId: that.form0.ParentId,
          itemId: item,
          _LogName: "string"
        });
        arr.push(obj);
      });
      arr.map(item => {
        that.form0.children2.map(val => {
          if (item.itemId == val.chooseid) {
            item.id = val.actualId;
          }
        });
      });
      var para2 = {
        postData: arr
      };
      request({
        url: "/manufacturing/api/TMMColorItem",
        method: "post",
        data: para2
      }).then(res => {
        if (res.code == 0) {
          this.$message.success("操作成功");
          that.getorderTypeoption(this.dicValue0);
          that.right_list();
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });

      return;

      // var para = {
      //   postData: {
      //     id: this.form0.ParentId,
      //     _LogName: "string"
      //   }
      // };
      // request({
      //   url: "/manufacturing/api/TMMColorItem/DeleteAll",
      //   method: "delete",
      //   data: para
      // }).then(res => {
      //   // this.getorderTypeoption();

      //   if (res.code == 0) {
      //     var arr = [];
      //     that.form0.children.map(item => {
      //       var obj = new Object({
      //         id: 0,
      //         packageId: that.form0.ParentId,
      //         itemId: item,
      //         _LogName: "string"
      //       });
      //       arr.push(obj);
      //     });
      //     var para2 = {
      //       postData: arr
      //     };

      //     request({
      //       url: "/manufacturing/api/TMMColorItem",
      //       method: "post",
      //       data: para2
      //     }).then(res => {
      //       if (res.code == 0) {
      //         this.$message.success("操作成功");
      //       } else {
      //         // this.$message.error("操作失败6");
      //       }
      //       that.getorderTypeoption(this.dicValue0);
      //       that.right_list();
      //     });
      //   } else {
      //     // this.$message.error("数据请求失败");

      //     var arr = [];
      //     that.form0.children.map(item => {
      //       var obj = new Object({
      //         id: 0,
      //         packageId: that.form0.ParentId,
      //         itemId: item,
      //         _LogName: "string"
      //       });
      //       arr.push(obj);
      //     });
      //     var para2 = {
      //       postData: arr
      //     };

      //     request({
      //       url: "/manufacturing/api/TMMColorItem",
      //       method: "post",
      //       data: para2
      //     }).then(res => {
      //       if (res.code == 0) {
      //         this.$message.success("操作成功");
      //       } else {
      //         // this.$message.error("操作失败7");
      //       }
      //       that.getorderTypeoption(this.dicValue0);
      //       that.right_list();
      //     });
      //   }
      // });
    },

    left_add() {
      this.showleft_add = true;
    },

    async nodeClick(data) {
      if (data.state != 1) {
        return;
      }
      if (data.children == null) {
        this.tableData = [];
        this.colorAddFlag = false;
      } else {
        this.colorAddFlag = true;
      }
      
      this.label = data.label;
      this.pageIndex = 1;
      this.dtData.solutionCode = "";
      this.setTreeStyle(data);
      this.form0 = {
        ParentName: null,
        ParentId: null,
        actualId: null,
        children: [],
        children2: [],
        id: null,
        label: null
      };
      this.clickNodes = data.id;
      this.nowclick = data; // 新增能用
      this.form0.actualId = data.actualId;
      this.form0.ParentId = data.ParentId;
      this.form0.label = data.label;
      this.form0.ParentName = data.name;
      // this.form1.childList = data.children;
      this.tableChildren = data.children;
      this.listId = data.id;
      this.right_list();
    },
    setTreeStyle(data) {
      // 设置选中的样式
      var a = document.querySelectorAll("div.el-tree-node__content");
      a.forEach(item => {
        item.style.background = "#FFF";
      }); // 重置所有按钮样式
      // console.log("tree-" + data.id)
      // console.log(closest(this.$refs["tree-" + this.nowclick.id]))
      closest(
        this.$refs["tree-" + data.id],
        ".el-tree-node__content"
      ).style.background = "#F5F7FA"; // 点击按钮样式
    },

    async asyncRandom(itemid, ParentName) {
      var that = this;
      // this.coloritem = await this.getallcolor();

      return new Promise(function(resolve, reject) {
        var para = {
          isPaging: false,
          pageSize: 0,
          pageIndex: 0,
          queryConditions: [
            { column: "packageid", content: itemid, condition: 0 }
          ],
          orderByConditions: null
        };
        var newarr2 = [];
        if (that.coloritem) {
          that.coloritem.map(item2 => {
            // console.log(item2)
            var obj2 = new Object({
              actualId: item2.id,
              deptName: item2.itemName,
              ParentId: itemid,
              ParentName: ParentName,
              chooseid: item2.itemId,
              children: []
            });

            if (item2.packageId == itemid) {
              // console.log(obj2)
              newarr2.push(obj2);
            }
          });
          resolve(newarr2);
        } else {
        }

        // request({
        //   url: "/manufacturing/api/TMMColorItem",
        //   method: "get",
        //   params: { RequestObject: para }
        // }).then(res => {
        //   if (res.code == 0) {
        //     var newarr2 = [];
        //     res.data.map(item2 => {
        //       var obj2 = new Object({
        //         actualId: item2.id,
        //         deptName: item2.itemName,
        //         ParentId: itemid,
        //         ParentName: ParentName,
        //         chooseid: item2.itemId,
        //         children: []
        //       });
        //       newarr2.push(obj2);
        //     });
        //     resolve(newarr2);
        //   } else {
        //   }
        // });
      });
    },
    getTMMColorItem(id) {
      return new Promise((resolve, reject) => {
        var para = {
          isPaging: false,
          pageSize: 0,
          pageIndex: 0,
          queryConditions: [
            { column: "packageid", content: id, condition: 0 }
          ],
          orderByConditions: null
        };

        request({
          url: "/manufacturing/api/TMMColorItem",
          method: "get",
          params: { RequestObject: para }
        }).then(res => {
          if (res.code == 0) {
            if (res.data.length > 0) {
              var id = [];
              var label = [];
              res.data.map(item => {
                id.push(item.itemId);
                label.push(item.itemName);
              });

              this.newitem = {
                id: id,
                label: label
              };
              resolve(res);
            } else {
              this.newitem = [];

              var aalist = [
                { column: "packageid", content: this.clickNodes, condition: 0 }
              ];

              if (this.dtData.solutionCode) {
                aalist.push({
                  column: "solutionCode",
                  content: trim(this.dtData.solutionCode),
                  condition: 6
                });
              }

              var para = {
                isPaging: true,
                pageSize: this.pageSize,
                pageIndex: this.pageIndex,
                queryConditions: null,
                orderByConditions: null
              };
              request({
                url: "/manufacturing/api/TMMColorSolutionMainNew",
                method: "get",
                params: { RequestObject: para }
              }).then(res => {
                if (res.code == 0) {
                  resolve(res);
                  this.tableData = res.data;
                  this.totalCount = res.totalNumber;
                }
              });
            }
          }
        });
      });
    },
    async right_list() {
      this.fullscreenLoading = true;
      var list = [
        { column: "packageid", content: this.listId, condition: 0 }
      ];

      if (trim(this.dtData.solutionCode)) {
        list.push({
          column: "solutionCode",
          content: trim(this.dtData.solutionCode),
          condition: 6
        });
      }

      var para = {
        isPaging: true,
        pageSize: this.pageSize,
        pageIndex: this.pageIndex,
        queryConditions: list,
        orderByConditions: null
      };
      request({
        url: "/manufacturing/api/TMMColorSolutionMainNew",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        setTimeout(() => {
          this.fullscreenLoading = false;
        }, 500);
        if (res.code == 0) {
          this.tableData = res.data;
          this.totalCount = res.totalNumber;
        }
      });
      this.$nextTick(() => {
       this.$refs.tablePs.doLayout();
      });
    },

    async getorderTypeoption(e) {
      this.fullscreenLoading = true;
      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "颜色档案", condition: 0 }
        ],
        orderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == 0) {
          this.colorlist = res.data;
        }
      });
      // this.coloritem = await this.getallcolor();

      if (e) {
        this.dicValue0 = e;
        // this.dicValue0="####"
        // this.showtree=false
        // setTimeout(() => {
        //   this.dicValue0=e
        // }, 2000);
      }
      this.inputdisable = true;
      this.fullscreenLoading = true;
      setTimeout(() => {
        this.inputdisable = false;
        this.fullscreenLoading = false;
      }, 1500);

      request({
        url: "/manufacturing/api/TMMColorSolutionMainNew/GetTMMPackageColorItem",
        method: "get"
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          // console.log(res.data)
          var that = this;
          that.orderTypeoption = res.data;
          that.newarr = [];

          function asyncRandom(itemid, ParentName) {
            return new Promise(function(resolve, reject) {
              var para = {
                isPaging: false,
                pageSize: 0,
                pageIndex: 0,
                queryConditions: [
                  { column: "packageid", content: itemid, condition: 0 }
                ],
                orderByConditions: null
              };

              var newarr2 = [];
              if (that.coloritem) {
                // this.getallcolor()
                that.coloritem.map(item2 => {
                  var obj2 = new Object({
                    actualId: item2.id,
                    deptName: item2.itemName,
                    ParentId: itemid,
                    ParentName: ParentName,
                    chooseid: item2.itemId,
                    children: []
                  });

                  if (item2.packageId == itemid) {
                    newarr2.push(obj2);
                  }
                });
                resolve(newarr2);
              }
              
            });
          }

          async function getlist() {
            // console.log(that.orderTypeoption)
            for (let i = 0; i < that.orderTypeoption.length; i++) {
              var item = that.orderTypeoption[i];
              // console.log(item)
              // var newarr2 = await asyncRandom(item.id, item.dicValue);
              if (item.id) {
                var obj = new Object({
                  id: item.id,
                  name: item.name,
                  parentId: item.parentId,
                  unixId: item.unixId,
                  children:item.children,
                  state: 1
                });
                that.newarr.push(obj);
              }
            }
            that.lastarr = that.newarr;

            // this.datas = this.GetTreeDataDisabled(that.lastarr);
            if (e) {
              setTimeout(() => {
                that.$refs.trees.filter(trim(e));
              }, 100);
            }
          }
          getlist();
        }
      });
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 500);
      //  this.dicValue0()
    },

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
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.right_list();
    },
    // currentChange(val) {
    //   this.pageIndex = val;
    //   //分页改变页数
    //   this.right_list();
    // },
    getTree() {
      // 获取列表数据
      var robject = RequestObject.CreateGetObject();
      request({
        url: "/system/api/TSMUser",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
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
        // setTimeout(() => {
        //  }, 100);

        var resObject = [];
        if (!data || data.length < 1) {
          return null;
        }
        data.forEach((v, i) => {
          if (v.children && v.children.length > 0) {
            resObject.push({
              id: v.id,
              name: v.name,
              label: v.name,
              children: treeObj(v.children),
              parentId: v.parentId,
              state: v.state,
              unixId: v.unixId
            });
          } else {
            resObject.push({
              id: v.id,
              name: v.name,
              label: v.name,
              children: treeObj(v.children),
              parentId: v.parentId,
              state: v.state,
              unixId: v.unixId
            });
          }
        });
        return resObject;
      };
      return treeObj(data, level);
    },

    initApptree() {
      var rqs = RequestObject.CreateGetObject();
      request({
        url: "system/api/TSMDept/GetTree",
        method: "GET",
        params: { requestObject: JSON.stringify(rqs) }
      }).then(res => {
        // this.datas = this.GetTreeDataDisabled(res.data);
      });
    },

    clickNode(val) {
      this.$refs.multipleTable.toggleRowSelection(val);
    },
    selectNode(val) {
      var dataRight = this.rightData;
    },
    handleSizeChange(val) {},
    handleCurrentChange(val) {},
    setStyle() {
      var navbar = document.getElementById("navbar_yfkj");
      var nv = navbar.clientHeight || navbar.offsetHeight;
      var elfooter = document.getElementById("elfooter");
      var f = elfooter.clientHeight || elfooter.offsetHeight;
      this.mainHeight = height - parseInt(this.headerHeight) - nv - f - 5;
       var mainHeight = this.mainHeight + 38
      this.treeDivStyle = `height:${mainHeight}px`;
      // this.treeStyle =
      //   "width:256px;height:" +
      //   (this.mainHeight - 4).toString() +
      //   "px;padding:0px;";
      // this.treeDivStyle =
      //   "width:100%;height:" +
      //   this.mainHeight.toString() +
      //   "px;padding:0px;background:drgb(230,235,245);";
    },
    setPages(dataNumber) {
      // var dataNumber=data.length;

      var total = Math.ceil(dataNumber / 20);
    },

    /**
     * 删除左侧表格树结构数据
     */

    /**
     * 初始化应用
     */
    initialize() {
      var getse = RequestObject.CreateGetObject();
      request({
        url: "/platform/api/TMenus/GetTree",
        method: "get",
        params: {
          requestObject: JSON.stringify(getse),
          withKey: 2
        }
      }).then(res => {
        if (res.code == 0) {
          this.data = AppTree.ToTableTreeDataDisabled(res.data);
        }
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

    handelData(data) {
      data.map(item => {
        if (
          item.headPicPath != null &&
          item.headPicPath.indexOf("el-icon") != -1
        ) {
          item.iconFlag = true;
        } else {
          item.iconFlag = false;
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
//  .el-form-item__label::after {
//     content: "*";
//     color: #ff4949;
//     margin-right: 4px;
// }
// .el-form-item__label::before {
//     content: "1";
// }
/deep/ .el-dialog__title {
  color: #1890ff;
}
/deep/ .el-dialog__body {
  // padding-top: 10px;
  padding: 10px 20px;
  .item__error .el-input__inner {
    border: 1px solid #ff4949;
  }
  .item__error > span {
    color: #ff4949;
    font-size: 12px;
  }
}
/deep/ .el-dialog__footer {
  text-align: right;
}
/deep/ .el-dialog__header {
  text-align: left;
}
.header {
  padding-top: 0px;
}
.link {
  color: #1890ff;
  cursor: pointer;
}
.el-header .el-form {
  border-bottom: 1px solid #eee;
  margin-bottom: 20px;
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
  font-size: 12px;
  padding-right: 8px;
  color: #333333;
}
/deep/.el-tree-node__content .custom-tree-node {
  display: block;
  width: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
/deep/ .el-tree-node__children .custom-tree-node {
  display: block;
  width: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

#e {
  width: 100%;
  border: 1px solid#dfe6ec;
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
  //   white-space:nowrap;
  // overflow: hidden;
  // text-overflow:ellipsis;
}

/deep/ .el-tree-node is-expanded is-focusable {
  width: 400px;
}

/deep/ .el-tree-node__children {
  overflow: visible;
}

/deep/ .el-tree-node__label {
  //  display:block;text-overflow: ellipsis;
  //   overflow:hidden

  //  width: 100px;
  // width: 100px;
  // height: 100px;
  //   display:block;
  // word-break: break-all;
  // word-wrap: break-word;
}
// word-wrap: normal | break-word ;
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
  color: #000000;
  width: 60px;
  height: 60px;
  line-height: 60px;
  text-align: center;
}
.avatar {
  width: 60px;
  height: 60px;
  display: block;
  // float:right;
}
.formItem {
  padding-top: 18px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.imgBox {
  margin-bottom: 0px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.pr {
  position: relative;
}
.imgText {
  position: absolute;
  font-size: 22px;
  left: 0px;
  width: 100%;
  text-align: center;
  opacity: 0.3;
  word-wrap: break-word; /*英文的时候需要加上这句，自动换行*/
}
.text1 {
  top: -12px;
}
.text2 {
  top: 12px;
}

.removeBox {
  display: block;
  width: 60px;
  height: 60px;
  line-height: 60px;
  // background: #e6e6e6;
  position: absolute;
  top: 0px;
  left: 0px;
  z-index: 999;
}
.removeBox button {
  color: #fff;
  font-size: 12px;
  width: 20px;
  height: 20px;
  margin: 0px;
  padding: 0px;
  position: absolute;
  right: 0px;
  top: 0px;
  font-weight: 800;
  vertical-align: middle;
}
.avatar2 {
  max-height: 500px;
  max-width: 500px;
}
// .removeBox {
//   display: block;
//   width: 60px;
//   height: 60px;
//   line-height: 60px;
//   background: #e6e6e6;
//   position: absolute;
//   top: 0px;
//   left: 0px;
//   z-index: 999;
// }
// .removeBox i {
//   display: block;
//   color: #409eff;
//   width: 60px;
//   height: 60px;
//   line-height: 60px;
//   font-size: 20px;
// }
.el-main {
  padding-bottom: 4px;
  overflow: hidden;
  padding-top: 0px;
}
/deep/
  .el-tree--highlight-current
  .el-tree-node.is-current
  > .el-tree-node__content {
  background-color: #fff;
}
.el-tree-node__content:hover {
  background-color: #f5f7fa;
}
.el-col-4 {
  width: 200px;
}
.el-col-20 {
  width: calc(100% - 220px);
  margin-left: 20px;
}
.el-table--group,
.el-table--border {
  border: 0px solid #dfe6ec;
}
.el-table::before {
  height: 0px !important;
  display: none;
}
.el-table--group::after,
.el-table--border::after {
  width: 0px;
}
/deep/ .el-table thead th:last-child {
  border-right: 0px solid #dfe6ec;
}
.el-table__fixed::before,
.el-table__fixed-right::before {
  height: 0px !important;
  width: 0px !important;
}
/deep/ .el-table__fixed::before,
.el-table__fixed-right::before {
  height: 0px !important;
  width: 0px !important;
}
/deep/ .el-table__fixed-right .el-table__fixed::before,
.el-table__fixed-right::before {
  height: 0px !important;
  width: 0px !important;
}
// background-color: #F5F7FA;
</style>

