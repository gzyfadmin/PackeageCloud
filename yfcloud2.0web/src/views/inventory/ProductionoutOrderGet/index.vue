<template>
  <el-container
    id="ProductionoutOrder"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    @click.native="listenClick"
  >
    <!-- 头部按钮区开始 -->
    <div class="btnHeader" id="btnHeader">
      <h1>生产出库单</h1>
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

          <!-- <el-form-item class="formItem" label="源单类型：">
            <div class="headerIp">
              <span>普通领料单</span>
            </div>
          </el-form-item>-->

          <el-form-item class="formItem" label="源单类型：">
            <div class="falseIptop">生产领料单</div>
          </el-form-item>

          <el-form-item class="formItem" label="单据号：" label-width="62px">
            <div class="falseIptop">{{dtData.orderNo?dtData.orderNo:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="状态：" label-width="48px">
            <div class="falseIptop">
              <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待出库</span>
              <span v-if="dtData.auditStatus==0">待审核</span>
              <span v-if="dtData.auditStatus==1">未通过</span>
              <span v-if="dtData.auditStatus==2">通过</span>
            </div>
          </el-form-item>

          <!-- <el-form-item class="formItem" label="编号：" prop="warehousingOrder">
            <div class="headerIp">{{ dtData.warehousingOrder }}</div>
          </el-form-item>-->

          <el-form-item class="formItem" label="编号：" label-width="48px">
            <div class="falseIptop">{{dtData.warehousingOrder?dtData.warehousingOrder:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="出库日期：">
            <div class="falseIptop">{{dtData.applyDate | formatTDate}}</div>
          </el-form-item>

          <!-- <el-form-item class="formItem" label="出库日期：" prop="applyDate">
            <el-date-picker
              v-model="dtData.applyDate"
              type="date"
              placeholder="选择日期"
              class="headerIp"
              :readonly="selectRow.auditStatus === 2"
            />
          </el-form-item>-->

          <!-- <el-form-item class="formItem cursor" label="单据号："  prop="orderNo">
            <el-input
              v-model="dtData.orderNo"
              class="headerIp"
              readonly
              @click.native="chooseNumber"
            >
              <i slot="suffix" class="el-input__icon el-icon-search" />
            </el-input>
          </el-form-item>-->

          <!-- <el-form-item class="formItem" label="发货地址：">
            <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:510px;" :readonly="selectRow.auditStatus === 2"></el-input>
          </el-form-item>-->

          <el-form-item class="formItem" label="收货地址：" prop="sourceNo">
            <div
              class="falseIptop ellipsis"
              style="max-width:400px;"
              :title="dtData.receiptAddress"
            >{{dtData.receiptAddress}}</div>
            <!-- <el-popover
              class="item"
              trigger="hover"
              placement="top-start"
              :content="dtData.receiptAddress"
            >
              <div
                class="ellipsis falseIptop"
                slot="reference"
              >{{dtData.receiptAddress?dtData.receiptAddress:"&nbsp;"}}</div>
            </el-popover>-->
          </el-form-item>

          <!-- <el-form-item class="formItem" label="采购方式：">
            <el-select v-model="dtData.purchasingMethod" placeholder="请选择" style="width:200px">
              <el-option label="现购" :value="0" />
              <el-option label="赊购" :value="1" />
            </el-select>
          </el-form-item>-->

          <!-- <el-form-item class="formItem" label="发货地址：">
            <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:510px;"></el-input>
          </el-form-item>-->
        </el-form>
      </div>
    </el-header>
    <!-- 头部表单结束 -->

    <!-- table列表开始 -->
    <el-main>
      <!-- 物料弹框开始 -->
      <div v-show="popoverState" class="findBox" :style="popoverStyle" @click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="materialCode" label="物料代码" width="180" />
          <el-table-column prop="materialName" label="物料名称" />
          <el-table-column prop="spec" label="规格型号" />
          <el-table-column prop="baseUnitName" label="基本计量单位" width="100" />
          <el-table-column prop="materialTypeName" label="物料分类" />
          <el-table-column prop="colorName" label="颜色" />
          <el-table-column prop="defaultWarehouseName" label="默认仓库" />
          <el-table-column prop="shelfLife" label="保质期（天）" width="100" />
        </el-table>
        <Pagination
          :page-index="pageIndex"
          :total-count="totalCount"
          :page-size="pageSize"
          @pagination="pagination"
        />
      </div>
      <!-- 物料弹框结束 -->

      <!-- 序号弹框开始 -->
      <div v-show="menuState" class="findBox" :style="menuStyle">
        <ul class="menuUl">
          <!-- <li @click="doMenuAdd">增加行</li> -->
          <li @click="doMenuDel">删除行</li>
        </ul>
      </div>
      <!-- 序号弹框结束 -->

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
          :row-class-name="tableRowClassName"
        >
          <el-table-column label="序号" width="70">
            <template slot="header">
              <span class="tableHeader">
                <span>序号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="textAlign">
                <span>
                  <!-- @click.stop="menuBar($event,scope.$index)" -->
                  {{ scope.$index+1 }}
                  <!-- <i class="el-icon-setting" /> -->
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="物料代码" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.materialCode.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="物料名称" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>物料名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.materialName.value }}</div>
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

          <el-table-column prop="defaultWarehouseName" label="发货仓库" width="120">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>发货仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <EditSelect
              @change="getWarehouseAmount()"
                v-model="scope.row.defaultWarehouseName"
                @clickEvent="clickEvent"
                :data="warehouseData"
                :isShowFlag="false"
              ></EditSelect>-->

              <div :class="{validStyle:scope.row.defaultWarehouseName.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.defaultWarehouseName.isShow"
                >{{scope.row.defaultWarehouseName.value}}</div>
                <el-select
                  v-show="scope.row.defaultWarehouseName.isShow"
                  v-model="scope.row.defaultWarehouseName.key"
                  :id="scope.row.id+'___defaultWarehouseName'"
                  placeholder="请选择"
                  @change="WarehouseChange(scope.row.defaultWarehouseName)"
                >
                  <el-option
                    v-for="item in warehouseData"
                    :key="item.id"
                    :label="item.warehouseName"
                    :value="item.id"
                  ></el-option>
                </el-select>
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
              <EditInput
                v-model="scope.row.PaidIn"
                :type="2"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
            </template>
          </el-table-column>

          <!-- <el-table-column  label="可用数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可用数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
              
              >{{scope.row.ShouldbeSReceiveNum.value}}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="account" label="可用数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可用数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.account.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="ShouldbeSReceiveNum" label="可发数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可发数量</span>
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
              <div
                class="tableTd"
                v-if="scope.row.warehouseUnitName.value"
              >{{ scope.row.warehouseUnitName.value }}</div>
              <div
                class="tableTd"
                v-if="!scope.row.warehouseUnitName.value"
              >{{ scope.row.baseUnitName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <EditInput
                v-model="scope.row.unitPrice"
                :type="3"
                :itemList="scope.row"
                :isShowFlag="false"
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
              <EditInput
                v-model="scope.row.amount"
                :type="4"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
            </template>
          </el-table-column>
          <!-- 
          <el-table-column prop="colorSolutionName" label="配色方案">
            <template slot="header">
              <span class="tableHeader">
                <span>配色方案</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.colorSolutionName.value }}</div>
            </template>
          </el-table-column>-->

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
          <el-table-column prop="baseUnitNum" label="基本单位可发数量" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位可发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitNum.value }}</div>
            </template>
          </el-table-column>
          <el-table-column prop="baseUnitNumActualReceived" label="基本单位实发数量" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位实发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitNumActualReceived.value }}</div>
            </template>
          </el-table-column>

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
                :isShowFlag="false"
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

        <el-form-item label="仓管员：">
          <div>
            <div class="falseIp">{{ dtData.whAdminName }}</div>
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
            style="width:200px;"
          >
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>-->

        <!-- <el-form-item label="发货员：">
          <div v-if="selectRow.auditStatus == 2" class="falseIp">{{ dtData.sendName }}</div>
          <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.sendId"
            clearable
            filterable
            placeholder="请选择"
            style="width:200px;"
          >
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            />
          </el-select>
        </el-form-item>-->

        <el-form-item label="发货员：">
          <div>
            <div class="falseIp">{{ dtData.sendName }}</div>
          </div>
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
    <InboundOrder ref="InboundOrder" title="生产出库单查询" @OnBtnSaveSubmit="OnBtnSaveSubmit" />
    <!-- 查看弹框结束 -->

    <!-- 单据号弹框开始 -->
    <!-- <DocumentNo ref="DocumentNo" title="普通领料单选择" @OnBtnSaveSubmit="OnBtnSaveDocument" /> -->
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
import InboundOrder from "./components/InboundOrder";
import DocumentNo from "./components/DocumentNo";
import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import BigNumber from "bignumber.js";

export default {
  name: "HLDYF",
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
    InboundOrder,
    DocumentNo,
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
        WarehousingType: "", // 客户名称id
        customerName: "", // 供应商名称
        applyDate: new Date(), // 出库日期
        warehousingOrder: "", // 编号
        orderTypeId: "", // 订单类型
        orderNo: "", // 单据号
        orderId: "", // 单据号ID
        tranSourceId: "", //单据ID
        receiptAddress: "", // 发货地址
        purchasingMethod: "",
        operatorName: "", // 制单人
        operatorId: "",
        whAdminId: "", // 仓管员
        whAdminName: "",
        sendId: "", // 发货员
        sendName: "", //发货员

        auditName: "", // 审核人
        auditId: "",
        auditStatus: -1, // 状态
        auditTime: "" // 审核时间
      },
      dtDataRules: {
        WarehousingType: [
          // { required: true, message: "请选择客户名称", trigger: "change" }
        ],
        applyDate: [
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
            // 物料代码
            id: "",
            value: "",
            key: "",
            materialId: "",
            isShow: false,
            valid: false
          },
          materialName: {
            // 物料名称
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
            // 发料仓库
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },

          PaidIn: {
            // 实发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          ShouldbeSReceiveNum: {
            // 应发数量
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
          account: {
            // 金额

            value: "",
            isShow: false,
            valid: false
          },
          // colorSolutionName: {
          //   // 配色方案
          //   value: "",
          //   isShow: false,
          //   valid: false
          // },
          colorName: {
            // 颜色
            value: "",
            isShow: false,
            valid: false
          },
          // workshopName: {
          //   // 车间
          //   value: "",
          //   isShow: false,
          //   valid: false
          // },
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
            // 基本单位应发数量
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumActualReceived: {
            // 基本单位实发数量
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
      realNameOptions: [], // 发货员列表
      addControl: true, // 是否显示新增按钮
      TabArr: [
        "defaultWarehouseName",
        "PaidIn",
        "unitPrice",
        "amount",
        "remark"
      ],
      materialItem: {},
      connectionData: [] // 客户名称数据
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
    if(this.atob(this.$route.query.id)==this.OtherData.id){
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
        this.OnBtnSaveSubmit(this.OtherData);
      }

      this.mountedState = false;
    },
    blurCheck(itemList, item, type, state) {
      //td框的类型（可用于数据校验等）
      switch (type) {
        case 1: //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.getMaterielData(itemList, (this.pageIndex = 1));
          }
          break;
        case 2: //实发数量
          this.PaidInBlur(itemList); //验证实发数量
          break;
        case 3: //单价
          this.unitPriceBlur(itemList); //验证单价
          break;
        case 4: //金额
          this.amountBlur(itemList); //验证金额
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
    },
    clickEvent() {
      this.defaultAll();
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
    // 设置Tab切换开始
    onKeydown(event) {
      if (event.keyCode !== 9) return;
      var data = this.findCheck(event);
      this.setCheck(data);
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
                    if (l >= this.tableData.length) {
                      l = this.tableData.length;
                    } else {
                      l = l + 1;
                    }
                  } else {
                    var go = k + 1;
                  }
                  var set = this.TabArr[go];
                  data.index = l;
                  data.name = set;
                  data.item = this.tableData[l];
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
    // 设置Tab切换结束
    resetData() {
      // 初始化数据
      this.getCode();
      this.resetDeafultData();
    },
    resetDeafultData() {
      // 重置数据
      this.$refs["dtData"].resetFields();
      this.isShowFlag = true;
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = [];
      this.tableData = this.setTable(30); // 添加30行
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.WarehousingType = "";
      this.dtData.applyDate = new Date();
      this.dtData.customerName = "";
      this.dtData.warehousingOrder = "";
      this.dtData.orderTypeId = "";
      this.dtData.orderNo = "";
      this.dtData.orderId = "";
      this.dtData.receiptAddress = "";
      this.dtData.purchasingMethod = "";
      this.dtData.operatorName = "";
      this.dtData.operatorId = "";
      this.dtData.whAdminId = "";
      this.dtData.whAdminName = "";
      this.dtData.sendName = "";
      this.dtData.sendId = "";
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
      var xh = 70; // 序号宽度
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
      // this.setTable(1);
      this.tableData = [...this.tableData, ...this.setTable(1)];
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
      // 仓库选择
      this.computeAccount(this.doItem);
      this.warehouseData.map(data => {
        if (item.key == data.id) {
          item.value = data.warehouseName;
        }
      });
      item.valid = false;
    },
    async computeAccount(item) {
      // 获取账存数量
      if (
        item.materialCode.value != "" &&
        item.defaultWarehouseName.key != ""
      ) {
        this.fullscreenLoading = true;
        var data = await this.getWarehouseAmount(
          item.materialCode.key,
          item.defaultWarehouseName.key
        );
        item.account.value = data.accountNum;
        this.fullscreenLoading = false;
      }
    },

    getWarehouseAmount(materialId, warehouseId) {
      // 请求账存数量
      return new Promise((reslove, reject) => {
        var postData = {
          materialId,
          warehouseId
        };

        if (Object.keys(this.selectRow).length > 0) {
          postData.editID = this.selectRow.id;
          postData.operateType = 4;
        }

        var data = {
          postData
        };
        request({
          url: "/warehouse/api/WarehouseAmount",
          method: "post",
          data: data
        }).then(res => {
          setTimeout(() => {
            reslove(res.data);
          }, 200);
        });
      });
    },
    checkSelect(dt) {
      // 查找输入数据项
      return new Promise((resolve, reject) => {
        dt.map(item => {
          for (var i in item) {
            item.editState = false;
            if (typeof item[i] === "object") {
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
        url: "warehouse/api/TWMProductionMain/GetOrderNo",
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = "";
        } else {
          this.dtData.warehousingOrder = res;
          Storage.LStorage.set("ProductionoutCode", res);
        }
        // this.dtData.warehousingOrder = res;
        // Storage.LStorage.set("ProcurementPutCode", res);
      });
    },
    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        var flag = false;
        // var computeFlag = true; // 验证：实发数量不能大于应发数量减已发数量
        var accountFlag = true; // 验证：实发数量不能大于应发数量
        this.$refs.dtData.validate(valid => {
          // 验证
          if (!valid) {
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
          } else {
            flag = true;
          }
        });
        if (!flag) return; // 验证头部数据

        var checkSelect = await this.checkSelect(this.tableData);
        var childList = [];
        checkSelect.map(item => {
          // 验证数据
          item.defaultWarehouseName.valid = false;
          item.warehouseUnitName.valid = false;
          item.PaidIn.valid = false;

          if (item.editState) {
            if (item.defaultWarehouseName.key == "") {
              // 验证物仓库ID
              item.defaultWarehouseName.valid = true;
              flag = false;
            }
            if (item.PaidIn.value == "" || isNaN(item.PaidIn.value)) {
              // 验证实发数量
              item.PaidIn.valid = true;
              flag = false;
            }
            if (item.PaidIn.value !== "" || item.PaidIn.value !== null) {
              //验证：实发数量不能大于应发数量减已发数量
              // if (item.PaidIn.value > item.ShouldbeSsentNum.value) {
              //   item.PaidIn.valid = true;
              //   computeFlag = false;
              // }
              if (item.PaidIn.value > item.ShouldbeSReceiveNum.value) {
                //验证：实发数量不能大于应发数量
                item.PaidIn.valid = true;
                accountFlag = false;
              }
              if (item.PaidIn.value > item.account.value) {
                //验证：实发数量不能大于应发数量
                item.PaidIn.valid = true;
                accountFlag = false;
              }
            }
            //修改

            var _LogName = "";
            var param = {
              id: item.id,
              mainId: this.selectRow.id ? this.selectRow.id : 0,
              materialId: parseFloat(item.materialCode.materialId) || "",
              warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
              actualNum: parseFloat(item.PaidIn.value) || ""
            };
            if (item.unitPrice !== "" || item.unitPrice !== null) {
              param.unitPrice = parseFloat(item.unitPrice.value);
            }
            if (item.amount !== "" || item.amount !== null) {
              param.amount = item.amount.value;
            }
            if (item.remark !== "" || item.remark !== null) {
              param.remark = item.remark.value;
            }
            _LogName += `物料:${item.materialName.value} 生产出库库 ${
              item.PaidIn.value
            }${
              item.warehouseUnitName.value === null
                ? ""
                : item.warehouseUnitName.value
            } 到 ${item.defaultWarehouseName.value}`;

            param._LogName = _LogName;
            childList.push(param);
          }
        });

        if (!flag) {
          this.$message.error("数据不完整");
          return;
        }
        if (childList.length == 0) {
          this.$message({
            message: "请输入数据（至少包含一条数据）",
            type: "error"
          });
          return;
        }
        // if (!computeFlag) {
        //   this.$message.error("实发数量不能大于应发数量减已发数量");
        //   return;
        // }
        if (!accountFlag) {
          this.$message.error("实发数量不能大于可发数量");
          return;
        }

        var currData = {
          id: 0,
          whSendType: 1,
          whSendDate: formatDate(this.dtData.applyDate),
          whSendOrder: this.dtData.warehousingOrder,
          operatorId: 0,
          sourceId: this.dtData.tranSourceId,

          // number: 0,
          childList: childList
        };

        if (this.dtData.whAdminId !== "") {
          currData.whAdminId = this.dtData.whAdminId;
        }
        if (this.dtData.sendId !== "") {
          currData.sendId = this.dtData.sendId;
        }
        if (
          this.dtData.receiptAddress !== "" &&
          this.dtData.receiptAddress !== null
        ) {
          currData.receiptAddress = this.dtData.receiptAddress;
        }
      }

      var method = "";
      if (state == 1) {
        currData.id = 0;
        var data = RequestObject.CreatePostObject(currData);
        this.postData(data, state);

        // this.$confirm("是否新增出库单", "确认信息", {
        //   type: "warning"
        // })
        //   .then(() => {
        //     this.postData(data, state);
        //   })
        //   .catch(() => {});
      }

      if (state == 2) {
        if (this.selectRow.auditStatus == 2) {
          this.$message.error("该出库单为通过状态，无法编辑");
          return;
        }
        if (this.selectRow.id === undefined) {
          this.$message.error("请选择数据");
          return;
        }
        // this.PostDataEdit
        currData.id = this.selectRow.id;
        var data = RequestObject.CreatePostObject(currData);

        this.postData(data, state);

        // this.$confirm("是否保存出库单", "确认信息", {
        //   type: "warning"
        // })
        //   .then(() => {
        //     this.postData(data, state);
        //   })
        //   .catch(() => {});
      }

      if (state == 3) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该出库单是通过状态，无法删除");
          return;
        }
        if (this.selectRow.id == undefined) {
          this.$message.error("请选择删除的数据");
          return;
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的采购出库单`
        };
        var data = RequestObject.CreatePostObject(currData);

        this.$confirm("是否删除出库单？", "确认信息", {
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
        url: "/warehouse/api/TWMProductionMain",
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
            Storage.LStorage.remove("ProductionoutCode");
          }
          if (state == 2) {
            this.OnBtnSaveSubmit(res.data);
          }
          if (state == 3) {
            this.tableData = [];
            this.resetDeafultData();
            var code = Storage.LStorage.get("ProductionoutCode");
            this.dtData.warehousingOrder = code;
            // this.setTable(30);
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
    getCustomer() {
      // 获取客户名称
      var rqs = RequestObject.CreateGetObject(false, 0, 0, null);
      request({
        url: "/basicset/api/TBMCustomerFile/GetMainList",
        method: "get",
        params: {
          requestObject: JSON.stringify(rqs)
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
          this.connectionData = res.data;
        }
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
    getMaterielData(item) {
      // 获取物料档案信息
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
    rowClick(row) {
      this.doItem.materialCode.value = row.materialCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.materialName;
      this.doItem.spec.value = row.spec;

      // this.doItem.warehouseUnitName.value = row.warehouseUnitName;
      // this.doItem.unit.value = row.baseUnitName;
      // this.doItem.baseUnitNumber.value = row.baseUnitNumber;

      this.listenClick();
      this.$set(this.doItem["defaultWarehouseName"], `isShow`, true);
      this.$nextTick(() => {
        var id_ = this.doItem["defaultWarehouseName"].id;
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
              return BigNumber(prev).plus(curr);
            } else {
              return prev;
            }
          }, 0);
          if (
            index == 6 ||
            index == 7 ||
            index == 8 ||
            index == 11 ||
            index == 13 ||
            index == 14
          ) {
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
      // // 双击显示input,监听input数据变化

      // //通过不能编辑
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
        if (document.getElementById(item.id + "___" + name)) {
          document.getElementById(item.id + "___" + name).focus();
          document.getElementById(item.id + "___" + name).select();
        }

        if (state == 1) {
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
      // this.doDefault(this.doItem);
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
      // 改变页数
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
        if (typeof data[i] === "object") {
          if (data[i]["isShow"]) {
            this.$set(data[i], `isShow`, false);
          }
        }
      }
    },
    PaidInBlur(item) {
      if (isRealNum(item.PaidIn.value) === false) {
        item.PaidIn.value = "";
        return;
      }
      if (item.PaidIn.value === "" || item.PaidIn.value === null) return;
      if (item.PaidIn.value <= 0) {
        item.PaidIn.value = "";
        return;
      }
      item.PaidIn.value = parseFloat(item.PaidIn.value);
      item.PaidIn.value = getFloat(item.PaidIn.value, 2);
      item.PaidIn.valid = false;
      this.unitPriceBlur(item);
      this.amountBlur(item);
    },
    unitPriceBlur(item) {
      if (isRealNum(item.unitPrice.value) === false) {
        item.unitPrice.value = "";
        return;
      }
      if (item.unitPrice.value === "" || item.unitPrice.value === null) return;
      if (item.unitPrice.value <= 0) {
        item.unitPrice.value = "";
        return;
      }

      if (item.PaidIn.value !== "") {
        item.unitPrice.value = parseFloat(item.unitPrice.value);
        item.unitPrice.value = keepTwoDecimalFull(item.unitPrice.value);
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
      if (isRealNum(item.amount.value) === false) {
        item.amount.value = "";
        return;
      }
      if (item.amount.value === "" || item.amount.value === null) return;
      if (item.amount.value <= 0) {
        item.amount.value = "";
        return;
      }
      item.amount.value = keepTwoDecimalFull(item.amount.value);
      if (item.PaidIn.value !== "") {
        item.amount.value = parseFloat(item.amount.value);
        item.unitPrice.value = keepTwoDecimalFull(
          item.amount.value / item.PaidIn.value
        );
      } else {
        item.amount.value = keepTwoDecimalFull(parseFloat(item.amount.value));
      }
    },
    chooseNumber() {
      // 选择单据号
      if (this.selectRow.auditStatus !== undefined) {
        return;
      }
      this.$refs.DocumentNo.open();
      this.$refs.DocumentNo.dtData.pickNo = "";
      this.$refs.DocumentNo.dtData.productionNo = "";
      this.$refs.DocumentNo.getMainList();
    },
    handelAddClick() {
      // 查询
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.dtData.warehousingOrder = "";
      this.$refs.InboundOrder.dtData.applyDate = "";
      this.$refs.InboundOrder.dtData.auditStatus = -1;
      this.$refs.InboundOrder.clickRow = {};
      this.$refs.InboundOrder.getMainList();
    },

    // OnBtnSaveDocument(row, state) {
    //   this.fullscreenLoading = true;
    //   this.dtData.orderNo = row.pickNo;
    //   this.dtData.orderId = row.id;
    //   this.dtData.tranSourceId = row.id;
    //   // this.dtData.WarehousingType = row.supplierId;
    //   // this.dtData.customerName = row.supplierName;
    //   this.dtData.receiptAddress = row.receiptAddress;
    //   if (!state) {
    //     var list = [
    //       {
    //         column: "id",
    //         content: row.id,
    //         condition: 0
    //       }
    //     ];
    //     var rqs = RequestObject.CreateGetObject(false, 0, 0, list);
    //     request({
    //       //  url: "manufacturing/api/TMMPickApplyMain/GetWholeMainData",
    //       url: "warehouse/api/TWMProductionMain/GetTopWholeMainData",
    //       params: { RequestObject: row.id },
    //       method: "GET"
    //     }).then(res => {
    //       this.fullscreenLoading = false;
    //       if (res.code === 0) {
    //         this.setTableData2(res.data.childList, res.data);
    //       } else {
    //         this.$confirm(res.info, "错误信息", {
    //           confirmButtonText: "确定",
    //           type: "error",
    //           showCancelButton: false
    //         });
    //       }
    //     });
    //   } else {
    //     this.setTableData2(row.childList, row);
    //   }
    // },

    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    OnBtnSaveSubmit(row, state) {
      // 根据ID获取数据
      // this.fullscreenLoading = true;

      // amount: 1
      // auditId: 1
      // auditName: "云飞"
      // auditStatus: 2
      // auditTime: "2019-09-20T11:32:09"
      // childList: [,…]
      // deleteFlag: false
      // id: 67
      // number: 1
      // operatorId: 1
      // operatorName: "云飞"
      // receiptAddress: "123"
      // sendId: 175
      // sendName: "aaaaaaaaaa"
      // sourceCode: "PMR201909200001"
      // sourceId: 20
      // whAdminId: 175
      // whAdminName: "aaaaaaaaaa"
      // whSendDate: "2019-09-20T00:00:00"
      // whSendOrder: "PDC201909200022"
      // whSendType: 1

      if (!state) {
        request({
          url: "/warehouse/api/TWMProductionMain/GetWholeMainData",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
          this.fullscreenLoading = false;
          if (res.code === 0) {
            this.selectRow = this.cloneObject(res.data);
            // this.dtData.customerName = row.supplierName;
            // this.dtData.WarehousingType = row.customerId;

            if (this.selectRow.auditStatus === 2) {
              this.isShowFlag = false;
            } else {
              this.isShowFlag = true;
            }
            this.dtData.sendId = res.data.sendId;
            this.dtData.sendName = res.data.sendName;
            this.dtData.receiptAddress = res.data.receiptAddress;
            this.dtData.applyDate =
              res.data.whSendDate != null
                ? new Date(res.data.whSendDate.split("T")[0])
                : "";
            this.dtData.warehousingOrder = res.data.whSendOrder;
            this.dtData.orderNo = res.data.sourceCode;
            this.dtData.orderId = "";
            this.dtData.tranSourceId = res.data.sourceId;
            // this.auditStatus = res.data.auditStatus;

            this.dtData.operatorName = res.data.operatorName;
            this.dtData.operatorId = res.data.operatorId;
            this.dtData.whAdminName = res.data.whAdminName;
            this.dtData.whAdminId = res.data.whAdminId;

            // sendId
            this.dtData.sendName = res.data.sendName;
            // this.dtData.sendId = res.data.sendId;

            this.dtData.auditName = res.data.auditName;
            this.dtData.auditId = res.data.auditId;
            this.dtData.auditStatus = res.data.auditStatus;
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split("T")[0]
                : "";

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
          mainId: this.selectRow.id ? this.selectRow.id : 0,
          materialId: parseFloat(item.materialCode.materialId) || "",
          warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
          actualNum: parseFloat(item.PaidIn.value) || ""
        };
        if (item.unitPrice !== "" || item.unitPrice !== null) {
          param.unitPrice = parseFloat(item.unitPrice.value);
        }
        if (item.amount !== "" || item.amount !== null) {
          param.amount = item.amount.value;
        }
        if (item.remark !== "" || item.remark !== null) {
          param.remark = item.remark.value;
        }
        _LogName += `物料:${item.materialName.value} 生产出库库 ${
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
      //   applyDate:
      //     this.selectRow.applyDate !== null ||
      //     this.selectRow.applyDate !== ""
      //       ? this.selectRow.applyDate.split("T")[0]
      //       : "",
      //   whSendOrder: this.selectRow.whSendOrder,
      //   sourceId: this.selectRow.sourceId,
      //   childList: childList
      // };

      // var currData = {
      //     id: this.selectRow.id,
      //     whSendType: this.selectRow.warehousingType,
      //     whSendDate:
      //     this.selectRow.whSendDate !== null ||
      //     this.selectRow.whSendDate !== ""
      //       ? this.selectRow.whSendDate.split("T")[0]
      //       : "",
      //     whSendOrder: this.selectRow.whSendOrder,
      //     sourceId: this.selectRow.sourceId,

      //     // number: 0,

      //     childList: childList
      //   };
      //           //  sendId: this.dtData.sendName,

      // if (
      //   this.selectRow.whAdminId !== "" &&
      //   this.selectRow.whAdminId !== null
      // ) {
      //   currData.whAdminId = this.dtData.whAdminId;
      // }
      // if (
      //   this.selectRow.sendId !== "" &&
      //   this.selectRow.sendId !== null
      // ) {
      //   currData.sendId = this.dtData.sendId;
      // }
      //  if (
      //   this.selectRow.receiptAddress !== "" &&
      //   this.selectRow.receiptAddress !== null
      // ) {
      //   currData.receiptAddress = this.dtData.receiptAddress;
      // }

      // this.PostDataEdit = currData;
    },
    setTableData(dt, row) {
      // 将数据导入table
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
            value: item.materialCode,
            key: item.materialId,
            materialId: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            // 物料名称
            value: item.materialName,
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
            // 发料仓库
            id: newGuid(),
            value: "",
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
            value: item.availableNum,
            isShow: false,
            valid: false
          },
          PaidIn: {
            // 实发数量
            id: newGuid(),
            value: item.actualNum,
            isShow: false,
            valid: false
          },
          ShouldbeSReceiveNum: {
            // 应发数量
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
          // colorSolutionName: {
          //   // 配色方案
          //   value: item.colorSolutionName,
          //   isShow: false,
          //   valid: false
          // },
          colorName: {
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },
          // workshopName: {
          //   // 车间
          //   value: item.workshopName,
          //   isShow: false,
          //   valid: false
          // },
          warehouseUnitName: {
            // 单位
            value: item.warehouseUnitName,
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
          //   baseUnitNum: {
          //   // 基本单位应发数量
          //   value: item.baseUnitTransNum,
          //   isShow: false,
          //   valid: false
          // },
          baseUnitNum: {
            // 基本单位应发数量
            value: item.baseUnitTransNum,
            isShow: false,
            valid: false
          },
          baseUnitNumActualReceived: {
            // 基本单位实发数量
            value: item.baseUnitActualNum,
            isShow: false,
            valid: false
          },
          productionDate: {
            // 生产/采购日期
            // value: row.applyDate != null ? row.applyDate.split("T")[0] : "",
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
        this.fullscreenLoading = false;
      }, 0);
    },
    setTableData2(dt, row) {
      // 将数据导入table
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: 0,
          editState: false,
          rowIndex: 0,
          materialCode: {
            // 物料代码
            id: newGuid(),
            value: item.materialCode,
            key: item.materialId,
            materialId: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            // 物料名称
            value: item.materialName,
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
            // 发料仓库
            id: newGuid(),
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
            // 实发数量
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          ShouldbeSReceiveNum: {
            // 应发数量
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
            value: "",
            isShow: false,
            valid: false
          },
          account: {
            // 金额

            value: item.availableNum,
            isShow: false,
            valid: false
          },
          amount: {
            // 金额
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          // colorSolutionName: {
          //   // 配色方案
          //   value: item.colorSolutionName,
          //   isShow: false,
          //   valid: false
          // },
          colorName: {
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },
          // workshopName: {
          //   // 车间
          //   value: item.workshopName,
          //   isShow: false,
          //   valid: false
          // },
          warehouseUnitName: {
            // 单位
            value: item.warehouseUnitName,
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
            // 基本单位应发数量
            value: item.baseUnitTransNum,
            isShow: false,
            valid: false
          },
          baseUnitNumActualReceived: {
            // 基本单位实发数量
            value: "",
            isShow: false,
            valid: false
          },
          productionDate: {
            // 生产/采购日期
            value: row.orderDate != null ? row.orderDate.split("T")[0] : "",
            isShow: false,
            valid: false
          },
          // shelfLife: {
          //   // 保质期（天）
          //   value: "",
          //   isShow: false,
          //   valid: false
          // },
          validityPeriod: {
            // 有效期至
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            id: newGuid(),
            value: "",
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
      // this.setCurrData(this.cloneTable);
      setTimeout(() => {
        this.tableData = [...listArr];
        this.fullscreenLoading = false;
      }, 0);
    },
    doOtherWhAudit(state) {
      // 审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error("该出库单已经为通过状态");
        return;
      }
      this.$confirm("是保存操作", {
        type: "warning"
      })
        .then(() => {
          var data = RequestObject.CreatePostObject({
            id: this.selectRow.id,
            auditStatus: state,
            ChildList: []
          });
          this.fullscreenLoading = true;
          request({
            url: "/warehouse/api/TWMProductionMain/Audit",
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
              this.OnBtnSaveSubmit({ id: this.selectRow.id });
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
        })
        .catch(() => {});
    },
    doOtherWhCancelAudit() {
      // 撤销审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus != 2) {
        this.$message.error("该出库单不是通过状态，无法撤销");
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
    }
  }
};
</script>
<style lang="scss" scoped>
#ProductionoutOrder /deep/ {
  .cursor .el-input__inner {
    cursor: pointer;
  }
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
  .falseIptop {
    // width: 200px;
    overflow: hidden;
    // border-bottom: 1px solid #dcdfe6;
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

