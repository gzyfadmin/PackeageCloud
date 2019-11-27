using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Caches;
using YfCloud.Framework.WebApi;
using YfCloud.Models;
using YfCloud.Utilities.NetOperater;

namespace YfCloud.Framework.CacheManager
{
    /// <summary>
    /// 基础数据缓存
    /// </summary>
    public class BasicCacheGet
    {
        /// <summary>
        /// 数据字典缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static List<TBMDictionaryCacheModel> GetDic(CurrentUser currentUser)
        {
            string key = string.Format(CacheKeyString.TBMDictionary, currentUser.CompanyID);

            List<TBMDictionaryCacheModel> result = new List<TBMDictionaryCacheModel>();

             var redis = CacheFactory.Instance(CacheType.Redis);

            if (redis.ContainsKey(key))
            {
                result= redis.GetValueByKey<List<TBMDictionaryCacheModel>>(key);
            }
            else
            {
                bool postresult = RestfulApiClient.Post<bool>(RestfulApiClient.CacheUrl, "api/Cache/PostDictionary", BuildToken(currentUser),null);

                if (postresult)
                {
                    result = redis.GetValueByKey<List<TBMDictionaryCacheModel>>(key);
                }
            }

            return result;
        }

        public static List<ChinaAreaRecord> GetChinaArea(CurrentUser currentUser)
        {
            string key = CacheKeyString.ChinaArea;
            List<ChinaAreaRecord> result = new List<ChinaAreaRecord>();

            var redis = CacheFactory.Instance(CacheType.Redis);

            if (redis.ContainsKey(key))
            {
                result = redis.GetValueByKey<List<ChinaAreaRecord>>(key);
            }
            else
            {

                bool postresult = RestfulApiClient.Post<bool>(RestfulApiClient.CacheUrl, "api/Cache/PostArea", BuildToken(currentUser), null);

                if (postresult)
                {
                    result = redis.GetValueByKey<List<ChinaAreaRecord>>(key);
                }
            }

            return result;

        }

        public static List<TBMDomainQueryModel> GetDomains(CurrentUser currentUser)
        {
            string key = CacheKeyString.Domain;
            List<TBMDomainQueryModel> result = new List<TBMDomainQueryModel>();

            var redis = CacheFactory.Instance(CacheType.Redis);

            if (redis.ContainsKey(key))
            {
                result = redis.GetValueByKey<List<TBMDomainQueryModel>>(key);
            }
            else
            {

                bool postresult = RestfulApiClient.Post<bool>(RestfulApiClient.CacheUrl, "api/Cache/PostDomain", BuildToken(currentUser), null);

                if (postresult)
                {
                    result = redis.GetValueByKey<List<TBMDomainQueryModel>>(key);
                }
            }

            return result;

        }


        public static List<TBMMaterialFileCacheModel> GetMaterial(CurrentUser currentUser)
        {
            string key = string.Format(CacheKeyString.Material, currentUser.CompanyID);
            List<TBMMaterialFileCacheModel> result = new List<TBMMaterialFileCacheModel>();

            var redis = CacheFactory.Instance(CacheType.Redis);

            if (redis.ContainsKey(key))
            {
                result = redis.GetValueByKey<List<TBMMaterialFileCacheModel>>(key);
            }
            else
            {

                bool postresult = RestfulApiClient.Post<bool>(RestfulApiClient.CacheUrl, "api/Cache/PostMaterial", BuildToken(currentUser), null);

                if (postresult)
                {
                    result = redis.GetValueByKey<List<TBMMaterialFileCacheModel>>(key);
                }
            }

            return result;
        }

        private static Dictionary<string,string> BuildToken(CurrentUser currentUser)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var palyloads = new Dictionary<string, object>
                    {
                        { "UserID", currentUser.UserID },
                        { "ID",Guid.NewGuid().ToString()},
                        { "CompanyID",currentUser.CompanyID},
                        { "UserName",currentUser.UserName}
                    };

            var token = TokenManager.CreateTokenByHandler(palyloads, 60 * 24);

            result.Add(TokenConfig.Instace.TokenKey, token);

            return result;
        }

    }
}
