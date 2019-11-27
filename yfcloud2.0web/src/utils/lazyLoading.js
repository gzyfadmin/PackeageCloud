// export default (name) => () => import('@/views/'+name+'.vue')

function getViews(path1, path2) {
  return resolve => {
    require.ensure([], (require) => {
      resolve(require(`@/views/${path1}/${path2}/index.vue`))
      // console.log(`@/views/${path1}/${path2}/index.vue`)
    })
  }
}
// function getViews(path1, path2) {
//   return resolve => {
//     require([`@/views/${path1}/${path2}/index.vue`],resolve)
//   }
// }

export default getViews;