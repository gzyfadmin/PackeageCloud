using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Manufacturing.Models
{
    public class TMMPackageColorItem
    {
        public string unixId { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级ID，为空则为顶级
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TMMPackageColorItem> Children { get; set; }
    }
}
