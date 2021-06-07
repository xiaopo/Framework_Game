local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local MapLoaderView = class(base)
-- 视图名
MapLoaderView.name = 'MapLoaderView' 
-- 视图预置件路径
MapLoaderView.prefab = 'gui_mapload_normal_view.prefab' 
-- 视图所在层级
MapLoaderView.layer = UIDefine.UILayer_2D.ViewLayer

function MapLoaderView:on_initView()
	base.on_initView(self)
	
	self.inject.ProgressBar.maxValue = 100
end



function MapLoaderView:on_refreshView()
	base.on_refreshView(self)
	
	local mapName_curent = self.viewData.mapname
	local assetLoader =  self.viewData.loader
	local checkId = 0
	checkId = self:add_timer(function ()
		
		local progress = assetLoader:Progress() * 100
		self.inject.ProgressBar:RunProgress(progress,0.4)
		
	end,0.1,-1)
	
end



function MapLoaderView:on_enabled()
	base.on_enabled(self)
end

function MapLoaderView:on_disable()
	base.on_disable(self)
end

function MapLoaderView:on_dispose()
	base.on_dispose(self)
end

return MapLoaderView