local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local MapfightModule = class(base)

-- 模块注册
function MapfightModule:on_register()
	base.on_register(self)
	MDefine.cache.mapfight = 'game.modules.mapfight.MapfightData'
	MDefine.cfg.mapfight = 'game.modules.mapfight.MapfightConfig'
	MDefine.proxy.mapfight = 'game.modules.mapfight.MapfightProxy'
end

-- 注册的网络事件
function MapfightModule:get_netEvents()
end


-- 注册的本地事件
function MapfightModule:get_localEvents()
end

-- 此模块需要注册的视图
function MapfightModule:get_views()
	return 'game.modules.mapfight.views.MapfightView'
end


-- 响应的网络事件
function MapfightModule:on_netEvent(cmd,data)
end

-- 响应的本地事件
function MapfightModule:on_localEvent(cmd,data)
end

return MapfightModule