<template>
  <el-container
    id="SalesReceipt"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <!-- 头部按钮区开始 -->
    <div id="btnHeader" class="btnHeader">
      <h1>销售出库单</h1>
    </div>
    <!-- 头部按钮区结束 -->

    <!-- 头部表单开始 -->
    <el-header id="elheader" class="header" height="auto">
      <div class="middleWidth">
        <el-form
          ref="dtData"
          :model="dtData"
          label-width="76px"
          class="FormInput"
          inline
          label-position="right"
          :rules="dtDataRules"
        >
          <el-form-item class="formItem" label="源单类型：">
            <div class="headerIp">
              <span>销售订单</span>
            </div>
          </el-form-item>
          <el-form-item class="formItem" label="单据号：">
            <!-- <el-input
              class="headerIp"
              v-model="dtData.orderNo"
              @click.native="chooseNumber"
              readonly
            >
              <i slot="suffix" class="el-input__icon el-icon-search"></i>
            </el-input>-->
            <div class="headerIp">{{ dtData.orderNo }}</div>
          </el-form-item>
          <el-form-item class="formItem disableType" label="客户名称：">
            <div class="headerIp">{{ dtData.customerName }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="状态：">
            <div class="headerIp">
              <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待入库</span>
              <span v-if="dtData.auditStatus==0">入库待审核</span>
              <span v-if="dtData.auditStatus==1">审核未通过</span>
              <span v-if="dtData.auditStatus==2">入库完成</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="编号：">
            <div class="headerIp">{{ dtData.warehousingOrder }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="出库日期：">
            <!-- <el-date-picker
              v-model="dtData.warehousingDate"
              type="date"
              placeholder="选择日期"
              class="headerIp"
            ></el-date-picker>-->
            <div class="headerIp">{{ dtData.warehousingDate |formatTDate }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="收货地址：">
            <!-- <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:510px;"></el-input> -->
            <div
              class="headerIp ellipsis"
              style="width:510px;"
              :title="dtData.receiptAddress"
            >{{ dtData.receiptAddress }}</div>
          </el-form-item>
        </el-form>
      </div>
    </el-header>
    <!-- 头部表单结束 -->

    <!-- table列表开始 -->
    <el-main>
      <!-- table区域开始 -->
      <div @click.stop="1==1">
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
                <span @click="menuBar($event,scope.$index)">
                  {{ scope.$index+1 }}
                  <!-- <i class="el-icon-setting"></i> -->
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="包型代码" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>包型代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.materialCode.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="包型名称" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>包型名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.materialName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="colorSolutionName" label="配色方案" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>配色方案</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.colorSolutionName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="spec" label="规格型号">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div @click="chengenum($event,scope.row,'spec')">
                <div class="tableTd">{{ scope.row.spec.value }}</div>
              </div>
            </template>
          </el-table-column>

          <!-- 新增颜色 -->
          <el-table-column prop="colorName" label="颜色">
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.colorName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="defaultWarehouseName" label="出货仓库">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>出货仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.defaultWarehouseName.value }}</div>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="PaidIn" label="实发数量">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>实发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.PaidIn.value }}</div>
              </div>
            </template>
          </el-table-column>
          <!-- 新增账存数量 -->
          <el-table-column prop="account" label="可用数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可用数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.account.value }}</div>
            </template>
          </el-table-column>

          <!-- 欠货数量 -->
          <el-table-column prop="missNum" label="欠货数量">
            <template slot="header">
              <span class="tableHeader">
                <span>欠货数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.missNum.value}}</div>
            </template>
          </el-table-column>

          <!-- 新增应发数量 -->
          <el-table-column prop="ShouldbeSsentNum" label="可发数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.ShouldbeSsentNum.value }}</div>
            </template>
          </el-table-column>

          <!-- 新增单位 -->
          <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.warehouseUnitName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.unitPrice.value }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="amount" label="金额">
            <template slot="header">
              <span class="tableHeader">
                <span>金额</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.amount.value }}</div>
            </template>
          </el-table-column>

          <!-- 新增已发数量 -->
          <!-- <el-table-column prop="sentNum" label="已发数量">
            <template slot="header">
              <span class="tableHeader">
                <span>已发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{scope.row.sentNum.value}}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="baseUnitName" label="基本单位名称">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.baseUnitName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitNum" label="基本单位可发数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位可发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{ scope.row.baseUnitNum.value }}</div>
            </template>
          </el-table-column>

          <!-- 新增基本单位实发数量 -->
          <!-- <el-table-column prop="baseUnitNumActualSent" label="基本单位实发数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位实发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{scope.row.baseUnitNumActualSent.value}}</div>
            </template>
          </el-table-column>-->
          <!-- 新增基本单位已发数量 -->
          <!-- <el-table-column prop="baseUnitNumAlreadySent" label="基本单位已发数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位已发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'spec')"
              >{{scope.row.baseUnitNumAlreadySent.value}}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="remark" label="备注" width="280">
            <template slot="header">
              <span class="tableHeader">
                <span class="start" />
                <span>备注</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                :class="{validStyle:scope.row.remark.valid}"
                @click="chengenum($event,scope.row,'remark')"
              >
                <div v-show="!scope.row.remark.isShow" class="tableTd">
                  <!-- <el-tooltip
                    class="item"
                    effect="light"
                    :content="scope.row.remark.value"
                    :open-delay="300"
                    placement="top-end"
                    v-if="scope.row.remark.value.length>=20"
                  >
                    <div class="ellipsis">{{scope.row.remark.value}}</div>
                  </el-tooltip>-->
                  <el-popover
                    v-if="scope.row.remark.value!=null&&scope.row.remark.value.length>=20"
                    placement="top-start"
                    trigger="hover"
                    :content="scope.row.remark.value"
                  >
                    <div slot="reference" class="ellipsis">{{ scope.row.remark.value }}</div>
                  </el-popover>
                  <div
                    v-if="scope.row.remark.value!=null&&scope.row.remark.value.length<20"
                    class="ellipsis"
                  >{{ scope.row.remark.value }}</div>
                </div>
                <el-input
                  v-show="scope.row.remark.isShow"
                  :id="scope.row.id+'___remark'"
                  v-model="scope.row.remark.value"
                  size="mini"
                />
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
      <!-- table区域结束 -->
    </el-main>
    <!-- table列表结束 -->

    <!-- 页脚开始 -->
    <div id="elfooter">
      <el-form label-width="76px" class="FormInput" inline>
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{ dtData.operatorName }}</div>
          </div>
        </el-form-item>

        <el-form-item label="仓管员：">
          <!-- <div class="falseIp" v-if="selectRow.auditStatus == 2">{{dtData.whAdminName}}</div> -->
          <div class="falseIp">{{ dtData.whAdminName }}</div>
        </el-form-item>

        <el-form-item label="发货员：">
          <div class="falseIp">{{ dtData.sendName }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>
        <el-form-item label="审核结果：">
          <div class="falseIp">
            <span v-if="dtData.auditStatus==-1" />
            <span v-if="dtData.auditStatus==0">待审核</span>
            <span v-if="dtData.auditStatus==1">未通过</span>
            <span v-if="dtData.auditStatus==2">通过</span>
            <span v-if="dtData.auditStatus==3">撤销审核</span>
          </div>
        </el-form-item>

        <el-form-item label="审核时间：">
          <div class="falseIp">{{ dtData.auditTime }}</div>
        </el-form-item>
      </el-form>
    </div>
    <!-- 页脚结束 -->

    <!-- 查看弹框开始 -->
    <!-- <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit" title="销售出库单查询"></InboundOrder> -->
    <!-- 查看弹框结束 -->

    <!-- 单据号弹框开始 -->
    <!-- <DocumentNo ref="DocumentNo" @OnBtnSaveSubmit="OnBtnSaveDocument" title="销售订单选择"></DocumentNo> -->
    <!-- 单据号弹框结束 -->
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from "./components/InboundOrder";
// import DocumentNo from "./components/DocumentNo";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import BigNumber from "bignumber.js";

export default {
  name: "HHHXSCK",
  filters: {
    formatTDate: value => {
      if (value == "") {
        return "";
      }
      return formatDate(value);
    }
  },
  components: {
    Pagination
    // InboundOrder,
    // DocumentNo
  },
  data() {
    return {
      popoverStyle: {
        left: "500px",
        top: "400px",
        width: "800px",
        height: "300px"
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
        WarehousingType: "", // 客户名称id
        customerName: "", // 客户名称
        warehousingDate: new Date(), // 出库日期
        warehousingOrder: "", // 编号
        orderTypeId: "", // 订单类型
        orderNo: "", // 单据号
        orderId: "", // 单据号ID
        receiptAddress: "", // 收货地址
        operatorName: "", // 制单人
        operatorId: "",
        whAdminId: "", // 仓管员
        whAdminName: "",
        sendId: "", // 发货员
        sendName: "",
        // receiptName: "", //收货员
        // receiptId: "",
        auditName: "", // 审核人
        auditId: "",
        auditStatus: -1, // 状态
        auditTime: "" // 审核时间
      },
      dtDataRules: {
        WarehousingType: [
          { required: true, message: "请选择客户名称", trigger: "change" }
        ],
        warehousingDate: [
          {
            type: "date",
            required: true,
            message: "请选择日期",
            trigger: "change"
          }
        ],
        warehousingOrder: [{ required: true, message: "获取编号失败" }],
        orderNo: [{ required: true, message: "请选择单据" }]
      },
      tableHeight: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: {},
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], // 物料信息
      warehouseData: [], // 仓库信息
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            // 包型代码
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          materialName: {
            // 包型名称
            value: "",
            isShow: false,
            valid: false
          },
          colorSolutionName: {
            // 配色方案
            value: "",
            isShow: false,
            valid: false
          },
          spec: {
            // 规格型号
            value: "",
            isShow: false,
            valid: false
          },
          colorName: {
            // 颜色
            value: "",
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            // 出货仓库
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            // 单位
            value: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
            // 实发数量
            value: "",
            isShow: false,
            valid: false
          },
          account: {
            // 账存数量
            value: "",
            isShow: false,
            valid: false
          },
          unitPrice: {
            // 单价
            value: "",
            isShow: false,
            valid: false
          },
          amount: {
            // 金额
            value: "",
            isShow: false,
            valid: false
          },
          ShouldbeSsentNum: {
            // 应发数量
            value: "",
            isShow: false,
            valid: false
          },
          sentNum: {
            // 已发数量
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitName: {
            // 基本单位名称
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNum: {
            // 基本单位应发数量
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumActualSent: {
            // 基本单位实发数量
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumAlreadySent: {
            // 基本单位已发数量
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
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
      btnAip: "", // 按钮权限
      OtherData: {},
      realNameOptions: [], // 收货员列表
      addControl: true, // 是否显示新增按钮
      TabArr: ["defaultWarehouseName", "PaidIn"],
      materialItem: {},
      connectionData: [], // 客户名称数据
      mountedState: false
    };
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
    // 获取按钮权限
    // this.btnAip = getBtnCtr(
    //   this.$route.path,
    //   this.$store.getters.userPermission
    // );
  },
  async mounted() {
    this.runing();
  },
  async activated() {
    if (this.atob(this.$route.query.id) == this.OtherData.id) {
      return;
    }
    this.runing();
  },
  deactivated() {
    // window.removeEventListener("keydown", this.onKeydown);
  },
  methods: {
    async runing() {
      if (this.mountedState == true) {
        return;
      }
      this.fullscreenLoading = true;
      this.setStyle(); // 设置样式
      this.warehouseData = await this.getWarehouseData(); // 仓库数据
      if (this.$route.query.id) {
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.getMainList(this.OtherData.id);
      }

      this.mountedState = false;
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = document.getElementById("btnHeader").offsetHeight; // 按钮高度
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        this.tableHeight = b - h - f - 30 - btn;
      });
    },
    findCheck(event) {
      var data = {};
      for (var h = 0; h < this.tableData.length; h++) {
        for (var i in this.tableData[h]) {
          if (typeof this.tableData[h][i] === "object") {
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
            document
              .getElementById(this.tableData[index].id + "___" + name)
              .focus();
            document
              .getElementById(this.tableData[index].id + "___" + name)
              .select();
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
    // 设置Tab切换结束
    resetData() {
      // 初始化数据
      this.getCode();
      this.resetDeafultData();
    },
    resetDeafultData() {
      // 重置数据
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = [];
      this.tableData = this.setTable(30); // 添加30行
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.WarehousingType = "";
      this.dtData.warehousingDate = new Date();
      this.dtData.customerName = "";
      this.dtData.warehousingOrder = "";
      this.dtData.orderTypeId = "";
      this.dtData.orderNo = "";
      this.dtData.orderId = "";
      this.dtData.receiptAddress = "";
      this.dtData.operatorName = "";
      this.dtData.operatorId = "";
      this.dtData.whAdminId = "";
      this.dtData.whAdminName = "";
      this.dtData.sendId = "";
      this.dtData.sendName = "";
      this.dtData.auditName = "";
      this.dtData.auditId = "";
      this.dtData.auditStatus = -1;
      this.dtData.auditTime = "";
    },
    getWarehouseData() {
      // 查看仓库档案信息
      return new Promise(function(reslove, reject) {
        var reqObject;
        reqObject = RequestObject.CreateGetObject(false, 0, 0, []);
        request({
          url: "/basicset/api/TBMWarehouseFile",
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
            // this.warehouseData = res.data;
            reslove(res.data);
          }
        });
      });
    },
    setTable(num) {
      // this.fullscreenLoading = true;
      // 初始化30条数据
      var listArr = [];
      for (var i = 0; i < num; i++) {
        var list = {};
        for (var j in this.tableData2[0]) {
          if (j == "id") {
            list["id"] = newGuid();
          } else {
            if (typeof this.tableData2[0][j] === "object") {
              list[j] = {};
              for (var k in this.tableData2[0][j]) {
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
      // setTimeout(()=>{
      //   this.fullscreenLoading = false;
      // },1000)
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
    getSummaries(param) {
      // table总计
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
              return BigNumber(prev)
                .plus(curr)
                .toNumber();
            } else {
              return prev;
            }
          }, 0);
          if (index == 7 || index == 8 || index == 10 || index == 13) {
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
      // 双击显示input,监听input数据变化
      event.stopPropagation();
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    getMainList(id) {
      request({
        url: "/warehouse/api/TWMSalesMain/GetWholeMainData",
        method: "GET",
        params: { RequestObject: id }
      }).then(res => {
        if (res.code === 0) {
          this.OnBtnSaveSubmit(res.data, 1);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    OnBtnSaveSubmit(row, state) {
      // 根据ID获取数据
      this.selectRow = this.cloneObject(row);
      this.dtData.customerName = row.customerName;
      // this.dtData.WarehousingType = row.customerId;
      this.dtData.warehousingDate =
        row.whSendDate != null ? new Date(row.whSendDate.split("T")[0]) : "";
      this.dtData.warehousingOrder = row.whSendOrder;
      this.dtData.orderNo = row.saleOrder;
      this.dtData.orderId = row.sourceId;
      // this.auditStatus = row.auditStatus;
      this.dtData.receiptAddress = row.receiptAddress;

      this.dtData.operatorName = row.operatorName;
      this.dtData.operatorId = row.operatorId;
      this.dtData.whAdminName = row.whAdminName;
      this.dtData.whAdminId = row.whAdminId;
      this.dtData.sendName = row.sendName;
      this.dtData.sendId = row.sendId;

      this.dtData.auditName = row.auditName;
      this.dtData.auditId = row.auditId;
      this.dtData.auditStatus = row.auditStatus;
      this.dtData.auditTime =
        row.auditTime != null ? row.auditTime.split("T")[0] : "";
      if (!state) {
        // var rqs = RequestObject.LonginBookObject(false, 0, 0, row.id);
        // this.fullscreenLoading = true;
        request({
          url: "/warehouse/api/TWMSalesMain/GetWholeMainData",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
          // setTimeout(() => {
          //   this.fullscreenLoading = false;
          // }, 200);
          this.fullscreenLoading = false;
          if (res.code === 0) {
            this.setTableData(res.data.childList);
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          }
        });
      } else {
        this.setTableData(row.childList);
      }
    },
    setCurrData(data) {
      // 设置编辑时候的日记数据
      var childList = [];
      var _LogName = "";
      this.cloneTable.map(item => {
        var param = {
          id: 0,
          mainId: 0,
          materialId: parseFloat(item.materialCode.key) || "",
          warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
          actualNum: parseFloat(item.PaidIn.value) || ""
        };
        if (item.unitPrice !== "" || item.unitPrice !== null) {
          param.unitPrice = item.unitPrice.value;
        }
        if (item.amount !== "" || item.amount !== null) {
          param.amount = item.amount.value;
        }
        if (item.remark !== "" || item.remark !== null) {
          param.remark = item.remark.value;
        }
        _LogName += `包型:${item.materialName.value} 销售出库 ${
          item.PaidIn.value
        }${
          item.warehouseUnitName.value === null
            ? ""
            : item.warehouseUnitName.value
        } 到 ${item.defaultWarehouseName.value}`;

        param._LogName = _LogName;
        childList.push(param);
      });
      var currData = {
        id: this.selectRow.id,
        whSendType: this.selectRow.whSendType,
        whSendDate:
          this.selectRow.whSendDate !== null || this.selectRow.whSendDate !== ""
            ? this.selectRow.whSendDate.split("T")[0]
            : "",
        whSendOrder: this.selectRow.whSendOrder,
        sourceId: this.selectRow.sourceId,
        childList: childList
      };

      if (
        this.selectRow.whAdminId !== "" &&
        this.selectRow.whAdminId !== null
      ) {
        currData.receiptId = this.dtData.whAdminId;
      }
      if (this.selectRow.sendId !== "" && this.selectRow.sendId !== null) {
        currData.sendId = this.dtData.sendId;
      }
      if (
        this.selectRow.receiptAddress !== "" &&
        this.selectRow.receiptAddress !== null
      ) {
        currData.receiptAddress = this.dtData.receiptAddress;
      }

      this.PostDataEdit = currData;
    },
    setTableData(dt) {
      // 将数据导入table
      // console.log(dt);
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            // 包型代码
            value: item.packageCode,
            key: "",
            isShow: false,
            valid: false
          },
          materialName: {
            // 包型名称
            value: item.packageName,
            isShow: false,
            valid: false
          },
          colorSolutionName: {
            // 配色方案
            value: item.colorSolutionName,
            isShow: false,
            valid: false
          },
          spec: {
            // 规格型号
            value: item.spec,
            isShow: false,
            valid: false
          },
          colorName: {
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            // 出货仓库
            value: "",
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            // 单位
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          PaidIn: {
            // 实发数量
            value: item.actualNum,
            isShow: false,
            valid: false
          },
          account: {
            // 账存数量
            value: item.availableNum,
            isShow: false,
            valid: false
          },
          missNum: {
            // 欠货数量
            value: item.missNum,
            isShow: false,
            valid: false
          },
          ShouldbeSsentNum: {
            // 应发数量
            value: item.shouldNum,
            isShow: false,
            valid: false
          },
          unitPrice: {
            // 单价
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          amount: {
            // 金额
            value: item.amount,
            isShow: false,
            valid: false
          },
          sentNum: {
            // 已发数量
            value: item.alreadyNum,
            isShow: false,
            valid: false
          },
          baseUnitName: {
            // 基本单位名称
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          baseUnitNum: {
            // 基本单位应发数量
            value: item.baseUnitShouldNum,
            isShow: false,
            valid: false
          },
          baseUnitNumActualSent: {
            // 基本单位实发数量
            value: item.baseUnitActualNum,
            isShow: false,
            valid: false
          },
          baseUnitNumAlreadySent: {
            // 基本单位已发数量
            value: item.baseUnitAlreadyNum,
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            value: item.remark,
            isShow: false,
            valid: false
          }
        };
        this.warehouseData.map(data => {
          if (list.defaultWarehouseName.key == data.id) {
            // console.log(data.warehouseName)
            list.defaultWarehouseName.value = data.warehouseName;
          }
        });
        listArr.push(list);
      });
      // this.cloneTable = [...listArr];
      // this.setCurrData(this.cloneTable);
      setTimeout(() => {
        this.tableData = [...listArr];
      }, 0);
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 200);
    }
  }
};
</script>
<style lang="scss" scoped>
#SalesReceipt /deep/ {
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
    flex-wrap: wrap;
  }
  .footerCenter {
    width: 1150px;
    margin: 0 auto;
  }
  .has-gutter tr td {
    text-align: center;
  }
  .formItem {
    margin-right: 20px;
    // width:283px;
  }
  .el-form-item__label {
    padding-right: 4px;
  }
  .el-table {
    overflow: visible !important;
  }
  .headerIp {
    width: 200px;
  }
  .el-select .el-input.is-disabled .el-input__inner,
  .el-input.is-disabled .el-input__icon {
    cursor: default;
    background: #fff;
    color: rgb(0, 0, 0);
  }
  .disableType i {
    border-top: 1px solid rgb(220, 223, 230);
    border-bottom: 1px solid rgb(220, 223, 230);
    height: 32px;
  }
  .middleWidth {
    width: 1240px;
  }
  @media screen and (max-width: 1510px) {
    .middleWidth {
      width: 929px;
    }
  }
  .el-table {
    overflow: visible !important;
  }
  .el-main {
    padding-bottom: 10px;
  }
}
@import "@/styles/receipts.scss";
</style>
