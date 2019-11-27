<template>
  <el-container id="menus" v-loading="loading" element-loading-spinner="el-icon-loading" element-loading-background="rgba(150, 150, 150, 0.6)">
    <el-header id="elheader" class="header" height="auto">
      <!-- <el-button type="primary" @click="handelAddClick(-1)">新增</el-button> -->
      <el-button v-if="btnAip.add&&btnAip.add.buttonCaption" type="primary" @click="handelAddClick(-1)">{{ btnAip.add.buttonCaption }}</el-button>
      <!-- <el-button v-if="btnAip.batchdelete.buttonCaption" size="small" type="info" @click="handelBatchDelete">{{ btnAip.batchdelete.buttonCaption }}</el-button> -->
    </el-header>
    <el-main :height="mainHeight">
      <el-table
        v-if="showtab"
        :height="mainHeight"
        :data="allDept"
        row-key="vitualId"
        border
        default-expand-all
        :tree-props="{children: 'children'}"
      >
        <el-table-column type="index" label="序号" />
        <el-table-column prop="deptName" label="部门名称" width="260" />
        <!-- <el-table-column prop="deptDesc" label="部门描述" class="ellipsis" /> -->
        <el-table-column prop="deptDesc" label="部门描述">
          <template slot-scope="scope">
            <el-tooltip
              v-if="scope.row.deptDesc.length>=20"
              class="item"
              effect="light"
              :content="scope.row.deptDesc"
              :open-delay="300"
              placement="top-end"
            >
              <div class="ellipsis">{{ scope.row.deptDesc }}</div>
            </el-tooltip>
            <div v-if="scope.row.deptDesc.length<20" class="ellipsis">{{ scope.row.deptDesc }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="seqNumber" label="排序" width="50" />
        <el-table-column label="操作" fixed="right" width="245">
          <template slot-scope="scope">
            <el-tooltip
              v-if="btnAip.on&&btnAip.on.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.on.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button type="primary" icon="el-icon-top" circle @click="handelMoveClick(scope.row.actualId,0)" />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.up&&btnAip.up.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.up.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button type="primary" icon="el-icon-bottom" circle @click="handelMoveClick(scope.row.actualId,1)" />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.addclass&&btnAip.addclass.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.addclass.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button type="primary" icon="el-icon-circle-plus-outline" circle @click="handelAddClick(scope.row)" />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.edit&&btnAip.edit.buttonCaption"
              class="item"
              effect="light"
              :content="btnAip.edit.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button type="primary" icon="el-icon-edit" circle @click="handelEditClick(scope.row)" />
            </el-tooltip>
            <el-tooltip
              v-if="btnAip.delete&&btnAip.delete.buttonCaption&&showtips"
              class="item"
              effect="light"
              :content="btnAip.delete.buttonCaption"
              placement="top-start"
              :open-delay="200"
            >
              <el-button type="danger" icon="el-icon-delete" circle @click="handelDelete(scope.row)" />
            </el-tooltip>
            <el-button v-if="btnAip.delete&&btnAip.delete.buttonCaption&&!showtips" type="danger" icon="el-icon-delete" circle @click="handelDelete(scope.row)" />
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <!-- <DeptDialog ref="DeptDialog" :dept="dept" /> -->
    <el-dialog
      :title="title"
      :visible.sync="dialogVisible"
      width="600px"
      :close-on-click-modal="false"
      :center="true"
    >
      <el-form ref="deptForm" :model="deptDataForm" label-width="100px" :rules="roleFrom">
        <el-form-item
          label="部门名称："
          prop="deptName"
        >
          <el-input v-model="deptDataForm.deptName" placeholder="请输入部门名称" />
        </el-form-item>
        <div v-if=" method=='post'">
          <el-form-item prop="parentName" label="上级：">
            <el-cascader
              v-if="!disabled"
              v-model="deptDataForm.parentName"
              style="width:100%"
              :options="allDept"
              :props="{ checkStrictly:true,value:'actualId',label:'deptName' }"
              clearable
            />
            <el-input v-if="disabled" v-model="deptDataForm.parentName" :disabled="disabled" />
          </el-form-item>
          <div v-if="disabled">
            <el-form-item v-if="!disabled" prop="parentName" label="上级：">
              <el-input v-if="disabled" v-model="deptDataForm.parentName" :disabled="disabled" />
            </el-form-item>
          </div>
        </div>
        <el-form-item label="排序：" prop="seqNumber">
          <el-input v-model="deptDataForm.seqNumber" />
        </el-form-item>
        <el-form-item label="部门描述：" prop="deptDesc">
          <el-input type="textarea" :rows="2" v-model="deptDataForm.deptDesc" placeholder="请输入部门描述" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="handelCloseClick">关闭</el-button>
        <el-button type="primary" :loading="saveBtn" @click="handelSave">保存</el-button>
      </div>
    </el-dialog>

  </el-container>
</template>
<script>
import height from '@/utils/height'
import request from '@/utils/request'
import RequestObject from '@/utils/requestObject'
import { getBtnCtr } from '@/utils/BtnControl'
export default {
  name: "viewsSysSettingsDeptindexvue",
  data() {
    var checkSeq = (rule, value, callback) => {
      var sum = /^[0-9]{1,2}$/
      
      // if (value == 0) {
      //   callback()
      // }
      if (!value) {
       if(value!=0) {
          return callback(new Error('序号不能为空'))
       }
      }
      if (!sum.test(value)) {
        callback(new Error('请输入两位数内的数字值'))
      } else {
        if (value < 0) {
          callback(new Error('必须是正整数'))
        } else {
          callback()
        }
      }
    }
    return {
      showtab:false,
      loading: true,
      saveBtn:false,
      btnAip: '',
      dept: '',
      title: '',
      disabled: '',
      deptData: false,
      headerHeight: '43px',
      mainHeight: '',
      footerHeight: '0px',
      tableHeight: '500',
      deptName: '',
      labelPosition: 'left',
      showtips:true,
      allDept: [],
      postDataEdit: {},
      deptDataForm: {
        deptDesc: '',
        deptName: '',
        seqNumber: 0,
        parentName: ''
      },
      roleFrom: {
        deptName: [
          { required: true, message: '请输入部门名称', trigger: 'blur' },
          { min: 2, max: 50, message: '长度需要在2到50个字符', trigger: 'blur' }
        ],
        seqNumber: [
          { required: true,validator: checkSeq, trigger: 'blur' }
        ]
      },
      method: 'post',
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 10, // 分页显示记录条数
      totalCount: 25, // 总记录数
      pageIndex: 1, // 页码
      sortColumn: '', // 排序字段
      sortOrder: '', // 排序规则
      menuName: '', // 菜单名称查询条件
      Status: '全部' // 菜单状态
    }
  },
  watch: {
    dialogVisible(val) {
      if (val) {
        setTimeout(() => {
          // this.pswOnly = false;
        }, 500)
      } else {
        // this.pswOnly = true
        this.saveBtn = false;
        this.$refs.deptForm.resetFields()
      }
    }
  },
  created() {
    // 动态头部按钮
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )
    // 高度
    this.mainHeight =
      (
        height -
        126 -
        parseInt(this.headerHeight) -
        parseInt(this.footerHeight)
      ).toString() + 'px'
    this.getDept()
  },
   activated() {
    this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
  },
  mounted() {
     this.showtab = false;
    setTimeout(() => {
      this.showtab = true;
    }, 10);
  },
  methods: {
    /**
     * getDept
     * 查询部门
     */
    getDept() {
      var reqObject = RequestObject.GetObject()
      request({
        url: '/system/api/TSMDept/GetOnlyDepAsync',
        method: 'get',
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        this.loading = false
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.allDept = res.data
        }
      })
        .catch(error => {
          this.loading = false
        })
    },
    /**
     * handelCloseClick
     * 关闭
     */
    handelCloseClick() {
      this.dialogVisible = false
    },
    /**
     * handelDelete
     * 删除
     */
    handelDelete(row) {
      this.showtips = false;
      var name
      if (row.parentId == -2 && row.actualId == null) {
        name = '公司'
      } else {
        name = '部门'
      }
      if (row.parentId == -2 || row.actualId == null) {
        this.$message({
          showClose: true,
          message: '此' + name + '名称不能删除',
          type: 'warning'
        })
        return
      }
      var postData = row.actualId
      this.$confirm('是否删除该部门下的所有内容？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.showtips = true;
        this.loading = true;
        var reqObject = RequestObject.DeleteObject(postData, null, null)
        request({
          url: '/system/api/TSMDept/ById',
          method: 'delete',
          data: reqObject
        }).then(res => {
          this.loading = false
          if (res.code == -1) {
            this.$confirm(res.info, '错误信息', {
              confirmButtonText: '确定',
              type: 'error',
              showCancelButton: false
            })
          } else {
            this.getDept()
            this.$message({
              message: '操作成功!',
              type: 'success'
            })
          }
        })
          .catch(error => {
            this.loading = false
          })
      }).catch(() => {
        this.showtips = true;
        this.$message({
          type: 'info',
          message: '已取消操作'
        })
      })
    },
    /**
     * handelEditClick
     * 编辑部门
     */
    handelEditClick(row) {
      this.title = '编辑部门信息'
      this.method = 'put'
      this.deptDataForm.pathLogic = '' // 5
      this.deptDataForm.deptDesc = row.deptDesc // 4
      this.deptDataForm.deptName = row.deptName // 3
      this.deptDataForm.parentId = row.parentId // 2
      this.deptDataForm.pathLogic = row.pathLogic
      this.deptDataForm.seqNumber = row.seqNumber
      this.deptDataForm.id = row.vitualId
      this.dialogVisible = true
      this.postDataEdit.id = 0
      this.postDataEdit.deptName = row.deptName
      this.postDataEdit.parentId = row.parentId
      this.postDataEdit.seqNumber = row.seqNumber
      this.postDataEdit.deptDesc = row.deptDesc
      this.postDataEdit.companyId = 0
      this.disabled = true
    },
    /**
     * handelAddClick
     * 添加下级
     */
    handelAddClick(row) {
      this.title = '添加部门'
      this.dialogVisible = true
      this.method = 'post'
      if (row == -1) {
        this.disabled = false
        this.deptDataForm.id = 0 // 1
        this.deptDataForm.deptName = ''// 3
        this.deptDataForm.parentId = -1 // 2
        this.deptDataForm.seqNumber = 99// 5
        this.deptDataForm.pathLogic = '' // 4
        this.deptDataForm.deptDesc = '' // 4
        this.deptDataForm.companyId = 0
        this.deptDataForm.parentName = ''
      } else {
        this.disabled = true
        this.deptDataForm.id = 0// 1
        this.deptDataForm.deptName = '' // 3
        this.deptDataForm.parentId = row.actualId // 2
        this.deptDataForm.seqNumber = 99 // 5
        this.deptDataForm.pathLogic = '' // 4
        this.deptDataForm.deptDesc = ''// 4
        this.deptDataForm.companyId = 0
        this.deptDataForm.parentName = row.deptName
      }
    },
    /**
     * handelSave
     * 保存
     */
    handelSave() {
      if (this.disabled == false) {
        if (this.deptDataForm.parentName == '' || this.deptDataForm.parentName.length == 0) {
          this.deptDataForm.parentId = -1
        } else {
          this.deptDataForm.parentId = this.deptDataForm.parentName[this.deptDataForm.parentName.length - 1]
        }
      }
      this.$refs.deptForm.validate(valid => {
        if (!valid) {
          this.$message({
            message: '数据不合法，无法保存',
            type: 'error'
          })
        } else {
          // this.loading = true;
          this.saveBtn = true;
          var postData = this.deptDataForm
          var reqObject
          if (this.method == 'post') {
            reqObject = RequestObject.GetObject(postData, null, null, null)
          } else {
            reqObject = RequestObject.GetObject(postData, null, null, this.postDataEdit)
          }
          request({
            url: '/system/api/TSMDept',
            method: this.method,
            data: reqObject
          }).then(res => {
            if (res.code == -1) {
              var timeId = setTimeout(()=>{
                // this.loading = false;
              this.$confirm(res.info, '错误信息', {
                confirmButtonText: '确定',
                type: 'error',
                showCancelButton: false
              })
              this.saveBtn = false;
              clearTimeout(timeId)
              },50)
            } else {
              var timeId = setTimeout(()=>{
                this.getDept()
              this.dialogVisible = false;
              this.$message({
                message: '操作成功!',
                type: 'success'
              })
              // this.saveBtn = false;
              // this.loading = false;
              clearTimeout(timeId)
              },50)
            }
          })
            .catch(error => {
              // this.saveBtn = false;
              // this.loading = false;
            })
        }
      })
    },
    /**
     * id:actualId
     * type: 0是上移，1是下移
     *
     * handelMoveClick
     * 上移下移
     */
    handelMoveClick(id, type) {
      this.loading = true
      var postData = {
        id: id,
        type: type
      }
      var reqObject = RequestObject.GetObject(postData, null, null)
      request({
        url: '/system/api/TSMDept/MovePostion',
        method: 'post',
        data: reqObject
      }).then(res => {
        if (res.code == -1) {
          this.loading = false
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.getDept()
          this.$message({
            message: '操作成功!',
            type: 'success'
          })
        }
      })
        .catch(error => {
          this.loading = false
        })
    }
  }
}
</script>
<style lang="scss" scoped>
#menus /deep/ {
  input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
    -webkit-appearance: none;
}
input[type="number"]{
    -moz-appearance: textfield;
}
  .header {
    padding-top: 10px;
  }
  .el-tag {
    cursor: pointer;
  }
  .ellipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    display: block;
    width: 100%;
    cursor: pointer;
  }
  .iconSty {
    width: 20px;
    height: 20px;
    vertical-align: middle;
  }
  .textCenter {
    text-align: center;
  }
  .dialog-footer {
    text-align: right;
  }
}
</style>
