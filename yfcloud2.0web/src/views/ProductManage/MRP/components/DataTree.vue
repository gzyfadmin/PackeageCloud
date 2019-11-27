<template>
  <div id="DataTree">
    <div class="filterBox">
      <el-input placeholder="名称" v-model="filterText" clearable>
        <i slot="suffix" class="el-input__icon el-icon-search"></i>
      </el-input>
    </div>

    <div class="treeBox">
      <div class="treeScrollBox">
        <!-- <el-tree
          class="filter-tree"
          :data="BrandData"
          :props="defaultProps"
          default-expand-all
          :filter-node-method="filterNode"
          ref="tree"
          @node-click="nodeClick"
        >
          <span slot-scope="{ node, data }" class="custom-tree-node pr">
            <span :id="'tree-'+data.id" :ref="'tree-'+data.id">{{ data.dicCode }}{{ data.dicValue }}</span>
          </span>
        </el-tree>-->
        <div
          class="el-tree-node__content"
          :class="{checkState:item.checkState}"
          v-for="item in BrandData"
          :key="item.id"
        >
          <div
            class="ellipsis"
            :title="item.dicCode+' '+item.dicValue"
          >{{ item.dicCode }} {{ item.dicValue }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { closest } from "@/utils/Dom";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { formatDate, trim } from "@/utils/common.js";

export default {
  data() {
    return {
      filterText: "",
      treeClickItem: {},
      BrandData: [],
      BrandData2: [],
      defaultProps: {
        children: "children",
        label: "label"
      }
    };
  },
  props: {
    editState: {
      // default: false,
    }
  },
  watch: {
    filterText(val) {
      this.BrandData = this.BrandData2.filter(item => {
        if (`${item.dicCode} ${item.dicValue}`.indexOf(trim(val)) !== -1) {
          return item;
        }
      });
    }
  },

  mounted() {
    this.getBrand();
  },
  methods: {
    filterNode(value, data) {
      if (!value) return true;
      var dt = data.dicCode + data.dicValue;
      return dt.indexOf(value) !== -1;
    },
    nodeClick(item) {
      // console.log(this.editState);
      if (this.editState === true) {
        this.$confirm("是否保存数据？", "提示", {
          confirmButtonText: "是",
          cancelButtonText: "否",
          type: "warning",
          distinguishCancelAndClose: true
        })
          .then(() => {
            this.$emit("doSave", false);
          })
          .catch(res => {
            if (res == "close") return;
            this.treeClickItem = item;
            this.setTreeStyle();
            this.$emit("nodeClick", item);
            this.$emit("setEditState", false);
          });
        return;
      }
      this.treeClickItem = item;
      this.setTreeStyle();
      this.$emit("nodeClick", item);
    },
    setTreeStyle() {
      // 设置选中的样式
      var a = document.querySelectorAll("div.el-tree-node__content");
      a.forEach(item => {
        item.style.background = "#FFF";
      }); // 重置所有按钮样式
      closest(
        this.$refs["tree-" + this.treeClickItem.id],
        ".el-tree-node__content"
      ).style.background = "#F5F7FA"; // 点击按钮样式
      // 获取右侧table数据
    },
    getBrand() {
      var reqObject = RequestObject.LonginBookObject(false, 0, 0, null, null, [
        {
          column: "createTime",
          condition: "desc"
        }
      ]);
      request({
        url: "/basicset/api/TBMPackage",
        method: "get",
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        this.loading = false;
        if (res.code == -1) {
          this.$confirm(res.info, "错误信息", {
            confirmButtonText: "确定",
            type: "error",
            showCancelButton: false
          });
        } else {
          this.BrandData = this.setDefault(res.data);
          this.BrandData2 = this.setDefault(res.data);
          this.$emit("setBrand", res.data);
        }
      });
    },
    setColor(arr) {
      this.BrandData.map(item => {
        this.$set(item, "checkState", false);
        arr.forEach(val => {
          if (item.id == val) {
            this.$set(item, "checkState", true);
          }
        });
      });
    },
    setDefault(dt) {
      dt.map(item => {
        this.$set(item, "checkState", false);
      });
      return dt;
    }
  }
};
</script>

<style lang="scss" scoped>
#DataTree /deep/ {
  height: 100%;
  padding-top: 53px;
  position: relative;
  .el-tree-node__content {
    padding: 20px 20px;
    font-size: 12px;
    border-top: 1px solid #dfe6ec;
    cursor: pointer;
    color: #606266;
    // padding-left: 24px;
  }
  .el-tree-node__content:first-child {
    border-top: 0px solid #dfe6ec !important;
  }
  .treeBox {
    // padding-top: 53px;
    height: 100%;
    overflow: auto;
  }
  .treeScrollBox {
    // border-top: 1px solid #dfe6ec;
  }
  .el-tree-node:focus > .el-tree-node__content {
    background-color: #fff;
  }
  .active {
    background: #f5f7fa;
  }
  .filterBox {
    position: absolute;
    background: rgb(255, 255, 255);
    width: 198px;
    padding: 10px;
    z-index: 99;
    // border-right: 1px solid #dfe6ec;
    top: 0px;
    border-bottom: 1px solid #dfe6ec;
  }
  .checkState {
    background: #f5f7fa;
  }
}
</style>

