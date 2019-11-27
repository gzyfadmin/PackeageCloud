<template>
  <el-container
    id="OverflowOrder"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <!-- 按钮区 -->
    <div class="btnHeader" id="btnHeader">
      <h1>采购入库单</h1>
    </div>
    <el-header id="elheader" class="header fromCenter" height="auto">
      <div class="middleWidth">
        <el-form
          ref="dtData"
          :model="dtData"
          label-width="76px"
          class="FormInput"
          inline
          label-position="right"
        >
          <!-- <el-form-item class="formItem disableType" label="供应商：" label-width="62px">
            <div class="headerIp">{{ dtData.customerName }}</div>
          </el-form-item>-->

          <el-form-item class="formItem" label="源单类型：">
            <div class="headerIp">
              <span>采购订单</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="单据号：" label-width="62px" prop="orderNo">
            <div class="headerIp">{{ dtData.orderNo }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="状态：" label-width="48px">
            <div class="headerIp">
              <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待入库</span>
              <span v-if="dtData.auditStatus==0">入库待审核</span>
              <span v-if="dtData.auditStatus==1">审核未通过</span>
              <span v-if="dtData.auditStatus==2">入库完成</span>
            </div>
          </el-form-item>

          <el-form-item class="formItem" label="编号：" label-width="48px" prop="warehousingOrder">
            <div class="headerIp">{{ dtData.warehousingOrder }}</div>
          </el-form-item>

          <el-form-item class="formItem" label="入库日期：" prop="warehousingDate">
            <div class="headerIp">{{ dtData.warehousingDate }}</div>
          </el-form-item>

          <!-- <el-form-item class="formItem" label="采购方式：">
            <el-select v-model="dtData.purchasingMethod" placeholder="请选择" style="width:200px">
              <el-option label="现购" :value="0" />
              <el-option label="赊购" :value="1" />
            </el-select>
          </el-form-item>-->

          <!-- <el-form-item class="formItem" label="收货地址：">
            <el-input class="headerIp" v-model="dtData.receiptAddress" style="width:510px;"></el-input>
          </el-form-item>-->
        </el-form>
      </div>
    </el-header>
    <!-- 列表区 -->
    <el-main>
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
                  <!-- <i class="el-icon-setting" /> -->
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="materialCode" label="物料代码">
            <template slot="header">
              <span class="tableHeader">
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.materialCode }}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="materialName" label="物料名称">
            <template slot="header">
              <span class="tableHeader">
                <span>物料名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.materialName }}</div>
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
              <div>
                <div class="tableTd">{{ scope.row.spec }}</div>
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
                <div class="tableTd">{{ scope.row.colorName }}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="warehouseName" label="收料仓库">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>收料仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.warehouseName }}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="actualNum" label="实收数量">
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>实收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.actualNum }}</div>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="shouldNum" label="可收数量">
            <template slot="header">
              <span class="tableHeader">
                <span>可收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.shouldNum }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="missNum" label="欠料数量">
            <template slot="header">
              <span class="tableHeader">
                <span>欠料数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.missNum }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.warehouseUnitName }}</div>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="unitPrice" label="单价">
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.unitPrice }}</div>
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
              <div>
                <div class="tableTd">{{ scope.row.amount }}</div>
              </div>
            </template>
          </el-table-column>
          

          <!-- <el-table-column prop="orderNo" label="源单单号" width="150">
            <template slot="header">
              <span class="tableHeader">
                <span>源单单号</span>
              </span>
            </template>
            <div class="tableTd">{{ dtData.orderNo }}</div>
          </el-table-column>-->

          <el-table-column prop="baseUnitName" label="基本单位名称" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitName }}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitShouldNum" label="基本单位可收数量">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位可收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitShouldNum }}</div>
            </template>
          </el-table-column>

          <!-- <el-table-column prop="baseUnitActualNum" label="基本单位实收数量">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位实收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{ scope.row.baseUnitActualNum }}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="deliveryPeriod" label="有效期至" width="145">
            <template slot="header">
              <span class="tableHeader">
                <span>有效期至</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{ scope.row.deliveryPeriod | formatTDate }}</div>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="remark" label="备注" width="280">
            <template slot="header">
              <span class="tableHeader">
                <span>备注</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">
                  <el-popover
                    v-if="scope.row.remark!=null&&scope.row.remark.length>=20"
                    placement="top-start"
                    trigger="hover"
                    :content="scope.row.remark"
                  >
                    <div class="ellipsis" slot="reference">{{ scope.row.remark }}</div>
                  </el-popover>
                  <div
                    class="ellipsis"
                    v-if="scope.row.remark!=null&&scope.row.remark.length<20"
                  >{{scope.row.remark}}</div>
                </div>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <!-- 页脚区 -->
    <div id="elfooter">
      <el-form label-width="76px" class="FormInput" inline>
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{ dtData.operatorName }}</div>
          </div>
        </el-form-item>
        <el-form-item label="仓管员：">
          <div class="falseIp">{{ dtData.whAdminName }}</div>
        </el-form-item>
        <el-form-item label="发货员：">
          <div class="falseIp">{{ dtData.receiptName }}</div>
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
    <!-- <InboundOrder ref="InboundOrder" @OnBtnSaveSubmit="OnBtnSaveSubmit"></InboundOrder> -->
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";
// import InboundOrder from "./components/InboundOrder";

import Pagination from "@/components/Pagination";
import BigNumber from "bignumber.js";
// import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";

export default {
  name: "CGRKD",
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
    // InboundOrder
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
        customerName: "",
        auditStatus: "",
        warehousingOrder: "",
        warehousingDate: "",
        orderNo: "",
        operatorName: "",
        whAdminName: "",
        receiptName: "",
        auditName: "",
        auditStatus: "",
        auditTime: ""
      },
      tableHeight: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: {},
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], // 物料信息
      warehouseData: [], // 仓库信息
      tableData: [], // table数据模型
      tableData2: [],
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      btnAip: "", // 按钮权限
      OtherData: {},
      realNameOptions: [], // 收货员列表
      addControl: true, // 是否显示新增按钮
      warehousingDateForMat: ""
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

      this.setStyle(); // 设置样式
      this.warehouseData = await this.getWarehouseData(); // 仓库数据
      if (this.$route.query.id) {
        // this.OtherData = this.$route.params;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.OnBtnSaveSubmit(this.OtherData);
      }

      this.mountedState = false;
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
      // 初始化30条数据
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
        this.tableData.push(list);
      }
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
          return Number(item[column.property]);
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
          if (
            index == 6 ||
            index == 11 
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
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMaterielData();
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    OnBtnSaveSubmit(row, state) {
      // 根据ID获取数据
      // this.selectRow = this.cloneObject(row)
      if (!state) {
        // var rqs = RequestObject.LonginBookObject(false, 0, 0, row.id);
        this.fullscreenLoading = true;
        request({
          url: "/warehouse/api/TWMPurchaseMain/GetWholeMainData",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 200);
          if (res.code === 0) {
            this.setTableData(res.data.childList);
            this.dtData.customerName = res.data.supplierName;
            this.dtData.auditStatus = res.data.auditStatus;
            this.dtData.warehousingOrder = res.data.warehousingOrderNo;
            this.dtData.warehousingDate =
              res.data.warehousingDate != null
                ? res.data.warehousingDate.split("T")[0]
                : "";
            this.dtData.orderNo = res.data.purchaseOrder;

            this.dtData.operatorName = res.data.operatorName;
            this.dtData.whAdminName = res.data.whAdminName;

            this.dtData.receiptName = res.data.receiptName;

            this.dtData.auditName = res.data.auditName;
            this.dtData.auditTime =
              res.data.auditTime != null
                ? res.data.auditTime.split("T")[0]
                : "";
            // this.tableData = res.data.childList;
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          }
        });
      }
    },
    setTableData(dt, row) {
      // 将数据导入table
      this.tableData = [];
      var listArr = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: item.materialCode, // 物料代码
          materialName: item.materialName, // 物料名称
          spec: item.spec, // 规格型号
          colorName: item.colorName,//颜色
          warehouseName: "", // 收料仓库
          warehouseId: item.warehouseId, // 收料仓库Id
          actualNum: item.actualNum, //实收数量
          shouldNum: item.shouldNum, //应发数量
          missNum:item.missNum,//欠料数量
          unitPrice: item.unitPrice, // 单价
          amount: item.amount, // 金额
          warehouseUnitName: item.warehouseUnitName, // 单位
          orderNo: this.dtData.orderNo, // 源单单号
          baseUnitName: item.baseUnitName, // 基本单位名称
          baseUnitShouldNum: item.baseUnitShouldNum, // 基本单位应收数量
          baseUnitActualNum: item.baseUnitActualNum, // 基本单位实收数量
          deliveryPeriod:
            item.deliveryPeriod != null
              ? item.deliveryPeriod.split("T")[0]
              : "", //有效期至
          remark: item.remark // 备注
        };
        this.warehouseData.map(data => {
          if (list.warehouseId == data.id) {
            list.warehouseName = data.warehouseName;
          }
        });
        listArr.push(list);
      });
      this.cloneTable = [...listArr];
      // this.setCurrData(this.cloneTable);
      setTimeout(() => {
        this.tableData = [...listArr];
      }, 0);
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 200);
    }
  }
};
</script>
<style lang="scss" scoped>
#OverflowOrder /deep/ {
  .el-table {
    overflow: visible !important;
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
    border-bottom: 1px solid #c0c4cc;
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
    // border-left: 1px solid #ccc;
    position: relative;
    left: -1px;
    height: 32px;
    vertical-align: middle;
    // border-top-left-radius: 0px;
    // border-bottom-left-radius: 0px;
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
    color: #606266;
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
  .el-main {
    padding-bottom: 10px;
  }
}
@import "@/styles/receipts.scss";
</style>
