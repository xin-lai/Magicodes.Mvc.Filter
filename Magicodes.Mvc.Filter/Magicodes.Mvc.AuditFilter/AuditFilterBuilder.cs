// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : AuditFilterBuilder.cs
//          description :
//  
//          created by 李文强 at  2016/09/28 10:39
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Web;

namespace Magicodes.Mvc.AuditFilter
{
    public class AuditFilterBuilder
    {
        private Action<AuditFilter, HttpContextBase> OnAuditLoging { get; set; }

        /// <summary>
        ///     创建实例
        /// </summary>
        /// <returns></returns>
        public static AuditFilterBuilder Create()
        {
            return new AuditFilterBuilder();
        }

        /// <summary>
        ///     添加审计日志数据处理逻辑
        /// </summary>
        /// <param name="auditAction"></param>
        /// <returns></returns>
        public AuditFilterBuilder UsingAuditDataAction(Action<AuditFilter, HttpContextBase> auditAction)
        {
            OnAuditLoging = auditAction;
            return this;
        }

        /// <summary>
        ///     执行
        /// </summary>
        public void Build()
        {
            Configs.OnAuditLoging = OnAuditLoging;
        }
    }
}