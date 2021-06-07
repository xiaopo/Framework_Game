local Application = CS.UnityEngine.Application
local RuntimePlatform = CS.UnityEngine.RuntimePlatform
local Platform = CS.UnityEngine.Application.platform
local SystemInfo = CS.UnityEngine.SystemInfo 
local SystemTipGUI = CS.SystemTipGUI
local BuglyAgent = CS.BuglyAgent
local DevicesComponent = CS.DevicesComponent
local XUtility = CS.XUtility
local DevicesState = CS.DevicesComponent.DevicesState
local SettingFunc = require("game.modules.setting.funcs.SettingFunc")
local SavePowerFunc = require("game.modules.common.funcs.SavePowerFunc")
local SystemUtility = {} 

function SystemUtility.get_oscode()
	if Platform == RuntimePlatform.Android then
		return '2'
	elseif Platform == RuntimePlatform.IPhonePlayer then
		return '1'
	end
	return '0'
end

-- mac地址
function SystemUtility.get_mac_address()
	return SystemInfo.deviceUniqueIdentifier
end

-- 唯一设备标识符
function SystemUtility.get_deviceUniqueIdentifier()
	return SystemInfo.deviceUniqueIdentifier
end

-- 安装包版本
function SystemUtility.get_appVersion()
	return Application.version
end


-- 在屏幕中心弹出个提示文字
function SystemUtility.show_tip(content)
    SystemTipGUI.ShowTip(string.format( '<color=#23F824FF>%s</color>',content))
end

function SystemUtility.show_tip_err(content)
    SystemTipGUI.ShowTip(string.format( '<color=red>%s</color>',content))
end



function SystemUtility.bugly_set_userId(str)
	if BuglyAgent then
		BuglyAgent.SetUserId(str)
	end
end

function SystemUtility.bugly_set_scene(int)
	if BuglyAgent then
		BuglyAgent.SetScene(int)
	end
end

local lastTime = 0;
--运行内存过低
DevicesComponent.onLowMemory = function()
	print('DevicesComponent.onLowMemory ')
	reportException('onLowMemory','onLowMemory','DevicesComponent.onLowMemory')


	if(Time.time - lastTime >= 5)then

		if CommonFunc.IsLowMemory() then
			CS.TimerManager.AddCoroutine( CAssetUtility.UnloadUnusedAssets())
		end
		
		lastTime = Time.time;
	end
end

--网络切换
DevicesComponent.onNetworkReachability = function()
	--print_info(string.format('DevicesComponent.onNetworkReachability IsNetworkValid=%s IsNetworkWifi=%s IsNetwork4G=%s'  ),XUtility.IsNetworkValid(),XUtility.IsNetworkValid(),XUtility.IsNetworkValid())
	Dispatcher.dispatchEvent(EventDefine.NETWORK_CHANGE)
end

-- public enum DevicesState
-- {
-- 	None,      //默认
-- 	PowerSave1,//省电模式1
-- 	PowerSave2,//省电模式2
-- }
DevicesComponent.onDoAction = function(devicesState)
	if CS.UnityEngine.Debug.isDebugBuild then
		-- 发布版不会有
		-- SystemUtility.show_tip(bool and string.format('退出省电模式 %s/s',DevicesComponent.noActionSecond) or string.format('进入省电模式 %s/s',DevicesComponent.noActionSecond))
		-- dump(devicesState,'devicesState')
	end

	local commonData = MDefine.cache.common

	local frontDevicesState = commonData:getDevicesState()

	if frontDevicesState == DevicesState.PowerSave1 and devicesState == DevicesState.None then
		Alertfunc.ShowAlert("已退出一级省电模式，当前为正常游戏模式")
	elseif frontDevicesState == DevicesState.PowerSave2 and devicesState == DevicesState.None then
		Alertfunc.ShowAlert("已退出二级省电模式，当前为正常游戏模式")
	elseif frontDevicesState ==  DevicesState.None and devicesState == DevicesState.PowerSave1 then
		Alertfunc.ShowAlert("进入一级省电模式")
	elseif frontDevicesState == DevicesState.PowerSave1 and devicesState == DevicesState.PowerSave2 then
		Alertfunc.ShowAlert("进入二级省电模式")
	end

	commonData:setDevicesState(devicesState)
	SavePowerFunc.UpdateAll()
end


return SystemUtility