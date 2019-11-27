<template>
  <el-container
    v-cloak
    id="Productorders"
    v-on:click.native="listenClick"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    element-loading-text="正在加载"
  >
    <div class="btnHeader" id="btnHeader">
      <h1>生产单</h1>
    </div>
    <el-header id="elheader" class="header" height="auto">
      <div class="middleWidth">
        <el-form
          ref="dtData"
          :model="dtData"
          label-width="104px"
          class="FormInput"
          inline
          label-position="right"
          :rules="dtDataRules"
        >
          <!-- <el-form-item class="formItem" label="源单类型：" label-width="106px">
            <div>销售订单</div>
          </el-form-item>-->

          <el-form-item class="formItem" label="源单类型：">
            <div class="falseIptop">销售订单</div>
          </el-form-item>

          <el-form-item class="formItem" label="源单号：">
            <div class="falseIptop">{{dtData.sourceNo?dtData.sourceNo:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem disableType" label="客户名称：">
            <div class="falseIptop">{{dtData.customerName?dtData.customerName:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem disableType" label="生产类型：">
            <div class="falseIptop">
              <div v-if="dtData.productionType==0">库存生产</div>
              <div v-if="dtData.productionType==1">订单生产</div>
            </div>
          </el-form-item>

          <el-form-item class="formItem disableType" label="订单日期：">
            <div class="falseIptop">{{dtData.orderDate?dtData.orderDate:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem disableType" label="计划开始时间：">
            <div class="falseIptop">{{dtData.beginDate?dtData.beginDate:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem disableType" label="计划开始时间：">
            <div class="falseIptop">{{dtData.endDate?dtData.endDate:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem disableType" label="生产单号：">
            <div class="falseIptop">{{dtData.orderNo?dtData.orderNo:"&nbsp;"}}</div>
          </el-form-item>

          <el-form-item class="formItem" label="状态：">
            <div class="falseIptop">
              <span v-if="dtData.auditStatus==-1"></span>
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

          <el-table-column prop="materialCode" label="包型代码" width="100">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>包型代码</span>
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
                  @focus="chengenum($event,scope.row,'materialCode',1)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                ></el-input>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="包型名称" width="100">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>包型名称</span>
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

          <el-table-column prop="defaultWarehouseName" label="配色方案">
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
              <div class="tableTd">{{scope.row.spec.value}}</div>
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

          <el-table-column prop="actualNum" label="数量" width="100">
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
          <el-table-column prop="produceUnitName" width="80">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                class="tableTd"
                v-if="scope.row.produceUnitName.value"
              >{{scope.row.produceUnitName.value}}</div>
              <div
                class="tableTd"
                v-if="!scope.row.produceUnitName.value"
              >{{scope.row.baseUnitName.value}}</div>
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

          <el-table-column prop="validityPeriod" label="交货日期" width="110">
            <template slot="header">
              <span class="tableHeader">
                <span class="start"></span>
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

          <el-table-column prop="defaultprincipalName" label="负责人">
            <template slot="header">
              <span class="tableHeader">
                <span class="start"></span>
                <span>负责人</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.defaultprincipalName.valid}">
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
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="workshopName" label="生产车间">
            <template slot="header">
              <span class="tableHeader">
                <span class="start"></span>
                <span>生产车间</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.workshopName.valid}">
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
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div id="elfooter">
      <el-form
        label-width="76px"
        class="FormInput"
        :rules="dtDataRules2"
        ref="dtData2"
        :model="dtData"
        inline
      >
        <!-- style="width:1368px" -->
        <el-form-item label="制单人：" v-if="addControl" prop="operatorId">
          <div class="falseIp" v-if="addControl">{{ dtData.operatorName }}</div>
        </el-form-item>
        <el-form-item label="制单人：" v-if="!addControl">
          <div class="falseIp" v-if="!addControl">{{ dtData.operatorName }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item label="审核时间：">
          <div class="falseIp">{{dtData.auditTime}}</div>
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
      </el-form>
    </div>
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit"></InboundOrder>

    <!-- @dialogid="dialogid"  -->
    <el-dialog :visible.sync="showdetialdialog" width="60%">
      <listdialog @salescode="getsalescode" v-if="showdetialdialog" />
    </el-dialog>
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
import InboundOrder from "./components/InboundOrder";
import listdialog from "./components/dialog";
import Pagination from "@/components/Pagination";
// import { promises } from 'dns';
import { setTimeout } from "timers";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";
import BigNumber from "bignumber.js";

export default {
  name: "HLDYG",

  components: {
    Pagination,
    InboundOrder,
    listdialog
  },
  data() {
    return {
      showdetialdialog: false,
      warehouseData: [],
      connectionData: [],
      orderTypeoption: [
        //  dicValue:"订单生产",
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
        sourceNoid: null,
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
        orderDate: "",
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
      materielData: [], //包型信息
      TabArr: [
        "materialCode",
        "materialName",
        "goodsCode",
        "goodsName",
        "actualNum",
        // "unitPrice",
        // "Totalamount",
        "validityPeriod",
        // "remark",
        "principal"
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

          warehouseUnitId: {
            //基本单位换算率id
            value: "",
            isShow: false,
            valid: false
          },
          produceRate: {
            //基本单位换算率
            value: "",
            isShow: false,
            valid: false
          },

          batchNumber: {
            value: "",
            isShow: false,
            valid: false
          },

          produceUnitName: {
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
          defaultprincipalName: {
            //数量
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          workshopName: {
            //数量
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
      workshopOption: [],
      addControl: true, //是否显示新增按钮
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
      if (this.$route.query.id) {
        this.OtherData = this.$route.params;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.fullscreenLoading = true;
        this.OnBtnSaveSubmit(this.OtherData);
      }

      this.mountedState = false;
    },
    sourceclear() {
      this.dtData.sourceNo = null;
      this.dtData.sourceNoid = null;
    },
    getsalescode(e) {
      //hihi
      this.dtData.sourceNo = e[0];
      this.dtData.sourceNoid = e[1];
      this.showdetialdialog = false;
      this.dtData.productionType = 1;
    },
    WarehouseChange(item) {
      // this.computeAccount(this.doItem);
      // 仓库选择
      this.warehouseData.map(data => {
        if (item.defaultWarehouseName.key == data.id) {
          item.defaultWarehouseName.value = data.warehouseName;
        }
      });
    },

    principalChange(item) {
      // this.computeAccount(this.doItem);
      // 仓库选择
      this.realNameOptions.map(data => {
        if (item.defaultprincipalName.key == data.id) {
          item.defaultprincipalName.value = data.realName;
        }
      });

      // this.tableData[i].defaultprincipalName.value
    },

    workshopChange(item) {
      // this.computeAccount(this.doItem);
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
        (this.dtData.customerId = null),
        (this.dtData.customerName = null),
        (this.dtData.id = null),
        (this.dtData.operatorId = null),
        (this.dtData.orderDate = ""),
        (this.dtData.productionType = null),
        (this.dtData.orderTypeName = null),
        (this.dtData.sourceNo = ""),
        (this.dtData.salesmanName = null),
        (this.dtData.settementTypeName = null),
        (this.dtData.beginDate = ""),
        (this.dtData.endDate = ""),
        (this.dtData.transferStatus = false);
      this.dtData.sourceNoid = false;
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
      //新增
      if (state == 1 || state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该生产订单是通过状态，无法编辑");
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
                  // item.validityPeriod.valid = false;
                  if (state == 1) {
                    var param = {
                      id: 0,
                      mainId: 0,
                      packageId: item.packageId.value,
                      materialId: item.materialCode.key,
                      goodsCode: item.goodsCode.value.toString(),
                      goodsName: item.goodsName.value.toString(),
                      principalId: item.defaultprincipalName.key,
                      workshopId: item.workshopName.key,
                      productionNum: parseInt(item.actualNum.value)
                        ? parseInt(item.actualNum.value)
                        : 0,

                      colorSolutionId: parseInt(item.defaultWarehouseName.key)
                        ? parseInt(item.defaultWarehouseName.key)
                        : "",

                      deliveryPeriod: formatDate(item.validityPeriod.value),

                      _LogName: "生产订单其中一条信息"
                    };
                  } else if (state == 2) {
                    var param = {
                      id: item.idandmainid.id ? item.idandmainid.id : 0,
                      colorSolutionId: parseInt(item.defaultWarehouseName.key)
                        ? parseInt(item.defaultWarehouseName.key)
                        : "",
                      packageId: item.packageId.value,

                      principalId: item.defaultprincipalName.key,
                      workshopId: item.workshopName.key,
                      mainId: this.globalid,
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
                  if (
                    // item.remark.value.length > 50 ||
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
                    // if (item.remark.value.length > 50) {
                    //   item.remark.valid = true;
                    // }
                    if (item.goodsName.value.length > 50) {
                      item.goodsName.valid = true;
                    }
                    if (item.goodsCode.value.length > 50) {
                      item.goodsCode.valid = true;
                    }
                    // if (!item.validityPeriod.value) {
                    //   item.validityPeriod.valid = true;
                    // }

                    childList.push(param);
                    if (!param.deliveryPeriod || !param.productionNum) {
                      flag = false;
                    }
                  }
                });

                // haha

                if (state == 1) {
                  //新增

                  var para = {
                    postData: {
                      id: 0,
                      productionType: this.dtData.productionType,
                      productionNo: this.dtData.orderNo,
                      sourceId: this.dtData.sourceNoid
                        ? this.dtData.sourceNoid
                        : null,
                      customerId: this.dtData.customerId,
                      orderDate: formatDate(this.dtData.orderDate),
                      beginDate: formatDate(this.dtData.beginDate),
                      endDate: formatDate(this.dtData.endDate),

                      // operatorId: 0,

                      // contactName: this.dtData.contactName,
                      // contactNumber: this.dtData.contactNumber,
                      childList: childList,
                      _LogName: ""
                    }
                  };
                } else if (state == 2) {
                  //编辑
                  var para = {
                    postData: {
                      id: this.globalid,
                      orderNo: this.dtData.orderNo,
                      productionType: this.dtData.productionType,
                      productionNo: this.dtData.orderNo,
                      sourceId: this.dtData.sourceNoid
                        ? this.dtData.sourceNoid
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
                var T1 = new Date(this.dtData.beginDate);
                var T2 = new Date(this.dtData.endDate);
                if (T2 > T1) {
                  var beginandend = true;
                } else {
                  var beginandend = false;
                }
                if (!this.actualRel) {
                  //校验数量
                  this.$message.error("数量过大必须小于999999999件");
                } else if (!this.remarkRel) {
                  //校验备注
                  this.$message.error("数值长度超标");
                } else if (!beginandend) {
                  this.$message.error("结束时间必须大于或等于开始时间");
                } else {
                  if (state == 1) {
                    if (para.postData.childList.length == 0) {
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
                    if (para.postData.childList.length == 0) {
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

      // workshopOption

      //改到这

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

    rowClick(row) {
      var that = this;

      this.doItem.materialCode.value = row.dicCode;
      this.doItem.materialCode.key = row.mMaterialFile.id;
      this.doItem.materialName.value = row.dicValue;
      this.doItem.packageId.value = row.id;
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
      this.doItem.warehouseData = newarritem;
      this.doItem.spec.value = row.mMaterialFile.spec;
      this.daItem.colorName.value = row.mMaterialFile.colorName;
      this.doItem.produceUnitName.value = row.mMaterialFile.produceUnitName;
      this.doItem.baseUnitName.value = row.mMaterialFile.baseUnitName;
      this.doItem.warehouseUnitId.value = row.mMaterialFile.warehouseUnitId;
      this.doItem.produceRate.value = row.mMaterialFile.produceRate;

      this.listenClick();
      this.doItem.actualNum.isShow = true;
      this.$nextTick(() => {
        document.getElementById(this.doItem.id + "___" + "actualNum").focus();
      });

      if (that.nowrow) {
        that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
      }
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
      if (
        item.warehouseUnitId.value != null &&
        item.produceRate.value != null
      ) {
        item.actualUnitNum.value = parseFloat(
          item.actualNum.value * item.produceRate.value
        )
          ? keepTwoDecimalFull(item.actualNum.value * item.produceRate.value)
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
    },

    unitPriceBlur(item, input) {
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; //正整数
      var flag = r.test(input); //验证
      var doc = input.split(".").length - 1; //验证小数点数

      item.unitPrice.value = item.unitPrice.value.replace(/[^\d|.]/g, "");
      // if (!flag && doc != 1) {
      //   //验证当小数点非一个时且不是正整数
      //   item.unitPrice.value = null;
      // } else if (doc == 1 && !flag) {
      //   //去掉除整数和小数点以外的字符
      //   item.unitPrice.value = item.unitPrice.value.replace(/[^\d|.]/g, "");
      // }
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
          this.dtData.salesmanName = res.data[0].salesmanName;

          this.dtData.settementTypeName = res.data[0].settementTypeName;
          this.dtData.transferStatus = res.data[0].transferStatus;

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

          // packageCode: "b1"
          // packageId: 319
          materialCode: {
            value: item.packageCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            value: item.packageName,
            isShow: false,
            valid: false
          },
          colorName: {
            value: item.colorName,
            isShow: false,
            valid: false
          },

          spec: {
            value: item.spec,
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
            value: item.productionNum,
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            //基本单位数量
            value: item.baseUnitNum,
            isShow: false,
            valid: false
          },

          produceRate: {
            //基本单位换算率
            value: item.produceRate,
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
          produceUnitName: {
            //单位
            value: item.produceUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitId: {
            //基本单位换算率id
            value: item.produceUnitId,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            // 出货仓库
            value: item.colorSolutionName,
            key: item.colorSolutionId,
            isShow: false,
            valid: false
          },
          // principalId: null
          // principalName: null
          defaultprincipalName: {
            //数量
            value: item.principalName,
            key: item.principalId,
            isShow: false,
            valid: false
          },
          // workshopId: null
          // workshopName: null
          workshopName: {
            //数量
            value: item.workshopName,
            key: item.workshopId,
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
        this.$message.error("该生产订单已经为通过状态");
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
#Productorders #elfooter .el-input__inner {
  border: 0px;
  border-radius: 0px;
  border-bottom: 1px solid #dcdfe6;
}
#Productorders .el-select-dropdown__empty {
  display: none !important;
}
</style>

<style lang="scss" scoped>
#Productorders /deep/ {
  [v-cloak] {
    display: none;
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
  }
  .middleWidth {
    width: 1598px;
  }
  // @media screen and (max-width: 1948px) {
  //   .middleWidth {
  //     width: 1498px;
  //   }
  //   .formItem {
  //     margin-left: 10px;
  //     width: 337px;
  //   }
  // }

  // @media screen and (max-width: 1788px) {
  //   .middleWidth {
  //     width: 1348px;
  //   }
  //   .formItem {
  //     margin-left: 10px;
  //     width: 337px;
  //   }
  // }

  // @media screen and (max-width: 1740px) {
  //   .middleWidth {
  //     width: 1248px;
  //   }
  //   .formItem {
  //     margin-left: 20px;
  //     width: 336px;
  //   }
  // }
  // @media screen and (max-width: 1388px) {
  //   .middleWidth {
  //     width: 948px;
  //   }
  //   .formItem {
  //     margin-left: 40px;
  //     width: 336px;
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
  .falseIptop {
    width: 200px;
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
