<template>
  <div :class="className" :style="{height:height,width:width}" />
</template>

<script>
import echarts from "echarts";
require("echarts/theme/macarons"); // echarts theme
import resize from "@/views/dashboard/admin/components/mixins/resize";
import BigNumber from "bignumber.js";
import { delcommafy } from "@/utils/common.js";

export default {
  mixins: [resize],
  props: {
    className: {
      type: String,
      default: "chart"
    },
    width: {
      type: String,
      default: "100%"
    },
    height: {
      type: String,
      default: "300px"
    }
  },
  data() {
    return { chart: null };
  },
  mounted() {},
  methods: {
    lodingData(res) {
      this.chart = echarts.init(this.$el, "macarons");
      // 绘制图表
      var dtlist = [];
      res.seriesData.map(item => {
        dtlist.push(delcommafy(item));
      });
      this.chart.setOption({
        tooltip: {
          trigger: "axis"
        },
        // grid: {
        //   left: "3%",
        //   right: "4%",
        //   bottom: "3%",
        //   containLabel: true
        // },
        xAxis: {
          type: "category",
          data: res.xAxisData
        },
        yAxis: {
          type: "value"
        },
        series: [
          {
            data: dtlist,
            type: "line"
          }
        ]
      });
    }
  }
};
</script>
<style scoped>
</style>
