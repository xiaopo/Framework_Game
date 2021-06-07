local UIDefine = require( 'game.defines.UIDefine' ) 
local base = require( 'game.framework.gui.GUILayer' )
local GUITouchLayer = class(base)

function GUITouchLayer:on_init()
--	self.gameObject:AddComponent(typeof(CS.XGUI.XNoDrawingView))
--	local touchClick = self.gameObject:AddComponent(typeof(CS.XGUI.XTouchClick))
--	touchClick.distance = 20
--	touchClick.onClick = function(pdata)self:on_click_event(pdata)end
--
--
--	local touchDrag =  self.gameObject:AddComponent(typeof(CS.XGUI.XTouchDrag))
--	touchDrag.onDrag = function(pdata)self:on_drag_event(pdata)end
--	touchDrag.onEndDrag = function(pdata)self:on_end_drag_event(pdata)end
end

function GUITouchLayer:on_click_event(pdata)
	print("GUITouchLayer.on_click_event!")
	Dispatcher.dispatchEvent(EventDefine.INPUT_TOUCH_CLICK,pdata.position)
end

function GUITouchLayer:on_drag_event(pdata)
	print("GUITouchLayer.on_drag_event!")
	Dispatcher.dispatchEvent(EventDefine.INPUT_TOUCH_DRAG,pdata.delta)
end

function GUITouchLayer:on_end_drag_event(pdata)
	print("GUITouchLayer.on_end_drag_event!")
	Dispatcher.dispatchEvent(EventDefine.INPUT_TOUCH_END_DRAG,pdata)
end

return GUITouchLayer