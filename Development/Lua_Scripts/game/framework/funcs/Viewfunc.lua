
local log = false 

local GopManager 		= CS.GopManager
local typeGameObject 	= typeof(CS.UnityEngine.GameObject)

local function on_child_panel_load_complete(self,assetName,async,panelClass)
	if not self.is_open or self.is_dispose then return end
	if log then print( string.format( 'Viewfunc:on_child_panel_load_complete panelName = %s  loadFinish frameCount = %s', panelClass.name ,CS.UnityEngine.Time.frameCount) )end

	if not async:IsSucceed()  then
		print( string.format( '<color=red>Viewfunc:on_child_panel_load_complete. asset load error assetName = "%s"  error = "%s"</color>', assetName,async.Message ) )
		return
	end

	local parentNode = self.inject[panelClass.node]

	if not parentNode then
		print( string.format( '<color=red>Viewfunc:on_child_panel_load_complete panelName = %s  node is nil  node = "%s" </color>', panelClass.name,panelClass.node ) )
		return
	end

	if not self.panelViews then self.panelViews = {} end
	if self.panelViews[panelClass.name] then
		print( string.format( '<color=yellow>Viewfunc:on_child_panel_load_complete. panelViews["%s"] already exist. </color>', panelClass.name ) )
		return
	end

	local panel  = async:Instantiate(parentNode.transform)
	local nClass = panelClass(panel,self) 
	self.panelViews[panelClass.name] = nClass

	xpcall(function ()
		nClass:on_initView()
		nClass:on_enabled()
	end,function (msg)
		print_err(msg)
	end)
	

	if log then print( string.format( 'Viewfunc:on_child_panel_load_complete panelName = "%s"  on_enabled..', panelClass.name ) )end
end

local function on_child_panel_load(self,assetName,panelClass,on_main_object_load_complete)
	
	local request = CAssetUtility.LoadAsset(assetName,typeGameObject,true)
	if not request then 
		return
	end
	
	local checkLoad_id = 0
	checkLoad_id = self:add_timer(function ()
		
		if request:IsDone() then
			self:del_timer(checkLoad_id)
			
			on_child_panel_load_complete(self,assetName,request,panelClass)
		end
		
	end,0,-1)
end

-- 加载创建激活子面板
local function enabled_child_panels(self)
	if not self.panels or #self.panels < 1 then return end
	if not self.is_enabled and self.panelViews then
		for k, v in pairs( self.panelViews ) do
			if log then print( string.format( 'Viewfunc:enabled_child_panels panelName = %s  on_disable', k ) )end
			xpcall(function ()
				v:on_disable()
			end,function (msg)
				print_err(msg)
			end)
		end
		return
	end

	for i, v in ipairs( self.panels ) do
		local panelClass = type(v) == 'string' and require( v ) or v
		if not panelClass or not panelClass.name then
			if not panelClass.name then
				print( string.format( '<color=red>Viewfunc:enabled_child_panels panelClass.name is nil 面板的"name"为空!  %s</color>', v ) )
			else
				print( string.format( '<color=red>Viewfunc:enabled_child_panels invalid lua %s</color>', v ) )
			end
		elseif self.panelViews and self.panelViews[panelClass.name] then
			if log then print( string.format( 'Viewfunc:enabled_child_panels panelName = "%s"  on_enabled..', panelClass.name ) ) end

			xpcall(function ()
				self.panelViews[panelClass.name]:on_enabled()
			end,function (msg)
				print_err(msg)
			end)
			
		else
			if log then print( string.format( 'Viewfunc:enabled_child_panels panelName = "%s"  loaded..', panelClass.name ) )end
			--self:loadView(panelClass.prefab,panelClass,on_child_panel_load_complete)
			on_child_panel_load(self,panelClass.prefab,panelClass,on_main_object_load_complete)
		end
	end
end

local tip = '视图 <color=red>%s</color> 没有定义正确的 <color=red>预置件</color> 路径！\n请将 <color=red>%s.lua</color> 文件头部位置中的 <color=red>%s.prefab 属性</color> 设置正确的预置件名字！' 
local function init_defulat_view(self)
	self.inject.content.text = string.format( tip, self.name,self.name,self.name )
end


-- 主视图加载完成
local function on_main_object_load_complete( self,assetName,async,data)
	if not self.is_open or self.is_dispose then return end
	if log then print( string.format( 'Viewfunc:on_main_object_load_complete assetName = %s  loadFinish ', assetName ) )end


	if not async:IsSucceed() then
		print( string.format( '<color=red>Viewfunc:on_main_object_load_complete. asset is nil  assetName = "%s"  error = "%s"</color>', assetName,async.Message ) )
		return
	end

	-- 主对象
	if assetName == self.prefab then
		if self.mainObject ~= nil then return end
		self.mainObject = async:Instantiate(self.transform)
		self.mainTransform = self.mainObject.transform

		-- 先将窗口禁掉等内容加载完后一起
		if not string.isEmptyOrNull(self.cprefab) then self.mainObject:SetActive(false)end
	end

	-- 内容对象
	if not string.isEmptyOrNull(self.cprefab) and assetName == self.cprefab  then
		if self.contentObject then return end

		local contentTs = self:find_component(nil,self.content)
		if not contentTs then
			print( string.format( '<color=red>Viewfunc:on_main_object_load_complete. content is nil 主对象下没有 content 节点  prefab = "%s"  cprefab = "%s"</color>', self.prefab,self.cprefab ) )
			return
		end		

		self.contentObject = async:Instantiate(contentTs)
		self.mainObject:SetActive(true)
	end


	-- 如果内容对象未加载完
	if self.mainObject and not string.isEmptyOrNull(self.cprefab) and not self.contentObject then
		return
	end

	
	local lError = false
	xpcall(function ()
		self:on_initView()
		self:on_open_refresh()
	end,function (msg)
		print_err(msg)
		lError = true
	end)
	

	-- 若没有指向正确的视图预置
	if 'GUI_Default_View.prefab' == self.prefab then init_defulat_view(self) end
	
	self:openEffect(self.on_open)
	
	if(self.uiloadDone)then
		self.uiloadDone()
	end

	if lError then self:close() end
end

local function preInstanceObj(self,list,nextFun)
	
	if list == nil or #list == 0 then 
		nextFun()
		return
	end
	
	local asetInfo 		= table.remove(list,1);
	local objName  		= asetInfo[1]
	local poolName    	= asetInfo[2];
	
	local gop = GopManager.Instance:TryGet(poolName)
	local bhad = gop:HadRubbish(objName);
	if(bhad)then
		--对象池已经拥有
		preInstanceObj(self,list,nextFun)
	else

		local request = CAssetUtility.LoadAsset(objName,typeGameObject,true)
		
		if not request then 
			preInstanceObj(self,list,nextFun)
		end
		
		local checkLoad_id = 0
		checkLoad_id = self:add_timer(function ()
			
			if request:IsDone() then
				self:del_timer(checkLoad_id)
				
				if log then print( string.format( 'Viewfunc: preview create gameObject assetName = %s  loadFinish ', objName ) )end

				if request:IsSucceed() then
					--创建一个放进对象池
					local obj = request:Instantiate(nil)
					gop:Release(obj,objName)
				end


				preInstanceObj(self,list,nextFun)
			end
			
		end,0,-1)
		
--		self:loadView(objName,nil,function (self,assetName,async,data)
--			
--			if log then print( string.format( 'Viewfunc: preview create gameObject assetName = %s  loadFinish ', assetName ) )end
--
--			if request:IsSucceed() then
--				创建一个放进对象池
--				local obj = async:Instantiate(nil)
--				gop:Release(obj,assetName)
--			end
--
--
--			preInstanceObj(self,list,nextFun)
--		end)
	end

end



local function load_assets(self,list,nextFun)
	
	if list == nil or #list == 0 then 
		nextFun()
		return
	end
	
	
	local info = table.remove(list,1)
	
	local request  = CAssetUtility.PreLoadAsset(info,true)

	local checkLoad_id = 0
	checkLoad_id = self:add_timer(function ()
		
		if request:IsDone() then
			
			self:del_timer(checkLoad_id)
			
			if #list > 0 then
				load_assets(self,list,nextFun)
			else
				nextFun()
			end
		end
		
	end,0,-1)


	
end


local function loadAssetsAndView(self,assets)
	
	if self._preAssetList then
		local list = {}
		for i,v in ipairs(self._preAssetList) do
			table.insert(list,v) 
		end
		
		load_assets(self,list,function ()
			
			self:loadView(assets,nil,on_main_object_load_complete)	
		end)
		
		return
	end
	

	self:loadView(assets,nil,on_main_object_load_complete)	
	
end


-- 开始加载主视图
local function load_views(self)

	local needLoad = not self.mainObject or (not string.isEmptyOrNull(self.cprefab) and not self.contentObject )
	if needLoad then

		if log then print( string.format( 'Viewfunc:load_views prefab = "%s"  loaded..', self.prefab ) )end
		local assets = {}

		-- 需要加载主对象
		if not self.mainObject then table.insert( assets, self.prefab )end 
 		-- 需要加载内容对象
		if not string.isEmptyOrNull(self.cprefab) and not self.contentObject then table.insert( assets, self.cprefab)end


		if(self._preGameObjects)then
			
			local _prObjs = {}
			for i,v in ipairs(self._preGameObjects) do
				table.insert(_prObjs,v) 
			end
			
			preInstanceObj(self,_prObjs,function ()
				loadAssetsAndView(self,assets)
			end)	
			
		else

			loadAssetsAndView(self,assets)
		end
		
		
	else

		xpcall(function ()
			self:on_open_refresh()
		end,function (msg)
			print_err(msg)
		end)

		--界面未完全打开或者在关闭过程中
		if not self.is_enabled or self.is_closeing then
			self:openEffect(self.on_open)
		else
			-- 刷新视图
			self:on_open()
		end
		
		self:add_timer(function ()
			if(self.uiloadDone)then
				self.uiloadDone();
			end
		end,0,1)
		
	end


end

-- 销毁视图
local function dispose_views(self)
	if self.panelViews then
		for _, v in pairs( self.panelViews ) do
			if v.gameObject then
				CAssetUtility.DestroyInstance(v.gameObject)
			end
			v:dispose() 
		end
		self.panelViews = nil
	end

	if self.mainObject then
		CAssetUtility.DestroyInstance(self.mainObject)
		self.mainObject 	= nil
		self.mainTransform 	= nil
	end

	if self.contentObject then
		CAssetUtility.DestroyInstance(self.contentObject)
		self.contentObject 	= nil
	end
end


return
{
	load_views = load_views,
	enabled_views = enabled_child_panels,
	dispose_views = dispose_views,
}