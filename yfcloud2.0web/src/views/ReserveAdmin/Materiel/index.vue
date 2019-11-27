<template>
  <el-container
    v-loading="loading"
     element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header
      id="elheader"
      class="headerBd"
      style="height:auto"
    >
      <el-form inline>
        <el-form-item
          label="物料代码："
          label-width="76px"
        >
          <el-input
            v-model="numSearch"
            placeholder="请输入物料代码"
            clearable
            @clear="getMakeReport()"
            @keyup.enter.native="getMakeReport()"
          >
            <i
              slot="suffix"
              class="el-icon-search"
              @click="getMakeReport()"
            />
          </el-input>
        </el-form-item>
        <el-form-item
          label="物料名称："
          label-width="76px"
        >
          <el-input
            v-model="nameSearch"
            placeholder="请输入物料名称"
            clearable
            @clear="getMakeReport()"
            @keyup.enter.native="getMakeReport()"
          >
            <i
              slot="suffix"
              class="el-icon-search"
              @click="getMakeReport()"
            />
          </el-input>
        </el-form-item>
      </el-form>
      <el-button
        v-if="btnAip.query&&btnAip.query.buttonCaption"
        type="primary"
        @click="getMakeReport"
      >{{ btnAip.query.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.add&&btnAip.add.buttonCaption"
        type="primary"
        @click="handelAddClick"
      >{{ btnAip.add.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.batchdelete&&btnAip.batchdelete.buttonCaption"
        :type="typeColor"
        @click="handelBatchDelete"
      >{{ btnAip.batchdelete.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.copy&&btnAip.copy.buttonCaption"
        @click="handelCopyClick"
      >{{ btnAip.copy.buttonCaption }}</el-button>
      <!-- <el-button type="primary" @click="handelAddClick">新增</el-button>
      <el-button type="info" @click="handelBatchDelete">批量删除</el-button> -->
    </el-header>
    <el-main class="wmltable" id="elmain">
      <el-table
        align="center"
        :data="materielData"
        border
        :height="mainHeight + 'px'"
        @selection-change="handleSelectionChange"
        @sort-change="handelSortChange"
      >
        <el-table-column
          type="selection"
          width="50"
        />
        <el-table-column
          prop="materialCode"
          label="物料代码"
          width="150"
          sortable='custom'
        />
        <el-table-column
          prop="materialName"
          label="物料名称"
          width="150"
          sortable='custom'
        />
        <el-table-column
          prop="spec"
          label="规格型号"
        />
        <el-table-column
          prop="baseUnitName"
          label="基本计量单位"
          width="100"
        />
        <el-table-column
          prop="materialTypeName"
          label="物料分类"
        />
        <el-table-column
          prop="colorName"
          label="颜色"
        />
        <el-table-column
          prop="defaultWarehouseName"
          label="默认仓库"
        />
        <el-table-column
          prop="lowInventory"
          label="最低库存数量"
          width="100"
        />
        <el-table-column
          prop="highInventory"
          label="最高库存数量"
          width="100"
        />
        <el-table-column
          prop="shelfLife"
          label="保质期（天）"
          width="100"
        />
        <!-- <el-table-column
          prop="remark"
          label="备注"
        /> -->
        <el-table-column
          prop="remark"
          label="备注"
          width="280"
        >
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.remark&&scope.row.remark.length>=20"
              class="item"
              effect="light"
              :content="scope.row.remark"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.remark }}</div>
            </el-tooltip>
            <div
              v-if="scope.row.remark&&scope.row.remark.length<20"
              class="ellipsis"
            >{{ scope.row.remark }}</div>
          </template>
        </el-table-column>
        <el-table-column
          prop="url"
          label="网址"
          width="230"
        >
          <template slot-scope="scope">
            <div v-html="getUrl(scope.row.url)"></div>
          </template>
        </el-table-column>

        <el-table-column
          label="操作"
          fixed="right"
          width="105"
        >
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.edit&&btnAip.edit.buttonCaption"
              class="item"
              effect="light"
              content="编辑"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-edit"
                circle
                @click="handelEditClick(scope.row)"
              />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.delete&&btnAip.delete.buttonCaption&&showtips"
              class="item"
              effect="light"
              content="删除"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="danger"
                icon="el-icon-delete"
                circle
                @click="handelDelete(scope.row)"
              />
            </el-tooltip>
            <el-button
              v-if="btnAip.delete&&btnAip.delete.buttonCaption&&!showtips"
              type="danger"
              icon="el-icon-delete"
              circle
              @click="handelDelete(scope.row)"
            />
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <!-- 新增和编辑弹窗  -->
    <el-dialog
      :title="title"
      :visible.sync="materielVisible"
      width="40%"
      :close-on-click-modal="false"
    >
      <div
        class="usersTap"
        style=" padding: 0px 20px;"
      >
        <div>
          <div
            :class="tapNum==1? 'color':''"
            @click="tapNum=1"
          >
            基本信息
          </div>
        </div>
        <div>
          <div
            class="long-box"
            :class="tapNum==2? 'color':''"
            @click="tapNum=2"
          >
            计量单位组
          </div>
        </div>
        <div>
          <div
            class="long-box"
            :class="tapNum==3? 'color':''"
            @click="tapNum=3"
          >
            网址信息
          </div>
        </div>
      </div>
      <el-form
        ref="materielForm"
        :model="materielForm"
        :rules="materielRules"
      >
        <div
          v-show="tapNum==1"
          class="d_inline"
        >
          <el-form-item
            prop="materialCode"
            label="物料代码："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入物料代码" v-model="materielForm.materialCode" />
          </el-form-item>
          <el-form-item
            prop="materialName"
            label="物料名称："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入物料名称" v-model="materielForm.materialName" />
          </el-form-item>
          <el-form-item
            prop="spec"
            label="规格型号："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入规格型号" v-model="materielForm.spec" />
          </el-form-item>
          <el-form-item
            prop="colorId"
            label="颜色："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-select
              v-model="materielForm.colorId"
              filterable
              clearable
              placeholder="请选择"
              style="width:100%"
            >
              <el-option
                v-for="item in colorData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="materialTypeId"
            label="物料分类："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-select
              v-model="materielForm.materialTypeId"
              filterable
              clearable
              placeholder="请选择"
              style="width:100%"
            >
              <el-option
                v-for="item in classifyData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="defaultWarehouseId"
            label="默认仓库："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-select
              v-model="materielForm.defaultWarehouseId"
              filterable
              clearable
              placeholder="请选择"
              style="width:100%"
            >
              <el-option
                v-for="item in warehouseData"
                :key="item.id"
                :label="item.warehouseName"
                :value="item.id"
                :disabled="!item.status"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="lowInventory"
            label="最低库存数量："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入最低库存数量" v-model="materielForm.lowInventory" />
          </el-form-item>
          <el-form-item
            prop="highInventory"
            label="最高库存数量："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入最高库存数量" v-model="materielForm.highInventory" />
          </el-form-item>
          <el-form-item
            prop="shelfLife"
            label="保质期（天）："
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input-number
              v-model="materielForm.shelfLife"
              step-strictly
              :step="0.5"
              :min="0.5"
              :max="1000"
            />
          </el-form-item>
          <el-form-item
                prop="remark"
                label="备注："
                :label-width="formLabelWidth"
                class="item"
              >
                <el-input
                 placeholder="请输入备注"
                  v-model="materielForm.remark"
                  type="textarea"
                  :rows="3"
                />
              </el-form-item>
        </div>
        <div
          v-show="tapNum==2"
          class="d_inline"
        >
          <div style="width:100%">
            <el-form-item
              prop="baseUnitId"
              label="基本计量单位："
              label-width="125px"
              style="width:40%"
            >
              <el-select
                v-model="materielForm.baseUnitId"
                filterable
                clearable
                placeholder="请选择"
                style="width:100%"
                @change="baseUnitId"
              >
                <el-option
                  v-for="item in rulerData"
                  :key="item.id"
                  :label="item.dicValue"
                  :value="item.id"
                />
              </el-select>
            </el-form-item>
          </div>
          <el-form-item
            prop="produceUnitId"
            label="生产计量单位："
            label-width="125px"
            class="d_inline_40"
          >
            <el-select
              v-model="materielForm.produceUnitId"
              filterable
              clearable
              placeholder="请选择"
              @change="produceUnitId"
            >
              <el-option
                v-for="item in rulerData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="produceRate"
            label="与基本计量单位换算率："
            label-width="200px"
            class="d_inline_60"
          >
            <el-input
              v-model="materielForm.produceRate"
              type="number"
            />
          </el-form-item>
          <el-form-item
            prop="purchaseUnitId"
            label="采购计量单位："
            label-width="125px"
            class="d_inline_40"
          >
            <el-select
              v-model="materielForm.purchaseUnitId"
              filterable
              clearable
              placeholder="请选择"
              @change="purchaseUnitId"
            >
              <el-option
                v-for="item in rulerData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="purchaseRate"
            label="与基本计量单位换算率："
            label-width="200px"
            class="d_inline_60"
          >
            <el-input v-model="materielForm.purchaseRate" />
          </el-form-item>
          <el-form-item
            prop="salesUnitId"
            label="销售计量单位："
            label-width="125px"
            class="d_inline_40"
          >
            <el-select
              v-model="materielForm.salesUnitId"
              filterable
              clearable
              placeholder="请选择"
              @change="salesUnitId"
            >
              <el-option
                v-for="item in rulerData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="salesRate"
            label="与基本计量单位换算率："
            label-width="200px"
            class="d_inline_60"
          >
            <el-input v-model="materielForm.salesRate" />
          </el-form-item>
          <el-form-item
            prop="warehouseUnitId"
            label="库存计量单位："
            label-width="125px"
            class="d_inline_40"
          >
            <el-select
              v-model="materielForm.warehouseUnitId"
              filterable
              clearable
              placeholder="请选择"
              @change="warehouseUnitId"
            >
              <el-option
                v-for="item in rulerData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            prop="warehouseRate"
            label="与基本计量单位换算率："
            label-width="200px"
            class="d_inline_60"
          >
            <el-input v-model="materielForm.warehouseRate" />
          </el-form-item>
        </div>
        <div v-show="tapNum==3"
          class="d_inline">
          <div class="item">
            <div
              class="connectionBox"
              style="margin-bottom: 20px;"
            >
              <div class="connectionDiv">
                网址名称
              </div>
              <div class="connectionDiv">
                网址路径
              </div>
              <div class="fontBox" />
            </div>
            <div
              v-for="(item,index) in materielForm.childList"
              :key="item.key"
              class="connectionBox"
            >
              <el-form-item
                class="connectionDiv"
                :prop="'childList['+index+'].contactName'"
              >
                <el-input
                  v-model="item.contactName"
                  placeholder="网址名称"
                  clearable
                  :class="item.Name?'contactNumber':''"
                />
                <div
                  v-if="item.Name"
                  class="el-form-item__error"
                >请输入网址名称！</div>
              </el-form-item>
              <el-form-item
                class="connectionDiv"
                :prop="'childList['+index+'].contactNumber'"
              >
                <el-input
                  v-model="item.contactNumber"
                  placeholder="网址路径"
                  :class="item.Number?'contactNumber':''"
                  clearable
                />
                <div
                  v-if="item.Number"
                  class="el-form-item__error"
                >请输入网址路径！</div>
              </el-form-item>
              <div class="fontBox">
                <el-button
                  type="primary"
                  plain
                  @click="contactAdd(index)"
                >添加</el-button>
                <el-button
                  type="danger"
                  plain
                  @click="contactDelete(index)"
                >删除</el-button>
              </div>
              </div>
              </div>
        </div>
      </el-form>
      <div
        slot="footer"
        class="dialog-footer"
      >
        <el-button @click="materielVisible = false">关闭</el-button>
        <el-button
          type="primary"
          :loading="saveBtn"
          @click="handelSave"
        >保存</el-button>
      </div>
    </el-dialog>
    <!-- 分页 -->
    <el-footer
      id="elfooter"
      :height="footHeight"
    >
      <Pagination
        :page-index="pageIndex"
        :total-count="total"
        :page-size="pageSize"
        @pagination="pagination"
      />
      <!-- <Pagination :total="total" @pagination="pagination" /> -->
    </el-footer>
  </el-container>
</template>
<script>
import Pagination from "@/components/Pagination";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { getBtnCtr } from "@/utils/BtnControl";
import { formatDate, trim } from "@/utils/common.js";
import { getStyle } from "@/utils/Dom.js";

export default {
  name: "viewsReserveAdminMaterielindexvue",
  components: {
    Pagination
  },
  data() {
    var warehouseNum = (rule, value, callback) => {
      const reg = /^\d{1,}(\.\d{1,4})?$/;
      if (value) {
        if (reg.test(value)) {
          callback();
        } else {
          callback(new Error("请输入正数，最多含四位小数"));
        }
      } else {
        callback();
      }
    };
    var codeName = (rule, value, callback) => {
      const codeReg = /[\u4E00-\u9FA5]|[\uFE30-\uFFA0]/gi;
      if (!value) {
        return callback(new Error("请输入物料代码"));
      }
      if (codeReg.test(value)) {
        callback(new Error("不能输入汉字"));
      } else {
        callback();
      }
    };
    var salesName = (rule, value, callback) => {
      if (!value) {
        return callback(new Error("请输入基本计量单位"));
      } else {
        callback();
      }
    };
    var produceToName = (rule, value, callback) => {
      if (value == this.materielForm.baseUnitId && value != "") {
        // this.materielForm.produceRate = 1
        callback();
      } else {
        // this.materielForm.produceRate = ''
        callback();
      }
    };
    var produceName = (rule, value, callback) => {
      const reg = /^(-)?\d{1,3}(\.\d{1,2})?$/;
      if (this.materielForm.produceUnitId) {
        if (value) {
          if (reg.test(value)) {
            callback();
          } else {
            callback(new Error("请输入三位以内，两位小数的数"));
          }
        } else {
          callback(new Error("请输入换算率"));
        }
      } else {
        callback();
      }
    };
    var purchaseToName = (rule, value, callback) => {
      if (value == this.materielForm.baseUnitId && value != "") {
        // this.materielForm.purchaseRate = 1
        callback();
      } else {
        // this.materielForm.purchaseRate = ''
        callback();
      }
    };
    var purchaseName = (rule, value, callback) => {
      const reg = /^(-)?\d{1,3}(\.\d{1,2})?$/;
      if (this.materielForm.purchaseUnitId) {
        if (value) {
          if (reg.test(value)) {
            callback();
          } else {
            callback(new Error("请输入三位以内，两位小数的数"));
          }
        } else {
          callback(new Error("请输入换算率"));
        }
      } else {
        callback();
      }
    };
    var salesToName = (rule, value, callback) => {
      if (value == this.materielForm.baseUnitId && value != "") {
        // this.materielForm.salesRate = 1
        callback();
      } else {
        // this.materielForm.salesRate = ''
        callback();
      }
    };
    var salesRateName = (rule, value, callback) => {
      const reg = /^(-)?\d{1,3}(\.\d{1,2})?$/;
      if (this.materielForm.salesUnitId) {
        if (value) {
          if (reg.test(value)) {
            callback();
          } else {
            callback(new Error("请输入三位以内，两位小数的数"));
          }
        } else {
          callback(new Error("请输入换算率"));
        }
      } else {
        callback();
      }
    };
    var warehouseToName = (rule, value, callback) => {
      if (value == this.materielForm.baseUnitId && value != "") {
        this.materielForm.warehouseRate = 1;
        callback();
      } else {
        // this.materielForm.warehouseRate = ''
        callback();
      }
    };
    var warehouseName = (rule, value, callback) => {
      const reg = /^(-)?\d{1,3}(\.\d{1,2})?$/;
      if (this.materielForm.warehouseUnitId) {
        if (value) {
          if (reg.test(value)) {
            callback();
          } else {
            callback(new Error("请输入三位以内，两位小数的数"));
          }
        } else {
          callback(new Error("请输入换算率"));
        }
      } else {
        callback();
      }
    };
    return {
      sortColumn: '',
      sortOrder: '',
      showtab: false,
      loading: true,
      saveBtn:false,
      btnAip: "",
      typeColor: "info",
      total: 0,
      headHeight: "50px",
      footHeight: "50px",
      mainHeight: "",
      datePicker: "",
      nameSearch: "",
      numSearch: "",
      tapNum: 1,
      pageSize: 25,
      pageIndex: 1,
      materielVisible: false,
      method: "post",
      title: "新增物料档案",
      showtips: true,
      formLabelWidth: "120px",
      allSelectionChange: [],
      materielData: [],
      warehouseData: [],
      colorData: [],
      rulerData: [],
      classifyData: [],
      materielRules: {
        materialCode: [
          { required: true, validator: codeName, trigger: "blur" },
          {
            min: 1,
            max: 20,
            message: "最大允许输入20个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        materialName: [
          { required: true, message: "请输入物料名称", trigger: "blur" },
          {
            min: 1,
            max: 50,
            message: "最大允许输入50个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        materialTypeId: [
          { required: true, message: "请选择物料分类", trigger: "blur" }
        ],
        spec: [
          { required: false, message: "请输入规格型号", trigger: "blur" },
          {
            min: 1,
            max: 50,
            message: "最大允许输入50个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        lowInventory: [
          {
            required: false,
            validator: warehouseNum,
            trigger: ["blur", "change"]
          }
        ],
        highInventory: [
          {
            required: false,
            validator: warehouseNum,
            trigger: ["blur", "change"]
          }
        ],
        remark: [
          {
            required: false,
            message: "请输入备注",
            trigger: ["blur", "change"]
          },
          {
            min: 1,
            max: 500,
            message: "最大允许输入500个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ],
        baseUnitId: [
          {
            required: true,
            message: "请选择基本计量单位",
            trigger: "blur"
          }
        ],
        produceUnitId: [
          {
            required: false,
            validator: produceToName,
            trigger: ["blur", "change"]
          }
        ],
        produceRate: [
          { required: false, validator: produceName, trigger: "blur" }
        ],
        purchaseUnitId: [
          {
            required: false,
            validator: purchaseToName,
            trigger: ["blur", "change"]
          }
        ],
        purchaseRate: [
          { required: false, validator: purchaseName, trigger: "blur" }
        ],
        salesUnitId: [
          {
            required: false,
            validator: salesToName,
            trigger: ["blur", "change"]
          }
        ],
        salesRate: [
          { required: false, validator: salesRateName, trigger: "blur" }
        ],
        warehouseUnitId: [
          {
            required: false,
            validator: warehouseToName,
            trigger: ["blur", "change"]
          }
        ],
        warehouseRate: [
          { required: false, validator: warehouseName, trigger: "blur" }
        ]
      },
      postDataEdit: {},
      materielForm: {
        materialCode: "",
        materialName: "",
        spec: "",
        colorId: "",
        materialTypeId: "",
        defaultWarehouseId: "",
        shelfLife: 20,
        highInventory: "",
        lowInventory: "",
        baseUnitId: "",
        produceUnitId: "",
        produceRate: "",
        purchaseUnitId: "",
        purchaseRate: "",
        salesUnitId: "",
        salesRate: "",
        warehouseUnitId: "",
        warehouseRate: "",
        remark: "",
        id: 0,
        _LogName: "",
        NoCopy:[],
        childList: [
        {
          contactName: "",
          contactNumber: "",
          Number: false,
          Name:false
        }
      ]
      }
    };
  },
  watch: {
    materielData() {
      if (this.pageIndex > 1 && this.materielData.length == []) {
        this.pageIndex--;
        // 初始化表格数据
        this.getMaterielData();
      }
    },
    materielVisible(val) {
      if (val) {
        // setTimeout(() => {
        //   // this.pswOnly = false;
        // }, 500);
        this.getWarehouseData(); // 查看仓库档案
        this.getColor(); // 查看颜色
        this.getRuler(); // 查看基本计量单位
        this.getClassify(); // 查看物料分类
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.materielForm.resetFields();
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    // 键盘返回键
    // var lett = this
    // document.onkeydown = function(e) {
    //   var key = window.event.keyCode
    //   if (key == 13) {
    //     lett.getMakeReport()
    //   }
    // }
    
    this.getMaterielData(); // 查看物料档案
    this.setStyle(); // 设置页面样式
  },
  activated() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 30);
  },
  mounted() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 30);
  },
  methods: {
    /**
     * setStyle
     * 设置页面样式
     */
    setStyle() {
      this.$nextTick(() => {
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        // this.mainHeight = b - h - f - 40
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.mainHeight = b - h - f - parseInt(pt) - parseInt(pb);
      });
    },

    // 监听计量单位值的变化
    baseUnitId() {
      if (
        this.materielForm.produceUnitId == this.materielForm.baseUnitId &&
        this.materielForm.baseUnitId
      ) {
        this.materielForm.produceRate = 1;
      } else {
        this.materielForm.produceRate = "";
      }
      if (
        this.materielForm.purchaseUnitId == this.materielForm.baseUnitId &&
        this.materielForm.baseUnitId
      ) {
        this.materielForm.purchaseRate = 1;
      } else {
        this.materielForm.purchaseRate = "";
      }
      if (
        this.materielForm.salesUnitId == this.materielForm.baseUnitId &&
        this.materielForm.baseUnitId
      ) {
        this.materielForm.salesRate = 1;
      } else {
        this.materielForm.salesRate = "";
      }
      if (
        this.materielForm.warehouseUnitId == this.materielForm.baseUnitId &&
        this.materielForm.baseUnitId
      ) {
        this.materielForm.warehouseRate = 1;
      } else {
        this.materielForm.warehouseRate = "";
      }
    },
    produceUnitId() {
      if (
        this.materielForm.produceUnitId == this.materielForm.baseUnitId &&
        this.materielForm.produceUnitId
      ) {
        this.materielForm.produceRate = 1;
      } else {
        this.materielForm.produceRate = "";
      }
    },
    purchaseUnitId() {
      if (
        this.materielForm.purchaseUnitId == this.materielForm.baseUnitId &&
        this.materielForm.purchaseUnitId
      ) {
        this.materielForm.purchaseRate = 1;
      } else {
        this.materielForm.purchaseRate = "";
      }
    },
    salesUnitId() {
      if (
        this.materielForm.salesUnitId == this.materielForm.baseUnitId &&
        this.materielForm.salesUnitId
      ) {
        this.materielForm.salesRate = 1;
      } else {
        this.materielForm.salesRate = "";
      }
    },
    warehouseUnitId() {
      if (
        this.materielForm.warehouseUnitId == this.materielForm.baseUnitId &&
        this.materielForm.warehouseUnitId
      ) {
        this.materielForm.warehouseRate = 1;
      } else {
        this.materielForm.warehouseRate = "";
      }
    },
    // 监听计量单位值的变化

    /**
     * getWarehouseData
     * 查看仓库档案
     */
    getWarehouseData() {
      var reqObject = RequestObject.LonginBookObject(
        false,
        this.pageSize,
        this.pageIndex,
        null,
        null
      );
      request({
        url: "/basicset/api/TBMWarehouseFile/GetAll",
        method: "GET",
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
          this.warehouseData = res.data;
        }
      });
    },
    /**
     * getColor
     * 查看颜色
     */
    getColor() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "颜色档案",
          condition: 0
        }
      ];
      var requsets = {
        IsPaging: false,
        PageSize: this.pageSize,
        PageIndex: this.pageIndex,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.colorData = res.data;
        }
      });
    },
    /**
     * getRule
     * 查看基本计量单位
     */
    getRuler() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "计量单位",
          condition: 0
        }
      ];
      var requsets = {
        IsPaging: false,
        PageSize: this.pageSize,
        PageIndex: this.pageIndex,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.rulerData = res.data;
        }
      });
    },
    /**
     * getRule
     * 查看物料分类
     */
    getClassify() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "物料分类",
          condition: 0
        }
      ];
      var requsets = {
        IsPaging: false,
        PageSize: this.pageSize,
        PageIndex: this.pageIndex,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      };
      request({
        url: "/basicset/api/TBMDictionary",
        method: "GET",
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.classifyData = res.data;
        }
      });
    },
    handelSortChange(currSort) {
      this.sortColumn = currSort.prop
      this.sortOrder = currSort.order == 'ascending' ? 'asc' : 'desc'
      this.getMaterielData();
    },
    /**
     * getMaterielData
     * 查看物料档案
     */
    getMaterielData() {
      var reqObject;
      var queryData = [];
      var orderConditions = [];
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        })
      }else {
        orderConditions = [
          {
            column: "materialCode",
            condition: 'desc'
          },
          {
            column: "materialName",
            condition: 'desc'
          }
        ]
      }

      if (this.numSearch != "" || this.nameSearch != "") {
        if (this.numSearch != "") {
          queryData.push({
            column: "materialCode",
            content: trim(this.numSearch),
            condition: 6
          });
        }
        if (this.nameSearch != "") {
          queryData.push({
            column: "materialName",
            content: trim(this.nameSearch),
            condition: 6
          });
        }
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          queryData,
          orderConditions
        );
      } else {
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          null,
          orderConditions
        );
      }

      request({
        url: "/basicset/api/TBMMaterialFile/GetNoMemory",
        method: "GET",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      })
        .then(res => {
          this.loading = false;
          if (res.code == -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            this.materielData = res.data;
            this.total = res.totalNumber;
          }
        })
        .catch(error => {
          this.loading = false;
        });
    },
    /**
     * contactAdd
     * 添加网址
     */
    contactAdd(index) {
      var obj = {
        contactName: "",
        contactNumber: "",
        Number: false,
        Name: false,
        key: Date.now()
      };
      if (this.materielForm.childList.length < 3) {
        this.materielForm.childList.splice(index + 1, 0, obj);
      } else {
        this.$message.error("最多只能添加三个网址");
      }
    }
    /**
     * contactDelete
     * 删除网址
     */,
    contactDelete(index) {
      if (this.materielForm.childList.length !== 1) {
        this.materielForm.childList.splice(index, 1);
      }
    },
    /**
     * getUrl
     * 过滤网址
     */
    getUrl(value) {
      if(value!=null){
        var val = value.split(";");
        var value = "";
      for(var i=0;i<val.length-1;i++) {
          value+= val[i].split(",")[0]+": "+"<a style='color:#0080ff' href="+val[i].split(",")[1]+" target='_blank'>"+val[i].split(",")[1]+"</a><br/>";
        }
        return value 
      }else {
        return value 
      }
    },
    /**
     * getMakeReport
     * 搜索
     */
    getMakeReport() {
      // 条件查询（物料编码，物料名称）
      this.getMaterielData();
    },
    /**
     * handelAddClick
     * 新增物料档案
     */
    handelAddClick() {
      this.tapNum = 1;
      this.method = "POST";
      this.postDataEdit = null;
      this.title = "新增物料档案";
      this.materielForm.materialCode = "";
      this.materielForm.materialName = "";
      this.materielForm.spec = "";
      this.materielForm.colorId = "";
      this.materielForm.materialTypeId = "";
      this.materielForm.defaultWarehouseId = "";
      this.materielForm.shelfLife = 20;
      this.materielForm.highInventory = "";
      this.materielForm.lowInventory = "";
      this.materielForm.baseUnitId = "";
      this.materielForm.produceUnitId = "";
      this.materielForm.produceRate = "";
      this.materielForm.purchaseUnitId = "";
      this.materielForm.purchaseRate = "";
      this.materielForm.salesUnitId = "";
      this.materielForm.salesRate = "";
      this.materielForm.warehouseUnitId = "";
      this.materielForm.warehouseRate = "";
      this.materielForm.remark = "";
      this.materielForm.id = 0;
      this.materielForm._LogName = "";
      this.materielForm.childList = [
        {
          contactName: "",
          contactNumber: "",
          Number: false,
          Name:false
        }
      ];
      this.materielVisible = true;
    },
    /**
     * handelEditClick
     * 编辑物料档案
     */
    handelEditClick(row) {
      this.tapNum = 1;
      this.method = "PUT";
      this.postDataEdit = row;

      this.materielForm.materialCode = row.materialCode;
      this.materielForm.materialName = row.materialName;
      this.materielForm.spec = row.spec;
      this.materielForm.colorId = row.colorId;
      this.materielForm.materialTypeId = row.materialTypeId;
      this.materielForm.defaultWarehouseId = row.defaultWarehouseId;
      this.materielForm.shelfLife = row.shelfLife;
      this.materielForm.highInventory = row.highInventory;
      this.materielForm.lowInventory = row.lowInventory;
      this.materielForm.baseUnitId = row.baseUnitId;
      this.materielForm.produceUnitId = row.produceUnitId;
      this.materielForm.produceRate = row.produceRate;
      this.materielForm.purchaseUnitId = row.purchaseUnitId;
      this.materielForm.purchaseRate = row.purchaseRate;
      this.materielForm.salesUnitId = row.salesUnitId;
      this.materielForm.salesRate = row.salesRate;
      this.materielForm.warehouseUnitId = row.warehouseUnitId;
      this.materielForm.warehouseRate = row.warehouseRate;
      this.materielForm.remark = row.remark;
      this.materielForm.id = row.id;
      this.materielForm._LogName = row._LogName;
      var childList = []
      if(row.url!=null) {
        // this.materielForm.childList = row.url.split(";");
        for(var i=0;i<row.url.split(";").length-1;i++) {
          childList.push({
            contactName: row.url.split(";")[i].split(",")[0],
            contactNumber: row.url.split(";")[i].split(",")[1],
            Name:false,
            Number:false
          })
          // this.materielForm.childList[i].contactName = row.url.split(";")[i].split(",")[0];
          // this.materielForm.childList[i].contactNumber = row.url.split(";")[i].split(",")[1];
        }
        this.materielForm.childList = childList;
      }else {
        this.materielForm.childList = [{
          contactName: "",
          contactNumber: "",
          Name:false,
          Number:false
        }]
      }
      this.materielVisible = true;
      this.title = "编辑物料档案";
    },
    /**
     * handelSave
     * 保存物料档案
     */
    handelSave() {
      var childListRes = true;
      this.$refs.materielForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
          if (
            this.materielForm.materialCode == "" ||
            this.materielForm.materialName == "" ||
            this.materielForm.materialTypeId == ""
          ) {
            this.tapNum = 1;
            return;
          } else if (this.materielForm.baseUnitId == "") {
            this.tapNum = 2;
            return;
          }
        } else {
          if (
            parseFloat(this.materielForm.lowInventory) >
            parseFloat(this.materielForm.highInventory)
          ) {
            this.tapNum = 1;
            this.$message({
              message: "最低库存数量不能高于最高库存数量",
              type: "error"
            });
            return;
          }
           if (
            this.materielForm.childList.length == 1 ||
            this.materielForm.childList.length == 2 ||
            this.materielForm.childList.length == 3
          ) {
            if (
              this.materielForm.childList.length == 1&&this.materielForm.childList[0].contactName != undefined||
              this.materielForm.childList.length == 2&&this.materielForm.childList[0].contactName != undefined||this.materielForm.childList[1].contactName != undefined||
              this.materielForm.childList.length == 3&&this.materielForm.childList[0].contactName != undefined||this.materielForm.childList[1].contactName != undefined||this.materielForm.childList[2].contactName != undefined
            ) {
              var childList = "";
              var obj;
              for (var i = 0; i < this.materielForm.childList.length; i++) {
                if(this.materielForm.childList[i].contactNumber!=''){
                  this.materielForm.childList[i].Number = false;
                  if(this.materielForm.childList[i].contactName==''){
                    this.materielForm.childList[i].Name = true;
                    childListRes = false;
                  }else {
                    this.materielForm.childList[i].Name = false;
                  }
                }
                if (this.materielForm.childList[i].contactName) {
                  this.materielForm.childList[i].Name = false;
                  if (
                    !this.materielForm.childList[i].contactNumber
                  ) {
                     this.materielForm.childList[i].Number = true;
                    childListRes = false;
                  } else {
                    this.materielForm.childList[i].Number = false;
                  }
                }
                 obj = {
                    contactName: this.materielForm.childList[i].contactName,
                    contactNumber: this.materielForm.childList[i].contactNumber
                  };
                  if(this.materielForm.childList[i].contactName!=""&&this.materielForm.childList[i].contactNumber!=""){
                    childList+=this.materielForm.childList[i].contactName+","+this.materielForm.childList[i].contactNumber+";";
                  }
              }
            }
          }
          if (!childListRes) {
             this.tapNum = 3;
            return;
          }
          // this.loading = true;
          this.saveBtn = true;
          this.materielForm._LogName = this.materielForm.materialName;
          if(childList.length>0) {
            this.materielForm.url = childList;
          }else {
            this.materielForm.url = null;
          }
         this.materielForm.materialCode = trim(this.materielForm.materialCode);
         this.materielForm.materialName = trim(this.materielForm.materialName);
          // this.materielForm.url = null;
          var data = RequestObject.GetObject(
            this.materielForm,
            null,
            null,
            this.postDataEdit
          );
          request({
            url: "/basicset/api/TBMMaterialFile",
            method: this.method,
            data: data
          })
            .then(res => {
              if (res.code === -1) {
               var setTime = setTimeout(()=>{
                  // this.loading = false;
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
                this.saveBtn = false;
                clearTimeout(setTime);
               },50)
              } else {
                var setTime = setTimeout(()=>{
                  // this.loading = false;
                  this.materielVisible = false;
                this.getMaterielData();
                this.$message({
                  message: "操作成功",
                  duration: 1500,
                  type: "success"
                });
                clearTimeout(setTime);
                },50)
              }
            })
            .catch(error => {
              // this.loading = false;
            });
        }
      });
    },
    /**
     * handleSelectionChange
     * 被勾选的数组
     */
    handleSelectionChange(val) {
      this.allSelectionChange = val;
      if (this.allSelectionChange.length == 0) {
        this.typeColor = "info";
      } else {
        this.typeColor = "danger";
      }
    },
    /**
     * isNoCopy
     * 包型对应的物料，不能复制
     */
    isNoCopy(data) {
      var text = "包型对应的物料，不能复制，物料代码为："
      for(var i=0;i<data.length;i++) {
        if(i!=data.length-1) {
          text+= data[i].materialCode + ","
        }else {
          text+= data[i].materialCode
        }
      }
      return text;
    },
    /**
     * handelBatchDelete
     * 批量复制
     */
    async handelCopyClick() {
       if (this.allSelectionChange.length == 0) {
        this.$message({
          message: "请选择要复制的物料",
          type: "error"
        });
        return;
      }
      var data = await this.handelData(2);
      if(this.NoCopy.length!=0) {
        var text = this.isNoCopy(this.NoCopy);
         this.$alert(text, '错误信息', {
           type: 'error',
          confirmButtonText: '确定'
        });
        return;
      }
      var reqObject = {
        ids:data
      };
      this.$confirm("是否复制？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.loading = true;
          request({
            url: "/basicset/api/TBMMaterialFile/Copy",
            method: "post",
            data: reqObject
          })
            .then(res => {
              if (res.code === -1) {
                this.loading = false;
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.getMaterielData();
                this.$message({
                  message: "操作成功",
                  type: "success",
                  duration: 1500
                });
              }
            })
            .catch(error => {
              this.loading = false;
            });
            this.loading = false;
            return;
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    },
    /**
     * handelBatchDelete
     * 批量删除
     */
    handelData(id) {
      return new Promise((resolve, reject) => {
        var data = [];
        this.NoCopy = [];
        for (var i = 0; i < this.allSelectionChange.length; i++) {
          var Object = {
            id: this.allSelectionChange[i].id,
            _LogName: this.allSelectionChange[i].materialName
          };
         if(id==1) {
            data.push(Object);
         }else {
           if(this.allSelectionChange[i].packageID>0) {
             this.NoCopy.push({
               materialCode: this.allSelectionChange[i].materialCode,
               materialName: this.allSelectionChange[i].materialName
             })
           }
            data.push(this.allSelectionChange[i].id);
         }
        }
        resolve(data);
      });
    },
    async handelBatchDelete() {
      if (this.allSelectionChange.length == 0) {
        this.$message({
          message: "请选择要删除的物料",
          type: "error"
        });
        return;
      }
      var data = await this.handelData(1);
      var reqObject = RequestObject.GetObject(null, data, null, null);
      this.$confirm("是否删除？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.loading = true;
          request({
            url: "/basicset/api/TBMMaterialFile",
            method: "delete",
            data: reqObject
          })
            .then(res => {
              if (res.code === -1) {
                this.loading = false;
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.getMaterielData();
                this.$message({
                  message: "操作成功",
                  type: "success",
                  duration: 1500
                });
              }
            })
            .catch(error => {
              this.loading = false;
            });
            this.loading = false;
            return;
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    },
    /**
     * handelDelete
     * 删除物料档案
     */
    handelDelete(row) {
      this.showtips = false;
      var data = {
        id: row.id,
        _LogName: row.materialName
      };
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        data,
        null
      );
      this.$confirm("是否删除" + row.materialName + "？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.showtips = true;
          this.loading = true;
          request({
            url: "/basicset/api/TBMMaterialFile",
            method: "delete",
            data: reqObject
          })
            .then(res => {
              if (res.code === -1) {
                this.loading = false;
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.getMaterielData();
                this.$message({
                  message: "操作成功",
                  type: "success",
                  duration: 1500
                });
              }
            })
            .catch(error => {
              this.loading = false;
            });
        })
        .catch(() => {
          this.showtips = true;
          this.$message({
            type: "info",
            message: "已取消操作"
          });
        });
    },
    /**
     * pagination
     * 分页信息
     */
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getMaterielData();
    }
  }
};
</script>
<style lang="scss" scoped>
@import "../../../styles/font/iconfont.css";
.el-header /deep/ {
  width: 100%;
  padding-top: 20px;
  // display: flex;
  .el-form {
    width: 100%;
    // display: flex;
    border-bottom: 1px solid #eee;
    // margin-bottom: 20px;
    .el-form-item {
      // flex: 1;
      // margin-right: 20px;
    }
  }
}
// /deep/ .el-dialog__title {
//    color: #1890ff;
// }

/deep/ .el-dialog__body {
  input::-webkit-outer-spin-button,
  input::-webkit-inner-spin-button {
    -webkit-appearance: none;
  }
  input[type="number"] {
    -moz-appearance: textfield;
  }
   .contactNumber .el-input__inner {
    border: 1px solid #ff4949;
  }
  .contactNumber > span {
    color: #ff4949;
    font-size: 12px;
  }
  padding: 0px;
  .el-form {
    margin-top: 20px;
    // height: 350px;
    // overflow-y: scroll;
    padding: 0px 20px;
    .d_inline {
      // display: inline;
      display: flex;
      flex-wrap: wrap;
      .d_inline_item {
        width: 50%;
      }
      .d_inline_40 {
        width: 40%;
      }
      .d_inline_60 {
        width: 60%;
      }
      .item {
        width: 100%;
      }
    }
  }
}
.colorBox /deep/ {
  width: 30px;
  height: 30px;
  border: 1px solid #ccc;
}
.usersTap {
  display: flex;
  margin-bottom: 5px;
  div {
    font-size: 16px;
    width: 70px;
    height: 40px;
    line-height: 40px;
    margin-right: 30px;
  }
  .long-box {
    width: 80px;
  }
  .color {
    color: #409eff;
    border-bottom: 2px solid #409eff;
    // width: 80px;
    line-height: 40px;
    height: 40px;
  }
}

.connectionBox {
    display: flex;
    margin-top: 10px;
    // padding: 0 20px;
    .connectionDiv {
      flex: 1;
      .el-input {
        width: 80%;
      }
      .el-select {
        width: 80%;
      }
    }
    .fontBox {
      width: 132px !important;
      .font {
        border: 1px solid #ccc;
        font-size: 36px;
        color: #0000ff;
        line-height: 26px;
        white-space: nowrap;
        // text-align: center;
        // width: 80px;
        // display: inline-block;
        padding: 10px;
      }
    }
  }
</style>
