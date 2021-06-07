local Command = class()
-- 注册
function Command:on_register()end

-- 卸载
function Command:on_unregister()end

-- 注册的网络事件
function Command:get_netEvents()end

-- 注册的本地事件
function Command:get_localEvents()end

-- 响应的网络事件
function Command:on_netEvent(cmd,data)end

-- 响应的本地事件
function Command:on_localEvent(cmd,data)end

return Command