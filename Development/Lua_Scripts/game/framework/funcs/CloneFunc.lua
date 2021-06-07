--克隆 深度复制对象
local rawset = rawset;
local setmetatable = setmetatable;
local getmetatable = getmetatable;
local pairs = pairs;
local type = type;
local function copyObj( lookup_table,object )
	if type( object ) ~= "table" then
		return object
	elseif lookup_table[object] then
		return lookup_table[object]
	end
	
	local new_table = {}
	lookup_table[object] = new_table
	for key, value in pairs( object ) do
		new_table[copyObj(lookup_table, key )] = copyObj( lookup_table,value )
	end
	
	return setmetatable( new_table, getmetatable( object ) )
end
	
function deepcopy( object )
    local lookup_table = {}

    return copyObj( lookup_table,object )
end

local function _copy(lookup_table,object)
	if type(object) ~= "table" then
		return object
	elseif lookup_table[object] then
		return lookup_table[object]
	end
	local new_table = {}
	lookup_table[object] = new_table
	for key, value in pairs(object) do
		new_table[_copy(lookup_table,key)] = _copy(lookup_table,value)
	end
	return setmetatable(new_table, getmetatable(object))
end

function clone(object)
    local lookup_table = {}

    return _copy(lookup_table,object)
end


--[[
return
{
	deepcopy = deepcopy,
	clone 	 = clone
}--]]