<template>
  <div :class="className" :style="{height:height,width:width}" />
</template>

<script>
import echarts from "echarts";
require("echarts/theme/macarons"); // echarts theme
import resize from "@/views/dashboard/admin/components/mixins/resize";
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
      // console.log(res)
      var dtlist = [];
      var dtName = [];
      res.seriesData.map(item => {
        var dt = [];
        item.seriesData.map(val => {
          dt.push(delcommafy(val));
        });
        dtlist.push({
          name: item.name,
          type: "line",
          data: dt
        });
        dtName.push(item.name);
      });

      this.chart.setOption({
        tooltip: {
          trigger: "axis"
        },
        legend: {
          data: dtName
        },
        grid: {
          left: "3%",
          right: "4%",
          bottom: "3%",
          containLabel: true
        },
        xAxis: {
          type: "category",
          boundaryGap: false,
          data: res.xAxisData
        },
        yAxis: {
          type: "value"
        },
        series: dtlist
      });
    }
  }
};
</script>
<style scoped>
</style>
