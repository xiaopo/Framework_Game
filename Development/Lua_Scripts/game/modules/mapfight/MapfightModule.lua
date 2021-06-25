local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local MapfightModule = class(base)

require("game.modules.mapfight.MapftPath")



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
	
	return EventDefine.ENTER_GAME_MAP
end

-- 此模块需要注册的视图
function MapfightModule:get_views()

end


-- 响应的网络事件
function MapfightModule:on_netEvent(cmd,data)
end

-- 响应的本地事件
function MapfightModule:on_localEvent(cmd,data)
	
	if cmd == EventDefine.ENTER_GAME_MAP then
		--启动
		doLuamt("MapfightProgram"):Launch()
	end
	
end

return MapfightModule