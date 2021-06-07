local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local MainuiModule = class(base)

-- 模块注册
function MainuiModule:on_register()
	base.on_register(self)
	MDefine.cache.mainui = 'game.modules.mainui.MainuiData'
	MDefine.cfg.mainui = 'game.modules.mainui.MainuiConfig'
	MDefine.proxy.mainui = 'game.modules.mainui.MainuiProxy'
end

-- 注册的网络事件
function MainuiModule:get_netEvents()
end


-- 注册的本地事件
function MainuiModule:get_localEvents()
end

-- 此模块需要注册的视图
function MainuiModule:get_views()
	return 'game.modules.mainui.views.MainuiView'
end


-- 响应的网络事件
function MainuiModule:on_netEvent(cmd,data)
end

-- 响应的本地事件
function MainuiModule:on_localEvent(cmd,data)
end

return MainuiModule