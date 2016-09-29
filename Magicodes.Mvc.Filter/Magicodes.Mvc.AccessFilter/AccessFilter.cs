// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : AccessFilter.cs
//          description :
//  
//          created by 李文强 at  2016/09/29 10:49
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
    public class AccessFilter : ActionFilterAttribute
    {
        private Stopwatch _currentStopwatch;
        internal static Action<AccessFilter, HttpContextBase> OnAccessLoging { get; set; }

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

        public string Action { get; set; }

        public string Controller { get; set; }

        /// <summary>
        ///     允许记录的Url前缀
        /// </summary>
        internal static string[] AccessUrlPrefixs { get; set; }

        internal bool EnableAccessLog { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _currentStopwatch = Stopwatch.StartNew();

            if (filterContext.HttpContext.Request.Url != null)
                RequestUrl = filterContext.HttpContext.Request.Url.ToString();
            if ((AccessUrlPrefixs != null) && (AccessUrlPrefixs.Length > 0) && (RequestUrl != null))
                foreach (var prefix in AccessUrlPrefixs)
                    if (RequestUrl.Contains(prefix))
                    {
                        EnableAccessLog = true;
                        break;
                    }
            else
                EnableAccessLog = true;
            if (EnableAccessLog)
            {
                Action = filterContext.ActionDescriptor.ActionName;
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                ClientIpAddress = filterContext.HttpContext.GetClientIpAddress();
                BrowserInfo = filterContext.HttpContext.GetBrowserInfo();
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _currentStopwatch.Stop();
            if (EnableAccessLog)
            {
                ExecutionDuration = _currentStopwatch.ElapsedMilliseconds;
                ContentLength = filterContext.HttpContext.Request.ContentLength;
                base.OnActionExecuted(filterContext);
                OnAccessLoging?.Invoke(this, filterContext.HttpContext);
            }
            base.OnActionExecuted(filterContext);
        }
    }
}