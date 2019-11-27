<template>
  <el-container
    id="ProcurementPut"
    v-loading="fullscreenLoading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    @click.native="listenClick"
  >
    <!-- <div style="padding:5px 0px 0px 20px;height: 37px;font-size:0px;">
      <div v-if="btnAip.add">
        <el-button-group class="groupBtn">
          <el-button
            v-if="addShow&&btnAip.add.buttonCaption"
            type="default"
            @click="addPutInStorage(1)"
          >{{ btnAip.add.buttonCaption }}</el-button>
          <el-button
            type="default"
            @click="resetData()"
          >重置</el-button>
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
        <el-dropdown
          split-button
          type="default"
          class="dropdown"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
        >
          审核
          <el-dropdown-menu
            slot="dropdown"
            style="border-bottom: 0px solid #ccc;"
          >
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>-->
    <div class="btnHeader">
      <div
        v-if="btnAip.add"
        class="btnHeaderBox"
      >
        <el-button
          v-if="btnAip.add.buttonCaption&&addShow"
          type="default"
          @click="addPutInStorage(1)"
        >{{ btnAip.add.buttonCaption }}</el-button>
        <el-button
          type="default"
          @click="resetData()"
        >重置</el-button>
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
    <el-header
      id="elheader"
      class="header fromCenter"
      height="auto"
    >
      <el-form
        ref="dtData"
        :model="dtData"
        label-width="86px"
        class="FormInput"
        inline
        label-position="left"
        :rules="dtDataRules"
      >
        <el-form-item
          class="formItem"
          label="出库类型："
          prop="WarehousingType"
        >
          <el-select
            v-model="dtData.WarehousingType"
            placeholder="请选择"
            style="width:200px"
          >
            <el-option
              label="销售出库"
              :value="0"
            />
            <el-option
              label="生产出库"
              :value="1"
            />
            <el-option
              label="裁片出库"
              :value="2"
            />
            <el-option
              label="委外出库"
              :value="3"
            />
            <!-- <el-option label="盘亏出库" :value="4" /> -->
            <el-option
              label="其他出库"
              :value="5"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          class="formItem"
          label="出库日期："
          prop="warehousingDate"
        >
          <el-date-picker
            v-model="dtData.warehousingDate"
            type="date"
            placeholder="选择日期"
            style="width:200px"
          />
        </el-form-item>
        <el-form-item
          class="formItem"
          label="编号："
          prop="warehousingOrder"
          label-width="60px"
        >
          <div
            class="bbfe6"
            style="height:32px;min-width:128px;"
          >{{ dtData.warehousingOrder }}</div>
        </el-form-item>
        <el-form-item
          class="formItem"
          label="出库状态："
        >
          <div
            class="bbfe6"
            style="height:32px;"
          >
            <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待出库</span>
            <span v-if="dtData.auditStatus==0">待审核</span>
            <span v-if="dtData.auditStatus==1">审核未通过</span>
            <span v-if="dtData.auditStatus==2">审核通过</span>
          </div>
        </el-form-item>
      </el-form>
    </el-header>
    <el-main>
      <div
        v-show="popoverState"
        class="findBox"
        :style="popoverStyle"
        @click.stop="1==1"
      >
        <el-table
          :data="materielData"
          :height="250"
          @row-click="rowClick"
        >
          <el-table-column
            prop="materialCode"
            label="物料代码"
            width="180"
          />
          <el-table-column
            prop="materialName"
            label="物料名称"
          />
          <el-table-column
            prop="spec"
            label="规格型号"
          />
          <el-table-column
            prop="baseUnitName"
            label="基本计量单位"
            width="100"
          />
          <el-table-column
            prop="materialTypeName"
            label="物料分类"
          />
          <el-table-column
            prop="colorName"
            label="颜色"
          />
          <el-table-column
            prop="defaultWarehouseName"
            label="默认仓库"
          />
          <el-table-column
            prop="shelfLife"
            label="保质期（天）"
            width="100"
          />
        </el-table>
        <Pagination
          :page-index="pageIndex"
          :total-count="totalCount"
          :page-size="pageSize"
          @pagination="pagination"
        />
      </div>
      <div
        v-show="menuState"
        class="findBox"
        :style="menuStyle"
      >
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
          <el-table-column
            label="序号"
            width="70"
          >
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

          <el-table-column
            prop="materialCode"
            label="物料代码"
          >
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
          <el-table-column
            prop="materialName"
            label="物料名称"
          >
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
          <el-table-column
            prop="spec"
            label="规格型号"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.spec.valid}"
              >
                <div class="tableTd">{{ scope.row.spec.value }}</div>
              </div>-->
              <EditInput
                v-model="scope.row.spec"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
              />
            </template>
          </el-table-column>
          <el-table-column
            prop="colorName"
            label="颜色"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.colorName.valid}"
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
          <el-table-column
            prop="defaultWarehouseName"
            label="发货仓库"
          >
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
                  @change="WarehouseChange(scope.row)"
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
                :is-show-flag="isShowFlag"
                :item-list="scope.row"
                :type="1"
                label="warehouseName"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @change="WarehouseChange(scope.row)"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="warehouseSum"
            label="可用数量"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>可用数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{ scope.row.warehouseSum.value }}</div> -->
              <EditInput
                v-model="scope.row.warehouseSum"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="PaidIn"
            label="实发数量"
          >
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>实发数量</span>
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
                :class="{redCol:scope.row.PaidIn.value<=0,greCol:scope.row.PaidIn.value>0}"
                :type="2"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
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

          <el-table-column
            prop="batchNumber"
            label="批号"
          >
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
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <!-- <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.warehouseUnitName.value }}</div>
            </template>
          </el-table-column>-->

          <el-table-column
            prop="unitPrice"
            label="单价"
          >
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
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>
          <el-table-column
            prop="amount"
            label="金额"
          >
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
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="defaultWarehouseName"
            label="有效期"
            width="145"
          >
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
    <div id="elfooter">
      <el-form
        label-width="76px"
        class="FormInput FormInput1"
        inline
      >
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{ dtData.operatorName }}</div>
          </div>
        </el-form-item>
        <el-form-item label="收货员：">
          <div
            v-if="selectRow.auditStatus == 2"
            class="falseIp"
          >{{ dtData.receiptName }}</div>
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
    <InboundOrder
      ref="InboundOrder"
      title="其他出库单查询"
      @OnBtnSaveSubmit="OnBtnSaveSubmit"
    />
  </el-container>
</template>
<script>
import RequestObject from '@/utils/requestObject'
import request from '@/utils/request'
// import {
//   formatDate,
//   keepTwoDecimalFull,
//   accMul,
//   trim
// } from "@/utils/common.js";
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
import InboundOrder from './components/outOrder'
import { getBtnCtr } from '@/utils/BtnControl'
import Pagination from '@/components/Pagination'
// import { promises } from 'dns';
import { setTimeout } from 'timers'
import { closest } from '@/utils/Dom'
import Storage from '@/utils/storage'
import EditInput from '@/components/EditTable/EditInput'
import EditSelect from '@/components/EditTable/EditSelect'
import { Receipt } from '@/utils/Receipt'
import BigNumber from 'bignumber.js'

export default {
  name: 'viewsinventoryCheckoutindexvue',
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
      editId:null,
      isShowFlag: true,
      isIF: true,
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
      materialItem: {},
      menuState: false,
      delNum: -1,
      fullscreenLoading: false,
      dtData: {
        WarehousingType: '',
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
          colorName: {
            id: '',
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            id: '',
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          batchNumber: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          warehouseSum: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          unit: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          PaidIn: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          unitPrice: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          amount: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          validityPeriod: {
            id: '',
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
      TabArr: [
        // tab数组
        'materialCode',
        'materialName',
        'defaultWarehouseName',
        'PaidIn',
        'batchNumber',
        'unitPrice',
        'amount',
        'validityPeriod',
        'remark'
      ],
      tableData: [],
      realNameOptions: [], // 收货员列表
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      routeData: '',
      isShow: true,
      addShow: true,
      btnAip: '',
      hisRow: {},
      mountedState: false
    }
  },
  //  // 离开之前触发的函数
  // beforeRouteLeave (to, from, next) {
  //   // this.$refs.InboundOrder.open();
  //   console.log(to, from, next)
  // },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )

    // this.deactivated()// 添加全局Tab键盘事件
  },
  deactivated() {
    this.$refs.InboundOrder.openNO()
    window.removeEventListener('keydown', this.onKeydown)
  },
  async mounted() {
    this.mountedState = true
    window.addEventListener('keydown', this.onKeydown)
    this.fullscreenLoading = true
    this.setStyle()
    // this.tableData2 = [...this.tableData]
    this.warehouseData = await this.getWarehouseData() // 仓库数据
    this.getMaterielData() // 物料数据
    this.getUserCompany() // 收货员列表
    this.routeData = this.$route.params

    if (this.routeData.type == 3) {
      this.resetData()
      setTimeout(() => {
        this.fullscreenLoading = false
      }, 500)
    } else {
      if (this.routeData.item) {
        this.$refs['dtData'].resetFields()
        this.OnBtnSaveSubmit(this.routeData.item)
        this.isShow = false
      } else {
        var code = Storage.LStorage.get('Checkout')
        if (!code) {
          this.getCode()
        } else {
          this.dtData.warehousingOrder = code
        }
        // this.getCode()
        this.dtData.operatorName = this.$store.state.user.name
        setTimeout(() => {
          this.fullscreenLoading = false
        }, 500)
      }
    }

    this.tableData = this.setTable(this.tableLen)
    //  this.fullscreenLoading = false
    // setTimeout(() => {
    //   this.fullscreenLoading = false;
    // }, 500);
    this.mountedState = false
  },
  activated() {
    if (this.mountedState == true) {
      return
    }
    this.setStyle()
    // this.fullscreenLoading = true;
    window.addEventListener('keydown', this.onKeydown) // 添加全局Tab键盘事件
    if (this.$route.params.type == 1 || this.$route.params.type == 2) {
      this.routeData = this.$route.params
      this.OnBtnSaveSubmit(this.routeData.item)
    }
    if (this.$route.params.type == 3) {
      this.resetData()
    }
    setTimeout(() => {
      // this.fullscreenLoading = false;
    }, 500)
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
      this.hisRow = this.cloneObject(this.doItem)
      if (type === "materialName" || type === "materialCode") {
        this.findBox(item)
        this.getMaterielData(itemList,type)
      }
    },
    clickEvent() {
      this.defaultAll()
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
    // 重置
    resetData() {
      // 初始化数据
      this.isShowFlag = true;
      this.editId = null;
      this.resetToData()
      this.getCode()
    },
    resetToData() {
      this.$refs['dtData'].resetFields()
      this.addShow = true
      this.dtData.operatorName = this.$store.state.user.name

      this.tableData = []
      this.tableData = this.setTable(this.tableLen)
      this.PostDataEdit = {}
      this.cloneTable = []
      this.selectRow = []

      this.dtData.WarehousingType = ''
      this.dtData.warehousingDate = new Date()
      this.dtData.warehousingOrder = ''
      this.dtData.remark = ''
      this.dtData.operatorId = ''
      this.dtData.receiptName = ''
      this.dtData.receiptId = ''
      this.dtData.auditName = ''
      this.dtData.auditId = ''
      this.dtData.auditStatus = -1
      this.dtData.auditTime = ''
    },
    computeAccount(item, id) {
      if (
        item.defaultWarehouseName.key != '' &&
        this.doItem.materialCode.key != ''
      ) {
        this.GetWarehouseNum(id,item)
      }
      this.DupRemoval(item)
    },
    DupRemoval(item) {
      this.isIF = true
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
            this.isIF = false
            // console.log(this.tableData)
            // this.tableData[i].isShow = true;
            //  console.log(this.tableData[i].isShow)
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
            if (this.tableData[i].unitPrice.value != '') {
              this.tableData[i].amount.value =
                this.tableData[i].unitPrice.value *
                this.tableData[i].PaidIn.value
            }
            this.PaidInBlur(this.tableData[i])
            this.tableData.splice(Index, 1)
          } else {
            this.isIF = false
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
            if (item.unitPrice.value != '') {
              item.amount.value = item.unitPrice.value * item.PaidIn.value
            }
            this.PaidInBlur(item)
            this.tableData.splice(i, 1)
          }
        }
      }
    },
    menuBar(event, num) {
      if (this.dtData.auditStatus == 2) {
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
      this.setTable(1)
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
      // if(typeof item.id!='number') {
      //    this.computeAccount(item,null)
      // }else {
      //   this.computeAccount(item,item.id)
      // }
      this.computeAccount(item,this.editId)
      // 仓库选择
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

      // this.tableData[i].defaultWarehouseName.value
    },
    /**
     * id	主单ID
     *
     *  materialId*	物料ID
     *
     * houseID*	仓库ID
     *
     * 计算可用数量
     */
    GetWarehouseNum(id, item) {
      if (item.materialCode.key == null || item.defaultWarehouseName.key == null) {
        return
      }
      var data = {
        postData: {
          editID: id,
          operateType: 0,
          materialId: item.materialCode.key,
          warehouseId: item.defaultWarehouseName.key
        }
      }
      request({
        url: '/warehouse/api/WarehouseAmount',
        method: 'post',
        data: data
      }).then(res => {
        if (res.code === -1) {
          this.loading = false
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          item.warehouseSum.value = res.data.avaiableNum;
        }
      })
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
        url: 'warehouse/api/TWMOtherWhSendMain/GetOrderNo',
        method: 'GET'
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = ''
        } else {
          this.dtData.warehousingOrder = res
          Storage.LStorage.set('Checkout', res)
        }
      })
    },
    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        // var res = /\d{1,14}(\.\d{1,4})?$/;
        var res = /^\d{1,14}(\.\d{1,4})?$/
        var flag = false
        var unitPriceRel = true
        var remarkRel = true
        var numRel = true
        var PaidIn = true
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

        var checkSelect = await this.checkSelect(this.tableData)
        var childList = []
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
              // 验证实发数量
              item.PaidIn.valid = true
              flag = false
            }
            if (item.PaidIn.value > item.warehouseSum.value) {
              item.PaidIn.valid = true
              PaidIn = false
            }
            if (!res.test(item.PaidIn.value)) {
              // 验证实发数量
              item.PaidIn.valid = true
              numRel = false
            }
            // this.doItem.materialCode.key
            var _LogName = ''
            var param = {
              id: 0,
              mainId: this.selectRow.id,
              materialId: parseFloat(item.materialCode.key) || '',
              warehouseId: parseFloat(item.defaultWarehouseName.key) || '',
              actualNumber: parseFloat(item.PaidIn.value) || ''
            }
            if (state == 1) {
              param.mainId = 0
            }
            // _LogName += `物料:${item.materialName.value} 出库 ${item.PaidIn.value}${item.unit.value} 到 ${item.defaultWarehouseName.value}`
            _LogName += item.materialName.value
            if (item.colorName.key !== '') {
              // 颜色
              param.colorId = item.colorName.key
            }
            if (item.batchNumber.value != '') {
              // 批号
              param.batchNumber = item.batchNumber.value
            }
            if (item.unitPrice.value != '' && item.unitPrice.value != null) {
              // 单价
              param.unitPrice = parseFloat(item.unitPrice.value) || 0
              item.unitPrice.valid = false
              if (param.unitPrice < 0) {
                item.unitPrice.valid = true
                unitPriceRel = false
              }
              if (!res.test(param.unitPrice)) {
                item.unitPrice.valid = true
                numRel = false
              }
            }
            if (item.amount.value != '' && !isNaN(item.amount.value)) {
              // 金额
              param.amount = parseFloat(item.amount.value) || 0
            }
            if (item.amount.value != '' && !res.test(param.amount)) {
              item.amount.valid = true
              numRel = false
            } else {
              item.amount.valid = false
            }
            if (
              item.validityPeriod.value != '' &&
              !isNaN(item.validityPeriod.value)
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
          this.$message.error('数据不完整')
          return
        }
        if (!PaidIn) {
          this.$message({
            message: '实发数量不能大于可用数量',
            type: 'error'
          })
          return
        }
        if (childList.length == 0) {
          this.$message({
            message: '请输入数据',
            type: 'error'
          })
          return
        }
        if (!numRel) {
          this.$message.error('数量不能是负数，也不能超过99999999999999')
          return
        }
        if (!unitPriceRel) {
          this.$message.error('单价不能小于零')
          return
        }
        if (!remarkRel) {
          this.$message.error('备注最大允许输入500个字符，请重新输入')
          return
        }
        var currData = {
          whSendType: this.dtData.WarehousingType,
          whSendDate: formatDate(this.dtData.warehousingDate),
          whSendOrder: this.dtData.warehousingOrder,
          // auditStatus: this.dtData.auditStatus,
          auditStatus: 0,
          operatorId: this.dtData.operatorId,
          receiptId: this.dtData.receiptId,
          childList: childList
        }
      }

      var method = ''
      if (state == 1) {
        currData.id = 0
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
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的出库单`
        }
        var data = RequestObject.CreatePostObject(currData)

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
        url: '/warehouse/api/TWMOtherWhSendMain',
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
            this.dtData.warehousingOrder = Storage.LStorage.remove('Checkout')
          }
          if (state == 2) {
            this.OnBtnSaveSubmit(res.data)
            // this.dtData.warehousingOrder = Storage.LStorage.remove("Checkout");
          }
          if (state == 3) {
            this.tableData = []
            this.resetToData()
            this.dtData.warehousingOrder = Storage.LStorage.get('Checkout')
            // this.setTable(30)
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
      this.computeAccount(this.doItem, this.editId)
      this.doItem.defaultWarehouseName.value = ''
      this.doItem.defaultWarehouseName.key = ''
      if (row.defaultWarehouseId !== null) {
        this.doItem.defaultWarehouseName.value = row.defaultWarehouseName
        this.doItem.defaultWarehouseName.key = row.defaultWarehouseId
        // this.computeAccount(this.doItem, this.editId)
      }
      var code = -1
      for (var i = 0; i < this.tableData.length; i++) {
        if (row.id == this.tableData[i].id) {
          code = i
          break
        }
      }
      for (var i = 0; i < this.tableData.length; i++) {
        if (row.id == this.tableData[i].id) {
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
              return BigNumber(prev)
                .plus(curr)
                .toNumber()
            } else {
              return prev
            }
          }, 0)
          if (index == 6 || index == 7 || index == 10) {
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
    // chengenum(event, item, name, state) {
    //   if (this.dtData.auditStatus != 2) {
    //     // 双击显示input,监听input数据变化
    //     event.stopPropagation();

    //     // this.doDefault(this.doItem)
    //     // this.popoverState = false
    //     this.defaultAll(this.doItem);
    //     item[name].isShow = true;
    //     this.doItem = item;
    //     this.doItemName = name;
    //     this.$nextTick(() => {
    //       if (document.getElementById(item.id + "___" + name)) {
    //         document.getElementById(item.id + "___" + name).focus();
    //       }

    //       if (state == 1) {
    //         this.findBox(item, name);
    //         this.getMaterielData();
    //       }
    //     });
    //   }
    // },
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
    // getMaterielData(item) {
    //   // 获取物料档案信息
    //   this.materielData = []
    //   // this.totalCount = 0

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
        this.tableHeight = b - h - f - 20 - btn
      })
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
      // if (item.PaidIn.value == "" || item.PaidIn.value == null) return;
      // item.PaidIn.value = parseFloat(item.PaidIn.value);
      // item.baseUnitNumber.value = item.PaidIn.value;
      if (isRealNum(item.PaidIn.value) === false) {
        item.PaidIn.value = ''
        return
      }
      if (item.PaidIn.value === '' || item.PaidIn.value === null) return
      if (item.PaidIn.value <= 0) {
        item.PaidIn.value = ''
        return
      }
      item.PaidIn.value = parseFloat(item.PaidIn.value)
      // item.PaidIn.value = keepTwoDecimalFull(item.PaidIn.value);
      item.PaidIn.value = getFloat(item.PaidIn.value, 2)
      item.PaidIn.valid = false
      this.unitPriceBlur(item)
      this.amountBlur(item)
    },
    unitPriceBlur(item) {
      // if (item.unitPrice.value == "" || item.unitPrice.value == null) return;

      // if (item.PaidIn.value != "") {
      //   item.unitPrice.value = parseFloat(item.unitPrice.value);
      //   item.amount.value = keepTwoDecimalFull(
      //     accMul(item.PaidIn.value, item.unitPrice.value)
      //   );
      // } else {
      //   item.unitPrice.value = keepTwoDecimalFull(
      //     parseFloat(item.unitPrice.value)
      //   );
      // }
      if (isRealNum(item.unitPrice.value) === false) {
        item.unitPrice.value = ''
        return
      }
      if (item.unitPrice.value === '' || item.unitPrice.value === null) return
      if (item.unitPrice.value <= 0) {
        item.unitPrice.value = ''
        return
      }

      if (item.PaidIn.value !== '') {
        item.unitPrice.value = parseFloat(item.unitPrice.value)
        item.unitPrice.value = keepTwoDecimalFull(item.unitPrice.value)
        item.amount.value = keepTwoDecimalFull(
          accMul(item.PaidIn.value, item.unitPrice.value)
        )
      } else {
        item.unitPrice.value = keepTwoDecimalFull(
          parseFloat(item.unitPrice.value)
        )
      }
    },
    amountBlur(item) {
      // if (item.amount.value == "" || item.amount.value == null) return;

      // if (item.PaidIn.value != "") {
      //   item.amount.value = parseFloat(item.amount.value);
      //   item.unitPrice.value = keepTwoDecimalFull(
      //     item.amount.value / item.PaidIn.value
      //   );
      // } else {
      //   item.amount.value = keepTwoDecimalFull(parseFloat(item.amount.value));
      // }
      if (isRealNum(item.amount.value) === false) {
        item.amount.value = ''
        return
      }
      if (item.amount.value === '' || item.amount.value === null) return
      if (item.amount.value <= 0) {
        item.amount.value = ''
        return
      }
      item.amount.value = keepTwoDecimalFull(item.amount.value)
      if (item.PaidIn.value !== '') {
        item.amount.value = parseFloat(item.amount.value)
        item.unitPrice.value = keepTwoDecimalFull(
          item.amount.value / item.PaidIn.value
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
      this.$refs.InboundOrder.getMainList()
    },
    cloneObject(origin) {
      return Object.assign({}, origin)
    },
    OnBtnSaveSubmit(row, state) {
      this.editId = row.id;
      if (row.auditStatus == 2) {
        this.isShowFlag = false
      } else {
        this.isShowFlag = true
      }
      this.fullscreenLoading = true
      // 根据ID获取数据
      this.selectRow = this.cloneObject(row)
      this.dtData.WarehousingType = row.whSendType
      // this.dtData.warehousingDate =
      //   row.whSendDate != null ? row.whSendDate.split("T")[0] : "";
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
        var QueryConditions = [
          {
            column: 'id',
            content: row.id,
            condition: 0
          }
        ]
        var rqs = RequestObject.LonginBookObject(
          false,
          0,
          0,
          null,
          QueryConditions
        )
        // this.fullscreenLoading = true
        request({
          url: 'warehouse/api/TWMOtherWhSendMain/GetWholeMainData',
          method: 'GET',
          params: { requestobject: JSON.stringify(row.id) }
        }).then(res => {
          this.addShow = false
          setTimeout(() => {
            // this.fullscreenLoading = false
          }, 200)

          if (res.code === 0) {
            this.dtData.WarehousingType = res.data.whSendType
            this.dtData.warehousingDate =
              res.data.whSendDate != null
                ? new Date(res.data.whSendDate.split('T')[0])
                : ''
            this.dtData.warehousingOrder = res.data.whSendOrder
            this.dtData.operatorId = res.data.operatorId
            this.dtData.operatorName = res.data.operatorName
            this.dtData.auditName = res.data.auditName
            this.dtData.auditId = res.data.auditId
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split('T')[0]
                : ''
            this.dtData.id = res.data.id
            this.dtData.receiptName = res.data.receiptName
            this.dtData.receiptId = res.data.receiptId

            this.setTableData(res.data.childList)
          } else {
            this.$confirm(res.info, '错误信息', {
              confirmButtonText: '确定',
              type: 'error',
              showCancelButton: false
            })
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
          actualNumber: parseFloat(item.PaidIn.value) || ''
        }
        _LogName += `物料:${item.materialName.value} 出库 ${item.PaidIn.value}${item.unit.value} 到 ${item.defaultWarehouseName.value}`
        if (item.batchNumber.value != '') {
          // 批号
          param.batchNumber = item.batchNumber.value
        }
        // 数量
        if (item.PaidIn.value != '' && !isNaN(item.PaidIn.value)) {
          param.PaidIn = parseInt(item.PaidIn.value) || 0
        }
        if (item.unitPrice.value != '' && !isNaN(item.unitPrice.value)) {
          // 单价parseInt
          param.unitPrice = parseFloat(item.unitPrice.value) || 0
        }
        if (item.amount.value != '' && !isNaN(item.amount.value)) {
          // 金额
          param.amount = parseFloat(item.amount.value) || 0
        }
        if (
          item.validityPeriod.value != '' &&
          !isNaN(item.validityPeriod.value)
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
        id: this.selectRow.id,
        warehousingType: this.selectRow.warehousingType,
        whSendDate:
          this.selectRow.whSendDate != null || this.selectRow.whSendDate != ''
            ? this.selectRow.whSendDate.split('T')[0]
            : '',
        whSendOrder: this.selectRow.whSendOrder,
        auditStatus: this.selectRow.auditStatus,
        receiptId: this.selectRow.receiptId,
        whSendType: this.selectRow.whSendType,
        operatorId: this.selectRow.operatorId,
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
          id: item.id,
          editState: false,
          rowIndex: 0,
          // 物料代码
          materialCode: {
            id: newGuid(),
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          // 物料名称
          materialName: {
            id: newGuid(),
            value: item.materialName,
            isShow: false,
            valid: false
          },
          // 规格
          spec: {
            id: newGuid(),
            value: item.spec,
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
          defaultWarehouseName: {
            id: newGuid(),
            // 出货仓库
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          warehouseSum: {
            id: newGuid(),
            // 可用量
            value: item.availableNum,
            isShow: false,
            valid: false
          },
          batchNumber: {
            id: newGuid(),
            value: item.batchNumber, // 批号
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
          // 仓库计量单位名称
          warehouseUnitName: {
            id: newGuid(),
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          // 基本单位数量
          baseUnitNumber: {
            id: newGuid(),
            value: item.baseUnitNumber,
            isShow: false,
            valid: false
          },
          // 出库仓
          PaidIn: {
            id: newGuid(),
            value: item.actualNumber,
            isShow: false,
            valid: false
          },
          // 价格
          unitPrice: {
            id: newGuid(),
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          // 金额
          amount: {
            id: newGuid(),
            value: item.amount,
            isShow: false,
            valid: false
          },
          // 有效期
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
      this.cloneTable = [...this.tableData]
      this.setCurrData(this.cloneTable)
      setTimeout(() => {
        // this.tableData = [...listArr, ...this.setTable(30)];
        if (listArr.length < this.tableLen) {
          this.tableData = [
            ...listArr,
            ...this.setTable(this.tableLen - listArr.length)
          ]
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
        url: 'warehouse/api/TWMOtherWhSendMain/GetWholeMainData',
        method: 'GET',
        params: { requestobject: JSON.stringify(id) }
      }).then(res => {
        this.addShow = false

        if (res.code === 0) {
          this.dtData.WarehousingType = res.data.whSendType
          this.dtData.warehousingDate =
            res.data.whSendDate != null
              ? new Date(res.data.whSendDate.split('T')[0])
              : ''
          this.dtData.warehousingOrder = res.data.whSendOrder
          this.dtData.operatorId = res.data.operatorId
          this.dtData.operatorName = res.data.operatorName
          this.dtData.auditName = res.data.auditName
          this.dtData.auditId = res.data.auditId
          this.dtData.auditTime =
            res.data.auditTime != null ? res.data.auditTime.split('T')[0] : ''
          this.dtData.id = res.data.id
          this.dtData.receiptName = res.data.receiptName
          this.dtData.receiptId = res.data.receiptId

          this.setTableData(res.data.childList)
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
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
      var text = '确定审核通过吗？'
      if (state == 1) {
        text = '确定审核未通过吗？'
      }
      var postData = {
        id: this.selectRow.id,
        auditStatus: state
      }
      if (this.dtData.auditStatus == -1) {
        this.dtData.auditStatus = 0
      }
      var postDataEdit = {
        id: this.selectRow.id,
        auditStatus: this.dtData.auditStatus
      }
      var data = RequestObject.CreatePostObject(
        postData,
        null,
        postDataEdit,
        null
      )
      this.fullscreenLoading = true
      this.$confirm(text, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          // request({
          //   url: "/warehouse/api/TWMDeficitMain/OtherWhAuditAsync",
          //   method: "put",
          //   data: data
          // }).then(res => {
          //   if (res.code === 0) {
          //     this.$message({
          //       message: "操作成功",
          //       type: "success"
          //     });
          //     this.dtData.auditStatus = state;
          //     this.selectRow.auditStatus = state;
          //     setTimeout(() => {
          //       this.state(this.selectRow.id);
          //       this.fullscreenLoading = false;
          //     }, 500);
          //   } else {
          //     this.$confirm(res.info, "错误信息", {
          //       confirmButtonText: "确定",
          //       type: "error",
          //       showCancelButton: false
          //     });
          //     setTimeout(() => {
          //       this.fullscreenLoading = false;
          //     }, 500);
          //   }
          // });
          request({
            url: '/warehouse/api/TWMOtherWhSendMain/OtherWhAuditAsync',
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
                //  this.OnBtnSaveSubmit({ id: this.selectRow.id });
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
    // 撤销审核
    // doOtherWhCancelAudit() {
    //   if (this.selectRow.auditStatus == undefined) {
    //     this.$message.error('请选择审核数据')
    //     return
    //   }
    //   if (this.selectRow.auditStatus != 2) {
    //     this.$message.error('该出库单不是通过状态，无法撤销')
    //     return
    //   }
    //   var data = RequestObject.CreatePostObject({
    //     id: this.selectRow.id,
    //     auditStatus: 3,
    //     ChildList: []
    //   })
    //   this.fullscreenLoading = true
    //   request({
    //     url: '/warehouse/api/TWMOtherWhMain/OtherWhCancelAudit',
    //     method: 'put',
    //     data: data
    //   }).then(res => {
    //     if (res.code === 0) {
    //       this.$message({
    //         message: '保存数据成功',
    //         type: 'success'
    //       })
    //       this.dtData.auditStatus = 3
    //       this.selectRow.auditStatus = 3
    //       setTimeout(() => {
    //         this.fullscreenLoading = false
    //       }, 500)
    //     } else {
    //       this.$confirm(res.info, '错误信息', {
    //         confirmButtonText: '确定',
    //         type: 'error',
    //         showCancelButton: false
    //       })
    //       setTimeout(() => {
    //         this.fullscreenLoading = false
    //       }, 500)
    //     }
    //   })
    // },
    handleDownload() {
      // 导出
      import('@/vendor/Export2Excel').then(excel => {
        const multiHeader = [
          ['name', this.dtData.supplier, 'amount1', 'amount2', 'amount3']
        ]
        const header = ['ID', 'materialCode', 'amount1', 'amount2', 'amount3']
        const filterVal = [
          'id',
          'materialCode',
          'amount1',
          'amount2',
          'amount3'
        ]
        const list = this.tableData
        const data = this.formatJson(filterVal, list)
        const merges = ['A1:A1', 'B1:B1']
        excel.export_json_to_excel({
          multiHeader,
          header,
          merges,
          data
        })
      })
    },
    formatJson(filterVal, jsonData) {
      // 导出
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === 'timestamp') {
            return parseTime(v[j])
          } else {
            return v[j]
          }
        })
      )
    }
  }
}
</script>
<style lang="scss" scoped>
#ProcurementPut /deep/ {
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
    border-bottom: 1px solid #ebeef5;
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
  .el-dropdown__caret-button {
    padding-bottom: 8px;
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
    margin-right: 40px;
  }
  .el-form-item__label {
    padding-right: 4px;
  }
  .FormInput1 {
    .el-input__inner,
    .el-checkbox__inner,
    .el-textarea__inner {
      border-radius: 0;
      border: none;
      border-bottom: 1px solid #ebeef5;
    }
  }
}
.el-table {
  overflow: visible !important;
}
@import "@/styles/receipts.scss";
</style>
