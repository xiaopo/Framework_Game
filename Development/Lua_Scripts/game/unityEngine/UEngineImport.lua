local basePath = 'game.unityEngine.';
local extendsLua = {
	'Time',
	'Mathf',
	'Vector3',
	'Quaternion',
	'Vector2',
}

for k,v in pairs(extendsLua) do
	
	_G[v] = require( basePath .. v );
end

--使用方式
--[[
	local v1 = Vector2.one;
	v1 = v1 + Vector2.New(2,2);
	print("VV:",v1.x,v1.y);

	local v3 = Vector3.one;
	local v2 = Vector3.New(2,3,4);
	local v4 = (v3 + v2) * 10;
	print("SS:",v4.x,v4.y,v4.z);
	v4:SetNormalize();
	local fightQua = Quaternion.LookRotation(v4);
	print("QQ:",fightQua.x,fightQua.y,fightQua.z,fightQua.w)
--]]