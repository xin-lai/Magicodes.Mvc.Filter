// ======================================================================
//  
//          Copyright (C) 2016-2020 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : RoleMenuFilterBuilder.cs
//          description :
//  
//          created by 李文强 at  2016/09/28 11:42
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub：https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Magicodes.Logger;

namespace Magicodes.Mvc.RoleMenuFilter
{
    public class RoleMenuFilterBuilder
    {
        /// <summary>
        ///     需要初始化的程序集
        /// </summary>
        private readonly List<string> _containAssemblys = new List<string>();

        /// <summary>
        ///     检索的基类，默认基于Controller
        /// </summary>
        private Type _controllerType = typeof(Controller);

        /// <summary>
        ///     初始化函数
        /// </summary>
        private Action<List<RoleMenuFilter>> _menuInitAction;

        /// <summary>
        ///     日志记录器
        /// </summary>
        private LoggerBase Logger { get; set; }

        /// <summary>
        ///     创建实例
        /// </summary>
        /// <returns></returns>
        public static RoleMenuFilterBuilder Create()
        {
            return new RoleMenuFilterBuilder();
        }

        /// <summary>
        ///     启用角色权限控制
        /// </summary>
        /// <returns></returns>
        public RoleMenuFilterBuilder WithRoleControl()
        {
            RoleMenuFilter.EnableRoleControl = true;
            return this;
        }

        /// <summary>
        ///     检索包含的程序集
        /// </summary>
        /// <param name="names">程序集名称</param>
        /// <returns></returns>
        public RoleMenuFilterBuilder WithContainAssemblyName(params string[] names)
        {
            _containAssemblys.AddRange(names);
            return this;
        }

        /// <summary>
        ///     检索的基类
        /// </summary>
        /// <param name="controllerType">控制类型</param>
        /// <returns></returns>
        public RoleMenuFilterBuilder WithControllerType(Type controllerType)
        {
            _controllerType = controllerType;
            return this;
        }

        /// <summary>
        ///     添加日志记录器
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public RoleMenuFilterBuilder WithLogger(LoggerBase logger)
        {
            Logger = logger;
            return this;
        }

        /// <summary>
        ///     是否开启菜单初始化（开启后，将自动根据筛选器设置来创建菜单数据）
        /// </summary>
        /// <returns></returns>
        public RoleMenuFilterBuilder WithMenuInitialization(Action<List<RoleMenuFilter>> menuInitAction)
        {
            _menuInitAction = menuInitAction;
            return this;
        }

        /// <summary>
        ///     执行
        /// </summary>
        public void Build()
        {
            InitRoleMenuList();
            _menuInitAction?.Invoke(RoleMenuFilter.RoleMenuList);
        }

        private void InitRoleMenuList()
        {
            foreach (
                var currentassembly in
                AppDomain.CurrentDomain.GetAssemblies().Where(p => _containAssemblys.Contains(p.GetName().Name)))
                currentassembly.GetTypes()
                    .Where(p => p.IsClass && p.IsSubclassOf(_controllerType))
                    .Each(controllerType =>
                    {
                        try
                        {
                            Logger?.Log(LoggerLevels.Debug, "找到控制器" + controllerType.FullName + "...");
                            var controllerRoleMenuAttr = controllerType.GetCustomAttribute<RoleMenuFilter>();
                            if (controllerRoleMenuAttr != null)
                            {
                                Logger?.Log(LoggerLevels.Debug, "正在加载控制器" + controllerType.FullName + "的角色菜单...");
                                controllerRoleMenuAttr.Controller = controllerType.Name;
                                RoleMenuFilter.RoleMenuList.Add(controllerRoleMenuAttr);
                            }

                            foreach (var action in controllerType.GetMethods().Where(p => p.IsPublic && !p.IsStatic))
                            {
                                Logger?.Log(LoggerLevels.Debug, "找到Action " + action.Name + "...");
                                var roleMenuFilter = action.GetCustomAttribute<RoleMenuFilter>();
                                if (roleMenuFilter == null) continue;
                                //TODO:特性判断
                                roleMenuFilter.Controller = controllerType.Name;
                                roleMenuFilter.Action = action.Name;
                                Logger?.Log(LoggerLevels.Debug, "正在加载Action " + action.Name + " 的角色菜单...");
                                RoleMenuFilter.RoleMenuList.Add(roleMenuFilter);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger?.Log(LoggerLevels.Error,
                                string.Format("创建角色菜单出错。Assembly:{0}，Type:{1}，{2}{3}", currentassembly.FullName,
                                    controllerType.FullName, Environment.NewLine, ex));
                        }
                    });
        }
    }
}