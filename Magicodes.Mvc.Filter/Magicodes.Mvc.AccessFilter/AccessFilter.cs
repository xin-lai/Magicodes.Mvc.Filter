// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : AccessFilter.cs
//          description :
//  
//          created by 李文强 at  2016/10/06 11:52
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;

namespace Magicodes.Mvc.AccessFilter
{
    /// <summary>
    /// 访问筛选器
    /// </summary>
    public class AccessFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        private Stopwatch _currentStopwatch;
        internal static Action<AccessFilter, HttpContextBase> OnAccessAction { get; set; }

        internal static Action<AccessFilter, AuthorizationContext> OnAuthorizationAction { get; set; }

        /// <summary>
        ///     Action请求数据
        /// </summary>
        public string ActionData { get; set; }

        /// <summary>
        ///     请求路径
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        ///     客户端IP
        /// </summary>
        public string ClientIpAddress { get; set; }

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
        /// <summary>
        /// Action名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        ///     允许记录的Url前缀
        /// </summary>
        internal static string[] AccessUrlPrefixs { get; set; }

        /// <summary>
        ///     排除的前缀
        /// </summary>
        internal static string[] ExcludeUrlPrefixs { get; set; }
        /// <summary>
        /// 是否记录访问日志
        /// </summary>
        internal bool EnableAccessLog { get; set; }

        /// <summary>
        ///     HTTP请求方法
        /// </summary>
        public string HttpMethod { get; set; }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _currentStopwatch = Stopwatch.StartNew();

            if (filterContext.HttpContext.Request.Url != null)
                RequestUrl = filterContext.HttpContext.Request.Url.ToString();
            if (RequestUrl != null)
            {
                EnableAccessLog = true;
            }
            if (EnableAccessLog && (ExcludeUrlPrefixs != null) && (ExcludeUrlPrefixs.Length > 0))
            {
                foreach (var prefix in ExcludeUrlPrefixs)
                    if (RequestUrl.ToLower().StartsWith(prefix.ToLower()))
                    {
                        EnableAccessLog = false;
                        break;
                    }
            }
            if (EnableAccessLog && (AccessUrlPrefixs != null) && (AccessUrlPrefixs.Length > 0))
            {
                foreach (var prefix in AccessUrlPrefixs)
                    if (RequestUrl.ToLower().StartsWith(prefix.ToLower()))
                    {
                        EnableAccessLog = true;
                        break;
                    }
            }
            if (EnableAccessLog)
            {
                Action = filterContext.ActionDescriptor.ActionName;
                HttpMethod = filterContext.HttpContext.Request.HttpMethod;
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                ClientIpAddress = filterContext.HttpContext.GetClientIpAddress();
                BrowserInfo = filterContext.HttpContext.GetBrowserInfo();
            }
            base.OnActionExecuting(filterContext);
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _currentStopwatch.Stop();
            if (EnableAccessLog)
            {
                ExecutionDuration = _currentStopwatch.ElapsedMilliseconds;
                ContentLength = filterContext.HttpContext.Request.ContentLength;
                base.OnActionExecuted(filterContext);
                OnAccessAction?.Invoke(this, filterContext.HttpContext);
            }
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 权限验证检查
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            OnAuthorizationAction?.Invoke(this, filterContext);
        }
    }
}