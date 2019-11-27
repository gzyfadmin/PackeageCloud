<template>
  <el-container
    id="ProcurementPut"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <div class="btnHeader" id="btnHeader">
      <h1>出库单</h1>
    </div>
    <el-header id="elheader" class="header" height="auto">
      <el-form
        ref="dtData"
        :model="dtData"
        label-width="76px"
        class="FormInput"
        inline
        label-position="left"
        :rules="dtDataRules"
      >
        <el-form-item class="formItem" label="出库类型：">
          <div>
            <div v-if="dtData.WarehousingType==0">销售出库</div>
            <div v-if="dtData.WarehousingType==1">生产入库</div>
            <div v-if="dtData.WarehousingType==2">裁片出库</div>
            <div v-if="dtData.WarehousingType==3">委外出库</div>
            <div v-if="dtData.WarehousingType==4">盘亏出库</div>
            <div v-if="dtData.WarehousingType==5">其他出库</div>
          </div>
        </el-form-item>
        <el-form-item class="formItem" label="出库日期：">
          <div>{{ warehousingDateForMat?warehousingDateForMat:"&nbsp;" }}</div>
        </el-form-item>
        <el-form-item class="formItem" label="编号：" label-width="48px">
          <div style="height:32px;">{{ dtData.warehousingOrder?dtData.warehousingOrder:"&nbsp;" }}</div>
        </el-form-item>
        <el-form-item class="formItem" label="出库状态：">
          <div style="height:32px;">
            <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待出库</span>
            <span v-if="dtData.auditStatus==0">待审核</span>
            <span v-if="dtData.auditStatus==1">审核未通过</span>
            <span v-if="dtData.auditStatus==2">审核通过</span>
          </div>
        </el-form-item>
      </el-form>
    </el-header>
    <el-main>
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
                <span>{{ scope.$index+1 }}</span>
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
                />
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="spec" label="规格型号">
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.spec.valid}">
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
              <div>
                <div class="tableTd">{{scope.row.colorName.value}}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="defaultWarehouseName" label="发货仓库">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>发货仓库</span>
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
                  @change="WarehouseChange(scope.row)"
                >
                  <el-option
                    v-for="item in warehouseData"
                    :key="item.id"
                    :label="item.warehouseName"
                    :value="item.id"
                  />
                </el-select>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="warehouseSum" label="可用数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可用数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.warehouseSum.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="PaidIn" label="实发数量">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>实发数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.PaidIn.valid}">
                <div v-show="!scope.row.PaidIn.isShow" class="tableTd">{{ scope.row.PaidIn.value }}</div>
                <el-input
                  v-show="scope.row.PaidIn.isShow"
                  :id="scope.row.id+'___PaidIn'"
                  v-model="scope.row.PaidIn.value"
                  size="mini"
                />
              </div>
            </template>
          </el-table-column>

          <!-- <el-table-column prop="unit" label="基本单位">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.unit.value }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitNumber" label="基本单位数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitNumber.value }}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="batchNumber" label="批号">
            <template slot="header">
              <span class="tableHeader">
                <span>批号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.batchNumber.valid}">
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
              </div>
            </template>
          </el-table-column>

          <!-- <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div v-if="scope.row.warehouseUnitName.value" class="tableTd">{{ scope.row.warehouseUnitName.value }}</div>
              <div v-if="!scope.row.warehouseUnitName.value" class="tableTd">{{ scope.row.unit.value }}</div>
            </template>
          </el-table-column>-->

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
                />
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="amount" label="金额">
            <template slot="header">
              <span class="tableHeader">
                <span>金额</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div :class="{validStyle:scope.row.amount.valid}">
                <div v-show="!scope.row.amount.isShow" class="tableTd">{{ scope.row.amount.value }}</div>
                <el-input
                  v-show="scope.row.amount.isShow"
                  :id="scope.row.id+'___amount'"
                  v-model="scope.row.amount.value"
                  size="mini"
                />
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="defaultWarehouseName" label="有效期" width="145">
            <template slot="header">
              <span class="tableHeader">
                <span>有效期</span>
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
              <div>
                <div class="tableTd">
                  <!-- <el-tooltip
                    class="item"
                    effect="light"
                    :content="scope.row.remark.value"
                    :open-delay="300"
                    placement="top"
                  >
                    <div class="ellipsis">{{scope.row.remark.value}}</div>
                  </el-tooltip>-->
                  <el-popover
                    v-if="scope.row.remark.value!=null&&scope.row.remark.value.length>=20"
                    placement="top-start"
                    trigger="hover"
                    :content="scope.row.remark.value"
                  >
                    <div class="ellipsis" slot="reference">{{ scope.row.remark.value }}</div>
                  </el-popover>
                  <div
                    v-if="scope.row.remark.value==null||scope.row.remark.value.length<20"
                    class="ellipsis"
                  >{{ scope.row.remark.value }}</div>
                </div>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div id="elfooter">
      <el-form label-width="70px" class="FormInput" inline>
        <div>
          <el-form-item label="制单人">
            <div class="falseIp">{{ dtData.operatorName }}</div>
          </el-form-item>
          <el-form-item label="收货员">
            <div class="falseIp">{{ dtData.receiptName }}</div>
          </el-form-item>
          <el-form-item label="审核人">
            <div class="falseIp">{{ dtData.auditName }}</div>
          </el-form-item>
          <el-form-item label="审核结果">
            <div class="falseIp">
              <span v-if="dtData.auditStatus==-1" />
              <span v-if="dtData.auditStatus==0">待审核</span>
              <span v-if="dtData.auditStatus==1">未通过</span>
              <span v-if="dtData.auditStatus==2">通过</span>
              <span v-if="dtData.auditStatus==3">撤销审核</span>
            </div>
          </el-form-item>
          <el-form-item label="审核时间">
            <div class="falseIp">{{ dtData.auditTime }}</div>
          </el-form-item>
        </div>
      </el-form>
    </div>
    <!-- <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit" /> -->
  </el-container>
</template>
<script>
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from './components/InboundOrder'
import { getBtnCtr } from "@/utils/BtnControl";
import Pagination from "@/components/Pagination";
// import { promises } from 'dns';
import { setTimeout } from "timers";
import { closest } from "@/utils/Dom";
import BigNumber from "bignumber.js";

export default {
  name: "HHHFuM",
  filters: {
    formatTDate: value => {
      if (value == "") {
        return "";
      }
      return formatDate(value);
    }
  },
  components: {
    Pagination
  },
  //   inventoryId: 174
  // warehouseId: 17
  data() {
    return {
      warehousingDateForMat: "",
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
        auditTime: ""
      },
      dtDataRules: {
        WarehousingType: [
          { required: true, message: "请选择出库类型", trigger: "change" }
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
      selectRow: [],
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], // 物料信息
      warehouseData: [], // 仓库信息
      tableData: [], // table数据模型
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
          materialName: {
            value: "",
            isShow: false,
            valid: false
          },
          spec: {
            value: "",
            isShow: false,
            valid: false
          },
          colorName: {
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
          batchNumber: {
            value: "",
            isShow: false,
            valid: false
          },
          warehouseSum: {
            value: "",
            isShow: false,
            valid: false
          },
          unit: {
            value: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            value: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
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
          },
          remark: {
            value: "",
            isShow: false,
            valid: false
          }
        }
      ],
      realNameOptions: [], // 收货员列表
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      routeData: "",
      isShow: true,
      addShow: true,
      btnAip: "",
      mountedState: false
    };
  },
  created() {
    // 动态头部按钮
    // this.setStyle();
  },
  mounted() {
    this.runing();
  },
  activated() {
     if(this.atob(this.$route.query.id)==this.routeData.id){
      return;
    }
    this.runing();
  },
  methods: {
    async runing() {
      if (this.mountedState == true) {
        return;
      }
      this.mountedState = true;
      this.fullscreenLoading = true;
      this.warehouseData = await this.getWarehouseData(); // 仓库数据
      this.setStyle();
      this.getUserCompany(); // 收货员列表
      this.btnAip = getBtnCtr(
        this.$route.path,
        this.$store.getters.userPermission
      );
      this.dtData.operatorName = this.$store.state.user.name;
      this.tableData2 = [...this.tableData];
      // this.routeData = this.$route.params;
      this.routeData = {
        id: this.atob(this.$route.query.id)
      };
      if (this.$route.query.id) {
        this.fullscreenLoading = true;
        this.getMainList(this.routeData.id);
        this.isShow = false;
      }
      this.mountedState = false;
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
    // 重置
    resetData() {
      // 初始化数据
      this.$refs["dtData"].resetFields();
      this.addShow = true;
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      // this.setTable(30)
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.WarehousingType = "";
      this.dtData.warehousingDate = new Date();
      this.dtData.warehousingOrder = "";
      this.dtData.remark = "";
      this.dtData.operatorId = "";
      this.dtData.receiptName = "";
      this.dtData.receiptId = "";
      this.dtData.auditName = "";
      this.dtData.auditId = "";
      this.dtData.auditStatus = -1;
      this.dtData.auditTime = "";
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
      // this.setTable(1)
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
      for (var i = 0; i < this.tableData.length; i++) {
        if (
          item.defaultWarehouseName.key != "" &&
          this.tableData[i].defaultWarehouseName.key ==
            item.defaultWarehouseName.key
        ) {
          if (this.tableData[i].materialName.value == item.materialName.value) {
            if (this.tableData[i].id == item.id) {
              // break
              continue;
            }
            this.tableData[i].materialCode.isShow = true;
            this.doItem.defaultWarehouseName.isShow = false;
            this.doItem.materialCode.value = "";
            this.doItem.materialCode.key = "";
            this.doItem.materialName.value = "";
            this.doItem.spec.value = "";
            this.doItem.warehouseUnitName.value = "";
            this.doItem.unit.value = "";
            this.doItem.baseUnitNumber.value = "";

            this.doItem.PaidIn.value = "";
            this.doItem.amount.value = "";
            this.doItem.defaultWarehouseName.key = "";
            this.doItem.defaultWarehouseName.value = "";
            this.doItem.rowIndex = 0;
            this.doItem.spec.value = "";
            this.doItem.unit.value = "";
            this.doItem.unitPrice.value = "";
            this.doItem.validityPeriod.value = "";
            this.doItem.remark.value = "";
            this.doItem.warehouseSum.value = "";
            this.doItem.warehouseUnitName.value = "";
            // this.listenClick()
          }
        }
      }
      // 仓库选择
      this.warehouseData.map(data => {
        if (item.defaultWarehouseName.key == data.id) {
          item.defaultWarehouseName.value = data.warehouseName;
        }
      });
      if (
        item.defaultWarehouseName.key != "" &&
        this.doItem.materialCode.key != ""
      ) {
        this.GetWarehouseNum(
          null,
          this.doItem.materialCode.key,
          item.defaultWarehouseName.key,
          item
        );
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
    GetWarehouseNum(id, materialId, houseID, item) {
      var data = {
        id: id,
        materialId: materialId,
        houseID: houseID
      };
      request({
        url: "/warehouse/api/TWMOtherWhSendMain/Calculate",
        method: "post",
        data: data
      }).then(res => {
        if (res.code === -1) {
          this.loading = false;
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          item.warehouseSum.value = res.data;
        }
      });
    },
    checkSelect(dt) {
      // 查找输出数据项
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
        url: "warehouse/api/TWMOtherWhSendMain/GetOrderNo",
        method: "GET"
      }).then(res => {
        this.dtData.warehousingOrder = res;
      });
    },
    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        var flag = false;
        this.$refs.dtData.validate(valid => {
          // 验证
          if (!valid) {
            this.$message({
              message: "数据不合法，无法保存",
              type: "error"
            });
          } else {
            flag = true;
          }
        });
        if (!flag) return; // 验证头部数据

        var checkSelect = await this.checkSelect(this.tableData);
        var childList = [];
        checkSelect.map(item => {
          // 验证数据
          item.materialCode.valid = false;
          item.materialName.valid = false;
          item.spec.valid = false;
          item.defaultWarehouseName.valid = false;
          item.PaidIn.valid = false;
          if (item.editState) {
            if (item.materialCode.key == "") {
              // 验证物料代码
              item.materialCode.valid = true;
              item.materialName.valid = true;
              item.spec.valid = true;
              flag = false;
            }
            if (item.defaultWarehouseName.key == "") {
              // 验证物仓库ID
              item.defaultWarehouseName.valid = true;
              flag = false;
            }
            if (item.PaidIn.value == "" || isNaN(item.PaidIn.value)) {
              // 验证实收数量
              item.PaidIn.valid = true;
              flag = false;
            }
            if (item.PaidIn.value !== "" || parseInt(item.PaidIn.value)) {
              // 验证实收数量
              item.PaidIn.valid = true;
              flag = false;
            }
            // this.doItem.materialCode.key
            var _LogName = "";
            var param = {
              id: 0,
              mainId: this.dtData.id,
              materialId: parseFloat(item.materialCode.key) || "",
              warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
              actualNumber: parseFloat(item.PaidIn.value) || ""
            };
            // if (item.id) {
            //   param.id = item.id
            // }
            // _LogName += `物料:${item.materialName.value} 出库 ${item.PaidIn.value}${item.unit.value} 到 ${item.defaultWarehouseName.value}`
            _LogName += item.materialName.value;
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
              // 日期
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
            message: "请输出数据",
            type: "error"
          });
          return;
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
        };
      }

      var method = "";
      if (state == 1) {
        currData.id = 0;
        var data = RequestObject.CreatePostObject(currData);

        this.postData(data, state);
      }

      if (state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该入库单是通过状态，无法编辑");
          return;
        }
        currData.id = this.selectRow.id;
        var data = RequestObject.CreatePostObject(
          currData,
          null,
          this.PostDataEdit
        );
        this.$confirm("是否保存出库单？", "确认信息", {
          type: "warning"
        })
          .then(() => {
            this.postData(data, state);
          })
          .catch(() => {});
      }
      if (state == 3) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error("该入库单是通过状态，无法删除");
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
        url: "/warehouse/api/TWMOtherWhSendMain",
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
          }
          if (state == 3) {
            this.tableData = [];
            this.resetData();
            // this.setTable(30)
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

    getMaterielData(item) {
      // 获取物料档案信息
      this.materielData = [];
      this.totalCount = 0;

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
    rowClick(row) {
      for (var i = 0; i < this.tableData.length; i++) {
        if (
          this.tableData[i].materialName.value == row.materialName &&
          this.tableData[i].defaultWarehouseName.key ==
            this.doItem.defaultWarehouseName.key
        ) {
          if (this.doItem.id == this.tableData[i].id) {
            // break
            continue;
          }
          this.tableData[i].materialCode.isShow = true;
          this.doItem.materialCode.isShow = false;
          this.doItem.materialCode.value = "";
          this.doItem.materialCode.key = "";
          this.doItem.materialName.value = "";
          this.doItem.spec.value = "";
          this.doItem.warehouseUnitName.value = "";
          this.doItem.unit.value = "";
          this.doItem.baseUnitNumber.value = "";

          this.doItem.PaidIn.value = "";
          this.doItem.amount.value = "";
          this.doItem.defaultWarehouseName.key = "";
          this.doItem.defaultWarehouseName.value = "";
          this.doItem.rowIndex = 0;
          this.doItem.spec.value = "";
          this.doItem.unit.value = "";
          this.doItem.unitPrice.value = "";
          this.doItem.validityPeriod.value = "";
          this.doItem.remark.value = "";
          this.doItem.warehouseSum.value = "";
          this.doItem.warehouseUnitName.value = "";
          this.listenClick();
          return;
        }
      }

      this.doItem.materialCode.value = row.materialCode;
      this.doItem.materialCode.key = row.id;
      this.doItem.materialName.value = row.materialName;
      this.doItem.spec.value = row.spec;

      this.doItem.warehouseUnitName.value = row.warehouseUnitName;
      this.doItem.unit.value = row.baseUnitName;
      this.doItem.baseUnitNumber.value = row.baseUnitNumber;
      if (this.doItem.defaultWarehouseName.value) {
        this.GetWarehouseNum(
          null,
          this.doItem.materialCode.key,
          this.doItem.defaultWarehouseName.key,
          this.doItem
        );
      }
      this.listenClick();
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
          if (index == 6||index == 7||index == 10) {
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

      this.doDefault(this.doItem);
      this.popoverState = false;
      item[name].isShow = true;
      this.doItem = item;
      this.doItemName = name;
      this.$nextTick(() => {
        if (document.getElementById(item.id + "___" + name)) {
          document.getElementById(item.id + "___" + name).focus();
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
      this.getMaterielData();
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
    PaidInBlur(item) {
      if (item.PaidIn.value == "" || item.PaidIn.value == null) return;
      item.PaidIn.value = parseFloat(item.PaidIn.value);
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
      this.$refs.InboundOrder.getMainList();
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    getMainList(id) {
      request({
        url: "warehouse/api/TWMOtherWhSendMain/GetWholeMainData",
        method: "GET",
        params: { requestObject: id }
      }).then(res => {
        if (res.code === 0) {
          this.OnBtnSaveSubmit(res.data, 1);
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        }
      });
    },
    OnBtnSaveSubmit(row, state) {
      // 根据ID获取数据
      this.selectRow = this.cloneObject(row);
      this.dtData.WarehousingType = row.whSendType;
      this.dtData.warehousingDate =
        row.warehousingDate != null
          ? new Date(row.whSendDate.split("T")[0])
          : "";
      this.warehousingDateForMat = row.whSendDate.split("T")[0];
      //  whSendDate
      this.dtData.warehousingOrder = row.whSendOrder;

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
        var QueryConditions = [
          {
            column: "id",
            content: row.id,
            condition: 0
          }
        ];
        var rqs = RequestObject.LonginBookObject(
          false,
          0,
          0,
          null,
          QueryConditions
        );
        this.fullscreenLoading = true;
        request({
          url: "warehouse/api/TWMOtherWhSendMain/GetWholeMainData",
          method: "GET",
          params: { requestobject: JSON.stringify(row.id) }
        }).then(res => {
          this.addShow = false;
          this.fullscreenLoading = false;
          if (res.code === 0) {
            this.dtData.WarehousingType = res.data.whSendType;
            this.dtData.warehousingDate =
              res.data.whSendDate != null
                ? new Date(res.data.whSendDate.split("T")[0])
                : "";
            this.dtData.warehousingOrder = res.data.whSendOrder;
            this.dtData.operatorId = res.data.operatorId;
            this.dtData.operatorName = res.data.operatorName;
            this.dtData.auditName = res.data.auditName;
            this.dtData.auditId = res.data.auditId;
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split("T")[0]
                : "";
            this.dtData.id = res.data.id;
            this.dtData.receiptName = res.data.receiptName;
            this.dtData.receiptId = res.data.receiptId;

            this.setTableData(res.data.childList);
            this.fullscreenLoading = false;
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
            this.fullscreenLoading = false;
          }
        });
      } else {
        this.setTableData(row.childList);
      }
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
          warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
          actualNumber: parseFloat(item.PaidIn.value) || ""
        };
        _LogName += `物料:${item.materialName.value} 出库 ${item.PaidIn.value}${item.unit.value} 到 ${item.defaultWarehouseName.value}`;
        if (item.batchNumber.value != "") {
          // 批号
          param.batchNumber = item.batchNumber.value;
        }
        // 数量
        if (item.PaidIn.value != "" && !isNaN(item.PaidIn.value)) {
          param.unitPrice = parseInt(item.PaidIn.value) || 0;
        }
        if (item.unitPrice.value != "" && !isNaN(item.unitPrice.value)) {
          // 单价parseInt
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
        whSendDate:
          this.selectRow.whSendDate != null || this.selectRow.whSendDate != ""
            ? this.selectRow.whSendDate.split("T")[0]
            : "",
        whSendOrder: this.selectRow.whSendOrder,
        auditStatus: this.selectRow.auditStatus,
        receiptId: this.selectRow.receiptId,
        whSendType: this.selectRow.whSendType,
        operatorId: this.selectRow.operatorId,
        childList: childList
      };

      this.PostDataEdit = currData;
    },
    setTableData(dt) {
      // 将数据导出table
      this.tableData = [];
      dt.map(item => {
        var list = {
          id: item.id,
          editState: false,
          rowIndex: 0,
          // 物料代码
          materialCode: {
            value: item.materialCode,
            key: item.materialId,
            isShow: false,
            valid: false
          },
          // 物料名称
          materialName: {
            value: item.materialName,
            isShow: false,
            valid: false
          },
          // 规格
          spec: {
            value: item.spec,
            isShow: false,
            valid: false
          },
          colorName: {
            value: item.colorName,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            // 出货仓库
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          warehouseSum: {
            // 可用量
            value: item.availableNum,
            isShow: false,
            valid: false
          },
          batchNumber: {
            value: item.batchNumber, // 批号
            isShow: false,
            valid: false
          },
          unit: {
            // 单位
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          // 仓库计量单位名称
          warehouseUnitName: {
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          // 基本单位数量
          baseUnitNumber: {
            value: item.baseUnitNumber,
            isShow: false,
            valid: false
          },
          // 入库仓
          PaidIn: {
            value: item.actualNumber,
            isShow: false,
            valid: false
          },
          // 价格
          unitPrice: {
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          // 金额
          amount: {
            value: item.amount,
            isShow: false,
            valid: false
          },
          remark: {
            value: item.remark,
            isShow: false,
            valid: false
          },
          // 有效期
          validityPeriod: {
            value:
              item.validityPeriod != null
                ? new Date(item.validityPeriod.split("T")[0])
                : "",
            isShow: false,
            valid: false
          }
        };
        this.warehouseData.map(data => {
          if (list.defaultWarehouseName.key == data.id) {
            list.defaultWarehouseName.value = data.warehouseName;
          }
        });
        this.tableData.push(list);
      });
      this.cloneTable = [...this.tableData];
      this.setCurrData(this.cloneTable);
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
      // if (this.selectRow.auditStatus == 2) {
      //   this.$message.error('该出库单已经为通过状态')
      //   return
      // }
      var postData = {
        id: this.selectRow.id,
        auditStatus: state
      };
      var postDataEdit = {
        id: this.selectRow.id,
        auditStatus: this.dtData.auditStatus
      };
      var data = RequestObject.CreatePostObject(
        postData,
        null,
        postDataEdit,
        null
      );
      this.fullscreenLoading = true;
      request({
        url: "/warehouse/api/TWMOtherWhSendMain/OtherWhAuditAsync",
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
#ProcurementPut /deep/ {
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
    border-left: 0px solid #ccc;
    height: 32px;
    vertical-align: middle;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
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
  .falseIptop {
    width: 200px;
    overflow: hidden;
    border-bottom: 1px solid #dcdfe6;
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
