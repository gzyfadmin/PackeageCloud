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
  mounted() {},
  methods: {
    lodingData(res) {
      this.chart = echarts.init(this.$el, "macarons");
      // 绘制图表
      // console.log(res);
      var dtlist = [];
      var dtName = [];
      var sum = 0;
      res.map(item => {
        dtlist.push({value: item.value, name: item.name});
        dtName.push(item.name)
        sum = BigNumber(item.value).plus(sum).toNumber()
      });

      this.chart.setOption({
        tooltip: {
          trigger: "item",
          formatter: "{a} {b}: {c} ({d}%)"
        },
        series: [
          {
            name: "",
            type: "pie",
            radius: ["50%", "70%"],
            avoidLabelOverlap: false,
            label: {
              normal: {
                show: true,
                position: "center",
                formatter:function(params) {
                  var texts = "";
                  texts += "库存总量\n"+sum;
                  return texts;
                },
                textStyle: {
                  fontSize: "14",
                }
              },
              emphasis: {
                show: false,
                textStyle: {
                  fontSize: "14",
                  fontWeight: "bold"
                }
              }
            },
            labelLine: {
              normal: {
                show: false
              }
            },
            data: dtlist
          }
        ]
      });
    }
  }
};
</script>
<style scoped>
</style>
