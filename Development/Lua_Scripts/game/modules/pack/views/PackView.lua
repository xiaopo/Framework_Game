local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local PackView = class(base)
-- 视图名
PackView.name = 'PackView' 
-- 视图预置件路径
PackView.prefab = 'PackUIView.prefab' 
-- 视图所在层级
PackView.layer = UIDefine.UILayer_2D.ViewLayer

function PackView:on_initView()
	base.on_initView(self)
	
	self:AddListener(self.inject.Button_close,function ()
		GUIManager.closeView("PackView")
	end)
end

function PackView:on_refreshView()
	base.on_refreshView(self)
end

function PackView:on_enabled()
	base.on_enabled(self)
end

function PackView:on_disable()
	base.on_disable(self)
end

function PackView:on_dispose()
	base.on_dispose(self)
end

return PackView