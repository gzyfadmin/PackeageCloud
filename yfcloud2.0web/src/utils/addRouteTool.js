
export function formateRoute(cloneRoute) {
    return new Promise((resolve, reject) => {
        var RoutesPath = cloneRoute
        RoutesPath.forEach((item) => {//循环每个主菜单
            if (item.alwaysShow) {
                hdRoutesPath(item)
            }
        })
        // hdRoutesPath(RoutesPath[2])
        function hdRoutesPath(routeItem) {//处理路由数据
            var routerList = [];
            hdRoute(routeItem)

            function hdRoute(data) {//获取所有子菜单
                if (data.children) {
                    for (var i = 0; i < data.children.length; i++) {
                        if (data.children[i].children) {
                            routerList = [...routerList, ...getRoute(data.children[i].children)]
                        }

                    }
                }
                hdDel(routeItem)//删除原数组有children数据

            }

            function getRoute(item) {//返回所有的二级菜单子路由
                var findChild = (item) => {
                    var list = [];
                    for (var i = 0; i < item.length; i++) {
                        if (item[i].children) {
                            findChild(item[i].children)
                        } else {
                            list.push(item[i])
                        }
                    }
                    return list;
                }
                return findChild(item);
            }

            function hdDel(dt) {//删除有二级子路由页面的数据并将二级菜单子路由添加到一级菜单
                if (dt.children) {
                    for (var j = 0; j < dt.children.length; j++) {
                        if (dt.children[j].children) {
                            // dt.children = []
                            delete dt.children[j]

                        }
                    }
                }
                routerList.forEach((val) => {
                    dt.children.push(val)
                })

            }



        }

        resolve(RoutesPath)

    });


}