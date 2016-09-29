// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : AuditFilter.cs
//          description :
//  
//          created by 李文强 at  2016/09/28 10:39
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Magicodes.Mvc.AuditFilter
{
    /// <summary>
    ///     审计相关
    /// </summary>
    public class AuditFilter : ActionFilterAttribute, IExceptionFilter
    {
        internal static Action<AuditFilter, HttpContextBase> OnAuditLoging { get; set; }
        private Stopwatch _currentStopwatch;

        public AuditFilter(string title, string code = null, string desc = null)
        {
            Title = title;
            Code = code;
            Remark = desc;
        }

        /// <summary>
        ///     唯一标识，为null则自动设置
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     关键说明
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        ///     Action请求数据
        /// </summary>
        public string ActionData { get; set; }

        /// <summary>
        ///     当前用户Id
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        ///     请求路径
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        ///     客户端IP
        /// </summary>
        public string ClientIpAddress { get; set; }

        /// <summary>
        ///     客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        ///     浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }

        /// <summary>
        ///     执行时间
        /// </summary>
        public long ExecutionDuration { get; set; }

        /// <summary>
        ///     当前请求数据大小
        /// </summary>
        public int ContentLength { get; set; }

        public void OnException(ExceptionContext filterContext)
        {
            Exception = filterContext.Exception;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _currentStopwatch = Stopwatch.StartNew();
            if (filterContext.HttpContext.Request.Url != null)
                RequestUrl = filterContext.HttpContext.Request.Url.ToString();
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                UserId = filterContext.HttpContext.User.Identity.Name;

            var actionData = filterContext.ActionParameters;
            ActionData = JsonConvert.SerializeObject(actionData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _currentStopwatch.Stop();
            ExecutionDuration = _currentStopwatch.ElapsedMilliseconds;
            BrowserInfo = filterContext.HttpContext.GetBrowserInfo();
            ClientName = filterContext.HttpContext.GetComputerName();
            ClientIpAddress = filterContext.HttpContext.GetClientIpAddress();
            if (string.IsNullOrWhiteSpace(Code))
                Code = string.Format("{0}-{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName);
            ContentLength = filterContext.HttpContext.Request.ContentLength;
            if (AuditFilter.OnAuditLoging == null) return;
            AuditFilter.OnAuditLoging.Invoke(this, filterContext.HttpContext);
        }
    }
}