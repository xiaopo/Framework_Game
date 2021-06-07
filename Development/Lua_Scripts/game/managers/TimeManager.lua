-- 游戏服务器时间
-- 必须与服务器时间同步
--[[
	"day"    日
	"hour"   小时 
	"mday"
	"min"    分
	"month"  月 1-12
	"sec"    秒
	"msec"   秒
	"usec"
	"wday"   星期(1是周日)
	"yday"
	"year"   年
--]]

local TimeManager = {}

local Timerfunc = require("game.framework.funcs.Timerfunc")

TimeManager.cur_time = os.time()		-- 当前秒 默认本地时间（秒）
TimeManager.record_time = os.time()		--记录服务器发过来一刻的本地时间
TimeManager.server_time = os.time()		--服务器时间
TimeManager.serverBirthTime = false 	--开服时间  "2011-01-01 00:00:00"

TimeManager.playerBirthTime = false		--创号时间  "2011-01-01 00:00:00"
TimeManager.playerBirthTimeInt = false	--创号时间  "123456"
TimeManager.playerUpdateTime = 0 		--记录角色升级时间
TimeManager.ping_value = 0 				--网络延迟时间

TimeManager.extendMergeTime = false --合服活动时间 "2011-01-01 00:00:00"
TimeManager.timerId = nil
TimeManager.onlineDays = 0			--在线天数

--同步服务器时间,启动差值定时器
function TimeManager.syn(sysDt)
	TimeManager.synServerTime(sysDt)
	TimeManager.synCurTime()
	TimeManager.startTimer()
end

--同步服务器时间
function TimeManager.synServerTime(sysDt)
	TimeManager.server_time = os.time(sysDt) 
	TimeManager.record_time = os.time()
end

--同步当前时间
function TimeManager.synCurTime()
	TimeManager.cur_time = TimeManager.server_time + (os.time() - TimeManager.record_time)
end

--同步在线天数
function TimeManager.synOnlineDays()
	if TimeManager.cur_time > 0 and TimeManager.cur_time % 86400 == 0 then
		TimeManager.onlineDays = TimeManager.onlineDays + 1
		Dispatcher.dispatchEvent(EventDefine.DAILY_REWARD_UPDATAE)
	end
end

--开始时间同步定时器
function TimeManager.startTimer()
	if TimeManager.timerId then return end

	TimeManager.timerId = Timerfunc.register(function()
		TimeManager.synCurTime()
		TimeManager.synOnlineDays()
	end,1,-1)
end

--结束时间同步定时器
function TimeManager.endTimer()
	Timerfunc.unregister(TimeManager.timerId)
	TimeManager.timerId = nil
end

--获取当前服务器时间date
function TimeManager.getServerDate()
	return os.date("*t",TimeManager.cur_time)
end

--获取开服时间date
function TimeManager.getServerBirthDate()
	if type(TimeManager.serverBirthTime) == "string" then
		local DateTimefunc = require("game.modules.common.funcs.DateTimefunc")
		return DateTimefunc.strToDate(TimeManager.serverBirthTime)
	end
	return nil
end

--获得开服以来的天数
function TimeManager.getServerOpenedDay()
 	local serverBirthDate = TimeManager.getServerBirthDate()
 	if serverBirthDate then
 		local day = TimeManager.getDifferFormDate(serverBirthDate,TimeManager.getServerDate())
 		return day < 0 and 1 or day+1
 	end
 	return 1
end

--比较date与当前服务器时间
function TimeManager.getDifftime(date)
	return os.difftime(os.time(date),TimeManager.cur_time)
end

--将date转成时间蹉（统一调用方便处理时区问题）
function TimeManager.dateToTime(date)
	if not date then return 0 end
	return os.time(date)
end

--日期是否是今天
function TimeManager.isToday(date)
	if not date then return false end

	local now = TimeManager.getServerDate()
	return now.year==date.year and now.month==date.month and now.day==date.day
end

--求2个时间的差值天数
function TimeManager.getDifferFormDate(date1,date2)
	local d1 = CS.System.DateTime(date1.year,date1.month,date1.day)
	local d2 = CS.System.DateTime(date2.year,date2.month,date2.day)
	local ts = d2 - d1
	return ts.Days
end

--获取当前时区和北京时区差的时间
function TimeManager.getTimeZoneDiff()
	local now = os.time()
	return 28800 - os.difftime(now, os.time(os.date("!*t", now)))
end

--获取中国格式今天周几
function TimeManager.getChinaWeekDay()
    local now = TimeManager.getServerDate()
    local wday = now.wday - 1
    if wday < 1 then wday = 7 end
    return wday
end

--返回指定date几天后的date 
--如传入 2019-11-29 20：00：00 , 3
--返回 2019-12-01 20:00:00
function TimeManager.getDateAfterDay(date, day)
	local seconds = os.time(date)
	seconds = seconds + (day * 24 * 60 * 60) 
	return os.date("*t", seconds) 
end

function TimeManager.getDateAfterSecond(date, second)
	local sec = os.time(date)
	sec = sec + second
	return os.date("*t", sec) 
end

---获取距离今天晚上12点还有多久
function TimeManager.getToDayEndDt()
	local curData = TimeManager.getServerDate()
	local endData = {}
	endData.year = curData.year
	endData.month = curData.month
	endData.day = curData.day + 1
	endData.hour = 0
	endData.min = 0
	endData.sec = 0

	return os.time(endData)
end

_G.TimeManager = TimeManager

return TimeManager