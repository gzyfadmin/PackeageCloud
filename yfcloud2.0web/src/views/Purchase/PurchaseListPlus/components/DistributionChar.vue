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
    packageData(res) {
      this.chart = echarts.init(this.$el, 'macarons')
      // 绘制图表
      var sum = 0;
      for(var i=0;i<res.length;i++) {
       sum = BigNumber(res[i].value).div(10000).plus(sum).toNumber()
      }
      this.chart.setOption({
        tooltip: {
          trigger: "item",
          formatter: function(params) {
            var texts = "";
            // '订单数量:'+params.data.value+'&nbsp'+'&nbsp'+'&nbsp'+'&nbsp'+
            texts +='姓名：'+params.data.userName + ' &nbsp;订单数量：' + params.data.orderCount + ' &nbsp;采购金额：' + BigNumber(params.data.value).div(10000).toNumber();
            return texts;
          }
        },
        graphic: {
          type: "text",
          left: "center",
          top: "center",
          style: {
            text: "各服装销售比",
            fill: "#fff",
            fontSize: 16,
            fontWeight: "bold"
          }
        },
        //   title: [
        //     {
        //       text: "业务业绩",
        //       top: "5%",
        //       left: "50%",
        //       textStyle: {
        //         color: "#000",
        //         fontSize: 18
        //       }
        //     }
        //   ],
        series: [
          {
            name: "访问来源",
            type: "pie",
            radius: ["50%", "70%"],
            avoidLabelOverlap: false,
            label: {
             normal: {
                show: true,
                position: "center",
                formatter:function(params) {
                  var texts = "";
                  texts += "本月\n"+sum;
                  return texts;
                },
                textStyle: {
                  fontSize: "14",
                  fontWeight: "bold"
                }
              },
              emphasis: {
                show: false,
                textStyle: {
                  fontSize: "30",
                  fontWeight: "bold"
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
