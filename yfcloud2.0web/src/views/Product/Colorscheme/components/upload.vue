<template>
  <div id="IconsEdit" class="icons-container">
    <el-dialog
      title="选择图标"
      :visible.sync="dialogVisible"
      width="800px"
      :close-on-click-modal="false"
      :center="true"
      :modal="false"
    >
      <div style="width:100%;overflow:auto;" class="clr" v-if="dialogVisible">
        <div class="vueCropperBox">
          <vueCropper
            class="vueCropper"
            ref="cropper"
            :img="option.img"
            :outputSize="option.size"
            :outputType="option.outputType"
            :info="true"
            :full="option.full"
            :canMove="option.canMove"
            :canMoveBox="option.canMoveBox"
            :original="option.original"
            :autoCrop="option.autoCrop"
            :autoCropWidth="option.autoCropWidth"
            :autoCropHeight="option.autoCropHeight"
            :fixedBox="option.fixedBox"
            @realTime="realTime"
            @imgLoad="imgLoad"
          ></vueCropper>
        </div>
        <div class="showbox">
          <div class="show-preview">
            <div class="preview">
              <img :src="previews.url" :style="previews.img" />
            </div>
          </div>
     

          <div class="upBtn pr" style="margin-left: 25px;">
            <el-button type="success">选择图片</el-button>
            <span class="opBtn">
              <!-- <label class="btn btn-orange" for="uploads">选择图片</label> -->
              <input
                type="file"
                id="uploads"
                :value="imgFile"
                accept="image/png, image/jpeg, image/gif, image/jpg"
                @change="uploadImg($event, 1)"
              />
            </span>
          </div>
          <div class="upBtn pr">
            <el-button type="success">上传头像</el-button>
            <span class="opBtn">
              <input type="button" class="btn btn-blue" value="上传头像" @click="finish('blob')" />
            </span>
          </div>

          <!-- <img :src="upImg"/> -->
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { VueCropper } from "vue-cropper";
import axios from "axios";

export default {
  name: "Icons",
  props: {
    // cut: {
    //   required: true,
    //   type: Object
    // },
    // types: {
    //   required: true,
    //   type: Number
    // }
  },
  data() {
    return {
      dialogVisible: false,
      headImg: "",
      //剪切图片上传
      crap: false,
      previews: {},
      option: {
        img: "",
        outputSize: 1, //剪切后的图片质量（0.1-1）
        full: false, //输出原图比例截图 props名full
        outputType: "png",
        canMove: true,
        original: false,
        canMoveBox: true,
        autoCrop: true,
        autoCropWidth: 150,
        autoCropHeight: 150,
        fixedBox: true
      },
      fileName: "", //本机文件地址
      downImg: "#",
      imgFile: "",
      uploadImgRelaPath: "", //上传后的图片的地址（不带服务器域名）
      upImg: ""
    };
  },
  components: {
    VueCropper
  },
  methods: {
    // 实时预览函数
    realTime(data) {
      // console.log("realTime");
      this.previews = data;
    },
    imgLoad(msg) {
      // console.log("imgLoad");
      // console.log(msg);
    },
    //上传图片（点击上传按钮）
    finish(type) {
      // console.log("finish");
      let _this = this;
      let formData = new FormData();
      // 输出
      if (type === "blob") {
        this.$refs.cropper.getCropBlob(data => {
          let img = window.URL.createObjectURL(data);
          this.model = true;
          this.modelSrc = img;
          formData.append("file", data, this.fileName);
          let config = {
            headers: {
              "Content-Type": "multipart/form-data"
            }
          };
          axios
            .post(
              "http://47.107.135.203:8000/fileupload/api/files/upload",
              formData,
              config
            )
            .then(res => {
              this.upImg = `http://47.107.135.203:20200/${res.data}`;
              this.$emit("upLoadClose", res.data);
              this.doClose();
            })
            .catch(res => {
              // console.log(res);
            });
        });
      } else {
        // alert(2)
        // this.$refs.cropper.getCropData(data => {
        //   this.model = true;
        //   this.modelSrc = data;
        // });
        // this.$message({
        //     message: "请选择图片",
        //     type: "error"
        //   });
      }
    },
    //选择本地图片
    uploadImg(e, num) {
      // console.log("uploadImg");
      var _this = this;
      //上传图片
      var file = e.target.files[0];

      // var image = new Image();
      // image.src = file;
      // var height = image.height;
      // var width = image.width;

      const isLt2M = file.size / 1024 / 1024 < 10;
      if (!isLt2M) {
        this.$message({
          showClose: true,
          message: "图片不能大于10M",
          type: "error"
        });
        return false;
      }
      _this.fileName = file.name;
      // if (!/\.(gif|jpg|jpeg|png|bmp|GIF|JPG|PNG)$/.test(e.target.value))
      if (!/\.(jpg|jpeg|png|bmp|JPG|PNG)$/.test(e.target.value)) {
        alert("图片类型必须是.jpeg,jpg,png,bmp中的一种");
        return false;
      }
      var reader = new FileReader();
      reader.onload = e => {
        let data;
        if (typeof e.target.result === "object") {
          // 把Array Buffer转化为blob 如果是base64不需要
          data = window.URL.createObjectURL(new Blob([e.target.result]));
        } else {
          data = e.target.result;
        }
        if (num === 1) {
          _this.option.img = data;
        } else if (num === 2) {
          _this.example2.img = data;
        }
      };
      // 转化为base64
      // reader.readAsDataURL(file)
      // 转化为blob
      reader.readAsArrayBuffer(file);
    },
    doOpen() {
      
      this.dialogVisible = true;
      this.fileUpload = null;
      this.option = {
        img: "",
        outputSize: 1, //剪切后的图片质量（0.1-1）
        full: false, //输出原图比例截图 props名full
        outputType: "png",
        canMove: true,
        original: false,
        canMoveBox: true,
        autoCrop: true,
        autoCropWidth: 150,
        autoCropHeight: 150,
        fixedBox: true
      };
      if (this.option.image) {
        this.$refs["cropper"].imgs = "";
      }
      this.previews = {};
      this.upImg = "";
    },
    doClose() {
      this.dialogVisible = false;
    }
  }
};
</script>

<style lang="scss" scoped>
/deep/.v-modal {
  z-index: 0;
}
.pr {
  position: relative;
}
.clr::after {
  content: "";
  display: block;
  height: 0;
  clear: both;
  visibility: hidden;
}
.vueCropperBox {
  width: 500px;
  height: 500px;
  float: left;
  padding: 20px;
}
.vueCropper {
  width: 460px;
  height: 460px;
}
.showbox {
  padding: 20px;
  width: 260px;
  float: left;
}
.show-preview {
  width: 150px;
  height: 150px;
  margin: 0 auto 20px;
}
.show-previewe {
  width: 255px;
  height: 319px;
  margin: 0 auto 20px;
}
.previewe{
  overflow: hidden;
  border: 1px solid #cccccc;
  background: #cccccc;
  width: 255px;
  height: 319px;
}
.preview {
  overflow: hidden;
  border-radius: 50%;
  border: 1px solid #cccccc;
  background: #cccccc;
  width: 150px;
  height: 150px;
}
.upBtn {
  width: 80px;
  height: 32px;
  display: inline-block;
  overflow: hidden;
}
.opBtn {
  position: absolute;
  top: 0px;
  left: 0px;
  display: inline-block;
  width: 100%;
  height: 100%;
}
.opBtn input {
  width: 100%;
  height: 100%;
  opacity: 0;
}
</style>
