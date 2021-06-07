local base = require("game.framework.gui.GUILayer")
local Adaptfunc = require("game.framework.funcs.Adaptfunc")
local GUIAdaptLayer = class(base)

function GUIAdaptLayer:add_view(view)
	
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
	
	if Adaptfunc.isCanAdpScale() and not view.notAdpScale then
		local rect = view.gameObject:GetComponent("RectTransform")
		if view.fullScreen then
			rect.pivot = Vector2(0.5, 0.5)
			rect.anchorMax = Vector2(0.5, 0.5)
			rect.anchorMin = Vector2(0.5, 0.5)
			rect.sizeDelta = Vector2(1280,720)
		end
		rect.localScale = rect.localScale * Adaptfunc.getAdpTargetScale()
	elseif Adaptfunc.isIphoneX() and view.fullScreenIphoneX then
		local rect = view.gameObject:GetComponent("RectTransform")
		rect.offsetMin = Vector2(Adaptfunc.getBangValue(),0)
		rect.offsetMax = Vector2(-Adaptfunc.getBangValue(),0)
	end

	if not self.views then self.views = {} end
	table.insert( self.views, view )
end

return GUIAdaptLayer