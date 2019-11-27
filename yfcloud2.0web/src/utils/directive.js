import Vue from 'vue'

// 注册一个全局自定义指令 `v-listenScroll`
Vue.directive('loadmore', {
	bind(el, binding) {
		const selectWrap = el.querySelector('.el-table__body-wrapper')
		selectWrap.addEventListener('scroll', function () {
			let sign = 100
			const scrollDistance = this.scrollHeight - this.scrollTop - this.clientHeight
			if (scrollDistance <= sign) {
				binding.value()
			}
		})
	}
})