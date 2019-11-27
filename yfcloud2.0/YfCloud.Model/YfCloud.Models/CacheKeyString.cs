using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 缓存的Key
    /// </summary>
    public class CacheKeyString
    {
        /// <summary>
        /// 系统管理用户的缓存
        /// </summary>
        public readonly static string UserAccount = @"System.T_SM_UserAccount_{0}";

        /// <summary>
        /// 系统用户登陆状态的保存
        /// </summary>
        public readonly static string UserLoginAllKey = @"T_Sm_UserAccount#{0}#{1}#{2}";

        /// <summary>
        /// 前缀
        /// </summary>
        public readonly static string UserLoginAllKeyPre = @"T_Sm_UserAccount";

        /// <summary>
        /// 仓库编码加锁Key
        /// </summary>
        public readonly static string LockTWMPurchaseWhMain = @"Lock_TWMPurchaseWhMainService_{0}";

        /// <summary>
        /// 其他入库key
        /// </summary>
        public readonly static string LockAutoCodeOtherOWR = @"LockAutoCodeOtherOWR_{0}";

        /// <summary>
        /// 其他入库日期
        /// </summary>
        public readonly static string LockAutoCodeOtherOWR_date = @"LockAutoCodeOtherOWR_data_{0}";


        /// <summary>
        /// 其他出库key
        /// </summary>
        public readonly static string LockAutoCodeOtherOWS = @"LockAutoCodeOtherOWS_{0}";

        /// <summary>
        /// 其他出库日期
        /// </summary>
        public readonly static string LockAutoCodeOtherOWS_date = @"LockAutoCodeOtherOWS_date_{0}";


        /// <summary>
        /// 盘点key
        /// </summary>
        public readonly static string LockGeneratorIC = @"LockGeneratorIC_{0}";

        /// <summary>
        /// 盘点日期
        /// </summary>
        public readonly static string LockGeneratorIC_date = @"LockGeneratorIC_date_{0}";

        /// <summary>
        /// 盘盈入库key
        /// </summary>
        public readonly static string LockGeneratorIR = @"LockGeneratorIR_{0}";

        /// <summary>
        /// 盘盈入库日期
        /// </summary>
        public readonly static string LockGeneratorIR_date = @"LockGeneratorIR_date_{0}";

        /// <summary>
        /// 盘亏出库key
        /// </summary>
        public readonly static string LockGeneratorIS = @"LockGeneratorIS_{0}";

        /// <summary>
        /// 盘亏出库日期
        /// </summary>
        public readonly static string LockGeneratorIS_date = @"LockGeneratorIS_date_{0}";

        /// <summary>
        /// 销售出库key
        /// </summary>
        public readonly static string LockGeneratorSOWR = @"LockGeneratorSOWR_{0}";

        /// <summary>
        /// 销售出库日期
        /// </summary>
        public readonly static string LockGeneratorSOWR_date = @"LockGeneratorSOWR_date_{0}";

        /// <summary>
        /// 销售单key
        /// </summary>
        public readonly static string LockGeneratorSO = @"LockGeneratorSO_{0}";

        /// <summary>
        /// 销售单日期
        /// </summary>
        public readonly static string LockGeneratorSO_date = @"LockGeneratorSO_date_{0}";

        /// <summary>
        /// 采购单Key
        /// </summary>
        public readonly static string LockGeneratorPO = @"LockGeneratorPO_{0}";

        /// <summary>
        /// 采购单日期
        /// </summary>
        public readonly static string LockGeneratorPO_date = @"LockGeneratorPO_date_{0}";

        /// <summary>
        /// 采购入库key
        /// </summary>
        public readonly static string LockGeneratorWO = @"LockGeneratorWO_{0}";

        /// <summary>
        /// 采购入库日期
        /// </summary>
        public readonly static string LockGeneratorWO_date = @"LockGeneratorWO_date_{0}";

        /// <summary>
        /// 生产单号
        /// </summary>
        public readonly static string LockGeneratorMO = @"LockGeneratorMO_{0}";
        /// <summary>
        /// 生产日期
        /// </summary>
        public readonly static string LockGeneratorMO_date = @"LockGeneratorMO_date_{0}";

        /// <summary>
        /// 数据字典缓存
        /// </summary>
        public readonly static string TBMDictionary = @"TBMDictionary_{0};";

        /// <summary>
        /// 地区缓存
        /// </summary>
        public readonly static string ChinaArea = @"ChinaArea@@";

        /// <summary>
        /// 地区缓存
        /// </summary>
        public readonly static string ChinaAreaTree = @"ChinaAreaTree";

        /// <summary>
        /// 站点配置
        /// </summary>
        public readonly static string Domain = @"Domain";

        /// <summary>
        /// 物料缓存
        /// </summary>
        public readonly static string Material = @"Material_{0}";

        /// <summary>
        /// 物料编码
        /// </summary>
        public readonly static string LockMaterialCode = @"LockMaterialCode_{0}";

        /// <summary>
        /// 物料编码日期
        /// </summary>
        public readonly static string LockMaterialCode_date = @"LockMaterialCode_date_{0}";

        /// <summary>
        /// 生产领料单key
        /// </summary>
        public readonly static string LockAutoCodePMR = @"LockAutoCodePMR_{0}";

        /// <summary>
        /// 生产领料单日期
        /// </summary>
        public readonly static string LockAutoPMR_date = @"LockAutoPMR_date_{0}";

        /// <summary>
        /// 生产领料单key
        /// </summary>
        public readonly static string LockAutoCodePDR = @"LockAutoCodePDR_{0}";

        /// <summary>
        /// 生产领料单日期
        /// </summary>
        public readonly static string LockAutoPDR_date = @"LockAutoPDR_date_{0}";


        /// <summary>
        /// 生产领料单key
        /// </summary>
        public readonly static string LockAutoCodePR = @"LockAutoCodePR_{0}";

        /// <summary>
        /// 生产领料单日期
        /// </summary>
        public readonly static string LockAutoPR_date = @"LockAutoPR_date_{0}";

        /// <summary>
        /// 生产出库key
        /// </summary>
        public readonly static string LockAutoCodePDC = @"LockAutoCodePDC_{0}";

        /// <summary>
        /// 生产出库日期
        /// </summary>
        public readonly static string LockAutoPDC_date = @"LockAutoPDC_date_{0}";


        /// <summary>
        /// 普通领料单key
        /// </summary>
        public readonly static string LockAutoCodeMR = @"LockAutoCodeMR_{0}";

        /// <summary>
        /// 普通领料单日期
        /// </summary>
        public readonly static string LockAutoMR_date = @"LockAutoMR_date_{0}";

        /// <summary>
        /// Any key
        /// </summary>
        public readonly static string LockAutoAnyCode = @"LockAutoCode{0}_{1}";

        /// <summary>
        /// Any Date
        /// </summary>
        public readonly static string LockAutoAnyCode_date = @"LockAutoCode{0}_date_{1}";

        /// <summary>
        /// 在线时长统计
        /// </summary>
        public readonly static string StaOnlineUser = @"StaOnlineUser_{0}_{1}";

        /// <summary>
        /// 登陆次数统计
        /// </summary>
        public readonly static string StaOnlineTimes = @"StaOnlineUserTime_{0}_{1}";

        /// <summary>
        /// 在线时长统计前缀
        /// </summary>
        public readonly static string StaOnlineUserPre = @"StaOnlineUser_";

        /// <summary>
        /// 注册密码的redisKey 
        /// </summary>
        public readonly static string RegisterMsgCode = @"RegisterMsgCode_{0}";

        /// <summary>
        /// 忘记密码的redisKey
        /// </summary>
        public readonly static string ForgetMsgCode = @"ForgetMsgCode_{0}";

        /// <summary>
        /// 多次登录失败后的redisKey
        /// </summary>
        public readonly static string TimePassMsgCode = @"TimePassMsgCode_{0}";

        /// <summary>
        /// 更换手机号码redisKey
        /// </summary>
        public readonly static string ChangeMobileMsgCode = @"ChangeMobileMsgCode_{0}";
         
        /// <summary>
        /// 通知
        /// </summary>
        public readonly static string Notice = @"Notice_{0}_{1}";

        /// <summary>
        /// 当前登录次数统计超过三次需要验证码
        /// </summary>
        public readonly static string LoginTimes = @"LoginTimes_{0}";
    }
}
