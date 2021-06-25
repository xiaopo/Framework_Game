local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local GameMapLoader = require( 'game.modules.mapfight.scene.GameMapLoader' )
local LoginView = class(base)
-- 视图名
LoginView.name = 'LoginView' 
-- 视图预置件路径
LoginView.prefab = 'gui_login_view.prefab' 
-- 视图所在层级
LoginView.layer = UIDefine.UILayer_2D.ViewLayer

local SingleSceneLoader = CS.Game.MScene.SingleSceneLoader()

function LoginView:on_initView()
	base.on_initView(self)
	
	--加载登录场景

	SingleSceneLoader:Load("scene_00.unity")

	SingleSceneLoader.LoadComplete = function ()
		CS.LauncherGUIManager.Instance:DestroyAll()
	end
	
			
	self:AddListener(self.inject.Button_enter,function ()
		
		Dispatcher.dispatchEvent(EventDefine.ENTER_GAME_MAP)
		Dispatcher.dispatchEvent(EventDefine.MAP_CUT_REQURE,"scene_maincity.unity")
	end)
	
end

function LoginView:on_open()
	base.on_open(self)
	
	

	GUIManager.openView("LoginAccountView")
	
	self.inject.Text_Title.text = "弓箭手"
	
	
end


function LoginView:on_enabled()
	base.on_enabled(self)
end

function LoginView:on_disable()
	base.on_disable(self)
end

function LoginView:on_dispose()
	base.on_dispose(self)
end

return LoginView