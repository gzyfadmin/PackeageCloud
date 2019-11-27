<template>
  <el-container
    id="menus"
    v-loading="loading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <el-header id="elheader" class="header headerBd" height="auto">
      <!-- <div @click="doopen">上传头像</div> -->
      <el-form :label-position="labelPosition" label-width="76px" class="FormInput" inline>
        <el-form-item label="账号：" label-width="48px">
          <el-input
            v-model="keyWords"
            placeholder="请输入手机/邮箱"
            clearable
            style="width:200px"
            @keyup.enter.native="getTreeDefaule"
            @clear="getTreeDefaule"
          >
            <i slot="suffix" class="el-input__icon el-icon-search" @click="getTreeDefaule"></i>
          </el-input>
        </el-form-item>

        <el-form-item label="账号姓名：">
          <el-input
            v-model="userName"
            placeholder="请输入账号姓名"
            clearable
            style="width:200px"
            @keyup.enter.native="getTreeDefaule"
            @clear="getTreeDefaule"
          >
            <i slot="suffix" class="el-input__icon el-icon-search" @click="getTreeDefaule"></i>
          </el-input>
        </el-form-item>

        <el-form-item label="创建时间：" label-width="76px">
          <el-date-picker
            v-model="createTime"
            style="width:400px"
            type="daterange"
            align="left"
            unlink-panels
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            :picker-options="pickerOptions"
            @change="getTreeDefaule"
          />
        </el-form-item>

        <el-form-item label="状态：" label-width="48px">
          <el-radio-group v-model="Status" @change="getTreeDefaule">
            <el-radio-button label="1">有效</el-radio-button>
            <el-radio-button label="0">无效</el-radio-button>
            <el-radio-button label="2">过期</el-radio-button>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <el-button
        v-if="btnAip.query&&btnAip.query.buttonCaption"
        type="primary"
        @click="getTree"
      >{{ btnAip.query.buttonCaption }}</el-button>
      <el-button
        v-if="btnAip.add&&btnAip.add.buttonCaption"
        type="primary"
        @click="handelClick(1)"
      >{{ btnAip.add.buttonCaption }}</el-button>
    </el-header>
    <el-main id="elmain">
      <el-table
        ref="table"
        :height="tableHeight"
        :data="tableData"
        style="width: 99.9%"
        row-key="id"
        border
      >
        <el-table-column type="index" label="序号" />
        <el-table-column prop="telAccount" label="手机账号" width="100" />
        <el-table-column prop="emailAccount" label="邮箱账号" width="200" />
        <el-table-column prop="accountName" label="账号姓名" width="120" />
        <el-table-column prop="status" label="账号状态" width="80">
          <template slot-scope="scope">
            <div v-if="scope.row.status==0">无效</div>
            <div v-if="scope.row.status==1">有效</div>
            <div v-if="scope.row.status==2">过期</div>
          </template>
        </el-table-column>
        <el-table-column prop="deptName" label="部门" width="90" />
        <el-table-column prop="roleName" label="角色" width="120" />
        <!-- <el-table-column prop="expDate" label="有效期" width="90"> -->
        <!-- <template slot-scope="scope">
            <div>{{ scope.row.expDate |formatTDate }}</div>
          </template>
        </el-table-column>-->
        <el-table-column prop="jobNumber" label="工号" />
        <el-table-column prop="realName" label="真实姓名" />
        <el-table-column prop="fixedNumber" label="固定电话" width="120" />
        <el-table-column prop="headPicPath" label="头像" width="80">
          <template slot-scope="scope">
            <img
              v-if="scope.row.headPicPath"
              :src="$store.state.user.imgUrlName + scope.row.headPicPath"
              class="avatarTable_"
            />
            <!-- <div
              v-if="scope.row.iconFlag&&scope.row.headPicPath"
              class="icon-item avatarTable"
              v-html="scope.row.headPicPath"
            />
            <svg-icon
              v-if="!scope.row.iconFlag&&scope.row.headPicPath"
              :icon-class="scope.row.headPicPath"
              class="avatarTable2"
            />-->
            <!-- {{scope.row.iconFlag}} -->
          </template>
        </el-table-column>
        <el-table-column prop="address" label="联系地址" width="250">
          <template slot-scope="scope">
            <div class="ellipsis">{{ scope.row.address }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="remarks" label="备注" width="250">
          <template slot-scope="scope">
            <div class="ellipsis">{{ scope.row.remarks }}</div>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.edit&&btnAip.edit.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.edit.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="primary"
                icon="el-icon-edit"
                circle
                @click="handelClick(2,scope.row)"
              />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.delete&&btnAip.delete.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.delete.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button
                type="danger"
                icon="el-icon-delete"
                circle
                @click="handelClick(3,scope.row)"
              />
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <el-footer id="elfooter">
      <Pagination
        :page-index="pageIndex"
        :total-count="totalCount"
        :page-size="pageSize"
        @pagination="pagination"
      />
    </el-footer>
    <el-dialog
      title="用户信息"
      :visible.sync="dialogVisible"
      width="600px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div class="el-dialog-div" @click="pswClick">
        <el-form ref="formData" :model="formData" label-width="100px" :rules="formRules">
          <el-form-item label="手机账号：" prop="telAccount">
            <el-input v-model="formData.telAccount" placeholder="请输入账号" @change="changeTel" />
          </el-form-item>
          <el-form-item label="邮箱账号：" prop="emailAccount">
            <el-input v-model="formData.emailAccount" placeholder="请输入账号" @change="changeEmail" />
          </el-form-item>
          <el-form-item label="密码" prop="passwd">
            <el-input
              v-model="formData.passwd"
              type="password"
              placeholder="请输入密码"
              :readonly="pswOnly"
            />
          </el-form-item>
          <el-form-item label="账户姓名：" prop="accountName">
            <el-input v-model="formData.accountName" placeholder="请输入账户姓名" />
          </el-form-item>
          <el-form-item label="角色：">
            <el-cascader
              v-model="roleCheck"
              style="width:100%"
              :options="roleList"
              :props="{ checkStrictly:true,value:'id',label:'roleName' }"
              clearable
            />
          </el-form-item>
          <el-form-item label="部门：">
            <el-cascader
              v-model="departmentCheck"
              style="width:100%"
              :options="departmentList"
              :props="{ checkStrictly:true,value:'actualId',label:'deptName' }"
              clearable
            />
          </el-form-item>
          <el-form-item label="工号：">
            <el-input v-model="formData.jobNumber" placeholder="请输入工号" />
          </el-form-item>
          <el-form-item label="真实姓名：">
            <el-input v-model="formData.realName" placeholder="请输入真实姓名" />
          </el-form-item>
          <!-- <el-form-item label="有效期：">
            <el-date-picker
              v-model="formData.expDate"
              type="date"
              placeholder="选择日期"
              style="width:100%;"
            />
          </el-form-item>-->
          <el-form-item label="状态：">
            <el-radio-group v-model="formData.status">
              <el-radio-button label="1">有效</el-radio-button>
              <el-radio-button label="0">无效</el-radio-button>
              <el-radio-button label="2">过期</el-radio-button>
            </el-radio-group>
          </el-form-item>
          <el-form-item label="固定电话：" prop="fixedNumber">
            <el-input v-model="formData.fixedNumber" placeholder="请输入固定电话" />
          </el-form-item>
          <el-form-item label="联系地址：" prop="address">
            <el-input
              v-model="formData.address"
              type="textarea"
              :rows="4"
              :autosize="{ minRows: 3, maxRows: 5}"
              placeholder="请输入联系地址"
            />
          </el-form-item>
          <!-- <el-form-item label="头像路径：">
            <el-upload
              class="avatar-uploader"
              action="http://47.107.135.203:8000/fileupload/api/files/upload"
              :show-file-list="false"
              :on-success="handleAvatarSuccess"
              :before-upload="beforeAvatarUpload"
              :disabled="setimgs"
              style="border:0px solid rgb(220 223 230)"
            >
              <img v-if="formData.headPicPath" :src="imgsrc+formData.headPicPath" class="avatar" />
              <i v-else class="el-icon-plus avatar" />
            </el-upload>
          </el-form-item>-->

          <!-- <el-form-item label="头像：" prop="headImg">
            <div class="iconfont">
              <div v-if="icon" class="icon-item" v-html="iconfontImg" />
              <svg-icon v-if="!icon" :icon-class="iconfontImg" />
              <div class="iconfontBox">
                <button @click="iconfontVisible = true">选择头像</button>
              </div>
            </div>
          </el-form-item>-->

          <el-form-item label="头像：" prop="headImg">
            <div class="iconfont">
              <img
                v-if="formData.headPicPath!=''&&formData.headPicPath!=null&&formData.headPicPath!='null'"
                :src="$store.state.user.imgUrlName+formData.headPicPath"
                class="avatarTable"
                @click="doopen"
              />
              <div v-else class="avatarTable" @click="doopen">
                <i class="el-icon-s-custom"></i>
              </div>

              <!-- <div v-if="iconFlag" class="icon-item" v-html="formData.headPicPath" /> -->
              <!-- <svg-icon v-if="!iconFlag" :icon-class="formData.headPicPath" /> -->
              <!-- <svg-icon v-if="!iconFlag" :icon-class="iconfontImg" />     -->
              <!-- <div v-if="!iconFlag"><span v-html="iconfontImg"></span></div> -->
              <!-- <div class="iconfontBox">
                <button type="button" @click="selectIco">选择头像</button>
              </div>-->
            </div>
          </el-form-item>

          <el-form-item label="备注：" prop="remarks">
            <el-input
              v-model="formData.remarks"
              type="textarea"
              :rows="4"
              :autosize="{ minRows: 3, maxRows: 5}"
              placeholder="请输入用户说明"
            />
          </el-form-item>
        </el-form>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="handelCloseClick">关闭</el-button>
        <el-button type="primary" :loading="saveBtn" @click="OnBtnSaveSubmit">保存</el-button>
      </div>
    </el-dialog>

    <!-- <el-dialog title="选择头像图标" :visible.sync="iconfontVisible" width="600px" append-to-body>
      <div class="el-tab-pane">
        <div
          v-for="item of svgIcons"
          :key="item"
          @click="handleClipboard(generateIconCode(item),$event)"
        >
          <div class="icon-item">
            <svg-icon :icon-class="item" class-name="disabled" />
          </div>
        </div>
        <div
          v-for="item of elementIcons"
          :key="item"
          @click="handleClipboard(generateElementIconCode(item),$event)"
        >
          <div class="icon-item">
            <i :class="'el-icon-' + item" />
          </div>
        </div>
      </div>
    </el-dialog>-->

    <IconsEdit ref="IconsEdit" @doSelect="doIcoSelect" />
    <UploadPic ref="UploadPic" @upLoadClose="upLoadClose" />
  </el-container>
</template>
<script>
import height from "@/utils/height";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { format } from "@/utils/dateUtil";

import svgIcons from "@/views/icons/svg-icons";
import elementIcons from "@/views/icons/element-icons";

import IconsEdit from "@/components/IconsEdit";
import Pagination from "@/components/Pagination";
import { getBtnCtr } from "@/utils/BtnControl";

import { trim } from "@/utils/common.js";
import { getStyle } from "@/utils/Dom.js";

import UploadPic from "@/components/UploadPic/index.vue";

export default {
  name: "viewsSysSettingsUserindexvue",
  components: {
    Pagination,
    IconsEdit,
    UploadPic
  },
  filters: {
    formatTDate: value => {
      if (value != null) {
        var d = value.split("T");
      }
      return d ? d[0] : value;
    }
  },
  data() {
    var checkName = (rule, value, callback) => {
      const phoneReg = /^1[3|4|5|6|7|8|9][0-9]{9}$/;
      const emailReg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
      if (!value) {
        return callback(new Error("账号不能为空"));
      }
      if (phoneReg.test(value)) {
        callback();
      }
      // if (emailReg.test(value)) {
      //   callback()
      // }
      else {
        callback(new Error("手机号码不正确"));
      }
    };
    var validatePass = (rule, value, callback) => {
      const reg = /^(?=.*[a-zA-Z])(?=.*\d)(?=.*[~!@#$%^&*()_+`\-={}:";'<>?,.\/]).{8,16}$/;
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        if (reg.test(value)) {
          callback();
        } else {
          callback(new Error("必须包含字母、数字、特殊符号、8~16位数"));
        }
      }
    };
    return {
      loading: false,
      saveBtn: false,
      btnAip: "",
      setimgs: false,
      // iconfontVisible: false, //选择头像
      iconfontImg: '<i class="el-icon-plus" />',
      iconFlag: true,
      // iconfontImg: '<i class="el-icon-plus" />',
      // svgIcons,
      // icon: true,
      // elementIcons,
      pickerOptions: {
        shortcuts: [
          {
            text: "最近一周",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit("pick", [start, end]);
            }
          },
          {
            text: "最近一个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit("pick", [start, end]);
            }
          },
          {
            text: "最近三个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit("pick", [start, end]);
            }
          }
        ]
      },
      headerHeight: "150px",
      mainHeight: "",
      footerHeight: "58px",
      tableHeight: "500",
      labelPosition: "left",
      tableData: [], // table数据模型
      formData: {
        id: -1,
        telAccount: "",
        emailAccount: "",
        passwd: "",
        accountName: "",
        userInfoId: 0,
        status: 1,
        expDate: "",
        jobNumber: "",
        realName: "",
        fixedNumber: "",
        address: "",
        headPicPath: "",
        remarks: "",
        cId: 0,
        roleId: 0,
        deptId: 0
      },
      formRules: {
        telAccount: [
          { required: true, message: "请输入手机账号", trigger: "blur" },
          { required: true, validator: checkName, trigger: "blur" }
        ],
        emailAccount: [
          { required: true, message: "请输入邮箱账号", trigger: "blur" },
          {
            type: "email",
            message: "请输入正确的邮箱地址",
            trigger: ["blur", "change"]
          }
        ],
        accountName: [
          { required: true, message: "请输入账户姓名", trigger: "blur" }
        ],
        fixedNumber: [
          {
            min: 8,
            max: 11,
            message: "请输入8-11位数的固定号码",
            trigger: "blur"
          }
        ],
        passwd: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { required: true, validator: validatePass, trigger: "blur" }
        ],
        address: [
          {
            min: 0,
            max: 100,
            message: "最大允许输入100个字符，请重新输入！",
            trigger: "blur"
          }
        ],
        remarks: [
          {
            min: 0,
            max: 100,
            message: "最大允许输入100个字符，请重新输入！",
            trigger: "blur"
          }
        ]
      },
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      sortColumn: "", // 排序字段
      sortOrder: "", // 排序规则
      menuName: "", // 菜单名称查询条件
      menuStatus: "全部", // 菜单状态
      Status: 1, // 账号状态（0无效，1有效，2过期)
      keyWords: "", // 账号
      userName: "", //姓名
      createTime: "",
      doState: 0,
      pswOnly: true,
      imgsrc: this.imgUrlName,
      imageUrl: "",
      roleList: [], // 角色列表
      departmentList: [], // 部门列表
      roleCheck: [],
      departmentCheck: [],
      SubmitPostDataEdit: {},
      PostDataEdit: {}
    };
  },
  watch: {
    dialogVisible(val) {
      if (val) {
        // setTimeout(() => {
        // this.pswOnly = false;
        // }, 500);
      } else {
        this.saveBtn = false;
        this.pswOnly = true;
        this.$refs.formData.resetFields();
      }
    }
  },
  async created() {
    // this.loading = true;
    this.loading = true;
    this.roleList = await this.getRoleList(); // 获取角色数据
    this.departmentList = await this.getDepartment(); // 获取部门数据
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    );
    this.getTree(); // 获取用户数据
    // this.loading = false;
  },
  mounted() {
    this.setStyle();
  },
  methods: {
    pagination(val) {
      // 改变页数
      this.pageSize = val.pageSize;
      this.pageIndex = val.pageIndex;
      this.getTree();
    },
    doopen() {
      this.$refs.UploadPic.doOpen();
    },
    upLoadClose(data) {
      this.formData.headPicPath = data;
      // this.iconfontImg = 'http://47.107.135.203:20200/' + data;
      // this.iconfontImg = this.$store.user.imgUrlName + data;
      // this.userForm.headPicPath = data;
    },
    selectIco() {
      // 选择图标
      // this.IcoDialogVisible = true;
      this.$refs.IconsEdit.doOpen();
    },
    doIcoSelect(text, svgText) {
      if (text.indexOf("el-icon") != -1) {
        this.iconFlag = true;
        this.formData.headPicPath = text;
      } else {
        this.iconFlag = false;
        this.formData.headPicPath = svgText;
      }
      this.$refs.IconsEdit.doClose();
    },

    // handleClipboard(text, event) {
    //   this.iconfontVisible = false;
    //   this.iconfontImg = text;
    // },
    // generateIconCode(symbol) {
    //   this.icon = false;
    //   return symbol;
    // },
    // generateElementIconCode(symbol) {
    //   this.icon = true;
    //   return `<i class="el-icon-${symbol}" />`;
    // },
    pswClick() {
      // 解决浏览器默认密码
      if (this.pswOnly) {
        this.pswOnly = false;
      }
    },
    getRoleList() {
      // 角色列表
      return new Promise((reslove, reject) => {
        var rqs = RequestObject.CreateGetObject();
        request({
          url: "system/api/TSMRoles",
          method: "GET",
          params: { RequestObject: JSON.stringify(rqs) }
        }).then(res => {
          if (res.code === 0) {
            // this.roleList = res.data;
            reslove(res.data);
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          }
        });
      });
    },
    getDepartment() {
      // b部门列表
      return new Promise((reslove, reject) => {
        var rqs = RequestObject.CreateGetObject();
        request({
          url: "system/api/TSMDept/GetOnlyDepAsync",
          method: "GET",
          params: { RequestObject: JSON.stringify(rqs) }
        }).then(res => {
          if (res.code === 0) {
            // this.departmentList = res.data;
            reslove(res.data)
          } else {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          }
        });
      });
    },
    getTreeDefaule() {
      this.pageIndex = 1;
      this.getTree();
    },
    beforeAvatarUpload(file) {
      // 上传图片验证格式
      const isJPG = file.type === "image/jpeg";
      const isPNG = file.type === "image/png";
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isJPG && !isPNG) {
        this.$message.error("上传头像图片只能是 JPG,PNG 格式!");
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 2MB!");
      }
      return (isJPG || isPNG) && isLt2M;
    },
    handleAvatarSuccess(res, file) {
      // 上传图片
      // console.log("进入此方法");
      this.setdeleat = true;
      // console.log(this.imageUrl);
      // console.log(res);
      // console.log(file);
      this.imgsrc = this.imgUrlName;
      this.formData.headPicPath = res;
      this.imageUrl = " this.imgsrc + res";
    },
    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var navbar = document.getElementById("navbar_yfkj");
        var nv = navbar.clientHeight || navbar.offsetHeight;
        var b = document.documentElement.clientHeight - nv;
        var elheader = document.getElementById("elheader");
        var elfooter = document.getElementById("elfooter");
        var h = elheader.clientHeight || elheader.offsetHeight;
        var f = elfooter.clientHeight || elfooter.offsetHeight;
        var pt = getStyle(document.getElementById("elmain"), "paddingTop");
        var pb = getStyle(document.getElementById("elmain"), "paddingBottom");
        this.tableHeight = b - h - f - parseInt(pt) - parseInt(pb);
      });
    },
    changeTel() {
      // 电话变化
      this.formRules.emailAccount[0].required = false;
      this.formRules.telAccount[0].required = true;
    },
    changeEmail() {
      // 电话变化
      this.formRules.telAccount[0].required = false;
      this.formRules.emailAccount[0].required = true;
    },
    formatDate(date) {
      // 格式化日期
      var format = "";
      var oY = "";
      var oM = "";
      var oD = "";
      var oDate = new Date(date);
      oY = oDate.getFullYear();
      oM = oDate.getMonth() + 1;
      oM = oM < 10 ? "0" + oM : oM;
      oD = oDate.getDate();
      oD = oD < 10 ? "0" + oD : oD;
      format = oY + "-" + oM + "-" + oD;
      return format;
    },
    statusChange() {
      // 查询状态搜索
      this.getTree();
    },
    getTree() {
      this.loading = true;
      // 获取列表数据
      var param = [{ column: "status", content: this.Status, condition: 0 }];
      if (this.createTime != null) {
        if (this.createTime[0]) {
          var startDate = this.formatDate(this.createTime[0]) + " 00:00:00";
          var endDate = this.formatDate(this.createTime[1]) + " 23:59:59";
          param.push({
            column: "createTime",
            content: this.createTime[0] ? startDate + "," + endDate : "",
            condition: 10
          });
        }
      }

      if (this.userName != "") {
        param.push({
          column: "accountName",
          content: trim(this.userName),
          condition: 6
        });
      }
      if (this.keyWords != "") {
        param.push({
          column: "keyWords",
          content: trim(this.keyWords),
          condition: 6
        });
      }
      var robject = RequestObject.CreateGetObject(
        true,
        this.pageSize,
        this.pageIndex,
        param
      );
      request({
        url: "/system/api/TSMUser",
        method: "get",
        params: {
          requestObject: robject
        }
      }).then(res => {
        if (res.code === 0) {
          this.tableData = this.handelData(res.data);
          this.pageSize = res.pageSize;
          this.pageIndex = res.pageIndex;
          this.totalCount = res.totalNumber;
          this.loading = false;
        } else {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          this.loading = false;
        }
      });
    },
    handelData(data) {
      data.map(item => {
        if (
          item.headPicPath != null &&
          item.headPicPath.indexOf("el-icon") != -1
        ) {
          item.iconFlag = true;
        } else {
          item.iconFlag = false;
        }
      });
      return data;
    },
    GetPostData() {
      // 处理添加 post请求数据
      var result = {
        telAccount: this.formData.telAccount,
        emailAccount: this.formData.emailAccount,
        passwd: this.formData.passwd,
        accountName: this.formData.accountName,
        status: this.formData.status,
        expDate: this.formData.expDate,
        jobNumber: this.formData.jobNumber,
        realName: this.formData.realName,
        fixedNumber: this.formData.fixedNumber,
        address: this.formData.address,
        headPicPath: this.formData.headPicPath,
        remarks: this.formData.remarks,
        roleId: this.roleCheck[this.roleCheck.length - 1],
        deptId: this.departmentCheck[this.departmentCheck.length - 1]
      };

      for (var i in result) {
        this.SubmitPostDataEdit[i] = this.PostDataEdit[i];
      }
      // console.log(this.SubmitPostDataEdit)
      return result;
    },
    SetFromData() {
      this.formData.telAccount = "";
      this.formData.emailAccount = "";
      this.formData.passwd = "";
      this.formData.accountName = "";
      this.formData.jobNumber = "";
      this.formData.realName = "";
      this.formData.expDate = "";
      this.formData.status = 1;
      this.formData.fixedNumber = "";
      this.formData.address = "";
      this.formData.headPicPath = "";
      this.formData.remarks = "";
    },
    OnBtnSaveSubmit() {
      // 新增编辑保存操作
      if (this.doState == 3) {
        // 删除
        this.doConfirm();
        return;
      }
      this.$refs.formData.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
        } else {
          this.doConfirm();
        }
      });
    },
    doConfirm() {
      if (this.doState == 1) {
        this.saveBtn = true;
        // 添加
        var currData = this.GetPostData();
        var currMethod = "post";
        var URL = "/system/api/TSMUser";
        currData.expDate =
          currData.expDate != "" ? this.formatDate(currData.expDate) + "" : "";
      }
      if (this.doState == 2) {
        this.saveBtn = true;
        // 修改
        var currMethod = "put";
        var URL = "/system/api/TSMUser";
        var currData = this.GetPostData();
        currData.id = this.formData.id;
        currData.cId = this.formData.cId;
        currData.userInfoId = this.formData.userInfoId;
        currData.expDate =
          currData.expDate != null
            ? this.formatDate(currData.expDate) + ""
            : null;

        this.SubmitPostDataEdit["id"] = this.PostDataEdit["id"];
        this.SubmitPostDataEdit["cId"] = this.PostDataEdit["cId"];
        this.SubmitPostDataEdit["userInfoId"] = this.PostDataEdit["userInfoId"];
        this.SubmitPostDataEdit["expDate"] = this.PostDataEdit["expDate"];

        // console.log(currData)
        // console.log(this.SubmitPostDataEdit)
        // return
      }
      if (this.doState == 3) {
        // 删除
        var currMethod = "delete";
        var URL = "/system/api/TSMUser/ById";
        var currData = this.formData.id;
      }
      // console.log(currData.roleId)
      // console.log(currData.deptId)
      // console.log(this.roleCheck)
      // console.log(this.departmentCheck)
      // console.log(this.doState)
      var data = RequestObject.CreatePostObject(currData);
      if (this.doState == 2) {
        // console.log("------------");
        var data = RequestObject.CreatePostObject(
          currData,
          null,
          this.SubmitPostDataEdit
        );
      } else {
        var data = RequestObject.CreatePostObject(currData);
      }
      request({
        url: URL,
        method: currMethod,
        data: data
      }).then(res => {
        if (res.code == -1) {
          // var setTime = setTimeout(() => {
          if (res.info.indexOf("Duplicate entry") != -1) {
            this.$confirm("账号已存在", "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
            this.saveBtn = false;
            return;
          }
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
          this.saveBtn = false;
          //   clearTimeout(setTime);
          // });
        } else {
          if (this.doState == 2) {
            // this.$store.dispatch('user/setAvatar',res.postData.headPicPath) //设置头像
          }
          var setTime = setTimeout(() => {
            this.getTree();
            this.$message({
              message: "操作成功",
              type: "success"
            });
            this.saveBtn = false;

            this.dialogVisible = false;
            clearTimeout(setTime);
          }, 50);
        }
      });
    },
    handleQuery() {},
    handelCloseClick() {
      this.dialogVisible = false;
    },
    findIdArr(arrlist, id, name) {
      var zjList = [];
      for (var k = 0; k < arrlist.length; k++) {
        if (arrlist[k][name] == id) {
          zjList.push(arrlist[k][name]);
        } else {
          let lt = [];
          lt.push(arrlist[k][name]);
          if (arrlist[k]["children"]) {
            getList(arrlist[k]["children"], id, lt);
          }
        }
      }

      function getList(arrlist, id, lt) {
        for (var i = 0; i < arrlist.length; i++) {
          var Lf = [...lt];
          Lf.push(arrlist[i][name]);
          if (arrlist[i][name] == id) {
            lt.push(arrlist[i][name]);
            zjList = lt;
            return lt;
          }
          if (arrlist[i].children.length > 0) {
            getList(arrlist[i].children, id, Lf);
          }
        }
      }
      // console.log(zjList);
      return zjList;
    },
    handelClick(state, item) {
      this.doState = state;
      if (state == 1) {
        this.dialogVisible = true;
        // 新增初始化数据
        this.formData.id = -1;
        this.formData.telAccount = "";
        this.formData.emailAccount = "";
        this.formData.passwd = "";
        this.formData.accountName = "";
        this.formData.jobNumber = "";
        this.formData.realName = "";
        this.formData.expDate = "";
        this.formData.status = 1;
        this.formData.fixedNumber = "";
        this.formData.address = "";
        this.iconFlag = true;
        this.formData.headPicPath = "";
        this.formData.remarks = "";
        this.formData.userInfoId = 0;
        this.cId = 0;
        this.imageUrl = "";
        this.roleCheck = [];
        this.departmentCheck = [];
        this.roleId = 0;
        this.deptId = 0;
      }

      if (state == 2) {
        // console.log(item)
        this.PostDataEdit = {};
        this.PostDataEdit = item;
        // 编辑初始化数据
        this.dialogVisible = true;
        this.formData.id = item.id;
        this.formData.telAccount = item.telAccount;
        this.formData.emailAccount = item.emailAccount;
        this.formData.passwd = item.passwd;
        this.formData.accountName = item.accountName;
        this.formData.jobNumber = item.jobNumber;
        this.formData.realName = item.realName;
        this.formData.expDate =
          item.expDate != null ? new Date(item.expDate) : null;
        this.formData.status = item.status;
        this.formData.fixedNumber = item.fixedNumber;
        this.formData.address = item.address;
        // this.formData.headPicPath =item.headPicPath != null ? item.headPicPath : "";
        // this.formData.headPicPath =(item.headPicPath != null&&item.headPicPath) != "" ? item.headPicPath : "<i class='el-icon-plus' />";
        this.formData.remarks = item.remarks;
        this.formData.cId = item.cId;
        this.formData.userInfoId = item.userInfoId;
        this.roleCheck = [];
        this.departmentCheck = [];
        // this.roleCheck.push(item.roleId);
        // this.departmentCheck.push(item.deptId);
        this.roleCheck = this.findIdArr(this.roleList, item.roleId, "id");
        this.departmentCheck = this.findIdArr(
          this.departmentList,
          item.deptId,
          "actualId"
        );
        this.roleId = item.roleId;
        this.deptId = item.deptId;
        // console.log(this.roleCheck);
        // console.log(this.departmentCheck);

        // console.log(item)

        // this.iconFlag = item.iconFlag; // 设置头像显示代码（svg or i）
        // if (item.headPicPath == "" || item.headPicPath == null) {
        //   this.iconFlag = true;
        //   this.formData.headPicPath = "";
        // } else {
        //   this.formData.headPicPath = item.headPicPath;
        // }
        this.formData.headPicPath = item.headPicPath;

        if (item.telAccount != "") {
          this.changeTel();
        }
        if (item.emailAccount != null) {
          this.changeEmail();
        }
      }

      if (state == 3) {
        // 删除
        this.formData.id = item.id;
        this.$confirm("是否删除用户？", "确认信息", {
          type: "warning"
        }).then(() => {
          this.OnBtnSaveSubmit();
        });
      }
    }
  }
};
</script>
<style lang="scss" scoped>
#menus /deep/ {
  .header {
    padding-top: 10px;
  }
  .el-header .el-form {
    border-bottom: 1px solid #eee;
    margin-bottom: 20px;
  }
  .el-tag {
    cursor: pointer;
  }
  .ellipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    display: block;
    width: 100%;
    cursor: pointer;
  }
  .iconSty {
    width: 20px;
    height: 20px;
    vertical-align: middle;
  }
  .textCenter {
    text-align: center;
  }
  .dialog-footer {
    text-align: right;
  }
  .el-dialog-div {
    height: 60vh;
    overflow: auto;
    padding: 0px 20px;
  }
  .el-dialog--center .el-dialog__body {
    padding: 20px 0px 0px;
  }
  .avatar {
    width: 40px;
    height: 40px;
    border: 1px solid #dcdfe6;
    font-size: 22px;
    text-align: center;
    line-height: 38px;
  }
  .avatarTable2 {
    width: 30px;
    height: 30px;
  }
  .avatarTable {
    width: 40px;
    height: 40px;
    line-height: 36px;
    font-size: 30px;
    border-radius: 50%;
    overflow: hidden;
    border: 1px solid #eee;
    vertical-align: middle;
    // border: 1px solid #dcdfe6;
  }
  .avatarTable_ {
    width: 30px;
    height: 30px;
    line-height: 36px;
    font-size: 30px;
    border-radius: 50%;
    overflow: hidden;
    border: 1px solid #eee;
    vertical-align: middle;
    // border: 1px solid #dcdfe6;
  }
  .el-dialog__header {
    text-align: left;
  }
  .el-dialog__title {
    color: #1890ff;
  }
}
.iconfont {
  font-size: 36px;
  color: #8c939d;
  width: 100%;
  height: 44px;
  display: flex;
  // line-height: 54px;
  text-align: center;
  display: flex;
  .iconfontBox {
    flex: 1;
    text-align: right;
    line-height: 0px;
    height: 26px;
    button {
      height: 26px;
      font-size: 14px;
      // border-radius: 3px;
    }
  }
  .el-form-item__label {
    padding-right: 4px;
  }
}
</style>
