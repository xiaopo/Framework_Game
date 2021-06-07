local MDefine = require( 'game.defines.MDefine' )
local base = require( 'game.framework.Module' )
local LoaderModule = class(base)

-- 模块注册
function LoaderModule:on_register()
	base.on_register(self)
	MDefine.cache.loader = 'game.modules.loader.LoaderData'
	MDefine.cfg.loader = 'game.modules.loader.LoaderConfig'
	MDefine.proxy.loader = 'game.modules.loader.LoaderProxy'
end

-- 注册的网络事件
function LoaderModule:get_netEvents()
	
end


-- 注册的本地事件
function LoaderModule:get_localEvents()
	return EventDefine.MAP_CUT_REQURE,EventDefine.MAP_CUT_DONE
end

-- 此模块需要注册的视图
function LoaderModule:get_views()
	return 'game.modules.loader.views.MapLoaderView'
end


-- 响应的网络事件
function LoaderModule:on_netEvent(cmd,data)
	

	
end

local start_timer ;
-- 响应的本地事件
function LoaderModule:on_localEvent(cmd,data)
	
	if(cmd == EventDefine.MAP_CUT_REQURE)then
		
		--打开加载界面
		GameMapLoader:LoadScene(data)
		GUIManager.openView("MapLoaderView",{loader = CAssetUtility.GetLoadinger(data),mapname = data,onOpen = function ()
			GUIManager.closeAllViews("MapLoaderView")
		end})
			
		
		
	elseif(cmd == EventDefine.MAP_CUT_DONE)then 
	
		GUIManager.closeAllViews()
		GUIManager.openView("MainuiView")
		
	end
end




return LoaderModule