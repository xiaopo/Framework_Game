local colors = 
{
    white = "FFFFFFFF",
    red = "FF0000FF",
    green = "00FF95FF",
    blue = "0095FFFF",
    yellow = "FFF465FF",
    black = "000000FF",
    pink = "FF9F9FFF",
}


local getDebugInfo = function(level)
    local luaPath = ""
    local line = ""

    local info = debug.getinfo(level+1, "S")
    if next(info) and info.short_src then
        luaPath = string.match(info.short_src, "/A_Scripts/Lua/.+%.lua") or ""
        luaPath = "打印位置：..."..luaPath 
    end

    local lineInfo = debug.getinfo(level+1, "l")
    if next(lineInfo) and lineInfo.currentline then
        line = lineInfo.currentline
    end
    return luaPath, line
end

function dump(value, desciption, nesting, isXprint, colorKey)
    -- xprint( debug.traceback(  ) )
    if type(nesting) ~= "number" then nesting = 3 end
    
    local lookupTable = {}
    local result = {}


    local function _v(v)
        if type(v) == "string" then
            v = "\"" .. v .. "\""
        end
        return tostring(v)
    end

    local function _dump(value, desciption, indent, nest, keylen)
        desciption = desciption or "<var>"
        local spc = ""
        if type(keylen) == "number" then
            spc = string.rep(" ", keylen - string.len(_v(desciption)))
        end
        if type(value) ~= "table" then
            result[#result +1 ] = string.format("%s%s%s = %s", indent, _v(desciption), spc, _v(value))
        elseif lookupTable[value] then
            result[#result +1 ] = string.format("%s%s%s = *REF*", indent, desciption, spc)
        else
            lookupTable[value] = true
            if nest > nesting then
                result[#result +1 ] = string.format("%s%s = *MAX NESTING*", indent, desciption)
            else
                result[#result +1 ] = string.format("%s%s = {", indent, _v(desciption))
                local indent2 = indent.."    "
                local keys = {}
                local keylen = 0
                local values = {}
                for k, v in pairs(value) do
                    keys[#keys + 1] = k
                    local vk = _v(k)
                    local vkl = string.len(vk)
                    if vkl > keylen then keylen = vkl end
                    values[k] = v
                end
                table.sort(keys, function(a, b)
                    if type(a) == "number" and type(b) == "number" then
                        return a < b
                    else
                        return tostring(a) < tostring(b)
                    end
                end)
                for i, k in ipairs(keys) do
                    _dump(values[k], k, indent2, nest + 1, keylen)
                end
                result[#result +1] = string.format("%s}", indent)
            end
        end
    end

    local luaPath, lineNum = "",""
    if isXprint then 
        luaPath, lineNum = getDebugInfo(3) 
    end      
    _dump(value, desciption, "- ", 1)

    for i, line in ipairs(result) do
        if isXprint then
            if colorKey and colors[colorKey] then 
                 line = string.format("<color=#%s>%s</color>", colors[colorKey], line) 
            end 
            line = line .. '\n\n' .. luaPath .. ":" .. lineNum 
            
        end
        print(line)
    end
end

function dump_err()
    
end

function delog(value,signal)
    -- if CS.UnityEngine.Application.isEditor then
    --     local str = "delog  "
    --     if signal then
    --         str = str..signal..":" 
    --     end
    --     local debugStr = ""
    --     if type(value) == "string" then
    --        debugStr = str.."'" ..value .. "'"
    --     elseif type(value) == "number" then
    --         debugStr = str..tostring(value)
    --     elseif type(value) == "table" then
    --         if value ~= nil then
    --             debugStr = str..table.toString(value)
    --         else
    --             debugStr = str.."value is nil"
    --         end
    --     else
    --        debugStr = str..tostring(value)
    --     end
    --     -- debugStr = "<color=red>" .. debugStr .. "</color>"
    --     print(debug.traceback(debugStr))
    -- end    
end


function XDump(_value,signal,colorKey)
    local color = colorKey or 'blue'
    local str = "打印开始: "
    local luaPath, line = getDebugInfo(2)
    if signal and type(signal) ~= 'table' then
        str = str..signal..":"
    end
    local bool = false
    local value = "value is nil"
    local colorStr = colors[color] or '0095FFFF'
    if type(_value) == "string" then
        value = _value
    elseif type(_value) == "number" then
        value = tonumber(_value)
    elseif type(_value) == "table" then
        if _value ~= nil then
            value = table.toString(_value)
            bool = true
        end
    else
        value = _value
    end
    if bool then
        print(string.format('<color=#0095FFFF>%s</color>',str))
        print(string.format('<color=#%s>%s</color>',colorStr,value))
        print(string.format('<color=#0095FFFF>%s:%s</color>',luaPath,line))
    else
        print(string.format('<color=#%s>%s%s %s:%s</color>',colorStr,str,value,luaPath,line))
    end
end

--function xdump(value, desciption, colorKey, nesting)
--    dump(value, desciption, nesting, true, colorKey)
--end

function xprint(value, desciption, colorKey)
    local valueStr = value
    local desciptionStr = desciption
 
    valueStr = valueStr ~= nil and tostring(valueStr) or ""
    desciptionStr = desciptionStr ~= nil and tostring(desciptionStr) or ""

    local luaPath, line = getDebugInfo(2) 

    local colorStr = colors[colorKey] 
    if colorStr then 
        valueStr = string.format("<color=#%s>%s</color>", colorStr, valueStr) 
        desciptionStr = string.format("<color=#%s>%s</color>", colorStr, desciptionStr) 
    end 

    local content = valueStr .. "   " .. desciptionStr .. '\n\n' .. luaPath .. ":" .. line 
    print(content)  
end

