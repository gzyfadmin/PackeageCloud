var format = function (v, format) {
  // v = toDate(v);
  if (v) {
    var o = {
      'M+': v.getMonth() + 1, // 月份
      'd+': v.getDate(), // 日
      'h+': v.getHours() % 12 == 0 ? 12 : v.getHours() % 12, // 小时
      'H+': v.getHours(), // 小时
      'm+': v.getMinutes(), // 分
      's+': v.getSeconds(), // 秒
      'q+': Math.floor((v.getMonth() + 3) / 3), // 季度
      'S': v.getMilliseconds() // 毫秒
    }
    var week = {
      '0': '\u65e5',
      '1': '\u4e00',
      '2': '\u4e8c',
      '3': '\u4e09',
      '4': '\u56db',
      '5': '\u4e94',
      '6': '\u516d'
    }
    if (/(y+)/.test(format)) {
      format = format.replace(RegExp.$1, (v.getFullYear() + '').substr(4 - RegExp.$1.length))
    }
    if (/(E+)/.test(format)) {
      format = format.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? '\u661f\u671f' : '\u5468') : '') + week[v.getDay() + ''])
    }
    for (var k in o) {
      if (new RegExp('(' + k + ')').test(format)) {
        format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (('00' + o[k]).substr(('' + o[k]).length)))
      }
    }
    return format
  }
}

const DateUtil = {
  Format: format
}

export function ArrObj_unique(arr1) {
  const res = new Map();
  return arr1.filter((a) => !res.has(a.id) && res.set(a.id, 1))
}

export default DateUtil
