using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.BasicSet.Models.TBMDictionary;
using YfCloud.App.Module.BasicSet.Service;
using YfCloud.App.Module.BasicSet.Services;
using YfCloud.App.Module.Warehouse.Models.TBMMaterialFile;
using YfCloud.App.Module.Warehouse.Service;
using YfCloud.Caches;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.BasicSet.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {


        private readonly IChinaAreaService _chinaAreaService;
        private readonly ITBMDictionaryService _iTBMDictionaryService;
        private readonly IDomain _iDomain;
        private readonly ITBMMaterialFileService _iMaterialService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheController(IChinaAreaService chinaAreaService, ITBMDictionaryService iTBMDictionaryService, IDomain iDomain, ITBMMaterialFileService iMaterialService)
        {
            _chinaAreaService = chinaAreaService;
            _iTBMDictionaryService = iTBMDictionaryService;
            iDomain = _iDomain;
            _iMaterialService = iMaterialService;
        }

        #region 数据字典缓存
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public async Task<bool> PostDictionary()
        {
            try
            {
                bool result = true;

                var redis = CacheFactory.Instance(CacheType.Redis);

                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
                string rediskey = string.Format(CacheKeyString.TBMDictionary, currentUser.CompanyID);
                List<TBMDictionaryCacheModel> list = await _iTBMDictionaryService.GetAllDictionary(currentUser.CompanyID);
                result = redis.AddKey<List<TBMDictionaryCacheModel>>(rediskey, list, 60 * 60 * 24);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region 地区缓存

        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public  bool PostArea()
        {
            try
            {
                bool result = true;

                var redis = CacheFactory.Instance(CacheType.Redis);

                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
                string rediskey = CacheKeyString.ChinaArea;
                List<ChinaAreaRecord> list = _chinaAreaService.GetAllDistricts();

                result = redis.AddKey<List<ChinaAreaRecord>>(rediskey, list, 60 * 60 * 24);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region 站点缓存

        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public bool PostDomain()
        {
            try
            {
                bool result = true;

                var redis = CacheFactory.Instance(CacheType.Redis);

                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
                string rediskey = CacheKeyString.Domain;
                List<TBMDomainQueryModel> list = _iDomain.GetTBMDomainQueryModel();
                result = redis.AddKey<List<TBMDomainQueryModel>>(rediskey, list, 60 * 60 * 24);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion

        #region 物料缓存

        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public async Task<bool> PostMaterial()
        {
            try
            {
                bool result = true;

                var redis = CacheFactory.Instance(CacheType.Redis);

                CurrentUser currentUser = TokenManager.GetCurentUserbyToken(Request.Headers);
                string rediskey = string.Format(CacheKeyString.Material, currentUser.CompanyID);

                var data = await _iMaterialService.GetAllAsync(currentUser);


                result = redis.AddKey<List<TBMMaterialFileCacheModel>>(rediskey, data, 60 * 60 * 24);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion
    }
}