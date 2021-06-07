local base = require( 'game.framework.View' )
local GUILayer = class(base)

local viewOffsetOrder = 50

function GUILayer:ctor(gameObject)
	base.ctor(self,gameObject)
	
	self.visible = true
end

function GUILayer:add_view(view)
	
	view.guiLayer = self
	view.transform:SetParent(self.transform)
	local ts = view.gameObject:GetComponent(typeof(CS.UnityEngine.RectTransform))
	if not ts or ts:IsNull() then
		ts = view.gameObject:AddComponent(typeof(CS.UnityEngine.RectTransform))
	end
	view.transform = ts
	ts.anchorMin = CS.UnityEngine.Vector2.zero
	ts.anchorMax = CS.UnityEngine.Vector2.one
	ts.offsetMin = CS.UnityEngine.Vector2.zero
	ts.offsetMax = CS.UnityEngine.Vector2.zero
	ts.localPosition = CS.UnityEngine.Vector3.zero
	ts.localScale = CS.UnityEngine.Vector3.one


	local order = self:get_lastSortingOrder()
	view.gameObject:AddCanvas(order)
	view.__sortingOrder = order
	
	if not self.views then self.views = {} end
	table.insert( self.views, view )
end

function GUILayer:remove_view(view)
	if not self.views then return end
	for i, v in ipairs( self.views ) do
		if v == view then
			local gameObject = view.gameObject
			GUIManager.removeView(view.name)
			view.guiLayer = nil
			view:dispose()
			table.remove( self.views, i )
			CS.UnityEngine.Object.Destroy(gameObject)
			return
		end
	end
end


function GUILayer:set_top(view)
	--一个视图不需要再提高层次
	if #self.views == 1 and self.views[1] == view then return end
	--已经在最高层级里面
	if self.views[#self.views] == view then return end
	
	local order = self:get_lastSortingOrder()
	if order >= self.maxSortingOrder then
		-- order 超出范围重置本层所有视图	
		for i, v in ipairs(self.views) do
			order = self.sortingOrder + (i - 1) * viewOffsetOrder
			v.gameObject:AddCanvas(order)
			v.__sortingOrder = order
			v.transform:SetAsLastSibling()
			v.gameObject:UpdateAllChildSortingOrder()
		end
	else
		view.gameObject:AddCanvas(order)
		view.__sortingOrder = order
		view.transform:SetAsLastSibling()

		--视图已经在当前层，再次打开需要提到最高层
		table.removeItem(self.views,view)
		table.insert( self.views, view )
		view.gameObject:UpdateAllChildSortingOrder()
	end
end


function GUILayer:set_sortingOrder(value,max)
	self.sortingOrder = value
	self.maxSortingOrder = max
end

function GUILayer:get_lastSortingOrder()
	local order = self.sortingOrder

	if self.views and #self.views > 0 then
		--当前显示的最高层级
		local lastView = self.views[#self.views]
		if lastView then
			order = lastView.__sortingOrder + viewOffsetOrder
		end
	end
	
	return order
end

function GUILayer:set_visible(b)
	if self.canvas then
		self.canvas.enabled = b
	end
	self.visible = b
	self:set_pos(b)
end

function GUILayer:set_pos(b)
	self.transform.localPosition = Vector3(b and 0 or -3000,0,0)
end

return GUILayer