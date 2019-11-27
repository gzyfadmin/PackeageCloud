<template>
  <el-container
    id="Productorders"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    @click.native="listenClick"
  >
    <!-- <div style="padding:5px 0px 0px 20px;height: 37px;">
      <el-button-group v-if="btnAip.add">
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
        >
          {{ btnAip.review.buttonCaption }}
          <el-dropdown-menu slot="dropdown" style="border-bottom: 0px solid #ccc;">
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-button-group>
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
        <!-- 查询 -->
        <el-button
          v-if="btnAip.query.buttonCaption"
          type="default"
          @click="handelAddClick"
        >{{ btnAip.query.buttonCaption }}</el-button>
        <el-dropdown
          v-if="btnAip.review.buttonCaption"
          split-button
          type="default"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
        >
          {{ btnAip.review.buttonCaption }}
          <el-dropdown-menu slot="dropdown" style="border-bottom: 0px solid #ccc;">
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>

    <el-header id="elheader" class="header fromCenter headerBd" height="auto">
      <div class="middleWidth">
        <el-form
          ref="dtData"
          :model="dtData"
          label-width="126px"
          class="FormInput"
          inline
          label-position="right"
          :rules="dtDataRules"
        >
          <el-form-item class="formItem" label="源单类型：">
            <div class="bbfe6">销售订单</div>
          </el-form-item>

          <el-form-item class="formItem disableType cursor" label="源单号：" prop="sourceNo">
            <el-input
              v-if="selectRow.auditStatus!= 2"
              v-model="dtData.sourceNo"
              style="height:32px;min-width:200px;"
              clearable
              @focus="handelAddClick2()"
              @clear="sourceclear"
            >
              <i slot="suffix" class="el-input__icon el-icon-search" />
            </el-input>

            <el-input
              v-if="selectRow.auditStatus== 2"
              v-model="dtData.sourceNo"
              readonly
              style="height:32px;min-width:200px;"
              clearable
              @clear="sourceclear"
            >
              <i slot="suffix" class="el-input__icon el-icon-search" />
            </el-input>
          </el-form-item>

          <el-form-item class="formItem disableType" label="客户名称：" prop="customerId">
            <el-select
              v-model="dtData.customerId"
              style="width:200px;"
              :disabled="selectRow.auditStatus === 2||dtData.sourceId?true:false"
              placeholder="请选择"
              @change="connectchange"
            >
              <el-option
                v-for="item in connectionData"
                :key="item.id"
                :label="item.customerName"
                :value="item.id"
              >
                <span style="color: #8492a6; font-size: 13px">{{ item.customerCode }}</span>
                <span>{{ item.customerName }}</span>
              </el-option>
            </el-select>
          </el-form-item>

          <el-form-item class="formItem disableType" label="生产类型：" prop="productionType">
            <el-select
              v-model="dtData.productionType"
              style="width:200px;"
              :disabled="selectRow.auditStatus == 2||dtData.sourceId?true:false"
              placeholder="请选择"
            >
              <el-option
                v-for="item in orderTypeoption"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item class="formItem" label="订单日期：" prop="orderDate">
            <el-date-picker
              v-model="dtData.orderDate"
              :readonly="selectRow.auditStatus === 2"
              type="date"
              placeholder="选择日期"
              style="width:200px"
            />
          </el-form-item>

          <el-form-item class="formItem" label="计划开始时间：" prop="beginDate">
            <el-date-picker
              v-model="dtData.beginDate"
              :readonly="selectRow.auditStatus === 2"
              type="date"
              placeholder="选择日期"
              style="width:200px"
            />
          </el-form-item>

          <el-form-item class="formItem" label="计划结束时间：" prop="endDate">
            <el-date-picker
              v-model="dtData.endDate"
              :readonly="selectRow.auditStatus === 2"
              type="date"
              placeholder="选择日期"
              style="width:200px"
            />
          </el-form-item>

          <el-form-item class="formItem" label="生产单号：" prop="orderNo">
            <div class="bbfe6">{{ dtData.orderNo?dtData.orderNo:'&nbsp;' }}</div>
          </el-form-item>
          <el-form-item class="formItem" label="状态：">
            <div class="bbfe6">
              <span v-if="dtData.auditStatus==-1" />
              <span v-if="dtData.auditStatus==0">待审核</span>
              <span v-if="dtData.auditStatus==1">未通过</span>
              <span v-if="dtData.auditStatus==2">通过</span>
              <span v-if="dtData.auditStatus==3">撤销审核</span>
            </div>
          </el-form-item>

          <!-- </div> -->
        </el-form>
      </div>
    </el-header>
    <el-main id="elmain">
      <!-- //左边table选择框 -->

      <div v-show="popoverState" class="findBox" :style="popoverStyle" @click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="dicCode" label="包型代码" width="180" />
          <el-table-column prop="dicValue" label="包型名称" />
          <el-table-column prop="mMaterialFile.spec" label="规格型号" />
          <el-table-column prop="mMaterialFile.colorName" label="颜色" />
          <el-table-column prop="mMaterialFile.baseUnitName" label="基本计量单位" />
        </el-table>
        <Pagination
          :page-index="pageIndex"
          :total-count="totalCount"
          :page-size="pageSize"
          @pagination="pagination"
        />
      </div>

      <div v-show="menuState" class="findBox" :style="menuStyle">
        <ul class="menuUl">
          <!-- <li @click="doMenuAdd">增加行</li> -->
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
                  <!-- v-if="!dtData.sourceId&&selectRow.auditStatus !== 2" -->
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="包型代码" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>包型代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'materialCode',1)"
                :class="{validStyle:scope.row.materialCode.valid}"
              >
                <div
                  class="tableTd"
                  v-if="!scope.row.materialCode.isShow"
                >{{scope.row.materialCode.value}}</div>
                <el-input
                  v-if="scope.row.materialCode.isShow"
                  v-model="scope.row.materialCode.value"
                  @input="getPackageDataIndex(scope.row)"
                  @focus="chengenum($event,scope.row,'materialCode',1)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.materialCode"
                type="dicCode"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="包型名称" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>包型名称</span>
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
                  @input="getPackageDataIndex(scope.row)"
                  :id="scope.row.id+'___materialName'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.materialName"
                type="dicValue"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <!-- @change="WarehouseChange(scope.row)" -->

          <el-table-column prop="defaultWarehouseName" label="配色方案">
            <template slot="header">
              <span class="tableHeader">
                <span class="start" />
                <span>配色方案</span>
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
                  filterable
                  @change="WarehouseChange(scope.row)"
                >
                  <el-option
                    v-for="item in scope.row.warehouseData"
                    :key="item.id"
                    :label="item.warehouseName"
                    :value="item.id"
                  />
                </el-select>
              </div>-->
              <EditSelect
                v-model="scope.row.defaultWarehouseName"
                :data="newarritem"
                :is-show-flag="isShowFlag"
                :item-list="scope.row"
                filterable
                clearable
                type="colorName"
                label="warehouseName"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @change="WarehouseChange(scope.row)"
              />
            </template>
          </el-table-column>

          <el-table-column prop="colorName" width="80">
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.colorName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="spec" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.spec.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitName" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitName.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="actualUnitNum" label="基本单位数量" width="120px">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                v-if="scope.row.actualUnitNum.value>0"
                class="tableTd"
              >{{ scope.row.actualUnitNum.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="actualNum" label="数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'actualNum')"
                :class="{validStyle:scope.row.actualNum.valid}"
              >
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
              </div>-->
              <EditInput
                v-model="scope.row.actualNum"
                :class="{redCol:scope.row.actualNum.value<=0,greCol:scope.row.actualNum.value>0}"
                :type="2"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <!-- 【单位】该单位的取值来源是包型档案中配置的库存计量单位。 -->
          <el-table-column prop="produceUnitName" width="80">
            <template slot="header">
              <span class="tableHeader">
                <span>单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                v-if="scope.row.produceUnitName.value"
                class="tableTd"
              >{{ scope.row.produceUnitName.value }}</div>
              <div
                v-if="!scope.row.produceUnitName.value"
                class="tableTd"
              >{{ scope.row.baseUnitName.value }}</div>
              <!-- <div class="tableTd">{{scope.row.produceUnitName.value}}</div> -->
            </template>
          </el-table-column>

          <el-table-column prop="goodsCode" label="客户商品代码" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>客户商品代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'goodsCode')"
                :class="{validStyle:scope.row.goodsCode.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.goodsCode.isShow"
                >{{scope.row.goodsCode.value}}</div>
                <el-input
                  v-show="scope.row.goodsCode.isShow"
                  v-model="scope.row.goodsCode.value"
                  :id="scope.row.id+'___goodsCode'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.goodsCode"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column prop="goodsName" label="客户商品名称" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>客户商品名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'goodsName')"
                :class="{validStyle:scope.row.goodsName.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.goodsName.isShow"
                >{{scope.row.goodsName.value}}</div>
                <el-input
                  v-show="scope.row.goodsName.isShow"
                  v-model="scope.row.goodsName.value"
                  :id="scope.row.id+'___goodsName'"
                  size="mini"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.goodsName"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column prop="validityPeriod" label="交货日期" width="142">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>交货日期</span>
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
                :is-show-flag="ShowFlag"
                @clickEvent="clickEvent"
                @onKeydown="onKeydown"
              />
            </template>
          </el-table-column>

          <el-table-column prop="defaultprincipalName" label="负责人">
            <template slot="header">
              <span class="tableHeader">
                <span class="start" />
                <span>负责人</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.defaultprincipalName.valid}"
                @click="chengenum($event,scope.row,'defaultprincipalName')"
              >
                <div
                  v-show="!scope.row.defaultprincipalName.isShow"
                  class="tableTd"
                >{{ scope.row.defaultprincipalName.value }}</div>
                <el-select
                  v-show="scope.row.defaultprincipalName.isShow"
                  :id="scope.row.id+'___defaultprincipalName'"
                  v-model="scope.row.defaultprincipalName.key"
                  placeholder="请选择"
                  filterable
                  @change="principalChange(scope.row)"
                >
                  <el-option
                    v-for="item in realNameOptions"
                    :key="item.id"
                    :label="item.realName"
                    :value="item.id"
                  />
                </el-select>
              </div>-->
              <EditSelect
                v-model="scope.row.defaultprincipalName"
                :data="realNameOptions"
                :is-show-flag="ShowFlag"
                :item-list="scope.row"
                label="realName"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @change="principalChange(scope.row)"
              />
            </template>
          </el-table-column>

          <el-table-column prop="workshopName" label="生产车间">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>生产车间</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                :class="{validStyle:scope.row.workshopName.valid}"
                @click="chengenum($event,scope.row,'workshopName')"
              >
                <div
                  v-show="!scope.row.workshopName.isShow"
                  class="tableTd"
                >{{ scope.row.workshopName.value }}</div>
                <el-select
                  v-show="scope.row.workshopName.isShow"
                  :id="scope.row.id+'___workshopName'"
                  v-model="scope.row.workshopName.key"
                  placeholder="请选择"
                  filterable
                  @change="workshopChange(scope.row)"
                >
                  <el-option
                    v-for="item in workshopOption"
                    :key="item.id"
                    :label="item.dicValue"
                    :value="item.id"
                  />
                </el-select>
              </div>-->
              <EditSelect
                v-model="scope.row.workshopName"
                :data="workshopOption"
                :is-show-flag="ShowFlag"
                :item-list="scope.row"
                label="dicValue"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @change="workshopChange(scope.row)"
              />
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div id="elfooter" class="fromCenter">
      <el-form
        ref="dtData2"
        label-width="90px"
        class="FormInput"
        :rules="dtDataRules2"
        :model="dtData"
        inline
      >
        <!-- style="width:1368px" -->
        <el-form-item v-if="addControl" label="制单人：" prop="operatorId">
          <div v-if="addControl" class="falseIp">{{ dtData.operatorName }}</div>
        </el-form-item>
        <el-form-item v-if="!addControl" label="制单人：">
          <div v-if="!addControl" class="falseIp">{{ dtData.operatorName }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item label="审核时间：">
          <div class="falseIp">{{ dtData.auditTime }}</div>
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
      </el-form>
    </div>
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit" />

    <!-- @dialogid="dialogid"  -->
    <!-- <el-dialog :visible.sync="showdetialdialog" width="60%" @close="showdetialdialog=false">    </el-dialog> -->
    <listdialog ref="listdialog" :btn-aip="btnAip" @salescode="getsalescode" @reSet="resetData" />
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
// import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import InboundOrder from "./components/InboundOrder";
import listdialog from "./components/listdialog";
import Pagination from "@/components/Pagination";
// import { promises } from 'dns';
import { setTimeout } from "timers";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import { getStyle } from "@/utils/Dom.js";
import BigNumber from "bignumber.js";
import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";
import { Receipt } from "@/utils/Receipt";
import {
  formatDate,
  keepTwoDecimalFull,
  getFloat,
  accMul,
  isRealNum,
  formatMoney,
  trim
} from "@/utils/common.js";

export default {
  name: "viewsProductManageProductordersindexvue",
  mixins: [Receipt],

  components: {
    Pagination,
    InboundOrder,
    listdialog,
    EditInput,
    EditSelect
  },
  filters: {
    formatTDate: value => {
      if (value == "") {
        return "";
      }
      return formatDate(value);
    }
  },
  data() {
    return {
      isShowFlag: true,
      ShowFlag: true,
      showdetialdialog: false,
      warehouseData: [],
      connectionData: [],
      orderTypeoption: [
        {
          dicValue: "库存生产",
          id: 0
        },
        {
          dicValue: "订单生产",
          id: 1
        }
      ],
      // 银行转账、现金结算、微信、支付宝
      settlementTypeoption: [],
      materialItem: {},
      remarkRel: true,
      actualRel: true,
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
        sourceId: null,
        orderNo: "",
        // remark: "",
        auditId: null,
        auditName: null,
        auditStatus: 0,
        auditTime: "",
        contactName: "",
        contactNumber: "",
        customerId: null,
        customerName: null,
        id: null,
        operatorId: null,
        operatorName: "",
        orderDate: new Date(),
        beginDate: "",
        endDate: "",
        orderNo: "",
        productionType: null,
        orderTypeName: null,
        sourceNo: "",
        salesmanName: null,
        settementTypeName: null,
        transferStatus: false
      },
      dtDataRules: {
        orderNo: [{ required: true, message: "获取编号失败" }],
        customerId: [
          { required: true, message: "请选择客户", trigger: "change" }
        ],

        productionType: [
          { required: true, message: "请选择订单类型", trigger: "change" }
        ],
        orderDate: [
          { required: true, message: "请选择日期", trigger: "change" }
        ],
        sourceNo: [
          { required: false, message: "输入正确源单号", trigger: "blur" },
          {
            required: false,
            min: 0,
            max: 50,
            message: "长度在50个字符以下",
            trigger: "blur"
          }
        ],
        beginDate: [
          { required: true, message: "请选择开始日期", trigger: "change" }
        ],
        endDate: [
          { required: true, message: "请选择结束日期", trigger: "change" }
        ]
        // beginDate
      },
      dtDataRules2: {
        contactNumber: [
          { required: false, message: "请输入合法的名称", trigger: "blur" },
          {
            pattern: /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/,
            message: "请输入正确的电话格式",
            trigger: ["blur", "change"]
          }
        ],
        // ^1[3|4|5|6|7|8][0-9]{9}$|

        contactName: [
          {
            required: false,
            min: 0,
            max: 20,
            message: "客户名不合法",
            trigger: "blur"
          },
          {
            pattern: /^[\u4E00-\u9FA5A-Za-z0-9_]+$/,
            message: "客户名不合法",
            trigger: ["blur", "change"]
          }
        ]
      },
      tableHeight: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: [],
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], // 包型信息
      TabArr: [
        "materialCode",
        "materialName",
        "actualNum",
        "goodsCode",
        "goodsName",
        "validityPeriod",
        "defaultprincipalName",
        "workshopName"
      ],
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          warehouseData: [],
          packageId: {
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          materialCode: {
            id: "",
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
          spec: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },

          baseUnitName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },

          accountNum: {
            id: "",
            // 数量
            value: "",
            isShow: false,
            valid: false
          },
          actualNum: {
            id: "",
            // 数量
            value: "",
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            id: "",
            // 基本单位数量
            value: "",
            isShow: false,
            valid: false
          },

          warehouseUnitId: {
            id: "",
            // 基本单位换算率id
            value: "",
            isShow: false,
            valid: false
          },
          produceRate: {
            id: "",
            // 基本单位换算率
            value: "",
            isShow: false,
            valid: false
          },

          batchNumber: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },

          produceUnitName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            id: "",
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          defaultprincipalName: {
            id: "",
            // 数量
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          workshopName: {
            id: "",
            // 数量
            value: "",
            key: "",
            isShow: false,
            valid: false
          },

          goodsCode: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          goodsName: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },

          baseUnitNumber: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          unitPrice: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          amount: {
            id: "",
            value: "",
            isShow: false,
            valid: false
          },
          validityPeriod: {
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
      workshopOption: [],
      addControl: true, // 是否显示新增按钮
      mountedState: false,
      newarritem: []
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

  deactivated() {
    window.removeEventListener("keydown", this.onKeydown);
    this.$refs.InboundOrder.close();
    this.$refs.listdialog.close();
  },

  beforeDestroy() {
    window.removeEventListener("keydown", this.onKeydown);
  },

  async activated() {
    if (this.mountedState == true) {
      return;
    }
    this.setStyle(); // 设置样式
    window.addEventListener("keydown", this.onKeydown);
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );

    if (this.$route.params.type == 1 || this.$route.params.type == 2) {
      this.OtherData = this.$route.params;
      // this.OnBtnSaveSubmit(this.OtherData.item);
      this.OnBtnSaveSubmit(this.OtherData.item);
    }
    if (this.$route.params.type == 3) {
      this.resetData();
    }
    if (this.$route.params.type == 4) {
      this.resetData();
      this.dtData.sourceNo = this.$route.params.item.orderNo;
      this.dtData.sourceId = this.$route.params.item.id;

      this.showdetialdialog = false;
      this.dtData.productionType = 1;
      var para = {
        id: this.dtData.sourceId
      };
      this.OnBtnSaveSubmit_sales(para);
    }
  },
  async mounted() {
    this.mountedState = true;
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.setStyle(); // 设置样式
    this.fullscreenLoading = true;
    window.addEventListener("keydown", this.onKeydown);
    this.warehouseData = await this.getWarehouseData(); // 仓库数据

    if (this.$route.params.type == 1 || this.$route.params.type == 2) {
      this.OtherData = this.$route.params;
      this.OnBtnSaveSubmit(this.OtherData.item);
    } else if (this.$route.params.type == 3) {
      var code = Storage.LStorage.get("Productcode");
      if (!code) {
        this.getCode();
      } else {
        this.dtData.orderNo = code;
      }
      this.dtData.operatorName = this.$store.state.user.name;
      this.dtData.operatorId = 0;
      this.resetData();
      this.fullscreenLoading = false;
    } else if (this.$route.params.type == 4) {
      var code = Storage.LStorage.get("Productcode");
      if (!code) {
        this.getCode();
      } else {
        this.dtData.orderNo = code;
      }
      this.dtData.operatorName = this.$store.state.user.name;
      this.dtData.operatorId = 0;
      this.resetData();
      this.dtData.sourceNo = this.$route.params.item.orderNo;
      this.dtData.sourceId = this.$route.params.item.id;

      this.showdetialdialog = false;
      this.dtData.productionType = 1;
      var para = {
        id: this.dtData.sourceId
      };
      this.OnBtnSaveSubmit_sales(para);
    } else {
      var code = Storage.LStorage.get("Productcode");
      if (!code) {
        this.getCode();
      } else {
        this.dtData.orderNo = code;
      }
      this.dtData.operatorName = this.$store.state.user.name;
      this.dtData.operatorId = 0;
      this.fullscreenLoading = false;
    }

    this.tableData = this.setTable(this.tableLen); // 添加30行
    this.getPackageData(); // 包型数据
    this.getUserCompany(); // 收货员列表
    this.getsettlementTypeoption(); // 收货员列表

    this.getCustomer();

    this.mountedState = false;
  },

  methods: {
    blurCheck(itemList, item, type, state) {
      // td框的类型（可用于数据校验等）
      // （type）对应组件type
      switch (type) {
        // case 1: // 物料代码
        //   if (state == 2) {
        //     // 按下Enter请求数据
        //     this.getPackageData(itemList, (this.pageIndex = 1));
        //   }
        //   break;
        case "dicCode": //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.pageIndex = 1;
            this.getPackageData(itemList, type);
            // this.setMaterielData(item);
            // this.materielData = this.materielData2;
          }
          break;
        case "dicValue": //物料代码
          if (state == 2) {
            //按下Enter请求数据
            this.pageIndex = 1;
            this.getPackageData(itemList, type);
            // this.setMaterielData(item);
            // this.materielData = this.materielData2;
          }
          break;
        case 2:
          this.actualNumBlur(item, itemList);
          break;
        case 3: // 单价
          this.WideValueBlur(item, itemList);
          break;
        default:
          null;
      }
    },
    clickEventAfter(itemList, item, type) {
      // 显示input框之后（创建选择物料代码框）
      this.doItem = itemList;
      if (type === "dicCode" || type === "dicValue") {
        this.findBox(item);
        this.getPackageData(itemList,type);
      }
    },
    clickEvent(itemList, item, type) {
      if (type == "colorName") {
        this.getcolorSolution(itemList);
      }
      this.defaultAll();
    },

    sourceclear() {
      // this.$refs["dtData"].resetFields();
      this.dtData.sourceNo = null;
      this.dtData.sourceId = null;
      (this.dtData.customerId = null),
        (this.dtData.customerName = null),
        this.resetData_sales();
      // hihi
    },

    resetData_sales() {
      // 初始化数据
      this.tableData = [];
      this.tableData = this.setTable(this.tableLen);
      (this.dtData.sourceNo = ""), (this.dtData.sourceId = false);
    },

    getsalescode(e) {
      // console.log(1)
      // hihi
      this.dtData.sourceNo = e[0];
      this.dtData.sourceId = e[1];
      this.showdetialdialog = false;
      this.dtData.productionType = 1;
      this.isShowFlag = false;
      //  this.menuState = false;
      // api/TSSMSalesOrderMain/GetMainList
      // "column":"transProdStatus","content":1,"condition":0
      var para = {
        id: this.dtData.sourceId
      };
      this.OnBtnSaveSubmit_sales(para);
    },

    OnBtnSaveSubmit_sales(row) {
      this.fullscreenLoading = true;
      var rqs = RequestObject.CreateGetObject(false, 0, 0, [
        {
          column: "id",
          content: row.id,
          condition: 0
        }
      ]);

      request({
        url: "/sales/api/TSSMSalesOrderMain/GetMainList",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.dtData.id = null;
          this.dtData.customerId = res.data[0].customerId;
          this.dtData.customerName = res.data[0].customerName;
          this.dtData.orderTypeName = null;
          request({
            url: "sales/api/TSSMSalesOrderMain/GetToProduceList",
            params: { RequestObject: row.id },
            method: "GET"
          }).then(res => {
            this.setTableData_sales(res.data);
          });
        }
      });
    },

    setTableData_sales(dt) {
      // 将数据导入table
      this.tableData = [];
      if (dt[0]) {
      }

      var listArr = [];
      dt.map(item => {
        var newarritem = [];
        this.warehouseData.map(newitem => {
          if (newitem.packageCode == item.packageId) {
            var obj = new Object({
              warehouseName: newitem.warehouseName,
              id: newitem.id,
              packageName: newitem.packageName,
              packageCode: newitem.packageCode
            });
            newarritem.push(obj);
          }
        });

        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          warehouseData: newarritem,
          idandmainid: {
            id: null,
            mainId: null,
            isShow: false,
            valid: false
          },
          packageId: {
            id: newGuid(),
            value: item.packageId,
            key: item.packageId,
            isShow: false,
            valid: false
          },
          materialCode: {
            id: newGuid(),
            value: item.packageCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            id: newGuid(),
            value: item.packageName,
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
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },

          baseUnitName: {
            id: newGuid(),
            // 基本单位名称
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },

          actualNum: {
            id: newGuid(),
            // 数量
            value: item.produceTransProdNum,
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            id: newGuid(),
            // 基本单位数量
            value: item.baseUnitNum,
            isShow: false,
            valid: false
          },
          produceRate: {
            id: newGuid(),
            // 基本单位换算率
            value: item.produceRate,
            isShow: false,
            valid: false
          },
          produceUnitName: {
            id: newGuid(),
            // 单位
            value: item.produceUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitId: {
            id: newGuid(),
            // 基本单位换算率id
            value: item.produceUnitId,
            isShow: false,
            valid: false
          },
          remark: {
            id: newGuid(),
            // 备注
            value: item.remark,
            isShow: false,
            valid: false
          },

          goodsCode: {
            id: newGuid(),
            value: item.goodsCode,
            isShow: false,
            valid: false
          },
          goodsName: {
            id: newGuid(),
            value: item.goodsName,
            isShow: false,
            valid: false
          },
          validityPeriod: {
            id: newGuid(),
            value: "",
            isShow: false,
            valid: false
          },

          warehouseUnitId: {
            id: newGuid(),
            // 基本单位换算率id
            value: item.warehouseUnitId,
            isShow: false,
            valid: false
          },

          defaultWarehouseName: {
            id: newGuid(),
            // 出货仓库
            value: item.colorSolutionName,
            key: item.colorSolutionId,
            isShow: false,
            valid: false
          },

          defaultprincipalName: {
            id: newGuid(),
            // 数量
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          workshopName: {
            id: newGuid(),
            // 数量
            value: "",
            key: "",
            isShow: false,
            valid: false
          }
        };
        listArr.push(list);
      });

      setTimeout(() => {
        // this.setTable(15);
        this.tableData = [...listArr];
      }, 0);
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 500);
    },

    async WarehouseChange(item) {
      // this.computeAccount(this.doItem);
      // //合并物料相同开始
      // var currentNum = -1;
      // for (var i = 0; i < this.tableData.length; i++) {
      //   if (item.id == this.tableData[i].id) {
      //     currentNum = i;
      //     break;
      //   }
      // }

      // for (var i = 0; i < this.tableData.length; i++) {
      //   if (item.id == this.tableData[i].id) {
      //     continue;
      //   }
      //   if (
      //     this.tableData[i].defaultWarehouseName.key ==
      //     item.defaultWarehouseName.key
      //   ) {
      //     this.listenClick();
      //     this.tableData[i].actualNum.isShow = true;
      //     this.$nextTick(() => {
      //       document
      //         .getElementById(this.tableData[i].id + "___" + "actualNum")
      //         .focus();
      //     });
      //     this.tableData.splice(currentNum, 1, this.setTable(1)[0]);
      //     return;
      //   }
      // }
      // //合并物料相同结束
      // console.log(1)
      item.actualUnitNum.value = "";
      // 仓库选择
      this.warehouseData.map(data => {
        if (item.defaultWarehouseName.key == data.id) {
          item.defaultWarehouseName.value = data.warehouseName;
        }
      });
      var MaterialData = await this.getMaterial(
        item.packageId.value,
        item.defaultWarehouseName.key
      );
      this.doItem.materialCode.key = MaterialData.id;
      this.doItem.colorName.value = MaterialData.colorName;
      this.doItem.spec.value = MaterialData.spec;
      this.doItem.produceUnitName.value = MaterialData.produceUnitName;
      this.doItem.baseUnitName.value = MaterialData.baseUnitName;
      this.doItem.warehouseUnitId.value = MaterialData.warehouseUnitId;
      this.doItem.produceRate.value =
        MaterialData.produceRate == null || MaterialData.produceRate == ""
          ? 1
          : MaterialData.produceRate;
      this.actualNumBlur({}, item);
    },

    principalChange(item) {
      // 仓库选择
      this.realNameOptions.map(data => {
        if (item.defaultprincipalName.key == data.id) {
          item.defaultprincipalName.value = data.realName;
        }
      });
    },

    workshopChange(item) {
      // 仓库选择
      this.workshopOption.map(data => {
        if (item.workshopName.key == data.id) {
          item.workshopName.value = data.dicValue;
        }
      });

      // this.tableData[i].workshopName.value
    },

    getWarehouseData(packageCode, packageName) {
      // 查看仓库档案信息
      return new Promise(function(reslove, reject) {
        var reqObject;

        var list = [];
        if (packageCode != -1) {
          list.push({
            column: "packageCode",
            content: packageCode,
            condition: 6
          });
        } else if (packageName != -1) {
          list.push({
            column: "packageName",
            content: packageName,
            condition: 6
          });
        } else {
          return;
        }

        var para = {
          isPaging: false,
          pageSize: 0,
          pageIndex: 0,
          queryConditions: [
            // { column: "packageid", content: 314, condition: 0 }
          ],
          orderByConditions: null
        };

        request({
          url: "/manufacturing/api/TMMColorSolutionMainNew",
          method: "get",
          params: { RequestObject: para }
        }).then(res => {
          var newarr = [];
          res.data.map(item => {
            var obj = new Object({
              warehouseName: item.solutionCode,
              id: item.id,
              packageName: item.packageId,
              packageCode: item.packageId
            });
            newarr.push(obj);
          });
          reslove(newarr);
        });
      });
    },
    connectchange() {
      this.dtData.contactName = null;
      this.dtData.contactNumber = null;
      this.connectionData.map(item => {
        if (item.id == this.dtData.customerId) {
          if (item.childList[0]) {
            this.dtData.contactName = item.childList[0].contactName;
            this.dtData.contactNumber = item.childList[0].contactNumber;
          }
        }
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
    onKeydown(event) {
      // 按下Tab键
      if (event.keyCode !== 9) return;
      var data = this.findCheck(event);
      this.setCheck(data);
    },
    findCheck(event) {
      // 寻找Tab下一个元素
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
      // 重置所有切换了的Input
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
    },

    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = 37; // 按钮高度
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        // this.tableHeight = b - h - f - 40 - 20 - btn;
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.tableHeight = b - h - f - parseInt(pt) - parseInt(pb) - btn;
      });
    },
    resetData() {
      // 初始化数据
      this.isShowFlag = true;
      this.ShowFlag = true;
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      this.tableData = this.setTable(this.tableLen);
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];
      (this.dtData.auditId = null),
        (this.dtData.auditName = null),
        (this.dtData.auditStatus = 0),
        (this.dtData.auditTime = ""),
        (this.dtData.contactName = ""),
        (this.dtData.contactNumber = ""),
        (this.dtData.customerId = null),
        (this.dtData.customerName = null),
        (this.dtData.id = null),
        (this.dtData.operatorId = null),
        (this.dtData.orderDate = new Date());
      (this.dtData.productionType = null),
        (this.dtData.orderTypeName = null),
        (this.dtData.sourceNo = ""),
        (this.dtData.salesmanName = null),
        (this.dtData.settementTypeName = null),
        (this.dtData.beginDate = ""),
        (this.dtData.endDate = ""),
        (this.dtData.transferStatus = false);
      this.dtData.sourceId = false;
    },
    menuBar(event, num) {
      if (this.selectRow.auditStatus === 2) {
        return;
      }
      if (this.dtData.sourceId) {
        return;
      }

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
        url: "/manufacturing/api/TMMProductionOrderMain/GetOrderNo",
        // url: "/sales/api/TSSMSalesOrderMain/GetOrderNo",

        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.orderNo = "";
        } else {
          Storage.LStorage.set("Productcode", res);
          this.dtData.orderNo = res;
        }
      });
    },

    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该生产订单是通过状态，无法编辑");
          return;
        }
        var flag = true;
        var checkSelect = await this.checkSelect(this.tableData);
        var rel = /^\d{1,14}(\.\d{1,4})?$/;
        this.$refs.dtData.validate(valid => {
          // 验证
          if (!valid) {
          } else {
            flag = true;
            this.$refs.dtData2.validate(valid => {
              // 验证
              if (!valid) {
              } else {
                flag = true;

                var childList = [];
                childList = [];
                this.remarkRel = true;
                this.actualRel = true;
                checkSelect.map(item => {
                  // 验证数据
                  item.materialCode.valid = false;
                  item.materialName.valid = false;
                  item.actualNum.valid = false;
                  // item.validityPeriod.valid = false;
                  // if (state == 1) {
                  //   var param = {
                  //     id: 0,
                  //     mainId: 0,
                  //     packageId: item.packageId.value,
                  //     materialId: item.materialCode.key,
                  //     goodsCode: item.goodsCode.value.toString(),
                  //     goodsName: item.goodsName.value.toString(),
                  //     principalId: item.defaultprincipalName.key,
                  //     workshopId: item.workshopName.key,
                  //     productionNum: parseInt(item.actualNum.value)
                  //       ? parseInt(item.actualNum.value)
                  //       : 0,

                  //     colorSolutionId: parseInt(item.defaultWarehouseName.key)
                  //       ? parseInt(item.defaultWarehouseName.key)
                  //       : "",

                  //     deliveryPeriod: formatDate(item.validityPeriod.value),
                  //     colorName: item.colorName.value,

                  //     _LogName: "生产订单其中一条信息"
                  //   };
                  //   if (!item.validityPeriod.value) {
                  //     item.validityPeriod.valid = true
                  //   }
                  //   if (!item.workshopName.key) {
                  //     item.workshopName.valid = true
                  //   }
                  //    childList.push(param);
                  //   if (||!param.workshopId||!param.deliveryPeriod) {
                  //     flag = false;
                  //   }
                  // } else 
                  if (state == 1||state == 2) {
                    var param = {
                      id: 0,
                      colorSolutionId: parseInt(item.defaultWarehouseName.key)
                        ? parseInt(item.defaultWarehouseName.key)
                        : "",
                      packageId: item.packageId.value,

                      principalId: item.defaultprincipalName.key,
                      workshopId: item.workshopName.key,
                      mainId: state == 2?this.globalid:0,
                      materialId: item.materialCode.key,
                      goodsCode: item.goodsCode.value.toString(),
                      goodsName: item.goodsName.value.toString(),

                      productionNum: parseInt(item.actualNum.value)
                        ? parseInt(item.actualNum.value)
                        : 0,

                      deliveryPeriod: formatDate(item.validityPeriod.value),

                      _LogName: "生产订单其中一条信息"
                    };
                  }
                  // console.log(item.idandmainid.id)
                  if (typeof item.idandmainid.id === "number") {
                    param.id = item.idandmainid.id;
                  }
                  if (
                    // item.remark.value.length > 50 ||
                    item.goodsName.value.length > 20 ||
                    item.goodsCode.value.length > 20
                  ) {
                    this.remarkRel = false;
                    //  this.$message.error("备注数值长度超标");
                  }

                  if (item.actualNum.value && !rel.test(item.actualNum.value)) {
                    this.actualRel = false;
                    item.actualNum.valid = true;
                  }

                  if (param.materialId) {
                    if (!item.materialCode.key) {
                      // 验证物料代码
                      item.materialCode.valid = true;
                      item.materialName.valid = true;
                    }
                    if (
                      !item.actualNum.value ||
                      item.actualNum.value > 99999999999999
                    ) {
                      item.actualNum.valid = true;
                    }
                    // if (item.remark.value&&item.remark.value.length > 50) {
                    //   item.remark.valid = true;
                    //   flag = false;
                    // }
                    if (!item.validityPeriod.value) {
                      item.validityPeriod.valid = true;
                    }else {
                      item.validityPeriod.valid = false;
                    }
                    if (!item.workshopName.key) {
                      item.workshopName.valid = true;
                    }else {
                      item.workshopName.valid = false;
                    }
                    if (item.goodsName.value&&item.goodsName.value.length > 50) {
                      item.goodsName.valid = true;
                      flag = false;
                    }else {
                      item.goodsName.valid = false;
                      flag = true;
                    }
                    if (item.goodsCode.value&&item.goodsCode.value.length > 50) {
                      item.goodsCode.valid = true;
                      flag = false;
                    }else {
                      item.goodsCode.valid = false;
                      flag = true;
                    }
                    childList.push(param);
                    if (!param.workshopId||!param.deliveryPeriod) {
                      flag = false;
                    }
                  }
                });

                // haha

                if (state == 1) {
                  // 新增

                  var para = {
                    postData: {
                      id: 0,
                      productionType: this.dtData.productionType,
                      productionNo: this.dtData.orderNo,
                      sourceId: this.dtData.sourceId
                        ? this.dtData.sourceId
                        : null,
                      customerId: this.dtData.customerId,
                      orderDate: formatDate(this.dtData.orderDate),
                      beginDate: formatDate(this.dtData.beginDate),
                      endDate: formatDate(this.dtData.endDate),
                      childList: childList,
                      _LogName: ""
                    }
                  };
                } else if (state == 2) {
                  // 编辑
                  var para = {
                    postData: {
                      id: this.globalid,
                      orderNo: this.dtData.orderNo,
                      productionType: this.dtData.productionType,
                      productionNo: this.dtData.orderNo,
                      sourceId: this.dtData.sourceId
                        ? this.dtData.sourceId
                        : null,
                      orderDate: formatDate(this.dtData.orderDate),
                      beginDate: formatDate(this.dtData.beginDate),
                      endDate: formatDate(this.dtData.endDate),
                      customerId: this.dtData.customerId,
                      childList: childList,
                      _LogName: ""
                    }
                  };
                }

                var beginandend = false;
                var T1 = new Date(this.dtData.beginDate).getTime();
                var T2 = new Date(this.dtData.endDate).getTime();
                // console.log(T1)
                //  console.log(T2)
                if (T2 > T1 || T2 == T1) {
                  var beginandend = true;
                } else {
                  var beginandend = false;
                }
                if (!this.actualRel) {
                  // 校验数量
                  this.$message.error("数量必须小于99999999999999");
                } else if (!this.remarkRel) {
                  // 校验备注
                  this.$message.error("数值长度超标");
                } else if (!beginandend) {
                  this.$message.error("结束时间必须大于或等于开始时间");
                } else {
                  if (state == 1) {
                    if (!flag || para.postData.childList.length == 0) {
                      this.$message.error("数据不完整");
                      return;
                    } else {
                      this.postData(para, state);
                    }
                  } else if (state == 2) {
                    if (this.selectRow.id == undefined) {
                      this.$message.error("请选择修改的数据");
                      return;
                    }
                    if (!flag || para.postData.childList.length == 0) {
                      this.$message.error("数据不完整");
                      return;
                    } else {
                      this.postData(para, state);
                    }
                  }
                }
              }
            });
          }
        });
      }

      if (state == 3) {
        // 删除
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该生产订单是通过状态，无法删除");
          return;
        }

        if (this.selectRow.id == undefined) {
          this.$message.error("请选择删除的数据");
          return;
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的生产订单`
        };

        var data = RequestObject.CreatePostObject(currData);

        this.$confirm("是否删除生产订单？", "确认信息", {
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
        url: "/manufacturing/api/TMMProductionOrderMain",

        // /api/TMMProductionOrderMain
        // url: "/sales/api/TSSMSalesOrderMain",
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
            Storage.LStorage.remove("Productcode");
            // this.resetData()
          }
          if (state == 2) {
            this.OnBtnSaveSubmit({ id: this.selectRow.id });
          }
          if (state == 3) {
            this.tableData = [];
            this.resetData();
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
    // getPackageData(item) {
    //   // 获取包型档案信息
    //   this.materielData = [];
    //   var queryData = [];
    //   if (item) {
    //     // value: item.packageCode,
    //     //         key: item.materialId,
    //     //         isShow: false,
    //     //         valid: false
    //     //       },
    //     //       materialName: {
    //     //         value: item.packageName,
    //     queryData.push({
    //       column: "dicCode",
    //       content: item.materialCode.value,
    //       condition: 6
    //     });
    //     queryData.push({
    //       column: "dicValue",
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
    //   // /basicset/api/TBMPackage
    //   request({
    //     // url: "/basicset/api/TBMMaterialFile",
    //     url: "/basicset/api/TBMPackage",
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
    //       // console.log(this.materielData)
    //       this.totalCount = res.totalNumber;
    //     }
    //   });
    // },
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

      // workshopOption

      // 改到这

      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "生产车间", condition: 0 }
        ],
        orderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.workshopOption = res.data;
          //  dicValue
          // id
        }
      });
    },
    getsettlementTypeoption() {
      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "支付方式", condition: 0 }
        ],
        orderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "get",
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.settlementTypeoption = res.data;
        }
      });
    },
    getMaterial(id, colorSolutionID) {
      return new Promise((reslove, reject) => {
        // console.log(row.id)
        // console.log(item.defaultWarehouseName)
        var data = {
          packageID: id,
          colorSolutionID: colorSolutionID || null
        };
        this.fullscreenLoading = true;
        request({
          url:
            "/manufacturing/api/TMMProductionOrderMain/GetMaterialFileByPackageColor",
          method: "post",
          data: data
        }).then(res => {
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
            this.fullscreenLoading = false;
          } else {
            this.fullscreenLoading = false;
            reslove(res.data);
          }
        });
      });
    },
    getcolorSolution(row) {
      this.newarritem = [];
      if (
        row.materialCode.value &&
        row.materialName.value &&
        row.packageId.value
      ) {
        this.warehouseData.map(newitem => {
          // if(newitem.packageCode==item.materialCode&&newitem.packageName==item.materialName){
          if (newitem.packageCode == row.packageId.value) {
            var obj = new Object({
              warehouseName: newitem.warehouseName,
              id: newitem.id,
              packageName: newitem.packageName,
              packageCode: newitem.packageCode
            });
            this.newarritem.push(obj);
          }
        });
      }
    },
    async rowClick(row) {
      this.defaultAll();
      var that = this;
      this.$emit("setEditState", true);
      // //合并物料相同开始
      // // console.log(row)
      // // console.log(row.materialId)
      // for (var i = 0; i < this.tableData.length; i++) {
      //   if (this.tableData[i].packageId.value == row.id) {
      //     if (
      //       this.tableData[i].defaultWarehouseName.key == "" &&
      //       this.doItem.defaultWarehouseName.key == ""
      //     ) {
      //       this.listenClick();
      //       this.tableData[i].actualNum.isShow = true;
      //       this.$nextTick(() => {
      //         document
      //           .getElementById(this.tableData[i].id + "___" + "actualNum")
      //           .focus();
      //       });
      //       // this.tableData.splice(currentNum, 1, this.setTable(1)[0]);
      //       return;
      //     }
      //   }
      // }
      // //合并物料相同结束
      // console.log(row.id);
      var MaterialData = await this.getMaterial(row.id);
      // console.log(MaterialData)

      this.doItem.materialCode.value = row.dicCode;
      this.doItem.materialCode.key = MaterialData.id;
      this.doItem.materialName.value = row.dicValue;
      this.doItem.packageId.value = row.id;
      // this.warehouseData = await this.getWarehouseData(1,2)

      // console.log(row)
      // this.getcolorSolution(row)
      // this.newarritem = []
      // this.warehouseData.map(newitem => {
      //   // if(newitem.packageCode==item.materialCode&&newitem.packageName==item.materialName){
      //   if (newitem.packageCode == row.id) {
      //     var obj = new Object({
      //       warehouseName: newitem.warehouseName,
      //       id: newitem.id,
      //       packageName: newitem.packageName,
      //       packageCode: newitem.packageCode
      //     })
      //     this.newarritem.push(obj)
      //   }
      // })

      // this.doItem.warehouseData = newarritem;
      this.doItem.colorName.value = MaterialData.colorName;
      this.doItem.spec.value = MaterialData.spec;
      this.doItem.produceUnitName.value = MaterialData.produceUnitName;
      this.doItem.baseUnitName.value = MaterialData.baseUnitName;
      this.doItem.warehouseUnitId.value = MaterialData.warehouseUnitId;
      // this.doItem.produceRate.value = MaterialData.produceRate;
      this.doItem.produceRate.value =
        MaterialData.produceRate == null || MaterialData.produceRate == ""
          ? 1
          : MaterialData.produceRate;

      this.listenClick();
      // this.doItem.actualNum.isShow = true;
      // this.$nextTick(() => {
      //   document.getElementById(this.doItem.id + "___" + "actualNum").focus();
      // });

      // if (that.nowrow) {
      //   that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
      // }
      this.$set(this.doItem["actualNum"], `isShow`, true);
      this.$nextTick(() => {
        var id_ = this.doItem["actualNum"].id;
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
          // actualNum
          if (index == 7 || index == 8) {
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
      // console.log(1)
      // if (this.selectRow.auditStatus === 2) {
      //   return;
      // }
      // if (this.dtData.sourceId) {
      //   console.log(1)
      //   if (
      //     name != "validityPeriod" &&
      //     name != "defaultprincipalName" &&
      //     name != "workshopName"
      //   ) {
      //     return;
      //   }
      // }
      // console.log(this.dtData.sourceId)
      // event.stopPropagation();
      // this.nowrow = item;
      // this.defaultAll(this.doItem);
      // // 这改了
      // item[name].isShow = true;
      // this.doItem = item;
      // this.doItemName = name;
      // this.$nextTick(() => {
      //   if (document.getElementById(item.id + "___" + name)) {
      //     document.getElementById(item.id + "___" + name).focus();
      //   }
      //   if (state == 1) {
      //     this.findBox(item, name);
      //     this.materialItem = [];
      //     this.pageIndex = 1;
      //     this.getPackageData();
      //   }
      // });
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
      // this.doDefault()
      this.defaultAll();
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
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      if (Object.keys(this.materialItem).length !== 0) {
        this.getPackageData(this.materialItem);
      } else {
        this.getPackageData();
      }
    },
    getPackageDataIndex(item) {
      this.pageIndex = 1;
      if (item) {
        this.materialItem = item;
        this.getPackageData(item);
      } else {
        this.getPackageData();
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
            data[i]["isShow"] = false;
          }
        }
      }
    },

    // 输入金额时顺便计算
    actualNumBlur(item, itemList) {
      // if (
      //   item.warehouseUnitId.value != null &&
      //   item.produceRate.value != null
      // ) {
      //   item.actualUnitNum.value = parseFloat(
      //     item.actualNum.value * item.produceRate.value
      //   )
      //     ? keepTwoDecimalFull(item.actualNum.value * item.produceRate.value)
      //     : null;
      // } else {
      //   item.actualUnitNum.value = parseFloat(item.actualNum.value);
      // }
      // // actualNum
      // var r = /^\d+(?=\.{0,1}\d+$|$)/; //正整数
      // var flag = r.test(input);
      // if (!flag) {
      //   item.actualNum.value = null;
      // } else if (item.actualNum.value == 0) {
      //   item.actualNum.value = 0;
      // }
      // var nowactualNum = item.actualNum.value;
      // nowactualNum = parseFloat(nowactualNum);
      // if(isNaN(item.value)||keepTwoDecimalFull(item.value)<0) {
      //   item.value = "";
      // }
      if (
        isRealNum(itemList.actualNum.value) == false ||
        itemList.actualNum.value <= 0
      ) {
        itemList.actualNum.value = "";
        return;
      }

      var actualNum = itemList.actualNum.value;
      var produceRate = itemList.produceRate.value;
      // itemList.actualUnitNum.value = BigNumber(actualNum)
      //   .multipliedBy(produceRate)
      //   .toNumber();
      item.value = getFloat(item.value);
      itemList.actualUnitNum.value = item.value
    },

    unitPriceBlur(item, input) {
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; // 正整数
      var flag = r.test(input); // 验证
      var doc = input.split(".").length - 1; // 验证小数点数

      item.unitPrice.value = item.unitPrice.value.replace(/[^\d|.]/g, "");
      var nowunitPrice = item.unitPrice.value;
      nowunitPrice = parseFloat(nowunitPrice);
      if (nowunitPrice) {
        if (item.actualNum.value != "") {
          item.Totalamount.value = keepTwoDecimalFull(
            accMul(item.actualNum.value, nowunitPrice)
          );
        }
      } else {
        item.Totalamount.value = 0;
      }
    },
    unitPriceBlur2(item, input) {
      var reg = /^\d+(?=\.{0,1}\d+$|$)/;
      var flag = reg.test(item.unitPrice.value);
      if (!flag) {
        item.unitPrice.value = null;
        item.Totalamount.value = null;
      } else {
        item.unitPrice.value = keepTwoDecimalFull(item.unitPrice.value);
      }
    },

    amountBlur(item, input) {
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; // 正整数
      var flag = r.test(input); // 验证
      var doc = input.split(".").length - 1; // 验证小数点数
      item.Totalamount.value = item.Totalamount.value.replace(/[^\d|.]/g, "");
      var nowTotalamount = item.Totalamount.value;
      nowTotalamount = parseFloat(nowTotalamount);

      if (nowTotalamount && item.actualNum.value) {
        if (item.Totalamount.value == "" || item.Totalamount.value == null) {
          return;
        }
        if (item.actualNum.value != "") {
          item.unitPrice.value = keepTwoDecimalFull(
            item.Totalamount.value / item.actualNum.value
          );
        }
      } else {
        item.unitPrice.value = 0;
      }
    },
    amountBlur2(item, input) {
      var reg = /^\d+(?=\.{0,1}\d+$|$)/;
      var flag = reg.test(item.Totalamount.value);
      if (!flag) {
        item.unitPrice.value = null;
        item.Totalamount.value = null;
      } else {
        item.Totalamount.value = keepTwoDecimalFull(item.Totalamount.value);
      }
    },

    handelAddClick() {
      // 查询
      // 点击添加按钮
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.dtData.orderNo = "";
      this.$refs.InboundOrder.dtData.auditStatus = -1;
      this.$refs.InboundOrder.clickRow = {};
      this.$refs.InboundOrder.getMainList();
    },
    handelAddClick2() {
      // 查询
      // 点击添加按钮
      // this.showdetialdialog=true
      this.$refs.listdialog.open();
      this.$refs.listdialog.getMainList();
      this.$refs.listdialog.getorderTypeoption();

      // this.$refs.listdialog.open();

      //   this.setStyle();
      //   this.pathData = getName("/HHHFuz", this.$store.getters.userPermission);
      //   this.getorderTypeoption();
      // this.$refs.InboundOrder.open();
      // this.$refs.InboundOrder.dtData.orderNo = "";
      // this.$refs.InboundOrder.dtData.auditStatus = -1;
      // this.$refs.InboundOrder.clickRow = {};
      // this.$refs.InboundOrder.getMainList();
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },

    OnBtnSaveSubmit(row) {
      this.fullscreenLoading = true;
      this.tableData = this.setTable(this.tableLen); // 添加30行
      var rqs = RequestObject.CreateGetObject(false, 0, 0, [
        {
          column: "id",
          content: row.id,
          condition: 0
        }
      ]);

      request({
        url: "/manufacturing/api/TMMProductionOrderMain/GetMainList",
        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.selectRow = this.cloneObject(res.data[0]);
          this.dtData.id = res.data[0].id;
          this.dtData.auditName = res.data[0].auditName;
          this.dtData.auditStatus = res.data[0].auditStatus;
          this.dtData.auditTime =
            res.data[0].auditTime != null
              ? res.data[0].auditTime.split("T")[0]
              : "";

          this.dtData.contactName = res.data[0].contactName;
          this.dtData.contactNumber = res.data[0].contactNumber;
          this.dtData.customerId = res.data[0].customerId;
          this.dtData.customerName = res.data[0].customerName;
          this.dtData.operatorId = res.data[0].operatorId;
          this.dtData.operatorName = res.data[0].operatorName;

          this.dtData.beginDate =
            res.data[0].beginDate != null
              ? res.data[0].beginDate.split("T")[0]
              : "";
          this.dtData.endDate =
            res.data[0].endDate != null
              ? res.data[0].endDate.split("T")[0]
              : "";
          this.dtData.orderDate =
            res.data[0].orderDate != null
              ? res.data[0].orderDate.split("T")[0]
              : "";

          this.dtData.orderNo = res.data[0].productionNo;
          this.dtData.productionType = res.data[0].productionType;
          this.dtData.orderTypeName = res.data[0].orderTypeName;

          this.dtData.sourceNo = res.data[0].sourceNo;
          this.dtData.sourceId = res.data[0].sourceId;
          this.dtData.salesmanName = res.data[0].salesmanName;

          this.dtData.settementTypeName = res.data[0].settementTypeName;
          this.dtData.transferStatus = res.data[0].transferStatus;
          this.isShowFlag = true;
          this.ShowFlag = true;
          if (this.dtData.sourceNo) {
            this.isShowFlag = false;
            // this.menuState = true;
          }
          if (this.dtData.auditStatus == 2) {
            this.isShowFlag = false;
            this.ShowFlag = false;
          }
          request({
            url: "/manufacturing/api/TMMProductionOrderMain/GetDetailList",
            params: { RequestObject: this.dtData.id },
            method: "GET"
          }).then(res => {
            // this.selectRow = this.cloneObject(res.data);
            this.setTableData(res.data);
          });
        }
      });
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
          warehouseId: parseFloat(item.defaultWarehouseName.key) || ""
          // actualNumber: parseFloat(item.PaidIn.value) || ""
        };
        if (item.batchNumber.value != "") {
          // 批号
          param.batchNumber = item.batchNumber.value;
        }
        if (item.unitPrice.value != "" && !isNaN(item.unitPrice.value)) {
          // 单价
          param.unitPrice = parseFloat(item.unitPrice.value) || 0;
        }
        if (item.amount.value != "" && !isNaN(item.amount.value)) {
          // 金额
          param.amount = parseFloat(item.amount.value) || 0;
        }

        param._LogName = _LogName;
        childList.push(param);
      });
      var currData = {
        id: this.selectRow.id,
        orderNo: this.selectRow.orderNo,
        childList: childList
      };

      this.PostDataEdit = currData;
    },

    setTableData(dt) {
      // 将数据导入table
      // this.tableData = [];
      this.fullscreenLoading = true;

      if (dt[0]) {
        this.globalid = dt[0].mainId;
      }

      var listArr = [];
      dt.map(item => {
        var newarritem = [];
        this.warehouseData.map(newitem => {
          if (newitem.packageCode == item.packageId) {
            var obj = new Object({
              warehouseName: newitem.warehouseName,
              id: newitem.id,
              packageName: newitem.packageName,
              packageCode: newitem.packageCode
            });
            newarritem.push(obj);
          }
        });

        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          warehouseData: newarritem,
          packageId: {
            id: newGuid(),
            value: item.packageId,
            key: item.packageId,
            isShow: false,
            valid: false
          },
          idandmainid: {
            id: item.id,
            mainId: item.mainId,
            isShow: false,
            valid: false
          },
          materialCode: {
            id: newGuid(),
            value: item.packageCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            id: newGuid(),
            value: item.packageName,
            isShow: false,
            valid: false
          },
          colorName: {
            id: newGuid(),
            value: item.colorName,
            isShow: false,
            valid: false
          },
          spec: {
            id: newGuid(),
            value: item.spec,
            isShow: false,
            valid: false
          },

          baseUnitName: {
            id: newGuid(),
            // 基本单位名称
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },

          actualNum: {
            id: newGuid(),
            // 数量
            value: item.productionNum,
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            id: newGuid(),
            // 基本单位数量
            value: item.baseUnitNum,
            isShow: false,
            valid: false
          },

          produceRate: {
            id: newGuid(),
            // 基本单位换算率
            value: item.produceRate,
            isShow: false,
            valid: false
          },

          goodsCode: {
            id: newGuid(),
            value: item.goodsCode,
            isShow: false,
            valid: false
          },
          goodsName: {
            id: newGuid(),
            value: item.goodsName,
            isShow: false,
            valid: false
          },
          validityPeriod: {
            id: newGuid(),
            value: item.deliveryPeriod,
            isShow: false,
            valid: false
          },
          produceUnitName: {
            id: newGuid(),
            // 单位
            value: item.produceUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitId: {
            id: newGuid(),
            // 基本单位换算率id
            value: item.produceUnitId,
            isShow: false,
            valid: false
          },

          defaultprincipalName: {
            id: newGuid(),
            // 数量
            value: item.principalName,
            key: item.principalId,
            isShow: false,
            valid: false
          },
          // workshopId: null
          // workshopName: null
          workshopName: {
            id: newGuid(),
            // 数量
            value: item.workshopName,
            key: item.workshopId,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            id: newGuid(),
            // 出货仓库
            value: item.colorSolutionName,
            key: item.colorSolutionId+"",
            isShow: false,
            valid: false
          }
        };

        listArr.push(list);
      });

      setTimeout(() => {
        // this.setTable(15);
        if (this.dtData.sourceId) {
          this.tableData = [...listArr];
          this.fullscreenLoading = false;
        } else {
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
        }
      }, 0);
    },

    doOtherWhAudit(state) {
      // 审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error("该生产订单已经为通过状态");
        return;
      }
      var data = RequestObject.CreatePostObject({
        id: this.selectRow.id,
        auditStatus: state
      });

      if (state == 1) {
        var tip = "确定审核未通过吗？";
      } else if (state == 2) {
        var tip = "确定审核通过吗？";
      }

      this.$confirm(tip, "确认信息", {
        type: "warning"
      })
        .then(() => {
          this.fullscreenLoading = true;
          request({
            url: "/manufacturing/api/TMMProductionOrderMain/ProduceAudit",
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
                this.OnBtnSaveSubmit({ id: this.globalid });
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
        .catch(() => {
          this.fullscreenLoading = false;
        });
    }
  }
};
</script>

<style>
/* #Productorders #elfooter .el-input__inner {
  border: 0px;
  border-radius: 0px;
  border-bottom: 1px solid #dcdfe6;
} */
#Productorders .el-select-dropdown__empty {
  display: none !important;
}
</style>

<style lang="scss" scoped>
#Productorders /deep/ {
  .redCol {
    color: red;
  }
  .greCol {
    color: #42b983;
  }
  .cursor .el-input__inner {
    cursor: pointer;
  }
  #elheader .el-input__inner {
    // border: 0px;
    // border-radius: 0px;
    // border-bottom: 1px solid #dcdfe6;
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
    border: 1px solid #dcdfe6;
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
    // justify-content: center;
  }
  .footerCenter {
    width: 1150px;
    margin: 0 auto;
  }
  .has-gutter tr td {
    text-align: center;
  }

  .el-form-item__label {
    padding-right: 4px;
  }
  .el-table {
    overflow: visible !important;
  }

  .formItem {
    margin-left: 50px;
    width: 336px;
  }
  .middleWidth {
    width: 1598px;
  }
  @media screen and (max-width: 1948px) {
    .middleWidth {
      width: 1498px;
    }
    .formItem {
      margin-left: 10px;
      width: 337px;
    }
  }

  @media screen and (max-width: 1788px) {
    .middleWidth {
      width: 1348px;
    }
    .formItem {
      margin-left: 10px;
      width: 337px;
    }
  }

  @media screen and (max-width: 1740px) {
    .middleWidth {
      width: 1248px;
    }
    .formItem {
      margin-left: 20px;
      width: 336px;
    }
  }
  @media screen and (max-width: 1388px) {
    .middleWidth {
      width: 948px;
    }
    .formItem {
      margin-left: 40px;
      width: 336px;
    }
  }
  .el-select .el-input.is-disabled .el-input__inner {
    cursor: default;
    color: #606266;
    background: #fff;
  }
  .el-input.is-disabled .el-input__icon {
    cursor: default;
  }
}
.headerBd .el-form {
  border-bottom: none;
  margin-bottom: 0px !important;
}
@import "@/styles/receipts.scss";
</style>
