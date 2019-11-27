<template>
  <el-container
    class="container"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-header>
      <yfheader type="1"></yfheader>
    </el-header>

    <el-main class="gary_bg">
      <div class="login_content reg_warp_heg pr" @click="serviceListState=false">
        <div class="process_warp">
          <div class="proscess_line"></div>
          <div class="process_node choose">
            <p class="iconfont">&#xe6e1;</p>
            <span>填写注册信息</span>
          </div>
          <div class="process_node align_right" :class="{choose:regs}">
            <p class="iconfont">&#xe6de;</p>
            <span>加入或创建公司</span>
          </div>
        </div>

        <div class="registered_table_warp forget_w" v-show="reg">
          <el-form
            ref="ruleForm"
            :model="ruleForm"
            :rules="rules"
            label-width="100px"
            class="demo-ruleForm"
            size="medium"
            label-position="right"
          >
            <el-form-item label="账号" prop="id">
              <el-input v-model="ruleForm.id" placeholder="请输入手机号或者邮箱" class="input_w290" />
            </el-form-item>
            <el-form-item label="姓名" prop="name">
              <el-input v-model="ruleForm.name" placeholder="请输入姓名" class="input_w290" />
            </el-form-item>
            <el-form-item label="密码" prop="password">
              <el-input
                v-model="ruleForm.password"
                placeholder="请输入至少8位密码"
                type="password"
                class="input_w290"
              />
            </el-form-item>
            <el-form-item label="确认密码" prop="confirmPd">
              <el-input
                v-model="ruleForm.confirmPd"
                placeholder="请确认密码"
                type="password"
                class="input_w290"
              />
            </el-form-item>
            <el-form-item prop="verification" label="验证码">
              <div id="elInput" style="width:168px;float:left;">
                <el-input v-model="ruleForm.verification" style="width:100%" class="codeIp" />
              </div>
              <div style="float:left;">
                <el-button
                  style="width:100px;height:40px;margin-left:12px;padding: 0px;"
                  :disabled="clickBefor"
                  :plain="true"
                  @click="clickAuthCode()"
                >{{ authCode }}</el-button>
              </div>
            </el-form-item>
            <el-form-item prop="checked1">
              <el-checkbox v-model="ruleForm.checked1" style="margin-right:10px" />我同意
              <a href="javascript:;" style="color:#1890ff" @click.stop="readDocument">服务条款</a>
            </el-form-item>
          </el-form>
          <div class="form_bnt_warp">
            <div class="submit_bnt1" @click="onSubmit()">下一步</div>
          </div>
          <!--服务条款-->
          <div class="serviceBox" v-if="serviceListState">
            <div class="contenSb" @click.stop="1==1">
              <Termsofservice />
            </div>
          </div>
        </div>
        <div v-show="regs">
          <!-- <el-steps
            :active="active"
            finish-status="success"
            style="width:768px;margin:0 auto;margin-top:25px"
            align-center
          >
            <el-step title="注册" />
            <el-step title="创建或加入企业" />
          </el-steps>-->

          <div style="width:768px;margin:0 auto;">
            <div style="width:100%;height:500px;padding-top:60px;">
              <div v-show="!versionState">
                <div style="margin: 10px auto;width:40%;">
                  <el-button
                    style="width:100%;background-color:#007CF4;color:white;"
                    class="styBtn"
                    @click="clickFound()"
                  >创建公司</el-button>
                </div>
                <div style="margin: 0 auto;width:40%">
                  <el-button
                    style="width:100%;background-color:#5CB85C;color:white;"
                    class="styBtn"
                    @click="addClick()"
                  >加入公司</el-button>
                </div>
                <div style="width:100%;min-height:300px;padding-top:20px;">
                  <el-form
                    v-show="found"
                    ref="ruleForms"
                    :model="ruleForms"
                    :rules="rulee"
                    label-width="100px"
                    class="demo-ruleForm"
                    size="medium"
                    style="margin:0 auto;margin-left:144px"
                    label-position="right"
                  >
                    <el-form-item label="团队名称" prop="team">
                      <el-input v-model="ruleForms.team" placeholder="团队名称" class="input_w290" />
                    </el-form-item>
                    <el-form-item label="联系人电话" prop="phoneName">
                      <el-input
                        v-model="ruleForms.phoneName"
                        placeholder="请输入手机号码"
                        class="input_w290"
                      />
                    </el-form-item>
                    <el-form-item label="邮箱" prop="excel">
                      <el-input
                        v-model="ruleForms.excel"
                        placeholder="请填写正确的邮箱"
                        class="input_w290"
                      />
                    </el-form-item>
                    <el-button
                      :plain="true"
                      style="margin-left:150px;background-color:#5CB85C;color:#fff"
                      @click="setExperience()"
                    >完成,进入体验</el-button>
                    <el-button @click="setResetting1()">重置</el-button>
                  </el-form>

                  <el-form
                    v-show="add"
                    ref="ruleForms1"
                    :model="ruleForms1"
                    label-width="100px"
                    class="demo-ruleForm"
                    style="margin-left:62px"
                    @submit.native.prevent
                  >
                    <el-form-item label="企业名称" prop="team1">
                      <el-input
                        v-model="ruleForms1.team1"
                        placeholder="请输入您的企业名称"
                        style="width:70%"
                        @keyup.enter.native="getTeam()"
                      />
                      <el-button @click="getTeam()">搜索</el-button>
                    </el-form-item>
                  </el-form>
                  <el-table
                    v-show="add"
                    :data="tableData"
                    border
                    class="company_search"
                    height="240px"
                  >
                    <el-table-column type="index" label="序号"></el-table-column>
                    <el-table-column prop="companyName" label="公司名称" width="380" />
                    <el-table-column v-if="false" prop="id" label="id" />
                    <el-table-column label="操作">
                      <template slot-scope="scope">
                        <el-button type="text" size="small" @click="handleClick(scope.row)">申请</el-button>
                      </template>
                    </el-table-column>
                  </el-table>
                </div>
              </div>

              <!--加入企业选择版本-->
              <div class="version_bg" v-if="versionState" @click="versionState=false">
                <div class="version_warp" @click.stop="1==1">
                  <h2>版本选择</h2>
                  <ul class="ersion_li">
                    <li>
                      <dt>
                        <img src="./images/re_icon_09.png" width="90" height="90" alt />
                      </dt>
                      <dd>
                        <h3>专业版</h3>
                        <p>进销存应用</p>
                      </dd>
                    </li>
                    <li class="nothing">
                      <dt>
                        <img src="./images/re_icon_03.png" width="90" height="90" alt />
                      </dt>
                      <dd>
                        <h3>企业版</h3>
                        <p>进销存应用+电商管理</p>
                      </dd>
                    </li>
                    <li class="nothing">
                      <dt>
                        <img src="./images/re_icon_06.png" width="90" height="90" alt />
                      </dt>
                      <dd>
                        <h3>旗舰版</h3>
                        <p>进销存应用+电商管理+条码管理</p>
                      </dd>
                    </li>
                  </ul>
                  <div class="version_bnt" @click="doVersion(0)">确认版本</div>
                </div>
              </div>
            </div>

            <div
              style="width:100%;text-align:center;color:rgb(0 124 244);cursor:pointer"
              @click="setlogin()"
            >返回登陆</div>
          </div>
        </div>
      </div>
    </el-main>
    <!-- <el-main v-show="reg">
      <el-steps
        :active="active"
        finish-status="success"
        style="margin-top:25px"
        class="middleMg"
        align-center
      >
        <el-step title="注册" style="color:#fff" />
        <el-step title="创建或加入企业" />
      </el-steps>
      <div class="middleMg">
        <div
          style="width:100%;height:500px;padding-left:100px;padding-top:60px;border:1px solid rgb(192 196 204);margin-top:10px"
        >
          <el-form
            ref="ruleForm"
            :model="ruleForm"
            :rules="rules"
            label-width="100px"
            class="demo-ruleForm"
            size="medium"
            label-position="right"
          >
            <el-form-item label="账号" prop="id">
              <el-input v-model="ruleForm.id" placeholder="请输入手机号或者邮箱" class="inputW" />
            </el-form-item>
            <el-form-item label="姓名" prop="name">
              <el-input v-model="ruleForm.name" placeholder="请输入姓名" class="inputW" />
            </el-form-item>
            <el-form-item label="密码" prop="password">
              <el-input
                v-model="ruleForm.password"
                placeholder="请输入至少8位密码"
                type="password"
                class="inputW"
              />
            </el-form-item>
            <el-form-item label="确认密码" prop="confirmPd">
              <el-input
                v-model="ruleForm.confirmPd"
                placeholder="请确认密码"
                type="password"
                class="inputW"
              />
            </el-form-item>
            <el-form-item prop="verification" style="width:67%" label="验证码">
              <div id="elInput" style="width:120px;float:left;">
                <el-input v-model="ruleForm.verification" style="width:100%" />
              </div>
              <div style="float:right;">
                <el-button
                  :disabled="clickBefor"
                  :plain="true"
                  @click="clickAuthCode()"
                >{{ authCode }}</el-button>
              </div>
            </el-form-item>
            <el-form-item prop="checked1">
              <el-checkbox v-model="ruleForm.checked1" style="margin-right:10px" />我同意
              <a href style="color:#1890ff">服务条款</a>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" :plain="true" @click="onSubmit()">下一步</el-button>
              <el-button @click="setRevers()">重置</el-button>
            </el-form-item>
          </el-form>
        </div>
      </div>
    </el-main>-->
    <!-- <el-main v-show="regs">
      <el-steps
        :active="active"
        finish-status="success"
        style="width:768px;margin:0 auto;margin-top:25px"
        align-center
      >
        <el-step title="注册" />
        <el-step title="创建或加入企业" />
      </el-steps>

      <div style="width:768px;margin:0 auto;">
        <div
          style="width:100%;height:500px;padding-top:60px;border:1px solid rgb(192 196 204);margin-top:10px"
        >
          <div v-show="!versionState">
            <div style="margin: 10px auto;width:40%;">
              <el-button
                style="width:100%;background-color:rgb(0 124 244);color:white;"
                @click="clickFound()"
              >创建团队</el-button>
            </div>
            <div style="margin: 0 auto;width:40%">
              <el-button
                style="width:100%;background-color:rgb(92 184 92);color:white;"
                @click="addClick()"
              >加入团队</el-button>
            </div>
            <div style="width:100%;min-height:300px;padding-top:20px;">
              <el-form
                v-show="found"
                ref="ruleForms"
                :model="ruleForms"
                :rules="rulee"
                label-width="100px"
                class="demo-ruleForm"
                size="medium"
                style="margin:0 auto;margin-left:120px"
                label-position="right"
              >
                <el-form-item label="团队名称" prop="team">
                  <el-input v-model="ruleForms.team" placeholder="团队名称" style="width:70%" />
                </el-form-item>
                <el-form-item label="联系人电话" prop="phoneName">
                  <el-input v-model="ruleForms.phoneName" placeholder="请输入手机号码" style="width:70%" />
                </el-form-item>
                <el-form-item label="邮箱" prop="excel">
                  <el-input v-model="ruleForms.excel" placeholder="请填写正确的邮箱" style="width:70%" />
                </el-form-item>
                <el-button
                  :plain="true"
                  style="margin-left:150px;background-color:rgb(92 184 92);color:#fff"
                  @click="setExperience()"
                >完成,进入体验</el-button>
                <el-button @click="setResetting1()">重置</el-button>
              </el-form>

              <el-form
                v-show="add"
                ref="ruleForms1"
                :model="ruleForms1"
                label-width="100px"
                class="demo-ruleForm"
                style="margin-left:100px"
              >
                <el-form-item label="企业名称" prop="team1">
                  <el-input v-model="ruleForms1.team1" placeholder="请输入您的企业名称" style="width:70%" />
                  <el-button @click="getTeam()">搜索</el-button>
                </el-form-item>
              </el-form>
              <el-table
                v-show="add"
                :data="tableData"
                border
                style="width:70%;margin-left:130px;overflow-y:auto;height:200px"
              >
                <el-table-column fixed prop="companyName" label="公司名称" width="400" />
                <el-table-column v-if="false" prop="id" label="id" />
                <el-table-column fixed="right" label="操作" width="100">
                  <template slot-scope="scope">
                    <el-button type="text" size="small" @click="handleClick(scope.row)">申请</el-button>
                  </template>
                </el-table-column>
              </el-table>
            </div>
          </div>

          <div v-if="versionState">
            <ul class="version_li">
              <li @click="doVersion(0)">
                <dt>
                  <img src="./images/ver_img_04.png" width="102" height="104" alt />
                </dt>
                <dd>
                  <h2>专业版</h2>
                  <p>进销存应用</p>
                </dd>
              </li>
              <li @click="doVersion(-1)">
                <dt>
                  <img src="./images/ver_img_06.png" width="105" height="104" alt />
                </dt>
                <dd>
                  <h2>企业版</h2>
                  <p>进销存应用+电商管理</p>
                </dd>
                <div class="shelter"></div>
              </li>
              <li @click="doVersion(-1)">
                <dt>
                  <img src="./images/ver_img_08.png" width="104" height="104" alt />
                </dt>
                <dd>
                  <h2>旗舰版</h2>
                  <p style="line-height: 24px;">
                    进销存应用+电商管理
                    <br />+条码管理
                  </p>
                </dd>
                <div class="shelter"></div>
              </li>
            </ul>
          </div>
        </div>

        <div
          style="width:100%;text-align:center;color:rgb(0 124 244);cursor:pointer"
          @click="setlogin()"
        >返回登陆</div>
      </div>
    </el-main>-->

    <!-- <div class="versionBox" v-if="versionState"></div> -->
    <Yffooter></Yffooter>
  </el-container>
</template>
<script>
import { type } from "os";
import request from "@/utils/request";
import { trim } from "@/utils/common.js";
import rquestObject from "@/utils/requestObject";
import yfheader from "@/components/Yfheader";
import Yffooter from "@/components/Yffooter";
import Termsofservice from "./components/Termsofservice";
import { getToken, setToken, removeToken } from "@/utils/auth";
// import { setToken } from "@/utils/auth.js";
export default {
  name: "SignIn",
  data() {
    /**
     * verifyName
     * 名字验证
     */
    var verifyName = (rulue, value, callback) => {
      var vName = /^[\u0391-\uFFE5A-Za-z]+$/;
      if (!vName.test(value)) {
        this.ruleForm.name = "";
        return callback("请输入中文和字母");
      } else {
        return callback();
      }
    };
    /**
     *   verifyTeam
     * 团队名称
     *
     */
    var verifyTeam = (rule, value, callback) => {
      var vName = /^[\u0391-\uFFE5A-Za-z]+$/;
      if (!vName.test(value)) {
        this.ruleForms.team = "";
        return callback("请输入中文和字母");
      } else {
        var postdata = {
          companyName: this.ruleForms.team
        };
        var postObj = rquestObject.CreatePostObject(postdata);
        request({
          url: "/system/api/TSMCompany/CheckIsExists",
          method: "POST",
          data: postObj
        }).then(res => {
          if (res.data) {
            return callback(new Error("已存在的公司"));
          } else {
            return callback();
          }
        });
      }
    };
    /**
     * 验证码验证
     *
     */
    var verifiers = (rulue, value, callback) => {
      // console.log(value);
      // console.log(this.ruleForm.verification);
      // console.log();
      if (trim(value).length != 6) {
        return callback("请输入正确的验证码");
      } else {
        callback();
      }
      // if (!(value == this.verifier)) {
      //   this.ruleForm.verification = "";
      //   return callback("请输入正确的验证码");
      // }
    };
    var phoneName = (rulue, value, callback) => {
      var phoneNumbers = new RegExp(/^1[3456789]\d{9}$/);
      if (!value) {
        return callback("手机号码不能为空");
      } else if (!phoneNumbers.test(value)) {
        this.ruleForms.phoneName = "";
        return callback("请输入正确的手机号码");
      } else {
        callback();
      }
    };
    var checkName = (rule, value, callback) => {
      var idReg = new RegExp(
        /^([-_a-zA-Z0-9\u4e00-\u9fa5\.])+@([-_a-zA-Z0-9])+(\.[a-zA-Z]{2,5}){1,3}$/
      );
      var phoneNumber = new RegExp(/^1[3456789]\d{9}$/);
      if (!value) {
        return callback(new Error("账号不能为空"));
      } else if (!idReg.test(value) && !phoneNumber.test(value)) {
        this.ruleForm.id = "";
        return callback(new Error("请输入正确的邮箱和手机号码"));
      } else {
        var postData = {
          keyWords: value
        };
        var postObj = rquestObject.CreatePostObject(postData);
        request({
          url: "/system/api/CheckAccount",
          method: "POST",
          data: postObj
        }).then(res => {
          if (res.data) {
            return callback(new Error("已存在的账号"));
          } else {
            return callback();
          }
        });
      }
    };
    var checkPassword = (rule, value, callback) => {
      var reg = /^(?=.*[a-zA-Z])(?=.*\d)(?=.*[~!@#$%^&*()_+`\-={}:";'<>?,.\/]).{8,16}$/;
      if (!value) {
        return callback(new Error("密码不能为空"));
      } else if (!reg.test(value)) {
        this.ruleForm.password = value;
        return callback(
          new Error("密码必须在8-16位之间且包含数字、字母、特殊符号")
        );
      }
      return callback();
    };
    var checkConfirmPd = (rule, value, callback) => {
      if (!value) {
        return callback(new Error("确认密码不能为空"));
      } else if (!(this.ruleForm.password === value)) {
        this.ruleForm.confirmPd = value;
        return callback(new Error("确认密码不正确请重新输入"));
      }
      return callback();
    };
    var checkVerification = (rule, value, callback) => {};
    return {
      fullscreenLoading: false,
      serviceListState: false, //服务条款
      versionState: false,
      versionFlag: false,
      versionType: -1,
      checked1: "",
      verifier: "",
      add: false,
      found: false,
      regs: false,
      reg: true,
      active: 0,
      clickBefor: false,
      authCode: "获取验证码",
      tableData: [],
      ruleForms1: {
        team1: ""
      },
      ruleForms: {
        excel: "",
        phoneName: "",
        team: ""
      },
      ruleForm: {
        name: "",
        id: "",
        password: "",
        confirmPd: "",
        type: [],
        verification: ""
      },
      rulee: {
        team: [{ required: true, validator: verifyTeam, trigger: "blur" }],
        phoneName: [{ required: true, validator: phoneName, trigger: "blur" }]
      },
      rules: {
        verification: [
          { required: true, validator: verifiers, trigger: "blur" }
        ],
        id: [{ required: true, validator: checkName, trigger: "blur" }],

        name: [{ required: true, validator: verifyName, trigger: "blur" }],
        password: [
          { required: true, validator: checkPassword, trigger: "blur" }
        ],
        confirmPd: [
          {
            required: true,
            validator: checkConfirmPd,
            trigger: "blur"
          }
        ]
      }
    };
  },
  components: {
    yfheader,
    Yffooter,
    Termsofservice
  },
  mounted() {
    if (this.$route.params.type == 1) {
      this.regs = true;
      this.reg = false;
      // this.ruleForms.phoneName = this.$route.params.tel;
      // this.ruleForms.excel = this.$route.params.email;
      this.versionFlag = true;
    }
  },
  watch: {
    checked1: function(newQuestion, oldQuestion) {}
  },
  methods: {
    /**服务条款**/
    readDocument() {
      // this.serviceListState = true;
      const { href } = this.$router.resolve({
        path: "/termsService"
      });
      window.open(href, "_blank");
    },
    /**
     * setlogin()
     * 返回登陆
     */
    setlogin() {
      removeToken();
      this.$router.push("/login");
    },
    /**
     * setRevers
     * 设置注册时候重置
     *
     */
    setRevers() {
      this.ruleForm.name = "";
      this.ruleForm.id = "";
      this.ruleForm.password = "";
      this.ruleForm.confirmPd = "";
      this.ruleForm.checked1 = "";
      this.ruleForm.verification = "";
    },
    /**
     * handleClick
     * 申请
     */
    handleClick(row) {
      var postData = {
        // id: 0,
        // account: this.ruleForm.id,
        companyId: row.id
        // applyTime: new Date(),
        // applyStatus: 0
      };
      // console.log(postData);
      var postObj = rquestObject.CreatePostObject(postData);
      request({
        url: "/system/api/TSMCompanyApply",
        method: "POST",
        data: postObj
      }).then(res => {
        if (res.code == 0) {
          this.$message({
            showClose: true,
            message: "操作成功",
            type: "success"
          });
          // setTimeout(() => {
          // window.location.href = "/";
          // }, 500);
        }
      });
    },
    /**
     * getTeam()
     * 搜索团队
     * 获取团队名称
     */
    getTeam() {
      var queryConditions = [
        {
          column: "keyWords",
          content: this.ruleForms1.team1,
          condition: 0
        }
      ];

      var postObj = rquestObject.CreateGetObject(
        false,
        0,
        0,
        queryConditions,
        null
      );
      request({
        url: "/system/api/TSMCompany",
        method: "GET",
        params: {
          requestObject: JSON.stringify(postObj)
        }
      }).then(res => {
        // console.log(res);
        this.tableData = res.data;
      });
    },
    doVersion(state) {
      this.versionType = state;
      if (this.versionType == -1) {
        this.$message({
          showClose: true,
          message: "敬请期待",
          type: "warning"
        });
        return;
      }
      var postdata = {
        id: 0,
        companyName: this.ruleForms.team,
        contactNumber: this.ruleForms.phoneName,
        mobile: this.ruleForms.phoneName,
        contactEmail: this.ruleForms.excel,
        versionType: this.versionType
      };
      var postObj = rquestObject.CreatePostObject(postdata);
      request({
        url: "/system/api/TSMCompany",
        method: "POST",
        data: postObj
      }).then(res => {
        if (res.code == 0) {
          (this.ruleForms.team = ""),
            (this.ruleForms.phoneName = ""),
            (this.ruleForms.excel = ""),
            this.$message({
              showClose: true,
              message: "操作成功",
              type: "success"
            });
          setToken(res.data);
          setTimeout(() => {
            window.location.href = "/";
          }, 500);

          // this.$router.push("/dashboard");
        } else {
          this.$message({
            showClose: true,
            message: res.info,
            type: "error"
          });
        }
      });
    },
    /**
     * setResetting1
     * 设置重置
     *
     */
    setResetting1() {
      (this.ruleForms.excel = ""),
        (this.ruleForms.phoneName = ""),
        (this.ruleForms.team = "");
    },
    /**
     * setExperience
     * 完成进入体验
     */
    setExperience() {
      var postdata = {
        companyName: this.ruleForms.team,
        contactNumber: this.ruleForms.phoneName,
        contactEmail: this.ruleForms.excel,
        versionType: this.versionType
      };
      // if (this.versionFlag) {
      // this.$refs["ruleForms"].resetFields();
      this.$refs["ruleForms"].validate(valid => {
        if (valid) {
          this.versionState = true;
        } else {
          this.$message({
            showClose: true,
            message: "请将信息填写完整",
            type: "warning"
          });
          return;
        }
      });
      return;
      // }

      // console.log(postdata);
      for (var key in postdata) {
        // console.log(postdata[key]);
        if (postdata[key] == "" || postdata[key] == undefined) {
          // console.log("进入");
          this.$message({
            showClose: true,
            message: "请将信息填写完整",
            type: "warning"
          });
          return;
        } else {
          const idReg = new RegExp(
            /^([-_a-zA-Z0-9\u4e00-\u9fa5\.])+@([-_a-zA-Z0-9])+(\.[a-zA-Z]{2,5}){1,3}$/
          );
          const phoneNumber = new RegExp(/^1[3456789]\d{9}$/);
          postdata.id = 0;
          if (idReg.test(this.ruleForm.id)) {
            postdata.email = this.ruleForm.id;
            postdata.mobile = null;
            var postObj = rquestObject.CreatePostObject(postdata);
            request({
              url: "/system/api/TSMCompany",
              method: "POST",
              data: postObj
            }).then(res => {
              (this.ruleForms.team = ""),
                (this.ruleForms.phoneName = ""),
                (this.ruleForms.excel = ""),
                this.$message({
                  showClose: true,
                  message: "操作成功",
                  type: "success"
                });
              this.$router.push("/login");
            });
            return;
          } else if (phoneNumber.test(this.ruleForm.id)) {
            postdata.email = null;
            postdata.mobile = this.ruleForm.id;
            var postObj = rquestObject.CreatePostObject(postdata);
            request({
              url: "/system/api/TSMCompany",
              method: "POST",
              data: postObj
            }).then(res => {
              // console.log(res);
              (this.ruleForms.team = ""),
                (this.ruleForms.phoneName = ""),
                (this.ruleForms.excel = ""),
                this.$message({
                  showClose: true,
                  message: "操作成功",
                  type: "success"
                });
            });
            return;
          }
        }
      }
    },
    /**
     * addClick
     * 加入团队
     */
    addClick() {
      this.add = true;
      this.found = false;
    },
    /**
     * clickFound
     * 创建团队
     * */
    clickFound() {
      this.found = true;
      this.add = false;
    },
    verifyCode() {
      return new Promise((reslove, reject) => {
        this.fullscreenLoading = true;
        var postObj = rquestObject.CreateGetObject(false, 0, 0, [
          {
            column: "code",
            content: trim(this.ruleForm.verification),
            condition: 0
          },
          {
            column: "mobile",
            content: trim(this.ruleForm.id),
            condition: 0
          }
        ]);
        request({
          url: "/system/api/Sms/GetVerificationCodeRegister",
          method: "get",
          params: {
            requestObject: JSON.stringify(postObj)
          }
        }).then(res => {
          if (res.code == 0) {
            if (res.data == 0) {
              this.$message.error("验证码失效！");
            }
            if (res.data == 2) {
              this.$message.error("验证码不正确！");
            }
            reslove(res.data);
          } else {
            this.fullscreenLoading = false;
            this.$message.error("验证码不正确！");
          }
        });
      });
    },
    /**
     * onSubmit
     * 注册下一步
     *
     */
    async onSubmit() {
      this.$refs["ruleForm"].validate(async valid => {
        if (valid) {
          if (this.ruleForm.checked1) {
            var getRuleForm = {
              name: this.ruleForm.name,
              id: this.ruleForm.id,
              password: this.ruleForm.password,
              checked1: this.ruleForm.checked1,
              verification: this.ruleForm.verification,
              confirmPd: this.ruleForm.confirmPd
            };
            for (var key in getRuleForm) {
              if (getRuleForm[key] == "") {
                this.$message.error("请将信息填写完整");
                return;
              } else {
                var idReg = new RegExp(
                  /^([-_a-zA-Z0-9\u4e00-\u9fa5\.])+@([-_a-zA-Z0-9])+(\.[a-zA-Z]{2,5}){1,3}$/
                );
                var phoneNumber = new RegExp(/^1[3456789]\d{9}$/);
                // console.log(idReg.test(this.ruleForm.id));
                if (idReg.test(this.ruleForm.id)) {
                  // console.log("进入");
                  var postData = {
                    id: 0,
                    telAccount: "",
                    emailAccount: this.ruleForm.id,
                    passwd: this.ruleForm.password,
                    accountName: this.ruleForm.name
                  };
                  var postObj = rquestObject.CreatePostObject(postData);
                  return;
                  request({
                    url: "/system/api/TSMUserAccount",
                    method: "POST",
                    data: postObj
                  }).then(res => {
                    // console.log(res);
                    if (res.code == -1) {
                      this.$message({
                        showClose: true,
                        message: "账户信息重复请更改",
                        type: "warning"
                      });
                      return;
                    } else {
                      if (this.active++ > 2) this.active = 0;
                      this.reg = false;
                      this.regs = true;
                      return;
                    }
                  });
                } else if (phoneNumber.test(this.ruleForm.id)) {
                  var postData = {
                    id: 0,
                    telAccount: this.ruleForm.id,
                    emailAccount: "",
                    passwd: this.ruleForm.password,
                    accountName: this.ruleForm.name
                  };
                  var postObj = rquestObject.CreatePostObject(postData);
                  var verifyState = await this.verifyCode();
                  console.log(verifyState);
                  if (verifyState != 1) {
                    this.fullscreenLoading = false;
                    return;
                  }
                  request({
                    url: "/system/api/TSMUserAccount",
                    method: "POST",
                    data: postObj
                  }).then(res => {
                    this.fullscreenLoading = false;
                    // console.log(res);
                    if (res.code == -1) {
                      this.$message({
                        showClose: true,
                        message: "账户信息重复请更改",
                        type: "warning"
                      });
                      return;
                    } else {
                      if (this.active++ > 2) this.active = 0;
                      setToken(res.data);
                      this.reg = false;
                      this.regs = true;
                      return;
                    }
                  });
                }
                return;
              }
            }
          } else {
            this.$message.error("请阅读服务条款");
          }
        } else {
          return false;
        }
      });
    },
    next() {
      if (this.active++ > 2) this.active = 0;
    },
    /**
     * 验证码倒计时
     */
    clickAuthCode() {
      let name;
      let id;
      let password;
      let confirmPd;
      var getRuleForm = {
        name: this.ruleForm.name,
        id: this.ruleForm.id,
        password: this.ruleForm.password,
        confirmPd: this.ruleForm.confirmPd
      };
      if (
        getRuleForm.name == "" ||
        getRuleForm.id == "" ||
        getRuleForm.password == "" ||
        getRuleForm.confirmPd == "" ||
        getRuleForm.confirmPd != getRuleForm.password
      ) {
        this.$message.error("请将页面信息填写正确");
        return;
      } else {
        /**
         * 判断用户名是手机号还是邮箱
         */
        const idReg = new RegExp(
          /^([-_a-zA-Z0-9\u4e00-\u9fa5\.])+@([-_a-zA-Z0-9])+(\.[a-zA-Z]{2,5}){1,3}$/
        );
        const phoneNumber = new RegExp(/^1[3456789]\d{9}$/);
        const moblies = this.ruleForm.id;
        // console.log(idReg.test(moblies));
        if (phoneNumber.test(moblies)) {
          const postData = {
            mobile: moblies
          };
          const postObj = rquestObject.CreatePostObject(postData);
          request({
            url: "/system/api/Sms/PostRegister",
            method: "POST",
            data: postObj
          }).then(res => {
            // console.log(res.data);
            this.verifier = res.data;
          });
        } else if (idReg.test(moblies)) {
          // console.log("进入邮箱");
          const postData = {
            mobile: moblies
          };
          const postObj = rquestObject.CreatePostObject(postData);
          request({
            url: "/system/api/Email",
            method: "POST",
            data: postObj
          }).then(res => {
            // console.log(res.data);
            this.verifier = res.data;
          });
        }
        var ids = this.ruleForm.id;
        this.authCode = 120;
        this.clickBefor = true;
        var _this = this;
        var setTime = setInterval(function() {
          _this.authCode--;
          if (_this.authCode == 0) {
            clearInterval(setTime);
            _this.authCode = "获取验证码";
            _this.clickBefor = false;
            // console.log(_this.authCode);
            return _this.authCode;
          }
        }, 1000);
        // this.authCode = setTime();
      }
    }
  }
};
</script>

<style scoped lang="scss">
@import "../../styles/login.css";
@import "../../styles/login_CSS.css";
@import "../../styles/icon/iconfont.css";
.header,
.el-header {
  padding-top: 0px !important;
}
.el-container {
  height: 100%;
}
.el-main {
  padding-top: 50px;
  min-width: 1024px;
  min-height: calc(100% - 100px);
  overflow-x: hidden;
  width: 100%;
}
.login_content {
  width: 900px;
}
.login_header {
  box-shadow: 0px 0px 0px #ccc;
}
.ersion_li {
  margin-top: 40px;
}
.login_content /deep/ {
  .input_w290 {
    width: 280px;
    height: 40px;
    line-height: 40px;
  }
  .el-input--medium .el-input__inner {
    height: 40px;
    line-height: 40px;
  }
  .el-form-item__label {
    padding-right: 0px;
    margin-right: 10px;
    line-height: 40px;
  }
  .submit_bnt1,
  .version_bnt {
    cursor: pointer;
  }
  .styBtn {
    padding: 13px 15px;
    font-size: 12px;
    border-radius: 3px;
  }
  .serviceBox {
    position: absolute;
    left: 0px;
    top: 121px;
    width: 100%;
    height: 360px;
    background: #fff;
    overflow: hidden;
    z-index: 999;
    padding: 0px 105px;
  }
  .contenSb {
    width: 100%;
    height: 100%;
    border: 1px solid #dfdfdf;
  }
}
</style>

