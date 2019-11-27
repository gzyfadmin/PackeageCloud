<template>
  <el-container
    id="Salesorders"
    v-on:click.native="listenClick"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    element-loading-text="正在加载"
  >
    <div class="btnHeader" id="btnHeader">
      <h1>销售单</h1>
    </div>
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
          <el-form-item class="formItem" label="客户名称：" prop="customerName" label-width="86px">
            <div class="falseIptop">{{dtData.customerName?dtData.customerName:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="业务员：" prop="customerName" label-width="86px">
            <div class="falseIptop">{{dtData.salesmanName?dtData.salesmanName:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="编号：" prop="orderNo" label-width="86px">
            <div class="falseIptop">{{dtData.orderNo?dtData.orderNo:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="订单类型：" prop="warehousingOrder" label-width="86px">
            <div class="falseIptop">{{dtData.orderTypeName?dtData.orderTypeName:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="结算方式：" prop="warehousingOrder" label-width="86px">
            <div class="falseIptop">{{dtData.settementTypeName?dtData.settementTypeName:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="币种：" label-width="86px">
            <div class="falseIptop">人民币</div>
          </el-form-item>

          <el-form-item class="formItem" label="日期：" prop="warehousingOrder" label-width="86px">
            <div class="falseIptop">{{dtData.orderDate?dtData.orderDate:"&nbsp;"}}</div>
          </el-form-item>
          <!-- warehousingDateForMat -->

          <el-form-item class="formItem" label="审核状态：" label-width="86px">
            <div class="falseIptop">
              <span v-if="dtData.auditStatus==-1">&nbsp;</span>
              <span v-if="dtData.auditStatus==0">待审核</span>
              <span v-if="dtData.auditStatus==1">未通过</span>
              <span v-if="dtData.auditStatus==2">通过</span>
              <span v-if="dtData.auditStatus==3">撤销审核</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="收货地址：" prop="warehousingOrder" label-width="86px">
            <!-- <el-popover
              class="item"
              trigger="hover"
              placement="top-start"
              :content="dtData.receiptAddress"
            >
              <div class="ellipsis falseIptop" slot="reference">{{dtData.receiptAddress?dtData.receiptAddress:"&nbsp;"}}</div>
            </el-popover>-->
            <div
              class="headerIp ellipsis"
              style="width:510px;"
              :title="dtData.receiptAddress"
            >{{dtData.receiptAddress}}</div>
          </el-form-item>
          <!-- </div> -->
        </el-form>
      </div>
    </el-header>
    <el-main>
      <!-- //左边table选择框 -->

      <div class="findBox" :style="popoverStyle" v-show="popoverState" v-on:click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="dicCode" label="包型代码" width="180" />
          <el-table-column prop="dicValue" label="包型名称" />
          <el-table-column prop="mMaterialFile.spec" label="规格型号" />
          <el-table-column prop="mMaterialFile.colorName" label="颜色" />
          <el-table-column prop="mMaterialFile.baseUnitName" label="基本计量单位" />
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
                <span>
                  {{scope.$index+1}}
                  <!-- <i class="el-icon-setting"></i> -->
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="物料代码" width="100">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
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
                  @input="getMaterielDataIndex(scope.row)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="物料名称" width="100">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
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
                  @input="getMaterielDataIndex(scope.row)"
                  :id="scope.row.id+'___materialName'"
                  size="mini"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <!-- @change="WarehouseChange(scope.row)" -->

          <!-- <el-table-column prop="defaultWarehouseName" label="配色方案">
            <template slot="header">
              <span class="tableHeader">
                <span class="start"></span>
                <span>配色方案</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.defaultWarehouseName.valid}">
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
              </div>
            </template>
          </el-table-column>-->

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

          <el-table-column prop="spec" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.spec.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="actualNum" label="数量" width="120">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>数量</span>
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

           <!-- 【单位】该单位的取值来源是包型档案中配置的库存计量单位。 -->
          <el-table-column prop="salesUnitName" width="80">
            <template slot="header">
              <span class="tableHeader">
                <span>单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                v-if="scope.row.salesUnitName.value"
              >{{scope.row.salesUnitName.value}}</div>
              <div
                class="tableTd"
                v-if="!scope.row.salesUnitName.value"
              >{{scope.row.baseUnitName.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.unitPrice.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.unitPrice.isShow"
                >{{scope.row.unitPrice.value}}</div>
                <el-input
                  v-show="scope.row.unitPrice.isShow"
                  v-model="scope.row.unitPrice.value"
                  :id="scope.row.id+'___unitPrice'"
                  size="mini"
                  @input="unitPriceBlur(scope.row,scope.row.unitPrice.value)"
                  @blur="unitPriceBlur2(scope.row)"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="Totalamount" label="金额" width="120">
            <template slot="header">
              <span class="tableHeader">
                <span>金额</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.Totalamount.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.Totalamount.isShow"
                >{{scope.row.Totalamount.value}}</div>
                <el-input
                  v-show="scope.row.Totalamount.isShow"
                  v-model="scope.row.Totalamount.value"
                  :id="scope.row.id+'___Totalamount'"
                  size="mini"
                  @input="amountBlur(scope.row,scope.row.Totalamount.value)"
                  @blur="amountBlur2(scope.row)"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="goodsCode" label="客户商品代码" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>客户商品代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.goodsCode.valid}">
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
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="goodsName" label="客户商品名称" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>客户商品名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.goodsName.valid}">
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
              </div>
            </template>
          </el-table-column>

           <el-table-column prop="baseUnitName" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.baseUnitName.value}}</div>
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
                class="tableTd"
                v-if="scope.row.actualUnitNum.value>0"
              >{{scope.row.actualUnitNum.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="validityPeriod" label="交货日期" width="110">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>交货日期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.validityPeriod.valid}">
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
              </div>
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
    <div id="elfooter">
      <el-form
        label-width="90px"
        class="FormInput"
        :rules="dtDataRules2"
        ref="dtData2"
        :model="dtData"
        inline
      >
        <!-- style="width:1368px" -->
        <el-form-item label="联系人：" v-if="dtData.auditStatus!=2" prop="contactName">
          <!-- <el-input  v-model="dtData.contactName"></el-input> -->
          <div class="falseIp">{{dtData.contactName}}</div>
        </el-form-item>

        <el-form-item label="联系人：">
          <div class="falseIp">{{ dtData.contactName }}</div>
        </el-form-item>

        <el-form-item label="联系人电话：" v-if="dtData.auditStatus!=2" prop="contactNumber">
          <!-- <el-input  v-model="dtData.contactNumber"></el-input> -->
          <div class="falseIp">{{dtData.contactNumber}}</div>
        </el-form-item>

        <el-form-item label="联系人电话：">
          <div class="falseIp">{{ dtData.contactNumber }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item label="制单人：" v-if="addControl" prop="operatorId">
          <div class="falseIp">{{ dtData.operatorName }}</div>
        </el-form-item>
        <el-form-item label="制单人：" v-if="!addControl">
          <div class="falseIp">{{ dtData.operatorName }}</div>
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
import Storage from "@/utils/storage";
import BigNumber from 'bignumber.js'

export default {
  name: "HLDYCY",
  data() {
    return {
      warehouseData: [],
      connectionData: [],
      orderTypeoption: [],
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
        orderNo: "",
        remark: "",
        auditId: null,
        auditName: null,
        auditStatus: 0,
        auditTime: "",
        contactName: "",
        contactNumber: "",
        currency: "",
        customerId: null,
        customerName: null,
        id: null,
        operatorId: null,
        operatorName: "",
        orderDate: "",
        orderNo: "",
        orderTypeId: null,
        orderTypeName: null,
        receiptAddress: "",
        salesmanId: null,
        salesmanName: null,
        settementTypeName: null,
        settlementTypeId: null,
        transferStatus: false
      },
      dtDataRules: {
        customerId: [
          { required: true, message: "请选择客户", trigger: "change" }
        ],
        salesmanId: [
          { required: true, message: "请选择业务员", trigger: "change" }
        ],
        orderTypeId: [
          { required: true, message: "请选择订单类型", trigger: "change" }
        ],
        orderDate: [
          { required: true, message: "请选择日期", trigger: "change" }
        ],
        receiptAddress: [
          { required: false, message: "输入正确地址", trigger: "blur" },
          {
            required: false,
            min: 0,
            max: 50,
            message: "长度在50个字符以下",
            trigger: "blur"
          }
        ]
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
      materielData: [], //包型信息
      TabArr: [
        "materialCode",
        "materialName",
        "goodsCode",
        "goodsName",
        "actualNum",
        "unitPrice",
        "Totalamount",
        "validityPeriod",
        "remark"
      ],
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          warehouseData: [],
          packageId: {
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
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

          baseUnitName: {
            value: "",
            isShow: false,
            valid: false
          },

          accountNum: {
            //数量
            value: "",
            isShow: false,
            valid: false
          },
          actualNum: {
            //数量
            value: "",
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            //基本单位数量
            value: "",
            isShow: false,
            valid: false
          },

          unitPrice: {
            //单价
            value: "",
            isShow: false,
            valid: false
          },
          Totalamount: {
            //金额
            value: "",
            isShow: false,
            valid: false
          },
          profitUnitNum: {
            //基本单位单价
            value: "",
            isShow: false,
            valid: false
          },
          deficitUnitNum: {
            //基本单位金额
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
          salesRate: {
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

          salesUnitName: {
            value: "",
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          goodsCode: {
            value: "",
            isShow: false,
            valid: false
          },
          goodsName: {
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
      OtherData: {},
      realNameOptions: [], //收货员列表
      addControl: true, //是否显示新增按钮
      mountedState:false
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

  deactivated() {
    // window.removeEventListener("keydown", this.onKeydown);
  },

  beforeDestroy() {
    // window.removeEventListener("keydown", this.onKeydown);
  },

  // async activated() {
  //   this.setStyle(); //设置样式
  //   this.getMaterielData(); //包型数据
  //   this.getUserCompany(); //收货员列表

  //   this.getorderTypeoption(); //收货员列表

  //   this.getsettlementTypeoption(); //收货员列表
  //   this.getCustomer();

  //   // window.addEventListener("keydown", this.onKeydown);
  //   // this.btnAip = getBtnCtr(
  //   //   this.$route.path,
  //   //   this.$store.getters.userPermission
  //   // );
  //   // console.log(this.$route.params.type)
  //   if (this.$route.params.type) {
  //     this.OtherData = this.$route.params;
  //     this.fullscreenLoading = true;
  //     // this.OnBtnSaveSubmit(this.OtherData.item);
  //     this.OnBtnSaveSubmit(this.OtherData.item);
  //   }
  //   // if (this.$route.params.type == 3) {
  //   //   this.resetData();
  //   // }
  // },
  mounted() {
    this.runing();
  },
  activated() {
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
      this.getMaterielData(); //包型数据
      this.getUserCompany(); //收货员列表

      this.getorderTypeoption(); //收货员列表

      this.getsettlementTypeoption(); //收货员列表
      this.getCustomer();

      if (this.$route.query.id) {
        // this.OtherData = this.$route.params;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        // this.fullscreenLoading = true;
        this.OnBtnSaveSubmit(this.OtherData);
      }

      this.mountedState = false;
    },
    WarehouseChange(item) {
      // this.computeAccount(this.doItem);
      // 仓库选择
      this.warehouseData.map(data => {
        if (item.defaultWarehouseName.key == data.id) {
          item.defaultWarehouseName.value = data.warehouseName;
        }
      });

      // this.tableData[i].defaultWarehouseName.value
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
          url: "/manufacturing/api/TMMColorSolutionMain",
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
    onKeydown(event) {
      //按下Tab键
      if (event.keyCode !== 9) return;
      var data = this.findCheck(event);
      this.setCheck(data);
    },
    findCheck(event) {
      //寻找Tab下一个元素
      var data = {};
      for (var h = 0; h < this.tableData.length; h++) {
        for (var i in this.tableData[h]) {
          if (typeof this.tableData[h][i] == "object") {
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
      //设置table切换
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
      //重置所有切换了的Input
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
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
    resetData() {
      //初始化数据

      // this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      this.tableData = this.setTable(0);
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];
      (this.dtData.auditId = null),
        (this.dtData.auditName = null),
        (this.dtData.auditStatus = 0),
        (this.dtData.auditTime = ""),
        (this.dtData.contactName = ""),
        (this.dtData.contactNumber = ""),
        (this.dtData.currency = ""),
        (this.dtData.customerId = null),
        (this.dtData.customerName = null),
        (this.dtData.id = null),
        (this.dtData.operatorId = null),
        (this.dtData.orderDate = ""),
        (this.dtData.orderTypeId = null),
        (this.dtData.orderTypeName = null),
        (this.dtData.receiptAddress = ""),
        (this.dtData.salesmanId = null),
        (this.dtData.salesmanName = null),
        (this.dtData.settementTypeName = null),
        (this.dtData.settlementTypeId = null),
        (this.dtData.transferStatus = false);
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
        url: "/sales/api/TSSMSalesOrderMainNoPackage/GetOrderNo",
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.orderNo = "";
        } else {
          Storage.LStorage.set("Salescode", res);
          this.dtData.orderNo = res;
        }
      });
    },

    async addPutInStorage(state) {
      //新增
      if (state == 1 || state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该销售订单是通过状态，无法编辑");
          return;
        }
        var flag = true;
        var checkSelect = await this.checkSelect(this.tableData);
        this.$refs.dtData.validate(valid => {
          //验证
          if (!valid) {
          } else {
            flag = true;
            this.$refs.dtData2.validate(valid => {
              //验证
              if (!valid) {
              } else {
                flag = true;

                var childList = [];
                childList = [];
                this.remarkRel = true;
                this.actualRel = true;
                checkSelect.map(item => {
                  //验证数据
                  item.materialCode.valid = false;
                  item.materialName.valid = false;
                  item.actualNum.valid = false;
                  item.validityPeriod.valid = false;
                  item.remark.valid = false;

                  if (state == 1) {
                    var param = {
                      id: 0,
                      mainId: 0,
                      packageId: item.packageId.value,
                      materialId: item.materialCode.key,
                      goodsCode: item.goodsCode.value.toString(),
                      goodsName: item.goodsName.value.toString(),
                      unitPrice: item.unitPrice.value
                        ? item.unitPrice.value
                        : 0,
                      salesNum: item.actualNum.value ? item.actualNum.value : 0,
                      colorSolutionId: item.defaultWarehouseName.key
                        ? item.defaultWarehouseName.key
                        : "",
                      salesAmount: item.Totalamount.value
                        ? item.Totalamount.value
                        : 0,
                      deliveryPeriod: formatDate(item.validityPeriod.value),
                      remark: item.remark.value,
                      _LogName: "销售订单其中一条信息"
                    };
                  } else if (state == 2) {
                    var param = {
                      id: item.idandmainid.id ? item.idandmainid.id : 0,
                      colorSolutionId: item.defaultWarehouseName.key
                        ? item.defaultWarehouseName.key
                        : "",

                      mainId: this.globalid,
                      packageId: item.packageId.value,
                      materialId: item.materialCode.key,
                      goodsCode: item.goodsCode.value.toString(),
                      goodsName: item.goodsName.value.toString(),
                      unitPrice: item.unitPrice.value
                        ? item.unitPrice.value
                        : 0,
                      salesNum: item.actualNum.value ? item.actualNum.value : 0,
                      salesAmount: item.Totalamount.value
                        ? item.Totalamount.value
                        : 0,
                      deliveryPeriod: formatDate(item.validityPeriod.value),
                      remark: item.remark.value,
                      _LogName: "销售订单其中一条信息"
                    };
                  }
                  if (
                    item.remark.value.length > 50 ||
                    item.goodsName.value.length > 20 ||
                    item.goodsCode.value.length > 20
                  ) {
                    this.remarkRel = false;
                    //  this.$message.error("备注数值长度超标");
                  }

                  if (item.actualNum.value > 99999999999) {
                    this.actualRel = false;
                  }

                  if (param.materialId) {
                    if (!item.materialCode.key) {
                      //验证物料代码
                      item.materialCode.valid = true;
                      item.materialName.valid = true;
                    }
                    if (
                      !item.actualNum.value ||
                      item.actualNum.value > 99999999999
                    ) {
                      item.actualNum.valid = true;
                    }
                    if (item.remark.value.length > 50) {
                      item.remark.valid = true;
                    }
                    if (item.goodsName.value.length > 50) {
                      item.goodsName.valid = true;
                    }
                    if (item.goodsCode.value.length > 50) {
                      item.goodsCode.valid = true;
                    }
                    if (!item.validityPeriod.value) {
                      item.validityPeriod.valid = true;
                    }

                    childList.push(param);
                    if (!param.deliveryPeriod || !param.salesNum) {
                      flag = false;
                    }
                  }
                });
                if (state == 1) {
                  //新增
                  var para = {
                    postData: {
                      id: 0,
                      salesmanId: this.dtData.salesmanId,
                      orderNo: this.dtData.orderNo,
                      orderTypeId: this.dtData.orderTypeId,
                      settlementTypeId: this.dtData.settlementTypeId,
                      receiptAddress: this.dtData.receiptAddress,
                      currency: this.dtData.currency,
                      orderDate: formatDate(this.dtData.orderDate),
                      operatorId: 0,
                      customerId: this.dtData.customerId,
                      contactName: this.dtData.contactName,
                      contactNumber: this.dtData.contactNumber,
                      childList: childList
                    }
                  };
                } else if (state == 2) {
                  //编辑
                  var para = {
                    postData: {
                      id: this.globalid,
                      salesmanId: this.dtData.salesmanId,
                      orderNo: this.dtData.orderNo,
                      orderTypeId: this.dtData.orderTypeId,
                      settlementTypeId: this.dtData.settlementTypeId,
                      receiptAddress: this.dtData.receiptAddress,
                      currency: this.dtData.currency,
                      orderDate: formatDate(this.dtData.orderDate),
                      operatorId: 0,
                      customerId: this.dtData.customerId,
                      contactName: this.dtData.contactName,
                      contactNumber: this.dtData.contactNumber,
                      childList: childList
                    }
                  };
                }
                if (!this.actualRel) {
                  //校验数量
                  this.$message.error("数量过大必须小于999999999件");
                } else if (!this.remarkRel) {
                  //校验备注
                  this.$message.error("数值长度超标");
                } else {
                  if (state == 1) {
                    if (
                      !flag ||
                      !para.postData.salesmanId ||
                      !para.postData.orderTypeId ||
                      para.postData.childList.length == 0
                    ) {
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
                    if (
                      !flag ||
                      !para.postData.salesmanId ||
                      !para.postData.orderTypeId ||
                      para.postData.childList.length == 0
                    ) {
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
        //删除
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该销售订单是通过状态，无法删除");
          return;
        }

        if (this.selectRow.id == undefined) {
          this.$message.error("请选择删除的数据");
          return;
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的销售订单`
        };

        var data = RequestObject.CreatePostObject(currData);

        this.$confirm("是否删除销售订单？", "确认信息", {
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
        url: "/sales/api/TSSMSalesOrderMainNoPackage",
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
            Storage.LStorage.remove("Salescode");
            // this.resetData()
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
      //获取包型档案信息
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
      // /basicset/api/TBMPackage
      request({
        // url: "/basicset/api/TBMMaterialFile",
        url: "/basicset/api/TBMPackage",
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

    getorderTypeoption() {
      var rqs = RequestObject.CreateGetObject(false, 0, 0, [
        { column: "typeId", content: 18, condition: 0 }
      ]);

      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "订单类型", condition: 0 }
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
          this.orderTypeoption = res.data;
          //  dicValue
          // id
        }
      });
    },
    getsettlementTypeoption() {
      // var rqs2 = RequestObject.CreateGetObject(false, 0, 0, [
      //   { column: "typeId", content: 19, condition: 0 }
      // ]);

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

    async rowClick(row) {
      var that = this;
      // console.log("row");
      // console.log(row);
      //   <!-- dicCode: "b12"
      // dicValue: "包型12" -->
      this.doItem.materialCode.value = row.dicCode;
      this.doItem.materialCode.key = row.mMaterialFile.id;
      this.doItem.packageId.value = row.id;
      this.doItem.materialName.value = row.dicValue;
      // this.warehouseData = await this.getWarehouseData(1,2)

      var newarritem = [];
      this.warehouseData.map(newitem => {
        // if(newitem.packageCode==item.materialCode&&newitem.packageName==item.materialName){
        if (newitem.packageCode == row.id) {
          var obj = new Object({
            warehouseName: newitem.warehouseName,
            id: newitem.id,
            packageName: newitem.packageName,
            packageCode: newitem.packageCode
          });
          newarritem.push(obj);
        }
      });
      this.doItem.defaultWarehouseName = {
        value: "",
        key: "",
        isShow: false,
        valid: false
      };
      this.doItem.warehouseData = newarritem;
      this.doItem.spec.value = row.mMaterialFile.spec;
      this.doItem.colorName.value = row.mMaterialFile.colorName;
      this.doItem.salesUnitName.value = row.mMaterialFile.salesUnitName;
      // this.doItem.defaultWarehouseName.key = row.defaultWarehouseId;
      this.doItem.baseUnitName.value = row.mMaterialFile.baseUnitName;
      this.doItem.warehouseUnitId.value = row.mMaterialFile.warehouseUnitId;
      this.doItem.salesRate.value = row.mMaterialFile.salesRate;
      this.listenClick();
      this.doItem.actualNum.isShow = true;
      this.$nextTick(() => {
        document.getElementById(this.doItem.id + "___" + "actualNum").focus();
      });
      if (that.nowrow) {
        that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
      }
      // that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
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
              return BigNumber(prev).plus(curr).toNumber();
            } else {
              return prev;
            }
          }, 0);
          // actualNum
          if (
            index == 6 || index == 8||index == 12
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
      //双击显示input,监听input数据变化
      if (this.selectRow.auditStatus === 2) {
        return;
      }
      event.stopPropagation();
      this.nowrow = item;
      this.defaultAll(this.doItem);
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
          this.getMaterielData();
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

    //输入金额时顺便计算
    actualNumBlur(item, input) {
      if (item.warehouseUnitId.value != null && item.salesRate.value != null) {
        item.actualUnitNum.value = parseFloat(
          item.actualNum.value * item.salesRate.value
        )
          ? keepTwoDecimalFull(item.actualNum.value * item.salesRate.value)
          : null;
      } else {
        item.actualUnitNum.value = parseFloat(item.actualNum.value);
      }
      // actualNum
      var r = /^\d+(?=\.{0,1}\d+$|$)/; //正整数
      var flag = r.test(input);
      if (!flag) {
        item.actualNum.value = null;
      } else if (item.actualNum.value == 0) {
        item.actualNum.value = 0;
      }
      var nowactualNum = item.actualNum.value;
      nowactualNum = parseFloat(nowactualNum);

      if (nowactualNum) {
        if (item.Totalamount.value > 0 && !item.unitPrice.value) {
          //当单价为0时，当金额不为0时
          item.unitPrice.value = keepTwoDecimalFull(
            item.Totalamount.value / nowactualNum
          );
        } else if (item.unitPrice.value > 0 || item.Totalamount.value > 0) {
          //单价不为0时
          item.Totalamount.value = keepTwoDecimalFull(
            item.unitPrice.value * nowactualNum
          );
        } else if (!item.unitPrice.value && !item.Totalamount.value) {
          item.Totalamount.value = 0;
          item.unitPrice.value = 0;
        }
      } else {
        item.Totalamount.value = 0;
      }
    },

    unitPriceBlur(item, input) {
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; //正整数
      var flag = r.test(input); //验证
      var doc = input.split(".").length - 1; //验证小数点数

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
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; //正整数
      var flag = r.test(input); //验证
      var doc = input.split(".").length - 1; //验证小数点数
      item.Totalamount.value = item.Totalamount.value.replace(/[^\d|.]/g, "");
      var nowTotalamount = item.Totalamount.value;
      nowTotalamount = parseFloat(nowTotalamount);

      if (nowTotalamount && item.actualNum.value) {
        if (item.Totalamount.value == "" || item.Totalamount.value == null)
          return;
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
      //查询
      //点击添加按钮
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.dtData.orderNo = "";
      this.$refs.InboundOrder.dtData.auditStatus = -1;
      this.$refs.InboundOrder.clickRow = {};
      this.$refs.InboundOrder.getMainList();
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },

    OnBtnSaveSubmit(row) {
      var rqs = RequestObject.CreateGetObject(false, 0, 0, [
        {
          column: "id",
          content: row.id,
          condition: 0
        }
      ]);

      request({
        url: "sales/api/TSSMSalesOrderMainNoPackage/GetMainList",
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
          this.dtData.currency = res.data[0].currency;
          this.dtData.customerId = res.data[0].customerId;
          this.dtData.customerName = res.data[0].customerName;
          this.dtData.operatorId = res.data[0].operatorId;
          this.dtData.operatorName = res.data[0].operatorName;
          this.dtData.orderDate =
            res.data[0].orderDate != null
              ? res.data[0].orderDate.split("T")[0]
              : "";
          // haha
          this.dtData.orderNo = res.data[0].orderNo;
          this.dtData.orderTypeId = res.data[0].orderTypeId;
          this.dtData.orderTypeName = res.data[0].orderTypeName;

          this.dtData.receiptAddress = res.data[0].receiptAddress;
          this.dtData.salesmanId = res.data[0].salesmanId;
          this.dtData.salesmanName = res.data[0].salesmanName;

          this.dtData.settementTypeName = res.data[0].settementTypeName;
          this.dtData.settlementTypeId = res.data[0].settlementTypeId;
          this.dtData.transferStatus = res.data[0].transferStatus;

          request({
            url: "sales/api/TSSMSalesOrderMainNoPackage/GetDetailList",
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
      //设置编辑时候的日记数据
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
      //将数据导入table

      this.tableData = [];
      if (dt[0]) {
        this.globalid = dt[0].mainId;
      }

      // console.log(dt);

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
            id: item.id,
            mainId: item.mainId,
            isShow: false,
            valid: false
          },
          packageId: {
            value: "",
            key: "",
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

          baseUnitName: {
            //基本单位名称
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },

          actualNum: {
            //数量
            value: item.salesNum,
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            //基本单位数量
            value: item.baseUnitNum,
            isShow: false,
            valid: false
          },

          unitPrice: {
            //单价
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          Totalamount: {
            //金额
            value: item.salesAmount,
            isShow: false,
            valid: false
          },

          salesRate: {
            //基本单位换算率
            value: item.salesRate,
            isShow: false,
            valid: false
          },
          remark: {
            //备注
            value: item.remark,
            isShow: false,
            valid: false
          },

          goodsCode: {
            value: item.goodsCode,
            isShow: false,
            valid: false
          },
          goodsName: {
            value: item.goodsName,
            isShow: false,
            valid: false
          },
          validityPeriod: {
            value: item.deliveryPeriod,
            isShow: false,
            valid: false
          },
          salesUnitName: {
            //单位
            value: item.salesUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitId: {
            //基本单位换算率id
            value: item.warehouseUnitId,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            // 出货仓库
            value: item.colorSolutionName,
            key: item.colorSolutionId,
            isShow: false,
            valid: false
          }
        };
        listArr.push(list);
      });

      setTimeout(() => {
        // this.setTable(0);
        this.tableData = [...listArr];
      }, 0);
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 500);
    },

    doOtherWhAudit(state) {
      //审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error("该销售订单已经为通过状态");
        return;
      }
      var data = RequestObject.CreatePostObject({
        id: this.selectRow.id,
        auditStatus: state
      });

      if (state == 1) {
        var tip = "审核是否更改为未通过？";
      } else if (state == 2) {
        var tip = "审核是否更改为通过？";
      }

      this.$confirm(tip, "确认信息", {
        type: "warning"
      })
        .then(() => {
          this.fullscreenLoading = true;
          request({
            url: "/sales/api/TSSMSalesOrderMainNoPackage/SalesOrderAudit",
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
#Salesorders #elfooter .el-input__inner {
  border: 0px;
  border-radius: 0px;
  border-bottom: 1px solid #dcdfe6;
}
#Salesorders .el-select-dropdown__empty {
  display: none !important;
}
</style>


<style lang="scss" scoped>
#Salesorders /deep/ {
  .falseIptop {
    width: 200px;
    overflow: hidden;
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
    margin-right: 20px;
    // width:283px;
  }
  .middleWidth {
    width: 1598px;
  }
  // @media screen and (max-width: 1948px) {
  //   .middleWidth {
  //     width: 1398px;
  //   }
  //   .formItem {
  //     margin-left: 10px;
  //     width: 287px;
  //   }
  // }

  // @media screen and (max-width: 1648px) {
  //   .middleWidth {
  //     width: 1248px;
  //   }
  //   .formItem {
  //     margin-left: 10px;
  //     width: 287px;
  //   }
  // }

  // @media screen and (max-width: 1530px) {
  //   .middleWidth {
  //     width: 1048px;
  //   }
  //   .formItem {
  //     margin-left: 20px;
  //     width: 306px;
  //   }
  // }
  // @media screen and (max-width: 1288px) {
  //   .middleWidth {
  //     width: 848px;
  //   }
  //   .formItem {
  //     margin-left: 40px;
  //     width: 326px;
  //   }
  // }
  .el-select .el-input.is-disabled .el-input__inner {
    cursor: default;
    color: #606266;
    background: #fff;
  }
  .el-input.is-disabled .el-input__icon {
    cursor: default;
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
