<template>
  <el-container id="MainBill" class="Wsty" v-on:click.native="listenClick">
    <!-- 右边列表框开始 -->
    <!-- {{packageId}} -->
    <div style="width:100%;">
      <!-- table开始 -->
      <el-table
        id="tableData"
        ref="table"
        style="width:99.9%"
        :height="tableHv"
        :data="tableData"
        row-key="id"
        border
        :show-summary="true"
        :summary-method="getSummaries"
        :row-class-name="tableRowClassName"
      >
        <el-table-column label="序号" width="70">
          <template slot="header">
            <span class="tableHeader">
              <span>
                序号
                <i class="el-icon-setting" @click="CopyOperate"></i>
              </span>
            </span>
          </template>
          <template slot-scope="scope">
            <div class="textAlign">
              <span @click.stop="menuBar($event,scope.$index)">
                {{scope.$index+1}}
                <!-- {{scope.row.itemId}} -->
                <i class="el-icon-setting"></i>
              </span>
            </div>
          </template>
        </el-table-column>

        <el-table-column prop="itemName" label="配色项目" width="150">
          <template slot="header">
            <span class="tableHeader">
              <span class="start">*</span>
              <span>配色项目</span>
            </span>
          </template>
          <template slot-scope="scope">
            <!-- <EditInput
              v-model="scope.row.itemName"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              type="itemName"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>-->
            <EditInput
              autocomplete
              v-model="scope.row.itemName"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              :data.sync="packageColorList"
              :isShowFlag="isShowFlag"
              :itemList="scope.row"
              :clearable="true"
              type="partName"
              :filterable="true"
              label="name"
              :filterMethod="filterMethod"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="materialCode" label="物料名称" width="150">
          <template slot="header">
            <span class="tableHeader">
              <span class="start">*</span>
              <span>物料名称</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              v-model="scope.row.materialCode"
              @clickEvent="clickEvent"
              @clickEventAfter="clickEventAfter"
              @blurCheck="blurCheck"
              :type="1"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="materialName" label="部位名称" width="150">
          <template slot="header">
            <span class="tableHeader">
              <span class="start"></span>
              <span>部位名称</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              autocomplete
              v-model="scope.row.materialName"
              @clickEvent="clickEvent"
              :data.sync="PartData"
              :isShowFlag="isShowFlag"
              :clearable="true"
              type="partName"
              :filterable="true"
              label="dicValue"
              :itemList="scope.row"
            ></EditInput>
            <!-- <EditInput
              v-model="scope.row.materialName"
              @clickEvent="clickEvent"
              :data.sync="PartData"
              :isShowFlag="isShowFlag"
              :clearable="true"
              type="partName"
              :filterable="true"
              label="dicValue"
              :filterMethod="filterMethod"
              @change="partNameChange"
              @keyup.enter.native="setPartName(scope.row)"
            ></EditInput>-->
          </template>
        </el-table-column>

        <el-table-column prop="lengthValue" label="长" width="90">
          <template slot="header">
            <span class="tableHeader">
              <!-- <span class="start">*</span> -->
              <span>长</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              v-model="scope.row.lengthValue"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              :type="2"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="widthValue" label="宽" width="90">
          <template slot="header">
            <span class="tableHeader">
              <!-- <span class="start">*</span> -->
              <span>宽</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              v-model="scope.row.widthValue"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              :type="2"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="numValue" label="件数" width="90">
          <template slot="header">
            <span class="tableHeader">
              <span class="start">*</span>
              <span>件数</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              v-model="scope.row.numValue"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              :type="2"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="wideValue" label="封度（宽幅）" width="100">
          <template slot="header">
            <span class="tableHeader">
              <!-- <span class="start">*</span> -->
              <span>封度（宽幅）</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              v-model="scope.row.wideValue"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              :type="2"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="lossValue" label="损耗" width="90">
          <template slot="header">
            <span class="tableHeader">
              <!-- <span class="start">*</span> -->
              <span>损耗</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditInput
              v-model="scope.row.lossValue"
              @clickEvent="clickEvent"
              @blurCheck="blurCheck"
              :type="2"
              :itemList="scope.row"
              :isShowFlag="isShowFlag"
            ></EditInput>
          </template>
        </el-table-column>

        <el-table-column prop="singleValue" label="单用量" width="120">
          <template slot="header">
            <span class="tableHeader">
              <span class="start">*</span>
              <span>单用量</span>
            </span>
          </template>
          <template slot-scope="scope">
            <div :class="{validStyle:scope.row.singleValue.valid}">
              <div class="tableTd">{{scope.row.singleValue.value}}</div>
            </div>
          </template>
        </el-table-column>

        <el-table-column prop="formula" label="单用量计算公式" width="150">
          <template slot="header">
            <span class="tableHeader">
              <span class="start">*</span>
              <span>单用量计算公式</span>
            </span>
          </template>
          <template slot-scope="scope">
            <EditSelect
              v-model="scope.row.formula"
              @clickEvent="clickEvent"
              :data="formulaData"
              :isShowFlag="isShowFlag"
              label="formulaName"
              :filterable="true"
              @change="formulaChange(scope.row)"
            ></EditSelect>
          </template>
        </el-table-column>

        <el-table-column prop="formulaNameZw" label="公式" width="400">
          <template slot="header">
            <span class="tableHeader">
              <span>公式</span>
            </span>
          </template>
          <template slot-scope="scope">
            <div :class="{validStyle:scope.row.formulaNameZw.valid}">
              <div class="tableTd">{{scope.row.formulaNameZw.value}}</div>
            </div>
          </template>
        </el-table-column>
      </el-table>
      <!-- table结束 -->
    </div>
    <!-- 右边列表框结束 -->
    <CopyOperate ref="CopyOperate" title="复制列选择框" @copyOpt="doCopyOpt"></CopyOperate>

    <div class="findBox" :style="popoverStyle" v-show="popoverState" v-on:click.stop="1==1">
      <el-table :data="materielData" :height="250" @row-click="rowClick">
        <el-table-column prop="materialCode" label="物料代码" width="180" />
        <el-table-column prop="materialName" label="物料名称" width="100" />
        <el-table-column prop="spec" label="规格型号" />
        <el-table-column prop="baseUnitName" label="基本计量单位" />
        <el-table-column prop="materialTypeName" label="物料分类" />
        <el-table-column prop="colorName" label="颜色" />
        <el-table-column prop="defaultWarehouseName" label="默认仓库" />
        <el-table-column prop="shelfLife" label="保质期（天）" width="100" />
      </el-table>
      <Pagination
        :pageIndex="pageIndex"
        :totalCount="totalCount"
        :pageSize="pageSize"
        @pagination="pagination"
      />
    </div>

    <div class="findBox" :style="menuStyle" v-show="menuState">
      <ul class="menuUl">
        <li @click="doMenuAdd">增加行</li>
        <li @click="doMenuDel">删除行</li>
        <li @click="copyRow">复制行</li>
      </ul>
    </div>
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
  getFloat,
  isEmpty,
  trim
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from "./components/InboundOrder";
// import MainBill from "./components/MainBill";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import { debuglog } from "util";

import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";
import CopyOperate from "./CopyOperate";

export default {
  data() {
    return {
      popoverStyle: {
        left: "500px",
        top: "400px",
        width: "800px",
        height: "320px"
      },
      menuStyle: {
        left: "500px",
        top: "400px",
        width: "80px",
        height: "auto",
        paddiing: "4px"
      },
      menuState: false,
      delNum: -1,
      fullscreenLoading: false,
      tableHeight: 500,
      tableHeight2: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: {},
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], //物料信息
      warehouseData: [], //仓库信息
      tableData2: [
        {
          id: newGuid(),
          itemId: "",
          editState: false,
          rowIndex: 0,
          itemName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          materialCode: {
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false,
            oldValue: ""
          },
          materialName: {
            //部位
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          lengthValue: {
            //长
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          widthValue: {
            //宽
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          numValue: {
            //件数
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          wideValue: {
            //封度
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          lossValue: {
            //损耗
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          singleValue: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          formula: {
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          formulaName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          formulaNameZw: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          }
        }
      ], // table数据模型
      tableData: [],
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      btnAip: "", //按钮权限
      OtherData: {},
      realNameOptions: [], //收货员列表
      addControl: true, //是否显示新增按钮
      TabArr: [
        "itemName",
        "materialCode",
        "materialName",
        "lengthValue",
        "widthValue",
        "numValue",
        "wideValue",
        "lossValue",
        "formula"
      ],
      formulaArr: {
        "【WideValue】": "", //封度宽幅
        "【LengthValue】": "", //长
        "【WidthValue】": "", //宽
        "【NumValue】": "", //件数
        "【LossValue】": "" //损耗
      },
      materialItem: {},
      // firstState: true,
      TabBom: [],
      isShowFlag: true,
      editFlag: false,
      colItemId: -1,
      firstFlag: true,
      PartNameSp: "",
      copyList: [], //复制行数组
      copyNum: -1, //复制的行数
      colorList: [],
      packageColorList: []
    };
  },
  props: {
    tableHv: {
      type: Number,
      default: 400
    },
    treeItem: {},
    PartData: {
      //部位信息
      type: Array
    },
    // showItem: {
    //   type: Number,
    //   required: true
    // },
    // editableTabs: {},
    // packageId: {}, //配色项目
    formulaData: {} //计算公式
    // bomData: {} //Bom信息
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
    EditInput,
    EditSelect,
    CopyOperate
    // MainBill,
    // InboundOrder,
  },
  watch: {
    selectRow(val) {
      if (val.id) {
        this.addControl = false;
      } else {
        this.addControl = true;
      }
    }
  },
  created() {
    //获取按钮权限
    this.fullscreenLoading = true;
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.isShowFlag = false;
  },
  async mounted() {
    window.addEventListener("keydown", this.onKeydown);
    this.tableData = this.setTable(this.tableLen); //添加30行
    var copyList = Storage.LStorage.get("copyList");
    if (!isEmpty(copyList)) {
      this.copyList = copyList;
    }
    this.colorList = await this.getColorItem();
    this.fullscreenLoading = false;
  },
  activated() {
    window.addEventListener("keydown", this.onKeydown);
  },
  beforeDestroy() {
    window.removeEventListener("keydown", this.onKeydown);
  },
  deactivated() {
    this.listenClick();
    window.removeEventListener("keydown", this.onKeydown);
  },
  methods: {
    getColorItem() {
      return new Promise((resolve, reject) => {
        request({
          url:
            "/manufacturing/api/TMMColorSolutionMainNew/GetTMMPackageColorItem",
          method: "get",
          params: {}
        }).then(res => {
          resolve(res.data);
        });
      });
    },
    blurCheck(itemList, item, type, state) {
      //td框的类型（可用于数据校验等）
      switch (type) {
        case 1: //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.getMaterielData(itemList);
          }
          if (state == 1) {
            if (itemList["materialCode"].isShow == true) {
              var id_ = itemList["materialCode"].id;
              document.getElementById(id_).focus();
            } else {
              if (isEmpty(itemList.materialCode.value)) {
                itemList.materialCode.value = "";
                itemList.materialCode.oldValue = "";
              } else {
                itemList.materialCode.value = itemList.materialCode.oldValue;
              }
            }
          }
          break;
        case 2: //验证长、宽、件数、损耗
          this.NumBlur(item, itemList); //验证长、宽、件数、损耗
          break;
        case 3:
          this.WideValueBlur(item, itemList); //封度（宽幅）
          break;
        case "itemName":
          this.$emit("setEditState", true);
          if (!isEmpty(item.value) && item.value.length > 50) {
            item.value = "";
          }
          break;
        default:
          null;
      }
    },
    clickEventAfter(itemList, item, type) {
      //显示input框之后（创建选择物料代码框）
      this.doItem = itemList;
      if (type === 1) {
        this.findBox(item);
        this.getMaterielData(itemList);
      }
      if (type == "partName") {
        this.PartNameSp == "";
      }
    },
    clickEvent() {
      this.editFlag = true;
      this.defaultAll();
    },
    filterMethod(val) {
      this.PartNameSp = val;
      var data2 = this.PartData.filter(item => {
        if (`${item["dicValue"]}`.indexOf(trim(val)) !== -1) {
          return item;
        }
      });
      return data2;
    },
    //设置Tab切换开始
    onKeydown(event) {
      if (event.keyCode !== 9) return;
      var data = this.findCheck(event);
      this.setCheck(data);
    },
    findCheck(event) {
      var data = {};
      for (var h = 0; h < this.tableData.length; h++) {
        for (var i in this.tableData[h]) {
          if (typeof this.tableData[h][i] == "object") {
            if (this.tableData[h][i]["isShow"] === true) {
              for (var k = 0; k < this.TabArr.length; k++) {
                if (this.TabArr[k] === i) {
                  event.preventDefault();
                  var l = h;
                  if (k + 1 > this.TabArr.length - 1) {
                    var go = 0;
                    // console.log("-");
                    if (l >= this.tableData.length) {
                      l = this.tableData.length;
                    } else {
                      l = l + 1;
                    }
                  } else {
                    // console.log("+");
                    var go = k + 1;
                  }
                  // console.log(go);
                  var set = this.TabArr[go];
                  data.index = l;
                  data.name = set;
                  data.item = this.tableData[l];
                  // console.log(data);
                  return data;
                }
              }
            }
          }
        }
      }
    },
    setCheck(data) {
      if (data == undefined) return;
      var { index, name, item } = data;
      this.defaultAll();
      for (var k in this.tableData[index]) {
        if (k == name) {
          this.$set(this.tableData[index][name], `isShow`, true);
          this.$nextTick(() => {
            var id = this.tableData[index][name]["id"];
            document.getElementById(id).focus();
            document.getElementById(id).select();
            if (
              document.getElementById(id).getAttribute("readonly") == "readonly"
            ) {
              document.getElementById(id).click();
            }
          });
        }
      }
    },
    defaultAll() {
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
    },
    resetTable() {
      this.isShowFlag = true;
      this.tableData = [];
      var list = this.setTable(this.tableLen);
      list.map(item => {
        this.tableData.push(item);
      });
      this.$emit("closeLoading");
    },
    //设置Tab切换结束
    resetData() {
      //初始化数据
      this.resetDeafultData();
    },
    resetDeafultData() {
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = [];
      // this.setTable(30);
      this.tableData = this.setTable(this.tableLen); //添加30行
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.WarehousingType = "";
      this.dtData.warehousingDate = new Date();
      this.dtData.warehousingOrder = "";
      this.dtData.remark = "";
      this.dtData.operatorName = this.$store.state.user.name;
      this.dtData.operatorId = "";
      this.dtData.receiptName = "";
      this.dtData.receiptId = "";
      this.dtData.auditName = "";
      this.dtData.auditId = "";
      this.dtData.auditStatus = -1;
      this.dtData.auditTime = "";
      this.fullscreenLoading = false;
    },
    menuBar(event, num) {
      var el = closest(event.target, ".textAlign");
      var MT = el.getBoundingClientRect().top;
      var ML = el.getBoundingClientRect().left;
      var xh = 70; //序号宽度
      if (this.$store.getters.sidebar.opened) {
        var ol = 210 - xh;
      } else {
        var ol = 54 - xh;
      }
      this.menuStyle.top = MT - 84 - 10 + "px";
      this.menuStyle.left = ML - ol + "px";
      this.listenClick();
      this.delNum = num;
      this.copyNum = num;
      this.menuState = true;
    },
    doMenuAdd() {
      this.$emit("setEditState", true);
      this.menuState = false;
      this.tableData = [...this.tableData, ...this.setTable(1)];
    },
    doMenuDel() {
      this.$emit("setEditState", true);
      this.menuState = false;
      if (this.delNum != -1) {
        this.tableData.splice(this.delNum, 1);
        this.delNum = -1;
      }
    },
    copyRow() {
      this.$prompt("请输入复制的条数", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        closeOnClickModal: false,
        inputPattern: /^\d{1,3}$/,
        inputErrorMessage: "请输一到三位的正整数"
      })
        .then(async ({ value }) => {
          this.$emit("setEditState", true);
          this.copyList.map(item => {
            if (item == "formula") {
              this.copyList.push("formulaName");
              this.copyList.push("formulaNameZw");
            }
          });
          var copyDt = this.tableData[this.copyNum];
          var addDt = this.setTable(value);
          addDt.map(item => {
            var ky = Object.keys(item);
            ky.map(k => {
              this.copyList.map(val => {
                if (k == val) {
                  // item[k] = deepClone(copyDt[k]);
                  item[k] = JSON.parse(JSON.stringify(copyDt[k]));
                }
              });
            });
          });
          var checkSelect = await this.checkSelect(this.tableData);
          var addNum = checkSelect.length;
          for (var i = 0; i < checkSelect.length; i++) {
            if (checkSelect[i].editState) {
              addNum = i;
            }
          }

          this.tableData.splice(addNum + 1, 0, ...addDt);

          this.$message({
            type: "success",
            message: "复制成功"
          });
        })
        .catch(() => {});
    },
    tableRowClassName({ row, rowIndex }) {
      if (row.rowIndex === 1) {
        return "color1";
      }
    },
    WarehouseChange(item) {
      //部位
      this.$emit("setEditState", true);
      this.PartData.map(data => {
        if (item.key == data.id) {
          item.value = data.dicValue;
        }
      });
      item.valid = false;
    },
    formulaChange(item) {
      //计算公式
      this.$emit("setEditState", true);
      this.formulaData.map(data => {
        if (item.formula.key == data.id) {
          item.formula.value = data.formulaName;
          item.formulaName.value = data.formula;
          item.formulaNameZw.value = data.frontFormula;
        }
      });
      this.setListFormula(item);
      item.valid = false;
    },
    NumBlur(item, data) {
      this.$emit("setEditState", true);
      if (isRealNum(item.value) === false) {
        item.value = "";
        data.singleValue.value = "";
        return;
      }
      if (item.value <= 0) {
        item.value = "";
        data.singleValue.value = "";
        return;
      }
      var num = item.value;
      // var result = num.toString().indexOf(".");
      // if (result != -1) {
      item.value = getFloat(item.value, 4);
      // }
      this.setListFormula(data);
    },
    WideValueBlur(item, data) {
      this.$emit("setEditState", true);
      if (isRealNum(item.value) === false) {
        item.value = "";
        this.setListFormula(data);
        return;
      }
      if (item.value <= 0) {
        item.value = "";
        this.setListFormula(data);
        return;
      }
      item.value = getFloat(item.value, 4);
      this.setListFormula(data);
    },
    setPartName(itemList) {
      this.$emit("setEditState", true);
      if (this.PartNameSp == "") {
        setTimeout(() => {
          this.selectInput(itemList, "lengthValue");
        }, 50);
        return;
      }
      var id_ = itemList["materialName"].id;
      itemList.materialName.value = this.PartNameSp;
      itemList.materialName.key = this.PartNameSp;
      setTimeout(() => {
        this.selectInput(itemList, "lengthValue");
      }, 50);
      var box = document.getElementsByClassName("el-select-dropdown");
      for (var sel in box) {
        if (box[sel].getAttribute && box[sel].getAttribute("x-placement")) {
          box[sel].style.display = "none";
        }
      }
    },
    partNameChange() {
      //重置输入的历史
      this.$emit("setEditState", true);
      this.PartNameSp = "";
    },
    selectInput(doItem, name) {
      this.listenClick();
      this.$set(doItem[name], `isShow`, true);
      this.$nextTick(() => {
        var id_ = doItem[name].id;
        document.getElementById(id_).focus();
        document.getElementById(id_).select();
        if (
          document.getElementById(id_).getAttribute("readonly") == "readonly"
        ) {
          document.getElementById(id_).click();
        }
      });
    },
    setListFormula(data) {
      data.singleValue.value = "";
      if (data.formula.value == "") {
        //计算公式不能为空
        return;
      }
      data.singleValue.valid = false;
      data.lengthValue.valid = false;
      data.widthValue.valid = false;
      data.wideValue.valid = false;
      data.lossValue.valid = false;

      var errMsg = "【error】";
      var lengthValue = `${errMsg}-【LengthValue】`; //长
      var widthValue = `${errMsg}-【WidthValue`; //宽
      var numValue = `${errMsg}-【NumValue`; //件数
      var wideValue = `${errMsg}-【WideValue`; //封度
      var lossValue = `${errMsg}-【LossValue`; //损耗
      if (!isEmpty(data.lengthValue.value)) {
        lengthValue = data.lengthValue.value;
      }
      if (!isEmpty(data.widthValue.value)) {
        widthValue = data.widthValue.value;
      }
      if (!isEmpty(data.numValue.value)) {
        numValue = data.numValue.value;
      }
      if (!isEmpty(data.wideValue.value)) {
        wideValue = data.wideValue.value;
      }
      if (!isEmpty(data.lossValue.value)) {
        lossValue = data.lossValue.value;
      }
      // console.log(data.formulaName)
      var formula = data.formulaName.value;
      var formulaRp = formula
        .replace(/\【WideValue\】/g, wideValue)
        .replace(/\【LengthValue\】/g, lengthValue)
        .replace(/\【WidthValue\】/g, widthValue)
        .replace(/\【NumValue\】/g, numValue)
        .replace(/\【LossValue\】/g, lossValue);

      if (formulaRp == "") {
        data.singleValue.value = "";
      } else {
        if (formulaRp.indexOf("【LengthValue】") != -1) {
          data.lengthValue.valid = true;
        }
        if (formulaRp.indexOf("【WidthValue") != -1) {
          data.widthValue.valid = true;
        }
        if (formulaRp.indexOf("【WideValue") != -1) {
          data.wideValue.valid = true;
        }
        if (formulaRp.indexOf("【LossValue") != -1) {
          data.lossValue.valid = true;
        }

        if (formulaRp.indexOf(errMsg) == -1) {
          data.singleValue.value = getFloat(eval(formulaRp), 4);
        } else {
          data.singleValue.value = "";
        }
      }
    },
    readOldColData(itemId) {
      //写出数据
      this.tableData = [];
      this.colItemId = itemId;
      this.editableTabs.map(item => {
        if (item.itemId == itemId) {
          //第一次进入时
          if (item.editState == false) {
            if (item.childList.length < this.tableLen) {
              this.tableData = [
                ...item.childList,
                ...this.setTable(this.tableLen - item.childList.length)
              ];
            } else {
              this.tableData = [...item.childList, ...this.setTable(1)];
            }
            // this.tableData = [...item.childList, ...this.setTable(1)];
            item.editState = true;
            setTimeout(() => {
              this.$emit("closeLoading", 2);
            }, 200);
          } else {
            this.tableData = [...item.childList];
            setTimeout(() => {
              this.$emit("closeLoading", 2);
            }, 200);
          }
        }
      });
    },
    setOldColData() {
      this.editableTabs.map(item => {
        if (item.itemId == this.colItemId) {
          item.childList = this.tableData;
        }
      });
    },
    setBomData(data) {
      this.isShowFlag = true;
      this.listenClick();
      this.formatBom(data);
      this.packageColorList = [];
      this.colorList.map(item => {
        if (item.id == this.treeItem.id) {
          this.packageColorList = item.children;
        }
      });
      // console.log(this.packageColorList)
      // packageColorList
      // if (this.firstState == false) {
      //   return;
      // }
      // this.listenClick();
      // this.isShowFlag = true;
      // if (this.firstFlag) {
      //   this.firstFlag = false;
      //   if (this.bomData.length == 0) {
      //     this.colItemId = itemId;
      //     this.tableData = [...this.setTable(this.tableLen)];
      //     this.$emit("closeLoading");
      //   } else {
      //     this.formatBom(this.bomData, itemId);
      //   }
      // } else {
      //   this.editableTabs.map(item => {
      //     if (item.itemId == this.colItemId) {
      //       item.childList = this.tableData;
      //     }
      //   });
      //   this.readOldColData(itemId);
      // }
    },
    formatBom(dt) {
      //插入BOM数据
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          itemId: item.itemId,
          editState: false,
          rowIndex: 0,
          itemName: {
            id: newGuid(),
            value: item.itemName,
            isShow: false,
            valid: false
          },
          materialCode: {
            id: newGuid(),
            value: item.materialName,
            key: "",
            isShow: false,
            valid: false,
            oldValue: item.materialName
          },
          materialName: {
            //部位
            id: newGuid(),
            value: item.partName,
            key: item.partId,
            isShow: false,
            valid: false
          },
          lengthValue: {
            id: newGuid(),
            value: item.lengthValue,
            isShow: false,
            valid: false
          },
          widthValue: {
            id: newGuid(),
            value: item.widthValue,
            isShow: false,
            valid: false
          },
          numValue: {
            id: newGuid(),
            value: item.numValue,
            isShow: false,
            valid: false
          },
          wideValue: {
            id: newGuid(),
            value: item.wideValue,
            isShow: false,
            valid: false
          },
          lossValue: {
            id: newGuid(),
            value: item.lossValue,
            isShow: false,
            valid: false
          },
          singleValue: {
            id: newGuid(),
            value: item.singleValue,
            isShow: false,
            valid: false
          },
          formula: {
            id: newGuid(),
            value: item.formulaName,
            key: parseFloat(item.formula) || "",
            isShow: false,
            valid: false
          },
          formulaName: {
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          formulaNameZw: {
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          }
        };

        this.formulaData.map(data => {
          if (list.formula.key == data.id) {
            // list.formula.value = data.formulaName;
            list.formulaName.value = data.formula;
            list.formulaNameZw.value = data.frontFormula;
          }
        });
        listArr.push(list);
      });

      setTimeout(() => {
        if (listArr.length < this.tableLen) {
          this.tableData = [
            ...listArr,
            ...this.setTable(this.tableLen - listArr.length)
          ];
        } else {
          this.tableData = [...listArr, ...this.setTable(1)];
        }

        this.$emit("closeLoading");
      }, 0);
      // this.tableData = [...listArr, ...this.setTable(1)];
      // console.log(listArr)
      // this.editableTabs.map(item => {
      //   item.childList = [];
      //   (item.editState = false),
      //     listArr.map(val => {
      //       if (val.itemId == item.itemId) {
      //         item.childList.push(val);
      //       }
      //     });
      // });
      // this.setItemTable(itemId);
      // setTimeout(() => {
      //   this.tableData = [...listArr, ...this.setTable(1)];
      //   this.$emit("closeLoading");
      // }, 0);
    },
    setItemTable(itemId) {
      this.tableData = [];
      this.colItemId = itemId;
      this.editableTabs.map(item => {
        if (item.itemId == itemId) {
          setTimeout(() => {
            if (item.childList.length < this.tableLen) {
              this.tableData = [
                ...item.childList,
                ...this.setTable(this.tableLen - item.childList.length)
              ];
            } else {
              this.tableData = [...item.childList, ...this.setTable(1)];
            }
            item.editState = true;
            this.$emit("closeLoading");
          }, 0);
        }
      });
    },
    async setFormula(id) {
      var sel = {};
      this.formulaData.map(item => {
        if (item.id == id) {
          sel = item;
        }
      });

      // list.formula.value = data.formulaName;
      // list.formulaName.value = data.formula;
      // list.formulaNameZw.value = data.frontFormula;
      var checkSelect = await this.checkSelect(this.tableData);
      checkSelect.map(item => {
        if (item.editState) {
          item.formula.value = sel.formulaName;
          item.formula.key = sel.id;
          item.formulaName.value = sel.formula;
          item.formulaNameZw.value = sel.frontFormula;
          this.setListFormula(item);
        }
      });
    },
    checkSelect(dt) {
      //查找输入数据项
      return new Promise((resolve, reject) => {
        dt.map(item => {
          for (var i in item) {
            item.editState = false;
            if (typeof item[i] == "object") {
              if (item[i].value != "") {
                item.editState = true;
                break;
              }
            }
          }
        });
        resolve(dt);
      });
    },
    async addPutInStorage(state) {
      //处理数据
      var flag = true;
      var flagArr = [];
      var singleFlag = true;
      var singleFlagArr = [];
      // this.setOldColData(this.colItemId); //保存当前数据
      // var allChildList = [];
      // this.editableTabs.map(item => {
      //   allChildList = [...allChildList, ...item.childList];
      // });
      // console.log(this.editableTabs)
      // console.log(allChildList)

      var checkSelect = await this.checkSelect(this.tableData);
      var childList = [];
      checkSelect.map(item => {
        //验证数据
        flag = true;
        singleFlag = true;
        item.itemName.valid = false;
        item.materialCode.valid = false;
        item.materialName.valid = false;
        item.lengthValue.valid = false;
        item.widthValue.valid = false;
        item.numValue.valid = false;
        item.wideValue.valid = false;
        item.lossValue.valid = false;
        item.singleValue.valid = false;
        item.formula.valid = false;

        if (item.editState) {
          if (isEmpty(item.itemName.value)) {
            //验证物料代码
            item.itemName.valid = true;
            flag = false;
          }
          if (isEmpty(item.materialCode.value)) {
            //验证物料代码
            item.materialCode.valid = true;
            flag = false;
          }
          // if (item.materialName.key === "") {
          //   //验证部位ID
          //   item.materialName.valid = true;
          //   flag = false;
          // }
          // if (item.lengthValue.value === "") {
          //   //验证长
          //   item.lengthValue.valid = true;
          //   flag = false;
          // }
          // if (item.widthValue.value === "") {
          //   //验证宽
          //   item.widthValue.valid = true;
          //   flag = false;
          // }
          if (item.numValue.value === "") {
            //验证件数
            item.numValue.valid = true;
            flag = false;
          }
          // if (item.wideValue.value === "") {
          //   //验证封度
          //   item.wideValue.valid = true;
          //   flag = false;
          // }
          // if (item.lossValue.value === "") {
          //   //验证损耗
          //   item.lossValue.valid = true;
          //   flag = false;
          // }
          if (item.singleValue.value === "" || item.singleValue.value <= 0) {
            //验证单用量
            item.singleValue.valid = true;
            flag = false;
          }
          //验证单用量格式
          if (isRealNum(item.singleValue.value) === false) {
            item.singleValue.valid = true;
            singleFlag = false;
          }
          if (item.singleValue.value <= 0) {
            item.singleValue.valid = true;
            singleFlag = false;
          }

          if (item.formula.key === "") {
            //验证单用量公式
            item.formula.valid = true;
            flag = false;
          }
          // console.log(this.packageId)
          var _LogName = "";
          var param = {
            id: 0,
            mainId: 0,
            // itemId: item.itemId,
            materialName: item.materialCode.value,
            itemName: item.itemName.value,
            // partId: item.materialName.key,
            // lengthValue: item.lengthValue.value,
            // widthValue: item.widthValue.value,
            numValue: item.numValue.value,
            // wideValue: item.wideValue.value,
            // lossValue: item.lossValue.value,
            singleValue: item.singleValue.value,
            formula: parseFloat(item.formula.key) || -1
          };
          if (!isEmpty(item.lengthValue.value)) {
            //长
            param.lengthValue = item.lengthValue.value;
          }
          if (!isEmpty(item.widthValue.value)) {
            //宽
            param.widthValue = item.widthValue.value;
          }
          if (!isEmpty(item.wideValue.value)) {
            //封度
            param.wideValue = item.wideValue.value;
          }
          if (!isEmpty(item.lossValue.value)) {
            //损耗
            param.lossValue = item.lossValue.value;
          }
          if (!isEmpty(item.materialName.value)) {
            //部位ID
            param.partName = item.materialName.value;
          }

          param._LogName = _LogName;
          childList.push(param);
          if (!flag) {
            flagArr.push(item.itemId);
          }
          if (!singleFlag) {
            singleFlagArr.push(item.itemId);
          }
        }
      });
      function unique(arr) {
        //数组去重
        return Array.from(new Set(arr));
      }
      var fl = unique(flagArr);
      var sl = unique(singleFlagArr);

      if (fl.length > 0) {
        return {
          code: -1,
          data: flagArr
        };
      }
      if (fl.singleFlagArr > 0) {
        this.$message.error("计算公式不正确");
        return {
          code: -1,
          data: singleFlagArr
        };
      }
      return {
        code: 1,
        data: childList
      };
    },
    setTable(num, col) {
      // 初始化数据
      var listArr = [];
      for (var i = 0; i < num; i++) {
        var list = {};
        for (var j in this.tableData2[0]) {
          if (j == "id") {
            list["id"] = newGuid();
          } else if (j == "itemId") {
            if (col) {
              list["itemId"] = col;
            } else if (this.colItemId) {
              list["itemId"] = this.colItemId;
            } else {
              list["itemId"] = "";
            }
          } else {
            if (typeof this.tableData2[0][j] === "object") {
              list[j] = {};
              for (var k in this.tableData2[0][j]) {
                if (k == "id") {
                  list[j][k] = newGuid();
                  continue;
                }
                if (typeof this.tableData2[0][j][k] === "boolean") {
                  list[j][k] = false;
                } else {
                  list[j][k] = "";
                }
              }
            } else {
              list[j] = "";
            }
          }
        }
        listArr.push(list);
      }
      return listArr;
    },
    getMaterielData(item) {
      //获取物料档案信息
      this.materielData = [];

      var queryData = [];
      if (item) {
        if (item.materialCode.value != "") {
          queryData.push({
            column: "materialCode",
            content: item.materialCode.value,
            condition: 6
          });
        }
        // queryData.push({
        //   column: "materialName",
        //   content: "",
        //   condition: 6
        // });
      }
      queryData.push({
        column: "materialTypeName",
        content: "产品",
        condition: 5
      });

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
    rowClick(row) {
      this.$emit("setEditState", true);
      this.doItem.materialCode.valid = false;

      this.doItem.materialCode.value = row.materialName;
      this.doItem.materialCode.oldValue = row.materialName;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialCode.oldValue;

      this.listenClick();
      this.$set(this.doItem["materialName"], `isShow`, true);
      this.$nextTick(() => {
        var id_ = this.doItem["materialName"].id;
        document.getElementById(id_).focus();
        document.getElementById(id_).select();
        if (
          document.getElementById(id_).getAttribute("readonly") == "readonly"
        ) {
          document.getElementById(id_).click();
        }
      });
    },
    getSummaries(param) {
      //table总计
      const { columns, data } = param;
      const sums = [];
      columns.forEach((column, index) => {
        if (index === 0) {
          sums[index] = "总计";
          return;
        }
        const values = data.map(item => {
          return Number(item[column.property].value);
        });
        if (!values.every(value => isNaN(value))) {
          sums[index] = values.reduce((prev, curr) => {
            const value = Number(curr);
            if (!isNaN(value)) {
              return prev + curr;
            } else {
              return prev;
            }
          }, 0);
          if (index == 8) {
            sums[index] = getFloat(sums[index], 4);
          } else {
            sums[index] = "";
          }
        } else {
          sums[index] = "";
        }
      });

      return sums;
    },
    chengenum(event, item, name, state) {
      //双击显示input,监听input数据变化
      if (this.packageId === -1) {
        return;
      }
      //通过不能编辑
      if (this.selectRow.auditStatus === 2) {
        return;
      }

      event.stopPropagation();

      // this.doDefault(this.doItem);
      this.defaultAll();
      // this.popoverState = false;
      item[name].isShow = true;
      this.doItem = item;
      this.doItemName = name;
      this.$nextTick(() => {
        if (document.getElementById(item.id + "___" + name).value !== "") {
          this.$emit("setEditState", true);
        }
        if (document.getElementById(item.id + "___" + name)) {
          document.getElementById(item.id + "___" + name).focus();
          document.getElementById(item.id + "___" + name).select();
        }

        if (state == 1) {
          this.getMaterielData(); //物料数据
          this.findBox(item, name);
        }
      });
    },
    findBox(item, name) {
      var IH = document.getElementById(item.id).offsetHeight + 8;
      var IW = document.getElementById(item.id).offsetWidth + 24;
      if (this.$store.getters.sidebar.opened) {
        var ol = 210;
      } else {
        var ol = 54;
      }
      var wl = document.documentElement.clientWidth; // 屏幕宽度
      var wh = document.documentElement.clientHeight; // 屏幕宽度
      var PoL = document.getElementById(item.id).getBoundingClientRect().left; // 弹框left值
      var PoT = document.getElementById(item.id).getBoundingClientRect().top; // 弹框top值
      var PoW = parseInt(this.popoverStyle.width);
      var PoH = parseInt(this.popoverStyle.height);
      if (PoW + PoL > wl) {
        this.popoverStyle.left = PoL - ol - PoW + IW + "px";
      } else {
        this.popoverStyle.left = PoL - ol + "px";
      }
      if (PoT + PoH > wh) {
        this.popoverStyle.top = PoT - PoH - 84 - 9 + "px";
      } else {
        this.popoverStyle.top = PoT - 84 + IH + "px";
      }
      this.popoverState = true;
    },
    listenClick() {
      this.popoverState = false;
      this.defaultAll();
    },
    getByClass(oParent, sClass) {
      var aResult = [];
      var aEle = oParent.getElementsByTagName("*");

      for (var i = 0; i < aEle.length; i++) {
        if (aEle[i].className == sClass) {
          aResult.push(aEle[i]);
        }
      }

      return aResult;
    },
    pagination(val) {
      //改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      if (Object.keys(this.materialItem).length !== 0) {
        this.getMaterielData(this.materialItem);
      } else {
        this.getMaterielData();
      }
    },
    getMaterielDataIndex(item) {
      this.pageIndex = 1;
      if (item) {
        this.materialItem = item;
        this.getMaterielData(item);
      } else {
        this.getMaterielData();
      }
    },
    defaultData(data) {
      data.map(item => {
        for (var i in item) {
          this.$set(item, i + "_YF_Edit", false);
        }
      });
      return data;
    },
    doDefault(data) {
      this.menuState = false;
      for (var i in data) {
        if (typeof data[i] == "object") {
          if (data[i]["isShow"]) {
            this.$set(data[i], `isShow`, false);
          }
        }
      }
    },
    CopyOperate() {
      // if (this.treeItem.id == undefined) {
      //   this.$message.error("请选择包型！");
      //   return false;
      // }
      this.$refs["CopyOperate"].open(this.copyList);
    },
    doCopyOpt(list) {
      Storage.LStorage.set("copyList", list);
      this.copyList = list;
    }
  }
};
</script>
<style lang="scss" scoped>
#MainBill /deep/ {
  .Wsty {
    width: calc(100% - 300px);
  }
  .findBox {
    position: absolute;
    z-index: 99;
    padding: 12px;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    border-radius: 4px;
    border: 1px solid #ebeef5;
    background: #fff;
    .menuUl {
      padding: 0px;
      margin: 0px;
      li {
        list-style: none;
        padding: 6px 0px;
        text-align: center;
        font-size: 12px;
        cursor: pointer;
      }
    }
  }
  .header {
    padding-top: 20px;
  }
  .el-popover {
    height: 500px;
    overflow: auto;
  }
  .pr {
    position: relative;
  }
  .cell {
    padding: 0px;
  }
  .tableTd {
    box-sizing: content-box;
    min-height: 23px;
    padding: 8px 10px;
  }
  .validStyle {
    background: #f3d4d4;
  }
  .el-table--small td {
    padding: 0px;
  }
  .falseIp {
    width: 300px;
    height: 32px;
    line-height: 32px;
    overflow: hidden;
    border-bottom: 1px solid #dcdfe6;
  }
  .textAlign {
    text-align: center;
  }
  .start {
    color: #ff4949;
  }
  .color1 {
    background: #ff4949 !important;
  }
  .tableHeader {
    display: inline-block;
    padding: 0px 10px;
  }
  .dropdown button {
    position: relative;
    left: -1px;
    height: 32px;
    vertical-align: middle;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
  }
  .groupBtn > .el-button:last-child {
    border-top-right-radius: 0px;
    border-bottom-right-radius: 0px;
  }
  .el-dropdown {
    vertical-align: middle;
  }
  .fromCenter {
    display: flex;
    justify-content: center;
  }
  .footerCenter {
    width: 1150px;
    margin: 0 auto;
  }
  .has-gutter tr td {
    text-align: center;
  }
  .formItem {
    margin-right: 60px;
  }
  .el-form-item__label {
    padding-right: 4px;
  }
  .el-table {
    overflow: visible !important;
  }
  #elfooter .el-input__inner {
    border: 0px;
    border-radius: 0px;
    border-bottom: 1px solid #dcdfe6;
  }
  .el-select .el-input.is-disabled .el-input__inner {
    color: #606266;
    background: #fff;
  }
  .el-input.is-disabled .el-input__icon {
    cursor: default;
  }
  .leftBox {
    float: left;
    width: 200px;
    border: 1px solid #dfe6ec;
    border-right: 0px solid #dfe6ec;
  }
  .rightBox {
    float: left;
    width: calc(100% - 200px);
  }
  .el-table--group,
  .el-table--border {
    border: 0px solid #dfe6ec;
  }
  #tableData input {
    padding: 0px 10px;
  }
  .el-pagination {
    padding: 12px 5px;
  }
}
</style>
