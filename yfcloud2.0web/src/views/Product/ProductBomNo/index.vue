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
        <!-- 表头按钮开始 -->
        <!-- <div class="headerBtn">
          <div v-if="btnAip.add">
            <el-button-group class="groupBtn">
              <el-button type="default" v-if="btnAip.preserve&&btnAip.preserve.buttonCaption" @click="doSave">{{ btnAip.preserve.buttonCaption }}</el-button>
              <el-button type="default" v-if="btnAip.preview&&btnAip.preview.buttonCaption" @click="doPreview">{{ btnAip.preview.buttonCaption }}</el-button>
              <el-button type="default" v-if="btnAip.savetemplate&&btnAip.savetemplate.buttonCaption" @click="saveTemplate">{{ btnAip.savetemplate.buttonCaption }}</el-button>
              <el-button type="default" v-if="btnAip.seltemplate&&btnAip.seltemplate.buttonCaption" @click="selectTel">{{ btnAip.seltemplate.buttonCaption }}</el-button>
              <el-button type="default" v-if="btnAip.selformula&&btnAip.selformula.buttonCaption" @click="selectFormula">{{ btnAip.selformula.buttonCaption }}</el-button>
            </el-button-group>
          </div>
        </div>-->
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
              @click="doPreview"
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

        <transition name="fade" v-for="item in tabBoxList" :key="item.id">
          <component
            v-show="changeTreeState"
            :ref="item.id"
            :is="item.component"
            :tableHv="tableHeight3"
            :PartData="PartData"
            :formulaData="formulaData"
            :bomData="WholeMainData"
            :packageId="item.id"
            @closeLoading="closeLoading"
            @setEditState="setEditState"
          ></component>
        </transition>

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
              @change="regionChange"
              filterable
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
  isRealNum
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import MainBill from "./components/MainBill";
import DataTree from "./components/DataTree";
import InboundOrder from "./components/InboundOrder";
import Preview from "./components/Preview";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import { setTimeout } from "timers";

export default {
  name: "viewsProductProductBomNoindexvue",
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
      imageUrl1: "",
      imageUrl2: "",
      imageUrl3: "",
      imgFlag1: false,
      imgFlag2: false,
      imgFlag3: false,
      ImgDialogVisible: false,
      dialogImageUrl: "",
      dialogImageUrl: "",
      dialogVisible: false, //图片标记
      Visible: false, //弹框标记
      templateVisible: false, //新增模板弹框
      changeTreeState: true,
      fullscreenLoading: false,
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
      TMMColorItem: [], //配色项目
      tableHeight: 500, //table高度
      tableHeight2: 500, //table高度
      tableHeight3: 500, //table高度
      editableTabsValue: "-1",
      tabPage: "0",
      editableTabs: [
        {
          title: "",
          id: "BOMNO",
          packageId: -1,
          component: MainBill
        }
      ],
      tabBoxList: [
        {
          title: "",
          id: "BOMNO",
          packageId: -1,
          component: MainBill
        }
      ],
      childList: [],
      tabVaildList: [], //验证每个Tab数据是否正确
      PreviewData: {} //预览数据
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
    Preview
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
    closeLoading() {
      //关闭Loading
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 500);
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
      // this.fullscreenLoading = false;
      // this.changeTreeState = false;
      // setTimeout(() => {
      //   this.changeTreeState = true;
      //   setTimeout(() => {

      //   }, 0);
      // }, 0);
      this.$refs["BOMNO"][0].EditFlag = true;
      this.setHeaderData(this.WholeMainData);
      this.$refs["BOMNO"][0].setBomData(this.WholeMainData);
      return;
      // editableTabsValue
      // this.editableTabs = [];
      // this.TMMColorItem.map(item => {
      //   this.editableTabs.push({
      //     title: item.itemName,
      //     id: `${this.treeItem.id}-${item.itemId}`,
      //     packageId: item.packageId,
      //     component: MainBill,
      //     valid: false
      //   });
      // });

      // if (this.editableTabs.length > 0) {
      //   this.changeTreeState = false;
      //   setTimeout(() => {
      //     this.changeTreeState = true;
      //   }, 0);

      //   this.tabBoxList = [];

      //   this.tabBoxList = [...this.editableTabs];
      //   this.tabPage = this.editableTabs[0].id;
      //   this.setHeaderData(this.WholeMainData);

      //   this.tabClick({
      //     $attrs: {
      //       id: this.editableTabs[0].id
      //     }
      //   });
      //   this.tabBoxList.map(item => {
      //     setTimeout(() => {
      //       this.$refs[item.id][0].setBomData();
      //     }, 0);
      //   });
      //   this.tableHeight3 = this.tableHeight2 - 22;
      // } else {
      //   this.changeTreeState = false;
      //   this.setHeaderData(this.WholeMainData);
      // }
    },
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
      this.imgFlag1 = true;
      this.imageUrl1 = res;
    },
    handleAvatarSuccess2(res, file) {
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      this.imgFlag2 = true;
      this.imageUrl2 = res;
    },
    handleAvatarSuccess3(res, file) {
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
      this.imgFlag3 = true;
      this.imageUrl3 = res;
    },
    imgOver(state) {
      this.imgFlag = state;
    },
    imgOut() {
      this.imgFlag = -1;
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
    tabClick(data) {
      //点击配色项目
      this.editableTabsValue = data.$attrs.id;
      setTimeout(() => {
        this.$refs[this.editableTabsValue][0].setBomData();
      }, 0);
      // this.tableHeight3 = this.tableHeight2;
      this.listenClick();
      var flag = false;

      this.tabBoxList.map(item => {
        if (item.id == this.editableTabsValue) {
          flag = true;
          return;
        }
      });
      if (flag === false) {
        this.editableTabs.map(item => {
          if (item.id == this.editableTabsValue) {
            this.tabBoxList.push(item);
          }
        });
      }
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = 30; //按钮高度
        var tabBtn = 41; //按钮高度
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        // var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        // var f = elfooter.clientHeight || elfooter.offsetHeight;
        this.tableHeight = b - 20;
        this.tableHeight2 = b - h - 20 - btn;
        this.tableHeight3 = b - h - 20 - btn;
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
      this.$refs["BOMNO"][0].setFormula(this.FormulaDt.region);
      // this.$refs[this.editableTabsValue][0].setFormula(this.FormulaDt.region);
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
        var QueryCondition = [{ column: "TypeName", content: "部位名称", condition: 0 }];
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
    async nodeClick(item) {
      //点击左边菜单
      this.$nextTick(() => {
        this.$refs["dtData"].resetFields();
      });
      this.listenClick();
      this.fullscreenLoading = true;
      this.treeItem = item;
      // this.TMMColorItem = await this.getTMMColorItem(item); //获取配色项目
      this.WholeMainData = await this.getWholeMainData(); //获取具体数据
      // console.log(this.WholeMainData)

      this.setEditableTabs(); //设置配色项目
    },
    setHeaderData(item) {
      //设置头部信息
      if (item === null) {
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
        }, 0);
      } else {
        this.dtData.pagerCode = item.pagerCode;
        this.dtData.maker = item.maker;
        this.imageUrl1 = item.frontImgPath;
        this.imageUrl2 = item.sideImgPath;
        this.imageUrl3 = item.backImgPath;

        if (this.imageUrl1 != "" && this.imageUrl1 != null) {
          this.imgFlag1 = true;
        } else {
          this.imgFlag1 = false;
        }
        if (this.imageUrl2 != "" && this.imageUrl2 != null) {
          this.imgFlag2 = true;
        } else {
          this.imgFlag2 = false;
        }
        if (this.imageUrl3 != "" && this.imageUrl3 != null) {
          this.imgFlag3 = true;
        } else {
          this.imgFlag3 = false;
        }
      }
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
    getWholeMainData() {
      return new Promise((reslove, reject) => {
        request({
          url: "/manufacturing/api/TMMBOMNMain/GetWholeMainData",
          method: "GET",
          params: { requestObject: this.treeItem.id }
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            var d = res.data !== null ? res.data : {};
            reslove(d);
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
      this.$nextTick(() => {
        this.tabBoxList.map(item => {
          if (this.$refs[`${item.id}`]) {
            if (this.$refs[`${item.id}`][0]) {
              this.$refs[`${item.id}`][0].listenClick();
              // this.$refs[`${item.id}`][0].firstState = true;
            }
          }
        });
      });
    },
    setFirst() {
      this.$nextTick(() => {
        this.tabBoxList.map(item => {
          if (this.$refs[`${item.id}`]) {
            if (this.$refs[`${item.id}`][0]) {
              this.$refs[`${item.id}`][0].firstState = true;
            }
          }
        });
      });
    },
    async doSave(state) {
      //点击保存
      // console.log(this.treeItem)
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }
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

      // 验证列表数据
      this.tabVaildList = [];
      this.childList = [];
      this.tabBoxList.map((item, index) => {
        if (this.$refs[`${item.id}`]) {
          this.$refs[`${item.id}`][0].addPutInStorage().then(res => {
            if (res === false) {
              this.tabVaildList.push(item.id.split("-")[1]);
              if (index == this.tabBoxList.length - 1) {
                if (state) {
                  this.postTMMBOMMainData(
                    this.childList,
                    this.tabVaildList,
                    state
                  );
                } else {
                  this.postTMMBOMMainData(this.childList, this.tabVaildList);
                }
              }
            } else {
              res.forEach(dt => {
                this.childList.push(dt);
              });
              if (index == this.tabBoxList.length - 1) {
                if (state) {
                  this.postTMMBOMMainData(
                    this.childList,
                    this.tabVaildList,
                    state
                  );
                } else {
                  this.postTMMBOMMainData(this.childList, this.tabVaildList);
                }
              }
            }
          });
        }
      });
    },
    postTMMBOMMainData(childList, valid, state) {
      //请求保存数据

      if (valid.length > 0) {
        this.$message({
          message: "数据不合法",
          type: "error"
        });
        //设置报错
        valid.forEach(item => {
          this.editableTabs.forEach(dt => {
            dt.valid = false;
            if (item == dt.id.split("-")[1]) {
              dt.valid = true;
            }
          });
        });
        this.fullscreenLoading = false;
        return;
      }

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
        childList.map(d => {
          this.PartData.map(item => {
            if (d.partId === item.id) {
              d.dicValue = item.dicValue;
            }
          });
          this.TMMColorItem.map(k => {
            if (d.itemId == k.itemId) {
              d.itemName = k.itemName;
            }
          });
        });

        this.$refs.Preview.setTableData(childList);
        this.$refs.Preview.open();
        return;
      }

      var param = {
        id: 0,
        packageId: this.treeItem.id,
        childList: childList
      };
      if (this.dtData.pagerCode !== "") {
        param.pagerCode = this.dtData.pagerCode;
      }
      if (this.dtData.maker !== "") {
        param.maker = this.dtData.maker;
      }

      if (this.imageUrl1 !== "") {
        param.frontImgPath = this.imageUrl1;
      }
      if (this.imageUrl2 !== "") {
        param.sideImgPath = this.imageUrl2;
      }
      if (this.imageUrl3 !== "") {
        param.backImgPath = this.imageUrl3;
      }
      var URL = "";
      if (state === 1) {
        URL = "/manufacturing/api/TMMBOMNTempMain";
        param.companyId = 0;
        param.tempName = this.TemplateDt.templateName;
      } else {
        URL = "/manufacturing/api/TMMBOMNMain";
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
      this.$refs.TelOrder.open();
      this.$refs.TelOrder.dtData.tempName = "";
      this.$refs.TelOrder.dtData.packageId = this.treeItem.id;
      this.$refs.TelOrder.getMainList(this.treeItem.id);
    },
    SelTelSaveSubmit(item) {
      this.fullscreenLoading = true;
      //保存选择模板
      request({
        url: "/manufacturing/api/TMMBOMNTempMain/GetWholeMainData",
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
          // this.setFirst(); //重置tab第一次模式
          // this.tabPage = this.editableTabs[0].id; //设置tab回来第一个
          this.WholeMainData = res.data; //设置数据
          this.setHeaderData(this.WholeMainData); //导入头部数据
          this.$refs["BOMNO"][0].setBomData(this.WholeMainData);
          // //导入第一个tab数据
          // this.tabClick({
          //   $attrs: {
          //     id: this.editableTabs[0].id
          //   }
          // });
        }
      });
    },
    doPreview() {
      //预览
      if (this.treeItem.id == undefined) {
        this.$message.error("请选择包型！");
        return false;
      }

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
      this.$refs.Preview.setData(this.PreviewData);
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
  .el-table--border {
    border-top: 1px solid #dfe6ec;
  }
  .imgBgStyle {
    width: 100%;
    height: 500px;
    background: #fff;
    background-size: contain;
    background-position: center;
    background-repeat: no-repeat;
  }
}
@import "@/styles/receipts.scss";
</style>
