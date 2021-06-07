local Adaptfunc = {}
local Screen =  CS.UnityEngine.Screen
local UIAdpat = CS.UIAdpat
local DeviceModel = CS.UnityEngine.SystemInfo.deviceModel
local Application 		= CS.UnityEngine.Application;
local RuntimePlatform	= CS.UnityEngine.RuntimePlatform
Adaptfunc.IsIphoneX = nil

local isScerrnAdaptation = false
function Adaptfunc.getAdpTargetScale()
    return UIAdpat.AdpTargetScale
end

function Adaptfunc.isCanAdpScale()
    return UIAdpat.IsCanAdpScale
end

function Adaptfunc.isIphoneX()
	
	if _SDK_PATTERN_ then
		if(Application.platform == RuntimePlatform.Android)then 	
			--安装刘海屏
			if isScerrnAdaptation then
				--设置屏幕正方向在Home键右边
				CS.UnityEngine.Screen.orientation = CS.UnityEngine.ScreenOrientation.LandscapeLeft
				Adaptfunc.IsIphoneX = true
				
			end
		end
	end

	
	if Adaptfunc.IsIphoneX == nil then
		if DeviceModel == "iPhone10,3" 
		or DeviceModel == "iPhone10,6" 
		or DeviceModel == "iPhone11,2" 
		or DeviceModel == "iPhone11,4" 
		or DeviceModel == "iPhone11,6" 
		or DeviceModel == "iPhone11,8" 
		or DeviceModel == "iPhone12,1" 
		or DeviceModel == "iPhone12,3" 
		or DeviceModel == "iPhone12,5" 
		
		then
			Adaptfunc.IsIphoneX = true
		else
			Adaptfunc.IsIphoneX = false
		end
	end
	
	return Adaptfunc.IsIphoneX
end

function Adaptfunc.getBangValue()
	return 66
end

function Adaptfunc.AdaptTransform(transform)
	transform.localScale = transform.localScale * Adaptfunc.getAdpTargetScale()
end

function Adaptfunc.AdaptIPhoneX(transform,iPhoneXParent,normalParent,isNotScale)
	if(not transform)then return end;
	if Adaptfunc.isIphoneX() then
		transform:SetParent(iPhoneXParent)
	    transform.anchoredPosition = CS.UnityEngine.Vector2.zero
	else
		transform:SetParent(normalParent)
	    transform.anchoredPosition = CS.UnityEngine.Vector2.zero
	end
	if not isNotScale then
		Adaptfunc.AdaptTransform(transform)
	end
end

return Adaptfunc