<template>
  <el-container
    id="SalesReceipt"
    v-on:click.native="listenClick"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <!-- 头部按钮区开始 -->
    <!-- <div style="padding:5px 0px 0px 20px;height: 37px;font-size:0px;">
      <div v-if="OtherData.type!=1&&btnAip.add">
        <el-button-group class="groupBtn">
          <el-button
            type="default"
            @click="addPutInStorage(1)"
            v-if="btnAip.add.buttonCaption&&addControl"
          >{{ btnAip.add.buttonCaption }}</el-button>

          <el-button type="default" @click="resetData()">重置</el-button>

          <el-button
            type="default"
            @click="addPutInStorage(2)"
            v-if="btnAip.modify.buttonCaption"
          >{{ btnAip.modify.buttonCaption }}</el-button>
          <el-button
            type="default"
            @click="addPutInStorage(3)"
            v-if="btnAip.delete.buttonCaption"
          >{{ btnAip.delete.buttonCaption }}</el-button>
          <el-button
            type="default"
            @click="handelAddClick"
            v-if="btnAip.query.buttonCaption"
          >{{ btnAip.query.buttonCaption }}</el-button>
        </el-button-group>
        <el-dropdown
          split-button
          type="default"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
          v-if="btnAip.review.buttonCaption"
          class="dropdown"
        >
          {{ btnAip.review.buttonCaption }}
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>-->
    <div class="btnHeader">
      <div v-if="btnAip.add" class="btnHeaderBox">
        <el-button
          type="default"
          @click="addPutInStorage(1)"
          v-if="btnAip.add.buttonCaption&&addControl"
        >{{ btnAip.add.buttonCaption }}</el-button>
        <el-button type="default" @click="resetData()">重置</el-button>
        <el-button
          type="default"
          @click="addPutInStorage(2)"
          v-if="btnAip.modify.buttonCaption"
        >{{ btnAip.modify.buttonCaption }}</el-button>
        <el-button
          type="default"
          @click="addPutInStorage(3)"
          v-if="btnAip.delete.buttonCaption"
        >{{ btnAip.delete.buttonCaption }}</el-button>
        <el-button
          type="default"
          @click="handelAddClick"
          v-if="btnAip.query.buttonCaption"
        >{{ btnAip.query.buttonCaption }}</el-button>
        <el-dropdown
          split-button
          type="default"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
          v-if="btnAip.review.buttonCaption"
          class="dropdown"
        >
          {{ btnAip.review.buttonCaption }}
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>
    <!-- 头部按钮区结束 -->

    <!-- 头部表单开始 -->
    <el-header id="elheader" class="header fromCenter" height="auto">
      <div class="middleWidth">
        <el-form
          ref="dtData"
          :model="dtData"
          label-width="86px"
          class="FormInput"
          inline
          label-position="right"
          :rules="dtDataRules"
        >
          <el-form-item class="formItem" label="源单类型：">
            <div class="headerIp bbfe6">
              <span>销售订单</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="单据号：" prop="orderNo">
            <el-input
              class="headerIp"
              v-model="dtData.orderNo"
              @click.native="chooseNumber"
              readonly
            >
              <i slot="suffix" class="el-input__icon el-icon-search"></i>
            </el-input>
          </el-form-item>

          <el-form-item class="formItem disableType" label="客户名称：">
            <!-- <el-select
              v-model="dtData.WarehousingType"
              clearable
              filterable
              placeholder="请选择"
              class="headerIp"
              @change="connectionChange"
              disabled
            >
              <el-option
                v-for="item in connectionData"
                :key="item.id"
                :label="item.customerName"
                :value="item.id"
              ></el-option>
            </el-select>-->
            <div class="headerIp bbfe6">{{dtData.customerName}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="状态：">
            <div class="headerIp bbfe6">
              <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待出库</span>
              <span v-if="dtData.auditStatus==0">出库待审核</span>
              <span v-if="dtData.auditStatus==1">审核未通过</span>
              <span v-if="dtData.auditStatus==2">出库完成</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="编号：" prop="warehousingOrder">
            <div class="headerIp bbfe6">{{dtData.warehousingOrder}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="出库日期：" prop="warehousingDate">
            <el-date-picker
              v-model="dtData.warehousingDate"
              type="date"
              placeholder="选择日期"
              class="headerIp"
              :readonly="selectRow.auditStatus === 2"
            ></el-date-picker>
          </el-form-item>

          <el-form-item class="formItem" label="收货地址：">
            <el-input
              class="headerIp"
              v-model="dtData.receiptAddress"
              style="width:510px;"
              :readonly="selectRow.auditStatus === 2"
            ></el-input>
          </el-form-item>
        </el-form>
      </div>
    </el-header>
    <!-- 头部表单结束 -->

    <!-- table列表开始 -->
    <el-main>
      <!-- 物料弹框开始 -->
      <div class="findBox" :style="popoverStyle" v-show="popoverState" v-on:click.stop="1==1">
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
          :pageIndex="pageIndex"
          :totalCount="totalCount"
          :pageSize="pageSize"
          @pagination="pagination"
        />
      </div>
      <!-- 物料弹框结束 -->

      <!-- 序号弹框开始 -->
      <div class="findBox" :style="menuStyle" v-show="menuState">
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
                <span @click.stop="menuBar($event,scope.$index)">
                  {{scope.$index+1}}
                  <i class="el-icon-setting"></i>
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
              <div class="tableTd">{{scope.row.materialCode.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="包型名称" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>包型名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.materialName.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="colorSolutionName" label="配色方案" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>配色方案</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.colorSolutionName.value}}</div>
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
                <div class="tableTd">{{scope.row.spec.value}}</div>
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
              <div class="tableTd">{{scope.row.colorName.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="defaultWarehouseName" label="出货仓库">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>出货仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'defaultWarehouseName')"
                :class="{validStyle:scope.row.defaultWarehouseName.valid}"
              >
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
              </div>-->
              <EditSelect
                v-model="scope.row.defaultWarehouseName"
                @clickEvent="clickEvent"
                :data="warehouseData"
                :isShowFlag="isShowFlag"
                label="warehouseName"
                @change="computeAccount(scope.row)"
              ></EditSelect>
            </template>
          </el-table-column>

          <el-table-column prop="PaidIn" label="实发数量">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>实发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'PaidIn')"
                :class="{validStyle:scope.row.PaidIn.valid}"
              >
                <div class="tableTd" v-show="!scope.row.PaidIn.isShow">{{scope.row.PaidIn.value}}</div>
                <el-input
                  v-show="scope.row.PaidIn.isShow"
                  v-model="scope.row.PaidIn.value"
                  :id="scope.row.id+'___PaidIn'"
                  size="mini"
                  @blur="PaidInBlur(scope.row)"
                  @keyup.enter.native="PaidInBlur(scope.row)"
                ></el-input>
              </div>-->
              <EditInput
                :class="{redCol:scope.row.PaidIn.value<=0,greCol:scope.row.PaidIn.value>0}"
                v-model="scope.row.PaidIn"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
                :type="2"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
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
              <div class="tableTd">{{scope.row.account.value}}</div>
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
              <div class="tableTd">{{scope.row.ShouldbeSsentNum.value}}</div>
            </template>
          </el-table-column>

          <!-- 新增单位 -->
          <el-table-column prop="warehouseUnitName" label="单位">
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
              >{{scope.row.baseUnitName.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{scope.row.unitPrice.value}}</div> -->
              <EditInput
                v-model="scope.row.unitPrice"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
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
              <!-- <div class="tableTd">{{scope.row.amount.value}}</div> -->
              <EditInput
                v-model="scope.row.amount"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
                :type="4"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
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
              <div class="tableTd">{{scope.row.sentNum.value}}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="baseUnitName" label="基本单位名称" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.baseUnitName.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitNum" label="基本单位可发数量" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位可发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.baseUnitNum.value}}</div>
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
                <span class="start"></span>
                <span>备注</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'remark')"
                :class="{validStyle:scope.row.remark.valid}"
              >
                <div class="tableTd" v-show="!scope.row.remark.isShow">
                  <el-popover
                    v-if="scope.row.remark.value.length>=20"
                    placement="top-start"
                    trigger="hover"
                    :content="scope.row.remark.value"
                  >
                    <div class="ellipsis" slot="reference">{{ scope.row.remark.value }}</div>
                  </el-popover>
                  <div
                    class="ellipsis"
                    v-if="scope.row.remark.value.length<20"
                  >{{scope.row.remark.value}}</div>
                </div>
                <el-input
                  v-show="scope.row.remark.isShow"
                  v-model="scope.row.remark.value"
                  :id="scope.row.id+'___remark'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.remark"
                @clickEvent="clickEvent"
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
            <div class="falseIp">{{dtData.operatorName}}</div>
          </div>
        </el-form-item>

        <el-form-item label="仓管员：">
          <div class="falseIp" v-if="selectRow.auditStatus == 2">{{dtData.whAdminName}}</div>
          <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.whAdminId"
            clearable
            filterable
            placeholder="请选择"
          >
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="发货员：">
          <div class="falseIp" v-if="selectRow.auditStatus == 2">{{dtData.sendName}}</div>
          <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.sendId"
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
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{dtData.auditName}}</div>
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
    <!-- 页脚结束 -->

    <!-- 查看弹框开始 -->
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit" title="销售出库单查询"></InboundOrder>
    <!-- 查看弹框结束 -->

    <!-- 单据号弹框开始 -->
    <DocumentNo
      ref="DocumentNo"
      @OnBtnSaveSubmit="OnBtnSaveDocument"
      title="销售订单选择"
      :btnAip="btnAip"
      @reSet="resetDeafultData"
    ></DocumentNo>
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
  isEmpty
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import InboundOrder from "./components/InboundOrder";
import DocumentNo from "./components/DocumentNo";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";

import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";
import { Receipt } from "@/utils/Receipt";
import BigNumber from "bignumber.js";

export default {
  name: "viewsinventorySalesReceiptindexvue",
  mixins: [Receipt],
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
        WarehousingType: "", //客户名称id
        customerName: "", //客户名称
        warehousingDate: new Date(), //出库日期
        warehousingOrder: "", //编号
        orderTypeId: "", //订单类型
        orderNo: "", //单据号
        orderId: "", //单据号ID
        receiptAddress: "", //收货地址
        operatorName: "", //制单人
        operatorId: "",
        whAdminId: "", //仓管员
        whAdminName: "",
        sendId: "", //发货员
        sendName: "",
        // receiptName: "", //收货员
        // receiptId: "",
        auditName: "", //审核人
        auditId: "",
        auditStatus: -1, //状态
        auditTime: "" //审核时间
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
      materielData: [], //物料信息
      warehouseData: [], //仓库信息
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          packageCode: {
            //物料代码
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          packageName: {
            //包型名称
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          materialCode: {
            //包型代码
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          materialName: {
            //包型名称
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          colorSolutionName: {
            //配色方案
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          spec: {
            //规格型号
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          colorName: {
            //颜色
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            //出货仓库
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            //单位
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
            //实发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          missNum: {
            //欠货数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          unitPrice: {
            //单价
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          amount: {
            //金额
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          ShouldbeSsentNum: {
            //应发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          sentNum: {
            //已发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitName: {
            //基本单位名称
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNum: {
            //基本单位应发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumActualSent: {
            //基本单位实发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumAlreadySent: {
            //基本单位已发数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            //备注
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
      TabArr: ["defaultWarehouseName", "PaidIn", "remark"],
      materialItem: {},
      connectionData: [], //客户名称数据
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
    InboundOrder,
    DocumentNo,
    EditInput,
    EditSelect
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
  },
  async mounted() {
    this.mountedState = true;
    window.addEventListener("keydown", this.onKeydown);
    this.setStyle(); //设置样式
    this.warehouseData = await this.getWarehouseData(); //仓库数据
    // this.getCustomer(); //获取客户名称
    // this.getMaterielData(); //物料数据
    this.getUserCompany(); //收货员列表

    if (this.$route.params.type) {
      if (this.$route.params.type == 3) {
        this.resetData();
      } else {
        this.OtherData = this.$route.params;
        this.OnBtnSaveSubmit(this.OtherData.item);
      }
    } else {
      var code = Storage.LStorage.get("SalesReceiptCode");
      if (!code) {
        this.getCode();
      } else {
        this.dtData.warehousingOrder = code;
      }
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = this.setTable(this.tableLen); //添加30行
      this.fullscreenLoading = false;
    }
    // this.$set(this.tableData[1]["materialName"], `isShow`, true)
    this.mountedState = false;
  },
  async activated() {
    if (this.mountedState == true) {
      return;
    }
    this.setStyle(); //设置样式
    window.addEventListener("keydown", this.onKeydown);
    if (this.$route.params.type) {
      if (this.$route.params.type == 3) {
        this.resetData();
      } else {
        this.OtherData = this.$route.params;
        this.OnBtnSaveSubmit(this.OtherData.item);
      }
    }
  },
  deactivated() {
    window.removeEventListener("keydown", this.onKeydown);
  },
  methods: {
    blurCheck(itemList, item, type, state) {
      //td框的类型（可用于数据校验等）
      switch (
        type //（type）对应组件type
      ) {
        case 1: //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.getMaterielData(itemList, (this.pageIndex = 1));
          }
          break;
        case 2: //实发数量
          this.PaidInBlur(itemList);
          break;
        case 3: //单价
          this.unitPriceBlur(itemList);
          break;
        case 4: //金额
          this.amountBlur(itemList);
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
    // setStyle() {
    //   // 设置页面样式
    //   this.$nextTick(() => {
    //     var btn = 30; //按钮高度
    //     var navbar = document.getElementById("navbar_yfkj");
    //     var nv = navbar.clientHeight || navbar.offsetHeight;
    //     var b = document.documentElement.clientHeight - nv;
    //     var elheader = document.getElementById("elheader");
    //     var elfooter = document.getElementById("elfooter");
    //     var h = elheader.clientHeight || elheader.offsetHeight;
    //     var f = elfooter.clientHeight || elfooter.offsetHeight;
    //     this.tableHeight = b - h - f - 40 - btn;
    //   });
    // },
    //设置Tab切换开始
    // onKeydown(event) {
    //   if (event.keyCode !== 9) return;
    //   var data = this.findCheck(event);
    //   this.setCheck(data);
    // },
    // findCheck(event) {
    //   var data = {};
    //   for (var h = 0; h < this.tableData.length; h++) {
    //     for (var i in this.tableData[h]) {
    //       if (typeof this.tableData[h][i] == "object") {
    //         if (this.tableData[h][i]["isShow"] === true) {
    //           for (var k = 0; k < this.TabArr.length; k++) {
    //             if (this.TabArr[k] === i) {
    //               event.preventDefault();
    //               var l = h;
    //               if (k + 1 > this.TabArr.length - 1) {
    //                 var go = 0;
    //                 // console.log("-");
    //                 if (l >= this.tableData.length) {
    //                   l = this.tableData.length;
    //                 } else {
    //                   l = l + 1;
    //                 }
    //               } else {
    //                 // console.log("+");
    //                 var go = k + 1;
    //               }
    //               // console.log(go);
    //               var set = this.TabArr[go];
    //               data.index = l;
    //               data.name = set;
    //               data.item = this.tableData[l];
    //               // console.log(data);
    //               return data;
    //             }
    //           }
    //         }
    //       }
    //     }
    //   }
    // },
    // setCheck(data) {
    //   if (data == undefined) return;
    //   var { index, name, item } = data;
    //   this.defaultAll();
    //   for (var k in this.tableData[index]) {
    //     if (k == name) {
    //       this.$set(this.tableData[index][name], `isShow`, true);
    //       this.$nextTick(() => {
    //         document
    //           .getElementById(this.tableData[index].id + "___" + name)
    //           .focus();
    //         document
    //           .getElementById(this.tableData[index].id + "___" + name)
    //           .select();
    //       });
    //     }
    //   }
    // },
    defaultAll() {
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
    },
    //设置Tab切换结束
    resetData() {
      //初始化数据
      this.isShowFlag = true;
      this.getCode();
      this.resetDeafultData();
    },
    resetDeafultData(state) {
      //重置数据
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = [];
      this.tableData = this.setTable(this.tableLen); //添加30行
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];
      this.isShowFlag = true;

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
      this.fullscreenLoading = false;
      if (state) {
        var code = Storage.LStorage.get("SalesReceiptCode");
        this.dtData.warehousingOrder = code;
      }
    },
    menuBar(event, num) {
      if (this.dtData.auditStatus == 2) {
        return;
      }
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
      //仓库选择
      this.computeAccount(this.doItem);
      // this.warehouseData.map(data => {
      //   if (item.key == data.id) {
      //     item.value = data.warehouseName;
      //   }
      // });
      // item.valid = false;
    },
    async computeAccount(item) {
      //获取账存数量
      if (
        item.materialCode.value != "" &&
        item.defaultWarehouseName.key != ""
      ) {
        this.fullscreenLoading = true;
        var data = await this.getWarehouseAmount(
          item.packageCode.key,
          item.defaultWarehouseName.key
        );
        item.account.value = data.avaiableNum;
        if (item.ShouldbeSsentNum.value > item.account.value) {
          item.PaidIn.value = item.account.value;
        } else if (item.ShouldbeSsentNum.value < item.account.value) {
          item.PaidIn.value = item.ShouldbeSsentNum.value;
        } else if (item.ShouldbeSsentNum.value == item.account.value) {
          item.PaidIn.value = item.ShouldbeSsentNum.value;
        }
        this.getMissNum(item);
        this.fullscreenLoading = false;
      }
    },
    getWarehouseAmount(materialId, warehouseId) {
      //请求账存数量
      return new Promise((reslove, reject) => {
        var postData = {
          materialId,
          warehouseId
        };

        if (Object.keys(this.selectRow).length > 0) {
          postData.editID = this.selectRow.id;
          postData.operateType = 2;
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
      //查找输出数据项
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
        url: "warehouse/api/TWMSalesMain/GetOrderNo",
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = "";
        } else {
          this.dtData.warehousingOrder = res;
          Storage.LStorage.set("SalesReceiptCode", res);
        }
        // this.dtData.warehousingOrder = res;
        // Storage.LStorage.set("ProcurementPutCode", res);
      });
    },
    async addPutInStorage(state) {
      //新增
      if (state == 1 || state == 2) {
        var flag = false;
        var computeFlag = true; //验证：实发数量不能大于应发数量减已发数量
        var accountFlag = true; //验证：实发数量不能大于账存数量
        this.$refs.dtData.validate(valid => {
          //验证
          if (!valid) {
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
          } else {
            flag = true;
          }
        });
        if (!flag) return; //验证头部数据

        var checkSelect = await this.checkSelect(this.tableData);
        var childList = [];
        checkSelect.map(item => {
          //验证数据
          item.defaultWarehouseName.valid = false;
          item.warehouseUnitName.valid = false;
          item.PaidIn.valid = false;

          if (item.editState) {
            if (item.defaultWarehouseName.key == "") {
              //验证物仓库ID
              item.defaultWarehouseName.valid = true;
              flag = false;
            }
            if (item.PaidIn.value == "" || isNaN(item.PaidIn.value)) {
              //验证实收数量
              item.PaidIn.valid = true;
              flag = false;
            }
            if (item.PaidIn.value !== "" || item.PaidIn.value !== null) {
              //验证：实发数量不能大于应发数量减已发数量
              if (item.PaidIn.value > item.ShouldbeSsentNum.value) {
                item.PaidIn.valid = true;
                computeFlag = false;
              }
              if (item.PaidIn.value > item.account.value) {
                //验证：实发数量不能大于账存数量
                item.PaidIn.valid = true;
                accountFlag = false;
              }
            }

            var _LogName = "";
            var num = /^[0-9]+$/;
            var param = {
              // id: parseFloat(item.id) || 0,
              mainId: 0,
              materialId: parseFloat(item.packageCode.key) || "",
              warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
              actualNum: parseFloat(item.PaidIn.value) || "",
              SalesOrderActualNum: 0,
              SalesOrderDetailId: item.salesOrderDetailId
            };
            if (num.test(item.id)) {
              param.id = item.id;
              param.mainId = this.selectRow.id;
            } else {
              param.id = 0;
            }
            if (item.unitPrice !== "" || item.unitPrice !== null) {
              param.unitPrice = item.unitPrice.value;
            }
            if (item.amount !== "" || item.amount !== null) {
              param.amount = item.amount.value;
            }
            if (item.remark !== "" || item.remark !== null) {
              param.remark = item.remark.value;
            }
            _LogName += `物料:${item.materialName.value} 销售出库 ${
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
            message: "请输出数据（至少包含一条数据）",
            type: "error"
          });
          return;
        }
        if (!computeFlag) {
          this.$message.error("实发数量不能大于可发数量");
          return;
        }
        if (!accountFlag) {
          this.$message.error("实发数量不能大于账存数量");
          return;
        }

        var currData = {
          whSendType: 0,
          whSendDate: formatDate(this.dtData.warehousingDate),
          whSendOrder: this.dtData.warehousingOrder,
          operatorId: 0,
          sourceId: this.dtData.orderId,
          childList: childList
        };

        if (this.dtData.whAdminId !== "") {
          currData.whAdminId = this.dtData.whAdminId;
        }
        if (this.dtData.sendId !== "") {
          currData.sendId = this.dtData.sendId;
        }
        if (this.dtData.receiptAddress !== "") {
          currData.receiptAddress = this.dtData.receiptAddress;
        }
      }

      var method = "";
      if (state == 1) {
        currData.id = 0;
        currData.IsMaterial = 0;
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
        currData.id = this.selectRow.id;
        var data = RequestObject.CreatePostObject(
          currData,
          null,
          this.PostDataEdit
        );
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
          _LogName: `删除ID：${this.selectRow.id} 的出库单`
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
        url: "/warehouse/api/TWMSalesMain",
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: "保存数据成功",
            type: "success"
          });
          if (state == 1) {
            this.OnBtnSaveSubmit(res.data);
            Storage.LStorage.remove("SalesReceiptCode");
          }
          if (state == 3) {
            this.tableData = [];
            this.resetDeafultData();
            var code = Storage.LStorage.get("SalesReceiptCode");
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
      //获取客户名称
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
    // setTable(num) {
    //   // this.fullscreenLoading = true;
    //   //初始化30条数据
    //   var listArr = [];
    //   for (var i = 0; i < num; i++) {
    //     var list = {};
    //     for (var j in this.tableData2[0]) {
    //       if (j == "id") {
    //         list["id"] = newGuid();
    //       } else {
    //         if (typeof this.tableData2[0][j] == "object") {
    //           list[j] = {};
    //           for (var k in this.tableData2[0][j]) {
    //             if (typeof this.tableData2[0][j][k] == "boolean") {
    //               list[j][k] = false;
    //             } else {
    //               list[j][k] = "";
    //             }
    //           }
    //         } else {
    //           list[j] = "";
    //         }
    //       }
    //     }
    //     listArr.push(list);
    //   }
    //   return listArr;
    //   // setTimeout(()=>{
    //   //   this.fullscreenLoading = false;
    //   // },1000)
    // },
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
    rowClick(row) {
      this.doItem.materialCode.value = row.packageCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.packageName;
      this.doItem.spec.value = row.spec;
      this.doItem.colorSolutionName.value = row.colorSolutionName;

      this.doItem.warehouseUnitName.value = row.warehouseUnitName;
      this.doItem.unit.value = row.baseUnitName;
      this.doItem.baseUnitNumber.value = row.baseUnitNumber;

      this.listenClick();
      this.$set(this.doItem["defaultWarehouseName"], `isShow`, true);
      this.$nextTick(() => {
        document
          .getElementById(this.doItem.id + "___" + "defaultWarehouseName")
          .focus();
        document
          .getElementById(this.doItem.id + "___" + "defaultWarehouseName")
          .select();
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
    // chengenum(event, item, name, state) {
    //   //双击显示input,监听input数据变化
    //   //通过不能编辑
    //   if (this.selectRow.auditStatus === 2) {
    //     return;
    //   }
    //   event.stopPropagation();

    //   // this.doDefault(this.doItem);
    //   this.defaultAll();
    //   // this.popoverState = false;
    //   item[name].isShow = true;
    //   this.doItem = item;
    //   this.doItemName = name;
    //   this.$nextTick(() => {
    //     if (document.getElementById(item.id + "___" + name)) {
    //       document.getElementById(item.id + "___" + name).focus();
    //       document.getElementById(item.id + "___" + name).select();
    //     }

    //     if (state == 1) {
    //       this.findBox(item, name);
    //     }
    //   });
    // },
    // findBox(item, name) {
    //   var IH = document.getElementById(item.id + "___" + name).offsetHeight + 8;
    //   var IW = document.getElementById(item.id + "___" + name).offsetWidth + 24;
    //   if (this.$store.getters.sidebar.opened) {
    //     var ol = 210;
    //   } else {
    //     var ol = 54;
    //   }
    //   var wl = document.documentElement.clientWidth; //屏幕宽度
    //   var wh = document.documentElement.clientHeight; //屏幕宽度
    //   var PoL = document
    //     .getElementById(item.id + "___" + name)
    //     .getBoundingClientRect().left; //弹框left值
    //   var PoT = document
    //     .getElementById(item.id + "___" + name)
    //     .getBoundingClientRect().top; //弹框top值
    //   var PoW = parseInt(this.popoverStyle.width);
    //   var PoH = parseInt(this.popoverStyle.height);
    //   if (PoW + PoL > wl) {
    //     this.popoverStyle.left = PoL - ol - PoW + IW + "px";
    //   } else {
    //     this.popoverStyle.left = PoL - ol + "px";
    //   }
    //   if (PoT + PoH > wh) {
    //     this.popoverStyle.top = PoT - PoH - 84 - 9 + "px";
    //   } else {
    //     this.popoverStyle.top = PoT - 84 + IH + "px";
    //   }
    //   this.popoverState = true;
    // },
    // listenClick() {
    //   this.popoverState = false;
    //   this.doDefault(this.doItem);
    // },
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
    PaidInBlur(item) {
      item.missNum.value=""
      if (isRealNum(item.PaidIn.value) === false) {
        item.PaidIn.value = "";
        return;
      }
      if (item.PaidIn.value === "" || item.PaidIn.value === null) return;
      if (item.PaidIn.value <= 0) {
        item.PaidIn.value = "";
        return;
      }
      item.PaidIn.value = keepTwoDecimalFull(item.PaidIn.value);
      item.PaidIn.value = parseFloat(item.PaidIn.value);
      this.unitPriceBlur(item); //单价算金额
      this.amountBlur(item); //金额算单机
      item.PaidIn.valid = false;
      this.getMissNum(item);

      // if (item.account.value !== "" && item.account.value !== null) {
      //   item.overFlow.value =
      //     parseFloat(item.PaidIn.value) - parseFloat(item.account.value);
      // }
    },
    getMissNum(item) {
      //欠货数量
      if (!isEmpty(item.ShouldbeSsentNum.value)) {
        item.missNum.value = BigNumber(item.ShouldbeSsentNum.value).minus(item.PaidIn.value)       
      }
    },
    unitPriceBlur(item) {
      if (
        item.unitPrice.value === "" ||
        item.unitPrice.value === null ||
        item.unitPrice.value === undefined
      )
        return;

      if (item.PaidIn.value !== "") {
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
      if (
        item.amount.value === "" ||
        item.amount.value === null ||
        item.amount.value === undefined
      )
        return;
      if (item.PaidIn.value !== "") {
        item.amount.value = parseFloat(item.amount.value);
        item.unitPrice.value = keepTwoDecimalFull(
          item.amount.value / item.PaidIn.value
        );
      } else {
        item.amount.value = keepTwoDecimalFull(parseFloat(item.amount.value));
      }
    },
    connectionChange(key) {
      //客户名称获取地址
      this.connectionData.map(item => {
        if (item.id === this.dtData.WarehousingType) {
          this.dtData.receiptAddress =
            `${item.cityName}${item.address}` === "null"
              ? ""
              : `${item.cityName}${item.address}`;
        }
      });
    },
    chooseNumber() {
      //选择单据号
      if (this.selectRow.auditStatus !== undefined) {
        return;
      }
      this.$refs.DocumentNo.open();
      this.$refs.DocumentNo.getMainList();
    },
    handelAddClick() {
      //查询
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.dtData.warehousingOrder = "";
      this.$refs.InboundOrder.dtData.warehousingDate = "";
      this.$refs.InboundOrder.dtData.auditStatus = -1;
      this.$refs.InboundOrder.clickRow = {};
      this.$refs.InboundOrder.getMainList();
    },
    OnBtnSaveDocument(row, state) {
      this.fullscreenLoading = true;
      this.dtData.orderNo = row.orderNo;
      this.dtData.orderId = row.id;
      this.dtData.WarehousingType = row.customerId;
      this.dtData.customerName = row.customerName;
      this.dtData.receiptAddress = row.receiptAddress;
      if (!state) {
        request({
          url: "sales/api/TSSMSalesOrderMain/GetTransferList",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
          this.fullscreenLoading = false;
          if (res.code === 0) {
            this.setTableData2(res.data);
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          }
        });
      } else {
        this.setTableData2(row.childList);
      }
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    OnBtnSaveSubmit(row, state) {
      if (row.auditStatus == 2) {
        this.isShowFlag = false;
      } else {
        this.isShowFlag = true;
      }
      //根据ID获取数据
      this.fullscreenLoading = true;
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
      //设置编辑时候的日记数据
      var childList = [];
      var _LogName = "";
      this.cloneTable.map(item => {
        var param = {
          id: 0,
          mainId: 0,
          materialId: parseFloat(item.packageCode.key) || "",
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
        currData.whAdminId = this.dtData.whAdminId;
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
      //将数据导出table
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: item.id,
          editState: false,
          rowIndex: 0,
          salesOrderDetailId: item.salesOrderDetailId,
          packageCode: {
            //包型代码
            id: newGuid(),
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          packageName: {
            //包型名称
            id: newGuid(),
            value: item.materialName,
            isShow: false,
            valid: false
          },
          materialCode: {
            //包型代码
            id: newGuid(),
            value: item.packageCode,
            key: item.packageId,
            isShow: false,
            valid: false
          },
          materialName: {
            //包型名称
            id: newGuid(),
            value: item.packageName,
            isShow: false,
            valid: false
          },
          colorSolutionName: {
            //配色方案
            id: newGuid(),
            value: item.colorSolutionName,
            isShow: false,
            valid: false
          },
          spec: {
            //规格型号
            id: newGuid(),
            value: item.spec,
            isShow: false,
            valid: false
          },
          colorName: {
            //颜色
            id: newGuid(),
            value: item.colorName,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            //出货仓库
            id: newGuid(),
            value: "",
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            //单位
            id: newGuid(),
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          PaidIn: {
            //实发数量
            id: newGuid(),
            value: item.actualNum,
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
            id: newGuid(),
            value: item.availableNum,
            isShow: false,
            valid: false
          },
          missNum: {
            //欠货数量
            id: "",
            value: item.missNum,
            isShow: false,
            valid: false
          },
          ShouldbeSsentNum: {
            //应发数量
            id: newGuid(),
            value: item.shouldNum,
            isShow: false,
            valid: false
          },
          unitPrice: {
            //单价
            id: newGuid(),
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          amount: {
            //金额
            id: newGuid(),
            value: item.amount,
            isShow: false,
            valid: false
          },
          sentNum: {
            //已发数量
            id: newGuid(),
            value: item.alreadyNum,
            isShow: false,
            valid: false
          },
          baseUnitName: {
            //基本单位名称
            id: newGuid(),
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          baseUnitNum: {
            //基本单位应发数量
            id: newGuid(),
            value: item.baseUnitShouldNum,
            isShow: false,
            valid: false
          },
          baseUnitNumActualSent: {
            //基本单位实发数量
            id: newGuid(),
            value: item.baseUnitActualNum,
            isShow: false,
            valid: false
          },
          baseUnitNumAlreadySent: {
            //基本单位已发数量
            id: newGuid(),
            value: item.baseUnitAlreadyNum,
            isShow: false,
            valid: false
          },
          remark: {
            //备注
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
    setTableData2(dt) {
      //将数据导出table
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          salesOrderDetailId: item.id,
          editState: false,
          rowIndex: 0,
          packageCode: {
            //包型代码
            id: newGuid(),
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialCode: {
            //包型代码
            id: newGuid(),
            value: item.packageCode,
            key: item.packageId,
            isShow: false,
            valid: false
          },
          packageName: {
            //包型名称
            id: newGuid(),
            value: item.materialName,
            isShow: false,
            valid: false
          },
          materialName: {
            //包型名称
            id: newGuid(),
            value: item.packageName,
            isShow: false,
            valid: false
          },
          colorSolutionName: {
            //配色方案
            id: newGuid(),
            value: item.colorSolutionName,
            isShow: false,
            valid: false
          },
          spec: {
            //规格型号
            id: newGuid(),
            value: item.spec,
            isShow: false,
            valid: false
          },
          colorName: {
            //颜色
            id: newGuid(),
            value: item.colorName,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            //出货仓库
            id: newGuid(),
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            //单位
            id: newGuid(),
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          PaidIn: {
            //实发数量
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          missNum: {
            //欠货数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          ShouldbeSsentNum: {
            //应发数量
            id: newGuid(),
            value: item.warehouseNum,
            isShow: false,
            valid: false
          },
          unitPrice: {
            //单价
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          amount: {
            //金额
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          sentNum: {
            //已发数量
            id: newGuid(),
            value: item.alreadyNum,
            isShow: false,
            valid: false
          },
          baseUnitName: {
            //基本单位名称
            id: newGuid(),
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          baseUnitNum: {
            //基本单位应发数量
            id: newGuid(),
            value: item.baseUnitNum,
            isShow: false,
            valid: false
          },
          baseUnitNumActualSent: {
            //基本单位实发数量
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumAlreadySent: {
            //基本单位已发数量
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            //备注
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
      // this.setCurrData(this.cloneTable);
      setTimeout(() => {
        this.tableData = [...listArr];
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
        this.$message.error("该出库单已经为通过状态");
        return;
      }
      var text = "确定审核通过吗？";
      if (state == 1) {
        text = "确定审核未通过吗？";
      }
      this.$confirm(text, {
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
            url: "/warehouse/api/TWMSalesMain/AuditAsync",
            method: "put",
            data: data
          }).then(res => {
            if (res.code === 0) {
              this.$message({
                message: "保存数据成功",
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
        })
        .catch(() => {});
    },
    doOtherWhCancelAudit() {
      //撤销审核
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
            message: "保存数据成功",
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
#SalesReceipt /deep/ {
  .redCol {
    color: red;
  }
  .greCol {
    color: #42b983;
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
}
@import "@/styles/receipts.scss";
</style>
