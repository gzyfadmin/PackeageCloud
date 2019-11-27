<template>
  <div class="sidebar-logo-container" :class="{'collapse':collapse}">
    <!-- <transition name="sidebarLogoFade"> -->
    <router-link
      key="collapse"
      class="sidebar-logo-link"
      to="/"
    >
      <img
        v-if="$store.state.user.companyData.logimg"
        :src="$store.state.user.companyData.logimg"
        class="sidebar-logo"
      />
      <img v-else class="sidebar-logo" src="./images/yunfeiLg.png" />

      <div class="divs" :class="{divsLong:logName_.length>9}">
        <div class="companySty" v-if="logName_.length<7">{{logName}}</div>
        <div
          v-else
          class="companySty"
          :class="{ 'ExtraLong': logName_.length>7, 'ExtraLongPlus': logName_.length>9 }"
          :title="logName_"
        >{{logName}}</div>
        <div class="userName">{{ this.$store.state.user.name }}</div>
      </div>
    </router-link>
    <!-- <router-link key="expand" class="sidebar-logo-link" to="">
        <img v-if="logo" :src='logo' class="sidebar-logo">
        <h1 class="sidebar-title">{{ title }} </h1>
    </router-link>-->
    <!-- <router-link v-if="$store.state.logoName.avatar!=''" key="collapse" class="sidebar-logo-link" to="">
        <img v-if="logo" :src="$store.state.logoName.avatar" class="sidebar-logo"> 
        <h1 v-else class="sidebar-title">{{ title }}</h1> 
      </router-link>
      <router-link v-else key="expand" class="sidebar-logo-link" to="">
        <img v-if="logo" :src='logo' class="sidebar-logo">
        <h1 class="sidebar-title">{{ title }} </h1>
    </router-link>-->
    <!-- </transition> -->
  </div>
</template>

<script>
import { mapState } from "vuex";
export default {
  name: "SidebarLogo",
  props: {
    collapse: {
      type: Boolean,
      required: true
    }
  },
  computed: {
    ...mapState({
      logName_: state => {
        if (state.user.companyData.logName != null) {
          return state.user.companyData.logName;
        }
        return "";
      },
      logName: state => {
        if (
          state.user.companyData.logName != null &&
          state.user.companyData.logName.length > 9
        ) {
          return state.user.companyData.logName.substring(0, 12);
        }
        if (
          state.user.companyData.logName != null &&
          state.user.companyData.logName.length > 7
        ) {
          return state.user.companyData.logName.substring(0, 9);
        }
        return state.user.companyData.logName;
      }
    })
  },
  created() {
    // console.log(this.$store.state.logoName.avatar)
  },
  mounted() {
    // console.log(this.$store.state.user.companyData.logimg)
    // console.log(this.$store.state.logoName.avatar)
  },
  data() {
    return {
      title: "Vue Element Admin",
      logo:
        "https://wpimg.wallstcn.com/69a1c46c-eb1c-4b46-8bd4-e9e686ef5251.png"
    };
  }
};
</script>

<style lang="scss" scoped>
.divs {
  float: right;
  width: 150px;
  height: 50px;
  color: white;
  font-size: 18px;
  // margin-right:40px;
  // margin-top:-10px;
  vertical-align: none;
}
.divsLong {
  width: 150px;
}
.sidebarLogoFade-enter-active {
  transition: opacity 1.5s;
}

.sidebarLogoFade-enter,
.sidebarLogoFade-leave-to {
  opacity: 0;
}

.sidebar-logo-container {
  position: relative;
  width: 100%;
  height: 50px;
  line-height: 50px;
  // background: #1890ff;
  background: #1481c9;
  text-align: left;
  overflow: hidden;

  & .sidebar-logo-link {
    height: 100%;
    width: 100%;

    & .sidebar-logo {
      width: 36px;
      height: 36px;
      vertical-align: middle;
      margin-right: 10px;
      margin-left: 7px;
      border-radius: 50%;
    }

    & .sidebar-title {
      float: left;
      display: inline-block;
      margin: 0;
      color: #fff;
      font-weight: 600;
      line-height: 50px;
      font-size: 20px;
      font-family: Avenir, Helvetica Neue, Arial, Helvetica, sans-serif;
      // vertical-align: middle;
      margin-left: 20px;
    }
  }

  &.collapse {
    .sidebar-logo {
      margin-right: 0px;
    }
  }
}
.companySty {
  font-size: 16px;
  padding: 0px;
  margin: 0px;
  line-height: 16px;
  margin-top: 8px;
  margin-bottom: 4px;
  white-space: nowrap;
  overflow: hidden;
}
.userName {
  font-size: 12px;
  height: 14px;
  line-height: 14px;
  // margin-left: -72px;
  text-align: left;
}
.ExtraLong {
  font-size: 14px;
}
.ExtraLongPlus{
   font-size: 12px;
}
</style>
