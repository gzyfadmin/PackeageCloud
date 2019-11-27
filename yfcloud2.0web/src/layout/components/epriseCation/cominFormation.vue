<template>
  <div>
    <el-form
      ref="formData"
      :model="formData"
      label-width="130px"
      size="small"
      :rules="formRules"
      style="width:100%"
    >
      <el-form-item v-if="false" label="租户编号：" prop="id">
        <el-input v-model="formData.id" placeholder="租户生成" readonly style="width:100%" />
      </el-form-item>
      <el-form-item label="公司中文名称：" prop="companyName">
        <el-input
          v-model="formData.companyName"
          placeholder="请输入公司名称"
          style="width:100%"
          :disabled="setDable"
        />
      </el-form-item>
      <el-form-item label="公司英文名称：" prop="tenantEngName">
        <el-input
          v-model="formData.tenantEngName"
          placeholder="请输入公司英文名称"
          style="width:100%"
          :disabled="setDable"
        />
      </el-form-item>
      <el-form-item label="公司简称：" prop="tenantShortName">
        <el-input
          v-model="formData.tenantShortName"
          placeholder="请输入公司简称"
          style="width:100%"
          :disabled="setDable"
        />
      </el-form-item>
      <el-form-item label="公司地址：" prop="address" style="width:100%">
        <el-input v-model="formData.address" placeholder="详细地址" :disabled="setDable" />
      </el-form-item>
      <el-form-item prop="tenantLogo" label="公司LOGO：">
        <uploadExcel ref="KKKK" @upLoadClose="sonZhi" />
        <el-button type="primary" size="mini" @click="clickImgs" :disabled="setDable">上传图片</el-button>
        <div style="font-size:10px;margin-top:0px;width: 370px;">支持JPG,JPEG,PNG,BMP格式</div>
        <div style="font-size:10px;margin-top:0px;width: 370px;">图片小于10M|最大尺寸100x50像素,超过此尺寸会被压缩并降低质量</div>
        <div
          style="width:100px;height:100px;border:1px solid rbg(250,250,250);border-radius:50%;float:right;margin-right:26px;margin-top:-95px;"
        >
          <img v-if="formData.tenantLogo!=null" :src="imgsrc+formData.tenantLogo" style="width:100%;height:100%;border-radius:50%;overflow:hidden;"/>
          <img v-else src="../Sidebar/images/yunfeiLg.png" style="width:100%;height:100%;border-radius:50%;overflow:hidden;"/>
        </div>
      </el-form-item>
      <el-form-item label="公司类型:" prop="enterpriseType">
        <el-select v-model="formData.enterpriseType" placeholder="请选择公司类型">
          <el-option
            v-for="item in options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          ></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="法人姓名：" prop="legalPerson">
        <el-input
          v-model="formData.legalPerson"
          placeholder="法人名称"
          :disabled="setDable"
          style="width:100%"
        />
      </el-form-item>
      <el-form-item label="法人电话：" prop="contactNumber">
        <el-input
          v-model="formData.contactNumber"
          placeholder="固定电话"
          :disabled="setDable"
          style="width:100%"
        />
      </el-form-item>
      <!-- <div
        style="width:400px;height:100px;background:rgb(255,251,246);padding-top:15px;float:right;margin-top:-130px;margin-right:30px"
      >
        <div style="float:left;margin-top:2px;margin-left:20px;">
          <img src='../../../assets/index_images/warning_img_03.png' alt />
        </div>
        <span style="font-size:18px;font-weight:800;color:rgb(255,136,0);padding-left:10px">认证状态:未认证</span>
        <div style="margin:10px 0 0 0">
          <span style="padding-left:10px">请及时对公司进行认证,以便享受vip的体验</span>
        </div>
        <span
          style="color:rgb(34,153,238);padding-left:10px;cursor:pointer"
          @click="setApprove"
        >马上去认证</span>
      </div>-->
      <!-- <el-collapse>
              <el-collapse-item title=" 可选填" name="1" style="width:96%">
                <el-form-item v-if="false" label="模板权限:" prop="templateId">
                  <el-switch
                    v-model="formData.templateId"
                    active-text="是"
                    inactive-text="否"
                    :disabled="setNewdisable"
                  />
                </el-form-item>
                <el-form-item v-if="false" label="租户状态：">
                  <el-switch
                    v-model="formData.status"
                    active-text="是"
                    inactive-text="否"
                    :disabled="setNewdisable"
                  />
                </el-form-item>
                <el-form-item v-if="false" label="租户有效期:" prop="validityPeriod">
                  <el-date-picker
                    v-model="formData.validityPeriod"
                    type="date"
                    placeholder="选择日期"
                    :disabled="setNewdisable"
                  />
                </el-form-item>
                <el-form-item label="所属区域" prop="area">
                  <el-select v-model="formData.area" placeholder="请选择所属区域" :disabled="setDisable">
                    <el-option
                      v-for="item in area"
                      :key="item.id"
                      :label="item.value"
                      :value="item.id"
                    />
                  </el-select>
                </el-form-item>
                <el-form-item label="所属行业" prop="industry">
                  <el-select
                    v-model="formData.industry"
                    placeholder="请选择所属行业"
                    :disabled="setDisable"
                  >
                    <el-option
                      v-for="item in industry"
                      :key="item.id"
                      :label="item.value"
                      :value="item.id"
                    />
                  </el-select>
                </el-form-item>
                <el-form-item label="租户规模" prop="tenantScale">
                  <el-input
                    v-model="formData.tenantScale"
                    placeholder="请输入注册资金"
                    style="width:40%"
                    :disabled="setDisable"
                  />
                </el-form-item>
                <el-form-item v-if="false" label="是否试用：">
                  <el-switch
                    v-model="formData.isTrial"
                    active-text="是"
                    inactive-text="否"
                    :disabled="setNewdisable"
                  />
                </el-form-item>
                <el-form-item v-if="false" label="试用有效期:" prop="trialDate">
                  <el-date-picker
                    v-model="formData.trialDate"
                    type="date"
                    placeholder="选择日期"
                    :disabled="setNewdisable"
                  />
                </el-form-item>
                <el-form-item label="注册资金：" prop="registeredCapital">
                  <el-input
                    v-model="formData.registeredCapital"
                    placeholder="请输入注册资金"
                    style="width:40%"
                    :disabled="setDisable"
                  />
                </el-form-item>
                <el-form-item label="注册资金" prop="registeredCapital" style="width:90%">
                  <el-input
                    v-model="formData.registeredCapital"
                    placeholder="注册资金"
                    :disabled="setDisable"
                  />
                </el-form-item>
                <el-form-item label="法人" prop="legalPerson" style="width:90%">
                  <el-input
                    v-model="formData.legalPerson"
                    placeholder="法人名称"
                    :disabled="setDisable"
                  />
                </el-form-item>
                <el-form-item label="主营业务" prop="mainBusiness" style="width:90%">
                  <el-input
                    v-model="formData.mainBusiness"
                    placeholder="主营业务"
                    :disabled="setDisable"
                  />
                </el-form-item>

                <el-form-item label="电子邮箱" prop="email" style="width:90%">
                  <el-input v-model="formData.email" placeholder="电子邮箱" :disabled="setDisable" />
                </el-form-item>

              </el-collapse-item>
      </el-collapse>-->
    </el-form>
  </div>
</template>
<style>
</style>
<script>
import uploadExcel from "../../../components/UploadPic/index";
import request from "@/utils/request";
import RequestObject from "@/utils/requestObject";
import { log } from "util";
export default {
  name: "cominFormation",
  components: {
    uploadExcel
  },
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
      if (this.formData.isAdmin) {
        this.setDable = false;
      } else {
        this.setDable = true;
      }
      this.log = this.formData.tenantLogo;
      this.$watch(
        "log",
        function(newVal, oldVal) {
          var newVals = newVal;
          var oldvals = oldVal;
          var objs = {
            newVals: newVal,
            oldvals: oldVal
          };
          this.$emit("oldval", objs);
        },
        { deep: true }
      );
    }
  },
  data() {
    var setCompanyName = (rulue, value, callback) => {
      var vName = /^[\u0391-\uFFE5A-Za-z]+$/;
      if (!value) {
        return callback("企业姓名不能为空");
      } else if (!vName.test(value)) {
        this.formData.companyName = "";
        return callback("请输入中文和字母");
      } else {
        return callback();
      }
    };
    var setTenantEngName = (rulue, value, callback) => {
      var vEngName = /^[A-Za-z]+$/;
      if (!vEngName.test(value)) {
        this.formData.tenantEngName = "";
        return callback("请输入字母");
      } else {
        return callback();
      }
    };
    return {
      setDable: false,
      options: [
        {
          value: "私营企业",
          label: "私营企业"
        },
        {
          value: "事业单位",
          label: "事业单位"
        },
        {
          value: "国营企业",
          label: "国营企业"
        },
        {
          value: "股份制企业",
          label: "股份制企业"
        },
        {
          value: "小本企业",
          label: "小本企业"
        }
      ],
      companyName: "",
      formdata: {
        companyName: "一切皆有可能"
      },
      setDisable: false,
      formRules: {
        companyName: [
          { validator: setCompanyName, trigger: "blur", required: true }
        ],
        tenantEngName: [{ validator: setTenantEngName, trigger: "blur" }],
        genre: [
          { required: true, message: "请选择公司类型", trigger: "change" }
        ]
      },
      genre: "",
      imgR:
        "http://47.107.135.203:20200/20190802/c9602b085d354f73a74582eee9f8adea.png",
      imgsrc: this.imgUrlName,
      imgData: "",
      log: ""
    };
  },
  methods: {
    OnBtnSaveSubmit() {
      this.$refs.formData.validate(valid => {
        if (valid) {
          alert("submit!");
        } else {
          // console.log("error submit!!");
          return false;
        }
      });

      this.$emit("obj", 1);
    },

    sonZhi(data) {
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
    clickImgs() {
      this.$refs.KKKK.doOpen();
    },
    setApprove() {},
    OnBtnClose() {}
  }
};
</script>


