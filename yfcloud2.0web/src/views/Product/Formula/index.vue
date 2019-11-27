<template>
  <el-container
    v-loading="loading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header
      id="elheader"
      style="height:auto"
      class="headerBd"
    >
      <el-button
        v-if="btnAip.add.buttonCaption"
        type="primary"
        @click="handelAddClick"
      >{{ btnAip.add.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.batchdelete.buttonCaption"
        :type="typeColor"
        @click="handelBatchDelete"
      >{{ btnAip.batchdelete.buttonCaption }}</el-button>
      <!-- <el-button type="primary" @click="handelAddClick">新增</el-button>
      <el-button type="info" @click="handelBatchDelete">批量删除</el-button> -->
    </el-header>
    <el-main id="elmain">
      <el-table
        align="center"
        :data="formulaData"
        style="width: 100%"
        border
        :header-cell-style="{background:'rgb(250, 250, 250)',color:'#333'}"
        :height="mainHeight + 'px'"
        @selection-change="handleSelectionChange"
      >
        <el-table-column
          type="selection"
          width="50"
        />
        <el-table-column
          prop="formulaName"
          label="公式名称"
        />
        <el-table-column
          prop="frontFormula"
          label="公式算法"
          width="350"
        />
        <!-- <el-table-column
          prop="formula"
          label="公式算法"
        /> -->
        <el-table-column
          prop="createTime"
          label="创建时间"
        >
          <template slot-scope="scope">{{scope.row.createTime|formatTDate}}</template>
        </el-table-column>
        <el-table-column
          prop="createName"
          label="创建人"
        />
        <el-table-column
          prop="updateTime"
          label="更新时间"
        >
          <template slot-scope="scope">{{scope.row.updateTime|formatTDate}}</template>
        </el-table-column>
        <el-table-column
          prop="updateName"
          label="更新人"
        />
        <el-table-column
          prop="remark"
          label="备注"
          width="280"
        >
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.remark&&scope.row.remark.length>=20"
              class="item"
              effect="light"
              :content="scope.row.remark"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.remark }}</div>
            </el-tooltip>
            <div
              v-if="scope.row.remark&&scope.row.remark.length<20"
              class="ellipsis"
            >{{ scope.row.remark }}</div>
          </template>
        </el-table-column>

        <el-table-column
          label="操作"
          fixed="right"
          width="105"
        >
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
              v-if="btnAip.delete.buttonCaption&&!showtips"
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
      :visible.sync="formulaVisible"
      width="40%"
      :close-on-click-modal="false"
    >
      <div class="formulaBox">
        <div class="formulaLis">
          <a @click="getOperator('长',false,1)">长</a>
          <a @click="getOperator('宽',false,1)">宽</a>
          <a @click="getOperator('件数',false,1)">件数</a>
          <a @click="getOperator('封度（宽幅）',false,1)">封度（宽幅）</a>
          <a @click="getOperator('损耗',false,1)">损耗</a>
          <a @click="getFocus">系数</a>
          <a
            @click="getCoeffNum"
            v-if="!coeffShow"
            :style="errorFe?'background: #fef0f0;':''"
            class="num"
          >
            <div v-if="coefficient&&coefficient.length<12">
              {{coefficient}}
            </div>
            <el-popover
              placement="top-start"
              width="150"
              trigger="hover"
              :content="coefficient"
              v-if="coefficient&&coefficient.length>=12"
            >
              <div
                slot="reference"
                class="ellipsis"
              >{{ coefficient }}</div>
            </el-popover>
          </a>
          <div v-if="coeffShow">
            <el-input
              id="number"
              v-model="coeffNum"
              @blur="coeffBlur"
            ></el-input>
          </div>
        </div>
        <el-form
          ref="formulaForm"
          :model="formulaForm"
          :rules="formulaRules"
        >
          <div class="operator">
            <a @click="getOperator('()',true,3)">()</a>
            <!-- <a @click="getOperator('[]',true,3)">[]</a> -->
            <a
              :style="formulaForm.frontFormula.length==0?'background-color: #eee;':''"
              @click="getOperator('+',false,2)"
            >+</a>
            <a
              :style="formulaForm.frontFormula.length==0?'background-color: #eee;':''"
              @click="getOperator('-',false,2)"
            >-</a>
            <a
              :style="formulaForm.frontFormula.length==0?'background-color: #eee;':''"
              @click="getOperator('*',false,2)"
            >*</a>
            <a
              :style="formulaForm.frontFormula.length==0?'background-color: #eee;':''"
              @click="getOperator('/',false,2)"
            >/</a>
          </div>
          <el-form-item
            label="公式名称："
            prop="formulaName"
            :label-width="formLabelWidth"
          >
            <el-input placeholder="请输入公式名称" v-model="formulaForm.formulaName" />
          </el-form-item>
          <el-form-item
            label="公式算法："
            prop="frontFormula"
            :label-width="formLabelWidth"
          >
            <div style="display: flex;">
              <el-input
              placeholder="请输入公式算法"
                :class="error?'contactNumber':''"
                id="Name"
                clearable
                v-model="formulaForm.frontFormula"
                @blur="frontBlur"
                @keydown.native="frontFormulaVal($event)"
                @keyup.native="chainVal($event)"
                style="width: 90%;"
              />
              <div
                v-if="error"
                class="el-form-item__error"
              >{{errorTex}}</div>
              <el-button
                @click="Back"
                style="margin-left:20px;"
                type="primary"
              >回退</el-button>
            </div>
            <!-- <div>{{formulaForm.frontFormula}}</div> -->
          </el-form-item>
          <el-form-item
            label="备注："
            prop="remark"
            :label-width="formLabelWidth"
            style="margin-bottom: 0px;"
          >
            <el-input
             placeholder="请输入备注"
              v-model="formulaForm.remark"
              type="textarea"
              :rows="3"
            />
          </el-form-item>
        </el-form>
      </div>
      <div
        slot="footer"
        class="dialog-footer"
      >
        <el-button @click="formulaVisible = false">关闭</el-button>
        <el-button
          type="primary"
          :loading="saveBtn"
          @click="handelSave"
        >保存</el-button>
      </div>
    </el-dialog>
    <!-- 分页 -->
    <el-footer
      id="elfooter"
      :height="footHeight"
    >
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
// import fireKeyEvent from "@/utils/fireKeyEvent";
import Pagination from "@/components/Pagination";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { getBtnCtr } from "@/utils/BtnControl";
import { getStyle } from "@/utils/Dom.js";

export default {
  name: "viewsReserveAdminWarehouseindexvue",
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
    return {
      loading: true,
      saveBtn: false,
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
      error: false,
      errorTex: "请输入正确的公式算法!",
      coeffNum: "",
      coefficient: "",
      errorFe: false,
      coeffShow: false,
      formulaVisible: false,
      title: "新增公式配置",
      method: "post",
      showtips: true,
      formLabelWidth: "100px",
      allSelectionChange: [],
      formulaData: [],
      realNameOptions: [],
      operatorLis: "",
      operatorData: [],
      isId: "",
      postDataEdit: {},
      formulaForm: {
        formulaName: "",
        formula: "",
        frontFormula: "",
        _LogName: "",
        remark: "",
        id: ""
      },
      formulaRules: {
        formulaName: [
          { required: true, message: "请输入公式名称", trigger: "blur" },
          {
            min: 1,
            max: 20,
            message: "最大允许输入20个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        // formula: [
        //   { required: true, message: "请输入公式名称", trigger: "blur" },
        //   {
        //     min: 1,
        //     max: 100,
        //     message: "最大允许输入20个字符，请重新输入！",
        //     trigger: "blur"
        //   }
        // ],
        frontFormula: [
          { required: true, message: "请输入公式名称", trigger: "blur" },
          {
            min: 1,
            max: 100,
            message: "最大允许输入100个字符，请重新输入！",
            trigger: "blur"
          }
        ],
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
    formulaData() {
      if (this.pageIndex > 1 && this.formulaData.length == []) {
        this.pageIndex--;
        // 初始化表格数据
        this.getFormulaData();
      }
    },
    formulaVisible(val) {
      if (val) {
        setTimeout(() => {
          // this.pswOnly = false;
        }, 500);
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.formulaForm.resetFields();
        this.error = false;
        this.coeffNum = "";
        this.coefficient = "";
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );

    this.getFormulaData(); // 查看公式配置
    this.setStyle(); // 设置页面样式
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
     * getFormulaData
     * 查看公式配置
     */
    getFormulaData() {
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        null,
        null
      );
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
    /**
     * coeffBlur
     * 系数失去焦点时
     */
    coeffBlur() {
      var reg = new RegExp("[\\u4E00-\\u9FFF]+", "g");
      var regFu = /[`~!@#$%^&*()_\-+=<>?:"{}|,\/;'\\[\]~！@#￥%……&*（）——\-+={}|《》？：“”【】、；‘’，。、]/im;
      var regEn = new RegExp("[a-zA-z]+", "g");
      var num = 0;
      if (this.coeffNum != 1) {
        num = this.coeffNum.split(".").length - 1;
        if (num > 1) {
          this.errorFe = true;
        } else if (
          this.coeffNum[this.coeffNum.length - 1] == "." ||
          this.coeffNum[0] == "."
        ) {
          this.errorFe = true;
        } else if (reg.test(this.coeffNum)) {
          this.errorFe = true;
        } else if (regFu.test(this.coeffNum)) {
          this.errorFe = true;
        } else if (regEn.test(this.coeffNum)) {
          this.errorFe = true;
        } else {
          this.errorFe = false;
        }
        this.coefficient = this.coeffNum;
      }
      this.coeffShow = false;
    },
    /**
     * getFocus
     * 修改系数
     */
    getFocus() {
      this.coeffShow = true;
      setTimeout(() => {
        var elInput = document.getElementById("number");
        elInput.focus();
      }, 100);
    },
    /**
     * getCoeffNum
     * 选择系数
     */
    getCoeffNum() {
      if (this.errorFe || this.coefficient == "") {
        return;
      }
      if (this.coefficient == "系数") {
        this.coeffShow = true;
        setTimeout(() => {
          var elInput = document.getElementById("number");
          elInput.focus();
        }, 100);
      } else {
        this.getOperator(this.coefficient, false, 1);
      }
    },
    /**
     * frontBlur
     * 公式失去光标
     */
    frontBlur() {
      if (this.formulaForm.frontFormula == "") {
        this.error = false;
      }
    },
    /**
     * getOperator
     * 编辑运算符
     */
    getOperator(item, isPosition, isId) {
      var elInput = document.getElementById("Name");
      // this.setCaretPosition(elInput, this.formulaForm.frontFormula.length);
      if (this.formulaForm.frontFormula == "") {
        this.operatorLis = "";
        this.operatorData = [];
        if (isId == 2) return;
      }
      this.operatorData.push(this.formulaForm.frontFormula);
      this.insertInputTxt("Name", item, isPosition);
    },
    /**
     * Back
     * 回退
     */
    Back() {
      var elInput = document.getElementById("Name");
      if (this.operatorData.length == 0 && this.title == "编辑公式配置") {
        this.setCaretPosition(elInput, this.formulaForm.frontFormula.length);
        return;
      }
      if (this.operatorData.length != 1) {
        this.formulaForm.frontFormula = this.operatorData[
          this.operatorData.length - 1
        ];
        this.operatorLis = this.formulaForm.frontFormula;
        this.operatorData.splice(this.operatorData.length - 1, 1);
      } else {
        this.formulaForm.frontFormula = this.operatorData[
          this.operatorData.length - 1
        ];
        this.operatorLis = this.formulaForm.frontFormula;
        this.setCaretPosition(elInput, this.formulaForm.frontFormula.length);
        return;
      }
      this.setCaretPosition(elInput, this.formulaForm.frontFormula.length - 1);
    },
    /**
     * chainVal
     * 禁止公式键盘默认事件
     */
    chainVal(e) {
      var elInput = document.getElementById("Name");
      this.operatorLis = this.formulaForm.frontFormula;
      if (e.keyCode != 8) {
        e.preventDefault();
        elInput.value = elInput.value.replace(/[\x00-\xff]/gi, "");
        elInput.value = elInput.value.replace(
          /[`~!@#$%^&*()_\-+=<>?:"{}|,.\/;'\\[\]·~！@#￥%……&*（）——\-+={}|《》？：“”【】、；‘’，。、]/im,
          ""
        );
        elInput.value = elInput.value.replace(/[a-zA-z]/gi, "");
        e.preventDefault();
      } else {
        e.preventDefault();
      }
      elInput.value = this.operatorLis;
    },
    /**
     * frontFormulaVal
     * 禁止公式键盘默认事件
     */
    frontFormulaVal(e) {
      // if (e.keyCode != 8) {
      var elInput = document.getElementById("Name");
      elInput.value = elInput.value.replace(/[\x00-\xff]/gi, "");
      e.preventDefault();
      // return false;
      // } else {
      //   e.preventDefault();
      // }
    },
    /**
     * insertInputTxt
     * 在光标位置后添加
     */
    insertInputTxt(id, insertTxt, isPosition) {
      var elInput = document.getElementById(id);
      elInput.focus();
      var startPos = elInput.selectionStart;
      var endPos = elInput.selectionEnd;
      if (startPos === undefined || endPos === undefined) return;
      var txt = elInput.value;
      var result =
        txt.substring(0, startPos) + insertTxt + txt.substring(endPos);
      elInput.value = result;
      elInput.selectionStart = startPos + insertTxt.length;
      elInput.selectionEnd = startPos + insertTxt.length;
      this.formulaForm.frontFormula = elInput.value;
      if (isPosition) {
        this.setCaretPosition(elInput, startPos + insertTxt.length - 1);
      }
    },
    /**
     * setCaretPosition
     * 设置光标位置
     */
    setCaretPosition(textDom, pos) {
      if (textDom.setSelectionRange) {
        // IE Support
        textDom.focus();
        textDom.setSelectionRange(pos, pos);
      } else if (textDom.createTextRange) {
        // Firefox support
        var range = textDom.createTextRange();
        range.collapse(true);
        range.moveEnd("character", pos);
        range.moveStart("character", pos);
        range.select();
      }
    },
    /**
     * handelEditClick
     * 编辑公式配置
     */
    handelEditClick(row) {
      this.method = "put";
      this.postDataEdit = row;
      this.formulaForm.id = row.id;
      this.formulaForm.formulaName = row.formulaName;
      this.formulaForm.formula = row.formula;
      this.formulaForm.principalId = row.principalId;
      this.formulaForm.frontFormula = row.frontFormula;
      this.formulaForm.remark = row.remark;
      this.formulaForm._LogName = row._LogName;
      this.formulaVisible = true;
      this.title = "编辑公式配置";
      this.operatorData = [this.formulaForm.frontFormula];
    },
    /**
     * handelAddClick
     * 新增公式配置
     */
    handelAddClick() {
      this.method = "post";
      this.postDataEdit = null;
      this.formulaForm.formulaName = "";
      this.formulaForm.formula = "";
      this.formulaForm.frontFormula = "";
      this.formulaForm._LogName = "";
      this.formulaForm.remark = "";
      this.formulaForm.id = 0;
      this.title = "新增公式配置";
      this.formulaVisible = true;
    },
    /**
     * isIdFor
     * 转换
     */
    isIdFor() {
      // this.isId = this.isId.replace(/[0-9]{1,}/g, 1);
      this.isId = this.isId.replace(/\d{1,14}(\.\d{1,})?/g, 1);
      this.isId = this.isId.replace(/封度（宽幅）/g, 1);
      this.isId = this.isId.replace(/长/g, 1);
      this.isId = this.isId.replace(/宽/g, 1);
      this.isId = this.isId.replace(/件数/g, 1);
      this.isId = this.isId.replace(/损耗/g, 1);
      this.isId = this.isId.replace(/[*_\-+\/]/g, 2);
      this.isId = this.isId.replace(/[(]/g, 3);
      this.isId = this.isId.replace(/[)]/g, 4);
    },
    /**
     * handelSave
     * 保存公式配置
     */
    handelSave() {
      this.error = false;
      this.isId = "";
      this.$refs.formulaForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
        } else {
          this.formulaForm._LogName = this.formulaForm.formulaName;
          // this.$nextTick(()=>{
          this.isId = this.formulaForm.frontFormula;
          //  })
          this.isIdFor(); //转换成12
          var rules = /[^\x00-\xff]/;
          var egl = /[a-zA-z]/;
          if (rules.test(this.isId)) {
            this.error = true;
          }
          if (egl.test(this.isId)) {
            this.error = true;
          }
          if (this.isId.length == 1) {
            if (this.isId[0] != 1) {
              this.error = true;
              this.errorTex = "请输入正确的公式算法!";
              this.$message({
                message: "数据不合法，无法保存",
                type: "error"
              });
              return;
            }
          }
          if (this.isId.length < 3 && this.isId.length != 1) {
            this.error = true;
            this.errorTex = "请输入正确的公式算法!";
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
            return;
          }

          if (this.isId[this.isId.length - 1] == 2) {
            this.error = true;
            this.errorTex = "请输入正确的公式算法!";
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
            return;
          }
          if (this.isId[0] == 2) {
            this.error = true;
            this.errorTex = "请输入正确的公式算法!";
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
            return;
          }
          // 判断括号是否补全
          var moduleJson = {
            //括号匹配模板，你可以增加别的匹配，比如“{}”，“<>”，等，只需写入moduleJson里面去
            ")": "("
          };

          var testStr = this.formulaForm.frontFormula; //测试字符串
          var tempSaveArray = []; //用于存储字符串的数组
          for (var i = 0; i < testStr.length; i++) {
            //如果存在括号字符，就加入数组
            for (var key in moduleJson) {
              if (testStr[i] == key || testStr[i] == moduleJson[key]) {
                //如果字符串中存在json中的key 和value字符，就加入数组
                tempSaveArray.push(testStr[i]);
              }
            }
          }
          if (tempSaveArray.length) {
            if (tempSaveArray.length % 2 != 0) {
              //如果括号的长度为奇数，肯定不匹配
              this.error = true;
            } else {
              //如果字符串括号长度为偶数，就进行遍历数组，进行判断 12345  0 4

              for (var j = 0; j < tempSaveArray.length; j++) {
                //（（（（））））
                if (moduleJson[tempSaveArray[j]]) {
                  //如果是右括号，就和前一个进行匹配。
                  //拿到数组前一位的字符，是否与自己匹配
                  if (j > 0) {
                    if (moduleJson[tempSaveArray[j]] == tempSaveArray[j - 1]) {
                      //说明两个括号进行了匹配，让其出栈
                      tempSaveArray.splice(j - 1, 2);
                      j = 0; //从新遍历数组
                    }
                  }
                }
              }

              if (tempSaveArray.length) {
                //没有移除完毕
                this.error = true;
              }
            }
          }
          //else {
          //   // console.log("你输入的字符串不存在括号");
          // }

          // 判断值与值，值与符号
          var isFor = true;
          // var isIndex = true;
          for (var i = 0; i < this.isId.length; i++) {
            // ( 的前面只能是符号和[
            if (i != 0 && this.isId[i] == 3) {
              // isFor = true;
              if (this.isId[i - 1] == 1) {
                this.error = true;
              }
              if (this.isId[i - 1] == 2) {
                if (this.isId[i - 2] != 1) {
                  this.error = true;
                }
              }
              if (this.isId[i + 1] == 2) {
                this.error = true;
              }
              if (this.isId[i + 2] == 4) {
                this.error = true;
              }
            }
            if (i == 0 && this.isId[i] == 3) {
              // isFor = false;
              if (this.isId[i + 1] == 2) {
                this.error = true;
              }
              if (this.isId[i + 2] == 4) {
                this.error = true;
              }
            }
            // 如果是（（要判断）有值）
            // if(this.isId[i] == 3&&this.isId[i+1] == 3) {
            //   isIndex = false;
            // }
            // ) 的前面只能是值和[,后面只能是符号和],后面可能没有
            if (this.isId[i] == 4) {
              // if(!isIndex) {
              //   if(this.isId[i + 1] != 2) {
              //     this.error = true;
              //   }
              // }
              if (i == 0) {
                this.error = true;
              }
              if (this.isId[i - 1] == 2) {
                this.error = true;
              }
              // if (!isFor) {
              //   // this.error = true;
              //   if (i + 1 == this.isId.length) {
              //     this.error = true;
              //   }
              // }
              if (this.isId[i + 1] == 3) {
                this.error = true;
              }
              if (this.isId[i - 1] == 3) {
                this.error = true;
              }
              // 如果）不是最后要判断他是不是符号
              if (i + 1 != this.isId.length) {
                if (this.isId[i + 1] == 1) {
                  this.error = true;
                }
                if (this.isId[i + 1] == 2) {
                  if (this.isId[i + 2] != 1) {
                    this.error = true;
                  }
                }
              }
            }

            // 判断前后是否一样，一样公式不正确
            if (this.isId[i] == this.isId[i + 1]) {
              if (this.isId[i] != 3 && this.isId[i] != 4) {
                this.error = true;
              }
            }
          }
          if (this.error) {
            this.errorTex = "请输入正确的公式算法!";
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
            return;
          }
          this.formulaForm.formula = this.formulaForm.frontFormula;
          this.formulaForm.formula = this.formulaForm.formula.replace(
            /封度（宽幅）/g,
            "【WideValue】"
          );
          this.formulaForm.formula = this.formulaForm.formula.replace(
            /长/g,
            "【LengthValue】"
          );
          this.formulaForm.formula = this.formulaForm.formula.replace(
            /宽/g,
            "【WidthValue】"
          );
          this.formulaForm.formula = this.formulaForm.formula.replace(
            /件数/g,
            "【NumValue】"
          );
          this.formulaForm.formula = this.formulaForm.formula.replace(
            /损耗/g,
            "【LossValue】"
          );
          if (this.formulaForm.formula.length > 100) {
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
            this.error = true;
            this.errorTex = "公式过长,请重新输入!";
            return;
          }
          // this.loading = true;
          this.saveBtn = true;
          var data = RequestObject.GetObject(
            this.formulaForm,
            null,
            null,
            this.postDataEdit
          );
          request({
            url: "/manufacturing/api/TMMFormula",
            method: this.method,
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                var setTime = setTimeout(() => {
                  // this.loading = false;
                  this.$confirm(res.info, "错误信息", {
                    confirmButtonText: "确定",
                    type: "error",
                    showCancelButton: false
                  });
                  this.saveBtn = false;
                  clearTimeout(setTime);
                }, 50);
              } else {
                var setTime = setTimeout(() => {
                  this.formulaVisible = false;
                  this.getFormulaData();
                  this.$message({
                    message: "操作成功",
                    type: "success"
                  });
                  clearTimeout(setTime);
                }, 50);
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
            _LogName: this.allSelectionChange[i].formulaName
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
      this.$confirm("是否删除?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.loading = true;
          request({
            url: "/manufacturing/api/TMMFormula",
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
                this.getFormulaData();
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
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    },
    /**
     * handelDelete
     * 删除公式配置
     */
    handelDelete(row) {
      this.showtips = false;
      var data = {
        id: row.id,
        _LogName: row.formulaName
      };
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        data,
        null
      );
      this.$confirm("是否删除" + row.formulaName + "?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.showtips = true;
          this.loading = true;
          request({
            url: "/manufacturing/api/TMMFormula",
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
                this.getFormulaData();
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
      this.getFormulaData();
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
    display: flex;
    border-bottom: 1px solid #eee;
    margin-bottom: 20px;
    .el-form-item {
      // flex: 1;
      margin-right: 20px;
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
  padding: 0px 10px;
}
.num {
  // width: 88px;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
.formulaBox {
  display: flex;
  padding: 0px 10px;
  .formulaLis /deep/ {
    border: 1px solid #dcdfe6;
    width: 160px;
    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
      -webkit-appearance: none;
    }
    input[type="number"] {
      -moz-appearance: textfield;
    }
    a {
      display: block;
      text-align: center;
      height: 30px;
      line-height: 30px;
      border-bottom: 1px solid #dcdfe6;
    }
  }
  .el-form {
    flex: 1;
    .operator {
      display: flex;
      a {
        border: 1px solid #dcdfe6;
        display: block;
        width: 60px;
        text-align: center;
        height: 30px;
        line-height: 30px;
        margin-left: 20px;
        margin-bottom: 20px;
      }
    }
  }
}
/deep/ .el-dialog__body {
  // padding-top: 10px;
  padding: 10px 20px;
  .contactNumber .el-input__inner {
    border: 1px solid #ff4949;
  }
  .contactNumber > span {
    color: #ff4949;
    font-size: 12px;
  }
}

</style>
