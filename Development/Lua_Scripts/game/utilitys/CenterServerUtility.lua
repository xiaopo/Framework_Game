-- 后台工具
local CenterServerUtility = {} 
local xcfg = CS.XConfig.defaultConfig 
local cache = require( 'game.defines.MDefine' ).cache
local SystemUtility = require( 'game.utilitys.SystemUtility' )
local TimeManager = require( 'game.managers.TimeManager' )
local Application = CS.UnityEngine.Application
local RuntimePlatform = CS.UnityEngine.RuntimePlatform
local WebSCRequest = CS.WebSCRequest 


local function get_platform()  
	return SDKManager.get_platform();
end

local function get_username() return cache.login.get_username() end


-- 返回服务器列表
function CenterServerUtility.get_server_list(fun,errfun,isReCommend)
	local sendData =
	{
		['appVersion'] 		= SDKManager.get_appVersion(),
		['mappingMD5'] 		= '1',
		['serverVersion'] 	= '1',
		['platform'] 		= get_platform(),
		['username'] 		= get_username(),
	} 

	if string.isEmptyOrNull(sendData.username) then
		sendData.username = 'aa'
	end

	if Application.isMobilePlatform and Application.platform == RuntimePlatform.IPhonePlayer then
		sendData.appVersion = Application.version
	end

	if isReCommend then
		WebSCRequest.LGet(sendData,'client', 'get_recommend_server',fun,errfun,5000)
	else
		sendData.getType = '2'
		WebSCRequest.LGet(sendData,'client', 'get_server',fun,errfun,5000)
	end
end

--返回公告数据
function CenterServerUtility.get_notice_list(fun,errfun)
	local param = {}
    param.platform = get_platform()
    param.status = 1
    --param.time = TimeManager.cur_time
    param.appVersion = G_SERVER_VERSION
    WebSCRequest.LGet(param,'client', 'updates_notice',fun,errfun,5000)
end

--返回隐私公告
function CenterServerUtility.get_privacy_notice(func,errfun)
	local param = {}
	param.platform = SDKManager.get_platform()
	--param.status = 1
    --param.time = TimeManager.cur_time
    --param.appVersion = G_SERVER_VERSION
	WebSCRequest.LGet(param,'client', 'privacy_notice',func,errfun,5000)
end

--返回是否第三方支付
function CenterServerUtility.get_other_pay(func,errfun)
	local param = {}
	param.platform = SDKManager.get_platform()
	WebSCRequest.LGet(param,'client', 'get_other_pay',func,errfun,5000)
end

return CenterServerUtility