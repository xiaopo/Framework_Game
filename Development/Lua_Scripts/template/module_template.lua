local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local ${NAME}Module = class(base)

-- 模块注册
function ${NAME}Module:on_register()
	base.on_register(self)
	MDefine.cache.${NAME_L} = 'game.modules.${NAME_L}.${NAME}Data'
	MDefine.cfg.${NAME_L} = 'game.modules.${NAME_L}.${NAME}Config'
	MDefine.proxy.${NAME_L} = 'game.modules.${NAME_L}.${NAME}Proxy'
end

-- 注册的网络事件
function ${NAME}Module:get_netEvents()
end


-- 注册的本地事件
function ${NAME}Module:get_localEvents()
end

-- 此模块需要注册的视图
function ${NAME}Module:get_views()
	return 'game.modules.${NAME_L}.views.${NAME}View'
end


-- 响应的网络事件
function ${NAME}Module:on_netEvent(cmd,data)
end

-- 响应的本地事件
function ${NAME}Module:on_localEvent(cmd,data)
end

return ${NAME}Module