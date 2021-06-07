Util = Util or {}
local cache = require( 'game.defines.MDefine' ).cache
local cfg = require( 'game.defines.MDefine' ).cfg

--处理不同职业物品
Util.filterRewardByCareer = function(strReward)
    local rewards = string.formatToArray(strReward)
    local career =cache.role:Career()
    local tbl = {}
    for _,reward in ipairs(rewards) do
        if reward[3] == nil or reward[3] == tostring(career) then
            table.insert(tbl,reward)
        end
    end
    return tbl
end

--将[100100,1],[100100,1] 转换为goodItem数据
Util.GoodDataConvert = function(data)
    local tab = {}
    for i,v in ipairs(data) do
        if not v.itemCode or not v.amount then
            local tempTab = cfg.itemConfig.getItemDataByCode(v[1],{itemAmount = v[2]})   --转换数据
            table.insert(tab,tempTab)
        else
            local tempTab = cfg.itemConfig.getItemDataByCode(v.itemCode,{itemAmount = v.amount})   --转换数据
            table.insert(tab,tempTab)
        end
    end
    return tab
end

Util.GetFirstFirday = function(date)
    local fridayDt = 0
    local dd = date.day-math.floor(date.day/7)*7

    if date.wday >1 and date.wday<=5 then
        fridayDt = dd+(5-date.wday)
    elseif date.wday == 6 or date.wday == 7 then
        if dd>2 then
            fridayDt = dd-2
        else
            fridayDt = dd+5
        end
    elseif date.wday == 1 then
        if dd>3 then
            fridayDt = dd-3
        else
            fridayDt = dd+4
        end
    end
    return fridayDt
end

Util.len = function(tbl)
    local n = 0
    for k,v in pairs(tbl) do
        n = n + 1
    end
    return n
end

-- 使用 ListToMap(tt, "id") 后将 tt = {
--  {id = 2, name = "23", sex = 0},
--  {id = 3, name = "23", sex = 1},
-- }
-- 变成tt = {
--  [2] = {id = 2, name = "23", sex = 0},
--  [3] = {id = 3, name = "23", sex = 1},
-- }
Util.ListToMap = function(list, ...)
    if nil == list then
        print_err("ListToMap:传的表为空")
        return nil
    end

    local map = {}
    local key_list = {...}
    local max_depth = #key_list

    if max_depth <= 0 then
        print_err("ListToMap:给表传的k为空")
        return nil
    end

    local function parse_item(t, item, depth)
        local key_name = key_list[depth]
        local key = item[key_name]
        if nil == t[key] then
            t[key] = {}
        end

        if depth < max_depth then
            parse_item(t[key], item, depth + 1)
        else
            t[key] = item
        end
    end

    for i,v in ipairs(list) do
        parse_item(map, v, 1)
    end

    return map
end

-- 使用 ListToMapList(tt, "id") 后将 tt = {
--  {id = 2, name = "23", sex = 0},
--  {id = 2, name = "23", sex = 1},
--  {id = 3, name = "23", sex = 0},
--  {id = 3, name = "23", sex = 1},
-- }
-- 变成tt = {
--  [2] = {{id = 2, name = "23", sex = 0},
--         {id = 2, name = "23", sex = 1},}

--  [3] = {{id = 3, name = "23", sex = 0},
--         {id = 3, name = "23", sex = 1},}
-- }
Util.ListToMapList = function(list, ...)
    if nil == list then
        print_err("ListToMapList:传的字典为空")
        print(debug.traceback())
        return nil
    end

    local map = {}
    local key_list = {...}
    local max_depth = #key_list

    if max_depth <= 0 then
        print_err("ListToMapList:给字典传的k为空")
        print(debug.traceback())
        return nil
    end

    local function parse_item(t, item, depth)
        local key_name = key_list[depth]
        local key = item[key_name]
        if nil == t[key] then
            t[key] = {}
        end

        if depth < max_depth then
            parse_item(t[key], item, depth + 1)
        else
            table.insert(t[key], item)
        end
    end

    for i,v in ipairs(list) do
        parse_item(map, v, 1)
    end

    return map
end

Util.ListToMapListToIter = function(iter, ...)
    if nil == iter then
        print_err("ListToMapList:传的迭代器方法为空")
        print(debug.traceback())
        return nil
    end

    local map = {}
    local key_list = {...}
    local max_depth = #key_list

    if max_depth <= 0 then
        print_err("ListToMapList:给字典传的k为空")
        print(debug.traceback())
        return nil
    end

    local function parse_item(t, item, depth)
        local key_name = key_list[depth]
        local key = item[key_name]
        if nil == t[key] then
            t[key] = {}
        end

        if depth < max_depth then
            parse_item(t[key], item, depth + 1)
        else
            table.insert(t[key], item)
        end
    end

    for i,v in iter() do
        parse_item(map, v, 1)
    end

    return map
end
