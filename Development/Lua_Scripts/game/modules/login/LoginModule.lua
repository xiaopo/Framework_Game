local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local LoginModule = class(base)

-- 模块注册
function LoginModule:on_register()
	base.on_register(self)
	MDefine.cache.login = 'game.modules.login.LoginData'
	MDefine.cfg.login = 'game.modules.login.LoginConfig'
	MDefine.proxy.login = 'game.modules.login.LoginProxy'
end

-- 注册的网络事件
function LoginModule:get_netEvents()
end


-- 注册的本地事件
function LoginModule:get_localEvents()
	
	return EventDefine.ETNER_LOGIN_SCENE
end

-- 此模块需要注册的视图
function LoginModule:get_views()
	return  'game.modules.login.views.LoginView', --账号界面
			'game.modules.login.views.LoginAccountView'--登录场景UI

end

-- 响应的网络事件
function LoginModule:on_netEvent(cmd,data)
end

-- 响应的本地事件
function LoginModule:on_localEvent(cmd,data)
	if cmd == EventDefine.ETNER_LOGIN_SCENE then
		--预加载场景
--		local loader = CAssetUtility.PreLoad("scene_00.unity")
--		local id = 0
--		id = self:add_timer(function ()
--			if loader:IsDone() then 
--				self:del_timer(id)
--				GUIManager.openView("LoginView")
--			end
--		end,0.1,-1)
		
		GUIManager.openView("LoginView")
	end
end

return LoginModule