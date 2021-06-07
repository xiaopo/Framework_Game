local timerfunc 		= require('game.framework.funcs.Timerfunc')
local loaderfunc 		= require('game.framework.funcs.Loaderfunc')
local ViewControlsfunc 	= require('game.framework.funcs.ViewControlsfunc')
local ViewEventsfunc	= require('game.framework.funcs.ViewEventsfunc')

local View = class()

View.createAvatar 	= ViewControlsfunc.createAvatar			--self:createAvatar()
View.createTabView 	= ViewControlsfunc.createTabView		--self:createTabView(self.inject.TabView)
View.createListView = ViewControlsfunc.createListView		--self:createListView(self.inject.ListView)
View.AddListener	= ViewEventsfunc.AddListener
View.RemoveListener	= ViewEventsfunc.RemoveListener
View.UnbundAllEvent	= ViewEventsfunc.UnbundAllEvent
View.ReBundAllEvent	= ViewEventsfunc.ReBundAllEvent		

View.add_timer 	 = timerfunc.add_timer
View.del_timer 	 = timerfunc.del_timer
View.clear_timer = timerfunc.clear_timer
-- 
View.load 		 = loaderfunc.load
View.loadView 	 = loaderfunc.load_ui
View.playAudio 	 = function(self,assetName)   end
View.sortOrder   = 0


--全局创建视图方法
function G_FUNC_CREATE_VIEW(viewPath,gameObject)
	local view_class = require(viewPath)
	if not view_class then
		print_err(string.format('View.lua->G_FUNC_CREATE_VIEW()  %s is nil',viewPath))
		return
	end
	return view_class(gameObject)
end

-- 注入自动化
local function createAutoInject(self)
	return setmetatable( {}, 
	{
		__index = function(t,k)
			if self.is_dispose then
				print(string.format( '<color=red>viewName="%s". createAutoInject view is_dispose 该视图已经被销毁! </color> \n%s',self.name,debug.traceback()))
				return
			end

			local value = self:get_component(k)
			-- 2次获取
			if not value or value:Equals(nil) then value = rawget(t,k) end
			
			if not value or value:Equals(nil) then
				if not value then
					print(string.format( '<color=red>viewName="%s". createAutoInject component is nil name="%s"  预置件身上没有此名字的对象! </color> \n%s',self.name,k,debug.traceback()))
				else
					print(string.format( '<color=red>viewName="%s". createAutoInject component is nil name="%s"  空CS对象! value= "%s" </color> \n%s',self.name,k,value,debug.traceback()))
				end
				return
			end
			rawset( t, k, value )
			return value
		end
	})
end


function View:ctor(gameObject)
	assert( gameObject, 'View:ctor. gameObject is nil ' )
	self.cview 		= nil
	self.is_dispose = nil
	self.is_enabled = nil
	self.gameObject = gameObject
	self.transform  = gameObject.transform
	self.inject 	= createAutoInject(self)
	self:on_init()
end



function View:on_init()end
function View:on_enabled()
	self.is_enabled = true 
	ViewControlsfunc.on_enabled(self)
	if self.name then
		Dispatcher.dispatchEvent(EventDefine.VIEW_ONENABLED,self)
	end
end

function View:on_disable()
	if self.name then
		Dispatcher.dispatchEvent(EventDefine.VIEW_DISABLE,self)
	end
	self.is_enabled = nil  ViewControlsfunc.on_disable(self) 
end

function View:get_component(name)
	return self:get_cview():Get(name)
end

function View:get_cview()
	if self.cview == nil then

		if self.contentObject then
			self.cview = self.contentObject:GetComponent(typeof(CS.XGUI.XView))
		elseif self.mainObject then
			self.cview = self.mainObject:GetComponent(typeof(CS.XGUI.XView))
		else
			self.cview = self.gameObject:GetComponent(typeof(CS.XGUI.XView))
		end

		if self.cview then
			self.cview:InitInject(self.inject)
			-- 将主对象和内容对象的 控件注入

			if self.contentObject and self.mainObject then
				local main_cview = self.mainObject:GetComponent(typeof(CS.XGUI.XView))
				if main_cview then
					main_cview:InitInject(self.inject)
				end
			end
		end
	end

	if not self.cview then print("<color=#red>View:get_cview. cview is nil</color>",debug.traceback(  )) end
	return self.cview
end


-- 搜索子组件
-- type 	: 组件类型
-- relative : 组件节点路径
function View:find_component(type,relative)
	local object = self.mainTransform and self.mainTransform:FindComponent(type,relative) or self.transform:FindComponent(type,relative)
	if not object then
		print(string.format( '<color=#red>View:find_component. relative="%s" type="%s" is nil </color>',relative, type,debug.traceback()))
	end 
	return  object
end

function View:set_visible(bool)
	if self.gameObject then
		self.gameObject:SetActive(bool)
	end
end


function View:on_loadComplete(assetName,async,data)
end

function View:dispose()
	if self.is_dispose then return end

	xpcall(function ()
		self:on_dispose()
	end,function (msg)
		print_err(msg)
	end)
end

function View:on_dispose()
	-- print('View:on_dispose', self.name )
	
	-- 销毁avatar
	ViewControlsfunc.on_dispose(self)
	self:clear_timer()
	if self.inject then
		for k, v in pairs( self.inject ) do self.inject[k] = nil end
	end
	
	self.inject 	= nil
	self.cview		= nil
	self.is_dispose = true
	self.gameObject = nil
	self.transform  = nil

end

return View