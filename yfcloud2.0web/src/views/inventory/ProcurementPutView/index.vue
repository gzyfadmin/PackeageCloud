<template>
  <el-container
    id="ProcurementPut"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <div class="btnHeader" id="btnHeader">
      <h1>入库单</h1>
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
        <el-form-item class="formItem" label="入库类型：" label-width="76px">
          <div>
            <div v-if="dtData.WarehousingType==0">采购入库</div>
            <div v-if="dtData.WarehousingType==1">生产入库</div>
            <div v-if="dtData.WarehousingType==2">委外入库</div>
            <div v-if="dtData.WarehousingType==3">其他入库</div>
            <div v-if="dtData.WarehousingType==4">盘盈入库</div>
          </div>
        </el-form-item>
        <el-form-item class="formItem" label="入库日期：" label-width="76px">
          <div>{{warehousingDateForMat}}</div>
        </el-form-item>
        <el-form-item class="formItem" label="编号：" label-width="48px">
          <div style="height:32px;">{{dtData.warehousingOrder}}</div>
        </el-form-item>
        <el-form-item class="formItem" label="入库状态：" label-width="76px">
          <div style="height:32px;">
            <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待入库</span>
            <span v-if="dtData.auditStatus==0">入库待审核</span>
            <span v-if="dtData.auditStatus==1">审核未通过</span>
            <span v-if="dtData.auditStatus==2">入库完成</span>
          </div>
        </el-form-item>
      </el-form>
    </el-header>
    <el-main>
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
                <span>{{scope.$index+1}}</span>
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
              <div>
                <div class="tableTd">{{scope.row.materialCode.value}}</div>
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
              <div>
                <div class="tableTd">{{scope.row.materialName.value}}</div>
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
                <div class="tableTd">{{scope.row.spec.value}}</div>
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
          <el-table-column prop="defaultWarehouseName" label="收料仓库">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>收料仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{scope.row.defaultWarehouseName.value}}</div>
              </div>
            </template>
          </el-table-column>
          
          <el-table-column prop="PaidIn" label="实收数量">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>实收数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{scope.row.PaidIn.value}}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.warehouseUnitName.value}}</div>
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
                <div class="tableTd">{{scope.row.unitPrice.value}}</div>
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
                <div class="tableTd">{{scope.row.amount.value}}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="batchNumber" label="批号">
            <template slot="header">
              <span class="tableHeader">
                <span>批号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{scope.row.batchNumber.value}}</div>
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
              <div class="tableTd">{{scope.row.unit.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="baseUnitNumber" label="基本单位数量" width="100">
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.baseUnitNumber.value}}</div>
            </template>
          </el-table-column>-->

          <el-table-column prop="defaultWarehouseName" label="有效期" width="145">
            <template slot="header">
              <span class="tableHeader">
                <span>有效期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{scope.row.validityPeriod.value | formatTDate}}</div>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div id="elfooter">
      <el-form label-width="76px" class="FormInput" inline>
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{dtData.operatorName}}</div>
          </div>
        </el-form-item>
        <el-form-item label="收货员：">
          <div class="falseIp">{{dtData.receiptName}}</div>
          <!-- <el-select
            v-if="selectRow.auditStatus != 2"
            v-model="dtData.receiptId"
            clearable
            filterable
            placeholder="请选择"
            style="width:300px;"
          >-->
          <!-- <el-option
              v-for="item in realNameOptions"
              :key="item.id"
              :label="item.realName"
              :value="item.id"
            ></el-option>
          </el-select>-->
        </el-form-item>
        <el-form-item label="审核人：">
          <div class="falseIp">{{dtData.auditName}}</div>
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
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { formatDate, keepTwoDecimalFull, accMul } from "@/utils/common.js";
import { newGuid } from "@/utils/guid";

import Pagination from "@/components/Pagination";
import { setTimeout } from "timers";
import { getBtnCtr } from "@/utils/BtnControl";
import { closest } from "@/utils/Dom";
import BigNumber from "bignumber.js";

export default {
  name: "KCHQtk",
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
          { required: true, message: "请选择入库类型", trigger: "change" }
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
      selectRow: {},
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: "",
      materielData: [], //物料信息
      warehouseData: [], //仓库信息
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
          }
        }
      ],
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      btnAip: "", //按钮权限
      OtherData: {},
      realNameOptions: [], //收货员列表
      addControl: true, //是否显示新增按钮
      warehousingDateForMat: "",
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
  components: {
    Pagination
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
  mounted() {
    this.runing();
  },
  activated() {
    if (this.atob(this.$route.query.id) == this.OtherData.id) {
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
      this.warehouseData = await this.getWarehouseData(); //仓库数据
      if (this.$route.query.id) {
        this.fullscreenLoading = true;
        this.OtherData = {
          id: this.atob(this.$route.query.id)
        };
        this.getMainList(this.OtherData.id);
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
              return BigNumber(prev)
                .plus(curr)
                .toNumber();
            } else {
              return prev;
            }
          }, 0);
          if (index == 6 || index == 9) {
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
      //改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMaterielData();
    },
    defaultData(data) {
      data.map(item => {
        for (var i in item) {
          this.$set(item, i + "_YF_Edit", false);
        }
      });
      return data;
    },
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    getMainList(id) {
      request({
        url: "warehouse/api/TWMOtherWhMain/GetWarehouseOrder",
        method: "GET",
        params: { id }
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
      //根据ID获取数据
      this.selectRow = this.cloneObject(row);
      this.dtData.WarehousingType = row.warehousingType;
      this.dtData.warehousingDate =
        row.warehousingDate != null
          ? new Date(row.warehousingDate.split("T")[0])
          : "";
      this.warehousingDateForMat = row.warehousingDate.split("T")[0];
      this.dtData.warehousingOrder = row.warehousingOrder;
      // console.log(row);

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
        var rqs = RequestObject.LonginBookObject(false, 0, 0, row.id);
        this.fullscreenLoading = true;
        request({
          url: "warehouse/api/TWMOtherWhMain/GetDetailList",
          method: "GET",
          params: { RequestObject: JSON.stringify(rqs) }
        }).then(res => {
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 200);
          if (res.code === 0) {
            this.setTableData(res.data);
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          }
        });
      } else {
        this.setTableData(row.childList);
      }
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
          warehouseId: parseFloat(item.defaultWarehouseName.key) || "",
          actualNumber: parseFloat(item.PaidIn.value) || ""
        };
        _LogName += `物料:${item.materialName.value} 入库 ${item.PaidIn.value}${item.unit.value} 到 ${item.defaultWarehouseName.value}`;
        if (item.batchNumber.value !== "") {
          //批号
          param.batchNumber = item.batchNumber.value;
        }
        if (item.unitPrice.value !== "" && !isNaN(item.unitPrice.value)) {
          //单价
          param.unitPrice = parseFloat(item.unitPrice.value) || 0;
        }
        if (item.amount.value !== "" && !isNaN(item.amount.value)) {
          //金额
          param.amount = parseFloat(item.amount.value) || 0;
        }
        if (
          item.validityPeriod.value !== "" &&
          !isNaN(item.validityPeriod.value)
        ) {
          //金额
          param.validityPeriod = formatDate(item.validityPeriod.value) + "";
        }

        param._LogName = _LogName;
        childList.push(param);
      });
      var currData = {
        id: this.selectRow.id,
        warehousingType: this.selectRow.warehousingType,
        warehousingDate:
          this.selectRow.warehousingDate != null ||
          this.selectRow.warehousingDate != ""
            ? this.selectRow.warehousingDate.split("T")[0]
            : "",
        warehousingOrder: this.selectRow.warehousingOrder,
        childList: childList
      };

      this.PostDataEdit = currData;
    },
    setTableData(dt) {
      //将数据导入table
      this.tableData = [];
      dt.map(item => {
        var list = {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
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
            value: item.colorName,
            isShow: false,
            valid: false
          },
          defaultWarehouseName: {
            //收料仓库
            value: item.defaultWarehouseName,
            key: item.warehouseId,
            isShow: false,
            valid: false
          },
          batchNumber: {
            value: item.batchNumber, //批号
            isShow: false,
            valid: false
          },
          unit: {
            //单位
            value: item.baseUnitName,
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            value: item.warehouseUnitName,
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            value: item.baseUnitNumber,
            isShow: false,
            valid: false
          },
          PaidIn: {
            value: item.actualNumber,
            isShow: false,
            valid: false
          },
          unitPrice: {
            value: item.unitPrice,
            isShow: false,
            valid: false
          },
          amount: {
            value: item.amount,
            isShow: false,
            valid: false
          },
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
    margin-right: 60px;
  }
  .el-form-item__label {
    padding-right: 4px;
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
