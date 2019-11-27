<template>
  <el-container
    id="MRP"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    @click.native="listenClick"
  >
    <!-- table列表开始 -->
    <el-main>
      <div class="leftBox" :style="'height:'+tableHeight+'px'">
        <DataTree ref="DataTree"></DataTree>
      </div>
      <div class="rightBox" :style="'height:'+tableHeight+'px'">
        <!-- 头部按钮区开始 -->
        <div style="padding:5px 0px 0px 20px;height: 37px;font-size:0px;">
          <div v-if="btnAip.add">
            <el-button-group class="groupBtn">
              <el-button
                v-if="btnAip.calmaterial.buttonCaption"
                :disabled="!mrpStatus"
                type="default"
                @click="doTrans(1)"
              >{{ btnAip.calmaterial.buttonCaption }}</el-button>
              <el-button
                v-if="btnAip.createpick.buttonCaption"
                :disabled="!isPick"
                type="default"
                @click="doTrans(2)"
              >{{ btnAip.createpick.buttonCaption }}</el-button>
              <el-button
                v-if="btnAip.createorder.buttonCaption"
                :disabled="!isPurchase"
                type="default"
                @click="doTrans(3)"
              >{{ btnAip.createorder.buttonCaption }}</el-button>
            </el-button-group>
          </div>
        </div>
        <!-- 头部按钮区结束 -->
        <!-- 头部表单开始 -->
        <el-header id="elheader" class="header" height="auto">
          <div>
            <el-form
              ref="dtData"
              :model="dtData"
              label-width="114px"
              class="FormInput"
              inline
              label-position="right"
              :rules="dtDataRules"
            >
              <!-- <el-form-item class="formItem disableType" label="生产订单编号：">
                <div class="headerIp">{{ dtData.customerName }}</div>
              </el-form-item>-->
              <el-form-item class="formItem" label="生产订单编号：" prop="orderNo" label-width="114px">
                <el-input
                  v-model="dtData.orderNo"
                  class="headerIp"
                  readonly
                  @click.native="chooseNumber"
                >
                  <i slot="suffix" class="el-input__icon el-icon-search" />
                </el-input>
              </el-form-item>

              <!-- <el-form-item class="formItem" label="配色方案：">
                <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:200px;"></el-input>
              </el-form-item>

              <el-form-item class="formItem" label="总数量">
                <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:200px;"></el-input>
              </el-form-item>

              <el-form-item class="formItem" label="交货日期：" prop="warehousingDate">
                <el-date-picker
                  v-model="dtData.warehousingDate"
                  type="date"
                  placeholder="选择日期"
                  class="headerIp"
                  :readonly="selectRow.auditStatus === 2"
                />
              </el-form-item>

              <el-form-item class="formItem" label="客户名称：">
                <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:200px;"></el-input>
              </el-form-item>-->
            </el-form>
          </div>
        </el-header>
        <!-- 头部表单结束 -->
        <!-- table区域开始 -->
        <div @click.stop="1==1">
          <el-table
            id="tableData"
            ref="table"
            :height="tableHeight3"
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
                    <i class="el-icon-setting" />
                  </span>
                </div>
              </template>
            </el-table-column>

            <el-table-column prop="materialCode" label="物料代码" width="200">
              <template slot="header">
                <span class="tableHeader">
                  <span>物料代码</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'materialCode')"
                >{{ scope.row.materialCode.value }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="materialName" label="物料名称" width="200">
              <template slot="header">
                <span class="tableHeader">
                  <span>物料名称</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'materialName')"
                >{{ scope.row.materialName.value }}</div>
              </template>
            </el-table-column>

            <!-- <el-table-column prop="colorSolutionCode" label="颜色对应序号" width="200">
              <template slot="header">
                <span class="tableHeader">
                  <span>颜色对应序号</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div @click="chengenum($event,scope.row,'colorSolutionCode')">
                  <div class="tableTd">{{ scope.row.colorSolutionCode.value }}</div>
                </div>
              </template>
            </el-table-column>
            <el-table-column prop="color" label="颜色">
              <template slot="header">
                <span class="tableHeader">
                  <span>颜色</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'color')"
                >{{ scope.row.color.value }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="singleNum" label="单色数量">
              <template slot="header">
                <span class="tableHeader">
                  <span>单色数量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'singleNum')"
                >{{ scope.row.singleNum.value }}</div>
              </template>
            </el-table-column> -->

            <!-- <el-table-column prop="purchaseNum" label="采购数量">
              <template slot="header">
                <span class="tableHeader">
                  <span>采购数量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'purchaseNum')"
                >{{ scope.row.purchaseNum.value }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="purchaseTransNum" label="已转采购申请单数量" width="140">
              <template slot="header">
                <span class="tableHeader">
                  <span>已转采购申请单数量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'purchaseTransNum')"
                >{{ scope.row.purchaseTransNum.value }}</div>
              </template>
            </el-table-column>

            <el-table-column prop="pickNum" label="领料数量">
              <template slot="header">
                <span class="tableHeader">
                  <span>领料数量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'pickNum')"
                >{{ scope.row.pickNum.value }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="pickTransNum" label="已转领料申请单数量" width="140">
              <template slot="header">
                <span class="tableHeader">
                  <span>已转领料申请单数量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'pickTransNum')"
                >{{ scope.row.pickTransNum.value }}</div>
              </template>
            </el-table-column> -->

            <el-table-column prop="singleValue" label="单个用量">
              <template slot="header">
                <span class="tableHeader">
                  <span>单个用量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'singleValue')"
                >{{ scope.row.singleValue.value }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="totalValue" label="总用量">
              <template slot="header">
                <span class="tableHeader">
                  <span>总用量</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'totalValue')"
                >{{ scope.row.totalValue.value }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="baseUnitName" label="基本单位">
              <template slot="header">
                <span class="tableHeader">
                  <span>基本单位</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'baseUnitName')"
                >{{ scope.row.baseUnitName.value }}</div>
              </template>
            </el-table-column>
            <el-table-column prop="produceUnitName" label="生产单位">
              <template slot="header">
                <span class="tableHeader">
                  <span>生产单位</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  class="tableTd"
                  @click="chengenum($event,scope.row,'produceUnitName')"
                >{{ scope.row.produceUnitName.value }}</div>
              </template>
            </el-table-column>

            <!-- <el-table-column prop="remark" label="备注" width="280">
              <template slot="header">
                <span class="tableHeader">
                  <span class="start" />
                  <span>备注</span>
                </span>
              </template>
              <template slot-scope="scope">
                <div
                  :class="{validStyle:scope.row.remark.valid}"
                  @click="chengenum($event,scope.row,'remark')"
                >
                  <div v-show="!scope.row.remark.isShow" class="tableTd">
                    <el-popover
                      v-if="scope.row.remark.value.length>=20"
                      placement="top-start"
                      trigger="hover"
                      :content="scope.row.remark.value"
                    >
                      <div class="ellipsis" slot="reference">{{ scope.row.remark.value }}</div>
                    </el-popover>
                    <div
                      v-if="scope.row.remark.value.length<20"
                      class="ellipsis"
                    >{{ scope.row.remark.value }}</div>
                  </div>
                  <el-input
                    v-show="scope.row.remark.isShow"
                    :id="scope.row.id+'___remark'"
                    v-model="scope.row.remark.value"
                    size="mini"
                    maxlength="500"
                  />
                </div>
              </template>
            </el-table-column>-->
          </el-table>
        </div>
        <!-- table区域结束 -->
      </div>
    </el-main>
    <!-- table列表结束 -->

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
        <li @click="doMenuDel">删除</li>
      </ul>
    </div>
    <!-- 序号弹框结束 -->

    <!-- 查看弹框开始 -->
    <!-- <InboundOrder ref="InboundOrder" title="采购入库单查询" @OnBtnSaveSubmit="OnBtnSaveSubmit" /> -->
    <!-- 查看弹框结束 -->

    <!-- 单据号弹框开始 -->
    <DocumentNo ref="DocumentNo" title="生产订单选择" @OnBtnSaveSubmit="OnBtnSaveDocument" />
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
  isRealNum
} from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from "./components/InboundOrder";
import DocumentNo from "./components/DocumentNo";
import DataTree from "./components/DataTree";

import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import Storage from "@/utils/storage";

export default {
  name: "viewsProductManageMRPNoindexvue",
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
    // InboundOrder,
    DocumentNo,
    DataTree
  },
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
        WarehousingType: "", // 客户名称id
        customerName: "", // 供应商名称
        warehousingDate: new Date(), // 出库日期
        warehousingOrder: "", // 编号
        orderTypeId: "", // 订单类型
        orderNo: "", // 单据号
        orderId: "", // 单据号ID
        // receiptAddress: "", // 收货地址
        purchasingMethod: "",
        operatorName: "", // 制单人
        operatorId: "",
        whAdminId: "", // 仓管员
        whAdminName: "",
        // sendId: "", // 发货员
        // sendName: "",
        receiptName: "", //收货员
        receiptId: "",
        auditName: "", // 审核人
        auditId: "",
        auditStatus: -1, // 状态
        auditTime: "" // 审核时间
      },
      dtDataRules: {
        WarehousingType: [
          // { required: true, message: "请选择客户名称", trigger: "change" }
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
      tableHeight: 800,
      tableHeight2: 800,
      tableHeight3: 800,
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
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          materialName: {
            // 物料名称
            value: "",
            isShow: false,
            valid: false
          },
          colorSolutionCode: {
            // 颜色序号
            value: "",
            isShow: false,
            valid: false
          },
          color: {
            //颜色
            value: "",
            isShow: false,
            valid: false
          },
          singleNum: {
            //单色数量
            value: "",
            isShow: false,
            valid: false
          },
          purchaseNum: {
            //采购数量
            value: "",
            isShow: false,
            valid: false
          },
          purchaseTransNum: {
            //已转采购申请单数量
            value: "",
            isShow: false,
            valid: false
          },
          pickNum: {
            //领料数量
            value: "",
            isShow: false,
            valid: false
          },
          pickTransNum: {
            //已转领料申请单数量
            value: "",
            isShow: false,
            valid: false
          },
          singleValue: {
            //单个用量
            value: "",
            isShow: false,
            valid: false
          },
          totalValue: {
            //总用量
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitName: {
            //基本单位
            value: "",
            isShow: false,
            valid: false
          },
          produceUnitName: {
            //生产单位
            value: "",
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
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
      TabArr: [],
      materialItem: {},
      DataTreeColArr: [],
      btnState: -1,
      mrpStatus: true,
      isPick: true,
      isPurchase: true
    };
  },
  created() {
    // 获取按钮权限
    this.fullscreenLoading = true;
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
  },
  async mounted() {
    window.addEventListener("keydown", this.onKeydown);
    this.setStyle(); // 设置样式
    setTimeout(() => {
      this.tableHeight3 = this.tableHeight2;
      this.fullscreenLoading = false;
    }, 800);
    this.tableData = this.setTable(20); // 添加30行
  },
  async activated() {
    window.addEventListener("keydown", this.onKeydown);
  },
  deactivated() {
    this.$refs.DocumentNo.openNO();
    window.removeEventListener("keydown", this.onKeydown);
  },
  methods: {
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = 37; // 按钮高度
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        // var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        // var f = elfooter.clientHeight || elfooter.offsetHeight;
        this.tableHeight = b - 40;
        this.tableHeight2 = b - h - 40 - btn;
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
                    // console.log("-");
                    if (l >= this.tableData.length) {
                      l = this.tableData.length;
                    } else {
                      l = l + 1;
                    }
                  } else {
                    // console.log("+");
                    var go = k + 1;
                  }
                  // console.log(go);
                  var set = this.TabArr[go];
                  data.index = l;
                  data.name = set;
                  data.item = this.tableData[l];
                  // console.log(data);
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
              return prev + curr;
            } else {
              return prev;
            }
          }, 0);
          if (index == 5 || index == 6 || index == 7) {
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

      //通过不能编辑
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
    chooseNumber() {
      // 选择单据号
      // if (this.selectRow.auditStatus !== undefined) {
      //   return;
      // }
      this.$refs.DocumentNo.open();
      this.$refs.DocumentNo.dtData.productionNo = "";
      this.$refs.DocumentNo.dtData.orderDate = "";
      this.$refs.DocumentNo.dtData.auditStatus = -1;
      this.$refs.DocumentNo.getMainList();
    },
    handelAddClick() {
      // 查询
      this.$refs.InboundOrder.open();
      this.$refs.InboundOrder.dtData.warehousingOrder = "";
      this.$refs.InboundOrder.dtData.warehousingDate = "";
      this.$refs.InboundOrder.dtData.auditStatus = -1;
      this.$refs.InboundOrder.clickRow = {};
      this.$refs.InboundOrder.getMainList();
    },
    OnBtnSaveDocument(row, state) {
      this.fullscreenLoading = true;
      this.selectRow = row;
      this.dtData.orderNo = row.productionNo;
      this.dtData.orderId = row.id;
      this.mrpStatus = !row.mrpStatus;
      this.isPick = row.isPick;
      this.isPurchase = row.isPurchase;

      if (!state) {
        request({
          url: "manufacturing/api/MRP/GetBom",
          method: "GET",
          params: { requestObject: row.id }
        }).then(res => {
          this.fullscreenLoading = false;
          if (res.code === 0) {
            this.setTableData2(res.data.mainList, row);
            this.$refs.DataTree.setColor(res.data.packageId);
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
    unique(array) {
      return Array.from(new Set(array));
    },
    setTableData2(dt) {
      // 将数据导入table
      this.tableData = [];
      // this.DataTreeColArr = [];
      var listArr = [];
      dt.map(item => {
        var addFlag = false;
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            // 物料代码
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          materialName: {
            // 物料名称
            value: item.materialName,
            isShow: false,
            valid: false
          },
          colorSolutionCode: {
            // 颜色序号
            value: item.colorSolutionCode,
            isShow: false,
            valid: false
          },
          color: {
            //颜色
            value: item.colorName,
            isShow: false,
            valid: false
          },
          singleNum: {
            //单色数量
            value: item.productionNum,
            isShow: false,
            valid: false
          },
          purchaseNum: {
            //采购数量
            value: item.purchaseNum,
            isShow: false,
            valid: false
          },
          purchaseTransNum: {
            //已转采购申请单数量
            value: item.purchaseTransNum,
            isShow: false,
            valid: false
          },
          pickNum: {
            //领料数量
            value: item.pickNum,
            isShow: false,
            valid: false
          },
          pickTransNum: {
            //已转领料申请单数量
            value: item.pickTransNum,
            isShow: false,
            valid: false
          },
          singleValue: {
            //单个用量
            value: item.singleValue,
            isShow: false,
            valid: false
          },
          totalValue: {
            //总用量
            value: item.totalValue,
            isShow: false,
            valid: false
          },
          baseUnitName: {
            //基本单位
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          produceUnitName: {
            //生产单位
            value: item.produceUnitName,
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            value: "",
            isShow: false,
            valid: false
          }
        };
        this.DataTreeColArr.push(item.packageId);
        listArr.unshift(list);
      });
      // this.DataTreeColArr = this.unique(this.DataTreeColArr);
      // this.$refs.DataTree.setColor(this.DataTreeColArr);
      setTimeout(() => {
        this.tableData = [...listArr];
        this.fullscreenLoading = false;
      }, 0);
    },
    doTrans(state) {
      if (this.dtData.orderId === "") {
        this.$message.error("请选择生产订单！");
        return false;
      }
      var URL = "";
      if (state == 1) {
        URL = "manufacturing/api/MRP/AutoComputeNOMRP";
      }
      if (state == 2) {
        URL = "manufacturing/api/MRP/TransPick";
      }
      if (state == 3) {
        URL = "manufacturing/api/MRP/TransPurchase";
      }
      this.fullscreenLoading = true;
      request({
        url: URL,
        method: "post",
        params: { requestObject: this.dtData.orderId }
      }).then(res => {
        if (res.code === 0) {
          if (state == 1) {
            this.$message({
              message: "操作成功",
              type: "success"
            });
            this.selectRow.mrpStatus = true;
            this.mrpStatus = false;
            this.setTableData2(res.data);
            this.fullscreenLoading = false;
          }

          if (state == 2) {
            this.$message({
              message: "操作成功",
              type: "success"
            });
            this.selectRow.isPick = false;
            this.OnBtnSaveDocument(this.selectRow);
            this.fullscreenLoading = false;
          }

          if (state == 3) {
            this.$message({
              message: "操作成功",
              type: "success"
            });
            this.selectRow.isPurchase = false;
            this.OnBtnSaveDocument(this.selectRow);
            this.fullscreenLoading = false;
          }
        } else {
          this.fullscreenLoading = false;
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    }
  }
};
</script>
<style lang="scss" scoped>
#MRP /deep/ {
  .link {
    color: #1890ff;
    cursor: pointer;
  }
  .el-main:after {
    clear: both;
    content: "";
    display: block;
    width: 0;
    height: 0;
    visibility: hidden;
  }
  #elfooter {
    padding: 0px 20px;
  }
  .el-table--group,
  .el-table--border {
    border: 0px solid #dfe6ec;
    border-top: 1px solid #dfe6ec;
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
  // .middleWidth {
  //   width: 1240px;
  // }
  // @media screen and (max-width: 1510px) {
  //   .middleWidth {
  //     width: 929px;
  //   }
  // }
  .leftBox {
    float: left;
    width: 200px;
    border: 1px solid #dfe6ec;
    overflow-y: auto;
    border-right: 0px solid #dfe6ec;
  }
  .rightBox {
    float: left;
    width: calc(100% - 200px);
    border: 1px solid #dfe6ec;
    overflow: hidden;
  }
}
</style>
