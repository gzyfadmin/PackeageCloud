<template>
  <el-container
    id="OverflowOrder"
    v-loading="fullscreenLoading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    @click.native="listenClick"
  >
    <!-- 按钮区 -->
    <!-- <div style="padding:5px 0px 0px 20px;height:37px;font-size:0px;">
      <div v-if="OtherData.type!=1&&btnAip.add">
        <el-button-group class="groupBtn">
          <el-button
            v-if="btnAip.add.buttonCaption&&addControl"
            type="default"
            @click="addPutInStorage(1)"
          >{{ btnAip.add.buttonCaption }}</el-button>

          <el-button type="default" @click="resetData()">重置</el-button>

          <el-button
            v-if="btnAip.modify.buttonCaption"
            type="default"
            @click="addPutInStorage(2)"
          >{{ btnAip.modify.buttonCaption }}</el-button>
          <el-button
            v-if="btnAip.delete.buttonCaption"
            type="default"
            @click="addPutInStorage(3)"
          >{{ btnAip.delete.buttonCaption }}</el-button>
          <el-button
            v-if="btnAip.query.buttonCaption"
            type="default"
            @click="handelAddClick"
          >{{ btnAip.query.buttonCaption }}</el-button>
        </el-button-group>
        <span />
        <el-dropdown
          v-if="btnAip.review.buttonCaption"
          split-button
          type="default"
          class="dropdown"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
          style="height: 34px;"
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
          v-if="btnAip.add.buttonCaption&&addControl"
          type="default"
          @click="addPutInStorage(1)"
        >{{ btnAip.add.buttonCaption }}</el-button>

        <el-button type="default" @click="resetData()">重置</el-button>

        <el-button
          v-if="btnAip.modify.buttonCaption"
          type="default"
          @click="addPutInStorage(2)"
        >{{ btnAip.modify.buttonCaption }}</el-button>
        <el-button
          v-if="btnAip.delete.buttonCaption"
          type="default"
          @click="addPutInStorage(3)"
        >{{ btnAip.delete.buttonCaption }}</el-button>
        <el-button
          v-if="btnAip.query.buttonCaption"
          type="default"
          @click="handelAddClick"
        >{{ btnAip.query.buttonCaption }}</el-button>
        <el-dropdown
          v-if="btnAip.review.buttonCaption"
          split-button
          type="default"
          class="dropdown"
          style="height: 34px;"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
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
        <el-form-item class="formItem disableType" label="出库类型：" prop="WarehousingType">
          <el-select
            v-model="dtData.WarehousingType"
            placeholder="请选择"
            style="width:200px;"
            disabled
          >
            <el-option label="盘亏出库" :value="4" />
          </el-select>
        </el-form-item>
        <el-form-item class="formItem" label="出库日期：" prop="warehousingDate">
          <el-date-picker
            v-model="dtData.warehousingDate"
            type="date"
            placeholder="选择日期"
            style="width:200px"
          />
        </el-form-item>
        <el-form-item class="formItem" label="编号：" prop="warehousingOrder" label-width="58px">
          <div class="bbfe6" style="height:32px;min-width:128px;">{{ dtData.warehousingOrder }}</div>
        </el-form-item>
        <el-form-item class="formItem" label="出库状态：" label-width="76px">
          <div class="bbfe6" style="height:32px;">
            <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待出库</span>
            <span v-if="dtData.auditStatus==0">出库待审核</span>
            <span v-if="dtData.auditStatus==1">审核未通过</span>
            <span v-if="dtData.auditStatus==2">出库完成</span>
          </div>
        </el-form-item>
      </el-form>
    </el-header>
    <!-- 列表区 -->
    <el-main id="elmain">
      <!-- 物料弹框 -->
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
      <!-- 工具栏弹框 -->
      <div v-show="menuState" class="findBox" :style="menuStyle">
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
                  {{ scope.$index+1 }}
                  <i class="el-icon-setting" />
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="物料代码">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.materialCode.valid}"
                @click="chengenum($event,scope.row,'materialCode',1)"
              >
                <div
                  v-show="!scope.row.materialCode.isShow"
                  class="tableTd"
                >{{ scope.row.materialCode.value }}</div>
                <el-input
                  v-show="scope.row.materialCode.isShow"
                  :id="scope.row.id+'___materialCode'"
                  v-model="scope.row.materialCode.value"
                  size="mini"
                  @keyup.enter.native="getMaterielDataIndex(scope.row)"
                />
              </div>-->
              <EditInput
                v-model="scope.row.materialCode"
                type="materialCode"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>
          <el-table-column prop="materialName" label="物料名称">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.materialName.valid}"
                @click="chengenum($event,scope.row,'materialName',1)"
              >
                <div
                  v-show="!scope.row.materialName.isShow"
                  class="tableTd"
                >{{ scope.row.materialName.value }}</div>
                <el-input
                  v-show="scope.row.materialName.isShow"
                  :id="scope.row.id+'___materialName'"
                  v-model="scope.row.materialName.value"
                  size="mini"
                  @keyup.enter.native="getMaterielDataIndex(scope.row)"
                />
              </div>-->
              <EditInput
                v-model="scope.row.materialName"
                type="materialName"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>
          <el-table-column prop="spec" label="规格型号">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.spec.valid}"
                @click="chengenum($event,scope.row,'spec')"
              >
                <div class="tableTd">{{ scope.row.spec.value }}</div>
              </div>-->
              <EditInput
                v-model="scope.row.spec"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>
          <el-table-column prop="colorName" label="颜色">
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.colorName.valid}"
                @click="chengenum($event,scope.row,'colorName')"
              >
                <div class="tableTd">{{ scope.row.colorName.value }}</div>
              </div>-->
              <EditInput
                v-model="scope.row.colorName"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>
          <el-table-column prop="defaultWarehouseName" label="发货仓库">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>发货仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.defaultWarehouseName.valid}"
                @click="chengenum($event,scope.row,'defaultWarehouseName')"
              >
                <div
                  v-show="!scope.row.defaultWarehouseName.isShow"
                  class="tableTd"
                >{{ scope.row.defaultWarehouseName.value }}</div>
                <el-select
                  v-show="scope.row.defaultWarehouseName.isShow"
                  :id="scope.row.id+'___defaultWarehouseName'"
                  v-model="scope.row.defaultWarehouseName.key"
                  placeholder="请选择"
                  @change="WarehouseChange(scope.row.defaultWarehouseName)"
                >
                  <el-option
                    v-for="item in warehouseData"
                    :key="item.id"
                    :label="item.warehouseName"
                    :value="item.id"
                  />
                </el-select>
              </div>-->
              <EditSelect
                v-model="scope.row.defaultWarehouseName"
                :data="warehouseData"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                label="warehouseName"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @change="WarehouseChange(scope.row)"
              />
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
                :class="{validStyle:scope.row.PaidIn.valid}"
                @click="chengenum($event,scope.row,'PaidIn')"
              >
                <div
                  v-show="!scope.row.PaidIn.isShow"
                  class="tableTd"
                >{{ scope.row.PaidIn.value }}</div>
                <el-input
                  v-show="scope.row.PaidIn.isShow"
                  :id="scope.row.id+'___PaidIn'"
                  v-model="scope.row.PaidIn.value"
                  size="mini"
                  @blur="PaidInBlur(scope.row)"
                  @keyup.enter.native="PaidInBlur(scope.row)"
                />
              </div>-->
              <EditInput
                v-model="scope.row.PaidIn"
                :type="2"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column prop="overFlow" label="盘亏数量">
            <template slot="header">
              <span class="tableHeader">
                <span>盘亏数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                :class="{redCol:scope.row.overFlow.value<=0,greCol:scope.row.overFlow.value>0}"
              >{{ scope.row.overFlow.value }}</div>
              <!-- <EditInput
                v-model="scope.row.overFlow"
                @clickEvent="clickEvent"
                :itemList="scope.row"
                :isShowFlag="false"
              ></EditInput>-->
            </template>
          </el-table-column>

          <el-table-column prop="account" label="账存数量">
            <template slot="header">
              <span class="tableHeader">
                <span>账存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{ scope.row.account.value }}</div> -->
              <EditInput
                v-model="scope.row.account"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
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
                :class="{validStyle:scope.row.unitPrice.valid}"
                @click="chengenum($event,scope.row,'unitPrice')"
              >
                <div
                  v-show="!scope.row.unitPrice.isShow"
                  class="tableTd"
                >{{ scope.row.unitPrice.value }}</div>
                <el-input
                  v-show="scope.row.unitPrice.isShow"
                  :id="scope.row.id+'___unitPrice'"
                  v-model="scope.row.unitPrice.value"
                  size="mini"
                  @blur="unitPriceBlur(scope.row)"
                  @keyup.enter.native="unitPriceBlur(scope.row)"
                />
              </div>-->
              <EditInput
                v-model="scope.row.unitPrice"
                :type="3"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @blurCheck="blurCheck"
              />
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
                :class="{validStyle:scope.row.amount.valid}"
                @click="chengenum($event,scope.row,'amount')"
              >
                <div
                  v-show="!scope.row.amount.isShow"
                  class="tableTd"
                >{{ scope.row.amount.value }}</div>
                <el-input
                  v-show="scope.row.amount.isShow"
                  :id="scope.row.id+'___amount'"
                  v-model="scope.row.amount.value"
                  size="mini"
                  @blur="amountBlur(scope.row)"
                  @keyup.enter.native="amountBlur(scope.row)"
                />
              </div>-->
              <EditInput
                v-model="scope.row.amount"
                :type="4"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @blurCheck="blurCheck"
              />
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
                :class="{validStyle:scope.row.batchNumber.valid}"
                @click="chengenum($event,scope.row,'batchNumber')"
              >
                <div
                  v-show="!scope.row.batchNumber.isShow"
                  class="tableTd"
                >{{ scope.row.batchNumber.value }}</div>
                <el-input
                  v-show="scope.row.batchNumber.isShow"
                  :id="scope.row.id+'___batchNumber'"
                  v-model="scope.row.batchNumber.value"
                  size="mini"
                />
              </div>-->
              <EditInput
                v-model="scope.row.batchNumber"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <!-- <el-table-column
            prop="unit"
            label="基本单位"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              //<div class="tableTd">{{ scope.row.unit.value }}</div>
              <EditInput v-model="scope.row.unit" @clickEvent="clickEvent" :itemList="scope.row" :isShowFlag="false"></EditInput>
            </template>
          </el-table-column>

          <el-table-column
            prop="baseUnitNumber"
            label="基本单位数量"
            width="110"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              //<div class="tableTd">{{ scope.row.baseUnitNumber.value }}</div>
              <EditInput v-model="scope.row.baseUnitNumber" @clickEvent="clickEvent" :itemList="scope.row" :isShowFlag="false"></EditInput>
            </template>
          </el-table-column>-->

          <!-- <el-table-column
            prop="warehouseUnitName"
            label="仓库单位"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.warehouseUnitName.value }}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="validityPeriod" label="有效期" width="145">
            <template slot="header">
              <span class="tableHeader">
                <span>有效期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.validityPeriod.valid}"
                @click="chengenum($event,scope.row,'validityPeriod')"
              >
                <div
                  v-show="!scope.row.validityPeriod.isShow"
                  class="tableTd"
                >{{ scope.row.validityPeriod.value | formatTDate }}</div>
                <el-date-picker
                  v-show="scope.row.validityPeriod.isShow"
                  :id="scope.row.id+'___validityPeriod'"
                  v-model="scope.row.validityPeriod.value"
                  style="width:140px;"
                  type="date"
                  placeholder="选择日期"
                  @keydown.native="onKeydown($event)"
                />
              </div>-->
              <EditInput
                v-model="scope.row.validityPeriod"
                type="date"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @onKeydown="onKeydown"
              />
            </template>
          </el-table-column>
          <el-table-column prop="remark" label="备注" width="280">
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
              />
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <!-- 页脚区 -->
    <div id="elfooter">
      <el-form label-width="76px" class="FormInput FormInput1" inline>
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{ dtData.operatorName }}</div>
          </div>
        </el-form-item>
        <el-form-item label="盘点人员：">
          <div v-if="selectRow.auditStatus == 2" class="falseIp">{{ dtData.receiptName }}</div>
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
            />
          </el-select>
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
    <InboundOrder ref="InboundOrder" title="盘亏出库单查询" @OnBtnSaveSubmit="OnBtnSaveSubmit" />
  </el-container>
</template>
<script>
import height from '@/utils/height'
import RequestObject from '@/utils/requestObject'
import request from '@/utils/request'
import {
  formatDate,
  keepTwoDecimalFull,
  getFloat,
  accMul,
  isRealNum,
  formatMoney,
  trim
} from '@/utils/common.js'
import { newGuid } from '@/utils/guid'
import InboundOrder from './components/InboundOrder'

import Pagination from '@/components/Pagination'
// import { promises } from 'dns';
import { setTimeout } from 'timers'
import { getBtnCtr } from '@/utils/BtnControl'
import { closest, getStyle } from '@/utils/Dom'
import Storage from '@/utils/storage'

import EditInput from '@/components/EditTable/EditInput'
import EditSelect from '@/components/EditTable/EditSelect'
import { Receipt } from '@/utils/Receipt'
import BigNumber from 'bignumber.js'

export default {
  name: 'viewsinventoryInventoryLossSlipindexvue',
  filters: {
    formatTDate: value => {
      if (value == '') {
        return ''
      }
      return formatDate(value)
    }
  },
  components: {
    Pagination,
    InboundOrder,
    EditInput,
    EditSelect
  },
  mixins: [Receipt],
  data() {
    return {
      isShowFlag: true,
      popoverStyle: {
        left: '500px',
        top: '400px',
        width: '800px',
        height: '320px'
      },
      menuStyle: {
        left: '500px',
        top: '400px',
        width: '80px',
        height: 'auto',
        paddiing: '4px'
      },
      menuState: false,
      delNum: -1,
      fullscreenLoading: false,
      dtData: {
        WarehousingType: 4,
        warehousingDate: new Date(),
        warehousingOrder: '',
        operatorName: '',
        operatorId: '',
        receiptName: '',
        receiptId: '',
        auditName: '',
        auditId: '',
        auditStatus: -1,
        auditTime: ''
      },
      dtDataRules: {
        WarehousingType: [
          { required: true, message: '请选择出库类型', trigger: 'change' }
        ],
        warehousingDate: [
          {
            type: 'date',
            required: true,
            message: '请选择日期',
            trigger: 'change'
          }
        ],
        warehousingOrder: [{ required: true, message: '获取编号失败' }]
      },
      tableHeight: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: [],
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: '',
      materielData: [], // 物料信息
      warehouseData: [], // 仓库信息
      materialItem: {},
      TabArr: [
        // tab数组
        'materialCode',
        'materialName',
        'defaultWarehouseName',
        'PaidIn',
        'unitPrice',
        'amount',
        'batchNumber',
        'validityPeriod',
        'remark'
      ],
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            id: '',
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          materialName: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          spec: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            id: '',
            // 收料仓库
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          batchNumber: {
            id: '',
            // 批号
            value: '',
            isShow: false,
            valid: false
          },
          // 颜色
          colorName: {
            id: '',
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          unit: {
            id: '',
            // 基本单位
            value: '',
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            id: '',
            // 仓库单位
            value: '',
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            id: '',
            // 基本单位数量
            value: '',
            isShow: false,
            valid: false
          },
          PaidIn: {
            id: '',
            // 实存数量
            value: '',
            isShow: false,
            valid: false
          },
          overFlow: {
            id: '',
            // 盘亏数量
            value: '',
            isShow: false,
            valid: false
          },
          account: {
            id: '',
            // 账存数量
            value: '',
            isShow: false,
            valid: false
          },
          unitPrice: {
            id: '',
            // 单价
            value: '',
            isShow: false,
            valid: false
          },
          amount: {
            id: '',
            // 金额
            value: '',
            isShow: false,
            valid: false
          },
          validityPeriod: {
            id: '',
            // 有效期
            value: '',
            isShow: false,
            valid: false
          },
          remark: {
            id: '',
            // 备注
            value: '',
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
      btnAip: '', // 按钮权限
      OtherData: {},
      realNameOptions: [], // 收货员列表
      addControl: true, // 是否显示新增按钮
      mountedState: false
    }
  },
  watch: {
    selectRow(val) {
      if (val.id) {
        this.addControl = false
      } else {
        this.addControl = true
      }
    }
  },
  deactivated() {
    this.$refs.InboundOrder.openNo()
    window.removeEventListener('keydown', this.onKeydown)
  },
  created() {
    // 获取按钮权限
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )
  },
  async mounted() {
    this.mountedState = true
    window.addEventListener('keydown', this.onKeydown) // 添加全局Tab键盘事件
    this.fullscreenLoading = true
    this.setStyle() // 设置样式
    // this.tableData2 = [...this.tableData] // 克隆原始table数据
    this.warehouseData = await this.getWarehouseData() // 仓库数据
    this.getMaterielData() // 物料数据
    this.getUserCompany() // 收货员列表
    if (this.$route.params.type) {
      // this.OtherData = this.$route.params;
      // this.OnBtnSaveSubmit(this.OtherData.item);
      if (this.$route.params.type == 3) {
        this.resetData()
        setTimeout(() => {
          this.fullscreenLoading = false
        }, 500)
      } else {
        this.OtherData = this.$route.params
        this.OnBtnSaveSubmit(this.OtherData.item)
      }
    } else {
      var code = Storage.LStorage.get('InventoryLossSlip')
      if (!code) {
        this.getCode()
      } else {
        this.dtData.warehousingOrder = code
      }
      // this.getCode()
      this.dtData.operatorName = this.$store.state.user.name
      this.tableData = this.setTable(this.tableLen)
      setTimeout(() => {
        this.fullscreenLoading = false
      }, 500)
    }

    this.mountedState = false
  },
  activated() {
    // this.fullscreenLoading = true;
    // window.addEventListener("keydown", this.onKeydown); // 添加全局Tab键盘事件
    // if (this.$route.params.type) {
    //   this.OtherData = this.$route.params;
    //   this.OnBtnSaveSubmit(this.OtherData.item);
    // }
    // setTimeout(() => {
    //   this.fullscreenLoading = false;
    // }, 500);

    if (this.mountedState == true) {
      return
    }
    window.addEventListener('keydown', this.onKeydown)
    if (this.$route.params.type) {
      if (this.$route.params.type == 3) {
        this.resetData()
      } else {
        this.OtherData = this.$route.params
        this.OnBtnSaveSubmit(this.OtherData.item)
      }
    }
  },
  methods: {
    blurCheck(itemList, item, type, state) {
      // td框的类型（可用于数据校验等）
      switch (
        type // （type）对应组件type
      ) {
        // case 1: // 物料代码
        //   if (state == 2) {
        //     // 按下Enter请求数据
        //     this.getMaterielData(itemList,this.pageIndex=1)
        //   }
        //   break
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
        case 2: // 实发数量
          this.PaidInBlur(itemList)
          break
        case 3: // 单价
          this.unitPriceBlur(itemList)
          break
        case 4: // 金额
          this.amountBlur(itemList)
          break
        default:
          null
      }
    },
    clickEventAfter(itemList, item, type) {
      // 显示input框之后（创建选择物料代码框）
      this.doItem = itemList
      if (type === "materialName" || type === "materialCode") {
        this.findBox(item)
        this.getMaterielData(itemList,type)
      }
    },
    clickEvent() {
      this.defaultAll()
    },

    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = 30 // 按钮高度
        var navbar = document.getElementById('navbar_yfkj')
        var nv = navbar.clientHeight || navbar.offsetHeight
        var b = document.documentElement.clientHeight - nv
        var elheader = document.getElementById('elheader')
        var elfooter = document.getElementById('elfooter')
        var h = elheader.clientHeight || elheader.offsetHeight
        var f = elfooter.clientHeight || elfooter.offsetHeight
        // this.tableHeight = b - h - f - 40  - btn;
        var pt = getStyle(document.getElementById('elmain'), 'paddingTop')
        var pb = getStyle(document.getElementById('elmain'), 'paddingBottom')
        this.tableHeight = b - h - f - parseInt(pt) - parseInt(pb) - btn
      })
    },
    // 设置Tab切换开始
    // onKeydown(event) {
    //   // 按下Tab键
    //   if (event.keyCode !== 9) return;
    //   var data = this.findCheck(event);
    //   this.setCheck(data);
    // },
    // findCheck(event) {
    //   // 寻找Tab下一个元素
    //   var data = {};
    //   for (var h = 0; h < this.tableData.length; h++) {
    //     for (var i in this.tableData[h]) {
    //       if (typeof this.tableData[h][i] === "object") {
    //         if (this.tableData[h][i]["isShow"] === true) {
    //           for (var k = 0; k < this.TabArr.length; k++) {
    //             if (this.TabArr[k] === i) {
    //               event.preventDefault();
    //               var l = h;
    //               if (k + 1 > this.TabArr.length - 1) {
    //                 var go = 0;
    //                 if (l >= this.tableData.length) {
    //                   l = this.tableData.length;
    //                 } else {
    //                   l = l + 1;
    //                 }
    //               } else {
    //                 var go = k + 1;
    //               }
    //               var set = this.TabArr[go];
    //               data.index = l;
    //               data.name = set;
    //               data.item = this.tableData[l];
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
      // 重置所有切换了的Input
      this.popoverState = false
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i])
      }
    },
    // 设置Tab切换结束
    resetData() {
      // 初始化数据
      this.isShowFlag = true
      this.resetToData()
      this.getCode()
    },
    resetToData() {
      this.$refs['dtData'].resetFields()
      this.dtData.operatorName = this.$store.state.user.name
      this.tableData = []
      this.tableData = this.setTable(this.tableLen)

      this.PostDataEdit = {}
      this.cloneTable = []
      this.selectRow = []

      this.dtData.WarehousingType = 4
      this.dtData.warehousingDate = new Date()
      this.dtData.warehousingOrder = ''
      this.dtData.remark = ''
      this.dtData.operatorName = this.$store.state.user.name
      this.dtData.operatorId = ''
      this.dtData.receiptName = ''
      this.dtData.receiptId = ''
      this.dtData.auditName = ''
      this.dtData.auditId = ''
      this.dtData.auditStatus = -1
      this.dtData.auditTime = ''
    },
    async computeAccount(item,id) {
      if (
        item.materialCode.value != '' &&
        item.defaultWarehouseName.key != ''
      ) {
        this.fullscreenLoading = true
        var data = await this.getWarehouseAmount(
          item.materialCode.key,
          item.defaultWarehouseName.key
        )
        item.account.value = data.accountNum
        this.fullscreenLoading = false
        if (item.PaidIn.value !== '' && item.PaidIn.value !== null) {
          item.overFlow.value =
            keepTwoDecimalFull(parseFloat(item.account.value) - parseFloat(item.PaidIn.value))
        }
      }
      // this.DupRemoval(item);
    },
    DupRemoval(item) {
      var Code1 = item.materialCode.key
      var Code2 = item.defaultWarehouseName.key
      var id = item.id
      var Index = 0
      this.tableData.map((dt, key) => {
        if (dt.id == id) {
          Index = key
          return
        }
      })
      // var WZ = 0
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.tableData[i].id == id) {
          var WZ = i
          continue
        }
        if (
          this.tableData[i].materialCode.key == Code1 &&
          this.tableData[i].defaultWarehouseName.key == Code2 &&
          Code1 &&
          Code2
        ) {
          if (WZ == undefined) {
            // 在上面
            var a = 0
            var b = 0
            if (
              this.tableData[i].PaidIn.value !== '' &&
              this.tableData[i].PaidIn.value !== null
            ) {
              a = this.tableData[i].PaidIn.value
            }
            if (item.PaidIn.value !== '' && item.PaidIn.value !== null) {
              b = item.PaidIn.value
            }
            this.tableData[i].PaidIn.value = a + b
            this.PaidInBlur(this.tableData[i])
            this.tableData.splice(Index, 1)
          } else {
            // 在下面
            var a = 0
            var b = 0
            if (
              this.tableData[i].PaidIn.value !== '' &&
              this.tableData[i].PaidIn.value !== null
            ) {
              a = this.tableData[i].PaidIn.value
            }
            if (item.PaidIn.value !== '' && item.PaidIn.value !== null) {
              b = item.PaidIn.value
            }
            item.PaidIn.value = a + b
            this.PaidInBlur(item)
            this.tableData.splice(i, 1)
          }
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
        }
        request({
          url: '/warehouse/api/WarehouseAmount',
          method: 'post',
          data: data
        }).then(res => {
          setTimeout(() => {
            reslove(res.data)
          }, 200)
        })
      })
    },
    menuBar(event, num) {
      if (this.dtData.auditStatus == 2) {
        return
      }
      var el = closest(event.target, '.textAlign')
      var MT = el.getBoundingClientRect().top
      var ML = el.getBoundingClientRect().left
      var xh = 70 // 序号宽度
      if (this.$store.getters.sidebar.opened) {
        var ol = 210 - xh
      } else {
        var ol = 54 - xh
      }
      this.menuStyle.top = MT - 84 - 10 + 'px'
      this.menuStyle.left = ML - ol + 'px'
      this.listenClick()
      this.delNum = num
      this.menuState = true
    },
    doMenuAdd() {
      this.menuState = false
      this.tableData = [...this.tableData, ...this.setTable(1)]
    },
    doMenuDel() {
      this.menuState = false
      if (this.delNum != -1) {
        this.tableData.splice(this.delNum, 1)
        this.delNum = -1
      }
    },
    tableRowClassName({ row, rowIndex }) {
      if (row.rowIndex === 1) {
        return 'color1'
      }
    },
    WarehouseChange(item) {
      // 仓库选择
      this.computeAccount(item,null)
      this.warehouseData.map(data => {
        if (item.key == data.id) {
          item.value = data.warehouseName
        }
      })
      var code = -1
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          code = i
          break
        }
      }
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          continue
        }
        if (
          this.doItem['materialCode']['key'] ==
          this.tableData[i]['materialCode']['key']
        ) {
          if (
            this.doItem['defaultWarehouseName']['key'] ==
            this.tableData[i]['defaultWarehouseName']['key']
          ) {
            this.listenClick()
            this.$set(
              this.tableData[i]['defaultWarehouseName'],
              `isShow`,
              true
            )
            this.$nextTick(() => {
              var id_ = this.tableData[i]['defaultWarehouseName'].id
              document.getElementById(id_).focus()
              document.getElementById(id_).select()
            })
            this.tableData.splice(code, 1, this.setTable(1)[0])
            return
          }
        }
      }
    },
    checkSelect(dt) {
      // 查找输出数据项
      return new Promise((resolve, reject) => {
        dt.map(item => {
          for (var i in item) {
            item.editState = false
            if (typeof item[i] === 'object') {
              if (item[i].value != '') {
                item.editState = true
                break
              }
            }
          }
        })
        resolve(dt)
      })
    },
    getCode() {
      request({
        url: 'warehouse/api/TWMDeficitMain/GetOrderNo',
        method: 'GET'
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = ''
        } else {
          this.dtData.warehousingOrder = res
          Storage.LStorage.set('InventoryLossSlip', res)
        }
      })
    },
    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        var flag = false
        var sum = true
        this.$refs.dtData.validate(valid => {
          // 验证
          if (!valid) {
            this.$message({
              message: '数据不合法，无法保存',
              type: 'error'
            })
          } else {
            flag = true
          }
        })
        if (!flag) return // 验证头部数据
        var res = /^\d{1,14}(\.\d{1,4})?$/
        var checkSelect = await this.checkSelect(this.tableData)
        var childList = []
        var computeFlag = true
        var PaidInFlag = true
        var remarkRel = true
        var numRel = true
        var priceRel = true
        checkSelect.map(item => {
          // 验证数据
          item.materialCode.valid = false
          item.materialName.valid = false
          item.spec.valid = false
          item.defaultWarehouseName.valid = false
          item.PaidIn.valid = false
          if (item.editState) {
            if (item.materialCode.key == '') {
              // 验证物料代码
              item.materialCode.valid = true
              item.materialName.valid = true
              item.spec.valid = true
              flag = false
            }
            if (item.defaultWarehouseName.key == '') {
              // 验证物仓库ID
              item.defaultWarehouseName.valid = true
              flag = false
            }
            if (item.PaidIn.value == '' || isNaN(item.PaidIn.value)) {
              // 验证实存数量
               if(item.PaidIn.value!=0) {
              item.PaidIn.valid = true
              flag = false
            }
            }
            if (!res.test(item.PaidIn.value)) {
              // 验证实发数量
              if(item.PaidIn.value!=0) {
                item.PaidIn.valid = true
                numRel = false
              }
            }
            if (item.PaidIn.value !== '' || item.PaidIn.value !== null) {
              // 验证：实存数量不能小于账存数量(请去盘亏出库操作)
              if (
                keepTwoDecimalFull(parseFloat(item.PaidIn.value) - parseFloat(item.account.value)) >
                0
              ) {
                item.PaidIn.valid = true
                computeFlag = false
              }
              if(keepTwoDecimalFull(parseFloat(item.PaidIn.value) - parseFloat(item.account.value))==0) {
                item.PaidIn.valid = true
                PaidInFlag = false
              }
            }

            var _LogName = ''
            var param = {
              id: 0,
              mainId: 0,
              materialId: parseFloat(item.materialCode.key) || '',
              warehouseId: parseFloat(item.defaultWarehouseName.key) || '',
              actualNumber: keepTwoDecimalFull(parseFloat(item.account.value) -
                  parseFloat(item.PaidIn.value)) ||
                 '',
              accountNum: item.account.value
            }
            _LogName += `物料:${item.materialName.value} 盘亏出库 ${param.actualNumber}${item.unit.value} 到 ${item.defaultWarehouseName.value}`
            if (
              item.batchNumber.value != '' &&
              item.batchNumber.value != null
            ) {
              // 批号
              param.batchNumber = item.batchNumber.value
            }
            if (item.colorName.key !== '') {
              // 颜色
              param.colorId = item.colorName.key
            }
            // if (item.overFlow.value > 0) {
            //   sum = false
            //   // item.overFlow.valid = false
            // } else {
            //   sum = true
            // }
            if (item.unitPrice.value != '' && item.unitPrice.value != null) {
              // 单价
              param.unitPrice = parseFloat(item.unitPrice.value) || 0
              item.unitPrice.valid = false
            }
            if (param.unitPrice != null && !res.test(param.unitPrice)) {
              item.unitPrice.valid = true
              priceRel = false
            }
            if (item.amount.value != '' && item.amount.value != null) {
              // 金额
              param.amount = parseFloat(item.amount.value) || 0
              item.unitPrice.valid = false
            }
            if (param.amount != null && !res.test(param.amount)) {
              item.amount.valid = true
              priceRel = false
            } else {
              item.amount.valid = false
              priceRel = true
            }
            if (
              item.validityPeriod.value != '' &&
              item.validityPeriod.value != null
            ) {
              // 日期
              param.validityPeriod = formatDate(item.validityPeriod.value) + ''
            }
            if (item.remark.value != '' && item.remark.value != null) {
              // 备注
              if (item.remark.value.length > 500) {
                item.remark.valid = true
                remarkRel = false
              }
              param.remark = item.remark.value
              item.remark.valid = false
            }
            param._LogName = _LogName
            childList.push(param)
          }
        })

        if (!flag) {
          this.$message.error('数据不合法，无法保存')
          return
        }
        if (childList.length == 0) {
          this.$message({
            message: '请输出数据',
            type: 'error'
          })
          return
        }
        if (!numRel) {
          this.$message.error('数量不能是负数，也不能超过99999999999999')
          return
        }
        if (!priceRel) {
          this.$message.error('价格不能是负数，也不能超过99999999999999')
          return
        }
        if (!remarkRel) {
          this.$message.error('备注最大允许输入500个字符，请重新输入')
          return
        }
        if (!computeFlag) {
          this.$message.error('实存数量不能大于账存数量(请去盘盈入库操作)')
          return
        }
        if(!PaidInFlag) {
          this.$message.error('实存数量要小于账存数量')
          return
        }
        var currData = {
          // operatorId: 0,
          whSendType: this.dtData.WarehousingType,
          whSendDate: formatDate(this.dtData.warehousingDate),
          whSendOrder: this.dtData.warehousingOrder,
          childList: childList
        }

        if (this.dtData.receiptId !== '') {
          currData.receiptId = this.dtData.receiptId
        }
      }
      // return
      var method = ''
      if (state == 1) {
        currData.id = 0
        currData.auditStatus = 0
        var data = RequestObject.CreatePostObject(currData)
        this.postData(data, state)
      }

      if (state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error('该出库单是通过状态，无法编辑')
          return
        }
        if (this.selectRow.id === undefined) {
          this.$message.error('请选择数据')
          return
        }
        currData.id = this.selectRow.id
        var data = RequestObject.CreatePostObject(
          currData,
          null,
          this.PostDataEdit
        )
        this.postData(data, state)
      }

      if (state == 3) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error('该出库单是通过状态，无法删除')
          return
        }
        if (this.selectRow.id == undefined) {
          this.$message.error('请选择删除的数据')
          return
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的盘亏出库单`
        }
        var data = { postData: currData }

        this.$confirm('是否删除出库单？', '确认信息', {
          type: 'warning'
        })
          .then(() => {
            this.postData(data, state)
          })
          .catch(() => {})
      }
    },
    postData(data, state) {
      var data = data
      var state = state
      var method = ''
      if (state == 1) {
        method = 'post'
      }
      if (state == 2) {
        method = 'put'
      }
      if (state == 3) {
        method = 'DELETE'
      }
      this.fullscreenLoading = true
      request({
        url: '/warehouse/api/TWMDeficitMain',
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: '操作成功',
            type: 'success'
          })
          if (state == 1) {
            this.OnBtnSaveSubmit(res.data)
            Storage.LStorage.remove('InventoryLossSlip')
          }
          if (state == 2) {
            this.OnBtnSaveSubmit(res.data)
            // Storage.LStorage.remove("InventoryLossSlip");
          }
          if (state == 3) {
            setTimeout(() => {
              this.tableData = []
              this.resetToData()
              this.dtData.warehousingOrder = Storage.LStorage.get(
                'InventoryLossSlip'
              )
              // this.setTable(30);
            }, 50)
          }
          setTimeout(() => {
            this.fullscreenLoading = false
          }, 500)
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          setTimeout(() => {
            this.fullscreenLoading = false
          }, 500)
        }
      })
    },
    getWarehouseData() {
      // 查看仓库档案信息
      return new Promise(function(reslove, reject) {
        var reqObject
        reqObject = RequestObject.CreateGetObject(false, 0, 0, [])
        request({
          url: '/basicset/api/TBMWarehouseFile',
          method: 'get',
          params: {
            requestObject: JSON.stringify(reqObject)
          }
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, '错误信息', {
              confirmButtonText: '确定',
              type: 'error',
              showCancelButton: false
            })
          } else {
            // this.warehouseData = res.data;
            reslove(res.data)
          }
        })
      })
    },
    // setTable(num) {
    //   // 初始化数据
    //   var listArr = [];
    //   for (var i = 0; i < num; i++) {
    //     var list = {};
    //     for (var j in this.tableData2[0]) {
    //       if (j == "id") {
    //         list["id"] = newGuid();
    //       } else {
    //         if (typeof this.tableData2[0][j] === "object") {
    //           list[j] = {};
    //           for (var k in this.tableData2[0][j]) {
    //             if (k == "id") {
    //               list[j][k] = newGuid();
    //               continue;
    //             }
    //             if (typeof this.tableData2[0][j][k] === "boolean") {
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
    // },
    // getMaterielData(item) {
    //   // 获取物料档案信息
    //   this.materielData = []
    //   // this.totalCount = 0;

    //   var queryData = []
    //   if (item) {
    //     queryData.push({
    //       column: 'materialCode',
    //       content: trim(item.materialCode.value),
    //       condition: 6
    //     })
    //     queryData.push({
    //       column: 'materialName',
    //       content: trim(item.materialName.value),
    //       condition: 6
    //     })
    //   }

    //   var reqObject
    //   reqObject = RequestObject.LonginBookObject(
    //     true,
    //     this.pageSize,
    //     this.pageIndex,
    //     null,
    //     queryData
    //   )
    //   request({
    //     url: '/basicset/api/TBMMaterialFile',
    //     method: 'get',
    //     params: {
    //       requestObject: JSON.stringify(reqObject)
    //     }
    //   }).then(res => {
    //     this.loading = false
    //     if (res.code == -1) {
    //       this.$confirm(res.info, '错误信息', {
    //         confirmButtonText: '确定',
    //         type: 'error',
    //         showCancelButton: false
    //       })
    //     } else {
    //       this.materielData = res.data
    //       this.totalCount = res.totalNumber
    //     }
    //   })
    // },
    getUserCompany() {
      /**
       * getUserCompany
       * 获取当前用户公司员工
       */
      var reqObject = RequestObject.LonginBookObject(false, 0, 0, null, null)
      request({
        url: '/system/api/TSMUser/GetUserInCurentCompany',
        method: 'get',
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.realNameOptions = res.data
        }
      })
    },
    rowClick(row) {
      this.doItem.materialCode.value = row.materialCode
      this.doItem.materialCode.key = row.id
      this.doItem.materialName.value = row.materialName
      this.doItem.spec.value = row.spec

      this.doItem.colorName.value = row.colorName
      this.doItem.colorName.key = row.colorId

      this.doItem.warehouseUnitName.value = row.warehouseUnitName
      this.doItem.unit.value = row.baseUnitName
      this.doItem.baseUnitNumber.value = row.baseUnitNumber
      // this.doItem.defaultWarehouseName.value = row.defaultWarehouseName;
      // this.doItem.defaultWarehouseName.key = row.defaultWarehouseId;
      this.computeAccount(this.doItem,null)
      this.doItem.defaultWarehouseName.value = ''
      this.doItem.defaultWarehouseName.key = ''
      if (row.defaultWarehouseId !== null) {
        this.doItem.defaultWarehouseName.value = row.defaultWarehouseName
        this.doItem.defaultWarehouseName.key = row.defaultWarehouseId
        // this.computeAccount(this.doItem,null)
      }
      var code = -1
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          code = i
          break
        }
      }
      for (var i = 0; i < this.tableData.length; i++) {
        if (this.doItem.id == this.tableData[i].id) {
          continue
        }
        if (
          this.doItem['materialCode']['key'] ==
          this.tableData[i]['materialCode']['key']
        ) {
          if (
            this.doItem['defaultWarehouseName']['key'] ==
            this.tableData[i]['defaultWarehouseName']['key']
          ) {
            this.listenClick()
            this.$set(
              this.tableData[i]['defaultWarehouseName'],
              `isShow`,
              true
            )
            this.$nextTick(() => {
              var id_ = this.tableData[i]['defaultWarehouseName'].id
              document.getElementById(id_).focus()
              document.getElementById(id_).select()
            })
            this.tableData.splice(code, 1, this.setTable(1)[0])
            return
          }
        }
      }
      this.listenClick()
      // this.$set(this.doItem["defaultWarehouseName"], `isShow`, true);
      // this.$nextTick(() => {
      //   document
      //     .getElementById(this.doItem.id + "___" + "defaultWarehouseName")
      //     .focus();
      //   document
      //     .getElementById(this.doItem.id + "___" + "defaultWarehouseName")
      //     .select();
      // });
      this.$set(this.doItem['defaultWarehouseName'], `isShow`, true)
      this.$nextTick(() => {
        var id_ = this.doItem['defaultWarehouseName'].id
        document.getElementById(id_).focus()
        document.getElementById(id_).select()
        if (this.doItem.defaultWarehouseName.key == '') {
          if (
            document.getElementById(id_).getAttribute('readonly') == 'readonly'
          ) {
            document.getElementById(id_).click()
          }
        }
      })
    },

    // rowClick(row) {
    //   this.$emit("setEditState", true);
    //   this.doItem.materialCode.valid = false;

    //   this.doItem.materialCode.value = row.materialName;
    //   this.doItem.materialCode.key = row.id;

    //   this.listenClick();
    //   this.$set(this.doItem["materialName"], `isShow`, true);
    //   this.$nextTick(() => {
    //     var id_ = this.doItem["materialName"].id;
    //     document.getElementById(id_).focus();
    //     document.getElementById(id_).select();
    //     if (
    //       document.getElementById(id_).getAttribute("readonly") == "readonly"
    //     ) {
    //       document.getElementById(id_).click();
    //     }
    //   });
    // },
    getSummaries(param) {
      // table总计
      const { columns, data } = param
      const sums = []
      columns.forEach((column, index) => {
        if (index === 0) {
          sums[index] = '总计'
          return
        }
        const values = data.map(item => {
          return Number(item[column.property].value)
        })
        if (!values.every(value => isNaN(value))) {
          sums[index] = values.reduce((prev, curr) => {
            const value = Number(curr)
            if (!isNaN(value)) {
              return BigNumber(prev).plus(curr).toNumber()
            } else {
              return prev
            }
          }, 0)
          if (index == 6 || index == 7 || index == 8 || index == 10) {
            sums[index] += ''
          } else {
            sums[index] = ''
          }
        } else {
          sums[index] = ''
        }
      })

      return sums
    },
    chengenum(event, item, name, state) {
      if (this.dtData.auditStatus != 2) {
        // 双击显示input,监听input数据变化
        event.stopPropagation()

        this.defaultAll(this.doItem)
        // this.popoverState = false
        item[name].isShow = true
        this.doItem = item
        this.doItemName = name
        this.$nextTick(() => {
          if (document.getElementById(item.id + '___' + name)) {
            document.getElementById(item.id + '___' + name).focus()
            document.getElementById(item.id + '___' + name).select()
          }

          if (state == 1) {
            this.findBox(item, name)
            this.getMaterielData()
          }
        })
      }
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
    //   // this.popoverState = false
    //   this.defaultAll(this.doItem);
    // },
    getByClass(oParent, sClass) {
      var aResult = []
      var aEle = oParent.getElementsByTagName('*')

      for (var i = 0; i < aEle.length; i++) {
        if (aEle[i].className == sClass) {
          aResult.push(aEle[i])
        }
      }

      return aResult
    },
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize
      this.pageIndex = val.pageIndex
      if (Object.keys(this.materialItem).length !== 0) {
        this.getMaterielData(this.materialItem)
      } else {
        this.getMaterielData()
      }
    },
    getMaterielDataIndex(item) {
      this.pageIndex = 1
      if (item) {
        this.materialItem = item
        this.getMaterielData(item)
      } else {
        this.getMaterielData()
      }
    },
    defaultData(data) {
      data.map(item => {
        for (var i in item) {
          this.$set(item, i + '_YF_Edit', false)
        }
      })
      return data
    },
    doDefault(data) {
      this.menuState = false
      for (var i in data) {
        if (typeof data[i] === 'object') {
          if (data[i]['isShow']) {
            data[i]['isShow'] = false
          }
        }
      }
    },
    PaidInBlur(item) {
      if (item.PaidIn.value === '' || item.PaidIn.value === null) return
      item.PaidIn.value = parseFloat(item.PaidIn.value)
      item.PaidIn.value = keepTwoDecimalFull(
        parseFloat(item.PaidIn.value)
      )
      // this.getWarehouseAmount()
      if (item.account.value !== '' && item.account.value !== null) {
        item.overFlow.value =
          keepTwoDecimalFull(parseFloat(item.account.value) - parseFloat(item.PaidIn.value))
        item.overFlow.value = keepTwoDecimalFull(item.overFlow.value)
        item.baseUnitNumber.value = item.overFlow.value
      }
      this.unitPriceBlur(item) // 单价算金额
      this.amountBlur(item) // 金额算单机
    },
    unitPriceBlur(item) {
      if (item.unitPrice.value === '' || item.unitPrice.value === null) return

      if (item.overFlow.value !== '') {
        item.unitPrice.value = parseFloat(item.unitPrice.value)
        item.amount.value = keepTwoDecimalFull(
          accMul(item.overFlow.value, item.unitPrice.value)
        )
      } else {
        item.unitPrice.value = keepTwoDecimalFull(
          parseFloat(item.unitPrice.value)
        )
      }
    },
    amountBlur(item) {
      if (item.amount.value === '' || item.amount.value === null) return

      if (item.overFlow.value !== '') {
        item.amount.value = parseFloat(item.amount.value)
        item.unitPrice.value = keepTwoDecimalFull(
          item.amount.value / item.overFlow.value
        )
      } else {
        item.amount.value = keepTwoDecimalFull(parseFloat(item.amount.value))
      }
    },
    handelAddClick() {
      // 查询
      // 点击添加按钮
      this.$refs.InboundOrder.open()
      this.$refs.InboundOrder.dtData.warehousingOrder = ''
      this.$refs.InboundOrder.dtData.warehousingDate = ''
      this.$refs.InboundOrder.dtData.auditStatus = -1
      this.$refs.InboundOrder.clickRow = {}
      this.$refs.InboundOrder.getMainList()
    },
    cloneObject(origin) {
      return Object.assign({}, origin)
    },
    OnBtnSaveSubmit(row, state) {
      if (row.auditStatus == 2) {
        this.isShowFlag = false
      } else {
        this.isShowFlag = true
      }
      this.fullscreenLoading = true
      // 根据ID获取数据
      this.selectRow = this.cloneObject(row)
      this.dtData.WarehousingType = row.whSendType
      this.dtData.warehousingDate =
        row.whSendDate != null ? new Date(row.whSendDate.split('T')[0]) : ''
      this.dtData.warehousingOrder = row.whSendOrder

      this.dtData.operatorName = row.operatorName
      this.dtData.operatorId = row.operatorId

      this.dtData.receiptName = row.receiptName
      this.dtData.receiptId = row.receiptId

      this.dtData.auditName = row.auditName
      this.dtData.auditId = row.auditId
      this.dtData.auditStatus = row.auditStatus
      this.dtData.auditTime =
        row.auditTime != null ? row.auditTime.split('T')[0] : ''
      if (!state) {
        // var rqs = RequestObject.LonginBookObject(false, 0, 0, row.id);
        // this.fullscreenLoading = true
        request({
          url: 'warehouse/api/TWMDeficitMain/GetWholeMainData',
          method: 'GET',
          params: { RequestObject: row.id }
        }).then(res => {
          if (res.code === 0) {
            this.setTableData(res.data.childList)
            // this.fullscreenLoading = false
          } else {
            this.$confirm(res.info, '错误信息', {
              confirmButtonText: '确定',
              type: 'error',
              showCancelButton: false
            })
            // this.fullscreenLoading = false
          }
        })
      } else {
        this.setTableData(row.childList)
      }
    },
    setCurrData(data) {
      // 设置编辑时候的日记数据
      var childList = []
      var _LogName = ''
      this.cloneTable.map(item => {
        var param = {
          id: 0,
          mainId: 0,
          materialId: parseFloat(item.materialCode.key) || '',
          warehouseId: parseFloat(item.defaultWarehouseName.key) || '',
          actualNumber:
            keepTwoDecimalFull(parseFloat(item.account.value) - parseFloat(item.PaidIn.value)) ||
            '',
          accountNum: item.account.value
        }
        _LogName += `物料:${item.materialName.value} 盘亏出库 ${param.actualNumber}${item.unit.value} 到 ${item.defaultWarehouseName.value}`

        if (item.batchNumber.value != '' && item.batchNumber.value != null) {
          // 批号
          param.batchNumber = item.batchNumber.value
        }
        if (item.unitPrice.value != '' && item.unitPrice.value != null) {
          // 单价
          param.unitPrice = parseFloat(item.unitPrice.value) || 0
        }
        if (item.amount.value != '' && item.amount.value != null) {
          // 金额
          param.amount = parseFloat(item.amount.value) || 0
        }
        if (
          item.validityPeriod.value != '' &&
          item.validityPeriod.value != null
        ) {
          // 日期
          param.validityPeriod = formatDate(item.validityPeriod.value) + ''
        }
        // 备注
        if (item.remark.value != '' && item.remark != null) {
          param.remark = item.remark.value
        }
        param._LogName = _LogName
        childList.push(param)
      })
      var currData = {
        accountNum: this.selectRow.accountNum,
        id: this.selectRow.id,
        operatorId:
          this.selectRow.operatorId != null ? this.selectRow.operatorId : 0,
        whSendType: this.selectRow.whSendType,
        whSendDate:
          this.selectRow.whSendDate != null || this.selectRow.whSendDate != ''
            ? this.selectRow.whSendDate.split('T')[0]
            : '',
        whSendOrder: this.selectRow.whSendOrder,
        auditStatus: this.selectRow.auditStatus,
        childList: childList
      }
      this.PostDataEdit = currData
    },
    setTableData(dt) {
      // 将数据导出table
      this.tableData = []
      var listArr = []
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
          defaultWarehouseName: {
            id: newGuid(),
            // 收料仓库
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          batchNumber: {
            id: newGuid(),
            value: item.batchNumber, // 批号
            isShow: false,
            valid: false
          },
          // 颜色
          colorName: {
            id: newGuid(),
            value: item.colorName,
            key: item.colorId,
            isShow: false,
            valid: false
          },
          unit: {
            id: newGuid(),
            // 单位
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            id: newGuid(),
            value: item.warehouseUnitName,
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
            id: newGuid(),
            // 实存数量
            value: keepTwoDecimalFull(parseFloat(item.accountNum) - parseFloat(item.actualNumber)),
            isShow: false,
            valid: false
          },
          overFlow: {
            id: newGuid(),
            // 盘亏数量
            value: item.actualNumber,
            isShow: false,
            valid: false
          },
          account: {
            id: newGuid(),
            // 账存数量
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
                ? new Date(item.validityPeriod.split('T')[0])
                : '',
            isShow: false,
            valid: false
          },
          remark: {
            id: newGuid(),
            value: item.remark,
            isShow: false,
            valid: false
          }
        }
        this.warehouseData.map(data => {
          if (list.defaultWarehouseName.key == data.id) {
            list.defaultWarehouseName.value = data.warehouseName
          }
        })
        listArr.push(list)
      })
      this.cloneTable = [...listArr]
      this.setCurrData(this.cloneTable)
      setTimeout(() => {
        // this.tableData = [...listArr, ...this.setTable(30)];
        if (listArr.length < this.tableLen) {
          this.tableData = [...listArr, ...this.setTable(this.tableLen - listArr.length)]
        } else {
          this.tableData = [...listArr, ...this.setTable(1)]
        }
      }, 0)
      setTimeout(() => {
        this.fullscreenLoading = false
      }, 500)
    },
    state(id) {
      request({
        url: 'warehouse/api/TWMDeficitMain/GetWholeMainData',
        method: 'GET',
        params: { RequestObject: id }
      }).then(res => {
        if (res.code === 0) {
          this.selectRow = this.cloneObject(res.data)
          this.dtData.WarehousingType = res.data.whSendType
          this.dtData.warehousingDate =
            res.data.whSendDate != null
              ? new Date(res.data.whSendDate.split('T')[0])
              : ''
          this.dtData.warehousingOrder = res.data.whSendOrder

          this.dtData.operatorName = res.data.operatorName
          this.dtData.operatorId = res.data.operatorId

          this.dtData.receiptName = res.data.receiptName
          this.dtData.receiptId = res.data.receiptId

          this.dtData.auditName = res.data.auditName
          this.dtData.auditId = res.data.auditId
          this.dtData.auditStatus = res.data.auditStatus
          this.dtData.auditTime =
            res.data.auditTime != null ? res.data.auditTime.split('T')[0] : ''
          this.setTableData(res.data.childList)
          // this.fullscreenLoading = false
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          // this.fullscreenLoading = false
        }
      })
    },
    doOtherWhAudit(state) {
      // 审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error('请选择审核数据')
        return
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error('该出库单已经为通过状态')
        return
      }
      // var data = RequestObject.CreatePostObject({
      //   id: this.selectRow.id,
      //   auditStatus: state,
      //   ChildList: []
      // });
      var text = '确定审核通过吗？'
      if (state == 1) {
        text = '确定审核未通过吗？'
      }
      var data = {
        postData: {
          id: this.selectRow.id,
          auditStatus: state
        },
        postDataEdit: {
          id: this.selectRow.id,
          auditStatus: this.selectRow.auditStatus
        }
      }
      this.fullscreenLoading = true
      this.$confirm(text, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          request({
            url: '/warehouse/api/TWMDeficitMain/OtherWhAuditAsync',
            method: 'put',
            data: data
          }).then(res => {
            if (res.code === 0) {
              this.$message({
                message: '操作成功',
                type: 'success'
              })
              this.dtData.auditStatus = state
              this.selectRow.auditStatus = state
              setTimeout(() => {
                this.state(this.selectRow.id)
                this.fullscreenLoading = false
              }, 500)
            } else {
              this.$confirm(res.info, '错误信息', {
                confirmButtonText: '确定',
                type: 'error',
                showCancelButton: false
              })
              setTimeout(() => {
                this.fullscreenLoading = false
              }, 500)
            }
          })
        })
        .catch(() => {
          this.fullscreenLoading = false
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    doOtherWhCancelAudit() {
      // 撤销审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error('请选择审核数据')
        return
      }
      if (this.selectRow.auditStatus != 2) {
        this.$message.error('该出库单不是通过状态，无法撤销')
        return
      }
      var data = RequestObject.CreatePostObject({
        id: this.selectRow.id,
        auditStatus: 3,
        ChildList: []
      })
      this.fullscreenLoading = true
      request({
        url: '/warehouse/api/TWMOtherWhMain/OtherWhCancelAudit',
        method: 'put',
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: '操作成功',
            type: 'success'
          })
          this.dtData.auditStatus = 3
          this.selectRow.auditStatus = 3
          setTimeout(() => {
            this.fullscreenLoading = false
          }, 500)
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          setTimeout(() => {
            this.fullscreenLoading = false
          }, 500)
        }
      })
    }
  }
}
</script>
<style lang="scss" scoped>
#OverflowOrder /deep/ {
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
    // color: #606266;
    background: #fff;
  }
  .disableType i {
    border-top: 1px solid rgb(220, 223, 230);
    border-bottom: 1px solid rgb(220, 223, 230);
    height: 32px;
  }
  .FormInput1 {
    .el-input__inner,
    .el-checkbox__inner,
    .el-textarea__inner {
      border-radius: 0;
      border: none;
      border-bottom: 1px solid #dcdfe6;
    }
  }
}
.el-table {
  overflow: visible !important;
}
@import "@/styles/receipts.scss";
</style>
