
print("Lua.main enter .......")
require( 'game.global' )
require( 'game.require' )

if jit then		
	if jit.opt then
		jit.opt.start(3)			
	end
	print("jit", jit.status())
	print(string.format("os: %s, arch: %s version: %s", jit.os, jit.arch,jit.version))
end


local function main()
    
    print("lua.main() 执行 开始游戏逻辑 ")
    
	--启动模块
    require( 'game.modules.module_register' )
    
    
    collectgarbage("setpause",100);
	if(CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.IPhonePlayer)then 	
		--lua 内存立即回收
		collectgarbage("setstepmul", 5000) 
	end
    
    
    --调试逻辑
    xpcall(function ()
        local debugPath 			= CS.UnityEngine.Application.persistentDataPath.."/debugLua.lua"
        dofile(debugPath)
    end,function (msg)
        
    end)
	
	--进入登录界面
	Dispatcher.dispatchEvent(EventDefine.ETNER_LOGIN_SCENE)
	

end





return
{
	main = main
}