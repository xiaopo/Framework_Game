--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
local rawget = rawget
local uTime = CS.UnityEngine.Time

local _Time = 
{	
	deltaTime			= 0,
	fixedDeltaTime 	 	= 0,
	maximumDeltaTime	= 0.3333333,
	fixedTime			= 0,
	frameCount			= 1,	
	realtimeSinceStartup=0,
	time 				= 0,
	timeScale			= 1,
	timeSinceLevelLoad	= 0,
	unscaledDeltaTime	= 0,	
	unscaledTime		= 0,	
}

local _set = {}

function _set.fixedDeltaTime(v)
	_Time.fixedDeltaTime = v
	uTime.fixedDeltaTime = v
end

function _set.maximumDeltaTime(v)
	_Time.maximumDeltaTime = v
	uTime.maximumDeltaTime = v
end

function _set.timeScale(v)
	_Time.timeScale = v
	uTime.timeScale = v
end

function _set.captureFramerate(v)
	_Time.captureFramerate = v
	uTime.captureFramerate = v
end

function _set.timeSinceLevelLoad(v)
	_Time.timeSinceLevelLoad = v
end

--_Time.__index = function(t, k)
--	return uTime.__index(uTime, k)	
--end
--
_Time.__newindex = function(t, k, v)
	local func = rawget(_set, k)

	if func then
		return func(v)
	end

	error(string.format("Property or indexer `UnityEngine.Time.%s' cannot be assigned to (it is read only)", k))	
end

--local Time = {}
local counter = 1

function _Time:SetDeltaTime(deltaTime, unscaledDeltaTime)	
	local _Time = _Time

	_Time.deltaTime = deltaTime	
	_Time.unscaledDeltaTime = unscaledDeltaTime
	counter = counter - 1

	if counter == 0 and uTime then	
		_Time.time = uTime.time
		_Time.timeSinceLevelLoad = uTime.timeSinceLevelLoad
		_Time.unscaledTime = uTime.unscaledTime
		_Time.realtimeSinceStartup = uTime.realtimeSinceStartup
		_Time.frameCount = uTime.frameCount
		counter = 1000000
	else
		_Time.time = _Time.time + deltaTime
		_Time.realtimeSinceStartup = _Time.realtimeSinceStartup + unscaledDeltaTime
		_Time.timeSinceLevelLoad = _Time.timeSinceLevelLoad + deltaTime	
		_Time.unscaledTime = _Time.unscaledTime + unscaledDeltaTime 
	end		
end

function _Time:SetFixedDelta(fixedDeltaTime)	

	_Time.fixedDeltaTime = fixedDeltaTime

	_Time.fixedTime = _Time.fixedTime + fixedDeltaTime
end

function _Time:SetFrameCount()
	_Time.frameCount = _Time.frameCount + 1
end

function _Time:SetTimeScale(scale)
	local last = _Time.timeScale
	_Time.timeScale = scale
	uTime.timeScale = scale
	return last
end

--Time.unity_Time = CS.UnityEngine.Time;
--CS.UnityEngine.Time = Time

if uTime ~= nil then
	_Time.maximumDeltaTime = uTime.maximumDeltaTime	
	_Time.timeScale = uTime.timeScale	
end


return _Time