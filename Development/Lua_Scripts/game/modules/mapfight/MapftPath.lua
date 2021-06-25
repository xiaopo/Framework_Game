local mapfting = "game.modules.mapfight.";

local etPaths =
{
	["MapfightProgram"]					=  mapfting .. "MapfightProgram";
}


function doLuamt(cName)
	
	local path = etPaths[cName];
	if(not path)then
		error("请在Mapfight 填写该文件的路径 " .. cName );
	end
	return require(path);
end


