<template>
  <el-container
    id="ProductBom"
    v-on:click.native="listenClick"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-main>
      <!-- 左边列表框开始 -->
      <div class="leftBox" :style="'height:'+tableHeight+'px'">
        <DataTree
          @nodeClick="nodeClick"
          :editState="editState"
          @setBrand="setBrand"
          @setEditState="setEditState"
          @doSave="doSave"
        ></DataTree>
      </div>
      <!-- 左边列表框结束 -->

      <!-- 右边列表框开始 -->
      <div class="rightBox" :style="'height:'+tableHeight+'px'" v-on:click.native="listenClick">
        <div class="btnHeader" id="btnHeader">
          <div v-if="btnAip.add" class="btnHeaderBox">
            <el-button
              type="default"
              v-if="btnAip.preserve&&btnAip.preserve.buttonCaption"
              @click="doSave"
            >{{ btnAip.preserve.buttonCaption }}</el-button>
            <el-button
              type="default"
              v-if="btnAip.preview&&btnAip.preview.buttonCaption"
              @click="doPreview(1)"
            >{{ btnAip.preview.buttonCaption }}</el-button>
            <el-button
              type="default"
              v-if="btnAip.savetemplate&&btnAip.savetemplate.buttonCaption"
              @click="saveTemplate"
            >{{ btnAip.savetemplate.buttonCaption }}</el-button>
            <el-button
              type="default"
              v-if="btnAip.seltemplate&&btnAip.seltemplate.buttonCaption"
              @click="selectTel"
            >{{ btnAip.seltemplate.buttonCaption }}</el-button>
            <el-button
              type="default"
              v-if="btnAip.selformula&&btnAip.selformula.buttonCaption"
              @click="selectFormula"
            >{{ btnAip.selformula.buttonCaption }}</el-button>
            <el-button
              type="default"
              v-if="btnAip.print&&btnAip.print.buttonCaption"
              @click="doPreview(2)"
            >{{ btnAip.print.buttonCaption }}</el-button>
            <el-button
              type="default"
              v-if="btnAip.export&&btnAip.export.buttonCaption"
              @click="doPreview(3)"
            >{{ btnAip.export.buttonCaption }}</el-button>
          </div>
        </div>
        <!-- 表头按钮结束 -->
        <!-- 头部表单开始 -->
        <el-header height="auto" id="elheader" class="headerFrom" v-on:click.native="listenClick">
          <el-form
            ref="dtData"
            :model="dtData"
            label-width="76px"
            class="FormInput"
            inline
            label-position="left"
            :rules="dtDataRules"
          >
            <el-form-item label="纸格编号：" class="formItem" prop="pagerCode">
              <el-input
                v-model="dtData.pagerCode"
                placeholder="纸格编号："
                clearable
                style="width:200px"
                @change="editChange"
                :disabled="!noPackageCol"
              />
            </el-form-item>
            <el-form-item class="formItem" label="出格师傅：">
              <el-select
                clearable
                filterable
                placeholder="请选择"
                style="width:200px;"
                v-model="dtData.maker"
                @change="editChange"
                :disabled="!noPackageCol"
              >
                <el-option
                  v-for="item in realNameOptions"
                  :key="item.id"
                  :label="item.realName"
                  :value="item.id"
                ></el-option>
              </el-select>
            </el-form-item>
            <el-form-item class="imgBox">
              <el-upload
                class="avatar-uploader"
                :action="$ajaxUrl+'/fileupload/api/files/upload'"
                :show-file-list="false"
                :on-success="handleAvatarSuccess1"
                :before-upload="beforeAvatarUpload"
                @click.native="doUpload($event)"
              >
                <img
                  v-if="imageUrl1"
                  :src="imgUrlName+imageUrl1"
                  class="avatar"
                  @mouseover="imgOver(1)"
                  @click.stop="viewImg(1)"
                />
                <i v-else class="el-icon-plus avatar-uploader-icon pr">
                  <font class="imgText text1">正</font>
                  <font class="imgText text2">面</font>
                </i>
                <span class="removeBox" v-if="imgFlag1">
                  <el-button
                    type="danger"
                    @click.stop="handleRemove(1)"
                    icon="el-icon-minus"
                    circle
                  ></el-button>
                </span>
              </el-upload>
            </el-form-item>
            <el-form-item class="imgBox">
              <el-upload
                class="avatar-uploader"
                :action="$ajaxUrl+'/fileupload/api/files/upload'"
                :show-file-list="false"
                :on-success="handleAvatarSuccess2"
                :before-upload="beforeAvatarUpload"
                @click.native="doUpload($event)"
              >
                <img
                  v-if="imageUrl2"
                  :src="imgUrlName+imageUrl2"
                  class="avatar"
                  @mouseover="imgOver(2)"
                  @click.stop="viewImg(2)"
                />
                <i v-else class="el-icon-plus avatar-uploader-icon pr">
                  <font class="imgText text1">侧</font>
                  <font class="imgText text2">面</font>
                </i>
                <span class="removeBox" v-if="imgFlag2">
                  <el-button
                    type="danger"
                    @click.stop="handleRemove(2)"
                    icon="el-icon-minus"
                    circle
                  ></el-button>
                </span>
              </el-upload>
            </el-form-item>
            <el-form-item class="imgBox">
              <el-upload
                class="avatar-uploader"
                :action="$ajaxUrl+'/fileupload/api/files/upload'"
                :show-file-list="false"
                :on-success="handleAvatarSuccess3"
                :before-upload="beforeAvatarUpload"
                @click.native="doUpload($event)"
              >
                <img
                  v-if="imageUrl3"
                  :src="imgUrlName+imageUrl3"
                  class="avatar"
                  @mouseover="imgOver(3)"
                  @click.stop="viewImg(3)"
                />
                <i v-else class="el-icon-plus avatar-uploader-icon pr">
                  <font class="imgText text1">背</font>
                  <font class="imgText text2">面</font>
                </i>
                <span class="removeBox" v-if="imgFlag3">
                  <el-button
                    type="danger"
                    @click.stop="handleRemove(3)"
                    icon="el-icon-minus"
                    circle
                  ></el-button>
                </span>
              </el-upload>
            </el-form-item>
          </el-form>
        </el-header>
        <!-- 头部表单结束 -->
        <!-- table开始 -->
        <div
          v-loading="fullscreenLoading2"
          element-loading-background="rgba(1, 1, 1, 0)"
          element-loading-spinner="el-icon-loading"
        >
          <!-- <div style="height:41px;">
            <el-tabs
              type="card"
              :closable="false"
              @tab-click="tabClick"
              v-on:click.native="listenClick"
              v-model="tabPage"
            >
              <el-tab-pane
                :key="item.id"
                v-for="item in editableTabs"
                :label="item.title"
                :id="item.id"
                :name="item.id"
              >
                <span slot="label" class="tabLabel" :class="{validSty:item.valid}">{{item.title}}</span>
              </el-tab-pane>
            </el-tabs>
          </div>-->

          <!-- <transition name="fade"> -->
          <MainBill
            ref="MainBillBox"
            :tableHv="tableHeight3"
            :PartData="PartData"
            @setEditState="setEditState"
            :formulaData="formulaData"
            @closeLoading="closeLoading"
            :treeItem.sync="treeItem"
          ></MainBill>
          <!-- <MainBill
              ref="MainBillBox"
              :tableHv="tableHeight3"
              :showItem="showItem"
              :PartData="PartData"
              :formulaData="formulaData"
              :bomData="bomData"
              :packageId="item.id"
              :editableTabs="editableTabs"
              @closeLoading="closeLoading"
              @setEditState="setEditState"
          ></MainBill>-->
          <!-- </transition> -->
        </div>

        <!-- table结束 -->
      </div>
      <!-- 右边列表框结束 -->
    </el-main>

    <!-- 选择计算公式开始 -->
    <el-dialog
      title="单用量计算公式"
      :visible.sync="Visible"
      width="500px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div class="el-dialog-div">
        <el-form :model="FormulaDt" :rules="FormulaDtRules" ref="FormulaDt" label-width="128px">
          <el-form-item label="单用量计算公式：" prop="region">
            <el-select
              v-model="FormulaDt.region"
              placeholder="请选择单用量计算公式"
              style="width:300px;"
              filterable
              @change="regionChange"
            >
              <el-option
                :label="item.formulaName"
                :value="item.id"
                v-for="item in formulaData"
                :key="item.id"
              ></el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="公式：">
            <div style="width:300px;">{{FormulaDt.regionName}}</div>
          </el-form-item>
        </el-form>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="Visible=false">关闭</el-button>
        <el-button type="primary" @click="selectFormulaSave">确定</el-button>
      </div>
    </el-dialog>
    <!-- 选择计算公式结束 -->

    <!-- 新增模板开始 -->
    <el-dialog
      title="新增模板"
      :visible.sync="templateVisible"
      width="500px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div class="el-dialog-div">
        <el-form
          :model="TemplateDt"
          :rules="TemplateDtRules"
          ref="TemplateDt"
          label-width="110px"
          @submit.native.prevent
        >
          <el-form-item label="模板名称：" prop="templateName">
            <el-input v-model="TemplateDt.templateName" placeholder="请输入模板名称" style="width:300px" />
          </el-form-item>
        </el-form>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="templateVisible=false">关闭</el-button>
        <el-button type="primary" @click="selectTemplateSave">确定</el-button>
      </div>
    </el-dialog>
    <!-- 新增模板结束 -->

    <!-- 选择模板开始 -->
    <InboundOrder
      ref="TelOrder"
      :BrandData="BrandData"
      @OnBtnSaveSubmit="SelTelSaveSubmit"
      title="模板"
    ></InboundOrder>
    <!-- 选择模板结束 -->

    <!-- 选择模板开始 -->
    <Preview ref="Preview" title="预览" @closeLoading="closeLoading"></Preview>
    <Print ref="Print" @closeLoading="closeLoading"></Print>
    <!-- 选择模板结束 -->

    <!-- 图片预览开始 -->
    <el-dialog :visible.sync="ImgDialogVisible">
      <!-- <img width="100%" :src="'http://47.107.135.203:20200/'+dialogImageUrl" alt /> -->
      <div :style="imgStyle" class="imgBgStyle"></div>
    </el-dialog>
    <!-- 图片预览结束 -->
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import {
  formatDate,
  keepTwoDecimalFull,
  accMul,
  isRealNum,
  isEmpty
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import MainBill from "./components/MainBill";
import DataTree from "./components/DataTree";
import InboundOrder from "./components/InboundOrder";
import Preview from "./components/Preview";
import Print from "./components/Print";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import { setTimeout } from "timers";

export default {
  name: "viewsProductProductBomindexvue",
  data() {
    var pagerCodePass = (rule, value, callback) => {
      if (value) {
        if (/[\u4E00-\u9FA5]/g.test(value)) {
          callback(new Error("不允许输入中文，请重新输入！"));
        } else {
          callback();
        }
      }
    };
    return {
      imgStyle: {
        backgroundImage: ""
      },
      editState: false,
      noPackageCol: false,
      imageUrl1: "",
      imageUrl2: "",
      imageUrl3: "",
      imgFlag1: false,
      imgFlag2: false,
      imgFlag3: false,
      ImgDialogVisible: false,
      dialogImageUrl: "",
      dialogVisible: false, //图片标记
      Visible: false, //弹框标记
      templateVisible: false, //新增模板弹框
      changeTreeState: true,
      fullscreenLoading: false,
      fullscreenLoading2: false,
      FormulaDt: {
        name: "",
        region: "",
        regionName: ""
      },
      FormulaDtRules: {
        region: [
          { required: true, message: "请选择计算公式", trigger: "change" }
        ]
      },
      dtData: {
        pagerCode: "",
        maker: ""
      },
      dtDataRules: {
        pagerCode: [
          {
            min: 0,
            max: 30,
            message: "最大允许输入30个字符，请重新输入！",
            trigger: "blur"
          },
          { validator: pagerCodePass, trigger: "blur" }
        ]
      },
      TemplateDt: {
        templateName: ""
      },
      TemplateDtRules: {
        templateName: [
          { required: true, message: "请选择模板", trigger: "blur" },
          {
            min: 0,
            max: 20,
            message: "最大允许输入20个字符，请重新输入！",
            trigger: "blur"
          }
        ]
      },
      BrandData: [],
      realNameOptions: [], //人员数据
      formulaData: [], //公式数据
      PartData: [], //部位数据
      treeItem: {}, //选中的tree品牌数据
      WholeMainData: [], //配色项目数据
      // TMMColorItem: [], //配色项目
      tableHeight: 500, //table高度
      tableHeight2: 500, //table高度
      tableHeight3: 500, //table高度
      editableTabsValue: "-1",
      tabPage: "0",
      editableTabs: [],
      // tabBoxList: [
      //   {
      //     title: "",
      //     id: -1,
      //     packageId: -1,
      //     component: MainBill
      //   }
      // ],
      childList: [],
      tabVaildList: [], //验证每个Tab数据是否正确
      PreviewData: {}, //预览数据
      bomData: [], //table里面的数据
      showItem: -1,
      printState: 0
    };
  },
  filters: {
    formatTDate: value => {
      if (value == "") {
        return "";
      }
      return formatDate(value);
    }
  },
  components: {
    Pagination,
    MainBill,
    DataTree,
    InboundOrder,
    Preview,
    Print
  },
  watch: {
    dialogImageUrl(val) {
      var url = `${this.imgUrlName}${val}`;
      this.imgStyle.backgroundImage = `url(${url})`;
    }
  },
  created() {
    //获取按钮权限
    // this.fullscreenLoading = true;
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
  },
  async mounted() {
    this.setStyle(); //设置样式
    this.getUserCompany(); //人员列表
    this.getFormulaData(); //公式列表
    this.PartData = await this.getPartName(); //获取部位信息
  },
  async activated() {},
  deactivated() {},
  methods: {
    closeLoading(state) {
      //关闭Loading
      setTimeout(() => {
        if (state) {
          this.fullscreenLoading2 = false;
        } else {
          this.fullscreenLoading = false;
        }
      }, 200);
    },
    setEditState(flag) {
      //设置编辑状态
      this.editState = flag;
      // console.log(this.editState)
    },
    editChange() {
      //设置头部编辑状态
      if (this.treeItem.id == undefined) {
        return false;
      }
      this.editState = true;
    },
    setBrand(data) {
      this.BrandData = data;
    },
    viewImg(state) {
      this.ImgDialogVisible = true;
      if (state == 1) {
        this.dialogImageUrl = this.imageUrl1;
      }
      if (state == 2) {
        this.dialogImageUrl = this.imageUrl2;
      }
      if (state == 3) {
        this.dialogImageUrl = this.imageUrl3;
      }
    },
    setEditableTabs() {
      //设置头部
      this.noPackageCol = true; //设置可填
      this.imageUrl1 = this.WholeMainData.frontImgPath;
      this.imageUrl2 = this.WholeMainData.sideImgPath;
      this.imageUrl3 = this.WholeMainData.backImgPath;
      this.dtData.pagerCode = this.WholeMainData.pagerCode;
      this.dtData.maker = this.WholeMainData.maker;
      if (isEmpty(this.WholeMainData.childList)) {
        setTimeout(() => {
          this.$refs["MainBillBox"].resetTable();
        }, 100);
      } else {
        this.$refs["MainBillBox"].isShowFlag = false;
        this.$refs["MainBillBox"].setBomData(this.WholeMainData.childList);
      }
    },
    async nodeClick(item) {
      //点击左边菜单
      this.fullscreenLoading = true;
      this.treeItem = item;
      this.$nextTick(() => {
        this.$refs["dtData"].resetFields();
      });
      this.WholeMainData = await this.getWholeMainData(item); //获取具体数据
      if (isEmpty(this.WholeMainData)) {
        this.setHeaderData_reset();
        setTimeout(() => {
          this.$refs["MainBillBox"].resetTable();
        }, 100);
      } else {
        this.setEditableTabs(); //设置配色项目
      }
    },
    // tabClick(data) {
    //   //点击配色项目
    //   this.fullscreenLoading2 = true;
    //   this.showItem = parseInt(data.name.split("-")[1]);
    //   this.$refs["MainBillBox"][0].setBomData(this.showItem);

    //   return;
    //   this.editableTabsValue = data.$attrs.id;
    //   setTimeout(() => {
    //     this.$refs[this.editableTabsValue][0].setBomData();
    //   }, 0);
    //   this.tableHeight3 = this.tableHeight2;
    //   this.listenClick();
    //   var flag = false;

    //   this.tabBoxList.map(item => {
    //     if (item.id == this.editableTabsValue) {
    //       flag = true;
    //       return;
    //     }
    //   });
    //   if (flag === false) {
    //     this.editableTabs.map(item => {
    //       if (item.id == this.editableTabsValue) {
    //         this.tabBoxList.push(item);
    //       }
    //     });
    //   }
    // },
    handleRemove(state) {
      this.editState = true;
      if (state == 1) {
        this.imageUrl1 = "";
        this.imgFlag1 = false;
      }
      if (state == 2) {
        this.imageUrl2 = "";
        this.imgFlag2 = false;
      }
      if (state == 3) {
        this.imageUrl3 = "";
        this.imgFlag3 = false;
      }
    },
    handleAvatarSuccess1(res, file) {
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      this.editState = true;
      this.imgFlag1 = true;
      this.imageUrl1 = res;
    },
    handleAvatarSuccess2(res, file) {
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      this.editState = true;
      this.imgFlag2 = true;
      this.imageUrl2 = res;
    },
    handleAvatarSuccess3(res, file) {
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      this.editState = true;
      this.imgFlag3 = true;
      this.imageUrl3 = res;
    },
    imgOver(state) {
      this.imgFlag = state;
    },
    imgOut() {
      this.imgFlag = -1;
    },
    doUpload(ev) {
      if (!this.noPackageCol) {
        ev.preventDefault();
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
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = 30; //按钮高度
        var tabBtn = 0; //按钮高度
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        // var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        // var f = elfooter.clientHeight || elfooter.offsetHeight;
        this.tableHeight = b - 20;
        this.tableHeight2 = b - h - 20 - btn - tabBtn;
        this.tableHeight3 = b - h - 20 - btn - tabBtn;
      });
    },
    regionChange() {
      var regionName = this.formulaData.filter(item => {
        if (item.id == this.FormulaDt.region) {
          return item;
        }
      });
      this.FormulaDt.regionName = regionName[0].frontFormula;
    },
    selectFormula() {
      //选择计算公式
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      if (!this.noPackageCol) {
        this.$message.error(
          "该包型还没进行配色方案的配置，请到配色方案中配置配色项目！"
        );
        return false;
      }

      this.Visible = true;
      this.FormulaDt.region = "";
      this.FormulaDt.regionName = "";
      this.$nextTick(() => {
        this.$refs["FormulaDt"].resetFields();
      });
    },
    selectFormulaSave() {
      var flag = false;
      this.$refs.FormulaDt.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
          flag = false;
        } else {
          flag = true;
        }
      });
      if (flag === false) {
        return;
      }
      this.$refs["MainBillBox"].setFormula(this.FormulaDt.region);
      this.Visible = false;
    },
    getFormulaData() {
      //查看公式配置
      var reqObject = RequestObject.LonginBookObject(false, 0, 0, null, null);
      var queryConditions = [];
      request({
        url: "/manufacturing/api/TMMFormula",
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
          this.formulaData = res.data;
          this.total = res.totalNumber;
        }
      });
    },
    getPartName() {
      return new Promise(function(reslove, reject) {
        //部位数据
        var QueryCondition = [
          { column: "TypeName", content: "部位名称", condition: 0 }
        ];
        var requsets = {
          IsPaging: false,
          PageSize: 0,
          PageIndex: 0,
          QueryConditions: QueryCondition,
          OrderByConditions: null
        };
        request({
          url: "/basicset/api/TBMDictionary",
          method: "GET",
          params: { requestObject: JSON.stringify(requsets) }
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            reslove(res.data);
          }
        });
      });
    },

    setHeaderData_reset() {
      //设置头部信息
      this.dtData.pagerCode = "";
      this.dtData.maker = "";
      this.imageUrl1 = "";
      this.imageUrl2 = "";
      this.imageUrl3 = "";
      this.imgFlag1 = false;
      this.imgFlag2 = false;
      this.imgFlag3 = false;

      this.tabBoxList = [];
      this.tabBoxList.push({
        title: "",
        id: -1,
        packageId: -1,
        component: MainBill
      });
      this.editableTabsValue = -1;
      this.changeTreeState = false;
      setTimeout(() => {
        this.changeTreeState = true;
        // this.fullscreenLoading = false;
      }, 0);
    },
    // getTMMColorItem(item) {
    //   this.treeItem = item;
    //   //获取配色方案和BOM
    //   var QueryCondition = [
    //     { column: "packageid", content: item.id, condition: 0 }
    //   ];
    //   var requsets = {
    //     IsPaging: false,
    //     PageSize: 0,
    //     PageIndex: 0,
    //     QueryConditions: QueryCondition,
    //     OrderByConditions: null
    //   };
    //   return new Promise((reslove, reject) => {
    //     request({
    //       url: "/manufacturing/api/TMMColorItem",
    //       method: "GET",
    //       params: { requestObject: JSON.stringify(requsets) }
    //     }).then(res => {
    //       if (res.code == -1) {
    //         this.$confirm(res.info, "错误信息", {
    //           confirmButtonText: "确定",
    //           type: "error",
    //           showCancelButton: false
    //         });
    //       } else {
    //         reslove(res.data);
    //       }
    //     });
    //   });
    // },
    getWholeMainData(item) {
      return new Promise((reslove, reject) => {
        request({
          // url: "/manufacturing/api/TMMBOMMain/GetWholeMainData",
          url: "/manufacturing/api/TMMBOMMainNew/GetWholeMainData",
          method: "GET",
          params: { requestObject: item.id }
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            // var d = res.data !== null ? res.data : {};
            reslove(res.data);
          }
        });
      });
    },
    getMaterielData(item) {
      //获取物料档案信息
      this.materielData = [];

      var queryData = [];
      if (item) {
        queryData.push({
          column: "materialCode",
          content: item.materialCode.value,
          condition: 6
        });
        queryData.push({
          column: "materialName",
          content: item.materialName.value,
          condition: 6
        });
      }

      var reqObject;
      reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        null,
        queryData
      );
      request({
        url: "/basicset/api/TBMMaterialFile",
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
          this.materielData = res.data;
          this.totalCount = res.totalNumber;
        }
      });
    },
    getUserCompany() {
      /**
       * getUserCompany
       * 获取当前用户公司员工
       */
      var reqObject = RequestObject.LonginBookObject(false, 0, 0, null, null);
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
    listenClick() {
      // this.$nextTick(() => {
      //   this.tabBoxList.map(item => {
      //     if (this.$refs[`${item.id}`]) {
      //       if (this.$refs[`${item.id}`][0]) {
      //         this.$refs[`${item.id}`][0].listenClick();
      //         // this.$refs[`${item.id}`][0].firstState = true;
      //       }
      //     }
      //   });
      // });
      this.$refs["MainBillBox"].listenClick();
    },
    // setFirst() {
    //   this.$nextTick(() => {
    //     this.tabBoxList.map(item => {
    //       if (this.$refs[`${item.id}`]) {
    //         if (this.$refs[`${item.id}`][0]) {
    //           this.$refs[`${item.id}`][0].firstState = true;
    //         }
    //       }
    //     });
    //   });
    // },
    async doSave(state) {
      //点击保存
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      // if (!this.noPackageCol) {
      //   this.$message.error(
      //     "该包型还没进行配色方案的配置，请到配色方案中配置配色项目！"
      //   );
      //   return false;
      // }
      //保存
      var flag = true; //验证头部数据
      this.$refs.dtData.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
          flag = false;
        } else {
          flag = true;
        }
      });
      if (!flag) return; //验证头部数据
      this.fullscreenLoading = true;
      this.tabVaildList = [];
      this.$refs[`MainBillBox`].addPutInStorage().then(res => {
        if (res.code != 1) {
          var valid = res.data.reverse();
          valid.forEach(item => {
            this.editableTabs.forEach(dt => {
              dt.valid = false;
              if (item == dt.id.split("-")[1]) {
                dt.valid = true;
                return;
              }
            });
          });
          this.$message({
            message: "数据不合法",
            type: "error"
          });
          this.fullscreenLoading = false;
        } else {
          if (state) {
            this.postTMMBOMMainData(res.data, state);
          } else {
            this.postTMMBOMMainData(res.data);
          }
        }
      });
    },

    postTMMBOMMainData(childList, state) {
      //请求保存数据
      if (childList.length <= 0) {
        this.$message({
          message: "请输入数据！",
          type: "error"
        });
        this.fullscreenLoading = false;
        return;
      }

      //预览
      if (state == 2) {
        // childList.map(d => {
        //   this.PartData.map(item => {
        //     if (d.partId === item.id) {
        //       d.dicValue = item.dicValue;
        //     }
        //   });
        //   this.TMMColorItem.map(k => {
        //     if (d.itemId == k.itemId) {
        //       d.itemName = k.itemName;
        //     }
        //   });
        // });
        if (this.printState == 1) {
          this.$refs.Preview.setTableData(childList);
          this.$refs.Preview.open(this.printState);
        }

        if (this.printState == 2 || this.printState == 3) {
          this.$refs.Print.setTableData(childList);
          this.$refs.Print.open(this.printState);
        }

        return;
      }

      var param = {
        id: 0,
        packageId: this.treeItem.id,
        childList: childList
      };
      if (this.dtData.pagerCode != "") {
        param.pagerCode = this.dtData.pagerCode;
      }
      if (this.dtData.maker != "") {
        param.maker = this.dtData.maker;
      }

      if (this.imageUrl1 != "") {
        param.frontImgPath = this.imageUrl1;
      }
      if (this.imageUrl2 != "") {
        param.sideImgPath = this.imageUrl2;
      }
      if (this.imageUrl3 != "") {
        param.backImgPath = this.imageUrl3;
      }
      // console.log(this.dtData)
      var URL = "";
      if (state === 1) {
        URL = "/manufacturing/api/TMMBOMTempMain";
        param.companyId = 0;
        param.tempName = this.TemplateDt.templateName;
      } else {
        URL = "/manufacturing/api/TMMBOMMainNew";
      }

      request({
        url: URL,
        method: "post",
        data: { postData: param }
      }).then(res => {
        this.fullscreenLoading = false;
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.editState = false;
          this.$message({
            message: "操作成功",
            type: "success"
          });
          if (state === 1) {
            this.templateVisible = false;
          } else {
            this.nodeClick(this.treeItem);
          }
        }
      });
    },
    saveTemplate() {
      //点击保存模板
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      if (!this.noPackageCol) {
        this.$message.error(
          "该包型还没进行配色方案的配置，请到配色方案中配置配色项目！"
        );
        return false;
      }

      this.templateVisible = true;
      this.$nextTick(() => {
        this.$refs["TemplateDt"].resetFields();
      });
      this.TemplateDt.templateName = "";
    },
    selectTemplateSave() {
      //保存模板
      var flag = false;
      this.$refs.TemplateDt.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
          flag = false;
        } else {
          flag = true;
        }
      });
      if (flag === false) {
        return;
      }
      this.doSave(1);
    },
    selectTel() {
      //选择模板
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      // if (!this.noPackageCol) {
      //   this.$message.error(
      //     "该包型还没进行配色方案的配置，请到配色方案中配置配色项目！"
      //   );
      //   return false;
      // }

      this.$refs.TelOrder.open();
      this.$refs.TelOrder.dtData.tempName = "";
      this.$refs.TelOrder.dtData.packageId = this.treeItem.id;
      this.$refs.TelOrder.getMainList(this.treeItem.id);
    },
    SelTelSaveSubmit(item) {
      //保存选择模板
      this.fullscreenLoading = true;
      request({
        url: "/manufacturing/api/TMMBOMTempMain/GetWholeMainData",
        method: "get",
        params: {
          requestObject: item.id
        }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.editState = true;
          this.WholeMainData = res.data;
          this.setEditableTabs();
        }
      });
    },
    doPreview(state) {
      //预览
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      this.printState = state;

      // realNameOptions
      var realName = "";
      this.realNameOptions.map(item => {
        if (item.id === this.dtData.maker) {
          realName = item.realName;
          return;
        }
      });
      this.PreviewData = {
        title: `${this.treeItem.dicCode}${this.treeItem.dicValue}`,
        pagerCode: this.dtData.pagerCode,
        maker: this.dtData.maker,
        realName: realName,
        frontImgPath: this.imageUrl1,
        sideImgPath: this.imageUrl2,
        backImgPath: this.imageUrl3,
        colorList: this.editableTabs
      };
      if (state == 1) {
        this.$refs.Preview.setData(this.PreviewData);
      }
      if (state == 2 || state == 3) {
        this.$refs.Print.setData(this.PreviewData);
      }

      this.doSave(2);
    }
  }
};
</script>
<style lang="scss" scoped>
#ProductBom /deep/ {
  #elfooter {
    padding: 0px 20px;
  }
  .headerBtn {
    padding: 5px 0px 0px 20px;
    height: 37px;
    font-size: 0px;
  }
  .dropdown button {
    position: relative;
    left: -1px;
    height: 32px;
    vertical-align: middle;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
  }
  .el-dropdown {
    vertical-align: middle;
  }
  .leftBox {
    float: left;
    width: 200px;
    border: 1px solid #dfe6ec;
    overflow-y: auto;
    border-right: 1px solid #dfe6ec;
  }
  .rightBox {
    float: left;
    width: calc(100% - 220px);
    border: 1px solid #dfe6ec;
    overflow: hidden;
    margin-left: 20px;
  }
  .el-tabs__header {
    margin: 0px;
  }
  is-active {
    border-bottom: 1px solid #dfe4ed;
  }
  .el-tabs__content {
    overflow: visible;
  }
  .el-tabs--card > .el-tabs__header .el-tabs__nav {
    border-left: 0px solid #dfe4ed;
    border-top-left-radius: 0px;
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
  .el-dialog__title {
    color: #1890ff;
  }
  .el-dialog__header {
    text-align: left;
  }
  .removeBox {
    position: absolute;
    right: 0px;
    top: 0px;
    font-weight: 800;
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
  .el-main {
    padding-bottom: 4px;
    overflow: hidden;
  }
  .validSty {
    background: #f3d4d4;
    display: inline-block;
    width: 100%;
    height: 100%;
  }
  .tabLabel {
    padding: 0px 20px;
  }
  .el-tabs__item {
    padding: 0px;
  }
  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.1s;
  }
  .fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
    opacity: 0;
  }
  .imgBgStyle {
    width: 100%;
    height: 500px;
    background: #fff;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
  }
  /deep/ .el-table::before {
    height: 0px;
  }
}
@import "@/styles/receipts.scss";
</style>
