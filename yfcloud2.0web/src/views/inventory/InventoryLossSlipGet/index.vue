<template>
  <el-container
    id="OverflowOrder"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <!-- 按钮区 -->
    <div class="btnHeader" id="btnHeader">
      <h1>盘亏出库单</h1>
    </div>
    <el-header id="elheader" class="header" height="auto">
      <el-form
        ref="dtData"
        :model="dtData"
        label-width="76px"
        class="FormInput"
        inline
        label-position="left"
      >
        <el-form-item class="formItem" label="出库类型：">
          <div>
            <div v-if="dtData.WarehousingType==4">盘亏出库</div>
          </div>
        </el-form-item>
        <el-form-item class="formItem" label="出库日期：">
          <div>{{warehousingDateForMat}}</div>
        </el-form-item>
        <el-form-item class="formItem" label="编号：" label-width="48px">
          <div style="height:32px;">{{dtData.warehousingOrder}}</div>
        </el-form-item>
        <el-form-item class="formItem" label="出库状态：" label-width="76px">
          <div style="height:32px;">
            <span v-if="dtData.auditStatus!=0&&dtData.auditStatus!=1&&dtData.auditStatus!=2">待出库</span>
            <span v-if="dtData.auditStatus==0">出库待审核</span>
            <span v-if="dtData.auditStatus==1">审核未通过</span>
            <span v-if="dtData.auditStatus==2">出库完成</span>
          </div>
        </el-form-item>
      </el-form>
    </el-header>
    <!-- 列表区 -->
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
          <el-table-column prop="defaultWarehouseName" label="发货仓库">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>发货仓库</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{scope.row.defaultWarehouseName.value}}</div>
              </div>
            </template>
          </el-table-column>
          <el-table-column prop="PaidIn" label="实存数量">
            <template slot="header">
              <span class="tableHeader">
                <!-- <span class="start">*</span> -->
                <span>实存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div>
                <div class="tableTd">{{scope.row.PaidIn.value}}</div>
              </div>
            </template>
          </el-table-column>

          <el-table-column prop="overFlow" label="盘亏数量">
            <template slot="header">
              <span class="tableHeader">
                <span>盘亏数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.overFlow.value}}</div>
            </template>
          </el-table-column>

          <el-table-column prop="account" label="账存数量">
            <template slot="header">
              <span class="tableHeader">
                <span>账存数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.account.value}}</div>
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

          <!-- <el-table-column prop="warehouseUnitName" label="仓库单位">
            <template slot="header">
              <span class="tableHeader">
                <span>仓库单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="tableTd">{{scope.row.warehouseUnitName.value}}</div>
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
    <!-- 页脚区 -->
    <div id="elfooter">
      <el-form label-width="76px" class="FormInput" inline>
        <el-form-item label="制单人：">
          <div>
            <div class="falseIp">{{dtData.operatorName}}</div>
          </div>
        </el-form-item>
        <el-form-item label="盘点人员：">
          <div class="falseIp">{{dtData.receiptName}}</div>
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
  name: "YKCKD",
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
        WarehousingType: 4,
        warehousingDate: "",
        warehousingOrder: "",
        operatorName: "",
        operatorId: "",
        receiptName: "",
        receiptId: "",
        auditName: "",
        auditId: "",
        auditStatus: -1,
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
            //收料仓库
            value: "",
            key: "",
            isShow: false,
            valid: false
          },
          batchNumber: {
            //批号
            value: "",
            isShow: false,
            valid: false
          },
          unit: {
            //基本单位
            value: "",
            isShow: false,
            valid: false
          },
          warehouseUnitName: {
            //仓库单位
            value: "",
            isShow: false,
            valid: false
          },
          baseUnitNumber: {
            //基本单位数量
            value: "",
            isShow: false,
            valid: false
          },
          PaidIn: {
            //实存数量
            value: "",
            isShow: false,
            valid: false
          },
          overFlow: {
            //盘亏数量
            value: "",
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
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
          amount: {
            //金额
            value: "",
            isShow: false,
            valid: false
          },
          validityPeriod: {
            //有效期
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
    Pagination
    // InboundOrder
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
  created() {
    // this.setStyle(); //设置样式
    //获取按钮权限
    // console.log(this.$route.params.type)
    // this.btnAip = getBtnCtr(
    //   this.$route.path,
    //   this.$store.getters.userPermission
    // );
  },
  mounted() {
    this.runing()
  },
  activated() {
    if(this.atob(this.$route.query.id)==this.OtherData.id){
      return;
    }
    this.runing()
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
        // this.OtherData = this.$route.params;
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
    resetData() {
      //初始化数据
      this.$refs["dtData"].resetFields();
      this.dtData.operatorName = this.$store.state.user.name;
      this.getCode();
      this.tableData = [];
      this.setTable(30);
      this.PostDataEdit = {};
      this.cloneTable = [];
      this.selectRow = [];

      this.dtData.WarehousingType = 4;
      this.dtData.warehousingDate = new Date();
      this.dtData.warehousingOrder = "";
      this.dtData.remark = "";
      this.dtData.operatorName = this.$store.state.user.name;
      this.dtData.operatorId = "";
      this.dtData.receiptName = "";
      this.dtData.receiptId = "";
      this.dtData.auditName = "";
      this.dtData.auditId = "";
      this.dtData.auditStatus = -1;
      this.dtData.auditTime = "";
    },
    setTable(num) {
      //初始化30条数据
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
        this.tableData.push(list);
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
              return BigNumber(prev)
                .plus(curr)
                .toNumber();
            } else {
              return prev;
            }
          }, 0);
          if (index == 6 || index == 7 || index == 8|| index == 10) {
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
    cloneObject(origin) {
      return Object.assign({}, origin);
    },
    getMainList(id) {
      request({
        url: "warehouse/api/TWMDeficitMain/GetWholeMainData",
        method: "GET",
        params: { RequestObject: id }
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
      this.dtData.WarehousingType = row.whSendType;
      this.dtData.warehousingDate =
        row.whSendDate != null ? new Date(row.whSendDate.split("T")[0]) : "";
      this.warehousingDateForMat = row.whSendDate.split("T")[0];
      this.dtData.warehousingOrder = row.whSendOrder;
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
        // var rqs = RequestObject.LonginBookObject(false, 0, 0, row.id);
        this.fullscreenLoading = true;
        request({
          url: "warehouse/api/TWMDeficitMain/GetWholeMainData",
          method: "GET",
          params: { RequestObject: row.id }
        }).then(res => {
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 200);
          if (res.code === 0) {
            this.setTableData(res.data.childList);
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
          actualNumber:
            parseFloat(item.account.value) - parseFloat(item.PaidIn.value) ||
            "",
          accountNum: item.account.value || ""
        };
        _LogName += `物料:${item.materialName.value} 盘亏出库 ${param.actualNumber}${item.unit.value} 到 ${item.defaultWarehouseName.value}`;

        if (item.batchNumber.value !== "" && item.batchNumber.value !== null) {
          //批号
          param.batchNumber = item.batchNumber.value;
        }
        if (item.unitPrice.value !== "" && item.unitPrice.value !== null) {
          //单价
          param.unitPrice = parseFloat(item.unitPrice.value) || 0;
        }
        if (item.amount.value !== "" && item.amount.value !== null) {
          //金额
          param.amount = parseFloat(item.amount.value) || 0;
        }
        if (
          item.validityPeriod.value !== "" &&
          item.validityPeriod.value !== null
        ) {
          //日期
          param.validityPeriod = formatDate(item.validityPeriod.value) + "";
        }

        param._LogName = _LogName;
        childList.push(param);
      });
      var currData = {
        id: this.selectRow.id,
        whSendType: this.selectRow.whSendType,
        whSendDate:
          this.selectRow.whSendDate != null || this.selectRow.whSendDate != ""
            ? this.selectRow.whSendDate.split("T")[0]
            : "",
        whSendOrder: this.selectRow.whSendOrder,
        childList: childList
      };
      this.PostDataEdit = currData;
    },
    setTableData(dt) {
      //将数据导出table
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
            //实存数量
            value: parseFloat(item.accountNum) - parseFloat(item.actualNumber),
            isShow: false,
            valid: false
          },
          overFlow: {
            //盘亏数量
            value: item.actualNumber,
            isShow: false,
            valid: false
          },
          account: {
            //账存数量
            value: item.accountNum,
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
          },
          remark: {
            value: item.remark,
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
      }, 200);
    }
  }
};
</script>
<style lang="scss" scoped>
#OverflowOrder /deep/ {
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
