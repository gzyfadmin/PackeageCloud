using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;

namespace YfCloud.Framework
{
    /// <summary>
    /// IDbContext
    /// </summary>
    public abstract class IDbContext
    {
        /// <summary>
        /// SqlSugarClient Instance
        /// </summary>
        public virtual SqlSugarClient Instance { get; set; }
    }
}
