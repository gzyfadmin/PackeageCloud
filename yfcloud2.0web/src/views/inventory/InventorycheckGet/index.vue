<template>
  <el-container
    id="ProcurementPut"
    v-on:click.native="listenClick"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    element-loading-text="正在加载"
  >
    <div class="btnHeader" id="btnHeader">
      <h1>盘点单</h1>
    </div>
    <el-header id="elheader" class="header fromCenter" height="auto">
      <el-form
        ref="dtData"
        :model="dtData"
        label-width="86px"
        class="FormInput"
        inline
        label-position="left"
        :rules="dtDataRules"
      >
        <el-form-item class="formItem" label="盘点仓库：">
          <div
            style="width:200px"
            class="falseIptop"
          >{{ dtData.warehouseName?dtData.warehouseName:"&nbsp;" }}</div>
        </el-form-item>

        <el-form-item class="formItem" label="盘点日期：">
          <div
            style="width:200px"
            class="falseIptop"
          >{{ warehousingDateForMat?warehousingDateForMat:"&nbsp;" }}</div>
        </el-form-item>
        <el-form-item class="formItem" label="编号：" label-width="58px">
          <div
            style="height:32px;min-width:128px;"
            class="falseIptop"
          >{{dtData.warehousingOrder?dtData.warehousingOrder:"&nbsp;"}}</div>
        </el-form-item>
      </el-form>
    </el-header>
    <el-main>
      <!-- //左边table选择框 -->

      <div class="findBox" :style="popoverStyle" v-show="popoverState" v-on:click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="materialCode" label="物料代码" width="180" />
          <el-table-column prop="materialName" label="物料名称" />
          <el-table-column prop="spec" label="规格型号" />
          <el-table-column prop="colorName" label="颜色" />
          <el-table-column prop="baseUnitName" label="基本计量单位" />
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
        </ul>
      </div>
      <div v-on:click.stop="1==1">
        <el-table
          id="tableData"
          ref="table"
          :height="tableHeight"
          :data="tableData"
          style="width: 99.9%"
          row-key="id"
          border
          :show-summary="true"
          :summary-method="getSummaries"
        >
          <el-table-column label="序号" width="70">
            <template slot="header">
              <span class="tableHeader">
                <span>序号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="textAlign">
                <span>{{scope.$index+1}}</span>
              </div>
            </template>
          </el-table-column>
          <!-- <span class="start">*</span> -->
          <el-table-column prop="materialCode" label="物料代码">
            <template slot="header">
              <span class="tableHeader">
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.materialCode.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.materialCode.isShow"
                >{{scope.row.materialCode.value}}</div>
                <el-input
                  v-show="scope.row.materialCode.isShow"
                  v-model="scope.row.materialCode.value"
                  @keyup.enter.native="getMaterielData(scope.row)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="物料名称">
            <template slot="header">
              <span class="tableHeader">
                <span>物料名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.materialName.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.materialName.isShow"
                >{{scope.row.materialName.value}}</div>
                <el-input
                  v-show="scope.row.materialName.isShow"
                  v-model="scope.row.materialName.value"
                  @keyup.enter.native="getMaterielData(scope.row)"
                  :id="scope.row.id+'___materialName'"
                  size="mini"
                ></el-input>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="spec" label="规格型号">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.spec.valid}">
                <div class="tableTd">{{scope.row.spec.value}}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="colorName" label="颜色">
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.colorName.valid}">
                <div class="tableTd">{{scope.row.colorName.value}}</div>
              </div>
            </template>
          </el-table-column>
           

          <!-- 【账存数量】是指库存计量单位的库存数量。不填 -->
          <el-table-column prop="accountNum" label="账存数量">
            <template slot="header">
              <span class="tableHeader">
                <span>账存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.accountNum.value}}</div>
            </template>
          </el-table-column>

          <!-- 【实存数量】=【账存数量】-【盘亏数量】 -->
          <!-- 【实存数量】属于必填，通过库存计量单位的实存数量，由制单人手动输入并且自动计算基本单位实存数量。 -->

          <el-table-column prop="actualNum" label="实存数量">
            <template slot="header">
              <span class="tableHeader">
                <span>实存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.actualNum.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.actualNum.isShow"
                >{{scope.row.actualNum.value}}</div>
                <el-input
                  v-show="scope.row.actualNum.isShow"
                  v-model="scope.row.actualNum.value"
                  :id="scope.row.id+'___actualNum'"
                  size="mini"
                  @input="actualNumBlur(scope.row,scope.row.actualNum.value)"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <!-- 【实存数量】=【账存数量】-【盘亏数量】 -->
          <!-- 【盘盈数量】中的盘盈数量，与基本计量单位换算率，换算成出该基本单位盘盈数量 -->
          <el-table-column prop="profitNum" label="盘盈数量">
            <template slot="header">
              <span class="tableHeader">
                <span>盘盈数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- {{scope.row.accountNum.value}} 账存-->
              <!-- {{scope.row.actualNum.value}} 实存-->
              <div
                class="tableTd"
                v-if="scope.row.profitNum.value>0"
                style="color:#00CC33"
              >{{scope.row.profitNum.value}}</div>
              <!-- <div v-if="scope.row.actualNum.value-scope.row.accountNum.value>0" class="tableTd">{{scope.row.actualNum.value-scope.row.accountNum.value}}</div> -->
            </template>
          </el-table-column>

          <!-- 【盘亏数量】数量大于等于0，取值来源是根据【实存数量】减去【账存数量】为负数转化为正数得结果，允许手动输入，输入后【实存数量】=【账存数量】-【盘亏数量】 -->
          <!-- 不填 -->

          <el-table-column prop="deficitNum" label="盘亏数量">
            <template slot="header">
              <span class="tableHeader">
                <span>盘亏数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                v-if="scope.row.deficitNum.value>0"
                style="color:red"
              >{{scope.row.deficitNum.value}}</div>
            </template>
          </el-table-column>

          <!-- 【单位】该单位的取值来源是物料档案中配置的库存计量单位。 -->
          <el-table-column prop="warehouseUnitName">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                v-if="scope.row.warehouseUnitName.value"
                class="tableTd"
              >{{scope.row.warehouseUnitName.value}}</div>
              <div
                v-if="!scope.row.warehouseUnitName.value"
                class="tableTd"
              >{{scope.row.unit.value}}</div>
            </template>
          </el-table-column>
          <el-table-column prop="unit" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.unit.value}}</div>
            </template>
          </el-table-column>

          <!-- 【基本单位账存数量】根据选择的物料代码    自动取数据库 中该物料在该盘点仓库存储的  物料数量。 -->
          <el-table-column prop="accountUnitNum" label="基本单位账存数量" width="120px">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位账存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.accountUnitNum.value}}</div>
            </template>
          </el-table-column>

          <!-- 【基本单位实存数量】根据库存 计量单位中输入的实存数量 再根据物料档案中配置的与基本计单位换算率自动计算得出。 -->

          <!-- 要填 -->
          <el-table-column prop="actualUnitNum" label="基本单位实存数量" width="120px">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位实存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                v-if="scope.row.actualUnitNum.value>0"
              >{{scope.row.actualUnitNum.value}}</div>
            </template>
          </el-table-column>

          <!-- 【基本单位盘盈数量】根据选择的物料代码，取值来源是通过本表单 -->
          <el-table-column prop="profitUnitNum" label="基本单位盘盈数量" width="120px">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位盘盈数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                v-if="scope.row.profitUnitNum.value>0"
              >{{scope.row.profitUnitNum.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="deficitUnitNum" label="基本单位盘亏数量" width="120px">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位盘亏数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                v-if="scope.row.deficitUnitNum.value>0"
              >{{scope.row.deficitUnitNum.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="remark" label="备注" width="240">
            <template slot="header">
              <span class="tableHeader">
                <span class="start"></span>
                <span>备注</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.remark.valid}">
                <div class="tableTd" v-show="!scope.row.remark.isShow">
                  <el-popover
                    class="item"
                    trigger="hover"
                    placement="top-start"
                    :content="scope.row.remark.value"
                    v-if="scope.row.remark.value.length>=15"
                  >
                    <div class="ellipsis" slot="reference">{{scope.row.remark.value}}</div>
                  </el-popover>
                  <div
                    class="ellipsis"
                    v-if="scope.row.remark.value.length<15"
                  >{{scope.row.remark.value}}</div>
                </div>
                <el-input
                  v-show="scope.row.remark.isShow"
                  v-model="scope.row.remark.value"
                  :id="scope.row.id+'___remark'"
                  size="mini"
                ></el-input>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div id="elfooter" class="fromCenter">
      <el-form
        label-width="76px"
        class="FormInput"
        :rules="dtDataRules"
        ref="dtData"
        :model="dtData"
        inline
      >
        <!-- style="width:1168px" -->
        <el-form-item label="仓管员：" prop="principalId">
          <el-select
            filterable
            v-if="disappear&&addControl"
            @change="changeprincipalId"
            v-model="dtData.principalId"
            style="width:200px;"
            placeholder="请选择"
          >
            <el-option
              v-for="item in warehousepeople"
              :key="item.principalId"
              :label="item.realName"
              :value="item.principalId"
            ></el-option>
          </el-select>

          <div class="falseIp" v-if="!addControl">{{ dtData.whAdminName }}</div>
          <!-- addControl -->
        </el-form-item>
        <!-- operatorlist -->

        <el-form-item label="盘点人：">
          <div class="falseIp">{{ dtData.inventoryName }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item label="制单人：" v-if="addControl" prop="operatorId">
          <el-select
            v-if="addControl"
            filterable
            v-model="dtData.operatorId"
            style="width:200px;"
            placeholder="请选择"
          >
            <el-option
              v-for="item in operatorlist"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="制单人：" v-if="!addControl">
          <div class="falseIp" v-if="!addControl">{{ dtData.operatorName }}</div>
        </el-form-item>

        <el-form-item label="审核结果：">
          <div class="falseIp">
            <span v-if="dtData.auditStatus==-1"></span>
            <span v-if="dtData.auditStatus==0">待审核</span>
            <span v-if="dtData.auditStatus==1">未通过</span>
            <span v-if="dtData.auditStatus==2">通过</span>
            <span v-if="dtData.auditStatus==3">撤销审核</span>
          </div>
        </el-form-item>

        <el-form-item label="审核时间：">
          <div class="falseIp">{{dtData.auditTime}}</div>
        </el-form-item>
      </el-form>
    </div>
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit"></InboundOrder>
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import InboundOrder from "./components/InboundOrder";

import Pagination from "@/components/Pagination";
// import { promises } from 'dns';
import { setTimeout } from "timers";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import BigNumber from "bignumber.js";

export default {
  name: "HLDYY",
  data() {
    return {
      OtherData: {},
      warehousingDateForMat: "",
      globalid: 0,
      disappear: true,
      nowrow: [],
      roleList: [],
      warehousepeople: [],
      operatorlist: [],
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
      dtData: {
        warehouseName: "",
        whAdminName: "",
        WarehousingType: "",
        warehousingDate: new Date(),
        warehousingOrder: "",
        remark: "",
        operatorName: "",
        operatorId: "",
        receiptName: "",
        receiptId: "",
        auditName: "",
        auditId: "",
        auditStatus: -1,
        auditTime: "",
        warehouseId: ""
      },
      dtDataRules: {
        warehouseId: [
          { required: true, message: "请选择盘点仓库", trigger: "change" }
        ],
        operatorId: [
          { required: true, message: "请选择制单人", trigger: "change" }
        ],
        WarehousingType: [
          { required: true, message: "请选择盘点类型", trigger: "change" }
        ],
        // hihi

        warehousingDate: [
          {
            type: "date",
            required: true,
            message: "请选择日期",
            trigger: "change"
          }
        ],
        warehousingOrder: [{ required: true, message: "获取编号失败" }]
      },
      tableHeight: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: [],
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], //物料信息
      warehouseData: [], //仓库信息
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          idandmainid: {
            id: 0,
            mainId: 0,
            isShow: false,
            valid: false
          },
          materialName: {
            value: "",
            isShow: false,
            valid: false
          },
          colorName: {
            value: "",
            isShow: false,
            valid: false
          },
          spec: {
            value: "",
            isShow: false,
            valid: false
          },
          unit: {
            value: "",
            isShow: false,
            valid: false
          },

          accountNum: {
            //账存数量
            value: "",
            isShow: false,
            valid: false
          },
          actualNum: {
            //实存数量
            value: "",
            isShow: false,
            valid: false
          },

          accountUnitNum: {
            //基本单位账存数量
            value: "",
            isShow: false,
            valid: false
          },
          actualUnitNum: {
            //基本单位实存数量
            value: "",
            isShow: false,
            valid: false
          },

          profitNum: {
            //盘盈数量
            value: "",
            isShow: false,
            valid: false
          },
          deficitNum: {
            //盘亏数量
            value: "",
            isShow: false,
            valid: false
          },
          profitUnitNum: {
            //基本单位盘盈数量
            value: "",
            isShow: false,
            valid: false
          },
          deficitUnitNum: {
            //基本单位盘亏数量
            value: "",
            isShow: false,
            valid: false
          },

          warehouseUnitId: {
            //基本单位换算率id
            value: "",
            isShow: false,
            valid: false
          },
          warehouseRate: {
            //基本单位换算率
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            //备注
            value: "",
            isShow: false,
            valid: false
          },

          batchNumber: {
            value: "",
            isShow: false,
            valid: false
          },

          warehouseUnitName: {
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            value: "",
            isShow: false,
            valid: false
          },
          unitPrice: {
            value: "",
            isShow: false,
            valid: false
          },
          amount: {
            value: "",
            isShow: false,
            valid: false
          },
          validityPeriod: {
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

      realNameOptions: [], //收货员列表
      addControl: true, //是否显示新增按钮
      mountedState: false
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
    InboundOrder
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
  async mounted() {
    this.runing();
  },
  async activated() {
    if(this.atob(this.$route.query.id)==this.OtherData.id){
      return;
    }
    this.runing();
  },
  methods: {
    async runing() {
      if (this.mountedState == true) {
        return;
      }
      this.fullscreenLoading = true;
      this.mountedState = true;

      this.setStyle(); //设置样式
      this.getMaterielData(); //物料数据
      this.getWarehouseData(); //仓库数据
      this.getUserCompany(); //收货员列表
      if (this.$route.query.id) {
        // this.OtherData = this.$route.params;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.getMainList2(this.OtherData);
      } else {
      }
      
      this.mountedState = false;
    },
    changeprincipalId() {
      var that = this;
      that.disappear = false;
      that.disappear = true;
    },
    changewarehousepeople() {
      var that = this;
      that.warehouseData.map(item => {
        if (item.id == that.dtData.warehouseId) {
          that.dtData.principalId = parseInt(item.principalId);

          return false;
        }
      });
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = document.getElementById("btnHeader").offsetHeight; //按钮高度
        // var btn = 37; //按钮高度
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        this.tableHeight = b - h - f - 40 - 20 - btn;
      });
    },
    resetData() {
      //初始化数据
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      // this.setTable(30);
      // this.tableData = this.setTable(30);
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.WarehousingType = "";
      this.dtData.warehousingDate = new Date();
      this.dtData.warehousingOrder = "";
      this.dtData.remark = "";
      this.dtData.operatorName = "";
      this.dtData.operatorId = "";
      this.dtData.receiptName = "";
      this.dtData.receiptId = "";
      this.dtData.whAdminName = "";
      // whAdminName
      this.dtData.auditName = "";
      this.dtData.auditId = "";
      this.dtData.auditStatus = -1;
      this.dtData.auditTime = "";
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
      this.menuState = true;
    },
    doMenuAdd() {
      this.menuState = false;
    },
    doMenuDel() {
      this.menuState = false;
      if (this.delNum != -1) {
        this.tableData.splice(this.delNum, 1);
        this.delNum = -1;
      }
    },
    tableRowClassName({ row, rowIndex }) {
      if (row.rowIndex === 1) {
        return "color1";
      }
    },
    WarehouseChange(item) {
      //仓库选择
      this.warehouseData.map(data => {
        if (item.key == data.id) {
          item.value = data.warehouseName;
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
    getCode() {
      request({
        url: "warehouse/api/TWMInventoryMain/GetOrderNo",
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = "";
        } else {
          this.dtData.warehousingOrder = res;
        }
      });
    },

    async addPutInStorage(state) {
      //新增
      if (state == 1 || state == 2) {
        var flag = true;

        this.$refs.dtData.validate(valid => {
          //验证
          if (!valid) {
            // this.$message({
            //   message: "数据不合法，无法保存",
            //   type: "error"
            // });
          } else {
            flag = true;
          }
        });
        var checkSelect = await this.checkSelect(this.tableData);
        var childList = [];

        childList = [];
        checkSelect.map(item => {
          //验证数据

          if (state == 1) {
            var param = {
              id: 0,
              mainId: 0,
              materialId: item.materialCode.key,
              warehouseId: this.dtData.warehouseId,
              accountNum: item.accountNum.value ? item.accountNum.value : 0,
              actualNum: parseInt(item.actualNum.value)
                ? parseInt(item.actualNum.value)
                : 0,
              profitNum: item.profitNum.value ? item.profitNum.value : 0,
              deficitNum: item.deficitNum.value ? item.deficitNum.value : 0,
              remark: item.remark.value,
              _LogName: "盘点单其中一条信息"
            };
          } else if (state == 2) {
            var param = {
              id: item.idandmainid.id ? item.idandmainid.id : 0,
              mainId: this.globalid,
              materialId: item.materialCode.key,
              warehouseId: this.dtData.warehouseId,
              accountNum: item.accountNum.value ? item.accountNum.value : 0,
              actualNum: parseInt(item.actualNum.value)
                ? parseInt(item.actualNum.value)
                : 0,
              profitNum: item.profitNum.value ? item.profitNum.value : 0,
              deficitNum: item.deficitNum.value ? item.deficitNum.value : 0,
              remark: item.remark.value,
              _LogName: "盘点单其中一条信息"
            };
          }

          if (param.materialId) {
            childList.push(param);

            if (!param.warehouseId || !param.actualNum) {
              //  this.$message.error("数据不完整");

              flag = false;
            }
          }
        });
        if (state == 1) {
          var para = {
            postData: {
              id: 0,
              warehouseId: this.dtData.warehouseId,
              inventoryDate: formatDate(this.dtData.warehousingDate),
              inventoryOrder: this.dtData.warehousingOrder,
              operatorId: this.dtData.operatorId,
              whAdminId: this.dtData.warehouseId,
              childList: childList,
              _LogName: "一个盘点单"
            }
          };
        } else if (state == 2) {
          var para = {
            postData: {
              id: this.globalid,
              warehouseId: this.dtData.warehouseId,
              inventoryDate: formatDate(this.dtData.warehousingDate),
              childList: childList,
              _LogName: "一个盘点单"
            }
          };
        }

        if (state == 1) {
          if (
            !flag ||
            !para.postData.warehouseId ||
            !para.postData.inventoryDate ||
            !para.postData.inventoryOrder ||
            !para.postData.operatorId ||
            !para.postData.whAdminId ||
            para.postData.childList.length == 0
          ) {
            this.$message.error("数据不完整");
            return;
          } else {
            // /api/TWMInventoryMain
            this.postData(para, state);
          }
        } else if (state == 2) {
          if (this.selectRow.id == undefined) {
            this.$message.error("请选择修改的数据");
            return;
          }
          if (
            !flag ||
            !para.postData.warehouseId ||
            !para.postData.inventoryDate ||
            !para.postData.id ||
            para.postData.childList.length == 0
          ) {
            this.$message.error("数据不完整");
            return;
          } else {
            // /api/TWMInventoryMain
            this.postData(para, state);
          }
        }
      }
      if (state == 3) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该盘点单是通过状态，无法删除");
          return;
        }
        if (this.selectRow.id == undefined) {
          this.$message.error("请选择删除的数据");
          return;
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的盘点单`
        };

        var data = RequestObject.CreatePostObject(currData);

        this.$confirm("是否删除盘点单？", "确认信息", {
          type: "warning"
        })
          .then(() => {
            this.postData(data, state);
          })
          .catch(() => {});
      }
    },

    postData(data, state) {
      var data = data;
      var state = state;
      var method = "";
      if (state == 1) {
        method = "post";
      }
      if (state == 2) {
        method = "put";
      }
      if (state == 3) {
        method = "DELETE";
      }
      this.fullscreenLoading = true;

      request({
        url: "/warehouse/api/TWMInventoryMain",
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "操作成功",
            type: "success"
          });
          if (state == 1) {
            this.OnBtnSaveSubmit(res.data);
          }
          if (state == 3) {
            this.tableData = [];
          }
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        }
      });
    },

    getWarehouseData() {
      // 查看仓库档案信息
      var reqObject;
      reqObject = RequestObject.CreateGetObject(false, 0, 0, []);
      request({
        url: "/basicset/api/TBMWarehouseFile",
        method: "get",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        // 获取仓库中的仓管员对象并去重

        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.warehouseData = res.data;
          var warehousepeople = [];
          res.data.map(item => {
            if (item.principalId) {
              var obj = new Object({
                principalId: item.principalId,
                realName: item.realName
              });
              warehousepeople.push(obj);
            }
          });

          let obj = {};
          let peon = warehousepeople.reduce((cur, next) => {
            obj[next.principalId]
              ? ""
              : (obj[next.principalId] = true && cur.push(next));
            return cur;
          }, []); //设置cur默认类型为数组，并且初始值为空的数组
          this.warehousepeople = peon;
        }
      });
    },
    setTable(num) {
      // this.fullscreenLoading = true;
      //初始化30条数据
      var listArr = [];
      for (var i = 0; i < num; i++) {
        var list = {};
        for (var j in this.tableData2[0]) {
          if (j == "id") {
            list["id"] = newGuid();
          } else {
            if (typeof this.tableData2[0][j] == "object") {
              list[j] = {};
              for (var k in this.tableData2[0][j]) {
                if (typeof this.tableData2[0][j][k] == "boolean") {
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
      this.totalCount = 0;

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
      var param = [];
      var robject = RequestObject.CreateGetObject(false, 0, 0, param);
      request({
        url: "/system/api/TSMUser",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
        if (res.code === 0) {
          var operatorlist = [];
          res.data.map(item => {
            var obj = new Object({
              id: item.id,
              realName: item.realName
            });
            operatorlist.push(obj);
          });
          this.operatorlist = operatorlist;
        } else {
        }
      });
      // },
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
    rowClick(row) {
      // warehouseName
      this.doItem.materialCode.value = row.materialCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.materialName;
      this.doItem.spec.value = row.spec;
      this.doItem.colorName.value = row.colorName;
      this.doItem.warehouseUnitName.value = row.warehouseUnitName;
      this.doItem.unit.value = row.baseUnitName;
      this.doItem.baseUnitNumber.value = row.baseUnitNumber;
      this.doItem.warehouseUnitId.value = row.warehouseUnitId;
      this.doItem.warehouseRate.value = row.warehouseRate;

      var currData = {
        postData: {
          materialId: row.id,
          warehouseId: row.defaultWarehouseId
        }
      };
      var that = this;
      request({
        url: "/warehouse/api/WarehouseAmount",
        method: "post",
        data: currData
      }).then(res => {
        if (res.code === 0) {
          this.doItem.accountNum.value = res.data.accountNum;
          that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
          // item.actualNum.value
        } else {
        }
      });
      this.listenClick();
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
              return BigNumber(prev).plus(curr);
            } else {
              return prev;
            }
          }, 0);
          // actualNum
          if (index == 5 || index == 6 || index == 7 || index == 8) {
            sums[index] += "";
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
      event.stopPropagation();
      this.nowrow = item;

      this.doDefault(this.doItem);
      this.popoverState = false;
      item[name].isShow = true;
      this.doItem = item;
      this.doItemName = name;
      this.$nextTick(() => {
        if (document.getElementById(item.id + "___" + name)) {
          document.getElementById(item.id + "___" + name).focus();
        }

        if (state == 1) {
          this.findBox(item, name);
        }
      });
    },
    findBox(item, name) {
      var IH = document.getElementById(item.id + "___" + name).offsetHeight + 8;
      var IW = document.getElementById(item.id + "___" + name).offsetWidth + 24;
      if (this.$store.getters.sidebar.opened) {
        var ol = 210;
      } else {
        var ol = 54;
      }
      var wl = document.documentElement.clientWidth; //屏幕宽度
      var wh = document.documentElement.clientHeight; //屏幕宽度
      var PoL = document
        .getElementById(item.id + "___" + name)
        .getBoundingClientRect().left; //弹框left值
      var PoT = document
        .getElementById(item.id + "___" + name)
        .getBoundingClientRect().top; //弹框top值

      //pol和PoT是主要数据

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
      this.doDefault(this.doItem);
    },
    changeBlur(item, value) {},
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
      this.getMaterielData();
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
            data[i]["isShow"] = false;
          }
        }
      }
    },

    //输入金额时顺便计算
    actualNumBlur(item, input) {
      // actualNum
      var r = /^\+?[1-9][0-9]*$/; //正整数
      var flag = r.test(input);

      if (!flag) {
        item.actualNum.value = null;
      }
      if (item.actualNum.value == "" || item.actualNum.value == null) {
        //如果不是输入数字实存数量默认为0
        var nowactualNum = 0;
      } else {
        var nowactualNum = item.actualNum.value;
      }
      nowactualNum = parseFloat(nowactualNum);
      if (nowactualNum - item.accountNum.value > 0) {
        item.profitNum.value = parseFloat(nowactualNum - item.accountNum.value);
      } else {
        item.profitNum.value = null;
      }
      if (nowactualNum - item.accountNum.value < 0) {
        item.deficitNum.value = parseFloat(
          item.accountNum.value - nowactualNum
        );
      } else {
        item.deficitNum.value = null;
      }

      if (
        item.warehouseUnitId.value != null &&
        item.warehouseRate.value != null
      ) {
        item.accountUnitNum.value = parseFloat(
          item.accountNum.value * item.warehouseRate.value
        )
          ? parseFloat(item.accountNum.value * item.warehouseRate.value)
          : null;
        item.actualUnitNum.value = parseFloat(
          nowactualNum * item.warehouseRate.value
        )
          ? parseFloat(nowactualNum * item.warehouseRate.value)
          : null;
        item.profitUnitNum.value = parseFloat(
          item.profitNum.value * item.warehouseRate.value
        )
          ? parseFloat(item.profitNum.value * item.warehouseRate.value)
          : null;
        item.deficitUnitNum.value = parseFloat(
          item.deficitNum.value * item.warehouseRate.value
        )
          ? parseFloat(item.deficitNum.value * item.warehouseRate.value)
          : null;
      } else {
        item.accountUnitNum.value = parseFloat(item.accountNum.value)
          ? parseFloat(item.accountNum.value)
          : null;
        item.actualUnitNum.value = parseFloat(nowactualNum)
          ? parseFloat(nowactualNum)
          : null;
        item.profitUnitNum.value = parseFloat(item.profitNum.value)
          ? parseFloat(item.profitNum.value)
          : null;
        item.deficitUnitNum.value = parseFloat(item.deficitNum.value)
          ? parseFloat(item.deficitNum.value)
          : null;
      }
    },
    unitPriceBlur(item) {
      if (item.unitPrice.value == "" || item.unitPrice.value == null) return;

      if (item.PaidIn.value != "") {
        item.unitPrice.value = parseFloat(item.unitPrice.value);
        item.amount.value = keepTwoDecimalFull(
          accMul(item.PaidIn.value, item.unitPrice.value)
        );
      } else {
        item.unitPrice.value = keepTwoDecimalFull(
          parseFloat(item.unitPrice.value)
        );
      }
    },
    amountBlur(item) {
      if (item.amount.value == "" || item.amount.value == null) return;

      if (item.PaidIn.value != "") {
        item.amount.value = parseFloat(item.amount.value);
        item.unitPrice.value = keepTwoDecimalFull(
          item.amount.value / item.PaidIn.value
        );
      } else {
        item.amount.value = keepTwoDecimalFull(parseFloat(item.amount.value));
      }
    },

    handelAddClick() {
      //查询
      //点击添加按钮
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.dtData.warehousingOrder = "";
      this.$refs.InboundOrder.dtData.warehousingDate = "";
      this.$refs.InboundOrder.dtData.auditStatus = -1;
      this.$refs.InboundOrder.clickRow = {};
      this.$refs.InboundOrder.getMainList();
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    getMainList2(e) {
      //列表
      request({
        url: "warehouse/api/TWMInventoryMain/GetInventoryOrder?id=" + e.id,
        method: "GET"
      }).then(res => {
        if (res.code === 0) {
          if (res.code === 0) {
            // warehouseName
            this.dtData.warehouseName = res.data.warehouseName;
            this.setTableData(res.data.childList);
            this.dtData.warehousingDate = formatDate(res.data.inventoryDate);
            this.warehousingDateForMat = res.data.inventoryDate.split("T")[0];
            this.dtData.warehousingOrder = res.data.inventoryOrder;
            this.selectRow = this.cloneObject(res.data);
            this.dtData.whAdminName = res.data.whAdminName;
            this.dtData.principalId = res.data.whAdminId; //仓管员
            this.dtData.operatorName = res.data.operatorName;
            this.dtData.auditName = res.data.auditName;
            this.dtData.auditStatus = res.data.auditStatus;
            this.dtData.inventoryId = res.data.inventoryId; //盘点员id
            this.dtData.inventoryName = res.data.inventoryName; //盘点员姓名
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split("T")[0]
                : "";
          }
        }
      });
    },

    OnBtnSaveSubmit(row) {
      this.dtData.warehousingDate = formatDate(row.inventoryDate);
      this.warehousingDateForMat = row.inventoryDate.split("T")[0];
      request({
        url: "warehouse/api/TWMInventoryMain/GetInventoryOrder?id=" + row.id,
        method: "GET"
      }).then(res => {
        if (res.code === 0) {
          this.setTableData(res.data.childList);
          this.setTableData(res.data.childList);
          this.selectRow = this.cloneObject(res.data);
          this.dtData.whAdminName = res.data.whAdminName;
          this.dtData.principalId = res.data.whAdminId;
          // whAdminId
          this.dtData.operatorName = res.data.operatorName;
          this.dtData.auditName = res.data.auditName;
          this.dtData.auditStatus = res.data.auditStatus;
          this.dtData.inventoryId = res.data.inventoryId;
          this.dtData.inventoryName = res.data.inventoryName; //盘点员姓名
          this.dtData.auditTime =
            res.data.auditTime != null ? res.data.auditTime.split("T")[0] : "";
        }
      });
      this.selectRow = this.cloneObject(row);
      this.dtData.whAdminName = row.whAdminName;
      this.dtData.operatorName = row.operatorName;
      this.dtData.auditName = row.auditName;
    },

    setCurrData(data) {
      //设置编辑时候的日记数据
      var childList = [];
      var _LogName = "";
      this.cloneTable.map(item => {
        var param = {
          id: 0,
          mainId: 0,
          materialId: parseFloat(item.materialCode.key) || "",
          warehouseId: parseFloat(item.defaultWarehouseName.key) || ""
        };
        if (item.batchNumber.value != "") {
          //批号
          param.batchNumber = item.batchNumber.value;
        }
        if (item.unitPrice.value != "" && !isNaN(item.unitPrice.value)) {
          //单价
          param.unitPrice = parseFloat(item.unitPrice.value) || 0;
        }
        if (item.amount.value != "" && !isNaN(item.amount.value)) {
          //金额
          param.amount = parseFloat(item.amount.value) || 0;
        }
        if (
          item.validityPeriod.value != "" &&
          !isNaN(item.validityPeriod.value)
        ) {
          //金额
          param.validityPeriod = formatDate(item.validityPeriod.value) + "";
        }

        param._LogName = _LogName;
        childList.push(param);
      });
      var currData = {
        id: this.selectRow.id,
        warehousingType: this.selectRow.warehousingType,
        warehousingOrder: this.selectRow.warehousingOrder,
        childList: childList
      };

      this.PostDataEdit = currData;
    },

    setTableData(dt) {
      //将数据导入table
      this.tableData = [];
      if (dt[0]) {
        this.globalid = dt[0].mainId;
      }
      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,

          idandmainid: {
            id: item.id,
            mainId: item.mainId,
            isShow: false,
            valid: false
          },
          materialCode: {
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            value: item.materialName,
            isShow: false,
            valid: false
          },
          spec: {
            value: item.spec,
            isShow: false,
            valid: false
          },
          colorName: {
            //颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },

          unit: {
            //单位
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          accountNum: {
            //账存数量
            value: item.accountNum,
            isShow: false,
            valid: false
          },
          actualNum: {
            //实存数量
            value: item.actualNum,
            isShow: false,
            valid: false
          },

          accountUnitNum: {
            //基本单位账存数量
            value: item.accountUnitNum,
            isShow: false,
            valid: false
          },
          actualUnitNum: {
            //基本单位实存数量
            value: item.actualUnitNum,
            isShow: false,
            valid: false
          },
          profitNum: {
            //盘盈数量
            value: item.profitNum,
            isShow: false,
            valid: false
          },
          deficitNum: {
            //盘亏数量
            value: item.deficitNum,
            isShow: false,
            valid: false
          },
          profitUnitNum: {
            //基本单位盘盈数量
            value: item.profitUnitNum,
            isShow: false,
            valid: false
          },
          deficitUnitNum: {
            //基本单位盘亏数量
            value: item.deficitUnitNum,
            isShow: false,
            valid: false
          },
          warehouseUnitId: {
            //基本单位换算率id
            value: item.warehouseUnitId,
            isShow: false,
            valid: false
          },
          warehouseRate: {
            //基本单位换算率
            value: item.warehouseRate,
            isShow: false,
            valid: false
          },
          remark: {
            //备注
            value: item.remark,
            isShow: false,
            valid: false
          },
          // remark

          defaultWarehouseName: {
            //无用
            //盘点仓库
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          batchNumber: {
            //无用
            value: item.batchNumber, //批号
            isShow: false,
            valid: false
          },

          warehouseUnitName: {
            //仓库单位
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            //无用
            value: item.baseUnitNumber,
            isShow: false,
            valid: false
          },
          unitPrice: {
            //无用
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          amount: {
            //无用
            value: item.amount,
            isShow: false,
            valid: false
          },
          validityPeriod: {
            //无用
            value:
              item.validityPeriod != null
                ? new Date(item.validityPeriod.split("T")[0])
                : "",
            isShow: false,
            valid: false
          }
        };
        this.warehouseData.map(data => {
          //无用
          if (list.defaultWarehouseName.key == data.id) {
            list.defaultWarehouseName.value = data.warehouseName;
          }
        });

        this.tableData.unshift(list);
      });
      this.cloneTable = [...listArr];
      this.setCurrData(this.cloneTable);
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 0);
    },
    doOtherWhAudit(state) {
      //审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error("该盘点单已经为通过状态");
        return;
      }
      var data = RequestObject.CreatePostObject({
        id: this.selectRow.id,
        auditStatus: state,
        ChildList: []
      });
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/TWMInventoryMain/InventoryAudit",
        method: "put",
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "操作成功",
            type: "success"
          });
          this.dtData.auditStatus = state;
          this.selectRow.auditStatus = state;
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        }
      });
    },
    doOtherWhCancelAudit() {
      //撤销审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus != 2) {
        this.$message.error("该盘点单不是通过状态，无法撤销");
        return;
      }
      var data = RequestObject.CreatePostObject({
        id: this.selectRow.id,
        auditStatus: 3,
        ChildList: []
      });
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/TWMOtherWhMain/OtherWhCancelAudit",
        method: "put",
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "操作成功",
            type: "success"
          });
          this.dtData.auditStatus = 3;
          this.selectRow.auditStatus = 3;
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 500);
        }
      });
    },
    handleDownload() {
      //导出
      import("@/vendor/Export2Excel").then(excel => {
        const multiHeader = [
          ["name", this.dtData.supplier, "amount1", "amount2", "amount3"]
        ];
        const header = ["ID", "materialCode", "amount1", "amount2", "amount3"];
        const filterVal = [
          "id",
          "materialCode",
          "amount1",
          "amount2",
          "amount3"
        ];
        const list = this.tableData;
        const data = this.formatJson(filterVal, list);
        const merges = ["A1:A1", "B1:B1"];
        excel.export_json_to_excel({
          multiHeader,
          header,
          merges,
          data
        });
      });
    },
    formatJson(filterVal, jsonData) {
      //导出
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === "timestamp") {
            return parseTime(v[j]);
          } else {
            return v[j];
          }
        })
      );
    }
  }
};
</script>
<style lang="scss" scoped>
#ProcurementPut /deep/ {
  #elfooter {
    padding: 0px 20px;
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
    border-bottom: 1px solid #c0c4cc;
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
  // .falseIptop {
  //   width: 200px;
  //   overflow: hidden;
  //   border-bottom: 1px solid #dcdfe6;
  // }
}
@import "@/styles/receipts.scss";
</style>
