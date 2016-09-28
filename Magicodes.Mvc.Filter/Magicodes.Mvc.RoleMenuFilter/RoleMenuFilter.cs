// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : RoleMenuFilter.cs
//          description :
//  
//          created by 李文强 at  2016/09/28 11:45
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Magicodes.Mvc.RoleMenuFilter
{
    public class RoleMenuFilter : ActionFilterAttribute
    {
        /// <summary>
        ///     检索出的RoleMenuFilter实例集合
        /// </summary>
        internal static readonly List<RoleMenuFilter> RoleMenuList = new List<RoleMenuFilter>();

        /// <summary>
        ///     是否启用角色控制
        /// </summary>
        internal static bool EnableRoleControl = false;

        /// <summary>
        ///     角色菜单控制和注册
        /// </summary>
        /// <param name="title">菜单名称</param>
        /// <param name="id">菜单Guid</param>
        /// <param name="orderNo">菜单排序号</param>
        /// <param name="parentId">父级菜单Id</param>
        /// <param name="url">路径</param>
        /// <param name="iconCls">图标</param>
        /// <param name="roleNames">角色名称，多个请用逗号分隔</param>
        /// <param name="tag">标记</param>
        public RoleMenuFilter(string title, string id, string roleNames, int orderNo = default(int),
            string parentId = null, string url = null, string iconCls = null,
            string tag = null)
        {
            Title = title;
            Id = Guid.Parse(id);
            OrderNo = orderNo == default(int) ? (int?) null : orderNo;
            ParentId = string.IsNullOrWhiteSpace(parentId) ? (Guid?) null : Guid.Parse(parentId);
            Url = url;
            IconCls = iconCls;
            RoleNames = roleNames;
            Tag = tag;
        }

        /// <summary>
        ///     菜单名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     菜单Guid
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     菜单排序号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        ///     父级菜单Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        ///     路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        public string IconCls { get; set; }

        /// <summary>
        ///     角色名称，多个请用逗号分隔
        /// </summary>
        public string RoleNames { get; set; }

        /// <summary>
        ///     标记
        /// </summary>
        public string Tag { get; set; }
    }
}