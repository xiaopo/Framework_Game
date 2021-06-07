local EventDispatcher = require( 'game.framework.EventDispatcher' )

--普通事件分发器(一般是视图层事件)
_G.Dispatcher = {}
local _dispatcher = EventDispatcher()

function Dispatcher.addEventListener(name, listener, listenerCaller, priority)
	_dispatcher:addEventListener(name, listener, listenerCaller, priority)
end

function Dispatcher.removeEventListener(name, listener,listenerCaller)
	_dispatcher:removeEventListener(name, listener,listenerCaller)
end

function Dispatcher.dispatchEvent(name, ...)
	_dispatcher:dispatchEvent(name, ...)
end

function Dispatcher.hasEventListener(name)
	_dispatcher:hasEventListener(name)
end

function Dispatcher.clear()
	_dispatcher:clear()
end

--------------------------------------------------------
--网络事件分发器(区分普通事件)
_G.NetDispatcher = {}
local _netDispatcher = EventDispatcher()

function NetDispatcher.addEventListener(name, listener, listenerCaller, priority)
	_netDispatcher:addEventListener(name, listener, listenerCaller, priority)
end

function NetDispatcher.removeEventListener(name, listener,listenerCaller)
	_netDispatcher:removeEventListener(name, listener,listenerCaller)
end

function NetDispatcher.dispatchEvent(name, ...)
	_netDispatcher:dispatchEvent(name, ...)
end

function NetDispatcher.hasEventListener(name)
	_netDispatcher:hasEventListener(name)
end

function NetDispatcher.clear()
	_netDispatcher:clear()
end