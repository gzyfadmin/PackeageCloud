import store from '@/store'

const { body } = document
const WIDTH = 500 // refer to Bootstrap's responsive design
const WIDTH2 = 1024 // refer to Bootstrap's responsive design

export default {
  watch: {
    $route(route) {
      if (this.device === 'mobile' && this.sidebar.opened) {
        // store.dispatch('app/closeSideBar', { withoutAnimation: false })
      }
    }
  },
  beforeMount() {
    window.addEventListener('resize', this.$_resizeHandler)
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.$_resizeHandler)
  },
  mounted() {
    const isMobile = this.$_isMobile()
    if (isMobile) {
      // store.dispatch('app/toggleDevice', 'mobile')
      // store.dispatch('app/closeSideBar', { withoutAnimation: true })
    }
  },
  methods: {
    // use $_ for mixins properties
    // https://vuejs.org/v2/style-guide/index.html#Private-property-names-essential
    $_isMobile() {
      const rect = body.getBoundingClientRect()
      return rect.width - 1 < WIDTH
    },
    $_isMobile2() {
      const rect = body.getBoundingClientRect()
      return rect.width - 1 < WIDTH2
    },
    $_resizeHandler() {
      if (!document.hidden) {
        const isMobile = this.$_isMobile()
        const isMobile2 = this.$_isMobile2()
        // store.dispatch('app/toggleDevice', isMobile ? 'mobile' : 'desktop')

        if (isMobile) {
          // store.dispatch('app/closeSideBar', { withoutAnimation: true })
        }
        if (isMobile2) {
          store.dispatch('app/closeSideBar', { withoutAnimation: true })
        }
      }
    }
  }
}
