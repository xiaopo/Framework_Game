--[[
--
-- 扩展系统类库 比如对table string进行扩展
-- 新增的是一些常用的函数
]]

--保留n位小数
function math.precise(nNum, n)

    if type(nNum) ~= "number" then
        return nNum;
    end
    n = n or 0;
    n = math.floor(n)
    if n < 0 then
        n = 0;
    end
    local nDecimal = 10 ^ n
    local nTemp = math.floor(nNum * nDecimal);
    local nRet = nTemp / nDecimal;
    return nRet;

end

--稳定的冒泡排序（由于lua自带排序为不稳定的快速排序）
function table.bubbleSort(tt, func)
    local flag = true
    for i = 1, #tt do
        if flag then
            flag = false
            for j = #tt, i + 1, -1 do
                if func(tt[j], tt[j - 1]) then
                    tt[j], tt[j - 1] = tt[j - 1], tt[j]
                    flag = true
                end
            end
        else
            break
        end
    end
end

function table.removeItem(tt,a)
    if(tt and a)then

        for i,v in ipairs(tt) do
           if(v == a)then
                table.remove(tt,i) 
                break;
            end;
        end

    end
end

--移除表中对应数据得到新表
function table.RemoveIndexGetNewTable(tt, tkey)
    local info = {}
    local isTable = type(tkey) == 'table'
    if tt or tkey then
        for i, v in pairs(tt) do
            if not isTable then
                if i ~= tkey then
                    table.insert(info, v)
                end
            else
                local isInsert = false
                for k, x in pairs(tkey) do
                    if (i ~= x) then
                        isInsert = true
                    else
                        isInsert = false
                        break
                    end
                end
                if isInsert then
                    table.insert(info, v)
                end
            end
        end
    else
        return tt
    end
    return info
end

--获取数组的长度
function table.Lenth(tab)
   if not tab then  return 0 end
   local lenth = 0

   for k,v in pairs(tab) do
       lenth = lenth + 1
   end
   return lenth
end


--将目标表拆分成指定大小的2维表
function table.BreakUpTable(tempData,size)
    local newData = {}
    local x = math.ceil(#tempData/size)
    for i = 1, x do
        local tempT = {}
        for k = (i-1)*size+1, i*size do
            if tempData[k] ~= nil then
                table.insert(tempT,tempData[k])
            end
        end
        table.insert(newData,tempT)
    end
    return newData
end

--转换为有序table
function table.toArray(a)

   local ntt = {};
    for i,v in pairs(a) do
        if(v ~= nil)then table.insert(ntt,v) end;
        a[i] = nil;
    end
    
    return ntt;
end

--转换为有序table
function table.toArray2(a)

   local ntt = {};
    for i,v in pairs(a) do
        if(v ~= nil)then table.insert(ntt,v) end;
    end
    
    return ntt;
end

function table.connect(a,b)
  local r={}
  for i=1,#a do
    r[i]=a[i]
  end
  for l=1,#b do
    r[l+#a]=b[l]
  end
  return r
end

--通过值检索是否存在
--list 即table表
--需要检索的value 可为table
function table.include(list , value)
    if(not list)then return false end;
    for k,v in pairs(list) do
        if v==value then 
            return true
        end
    end
    return false
end


--通过值检索是否存在
--list 即table表
--需要检索的value 可为table
function table.indexOf(list , value)
    for k,v in pairs(list) do
        if v==value then 
            return k
        end
    end
    return nil
end

--通过值检索是否存在
--list 即table表
--需要检索的value 可为table
function table.includeOf(list , value)
    for k,v in pairs(list) do
        for k1,v1 in pairs(v) do
            if v1 == value then
                 return k
            end
        end
    end
    return nil
end

function table.clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    if object then
        return _copy(object)
    else
        return object
    end
end

function table.nums(t)
    local count = 0
    for k, v in pairs(t) do
        count = count + 1
    end
    return count
end

function table.containKey( t, key )
    for k, v in pairs(t) do
        if key == k then
            return true;
        end
    end
    return false;
end

function table.containValue( t, value )
    for k, v in pairs(t) do
        if value == v then
            return true;
        end
    end
    return false;
end

function table.reverse(tbl)
    local newTbl = {}
    for i=#tbl,1,-1 do table.insert(newTbl, tbl[i])end
    return newTbl
end

function table.slice(tbl,startIdx,endIdx)
    if endIdx and endIdx > #tbl then return nil end
    endIdx = endIdx or #tbl
    local nArr = {} 
    for i = startIdx, endIdx do table.insert(nArr, tbl[i]) end
    return nArr
end

local function ToStringEx(value)
    if type(value)=='table' then
       return table.tostr(value)
    elseif type(value)=='string' then
        return value
    else
       return tostring(value)
    end
end

--表内元素转字符串
function table.vtostr(t)
    if t == nil then return "" end
   
    local retstr= ""
    for key,value in pairs(t) do
        retstr = retstr .. ToStringEx(value)
    end

    return retstr
end

--判断tab是不是空的
function table.isNull(tab)
    return not tab or next(tab) == nil
end

--tab安全移除
function table.removeEx(tab,fun)
    if table.isNull(tab) then return end
    for i = #tab, 1,-1 do
        if fun(tab[i],i) then
            table.remove(tab,i)
        end
    end
end

--求出int table中最小值
function table.getMiniInt(list)
    if not list or #list <= 0 then return 0 end

    table.sort(list)
    return list[1]
end

--字符串连接
function string.connect(...)
    local ta = {...}
    return table.concat(ta)
end

--字符分割
function string.split(str, delimiter)
    if str==nil or str=='' or delimiter==nil then
        return nil
    end
    
    local result = {}
    for match in (str..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match)
    end
    return result
end

function string.split2(szFullString, szSeparator)  
    local nFindStartIndex = 1  
    local nSplitIndex = 1  
    local nSplitArray = {}  
    while true do  
       local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)  
       if not nFindLastIndex then  
        nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))  
        break  
       end  
       nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)  
       nFindStartIndex = nFindLastIndex + string.len(szSeparator)  
       nSplitIndex = nSplitIndex + 1  
    end  
    return nSplitArray  
end 

function string.strSplit(str,delimeter)  
    local find, sub, insert = string.find, string.sub, table.insert  
    local res = {}  
    local start, start_pos, end_pos = 1, 1, 1  
    while true do  
        start_pos, end_pos = find(str, delimeter, start, true)  
        if not start_pos then  
            break  
        end  
        insert(res, sub(str, start, start_pos - 1))  
        start = end_pos + 1    
    end  
    insert(res, sub(str,start))  
    return res  
end 

-- 去除字符串两边的空格  
function string.trim(s)   
    return (string.gsub(s, "^%s*(.-)%s*$", "%1"))  
end  

--[[
    格式化秒为字符串
    00:00
--]]

function string.formatToTime2(num)
    local minute = math.floor(num / 60);
    local second = num %60;

    local str = "";

   if(minute > 0)then
        str = str .. ((minute > 9 and tostring(minute)) or "0" .. minute) ;
    else
        str = str .. "00" ;
    end
    str = str .. ":";
    
    if(second > 0)then
        str = str .. ((second > 9 and tostring(second)) or "0" .. second) ;
    else
        str = str .. "00" ; 
     end

     return str;
end

function string.formatToTime3(num)
    -- body
    local hour = math.floor(num/3600)
    local minute = math.floor((num%3600)/60)
    local h = ""
    local m = ""
    if(hour==0) then
        h = "00"
        elseif(hour <10) then
            h = "0"..tostring(hour)
        else
            h = h..tostring(hour)
    end

    if(minute==0) then
        m = "00"
        elseif(minute <10) then
            m = "0"..tostring(minute)
        else
            m = m..tostring(minute)
    end 
    return h.."小时"..m.."分"
end

function string.formatTotime4(num)
    local day = math.floor(num / (3600 * 24));
    local hour = math.floor(num % (3600 * 24) / 3600);
    
    return day .. "天" .. hour .."小时"
end

function string.formatToTime5(time)
    local str = '';
    if (time <= 0) then
        str = '0分钟';
    else
        if (time >= 3600) then
            if (time % 3600  == 0) then
                local h = math.ceil(time / 3600);
                str = h..'小时'..'0分钟';
            else
                local h = math.floor(time / 3600);
                local m = math.ceil(time % 3600 / 60);
                str = h..'小时'..m..'分钟';
            end
        else
            local m = math.ceil(time / 60);
            str = m..'分钟';
        end
    end
    return str;
end

function string.formatToTime6(time)
    local str = '';
    if (time <= 0) then
        str = '0分钟';
    else
        if (time >= 3600) then
            local h = math.floor(time/3600)
            str = h..'小时'
        else
            local m = math.ceil(time / 60);
            str = m..'分钟';
        end
    end
    return str;
end

--[[
    格式化秒为字符串
    type == 1  0天 00:00:00
    type == 2  0天 00時00分00秒   
--]]
function string.formatToTime(number,type)
    local num = number or 0;
    local day = math.floor(num / (3600 * 24));
    local hour = math.floor(num % (3600 * 24) / 3600);
    local minute = math.floor((num % (3600 * 24) % 3600) / 60);
    local second = (num % (3600 * 24) % 3600) % 60;

    local str = "";
    if(day > 0)then
        str = day .. "天";
    end

    if(hour > 0)then
        str = str .. ((hour > 9 and tostring(hour)) or "0" .. hour) ;
    else
        str = str .. "00"; 
    end

     if(type == 2)then
        str = str .. "时"
     else
        str = str .. ":"
     end

   
    if(minute > 0)then
          str = str .. ((minute > 9 and tostring(minute)) or "0" .. minute) ;
    else
          str = str .. "00" ;
    end

    if(type == 2)then
        str = str .. "分"
     else
        str = str .. ":"
     end
    if(second > 0)then
          str = str .. ((second > 9 and tostring(second)) or "0" .. second) ;
    else
        str = str .. "00" ; 
     end

     if(type == 2)then
        str = str .. "秒"
     end

     return str;
end 

--[[
    格式化秒为字符串
    a.＜3小時：xx：xx：xx
    b.≥3小時，且＜6小時：xx時xx分
    c.≥6小時，且＜24小時：xx時
    d.≥24小時：xx天xx時   
--]]
function string.formatCutdownTime(number)
    local num = number or 0;
    local day = math.floor(num / (3600 * 24));
    local hour = math.floor(num % (3600 * 24) / 3600);
    local minute = math.floor((num % (3600 * 24) % 3600) / 60);
    local second = (num % (3600 * 24) % 3600) % 60;

    local str = ""
    if(hour < 3)then
        str = ((hour > 9 and tostring(hour)) or "0" .. hour) .. ":" .. ((minute > 9 and tostring(minute)) or "0" .. minute) ..":"..((second > 9 and tostring(second)) or "0" .. second);
    elseif(hour >=3 and hour < 6)then
        str = ((hour > 9 and tostring(hour)) or "0" .. hour) .. "时"..  ((minute > 9 and tostring(minute)) or "0" .. minute) .."分";
    elseif(hour >=6 and hour < 24)then
         str = ((hour > 9 and tostring(hour)) or "0" .. hour) .. "时";
    elseif(hour >= 24)then
         str =  day .. "天" .. ((hour > 9 and tostring(hour)) or "0" .. hour) .. "时";
    end

     return str;
end 
--[[
    显示为“xx:xx"
    ③上述時间单位为“分：秒”。
--]]
function string.formatCutdownTime2(number)
    local num = number or 0;
    local minute = math.floor((num / 60));
    local second = num % 60;
 
    local str =  ((minute > 9 and tostring(minute)) or "0" .. minute) ..":"..((second > 9 and tostring(second)) or "0" .. second);
  
     return str;
end 

--[[
    显示为“xx:xx"
    ③上述時间单位为“時：分”。
--]]
function string.formatCutdownTime3(number)
    local num = number or 0;
    local hour = math.floor(num % (3600 * 24) / 3600);
    local minute = math.floor((num % (3600 * 24) % 3600) / 60);
    return (hour > 9 and tostring( hour ) or "0"..hour)..":"..(minute > 9 and tostring( minute ) or "0"..minute)
end

--是整数的话，不显示小数 ，不是整数的话，保留小数点后1位，四舍五入
function string.formatToFloat(str)
    if math.floor(str)<str then
        str = string.format("%.1f",str)
    end
    return str
end

--A.    小于10萬的数字，显示实际的位数：如12345；
--B.    10萬<=n<100萬的数字，显示以【萬】为单位，且后面保留1为小数，其余去尾显示（小数位为0時，不显示小数）；如123456显示为 12.3萬；120000显示为12萬；
--C.    ≥100萬的数字，不显示小数部分
function string.formatMoney3(num)
    if not num then return "" end
    local wan = 10000
    local tenwan = 100000
    local baiwan = 1000000

    local str = ""
    
    function checkIfZero()        
        local s1 = string.sub(str,1,#str - 5)
        local s2 = string.sub(str,#str-4,#str-3)
        local s3 = string.sub(str,#str-2,#str)
        if s2 == ".0" then
            str = s1 .. s3
        else
            str = s1 .. s2 .. s3
        end
    end

    if num >= baiwan then
        str = string.format("%d%s",math.floor(num/wan),"万")
    elseif num >= tenwan then
        str = str .. string.format("%.1f", (num/wan)) .. "万"
        checkIfZero()
    else
        str = tostring(num)
    end

    return str
end

--A.	小于10萬的数字，显示实际的位数：如12345；
--B.	≥10萬，＜1億的数字，显示以【萬】为单位，且后面保留1为小数，其余去尾显示（小数位为0時，不显示小数）；如123456显示为 12.3萬；120000显示为12萬；
--C.	≥1億的数字，显示以【億】为单位，且后面能保留2位小数，其余去尾显示（小数位数为0時，不显示）；如123456789显示为：1.23億；120000000显示为1.2億；
--D.    新增萬億规则同上
function string.formatNumberold(num)

    if(not num)then return "" end;
    local onum = num;
    local wan = 10000
    local tenwan = 100000;
    local yi =  100000000;
    local wanyi = 1000000000000
    local str = "";
    
    local function checkIfZero()
        local s1 = string.sub(str,1,#str - 2)
        local s2 = string.sub(str,#str-1,#str)
        if s2 == ".0" then
            str = s1 
        else
            str = s1 .. s2 
        end
    end

    local function checkIfOneZero(amount)
        str = str .. string.format("%.2f", (num/amount))
        local s1 = string.sub(str,1,#str -1)
        local s2 = string.sub(str,#str,#str)
        if s2 == "0" then
            str = s1
        else
            str = s1 .. s2
        end
        checkIfZero()
    end

    if num >= wanyi then
        checkIfOneZero(wanyi)
        str = str .. "万亿"
    elseif(num >= yi)then
        checkIfOneZero(yi)
        str = str .. "亿"
    elseif(num >= tenwan) then
        str = str .. string.format("%.1f", (num/wan))
        checkIfZero()
        str = str .. "万"
    else
        str = num..""
    end

    return str;
end

-- 1、12、123、1234
-- 1.23萬、12.34萬、123.4萬、1234萬
-- 1.23億、12.34億、123.4億、1234億
-- 1.23萬億、12.34萬億、123.4萬億、1234萬億
-- 如果小数点后是0，则不显示
function string.formatNumber(num,isItem)
    if(not num or type(num) ~= 'number')then return "" end;
    local shiwan = 100000
    local wan = 10000
    local yi =  100000000
    local wanyi = 1000000000000
    local str = ""

    local function subZero(withZero)
        local len = string.len(withZero)
        local charStr = string.sub(withZero,len,len)
        if charStr == "0" then
            withZero = string.sub(withZero,1,len-1)
            withZero = subZero(withZero)
        end

        return withZero
    end

    local function changeFormat(amount)
        num = num/amount
        --把字符串分为小数点前和小数点后
        local strArray = string.strSplit(tostring(num),".")  
        --先计算小数点前的位数
        local highLen = string.len(strArray[1])
        --若大于4
        if highLen >= 4 then
            str = string.sub(strArray[1],1,4)
        --若小于4 
        else
            --计算小数点后位数
            str = string.sub(strArray[1],1,highLen)
            local offset = 3 - highLen
            if offset == 2 or offset == 0 then
                offset = 1
            end
            if strArray[2] then
                local lowLen = string.len(strArray[2])
                if lowLen >= offset then
                    --去掉0
                    local subStr = subZero(string.sub(strArray[2],1,offset))
                    if subStr == "" then
                        str = str .. subStr
                    else
                        str = str ..".".. subStr
                    end
                else
                    local subStr =  subZero(string.sub(strArray[2],1,lowLen))
                    if subStr == "" then
                        str = str .. subStr
                    else
                        str = str ..".".. subStr
                    end
                end 
            end
        end 
    end

    if isItem then
        shiwan = 10000
    end

    if num >= wanyi then
        changeFormat(wanyi)
        str = str .. "万亿"
    elseif(num >= yi)then
        changeFormat(yi)
        str = str .. "亿"
    elseif(num >= shiwan) then
        changeFormat(wan)
        str = str .. "万"
    else
        --萬以下不用处理
        str = num..""
    end

    return str
end

--保留4位有效数字,是否保留小数
function string.formatBoold(num,isNum)
    
    if(not num)then return "" end;
    local shiwan = 100000
    local wan = 10000
    local yi =  100000000
    local wanyi = 1000000000000

    if num >= wanyi then
        num = num/wanyi
        str = string.GetPreciseDecimal(num,4).. "万亿"
    elseif(num >= yi)then
        num = num/yi
        str = string.GetPreciseDecimal(num,4).. "亿"
    elseif(num >= shiwan) then
        num = num/wan
        str = string.GetPreciseDecimal(num,4).. "万"
    else
        --萬以下不用处理
        if isNum ~= nil then 
            if isNum == false then
                num = math.ceil(num) 
            end
        end
        str = num..""
    end

    return str
end

function string.formatDamage(num,isNum)--伤害数值处理
    
    if(not num)then return "" end;
    local shiwan = 100000
    local wan = 10000
    local yi =  100000000
    local wanyi = 1000000000000

    if num >= wanyi then
        num = num/wanyi
        str = string.FormatNumWithoutZero(num).. "万亿"
    elseif(num >= yi)then
        num = num/yi
        str = string.FormatNumWithoutZero(num).. "亿"
    elseif(num >= shiwan) then
        num = num/wan
        str = string.FormatNumWithoutZero(num).. "万"
    else
        --萬以下不用处理
        if isNum ~= nil then 
            if isNum == false then
                num = math.ceil(num) 
            end
        end
        str = num..""
    end

    return str
end

function string.FormatNumWithoutZero(num) --去除小数位的0,保留两位小数
    if num <= 0 then
        return 0
    else
        local t1, t2 = math.modf(num-num%0.1)
        ---小数如果为0，则去掉
        if t2 > 0 then
            return num-num%0.1
        else
            return t1
        end
    end
end

--保留N位有效数字，不四舍五入,不符合规格返回nil
function string.GetPreciseDecimal(nNum,n)

    if type(nNum) == "table" or n <= 0 then
        return 
    end

    n = math.floor(n)

    local strArray = string.strSplit(tostring(nNum),".")
    local highLen = string.len(strArray[1])
    local lowLen = 0
    if strArray[2] then
        lowLen = string.len(strArray[2])
    end
    local str = ""
    if highLen < n then
        if strArray[2] then
            str = strArray[1] .. strArray[2]
        else
            str = strArray[1]
            for i=1,n-highLen do
                str = str .. "0"
            end
        end
    elseif highLen == n then
        str = strArray[1]
    else
        return
    end

    local decimal = string.sub(str,1,n)
    local strHigh = string.sub(decimal,1,highLen)
    local strLow = string.sub(decimal,highLen + 1,#decimal)
    
    if highLen < n then
        return strHigh .. "." ..strLow 
    else
        return strHigh
    end
end

--取N位小数 没有四舍五入
function GetPreciseDecimal(nNum, n)
	
	if type(nNum) ~= "number" then
        return nNum;
    end
    n = n or 0;
    n = math.floor(n)
    if n < 0 then
        n = 0;
    end
	
    local nDecimal = 10 ^ n
    local nTemp = math.floor(nNum * nDecimal);
    local nRet = nTemp / nDecimal;
	
    return nRet;
end


--格式化
--默认 萬
--格式起点 unit: 1萬， 2十萬，3百萬，4千萬，5億,6十億
function string.formatMoney(num,minUnit)
    if(not num or num == 0)then return "0" end;
    local str = "";
    local numss = {10000,100000,1000000,10000000,100000000,1000000000};

    if(not minUnit)then minUnit = 1; end;
    if(minUnit < 1)then minUnit = 1; end;
    if(minUnit > #numss)then minUnit = #numss; end
   
    local syi = numss[#numss]--单位億起点 十億开始
    local yi = numss[#numss-1]

    if(num >= syi)then
        str = math.floor(num/yi) .. "亿";
    elseif(num >= numss[minUnit])then
        str = math.floor(num/numss[1]) .. "万";
       -- local remain =  num%numss[1];
    else
        str = tostring(num);
    end

    return str;
end

-- 1、12、123、1234
-- 1.23万、12.34万、123.4万、1234万
-- 1.23亿、12.34亿、123.4亿、1234亿
-- 1.23万亿、12.34万亿、123.4万亿、1234万亿
-- 如果小数点后是0，则不显示
function string.formatMoney2(num,isItem,noDecimals)
    if(not num)then return "" end;
    local shiwan = 100000
    local wan = 10000
    local yi =  100000000
    local wanyi = 1000000000000
    local str = ""

    local function subZero(withZero)
        local len = string.len(withZero)
        local charStr = string.sub(withZero,len,len)
        if charStr == "0" then
            withZero = string.sub(withZero,1,len-1)
            withZero = subZero(withZero)
        end

        return withZero
    end

    local function changeFormat(amount,noDecimals)
        num = num/amount
        --把字符串分为小数点前和小数点后
        local strArray = string.strSplit(tostring(num),".")  
        --先计算小数点前的位数
        local highLen = string.len(strArray[1])
        if noDecimals then
            str = string.sub(strArray[1],1,highLen)
            return
        end
        --若大于4
        if highLen >= 4 then
            str = string.sub(strArray[1],1,4)
        --若小于4 
        else
            --计算小数点后位数
            str = string.sub(strArray[1],1,highLen)
            local offset = 4 - highLen
            if offset == 3 then
                offset = 2
            end
            if strArray[2] then
                local lowLen = string.len(strArray[2])
                if lowLen >= offset then
                    --去掉0
                    local subStr = subZero(string.sub(strArray[2],1,offset))
                    if subStr == "" then
                        str = str .. subStr
                    else
                        str = str ..".".. subStr
                    end
                else
                    local subStr =  subZero(string.sub(strArray[2],1,lowLen))
                    if subStr == "" then
                        str = str .. subStr
                    else
                        str = str ..".".. subStr
                    end
                end 
            end
        end 
    end

    if isItem then
        shiwan = 10000
    end

    if num >= wanyi then
        changeFormat(wanyi,noDecimals)
        str = str .. "万亿"
    elseif(num >= yi)then
        changeFormat(yi)
        str = str .. "亿"
    elseif(num >= shiwan) then
        changeFormat(wan)
        str = str .. "万"
    else
        --万以下不用处理
        str = num..""
    end

    return str
end

-- 把[a,b,c,d ...][1000,3,...] 转成table {{a,b,c,d ...},{1000,3 ...}}
function string.formatToArray(str)
    local r = {}
    if str and #str > 0 then
        for k in string.gmatch(str,"%b[]") do
            k=string.sub(k,2,-2)
            table.insert(r,string.split(k,","))
        end
    end
    return r
end

-- 把[a,b,c,d ...][1000,3,...] 转成table {{a,b,c,d ...},{1000,3 ...}}
--邮件js特殊使用
function string.formatToArray2(str)
    local r = {}
    if str and #str > 0 then
        for k in string.gmatch(str,"%b[]") do
            k = string.sub(k,2,-2)
            local js = string.match(k,"%b{}")
            k = string.gsub(k,",%b{}","")
            local split = string.split(k,",")
            table.insert(split,js)
            table.insert(r,split)
        end
    end
    return r
end

function string.isUrl(path)
    if(not path)then return false end;
      local path = StringUtil.trim(path)
    local urlTyps = {".jpg",".png",".swf",".gif"};

    for i,v in ipairs(urlTyps) do
        local s,d = string.find(path,v);
        if(s and d )then
            if(s ~= 1)then
                return true;
            end
        end
    end

    return false;
end

-- 拆分出单个字符
function string.toChars(str)
    -- 主要用了Unicode(UTF-8)编码的原理分隔字符串
    -- 简单来说就是每个字符的第一位定义了该字符占据了多少字节
    -- UTF-8的编码：它是一种变长的编码方式
    -- 对于单字节的符号，字节的第一位设为0，后面7位为这个符号的unicode码。因此对于英语字母，UTF-8编码和ASCII码是相同的。
    -- 对于n字节的符号（n>1），第一个字节的前n位都设为1，第n+1位设为0，后面字节的前两位一律设为10。
    -- 剩下的没有提及的二进制位，全部为这个符号的unicode码。
    local list = {}
    if(not str)then
        return list;
    end
    
    local len = string.len(str)
    local i = 1
    while i <= len do
        local c = string.byte(str, i)
        local shift = 1
        if c > 0 and c <= 127 then
            shift = 1
        elseif (c >= 192 and c <= 223) then
            shift = 2
        elseif (c >= 224 and c <= 239) then
            shift = 3
        elseif (c >= 240 and c <= 247) then
            shift = 4
        end
        local char = string.sub(str, i, i+shift-1)
        i = i + shift
        table.insert(list, char)
    end
    return list
end

--[[统计str中substr出现的次数。from, to用于指定起始位置，缺省状态下from为1，to为字符串长度。成功返回统计个数，失败返回nil和失败信息]]
function string.count(str, substr, from, to)
    if str == nil or substr == nil then
        return nil, "the string or the sub-string parameter is nil"
    end
    from = from or 1
    if to == nil or to > string.len(str) then
        to = string.len(str)
    end
    local str_tmp = string.sub(str, from ,to)
    local n = 0
    for _, _ in string.gmatch(str_tmp, substr) do
        n = n + 1
    end
    return n
end

--获得字符个数 
function string.getWordCount(str)
    local fontSize = 20
    local lenInByte = #str
    local count = 0
    local i = 1
    local h = 0
    while true do
        local curByte = string.byte(str, i)
        if i > lenInByte then
            break
        end
        local byteCount = 1
        if curByte > 0 and curByte < 128 then
            byteCount = 1
        elseif curByte>=128 and curByte<224 then
            byteCount = 2
        elseif curByte>=224 and curByte<240 then
            byteCount = 3
            h = h + 1
        elseif curByte>=240 and curByte<=247 then
            byteCount = 4
            h = h + 1
        else
            break
        end
        i = i + byteCount
        count = count + 1
    end
    return (count-h) + h*2
end



--utf8 字符截取
--s字符 n截取长度
function string.subUTF8(s, n)  
  local _byte = string.byte(s, n+1)  
  if not _byte then return s end  
  if _byte >= 128 and _byte < 192 then  
    return string.subUTF8(s, n-1)  
  end  
  return string.sub(s, 1, n)  
end

function string.GetStringA_Z_Chinese(str)
    local ss = {}
    local k = 1
    local index = 0
    while true do
        if k > #str then
            return true
        end
        index = index + 1
        local c = string.byte(str, k)
        if not c then
            return false,index
        end
        if c < 192 then
            if (c >= 65 and c <= 90) or (c >= 97 and c <= 122) then
                table.insert(ss, string.char(c))
            else
                return false,index
            end
            k = k + 1
        elseif c < 224 then
            k = k + 2
            return false,index
        elseif c < 240 then
            if c >= 228 and c <= 233 then
                local c1 = string.byte(str, k + 1)
                local c2 = string.byte(str, k + 2)
                if c1 and c2 then
                    local a1, a2, a3, a4 = 128, 191, 128, 191
                    if c == 228 then
                        a1 = 184
                    elseif c == 233 then
                        a2, a4 = 190, c1 ~= 190 and 191 or 165
                    end
                    if c1 >= a1 and c1 <= a2 and c2 >= a3 and c2 <= a4 then
                        table.insert(ss, string.char(c, c1, c2))
                    else
                        return false,index
                    end
                else
                    return false,index
                end
            else
                return false,index
            end
            k = k + 3
        elseif c < 248 then
            k = k + 4
            return false,index
        elseif c < 252 then
            k = k + 5
            return false,index
        elseif c < 254 then
            k = k + 6
            return false,index
        end
    end
end


--gsub替换字符 source源字符串 mat匹配 rep代替 最多支持替换4个值
function string.replace(source,mat1,rep1,mat2,rep2,mat3,rep3,mat4,rep4)
    local str= nil 
    if mat1~=nil then 
        str = string.gsub(source,mat1,tostring(rep1),1)
    end
    if mat2~=nil then 
        str = string.gsub(str,mat2,tostring(rep2),1)
    end
    if mat3~=nil then 
        str = string.gsub(str,mat3,tostring(rep3),1)
    end
    if mat4~=nil then 
        str = string.gsub(str,mat4,tostring(rep4),1)
    end    
    return  str
end

--字符串是否为空
function string.isEmptyOrNull(str)
    return str == nil or str ==''
end

-- 在字符串的指定位置插入字符串  rep 指定字符位置   insertion 插入字符串
function string.stringInsertion(str,rep,insertion)
    if str == "" then return str end
    local num = string.find( str,rep )
    if num and num > 0 then
        local temp = string.split(str,rep)
        str = temp[1] .. rep .. insertion .. temp[2]
    end
    return str
end


local function decodeTable(tt,ss,format,depth)
   local sss = ""

    if format then
        if(not ss) then ss = "" end;
    else
        if(not ss) then ss = "  " end;
    end

    for k,v in pairs(tt) do
        if(type(k) == "string")then
            k = "['" ..k .. "']";
        else
            k = "[" ..k .. "]"
        end

        if(type(v) == "table")then
            if format then
                sss = sss .. "" .. ss..k.. " = " .. "" .. ss .. "{" .. decodeTable(v,ss .. ss,format,depth + 1 ) .. "" .. ss .."},";
            else
                sss = sss .. "\n" .. ss..k.. " = " .. "\n" .. ss .. "{" .. decodeTable(v,ss .. ss,format,depth + 1 ) .. "\n" .. ss .."},";
            end

        elseif(type(v) == "string")then
            if format then
                sss = sss .. '' .. ss ..k .. " = '"  .. v .."',";
            else
                sss = sss .. '\n' .. ss ..k .. " = '"  .. v .."',";
            end
        else
            if format then
                sss = sss .. '' .. ss ..k .. " = "  .. tostring(v) ..",";
            else
                sss = sss .. '\n' .. ss ..k .. " = "  .. tostring(v) ..",";
            end
        end
    end

    return sss;
end


--把table 转换为 string 格式
function table.toString(tt)
    local sss = "{\n";
    
    sss = sss .. decodeTable(tt,nil,nil,1);

    sss = sss .. "\n}"

    return sss;
end


function table.tostring(tt)
    local sss = "{";
    
    sss = sss .. decodeTable(tt,nil,true,1);

    sss = sss .. "}"

    return sss;
end

---string串转表
function string.totable( str )
   if str == nil or type(str) ~= "string" then
        return
    end

    return loadstring("return " .. str)()
end


local function ToStringEx2(value)
    if type(value)=='table' then
        return table.TableToStr(value)
    elseif type(value)=='string' then
        return "\'"..value.."\'"
    else
        return tostring(value)
    end
end

--table转成string
function table.TableToStr(t)
    if t == nil then return "" end
    local retstr= "{"

    local i = 1
    for key,value in pairs(t) do
        local signal = ","
        if i==1 then
            signal = ""
        end

        if key == i then
            retstr = retstr..signal..ToStringEx2(value)
        else
            if type(key)=='number' or type(key) == 'string' then
                retstr = retstr..signal..'['..ToStringEx2(key).."]="..ToStringEx2(value)
            else
                if type(key)=='userdata' then
                    retstr = retstr..signal.."*s"..TableToStr(getmetatable(key)).."*e".."="..ToStringEx2(value)
                else
                    retstr = retstr..signal..key.."="..ToStringEx2(value)
                end
            end
        end

        i = i+1
    end

    retstr = retstr.."}"
    return retstr
end


function string.GetFourString(str,space1,space2)
	local num = CS.XOptimizeUtility.GetSubStringLen(str)
	space1 = space1 or "　　";
	space2 = space2 or "  ";
	if num == 4 then
		local chars = string.toChars(str)
		return chars[1].. space1 ..chars[2]
	elseif num == 6 then
		local chars = string.toChars(str)
		return chars[1].. space2 ..chars[2].. space2..chars[3]
	else
		return str
	end		
end

function string.GetThreeString(str,space)
    space = space or "   ";
    local chars = string.toChars(str)
    return chars[1].. space ..chars[2]
end

function string.GetTwoString(str,space)
    space = space or "  ";
    local chars = string.toChars(str)
    return chars[1].. space ..chars[2]
end


function math.clamp01(value)
    return value < 0 and 0 or (value > 1 and 1 or value)
end

function math.lerp(a,b,t)
    return a + (b - a) * math.clamp01(t)
end

function math.inverseLerp(a,b,value)
    return a ~= b and math.clamp01((value - 1) / b - a) or 0
end