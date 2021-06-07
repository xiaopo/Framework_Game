local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local LoginAccountView = class(base)
-- 视图名
LoginAccountView.name = 'LoginAccountView' 
-- 视图预置件路径
LoginAccountView.prefab = 'gui_login_register.prefab' 
-- 视图所在层级
LoginAccountView.layer = UIDefine.UILayer_2D.ViewLayer

function LoginAccountView:on_initView()
	base.on_initView(self)
	
	self:AddListener(self.inject.Button_enter,function ()
		self:close()
	end)
end

function LoginAccountView:on_open()
	base.on_open(self)
	

	
end

function LoginAccountView:on_enabled()
	base.on_enabled(self)
end

function LoginAccountView:on_disable()
	base.on_disable(self)
end

function LoginAccountView:on_dispose()
	base.on_dispose(self)
end

return LoginAccountView