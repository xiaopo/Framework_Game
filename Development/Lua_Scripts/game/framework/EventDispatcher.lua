local EventDispatcher = class()

--构造方法
function EventDispatcher:ctor(args)
	self.__eventMap__ = {}
	self.__callWithoutName__ = false--执行回调时是否不带名字参数
	
	if args and args.callWithoutName == true then
		self.__callWithoutName__ = true
	end
end

function EventDispatcher:clear()
	self.__eventMap__ = {}
end

--用于权重排序
local function sortListeners(a, b)
	if a.priority == b.priority then
		return a.index < b.index
	else
		return a.priority > b.priority
	end
end

--[[
添加事件侦听
@name			[string]事件名
@listener		[function]侦听器
@listenerCaller	[Object]侦听函数调用者
@priority		[int]权重，值越大越先被执行，为0时按添加的先后顺序执行(默认为0)
--]]
function EventDispatcher:addEventListener(name, listener, listenerCaller, priority)
	if type(name) ~= "string" and type(name) ~= "number" then
		print(debug.traceback())
		return
	end

	-- assert(type(name) == "string" or type(name) == "number", "Invalid event name of argument 1, need a string or number!")
	
	assert(type(listener) == "function", "Invalid listener function!")

	priority = priority or 0
	assert(type(priority) == "number", "Invalid priority value, need a int!")
	
	if self.__eventMap__[name] == nil then
		self.__eventMap__[name] = {__index__ = 0}
	else
		local newT = {__index__ = self.__eventMap__[name].__index__}
		for k, v in ipairs(self.__eventMap__[name]) do
			table.insert(newT, v)
		end
		self.__eventMap__[name] = newT
	end

	local isExist = false
	local needSort = false
	for k, v in ipairs(self.__eventMap__[name]) do
		if v.listener == listener and v.listenerCaller == listenerCaller then
			isExist = true

			if self.__eventMap__[name].priority ~= priority then
				self.__eventMap__[name].priority = priority

				needSort = true
			end
			break
		end
	end
	
	if not isExist then
		self.__eventMap__[name].__index__ = self.__eventMap__[name].__index__ + 1
		table.insert(self.__eventMap__[name], {listener = listener, listenerCaller = listenerCaller, priority = priority, index=self.__eventMap__[name].__index__})
		needSort = true
	end

	if needSort and #self.__eventMap__[name] > 1 then
		table.sort(self.__eventMap__[name], sortListeners)
	end
end

--[[
移除事件侦听
@name		[string]事件名
@listener	[function]侦听器
--]]
function EventDispatcher:removeEventListener(name, listener,listenerCaller)
	if not name then 
		print("=================================事件未定义==========================================")
		print(debug.traceback())
	end
	assert(type(name) == "string" or type(name) == "number", "Invalid event name of argument 1, need a string or number!")
	assert(type(listener) == "function", "Invalid listener function!")

	if self.__eventMap__[name] ~= nil then
		local newT = nil
		for k, v in ipairs(self.__eventMap__[name]) do
			if v.listener ~= listener or (v.listenerCaller ~= listenerCaller and listenerCaller) then
				if not newT then 
					newT = {__index__ = self.__eventMap__[name].__index__}
				end
				table.insert(newT, v)
			end
		end
		
		self.__eventMap__[name] = newT
	end
end




g_event_call_time = {}
g_event_call_time_len = 100
g_event_call_time_minValue = 0.02
g_event_call_time_enabled = true
local function addCallTime(self,time,listener,listenerCaller,name)
	local dname = self.__callWithoutName__ and 'NetDispatcher' or 'Dispatcher'
	if time >= g_event_call_time_minValue then
		local finfo = debug.getinfo( listener )
		local str = '[type]: '..dname..' [callTime]: '..time..'/ms  [frame]:'..CS.UnityEngine.Time.frameCount..' [source]:'..finfo.source..' [lineNum]:'..finfo.linedefined..' [event]:'..name
		table.insert( g_event_call_time,str )
	end

	if #g_event_call_time > g_event_call_time_len then
		table.remove( g_event_call_time,1 )
	end
end


EVENT_SAFE = not CS.UnityEngine.Debug.isDebugBuild--安全模式
--[[
发布事件
@name		[string]事件名
@...		其它参数
--]]
function EventDispatcher:dispatchEvent(name, ...)
	--assert(type(name) == "string" or type(name) == "number", string.format("Invalid event name of argument 1, need a string or number!(%s)",name))
	local listeners = self.__eventMap__[name]
	if listeners ~= nil then
	   local index = 0
		for k, v in ipairs(listeners) do
		  index = index +1
		  	local stime = os.clock()
			
			if(EVENT_SAFE)then
				 
				--使用安全模式
				if not self.__callWithoutName__ then
					pcall(v.listener,v.listenerCaller, name, ...)
				else
					pcall(v.listener,v.listenerCaller, ...)
				end
				
			else
		
				if not self.__callWithoutName__ then
					v.listener(v.listenerCaller, name, ...)
				else
					v.listener(v.listenerCaller, ...)
				end
			end

			if g_event_call_time_enabled then
				addCallTime(self,os.clock() - stime,v.listener,v.listenerCaller,name)
			end
		end
	end
end

--[[
是否存在该事件侦听
@name	事件名
--]]
function EventDispatcher:hasEventListener(name)
	assert(type(name) == "string" or type(name) == "number", "Invalid event name of argument 1, need a string or number!")
	return self.__eventMap__[name] ~= nil
end

return EventDispatcher