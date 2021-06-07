-- 简单的字典实现  有序字典
--for k, v in dict:iter() do
--
--end
-- 
-- 



local mt = {}
mt.__index = mt
mt.__call = function()
	local obj 	= {}
	obj.keys 	= {}
	obj.values 	= {}
	setmetatable( obj, mt ) 
	return obj
end

local dict = setmetatable( {}, mt )

function mt:add(key,value)
	if self.values[key] then
		-- 增加已经存在的值
		return
	else
		table.insert( self.keys,key)
		self.values[key] = value
	end
end


function mt:remove(key)
	if not self.values[key] then
		-- 移除不存在的值
		return
	else
		self.values[key] = nil
		for i, v in ipairs( self.keys ) do
			if v == key then
				table.remove( self.keys, i )
				return
			end
		end
	end
end

function mt:remove_idx(idx)
	local key = self.keys[idx]
	if key then
	 	table.remove( self.keys, idx )
	 	self.values[key] = nil
	end 
end

function mt:get(key)
	return self.values[key]
end

function mt:contains(key)
	return self.values[key] ~= nil
end

function mt:iter()
	local i = 0
	return function()
		i = i + 1
		local key = self.keys[i]
		if key then
			return key,self.values[key]
		end
	end
end


function mt:clear()
	self.keys   = {}
	self.values = {}
end


return dict