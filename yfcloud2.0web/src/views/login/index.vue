<template>
  <div
    class="gary_bg"
    :style="this.height"
    v-loading="loading"
    element-loading-spinner="el-icon-loading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
  >
    <Yfheader></Yfheader>
    <!--注册头部信息-->
    <!-- <div class="login_content">
      <div class="lgoin_logo_img">
        <img src="../../images/Login_logo_03.png" width="217" height="60" alt />
      </div>
    </div> -->

    <div class="login-container">
      <el-form
        ref="loginForm"
        :model="loginForm"
        :rules="loginRules"
        class="login-form"
        autocomplete="on"
        label-position="left"
      >
        <div class="title-container">
          <h2
            class="title"
            style="font-size: 30px;font-weight: 200;text-align: center;height: 40px;line-height: 40px;margin-bottom: 30px;"
          >用户登录</h2>
        </div>
        <el-form-item prop="userName">
          <el-input @input="mediumRul" v-model="loginForm.userName" size="medium" placeholder="账号">
            <i style="font-size: 22px;line-height: 40px;color: #999;width: 35px;text-align: center;" slot="prefix" class="el-input__icon el-icon-user"></i>
          </el-input>
          <!-- <div class="login_input_w"><span class="iconfont">&#xe62c;</span><input
              v-model="loginForm.userName"
              type="text"
              placeholder="账号"
            >
            <p class="hint_text hint_text_line">请输入正确的手机号码</p>
          </div> -->
        </el-form-item>
        <el-form-item prop="userPassword">
          <el-input
            v-model="loginForm.userPassword"
            size="medium"
            show-password
            type="password"
            placeholder="密码"
          >

              <i style="font-size: 22px;line-height: 40px;color: #999;width: 35px;text-align: center;" slot="prefix" class="el-input__icon el-icon-lock"></i>
          </el-input>
          <!-- <div class="login_input_w no_login_p"><span class="iconfont">&#xe614;</span><input
              v-model="loginForm.userPassword"
              type="password"
              placeholder="密码"
            ></div> -->
        </el-form-item>
        <el-form-item
          v-if="rulesSum>=3"
          class="number"
          prop="userNum"
          style="display: flex;"
        >
          <el-input
            v-model="loginForm.userNum"
            size="medium"
            type="number"
            placeholder="手机验证码"
            style="width: 245px;margin-right:10px"
            onkeypress="return( /[\d]/.test(String.fromCharCode(event.keyCode) ) )"
          >
            <i style="font-size: 22px;line-height: 40px;color: #999;width: 35px;text-align: center;" slot="prefix" class="el-input__icon el-icon-mobile-phone"></i>
          </el-input>
           <el-button
              v-if="btnNum"
              @click="getNum"
               plain
              style="width: 120px;height: 42px;font-size: 14px;"
            >获取验证码</el-button>
            <el-button
              v-if="!btnNum"
              style="width: 120px;height: 42px;font-size: 14px;"
              plain
              disabled
            >{{ num }}s后重新获取</el-button>
        </el-form-item>
        <div class="itemBox">
          <el-checkbox v-model="checked">自动登录</el-checkbox>
          <div>
            <router-link
              style="font-size: 12px;"
              to="/password"
            >忘记密码</router-link>
            <!-- <router-link to="/signIn">注册</router-link> -->
          </div>
        </div>
        <el-form-item class="butt">
          <el-button
            style="width: 380px;height: 46px;"
            size="medium"
            type="primary"
            @click="openLogin"
          >立即登录</el-button>
        </el-form-item>
        <div class="go_reg">
          <router-link
            style="color: #20a0ff;font-size: 13px;"
            to="/signIn"
          >没有账号？免费注册 →</router-link>
        </div>
        <!-- <div class="party-login">
          <div>使用第三方账号登录</div>
          <div class="party-icon">
            <div class="icon">
              <div />
              <div class="title-icon">QQ</div>
            </div>
            <div class="icon">
              <div />
              <div class="title-icon">微博</div>
            </div>
            <div class="icon">
              <div />
              <div class="title-icon">微信</div>
            </div>
            <div class="icon">
              <div />
              <div class="title-icon">企业微信</div>
            </div>
          </div>
        </div>-->
      </el-form>
    </div>
    <!--底部信息-->
    <!-- <div class="login_footer">Copyright ©2018 云飞科技湘ICP备14003093号-1</div> -->
    <Yffooter></Yffooter>
  </div>
</template>

<script>
import request from "@/utils/request";
import { getToken, setToken, removeToken } from "@/utils/auth";
import RequestObject from "@/utils/requestObject";
import store from "@/store";
import { formateRoute } from "@/utils/addRouteTool";
import { setTimeout } from "timers";
import Yfheader from "@/components/Yfheader/index.vue";
import Yffooter from "@/components/Yffooter/index.vue";

export default {
  components: {
    Yfheader,
    Yffooter
  },
  data() {
    var checkName = (rule, value, callback) => {
      const phoneReg = /^1[3|5|7|8|9][0-9]{9}$/;
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
        // callback(new Error("手机号码或邮箱格式不正确"));
        callback(new Error("手机号码格式不正确"));
      }
    };

    return {
      height:"",
      loading: false,
      rulesSum: 0,
      formLabelWidth: "70px",
      checked: false,
      btnNum: true,
      num: 120,
      suc: 180,
      sucNum: "",
      loginForm: {
        userName: "",
        userPassword: "",
        userNum: ""
      },
      loginRules: {
        userName: [{ required: true, validator: checkName, trigger: 'change' }],
        userPassword: [
          { required: true, message: "请输入密码", trigger:'change' }
        ]
      },
      phoneReg: /^1[3|5|7|8|9][0-9]{9}$/,
      emailReg: /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/,
      loginData: {
        telAccount: "",
        emailAccount: "",
        passwd: ""
      }
    };
  },
  watch: {
    checked() {//自动登录
      if (this.checked) {
        localStorage.setItem("checked", JSON.stringify(this.checked));
      } else {
        // localStorage.setItem("checked", JSON.stringify(this.checked));
        localStorage.removeItem("lodinData");
        localStorage.removeItem("checked");
      }
    }
    // rulesSum() {//验证码
    //   console.log(this.rulesSum)
    //   if(this.rulesSum==3) {
    //     localStorage.setItem("rulesSum", JSON.stringify(3));
    //   }
    // }
  },
  created() {
    this.setStyle();
    var lett = this;
    document.onkeydown = function(e) {
      var key = window.event.keyCode;
      if (key == 13) {
        lett.openLogin();
      }
    };
    // if (localStorage.getItem("rulesSum") != null) {//验证码
    //   this.rulesSum = JSON.parse(localStorage.getItem("rulesSum"));
    // }
    if (localStorage.getItem("checked") != null) {//自动登录
      this.checked = JSON.parse(localStorage.getItem("checked"));
    }
    if (this.checked) {
      if(JSON.parse(localStorage.getItem("lodinData"))) {
        var data = JSON.parse(localStorage.getItem("lodinData"));
      this.loginForm.userName = data.userName;
      this.loginForm.userPassword = data.userPassword;
      if(this.loginForm.userName!=""&&this.loginForm.userPassword!="") {
        this.onLogin();
      }
      }
    }
  },
  methods: {
    mediumRul(num) {
      if(this.phoneReg.test(num)) {
        var data = RequestObject.CreateGetObject(false, 0, 0, [
            {
              column: "mobile",
              content: num,
              condition: 0
            }
          ]);
      request({
        url: "/systemlogin/api/Login/GetVerification",
        method: "get",
       params: { RequestObject: JSON.stringify(data) }
      }).then(res => {
        if(res.code == -1) {
          if(res.data==2) {
            this.rulesSum=3;
          }else {
            this.rulesSum=0;
          }
        }
      })
      }
    },
    setStyle() {
      this.$nextTick(() => {
        var navbar = document.getElementById("app");
        this.height = "height:"+navbar.clientHeight+"px";
      });
    },
    openLogin() {
      this.$refs.loginForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法登录",
            type: "error"
          });
        } else {
          this.loading = true;
          this.onLogin();
        }
      });
    },
    onLogin() {
      // var reg = false;
      // if (
      //   this.loginData.telAccount == this.loginForm.userName ||
      //   this.loginData.emailAccount == this.loginForm.userName
      // ) {
      //   reg = true;
      // } else {
      //   reg = false;
      //   this.rulesSum = 1;
      // }
       if(this.loginForm.userNum) {
         this.loginData.verificationCode = this.loginForm.userNum
       }
      if (this.phoneReg.test(this.loginForm.userName)) {
        this.loginData.telAccount = this.loginForm.userName;
      }
      if (this.emailReg.test(this.loginForm.userName)) {
        this.loginData.emailAccount = this.loginForm.userName;
      }
      this.loginData.passwd = this.loginForm.userPassword;
      var data = RequestObject.CreatePostObject(this.loginData);
      request({
        url: "/systemlogin/api/Login",
        method: "post",
        data: data
      }).then(res => {
        this.sucNum = "";
        this.loading = false;
        if (res.code === -1) {
          if (res.info == "登录失败，请输入验证码") {
            this.rulesSum=3;
          }
          this.$message.error(res.info);
        } else {
          if (res.data.isHavaCompany) {
            setToken(res.data.token);
            if (this.checked) {
              localStorage.setItem("lodinData", JSON.stringify(this.loginForm));
            }
            this.getRole();
            // localStorage.clear();
          }else {
            this.$message({
              message: "请先创建或加入企业",
              type: "warning"
            });
            setToken(res.data.token);
            // if(this.loginData.telAccount) {
              this.$router.push({ name: "signIn", params: {type:1, tel: this.loginData.telAccount,email:this.loginData.emailAccount } });
              return;
            // }else if(this.loginData.emailAccount) {
              // this.$router.push({ name: "signIn", params: {type:1, tel: this.loginData.emailAccount } });
            // }
          }
        }
      });
    },
    async getRole() {
      window.location.href = "/";
    },
    getNum() {
      if(this.loginForm.userName==""||!this.phoneReg.test(this.loginForm.userName)) {
        this.$message.error("请填写账号");
        return;
      }
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
      if (this.phoneReg.test(this.loginForm.userName)) {
        getData.mobile = this.loginForm.userName;
        aipUrl = "/system/api/Sms/PostTimePass";
      }
      if (this.emailReg.test(this.loginForm.userName)) {
        getData.email = this.loginForm.userName;
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
  }
};
</script>

<style lang="scss" scoped>
@import "../../styles/login.css";
@import "../../styles/login_CSS.css";
@import "../../styles/icon/iconfont.css";
.el-input--prefix .el-input__inner {
    padding-left: 50px;
}
/deep/.el-button--small {
    padding: 10px;
}
.login_content {
  padding-top: 10px;
}
.number /deep/ {
  input::-webkit-outer-spin-button,
  input::-webkit-inner-spin-button {
    -webkit-appearance: none;
  }
  input[type="number"] {
    -moz-appearance: textfield;
  }
}
.login-container {
  // height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 5%;
  /deep/.el-input--medium .el-input__inner {
    height: 42px;
    line-height: 42px;
    width: 382;
    padding-left: 40px;
  }
  .login_input_w {
    margin-bottom: 0px;
  }
  .el-form-item__error {
    padding-bottom: 20px;
  }
  .title-container {
    width: 100%;
    text-align: center;
    padding: 50px 0px 0px 0px;
    background: #fff;
  }
  .login-form {
    width: 500px;
    background: #fff;
    margin-bottom: 5%;
    padding-bottom: 50px;
    .el-form-item {
      padding: 0 60px;
    }
    .itemBox {
      display: flex;
      width: 100%;
      padding-right: 60px;
      padding-left: 60px;
      .el-checkbox {
        flex: 1;
      }
      div {
        flex: 1;
        font-size: 14px;
        text-align: right;
        color: #0080ff;
      }
    }
    .butt {
      margin-top: 20px;
      margin-bottom: 20px;
      text-align: center;
      .el-button {
        width: 150px;
        font-size: 16px;
      }
    }
    .party-login {
      width: 80%;
      border-top: 1px solid #eee;
      margin: 0 auto;
      margin-top: 20px;
      div {
        text-align: center;
        margin: 10px 0px;
      }
      .party-icon {
        display: flex;
        justify-content: center;
        .icon {
          margin: 0px 10px;
          .title-icon {
            color: #999;
            font-size: 14px;
          }
        }
      }
    }
  }
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
