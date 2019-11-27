<template>
  <div>
    <el-form
      ref="formData"
      :model="formData"
      label-width="130px"
      size="small"
      style="width:100%"
    >
      <el-form-item label="所属行业：">
        <el-select v-model="value1" placeholder="请选择">
          <el-option
            v-for="item in options1"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          ></el-option>
        </el-select>

        <el-select v-model="value2" collapse-tags style="margin-left: 20px;" placeholder="请选择">
          <el-option
            v-for="item in options1"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          ></el-option>
        </el-select>
      </el-form-item>

      <el-form-item label="所属区域：">
        <el-select v-model="value3" placeholder="请选择">
          <el-option
            v-for="item in options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          ></el-option>
        </el-select>

        <el-select v-model="value4" collapse-tags style="margin-left: 20px;" placeholder="请选择">
          <el-option
            v-for="item in options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          ></el-option>
        </el-select>
      </el-form-item>

      <el-form-item label="资质证明：">
        <div>
          <p style="margin:0px">1. 公司、事业单位、个人工商户可直接上传营业执照副本、组织机构代码、税务登记证等任意一种</p>
          <p style="margin:0px">2. 上传资质证明为扫描件或电子照片，请确保信息清晰可辨</p>
          <p style="margin:0px">3. 支持jpg、jpeg、png、gif、bmp格式，且照片小于5M</p>
          <p style="margin:0px">4. 我们仅用于企业资质审核，不用于其它商业目的</p>
        </div>
        <el-upload
          class="upload-demo"
          :action="$ajaxUrl+'/fileupload/api/files/upload'"
          :file-list="fileList"
          :on-change="handleChange"
          :on-success="handleAvatarSuccess"
        >
          <el-button size="small" type="primary" :disabled="setDable">上传图片</el-button>
        </el-upload>
        <!-- <div style="width:100px;height:100px;">
          <img :src="imgSrc+imgsrc" alt style="width:100%" />
        </div>
        <uploadExcel ref="lllll" style="z-index:0" @upLoadClose="sonZhi" :cut="cut" :types="types"/>
        <el-button @click="clickImg" type="primary" size="mini">上传图片</el-button>-->
        <div
          style="width:98%;height:424px; border:1px solid rgb(229,229,229); margin:20px 0;background-color:rgb(248,248,248)"
        >
          <img :src="imgSrc+formData.businessLogo" v-if="formData.businessLogo!=null" :model="log" alt style="width:100%;"/>
          

          <!-- <img :src="imgSrc+imgsrc" alt width="100%" /> -->
        </div>
      </el-form-item>
    </el-form>
  </div>
</template>
<script>
import uploadExcel from "../../../components/UploadPic/index";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
export default {
  components: {
    uploadExcel
  },
  name: "eCantion",
  props: {
    formData: {
      required: true,
      type: Object
    }
  },
  created() {
    if (this.formData == null) {
      this.formdata = this.formData;
    } else {
      this.log = this.formData.businessLogo;
      if (this.formData.isAdmin) {
        this.setDable = false;
      } else {
        this.setDable = true;
      }
      this.$watch(
        "log",
        function(newVal, oldVal) {
          var newVals = newVal;
          var oldvals = oldVal;
          var objs = {
            newVals: newVal,
            oldvals: oldVal
          };
          this.$emit("eCantion", objs);
        },
        { deep: true }
      );
    }
  },
  data() {
    return {
      value: "",
      one1: "",
      one2: "",
      one3: "",
      one4: "",
      setDable: false,
      log: "",

      options: [
        {
          value: "北京",
          label: "北京"
        },
        {
          value: "上海",
          label: "上海"
        },
        {
          value: "深圳",
          label: "深圳"
        },
        {
          value: "广州",
          label: "广州"
        },
        {
          value: "芝加哥",
          label: "芝加哥"
        }
      ],
      options1: [
        {
          value: "机械制造",
          label: "机械制造"
        },
        {
          value: "渔业",
          label: "渔业"
        },
        {
          value: "畜牧业",
          label: "畜牧业"
        },
        {
          value: "电商",
          label: "电商"
        },
        {
          value: "布料行业",
          label: "布料行业"
        }
      ],
      value1: [],
      value2: [],
      value3: [],
      value4: [],

      cut: {
        width: 225,
        height: 319
      },
      imgsetURL: "",
      stys:
        "width:255px;height:320px;border:9px solid rgb(217,217,217);margin:42px 23px 21px 23px;",
      types: 0,
      industry: "",
      area: "",
      fileList: [],
      imgurl:
        this.imgUrlName + "20190802/60f712a1c26340b5b6fc8ba261f35dcf.png",
      imgSrc: this.imgUrlName,
      imgsrc: ""
      
    };
  },
  methods: {
    sonXhi(data) {
      this.log = data;
      this.imgR = this.imgsrc + data;
      var imge = this.imgsrc + data;
      this.formData.tenantLogo = data;

      // this.imgsrc1 = data;
      // console.log(imge);
      // this.$store.dispatch('app/toggleSideBar')
      // this.$store.dispatch("user/set_avatar", imge);
      this.$store.dispatch("logoName/setLogImg", imge);
    },

    handleAvatarSuccess(res, file, fileList) {
      this.formData.businessLogo = res;
      this.imgurl = this.imgSrc + res;
      this.log = res;
      this.fileList = [];
      (this.stys = "width:255px;height:320px;margin:42px 23px 21px 23px;"),
        (this.imgsrc = res);
    },
    handleChange(file, fileList) {},
    clickImg() {
      this.$refs.lllll.doOpen();
    },
    clickDong() {
      var comindata = {
        businessLogo: this.imgsrc
      };

      var postdata = RequestObject.CreatePostObject(comindata);
      request({
        url: "/system/api/TSMCompany/ModifyBusinessLogo",
        method: "PUT",
        data: postdata
      }).then(res => {});
    }
  }
};
</script>
<style scoped>
</style>


