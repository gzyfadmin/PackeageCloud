<template>
  <div :class="className" :style="{height:height,width:width}" />
</template>

<script>
import echarts from "echarts";
require("echarts/theme/macarons"); // echarts theme
import resize from "@/views/dashboard/admin/components/mixins/resize";
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
    packageData(res) {
      var classifyData = [];
      var dtData = [];
      for (var i = 0; i < res.length; i++) {
        classifyData.push(res[i].name);
        dtData.push({
          value: res[i].value,
          name: res[i].nameValue
        });
      }
      this.chart = echarts.init(this.$el, "macarons"); // 绘制图表
      this.chart.setOption({
        tooltip: {
          trigger: "item",
          formatter: function(params) {
            var texts = "";
            texts +=
              params.data.name +
              "&nbsp" +
              "&nbsp" +
              params.data.value;
            return texts;
          }
        },

        legend: {
          orient: "vertical",
          zlevel: 2,
          padding: [
            5, // 上
            10, // 右
            5, // 下
            10 // 左
          ],
          x: "left",
          y: "center",
          data: classifyData
        },
        series: [
          {
            type: "pie",
            radius: ["50%", "70%"],
            avoidLabelOverlap: false,
            center: ["65%", "50%"],
            label: {
              normal: {
                show: true,
                position: "center",
                formatter: function(params) {
                  var texts = "本月款型";
                  return texts;
                }
              },
              emphasis: {
                show: false,
                textStyle: {
                  fontSize: "14",
                  fontWeight: "bold",
                }
              }
            },
            labelLine: {
              normal: {
                show: false
              }
            },
            data: res
          }
        ]
      });
    }
  }
};
</script>
<style scoped>
</style>
