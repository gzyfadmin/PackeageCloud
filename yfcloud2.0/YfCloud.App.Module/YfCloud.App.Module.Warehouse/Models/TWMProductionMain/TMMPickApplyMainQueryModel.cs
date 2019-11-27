using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models.TWMProductionMain
{
    public class TMMPickApplyMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 源单号ID（生产单ID）
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        /// 领料申请单号
        /// </summary>
        public string PickNo { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 制单人ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime OperatorTime { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识 false未删除 true已删除
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 转单标识 false不可转 true可转
        /// </summary>
        public bool TransferFlag { get; set; }


        /// <summary>
        /// 是否显示编辑按钮
        /// </summary>
        public bool IsShowEdit { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TMMPickApplyDetailQueryModel> ChildList { get; set; }
    }

    public class TMMPickApplyDetailQueryModel
    {

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialId { get; set; }



        /// <summary>
        /// 应收数量(生产)
        /// </summary>
        public decimal TransNum { get; set; }

        /// <summary>
        /// 应收数量(基本单位)
        /// </summary>
        public decimal BaseUnitTransNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (ProduceUnitId != null && ProduceRate != null)
                {
                    return decimal.Round(TransNum * (decimal)ProduceRate, 4);
                }
                return TransNum;
            }
        }


        /// <summary>
        /// 应收数量(库存)
        /// </summary>
        public decimal WarehouseUnitTransNum
        {
            get
            {
                //基本单位 = 生产单位数量 * 换算率
                //仓库单位 = 基本单位数量 / 换算率
                if (WarehouseUnitId != null && WarehouseRate != null)
                {
                    return decimal.Round(BaseUnitTransNum / (decimal)WarehouseRate, 4);
                }
                return BaseUnitTransNum;
            }
        }

        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料分类ID
        /// </summary>
        public int MaterialTypeId { get; set; }

        /// <summary>
        /// 物料分类名称
        /// </summary>
        public string MaterialTypeName { get; set; }

        /// <summary>
        /// 颜色Id
        /// </summary>
        public int? ColorId { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 基本计量单位ID
        /// </summary>
        public int BaseUnitId { get; set; }

        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string BaseUnitName { get; set; }

        /// <summary>
        /// 仓库计量单位ID
        /// </summary>
        public int? WarehouseUnitId { get; set; }

        /// <summary>
        /// 仓库计量单位名称
        /// </summary>
        public string WarehouseUnitName { get; set; }

        /// <summary>
        /// 仓库换算率
        /// </summary>
        public decimal? WarehouseRate { get; set; }

        /// <summary>
        /// 生产计量单位名称
        /// </summary>
        public string ProduceUnitName { get; set; }

        /// <summary>
        /// 生产单位ID
        /// </summary>
        public decimal? ProduceUnitId { get; set; }

        /// <summary>
        /// 生产换算率
        /// </summary>
        public decimal? ProduceRate { get; set; }
    }
}
