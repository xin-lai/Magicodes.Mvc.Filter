// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : Configs.cs
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
    internal static class Configs
    {
        internal static Action<AuditFilter, HttpContextBase> OnAuditLoging { get; set; }
    }
}