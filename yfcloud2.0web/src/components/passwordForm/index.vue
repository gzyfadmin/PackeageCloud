<template>
  <div>
    <el-dialog
      title="修改密码"
      :visible.sync="passwordFormVisible"
      label-position="left"
      width="30%"
      :close-on-click-modal="false"
    >
      <div class="password">
        <el-form
          ref="passwordsForm"
          :model="passwordForm"
          size="medium"
          :rules="passwordRules"
        >
          <el-form-item
            label="密码："
            label-width="100px"
            prop="oldPassword"
            style="margin-top:40px"
          >
            <el-input
              v-model="passwordForm.oldPassword"
              show-password
              type="password"
            />
          </el-form-item>
          <el-form-item
            label="新密码："
            label-width="100px"
            prop="nexPassword"
          >
            <el-input
              v-model="passwordForm.nexPassword"
              show-password
              type="password"
            />
          </el-form-item>
          <el-form-item
            label="确认新密码："
            label-width="100px"
            prop="checkNexPassword"
          >
            <el-input
              v-model="passwordForm.checkNexPassword"
              show-password
              type="password"
            />
          </el-form-item>
        </el-form>
      </div>
      <div
        slot="footer"
        class="dialog-footer"
      >
        <el-button
          ref="child"
          @click="childClick"
        >关闭</el-button>
        <el-button
          type="primary"
          @click="getPassword"
        >确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
// export default RequestObject
import request from "@/utils/request";
import { getToken, setToken, removeToken } from "@/utils/auth";
import RequestObject from "@/utils/requestObject";
export default {
  data() {
    var validatePass = (rule, value, callback) => {
      const reg = /^(?![a-zA-Z]+$)(?![A-Z0-9]+$)(?![A-Z._~!@#$^&*]+$)(?![a-z0-9]+$)(?![a-z._~!@#$^&*]+$)(?![0-9._~!@#$^&*]+$)[a-zA-Z0-9._~!@#$^&*]{8,16}$/;
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        this.$nextTick(function() {
          if (this.passwordForm.checkNexPassword !== "") {
            this.$refs.passwordsForm.validateField("checkNexPassword");
          }
        });
        if (reg.test(value)) {
          callback();
        } else {
          callback(new Error("必须包含字母、数字、特殊符号、8~16位数"));
        }
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.passwordForm.nexPassword) {
        callback(new Error("两次输入密码不一致!"));
      } else {
        callback();
      }
    };
    return {
      passwordFormVisible: false,
      passwordForm: {
        oldPassword: "",
        nexPassword: "",
        checkNexPassword: ""
      },
      passwordRules: {
        oldPassword: [
          { required: true, message: "请输入原来的密码", trigger: "blur" }
        ],
        nexPassword: [{ validator: validatePass, trigger: "blur" }],
        checkNexPassword: [{ validator: validatePass2, trigger: "blur" }]
      }
    };
  },
  methods: {
    childClick() {
      // childByValue是在父组件on监听的方法
      // 第二个参数this.childValue是需要传的值
      this.passwordForm = {
        oldPassword: "",
        nexPassword: "",
        checkNexPassword: ""
      };
      this.$refs["passwordsForm"].resetFields();
      this.passwordFormVisible = false;
      // var password = false
      // this.$emit('sendiptVal', password)
    },
    /**
     * getPassword
     * 修改密码
     */
    getPassword() {
      this.$refs.passwordsForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: "数据不合法，无法获取验证码",
            type: "error"
          });
        } else {
          var passwordData = {};
          passwordData.oldPasswd = this.passwordForm.oldPassword;
          passwordData.passwd = this.passwordForm.nexPassword;
          var data = RequestObject.CreatePostObject(
            passwordData,
            null,
            passwordData,
            null
          );
          request({
            url: "/system/api/TSMUser/PersonalSetMobile",
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
                removeToken();
                // localStorage.removeItem("lodinData");
                localStorage.removeItem("checked");
                window.location.reload();
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
.password {
  padding: 0px 30px;
}
.dialog-footer {
  text-align: right;
  padding-right: 10px;
}
</style>
