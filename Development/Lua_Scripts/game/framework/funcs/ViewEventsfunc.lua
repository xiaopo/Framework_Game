--View 界面提供统一的事件注册接口



local function AddListener(self,button,func)
	if not self._events then self._events = {} end
	
	local list = self._events[button]
	if(not list)then 
		list = {} 
		self._events[button] = list 
	end

	table.insert(list,func)
	
	button:AddClickEvent(func)
	
end

local function RemoveListener(self,button,func)
	if not self._events then return end

	local list = self._events[button]
	if not list then return end
	
	for i,fun in ipairs(list) do
		if fun == func then
			table.remove(list,i)
			break
		end
	end
	
	button:RemoveClickEvent(func)
end

--全部解除
local function UnbundAllEvent(self)
	if not self._events then return end
	
	self.isUnbund = true
	local list = self._events[button]
	if list then
		for i,func in ipairs(list) do
			button:RemoveClickEvent(func)
		end
	end
	
end

--全部重绑
local function ReBundAllEvent(self)
	if not self._events or  not self.isUnbund then return end

	self.isUnbund = false
	
	local list = self._events[button]
	if list then
		for i,func in ipairs(list) do
			button:AddClickEvent(func)
		end
	end
	
end

return
{
	AddListener = AddListener,
	RemoveListener = RemoveListener,
	UnbundAllEvent = UnbundAllEvent,
	ReBundAllEvent = ReBundAllEvent
}