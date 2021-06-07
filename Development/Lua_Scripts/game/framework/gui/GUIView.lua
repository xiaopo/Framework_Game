local Vector3 			= Vector3--CS.UnityEngine.Vector3

local UIDefine 			= require( 'game.defines.UIDefine' ) 
local base 				= require( 'game.framework.View' )
local GUIConfig 		= require('game.framework.gui.GUIConfig')
local ViewControlsfunc 	= require('game.framework.funcs.ViewControlsfunc')
local GUIView 			= class(base)
GUIView.viewfunc     	= require( 'game.framework.funcs.Viewfunc' )   -- 视图内部内容处理方法        	(可选)
GUIView.layer 		 	= UIDefine.UILayer_2D.ViewLayer
-- GUIView.destroyTime  = (CommonFunc.IsLowMemory() or CS.UnityEngine.Application.isEditor) and 0 or 20 --1g内存手机或者是editor模式就立即销毁，便于看dotween报错
GUIView.prefab 		 	= 'GUI_Default_View.prefab'					-- 视图预置资源					(必选)*
GUIView.fullScreen   	= false;	--是否是全屏界面
GUIView.___clown	 	= false;

--GUIView._preGameObjects		= {}								-- 预创建实例对象 放入对象池
--格式 { 
--		  {"ui_beibaobeijing.prefab",CS.GopManager.GUIAvatarScene}
--	   }


--GUIView._preAssetList 		= {}								-- 预加载源对象 
-- layer 		=  uidefine.UILayer_2D.ViewLayer  					-- 层级							(可选默认为 ViewLayer)
-- music 		= 'test.ogg'  					  					-- 打开界面的声音					(可选)
-- destroyTime 	= 10							  					-- 关闭界面后销毁的时间			(可选默认10s 设置为空或者-1将是永不销毁:慎重选择)
-- panels 		= {'game.framework.gui.GUIPanel'} 					-- 当前视图的所有子面板			(可选)
-- maskImage     = ""												-- 蒙黑图片名字
-- tweenfunc	= Tweenfunc.ui_tween_norm							-- 打开视图与关闭视图的效果 		(可选)
-- openAudio	= ''												-- 界面打开的音效 				(可选)
-- closeAudio	= ''												-- 界面关闭的音效 				(可选)
-- ___blurCap	= false												-- 界面打开是否需要模糊截屏 	 (可选)
-- ___clown     = false												-- 全屏界面打开效果（2）		 （可选）
-- ___hideTalk  = false												-- 是否打开界面时不弹对话界面		(可选)
--___showWaitUI	= false												-- 初始化UI时 先显示一个等待ＵＩ
-- open 生命周期
-- GUIManager.openView() 
-- 		  ↓
-- GUIView:open() 
-- 		  ↓
-- viewfunc.load_views()
-- 		  ↓
-- GUIView:on_initView()
-- 		  ↓
-- GUIView:on_open_refresh()
-- 		  ↓
-- GUIView:openEffect()
-- 		  ↓
-- GUIView:on_open()
-- 		  ↓
-- GUIView:on_enabled()
-- 		  ↓
-- GUIView:refreshView()
-- 		  ↓
-- GUIView:on_refreshView()
-- 		  ↓  子面板
-- 		 viewfunc.enabled_views()
-- 		  ↓
--		 GUIPanel:on_initView()
-- 		  ↓
-- 		 GUIPanel:on_enabled()
-- 		  ↓
-- 		 GUIPanel:refreshView()
-- 		  ↓
-- 		 GUIPanel:on_refreshView()



local function viewlog(self,content)
	if G_GUI_FUNC_CALL_TRACE then
		--print(string.format('GUIView:viewlog.  name = %s  %s   %s',self.name,content,debug.traceback()))
		print(string.format('GUIView:viewlog.  name = %s  %s  ',self.name,content))
	end
end


function GUIView:ctor(gameObject)

	self.is_open 	 = nil   --界面是否打开
	self.is_closeing = nil   --是否在关闭过程中
	self.panelViews = self.panels and {} or nil -- 所有子面板
	self.destroyTime = GUIConfig.DealDestroyTime(self.name)
	base.ctor(self,gameObject)
end

-- 视图打开瞬间调用，但不一定视图在这个时候是可见的	
function GUIView:open()

	
	-- 如果视图已经打开但未加载完成，再次调用此接口将跳出
	if self.is_open and not self.is_enabled then return end

	self:set_visible(true)

	-- 取消延迟关闭
	if self.__delayCloseTimeId then self:del_timer(self.__delayCloseTimeId) end

	self.is_open = true
	
	-- 加载主视图
	if(self.___showWaitUI)then
		-- Alertfunc.ShowBaGuaWaitAlert("正在加载...",10,true)
	end
	
	self.viewfunc.load_views(self)
	
	-- 取消延迟销毁
	self:delayDestroy()

end

-- 当视图成功打开后，若有缓动将等待缓动成功执行完后被调用
function GUIView:on_open()

	
	self.is_enabled = true

	-- 主视图激活
	xpcall(function ()
		self:on_enabled()
	end,function (msg)
		print_err(msg)
	end)

	if(self.___showWaitUI)then
		-- Alertfunc.ShowBaGuaWaitAlert()
	end
	-- 子面板激活
	self.viewfunc.enabled_views(self)

	--若有打开回调则执行
	self:on_open_complete()
end

function GUIView:on_open_complete()
	--若有打开回调则执行
	if self.viewData and self.viewData.onOpen then
		self.viewData.onOpen(self)
	end
	
	self:ReBundAllEvent()
end

-- 当界面成功关闭后，若有缓动将等待缓动成功执行完后被调用
function GUIView:_on_close_internal()
	
	self.is_closeing = nil
	if self.is_enabled then 
		self.is_enabled = nil

		-- 子面板禁用
		self.viewfunc.enabled_views(self)

		-- 主视图
		xpcall(function ()
			self:on_disable()
		end,function (msg)
			print_err(msg)
		end)
		

		-- 视图关闭启动延迟销毁
		self:delayDestroy()
	end

	self:set_visible(false)
	
	self:clear_timer()--移除
	
	if self.mainObject then
		self:on_close()
	end
end

-- 关闭视图
function GUIView:close()
	
	if not self.is_open then return end
	
	
	self.is_open = false
	self.is_closeing = true
	
	-- 通知管理器此视图关闭
	GUIManager.closeView(self.name)

	if self.is_enabled then
		self:closeEffect(self._on_close_internal)
	else
		-- 界面尚未激活 直接关掉
		self:_on_close_internal()
	end
	
	self:UnbundAllEvent()
end

function GUIView:on_close()

	--若有打开回调则执行
	if self.viewData and self.viewData.onClose then
		self.viewData.onClose(self)
	end
end

-- 延迟关闭界面 若延迟时间未到再次调用 打开界面 延迟取消
function GUIView:delayClose(time)

	if self.__delayCloseTimeId then self:del_timer(self.destroyTimeId) end
	self.__delayCloseTimeId = self:add_timer(function()
		self:del_timer(self.__delayCloseTimeId)
		self.__delayCloseTimeId = nil
		self:close()
	end,1,1,time)
end


function GUIView:openEffect(func)

	if(self.___showWaitUI)then
		-- Alertfunc.ShowBaGuaWaitAlert()
	end
	
	if self.tweenfunc then
		self.tweenfunc(self,func,true)
	else
		func(self)
	end

	-- 打开界面音效
	if not string.isEmptyOrNull(self.openAudio) then self:playAudio(self.openAudio)end

end

function GUIView:closeEffect(func)

	if self.tweenfunc then
		self.tweenfunc(self,func,false)
	else
		func(self)
	end

	-- 关闭界面音效
	if not string.isEmptyOrNull(self.closeAudio) then self:playAudio(self.closeAudio)end
end

function GUIView:on_enabled()

	base.on_enabled(self)

	self:refreshView()
end

function GUIView:on_disable()

	base.on_disable(self)
end

function GUIView:set_visible(bool)
	
	if not self.gameObject or not self.transform then return end
	if not self.canvasGroup then
		self.canvasGroup 	= self.gameObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	self.canvasGroup.alpha 			= bool and 1 or 0
	self.canvasGroup.interactable 	= bool
	self.canvasGroup.blocksRaycasts = bool

	local pos = bool and Vector3(0,0,0) or Vector3(0,-5000,0)
	self.transform.localPosition = pos
	
	if(self.objectHide)then
		self.gameObject:SetActive(true)
	end
end

-- 第一次创建视图时触发
function GUIView:on_initView()viewlog(self,'GUIView:on_initView')end
-- 每次打开界面播完打开效果后触发
function GUIView:on_refreshView()viewlog(self,'GUIView:on_refreshView')end
-- 第次打开界面在打开效果之前触发
function GUIView:on_open_refresh()
	viewlog(self,'GUIView:on_open_refresh')

end

-- 可外部手动调用
function GUIView:refreshView() 
	if self.is_enabled then 
		xpcall(function ()
			self:on_refreshView()
		end,function (msg)
			print_err(msg)
		end)
	end 
end


--缓存很长时间的界面，到一定时间后需要隐藏
function GUIView:delayActive()
	
	if(self.destroyTime and self.destroyTime ~= -1 and self.destroyTime < 3)then return end;
	
	if self.is_open then
		
		if self.destroyActiveId then
			self:del_timer(self.destroyActiveId)
			self.destroyActiveId = nil
		end
		
	else
	
		self.destroyActiveId = self:add_timer(function() 
			
			if not self.gameObject or not self.transform then return end
			
			self.destroyActiveId = nil
			
			self.gameObject:SetActive(false)
			
			ViewControlsfunc.set_visible(self,false)
			
			self.objectHide = true
			
		end,1,1,3)
	
	end

end


-- 延迟销毁视图
function GUIView:delayDestroy()

	self:delayActive()
	
	if not self.destroyTime or self.destroyTime == -1 then return end

	
	if self.is_open then
		
		-- 打开界面清除延迟销毁定时器
		if self.destroyTimeId then
			self:del_timer(self.destroyTimeId)
			self.destroyTimeId = nil
		end
		
	else
		-- 关闭界面启动延迟销毁定时器
		
		self.destroyTimeId = self:add_timer(function() 
			if(self.is_enabled)then
				xpcall(function ()
					self:on_disable()
				end,function (msg)
					print_err(msg)
				end)
			end
			
			self:destroy() 
		end,1,1,self.destroyTime)
	end
end

--立即销毁
function GUIView:destroy()
	if self.is_open then
		print( string.format( '<color=red> GUIView::destroy. 界面打开状态下执行了销毁了 %s </color>', self.name ) )
		return
	end

	if(self.guiLayer)then
		self.guiLayer:remove_view(self)
	end
	
	self:del_timer(self.destroyTimeId)
	self.destroyTimeId = nil
	
	self:del_timer(self.destroyActiveId)
	self.destroyActiveId = nil

end

local timerfunc 	= require('game.framework.funcs.Timerfunc')
local view_dispose_frame_id = nil
-- 视图销毁时被调用
function GUIView:on_dispose()

	self.canvasGroup = nil

	self.viewfunc.dispose_views(self)
	
	if not CS.UnityEngine.Application.isMobilePlatform then
		local ATestFunc = require("game.modules.atest.ATestFunc")
		ATestFunc.AddDisposeView(self.name)
	end
	
	base.on_dispose(self)
	
	
	--通知管理器销毁了
	--一帧只能调用一次
	if not view_dispose_frame_id then
		
		view_dispose_frame_id = timerfunc.add_timer(nil,function() 
			
			GUIManager.on_view_dispose()
			
			view_dispose_frame_id = nil
		end,0,1)
		
		
		
	end


end

return GUIView