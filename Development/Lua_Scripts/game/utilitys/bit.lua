
--二进制位运算类

local bit  = {}
bit.data32 = bit.data32 or {}

for i=1,32 do
    bit.data32[i]=2^(32-i)
end

--二进制转十进制
function bit:d2b(arg)
    local tr={}
    for i=1,32 do
        if arg >= self.data32[i] then
        tr[i]=1
        arg=arg-self.data32[i]
        else
        tr[i]=0
        end
    end
    return   tr
end   --bit:d2b

    
function bit:b2d(arg)
    local nr=0
    for i=1,32 do
        if arg[i] ==1 then
        nr=nr+2^(32-i)
        end
    end
    return  nr
end   --bit:b2d	

function bit:_xor(a,b)
    local op1 = self:d2b(a)
    local op2 = self:d2b(b)
    local r={}

    for i=1,32 do
        if op1[i]==op2[i] then
            r[i]=0
        else
            r[i]=1
        end
    end
    return  self:b2d(r)
end --bit:xor

-- 与运算
function bit:_and(a,b)
    local op1 = self:d2b(a)
    local op2 = self:d2b(b)
    local r={}
    
    for i=1,32 do
        if op1[i]==1 and op2[i]==1  then
            r[i]=1
        else
            r[i]=0
        end
    end
    return  self:b2d(r)
    
end --bit:_and


-- 或运算
--a 数1 
--b 数2
function bit:_or(a,b)
    local op1 = self:d2b(a)
    local op2 = self:d2b(b)
    local r={}
    
    for i=1,32 do
        if  op1[i]==1 or   op2[i]==1   then
            r[i]=1
        else
            r[i]=0
        end
    end
    return  self:b2d(r)
end --bit:_or

function  bit:_not(a)
    local op1 = self:d2b(a)
    local r={}

    for i=1,32 do
        if  op1[i]==1   then
            r[i]=0
        else
            r[i]=1
        end
    end
    return  self:b2d(r)
end --bit:_not

--右位移
function bit:_rshift(a,n)
    local op1 = self:d2b(a)
    local r = self:d2b(0)
    
    if n < 32 and n > 0 then
        for i=1,n do
            for i=31,1,-1 do
                op1[i+1]=op1[i]
            end
            op1[1]=0
        end
    r=op1
    end
    return  self:b2d(r)
end --bit:_rshift

--左位移
function bit:_lshift(a,n)
    local op1 = self:d2b(a)
    local r = self:d2b(0)
    
    if n < 32 and n > 0 then
        for i=1,n   do
            for i=1,31 do
                op1[i]=op1[i+1]
            end
            op1[32]=0
        end
    r=op1
    end
    return  self:b2d(r)
end --bit:_lshift


function bit:print(ta)
    local sr=""
    for i=1,32 do
        sr=sr..ta[i]
    end
    print(sr)
end

--num 分解成二进制数字 比如num=7 返回 {1,2,4}
function bit:split(num)
    local nums = {}
    local s = 0
    for i=0,31 do
        s = 2^i
        if self:_and(num,s)>0 then
            table.insert(nums,s)
        end
    end
    return nums
end

_G.bit = bit;

return bit;
--print(bit:_lshift(1,3))

-- bs=bit:d2b(7)
-- bit:print(bs)                          
-- -->00000000000000000000000000000111
-- bit:print(bit:d2b(bit:_not(7)))         
-- -->11111111111111111111111111111000
-- bit:print(bit:d2b(bit:_rshift(7,2)))    
-- -->00000000000000000000000000000001
-- bit:print(bit:d2b(bit:_lshift(7,2)))    
-- -->00000000000000000000000000011100
-- print(bit:b2d(bs))                      -->     7
-- print(bit:_xor(7,2))                    -->     5
-- print(bit:_and(7,4))                    -->     4
-- print(bit:_or(5,2))                     -->     7
