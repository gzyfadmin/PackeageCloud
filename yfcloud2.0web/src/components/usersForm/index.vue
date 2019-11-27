<template>
  <div>
    <el-dialog
      title="账号绑定"
      :visible.sync="accountVisible"
      label-position="left"
      width="30%"
      :close-on-click-modal="false"
    >
      <el-form
        ref="accountForm"
        :model="accountForm"
        size="medium"
        label-width="105px"
        style="padding-top: 15px;padding-right: 30px;height:150px"
      >
        <el-form-item
          v-if="account==1"
          label="手机账号:"
          prop="accountName"
          :rules="[
            { required: true, message: '请输入账号', trigger: 'blur' },
            { pattern: /^1[3|4|5|6|7|8][0-9]{9}$/, message: '请输入正确的手机号', trigger: ['blur', 'change'] }
          ]"
        >
          <div class="no-input">
            <el-input v-model="accountForm.accountName" />
          </div>
        </el-form-item>
        <el-form-item
          v-if="account==2"
          label="邮箱账号："
          prop="accountName"
          :rules="[
            { required: true, message: '请输入账号', trigger: 'blur' },
            { type: 'email', message: '请输入正确的邮箱', trigger: ['blur', 'change'] }
          ]"
        >
          <div class="no-input">
            <el-input v-model="accountForm.accountName" />
          </div>
        </el-form-item>
        <el-form-item
          label="获取验证码:"
          prop="bindNum"
        >
          <div style="display: flex;">
            <div style="padding-right: 30px;">
              <el-input v-model="accountForm.bindNum" />
            </div>
            <el-button
              v-if="btnNum"
              @click="onBindNum"
            >获取验证码</el-button>
            <el-button
              v-if="!btnNum"
              style="background-color: #f5f7fa;"
            >{{ num }}s后重新获取</el-button>
          </div>
        </el-form-item>
      </el-form>
      <div
        slot="footer"
        class="dialog-footer"
      >
        <el-button @click="accountVisible=false">关闭</el-button>
        <el-button
          type="primary"
          @click="accountUser"
        >确 定</el-button>
      </div>
    </el-dialog>
    <el-dialog
      title="个人信息"
      :visible.sync="userFormVisible"
      width="600px"
      class="userForm"
      :close-on-click-modal="false"
    >
      <UploadPic
        ref="Upload"
        @upLoadClose="upLoadClose"
      />
      <div class="users">
        <div class="usersTap">
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
              :class="tapNum==2? 'color':''"
              @click="tapNum=2"
            >
              个人信息
            </div>
          </div>
          <div>
            <div
              :class="tapNum==3? 'color':''"
              @click="tapNum=3"
            >
              修改头像
            </div>
          </div>
        </div>
        <el-form
          ref="userForm"
          :model="userForm"
          size="medium"
          label-width="140px"
          style="padding-top: 15px;height:500px; overflow-y: scroll; padding-left: 20px;padding-right: 80px;"
          :rules="userRule"
        >
          <div v-if=" tapNum==1">
            <el-form-item
              label="姓名："
              prop="accountName"
            >
              <el-input v-model="userForm.accountName" />
            </el-form-item>
            <el-form-item
              label="手机账号："
              prop="telAccount"
            >
              <div class="inputBox">
                <el-input
                  v-model="userForm.telAccount"
                  readonly
                  class="my-input"
                />
                <a
                  class="bind"
                  @click="openBind(1)"
                >{{ telText }}</a>
              </div>
            </el-form-item>
            <el-form-item
              label="邮箱账号："
              prop="emailAccount"
            >
              <div class="inputBox">
                <el-input
                  v-model="userForm.emailAccount"
                  readonly
                  class="my-input"
                />
                <a
                  class="bind"
                  @click="openBind(2)"
                >{{ emailText }}</a>
              </div>
            </el-form-item>
            <el-form-item
              label="性别："
              prop="sex"
            >
              <el-radio
                v-model="userForm.sex"
                label="1"
              >男</el-radio>
              <el-radio
                v-model="userForm.sex"
                label="0"
              >女</el-radio>
            </el-form-item>
            <el-form-item
              label="入职时间："
              prop="entryTime"
            >
              <div class="bbfe6">{{ userForm.entryTime }}</div>
              <!-- <el-input
                v-model="userForm.entryTime"
                placeholder="请输入日期格式为‘2019-08-02’"
                @change="entryTimeChange"
              /> -->
              <!-- <el-date-picker
                v-model="userForm.entryTime"
                type="date"
                placeholder="选择日期"
                @change="entryTimeChange"
              >
              </el-date-picker> -->
            </el-form-item>
            <el-form-item
              label="工号："
              prop="workNo"
            >
              <div class="bbfe6">{{ userForm.workNo }}</div>
              <!-- <el-input v-model="userForm.workNo" /> -->
            </el-form-item>
            <el-form-item
              label="部门："
              prop="deptId"
            >
              <div class="bbfe6">{{ userForm.deptName }}</div>
              <!-- <el-input v-model="userForm.deptName" /> -->
              <!-- <el-cascader
                v-model="userForm.deptId"
                style="width:100%"
                :options="allDept"
                :props="{ checkStrictly:true,value:'actualId',label:'deptName' }"
                clearable
              /> -->
            </el-form-item>
            <el-form-item
              label="角色："
              prop="roleId"
            >
              <div class="bbfe6">{{ userForm.roleName }}</div>
              <!-- <el-input v-model="userForm.roleName" /> -->
              <!-- <el-cascader
                v-model="userForm.roleId"
                style="width:100%"
                :options="outTableData"
                :props="{ checkStrictly:true,value:'id',label:'roleName' }"
                clearable
              /> -->
            </el-form-item>
            <el-form-item
              label="状态："
              prop="status"
            >
              <div class="bbfe6">{{ ['无效','有效','过期'][userForm.status] }}</div>
            </el-form-item>
          </div>
          <div v-if="tapNum==2">
            <el-form-item
              label="身份证号码："
              prop="idCard"
            >
              <el-input v-model="userForm.idCard" />
            </el-form-item>
            <el-form-item
              label="出生日期："
              prop="birthday"
            >
              <el-date-picker
                v-model="userForm.birthday"
                type="date"
                placeholder="选择日期"
                @change="birthdayChange"
              >
              </el-date-picker>
              <!-- <el-input
                v-model="userForm.birthday"
                placeholder="请输入日期格式为‘2019-08-02’"
                @change="birthdayChange"
              /> -->
            </el-form-item>
            <el-form-item
              label="年龄："
              prop="age"
            >
              <!-- <el-input
                v-model="userForm.age"
                readonly
              /> -->
              <div class="bbfe6">{{ userForm.age }}</div>
            </el-form-item>
            <el-form-item
              label="学历："
              prop="education"
            >
              <el-select
                v-model="userForm.education"
                style="width: 100%;"
                placeholder="请选择"
              >
                <el-option
                  v-for="item in education"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
            <el-form-item
              label="婚姻状态："
              prop="marriage"
            >
              <el-radio
                v-model="userForm.marriage"
                label="1"
              >未婚</el-radio>
              <el-radio
                v-model="userForm.marriage"
                label="2"
              >已婚</el-radio>
              <el-radio
                v-model="userForm.marriage"
                label="3"
              >离异</el-radio>
            </el-form-item>
            <el-form-item
              label="户口类型："
              prop="registeredType"
            >
              <el-select
                v-model="userForm.registeredType"
                style="width: 100%;"
                placeholder="请选择"
              >
                <el-option
                  v-for="item in nativePlace"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
            <el-form-item
              label="民族："
              prop="nation"
            >
              <el-input v-model="userForm.nation" />
            </el-form-item>
            <el-form-item
              label="家庭地址："
              prop="homeAddress"
            >
              <el-input v-model="userForm.homeAddress" />
            </el-form-item>
            <el-form-item
              label="司龄："
              prop="entryAge"
            >
              <!-- <el-input
                v-model="userForm.entryAge"
                readonly
              /> -->
              <div class="bbfe6">{{ userForm.entryAge }}</div>
            </el-form-item>
            <el-form-item
              label="紧急联系人："
              prop="emergencyContact"
            >
              <el-input v-model="userForm.emergencyContact" />
            </el-form-item>
            <el-form-item
              label="紧急联系人电话："
              prop="emergencyContactaPhone"
            >
              <el-input v-model="userForm.emergencyContactaPhone" />
            </el-form-item>
            <el-form-item
              label="与紧急联系人关系："
              prop="emergencyRealtionShip"
            >
              <el-input v-model="userForm.emergencyRealtionShip" />
            </el-form-item>
          </div>
          <el-form-item
            v-if=" tapNum==3"
            label="头像："
            prop="headImg"
            class="headedImg"
          >
            <div
              class="iconfont"
              @click="openUpload"
            >
              <div
                v-if="!iconfontImg"
                style="border: 1px #c1c1bf dashed;width: 40px;height: 40px;text-align: center;line-height: 40px;"
              >
                <i class="el-icon-plus" />
              </div>
              <div
                v-if="iconfontImg !=''"
                class="headImg"
              >
                <img
                  :src="iconfontImg"
                  alt=""
                >
              </div>
            </div>
          </el-form-item>
        </el-form>
      </div>
      <div
        slot="footer"
        class="dialog-footer"
      >
        <el-button @click="childClick">关闭</el-button>
        <el-button
          type="primary"
          @click="reviseUser"
        >保存</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import UploadPic from "@/components/UploadPic";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import dateUtil from "@/utils/dateUtil";
export default {
  components: {
    UploadPic
  },
  data() {
    var regDate = (rule, value, callback) => {
      const Time = /^[1-9]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/;
      if (!value) {
        callback();
      }
      if (Time.test(value)) {
        callback();
      } else {
        callback(new Error("日期格式不正确"));
      }
    };
    var idCard = (rule, value, callback) => {
      const Time = /^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i;
      if (!value) {
        callback();
      }
      if (Time.test(value)) {
        callback();
      } else {
        callback(new Error("身份证格式不正确"));
      }
    };
    var checkPhone = (rule, value, callback) => {
      const phoneReg = /^1[3|4|5|6|7|8|9][0-9]{9}$/;
      if (!value) {
        callback();
      }
      if (phoneReg.test(value)) {
        callback();
      } else {
        callback(new Error("请输入正确的手机号"));
      }
    };
    return {
      userFormVisible: false,
      iconfontVisible: false,
      accountVisible: false,
      telText: "【绑定】",
      emailText: "【绑定】",
      account: "",
      id: 0,
      iconfontImg: "",
      picture: "20190729/274f8041d3f6449d8e4d942954ea3815.jpg",
      iconName: "",
      icon: true,
      revise: "post",
      tapNum: 1,
      num: 120,
      btnNum: true,
      sucNum: "",
      dataTime: /^[1-9]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/,
      phoneReg: /^1[3|4|5|6|7|8|9][0-9]{9}$/,
      emailReg: /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/,
      userForm: {
        accountName: "",
        address: "",
        age: "",
        birthday: "",
        cId: "",
        companyId: "",
        createTime: "",
        deptId: "",
        deptName: "",
        education: "",
        emailAccount: "",
        emergencyContact: "",
        emergencyContactaPhone: "",
        emergencyRealtionShip: "",
        entryAge: "",
        entryTime: "",
        expDate: "",
        fixedNumber: "",
        headPicPath: "",
        homeAddress: "",
        id: "",
        idCard: "",
        jobNumber: "",
        keyWords: "",
        marriage: "",
        nation: "",
        passwd: "",
        realName: "",
        registeredType: "",
        remarks: "",
        roleId: "",
        roleName: "",
        salt: "",
        sex: "",
        status: "",
        telAccount: "",
        workNo: "",
        workStatus: ""
      },
      postDataEdit: {},
      userRule: {
        accountName: [
          { required: true, message: "请输入姓名", trigger: "blur" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符", trigger: "blur" }
        ],
        // entryTime: [
        //   {
        //     required: false,
        //     validator: regDate,
        //     trigger: "blur",
        //     transform(value) {
        //       return value;
        //     }
        //   }
        // ],
        // birthday: [{ required: false, validator: regDate, trigger: "blur" }],
        idCard: [{ required: false, validator: idCard, trigger: "blur" }],
        emergencyContactaPhone: [
          {
            required: false,
            validator: checkPhone,
            trigger: ["blur", "change"]
          }
        ]
      },
      accountForm: {
        accountName: "",
        bindNum: ""
      },
      allDept: [],
      outTableData: [],
      education: [
        {
          value: "初中以下",
          label: "初中以下"
        },
        {
          value: "高中",
          label: "高中"
        },
        {
          value: "中技（中专/技校/职高）",
          label: "中技（中专/技校/职高）"
        },
        {
          value: "大专",
          label: "大专"
        },
        {
          value: "本科以上",
          label: "本科以上"
        }
      ],
      nativePlace: [
        {
          value: "本市城镇",
          label: "本市城镇"
        },
        {
          value: "本市农村",
          label: "本市农村"
        },
        {
          value: "外埠城镇",
          label: "外埠城镇"
        },
        {
          value: "外埠农村",
          label: "外埠农村"
        }
      ]
    };
  },
  watch: {
    accountVisible(val) {
      if (val) {
        setTimeout(() => {
          // this.pswOnly = false;
        }, 500);
      } else {
        // this.pswOnly = true
        this.btnNum = true;
        this.$refs.accountForm.resetFields();
      }
    }
  },
  methods: {
    /**
     * getAge
     * 计算日期
     */
    getAge(date, getDate) {
      // const birthdays = new Date(this.teacherBirthday.replace(/-/g, '/'))
      const birthdays = new Date(date);
      const d = new Date();
      const age =
        d.getFullYear() -
        birthdays.getFullYear() -
        (d.getMonth() < birthdays.getMonth() ||
        (d.getMonth() == birthdays.getMonth() &&
          d.getDate() < birthdays.getDate())
          ? 1
          : 0);
      if (getDate == 1) {
        this.userForm.entryAge = age;
      } else if (getDate == 2) {
        this.userForm.age = age;
      }
    },
    /**
     * entryTimeChange
     * 入职时间发生变化时触发
     */
    entryTimeChange(v) {
      this.getAge(v, 1);
      // if (this.dataTime.test(v)) {
      //   this.getAge(v, 1);
      // }
    },
    /**
     * birthdayChange
     * 生日发生变化时触发
     */
    birthdayChange(v) {
      this.getAge(v, 2);
      // if (this.dataTime.test(v)) {
      //   this.getAge(v, 2);
      // }
    },
    /**
     * openBind
     * 绑定账号（电话，邮箱）
     */
    openBind(index) {
      // this.$refs['accountForm'].resetFields()
      this.account = index;
      this.accountVisible = true;
    },
    /**
     * onBindNum
     * 获取验证码
     */
    onBindNum() {
      this.$refs.accountForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法获取验证码",
            type: "error"
          });
        } else {
          this.btnNum = false;
          this.num = 120;
          const time = setInterval(() => {
            this.num--;
            if (this.num == 0) {
              clearInterval(time);
              this.btnNum = true;
            }
          }, 1000);
          const getData = {};
          var aipUrl = "";
          if (this.phoneReg.test(this.accountForm.accountName)) {
            getData.mobile = this.accountForm.accountName;
            aipUrl = "/system/api/Sms/PostChangeMobile";
          }
          if (this.emailReg.test(this.accountForm.accountName)) {
            getData.email = this.accountForm.accountName;
            aipUrl = "/system/api/Email";
          }
          var data = RequestObject.CreatePostObject(getData);
          request({
            url: aipUrl,
            method: "post",
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.$message({
                  message: "获取验证码成功，请注意查收",
                  type: "success"
                });
                this.sucNum = res.data;
                this.suc = 180;
                var time = setInterval(() => {
                  this.suc--;
                  if (this.suc == 0) {
                    clearInterval(time);
                    this.sucNum = "";
                  }
                }, 1000);
              }
            })
            .catch(error => console.log(error));
        }
      });
    },
    /**
     * openUpload
     * 上传头像
     */
    openUpload() {
      this.$refs.Upload.dialogVisible = true;
    },
    upLoadClose(data) {
      // this.iconfontImg = 'http://47.107.135.203:20200/' + data;
      this.iconfontImg = this.$store.state.user.imgUrlName + data;
      this.userForm.headPicPath = data;
    },
    /**
     * getDept
     * 查询部门
     */
    getDept() {
      var reqObject = RequestObject.GetObject();
      request({
        url: "/system/api/TSMDept/GetOnlyDepAsync",
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
          this.allDept = res.data;
        }
      });
    },
    /**
     * InitTemplates
     * 角色
     */
    InitTemplates() {
      var reqObject = RequestObject.CreateGetObject();
      request({
        url: "/system/api/TSMRoles",
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
          this.outTableData = res.data;
        }
      });
    },
    // /**
    //  * childClick
    //  * 个人信息取消
    //  */
    childClick() {
      this.userFormVisible = false;
    },
    /**
     * Users
     * 查看个人信息
     */
    Users() {
      this.getDept();
      this.InitTemplates();
      this.getAge();
      // this.iconfontImg = ''
      var data = RequestObject.CreateQueryObject();
      request({
        url: "/system/api/TSMUser/GetCurentAsync",
        method: "get",
        params: {
          requestObject: JSON.stringify(data)
        }
      })
        .then(res => {
          if (res.code === -1) {
            this.$confirm(res.info, "错误信息", {
              confirmButtonText: "确定",
              type: "error",
              showCancelButton: false
            });
          } else {
            this.postDataEdit = res.data[0];
            var form = res.data[0];
            this.userForm.accountName = form.accountName;
            this.userForm.address = form.address;
            this.userForm.age = form.age;
            this.userForm.birthday = form.birthday;
            this.userForm.cId = form.cId;
            this.userForm.companyId = form.companyId;
            this.userForm.createTime = form.createTime;
            this.userForm.deptId = form.deptId;
            this.userForm.deptName = form.deptName;
            this.userForm.education = form.education;
            this.userForm.emailAccount = form.emailAccount;
            this.userForm.emergencyContact = form.emergencyContact;
            this.userForm.emergencyContactaPhone = form.emergencyContactaPhone;
            this.userForm.emergencyRealtionShip = form.emergencyRealtionShip;
            this.userForm.entryAge = form.entryAge;
            this.userForm.entryTime = form.entryTime;
            this.userForm.expDate = form.expDate;
            this.userForm.fixedNumber = form.fixedNumber;
            if (form.headPicPath) {
              this.iconfontImg = this.imgUrlName + form.headPicPath;
            } else {
              this.iconfontImg = "";
            }
            this.userForm.homeAddress = form.homeAddress;
            this.userForm.id = form.id;
            this.userForm.idCard = form.idCard;
            this.userForm.jobNumber = form.jobNumber;
            this.userForm.keyWords = form.keyWords;
            this.userForm.marriage = form.marriage;
            this.userForm.nation = form.nation;
            this.userForm.passwd = form.passwd;
            this.userForm.realName = form.realName;
            this.userForm.registeredType = form.registeredType;
            this.userForm.remarks = form.remarks;
            this.userForm.roleId = form.roleId;
            this.userForm.roleName = form.roleName;
            this.userForm.salt = form.salt;
            this.userForm.sex = form.sex;
            this.userForm.status = form.status;
            this.userForm.telAccount = form.telAccount;
            this.userForm.workNo = form.workNo;
            this.userForm.workStatus = form.workStatus;

            if (this.userForm.sex != "") {
              this.userForm.sex = JSON.stringify(this.userForm.sex);
            }
            if (this.userForm.sex == 0) {
              this.userForm.sex = "0";
            }
            if (this.userForm.telAccount) {
              this.telText = "【更换绑定】";
            }
            if (this.userForm.emailAccount) {
              this.emailText = "【更换绑定】";
            }
            if (this.userForm.entryTime) {
              this.userForm.entryTime = dateUtil.Format(
                new Date(this.userForm.entryTime),
                "yyyy-MM-dd"
              );
            }
            if (this.userForm.birthday) {
              this.userForm.birthday = dateUtil.Format(
                new Date(this.userForm.birthday),
                "yyyy-MM-dd"
              );
            }
            // this.iconfontImg = this.userForm.headPicPath
          }
        })
        .catch(error => console.log(error));
    },
    /**
     * accountUser
     * 修改账号
     */
    accountUser() {
      this.$refs.accountForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法修改账号",
            type: "error"
          });
        } else {
          if (this.accountForm.bindNum == "") {
            this.$message({
              message: "请输入验证码",
              type: "warning"
            });
            return;
          }
          const getData = {};
          var aipUrl = "";
          var postDataEdit = {};
          if (this.accountForm.bindNum) {
            getData.code = this.accountForm.bindNum;
          }
          if (this.phoneReg.test(this.accountForm.accountName)) {
            getData.mobile = this.accountForm.accountName;
            postDataEdit.mobile = this.userForm.telAccount;
            aipUrl = "/system/api/TSMUser/ModifyMobile";
          }
          if (this.emailReg.test(this.accountForm.accountName)) {
            getData.email = this.accountForm.accountName;
            postDataEdit.email = this.userForm.emailAccount;
            aipUrl = "/system/api/TSMUser/ModifyEmail";
          }
          var data = RequestObject.CreatePostObject(
            getData,
            null,
            postDataEdit,
            null
          );
          request({
            url: aipUrl,
            method: "put",
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                if(res.data == 2) {
                  this.$message({
                  message: "修改账号失败，输入的验证码不一致",
                  type: "error"
                });
                }
                if(res.data == 0) {
                  this.$message({
                  message: "修改账号失败，验证码失效",
                  type: "error"
                });
                }
                if(res.data == 3) {
                  this.$message({
                  message: "修改账号失败，手机号码被占用",
                  type: "error"
                });
                }
                if (res.data == 1) {
                  this.accountVisible = false;
                  this.sucNum = "";
                  this.Users();
                  this.$message({
                    message: "修改账号成功",
                    type: "success"
                  });
                }
              }
            })
            .catch(error => console.log(error));
        }
      });
    },
    /**
     * reviseUser
     * 修改个人信息
     */
    reviseUser() {
      this.$refs.userForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法进行下一步",
            type: "error"
          });
        } else {
          this.userFormVisible = false;
          if (this.userForm.sex) {
            this.userForm.sex = Number(this.userForm.sex);
          }

          // if (
          //   this.userForm.deptId != null &&
          //   this.userForm.deptId.constructor == Array
          // ) {
          //   this.userForm.deptId = this.userForm.deptId[
          //     this.userForm.deptId.length - 1
          //   ];
          // }
          // if (
          //   this.userForm.deptId != null &&
          //   this.userForm.roleId.constructor == Array
          // ) {
          //   this.userForm.roleId = this.userForm.roleId[
          //     this.userForm.roleId.length - 1
          //   ];
          // }
          if (this.iconfontImg) {
            this.userForm.headPicPath = this.userForm.headPicPath;
          }
          var data = RequestObject.GetObject(
            this.userForm,
            null,
            null,
            this.postDataEdit
          );
          request({
            url: "/system/api/TSMUser/PersonalSet",
            method: "put",
            data: data
          })
            .then(res => {
              if (res.code === -1) {
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                this.$store.dispatch(
                  "user/setAvatar",
                  this.userForm.headPicPath
                ); //系统页面全部按钮
                this.$message({
                  message: "修改个人信息成功",
                  type: "success"
                });
              }
            })
            .catch(error => console.log(error));
        }
      });
    }
  }
};
</script>
<style lang="scss" scoped>
.userForm /deep/ {
  .el-form-item--medium .el-form-item__content {
    height: 36px !important;
  }
  .bbfe6 {
    -webkit-appearance: none;
    background-color: #ffffff;
    background-image: none;
    border-radius: 4px;
    border: 1px solid #dcdfe6;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    color: #606266;
    display: inline-block;
    font-size: inherit;
    height: 36px;
    line-height: 36px;
    outline: none;
    padding: 0 15px;
    -webkit-transition: border-color 0.2s cubic-bezier(0.645, 0.045, 0.355, 1);
    transition: border-color 0.2s cubic-bezier(0.645, 0.045, 0.355, 1);
    width: 100%;
  }
  .el-date-editor.el-input,
  .el-date-editor.el-input__inner {
    width: 100%;
  }
  // .el-input__inner,
  // .el-checkbox__inner,
  // .el-textarea__inner {
  //   border-radius: 0;
  //   border: none;
  //   border-bottom: 1px solid #dcdfe6;
  // }
  .inputBox {
    display: flex;
    // border-bottom: 1px solid #dcdfe6;
    .bind {
      padding-left: 10px;
      color: #0080ff;
      width: 96px;
    }
    .my-input {
      // -web-kit-appearance: none;
      // -moz-appearance: none;
      // border: none;
      // outline: 0;
      flex: 1;
      height: 36px;
      // padding-left: 10px;
    }
  }
}
.users {
  // padding: 0px 30px;
  .el-select {
    .el-select-dropdown__wrap {
      width: 200px;
    }
    .el-input {
      width: 100%;
    }
  }
  .headImg {
    // background: url("../../images/timg.jpg") no-repeat;
    // background-size: 100% 100%;
    // width: 100px;
    // height: 100px;
    border: 1px solid #eee;
    width: 150px;
    height: 150px;
    border-radius: 50%;
    overflow: hidden;
  }
  .headedImg {
    // margin-top: 60px;
  }
  .usersTap {
    display: flex;
    // margin-bottom: 25px;
    margin: 0px 20px 25px;
    border-bottom: 1px solid #dfdfdf;
    div {
      font-size: 16px;
      width: 100px;
      height: 40px;
      line-height: 40px;
      text-align: center;
      cursor: pointer;
      position: relative;
      top: 1px;
    }
    .color {
      color: #409eff;
      border-bottom: 2px solid #409eff;
      // width: 70px;
      // line-height: 40px;
      // height: 40px;
    }
  }
}
// .no-input /deep/ {
//   .el-input__inner,
//   .el-checkbox__inner,
//   .el-textarea__inner {
//     border-radius: 0;
//     border: none;
//     border-bottom: 1px solid #dcdfe6;
//   }
// }
</style>
