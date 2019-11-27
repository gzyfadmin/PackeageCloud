<template>
  <el-dialog
    :title="title"
    :visible.sync="Visible"
    :width="dialogWidth"
    top
    :modal="appendBody"
    :show-close="showclose"
    class="modalBox"
    :id="refId"
    v-if="Visible"
    :close-on-click-modal="closeClickModal"
    :ref="refId"
  >
    <div class="dialogBody">
      <slot name="dialogBody"></slot>
    </div>

    <span slot="footer" class="dialog-footer">
      <el-button size="small" @click="Visible=false">取 消</el-button>
      <el-button size="small" type="primary" @click="doConfirm" v-if="confirmbtn">确定</el-button>
      <slot name="dialogFooter"></slot>
    </span>
  </el-dialog>
</template>
<script>
export default {
  data: () => ({
    showclose: true,
    Visible: false,
    refId: "yf" + new Date().getTime()
  }),
  props: {
    title: {
      default: "详情"
    },
    dialogWidth: {
      default: "80%"
    },
    dialogHeight: {
      default: "100%"
    },
    appendBody: {
      default: true
    },
    dialogHeight: {
      default: "100%"
    },
    closeClickModal: {
      default: false
    },
    confirmbtn: {
      default: false
    },
    top: {
      default: ""
    },
    center: {
      default: false
    }
  },
  mounted() {},
  watch: {
    Visible(newData) {
      if (newData == false) {
        this.$emit("close");
      }
    }
  },
  methods: {
    doConfirm() {
      this.$emit("doConfirm");
    },
    dialog(type) {
      //打开弹框
      if (type == "open") {
        this.Visible = true;
        this.$nextTick(() => {
          //设置宽度的情况
          var mb = document.getElementById(this.refId);
          mb.style.height = document.documentElement.clientHeight - 84 + "px";
          mb.style.position = "absolute";

          if (this.dialogHeight != "100%") {
            mb.getElementsByClassName("dialogBody")[0].style.height =
              parseInt(this.dialogHeight) - 84 - 54 - 62 - 2 + "px";
          } else {
            mb.getElementsByClassName("dialogBody")[0].style.height =
              document.documentElement.clientHeight - 84 - 54 - 62 - 2 + "px";
          }
          mb.getElementsByClassName("el-dialog__footer")[0].style.borderTop =
            "1px solid #e4e4e4";
          // mb.getElementsByClassName("el-dialog__header")[0].style.borderBottom =
          // "1px solid #e4e4e4";
          mb.getElementsByClassName("el-dialog__body")[0].style.padding = "0px";
          mb.getElementsByClassName("el-dialog__body")[0].style.overflow =
            "auto";
          mb.getElementsByClassName("el-dialog__header")[0].style.padding =
            "10px 20px";
          // mb.getElementsByClassName(
          //   "el-dialog__header"
          // )[0].style.backgroundColor = "#304156";
          mb.getElementsByClassName("el-dialog__title")[0].style.color =
            "#1890ff";
          mb.getElementsByClassName("el-dialog__headerbtn")[0].style.top =
            "13px";
        });
      }
      if (type == "close") {
        this.Visible = false;
      }
    }
  }
};
</script>