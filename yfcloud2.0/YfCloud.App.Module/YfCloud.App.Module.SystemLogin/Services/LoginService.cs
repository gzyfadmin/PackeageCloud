using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YfCloud.App.Module.SystemLogin.Models.TSMLoginLog;
using YfCloud.App.Module.SystemLogin.Models.TSMUserAccount;
using YfCloud.Framework;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;
using YfCloud.DBModel;
using YfCloud.Caches;
using YfCloud.Utilities.MongoDb;
using YfCloud.DBModel.System;
using SqlSugar;
using YfCloud.Utilities.SqlSugar;
using YfCloud.Framework.WebApi;

namespace YfCloud.App.Module.SystemLogin.Services
{
    /// <summary>
    /// 系统登录服务
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(ILoginService))]
    public class LoginService : ILoginService
    {
        private readonly IDbContext _db;//数据库操作实例

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public LoginService(IDbContext dbContext)
        {
            _db = dbContext;
        }


        /// <summary>
        /// 验证登录是否需要验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<int>> GetVerification(RequestGet request)
        {
            try
            {
                String Mobile = "";
                if (request.QueryConditions != null && request.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(request.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "mobile")
                        {
                            Mobile = item.FieldValue;
                            continue;
                        }
                    }
                }
                var verification = CacheFactory.Instance(CacheType.Redis);
                if (verification.ContainsKey(string.Format(CacheKeyString.LoginTimes, Mobile)))
                {
                    string RedisValue = verification.GetValueByKey<string>(string.Format(CacheKeyString.LoginTimes, Mobile));
                    if (Convert.ToInt32(RedisValue) >= 3)
                    {
                        return ResponseUtil<int>
                       .FailResult(2, "登录次数超过限制，请输入验证码登录");
                    }
                }
                return ResponseUtil<int>
                       .FailResult(1, "无需验证");
            }
            catch (Exception ex)
            {
                //返回异常信息
                return ResponseUtil<int>
                       .FailResult(0, ex.Message);
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<TSMUserLoginResult>> LoginAsync(RequestPost<TSMUserAccountAddModel> requestObject)
        {
            try
            {
                TSMUserLoginResult tSMUserLoginResult = new TSMUserLoginResult();

                if (string.IsNullOrEmpty(requestObject.PostData.TelAccount) &&
               string.IsNullOrEmpty(requestObject.PostData.EmailAccount))
                {
                    return ResponseUtil<TSMUserLoginResult>
                        .FailResult(null, "手机号或邮箱号至少需要一个");
                }

                //当前登录账户信息
                var curr = requestObject.PostData;
                //查询数据库是否有该账户
                var dbModel = await _db.Instance.Queryable<TSMUserAccountDbModel>()
                    .Where(p => p.TelAccount == curr.TelAccount || p.EmailAccount == curr.EmailAccount)
                    .FirstAsync();

                //如果没有该账户返回错误信息
                if (dbModel == null)
                {
                    return ResponseUtil<TSMUserLoginResult>
                       .FailResult(null, "登录失败，账户不存在!");
                }

                if (dbModel.Status != 1)
                {
                    return ResponseUtil<TSMUserLoginResult>
                      .FailResult(null, "登录失败，账户无效或过期");
                }
                #region 如果登录三次以上需要提供手机验证码
                var verification = CacheFactory.Instance(CacheType.Redis);
                if (verification.ContainsKey(string.Format(CacheKeyString.LoginTimes, curr.TelAccount)))
                {
                    string RedisValue = verification.GetValueByKey<string>(string.Format(CacheKeyString.LoginTimes, curr.TelAccount));
                    if (Convert.ToInt32(RedisValue) >= 3)
                    {
                        if (string.IsNullOrWhiteSpace(curr.VerificationCode))
                        {
                            return ResponseUtil<TSMUserLoginResult>.FailResult(null, "登录失败，请输入验证码");
                        }
                        if (curr.VerificationCode != verification.GetValueByKey<string>(string.Format(CacheKeyString.TimePassMsgCode, curr.TelAccount)))
                        {
                            return ResponseUtil<TSMUserLoginResult>.FailResult(null, "登录失败，输入的验证码不一致");
                        }
                    }
                }
                #endregion
                //验证密码
                var currPwd = EncryptUtil.DeAESbyKey(dbModel.Passwd, Encoding.UTF8, dbModel.Salt);
                if (string.Equals(curr.Passwd, currPwd))
                {

                    var dbUserInfo = await _db.Instance.Queryable<TSMUserInfoDbModel>()
                    .Where(p => p.ID == dbModel.UserInfoId)
                    .FirstAsync();

                    var rolesDbModel = _db.Instance.Queryable<TSMRoleUserRelationDbModel, TSMRolesDbModel>(
                                   (t1, t2) => new object[]
                                   {
                                    JoinType.Left,  t1.RoleId== t1.Id

                                   }).Where((t1, t2) => t1.UserId == dbModel.ID).Select((t1, t2) => t2).First();

                    var palyloads = new Dictionary<string, object>
                    {
                        { "UserID", dbModel.ID },
                        { "ID",Guid.NewGuid().ToString()},
                        { "CompanyID",dbModel.CompanyId==null?0:dbModel.CompanyId.Value},
                        { "UserName",dbModel.AccountName}

                    };
                    var token = TokenManager.CreateTokenByHandler(palyloads, 60 * 24);

                    try
                    {
                        #region 缓存当前用户的个人信息到redis
                        var redis = CacheFactory.Instance(CacheType.Redis);
                        string key = string.Format(CacheKeyString.UserAccount, dbModel.ID);
                        if (redis.ContainsKey(key))
                        {
                            redis.RemoveKey(key);
                        }
                        SMUserInfo sMUserInfo = new SMUserInfo();
                        sMUserInfo.UserID = dbModel.ID;
                        sMUserInfo.CompanyId = dbModel.CompanyId;
                        sMUserInfo.EmailAccount = dbModel.EmailAccount;
                        sMUserInfo.TelAccount = dbModel.TelAccount;
                        sMUserInfo.AccountName = dbModel.AccountName;
                        sMUserInfo.RealName = dbUserInfo.RealName;
                        sMUserInfo.RoleName = rolesDbModel?.RoleName;
                        redis.AddKey<SMUserInfo>(key, sMUserInfo, 60 * 60 * 24);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                    }
                    tSMUserLoginResult.Token = token;
                    tSMUserLoginResult.IsHavaCompany = dbModel.CompanyId == null ? false : true;
                    #region 登录成功删除【登录失败记录次数】
                    var Successredis = CacheFactory.Instance(CacheType.Redis);
                    if (Successredis.ContainsKey(string.Format(CacheKeyString.LoginTimes, curr.TelAccount)))
                    {
                        Successredis.RemoveKey(string.Format(CacheKeyString.LoginTimes, curr.TelAccount));
                    }
                    #endregion
                    //返回验证成功信息
                    return ResponseUtil<TSMUserLoginResult>
                   .SuccessResult(tSMUserLoginResult);
                }
                #region 登录失败记录次数
                var redisError = CacheFactory.Instance(CacheType.Redis);
                string keyError = string.Format(CacheKeyString.LoginTimes, curr.TelAccount);
                string RedisCode = redisError.GetValueByKey<string>(keyError);
                redisError.AddOrUpdateKey<string>(keyError, RedisCode == "" ? "1" : (Convert.ToInt32(RedisCode) + 1).ToString(), 3600);
                #endregion
                //返回密码验证失败的错误信息
                return ResponseUtil<TSMUserLoginResult>.FailResult(null, "登录失败，密码错误");
            }
            catch (Exception ex)
            {
                //返回异常信息
                return ResponseUtil<TSMUserLoginResult>
                       .FailResult(null, ex.Message);
            }
        }

        /// <summary>
        /// 根据用户ID获取账户信息
        /// </summary>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public ResponseObject<int, UserInfo> GetInfo(int iUserId)
        {
            try
            {
                UserInfo userModel = new UserInfo();
                SMUserInfo sMUserInfo = SMCurentUserManager.GetCurentUserID(iUserId, _db.Instance);
                if (sMUserInfo == null)
                    return ResponseUtil<int, UserInfo>.FailResult(new RequestObject<int>(), null, "未找到该用户信息");

                userModel.Roles = sMUserInfo.RoleName;
                userModel.Name = sMUserInfo.AccountName;

                RequestObject<TRolePermissionsModel> roleQuery = new RequestObject<TRolePermissionsModel>() { IsPaging = false, QueryConditions = new List<QueryCondition>() };
                roleQuery.QueryConditions.Add(new QueryCondition() { Column = "roleID", Content = userModel.RoleId.ToString(), Condition = ConditionEnum.Equal });
                userModel.Permissions = LoadMenuByRoles(sMUserInfo.UserID, sMUserInfo.CompanyId.Value);

                var company = _db.Instance.Queryable<TSMCompanyDbModel, TTenantsModel>(

                              (t1, t2) => new object[]
                              {
                                    JoinType.Left,  t1.CompanyInfoId== t2.ID

                              }).Where((t1, t2) => t1.ID == sMUserInfo.CompanyId).Select((t1, t2) => new { t1.CompanyName, t2.TenantLogo, t2.TenantEngName }).First();

                if (company != null)
                {
                    userModel.CompanyID = sMUserInfo.CompanyId.Value;
                    userModel.TenantLogo = company.TenantLogo;
                    userModel.CompanyName = company.CompanyName;
                    userModel.TenantEngName = company.TenantEngName;
                }

                var userInfo = _db.Instance.Queryable<TSMUserAccountDbModel, TSMUserInfoDbModel>((t, t1) => new object[] {
                    JoinType.Inner,t.UserInfoId==t1.ID
                }).Where((t, t1) => t.ID == iUserId).Select((t, t1) => new { main = t, deatail = t1 }).First();

                userModel.Avatar = userInfo.deatail.HeadPicPath;



                return ResponseUtil<int, UserInfo>.SuccessResult(new RequestObject<int>(), userModel);

            }
            catch (Exception ex)
            {
                return ResponseUtil<int, UserInfo>.FailResult(new RequestObject<int>(), null, $"获取权限发生异常{Environment.NewLine} {ex.Message}");
            }
        }

        private List<MenuViewModel> LoadMenuByRoles(int userID, int CompanyID)
        {
            try
            {

                if (_db.Instance.Queryable<TSMCompanyDbModel>().Any(p => p.ID == CompanyID && p.AdminId == userID))//当前用户为管理员
                {
                    List<TButtonsModel> buttonsModels = _db.Instance.Queryable<TButtonsModel>().ToList();

                    var menuDicList = _db.Instance.Queryable<TSMCompanyDbModel, TTenantPermissionsModel, TPMMenusDbModel>((t2, t3, t4) => new object[]
                                     {
                                       JoinType.Inner, t2.ID==t3.TenantId,
                                       JoinType.Inner,t4.Id==t3.MenuId
                                     }).Where((t2, t3, t4) => t2.ID == CompanyID).Select((t2, t3, t4) => new { perission = t3, Menu = t4 }).ToList();


                    var menuDicbuttons = menuDicList.ToDictionary(p => p.perission.MenuId, p => p.perission.ButtonIds);

                    List<int> menuIDList = menuDicbuttons.Select(p => p.Key).ToList();

                    List<int> allMenuIDS = new List<int>();

                    foreach (var item in menuDicList)
                    {
                        var ids = item.Menu.LogicPath.Split('.').Select(p => Convert.ToInt32(p)).ToList();
                        allMenuIDS.AddRange(ids);
                    }

                    allMenuIDS = allMenuIDS.Distinct().ToList();

                    var allDisplayNodes = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => allMenuIDS.Contains(p.Id) && p.Status == true).ToList(); //所有需要展示的菜单

                    var data = GetMenuTree(allDisplayNodes, menuDicbuttons, buttonsModels, menuDicbuttons, -1);

                    return data;
                }
                else
                {
                    List<TButtonsModel> buttonsModels = _db.Instance.Queryable<TButtonsModel>().ToList();

                    var menuDicbuttons = _db.Instance.Queryable<TSMCompanyDbModel, TTenantPermissionsModel>((t2, t3) => new object[]
                                     {
                                    JoinType.Inner, t2.ID==t3.TenantId
                                     }).Where((t2, t3) => t2.ID == CompanyID).Select((t2, t3) => t3).ToList().ToDictionary(p => p.MenuId, p => p.ButtonIds);

                    List<int> menuIDList = menuDicbuttons.Select(p => p.Key).ToList();


                    RefAsync<int> totalNumber = -1;//总记录数
                    var query = _db.Instance.Queryable<TSMRoleUserRelationDbModel, TSMRolePermissionsDbModel, TSMRolesDbModel, TPMMenusDbModel>(
                                    (t_0, t, t0, t2) => new object[]
                                    {
                                    JoinType.Inner,  t_0.RoleId== t.RoleId,
                                    JoinType.Inner,  t.RoleId== t0.Id,
                                    JoinType.Inner,  t.MenuId== t2.Id
                                    }).Where((t_0, t, t0, t2) => t_0.UserId == userID && menuIDList.Contains(t2.Id));


                    //获取菜单
                    var memu = query.Select((t_0, t, t0, t2) => new { buttons = t.ButtonIds, memu = t2 }).ToList();

                    var menuDictoray = memu.ToDictionary(p => p.memu.Id, p => p.buttons);


                    List<int> allMenuIDS = new List<int>();

                    foreach (var item in memu)
                    {
                        var ids = item.memu.LogicPath.Split('.').Select(p => Convert.ToInt32(p)).ToList();
                        allMenuIDS.AddRange(ids);
                    }

                    allMenuIDS = allMenuIDS.Distinct().ToList();

                    var allDisplayNodes = _db.Instance.Queryable<TPMMenusDbModel>().Where(p => allMenuIDS.Contains(p.Id) && p.Status == true).ToList(); //所有需要展示的菜单

                    var data = GetMenuTree(allDisplayNodes, menuDictoray, buttonsModels, menuDicbuttons, -1);

                    return data;
                }

            }
            catch (Exception ex)
            {
                return new List<MenuViewModel>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aimData">分配到的所有菜单</param>
        /// <param name="menuButtons">平台管理里面配置的菜单与按钮的对应</param>
        /// <param name="buttonsModels">平台所有的按钮</param>
        /// <param name="allbuttonsModels">分配的所有菜单与按钮</param>
        /// <param name="pid">上级ID</param>
        /// <returns></returns>
        private List<MenuViewModel> GetMenuTree(List<TPMMenusDbModel> aimData, Dictionary<int, string> menuButtons, List<TButtonsModel> buttonsModels, Dictionary<int, string> allbuttonsModels, int pid)
        {
            List<MenuViewModel> tree = new List<MenuViewModel>();
            var children = aimData.Where(p => p.ParentID == pid).ToList();
            if (children.Count > 0)
            {
                foreach (var item in aimData.Where(p => p.ParentID == pid).OrderBy(p => p.Seq))
                {
                    string code = RandCodeCreate.CreateCodeByID(item.Id);

                    MenuViewModel node = new MenuViewModel();
                    node.path = "/" + code;
                    node.component = item.IsMenu ? item.MenuPath : "Layout";
                    node.meta = new MenuNodeViewModel() { icon = item.MenuIcon, title = item.MenuName };
                    node.alwaysShow = item.IsMenu ? false : true;
                    node.name = node.component.Replace("/", "").Replace(".", "");

                    if (menuButtons.Keys.Any(p => p == item.Id))
                    {
                        if (!string.IsNullOrEmpty(menuButtons[item.Id]))
                        {
                            List<int> idsALL = allbuttonsModels[item.Id].Split(",").Select(p => Convert.ToInt32(p)).ToList();

                            List<int> ids = menuButtons[item.Id].Split(",").Select(p => Convert.ToInt32(p)).ToList(); //交集

                            var newids = ids.Intersect(idsALL).ToList();
                            node.buttons = buttonsModels.Where(p => newids.Contains(p.Id)).ToList();

                        }
                    }

                    node.children = GetMenuTree(aimData, menuButtons, buttonsModels, allbuttonsModels, item.Id);

                    tree.Add(node);
                }
            }
            return tree;
        }
    }
}
