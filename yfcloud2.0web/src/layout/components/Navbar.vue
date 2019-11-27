<template>
  <div class="navbar">
    <hamburger
      id="hamburger-container"
      :is-active="sidebar.opened"
      class="hamburger-container"
      @toggleClick="toggleSideBar"
    />
    <breadcrumb id="breadcrumb-container" class="breadcrumb-container" />
    <el-dialog
      :visible.sync="dialogTableVisible"
      width="700px"
      style="padding:0px"
      title="公司详情"
      :before-close="handleClose"
      :close-on-click-modal="false"
    >
      <div class="cantion">
        <div class="cantionTap">
          <div :class="cantionTapNum==1? 'color':''" @click="cantionTapNum=1">公司信息</div>
          <div :class="cantionTapNum==2? 'color':''" @click="cantionTapNum=2">公司认证</div>
        </div>
        <div style="padding-top: 15px;height:500px; overflow-y: scroll; padding: 0 20px;">
          <div v-if="cantionTapNum==1">
            <cominFormation style="width:100%" :formData="formData" @oldval="oldval" />
          </div>
          <div v-if="cantionTapNum==2">
            <eCantion style="width:100%" :formData="formData" @eCantion="eCantion" />
          </div>
        </div>
      </div>
      <!-- <el-tabs v-model="activeName" type="border-card" style="width:100%" @tab-click="handleClick">
        <el-tab-pane label="公司信息" name="first">
          <cominFormation style="width:100%" :formData="formData" @oldval="oldval" />
        </el-tab-pane>
        <el-tab-pane label="公司认证" name="second">
          <eCantion style="width:100%" :formData="formData" @eCantion="eCantion" />
        </el-tab-pane>
        <div style="margin-top:10px;" class="dialog-footer">
          <el-button size="small" @click="OnBtnClose">关闭</el-button>
          <el-button type="primary" size="small" @click="OnBtnSaveSubmit">确定</el-button>
        </div>
      </el-tabs>-->

      <span slot="footer" class="dialog-footer">
        <el-button size="small" @click="OnBtnClose">关闭</el-button>
        <el-button type="primary" size="small" @click="OnBtnSaveSubmit">保存</el-button>
      </span>
    </el-dialog>
    <div class="right-menu">
      <!-- <template v-if="device!=='mobile'">
        <search id="header-search" class="right-menu-item" />
        <error-log class="errLog-container right-menu-item hover-effect" />
        <screenfull id="screenfull" class="right-menu-item hover-effect" />

        <el-tooltip content="Global Size" effect="dark" placement="bottom">
          <size-select id="size-select" class="right-menu-item hover-effect" />
        </el-tooltip>
      </template>-->
      <div class="messageBox" @hover.stop="1==1">
        <el-popover
          ref="popover"
          placement="bottom"
          width="280"
          height="200"
          trigger="hover"
          :open-delay="100"
          :close-delay="200"
          popper-class="popperBox"
        >
          <div style="height:200px;overflow:auto;" class="innerbox">
            <!-- :class="{'el-card-none':index+1!=MsgData.length}" -->
            <ul>
              <li @click="readMsg" v-for="(item) in msgData" :key="item.id">
                <div class="clr">
                  <div class="fl col1">{{item.title}}</div>
                  <div class="fr col2">{{item.createDate |formatTDate}}</div>
                </div>

                <div class="col3 ellipsis" :title="item.content">{{item.content}}</div>
              </li>
            </ul>

            <div class="showMore" @click="readMsg" v-if="msgData.length>0">查看更多消息</div>
            <!-- <el-button
              class="showMore"
              type="primary"
              round
              @click="readMsg"
              v-if="MsgData.length>0"
            >展示更多</el-button>-->
            <div v-else class="noList">暂无消息</div>
          </div>
          <div slot="reference" class="msgLg">
            <el-badge
              :value="$store.state.user.MsgNum"
              :hidden="$store.state.user.MsgNum<=0"
              class="item"
            >
              <i class="el-icon-bell"></i>
            </el-badge>
          </div>
        </el-popover>
      </div>

      <el-dropdown class="avatar-container right-menu-item hover-effect" trigger="click">
        <div class="avatar-wrapper">
          <img v-if="avatar" :src="this.$store.state.user.imgUrlName + avatar" class="user-avatar" />
          <img v-else src="./Sidebar/images/avatar.png" class="user-avatar" />
          <i class="el-icon-caret-bottom" />
        </div>
        <!-- <el-dropdown-menu slot="dropdown">
          <el-dropdown-item>
            <span class="linkBox" @click="openUsers">个人信息</span>
          </el-dropdown-item>
          <el-dropdown-item>
            <span class="linkBox" @click="setBusiness">企业信息</span>
          </el-dropdown-item>

          <el-dropdown-item>
            <span class="linkBox" @click="openPassword">修改密码</span>
          </el-dropdown-item>
          <el-dropdown-item>
            <span class="linkBox" @click="logout">退出登录</span>
          </el-dropdown-item>
        </el-dropdown-menu>-->
        <el-dropdown-menu slot="dropdown">
          <el-dropdown-item><div class="linkBox" @click="openUsers">个人信息</div></el-dropdown-item>
          <el-dropdown-item><div class="linkBox" @click="setBusiness">企业信息</div></el-dropdown-item>
          <el-dropdown-item><div class="linkBox" @click="openPassword">修改密码</div></el-dropdown-item>
          <el-dropdown-item ><div class="linkBox" @click="logout">退出登录</div></el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </div>
    <!-- 个人信息 -->
    <usersForm ref="userVisible" />
    <!-- 修改密码 -->
    <passwordForm ref="password" />
  </div>
</template>

<script>
import uploadExcel from "../../components/UploadPic/index";
import { mapGetters, mapState } from "vuex";
import Breadcrumb from "@/components/Breadcrumb";
import Hamburger from "@/components/Hamburger";
import ErrorLog from "@/components/ErrorLog";
import Screenfull from "@/components/Screenfull";
import SizeSelect from "@/components/SizeSelect";
import Search from "@/components/HeaderSearch";
import DateUtil from "@/utils/dateUtil";
import passwordForm from "@/components/passwordForm";
import usersForm from "@/components/usersForm";
import clipboard from "@/utils/clipboard";
import svgIcons from "../../views/icons/svg-icons";
import elementIcons from "../../views/icons/element-icons";
import eCantion from "./epriseCation/eCantion";
import cominFormation from "./epriseCation/cominFormation";
import { getToken, setToken, removeToken } from "@/utils/auth";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import Storage from "@/utils/storage";

export default {
  components: {
    cominFormation,
    eCantion,
    uploadExcel,
    Breadcrumb,
    Hamburger,
    ErrorLog,
    Screenfull,
    SizeSelect,
    Search,
    passwordForm,
    usersForm
  },
  data() {
    /**
     * verifyName
     * 名字验证
     */
    var verifyName1 = (rulue, value, callback) => {
      // if (!value) {
      //   return callback(new Error('年龄不能为空'));
      // }
      var vName = /^[\u4e00-\u9fa5a-zA-Z]{1,6}$/;
      if (!vName.test(value)) {
        this.formData.tenantShortName = "";
        return callback(new Error("请输入中文和字母并且不能超过6个字符"));
      } else {
        callback();
      }
    };
    return {
      cantionTapNum: 1,
      //营业执照
      newbusinessLogo: "",
      beforbusinessLogo: "",
      //已更改标记
      newbeiforlogo: "",
      getbeiforlogo: "",
      gen: -1,
      genZ: -1,
      eCantionData: "",
      activeName: "first",
      elementIcons,
      setdeleat: false,
      fale: true,
      setimgs: false,
      setNewdisable: true,
      jurisdiction: "",
      isTrial: "全部",
      trialDate: new Date(),
      status: "全部",
      responses: "",
      imgsrc: this.imgUrlName,
      imgsrc1: "",
      setDisable: false,
      add: true,
      dialogImageUrl: "",
      dialogVisible: true,
      dialogTableVisible: false,
      imageUrl: "",
      tenantScale: "",
      area: [],
      industry: [],
      genre: "",
      formData: {
        genre: "",
        id: 0,
        tenantName: "",
        tenantShortName: "",
        tenantEngName: "",
        isTrial: false,
        trialDate: new Date(),
        status: false,
        templateId: false,
        validityPeriod: new Date(),
        industry: "",
        legalPerson: "",
        tenantScale: "",
        registeredCapital: 0,
        mainBusiness: "",
        fixedTele: "",
        email: "",
        address: "",
        tUsersUserName: "",
        rePublish: false,
        createTime: new Date(),
        createId: 0,
        tenantLogo: "",
        imgs: "",
        area: ""
      },
      formRules: {
        tenantName: [
          { required: true, message: "请输入企业名称", trigger: "blur" },
          {
            min: 0,
            max: 100,
            message: "长度需要在0到100个字符",
            trigger: "blur"
          }
        ],
        // tenantShortName: [
        //   { validator: verifyName1, trigger: "blur", required: true }
        // ],
        // tenantEngName: [
        //   { required: true, message: "请输入英文名称", trigger: "blur" },
        //   {
        //     min: 0,
        //     max: 100,
        //     message: "长度需要在0到100个字符",
        //     trigger: "blur"
        //   }
        // ],
        bulletinContent: [
          { required: true, message: "请输入公告详情", trigger: "blur" },
          {
            min: 10,
            max: 200,
            message: "详情字符范围在10到200个",
            trigger: "blur"
          }
        ],
        trialDate: [
          {
            type: "date",
            required: true,
            message: "试用有效期",
            trigger: "change"
          }
        ],
        validityPeriod: [
          {
            type: "date",
            required: true,
            message: "租户有效期",
            trigger: "change"
          }
        ],
        area: [
          { required: true, message: "请选择所属地区", trigger: "change" }
        ],
        industry: [
          { required: true, message: "请选择所属行业", trigger: "change" }
        ],

        createTime: [
          {
            type: "date",
            required: true,
            message: "请选择创建时间",
            trigger: "change"
          }
        ]
      }
    };
  },
  filters: {
    formatTDate: value => {
      if (value != null) {
        var d = value.split("T");
        var day = d[0].slice(5);
        var hour = d[1].slice(0, 5);
      }
      return d ? `${day} ${hour}` : value;
    }
  },
  computed: {
    ...mapGetters(["sidebar", "avatar", "device"]),
    ...mapState({
      msgData: state => state.user.msgData
    })
  },
  mounted() {
    this.imgsrc = this.$store.state.user.imgUrlName;
  },
  created() {
    this.getMsg();
    /**
     * 初始化权限接口
     *
     */
    this.initJurisdiction(); //公司
    // 初始化接收所属地区
    var areaKeyId = 161;
    var areaKeyIds = [
      {
        column: "keyId",
        content: areaKeyId,
        codition: 0
      }
    ];
    var area = RequestObject.CreateGetObject(false, 0, 0, areaKeyIds, null);
    request({
      url: "/platform/api/DictionaryValues",
      method: "get",
      params: {
        requestObject: JSON.stringify(area)
      }
    }).then(res => {
      this.area = res.data;
    });
    var industrKeyId = 162;
    var industrKeyIds = [
      {
        column: "keyId",
        content: industrKeyId,
        codition: 0
      }
    ];
    var industr = RequestObject.CreateGetObject(
      false,
      0,
      0,
      industrKeyIds,
      null
    );
    request({
      url: "/platform/api/DictionaryValues",
      method: "get",
      params: {
        requestObject: JSON.stringify(industr)
      }
    }).then(res => {
      this.industry = res.data;
    });
  },
  methods: {
    getMsg() {
      this.$store.dispatch("user/getMsg");
      setTimeout(() => {
        this.$store.dispatch("user/getMsg");
      }, 1000 * 60 * 3);
    },
    readMsg() {
      this.$refs.popover.doClose();
      this.$router.push({
        name: "viewsSysSettingsSystemMsgindexvue"
      });
    },
    handleClose() {
      this.OnBtnClose();
    },
    eCantion(data) {
      if (data) {
        this.genZ = 0;
        this.newbusinessLogo = data.newVals;
        this.beforbusinessLogo = data.oldvals;
      } else {
        this.genZ = -1;
      }
    },
    oldval(data) {
      if (data) {
        this.gen = 0;
        this.newbeiforlogo = data.newVals;
        this.getbeiforlogo = data.oldvals;
      } else {
        this.gen = -1;
      }
    },
    dataShik(data) {
      this.eCantionData = data;
    },
    fatheJieShou(data, num) {
      this.activeName = data;
    },
    handleClick(tab, event) {},
    setApprove() {
      this.activeName = "second";
    },

    generateIconCode(symbol) {
      this.icon = false;
      return symbol;
    },

    /**
     * 开始
     */
    /**
     * openUsers
     * 查看个人信息
     */
    openUsers() {
      this.$refs.userVisible.userFormVisible = true;
      this.$refs.userVisible.Users();
    },
    /**
     * openPassword
     * 修改密码
     */
    openPassword() {
      this.$refs.password.passwordFormVisible = true;
    },
    /**
     * 结束
     */

    OnBtnClose() {
      if (this.gen == 0) {
        var deleatImg = this.imgsrc + this.newbeiforlogo;
        request({
          url: this.$ajaxUrl + "/fileupload/api/files/delete",
          method: "delete",
          params: {
            strFileName: this.newbeiforlogo
          }
        }).then(res => {
          this.dialogTableVisible = false;
          this.gen = -1;
        });
      } else if (this.genZ == 0) {
        request({
          url: this.$ajaxUrl + "/fileupload/api/files/delete",
          method: "delete",
          params: {
            strFileName: this.newbusinessLogo
          }
        }).then(res => {
          this.dialogTableVisible = false;
          this.genZ = -1;
        });
      } else {
        this.dialogTableVisible = false;
        return;
      }
    },
    /**
     * setBusiness
     * 设置企业信息
     * 要保存数据的话就保存this.responses
     */
    async setBusiness() {
      this.dialogTableVisible = true;
      this.initJurisdiction();
    },

    /**
     * setDeleat
     * 删除图片的方法
     */
    setDeleat() {
      request({
        url: this.$ajaxUrl + "/fileupload/api/files/delete",
        method: "delete",
        params: {
          strFileName: this.formData.tenantLogo
        }
      }).then(res => {
        this.formData.tenantLogo = "";
        this.imgsrc = "";
        this.setdeleat = false;
        this.imageUrl = "";
      });
    },
    handleAvatarSuccess(res, file) {
      this.setdeleat = true;
      this.imgsrc = this.$store.state.user.imgUrlName;
      this.formData.tenantLogo = res;
      this.imageUrl = " this.imgsrc + res";
    },
    beforeAvatarUpload(file) {
      const isJPG = file.type === "image/jpeg";
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isJPG) {
        this.$message.error("上传头像图片只能是 JPG 格式!");
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 2MB!");
      }
      return isJPG && isLt2M;
    },
    deleatIMGwORK() {
      if (this.gen == 0) {
        if (this.getbeiforlogo) {
          var deleatImg = this.imgsrc + this.getbeiforlogo;
          request({
            url: this.$ajaxUrl + "/fileupload/api/files/delete",
            method: "delete",
            params: {
              strFileName: this.getbeiforlogo
            }
          }).then(res => {
            this.gen = -1;
          });
        }
      } else if (this.genZ == 0) {
        if (this.beforbusinessLogo) {
          var deleatImg = this.imgsrc + this.beforbusinessLogo;
          request({
            url: this.$ajaxUrl + "/fileupload/api/files/delete",
            method: "delete",
            params: {
              strFileName: this.beforbusinessLogo
            }
          }).then(res => {
            this.genZ = -1;
          });
        }
      }
    },

    OnBtnSaveSubmit() {
      if (this.formData.isAdmin) {
        this.$confirm("此操作将永久保存企业信息, 是否继续?", "提示", {
          cancelButtonText: "取消",
          confirmButtonText: "确定",
          type: "warning"
        })
          .then(() => {
            if (
              this.formData.companyName == "" ||
              this.formData.businessLogo == ""
            ) {
              this.$message("请填写好相关信息以及企业认证");
            } else {
              this.deleatIMGwORK(); //删除图片
              var comindata = {
                id: this.formData.id,
                companyName: this.formData.companyName,
                legalPerson: this.formData.legalPerson,
                contactNumber: this.formData.contactNumber,
                tenantShortName: this.formData.tenantShortName,
                tenantEngName: this.formData.tenantEngName,
                tenantLogo: this.formData.tenantLogo,
                companyInfoId: this.formData.companyInfoId,
                // enterpriseType: this.formdata.enterpriseType,
                enterpriseType: this.formData.enterpriseType,
                cId: this.formData.cId,
                businessLogo: this.formData.businessLogo
              };

              var postdata = RequestObject.CreatePostObject(comindata);
              request({
                url: "/system/api/TSMCompany/ModifyCurentCompany",
                method: "PUT",
                data: postdata
              })
                .then(res => {
                  if (res.code == 0) {
                    this.$message({
                      message: "操作成功",
                      type: "success"
                    });
                    var imge = this.imgsrc + this.formData.tenantLogo;
                    this.$store.dispatch(
                      "user/setlogName",
                      this.formData.companyName
                    );
                    this.$store.dispatch(
                      "user/setlogNameEnglish",
                      this.formData.tenantEngName
                    );
                    if (
                      this.formData.tenantLogo != null &&
                      this.formData.tenantLogo != "null"
                    ) {
                      this.$store.dispatch("user/setlog", imge);
                    }

                    this.dialogTableVisible = false;
                  } else {
                    this.$message.error("保存失败");
                  }
                })
                .catch(res => {
                  this.$message.error("保存失败");
                });
            }
          })
          .catch(() => {
            this.$message({
              type: "info",
              message: "已取消"
            });
          });
      } else {
        this.dialogTableVisible = false;
      }
    },
    GetPostData() {
      var result = {
        genre: Number(this.formData.genre),
        id: this.formData.id,
        tenantName: this.formData.tenantName,
        tenantShortName: this.formData.tenantShortName,
        tenantEngName: this.formData.tenantEngName,
        area: Number(this.formData.area),
        industry: Number(this.formData.industry),
        legalPerson: this.formData.legalPerson,
        tenantScale: Number(this.formData.tenantScale),
        registeredCapital: Number(this.formData.registeredCapital),
        mainBusiness: this.formData.mainBusiness,
        fixedTele: this.formData.fixedTele,
        email: this.formData.email,
        address: this.formData.address,
        tenantLogo: this.formData.tenantLogo
      };
      return result;
    },
    initJurisdiction() {
      // con
      request({
        url: "/system/api/TSMCompany/PersonalGet",
        method: "GET"
      }).then(res => {
        // console.log(res);
        this.jurisdiction = res.data;
        this.formData = res.data;
        if (localStorage.getItem("companyId") != null) {
          //如果有公司id
          // this.checked = JSON.parse(localStorage.getItem("checked"));
          if (res.data.companyInfoId == localStorage.getItem("companyId")) {
            // Storage.LStorage.removeColde()
            // localStorage.setItem("companyId", JSON.stringify(res.data.companyInfoId));
            // console.log(1)
          } else {
            Storage.LStorage.removeColde();
            var time = setTimeout(() => {
              localStorage.setItem(
                "companyId",
                JSON.stringify(res.data.companyInfoId)
              );
              clearTimeout(time);
            }, 100);
          }
        } else {
          localStorage.setItem(
            "companyId",
            JSON.stringify(res.data.companyInfoId)
          );
        }
      });
    },
    toggleSideBar() {
      this.$store.dispatch("app/toggleSideBar");
    },
    logout() {
      /**
       * 接口改变直接请求一下
       * 暂时解决这个问题
       */
      // this.$router.push('/login');
      // removeToken();
      // window.location.href='/login'
      // window.location.href = "/login/index.vue";
      // window.open("/login/index.vue")
      console.log("退出登录");
      request({
        url: "/systemlogin/api/Logout",
        method: "POST"
      }).then(res => {
          removeToken();
          localStorage.removeItem("lodinData");
          localStorage.removeItem("checked");
          window.location.href = "/";
      }).catch(res=>{
        removeToken();
          localStorage.removeItem("lodinData");
          localStorage.removeItem("checked");
          window.location.href = "/";
      });
    },

    /**
     * 查询接口
     *handleQuery
     */
    handleQuery() {
      var queryConditions = [];
      var orderConditions = [];
      queryConditions.push({
        column: "tenantName",
        content: this.bulletinTitle,
        condition: 0
      });
      var queryRequest = RequestObject.CreateGetObject(
        false,
        0,
        0,
        queryConditions,
        null
      );
      request({
        url: "/platform/api/TTenants",
        method: "get",
        params: {
          requestObject: JSON.stringify(queryRequest)
        }
      }).then(res => {
        // this.tableData = res.data;
        // this.totalCount = res.totalNumber;
      });
    }
  }
};
</script>

<style lang="scss" scoped>
@import "../../assets/iconfot/iconfont.css";

/deep/.el-card__body {
  padding: 10px 10px;
}
.el-card.is-always-shadow {
  -webkit-box-shadow: 0 0px 0px 0 rgba(0, 0, 0, 0.1);
  box-shadow: 0 0px 0px 0 rgba(0, 0, 0, 0.1);
}
.el-card {
  border: 1px solid #e6ebf5;
  cursor: pointer;
}
.el-card-none {
  border-bottom: 0px solid #e6ebf5;
}
/deep/.el-dialog__body {
  padding: 0;
}
.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178px;
  height: 178px;
  line-height: 178px;
  text-align: center;
}
.avatar {
  width: 178px;
  height: 178px;
  display: block;
}

.el-tab-pane {
  display: flex;
  flex-wrap: wrap;
  .icon-item {
    font-size: 36px;
    width: 50px;
    height: 50px;
    text-align: center;
    line-height: 50px;
  }
}
.icons-container {
  margin: 10px 20px 0;
  overflow: hidden;

  .icon-item {
    margin: 20px;
    height: 85px;
    text-align: center;
    width: 100px;
    float: left;
    font-size: 30px;
    color: #24292e;
    cursor: pointer;
  }

  .disabled {
    pointer-events: none;
  }
}
.cantion {
  // padding: 0px 30px;
  .el-select {
    .el-select-dropdown__wrap {
      width: 200px;
    }
    .el-input {
      width: 100%;
    }
  }
  .cantionTap {
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
  .svg-icon[data-v-c8a70580] {
    width: 36px;
    height: 36px;
    text-align: center;
    line-height: 36px;
  }
}
.iconBox {
  display: flex;
  flex-wrap: wrap;
  div {
    width: 50px;
    height: 40px;
    font-size: 20px;
    text-align: center;
    line-height: 40px;
  }
}
.box {
  background: rgba($color: #0080ff, $alpha: 0.3);
}

.navbar {
  height: 50px;
  overflow: hidden;
  position: relative;
  background: #fff;
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.16);
  .messageBox {
    display: inline-block;
    width: 50px;
    height: 50px;
    .msgLg {
      text-align: center;
      font-size: 20px;
      cursor: pointer;
      width: 20px;
      height: 20px;
      vertical-align: middle;
      margin-left: 15px;
      margin-top: 15px;
      i {
        vertical-align: top;
      }
    }
  }
  .hamburger-container {
    line-height: 46px;
    height: 100%;
    float: left;
    cursor: pointer;
    transition: background 0.3s;
    -webkit-tap-highlight-color: transparent;

    &:hover {
      background: rgba(0, 0, 0, 0.025);
    }
  }

  .breadcrumb-container {
    float: left;
  }

  .errLog-container {
    display: inline-block;
    vertical-align: top;
  }

  .right-menu {
    float: right;
    height: 100%;
    line-height: 50px;

    &:focus {
      outline: none;
    }

    .right-menu-item {
      display: inline-block;
      padding: 0 8px;
      height: 100%;
      font-size: 18px;
      color: #5a5e66;
      vertical-align: text-bottom;

      &.hover-effect {
        cursor: pointer;
        transition: background 0.3s;

        &:hover {
          background: rgba(0, 0, 0, 0.025);
        }
      }
    }

    .avatar-container {
      margin-right: 30px;

      .avatar-wrapper {
        margin-top: 5px;
        position: relative;

        .user-avatar {
          cursor: pointer;
          width: 40px;
          height: 40px;
          border-radius: 10px;
        }

        .el-icon-caret-bottom {
          cursor: pointer;
          position: absolute;
          right: -20px;
          top: 25px;
          font-size: 12px;
        }
      }
    }
  }
}
.showMore {
  width: 100%;
  padding: 10px 0px;
  text-align: center;
  color: #1c92ff;
  background: #f3f4f8;
  position: absolute;
  bottom: 0px;
  cursor: pointer;
  font-size: 12px;
}
.noList {
  text-align: center;
  margin-top: 86px;
}
.innerbox {
  position: relative;
  overflow: auto;
  .fl {
    float: left;
  }
  .fr {
    float: right;
  }
  .col1 {
    color: #444444;
  }
  .col2 {
    color: #999999;
    font-size: 12px;
    line-height: 24px;
  }
  .col3 {
    color: #9a9a9a;
  }
  ul {
    padding: 0px;
    cursor: pointer;
  }
  li {
    list-style: none;
    border-bottom: 1px solid #dfdfdf;
    padding: 0px 12px 6px;
  }
}

.innerbox::-webkit-scrollbar {
  /*滚动条整体样式*/
  width: 4px; /*高宽分别对应横竖滚动条的尺寸*/
  height: 4px;
}
.innerbox::-webkit-scrollbar-thumb {
  /*滚动条里面小方块*/
  border-radius: 5px;
  -webkit-box-shadow: inset 0 0 5px rgba(0, 0, 0, 0.2);
  background: rgba(0, 0, 0, 0.2);
}
.innerbox::-webkit-scrollbar-track {
  /*滚动条里面轨道*/
  -webkit-box-shadow: inset 0 0 5px rgba(0, 0, 0, 0.2);
  border-radius: 0;
  background: rgba(0, 0, 0, 0.1);
}
.el-dropdown-menu--small .el-dropdown-menu__item {
  padding: 0px;
  .linkBox {
    padding: 0px 15px;
  }
}
.el-dropdown-menu--small
  .el-dropdown-menu__item.el-dropdown-menu__item--divided:before {
  margin: 0px;
}
</style>
