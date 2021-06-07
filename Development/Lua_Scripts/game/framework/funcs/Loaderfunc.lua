local util = require( 'xlua.util' ) 
local yield = require( 'xlua.coroutine' ).yield

local typeGameObject = typeof(CS.UnityEngine.GameObject)

-- atonce 立即执行加载，否则等到下一帧
-- gameObject 需要显示加载圈的对象
local async_load = util.async_to_sync(function(assetName,frameQueue,atonce,gameObject,view,fun)
	local request = CAssetUtility.LoadAsset(assetName,typeGameObject,true)
	if not request then
		fun(nil)
		print( string.format( '<color=red>Loaderfunc:async_load. request is nil assetName = "%s" </color>', assetName ) )
		return
	end
	
	-- 立即执行
	if atonce then
		--request:Update()
	end
	
	if frameQueue and frameQueue > 0 then for i = 1, frameQueue do yield(0) end end
	
	yield(request)
	--等一帧
	yield(0);
	
	fun(request)
end)


local function load_ui(self,assetName,data,func,frameQueue)
	local callback = func or self.on_loadComplete

	if not callback then
		-- 无回调
		print( string.format( '<color=red>Loaderfunc:load_ui. callback is nil assetName = "%s" </color>', type(assetName) == 'table' and table.tostring( assetName ) or assetName ) )
		return
	end

	if type(assetName) == 'table' then
		-- 多个顺序加载
		for i, v in ipairs( assetName ) do
			--只有第一个对象才会有加载
			local gameObject = false 
			if i == 1 then 
				gameObject = self.gameObject ~= nil and self.gameObject or false 
			end
			
			callback(self,v,async_load(v,frameQueue or 0,true,gameObject,self),data)
			-- yield(CS.UnityEngine.WaitForSeconds(5))  测试异步延迟异常
		end
	else
		callback(self,assetName,async_load(assetName,frameQueue or 0,true,self.gameObject ~= nil and self.gameObject or false,self),data)
	end
end


local function load(self,assetName,data,func,frameQueue)
	if func then
		func(self,assetName,async_load(assetName,frameQueue or 0,false,false,false),data)
	elseif self.on_loadComplete then
		self:on_loadComplete(assetName,async_load(assetName,frameQueue or 0,false,false,false),data)
	else
		-- 无回调
		print( string.format( '<color=red>Loaderfunc:load. callback is nil assetName = "%s" </color>', assetName ) )
	end
end


return
{
	load = util.coroutine_call(load),
	load_ui = util.coroutine_call(load_ui),
}