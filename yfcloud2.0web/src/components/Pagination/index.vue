<template>
  <div id="pagination">
    <el-pagination
      :current-page="pageIndex"
      :page-sizes="pageSizes"
      @size-change="handleSizeChange"
      :page-size="pageSize"
      @current-change="handleCurrentChange"
      :layout="layout"
      :total="totalCount"
    />
  </div>
</template>
<script>
export default {
  props: {
    totalCount: {
      required: true,
      type: Number
    },
    pageIndex: {
      required: true,
      type: Number
    },
    pageSize: {
      required: true,
      type: Number
    },
    pageSizes: {
      type: Array,
      default() {
        return [25, 50, 100, 200]
      }
    },
    layout: {
      type: String,
      default: 'total, sizes, prev, pager, next, jumper'
    }
  },
  data: () => ({
    sizeFrom: 20
  }),
  mounted() {},
  methods: {
    handleSizeChange(val) {
      this.$emit('pagination', { pageSize: val, pageIndex: 1 })
    },
    handleCurrentChange(val) {
      this.$emit('pagination', { pageSize: this.pageSize, pageIndex: val })
    }
  }
}
</script>
<style lang="scss" scoped>
#pagination /deep/{
  .el-pagination{
    text-align: center;
  }
}
</style>
