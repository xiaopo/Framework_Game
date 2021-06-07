local tags ={} 
tags.Player 		= 'Player'
tags.GUICamera 		= 'GUICamera'
tags.MapCamera 		= 'MapCamera'


local layers = {}
layers.UI 				= 'UI'
layers.UIHide 			= 'UIHide'
layers.UIModel 			= 'UIModel'

layers.Wall             = 'Wall'          	    --墙
layers.Water   			= 'Water'   	  	    --水
layers.Floor  			= 'Floor' 	 			--地板
layers.Entity 			= 'Entity'  	 		--实体
-- layers.EntityLight 		= 'EntityLight' 		--受光实体  不再需要
layers.FightEffect      = 'FightEffect'  		--战斗特效层

	
local sortinglayers ={} 

local NameToLayer = CS.UnityEngine.LayerMask.NameToLayer;
-- 检查层是否为实体层
local function IsEntityLayer(layer)
	return layer == NameToLayer(layers.Entity)
end

return
{
	tags = tags,
	layers = layers,
	sortinglayers = sortinglayers,
	IsEntityLayer = IsEntityLayer,

}