using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using YfCloud.Caches;
using YfCloud.DBModel;
using YfCloud.DBModel.System;
using YfCloud.Models;

namespace YfCloud.Framework.WebApi
{
    /// <summary>
    /// 系统用户管理
    /// </summary>
    public class SMCurentUserManager
    {
       
        public static SMUserInfo GetCurentUserID(IHeaderDictionary heads, SqlSugarClient Instance)
        {
            int UserID = TokenManager.GetUserIDbyToken(heads);

            SMUserInfo result = new SMUserInfo();

            string Key = string.Format(CacheKeyString.UserAccount, UserID);

            //var redis = CacheFactory.Instance(CacheType.Redis);
            //result = redis.GetValueByKey<SMUserInfo>(Key);

            if (result == null)
            {
                var dbModel = Instance.Queryable<TSMUserAccountDbModel>()
                  .Where(p => p.ID == UserID).First();


                var dbUserInfo = Instance.Queryable<TSMUserInfoDbModel>()
                   .Where(p => p.ID == dbModel.UserInfoId)
                   .First();

                var rolesDbModel = Instance.Queryable<TSMRoleUserRelationDbModel, TSMRolesDbModel>(
                               (t1, t2) => new object[]
                               {
                                    JoinType.Left,  t1.RoleId== t2.Id

                               }).Where((t1, t2) => t1.UserId == dbModel.ID).Select((t1, t2) => t2).First();

                SMUserInfo sMUserInfo = new SMUserInfo();
                sMUserInfo.UserID = dbModel.ID;
                sMUserInfo.CompanyId = dbModel.CompanyId;
                sMUserInfo.EmailAccount = dbModel.EmailAccount;
                sMUserInfo.TelAccount = dbModel.TelAccount;
                sMUserInfo.AccountName = dbModel.AccountName;
                sMUserInfo.RealName = dbUserInfo?.RealName;
                sMUserInfo.RoleName = rolesDbModel?.RoleName;

                //redis.AddKey<SMUserInfo>(Key, sMUserInfo, 60 * 60 * 24);

                result = sMUserInfo;
            }

            return result;
        }

        public static SMUserInfo GetCurentUserID(int UserID, SqlSugarClient Instance)
        {
            SMUserInfo result = new SMUserInfo();

            string Key = string.Format(CacheKeyString.UserAccount, UserID);

            //var redis = CacheFactory.Instance(CacheType.Redis);
            //result = redis.GetValueByKey<SMUserInfo>(Key);

            result = null; //先强行查数据库
            if (result == null)
            {
                var dbModel = Instance.Queryable<TSMUserAccountDbModel>()
                  .Where(p => p.ID == UserID).First();


                var dbUserInfo =  Instance.Queryable<TSMUserInfoDbModel>()
                   .Where(p => p.ID == dbModel.UserInfoId)
                   .First();

                var rolesDbModel = Instance.Queryable<TSMRoleUserRelationDbModel, TSMRolesDbModel>(
                               (t1, t2) => new object[]
                               {
                                    JoinType.Left,  t1.RoleId== t2.Id

                               }).Where((t1, t2) => t1.UserId == dbModel.ID).Select((t1, t2) => t2).First();

                SMUserInfo sMUserInfo = new SMUserInfo();
                sMUserInfo.UserID = dbModel.ID;
                sMUserInfo.CompanyId = dbModel.CompanyId;
                sMUserInfo.EmailAccount = dbModel.EmailAccount;
                sMUserInfo.TelAccount = dbModel.TelAccount;
                sMUserInfo.AccountName = dbModel.AccountName;
                sMUserInfo.RealName = dbUserInfo?.RealName;
                sMUserInfo.RoleName = rolesDbModel?.RoleName;
                //redis.AddKey<SMUserInfo>(Key, sMUserInfo, 60 * 60 * 24);

                result = sMUserInfo;
            }

            return result;
        }
    }
}
