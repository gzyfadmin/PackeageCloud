
<style lang="scss" scoped>
#dashboard /deep/ {
  table {
    // border: 0px solid #fff;
  }
  td {
    //  background-color: #fff !important;
    border-color: #fff !important;
  }

  .fc-unthemed th,
  .fc-unthemed td,
  .fc-unthemed thead,
  .fc-unthemed tbody,
  .fc-unthemed .fc-divider,
  .fc-unthemed .fc-row,
  .fc-unthemed .fc-content,
  .fc-unthemed .fc-popover,
  .fc-unthemed .fc-list-view,
  .fc-unthemed .fc-list-heading td {
    border-color: #fff !important;
  }
  .fc-unthemed td.fc-today {
    background-color: #fff;
    color: #1890ff;
  }
  fc-day-top fc-wed fc-future th {
    height: 3vw;
    line-height: 3vw;
    background-color: #fff !important;
    border-color: #fff !important;
  }
  thead td {
    height: 3vw;
    line-height: 3vw;
    text-align: center;
    cursor: pointer !important;
    // border: 0 !important;
  }
  tbody td {
    //  display: none;
    height: 0vw;
  }

  // fc-scroller fc-day-grid-container
  .fc-scroller {
    height: 15.2vw !important;
  }
  // tbody tr{
  //     height: 0vw !important;
  //   line-height: 3vw;
  //   text-align: center;
  // }
  .fc-ltr .fc-basic-view .fc-day-top .fc-day-number {
    float: none;
    margin: 0 auto;
  }
  // .fc-bg{
  //       height: 0vw !important;
  //   line-height: 3vw;
  //   text-align: center;
  // }
  .fc-basic-view .fc-body .fc-row {
    min-height: 3vw;
    max-height: 3vw;
    // display: none;
    //      height: 0vw !important;
    // line-height: 3vw;
    // text-align: center;
  }
  .rightboxmid {
    margin-bottom: 1vw;
    height: 22vw;
    width: 100%;
    background-color: #fff;
    border: 1px solid #dbe0e6;
  }
  .rightboxbottom {
    // margin-top: 1vw;
    height: 10.5vw;
    width: 100%;
    background-color: #fff;
    border: 1px solid #dbe0e6;
  }
  .rightboxbox {
    text-align: left;
    height: 3vw;
    font-size: 1vw;
    padding-left: 1vw;
    line-height: 3vw;
    border-bottom: 1px solid #dbe0e6;
    width: 100%;
    background-color: #f3f4f8;
    font-weight: bold;
  }

  .rightboxtop {
    user-select: none;
    cursor: pointer;
    height: 4vw;
    width: 100%;
    background-color: #1890ff;
    margin-bottom: 0.5vw;
    padding-top: 0.5vw;
  }
  .rightboxtop_box {
    height: 3vw;
    background-color: #1890ff;
    margin: 0 auto;
    width: 60%;
    box-sizing: border-box;
  }
  .rightboxtop_box_left {
    padding: 0.5vw;
    padding-left: 2vw;
    float: left;
    height: 3vw;
    width: 40%;
    text-align: center;
  }
  .rightboxtop_box_left_icon {
    width: 2vw;
    height: 2vw;
  }
  .rightboxtop_box_right {
    float: left;
    height: 3vw;
    width: 60%;
    text-align: left;
    line-height: 3vw;
    font-size: 1vw;
    font-weight: 300;
    color: #fff;
  }

  .rightboxtop_box1 {
    color: #000;
    height: 3vw;
    background-color: #fff;
    margin: 0 auto;
    margin-top: 0.5vw;
    width: 90%;
    box-sizing: border-box;
  }
  .rightboxtop_box_left1 {
    padding: 0.5vw;
    padding-left: 1vw;
    float: left;
    height: 3vw;
    width: 20%;
    text-align: center;
  }
  .rightboxtop_box_left_icon1 {
    width: 2vw;
    height: 2vw;
  }
  .rightboxtop_box_right1 {
    float: left;
    height: 3vw;
    width: 80%;
    text-align: left;
    line-height: 3vw;
    font-size: 1vw;
    font-weight: 300;
    color: #000;
  }

  .rightboxtop_box2 {
    user-select: none;
    cursor: pointer;
    border-radius: 0.1vw;
    height: 2vw;
    background-color: #1890ff;
    margin: 0.2vw auto;
    width: 25%;
    box-sizing: border-box;
  }
  .rightboxtop_box_left2 {
    padding: 0.3vw;
    padding-left: 0.6vw;
    float: left;
    height: 2vw;
    width: 45%;
    text-align: center;
  }
  .rightboxtop_box_left_icon2 {
    width: 1.5vw;
    height: 1.5vw;
  }
  .rightboxtop_box_right2 {
    float: left;
    height: 2vw;
    width: 55%;
    text-align: left;
    line-height: 2.1vw;
    font-size: 1vw;
    font-weight: 300;
    color: #fff;
  }

  .nodataimg {
    height: 8vw;
    width: 8vw;
    margin: 0 auto;
    //  margin-top: 6vw;
    // background-color: #f3f4f8;
    box-sizing: border-box;
  }
  .nodataimg_in {
    margin-left: 0.5vw;
    height: 6vw;
    width: 6vw;
    margin-top: 3vw;
  }
  .nodataword_in {
    width: 100%;
    height: 2vw;
    text-align: center;
    line-height: 2vw;
    font-size: 1vw;
    color: gainsboro;
  }
  .biggestbox {
    // min-height: 42vw;
    // 84px;
    height: calc(100vh - 86px);
    box-sizing: border-box;

    //  calc(100vh - 84px);
    // background-color: rgb(233, 87, 87);
    // width: 100%;

    // display: flex;
    //         align-items: center; /*定义body的元素垂直居中*/
    //         justify-content: center; /*定义body的里的元素水平居中*/
    //         text-align:center
  }
  .bigboxleft {
    // margin-top: calc(5.2vw - 84px);
    margin-top: 1.1vw;
    height: 41.1vw;
    width: 70%;
    background-color: #fff;
    float: left;
    box-sizing: border-box;
  }
  .bigboxright {
    margin-top: 1.1vw;
    //  margin-top: calc(5.2vw - 84px);
    height: 41.1vw;
    width: 30%;
    background-color: #fff;
    float: left;
    box-sizing: border-box;
  }

  .leftbox {
    border: 1px solid #dcdfe6;
    height: 38vw;
    width: 96%;
    margin-top: 1vw;
    margin-left: 4%;
    background-color: #fff;
  }
  .rightbox {
    height: 38vw;
    width: 90%;
    margin-top: 1vw;
    margin-left: 5%;
    background-color: #fff;
  }
  .leftboxtop {
    text-align: left;
    border-bottom: 1px solid #dcdfe6;
    height: 3vw;
    width: 100%;
    background-color: #f3f4f8;
    font-size: 1vw;
    font-weight: bold;
    color: #000;
    padding-left: 1vw;
    line-height: 3vw;
  }
  .leftboxmid {
    height: 12vw;
    width: 100%;
    background-color: #fff;
  }
  .leftboxmidmid {
    margin: 0 auto;
    height: 12vw;
    width: 90%;
    // background-color: cadetblue;
    border-bottom: 2px dashed #cbcbcb;
  }
  .leftboxmidmidbox1 {
    float: left;
    margin-right: 2%;
    margin-top: 1.5vw;
    height: 9vw;
    width: 32%;
    background-color: #fff;
    border: 1px solid #dbe0e6;
    // border-bottom: 2px dashed #cbcbcb;
  }
  .leftboxmidmidbox {
    float: left;
    margin-right: 2%;
    margin-top: 1.5vw;
    height: 9vw;
    width: 32%;
    background-color: #fff;
    border: 1px solid #dbe0e6;
    // border-bottom: 2px dashed #cbcbcb;
  }
  .leftboxbottom {
    height: 21vw;
    width: 100%;
    background-color: #fff;
    box-sizing: border-box;
  }
  .leftboxbottomtop {
    margin-top: 0.5vw;
    height: 3vw;
    width: 100%;
    background-color: #fff;
    line-height: 3vw;
    text-align: center;
    font-size: 1.2vw;
    font-weight: bold;
    color: #000;
  }
  .leftboxbottombottombox {
    height: 3vw;
    width: 100%;
    // background-color: blanchedalmond;
    box-sizing: border-box;
    margin: 0 auto;
    margin-bottom: 0.8vw;
  }
  .leftboxbottombottom {
    height: 18vw;
    width: 90%;
    background-color: #fff;

    margin: 0 auto;
    margin-bottom: 3px;
    box-sizing: border-box;
  }

  .leftword {
    height: 3vw;
    width: 14%;
    // background-color: #f3f4f8;
    text-align: left;
    // padding-left: 2%;
    font-size: 1vw;
    line-height: 3vw;
    box-sizing: border-box;
    float: left;
  }

  .leftimg {
    padding-left: 1vw;
    min-width: 4vw;
    max-width: 10%;
    height: 3vw;
    //  background-color: cadetblue;
    // background-image: url("");
    float: left;
  }
  // background: url("./timg.jpg") no-repeat;
  .leftimg_in {
    background-size: 100% 100%;
    width: 3vw;
    height: 3vw;
    // background-color: yellowgreen;
    border-radius: 3vw;
    float: left;
  }
  // .leftimg_in2 {

  //     background-size: 100% 100%;
  //   width: 3vw;
  //   height: 3vw;
  //   // background-color: yellowgreen;
  //   border-radius: 3vw;
  //   float: left;
  // }

  .el-progress {
    line-height: 3vw;
  }
  .el-progress--line {
    line-height: 3vw;
  }
  .leftprogram {
    cursor: pointer;
    line-height: 3vw;
    // padding-top: 1vw;
    width: 60%;
    height: 3vw;
    //  background-color: aqua;
    float: right;
  }
  .rightword {
    width: 10%;
    height: 3vw;
    //  background-color: rgb(67, 100, 100);
    float: right;
    text-align: right;
    padding-left: 2%;
    font-size: 1vw;
    line-height: 3vw;
    box-sizing: border-box;
  }

  .el-calendar-day {
    height: 2vw;
    width: 2vw;
    border-radius: 2vw;
    margin: 0 auto;
    line-height: 2vw;
    text-align: center;
    font-size: 0.8vw;
    padding: 0;
  }
  .is-selected {
    background-color: #fff;
  }
  .is-selected .el-calendar-day {
    height: 1.8vw;
    width: 1.8vw;
    border-radius: 1.8vw;
    background-color: #1890ff;
    color: #fff;
  }
  .el-calendar-table tr td {
    border: 0px;
    text-align: center;
  }
  .fc-toolbar {
    display: none !important;
  }
  .el-calendar__header {
    display: none;
  }

  .el-date-editor.el-input,
  .el-date-editor.el-input__inner {
    font-size: 0.8vw;
    float: left;
    // margin-left: 4vw;
    // box-sizing: border-box;
  }
  .el-input--small .el-input__inner {
    height: 2.5vw;
  }
  .el-input--small .el-input__icon {
    line-height: 2.5vw;
  }
  .el-calendar {
    background-color: rgba(22, 22, 22, 0);
  }
  // margin: 0 auto;
  // height: 11vw;
  // width: 90%;
  // // background-color: cadetblue;
  // border-bottom: 2px dashed #cbcbcb;
  .leftboxmidmidtop {
    color: #666666;
    width: 100%;
    height: 5vw;
    line-height: 5vw;
    font-size: 1vw;
    text-align: center;
  }
  .leftboxmidmidbottom {
    width: 100%;
    height: 5vw;
    line-height: 1vw;
    font-size: 3.5vw;
    text-align: center;
    font-weight: 500;
  }

  .leftboxmidmidtop2 {
    color: #666666;
    width: 100%;
    height: 4vw;
    line-height: 5vw;
    font-size: 1vw;
    box-sizing: border-box;
    padding-left: 3vw;
    text-align: left;
  }
  .leftboxmidmidbottom2 {
    color: #666666;
    width: 100%;
    height: 1.5vw;
    line-height: 1.5vw;
    font-size: 1vw;
    box-sizing: border-box;
    padding-left: 3vw;
    text-align: left;
  }
  //  <div class="leftboxmidmidtop">用户总数</div>
  //                    <div class="leftboxmidmidbottom">18</div>
}
</style>

<template>
  <div id="dashboard" class="dashboard-container">
    <!-- <component :is="currentRole" /> -->

    <div class="biggestbox">
      <div class="bigboxleft">
        <div class="leftbox">
          <div class="leftboxtop">系统使用时长和登录排名</div>
          <div class="leftboxmid">
            <!-- -->
            <div class="leftboxmidmid">
              <div v-for="(item,index) in midbox" :key="index">
                <div v-if="index==0" class="leftboxmidmidbox1">
                  <div class="leftboxmidmidtop">用户总数</div>
                  <div class="leftboxmidmidbottom">{{form.totalUser}}</div>
                </div>
                <div v-if="index==1" class="leftboxmidmidbox">
                  <div class="leftboxmidmidtop2">上周使用情况</div>

                  <div class="leftboxmidmidbottom2">
                    时长：
                    <span style="color:#000;font-weight:bold">{{form.curentStaLW.timeStr}}</span>
                  </div>
                  <div class="leftboxmidmidbottom2">
                    排名：
                    <span style="color:#ea2804;font-weight:bold">{{form.curentStaLW.place}}</span>
                  </div>
                </div>
                <div v-if="index==2" class="leftboxmidmidbox" style="margin-right:0;">
                  <div class="leftboxmidmidtop2">本周使用情况</div>
                  <div class="leftboxmidmidbottom2">
                    时长：
                    <span style="color:#000;font-weight:bold">{{form.curentStaTW.timeStr}}</span>
                  </div>
                  <div class="leftboxmidmidbottom2">
                    排名：
                    <span style="color:#ea2804;font-weight:bold">{{form.curentStaTW.place}}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="leftboxbottom">
            <div class="leftboxbottomtop">本周登录时长TOP5</div>

            <div class="leftboxbottombottom">
              <!-- <img src="@/assets/404_images/404_cloud.png" alt="" v-show="true"> -->
              <div class="nodataimg" v-show="form.topPlace.length==0||!form.topPlace.length">
                <img class="nodataimg_in" src="@/assets/firstpage/nodata.png" alt />
                <div class="nodataword_in">还没有数据哦！</div>
              </div>

              <div
                v-for="(item,index) in form.topPlace"
                :key="index"
                v-show="form.topPlace.length>0"
              >
                <div class="leftboxbottombottombox">
                  <div class="leftword">
                    <div v-if="item.accountName">
                      <el-popover
                        :open-delay="1"
                        :close-delay="1"
                        v-if="item.accountName.length>4"
                        class="item"
                        trigger="hover"
                        placement="top-start"
                        :content="item.accountName"
                      >
                        <div
                          class="ellipsis"
                          style="cursor: pointer;"
                          slot="reference"
                        >{{item.place}} &nbsp; &nbsp; &nbsp; {{item.accountName}}</div>
                      </el-popover>
                      <div
                        class="ellipsis"
                        slot="reference"
                        v-if="item.accountName.length<=4"
                      >{{item.place}} &nbsp; &nbsp; &nbsp; {{item.accountName}}</div>
                    </div>

                    <div v-if="!item.accountName">
                      <div
                        class="ellipsis"
                        slot="reference"
                      >{{item.place}} &nbsp; &nbsp; &nbsp; {{item.accountName}}</div>
                    </div>
                  </div>
                  <!-- leftimg_in2 -->
                  <div class="leftimg">
                    <!-- <img class="leftimg_in" v-bind:src="bigimg"  alt="" /> -->
                    <!-- <img
                      :src="item.headPicPath? imgUrlName+item.headPicPath:'./Sidebar/images/avatar.png'"
                      alt
                      class="leftimg_in"
                    /> -->
                    <img v-if="item.headPicPath" :src="imgUrlName+item.headPicPath" class="leftimg_in" />
                    <img v-else src="../../layout/components/Sidebar/images/avatar.png" class="leftimg_in" />
                  </div>
                  <div class="rightword">{{item.times}}次</div>
                  <div class="leftprogram">
                    <el-popover
                      placement="top-end"
                      :open-delay="1"
                      :close-delay="1"
                      title="在线时长："
                      width="200"
                      trigger="hover"
                      :content="item.timeStr"
                    >
                      <el-progress
                        slot="reference"
                        v-if="index==0"
                        :stroke-width="10"
                        :percentage="(item.timeSpend/sum)*100"
                        color="#1890ff"
                        :format="format"
                      ></el-progress>
                      <el-progress
                        slot="reference"
                        v-if="index==1"
                        :stroke-width="10"
                        :percentage="(item.timeSpend/sum)*100"
                        color="#00cc99"
                        :format="format"
                      ></el-progress>
                      <el-progress
                        slot="reference"
                        v-if="index==2"
                        :stroke-width="10"
                        :percentage="(item.timeSpend/sum)*100"
                        color="#6599ff"
                        :format="format"
                      ></el-progress>
                      <el-progress
                        slot="reference"
                        v-if="index==3"
                        :stroke-width="10"
                        :percentage="(item.timeSpend/sum)*100"
                        color="#9966cd"
                        :format="format"
                      ></el-progress>
                      <el-progress
                        slot="reference"
                        v-if="index==4"
                        :stroke-width="10"
                        :percentage="(item.timeSpend/sum)*100"
                        color="#cc33cc"
                        :format="format"
                      ></el-progress>
                    </el-popover>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="bigboxright">
        <div class="rightbox">
          <div class="rightboxtop">
            <div class="rightboxtop_box">
              <div class="rightboxtop_box_left">
                <img class="rightboxtop_box_left_icon" src="@/assets/firstpage/icon1.png" alt />
              </div>
              <div class="rightboxtop_box_right">报表中心</div>
            </div>
          </div>
          <div class="rightboxmid">
            <div
              style="padding-top:0.5vw;padding-bottom:0.5vw;width:100%;background-color:#f3f4f8;box-sizing: border-box;height:3.5vw;"
            >
              <el-date-picker
                style="width:10vw;margin-left:1vw;"
                v-model="selectDate"
                size="small"
                type="month"
                placeholder="选择月"
                value-format="yyyy-MM"
                @change="changeDate"
              />
            </div>
            <el-calendar v-model="selectDate" @click="clickcalendar"></el-calendar>
          </div>
          <div class="rightboxbottom">
            <div class="rightboxbox">使用手册</div>
            <div class="rightboxtop_box1">
              <div class="rightboxtop_box_left1">
                <img class="rightboxtop_box_left_icon1" src="@/assets/firstpage/icon2.png" alt />
              </div>
              <div class="rightboxtop_box_right1">包管云系统使用操作说明书.doc</div>
            </div>

            <div class="rightboxtop_box2" @click="download">
              <div class="rightboxtop_box_left2">
                <img class="rightboxtop_box_left_icon2" src="@/assets/firstpage/icon3.png" alt />
              </div>
              <div class="rightboxtop_box_right2">下载</div>
            </div>
          </div>
        </div>
      </div>
      <!-- <iframe
        v-if="cacheUrl!=''"
        :src="'/#'+cacheUrl"
        frameborder="0"
        style="position:absolute;top:0;left:0px;width:0px;height:0px;opacity: 0;"
      ></iframe> -->
    </div>
  </div>
</template>




<script>
import { mapGetters } from "vuex";
import adminDashboard from "./admin";
import editorDashboard from "./editor";
import request from "@/utils/request";
import img from "@/assets/firstpage/nodata.png";
import img2 from "@/images/timg.jpg";
import { getPath } from "@/utils/BtnControl";
// timg.jpg
export default {
  name: "Dashboard",
  components: { adminDashboard, editorDashboard },
  data() {
    return {
      cacheUrl: "",
      bigimg: img2,
      sum: 100,
      form: {
        curentStaLW: {
          accountName: "",
          place: "",
          timeSpend: "",
          timeStr: "",
          times: ""
        },
        curentStaTW: {
          accountName: "",
          place: "",
          timeSpend: "",
          timeStr: "",
          times: ""
        },
        topPlace: [
          {
            accountName: "",
            place: "",
            timeSpend: "",
            timeStr: "",
            times: ""
          }
        ],
        totalUser: null
      },
      selectDate: new Date(), // 日期选择器选中的月份
      midbox: ["", "", ""],
      midbox5: ["", "", "", "", ""],
      currentRole: "adminDashboard",
      name: "小明明明明"
      //  value: "2019-08"
    };
  },
  methods: {
    download() {
      // window.open("你的url")
      var that = this;
      request({
        url: "/report/api/StaOnlineTime/GetFile",
        method: "get",
        params: {}
      }).then(res => {
        // '/req': {
        //         target: 'http://47.107.135.203:20200',
        //         ws: true,  // proxy websockets
        //         changeOrigin: true,  //允许跨域
        //         pathRewrite: {
        //             '^/req': ''  // rewrite path
        //         }
        //       }

        // console.log(res)
        window.open("http://47.107.135.203:20200/" + res.data.docPath);

        // function getBlob(url) {
        //   return new Promise(resolve => {
        //     const xhr = new XMLHttpRequest();

        //     xhr.open("GET", url, true);
        //     xhr.responseType = "blob";
        //     xhr.onload = () => {
        //       if (xhr.status === 200) {
        //         resolve(xhr.response);
        //       }
        //     };

        //     xhr.send();
        //   });
        // }

        // function saveAs(blob, filename) {
        //   if (window.navigator.msSaveOrOpenBlob) {
        //     navigator.msSaveBlob(blob, filename);
        //   } else {
        //     const link = document.createElement("a");
        //     const body = document.querySelector("body");

        //     link.href = window.URL.createObjectURL(blob);
        //     link.download = filename;

        //     // fix Firefox
        //     link.style.display = "none";
        //     body.appendChild(link);

        //     link.click();
        //     body.removeChild(link);

        //     window.URL.revokeObjectURL(link.href);
        //   }
        // }

        // function download(url, filename) {
        //   getBlob(url).then(blob => {
        //     saveAs(blob, filename);
        //   });
        // }

        // console.log(process.env.NODE_ENV, "环境");

        // // production

        // console.log(res.data);
        // if (process.env.NODE_ENV == "development") {
        //   download("/req" + res.data.docPath, res.data.docName);
        // } else {
        //   download(
        //     "http://47.107.135.203:20200/" + res.data.docPath,
        //     res.data.docName
        //   );
        // }

        //
      });
    },
    changeDate() {
      //
    },

    clickcalendar(e) {},
    format(percentage) {
      return percentage <= 100 ? "" : `${percentage}%`;
    }
  },
  computed: {
    ...mapGetters(["roles"])
  },
  filters: {
    numFilter(num) {
      if (num) {
        var hours = num / 3600;
        var mins = (num % 3600) / 60;
        return hours.toFixed(0) + "小时" + mins.toFixed(0) + "分钟";
      } else {
        return "0小时";
      }
    }
  },
  created() {
    // if (
    //   this.$store.getters.userPermission[0] &&
    //   this.$store.getters.userPermission[0]["children"] &&
    //   this.$store.getters.userPermission[0]["children"][0]
    // ) {
    //   this.cacheUrl = this.$store.getters.userPermission[0]["children"][0][
    //     "path"
    //   ];
    // }
  },
  mounted() {
    request({
      url: "/report/api/StaOnlineTime/GetStaOnline",
      method: "get",
      params: {}
    }).then(res => {
      if (res.code == 0) {
        this.form = res.data;
        var precent = null;
        var sum = 0;
        res.data.topPlace.map(item => {
          if (item.timeSpend) {
            sum = sum + item.timeSpend;
          }
        });
        this.sum = sum;
      }
    });

    // if (!this.roles.includes("admin")) {
    //   this.currentRole = "adminDashboard";
    // }
  }
};
</script>
