using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.BasicSet.Services;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.BasicSet.Controllers
{

    /// <summary>
    /// 地区
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ChinaAreaController : ControllerBase
    {
        private readonly IChinaAreaService _chinaAreaService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="chinaAreaService"></param>
        public ChinaAreaController(IChinaAreaService chinaAreaService)
        {
            _chinaAreaService = chinaAreaService;
        }


        /// <summary>
        /// 获取所有地区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ResponseObject<List<ChinaAreaChildRecord>> GetAllDistricts()
        {
            List<ChinaAreaChildRecord> result = new List<ChinaAreaChildRecord>();
            try {
                
                result= _chinaAreaService.GetAllTreeDistricts();
              return  ResponseUtil<List<ChinaAreaChildRecord>>.SuccessResult(result, -1);

            }
            catch(Exception ex) {
                return ResponseUtil<List<ChinaAreaChildRecord>>.FailResult(result,ex.Message);
            }

        }

        /// <summary>
        /// 获取某编码下的下级地区
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ResponseObject<List<ChinaAreaRecord>> GetAreasByCode(string cityCode)
        {
            List<ChinaAreaRecord> result = new List<ChinaAreaRecord>();
            try
            {

                result = _chinaAreaService.GetAreas(cityCode);
                return ResponseUtil<List<ChinaAreaRecord>>.SuccessResult(result, -1);

            }
            catch (Exception ex)
            {
                return ResponseUtil<List<ChinaAreaRecord>>.FailResult(result, ex.Message);
            }
        }

        /// <summary>
        /// 获取某Level的地区
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ResponseObject<List<ChinaAreaRecord>> GetDistrictsByLevel(int level)
        {
            List<ChinaAreaRecord> result = new List<ChinaAreaRecord>();
            try
            {

                result = _chinaAreaService.GetDistrictsByLevel(level);
                return ResponseUtil<List<ChinaAreaRecord>>.SuccessResult(result, -1);

            }
            catch (Exception ex)
            {
                return ResponseUtil<List<ChinaAreaRecord>>.FailResult(result, ex.Message);
            }
        }
    }
}