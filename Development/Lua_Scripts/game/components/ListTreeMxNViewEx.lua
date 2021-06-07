local ListTreeItemRendererEx = require( 'game.components.renderers.ListTreeItemRendererEx' )
local base = require( 'game.components.ListTreeMxNView' )
local ListTreeMxNViewEx = class(base)

function ListTreeMxNViewEx:ctor(gameObject)
	base.ctor(self,gameObject)
	self.selectFunctionEx = nil
	self.selectedData = nil
	self.selectFunction = function (data)
		self:OnSelectFunction(data)
	end
end

function ListTreeMxNViewEx:OnSelectFunction(data)
	local seInfo = (data.list and data.list[1]) or data[1]
	if(not seInfo)then
		if(self.selectedData ~= data)then
			self.selectedData = data
			if self.selectFunctionEx then
				self.selectFunctionEx(data)
			end
		end
	end
end

function ListTreeMxNViewEx:get_itemRenderClass(layer)
	layer = math.max(1,math.min(#self.rednderClass,layer))
	return self.rednderClass[layer] or ListTreeItemRendererEx
end

function ListTreeMxNViewEx:CancelSelected()
	base.CancelSelected(self)
	self.selectedData = nil
end

function ListTreeMxNViewEx:get_itemByIdx(key)
	local cItem = self.xLisTreeView:GetTreeItem(key)
	if(cItem and cItem.visible and  cItem.gameObject)then
		local instanceId = cItem.gameObject:GetInstanceID()
		local lItem = self.totalItems[instanceId]
		return lItem
	end
end

function ListTreeMxNViewEx:set_data(listTree)
	self:set_DataB(listTree)
	--设置单选模式
	self:set_select_type(1)
end

function ListTreeMxNViewEx:on_dispose()
	self.selectFunctionEx = nil
	self.listTreeHotName  = nil
	self.selectedData = nil
	base.on_dispose(self)
end

return ListTreeMxNViewEx