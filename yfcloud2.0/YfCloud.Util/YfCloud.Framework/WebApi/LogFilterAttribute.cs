using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using YfCloud.Framework.Log;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.AutoMapper;

namespace YfCloud.Framework.WebApi
{
    public class LogFilterAttribute : ActionFilterAttribute,IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //
            Task.Factory.StartNew(() => {

                try
                {
                    string method = context.HttpContext.Request.Method.ToUpper();

                    if (method == "GET") //不记录
                    {
                        return;
                    }

                    #region 检查是否忽略日志

                    ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

                    if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(SkipLogAttribute), true).Length > 0) //
                    {
                        return;
                    }
                    else if (controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(SkipLogAttribute), true).Length > 0)
                    {
                        return;
                    }

                    #endregion

                    var resultObj = context.Result.GetType().GetProperty("Value").GetValue(context.Result, null);

                    string code = resultObj.GetType().GetProperty("Code").GetValue(resultObj, null).ToString();

                    if (code != "0")
                    {
                        return;
                    }

                    string ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                    var heads = context.HttpContext.Request.Headers;

                    if (method == "POST") //新增
                    {
                        LogModelBase postData= resultObj.GetType().GetProperty("PostData").GetValue(resultObj, null) as LogModelBase;
                        List<LogModelBase> postDataList = resultObj.GetType().GetProperty("PostDataList").GetValue(resultObj, null) as List<LogModelBase>;

                        if (postData != null)
                        {
                            List<LogModelBase> pList = new List<LogModelBase>() { postData };
                            OperateLogManager.AddOrDeleteLog(pList, heads, "123", ipAddress, OpetateEnum.Add);
                        }
                        if (postDataList != null&& postDataList.Count()>0)
                        {
                            OperateLogManager.AddOrDeleteLog(postDataList, heads, "123", ipAddress, OpetateEnum.Add);
                        }
                    }
                    else if (method == "PUT")//修改
                    {

                        var  postData = resultObj.GetType().GetProperty("PostData").GetValue(resultObj, null)  ;
                        var postDataEdit = resultObj.GetType().GetProperty("PostDataEdit").GetValue(resultObj, null);

                        if (postData != null && postDataEdit != null)
                        {
                            string changeMsg = ReflexCompare.CompareWith(postData, postDataEdit, null);
                        }
                        else
                        {
                            var postDataList = resultObj.GetType().GetProperty("PostDataList").GetValue(resultObj, null);
                            var postDataEditList = resultObj.GetType().GetProperty("PostDataEditList").GetValue(resultObj, null);
                        }

                        

                    }
                    else if (method == "DELETE")//删除
                    {
                        LogModelBase postData = resultObj.GetType().GetProperty("PostData").GetValue(resultObj, null) as LogModelBase;
                        List<LogModelBase> postDataList = resultObj.GetType().GetProperty("PostDataList").GetValue(resultObj, null) as List<LogModelBase>;

                        if (postData != null)
                        {
                            List<LogModelBase> pList = new List<LogModelBase>() { postData };
                            OperateLogManager.AddOrDeleteLog(pList, heads, "123", ipAddress, OpetateEnum.Delete);
                        }
                        if (postDataList != null && postDataList.Count() > 0)
                        {
                            OperateLogManager.AddOrDeleteLog(postDataList, heads, "123", ipAddress, OpetateEnum.Delete);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }


    }
}
