
local context 				= require( 'game.framework.Context' )
local uidefine 				= require( 'game.defines.UIDefine' )
local tagdefine 			= require( 'game.defines.TagDefine' )

local GUIManager 			= {}
local isInit 				= false

GUIManager.views 			= {}
GUIManager.guiLayers		= {}
GUIManager.guiGameObject 	= nil
GUIManager.guiCamera 		= nil
GUIManager.guiRoot			= nil

GUIManager.offsetOrder      = 1500
GUIManager._delayShowViews  = {}
GUIManager.viewOrderList 	= {}
local LayerMask 			= CS.UnityEngine.LayerMask

--自动回退
local UILayer_2D_BackPressed =
{
	uidefine.UILayer_2D.ViewLayer,
	uidefine.UILayer_2D.WindowLayer,
	uidefine.UILayer_2D.AlertLayer,
	uidefine.UILayer_2D.StoryTopLayer,
	uidefine.UILayer_2D.TopLayer,

}

local viewsortOrder = 0

--创建一个GUI 环境
function GUIManager.init()
	print("GUIManager.init===============================")
	if isInit then return end
	
	--GUI root
	GUIManager.guiGameObject = CS.UnityEngine.GameObject('GUI')
	CS.UnityEngine.Object.DontDestroyOnLoad(GUIManager.guiGameObject)

	--EventSystem
	local gui_ts = GUIManager.guiGameObject.transform
	gui_ts.localPosition = CS.UnityEngine.Vector3(-200,0,0)
	
	--公用一个事件系统 EventSystemManager

	--Camera
	local ucamera_gui = CGameCameraUtiliy.CreateUICamera()
	local cam_go = ucamera_gui.gameObject
	cam_go.tag = tagdefine.tags.GUICamera
	cam_go.transform:SetParentOEx(gui_ts)

	local guiCamera = ucamera_gui.camera
	guiCamera.depth = 2
	guiCamera.clearFlags = CS.UnityEngine.CameraClearFlags.Depth
	guiCamera.backgroundColor = CS.UnityEngine.Color.black
	guiCamera.nearClipPlane = 2
	guiCamera.farClipPlane = 10
	guiCamera.fieldOfView = 40
	guiCamera.allowHDR = false
	guiCamera.allowMSAA = false
	guiCamera.useOcclusionCulling = false
	guiCamera:SetMask(tagdefine.layers.UI,tagdefine.layers.UIModel)
	
	--Canvas
	local gui_canvas = CS.UnityEngine.GameObject('Canvas')
	gui_canvas.transform:SetParent(gui_ts)
	local canvas = gui_canvas:AddComponent(typeof(CS.UnityEngine.Canvas))
	canvas.renderMode = CS.UnityEngine.RenderMode.ScreenSpaceCamera
	canvas.worldCamera = guiCamera
	canvas.planeDistance = 10
	
	--CanvasScaler
	local scaler = gui_canvas:AddComponent(typeof(CS.UnityEngine.UI.CanvasScaler))
	scaler.uiScaleMode = CS.UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize
	scaler.referenceResolution = CS.UnityEngine.Vector2(CS.ResolutionDefine.resolution_width,CS.ResolutionDefine.resolution_hight)
	scaler.matchWidthOrHeight = 1
	
	--GraphicRaycaster
	gui_canvas:AddComponent(typeof(CS.UnityEngine.UI.GraphicRaycaster))

	GUIManager.guiCamera = guiCamera
	GUIManager.guiRoot 	 = gui_canvas.transform
	GUIManager.guiRoot:SetLayer(tagdefine.layers.UI)
	GUIManager.initLayer(tagdefine.layers.UI)
	isInit = true
end


function GUIManager.initLayer(l)
	if isInit then return end
	for i, v in ipairs( uidefine.UILayer_2D_Level ) do
        local order = (i-1) * GUIManager.offsetOrder
		local layer = CS.UnityEngine.GameObject(string.format('%s [%s-%s]',v.name,order,order + GUIManager.offsetOrder))
		local layerRect = layer:AddComponent(typeof(CS.UnityEngine.RectTransform))
		layerRect:SetParent(GUIManager.guiRoot ,true)
		layerRect:SetLayer(l)
		layerRect.anchorMin = CS.UnityEngine.Vector2.zero
		layerRect.anchorMax = CS.UnityEngine.Vector2.one
		layerRect.offsetMin = CS.UnityEngine.Vector2.zero
		layerRect.offsetMax = CS.UnityEngine.Vector2.zero
		layerRect.localPosition = CS.UnityEngine.Vector3.zero
		layerRect.localScale = CS.UnityEngine.Vector3.one

		local canvas = layer:AddComponent(typeof(CS.UnityEngine.Canvas))
		canvas.overrideSorting = true
		canvas.sortingOrder = order
		layer:AddComponent(typeof(CS.UnityEngine.UI.GraphicRaycaster))
		GUIManager.guiLayers[v.name] = require( v.class )(layer)	
		GUIManager.guiLayers[v.name].canvas = canvas
		GUIManager.guiLayers[v.name]:set_sortingOrder(order,order + GUIManager.offsetOrder)
		GUIManager.guiLayers[v.name].layerName = v.name
	end
end

function GUIManager.getLayer(name)
	return GUIManager.guiLayers[name]
end

local function createView(viewName)
	-- 视图尚未打开过
	local viewClass = context.get_viewRequire(viewName)

	if not viewClass then
		print( string.format( 'GUIManager.openView viewName=%s  视图未定义请在模块中定义 Module:get_views()',viewName) )
		return
	end

	viewClass = require( viewClass )

	local layer = GUIManager.guiLayers[viewClass.layer] 

	if not layer then
		print( string.format( 'GUIManager.openView viewName=%s  视图定义的层不存在 layer=%s.',viewName,viewClass.layer) )
		return
	end

	-- 界面为异步加载创建个空对象为载体
	local go = CS.UnityEngine.GameObject(viewName..' Contents')
	
	local view = viewClass(go)
	layer:add_view(view)

	go.layer = LayerMask.NameToLayer("UI")
	
	go.transform:ResetTRS()
	
	
	return view
end


local function addOrder(view)
	viewsortOrder = viewsortOrder + 1
	view.sortOrder = viewsortOrder
end


function GUIManager.ForeceOpenBlur()
	GUIManager.notClearBlurCap = true
	local cameraResolution 	=  require( 'game.utilitys.CameraUtility' ).get_cameraResolution()
	cameraResolution.enabled = true
	cameraResolution.isCapture = true
end

function GUIManager.ForceCleanBlur()
	GUIManager.notClearBlurCap = false
	local cameraResolution 	=  require( 'game.utilitys.CameraUtility' ).get_cameraResolution()
	cameraResolution:ClearBlur()
end

-- 调用相应模块逻辑
function GUIManager.isCanOpenView(viewName,viewData)
	local vmodule = context.getModuleByView(viewName)
	if not vmodule then return true end
	local result,newViewName = vmodule:is_canOpenView(viewName,viewData)
	return result or result == nil,newViewName
end

-- 打开视图(视图注册在各自模块处)
-- function AchievementModule:get_views()
-- 	return 'game.modules.achievement.views.AchievementView'
-- end



function GUIManager.DelayOpenView()
	if #GUIManager._delayShowViews > 0 then 
		for i,v in ipairs(GUIManager._delayShowViews) do
			 GUIManager.openView(v.viewName,v.viewData)
		end
		GUIManager._delayShowViews = {}
	end
end

local function effect_fullSceen_ui(view,isOpen)
	
end

local function effect_fallAvatar_ui(view,isOpen)

end


-- viewData.onOpen 视图成功打开后回调
function GUIManager.openView(viewName,viewData)
	local isCanOpen,newName = GUIManager.isCanOpenView(viewName,viewData)
	if not isCanOpen then
		return
	else
		if newName then
			viewName = newName
		end
	end
	

	local view = GUIManager.getView(viewName)

	if not view then
		view = createView(viewName)
		if not view then return end
		GUIManager.views[viewName] = view
	else
		-- 放到最上层
		local layer = GUIManager.guiLayers[view.layer]
		layer:set_top(view)
	end
	
	
	local UILayer_2D = uidefine.UILayer_2D
	if view.guiLayer and view.guiLayer.layerName == UILayer_2D.ViewLayer then
		--开启ＵＩ模块时，需要关闭目前正在加载的ＵＩ
		GUIManager.closeLoadingView(viewName)
	end
	
	
	view.isPreview = false
	view.viewData = viewData
	
	view:open()
	addOrder(view)
	effect_fullSceen_ui(view,true)

	return view
end



--预创建一个  View
function GUIManager.prestrainView(viewName)
	
	if not GUIManager.isCanOpenView(viewName) then return end
	local view = GUIManager.getView(viewName)
	if not view then
		
		view = createView(viewName)
		if not view then return end
		GUIManager.views[viewName] = view
		
		view.uiloadDone = function ()
			view.uiloadDone = nil;
			view:close()
		end
		view.isPreview = true
		view:open()
		addOrder(view)

	end
	

	return view
	
end

--不管调用多少次只打开一次
function GUIManager.onlyOpenView(viewName,viewData)
	
	if not GUIManager.isCanOpenView(viewName,viewData) then return end
	local view = GUIManager.getView(viewName)
	if not view then
		view = createView(viewName)
		if not view then return end
		GUIManager.views[viewName] = view
	end
	
	view.viewData = viewData
	
	local UILayer_2D = uidefine.UILayer_2D
	if view.guiLayer and view.guiLayer.layerName == UILayer_2D.ViewLayer then
		--开启ＵＩ模块时，需要关闭目前正在加载的ＵＩ
		GUIManager.closeLoadingView(viewName)
	end
	
	if view.is_open then
		if(view.cview)then
			view:on_open_refresh()
			view:refreshView()
		end
	else

		view:open()	
		addOrder(view)
		effect_fullSceen_ui(view,true)
	end
	
	
	return view
end


-- 关闭视图
--isDestroy 是否立即销毁
function GUIManager.closeView(viewName,isDestroy)
	local view = GUIManager.getView(viewName)
	if not view then
		--print( string.format( 'GUIManager.closeView viewName=%s  尝试关闭不存在的的视图  %s',viewName,debug.traceback()) )
		return
	end
	
	if view.viewData and type(view.viewData) == 'table' and view.viewData.close_func then
		view.viewData.close_func() 
		view.viewData.close_func = nil 
	end

	if view.viewData and type(view.viewData) == 'table' and view.viewData.is_death then	--是否需要抛关闭界面事件
		Dispatcher.dispatchEvent(EventDefine.GUIVIEW_DISABLE)
	end
	view.isPreview = false
	
	if view.is_open then
		view:close()
	end
	
	effect_fullSceen_ui(view,false)

    if isDestroy then 
		
		if(view.is_enabled)then
			xpcall(function ()
				view:on_disable();
			end,function ()
				print_err(debug.traceback( ))
			end)
		end
		
		view:destroy() 
	end
	

end

--立即销毁视图
function GUIManager.destroyView(viewName)
    local view = GUIManager.getView(viewName)
    if not view then
        --print( string.format( 'GUIManager.destroyView viewName=%s  尝试销毁不存在的视图  %s',viewName,debug.traceback()) )
        return
    end
    GUIManager.closeView(viewName,true)
	
	GUIManager.isFullScreen();
end

function GUIManager.removeView(viewName)
	if GUIManager.views[viewName] then
		GUIManager.views[viewName] = nil
	else
		print( string.format( 'GUIManager.removeView viewName=%s  尝试移除一个不存在的视图  %s',viewName,debug.traceback()) )
	end
	
	GUIManager.isFullScreen();
end

function GUIManager.getView(viewName)
	return GUIManager.views[viewName]
end


function GUIManager.isFullScreen()
	local views = GUIManager.views

	for _,view in pairs(views) do
		if view and view.is_open and view.fullScreen then
			GUIManager.isFullScreenNow = true;
			return true
		end
	end
	
	return false
end

function GUIManager.isblurCap()
	local views = GUIManager.views
	for _,view in pairs(views) do
		if view and view.is_open and view.mainObject and view.___blurCap then
			return true
		end
	end
	return false
end

function GUIManager.isClown()
	local views = GUIManager.views
	for _,view in pairs(views) do
		if view and view.is_open and view.mainObject and view.___clown then
			--print("clow  ",view.name)
			return true
		end
	end
	
	return false
end


--判断界面是否已打开
function GUIManager.isShowing(viewName)
	local view = GUIManager.views[viewName] 
	if view then
		return view.is_open
	end
	return false
end

--隐藏其它多个层
function GUIManager.visibleLayer(b,...)
	local names = {...}
	for _, v in ipairs( names ) do names[v] = true end
	for k2, v2 in pairs( GUIManager.guiLayers ) do
		if not names[k2] then
			v2:set_visible(b)
		end
	end
end

--隐藏多个层
function GUIManager.visibleLayers(b,...)
	local names = {...}
	for _, v in ipairs( names ) do names[v] = true end
	for k2, v2 in pairs( GUIManager.guiLayers ) do
		if names[k2] then
			v2:set_visible(b)
		end
	end
end


-- 关闭多个界面
function GUIManager.closeViews(...)
	local names = {...}
	for k2, v2 in pairs( names ) do
		GUIManager.closeView(k2)
	end
	
	GUIManager.isFullScreen();
end

function GUIManager.closeFullAvatarViews()
	local views = GUIManager.views
	for _,view in pairs(views) do
		if view and view.is_open and view.__fullAvatar then
			GUIManager.closeView(view.name)
		end
	end
end

--关闭正在加载的UI
function GUIManager.closeLoadingView(viewName)
	local views = GUIManager.views
	local UILayer_2D = uidefine.UILayer_2D
	for _,view in pairs(views) do
		if view and view.is_open and not view.mainObject then
			if view.guiLayer and view.name ~= viewName and view.guiLayer.layerName == UILayer_2D.ViewLayer then
				GUIManager.closeView(view.name)
				print("closeLoadingView",view.name)
			end
		end
	end
end

-- 关闭所有打开的界面，忽略哪些界面
function GUIManager.closeAllViews(...)
	local names = {...}
	table.insert(names,"MainuiView")
	local flag  = {}				
	for _, v in ipairs( names ) do 
		flag[v] = true 
	end
	for k2, v2 in pairs( GUIManager.views ) do
		if not flag[k2] then
			GUIManager.closeView(k2)
		end
	end
	
	GUIManager.isFullScreen();

end


function GUIManager.closeAllViewsSimple(...)
	local names = {...}
	local flag  = {}
	for _, v in ipairs( names ) do 
		flag[v] = true 
	end
	
	for k2, v2 in pairs( GUIManager.views ) do
		if not flag[k2] then
			GUIManager.closeView(k2)
		end
	end
	
	GUIManager.isFullScreen();

end


-- 销毁所有打开的界面，忽略哪些界面
function GUIManager.destroyAllViews(...)
	local names = {...}
	for _, v in ipairs( names ) do names[v] = true end
	for k2, v2 in pairs( GUIManager.views ) do
		if not names[k2] then
			GUIManager.closeView(k2,true)
		end
	end
	
	GUIManager.isFullScreen();
end

-- 销毁所有已经关闭的界面
function GUIManager.destroyAllCloseViews()
	for k2, v2 in pairs( GUIManager.views ) do
		if not v2.is_open then
			
			GUIManager.closeView(k2,true)
		end
	end
end




GUIManager.init()
_G.GUIManager = GUIManager

return GUIManager