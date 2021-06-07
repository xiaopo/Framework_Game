local ${NAME}Command = class(require( 'game.framework.Command' ))

-- 注册的网络事件
function ${NAME}Command:get_netEvents()
end

-- 注册的本地事件
function ${NAME}Command:get_localEvents()
end

-- 响应的网络事件
function ${NAME}Command:on_netEvent(cmd,data)
end

-- 响应的本地事件
function ${NAME}Command:on_localEvent(cmd,data)
end

return ${NAME}Command