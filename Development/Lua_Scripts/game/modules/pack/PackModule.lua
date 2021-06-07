local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local PackModule = class(base)

-- 模块注册
function PackModule:on_register()
	base.on_register(self)
	MDefine.cache.pack = 'game.modules.pack.PackData'
	MDefine.cfg.pack = 'game.modules.pack.PackConfig'
	MDefine.proxy.pack = 'game.modules.pack.PackProxy'
end

-- 注册的网络事件
function PackModule:get_netEvents()
end


-- 注册的本地事件
function PackModule:get_localEvents()
end

-- 此模块需要注册的视图
function PackModule:get_views()
	return 'game.modules.pack.views.PackView'
end


-- 响应的网络事件
function PackModule:on_netEvent(cmd,data)
end

-- 响应的本地事件
function PackModule:on_localEvent(cmd,data)
end

return PackModule