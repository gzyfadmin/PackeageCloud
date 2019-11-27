<template>
  <div
    class="gary_bg"
    :style="this.height"
    v-loading="loading"
    style="box"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <!--注册头部信息-->
    <yfheader type="1"></yfheader>
    <!-- <div class="login_header">
      <div class="header_warp">
        <div class="header_left"><p class="header_logo"><img src="../../images/Login_logo_03.png" alt=""></p><p class="login_title">找回密码</p> </div>
        <div class="header_bnt">已有账号，立即<router-link to="/" class="lgoin_bnt">登录</router-link></div>
      </div>
    </div> -->
    <div class="formBox">
      <!-- <div class="process_warp">
        <el-steps :active="onSteps" align-center>
          <el-step title="获取验证码" />
          <el-step title="重置密码" />
        </el-steps>
      </div> -->
      <div class="process_warp">
        <!--流程-->
        <div
          class="proscess_line"
          :class="onSteps==1?'':'proscess_line_blue'"
        ></div>
        <div class="process_node choose">
          <p class="iconfont">{{onSteps==1?'&#xe6e1;':'&#xe6de;'}}</p>
          <span>获取验证码</span>
        </div>
        <div
          class="process_node align_right"
          :class="onSteps==1?'':'choose'"
        >
          <p class="iconfont">{{onSteps==1?'&#xe6de;':'&#xe6e1;'}}</p>
          <span>重置密码</span>
        </div>
      </div>
      <!-- 1 -->
      <el-form
        v-if="!next"
        ref="retrieveForm"
        size="medium"
        :model="retrieveForm"
        :rules="retrieveRules"
        class="retrieve-form"
        autocomplete="on"
        label-position="left"
      >
        <el-form-item
          label="手机号码："
          :label-width="formLabelWidth"
          prop="user"
        >
          <el-input
            style="width: 282px"
            size="medium"
            v-model="retrieveForm.user"
            placeholder="请输入手机号码"
          />
        </el-form-item>
        <el-form-item
          label="短信验证码："
          :label-width="formLabelWidth"
          prop="userNum"
        >
          <el-input
            style="width: 150px;margin-right:10px"
            size="medium"
            v-model="retrieveForm.userNum"
            placeholder="请输入验证码"
          ></el-input>
          <el-button
            style="width: 120px;height: 42px;font-size: 14px;"
            v-if="btnNum"
            plain
            @click="getNum"
          >获取验证码</el-button>
          <el-button
            style="width: 120px;height: 42px;font-size: 14px;"
            plain
            disabled
            v-if="!btnNum"
          >{{ num }}s后重新获取</el-button>
        </el-form-item>
        <el-form-item style="text-align: center; margin:40px 0px;padding-bottom:80px">
          <el-button
            style="width: 160px;height: 40px;font-size: 16px;"
            size="medium"
            type="primary"
            @click="nextStep"
          >下一步</el-button>
        </el-form-item>
      </el-form>
      <!-- 2 -->
      <el-form
        v-if="next"
        ref="passwordForm"
        size="medium"
        :model="passwordForm"
        :rules="passwordRules"
        class="retrieve-form"
        autocomplete="on"
        label-position="left"
      >
        <el-form-item
          label="新密码："
          prop="pass"
          :label-width="formLabelWidth"
        >
          <el-input
            size="medium"
            style="width: 282px"
            v-model="passwordForm.pass"
            type="password"
            autocomplete="off"
            placeholder="请输入至少8位数密码"
          />
        </el-form-item>
        <el-form-item
          label="确认密码："
          prop="checkPass"
          :label-width="formLabelWidth"
        >
          <el-input
            size="medium"
            style="width: 282px"
            v-model="passwordForm.checkPass"
            type="password"
            autocomplete="off"
            placeholder="请确认密码"
          />
        </el-form-item>
        <el-form-item style="text-align: center; margin:40px 0px;padding-bottom:80px">
          <el-button
            size="medium"
            style="width: 160px;height: 40px;font-size: 16px;"
            type="primary"
            @click="onComplete"
          >完成</el-button>
        </el-form-item>
      </el-form>
    </div>
    <!--底部信息-->
    <Yffooter></Yffooter>
    <!-- <el-footer height="50px">
      <div class="login_footer">Copyright ©2018 云飞科技湘ICP备14003093号-1</div>
    </el-footer> -->
  </div>
</template>

<script>
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import Yfheader from "@/components/Yfheader/index.vue";
import Yffooter from "@/components/Yffooter/index.vue";

export default {
  components: {
    Yfheader,
    Yffooter
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
      if (emailReg.test(value)) {
        callback();
      } else {
        callback(new Error("手机号码格式不正确"));
      }
    };
    var validatePass = (rule, value, callback) => {
      const reg = /^(?![a-zA-Z]+$)(?![A-Z0-9]+$)(?![A-Z._~!@#$^&*]+$)(?![a-z0-9]+$)(?![a-z._~!@#$^&*]+$)(?![0-9._~!@#$^&*]+$)[a-zA-Z0-9._~!@#$^&*]{8,16}$/;
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        this.$nextTick(function() {
          if (this.passwordForm.checkPass !== "") {
            this.$refs.passwordForm.validateField("checkPass");
          }
        });
        if (reg.test(value)) {
          callback();
        } else {
          callback(new Error("字母,数字,符号三个组合，不低于8位"));
        }
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.passwordForm.pass) {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };
    return {
      height: "",
      loading: false,
      next: false,
      onSteps: 1,
      formLabelWidth: "110px",
      btnNum: true,
      num: 120,
      phoneReg: /^1[3|4|5|6|7|8|9][0-9]{9}$/,
      emailReg: /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/,
      sucNum: "",
      suc: 180,
      retrieveForm: {
        user: "",
        userNum: ""
      },
      passwordForm: {
        pass: "",
        checkPass: ""
      },
      retrieveRules: {
        user: [{ required: true, validator: checkName, trigger: "change" }],
        userNum: [
          { required: true, message: "验证码不能为空", trigger: "change" }
        ]
      },
      passwordRules: {
        pass: [
          { required: true, message: "请输入密码", trigger: "change" },
          { validator: validatePass, trigger: "change" }
        ],
        checkPass: [
          { required: true, message: "请输入确认密码", trigger: "change" },
          { validator: validatePass2, trigger: "change" }
        ]
      }
    };
  },
  created() {
    this.setStyle();
  },
  methods: {
    setStyle() {
      this.$nextTick(() => {
        var navbar = document.getElementById("app");
        this.height = "height:" + navbar.clientHeight + "px";
      });
    },
    /**
     * getName
     * 判断账号是否存在
     */
    getName() {},
    /**
     * getNum
     * 获取验证码
     */
    getNum() {
      if (!this.phoneReg.test(this.retrieveForm.user)) {
        this.$message({
          message: "请输入正确的手机号",
          type: "error"
        });
      } else {
        const getData = {};
        var aipUrl = "";
        var data = RequestObject.CreatePostObject(getData, null, null);
        if (this.phoneReg.test(this.retrieveForm.user)) {
          getData.mobile = this.retrieveForm.user;
          aipUrl = "/system/api/Sms/PostForget";
        }
        if (this.emailReg.test(this.retrieveForm.user)) {
          getData.email = this.retrieveForm.user;
          aipUrl = "/system/api/Email";
        }
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
              this.btnNum = false;
              this.num = 120;
              var time = setInterval(() => {
                this.num--;
                if (this.num == 0) {
                  clearInterval(time);
                  this.btnNum = true;
                }
              }, 1000);
              // this.suc = 180;
              // var time = setInterval(() => {
              //   this.suc--;
              //   if (this.suc == 0) {
              //     clearInterval(time);
              //     this.sucNum = "";
              //   }
              // }, 1000);
            }
          })
          .catch(error => console.log(error));
      }
    },
    /**
     * nextStep
     * 下一步
     */
    nextStep() {
      this.$refs.retrieveForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法进行下一步",
            type: "error"
          });
        } else {
          // if (this.retrieveForm.userNum != this.sucNum) {
          //   this.$message.error("验证码不正确");
          //   return;
          // }
          var requestData = RequestObject.CreateGetObject(false, 0, 0, [
            {
              column: "code",
              content: this.retrieveForm.userNum,
              condition: 0
            },
            {
              column: "mobile",
              content: this.retrieveForm.user,
              condition: 0
            }
          ]);
          request({
            url: "/system/api/Sms/GetVerificationCodeForget",
            method: "get",
            params: { RequestObject: JSON.stringify(requestData) }
          })
            .then(res => {
              if (res.code === -1) {
                this.$confirm(res.info, "错误信息", {
                  confirmButtonText: "确定",
                  type: "error",
                  showCancelButton: false
                });
              } else {
                if (res.data == 0) {
                  this.$message({
                    message: "验证码失效，请重新获取",
                    type: "error"
                  });
                  return;
                }
                if (res.data == 2) {
                  this.$message({
                    message: "验证码不正确，请重新输入",
                    type: "error"
                  });
                  return;
                }
                if (res.data == 1) {
                  this.getUset();
                }
              }
            })
            .catch(error => console.log(error));
        }
      });
    },
    /**
     * getUset
     * 验证账号存不存在
     */
    getUset() {
      const userData = {
        keyWords: this.retrieveForm.user
      };
      var data = RequestObject.CreatePostObject(userData);
      request({
        url: "/system/api/CheckAccount",
        method: "post",
        data: data
      })
        .then(res => {
          if (!res.data) {
            this.$message.error("账号不存在");
          } else {
            this.sucNum = "";
            this.next = true;
            this.onSteps++;
          }
        })
        .catch(error => console.log(error));
    },
    /**
     * onComplete
     * 完成
     */
    onComplete() {
      this.$refs.passwordForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法保存",
            type: "error"
          });
        } else {
          this.loading = true;
          const completeData = {
            id: 0,
            telAccount: "",
            emailAccount: "",
            passwd: this.passwordForm.pass
          };
          if (this.phoneReg.test(this.retrieveForm.user)) {
            completeData.telAccount = this.retrieveForm.user;
          }
          if (this.emailReg.test(this.retrieveForm.user)) {
            completeData.emailAccount = this.retrieveForm.user;
          }
          var data = RequestObject.CreatePostObject(completeData);
          request({
            url: "/system/api/TSMUserAccount/ResetPassword",
            method: "put",
            data: data
          })
            .then(res => {
              this.loading = false;
              if (!res.data) {
                this.$message.error("账号不存在");
              } else {
                this.$message({
                  message: "修改密码成功",
                  type: "success"
                });
                this.$router.push({ path: "/" });
              }
            })
            .catch(error => {
              this.loading = false;
            });
        }
      });
    }
  }
};
</script>
<style lang="scss" scoped>
@import "../../styles/login.css";
@import "../../styles/login_CSS.css";
@import "../../styles/icon/iconfont.css";
.formBox {
  width: 900px;
  margin: 0 auto;
  margin-top: 50px;
  background: #fff;
  .process_warp {
    margin: 0 110px;
  }
  .retrieve-form {
    margin: 50px 240px;
    .el-form-item {
      // padding: 0 60px;
    }
  }
}
/deep/.el-input--medium .el-input__inner {
  height: 42px;
  line-height: 42px;
  // width: 282px !important;
}
/deep/.el-button--medium {
  padding: 10px 10px;
  font-size: 14px;
  border-radius: 4px;
}
.login_footer {
  width: 100%;
  position: fixed;
  bottom: 0;
  margin: 0 auto;
  height: 50px;
  line-height: 50px;
}
</style>

