<template>
  <el-container
    id="Inventorycheck"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    element-loading-text="正在加载"
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

    <el-header id="elheader" class="header" height="auto">
      <el-form
        ref="dtData2"
        :model="dtData"
        label-width="86px"
        class="FormInput"
        inline
        label-position="left"
        :rules="dtDataRules"
      >
        <el-form-item class="formItem disableType" label="盘点仓库：" prop="warehouseId">
          <el-select
            v-model="dtData.warehouseId"
            :disabled="selectRow.auditStatus === 2"
            placeholder="请选择"
            @change="changewarehousepeople"
          >
            <el-option
              v-for="item in warehouseData"
              :key="item.id"
              :label="item.warehouseName"
              :value="item.id"
            />
          </el-select>
        </el-form-item>

        <el-form-item class="formItem" label="盘点日期：" prop="warehousingDate">
          <el-date-picker
            v-model="dtData.warehousingDate"
            type="date"
            placeholder="选择日期"
            style="width:200px"
            :readonly="selectRow.auditStatus === 2"
          />
        </el-form-item>
        <el-form-item class="formItem" label="编号：" prop="warehousingOrder" label-width="58px">
          <div
            class="bbfe6"
            style="height:32px;min-width:128px;"
          >{{ dtData.warehousingOrder?dtData.warehousingOrder:'&nbsp;' }}</div>
        </el-form-item>
      </el-form>
    </el-header>
    <el-main>
      <!-- //左边table选择框 -->

      <div v-show="popoverState" class="findBox" :style="popoverStyle" @click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="materialCode" label="物料代码" width="180" />
          <el-table-column prop="materialName" label="物料名称" />
          <el-table-column prop="spec" label="规格型号" />
          <el-table-column prop="colorName" label="颜色" />
          <el-table-column prop="baseUnitName" label="基本计量单位" />
        </el-table>
        <!-- :pageSizes="[25,1,2]" -->
        <Pagination
          :page-index="pageIndex"
          :total-count="totalCount"
          :page-size="pageSize"
          @pagination="pagination"
        />
      </div>

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

          <el-table-column prop="materialCode" label="物料代码" width="100">
            <!-- <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
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
                  @input="getMaterielDataIndex(scope.row)"
                  @focus="chengenum($event,scope.row,'materialCode',1)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                ></el-input>
              </div>
            </template>-->
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
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

          <el-table-column prop="materialName" label="物料名称" width="100">
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
                  @input="getMaterielDataIndex(scope.row)"
                  :id="scope.row.id+'___materialName'"
                  size="mini"
                ></el-input>
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
          <el-table-column prop="spec" label="规格型号" width="80">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                :class="{validStyle:scope.row.spec.valid}"
                @click="chengenum($event,scope.row,'spec')"
              >
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
              <div
                :class="{validStyle:scope.row.colorName.valid}"
                @click="chengenum($event,scope.row,'colorName')"
              >
                <div class="tableTd">{{ scope.row.colorName.value }}</div>
              </div>
            </template>
          </el-table-column>

          <!-- 【账存数量】是指库存计量单位的库存数量。不填 -->
          <el-table-column prop="accountNum" label="账存数量" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>账存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.accountNum.value }}</div>
            </template>
          </el-table-column>

          <!-- 【实存数量】=【账存数量】-【盘亏数量】 -->
          <!-- 【实存数量】属于必填，通过库存计量单位的实存数量，由制单人手动输入并且自动计算基本单位实存数量。 -->

          <el-table-column prop="actualNum" label="实存数量" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>实存数量</span>
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
                  @blur="actualNumBlur2(scope.row,scope.row.actualNum.value)"
                ></el-input>
              </div>-->
              <EditInput
                v-model="scope.row.actualNum"
                :type="2"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
                @inputChange="computerNum(scope.row)"
              />
            </template>
          </el-table-column>

          <!-- 【实存数量】=【账存数量】-【盘亏数量】 -->
          <!-- 【盘盈数量】中的盘盈数量，与基本计量单位换算率，换算成出该基本单位盘盈数量 -->
          <el-table-column prop="profitNum" label="盘盈数量" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>盘盈数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- {{scope.row.accountNum.value}} 账存-->
              <!-- {{scope.row.actualNum.value}} 实存-->
              <div
                v-if="scope.row.profitNum.value>0"
                class="tableTd"
                style="color:#00CC33"
              >{{ scope.row.profitNum.value }}</div>
              <!-- <div v-if="scope.row.actualNum.value-scope.row.accountNum.value>0" class="tableTd">{{scope.row.actualNum.value-scope.row.accountNum.value}}</div> -->
            </template>
          </el-table-column>

          <!-- 【盘亏数量】数量大于等于0，取值来源是根据【实存数量】减去【账存数量】为负数转化为正数得结果，允许手动输入，输入后【实存数量】=【账存数量】-【盘亏数量】 -->
          <!-- 不填 -->

          <el-table-column prop="deficitNum" label="盘亏数量" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>盘亏数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                v-if="scope.row.deficitNum.value>0"
                class="tableTd"
                style="color:red"
              >{{ scope.row.deficitNum.value }}</div>
              <!-- <div v-if="scope.row.accountNum.value-scope.row.actualNum.value>0" class="tableTd">{{scope.row.accountNum.value-scope.row.actualNum.value}}</div> -->
            </template>
          </el-table-column>
           <!-- 【单位】该单位的取值来源是物料档案中配置的库存计量单位。 -->
          <el-table-column prop="warehouseUnitName" width="80">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- scope.row.unit.value -->
              <div
                v-if="scope.row.warehouseUnitName.value"
                class="tableTd"
              >{{ scope.row.warehouseUnitName.value }}</div>
              <div
                v-if="!scope.row.warehouseUnitName.value"
                class="tableTd"
              >{{ scope.row.unit.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="unit" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.unit.value }}</div>
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
              <div class="tableTd">{{ scope.row.accountUnitNum.value }}</div>
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
                v-if="scope.row.actualUnitNum.value>0"
                class="tableTd"
              >{{ scope.row.actualUnitNum.value }}</div>
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
                v-if="scope.row.profitUnitNum.value>0"
                class="tableTd"
              >{{ scope.row.profitUnitNum.value }}</div>
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
                v-if="scope.row.deficitUnitNum.value>0"
                class="tableTd"
              >{{ scope.row.deficitUnitNum.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="remark" label="备注" width="240">
            <template slot="header">
              <span class="tableHeader">
                <span class="start" />
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
    <div id="elfooter">
      <el-form
        ref="dtData"
        label-width="76px"
        class="FormInput"
        :rules="dtDataRules"
        :model="dtData"
        inline
      >
        <el-form-item v-if="dtData.auditStatus!=2" label="仓管员：">
          <el-select
            v-if="disappear"
            v-model="dtData.principalId"
            filterable
            placeholder="请选择"
            @change="changeprincipalId"
          >
            <el-option
              v-for="item in warehousepeople"
              :key="item.principalId"
              :label="item.realName"
              :value="item.principalId"
            />
          </el-select>

          <!-- <div class="falseIp" v-if="!addControl">{{ dtData.whAdminName }}</div> -->
          <!-- addControl -->
        </el-form-item>

        <el-form-item v-if="dtData.auditStatus==2" label="仓管员：">
          <div v-if="dtData.auditStatus==2" class="falseIp">{{ dtData.whAdminName }}</div>
          <!-- addControl -->
        </el-form-item>
        <!-- principalId -->
        <!-- operatorlist -->
        <el-form-item v-if="dtData.auditStatus!=2" label="盘点员：" prop="inventoryId">
          <el-select v-model="dtData.inventoryId" filterable placeholder="请选择">
            <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            />
          </el-select>
        </el-form-item>

        <el-form-item v-if="dtData.auditStatus==2" label="盘点员：">
          <div v-if="dtData.auditStatus==2" class="falseIp">{{ dtData.inventoryName }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item v-if="addControl" label="制单人：" prop="operatorId">
          <div v-if="addControl" class="falseIp">{{ dtData.operatorName }}</div>
        </el-form-item>
        <el-form-item v-if="!addControl" label="制单人：">
          <div v-if="!addControl" class="falseIp">{{ dtData.operatorName }}</div>
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
    <InboundOrder ref="InboundOrder" title="盘点单详情" @OnBtnSaveSubmit="OnBtnSaveSubmit" />
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import {
  formatDate,
  keepTwoDecimalFull,
  accMul,
  isRealNum,
  isEmpty,
  getFloat
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import InboundOrder from "./components/InboundOrder";

import Pagination from "@/components/Pagination";
// import { promises } from 'dns';
import { setTimeout } from "timers";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import BigNumber from "bignumber.js";
import EditInput from "@/components/EditTable/EditInput";
import EditSelect from "@/components/EditTable/EditSelect";
import { Receipt } from "@/utils/Receipt";

export default {
  name: "viewsinventoryInventorycheckindexvue",
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
  mixins: [Receipt],
  data() {
    return {
      materialItem: {},
      remarkRel: true,
      actualRel: true,
      TabArr: ["materialCode", "materialName", "actualNum", "remark"],
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
        principalId: "",
        inventoryId: "",
        whAdminName: "",
        inventoryName: "",
        whAdminId: "",
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
        principalId: [
          { required: true, message: "请选择仓管员", trigger: "change" }
        ],
        inventoryId: [
          { required: true, message: "请选择盘点员", trigger: "change" }
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
      materielData: [], // 物料信息
      warehouseData: [], // 仓库信息
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            value: "",
            key: "",
            isShow: false,
            valid: false,
            id: ""
          },
          idandmainid: {
            id: 0,
            value: "",
            mainId: 0,
            isShow: false,
            valid: false,
            id: ""
          },
          materialName: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          colorName: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          spec: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          unit: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          accountNum: {
            // 账存数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          actualNum: {
            // 实存数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          accountUnitNum: {
            // 基本单位账存数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          actualUnitNum: {
            // 基本单位实存数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          profitNum: {
            // 盘盈数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          deficitNum: {
            // 盘亏数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          profitUnitNum: {
            // 基本单位盘盈数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          deficitUnitNum: {
            // 基本单位盘亏数量
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          warehouseUnitId: {
            // 基本单位换算率id
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          warehouseRate: {
            // 基本单位换算率
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          remark: {
            // 备注
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          batchNumber: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },

          warehouseUnitName: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          baseUnitNumber: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          unitPrice: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          amount: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
          },
          validityPeriod: {
            value: "",
            isShow: false,
            valid: false,
            id: ""
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
      addControl: true // 是否显示新增按钮
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
    this.$refs.InboundOrder.close();
    window.removeEventListener("keydown", this.onKeydown);
  },

  async activated() {
    this.setStyle(); // 设置样式
    window.addEventListener("keydown", this.onKeydown);
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    if (this.$route.params.type == 3) {
      this.resetData();
    } else {
      if (this.$route.params.type) {
        this.OtherData = this.$route.params;
        this.getMainList2(this.OtherData.item);
      }
    }
  },
  mounted() {
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    // this.tableData2 = [...this.tableData]; //克隆原始table数据
    this.fullscreenLoading = true;
    window.addEventListener("keydown", this.onKeydown);
    this.setStyle(); // 设置样式

    if (this.$route.params.type == 3) {
      this.resetData();
    } else {
      if (this.$route.params.type) {
        this.OtherData = this.$route.params;
        // this.OnBtnSaveSubmit(this.OtherData.item);
        this.getMainList2(this.OtherData.item);
      } else {
        var code = Storage.LStorage.get("checkcode");
        if (!code) {
          this.getCode();
        } else {
          this.dtData.warehousingOrder = code;
        }
        this.dtData.operatorName = this.$store.state.user.name;
        this.dtData.operatorId = 0;
      }
    }

    this.tableData = this.setTable(this.tableLen); // 添加30行
    this.getMaterielData(); // 物料数据
    this.getWarehouseData(); // 仓库数据
    this.getUserCompany(); // 收货员列表
    this.fullscreenLoading = false;
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
        //     this.getMaterielData(itemList)
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
        case 2:
          this.numBlur(item, itemList);
          break;
        case 3: // 单价
          break;
        default:
          null;
      }
    },
    clickEventAfter(itemList, item, type) {
      // 显示input框之后（创建选择物料代码框）
      this.doItem = itemList;
      if (type === "materialName" || type === "materialCode") {
        this.findBox(item);
        this.getMaterielData(itemList,type);
      }
    },
    clickEvent() {
      this.defaultAll();
    },
    numBlur(item, itemList) {
      if (
        isRealNum(item.value) === false ||
        BigNumber(item.value).toNumber() < 0
      ) {
        item.value = "";
      } else {
        item.value = getFloat(BigNumber(item.value).toNumber(), 4);
      }
      this.computerNum(itemList);
    },
    computerNum(itemList) {
      // 计算数据
      // 初始化数量
      itemList.accountUnitNum.value = ""; // 基本单位账存数量
      itemList.actualUnitNum.value = ""; // 基本单位实存数量
      itemList.profitUnitNum.value = ""; // 基本单位盘盈数量
      itemList.deficitUnitNum.value = ""; // 基本单位盘亏数量
      itemList.profitNum.value = ""; // 盘盈数量
      itemList.deficitNum.value = ""; // 盘亏数量

      // 判断是否是数字
      if (
        isRealNum(itemList.accountNum.value) == false ||
        isRealNum(itemList.actualNum.value) == false
      ) {
        return;
      }

      var accountNum = itemList.accountNum.value; // 账存数量
      var actualNum = itemList.actualNum.value; // 实存数量
      var warehouseRate = itemList.warehouseRate.value; // 实存数量

      var minusNum = BigNumber(actualNum)
        .minus(accountNum)
        .toNumber();
      // 设置基本单位账存数量
      this.setBaseNum(itemList);
      // 设置基本单位实存数量
      itemList.actualUnitNum.value = BigNumber(actualNum)
        .multipliedBy(warehouseRate)
        .toNumber();

      // 盘盈
      if (minusNum > 0) {
        itemList.deficitNum.value = "";
        itemList.profitNum.value = BigNumber(actualNum)
          .minus(accountNum)
          .toNumber();
        itemList.profitUnitNum.value = BigNumber(itemList.profitNum.value)
          .multipliedBy(warehouseRate)
          .toNumber();
      }
      // 盘亏
      if (minusNum < 0) {
        itemList.profitNum.value = "";
        itemList.deficitNum.value = BigNumber(accountNum)
          .minus(actualNum)
          .toNumber();
        itemList.deficitUnitNum.value = BigNumber(itemList.deficitNum.value)
          .multipliedBy(warehouseRate)
          .toNumber();
      }
      // if (minusNum == 0) {
      //   itemList.profitNum.value = "";
      //   itemList.deficitNum.value = "";
      // }
      // new BigNumber(0.1).minus(0.1);
      // console.log(itemList.accountNum.value);
      // console.log(itemList.actualNum.value);
    },
    setBaseNum(itemList) {
      var accountNum = itemList.accountNum.value;
      var warehouseRate = itemList.warehouseRate.value;
      itemList.accountUnitNum.value = BigNumber(accountNum)
        .multipliedBy(warehouseRate)
        .toNumber();
    },
    // WarehouseChange(item) {
    //     //仓库选择
    //     this.warehouseData.map(data => {
    //       if (item.key == data.id) {
    //         item.value = data.warehouseName;
    //       }
    //     });
    //   },

    // changewarehousepeople() {

    //   },
    changewarehousepeople(item) {
      // 仓库选择

      var that = this;
      that.warehouseData.map(item => {
        if (item.id == that.dtData.warehouseId) {
          if (!isEmpty(item.principalId)) {
            that.dtData.principalId = parseInt(item.principalId);
          }else{
            that.dtData.principalId = "";
          }

          return false;
        }
      });

      this.computeAccount();
      // this.computerNum(this.doItem)
      // this.warehouseData.map(data => {
      //   if (item.key == data.id) {
      //     item.value = data.warehouseName;
      //   }
      // });
      // item.valid = false;
    },
    async computeAccount2() {
      var data = await this.getWarehouseAmount(
        this.doItem.materialCode.key,
        this.dtData.warehouseId
      );
      this.doItem.accountNum.value = data.accountNum;
      this.computerNum(this.doItem);
      this.setBaseNum(this.doItem);

      // this.actualNumBlur_(this.doItem, this.doItem.actualNum.value);
    },
    async computeAccount(e) {
      // 获取账存数量
      var that = this;
      this.fullscreenLoading = true;
      var checkSelect = await this.checkSelect(this.tableData);

      // if(checkSelect.length==0){
      //   this.fullscreenLoading=false
      // }
      var havedata = [];
      checkSelect.map(async item => {
        if (item.materialCode.key) {
          var data = await this.getWarehouseAmount(
            item.materialCode.key,
            that.dtData.warehouseId
          );
          item.accountNum.value = data.accountNum;
          // that.actualNumBlur(item, item.actualNum.value);
          this.computerNum(item);
          this.setBaseNum(item);
          havedata.push(item.materialCode.key);
        }
      });
      // if (havedata.length == 0) {
      this.fullscreenLoading = false;
      // }

      // if (e == "nowrow") {
      // that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
      // }
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
        this.fullscreenLoading = true;
        request({
          url: "/warehouse/api/WarehouseAmount",
          method: "post",
          data: data
        }).then(res => {
          setTimeout(() => {
            reslove(res.data);
            this.fullscreenLoading = false;
          }, 200);
        });
      });
    },

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
    //   //设置table切换
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
      // 重置所有切换了的Input
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
    },
    changeprincipalId() {
      var that = this;
      that.disappear = false;
      that.disappear = true;
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
    //     this.tableHeight = b - h - f - 20 - btn;
    //   });
    // },
    resetData() {
      // 初始化数据
      this.isShowFlag = true;
      // this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      this.tableData = this.setTable(this.tableLen);
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.principalId = "";
      this.dtData.warehouseId = "";

      this.dtData.WarehousingType = "";
      this.dtData.warehousingDate = new Date();
      this.dtData.warehousingOrder = "";
      this.dtData.remark = "";
      this.dtData.operatorName = "";
      this.dtData.operatorId = "";
      this.dtData.inventoryId = "";
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
      if (this.dtData.auditStatus == 2) {
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
        url: "warehouse/api/TWMInventoryMain/GetOrderNo",
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.warehousingOrder = "";
        } else {
          Storage.LStorage.set("checkcode", res);
          this.dtData.warehousingOrder = res;
        }
      });
    },

    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该盘点单是通过状态，无法编辑");
          return;
        }
        var flag = true;
        var checkSelect = await this.checkSelect(this.tableData);
        this.$refs.dtData2.validate(valid => {
          // 验证
          if (!valid) {
            // this.$message({
            //   message: "数据不合法，无法保存",
            //   type: "error"
            // });
          } else {
            this.$refs.dtData.validate(valid => {
              // 验证
              if (!valid) {
                // this.$message({
                //   message: "数据不合法，无法保存",
                //   type: "error"
                // });
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
                  item.remark.valid = false;

                  if (item.editState) {
                    if (item.materialCode.key == "") {
                      // 验证物料代码
                      item.materialCode.valid = true;
                      item.materialName.valid = true;
                      item.spec.valid = true;
                      flag = false;
                    }
                    // if (item.actualNum.value == "") {
                    //   // 验证物料代码
                    //   if() {
                    //     item.actualNum.valid = true;
                        
                    //   }
                    // }

                    if (state == 1) {
                      var param = {
                        id: 0,
                        mainId: 0,
                        materialId: item.materialCode.key,
                        warehouseId: this.dtData.warehouseId,
                        accountNum: item.accountNum.value
                          ? item.accountNum.value
                          : 0,
                        actualNum: parseInt(item.actualNum.value)
                          ? parseInt(item.actualNum.value)
                          : 0,
                        profitNum: item.profitNum.value
                          ? item.profitNum.value
                          : 0,
                        deficitNum: item.deficitNum.value
                          ? item.deficitNum.value
                          : 0,
                        remark: item.remark.value,
                        _LogName: "盘点单其中一条信息"
                      };
                    } else if (state == 2) {
                      var param = {
                        id: item.idandmainid.id ? item.idandmainid.id : 0,
                        mainId: this.globalid,
                        materialId: item.materialCode.key,
                        warehouseId: this.dtData.warehouseId,
                        accountNum: item.accountNum.value
                          ? item.accountNum.value
                          : 0,
                        actualNum: parseInt(item.actualNum.value)
                          ? parseInt(item.actualNum.value)
                          : 0,
                        profitNum: item.profitNum.value
                          ? item.profitNum.value
                          : 0,
                        deficitNum: item.deficitNum.value
                          ? item.deficitNum.value
                          : 0,
                        remark: item.remark.value,
                        _LogName: "盘点单其中一条信息"
                      };
                    }

                    if (param.materialId) {
                      if (!item.materialCode.key) {
                        // 验证物料代码
                        item.materialCode.valid = true;
                        item.materialName.valid = true;
                      }

                      if (!item.actualNum.value || isNaN(item.actualNum.value)) {
                        if(item.actualNum.value!=0) {
                        item.actualNum.valid = true;
                        flag = false;
                      }
                      }
                      if (item.remark.value.length > 50) {
                        item.remark.valid = true;
                      }
                      childList.push(param);
                    }

                    if (item.remark.value.length > 50) {
                      this.remarkRel = false;
                      //  this.$message.error("备注数值长度超标");
                    }

                    if (item.actualNum.value > 99999999999999) {
                      if(item.actualNum.value!=0) {
                        this.actualRel = false;
                      }
                      //  this.$message.error("备注数值长度超标");
                    }
                  }
                });
                if (flag == false) {
                  this.$message({
                    message: "数据不合法，无法保存",
                    type: "error"
                  });
                  return;
                }
                if (state == 1) {
                  var para = {
                    postData: {
                      id: 0,
                      warehouseId: this.dtData.warehouseId,
                      inventoryDate: formatDate(this.dtData.warehousingDate),
                      inventoryOrder: this.dtData.warehousingOrder,
                      operatorId: 0,
                      inventoryId: this.dtData.inventoryId,
                      whAdminId: this.dtData.principalId,
                      childList: childList,
                      _LogName: `新增一个编号为${this.dtData.warehousingOrder}的盘点单`
                    }
                  };
                } else if (state == 2) {
                  var para = {
                    postData: {
                      id: this.globalid,
                      warehouseId: this.dtData.warehouseId,
                      inventoryDate: formatDate(this.dtData.warehousingDate),
                      inventoryId: this.dtData.inventoryId,
                      whAdminId: this.dtData.principalId,
                      childList: childList,
                      _LogName: `修改一个编号为${this.dtData.warehousingOrder}的盘点单`
                    }
                  };
                }

                if (!this.actualRel) {
                  this.$message.error("实存数量不能是负数，也不能超过99999999999999");
                } else if (!this.remarkRel) {
                  this.$message.error("备注数值长度超标");
                } else {
                  if (state == 1) {
                    // !para.postData.operatorId ||
                    if (
                      !flag ||
                      !para.postData.warehouseId ||
                      !para.postData.inventoryDate ||
                      !para.postData.inventoryOrder ||
                      !para.postData.inventoryId ||
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
              }
            });
          }
        });
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
            Storage.LStorage.remove("checkcode");
            // this.resetData()
          }
          if (state == 2) {
            this.OnBtnSaveSubmit({ id: this.selectRow.id });
          }

          if (state == 3) {
            this.tableData = [];
            // this.setTable(15);
            // this.tableData = this.setTable(15);
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

          const obj = {};
          const peon = warehousepeople.reduce((cur, next) => {
            obj[next.principalId]
              ? ""
              : (obj[next.principalId] = true && cur.push(next));
            return cur;
          }, []); // 设置cur默认类型为数组，并且初始值为空的数组
          this.warehousepeople = peon;
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
    // },
    // getMaterielData(item) {
    //   // 获取物料档案信息

    //   this.materielData = [];
    //   // this.totalCount = 0;

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
        // /system/api/TSMUser/GetUserInCurentCompany
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
      if (this.dtData.warehouseId == "") {
        this.$message.error("请选择盘点仓库");
        return;
      }
      this.doItem.materialCode.value = row.materialCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.materialName;
      this.doItem.spec.value = row.spec;
      this.doItem.colorName.value = row.colorName;
      this.doItem.warehouseUnitName.value = row.warehouseUnitName;
      this.doItem.unit.value = row.baseUnitName;
      this.doItem.baseUnitNumber.value = row.baseUnitNumber;
      this.doItem.warehouseUnitId.value = row.warehouseUnitId;
      this.doItem.warehouseRate.value =
        row.warehouseRate == null || row.warehouseRate == ""
          ? 1
          : row.warehouseRate;
      // console.log(row.warehouseRate)

      // var currData = {
      //   postData: {
      //     materialId: row.id,
      //     warehouseId: row.defaultWarehouseId
      //   }
      // };
      // var that = this;
      // request({
      //   url: "/warehouse/api/WarehouseAmount",
      //   method: "post",
      //   data: currData
      // }).then(res => {
      //   if (res.code === 0) {
      //     this.doItem.accountNum.value = res.data.accountNum;
      //     that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
      //     // item.actualNum.value
      //   } else {
      //   }
      // });
      // 合并物料相同开始
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
          this.selectInput(this.tableData[i], "actualNum");
          this.tableData.splice(currentNum, 1, this.setTable(1)[0]);
          return;
        }
      }
      // 合并物料相同结束

      this.computeAccount2();
      this.selectInput(this.doItem, "actualNum");
      // this.listenClick();
      // this.tableData[i]["actualNum"].isShow = true;
      // this.$nextTick(() => {
      //   document
      //     .getElementById(this.tableData[i].id + "___" + "actualNum")
      //     .focus();
      // });
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
      // 双击显示input,监听input数据变化
      if (this.selectRow.auditStatus === 2) {
        return;
      }
      event.stopPropagation();
      this.nowrow = item;
      this.defaultAll(this.doItem);
      // this.doDefault(this.doItem);
      // this.popoverState = false;
      // 这改了
      item[name].isShow = true;
      this.doItem = item;
      this.doItemName = name;
      this.$nextTick(() => {
        if (document.getElementById(item.id + "___" + name)) {
          document.getElementById(item.id + "___" + name).focus();
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

    //   //pol和PoT是主要数据

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
    // pagination(val) {
    //   //改变页数
    //   this.pageSize = val.pageSize;
    //   this.pageIndex = val.pageIndex;
    //   this.getMaterielData();
    // },
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
            data[i]["isShow"] = false;
          }
        }
      }
    },

    actualNumBlur2(item, input) {
      var reg = /^\d+(?=\.{0,1}\d+$|$)/;
      var flag = reg.test(item.actualNum.value);
      if (!flag) {
        item.actualNum.value = null;
      } else {
        // item.actualNum.value = keepTwoDecimalFull(item.actualNum.value);
        item.actualNum.value = getFloat(parseFloat(item.actualNum.value), 4);
      }

      if (item.actualNum.value == "" || item.actualNum.value == null) {
        // 如果不是输入数字实存数量默认为0
        var nowactualNum = 0;
        // item.PaidIn.value = parseFloat(item.PaidIn.value)
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

    // 输入金额时顺便计算
    actualNumBlur(item, input) {
      // hihi

      // actualNum

      // var r = /^\d+(?=\.{0,1}\d+$|$)/; //正整数
      // var flag = r.test(input);

      // if (!flag) {
      //   item.actualNum.value = null;
      // } else if (item.actualNum.value == 0) {
      //   item.actualNum.value = 0;
      // }

      // if (isRealNum(item.actualNum.value) === false) {
      //         item.actualNum.value = "";
      //         return;
      //       }
      //       if (item.actualNum.value === "" || item.actualNum.value === null) return;
      //       if (item.actualNum.value <= 0) {
      //         item.actualNum.value = "";
      //         return;
      //       }
      //       item.actualNum.value = parseFloat(item.actualNum.value);
      //       // item.actualNum.value = keepTwoDecimalFull(item.actualNum.value);
      //       item.actualNum.value = getFloat(item.actualNum.value, 2);

      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; // 正整数
      var flag = r.test(input); // 验证
      // input = (input == null||input == null) ? 0 : input;
      // console.log(input);
      var doc = input.split(".").length - 1; // 验证小数点数

      item.actualNum.value = item.actualNum.value.replace(/[^\d|.]/g, "");
      // if (!flag && doc != 1) {
      //   //验证当小数点非一个时且不是正整数
      //   item.actualNum.value = null;
      // } else if (doc == 1 && !flag) {
      //   //去掉除整数和小数点以外的字符
      //   item.actualNum.value = item.actualNum.value.replace(/[^\d|.]/g, "");
      // }
      var nowactualNum = item.actualNum.value;
      nowactualNum = parseFloat(nowactualNum);

      // if (item.actualNum.value == "" || item.actualNum.value == null) return;
      if (item.actualNum.value == "" || item.actualNum.value == null) {
        // 如果不是输入数字实存数量默认为0
        var nowactualNum = 0;
        // item.PaidIn.value = parseFloat(item.PaidIn.value)
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
      // this.doItem.actualNum.isShow = true;
      // this.$set(this.doItem["actualNum"], `isShow`, true);
      this.$nextTick(() => {
        this.selectInput(this.doItem, "actualNum");
        // document.getElementById(this.doItem.id + "___" + "actualNum").focus();
        // document.getElementById(this.doItem.id + "___" + "actualNum").select();
      });
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
      // 查询
      // 点击添加按钮
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

    OnBtnSaveSubmit(row) {
      if (row.auditStatus == 2) {
        this.isShowFlag = false;
      } else {
        this.isShowFlag = true;
      }
      request({
        url: "warehouse/api/TWMInventoryMain/GetInventoryOrder?id=" + row.id,
        method: "GET"
      }).then(res => {
        if (res.code === 0) {
          this.setTableData(res.data.childList);
          this.selectRow = this.cloneObject(res.data);
          this.dtData.warehousingDate = new Date(
            res.data.inventoryDate.split("T")[0]
          );
          this.dtData.warehouseId = res.data.warehouseId;
          this.dtData.warehousingOrder = res.data.inventoryOrder;
          this.dtData.warehouseName = res.data.warehouseName;
          this.dtData.warehousingOrder = res.data.inventoryOrder;
          this.dtData.whAdminName = res.data.whAdminName;
          this.dtData.principalId = res.data.whAdminId;
          this.dtData.operatorName = res.data.operatorName;
          this.dtData.auditName = res.data.auditName;
          this.dtData.auditStatus = res.data.auditStatus;
          this.dtData.inventoryId = res.data.inventoryId;
          this.dtData.inventoryName = res.data.inventoryName; // 盘点员姓名
          this.dtData.auditTime =
            res.data.auditTime != null ? res.data.auditTime.split("T")[0] : "";
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
        if (
          item.validityPeriod.value != "" &&
          !isNaN(item.validityPeriod.value)
        ) {
          // 金额
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
      // 将数据导入table
      this.tableData = [];
      if (dt[0]) {
        this.globalid = dt[0].mainId;
      }

      var listArr = [];
      dt.map(item => {
        var list = {
          id: item.id,
          editState: false,
          rowIndex: 0,
          idandmainid: {
            id: item.id,
            value: "",
            mainId: item.mainId,
            isShow: false,
            valid: false
          },
          materialCode: {
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          materialName: {
            value: item.materialName,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          spec: {
            value: item.spec,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          colorName: {
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false,
            id: newGuid()
          },

          unit: {
            // 单位
            value: item.baseUnitName,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          accountNum: {
            // 账存数量
            value: item.accountNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          actualNum: {
            // 实存数量
            value: item.actualNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },

          accountUnitNum: {
            // 基本单位账存数量
            value: item.accountUnitNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          actualUnitNum: {
            // 基本单位实存数量
            value: item.actualUnitNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          // profitNum: 2//盘盈数量
          // profitUnitNum: 2

          // deficitNum: 0//盘亏数量
          // deficitUnitNum: 0
          profitNum: {
            // 盘盈数量
            value: item.profitNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          deficitNum: {
            // 盘亏数量
            value: item.deficitNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          profitUnitNum: {
            // 基本单位盘盈数量
            value: item.profitUnitNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          deficitUnitNum: {
            // 基本单位盘亏数量
            value: item.deficitUnitNum,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          warehouseUnitId: {
            // 基本单位换算率id
            value: item.warehouseUnitId,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          warehouseRate: {
            // 基本单位换算率
            value: item.warehouseRate,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          remark: {
            // 备注
            value: item.remark,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          // remark

          defaultWarehouseName: {
            // 无用
            // 盘点仓库
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          batchNumber: {
            // 无用
            value: item.batchNumber, // 批号
            isShow: false,
            valid: false,
            id: newGuid()
          },

          warehouseUnitName: {
            // 仓库单位
            value: item.warehouseUnitName,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          baseUnitNumber: {
            // 无用
            value: item.baseUnitNumber,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          unitPrice: {
            // 无用
            value: item.unitPrice,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          amount: {
            // 无用
            value: item.amount,
            isShow: false,
            valid: false,
            id: newGuid()
          },
          validityPeriod: {
            // 无用
            value:
              item.validityPeriod != null
                ? new Date(item.validityPeriod.split("T")[0])
                : "",
            isShow: false,
            valid: false,
            id: newGuid()
          }
        };
        this.warehouseData.map(data => {
          // 无用
          if (list.defaultWarehouseName.key == data.id) {
            list.defaultWarehouseName.value = data.warehouseName;
          }
        });

        // this.tableData.unshift(list);
        listArr.push(list);
      });
      this.cloneTable = [...listArr];
      this.setCurrData(this.cloneTable);
      setTimeout(() => {
        // this.setTable(15);
        // this.tableData = [...listArr, ...this.setTable(30)];
        if (listArr.length < this.tableLen) {
          this.tableData = [
            ...listArr,
            ...this.setTable(this.tableLen - listArr.length)
          ];
        } else {
          this.tableData = [...listArr, ...this.setTable(1)];
        }
      }, 0);
    },

    getMainList2(e) {
      // 列表

      request({
        url: "warehouse/api/TWMInventoryMain/GetInventoryOrder?id=" + e.id,
        method: "GET"
      }).then(res => {
        if (res.code === 0) {
          if (res.code === 0) {
            this.setTableData(res.data.childList);
            this.dtData.warehouseId = res.data.warehouseId;
            this.dtData.warehousingOrder = res.data.inventoryOrder;
            this.selectRow = this.cloneObject(res.data);
            this.dtData.whAdminName = res.data.whAdminName;
            this.dtData.principalId = res.data.whAdminId; // 仓管员
            this.dtData.operatorName = res.data.operatorName;
            this.dtData.auditName = res.data.auditName;
            this.dtData.auditStatus = res.data.auditStatus;
            this.dtData.inventoryId = res.data.inventoryId; // 盘点员id
            this.dtData.inventoryName = res.data.inventoryName; // 盘点员姓名
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split("T")[0]
                : "";
          }
        }
      });
    },
    getMainList3(e) {
      // 列表

      request({
        url: "warehouse/api/TWMInventoryMain/GetInventoryOrder?id=" + e,
        method: "GET"
      }).then(res => {
        if (res.code === 0) {
          if (res.code === 0) {
            this.setTableData(res.data.childList);
            this.dtData.warehouseId = res.data.warehouseId;
            this.dtData.warehousingOrder = res.data.inventoryOrder;
            this.selectRow = this.cloneObject(res.data);
            this.dtData.whAdminName = res.data.whAdminName;
            this.dtData.principalId = res.data.whAdminId; // 仓管员
            this.dtData.operatorName = res.data.operatorName;
            this.dtData.auditName = res.data.auditName;
            this.dtData.auditStatus = res.data.auditStatus;
            this.dtData.inventoryId = res.data.inventoryId; // 盘点员id
            this.dtData.inventoryName = res.data.inventoryName; // 盘点员姓名
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split("T")[0]
                : "";
          }
        }
      });
    },
    doOtherWhAudit(state) {
      // 审核
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

      if (state == 1) {
        var tip = "确定审核未通过吗？";
      } else if (state == 2) {
        var tip = "确定审核通过吗？";
      }

      this.$confirm(tip, "确认信息", {
        type: "warning"
      })
        .then(() => {
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
                this.getMainList3(this.globalid);
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
    },
    doOtherWhCancelAudit() {
      // 撤销审核
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
      // 导出
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
      // 导出
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
#Inventorycheck /deep/ {
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
  .el-table {
    overflow: visible !important;
  }
  // .el-select .el-input.is-disabled .el-input__inner,
  // .el-input.is-disabled .el-input__icon {
  //   cursor: default;
  //   color: #606266;
  //   background: #fff;
  // }
  // .disableType i {
  //   border-top: 1px solid rgb(220, 223, 230);
  //   border-bottom: 1px solid rgb(220, 223, 230);
  //   height: 32px;
  // }
  .el-select .el-input.is-disabled .el-input__inner {
    cursor: default;
    color: #606266;
    background: #fff;
  }
  .el-input.is-disabled .el-input__icon {
    cursor: default;
  }
}
@import "@/styles/receipts.scss";
</style>
