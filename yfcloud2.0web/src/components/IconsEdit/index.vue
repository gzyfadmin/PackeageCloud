<template>
  <div id="IconsEdit" class="icons-container">
    <!-- <aside>
      <a href="https://panjiachen.github.io/vue-element-admin-site/guide/advanced/icon.html" target="_blank">Add and use
      </a>
    </aside> -->

    <el-dialog title="选择图标" :visible.sync="dialogVisible" width="55%" :close-on-click-modal="false" :center="true">
        <div class="el-tab-pane">
          <div
            v-for="item of svgIcons"
            :key="item"
            @click="handleClipboard(generateIconCode(item),$event)"
          >
            <div class="icon-item">
              <svg-icon :icon-class="item" class-name="disabled" />
            </div>
          </div>
          <div
            v-for="item of elementIcons"
            :key="item"
            @click="handleClipboard(generateElementIconCode(item),$event)"
          >
            <div class="icon-item">
              <i :class="'el-icon-' + item" />
            </div>
          </div>
        </div>
    </el-dialog>
  </div>
</template>

<script>
import clipboard from '@/utils/clipboard'
import svgIcons from './svg-icons'
import elementIcons from './element-icons'

export default {
  name: 'Icons',
  props: {

  },
  data() {
    return {
      svgIcons,
      elementIcons,
      dialogVisible: false,
      svgText:'',
    }
  },
  methods: {
    doOpen() {
      this.dialogVisible = true
    },
    doClose() {
      this.dialogVisible = false
    },
    generateIconCode(symbol) {
      this.svgText=symbol;
      // return symbol
      return `<svg-icon icon-class="${symbol}" />`
    },
    generateElementIconCode(symbol) {
      this.svgText='';
      // return symbol
      return `<i class="el-icon-${symbol}" />`
    },
    handleClipboard(text, event, state) {
      this.$emit('doSelect', text, this.svgText)
      // clipboard(text, event)
    }
  }
}
</script>

<style lang="scss" scoped>
.icons-container {
  // margin: 10px 20px 0;
  overflow: hidden;

  .icon-item {
    margin: 20px;
    height: 85px;
    text-align: center;
    width: 100px;
    float: left;
    font-size: 30px;
    color: #24292e;
    cursor: pointer;
  }

  span {
    display: block;
    font-size: 16px;
    margin-top: 10px;
  }

  .disabled {
    pointer-events: none;
  }
}
#IconsEdit /deep/ {
  .el-dialog__body{
    padding:10px;
  }
  .iconStyle{
    margin:10px;
  }
}


.el-tab-pane {
  display: flex;
  flex-wrap: wrap;
  .icon-item {
    font-size: 36px;
    width: 50px;
    height: 50px;
    text-align: center;
    line-height: 50px;
    margin:0px;
  }
}
.icon-item {
  font-size: 36px;
  width: 50px;
  height: 50px;
  i{
    color: #606266;
  }
}
svg{
  color: #606266;
}

</style>
