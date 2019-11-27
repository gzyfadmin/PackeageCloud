<template>
  <div
    :class="className"
    :style="{height:height,width:width}"
  />
</template>

<script>
import echarts from "echarts";
require("echarts/theme/macarons"); // echarts theme
import { delcommafy } from "@/utils/common.js";
import resize from "@/views/dashboard/admin/components/mixins/resize";
import BigNumber from 'bignumber.js'
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
  created() {},
  mounted() {},
  methods: {
    createChart(res) {
      this.chart = echarts.init(this.$el, "macarons");
      // 绘制图表
      this.chart.setOption({
        tooltip: {
          trigger: "item",
          formatter: function(params) {
            var texts = "";
            // '订单数量:'+params.data.value+'&nbsp'+'&nbsp'+'&nbsp'+'&nbsp'+
            texts += "采购订单金额: " + params.value + "万";
            return texts;
          }
        },
        xAxis: {
          name: "日",
          type: "category",
          data: res.xAxisData
        },
        yAxis: {
          name: "万",
          type: "value"
        },
        series: [
          {
            data: res.seriesData,
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
