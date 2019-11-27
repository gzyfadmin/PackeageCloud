using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using YfCloud.Framework.WebApi;
using YfCloud.Models;

namespace YfCloud.Framework
{
    /// <summary>
    /// 权限认证过滤器
    /// </summary>
    public class YFAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 重写认证接口
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length>0) //
            {
                return;
            }
            else if (controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute),true).Length>0)
            {
                return;
            }
            else
            {
                var result = new ResponseObject<string, bool>()
                {
                    TotalNumber = -2,
                    Data = false,
                    Info = "Token无效",
                    Code = -1,
                    IsPaging = false,
                    PageIndex = 0,
                    PageSize = 0,
                    PostData = null,
                    QueryConditions = null,
                    OrderByConditions = null,
                    PostDataList = null
                };

                if (!context.HttpContext.Request.Headers.ContainsKey(TokenConfig.Instace.TokenKey))
                {
                    context.Result = new JsonResult(result);

                    return;
                }
                else
                {
                    string token = context.HttpContext.Request.Headers[TokenConfig.Instace.TokenKey].ToString();
                    if (!TokenManager.Validate(token))
                    {
                        context.Result = new JsonResult(result);

                        return;
                    }
                }

            }
        }
    }
}
