local warningList = {}
local function createInfo(fun)
    local info = {}
    info.__funInfo = debug.getinfo(fun)
    return info
end

local function totstring(info)
    return string.format('id:%s interval:%s duration:%s delay:%s runback:%s expendTime:(%s) target:[%s:%s]',
            info.id, info.interval, info.duration, info.delay, info.runback, info.expendTime, info.__funInfo.source, info.__funInfo.linedefined)
end

local function beginProfiler() if G_TIME_THRESHOLD then return os.clock()end end
local function endProfiler(expendTime, id, fun, interval, duration, delay, runback)
    
    if not G_TIME_THRESHOLD then return end
    if expendTime >= G_TIME_THRESHOLD then
        local info = warningList[id]
        if not info then
            info = createInfo(fun)
            info.id = id
            info.fun = fun
            info.interval = interval
            info.duration = duration
            info.delay = delay
            info.runback = runback
            warningList[id] = info
        end

        info.expendTime = expendTime

        print_war(totstring(info))
    end
end


-- fun 		 回调
-- interval  触发间距 
-- duration  持续次数 -1 无限
-- delay     启动延迟
-- runback   后台运行
local TimerManager = CS.TimerManager
local function add_timer(self, fun, interval, duration, delay, runback)
    local id
    id = TimerManager.AddTimer(function()

        --定时器回调性能分析
        local stime = beginProfiler()

        if self then
            if not self.is_dispose then
                fun()
            end
        else
            fun()
        end

        --定时器回调性能分析
        endProfiler(stime and os.clock() - stime or  0, id, fun, interval, duration, delay, runback)

    end, interval or 1, duration or 1, delay or 0, runback == nil and true or runback)

    if self then
        if not self.timers then self.timers = {} end
        self.timers[id] = id
    end

    return id
end

-- 重置定时器,重置的定时器必须是正在执行中的定时器,执行完后的定时器将无法重置
local function reset_timer(self, id)
    return TimerManager.ResetStart(id)
end

-- 移除定时器
local function del_timer(self, id)
    if self then
        if not self.timers then return end
        if self.timers[id] then
            TimerManager.DelTimer(id)
            self.timers[id] = nil
        end
    else
        TimerManager.DelTimer(id)
    end
end

-- 清理实现了定时器接口对象身上的所有定时器
local function clear_timer(self)
    if self then
        if not self.timers then return end
        for _, v in pairs(self.timers) do TimerManager.DelTimer(v) end
        self.timers = nil
    end
end



--===============================================================方便任意地方使用定时器================================================
local function register(fun, interval, duration, delay, runback) return add_timer(nil, fun, interval, duration, delay, runback) end
local function unregister(id) del_timer(nil, id) end
local function reset(id) return reset_timer(nil, id) end

return
{
    add_timer = add_timer,
    del_timer = del_timer,
    clear_timer = clear_timer,
    reset_timer = reset_timer,

    register = register,
    unregister = unregister,
    reset = reset,
}