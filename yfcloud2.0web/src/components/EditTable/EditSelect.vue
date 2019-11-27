<template>
  <div :class="{validStyle:value.valid}" class="EditSty">
    <div @click.stop="doShow" v-show="!value.isShow" class="tableTd">
      <div class="ellipsis" :title="value.value">{{value.value}}</div>
    </div>
    <el-select
      :filterable="filterable"
      :clearable="clearable"
      :ref="value.id"
      :id="value.id"
      v-if="value.isShow"
      type="text"
      v-model="model"
      @change="doChange"
      @click.stop.native="doClick"
      :filter-method="filter_Method"
    >
      <el-option v-for="item in data2" :key="item.id" :value="item.id" :label="item[label]" />
    </el-select>
    <!-- warehouseName -->
  </div>
</template>
 
<script>
import { trim } from "@/utils/common.js";
export default {
  data() {
    return {
      data2: []
    };
  },
  props: {
    value: {
      type: Object,
      default: {}
    },
    itemList: {},
    type: {
      default: -1
    },
    data: {
      type: Array,
      default: []
    },
    isShowFlag: {
      type: Boolean,
      default: true
    },
    label: {
      type: String,
      required: true
    },
    filterable: {
      type: Boolean,
      default: false
    },
    clearable: {
      type: Boolean,
      default: false
    },
    filterMethod: {
      default: -1
    }
  },
  computed: {
    model: {
      get() {
        return this.value.key;
      },
      set(newVal) {
        var val = "";
        this.data.map(item => {
          if (item.id == newVal) {
            val = item[this.label];
          }
        });
        let obj = {};
        for (var key in this.value) {
          obj[key] = this.value[key];
        }
        obj.value = val;
        obj.key = newVal;
        this.$emit("input", obj);

        // this.$emit("input", {
        //   value: val,
        //   key: newVal,
        //   id: this.value.id,
        //   isShow: this.value.isShow,
        //   valid: false
        // });
      }
    }
  },
  mounted() {
    this.data2 = this.data;
  },
  methods: {
    filter_Method(val) {
      // console.log(val)
      if (this.filterMethod != -1) {
        this.data2 = this.filterMethod(val);
      } else {
        this.data2 = this.data.filter(item => {
          if (`${item[this.label]}`.indexOf(trim(val)) !== -1) {
            return item;
          }
        });
      }
    },
    doShow() {
      this.$emit("clickEvent", this.itemList, this.value, this.type);
      if (this.isShowFlag === false) return;
      this.value.isShow = true;
      setTimeout(() => {
        document.getElementById(this.value.id).click();
        this.$emit("clickEventAfter", this.itemList, this.value);
      }, 200);
    },
    doChange() {
      this.$emit("change");
    },
    doClick() {
      if (this.data.length != this.data2.length) {
        this.data2 = this.data;
      }
      this.$emit("doClick");
    },
    setSelectData(dt) {
      this.data2 = dt;
    }
  }
};
</script>

<style lang="scss" scoped>
.tableTd {
  box-sizing: content-box;
  min-height: 23px;
  padding: 8px 10px;
}
</style>