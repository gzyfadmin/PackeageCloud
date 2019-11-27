// 将接口AppTree数据转成Select Options格式
var ToSelectOptions = function(data) {
  return GetSelectOptionsData(data, 1)
}
var ToTreeData = function(data) {
  return GetTreeData(data, 1)
}

function GetSelectOptionsData(data, level) {
  var resObject = []
  if (!data || data.length < 1) { return null }
  data.forEach((v, i) => {
    var thisId = v.id.toString()
    if (v.id === -1) {
      thisId = 'Tree' + level + '' + i
    }
    if (v.children && v.children.length > 0) {
      resObject.push({
        value: thisId,
        label: v.label,
        children: GetSelectOptionsData(v.children, level + 1)
      })
    } else {
      resObject.push({
        value: thisId,
        label: v.label,
        children: GetSelectOptionsData(v.children, level + 1)
      })
    }
  })
  return resObject
}
function GetTreeData(data, level) {
  var resObject = []
  if (!data || data.length < 1) { return null }
  data.forEach((v, i) => {
    var thisId = v.id.toString()
    if (v.id === -1) {
      thisId = 'Tree' + level + '' + i
    }
    if (v.children && v.children.length > 0) {
      resObject.push({
        id: thisId,
        label: v.label,
        children: GetTreeData(v.children, level + 1)
      })
    } else {
      resObject.push({
        id: thisId,
        label: v.label,
        children: GetTreeData(v.children, level + 1)
      })
    }
  })
  return resObject
}
// 增加disabled
function GetTreeDataDisabled(data, level) {
  var resObject = []
  if (!data || data.length < 1) { return null }
  data.forEach((v, i) => {
    var thisId = v.id.toString()
    var dis = false
    if (v.id === -1) {
      thisId = 'Tree' + level + '' + i
      dis = true
    }
    if (v.children && v.children.length > 0) {
      resObject.push({
        id: thisId,
        label: v.label,
        disabled: true,
        children: GetTreeDataDisabled(v.children, level + 1)
      })
    } else {
      resObject.push({
        id: thisId,
        label: v.label,
        disabled: true,
        children: GetTreeDataDisabled(v.children, level + 1)
      })
    }
  })
  return resObject
}

function GetTreeOnButtons(data) {
  var resObject = []
  if (!data || data.length < 1) { return null }
  data.forEach((v, i) => {
    if (v.children && v.children.length > 0) {
      resObject.push({
        btnIf: v.btnIf,
        btnStatebtnState: v.btnState,
        buttons: [],
        children: GetTreeOnButtons(v.children),
        createId: v.createId,
        createTime: v.createTime,
        id: v.id,
        isMenu: v.isMenu,
        menuAnotherName: v.menuAnotherName,
        menuDesc: v.menuDesc,
        menuIcon: v.menuIcon,
        menuName: v.menuName,
        menuPath: v.menuPath,
        onButtons: v.buttons,
        parentID: v.parentID,
        seq: v.seq,
        status: v.status,
        btnState: false
      })
    } else {
      resObject.push({
        btnIf: v.btnIf,
        btnStatebtnState: v.btnState,
        buttons: [],
        children: GetTreeOnButtons(v.children),
        createId: v.createId,
        createTime: v.createTime,
        id: v.id,
        isMenu: v.isMenu,
        menuAnotherName: v.menuAnotherName,
        menuDesc: v.menuDesc,
        menuIcon: v.menuIcon,
        menuName: v.menuName,
        menuPath: v.menuPath,
        onButtons: v.buttons,
        parentID: v.parentID,
        seq: v.seq,
        status: v.status,
        btnState: false
      })
    }
  })
  return resObject
}

var AppTreeUtil = {
  ToSelectOptions: ToSelectOptions,
  ToTreeData: ToTreeData,
  ToTreeDataDisabled: GetTreeDataDisabled,
  GetTreeOnButtons: GetTreeOnButtons
}

export default AppTreeUtil
