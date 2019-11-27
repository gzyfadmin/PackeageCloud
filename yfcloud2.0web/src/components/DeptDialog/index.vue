<template>
  <div>
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="600px"
      :close-on-click-modal="false"
      :center="true"
    >
      <el-form ref="formData" label-width="100px">
        <el-form-item
          label="部门名称："
        >
          <el-input v-model="deptData.deptName" placeholder="请输入部门名称" />
        </el-form-item>
        <el-form-item label="部门描述：">
          <el-input v-model="deptData.deptDesc" placeholder="请输入部门描述" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="handelCloseClick">关闭</el-button>
        <el-button type="primary" @click="handelSave">保存</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import request from '@/utils/request'
import RequestObject from '@/utils/requestObject'
export default {
  props: {
    dept: {
      type: Object
    }
  },
  data() {
    // const name = this.deptData.name
    return {
      dialogVisible: false,
      title: '编辑部门信息',
      deptData: {
        actualId: this.dept.actualId,
        companyId: this.dept.companyId,
        createId: this.dept.createId,
        createTime: this.dept.createTime,
        deptDesc: this.dept.deptDesc,
        deptName: this.dept.deptName,
        parentId: this.dept.parentId,
        pathLogic: this.dept.pathLogic,
        seqNumber: this.dept.seqNumber,
        vitualId: this.dept.vitualId
      }
    }
  },
  created() {},
  //   mounted() {
  //       this.$nextTick
  //   },
  methods: {
    /**
     * handelCloseClick
     * 关闭
     */
    handelCloseClick() {
      this.dialogVisible = false
    },
    handelOpenClick() {
      this.dialogVisible = true
    },
    /**
     * handelSave
     * 保存
     */
    handelSave() {
      console.log(this.deptData.deptName)
      this.dialogVisible = false
      var postData = this.deptData
      // if (postData.deptName == '' || postData.deptName.split('').length > 30) {
      //   this.$message({
      //     showClose: true,
      //     message: '部门名称不为空及小于30个字符',
      //     type: 'error'
      //   })
      //   return
      // }
      var reqObject = RequestObject.GetObject(postData, null, null)
      console.log('reqObject', reqObject)
      request({
        url: '/system/api/TSMDept',
        method: 'post',
        data: reqObject
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.$message({
            message: '保存部门成功!',
            type: 'success'
          })
        }
      })
    }
  }
}
</script>
