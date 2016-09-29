// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : AccessFilterBuilder.cs
//          description :
//  
//          created by 李文强 at  2016/09/29 10:49
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Web;

namespace Magicodes.Mvc.AccessFilter
{
    public class AccessFilterBuilder
    {
        private Action<AccessFilter, HttpContextBase> OnAccessLoging { get; set; }
        /// <summary>
        /// 包含的前缀
        /// </summary>
        private string[] AccesssUrlPrefixs { get; set; }
        /// <summary>
        /// 排除的前缀
        /// </summary>
        private string[] ExcludeUrlPrefixs { get; set; }

        /// <summary>
        ///     创建实例
        /// </summary>
        /// <returns></returns>
        public static AccessFilterBuilder Create()
        {
            return new AccessFilterBuilder();
        }

        /// <summary>
        ///     添加访问日志数据处理逻辑
        /// </summary>
        /// <returns></returns>
        public AccessFilterBuilder UsingAccessDataAction(Action<AccessFilter, HttpContextBase> accessAction)
        {
            OnAccessLoging = accessAction;
            return this;
        }

        /// <summary>
        ///     只有拥有该前缀的访问才会记录
        /// </summary>
        /// <param name="urlPrefixs"></param>
        /// <returns></returns>
        public AccessFilterBuilder WithIncludeUrlPrefixs(params string[] urlPrefixs)
        {
            AccesssUrlPrefixs = urlPrefixs;
            return this;
        }

        /// <summary>
        /// 排除的前缀
        /// </summary>
        /// <param name="urlPrefixs"></param>
        /// <returns></returns>
        public AccessFilterBuilder WithExcludeUrlPrefixs(params string[] urlPrefixs)
        {
            ExcludeUrlPrefixs = urlPrefixs;
            return this;
        }

        /// <summary>
        ///     执行
        /// </summary>
        public void Build()
        {
            AccessFilter.OnAccessLoging = OnAccessLoging;
            AccessFilter.AccessUrlPrefixs = AccesssUrlPrefixs;
            AccessFilter.ExcludeUrlPrefixs = ExcludeUrlPrefixs;
        }
    }
}