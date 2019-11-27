export function formatDate(date) {
    // 格式化日期
    if (date == "" || date == null) {
        return ""
    }
    var format = "";
    var oY = "";
    var oM = "";
    var oD = "";
    var oDate = new Date(date);
    oY = oDate.getFullYear();
    oM = oDate.getMonth() + 1;
    oM = oM < 10 ? "0" + oM : oM;
    oD = oDate.getDate();
    oD = oD < 10 ? "0" + oD : oD;
    format = oY + "-" + oM + "-" + oD;
    return format;
}
//四舍五入保留2位小数（不够位数，则用0替补）
export function keepTwoDecimalFull(num) {
    // var result = parseFloat(num);
    // if (isNaN(result)) {
    //     alert('传递参数错误，请检查！');
    //     return false;
    // }
    // result = Math.round(num * 100) / 100;
    // var s_x = result.toString();
    // var pos_decimal = s_x.indexOf('.');
    // if (pos_decimal < 0) {
    //     pos_decimal = s_x.length;
    //     s_x += '.';
    // }
    // while (s_x.length <= pos_decimal + 2) {
    //     s_x += '0';
    // }
    var s_x = getFloat(num)
    return s_x;
}
//解决乘法丢精度问题
export function accMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { m += s1.split(".")[1].length } catch (e) { }
    try { m += s2.split(".")[1].length } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
}

//解决除法丢精度问题
// export function accDiv(arg1, arg2) {
//     var t1 = 0, t2 = 0, r1, r2;
//     try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
//     try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
//     // with (Math) {
//     r1 = Math.Number(arg1.toString().replace(".", ""))
//     r2 = Math.Number(arg2.toString().replace(".", ""))
//     return (r1 / r2) * pow(10, t2 - t1);
//     // }
// }

//加法 
export function accAdd(arg1, arg2) {
    var r1, r2, m;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2))
    return (arg1 * m + arg2 * m) / m
}
//减法 
export function Subtr(arg1, arg2) {
    var r1, r2, m, n;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2));
    n = (r1 >= r2) ? r1 : r2;
    return ((arg1 * m - arg2 * m) / m).toFixed(n);
}

//数组对象去重相加
export function mergeArr(arr, id, name) {
    var newArr = [];
    arr.forEach(item => {
        var dataItem = item
        if (newArr.length > 0) {
            var filterValue = newArr.filter(v => {
                return v[id] == dataItem[id]
            })
            if (filterValue.length > 0) {
                newArr.forEach(n => {
                    if (n[id] == filterValue[0][id]) {
                        n[name] = filterValue[0][name] + dataItem[name]
                    }
                })
            } else {
                newArr.push(dataItem)
            }
        } else {
            newArr.push(dataItem)
        }

    })
    return newArr
}

//查找数组对象重复字段
export function mergeArrRepeat(arr, id) {
    var newArr = [];
    var repeatArr = [];
    arr.forEach(item => {
        var dataItem = item
        if (newArr.length > 0) {
            var filterValue = newArr.filter(v => {
                return v[id] == dataItem[id]
            })
            if (filterValue.length > 0) {
                newArr.forEach(n => {
                    if (n[id] == filterValue[0][id]) {
                        repeatArr.push(n[id])
                    }
                })
            } else {
                newArr.push(dataItem)
            }
        } else {
            newArr.push(dataItem)
        }

    })
    return repeatArr
}

export function isRealNum(val) {
    // isNaN()函数 把空串 空格 以及NUll 按照0来处理 所以先去除，

    if (val === "" || val == null) {
        return false;
    }
    if (!isNaN(val)) {
        //对于空数组和只有一个数值成员的数组或全是数字组成的字符串，isNaN返回false，例如：'123'、[]、[2]、['123'],isNaN返回false,
        //所以如果不需要val包含这些特殊情况，则这个判断改写为if(!isNaN(val) && typeof val === 'number' )

        return checkNumLength(val);
    }

    else {
        return false;
    }
}

//验证是否超过14位

function checkNumLength(val) {
    var d = parseInt(val)
    if (val > 99999999999999) {
        return false
    }
    if ((d + '').length > 14) {
        return false
    } else {
        return true
    }
}

//js中保留4位小数，超过4位截取保留4位
export function getFloat(number, s) {
    var n = 4;
    n = n ? parseInt(n) : 0;
    if (n <= 0) {
        return Math.round(number);
    }
    number = Math.round(number * Math.pow(10, n)) / Math.pow(10, n); //四舍五入
    // number = Number(number).toFixed(n); //补足位数
    return number;
}

export function trim(str) {
    return str.replace(/^(\s|\u00A0)+/, '').replace(/(\s|\u00A0)+$/, '');
}


export function formatMoney(value, num) {
    num = num > 0 && num <= 20 ? num : 2;

    value = parseFloat((value + "").replace(/[^\d\.-]/g, "")).toFixed(num) + ""; //将金额转成比如 123.45的字符串

    var valueArr = value.split(".")[0].split("") //将字符串的数变成数组

    const valueFloat = value.split(".")[1]; // 取到 小数点后的值

    let valueString = "";

    for (let i = 0; i < valueArr.length; i++) {

        valueString += valueArr[i] + ((i + 1) % 3 == 0 && (i + 1) != valueArr.length ? "," : ""); //循环 取数值并在每三位加个','

    }

    const money = valueString.split("").join("") + "." + valueFloat; //拼接上小数位

    return money

}

//判断字符是否为空的方法
export function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "" || obj == "null") {
        return true;
    } else {
        return false;
    }
}

//千分符转数字
export function delcommafy(str) {
    str = str.replace(/,/g, '');
    var num = Number(str);
    return str
}
//数组去重
export function unique(arr) {
    return Array.from(new Set(arr))
}
//数组多个字段排序
export function getMutipSort(arr) {
    return function (a, b) {
        var tmp, i = 0;

        do {
            tmp = arr[i++](a, b);
        } while (tmp == 0 && i < arr.length);

        return tmp;
    }
}
//数组多个字段排序
export function getSort(fn) {
    return function (a, b) {
        var ret = 0;

        if (fn.call(this, a, b)) {
            ret = -1;
        } else if (fn.call(this, b, a)) {
            ret = 1;
        }

        return ret;
    }
}

//克隆
export function deepClone(obj) {
    let newObj = Array.isArray(obj) ? [] : {}

    if (obj && typeof obj === "object") {
        for (let key in obj) {
            if (obj.hasOwnProperty(key)) {
                newObj[key] = (obj && typeof obj[key] === 'object') ? deepClone(obj[key]) : obj[key];
            }
        }
    }
    return newObj
}


//验证是否大于0，最多四位小数
export function islegalNum(num) {
    //是否是数字
    if (isRealNum(num)) {
        //不能小于0
        if (num <= 0) {
            return false;
        }
        var x = String(num).indexOf('.') + 1; //小数点的位置
        //是否是小数
        if (x == 0) {
            return true;
        }
        var y = String(num).length - x; //小数的位数
        if (y > 4) {
            return false
        } else {
            return true;
        }
    } else {
        return false;
    }

}