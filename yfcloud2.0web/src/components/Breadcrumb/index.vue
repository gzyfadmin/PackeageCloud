<template>
  <el-breadcrumb class="app-breadcrumb" separator="/">
    <transition-group name="breadcrumb">
      <el-breadcrumb-item v-for="(item,index) in levelList" :key="item.path">
        <span v-if="item.redirect==='noRedirect'||index==levelList.length-1" class="no-redirect">
          <i
            @click.stop.prevent="1==1"
            v-if="item.meta.title=='首页'"
            class="el-icon-location icoSty"
          ></i>
          <font class="fosize">{{ item.meta.title }}</font>
        </span>
        <a v-else @click.prevent="handleLink(item)">
          <i
            @click.stop.prevent="1==1"
            v-if="item.meta.title=='Dashboard'"
            class="el-icon-location icoSty"
          ></i>
          <font class="fosize">{{ item.meta.title |filterTitle}}</font>
        </a>
      </el-breadcrumb-item>
    </transition-group>
  </el-breadcrumb>
</template>

<script>
import pathToRegexp from "path-to-regexp";

export default {
  data() {
    return {
      levelList: null
    };
  },
  filters: {
    filterTitle(val){
      if(val=='Dashboard'||val=='dashboard'){
        return '首页'
      }else{
        return val
      }
    }
  },
  watch: {
    $route(route) {
      // if you go to the redirect page, do not update the breadcrumbs
      if (route.path.startsWith("/redirect/")) {
        return;
      }
      this.getBreadcrumb();
    }
  },
  created() {
    this.getBreadcrumb();
  },
  methods: {
    getBreadcrumb() {
      // only show routes with meta.title
      let matched = this.$route.matched.filter(
        item => item.meta && item.meta.title
      );
      const first = matched[0];

      if (!this.isDashboard(first)) {
        matched = [{ path: "/dashboard", meta: { title: "Dashboard" } }].concat(
          matched
        );
      }

      this.levelList = matched.filter(
        item => item.meta && item.meta.title && item.meta.breadcrumb !== false
      );
    },
    isDashboard(route) {
      const name = route && route.name;
      if (!name) {
        return false;
      }
      return (
        name.trim().toLocaleLowerCase() === "Dashboard".toLocaleLowerCase()
      );
    },
    pathCompile(path) {
      // To solve this problem https://github.com/PanJiaChen/vue-element-admin/issues/561
      const { params } = this.$route;
      var toPath = pathToRegexp.compile(path);
      return toPath(params);
    },
    handleLink(item) {
      const { redirect, path } = item;
      if (redirect) {
        this.$router.push(redirect);
        return;
      }
      if(path=='/dashboard'){
        this.$router.push("/");
      }else{
        this.$router.push(this.pathCompile(path));
      }
    }
  }
};
</script>

<style lang="scss" scoped>
.app-breadcrumb.el-breadcrumb {
  display: inline-block;
  font-size: 14px;
  line-height: 50px;
  margin-left: 8px;
  a {
    color: #97a8be;
  }
  .no-redirect {
    // color: #97a8be;
    color: #303133;
    cursor: text;
  }
  .fosize {
    font-size: 12px;
  }
  .icoSty {
    font-size: 18px;
    color: #1890ff;
    margin-right: 0px;
  }
}
</style>
