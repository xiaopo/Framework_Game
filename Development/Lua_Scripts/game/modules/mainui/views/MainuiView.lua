local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local MainuiView = class(base)
-- 视图名
MainuiView.name = 'MainuiView' 
-- 视图预置件路径
MainuiView.prefab = 'gui_mainui_main.prefab' 
-- 视图所在层级
MainuiView.layer = UIDefine.UILayer_2D.MainUILayer

function MainuiView:on_initView()
	base.on_initView(self)
	
	
	self:AddListener(self.inject.Button_shop,function ()
		GUIManager.openView("PackView")
	end)
	
	self:AddListener(self.inject.Button_set,function ()
		CS.ProfileGUIManager.Instance:ShowAssetBundleView()
	end)
	
	
end

function MainuiView:on_refreshView()
	base.on_refreshView(self)
end

function MainuiView:on_enabled()
	base.on_enabled(self)
end

function MainuiView:on_disable()
	base.on_disable(self)
end

function MainuiView:on_dispose()
	base.on_dispose(self)
end

return MainuiView