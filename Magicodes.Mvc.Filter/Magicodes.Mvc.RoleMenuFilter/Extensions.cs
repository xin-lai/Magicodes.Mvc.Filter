// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : Extensions.cs
//          description :
//  
//          created by 李文强 at  2016/09/28 11:28
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Collections.Generic;

namespace Magicodes.Mvc.RoleMenuFilter
{
    internal static class Extensions
    {
        /// <summary>
        ///     遍历
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="instance">当前枚举对象</param>
        /// <param name="action">处理函数</param>
        /// <returns>当前集合</returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            foreach (var item in instance)
                action(item);
            return instance;
        }
    }
}