
--游戏地图切换逻辑
--龙跃

local CMapLoader = CS.Map.GameMapLoader()

local GameMapLoader = {}

--预加载场景资源到内存
function GameMapLoader.PreLoad(mapName)
	
	

end

function GameMapLoader:Progress()
	
	return CMapLoader
end

--加载并且显示场景

function GameMapLoader:LoadScene(mapName)
	print("=======================GameMapLoader:LoadScene===========================",Time.time,mapName)
	

	Dispatcher.dispatchEvent(EventDefine.MAP_CUT_BEGIN)
	--开启加载
	CMapLoader:LoadClamp(2,mapName)
	
	--加载完成
	CMapLoader.LoadComplete = function ()
		CMapLoader.LoadComplete = nil
		self:LoadComplte()
	end
	

end


function GameMapLoader:LoadComplte()
	print("=======================GameMapLoader:LoadComplte===========================",Time.time,mapName)
	
	Dispatcher.dispatchEvent(EventDefine.MAP_CUT_DONE)
	
end



_G.GameMapLoader = GameMapLoader

return GameMapLoader