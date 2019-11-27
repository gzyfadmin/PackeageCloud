<template>
  <el-container
    v-loading="loading"
     element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header
      id="elheader"
      style="height:auto"
      class="headerBd"
    >
      <el-form inline>
        <el-form-item
          label="供应商名称："
          label-width="90px"
        >
          <el-input placeholder="请输入供应商名称" v-model="queryData.name" clearable @clear="getConnectionData()" @keyup.enter.native="getConnectionData()">
            <i
              slot="suffix"
              class="el-icon-search"
              @click="getConnectionData()"
            />
          </el-input>
        </el-form-item>
        <el-form-item
          label="供应商联系人："
          label-width="104px"
        >
          <el-input v-model="queryData.people" placeholder="请输入供应商联系人" clearable @clear="getConnectionData()" @keyup.enter.native="getConnectionData()">
            <i
              slot="suffix"
              class="el-icon-search"
              @click="getConnectionData()"
            />
          </el-input>
        </el-form-item>
        <el-form-item
          label="供应商类型："
          label-width="90px"
        >
          <el-select
            v-model="queryData.type"
            filterable
            clearable
            placeholder="请选择"
            style="width:100%"
            @change="getConnectionData()"
          >
            <el-option
              v-for="item in typeData"
              :key="item.id"
              :label="item.dicValue"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </el-form>
      <el-button v-if="btnAip.query&&btnAip.query.buttonCaption" type="primary" @click="getConnectionData">{{ btnAip.query.buttonCaption }}</el-button>
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
      <!-- <el-button type="primary" @click="handelAddClick">新增</el-button>
      <el-button type="info" @click="handelBatchDelete">批量删除</el-button> -->
    </el-header>
    <el-main id="elmain">
      <el-table
        v-if="showtab"
        align="center"
        :data="supplierData"
        style="width: 100%"
        border
        :height="mainHeight + 'px'"
        @selection-change="handleSelectionChange"
      >
        <el-table-column
          type="selection"
          width="50"
        />
        <el-table-column
          prop="supplierCode"
          label="编号"
          width="100"
        />
        <el-table-column
          prop="supplierName"
          label="供应商名称"
          width="100"
        />
        <el-table-column
          prop="shortName"
          label="简称"
        />
        <el-table-column
          prop="supplierTypeName"
          label="供应商类型"
        />

        <el-table-column
          prop="industryName"
          label="行业"
          width="90"
        />
        <el-table-column
          prop="contactName"
          label="联系人"
          width="150"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.childList.length>0">{{ scope.row.childList[0].contactName }}</div>
          </template>
        </el-table-column>
        <el-table-column
          prop="contactNumber"
          label="联系电话"
          width="150"
        >
          <template slot-scope="scope">
            <div v-if="scope.row.childList.length>0">{{ scope.row.childList[0].contactNumber }}</div>
          </template>
        </el-table-column>
        <el-table-column
          prop="cityName"
          label="省份城市"
        />
        <el-table-column
          prop="address"
          label="地址"
          width="280"
        >
         <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.address.length>=20"
              class="item"
              effect="light"
              :content="scope.row.address"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.address }}</div>
            </el-tooltip>
            <div
              v-if="scope.row.address.length<20"
              class="ellipsis"
            >{{ scope.row.address }}</div>
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
    <!-- 新增和编辑弹窗 -->
    <el-dialog
      :title="title"
      :visible.sync="connectionVisible"
      width="40%"
      :close-on-click-modal="false"
    >
      <div class="title">
        基本资料
      </div>
      <el-form
        ref="supplierForm"
        :model="supplierForm"
        :rules="connectionRules"
      >
        <div class="d_inline">
          <el-form-item
            label="供应商编号："
            prop="supplierCode"
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入供应商编号" v-model="supplierForm.supplierCode" />
          </el-form-item>
          <el-form-item
            label="供应商名称："
            prop="supplierName"
            label-width="115px"
            class="d_inline_item"
          >
            <el-input placeholder="请输入供应商名称" v-model="supplierForm.supplierName" />
          </el-form-item>
          <el-form-item
            label="供应商简称："
            prop="shortName"
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入供应商简称" v-model="supplierForm.shortName" />
          </el-form-item>
          <el-form-item
            label="供应商类型："
            prop="supplierTypeId"
            label-width="115px"
            class="d_inline_item"
          >
            <el-select
              v-model="supplierForm.supplierTypeId"
              filterable
              clearable
              placeholder="请选择"
              style="width:100%"
            >
              <el-option
                v-for="item in typeData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            label="公司法人："
            prop="legalPerson"
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入公司法人" v-model="supplierForm.legalPerson" />
          </el-form-item>
          <el-form-item
            label="行业："
            prop="industryId"
            label-width="115px"
            class="d_inline_item"
          >
            <el-select
              v-model="supplierForm.industryId"
              filterable
              clearable
              placeholder="请选择"
              style="width:100%"
            >
              <el-option
                v-for="item in industryData"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            label="省份城市："
            prop="city"
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-cascader
              v-model="supplierForm.city"
              style="width:100%"
              :options="addressData"
              :props="{ checkStrictly:true,value:'code',label:'name' }"
              clearable
              filterable
            />
          </el-form-item>
          <el-form-item
            label="供应商地址："
            prop="address"
            label-width="115px"
            class="d_inline_item"
          >
            <el-input placeholder="请输入供应商地址" v-model="supplierForm.address" />
          </el-form-item>
        </div>
        <div class="title1">
          联系人信息
        </div>
        <div
          class="connectionBox"
          style="margin-bottom: 20px;"
        >
          <div class="connectionDiv">
            供应商联系人
          </div>
          <div class="connectionDiv">
            联系电话
          </div>
          <div class="connectionDiv">
            联系优先级
          </div>
          <div class="fontBox" />
        </div>
        <div
          v-for="(item,index) in supplierForm.childList"
          :key="item.key"
          class="connectionBox"
        >
          <el-form-item
            class="connectionDiv"
            :prop="'childList['+index+'].contactName'"
          >
            <el-input
              v-model="item.contactName"
              placeholder="请输入供应商联系人"
              clearable
               :class="item.Name?'contactNumber':''"
            />
            <div
              v-if="item.Name"
              class="el-form-item__error"
            >请输入10位以内的字符串！</div>
          </el-form-item>
          <el-form-item
            class="connectionDiv"
            :prop="'childList['+index+'].contactNumber'"
          >
            <el-input
              v-model="item.contactNumber"
              placeholder="请输入联系电话"
              :class="item.Number?'contactNumber':''"
              clearable
            />
            <div
              v-if="item.Number"
              class="el-form-item__error"
            >请输入20位以内的数字！</div>
          </el-form-item>
          <el-form-item
            class="connectionDiv"
            :prop="'childList['+index+'].priority'"
          >
            <el-select
              v-model="item.priority"
              clearable
              filterable
              placeholder="请选择"
              :class="item.checkPriority?'contactNumber':''"
            >
              <el-option
                v-for="item in priorityData"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              />
            </el-select>
            <div
              v-if="item.checkPriority"
              class="el-form-item__error"
            >请选择优先级！</div>
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

            <!-- <a
              class="font"
              @click="contactAdd(index)"
            >+</a> -->
            <!-- <a
              class="font"
              @click="contactDelete(index)"
            >-</a> -->
          </div>
        </div>

      </el-form>
      <div
        slot="footer"
        class="dialog-footer"
      >
        <el-button @click="connectionVisible = false">关闭</el-button>
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
import data from "@/utils/GetAllDistricts";
import { formatDate,trim } from "@/utils/common.js";
import { getStyle } from "@/utils/Dom.js";

export default {
  name: "ViewsReserveAdminSupplierindexvue",
  components: {
    Pagination
  },
  data() {
    return {
      showtab:false,
      saveBtn:false,
      loading: true,
      btnAip: "",
      typeColor: "info",
      total: 0,
      headHeight: "50px",
      footHeight: "50px",
      mainHeight: "",
      datePicker: "",
      pageSize: 25,
      pageIndex: 1,
      connectionVisible: false,
      title: "新增供应商档案",
      method: "post",
      showtips: true,
      formLabelWidth: "100px",
      allSelectionChange: [],
      supplierData: [],
      realNameOptions: [],
      typeData: [],
      industryData: [],
      addressData: data,
      value: [],
      priorityData: [
        {
          value: 1,
          label: "第一联系人"
        },
        {
          value: 2,
          label: "重要联系人"
        },
        {
          value: 3,
          label: "一般联系人"
        }
      ],
      postDataEdit: {},
      supplierForm: {
        // 新增或修改
        supplierCode: "",
        supplierName: "",
        shortName: "",
        supplierTypeId: "",
        legalPerson: "",
        industryId: "",
        city: "",
        address: "",
        _LogName: "",
        id: "",
        childList: [
          {
            id: 0,
            supplierId: 0,
            contactName: "",
            contactNumber: "",
            priority: "",
            _LogName: "",
            checkPriority: false,
            Number: false,
            Name:false
          }
        ]
      },
      queryData: {
        name: "",
        type: "",
        people: ""
      },
      connectionRules: {
        supplierCode: [
          {
            required: true,
            message: "请输入供应商编号",
            trigger: "blur"
          },
          {
            min: 1,
            max: 10,
            message: "最大允许输入10个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ],
        supplierName: [
          {
            required: true,
            message: "请输入供应商名称",
            trigger: "blur"
          },
          {
            min: 1,
            max: 50,
            message: "最大允许输入50个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ],
        shortName: [
          {
            required: false,
            message: "请输入供应商简称",
            trigger: ["blur", "change"]
          },
          {
            min: 1,
            max: 10,
            message: "最大允许输入10个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ],
        supplierTypeId: [
          {
            required: true,
            message: "请选择供应商类型",
            trigger: "blur"
          }
        ],
        legalPerson: [
          {
            required: false,
            message: "请输入公司法人",
            trigger: ["blur", "change"]
          },
          {
            min: 1,
            max: 10,
            message: "最大允许输入10个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ],
        address: [
          {
            required: false,
            message: "请输入供应商地址",
            trigger: ["blur", "change"]
          },
          {
            min: 1,
            max: 100,
            message: "最大允许输入100个字符，请重新输入！",
            trigger: ["blur", "change"]
          }
        ]
      }
    };
  },
  watch: {
    supplierData() {
      if (this.pageIndex > 1 && this.supplierData.length == []) {
        this.pageIndex--;
        // 初始化表格数据
        this.getConnectionData();
      }
    },
    connectionVisible(val) {
      if (val) {
        // setTimeout(() => {
          // this.pswOnly = false;
        // }, 500);
        this.getType(); // 供应商类型
        this.getIndustry(); // 行业
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.supplierForm.resetFields();
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
    // var lett = this;
    // document.onkeydown = function(e) {
    //   var key = window.event.keyCode;
    //   if (key == 13) {
    //     lett.getConnectionData();
    //   }
    // };
    this.getConnectionData(); // 查看供应商档案
    this.setStyle(); // 设置页面样式
  },
   activated() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
  },
  mounted() {
     this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
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
        // this.mainHeight = b - h - f - 40;
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.mainHeight = b - h - f - parseInt(pt) - parseInt(pb);
      });
    },
    /**
     * getType
     * 查看供应商类型
     */
    getType() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "供应商类型",
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
          this.typeData = res.data;
        }
      });
    },
    /**
     * getIndustry
     * 查看行业
     */
    getIndustry() {
      var QueryCondition = [
        {
          column: "TypeName",
          content: "行业",
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
          this.industryData = res.data;
        }
      });
    },
    /**
     * getConnectionData
     * 查看供应商档案
     */
    getConnectionData() {
      var reqObject;
      var queryConditions = [];
      var orderConditions = [
          {
            column: "id",
            condition: 'asc'
          }
        ]
      if (
        this.queryData.name != "" ||
        this.queryData.type != "" ||
        this.queryData.people
      ) {
        if (trim(this.queryData.name) != "") {
          queryConditions.push({
            column: "supplierName",
            content: this.queryData.name,
            condition: 6
          });
        }
        if (this.queryData.type != "") {
          queryConditions.push({
            column: "supplierTypeId",
            content: this.queryData.type,
            condition: 0
          });
        }
        if (trim(this.queryData.people) != "") {
          queryConditions.push({
            column: "contactName",
            content: this.queryData.people,
            condition: 6
          });
        }
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          queryConditions,
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
        url: "/basicset/api/TBMSupplierFile/GetMainList",
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
          this.supplierData = res.data;
          this.total = res.totalNumber;
          // this.loading = false
        }
      });
    },
    /**
     * handelEditClick
     * 编辑供应商档案
     */
    handelEditClick(row) {
      this.method = "put";
      this.postDataEdit = row;
      this.supplierForm.id = row.id;
      this.supplierForm.supplierCode = row.supplierCode;
      this.supplierForm.supplierName = row.supplierName;
      this.supplierForm.shortName = row.shortName;
      this.supplierForm.supplierTypeId = row.supplierTypeId;
      this.supplierForm.legalPerson = row.legalPerson;
      this.supplierForm.industryId = row.industryId;
      this.supplierForm.city = row.city;
      this.supplierForm.address = row.address;
      this.supplierForm._LogName = "编辑" + row.supplierName;
      if (row.childList.length > 0) {
        this.supplierForm.childList = [];
        var obj;
        for (var i = 0; i < row.childList.length; i++) {
          obj = {
            id: row.childList[i].id || 0,
            supplierId: row.childList[i].supplierId || 0,
            contactName: row.childList[i].contactName,
            contactNumber: row.childList[i].contactNumber,
            priority: row.childList[i].priority,
            _LogName: row.childList[i].contactName,
            checkPriority: false,
            Number: false,
            Name:false
          };
          this.supplierForm.childList.push(obj);
        }
      } else {
        this.supplierForm.childList = [
          {
            id: 0,
            supplierId: 0,
            contactName: "",
            contactNumber: "",
            priority: "",
            _LogName: "",
            checkPriority: false,
            Number: false,
            Name:false
          }
        ];
      }
      this.connectionVisible = true;
      this.title = "编辑供应商档案";
    },
    /**
     * handelAddClick
     * 新增供应商档案
     */
    handelAddClick() {
      this.method = "post";
      this.postDataEdit = null;
      this.supplierForm.supplierCode = "";
      this.supplierForm.supplierName = "";
      this.supplierForm.shortName = "";
      this.supplierForm.supplierTypeId = "";
      this.supplierForm.legalPerson = "";
      this.supplierForm.industryId = "";
      this.supplierForm.city = "";
      this.supplierForm.address = "";
      this.supplierForm.id = 0;
      this.supplierForm._LogName = "新增" + this.supplierForm.supplierName;
      this.supplierForm.childList = [
        {
          id: 0,
          supplierId: 0,
          contactName: "",
          contactNumber: "",
          priority: "",
          _LogName: "",
          checkPriority: false,
          Number: false,
          Name:false
        }
      ];
      this.title = "新增供应商档案";
      this.connectionVisible = true;
    },

    /**
     * handelSave
     * 保存供应商档案
     */
    handelSave() {
      var res = /^\d{1,20}$/;
      var childListRes = true;
      this.$refs.supplierForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
        } else {
          if (this.supplierForm.city.constructor === Array) {
            this.supplierForm.city = this.supplierForm.city[
              this.supplierForm.city.length - 1
            ];
          }
          var lis = {
            supplierCode: this.supplierForm.supplierCode,
            supplierName: this.supplierForm.supplierName,
            shortName: this.supplierForm.shortName,
            supplierTypeId: this.supplierForm.supplierTypeId,
            legalPerson: this.supplierForm.legalPerson,
            industryId: this.supplierForm.industryId,
            city: this.supplierForm.city,
            address: this.supplierForm.address,
            _LogName: "",
            id: this.supplierForm.id
          };
          if (this.method == "post") {
            lis._LogName = "新增" + this.supplierForm.supplierName;
          } else {
            lis._LogName = "编辑" + this.supplierForm.supplierName;
          }
          if (
            this.supplierForm.childList.length == 1 ||
            this.supplierForm.childList.length == 2 ||
            this.supplierForm.childList.length == 3
          ) {
            if (
              this.supplierForm.childList.length == 1&&this.supplierForm.childList[0].contactName != undefined||
              this.supplierForm.childList.length == 2&&this.supplierForm.childList[0].contactName != undefined||this.supplierForm.childList[1].contactName != undefined||
              this.supplierForm.childList.length == 3&&this.supplierForm.childList[0].contactName != undefined||this.supplierForm.childList[1].contactName != undefined||this.supplierForm.childList[2].contactName != undefined
            ) {
              lis.childList = [];
              var obj;
              for (var i = 0; i < this.supplierForm.childList.length; i++) {
                if(this.supplierForm.childList[i].contactNumber!=''||this.supplierForm.childList[i].priority!=''){
                  if(this.supplierForm.childList[i].contactNumber!='') {
                  this.supplierForm.childList[i].Number = false
                  if(this.supplierForm.childList[i].priority=="") {
                    this.supplierForm.childList[i].checkPriority = true;
                  }else {
                    this.supplierForm.childList[i].checkPriority = false
                  }
                }else if(this.supplierForm.childList[i].priority!='') {
                  this.supplierForm.childList[i].checkPriority = false
                  if(this.supplierForm.childList[i].contactNumber=="") {
                    this.supplierForm.childList[i].Number = true;
                  }else {
                    this.supplierForm.childList[i].Number = false
                  }
                }
                  if(this.supplierForm.childList[i].contactName==''){
                    this.supplierForm.childList[i].Name = true;
                    childListRes = false;
                  }else {
                    this.supplierForm.childList[i].Name = false;
                  }
                }
                if (this.supplierForm.childList[i].contactName) {
                  this.supplierForm.childList[i].Name = false;
                  if (
                    !res.test(this.supplierForm.childList[i].contactNumber) ||
                    !this.supplierForm.childList[i].priority
                  ) {
                    if (
                      res.test(this.supplierForm.childList[i].contactNumber)
                    ) {
                      this.supplierForm.childList[i].Number = false;
                    } else {
                      this.supplierForm.childList[i].Number = true;
                    }

                    if (!this.supplierForm.childList[i].priority) {
                      this.supplierForm.childList[i].checkPriority = true;
                    } else {
                      this.supplierForm.childList[i].checkPriority = false;
                    }
                    childListRes = false;
                  } else {
                    this.supplierForm.childList[i].Number = false;
                    this.supplierForm.childList[i].checkPriority = false;
                  }
                  obj = {
                    id: this.supplierForm.childList[i].id || 0,
                    supplierId: this.supplierForm.childList[i].supplierId || 0,
                    contactName: this.supplierForm.childList[i].contactName,
                    contactNumber: this.supplierForm.childList[i].contactNumber,
                    priority: this.supplierForm.childList[i].priority,
                    _LogName: this.supplierForm.childList[i].contactName
                  };
                  lis.childList.push(obj);
                }
              }
            }
          }
          var data = RequestObject.GetObject(
            lis,
            null,
            null,
            this.postDataEdit
          );
          if (!childListRes) {
            return;
          }
          // this.loading = true;
          this.saveBtn = true;
          request({
            url: "/basicset/api/TBMSupplierFile",
            method: this.method,
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                var setTime = setTimeout(()=>{
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
                  this.connectionVisible = false;
                this.getConnectionData();
                this.$message({
                  message: "操作成功",
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
     * handelBatchDelete
     * 批量删除
     */
    handelData() {
      return new Promise((resolve, reject) => {
        var data = [];
        for (var i = 0; i < this.allSelectionChange.length; i++) {
          var Object = {
            id: this.allSelectionChange[i].id,
            _LogName: this.allSelectionChange[i].supplierName
          };
          data.push(Object);
        }
        resolve(data);
      });
    },
    async handelBatchDelete() {
      if (this.allSelectionChange.length == 0) {
        this.$message({
          message: "请选择要删除的供应商",
          type: "error"
        });
        return;
      }
      var data = await this.handelData();
      var reqObject = RequestObject.GetObject(null, data, null, null);
      this.$confirm("是否删除？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.loading = true;
          request({
            url: "/basicset/api/TBMSupplierFile",
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
                this.getConnectionData();
                this.$message({
                  message: "操作成功",
                  type: "success"
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
     * 删除供应商档案
     */
    handelDelete(row) {
      this.showtips = false;
      var data = {
        id: row.id,
        _LogName: row.supplierName
      };
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        data,
        null
      );
      this.$confirm("是否删除" + row.supplierName + "？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.showtips = true;
          this.loading = true;
          request({
            url: "/basicset/api/TBMSupplierFile",
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
                this.getConnectionData();
                this.$message({
                  message: "操作成功",
                  type: "success"
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
     * contactAdd
     * 添加联系人
     */
    contactAdd(index) {
      var obj = {
        contactName: "",
        contactNumber: "",
        priority: "",
        Number: false,
        Name:false,
        checkPriority: false,
        key: Date.now()
      };
      if (this.supplierForm.childList.length < 3) {
        this.supplierForm.childList.splice(index + 1, 0, obj);
      } else {
        this.$message.error("最多只能添加三个联系人");
      }
    },
    /**
     * contactDelete
     * 删除联系人
     */
    contactDelete(index) {
      if (this.supplierForm.childList.length !== 1) {
        this.supplierForm.childList.splice(index, 1);
      }
    },
    /**
     * pagination
     * 分页信息
     */
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getConnectionData();
    }
  }
};
</script>

<style lang="scss" scoped>
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
.el-main {
  i {
    font-size: 20px;
    text-align: center;
    color: #67c23a;
  }
}
/deep/ .el-dialog__title {
  color: #1890ff;
}
/deep/ .el-dialog__body {
  // padding-top: 10px;
  padding: 10px 20px;
  .contactNumber .el-input__inner {
    border: 1px solid #ff4949;
  }
  .contactNumber > span {
    color: #ff4949;
    font-size: 12px;
  }
}
.el-dialog {
  .title {
    font-size: 16px;
    font-weight: 600;
    padding: 0 20px;
  }
  .title1 {
    padding:10px 0px;
    font-size: 16px;
    font-weight: 600;
  }
  .el-form {
    // margi: 20px;
    // height: 350px;
    // overflow-y: scroll;
    padding: 20px;
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
}
</style>
