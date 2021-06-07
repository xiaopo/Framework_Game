local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local Tweenfunc = require( 'game.framework.funcs.Tweenfunc' ) 

local GUIWindow = class(base)


-- 视图名
GUIWindow.name 		 = 'GUIWindow' 
-- 视图预置件路径 如果需要其它样式请在 Common->CommonWindow 下面找
GUIWindow.prefab	 = 'GUI_Window1_View.prefab' 
-- 视图所在层级
GUIWindow.layer 	 = UIDefine.UILayer_2D.WindowLayer
-- 缓动方法
GUIWindow.tweenfunc  = Tweenfunc.ui_tween_Mask
-- 内容父节点路径
GUIWindow.content    = 'win/content'

GUIWindow.openAudio  = 'ui_jinrujiemian3.ogg'

GUIWindow.___blurCap 	 = false
GUIWindow.___hideTalk    = true
GUIWindow.___showWaitUI  = true
--GUIWindow.cprefab  内容资源(string)
--GUIWindow.size     窗口大小(Vector2) 
--GUIWindow.title    窗口标题(string)




function GUIWindow:on_init()
	base.on_init(self)
end

function GUIWindow:on_initView()
	base.on_initView(self)

	local win_closeBtn =  self:get_win_component('win_closeBtn')
	if win_closeBtn then win_closeBtn:AddClickEvent(function()self:on_click_close()end)end
	
	local win_titleText = self:get_win_component('win_titleText')
	if win_titleText then
		win_titleText.text = self.title or ''
	end
	
	if self.size then
		local win_rect = self:get_win_component('win_rect')
		if win_rect then
			win_rect.sizeDelta = self.size
		end
	end
end

function GUIWindow:get_win_component(name)
	if not self.main_cview and self.mainObject then
		self.main_cview = self.mainObject:GetComponent(typeof(CS.XGUI.XView))
	end

	assert( self.main_cview, 'GUIWindow:get_win_component. main_cview is nil ' )

	return self.main_cview:Get(name)
end

function GUIWindow:on_click_close()
	self:close()
end


function GUIWindow:on_dispose()
	self.main_cview = nil
	base.on_dispose(self)
end

return GUIWindow