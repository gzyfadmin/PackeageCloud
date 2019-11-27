
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using SqlSugar;
//using YfCloud.Attributes;
//using YfCloud.Models;

//namespace YfCloud.App.Module.Platform.Repositories
//{
//    /// <summary>
//    /// DbContext
//    /// </summary>
//    [UseDI(ServiceLifetime.Scoped, typeof(IDbContext))]
//    public class DbContext : IDbContext
//    {
//        private readonly AppConfig _appConfig;
//        private readonly SqlSugarClient _instance;

//        /// <summary>
//        /// 默认构造方法
//        /// </summary>
//        /// <param name="option"></param>
//        public DbContext(IOptions<AppConfig> option)
//        {
//            _appConfig = option.Value;
//            _instance = new SqlSugarClient(new ConnectionConfig
//            {
//                ConnectionString = _appConfig.ConnString,
//                IsAutoCloseConnection = _appConfig.IsAutoCloseConnection,
//                DbType = _appConfig.DbType.ToLower().Equals("mysql") ? DbType.MySql : DbType.MyCat,
//                InitKeyType = InitKeyType.Attribute
//            });
//        }

//        /// <summary>
//        /// SqlSugarClient Instance
//        /// </summary>
//        public override SqlSugarClient Instance
//        {
//            get
//            {
//                return _instance;
//            }
//        }
//    }
//}
