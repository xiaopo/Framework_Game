local ListItemRenderer = require( 'game.components.renderers.ListItemRenderer' )
local base = require( 'game.framework.View' )
local ListView = class(base)

function ListView:ctor(gameObject)
	self.data 		  	= nil
	self.items 		  	= {}
	self.itemIdxs       = {}
	self.xlistView	  	= nil
	self.itemRenderer 	= nil
	self.selectFunction = nil
	self.clickFunction  = nil
	self.enabledClick   = nil
	self.labelField   	= nil
	self.labelFunction	= nil
	self.selectIndex    = -1
	self.buttonGroup    = nil
	self.totalCount 	= 0
	self.playTween 		= nil
	self.onCompleteFunction = nil
	self.onGuideFunction = nil
	self.isAutoScroll 	= nil
	self.removeIndex 	= nil
	self.listHotName    = nil
	base.ctor(self,gameObject)
end


function ListView:on_init()
	self.xlistView = self:get_cview()

	assert( self.xlistView, 'ListView:on_init.  xlistView is nil' )

	self.xlistView.onCreateRenderer:AddListener(function(citem)self:on_createItemRenderer(citem)end)
	self.xlistView.onUpdateRendererLua:AddListener(function(instanceID)self:on_onUpdateRenderer(instanceID)end)
	self.xlistView.onUpdatePost:AddListener(function(instanceID)self:on_UpdatePost(instanceID)end)
	self.xlistView.onRecycleRendererLua:AddListener(function(instanceID)self:on_recycleRenderer(instanceID)end)
	self.xlistView.onDestroy:AddListener(function()base.dispose(self)end)
end

function ListView:on_createItemRenderer(citem)
	local item = self.itemRenderer and self.itemRenderer(citem,self) or ListItemRenderer(citem,self)
	local instanceID = citem.instanceID
	self.items[instanceID] = item
	item:on_create()
	local button = item:get_button()
	if button then
		
		-- if button.onSelect then button.onSelect:RemoveAllListeners() end
		-- if button.onClick then button.onClick:RemoveAllListeners() end
		-- if button.onPointDown then button.onPointDown:RemoveAllListeners() end
			
		if button:GetType() == typeof(CS.XGUI.XButton) then
			button.group = self:get_buttonGroup()
			button.onSelect:AddListener(function()self:on_itemClick(item)end)
			if self.enabledClick or self.clickFunction then
				button:AddClickEvent(function()self:on_itemClickEx(item)end)
			end
		else
			button.onClick:AddListener(function()self:on_itemClick(item)end)
		end
	end


	if self.scrollIdx then
		self.xlistView:ScrollToIndex(self.scrollIdx,self.scrollTime)
		self.scrollIdx = nil
	end

	item:on_enabled()
	self:on_onUpdateRenderer(instanceID)
end

function ListView:on_onUpdateRenderer(instanceID)
	local item = self.items[instanceID]
	if not item then
		print_err(string.format( 'ListView:on_onUpdateRenderer.  item is nil  citem=%s instanceID=%s',tostring( citem ),instanceID))
		return
	end
	
	self.itemIdxs[item:get_index()] = item
	if item:get_index() == self.selectIndex then
		self:on_selectHandle(item,true)
	end
	item:on_data()

	-- 每次update 调用的缓动
	if not self.playTween and item.updateTweenfunc then
		item.updateTweenfunc(item)
	end

	if self.isNoScroll then
		for _, v in pairs(self.items) do
			v.transform:DOKill(true)
		end
		self.isNoScroll = false
	end
end

-- item刷新结束
function ListView:on_UpdatePost()
	-- self:add_timer(function ()
		local flag = false					--动画的时候禁止拖动
		if self.playTween then
			for _, v in pairs(self.items) do
				if v.updateTweenfunc then
					v.updateTweenfunc(v)
				elseif v.createTweenfunc then
					v.createTweenfunc(v)
					flag = true
				end
			end
			self.playTween = nil
		end

		if self.onCompleteFunction then
			self.onCompleteFunction()
		end

		if self.onGuideFunction and not table.isNull(self.data) then
			self.onGuideFunction()
		end

		if flag and self.gameObject.xScrollRect then
			if not self.tweenID then
				local v1 = self.gameObject.xScrollRect.vertical
				local v2 = self.gameObject.xScrollRect.horizontal
				self.gameObject.xScrollRect.vertical = false
				self.gameObject.xScrollRect.horizontal = false
				self.tweenID = self:add_timer(function ()
					self.gameObject.xScrollRect.vertical = v1
					self.gameObject.xScrollRect.horizontal = v2
					self:del_timer(self.tweenID)
					self.tweenID = nil
				end, 1, 1)
			end
		end

		if self.isAutoScroll and self.removeIndex then
			self.isAutoScroll = nil
			self.isNoScroll = true
			for _, v in pairs(self.items) do
				if v:get_index() >= self.removeIndex then
					v:auto_scroll(self.xlistView.layout,{hSpacing = self.xlistView.horizontalSpacing,vSpacing = self.xlistView.verticalSpacing},self.removeIndex)
				end
			end
			self.removeIndex = nil
		end
	-- end, 1, 1)
end

function ListView:SetAutoScroll(index)
	self.isAutoScroll = true
	self.removeIndex = index
end

function ListView:on_recycleRenderer(instanceID)
	local item = self.items[instanceID]
	assert( item, 'ListView:on_recycleRenderer.  item is nil  citem='.. tostring( citem ))
	item:on_recycle()
	self:on_selectHandle(item,false)
end




function ListView:get_buttonGroup()
	if not self.buttonGroup then
		self.buttonGroup = self.xlistView.gameObject:AddComponent(typeof(CS.XGUI.XButtonGroup))
	end
	return self.buttonGroup
end


function ListView:set_data(data,count)
	assert( type(data) == 'table', string.format( 'ListView:set_data.  data not is table  type=%s', type(data) ) )

	local force = self.data ~= data
	self.data = data
	self.totalCount = count or (self.data and #self.data or 0)
	self.xlistView.dataCount = self.totalCount

	if force then self:forceRefresh()end
end


function ListView:get_selectedData()
	return self:get_dataByIdx(self.selectIndex + 1)
end

function ListView:get_selectedItem()
	return self:get_itemByIdx(self.selectIndex)
end

function ListView:get_dataByIdx(idx)
	return self.data and (idx <= self.totalCount and self.data[idx] or nil )
end


function ListView:get_itemByIdx(idx)
	return self.itemIdxs[idx]
end


function ListView:get_items()
    return self.items
end


function ListView:enabled_select(bool)
	for _, item in pairs(self.items) do
		if item.button then
			item.button.enabled = bool
		end
	end
end


--选中状态下再次点击此方法不会被触发
function ListView:on_itemClick(item)

	local button = item:get_button() 
	if button:GetType() == typeof(CS.XGUI.XButton) then
		-- 可点击的无效按钮状态
		if not button.enableSelect then
			self:on_selectChange(item:get_data(),false)
			return
		end
	end

	self:set_selectIndex(item:get_index(),true)
	item:on_click()
end

--每次点击都会触发
function ListView:on_itemClickEx(item)
	item:on_click()
	if self.clickFunction then
		self.clickFunction(item:get_data(),item:get_index())
	end
end

-- 设置数据项渲染器
function ListView:set_itemRenderer(renderer)
	if type(renderer) == 'string' then
		self.itemRenderer = require( renderer )
	else
		self.itemRenderer = renderer
	end
end

function ListView:set_selectIndex(idx,isClick)
	if idx < 0 or idx >= self.totalCount then
		self.selectIndex = idx
		if self.buttonGroup then self.buttonGroup:SetAllTogglesOff()end
		return
	end

	if self.selectIndex == idx then
		return
	end

	self.selectIndex = idx

	local item = self:get_itemByIdx(idx)

	if item and isClick == nil then 
		if self.buttonGroup and self.buttonGroup:AnyTogglesOn() then
			self.buttonGroup:SetAllTogglesOff()
		end

		local isSelect = item:get_index() == self.selectIndex 

		self:on_selectHandle(item,isSelect)
	end

	if item then item:on_select() end

	idx = idx + 1
	local data = self:get_dataByIdx(idx) 

	self:on_selectChange(data,true)
end

function ListView:set_scrollIndex(idx,time)
	self.scrollIdx = idx or 0
	self.scrollTime = time or 0
	self.xlistView:ScrollToIndex(self.scrollIdx,self.scrollTime)
end

-- enableSelect 按钮是否设置为无效 但可点击
function ListView:on_selectChange(data,enableSelect)
	if self.selectFunction then
		self.selectFunction(data,enableSelect)
	end
end

function ListView:set_clickIndex(idx)
	local data = self:get_dataByIdx(idx+1)
	if self.clickFunction then
		self.clickFunction(data,idx)
	end 
end

-- 让item显示选中状态不回调 selectFunction
function ListView:set_selectNoChange(idx)
	self.selectIndex = idx
	local item = self:get_itemByIdx(idx)
	if item then
		self:on_selectHandle(item,true)
	end
end


function ListView:on_selectHandle(renderer,bool)
	local button = renderer:get_button()
	if button then
		if bool then
			if self.buttonGroup then
				button.toggleTransition = CS.UnityEngine.UI.Toggle.ToggleTransition.Fade
				button:SetSelected(true,false)
			else
				button:Select()
			end
		else
			if self.buttonGroup then
				button.toggleTransition = CS.UnityEngine.UI.Toggle.ToggleTransition.None
				button:SetSelected(false,false)
			end
		end
	end
end

function ListView:playItemTween()
	self.playTween = true
	self:forceRefresh()
end

-- 强制刷新当前界面数据
function ListView:forceRefresh(idx)
	if not idx then
		self.xlistView:ForceRefresh()
		return
	end
	
	if type(idx) == 'table' then
		for _,v in ipairs(idx) do
			local item = self:get_itemByIdx(v)
			if item then item:on_data() end
		end
	else
		local item = self:get_itemByIdx(idx)
		if item then item:on_data() end
	end
end

-- 换模版
function ListView:set_template(template)
	if template == self.xlistView.template then
		return
	end
	self:clear()
	self.xlistView.template = template
end


function ListView:on_enabled()
    base.on_enabled(self)
    if self.items then
        for _, v in pairs(self.items) do
            v:on_enabled()
        end
    end
end

function ListView:on_disable()
    base.on_disable(self)
    if self.items then
        for _, v in pairs(self.items) do
            v:on_disable()
        end
    end
end


-- 清理
function ListView:clear()
	local button
	for _, v in pairs( self.items ) do
		button = v:get_button()
		if button then
			if self.buttonGroup then
				if button.onSelect then
					button.onSelect:RemoveAllListeners()
				end
			else
				if button.onClick then
					button.onClick:RemoveAllListeners()
				end
			end
		end
		v:dispose()
	end

	self.items = {}
	self.itemIdxs = {}
end



function ListView:removeEvents()
	if self.xlistView then
		if self.xlistView.onCreateRenderer then
			self.xlistView.onCreateRenderer:RemoveAllListeners()
		end
	
		if self.xlistView.onUpdatePost then
			self.xlistView.onUpdatePost:RemoveAllListeners()
		end
		
		if self.xlistView.onUpdateRendererLua then
			self.xlistView.onUpdateRendererLua:RemoveAllListeners()
		end
		if self.xlistView.onRecycleRendererLua then
			self.xlistView.onRecycleRendererLua:RemoveAllListeners()
		end
		if self.xlistView.onDestroy then
			self.xlistView.onDestroy:RemoveAllListeners()
		end
	end

	if self.items then
		local button
		for _, v in pairs( self.items ) do
			button = v:get_button()
			if button then
				if self.buttonGroup then
					if button.onSelect then
						button.onSelect:RemoveAllListeners()
					end
				else
					if button.onClick then
						button.onClick:RemoveAllListeners()
					end
				end
			end
			v:dispose()
		end
	end 
end

function ListView:on_dispose()
	self:removeEvents()
	self.data 			= nil
	self.items 			= nil
	self.itemIdxs       = nil
	self.xlistView 		= nil
	self.itemRenderer 	= nil
	self.selectFunction	= nil
	self.clickFunction  = nil
	self.labelField   	= nil
	self.labelFunction	= nil
	self.buttonGroup 	= nil
	self.onCompleteFunction = nil
	self.isAutoScroll 	= nil
	self.removeIndex 	= nil
	self.listHotName  	= nil
	base.on_dispose(self)
end

return ListView