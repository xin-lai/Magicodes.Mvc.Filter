// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : Extensions.cs
//          description :
//  
//          created by 李文强 at  2016/09/29 10:49
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Magicodes.Mvc.AccessFilter
{
    internal static class Extensions
    {
        /// <summary>
        ///     获取浏览器信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        internal static string GetBrowserInfo(this HttpContextBase httpContext)
        {
            return httpContext.Request.Browser.Browser + " / " +
                   httpContext.Request.Browser.Version + " / " +
                   httpContext.Request.Browser.Platform;
        }

        /// <summary>
        ///     获取客户端IP信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        internal static string GetClientIpAddress(this HttpContextBase httpContext)
        {
            var clientIp = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                           httpContext.Request.ServerVariables["REMOTE_ADDR"];

            try
            {
                foreach (var hostAddress in Dns.GetHostAddresses(clientIp))
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                        return hostAddress.ToString();
                foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                        return hostAddress.ToString();
            }
            catch (Exception)
            {
            }

            return clientIp;
        }

        /// <summary>
        ///     获取电脑名称
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        internal static string GetComputerName(this HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsLocal)
                return null;

            try
            {
                var clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return Dns.GetHostEntry(IPAddress.Parse(clientIp)).HostName;
            }
            catch
            {
                return null;
            }
        }
    }
}