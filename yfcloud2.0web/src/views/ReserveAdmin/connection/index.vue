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
          label="客户名称："
          label-width="76px"
        >
          <el-input v-model="queryData.name" placeholder="请输入客户名称" clearable @clear="getConnectionData()" @keyup.enter.native="getConnectionData()">
            <i
              slot="suffix"
              class="el-icon-search"
              @click="getConnectionData()"
            />
          </el-input>
        </el-form-item>
        <el-form-item
          label="客户联系人："
          label-width="90px"
        >
          <el-input v-model="queryData.people" placeholder="请输入客户联系人" clearable @clear="getConnectionData()" @keyup.enter.native="getConnectionData()">
            <i
              slot="suffix"
              class="el-icon-search"
              @click="getConnectionData()"
            />
          </el-input>
        </el-form-item>
        <el-form-item
          label="客户类型："
          label-width="76px"
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
        v-if="btnAip.add.buttonCaption"
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
        :data="connectionData"
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
          prop="customerCode"
          label="编号"
          width="100"
        />
        <el-table-column
          prop="customerName"
          label="客户名称"
          width="200"
        />
        <el-table-column
          prop="shortName"
          label="客户简称"
        />
        <el-table-column
          prop="customerTypeName"
          label="客户类型"
          width="80"
        />
        <el-table-column
          prop="industryName"
          label="行业"
          width="90"
        />
        <el-table-column
          prop="contactName"
          label="客户联系人"
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
          label="客户地址"
          width="280"
        >
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.address&&scope.row.address.length>=20"
              class="item"
              effect="light"
              :content="scope.row.address"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.address }}</div>
            </el-tooltip>
            <div
              v-if="scope.row.address&&scope.row.address.length<20"
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
        ref="connectionForm"
        :model="connectionForm"
        :rules="connectionRules"
      >
        <div class="d_inline">
          <el-form-item
            label="客户编号："
            prop="customerCode"
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入客户编号" v-model="connectionForm.customerCode" />
          </el-form-item>
          <el-form-item
            label="客户名称："
            prop="customerName"
            label-width="105px"
            class="d_inline_item"
          >
            <el-input placeholder="请输入客户名称" v-model="connectionForm.customerName" />
          </el-form-item>
          <el-form-item
            label="客户简称："
            prop="shortName"
            :label-width="formLabelWidth"
            class="d_inline_item"
          >
            <el-input placeholder="请输入客户简称" v-model="connectionForm.shortName" />
          </el-form-item>
          <el-form-item
            label="客户类型："
            prop="customerTypeId"
            label-width="105px"
            class="d_inline_item"
          >
            <el-select
              v-model="connectionForm.customerTypeId"
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
            <el-input placeholder="请输入公司法人" v-model="connectionForm.legalPerson" />
          </el-form-item>
          <el-form-item
            label="行业："
            prop="industryId"
            label-width="105px"
            class="d_inline_item"
          >
            <el-select
              v-model="connectionForm.industryId"
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
              v-model="connectionForm.city"
              style="width:100%"
              :options="addressData"
              :props="{ checkStrictly:true,value:'code',label:'name' }"
              clearable
              filterable
            />
          </el-form-item>
          <el-form-item
            label="请输入客户地址："
            prop="address"
            label-width="105px"
            class="d_inline_item"
          >
            <el-input placeholder="客户地址" v-model="connectionForm.address" />
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
            客户联系人
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
          v-for="(item,index) in connectionForm.childList"
          :key="item.key"
          class="connectionBox"
        >
          <el-form-item
            class="connectionDiv"
            :prop="'childList['+index+'].contactName'"
          >
            <el-input
              v-model="item.contactName"
              placeholder="请输入客户联系人"
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
import Pagination from '@/components/Pagination'
import request from '@/utils/request'
import RequestObject from '@/utils/requestObject'
import { getBtnCtr } from '@/utils/BtnControl'
import data from '@/utils/GetAllDistricts'
import { formatDate,trim } from "@/utils/common.js";
import { getStyle } from "@/utils/Dom.js";

export default {
  name: 'ViewsReserveAdminconnectionindexvue',
  components: {
    Pagination
  },
  data() {
    return {
      showtab:false,
      loading: true,
      saveBtn:false,
      btnAip: '',
      typeColor: 'info',
      total: 0,
      headHeight: '50px',
      footHeight: '50px',
      mainHeight: '',
      datePicker: '',
      pageSize: 25,
      pageIndex: 1,
      connectionVisible: false,
      showtips: true,
      title: '新增客户档案',
      method: 'post',
      formLabelWidth: '85px',
      allSelectionChange: [],
      connectionData: [],
      typeData: [],
      industryData: [],
      addressData: data,
      value: [],
      priorityData: [
        {
          value: 1,
          label: '第一联系人'
        },
        {
          value: 2,
          label: '重要联系人'
        },
        {
          value: 3,
          label: '一般联系人'
        }
      ],
      postDataEdit: {},
      connectionForm: {
        // 新增或修改
        customerCode: '',
        customerName: '',
        shortName: '',
        customerTypeId: '',
        legalPerson: '',
        industryId: '',
        city: '',
        address: '',
        companyId: '',
        deleteFlag: '',
        _LogName: '',
        id: '',
        cellNume:0,
        childList: [
          {
            id: 0,
            customerId: 1,
            contactName: '',
            contactNumber: '',
            priority: '',
            _LogName: '',
            checkPriority: false,
            Number: false,
            Name: false
          }
        ]
      },
      queryData: {
        name: '',
        type: '',
        people: ''
      },
      connectionRules: {
        customerCode: [
          {
            required: true,
            message: '请输入客户编号',
            trigger: 'blur'
          },
          {
            min: 1,
            max: 10,
            message: '最大允许输入10个字符，请重新输入！',
            trigger: ['blur', 'change']
          }
        ],
        customerName: [
          {
            required: true,
            message: '请输入客户名称',
            trigger: 'blur'
          },
          {
            min: 1,
            max: 50,
            message: '最大允许输入50个字符，请重新输入！',
            trigger: ['blur', 'change']
          }
        ],
        shortName: [
          {
            required: false,
            message: '请输入客户简称',
            trigger: ['blur', 'change']
          },
          {
            min: 1,
            max: 10,
            message: '最大允许输入10个字符，请重新输入！',
            trigger: ['blur', 'change']
          }
        ],
        customerTypeId: [
          {
            required: true,
            message: '请选择客户类型',
            trigger: 'blur'
          }
        ],
        legalPerson: [
          {
            required: false,
            message: '请输入公司法人',
            trigger: ['blur', 'change']
          },
          {
            min: 1,
            max: 10,
            message: '最大允许输入10个字符，请重新输入！',
            trigger: ['blur', 'change']
          }
        ],
        address: [
          {
            required: false,
            message: '请输入客户地址',
            trigger: ['blur', 'change']
          },
          {
            min: 1,
            max: 100,
            message: '最大允许输入100个字符，请重新输入！',
            trigger: ['blur', 'change']
          }
        ]
      }
    }
  },
  watch: {
    connectionData() {
      if (this.pageIndex > 1 && this.connectionData.length == []) {
        this.pageIndex--
        // 初始化表格数据
        this.getConnectionData()
      }
    },
    connectionVisible(val) {
      if (val) {
        // setTimeout(() => {
          // this.pswOnly = false;
        // }, 500)
        this.getType() // 客户类型
        this.getIndustry() // 行业
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.connectionForm.resetFields();
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )

    // 键盘返回键
    // var lett = this
    // document.onkeydown = function(e) {
    //   var key = window.event.keyCode
    //   if (key == 13) {
    //     lett.getConnectionData()
    //   }
    // }
    this.getConnectionData() // 查看客户档案
    this.setStyle() // 设置页面样式
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
     * checkContactName
     * 验证联系人
     */
    checkContactName: (rule, value, callback) => {
      if (!value) {
        callback()
      } else if (value.length > 10) {
        callback(new Error('最大允许输入10个字符，请重新输入！'))
      } else {
        callback()
      }
    },
    /**
     * setStyle
     * 设置页面样式
     */
    setStyle() {
      this.$nextTick(() => {
        var navbar = document.getElementById('navbar_yfkj')
        var nv = navbar.clientHeight || navbar.offsetHeight
        var b = document.documentElement.clientHeight - nv
        var elheader = document.getElementById('elheader')
        var elfooter = document.getElementById('elfooter')
        var h = elheader.clientHeight || elheader.offsetHeight
        var f = elfooter.clientHeight || elfooter.offsetHeight
        // this.mainHeight = b - h - f - 40
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.mainHeight = b - h - f - parseInt(pt) - parseInt(pb);
      })
    },
    /**
     * getType
     * 查看客户类型
     */
    getType() {
      var QueryCondition = [
        {
          column: 'TypeName',
          content: '客户类型',
          condition: 0
        }
      ]
      var requsets = {
        IsPaging: false,
        PageSize: this.pageSize,
        PageIndex: this.pageIndex,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      }
      request({
        url: '/basicset/api/TBMDictionary',
        method: 'GET',
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.typeData = res.data
        }
      })
    },
    /**
     * getIndustry
     * 查看行业
     */
    getIndustry() {
      var QueryCondition = [
        {
          column: 'TypeName',
          content: '行业',
          condition: 0
        }
      ]
      var requsets = {
        IsPaging: false,
        PageSize: this.pageSize,
        PageIndex: this.pageIndex,
        QueryConditions: QueryCondition,
        OrderByConditions: null
      }
      request({
        url: '/basicset/api/TBMDictionary',
        method: 'GET',
        params: { requestObject: JSON.stringify(requsets) }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.industryData = res.data
        }
      })
    },
    /**
     * getConnectionData
     * 查看客户档案
     */
    getConnectionData() {
      var reqObject
      var queryConditions = []
       var orderConditions = [
          {
            column: "id",
            condition: 'asc'
          }
        ]
      if (
        this.queryData.name != '' ||
        this.queryData.type != '' ||
        this.queryData.people
      ) {
        if (trim(this.queryData.name) != '') {
          queryConditions.push({
            column: 'customerName',
            content: this.queryData.name,
            condition: 6
          })
        }
        if (this.queryData.type != '') {
          queryConditions.push({
            column: 'customerTypeId',
            content: this.queryData.type,
            condition: 0
          })
        }
        if (trim(this.queryData.people) != '') {
          queryConditions.push({
            column: 'contactName',
            content: this.queryData.people,
            condition: 6
          })
        }
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          queryConditions,
          orderConditions
        )
      } else {
        reqObject = RequestObject.LonginBookObject(
          true,
          this.pageSize,
          this.pageIndex,
          null,
          null,
          orderConditions
        )
      }
      request({
        url: '/basicset/api/TBMCustomerFile/GetMainList',
        method: 'get',
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        this.loading = false
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.connectionData = res.data
          this.total = res.totalNumber
          // this.loading = false
        }
      })
    },
    /**
     * handelEditClick
     * 编辑客户档案
     */
    handelEditClick(row) {
      this.method = 'put'
      this.postDataEdit = row
      this.connectionForm.id = row.id
      this.connectionForm.customerCode = row.customerCode
      this.connectionForm.customerName = row.customerName
      this.connectionForm.shortName = row.shortName
      this.connectionForm.customerTypeId = row.customerTypeId
      this.connectionForm.legalPerson = row.legalPerson
      this.connectionForm.industryId = row.industryId
      this.connectionForm.city = row.city
      this.connectionForm.address = row.address
      this.connectionForm.companyId = row.companyId
      this.connectionForm.deleteFlag = row.deleteFlag
      this.connectionForm._LogName = '编辑' + row.customerName
      if (row.childList.length > 0) {
        this.connectionForm.childList = []
        var obj
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
            Name: false
          }
          this.connectionForm.childList.push(obj)
        }
      } else {
        this.connectionForm.childList = [
          {
            id: 0,
            customerId: 0,
            contactName: '',
            contactNumber: '',
            priority: '',
            _LogName: '',
            checkPriority: false,
            Number: false,
            Name: false
          }
        ]
      }
      this.connectionVisible = true
      this.title = '编辑客户档案'
    },
    /**
     * handelAddClick
     * 新增客户档案
     */
    handelAddClick() {
      this.method = 'post'
      this.postDataEdit = null
      this.connectionForm.customerCode = ''
      this.connectionForm.customerName = ''
      this.connectionForm.connectionAddress = ''
      this.connectionForm.shortName = ''
      this.connectionForm.customerTypeId = ''
      this.connectionForm.industryId = ''
      this.connectionForm.city = ''
      this.connectionForm.address = ''
      this.connectionForm.companyId = ''
      this.connectionForm.deleteFlag = ''
      this.connectionForm.id = 0
      this.connectionForm._LogName = '新增' + this.connectionForm.customerName
      this.connectionForm.childList = [
        {
          id: 0,
          customerId: 0,
          contactName: '',
          contactNumber: '',
          priority: '',
          _LogName: '',
          checkPriority: false,
          Number: false,
          Name: false
        }
      ]
      this.title = '新增客户档案'
      this.connectionVisible = true
    },

    /**
     * handelSave
     * 保存客户档案
     */
    handelSave() {
      var res = /^\d{1,20}$/
      var childListRes = true
      this.$refs.connectionForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: '数据不合法，无法保存',
            type: 'error'
          })
        } else {
          if (this.connectionForm.city.constructor === Array) {
            this.connectionForm.city = this.connectionForm.city[
              this.connectionForm.city.length - 1
            ]
          }
          var data = {
            customerCode: this.connectionForm.customerCode,
            customerName: this.connectionForm.customerName,
            shortName: this.connectionForm.shortName,
            customerTypeId: this.connectionForm.customerTypeId,
            legalPerson: this.connectionForm.legalPerson,
            industryId: this.connectionForm.industryId,
            city: this.connectionForm.city,
            address: this.connectionForm.address,
            companyId: this.connectionForm.companyId,
            deleteFlag: this.connectionForm.deleteFlag,
            _LogName: '',
            id: this.connectionForm.id
          }
          if (this.method == 'post') {
            data._LogName = '新增' + this.connectionForm.customerName
          } else {
            data._LogName = '编辑' + this.connectionForm.customerName
          }
          if ( this.connectionForm.childList.length == 1 ||
            this.connectionForm.childList.length == 2 ||
            this.connectionForm.childList.length == 3) {
              if (
              this.connectionForm.childList.length == 1&&this.connectionForm.childList[0].contactName != undefined||
              this.connectionForm.childList.length == 2&&this.connectionForm.childList[0].contactName != undefined||this.connectionForm.childList[1].contactName != undefined||
              this.connectionForm.childList.length == 3&&this.connectionForm.childList[0].contactName != undefined||this.connectionForm.childList[1].contactName != undefined||this.connectionForm.childList[2].contactName != undefined
            ) {
            data.childList = []
            var obj
            for (var i = 0; i < this.connectionForm.childList.length; i++) {
              if(this.connectionForm.childList[i].contactNumber!=''||this.connectionForm.childList[i].priority!=''){
                if(this.connectionForm.childList[i].contactNumber!='') {
                  this.connectionForm.childList[i].Number = false;
                  if(this.connectionForm.childList[i].priority==""){
                    this.connectionForm.childList[i].checkPriority = true;
                  }else {
                    this.connectionForm.childList[i].checkPriority = false;
                  }
                }else if(this.connectionForm.childList[i].priority!='') {
                  this.connectionForm.childList[i].checkPriority = false;
                  if(this.connectionForm.childList[i].contactNumber=="") {
                    this.connectionForm.childList[i].Number = true;
                  }else {
                    this.connectionForm.childList[i].Number = false;
                  }
                }
                  if(this.connectionForm.childList[i].contactName==''){
                    this.connectionForm.childList[i].Name = true;
                    childListRes = false;
                  }else {
                    this.connectionForm.childList[i].Name = false;
                  }
                }
              if (this.connectionForm.childList[i].contactName) {
                this.connectionForm.childList[i].Name = false;
                if (
                  !res.test(this.connectionForm.childList[i].contactNumber) ||
                  !this.connectionForm.childList[i].priority
                ) {
                  if (res.test(this.connectionForm.childList[i].contactNumber)) {
                    this.connectionForm.childList[i].Number = false
                  } else {
                    this.connectionForm.childList[i].Number = true
                  }

                  if (!this.connectionForm.childList[i].priority) {
                    this.connectionForm.childList[i].checkPriority = true
                  } else {
                    this.connectionForm.childList[i].checkPriority = false
                  }
                  childListRes = false
                } else {
                  this.connectionForm.childList[i].Number = false
                  this.connectionForm.childList[i].checkPriority = false
                }
                obj = {
                  id: this.connectionForm.childList[i].id || 0,
                  customerId: this.connectionForm.childList[i].customerId || 0,
                  contactName: this.connectionForm.childList[i].contactName,
                  contactNumber: this.connectionForm.childList[i].contactNumber,
                  priority: this.connectionForm.childList[i].priority,
                  _LogName: this.connectionForm.childList[i].contactName
                }
                data.childList.push(obj)
              }
            }
            }
          }
          var data = RequestObject.GetObject(
            data,
            null,
            null,
            this.postDataEdit
          )
          if (!childListRes) {
            return
          }
          // this.loading = true
          this.saveBtn = true;
          request({
            url: '/basicset/api/TBMCustomerFile',
            method: this.method,
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                var setTime = setTimeout(()=>{
                  this.$confirm(res.info, '错误信息', {
                  confirmButtonText: '确定',
                  type: 'error',
                  showCancelButton: false
                })
                this.saveBtn = false;
                clearTimeout(setTime)
                },50)
              } else {
                var setTime = setTimeout(()=>{
                  this.connectionVisible = false
                this.getConnectionData()
                this.$message({
                  message: '操作成功',
                  type: 'success'
                })
                clearTimeout(setTime)
                },50)
              }
            })
            .catch(error => {
              // this.loading = false
            })
        }
      })
    },
    /**
     * handleSelectionChange
     * 被勾选的数组
     */
    handleSelectionChange(val) {
      this.allSelectionChange = val
      if (this.allSelectionChange.length == 0) {
        this.typeColor = 'info'
      } else {
        this.typeColor = 'danger'
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
            _LogName: this.allSelectionChange[i].customerName
          };
          data.push(Object);
        }
        resolve(data);
      });
    },
    async handelBatchDelete() {
      if (this.allSelectionChange.length == 0) {
        this.$message({
          message: '请选择要删除的客户',
          type: 'error'
        })
        return
      }else {
      var data = await this.handelData();
      var reqObject = RequestObject.GetObject(null, data, null, null);
      this.$confirm('是否删除？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          this.loading = true
          request({
            url: '/basicset/api/TBMCustomerFile',
            method: 'delete',
            data: reqObject
          })
            .then(res => {
              if (res.code === -1) {
                this.loading = false
                this.$confirm(res.info, '错误信息', {
                  confirmButtonText: '确定',
                  type: 'error',
                  showCancelButton: false
                })
              } else {
                this.getConnectionData()
                this.$message({
                  message: '操作成功',
                  type: 'success'
                })
              }
            })
            .catch(error => {
              this.loading = false
            })
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消操作'
          })
        })

      }
    },
    /**
     * handelDelete
     * 删除客户档案
     */
    handelDelete(row) {
      this.showtips = false;
      var data = {
        id: row.id,
        _LogName: row.customerName
      }
      var reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        data,
        null
      )
      this.$confirm('是否删除' + row.customerName + '?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          this.showtips = true;
          this.loading = true
          request({
            url: '/basicset/api/TBMCustomerFile',
            method: 'delete',
            data: reqObject
          })
            .then(res => {
              if (res.code === -1) {
                this.loading = false
                this.$confirm(res.info, '错误信息', {
                  confirmButtonText: '确定',
                  type: 'error',
                  showCancelButton: false
                })
              } else {
                this.getConnectionData()
                this.$message({
                  message: '操作成功',
                  type: 'success'
                })
              }
            })
            .catch(error => {
              this.loading = false
            })
        })
        .catch(() => {
          this.showtips = true;
          this.$message({
            type: 'info',
            message: '已取消操作'
          })
        })
    },
    /**
     * contactAdd
     * 添加联系人
     */
    contactAdd(index) {
      var obj = {
        contactName: '',
        contactNumber: '',
        priority: '',
        Number: false,
        Name: false,
        checkPriority: false,
        key: Date.now()
      }
      if (this.connectionForm.childList.length < 3) {
        this.connectionForm.childList.splice(index + 1, 0, obj)
      } else {
        this.$message.error('最多只能添加三个联系人')
      }
    },
    /**
     * contactDelete
     * 删除联系人
     */
    contactDelete(index) {
      if (this.connectionForm.childList.length !== 1) {
        this.connectionForm.childList.splice(index, 1)
      }
    },
    /**
     * pagination
     * 分页信息
     */
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize
      this.pageIndex = val.pageIndex
      this.getConnectionData()
    }
  }
}
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
