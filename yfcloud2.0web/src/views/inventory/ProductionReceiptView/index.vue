<template>
  <el-container
    id="ProductionReceipt"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    @click.native="listenClick"
  >
    <!-- 头部按钮区开始 -->

    <div class="btnHeader" id="btnHeader">
      <h1>生产入库单</h1>
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
          <!-- <el-form-item class="formItem disableType" label="供应商：">
            <div class="headerIp">{{ dtData.customerName }}</div>
          </el-form-item>-->

          <el-form-item class="formItem" label="源单类型：">
            <div>
              <span>生产订单</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="单据号：" label-width="62px">
            <!-- <el-input
              v-model="dtData.orderNo"
              class="headerIp"
              readonly
              @click.native="chooseNumber"
            >
              <i slot="suffix" class="el-input__icon el-icon-search" />
            </el-input>-->
            <div>{{dtData.orderNo}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="状态：" label-width="48px">
            <div>
              <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待入库</span>
              <span v-if="dtData.auditStatus==0">入库待审核</span>
              <span v-if="dtData.auditStatus==1">审核未通过</span>
              <span v-if="dtData.auditStatus==2">入库完成</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="编号：" label-width="48px">
            <div>{{ dtData.warehousingOrder }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="入库日期：">
            <div>{{warehousingDateForMat}}</div>
          </el-form-item>

          <!-- <el-form-item class="formItem" label="采购方式：">
            <el-select v-model="dtData.purchasingMethod" placeholder="请选择" style="width:200px">
              <el-option label="现购" :value="0" />
              <el-option label="赊购" :value="1" />
            </el-select>
          </el-form-item>-->

          <!-- <el-form-item class="formItem" label="收货地址：">
            <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:510px;"></el-input>
          </el-form-item>-->
        </el-form>
      </div>
    </el-header>
    <!-- 头部表单结束 -->

    <!-- table列表开始 -->
    <el-main>
      <!-- table区域开始 -->
      <div>
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
                <span>{{ scope.$index+1 }}</span>
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
              <div class="tableTd">{{ scope.row.materialCode.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="包型名称" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>包型名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.materialName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="colorSolutionName" label="配色方案">
            <template slot="header">
              <span class="tableHeader">
                <span>配色方案</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.colorSolutionName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="colorName" label="颜色">
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.colorName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="spec" label="规格型号">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.spec.value }}</div>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="defaultWarehouseName" label="收货仓库" width="120">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>收货仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <EditSelect
                v-model="scope.row.defaultWarehouseName"
                :data="warehouseData"
                :isShowFlag="isShowFlag"
                label="warehouseName"
              ></EditSelect>
            </template>
          </el-table-column>

          <el-table-column prop="PaidIn" label="实收数量">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>实收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <EditInput
                v-model="scope.row.PaidIn"
                :type="2"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>
          <el-table-column prop="ShouldbeSReceiveNum" label="可收数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.ShouldbeSReceiveNum.value }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.warehouseUnitName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{ scope.row.unitPrice.value }}</div> -->
              <EditInput
                v-model="scope.row.unitPrice"
                :type="3"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>

          <el-table-column prop="amount" label="金额">
            <template slot="header">
              <span class="tableHeader">
                <span>金额</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{ scope.row.amount.value }}</div> -->
              <EditInput
                v-model="scope.row.amount"
                :type="4"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>

          <!-- <el-table-column prop="workshopName" label="生产车间">
            <template slot="header">
              <span class="tableHeader">
                <span>生产车间</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.workshopName.value }}</div>
            </template>
          </el-table-column>-->

          <!-- <el-table-column prop="sourceNumber" label="源单单号" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>源单单号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.sourceNumber.value }}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="baseUnitName" label="基本单位名称" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitName.value }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="baseUnitNum" label="基本单位可收数量" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位可收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitNum.value }}</div>
            </template>
          </el-table-column>
          <!-- <el-table-column prop="baseUnitNumActualReceived" label="基本单位实收数量" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位实收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitNumActualReceived.value }}</div>
            </template>
          </el-table-column>-->

          <!-- <el-table-column prop="productionDate" label="生产/采购日期">
            <template slot="header">
              <span class="tableHeader">
                <span>生产/采购日期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'productionDate')"
              >{{ scope.row.productionDate.value }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="shelfLife" label="保质期（天）">
            <template slot="header">
              <span class="tableHeader">
                <span>保质期（天）</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                @click="chengenum($event,scope.row,'shelfLife')"
              >{{ scope.row.shelfLife.value }}</div>
            </template>
          </el-table-column>-->

          <!-- <el-table-column prop="productionDate" label="生产/采购日期" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>生产/采购日期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.productionDate.value }}</div>
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
              <EditInput
                v-model="scope.row.remark"
                type="remark"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
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

        <!-- <el-form-item label="仓管员：">
          <div class="falseIp" v-if="selectRow.auditStatus == 2">{{dtData.whAdminName}}</div>
          <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.whAdminId"
            clearable
            filterable
            placeholder="请选择"
            style="width:300px;"
          >
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>-->

        <el-form-item label="收货员：">
          <div class="falseIp">{{ dtData.receiptName }}</div>
          <!-- <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.receiptId"
            clearable
            filterable
            placeholder="请选择"
            style="width:300px;"
          >
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            />
          </el-select>-->
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
    <!-- <InboundOrder ref="InboundOrder" title="生产入库单查询" @OnBtnSaveSubmit="OnBtnSaveSubmit" /> -->
    <!-- 查看弹框结束 -->

    <!-- 单据号弹框开始 -->
    <!-- <DocumentNo ref="DocumentNo" title="生产订单选择" @OnBtnSaveSubmit="OnBtnSaveDocument" /> -->
    <!-- 单据号弹框结束 -->
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
  getFloat
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from "./components/InboundOrder";
// import DocumentNo from "./components/DocumentNo";
import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import BigNumber from "bignumber.js";

export default {
  name: "HHSCRK",
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
    // InboundOrder,
    // DocumentNo,
    EditInput,
    EditSelect
  },
  data() {
    return {
      isShowFlag: true,
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
        customerName: "", // 供应商名称
        warehousingDate: new Date(), // 出库日期
        warehousingOrder: "", // 编号
        orderTypeId: "", // 订单类型
        orderNo: "", // 单据号
        orderId: "", // 单据号ID
        tranSourceId: "", //单据ID
        // receiptAddress: "", // 收货地址
        purchasingMethod: "",
        operatorName: "", // 制单人
        operatorId: "",
        whAdminId: "", // 仓管员
        whAdminName: "",
        // sendId: "", // 发货员
        // sendName: "",
        receiptName: "", //收货员
        receiptId: "",
        auditName: "", // 审核人
        auditId: "",
        auditStatus: -1, // 状态
        auditTime: "" // 审核时间
      },
      dtDataRules: {
        WarehousingType: [
          // { required: true, message: "请选择客户名称", trigger: "change" }
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
            id: "",
            value: "",
            key: "",
            materialId: "",
            isShow: false,
            valid: false
          },
          materialName: {
            // 包型名称
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
          defaultWarehouseName: {
            // 收料仓库
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
            // 实收数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          ShouldbeSReceiveNum: {
            // 应收数量
            value: "",
            isShow: false,
            valid: false
          },
          // batchNumber: {
          //   // 批号
          //   value: "",
          //   isShow: false,
          //   valid: false
          // },
          unitPrice: {
            // 单价
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          amount: {
            // 金额
            id: "",
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
          colorName: {
            // 颜色
            value: "",
            isShow: false,
            valid: false
          },
          workshopName: {
            // 车间
            value: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            // 单位
            value: "",
            isShow: false,
            valid: false
          },
          sourceNumber: {
            // 源单单号
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
            // 基本单位应收数量
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumActualReceived: {
            // 基本单位实收数量
            value: "",
            isShow: false,
            valid: false
          },
          productionDate: {
            // 生产/采购日期
            value: "",
            isShow: false,
            valid: false
          },
          shelfLife: {
            // 保质期（天）
            value: "",
            isShow: false,
            valid: false
          },
          validityPeriod: {
            // 有效期至
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
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
      btnAip: "", // 按钮权限
      OtherData: {},
      realNameOptions: [], // 收货员列表
      addControl: true, // 是否显示新增按钮
      TabArr: [
        "defaultWarehouseName",
        "PaidIn",
        "unitPrice",
        "amount",
        "remark"
      ],
      materialItem: {},
      connectionData: [], // 客户名称数据
      warehousingDateForMat: "",
      mountedState:false,
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
  activated() {
    if (this.atob(this.$route.query.id) == this.OtherData.id) {
      return;
    }
    this.runing();
  },
  mounted() {
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
      this.warehouseData = await this.getWarehouseData(); //仓库数据
      if (this.$route.query.id) {
        // this.OtherData = this.$route.params;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.fullscreenLoading = true;
        this.getMainList(this.OtherData.id);
      }

      this.mountedState = false;
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = document.getElementById("btnHeader").offsetHeight; //按钮高度
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
    // 设置Tab切换开始,
    defaultAll() {
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
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
      // setTimeout(()=>{
      //   this.fullscreenLoading = false;
      // },1000)
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
          if (index == 7 || index == 8 || index == 11||index==13) {
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
    listenClick() {
      this.popoverState = false;
      this.defaultAll();
      // this.doDefault(this.doItem);
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
        if (typeof data[i] === "object") {
          if (data[i]["isShow"]) {
            this.$set(data[i], `isShow`, false);
          }
        }
      }
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    getMainList(id) {
      request({
        url: "/warehouse/api/TWMProductionWhMain/GetWholeMainData",
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
      this.fullscreenLoading = true;
      // console.log(row);
      this.selectRow = this.cloneObject(row);
      // this.dtData.customerName = row.supplierName;
      // this.dtData.WarehousingType = row.customerId;
      // if (this.selectRow.auditStatus === 2) {
      //   this.isShowFlag = false;
      // } else {
      //   this.isShowFlag = true;
      // }
      this.isShowFlag = false;
      this.dtData.warehousingDate =
        row.warehousingDate != null
          ? new Date(row.warehousingDate.split("T")[0])
          : "";
      this.warehousingDateForMat = row.warehousingDate.split("T")[0];
      this.dtData.warehousingOrder = row.warehousingOrderNo;
      this.dtData.orderNo = row.sourceCode;
      this.dtData.orderId = "";
      this.dtData.tranSourceId = row.sourceId;
      // this.auditStatus = row.auditStatus;
      // this.dtData.receiptAddress = row.receiptAddress;

      this.dtData.operatorName = row.operatorName;
      this.dtData.operatorId = row.operatorId;
      this.dtData.whAdminName = row.whAdminName;
      this.dtData.whAdminId = row.whAdminId;
      this.dtData.receiptName = row.receiptName;
      this.dtData.receiptId = row.receiptId;

      this.dtData.auditName = row.auditName;
      this.dtData.auditId = row.auditId;
      this.dtData.auditStatus = row.auditStatus;
      this.dtData.auditTime =
        row.auditTime != null ? row.auditTime.split("T")[0] : "";
      if (!state) {
        request({
          url: "/warehouse/api/TWMProductionWhMain/GetWholeMainData",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
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
        this.setTableData(row.childList, row);
      }
    },
    setCurrData(data) {
      // 设置编辑时候的日记数据
      var childList = [];
      this.cloneTable.map(item => {
        var _LogName = "";
        var param = {
          id: item.id,
          mainId: 0,
          materialId: parseFloat(item.materialCode.materialId) || "",
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
        _LogName += `物料:${item.materialName.value} 生产入库库 ${
          item.PaidIn.value
        }${
          item.warehouseUnitName.value === null
            ? ""
            : item.warehouseUnitName.value
        } 到 ${item.defaultWarehouseName.value}`;

        param._LogName = _LogName;
        childList.push(param);
      });

      // var currData = {
      //   id: this.selectRow.id,
      //   warehousingType: this.selectRow.warehousingType,
      //   warehousingDate:
      //     this.selectRow.warehousingDate !== null ||
      //     this.selectRow.warehousingDate !== ""
      //       ? this.selectRow.warehousingDate.split("T")[0]
      //       : "",
      //   warehousingOrderNo: this.selectRow.warehousingOrderNo,
      //   sourceId: this.selectRow.sourceId,
      //   childList: childList
      // };
      var currData = {
        id: this.selectRow.id,
        warehousingType: this.selectRow.warehousingType,
        warehousingDate:
          this.selectRow.warehousingDate !== null ||
          this.selectRow.warehousingDate !== ""
            ? this.selectRow.warehousingDate.split("T")[0]
            : "",
        warehousingOrderNo: this.selectRow.warehousingOrderNo,
        sourceId: this.selectRow.sourceId,
        companyId: 0,
        deleteFlag: 0,
        number: 0,
        childList: childList
      };

      // if (
      //   this.selectRow.whAdminId !== "" &&
      //   this.selectRow.whAdminId !== null
      // ) {
      //   currData.whAdminId = this.dtData.whAdminId;
      // }
      if (
        this.selectRow.receiptId !== "" &&
        this.selectRow.receiptId !== null
      ) {
        currData.receiptId = this.dtData.receiptId;
      }

      this.PostDataEdit = currData;
    },
    setTableData(dt, row) {
      // 将数据导入table
      // console.log(dt);
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: item.id,
          editState: false,
          rowIndex: 0,
          materialCode: {
            // 物料代码
            id: newGuid(),
            value: item.packageCode,
            key: item.packageId,
            materialId: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            // 物料名称
            value: item.packageName,
            isShow: false,
            valid: false
          },
          spec: {
            // 规格型号
            value: item.spec,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            // 收料仓库
            id: newGuid(),
            value: "",
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          PaidIn: {
            // 实收数量
            id: newGuid(),
            value: item.actualNum,
            isShow: false,
            valid: false
          },
          ShouldbeSReceiveNum: {
            // 应收数量
            value: item.transNum,
            isShow: false,
            valid: false
          },
          // batchNumber: {
          //   // 批号
          //   value: "",
          //   isShow: false,
          //   valid: false
          // },
          unitPrice: {
            // 单价
            id: newGuid(),
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          amount: {
            // 金额
            id: newGuid(),
            value: item.amount,
            isShow: false,
            valid: false
          },
          colorSolutionName: {
            // 配色方案
            value: item.colorSolutionName,
            isShow: false,
            valid: false
          },
          colorName: {
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },
          workshopName: {
            // 车间
            value: item.workshopName,
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            // 单位
            value: item.produceUnitName,
            isShow: false,
            valid: false
          },
          sourceNumber: {
            // 源单单号
            value: this.dtData.orderNo,
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
            // 基本单位应收数量
            value: item.baseUnitTransNum,
            isShow: false,
            valid: false
          },
          baseUnitNumActualReceived: {
            // 基本单位实收数量
            value: item.baseUnitActualNum,
            isShow: false,
            valid: false
          },
          productionDate: {
            // 生产/采购日期
            // value: row.warehousingDate != null ? row.warehousingDate.split("T")[0] : "",
            value: "",
            isShow: false,
            valid: false
          },
          shelfLife: {
            // 保质期（天）
            value: "",
            isShow: false,
            valid: false
          },
          validityPeriod: {
            // 有效期至
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            id: newGuid(),
            value: item.remark,
            isShow: false,
            valid: false
          }
        };
        this.warehouseData.map(data => {
          if (list.defaultWarehouseName.key == data.id) {
            list.defaultWarehouseName.value = data.warehouseName;
          }
        });
        listArr.push(list);
      });
      this.cloneTable = [...listArr];
      this.setCurrData(this.cloneTable);
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
#ProductionReceipt /deep/ {
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
    margin-right: 60px;
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
  #elfooter .el-input__inner {
    border: 0px;
    border-radius: 0px;
    border-bottom: 1px solid #dcdfe6;
  }
  .middleWidth {
    width: 1240px;
  }
  @media screen and (max-width: 1510px) {
    .middleWidth {
      width: 929px;
    }
  }
  #tableData input {
    padding: 0px 10px;
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

