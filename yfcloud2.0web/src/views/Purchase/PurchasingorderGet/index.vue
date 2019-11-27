<template>
  <el-container
    id="Purchasingorder"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    element-loading-text="正在加载"
    @click.native="listenClick"
  >
    <div id="btnHeader" class="btnHeader">
      <h1>采购订单</h1>
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
          <!-- <el-form-item class="formItem" label="供应商：" label-width="86px">
            <div class="falseIptop">{{dtData.supplierName?dtData.supplierName:"&nbsp;"}}</div>
          </el-form-item>-->

          <el-form-item class="formItem" label="采购员：" label-width="86px">
            <div class="falseIptop">{{ dtData.buyerName?dtData.buyerName:"&nbsp;" }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="采购方式：" label-width="86px">
            <div class="falseIptop">{{ dtData.orderTypeName?dtData.orderTypeName:"&nbsp;" }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="日期：" label-width="86px">
            <div class="falseIptop">{{ warehousingDateForMat?warehousingDateForMat:"&nbsp;" }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="结算方式：" label-width="86px">
            <div
              class="falseIptop"
            >{{ dtData.settlementTypeName?dtData.settlementTypeName:"&nbsp;" }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="币种：" label-width="86px">
            <div class="falseIptop">人民币</div>
          </el-form-item>

          <el-form-item class="formItem" label="编号：" label-width="86px">
            <div class="falseIptop">{{ dtData.orderNo?dtData.orderNo:"&nbsp;" }}</div>
          </el-form-item>
          <el-form-item class="formItem" label="审核状态：" label-width="86px">
            <div class="falseIptop">
              <span v-if="dtData.auditStatus==-1">&nbsp;</span>
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

      <div v-show="popoverState" class="findBox" :style="popoverStyle" @click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="materialCode" label="物料代码" width="180" />
          <el-table-column prop="materialName" label="物料名称" />
          <el-table-column prop="spec" label="规格型号" />
          <el-table-column prop="colorName" label="颜色" />
          <el-table-column prop="baseUnitName" label="基本计量单位" />
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
          <li @click="doMenuAdd">增加行</li>
          <li @click="doMenuDel">删除行</li>
        </ul>
      </div>
      <div @click.stop="1==1">
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
                  {{ scope.$index+1 }}
                  <!-- <i class="el-icon-setting"></i> -->
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="物料代码">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.materialCode.valid}">
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
                  @focus="chengenum($event,scope.row,'materialCode',1)"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                />
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialName" label="物料名称">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>物料名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.materialName.valid}">
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
              <div :class="{validStyle:scope.row.colorName.valid}">
                <div class="tableTd">{{ scope.row.colorName.value }}</div>
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
              <div class="tableTd">{{ scope.row.spec.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitName">
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

          <el-table-column prop="actualNum" label="数量">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.actualNum.valid}">
                <div
                  v-show="!scope.row.actualNum.isShow"
                  class="tableTd"
                >{{ scope.row.actualNum.value }}</div>
                <el-input
                  v-show="scope.row.actualNum.isShow"
                  :id="scope.row.id+'___actualNum'"
                  v-model="scope.row.actualNum.value"
                  size="mini"
                  @input="actualNumBlur(scope.row,scope.row.actualNum.value)"
                />
              </div>
            </template>
          </el-table-column>

          <!-- 【单位】该单位的取值来源是物料档案中配置的库存计量单位。 -->
          <el-table-column prop="purchaseUnitName">
            <template slot="header">
              <span class="tableHeader">
                <span>单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div
                v-if="scope.row.purchaseUnitName.value"
                class="tableTd"
              >{{ scope.row.purchaseUnitName.value }}</div>
              <div
                v-if="!scope.row.purchaseUnitName.value"
                class="tableTd"
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
              <div :class="{validStyle:scope.row.unitPrice.valid}">
                <div
                  v-show="!scope.row.unitPrice.isShow"
                  class="tableTd"
                >{{ scope.row.unitPrice.value }}</div>
                <el-input
                  v-show="scope.row.unitPrice.isShow"
                  :id="scope.row.id+'___unitPrice'"
                  v-model="scope.row.unitPrice.value"
                  size="mini"
                  @input="unitPriceBlur(scope.row,scope.row.unitPrice.value)"
                />
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="Totalamount" label="金额">
            <template slot="header">
              <span class="tableHeader">
                <span>金额</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.Totalamount.valid}">
                <div
                  v-show="!scope.row.Totalamount.isShow"
                  class="tableTd"
                >{{ scope.row.Totalamount.value }}</div>
                <el-input
                  v-show="scope.row.Totalamount.isShow"
                  :id="scope.row.id+'___Totalamount'"
                  v-model="scope.row.Totalamount.value"
                  size="mini"
                  @input="amountBlur(scope.row,scope.row.Totalamount.value)"
                />
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="validityPeriod" label="交货日期" width="90px">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>交货日期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.validityPeriod.valid}">
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
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="supplierId" label="供应商" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>供应商</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.supplierId.value }}</div>
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
              <div :class="{validStyle:scope.row.remark.valid}">
                <div v-show="!scope.row.remark.isShow" class="tableTd">
                  <el-popover
                    v-if="scope.row.remark.value.length>=15"
                    class="item"
                    trigger="hover"
                    placement="top-start"
                    :content="scope.row.remark.value"
                  >
                    <div slot="reference" class="ellipsis">{{ scope.row.remark.value }}</div>
                  </el-popover>
                  <div
                    v-if="scope.row.remark.value.length<15"
                    class="ellipsis"
                  >{{ scope.row.remark.value }}</div>
                </div>
                <el-input
                  v-show="scope.row.remark.isShow"
                  :id="scope.row.id+'___remark'"
                  v-model="scope.row.remark.value"
                  size="mini"
                />
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div id="elfooter">
      <el-form
        ref="dtData2"
        label-width="90px"
        class="FormInput"
        :rules="dtDataRules2"
        :model="dtData"
        inline
      >
        <!-- style="width:1368px" -->

        <el-form-item label="联系人：">
          <div class="falseIp">{{ dtData.contactName }}</div>
        </el-form-item>

        <el-form-item label="联系人电话：">
          <div class="falseIp">{{ dtData.contactNumber }}</div>
        </el-form-item>

        <el-form-item label="审核人：">
          <div class="falseIp">{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item label="制单人：">
          <div class="falseIp">{{ dtData.operatorName }}</div>
        </el-form-item>
        <el-form-item label="制单人：">
          <div class="falseIp">{{ dtData.operatorName }}</div>
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
    <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit" />
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
import BigNumber from "bignumber.js";

export default {
  name: "HLDYD",
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
  data() {
    return {
      warehousingDateForMat: "",
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
        supplierId: null,
        supplierName: null,
        id: null,
        operatorId: null,
        operatorName: "",
        orderDate: new Date(),
        orderNo: "",
        orderTypeId: null,
        orderTypeName: null,
        buyerId: null,
        buyerName: null,
        settlementTypeName: null,
        settlementTypeId: null,
        transferStatus: false
      },
      dtDataRules: {
        supplierId: [
          { required: true, message: "请选择客户", trigger: "change" }
        ],
        buyerId: [
          { required: true, message: "请选择采购员", trigger: "change" }
        ],
        orderTypeId: [
          { required: true, message: "请选择采购方式", trigger: "change" }
        ],
        orderDate: [
          { required: true, message: "请选择日期", trigger: "change" }
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
          { required: false, message: "客户名不合法", trigger: "blur" },
          {
            pattern: /^[\u4E00-\u9FA5A-Za-z0-9_]+$/,
            message: "请输入正确的电话格式",
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
      materielData: [], // 物料信息
      TabArr: [
        "materialCode",
        "materialName",
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
            // 数量
            value: "",
            isShow: false,
            valid: false
          },
          actualNum: {
            // 数量
            value: "",
            isShow: false,
            valid: false
          },

          accountUnitNum: {
            // 基本单位数量
            value: "",
            isShow: false,
            valid: false
          },
          actualUnitNum: {
            // 基本单位数量
            value: "",
            isShow: false,
            valid: false
          },

          unitPrice: {
            // 单价
            value: "",
            isShow: false,
            valid: false
          },
          Totalamount: {
            // 金额
            value: "",
            isShow: false,
            valid: false
          },
          profitUnitNum: {
            // 基本单位单价
            value: "",
            isShow: false,
            valid: false
          },
          deficitUnitNum: {
            // 基本单位金额
            value: "",
            isShow: false,
            valid: false
          },

          warehouseUnitId: {
            // 基本单位换算率id
            value: "",
            isShow: false,
            valid: false
          },
          purchaseRate: {
            // 基本单位换算率
            value: "",
            isShow: false,
            valid: false
          },
          supplierId: {
            // 备注
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            value: "",
            isShow: false,
            valid: false
          },

          batchNumber: {
            value: "",
            isShow: false,
            valid: false
          },

          purchaseUnitName: {
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
      btnAip: "", // 按钮权限
      OtherData: {},
      realNameOptions: [], // 收货员列表
      addControl: true, // 是否显示新增按钮
      mountedState:false
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
    // window.removeEventListener("keydown", this.onKeydown);
  },

  beforeDestroy() {
    // window.removeEventListener("keydown", this.onKeydown);
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
      if (this.$route.query.id) {
        this.fullscreenLoading = true;
        this.mountedState = true;
        this.setStyle(); // 设置样式

        // this.OtherData = this.$route.params;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.OnBtnSaveSubmit(this.OtherData);
      }

      this.mountedState = false;
    },
    connectchange() {
      this.dtData.contactName = null;
      this.dtData.contactNumber = null;
      this.connectionData.map(item => {
        if (item.id == this.dtData.supplierId) {
          if (item.childList[0]) {
            this.dtData.contactName = item.childList[0].contactName;
            this.dtData.contactNumber = item.childList[0].contactNumber;
          }
        }
      });
    },
    getCustomer() {
      // 获取供应商
      var rqs = RequestObject.CreateGetObject(false, 0, 0, null);
      request({
        // url: "/basicset/api/TBMCustomerFile/GetMainList",
        url: "/basicset/api/TBMSupplierFile/GetMainList",
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
      // 设置table切换
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
      // 重置所有切换了的Input
      this.popoverState = false;
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i]);
      }
    },

    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = document.getElementById("btnHeader").offsetHeight; // 按钮高度
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
      // 初始化数据

      // this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      this.tableData = this.setTable(30);
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
        (this.dtData.supplierId = null),
        (this.dtData.supplierName = null),
        (this.dtData.id = null),
        (this.dtData.operatorId = null),
        (this.dtData.orderDate = ""),
        (this.dtData.orderTypeId = null),
        (this.dtData.orderTypeName = null),
        (this.dtData.buyerId = null),
        (this.dtData.buyerName = null),
        (this.dtData.settlementTypeName = null),
        (this.dtData.settlementTypeId = null),
        (this.dtData.transferStatus = false);
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
        url: "/purchase/api/TPSMPurchaseOrderMain/GetOrderNo",
        //
        method: "GET"
      }).then(res => {
        if (res.code == -1) {
          this.dtData.orderNo = "";
        } else {
          Storage.LStorage.set("Purchasingcode", res);
          this.dtData.orderNo = res;
        }
      });
    },

    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该采购订单是通过状态，无法编辑");
          return;
        }
        var flag = true;
        var checkSelect = await this.checkSelect(this.tableData);
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
                  item.validityPeriod.valid = false;

                  if (state == 1) {
                    //              "id": 0,
                    // "mainId": 0,
                    // "materialId": 0,
                    // "unitPrice": 0,
                    // "purchaseNum": 0,
                    // "purchaseAmount": 0,
                    // "deliveryPeriod": "2019-08-27T03:12:40.742Z",
                    // "transferNum": 0,
                    // "remark": "string",
                    // "_LogName": "string"
                    var param = {
                      id: 0,
                      mainId: 0,
                      materialId: item.materialCode.key,
                      unitPrice: item.unitPrice.value
                        ? item.unitPrice.value
                        : 0,
                      purchaseNum: item.actualNum.value
                        ? item.actualNum.value
                        : 0,
                      purchaseAmount: item.Totalamount.value
                        ? item.Totalamount.value
                        : 0,
                      deliveryPeriod: formatDate(item.validityPeriod.value),
                      transferNum: 0,
                      remark: item.remark.value,
                      _LogName: "采购订单其中一条信息"
                    };
                  } else if (state == 2) {
                    var param = {
                      id: item.idandmainid.id ? item.idandmainid.id : 0,
                      mainId: this.globalid,
                      materialId: item.materialCode.key,
                      unitPrice: item.unitPrice.value
                        ? item.unitPrice.value
                        : 0,
                      purchaseNum: item.actualNum.value
                        ? item.actualNum.value
                        : 0,
                      purchaseAmount: item.Totalamount.value
                        ? item.Totalamount.value
                        : 0,
                      deliveryPeriod: formatDate(item.validityPeriod.value),
                      transferNum: 0,
                      remark: item.remark.value,
                      _LogName: "采购订单其中一条信息"
                    };
                  }

                  if (param.materialId) {
                    if (!item.materialCode.key) {
                      // 验证物料代码
                      item.materialCode.valid = true;
                      item.materialName.valid = true;
                    }
                    if (!item.actualNum.value) {
                      // 验证物仓库ID
                      item.actualNum.valid = true;
                    }
                    if (!item.validityPeriod.value) {
                      // 验证物仓库ID
                      item.validityPeriod.valid = true;
                    }

                    childList.push(param);
                    if (!param.deliveryPeriod || !param.purchaseNum) {
                      flag = false;
                    }
                  }

                  if (item.remark.value.length > 50) {
                    this.remarkRel = false;
                    //  this.$message.error("备注数值长度超标");
                  }

                  if (item.actualNum.value > 99999999999) {
                    this.actualRel = false;
                  }
                });
                if (state == 1) {
                  // 新增

                  //                   "id": 0,
                  // "supplierId": 0,
                  // "buyerId": 0,
                  // "orderNo": "string",
                  // "orderTypeId": 0,
                  // "settlementTypeId": 0,
                  // "currency": "string",
                  // "orderDate": "2019-08-27T03:12:40.742Z",
                  // "operatorId": 0,
                  // "contactName": "string",
                  // "contactNumber": "string",
                  var para = {
                    postData: {
                      id: 0,
                      buyerId: this.dtData.buyerId,
                      orderNo: this.dtData.orderNo,
                      orderTypeId: this.dtData.orderTypeId,
                      settlementTypeId: this.dtData.settlementTypeId,
                      currency: this.dtData.currency,
                      orderDate: formatDate(this.dtData.orderDate),
                      operatorId: 0,
                      supplierId: this.dtData.supplierId,
                      contactName: this.dtData.contactName,
                      contactNumber: this.dtData.contactNumber,
                      childList: childList
                    }
                  };
                } else if (state == 2) {
                  // 编辑
                  var para = {
                    postData: {
                      id: this.globalid,
                      buyerId: this.dtData.buyerId,
                      orderNo: this.dtData.orderNo,
                      orderTypeId: this.dtData.orderTypeId,
                      settlementTypeId: this.dtData.settlementTypeId,
                      currency: this.dtData.currency,
                      orderDate: formatDate(this.dtData.orderDate),
                      operatorId: 0,
                      supplierId: this.dtData.supplierId,
                      contactName: this.dtData.contactName,
                      contactNumber: this.dtData.contactNumber,
                      childList: childList
                    }
                  };
                }
                if (!this.actualRel) {
                  // 校验数量
                  this.$message.error("数量过大必须小于999999999件");
                } else if (!this.remarkRel) {
                  // 校验备注
                  this.$message.error("备注或商品代码、名称数值长度超标");
                } else {
                  if (state == 1) {
                    if (
                      !flag ||
                      !para.postData.buyerId ||
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
                      !para.postData.buyerId ||
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
        // 删除
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该采购订单是通过状态，无法删除");
          return;
        }

        if (this.selectRow.id == undefined) {
          this.$message.error("请选择删除的数据");
          return;
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的采购订单`
        };

        var data = RequestObject.CreatePostObject(currData);

        this.$confirm("是否删除采购订单？", "确认信息", {
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
        // /api/TPSMPurchaseOrderMain
        url: "/purchase/api/TPSMPurchaseOrderMain",
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
            Storage.LStorage.remove("Purchasingcode");
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
        { column: "typeId", content: 21, condition: 0 }
      ]);

      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [
          { column: "TypeName", content: "采购方式", condition: 0 }
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

    rowClick(row) {
      var that = this;

      this.doItem.materialCode.value = row.materialCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.materialName;
      this.doItem.spec.value = row.spec;
      this.doItem.colorName.value = row.colorName;
      this.doItem.purchaseUnitName.value = row.purchaseUnitName;
      this.doItem.baseUnitName.value = row.baseUnitName;
      this.doItem.warehouseUnitId.value = row.warehouseUnitId;
      this.doItem.purchaseRate.value = row.purchaseRate;
      this.listenClick();
      this.doItem.actualNum.isShow = true;
      this.$nextTick(() => {
        document.getElementById(this.doItem.id + "___" + "actualNum").focus();
      });

      that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
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
          // actualNum
          if (index == 6 || index == 7 || index == 10) {
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
      var wl = document.documentElement.clientWidth; // 屏幕宽度
      var wh = document.documentElement.clientHeight; // 屏幕宽度
      var PoL = document
        .getElementById(item.id + "___" + name)
        .getBoundingClientRect().left; // 弹框left值
      var PoT = document
        .getElementById(item.id + "___" + name)
        .getBoundingClientRect().top; // 弹框top值
      // pol和PoT是主要数据
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

    // 输入金额时顺便计算
    actualNumBlur(item, input) {
      if (
        item.warehouseUnitId.value != null &&
        item.purchaseRate.value != null
      ) {
        item.actualUnitNum.value = parseFloat(
          item.actualNum.value * item.purchaseRate.value
        )
          ? keepTwoDecimalFull(item.actualNum.value * item.purchaseRate.value)
          : null;
      } else {
        item.actualUnitNum.value = parseFloat(item.actualNum.value);
      }
      // actualNum
      var r = /^\+?[1-9][0-9]*$/; // 正整数
      var flag = r.test(input);

      if (!flag) {
        item.actualNum.value = null;
      }
      var nowactualNum = item.actualNum.value;
      nowactualNum = parseFloat(nowactualNum);

      if (nowactualNum) {
        if (item.Totalamount.value > 0 && !item.unitPrice.value) {
          // 当单价为0时，当金额不为0时
          item.unitPrice.value = keepTwoDecimalFull(
            item.Totalamount.value / nowactualNum
          );
        } else if (item.unitPrice.value > 0 || item.Totalamount.value > 0) {
          // 单价不为0时
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
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; // 正整数
      var flag = r.test(input);

      if (!flag) {
        item.unitPrice.value = null;
      }

      var nowunitPrice = item.unitPrice.value;
      nowunitPrice = parseFloat(nowunitPrice);

      if (nowunitPrice) {
        if (item.actualNum.value != "") {
          item.unitPrice.value = nowunitPrice;
          item.Totalamount.value = keepTwoDecimalFull(
            accMul(item.actualNum.value, nowunitPrice)
          );
        } else {
          item.unitPrice.value = nowunitPrice;
        }
      } else {
        item.Totalamount.value = 0;
      }
    },
    amountBlur(item, input) {
      var r = /^(0|[1-9][0-9]*)(\.\d+)?$/; // 正整数
      var flag = r.test(input);

      if (!flag) {
        item.Totalamount.value = null;
      }
      // if (item.Totalamount.value == "" || item.Totalamount.value == null) return;

      var nowTotalamount = item.Totalamount.value;
      nowTotalamount = parseFloat(nowTotalamount);

      if (nowTotalamount) {
        if (item.Totalamount.value == "" || item.Totalamount.value == null) {
          return;
        }
        if (item.actualNum.value != "") {
          item.Totalamount.value = parseFloat(item.Totalamount.value);
          item.unitPrice.value = keepTwoDecimalFull(
            item.Totalamount.value / item.actualNum.value
          );
        } else {
          item.Totalamount.value = keepTwoDecimalFull(
            parseFloat(item.amount.value)
          );
        }
      } else {
        item.unitPrice.value = 0;
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
        url: "/purchase/api/TPSMPurchaseOrderMain/GetMainList",
        // /api/TPSMPurchaseOrderMain/GetMainList

        method: "GET",
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          // {
          //   "info": "string",
          //   "code": 0,
          //   "totalNumber": 0,
          //   "data": [
          //     {
          //       "id": 0,
          //       "supplierId": 0,
          //       "supplierName": "string",
          //       "buyerId": 0,
          //       "buyerName": "string",
          //       "orderNo": "string",
          //       "orderTypeId": 0,
          //       "orderTypeName": "string",
          //       "settlementTypeId": 0,
          //       "settlementTypeName": "string",
          //       "currency": "string",
          //       "orderDate": "2019-08-27T02:47:01.848Z",
          //       "auditStatus": 0,
          //       "auditId": 0,
          //       "auditName": "string",
          //       "auditTime": "2019-08-27T02:47:01.848Z",
          //       "operatorId": 0,
          //       "operatorName": "string",
          //       "contactName": "string",
          //       "contactNumber": "string",
          //       "companyId": 0,
          //       "deleteFlag": true,
          //       "transferStatus": true,
          //       "purchaseNum": 0,
          //       "purchaseAmount": 0,
          //       "allowEdit": true,
          //       "childList": []
          //     }
          //   ]
          // }
          this.warehousingDateForMat = res.data[0].orderDate.split("T")[0];
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
          this.dtData.supplierId = res.data[0].supplierId;
          this.dtData.supplierName = res.data[0].supplierName;
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

          this.dtData.buyerId = res.data[0].buyerId;
          this.dtData.buyerName = res.data[0].buyerName;

          this.dtData.settlementTypeName = res.data[0].settlementTypeName;
          this.dtData.settlementTypeId = res.data[0].settlementTypeId;
          this.dtData.transferStatus = res.data[0].transferStatus;

          request({
            // /api/TPSMPurchaseOrderMain/GetDetailList
            url: "/purchase/api/TPSMPurchaseOrderMain/GetDetailList",
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
          materialId: parseFloat(item.materialCode.key) || ""
          // warehouseId: parseFloat(item.defaultWarehouseName.key) || ""
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
      this.tableData = [];
      if (dt[0]) {
        this.globalid = dt[0].mainId;
      }

      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,

          idandmainid: {
            id: item.id,
            mainId: item.mainId,
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
            // 颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },

          baseUnitName: {
            // 基本单位名称
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },

          actualNum: {
            // 数量
            value: item.purchaseNum,
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            // 基本单位数量
            value: item.baseUnitNum,
            isShow: false,
            valid: false
          },

          unitPrice: {
            // 单价
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          Totalamount: {
            // 金额
            value: item.purchaseAmount,
            isShow: false,
            valid: false
          },

          purchaseRate: {
            // 基本单位换算率
            value: item.purchaseRate,
            isShow: false,
            valid: false
          },
          supplierId: {
            // 备注
            value: item.supplierName,
            key: item.supplierId,
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            value: item.remark ? item.remark : "",
            isShow: false,
            valid: false
          },

          validityPeriod: {
            value: item.deliveryPeriod,
            isShow: false,
            valid: false
          },
          purchaseUnitName: {
            // 单位
            value: item.purchaseUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitId: {
            // 基本单位换算率id
            value: item.warehouseUnitId,
            isShow: false,
            valid: false
          }
        };

        listArr.push(list);
      });

      setTimeout(() => {
        // this.setTable(30);
        this.tableData = [...listArr];
      }, 0);
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 500);
    },

    doOtherWhAudit(state) {
      // 审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error("请选择审核数据");
        return;
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error("该采购订单已经为通过状态");
        return;
      }
      // var data = RequestObject.CreatePostObject({
      //   id: this.selectRow.id,
      //   auditStatus: state
      // });
      var data = {
        postData: {
          id: this.selectRow.id,
          auditStatus: state
        }
      };
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
            url: "/Purchase/api/TPSMPurchaseOrderMain/PurchaseAudit",
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
#Purchasingorder #elfooter .el-input__inner {
  border: 0px;
  border-radius: 0px;
  border-bottom: 1px solid #dcdfe6;
}
</style>

<style lang="scss" scoped>
#Purchasingorder /deep/ {
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
  .middleWidth {
    width: 1398px;
  }

  .falseIptop {
    width: 200px;
    overflow: hidden;
    // border-bottom: 1px solid #dcdfe6;
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
  //   width: 1398px;
  // }
  //    .formItem {

  //   margin-left:10px;
  //   width: 287px;
  // }
  // }

  // @media screen and (max-width: 1648px) {
  //   .middleWidth {
  //     width: 1248px;
  //   }
  //    .formItem {

  //   margin-left: 10px;
  //   width: 287px;
  // }
  // }

  //  @media screen and (max-width: 1530px) {
  //   .middleWidth {
  //     width: 1048px;
  //   }
  //    .formItem {

  //   margin-left: 20px;
  //   width: 306px;
  // }
  // }
  // @media screen and (max-width: 1288px) {
  //   .middleWidth {
  //     width: 848px;
  //   }
  //    .formItem {

  //   margin-left: 40px;
  //   width: 326px;
  // }
  // }
  .el-table {
    overflow: visible !important;
  }
  .el-main {
    padding-bottom: 10px;
  }
}
@import "@/styles/receipts.scss";
</style>
