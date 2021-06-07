local Linqnear = class()
function Linqnear:ctor(_table)
    assert( _table, 'Linqnear:ctor. table is nil ' )
    self._table = _table
    -- setmetatable(self,{__index = _table ,__newindex = _table})
end

function Linqnear:defaultPredicate(predicate,struct)
    if predicate and type(predicate) ~= "function" then
        local val = predicate
        predicate = function(arg) return arg == val end
    else
        predicate = predicate or function(arg,index) return arg end
    end
    return predicate ,struct or function(arg) return arg end
end

function Linqnear:length()
    return #self._table
end

function Linqnear:dopack(list)
    local meta
    meta = {
        __index = function(t,k)
            if not rawget(meta,"__lq") then
                rawset(meta,"__lq",Linqnear(list))
            end
            return rawget(meta,"__lq")[k]
        end
    }
    return setmetatable(list,meta)
end

----------------------------------------paris near
function Linqnear:all(predicate) 
    local predicate = self:defaultPredicate(predicate)
    for i,v in pairs(self._table) do
        if not predicate(v,i) then return false end
    end
    return true
end

function Linqnear:any(predicate) 
    local predicate = self:defaultPredicate(predicate)
    for i,v in pairs(self._table) do
        if predicate(v,i) then return true end
    end
    return false
end

function Linqnear:max(predicate)
    local predicate = self:defaultPredicate(predicate)
    local temp,index = 2^(-64)
    for i,v in pairs(self._table) do
        if predicate(v,i) > temp then 
            temp,index = v,i
        end
    end
    return temp,index
end
function Linqnear:min(predicate)
    local predicate = self:defaultPredicate(predicate)
    local temp,index = 2^(64)
    for i,v in pairs(self._table) do
        if predicate(v,i) < temp then 
            temp,index = v,i
        end
    end
    return temp,index
end

function Linqnear:find(predicate,struct) 
    local predicate,struct = self:defaultPredicate(predicate,struct)
    for i,v in pairs(self._table) do
        if predicate(v,i) then return struct(v,i),i end
    end
    return nil
end



function Linqnear:where(predicate,struct) 
    local predicate,struct = self:defaultPredicate(predicate,struct)
    local temp = {}
    for i,v in pairs(self._table) do
        if predicate(v,i) then table.insert(temp,struct(v,i)) end
    end
    return self:dopack(temp)
end

function Linqnear:finds(...)
    return self:where(...)
end

----------------------------------------iparis near
function Linqnear:iall(predicate) 
    local predicate = self:defaultPredicate(predicate)
    for i,v in ipairs(self._table) do
        if not predicate(v,i) then return false end
    end
    return true
end

function Linqnear:iany(predicate) 
    local predicate = self:defaultPredicate(predicate)
    for i,v in ipairs(self._table) do
        if predicate(v,i) then return true end
    end
    return false
end

function Linqnear:imax(predicate)
    local predicate = self:defaultPredicate(predicate)
    local temp,index = 2^(-64)
    for i,v in ipairs(self._table) do
        if predicate(v,i) > temp then 
            temp,index = v,i
        end
    end
    return temp,index
end
function Linqnear:imin(predicate)
    local predicate = self:defaultPredicate(predicate)
    local temp,index = 2^(64)
    for i,v in ipairs(self._table) do
        if predicate(v,i) < temp then 
            temp,index = v,i
        end
    end
    return temp,index
end

function Linqnear:ifind(predicate,struct) 
    local predicate,struct = self:defaultPredicate(predicate,struct)
    for i,v in ipairs(self._table) do
        if predicate(v,i) then return struct(v,i),i end
    end
    return nil
end

function Linqnear:iwhere(predicate,struct) 
    local predicate,struct = self:defaultPredicate(predicate,struct)
    local temp = {}
    for i,v in ipairs(self._table) do
        if predicate(v,i) then table.insert(temp,struct(v,i)) end
    end
    return self:dopack(temp)
end

function Linqnear:ifinds(...)
    return self:iwhere(...)
end

function Linqnear:ToArray()
    return self._table
end

-----------------------------------extend
function Linqnear:sort(func)
    table.sort(self._table,func)
end

function Linqnear:Do(predicate)
    for i,v in pairs(self._table) do
        predicate(v,i)
    end
end
function Linqnear:iDo(predicate)
    for i,v in ipairs(self._table) do
        predicate(v,i)
    end
end
    
function Linqnear:pairs()
    return next,self._table,nil
end
function Linqnear.__ilter(t,i)
    i = i + 1
    local v = t[i]
    if v then
        return i,v
    end
end
function Linqnear:ipairs()
    return self.__ilter,self._table,0
end

return Linqnear
