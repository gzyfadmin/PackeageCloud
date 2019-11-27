<template>
  <div :class="{validStyle:value.valid}" class="EditSty">
    <div v-if="autocomplete==true">
      <div @click.stop="doShow" v-show="!value.isShow" class="tableTd">
        <div class="ellipsis" :title="value.value">{{value.value}}</div>
      </div>
      <el-autocomplete
        class="inline-input"
        :ref="value.id"
        :id="value.id"
        v-if="value.isShow"
        @click.native.stop="1==1"
        @blur="blurCheck(1)"
        @keyup.enter.native="blurCheck(2)"
        @input="doInput"
        v-model="model"
        :value="label"
        :fetch-suggestions="querySearch"
        placeholder
        @select="handleSelect"
      ></el-autocomplete>
    </div>
    <div v-else-if="type=='date'">
      <div @click.stop="doShow" v-show="!value.isShow" class="tableTd">
        <div class="ellipsis" :title="value.value |formatTDate">{{value.value | formatTDate}}</div>
      </div>
      <el-date-picker
        :ref="value.id"
        :id="value.id"
        v-show="value.isShow"
        v-model="model"
        @click.native.stop="1==1"
        @blur="blurCheck(1)"
        @keyup.enter.native="blurCheck(2)"
        type="date"
        placeholder="选择日期"
        v-on:keydown.native="onKeydown($event)"
        style="width:140px;"
      ></el-date-picker>
    </div>
    <div v-else-if="type=='remark'">
      <div v-show="!value.isShow" @click.stop="doShow" class="tableTd">
        <el-popover
          v-if="value.value!=null&&value.value.length>=20"
          placement="top-start"
          trigger="hover"
          :content="value.value"
        >
          <div class="ellipsis" slot="reference">{{ value.value }}</div>
        </el-popover>
        <div v-if="value.value==null||value.value.length<20" class="ellipsis">{{ value.value }}</div>
      </div>
      <el-input
        :ref="value.id"
        :id="value.id"
        v-show="value.isShow"
        type="text"
        v-model="model"
        @click.native.stop="1==1"
        @blur="blurCheck(1)"
        @keyup.enter.native="blurCheck(2)"
        size="mini"
        maxlength="500"
      />
    </div>
    <div v-else>
      <div @click.stop="doShow" v-show="!value.isShow" class="tableTd">
        <div class="ellipsis" :title="value.value">{{value.value}}</div>
      </div>

      <el-input
        :ref="value.id"
        :id="value.id"
        v-if="value.isShow"
        type="text"
        v-model="model"
        @click.native.stop="1==1"
        @blur="blurCheck(1)"
        @keyup.enter.native="blurCheck(2)"
        @input="doInput"
      />
    </div>
  </div>
</template>
 
<script>
import { formatDate } from "@/utils/common.js";
export default {
  props: {
    value: {
      type: Object,
      default: {}
    },
    data: {},
    itemList: {},
    type: {
      default: -1
    },
    label: {
      type: String,
      default: "value"
    },
    isShowFlag: {
      type: Boolean,
      default: true
    },
    autocomplete: {
      type: Boolean,
      default: false
    }
  },
  filters: {
    formatTDate: value => {
      if (value == "") {
        return "";
      }
      return formatDate(value);
    }
  },
  computed: {
    model: {
      get() {
        return this.value.value;
      },
      set(newVal) {
        // this.$emit("input", {
        //   value: newVal,
        //   key: this.value.key,
        //   id: this.value.id,
        //   isShow: this.value.isShow,
        //   valid: false
        // });

        let obj = {};
        for (var key in this.value) {
          obj[key] = this.value[key];
        }
        obj.value = newVal;
        this.$emit("input", obj);
      }
    }
  },
  methods: {
    createFilter(queryString) {
      return restaurant => {
        return (
          restaurant.value.toLowerCase().indexOf(queryString.toLowerCase()) ===
          0
        );
      };
    },
    querySearch(queryString, cb) {
      var restaurants = this.data;
      if (this.label != "value")
        restaurants.map(item => {
          item["value"] = item[this.label];
        });
      var results = queryString
        ? restaurants.filter(this.createFilter(queryString))
        : restaurants;
      // 调用 callback 返回建议列表的数据
      cb(results);
    },
    handleSelect(item) {
      // console.log(item);
    },
    doShow() {
      this.$emit("clickEvent", this.itemList, this.value, this.type);
      if (this.isShowFlag === false) return;
      this.value.isShow = true;
      this.$nextTick(() => {
        if (this.autocomplete) {
          document.getElementById(this.value.id).focus();
          document.getElementById(this.value.id).select();
          return;
        }

        this.$refs[this.value.id].focus();
        if (this.type != "date") {
          this.$refs[this.value.id].select();
        }

        this.$emit("clickEventAfter", this.itemList, this.value, this.type);
      });
    },
    onKeydown($event) {
      if (event.keyCode !== 9) return;
      this.$emit("onKeydown", $event);
    },
    blurCheck(state) {
      /***
      itemList（整条数据）
      value（点击的组件框数据）
      type（组件状态用于区分组件）
      state（事件状态 1：失去焦点  2：按下Enter）
      ***/
      this.$emit("blurCheck", this.itemList, this.value, this.type, state);
    },
    doInput() {
      this.$emit("inputChange");
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