<template>
  <el-container
    id="OverflowOrder"
    v-on:click.native="listenClick"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <!-- 按钮区 -->
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
    <el-header id="elheader" class="header" height="auto">
      <el-form
        ref="dtData"
        :model="dtData"
        label-width="86px"
        class="FormInput"
        inline
        label-position="left"
        :rules="dtDataRules"
      >
        <el-form-item class="formItem disableType" label="入库类型：" prop="WarehousingType">
          <!-- <el-select
            v-model="dtData.WarehousingType"
            placeholder="请选择"
            style="width:200px;"
            disabled
          >
            <el-option label="盘盈入库" :value="4"></el-option>
          </el-select>-->
          <div v-if="dtData.WarehousingType==4" class="bbfe6">盘盈入库</div>
        </el-form-item>
        <el-form-item class="formItem" label="入库日期：" prop="warehousingDate">
          <el-date-picker
            v-model="dtData.warehousingDate"
            type="date"
            placeholder="选择日期"
            style="width:200px"
            :readonly="selectRow.auditStatus === 2"
          ></el-date-picker>
        </el-form-item>
        <el-form-item class="formItem" label="编号：" prop="warehousingOrder" label-width="58px">
          <div style="height:32px;min-width:128px;" class="bbfe6">{{dtData.warehousingOrder}}</div>
        </el-form-item>
        <el-form-item class="formItem" label="入库状态：" label-width="76px">
          <div style="height:32px;" class="bbfe6">
            <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待入库</span>
            <span v-if="dtData.auditStatus==0">入库待审核</span>
            <span v-if="dtData.auditStatus==1">审核未通过</span>
            <span v-if="dtData.auditStatus==2">入库完成</span>
          </div>
        </el-form-item>
      </el-form>
    </el-header>
    <!-- 列表区 -->
    <el-main>
      <!-- 物料弹框 -->
      <div class="findBox" :style="popoverStyle" v-show="popoverState" v-on:click.stop="1==1">
        <el-table :data="materielData" :height="250" border @row-click="rowClick">
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
      <!-- 工具栏弹框 -->
      <div class="findBox" :style="menuStyle" v-show="menuState">
        <ul class="menuUl">
          <li @click="doMenuAdd">增加行</li>
          <li @click="doMenuDel">删除行</li>
        </ul>
      </div>
      <div>
        <el-table
          id="tableData"
          ref="table"
          :height="tableHeight"
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

          <el-table-column prop="materialCode" label="物料代码" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'materialCode',1)"
                :class="{validStyle:scope.row.materialCode.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.materialCode.isShow"
                >{{scope.row.materialCode.value}}</div>
                <el-input
                  v-show="scope.row.materialCode.isShow"
                  v-model="scope.row.materialCode.value"
                  @keyup.enter.native="getMaterielDataIndex(scope.row)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.materialCode"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
                type="materialCode"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>
          <el-table-column prop="materialName" label="物料名称" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'materialName',1)"
                :class="{validStyle:scope.row.materialName.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.materialName.isShow"
                >{{scope.row.materialName.value}}</div>
                <el-input
                  v-show="scope.row.materialName.isShow"
                  v-model="scope.row.materialName.value"
                  @keyup.enter.native="getMaterielDataIndex(scope.row)"
                  :id="scope.row.id+'___materialName'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.materialName"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
                type="materialName"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>
          <el-table-column prop="spec" label="规格型号">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div :class="{validStyle:scope.row.spec.valid}">
                <div class="tableTd">{{scope.row.spec.value}}</div>
              </div>-->
              <EditInput
                v-model="scope.row.spec"
                @clickEvent="clickEvent"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
            </template>
          </el-table-column>
          <el-table-column prop="colorName" label="颜色">
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <EditInput
                v-model="scope.row.colorName"
                @clickEvent="clickEvent"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
            </template>
          </el-table-column>
          <el-table-column prop="defaultWarehouseName" label="收料仓库">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>收料仓库</span>
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
                @clickEventAfter="clickEventAfter"
                :itemList="scope.row"
                :data="warehouseData"
                :isShowFlag="isShowFlag"
                label="warehouseName"
                @change="WarehouseChange(scope.row)"
              ></EditSelect>
            </template>
          </el-table-column>
         
          <el-table-column prop="PaidIn" label="实存数量">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>实存数量</span>
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

          <el-table-column prop="overFlow" label="盘盈数量">
            <template slot="header">
              <span class="tableHeader">
                <span>盘盈数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{redCol:scope.row.overFlow.value<=0,greCol:scope.row.overFlow.value>0}">
                <!-- {{scope.row.overFlow.value}} -->
                <EditInput
                  v-model="scope.row.overFlow"
                  @clickEvent="clickEvent"
                  :itemList="scope.row"
                  :isShowFlag="false"
                ></EditInput>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="account" label="账存数量">
            <template slot="header">
              <span class="tableHeader">
                <span>账存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{scope.row.account.value}}</div> -->
              <EditInput
                v-model="scope.row.account"
                @clickEvent="clickEvent"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
            </template>
          </el-table-column>
           <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{scope.row.warehouseUnitName.value}}</div> -->
              <EditInput
                v-model="scope.row.warehouseUnitName"
                @clickEvent="clickEvent"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
              <EditInput
                v-if="!scope.row.warehouseUnitName"
                v-model="scope.row.unit"
                @clickEvent="clickEvent"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'unitPrice')"
                :class="{validStyle:scope.row.unitPrice.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.unitPrice.isShow"
                >{{scope.row.unitPrice.value}}</div>
                <el-input
                  v-show="scope.row.unitPrice.isShow"
                  v-model="scope.row.unitPrice.value"
                  :id="scope.row.id+'___unitPrice'"
                  size="mini"
                  @blur="unitPriceBlur(scope.row)"
                  @keyup.enter.native="unitPriceBlur(scope.row)"
                ></el-input>
              </div>-->
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
              <!-- <div
                @click="chengenum($event,scope.row,'amount')"
                :class="{validStyle:scope.row.amount.valid}"
              >
                <div class="tableTd" v-show="!scope.row.amount.isShow">{{scope.row.amount.value}}</div>
                <el-input
                  v-show="scope.row.amount.isShow"
                  v-model="scope.row.amount.value"
                  :id="scope.row.id+'___amount'"
                  size="mini"
                  @blur="amountBlur(scope.row)"
                  @keyup.enter.native="amountBlur(scope.row)"
                ></el-input>
              </div>-->
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
          <el-table-column prop="batchNumber" label="批号">
            <template slot="header">
              <span class="tableHeader">
                <span>批号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'batchNumber')"
                :class="{validStyle:scope.row.batchNumber.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.batchNumber.isShow"
                >{{scope.row.batchNumber.value}}</div>
                <el-input
                  v-show="scope.row.batchNumber.isShow"
                  v-model="scope.row.batchNumber.value"
                  :id="scope.row.id+'___batchNumber'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.batchNumber"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>

          <!-- <el-table-column prop="unit" label="基本单位">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              //<div class="tableTd">{{scope.row.unit.value}}</div>
              <EditInput v-model="scope.row.unit" @clickEvent="clickEvent" :itemList="scope.row" :isShowFlag="false"></EditInput>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitNumber" label="基本单位数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              //<div class="tableTd">{{scope.row.baseUnitNumber.value}}</div>
              <EditInput v-model="scope.row.baseUnitNumber" @clickEvent="clickEvent" :itemList="scope.row" :isShowFlag="false"></EditInput>
            </template>
          </el-table-column>-->

          <el-table-column prop="defaultWarehouseName" label="有效期" width="145">
            <template slot="header">
              <span class="tableHeader">
                <span>有效期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'validityPeriod')"
                :class="{validStyle:scope.row.validityPeriod.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.validityPeriod.isShow"
                >{{scope.row.validityPeriod.value | formatTDate}}</div>
                <el-date-picker
                  style="width:140px;"
                  v-model="scope.row.validityPeriod.value"
                  v-show="scope.row.validityPeriod.isShow"
                  :id="scope.row.id+'___validityPeriod'"
                  type="date"
                  placeholder="选择日期"
                  v-on:keydown.native="onKeydown($event)"
                ></el-date-picker>
              </div>-->
              <EditInput
                v-model="scope.row.validityPeriod"
                @clickEvent="clickEvent"
                @onKeydown="onKeydown"
                type="date"
                :itemList="scope.row"
                :isShowFlag="isShowFlag"
              ></EditInput>
            </template>
          </el-table-column>
          <el-table-column
            prop="remark"
            label="备注"
            width="280"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>备注</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.remark.valid}"
                @click="chengenum($event,scope.row,'remark')"
              >
                <el-tooltip
                  v-if="scope.row.remark.length>=20"
                  class="item"
                  effect="light"
                  :content="scope.row.remark"
                  :open-delay="300"
                  placement="top-end"
                >
                  <div class="ellipsis">{{ scope.row.remark }}</div>
                </el-tooltip>
                <div
                  v-show="!scope.row.remark.isShow"
                  class="tableTd ellipsis"
                >{{ scope.row.remark.value }}</div>
                <el-input
                  v-show="scope.row.remark.isShow"
                  :id="scope.row.id+'___remark'"
                  v-model="scope.row.remark.value"
                  size="mini"
                />
              </div>-->
              <EditInput
                v-model="scope.row.remark"
                type="remark"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
              />
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <!-- 页脚区 -->
    <div id="elfooter" class="fromCenter">
      <el-form label-width="76px" class="FormInput" inline>
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{dtData.operatorName}}</div>
          </div>
        </el-form-item>
        <el-form-item label="盘点人员：">
          <div class="falseIp" v-if="selectRow.auditStatus == 2">{{dtData.receiptName}}</div>
          <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.receiptId"
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
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit" title="盘盈入库单查询"></InboundOrder>
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import {
  formatDate,
  keepTwoDecimalFull,
  accMul,
  getFloat,
  isRealNum
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from "./components/InboundOrder";ProcurementPut
import InboundOrder from "./components/InboundOrder";
import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import { setTimeout } from "timers";
import { Receipt } from "@/utils/Receipt";
import BigNumber from "bignumber.js";

export default {
  name: "viewsinventoryOverflowOrderindexvue",
  mixins: [Receipt],
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
      dtData: {
        WarehousingType: 4,
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
        auditTime: ""
      },
      dtDataRules: {
        WarehousingType: [
          { required: true, message: "请选择入库类型", trigger: "change" }
        ],
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
          materialCode: {
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          materialName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          spec: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          colorName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            //收料仓库
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          batchNumber: {
            //批号
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          unit: {
            //基本单位
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            //仓库单位
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            //基本单位数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
            //实存数量
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          overFlow: {
            //盘盈数量
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
          validityPeriod: {
            //有效期
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            //有效期
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
        "materialCode",
        "materialName",
        "defaultWarehouseName",
        "PaidIn",
        "unitPrice",
        "amount",
        "batchNumber",
        "validityPeriod",
        "remark"
      ],
      materialItem: {},
      isShowFlag: true,
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
    this.fullscreenLoading = true;
    //获取按钮权限
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
  },
  async mounted() {
    this.mountedState = true;
    window.addEventListener("keydown", this.onKeydown);
    this.setStyle(); //设置样式
    // this.tableData2 = [...this.tableData]; //克隆原始table数据
    this.warehouseData = await this.getWarehouseData(); //仓库数据
    this.getMaterielData(); //物料数据
    this.getUserCompany(); //收货员列表
    if (this.$route.params.type) {
      if (this.$route.params.type == 3) {
        this.resetData();
      } else {
        this.OtherData = this.$route.params;
        this.OnBtnSaveSubmit(this.OtherData.item);
      }
    } else {
      var code = Storage.LStorage.get("OverflowOrderCode");
      if (!code) {
        this.getCode();
      } else {
        this.dtData.warehousingOrder = code;
      }
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = this.setTable(this.tableLen); //添加30行
      this.fullscreenLoading = false;
    }
    this.mountedState = false;
  },
  async activated() {
    if (this.mountedState == true) {
      return;
    }
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
        // case 1: //物料代码
        //   if (state == 2) {
        //     //按下Enter请求数据
        //     this.getMaterielData(itemList,this.pageIndex=1);
        //   }
        //   break;
        case "materialCode": //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.pageIndex = 1;
            this.getMaterielData(itemList, type);
            // this.setMaterielData(item);
            // this.materielData = this.materielData2;
          }
          break;
        case "materialName": //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.pageIndex = 1;
            this.getMaterielData(itemList, type);
            // this.setMaterielData(item);
            // this.materielData = this.materielData2;
          }
        break;
        case 2: //实收数量
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
      if (type === "materialName" || type === "materialCode") {
        this.findBox(item);
        this.getMaterielData(itemList,type);
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
    //   //按下Tab键
    //   if (event.keyCode !== 9) return;
    //   var data = this.findCheck(event);
    //   this.setCheck(data);
    // },
    // findCheck(event) {
    //   //寻找Tab下一个元素
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
    //         var id = this.tableData[index][name]["id"];
    //         document.getElementById(id).focus();
    //         document.getElementById(id).select();
    //         if (
    //           document.getElementById(id).getAttribute("readonly") == "readonly"
    //         ) {
    //           document.getElementById(id).click();
    //         }
    //       });
    //     }
    //   }
    // },
    defaultAll() {
      //重置所有切换了的Input
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
    },
    //设置Tab切换结束
    resetData() {
      //初始化数据
      this.getCode();
      this.resetDeafultData();
    },
    resetDeafultData() {
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData = [];
      this.tableData = this.setTable(this.tableLen);
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];
      this.isShowFlag = true;

      this.dtData.WarehousingType = 4;
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
    async computeAccount(item) {
      // console.log(item.materialCode.value);
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
        if (item.PaidIn.value !== "" && item.PaidIn.value !== null) {
          item.overFlow.value = getFloat(
            new BigNumber(item.PaidIn.value)
              .minus(item.account.value)
              .toNumber(),
            4
          );
        }
      }
    },
    getWarehouseAmount(materialId, warehouseId) {
      return new Promise(function(reslove, reject) {
        var data = {
          postData: {
            materialId,
            warehouseId
          }
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
      // var tr = this.setTable(1)
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
      //合并物料相同开始
      this.computeAccount(item);
      var currentNum = -1;
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          currentNum = i;
          break;
        }
      }

      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          continue;
        }
        if (
          this.doItem["materialCode"]["key"] ==
          this.tableData[i]["materialCode"]["key"]
        ) {
          if (
            this.doItem["defaultWarehouseName"]["key"] ==
            this.tableData[i]["defaultWarehouseName"]["key"]
          ) {
            this.listenClick();
            this.$set(
              this.tableData[i]["defaultWarehouseName"],
              `isShow`,
              true
            );
            this.$nextTick(() => {
              var id_ = this.tableData[i]["defaultWarehouseName"].id;
              document.getElementById(id_).focus();
              document.getElementById(id_).select();
            });
            this.tableData.splice(currentNum, 1, this.setTable(1)[0]);
            return;
          }
        }
      }
      console.log(item)
      //合并物料相同结束
      this.warehouseData.map(data => {
        if (item.defaultWarehouseName.key == data.id) {
          item.defaultWarehouseName.value = data.warehouseName;
        }
      });
      item.valid = false;
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
        url: "warehouse/api/TWMProfitMain/GetOrderNo",
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = "";
        } else {
          this.dtData.warehousingOrder = res;
          Storage.LStorage.set("OverflowOrderCode", res);
        }
        // this.dtData.warehousingOrder = res;
        // Storage.LStorage.set("OverflowOrderCode", res);
      });
    },
    async addPutInStorage(state) {
      //新增
      if (state == 1 || state == 2) {
        var flag = false; //验证数据是否为空
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
        var computeFlag = true; //验证：实存数量不能小于等于账存数量(小于请去盘亏出库操作)
        checkSelect.map(item => {
          //验证数据
          item.materialCode.valid = false;
          item.materialName.valid = false;
          item.spec.valid = false;
          item.colorName.valid = false;
          item.defaultWarehouseName.valid = false;
          item.PaidIn.valid = false;
          if (item.editState) {
            if (item.materialCode.key === "") {
              //验证物料代码
              item.materialCode.valid = true;
              item.materialName.valid = true;
              item.spec.valid = true;
              item.colorName.valid = true;
              flag = false;
            }
            if (item.defaultWarehouseName.key === "") {
              //验证物仓库ID
              item.defaultWarehouseName.valid = true;
              flag = false;
            }
            if (item.PaidIn.value === "" || item.PaidIn.value === null) {
              //验证实存数量
              item.PaidIn.valid = true;
              flag = false;
            }
            if (item.PaidIn.value !== "" || item.PaidIn.value !== null) {
              //验证：实存数量不能小于等于账存数量(小于请去盘亏出库操作)
              if (
                parseFloat(item.PaidIn.value) -
                  parseFloat(item.account.value) <=
                0
              ) {
                item.PaidIn.valid = true;
                computeFlag = false;
              }
            }
            var _LogName = "";
            var param = {
              id: 0,
              mainId: 0,
              materialId: parseFloat(item.materialCode.key) || "",
              warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
              actualNumber:item.overFlow.value,
                // parseFloat(item.PaidIn.value) -
                //   parseFloat(item.account.value) || "",
              accountNum: item.account.value
            };
            _LogName += `物料:${item.materialName.value} 盘盈入库 ${param.actualNumber}${item.unit.value} 到 ${item.defaultWarehouseName.value}`;
            if (
              item.batchNumber.value !== "" &&
              item.batchNumber.value !== null
            ) {
              //批号
              param.batchNumber = item.batchNumber.value;
            }
            if (
              item.remark.value !== "" &&
              item.remark.value !== null
            ) {
              //备注
              param.remark = item.remark.value;
            }
            if (item.unitPrice.value !== "" && item.unitPrice.value !== null) {
              //单价
              param.unitPrice = parseFloat(item.unitPrice.value) || 0;
            }
            if (item.amount.value !== "" && item.amount.value !== null) {
              //金额
              param.amount = parseFloat(item.amount.value) || 0;
            }
            if (
              item.validityPeriod.value !== "" &&
              item.validityPeriod.value !== null
            ) {
              //日期
              param.validityPeriod = formatDate(item.validityPeriod.value) + "";
            }

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
        if (!computeFlag) {
          this.$message.error(
            "实存数量不能小于等于账存数量(小于请去盘亏出库操作)"
          );
          return;
        }

        var currData = {
          operatorId: 0,
          warehousingType: this.dtData.WarehousingType,
          warehousingDate: formatDate(this.dtData.warehousingDate),
          warehousingOrder: this.dtData.warehousingOrder,
          childList: childList
        };

        if (this.dtData.receiptId !== "") {
          currData.receiptId = this.dtData.receiptId;
        }
      }

      var method = "";
      if (state == 1) {
        currData.id = 0;
        var data = RequestObject.CreatePostObject(currData);
        this.postData(data, state);
      }

      if (state == 2) {
        if (this.selectRow.auditStatus == 2) {
          this.$message.error("该入库单为通过状态，无法编辑");
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
      }

      if (state == 3) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该入库单是通过状态，无法删除");
          return;
        }
        if (this.selectRow.id == undefined) {
          this.$message.error("请选择删除的数据");
          return;
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的盘盈入库单`
        };
        var data = { postData: currData };

        this.$confirm("是否删除入库单？", "确认信息", {
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
        url: "/warehouse/api/TWMProfitMain",
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
            Storage.LStorage.remove("OverflowOrderCode");
          }
          if (state == 3) {
            setTimeout(() => {
              this.tableData = [];
              this.resetDeafultData();
              var code = Storage.LStorage.get("OverflowOrderCode");
              this.dtData.warehousingOrder = code;
              // this.setTable(30);
            }, 50);
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
    setTable(num) {
      // 初始化数据
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
    },
    // getMaterielData(item) {
    //   //获取物料档案信息
    //   this.materielData = [];
    //   var queryData = [];
    //   if (item) {
    //     queryData.push({
    //       column: "materialCode",
    //       content: item.materialCode.value,
    //       condition: 6
    //     });
    //     queryData.push({
    //       column: "materialName",
    //       content: item.materialName.value,
    //       condition: 6
    //     });
    //   }

    //   var reqObject;
    //   reqObject = RequestObject.LonginBookObject(
    //     true,
    //     this.pageSize,
    //     this.pageIndex,
    //     null,
    //     queryData
    //   );
    //   request({
    //     url: "/basicset/api/TBMMaterialFile",
    //     method: "get",
    //     params: {
    //       requestObject: JSON.stringify(reqObject)
    //     }
    //   }).then(res => {
    //     this.loading = false;
    //     if (res.code == -1) {
    //       this.$confirm(res.info, "错误信息", {
    //         confirmButtonText: "确定",
    //         type: "error",
    //         showCancelButton: false
    //       });
    //     } else {
    //       this.materielData = res.data;
    //       this.totalCount = res.totalNumber;
    //     }
    //   });
    // },
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
      this.doItem.materialCode.valid = false;
      this.doItem.materialName.valid = false;
      this.doItem.spec.valid = false;
      this.doItem.colorName.valid = false;

      this.doItem.materialCode.value = row.materialCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.materialName;
      this.doItem.spec.value = row.spec;
      this.doItem.colorName.value = row.colorName;

      this.doItem.warehouseUnitName.value = row.warehouseUnitName;
      this.doItem.unit.value = row.baseUnitName;
      this.doItem.baseUnitNumber.value = row.baseUnitNumber;
      // this.computeAccount(this.doItem);
      // if (row.warehouseUnitName == null) {
      //   this.doItem.unit.value = row.baseUnitName;
      // } else {
      //   this.doItem.unit.value = row.warehouseUnitName;
      // }
      this.doItem.defaultWarehouseName.value = "";
      this.doItem.defaultWarehouseName.key = "";
      if (row.defaultWarehouseId !== null) {
        this.doItem.defaultWarehouseName.value = row.defaultWarehouseName;
        this.doItem.defaultWarehouseName.key = row.defaultWarehouseId;
        this.computeAccount(this.doItem);
      }

      //合并物料相同开始
      var currentNum = -1;
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          currentNum = i;
          break;
        }
      }
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          continue;
        }
        if (
          this.doItem["materialCode"]["key"] ==
          this.tableData[i]["materialCode"]["key"]
        ) {
          if (
            this.doItem["defaultWarehouseName"]["key"] ==
            this.tableData[i]["defaultWarehouseName"]["key"]
          ) {
            this.listenClick();
            this.$set(
              this.tableData[i]["defaultWarehouseName"],
              `isShow`,
              true
            );
            this.$nextTick(() => {
              var id_ = this.tableData[i]["defaultWarehouseName"].id;
              document.getElementById(id_).focus();
              document.getElementById(id_).select();
            });
            this.tableData.splice(currentNum, 1, this.setTable(1)[0]);
            return;
          }
        }
      }
      //合并物料相同结束

      this.listenClick();
      this.$set(this.doItem["defaultWarehouseName"], `isShow`, true);
      this.$nextTick(() => {
        var id_ = this.doItem["defaultWarehouseName"].id;
        document.getElementById(id_).focus();
        document.getElementById(id_).select();
        if (this.doItem.defaultWarehouseName.key == "") {
          if (
            document.getElementById(id_).getAttribute("readonly") == "readonly"
          ) {
            document.getElementById(id_).click();
          }
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
              return BigNumber(prev)
                .plus(curr)
                .toNumber();
            } else {
              return prev;
            }
          }, 0);
          if (index == 6 || index == 7 || index == 8 || index == 11) {
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
      //通过不能编辑
      if (this.selectRow.auditStatus === 2) {
        return;
      }

      event.stopPropagation();

      // this.doDefault(this.doItem);
      // this.popoverState = false;
      this.defaultAll(this.doItem);
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
          this.materialItem = [];
          this.pageIndex = 1;
          this.getMaterielData();
        }
      });
    },
    // findBox(item, name) {
    //   var IH = document.getElementById(item.id).offsetHeight + 8;
    //   var IW = document.getElementById(item.id).offsetWidth + 24;
    //   if (this.$store.getters.sidebar.opened) {
    //     var ol = 210;
    //   } else {
    //     var ol = 54;
    //   }
    //   var wl = document.documentElement.clientWidth; // 屏幕宽度
    //   var wh = document.documentElement.clientHeight; // 屏幕宽度
    //   var PoL = document.getElementById(item.id).getBoundingClientRect().left; // 弹框left值
    //   var PoT = document.getElementById(item.id).getBoundingClientRect().top; // 弹框top值
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
    //   this.defaultAll();
    //   // this.doDefault(this.doItem);
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
            data[i]["isShow"] = false;
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
      this.unitPriceBlur(item); //单价算金额
      this.amountBlur(item); //金额算单机
      if (item.account.value !== "" && item.account.value !== null) {
        item.overFlow.value = getFloat(
          new BigNumber(item.PaidIn.value).minus(item.account.value).toNumber(),
          4
        );
      }
      item.PaidIn.valid = false;
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
      if (item.overFlow.value !== "") {
        item.unitPrice.value = parseFloat(item.unitPrice.value);
        item.unitPrice.value = keepTwoDecimalFull(item.unitPrice.value);
        item.amount.value = keepTwoDecimalFull(
          accMul(item.overFlow.value, item.unitPrice.value)
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
      if (item.overFlow.value !== "") {
        item.amount.value = parseFloat(item.amount.value);
        item.unitPrice.value = keepTwoDecimalFull(
          item.amount.value / item.overFlow.value
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
    OnBtnSaveSubmit(row, state) {
      if (row.auditStatus == 2) {
        this.isShowFlag = false;
      } else {
        this.isShowFlag = true;
      }
      //根据ID获取数据
      this.fullscreenLoading = true;
      this.selectRow = this.cloneObject(row);
      this.dtData.WarehousingType = row.warehousingType;
      this.dtData.warehousingDate =
        row.warehousingDate != null
          ? new Date(row.warehousingDate.split("T")[0])
          : "";
      this.dtData.warehousingOrder = row.warehousingOrder;
      // console.log(row);

      this.dtData.operatorName = row.operatorName;
      this.dtData.operatorId = row.operatorId;

      this.dtData.receiptName = row.receiptName;
      this.dtData.receiptId = row.receiptId;

      this.dtData.auditName = row.auditName;
      this.dtData.auditId = row.auditId;
      this.dtData.auditStatus = row.auditStatus;
      this.dtData.auditTime =
        row.auditTime != null ? row.auditTime.split("T")[0] : "";
      if (!state) {
        // var rqs = RequestObject.LonginBookObject(false, 0, 0, row.id);
        // this.fullscreenLoading = true;
        request({
          url: "warehouse/api/TWMProfitMain/GetDetailList",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
          // setTimeout(() => {
          // this.fullscreenLoading = false;
          // }, 500);
          if (res.code === 0) {
            this.setTableData(res.data);
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
          materialId: parseFloat(item.materialCode.key) || "",
          warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
          actualNumber:
            parseFloat(item.PaidIn.value) - parseFloat(item.account.value) ||
            "",
          accountNum: item.account.value || ""
        };
        _LogName += `物料:${item.materialName.value} 盘盈入库 ${param.actualNumber}${item.unit.value} 到 ${item.defaultWarehouseName.value}`;

        if (item.batchNumber.value !== "" && item.batchNumber.value !== null) {
          //批号
          param.batchNumber = item.batchNumber.value;
        }
        if (item.remark.value !== "" && item.remark.value !== null) {
          //备注
          param.remark = item.remark.value;
        }
        if (item.unitPrice.value !== "" && item.unitPrice.value !== null) {
          //单价
          param.unitPrice = parseFloat(item.unitPrice.value) || 0;
        }
        if (item.amount.value !== "" && item.amount.value !== null) {
          //金额
          param.amount = parseFloat(item.amount.value) || 0;
        }
        if (
          item.validityPeriod.value !== "" &&
          item.validityPeriod.value !== null
        ) {
          //日期
          param.validityPeriod = formatDate(item.validityPeriod.value) + "";
        }

        param._LogName = _LogName;
        childList.push(param);
      });
      var currData = {
        id: this.selectRow.id,
        operatorId:
          this.selectRow.operatorId != null ? this.selectRow.operatorId : 0,
        warehousingType: this.selectRow.warehousingType,
        warehousingDate:
          this.selectRow.warehousingDate != null ||
          this.selectRow.warehousingDate != ""
            ? this.selectRow.warehousingDate.split("T")[0]
            : "",
        warehousingOrder: this.selectRow.warehousingOrder,
        childList: childList
      };
      // console.log(currData);
      this.PostDataEdit = currData;
    },
    setTableData(dt) {
      //将数据导入table
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            id: newGuid(),
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            id: newGuid(),
            value: item.materialName,
            isShow: false,
            valid: false
          },
          spec: {
            id: newGuid(),
            value: item.spec,
            isShow: false,
            valid: false
          },
          colorName: {
            id: newGuid(),
            value: item.colorName,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            //收料仓库
            id: newGuid(),
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          batchNumber: {
            id: newGuid(),
            value: item.batchNumber, //批号
            isShow: false,
            valid: false
          },
          unit: {
            //单位
            id: newGuid(),
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            id: newGuid(),
            value:
              item.warehouseUnitName == null
                ? item.baseUnitName
                : item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            id: newGuid(),
            value: item.baseUnitNumber,
            isShow: false,
            valid: false
          },
          PaidIn: {
            //实存数量
            id: newGuid(),
            value: parseFloat(item.accountNum) + parseFloat(item.actualNumber),
            isShow: false,
            valid: false
          },
          overFlow: {
            //盘盈数量
            id: newGuid(),
            value: item.actualNumber,
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
            id: newGuid(),
            value: item.accountNum,
            isShow: false,
            valid: false
          },
          unitPrice: {
            id: newGuid(),
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          amount: {
            id: newGuid(),
            value: item.amount,
            isShow: false,
            valid: false
          },
          validityPeriod: {
            id: newGuid(),
            value:
              item.validityPeriod != null
                ? new Date(item.validityPeriod.split("T")[0])
                : "",
            isShow: false,
            valid: false
          },
          remark: {
            //有效期
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
        listArr.unshift(list);
      });
      this.cloneTable = [...listArr];
      this.setCurrData(this.cloneTable);
      setTimeout(() => {
        // this.tableData = [...listArr, ...this.setTable(30)];
        if (listArr.length < this.tableLen) {
          this.tableData = [
            ...listArr,
            ...this.setTable(this.tableLen - listArr.length)
          ];
        } else {
          this.tableData = [...listArr, ...this.setTable(1)];
        }
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
        this.$message.error("该入库单已经为通过状态");
        return;
      }
      // var data = RequestObject.CreatePostObject({
      //   id: this.selectRow.id,
      //   auditStatus: state,
      //   ChildList: []
      // });
      // console.log(this.selectRow.auditStatus)
       var text = '确定审核通过吗？'
      if (state == 1) {
        text = '确定审核未通过吗？'
      }
      this.$confirm(text, {
        type: "warning"
      })
        .then(() => {
          var data = {
            postData: {
              id: this.selectRow.id,
              auditStatus: state
            },
            postDataEdit: {
              id: this.selectRow.id,
              auditStatus: this.selectRow.auditStatus
            }
          };
          this.fullscreenLoading = true;
          request({
            url: "/warehouse/api/TWMProfitMain/ProfitAudit",
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
        })
        .catch(res => {});
    },
    doOtherWhCancelAudit() {
      //撤销审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus != 2) {
        this.$message.error("该入库单不是通过状态，无法撤销");
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
#OverflowOrder /deep/ {
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
  .el-select .el-input.is-disabled .el-input__inner,
  .el-input.is-disabled .el-input__icon {
    cursor: default;
    background: #fff;
  }
  .disableType i {
    border-top: 1px solid rgb(220, 223, 230);
    border-bottom: 1px solid rgb(220, 223, 230);
    height: 32px;
  }
  .el-table {
    overflow: visible !important;
  }
  #elfooter .el-input__inner {
    border: 0px;
    border-radius: 0px;
    border-bottom: 1px solid #dcdfe6;
  }
  .redCol {
    color: red;
  }
  .greCol {
    color: #42b983;
  }
}
@import "@/styles/receipts.scss";
</style>
