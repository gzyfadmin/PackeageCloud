<template>
  <el-container class="container">
    <el-header>
      <yfheader type="1"></yfheader>
    </el-header>

    <el-main class="gary_bg">
      <div class="login_content reg_warp_heg pr">
        <h1>用户服务协议</h1>
        <!--服务条款-->
        <div class="serviceBox">
          <div class="contenSb">
            <Termsofservice />
          </div>
        </div>
      </div>
    </el-main>
    <Yffooter></Yffooter>
  </el-container>
</template>
<script>
import { type } from "os";
import request from "@/utils/request";
import rquestObject from "@/utils/requestObject";
import yfheader from "@/components/Yfheader";
import Yffooter from "@/components/Yffooter";
import Termsofservice from "./components/Termsofservice";

import { setToken } from "@/utils/auth.js";
export default {
  name: "SignIn",
  data() {
    return {};
  },
  components: {
    yfheader,
    Yffooter,
    Termsofservice
  },
  mounted() {},
  methods: {}
};
</script>

<style scoped lang="scss">
@import "../../styles/login.css";
@import "../../styles/login_CSS.css";
@import "../../styles/icon/iconfont.css";
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
.login_content /deep/ {
  h1 {
    padding: 20px;
    text-align: center;
    font-size: 24px;
  }
  .serviceBox {
    // position: absolute;
    // left: 0px;
    // top: 121px;
    width: 100%;
    height: 524px;
    background: #fff;
    overflow: hidden;
    z-index: 999;
    padding: 0px 40px;
  }
  .contenSb {
    width: 100%;
    height: 100%;
    border: 1px solid #dfdfdf;
  }
  .el-header{
  }
}
.el-header {
  padding: 0px !important;
}
</style>

