<template>
  <div :class="className" :style="{height:height,width:width}" />
</template>

<script>
import echarts from "echarts";
require("echarts/theme/macarons"); // echarts theme
import resize from "@/views/dashboard/admin/components/mixins/resize";
import BigNumber from "bignumber.js";
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
        // color: ["#3398DB"],
        tooltip: {
          trigger: "axis",
          axisPointer: {
            // 坐标轴指示器，坐标轴触发有效
            type: "shadow" // 默认为直线，可选为：'line' | 'shadow'
          }
        },
        grid: {
          left: "3%",
          right: "4%",
          bottom: "3%",
          containLabel: true
        },
        xAxis: [
          {
            type: "category",
            data: ["本月入库量", "本月出库量", "库存量"],
            axisTick: {
              alignWithLabel: true
            }
          }
        ],

        tooltip: {
          axisPointer: {
            type: ""
          }
        },
        yAxis: [
          {
            name: "万",
            type: "value"
          }
        ],
        series: [
          {
            name: "",
            type: "bar",
            barWidth: "20%",
            barGap: 10,
            data: [
              BigNumber(res.inAmountCount)
                .div(10000)
                .toNumber(),
              BigNumber(res.outAmountCount)
                .div(10000)
                .toNumber(),
              BigNumber(res.inventory)
                .div(10000)
                .toNumber()
            ],
            // itemStyle: {
            //   normal: {
            //     label: {
            //       show: true, //开启显示
            //       position: "top", //在上方显示
            //       // rotate: 0, //倾斜度 -90 至 90 默认为0
            //       textStyle: {
            //         //数值样式
            //         color: "#2ec7c9",
            //         fontSize: 14
            //       }
            //     }
            //   }
            // }
            // emphasis: {
            //   label: {
            //     show: true,
            //     position: "top"
            //   }
            // }
          }
        ]
      });
    }
  }
};
</script>
<style scoped>
</style>
