export function getPageItem(path, dt) {
    let item = {};
    let dofing = (path, dt) => {
        for (var i = 0; i < dt.length; i++) {
            if (path == dt[i].path) {
                // console.log(dt[i])
                item = dt[i];
            }
            if (dt[i].children && dt[i].children.length > 0) {
                dofing(path, dt[i].children);
            }
        }
    };
    dofing(path, dt);
    return item;
}
