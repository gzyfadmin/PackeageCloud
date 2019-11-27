<template>
  <!--头部信息-->
  <div class="login_header">
    <div class="login_h_w">
      <div class="header_left">
        <p class="header_logo">
          <a href="http://bao.cloudyf.com">
          <img src="./images/reg_logo_03.png" alt="包管云LOGO" /></a>
        </p>
        <ul class="login_nav">
          <li>
            <a href="http://bao.cloudyf.com">产品优势</a>
          </li>
          <li>
            <a href="http://bao.cloudyf.com">应用场景</a>
          </li>
          <li>
            <a href="http://bao.cloudyf.com">版本介绍</a>
          </li>
          <li>
            <a href="http://bao.cloudyf.com">关于云飞</a>
          </li>
        </ul>
      </div>
      <div class="header_bnt" v-if="type==1">已有账号，立即<a class="lgoin_bnt" href="#" @click="goPage">登录</a></div>	
    </div>
  </div>
  <!--底部信息-->
  <!-- <div class="login_footer">Copyright ©2018 云飞科技湘ICP备14003093号-1</div> -->
  <!-- </div> -->
</template>
<script>
import { removeToken } from "@/utils/auth.js";
export default {
  props: {},
  data: () => ({}),
  props: ['type'],
  mounted() {},
  methods: {
    goPage(){
      removeToken()
      this.$router.push("/login");
      // window.location.href = "/#/login?redirect=%2F";
    }
  }
};
</script>
<style style lang="scss" scoped>
@import "../../styles/login_CSS.css";
@import "../../styles/icon/iconfont.css";
img{
  width:140px;
}
</style>
