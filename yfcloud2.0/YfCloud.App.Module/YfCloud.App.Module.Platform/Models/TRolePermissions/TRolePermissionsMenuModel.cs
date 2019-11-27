using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.DBModel;

namespace YfCloud.App.Module.Platform.Models.TRolePermissions
{
    /// <summary>
    /// 菜单视图模型，用于前端展示数据
    /// </summary>
    public class MenuViewModel
    {
        /// <summary>
        /// path
        /// </summary>
        public string path
        {
            get;
            set;
        }

        public string component
        {
            get;
            set;
        }

        public MenuNodeViewModel meta
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }


        public bool alwaysShow { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuViewModel> children { get; set; }

        /// <summary>
        /// 按钮
        /// </summary>
        public List<TButtonsModel> buttons { get; set; }
    }

    /// <summary>
    /// 菜单节点
    /// </summary>
    public class MenuNodeViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
    }
}
