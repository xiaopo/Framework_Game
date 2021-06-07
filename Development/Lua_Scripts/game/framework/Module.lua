local timerfunc 	= require('game.framework.funcs.Timerfunc')

local Module = class()
Module.add_timer 	 = timerfunc.add_timer
Module.del_timer 	 = timerfunc.del_timer
Module.clear_timer   = timerfunc.clear_timer

-- 模块注册
function Module:on_register()end

-- 模块卸载
function Module:on_unregister()self:clear_timer()end

-- 注册的网络事件
function Module:get_netEvents()end

-- 响应的网络事件
function Module:on_netEvent(cmd,data)end

-- 注册的本地事件
function Module:get_localEvents()end

-- 响应的本地事件
function Module:on_localEvent(cmd,data)end

-- 此模块需要注册的视图
function Module:get_views()end

-- 是否可以打开视图 若返回nil 或返回 true 则表示此视图可以打开
function Module:is_canOpenView(viewName,data)end

-- 返回登陆
--isReConnect 断线重新
function Module:on_relogin(isReConnect)end

return Module

