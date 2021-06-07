local base = require( 'game.framework.View' )
local ListView = require( 'game.components.ListView' )
local ContentRenderer = require( 'game.components.renderers.ContentRenderer' )
local TabView = class(base)
function TabView:ctor(gameObject,viewport)
	self.data 		  	      = nil
	self.tabs 				  = nil
	self.viewport 			  = viewport
	self.ctabView  			  = nil
	self.renderers   		  = nil
	self.instancePrefabs 	  = nil
	self.activeRenderer 	  = nil
	self.lastActiveRenderer   = nil
	self.selectFunction		  = nil
	self.invalidClickFunction = nil
	base.ctor(self,gameObject)
end

function TabView:on_init()
	self:on_initTabs()
	self:on_initViewport()
end


function TabView:on_initTabs()
	local ts = self:find_component(nil,'Tabs')
	assert( ts, 'TabView:on_initTabs. find_component "Tabs" is nil' )
	self.tabs = ListView(ts.gameObject)
	self.tabs.labelField = 'label'
	self.tabs.selectFunction = function(data,enable)self:on_tabSelect_change(data,enable)end
end

function TabView:on_initViewport()
	self.viewport = self.viewport or self:find_component(typeof(CS.XGUI.XViewport),'Viewport')
	assert( self.viewport, 'TabView:on_initViewport. find_component "Viewport" is nil' )
	self.viewport.onChange:AddListener(function(name,gameObject)self:on_viewport_change(name,gameObject)end)

	self.ctabView = self:get_cview()
	self.ctabView.onDestroy:AddListener(function()base.dispose(self)end)
end

-- tab列表事件改变
function TabView:on_tabSelect_change(data,enable)
	-- 无效的标签点击
	if enable == false then self:on_invalidClickFunction(data) return end

	assert( data.view and data.view ~= '', string.format( '<color=red>TabView:on_tabSelect_change. data.view is nil  selectIndex = "%s" 视图类不存在 </color>',self:get_selectIndex() ) )

	local view = type(data.view) == 'string' and require( data.view ) or data.view
	local prefab = data.prefab or view.prefab

	assert( prefab and prefab ~= '', string.format( '<color=red>TabView:on_tabSelect_change. data.prefab is nil  prefab = "%s" 预置件属性为空 </color>',prefab ) )
	
	local instanceObject = self.instancePrefabs and  self.instancePrefabs[prefab] or nil
	
	if self.viewport.activeView == prefab and instanceObject then
		--2个不一样的视图拥有相同预置件
		
		if self.lastActiveRenderer then self.lastActiveRenderer:on_disable() end   --and self.lastActiveRenderer ~= self.activeRenderer
		
		if not self.renderers then self.renderers = {} end

		local vname = view.name and view.name or tostring(view)
		
		self.activeRenderer = self.renderers[vname]

		
		
		if not self.activeRenderer then
			self.activeRenderer = view(self:get_selectIndex(),instanceObject)
			self.activeRenderer.viewData = data
			self.activeRenderer:on_initView()
			self.activeRenderer.mainTab = self
			self.renderers[vname] = self.activeRenderer
		end

		self.lastActiveRenderer = self.activeRenderer
		self.activeRenderer.viewData = data
		self.activeRenderer:on_enabled()
		self:on_selectChange(data)
	else
		self.viewport.activeView = prefab
	end
end


-- 视图激活加载完成改变
function TabView:on_viewport_change( name, gameObject)
	local data = self:get_selectedData()
	if not data then return end
	
	local view = type(data.view) == 'string' and require( data.view ) or data.view
	
	if not self.instancePrefabs then self.instancePrefabs = {} end

	if not self.instancePrefabs[name] then
		self.instancePrefabs[name] = gameObject
	end
	
	
	if not self.renderers then self.renderers = {} end
	
	if self.lastActiveRenderer then self.lastActiveRenderer:on_disable() end
	
	local vname = view.name and view.name or tostring(view)
	
	local activeRenderer = self.renderers[vname]
	
	if not activeRenderer then
		activeRenderer = view(self:get_selectIndex(),gameObject)
		activeRenderer.viewData = data
		activeRenderer:on_initView()
		activeRenderer.mainTab = self
		self.renderers[vname] = activeRenderer
	end

	activeRenderer.viewData = data
	activeRenderer:on_enabled()
	
	self.activeRenderer = activeRenderer
	self.lastActiveRenderer = self.activeRenderer
	self:on_selectChange(data)
end


function TabView:get_selectedData()return self.tabs:get_selectedData()end
function TabView:get_selectIndex()return self.tabs.selectIndex end


function TabView:on_enabled()
	base.on_enabled(self)
	 -- if self.activeRenderer then
	 -- 	self.activeRenderer:on_enabled()
	 -- end
end

function TabView:on_disable()
	base.on_disable(self)
	if self.activeRenderer then
		self.activeRenderer:on_disable()
	end
end

function TabView:set_data(data)
	
	assert( type(data) == 'table', string.format( 'TabView:set_data.  data not is table  type=%s', type(data) ) )

	self.data = data
	self.tabs:set_data(data)
end

function TabView:set_selectIndex(idx)
	self.tabs:set_selectIndex(idx)
end

function TabView:on_selectChange(data)
	if self.selectFunction then
		self.selectFunction(data)
	end
end

function TabView:on_invalidClickFunction(data)
	if self.invalidClickFunction then
		self.invalidClickFunction(data)
	end
end



function TabView:on_dispose()
	self.ctabView.onDestroy:RemoveAllListeners()
	self.viewport.onChange:RemoveAllListeners()

	if self.tabs then
		self.tabs:dispose()
	end
	
	-- 清除渲染器
	if self.renderers then for _, v in pairs( self.renderers ) do v:dispose() end end

	self.data 		  		= nil
	self.tabs 				= nil
	self.viewport 			= nil
	self.ctabView  			= nil
	self.renderers 			= nil
	self.selectFunction 	= nil
	self.instancePrefabs 	= nil
	self.activeRenderer 	= nil
	self.lastActiveRenderer = nil
	base.on_dispose(self)
end



return TabView