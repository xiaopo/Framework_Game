require( 'game.framework.Dispatcher' )

local allModules 	= {}
local allCommand 	= {}
local allViews 		= {}
local viewToModule  = {} --视图所对应的模块

local function getName(path)
	return string.match( path, "(%w+)$" )
end

-- 注册视图
local function registerView(path,module)
	local name = getName(path)
	assert( not allViews[name], '' )
	allViews[name] = path
	viewToModule[name] = module
end

-- 卸载视图
local function unregisterView(name)
	local name = getName(name)
	assert( allViews[name], '' )
	allViews[name] = nil
end

-- 返回注册的视图
local function get_viewRequire(name)
	local name = getName(name)
	return allViews[name] or nil
end

-- 注册模块
local function registerModule(path)
	local name = getName(path)
	
	--模块注册过了
	if allModules[name] then return end
	
	--assert( not allModules[name], '' )
	
	local mclass = require( path )()
	allModules[name] = mclass
	
	mclass:on_register()
	
	local list = {mclass:get_netEvents()}
	for i = 1, #list do
		if list[i] then
			NetDispatcher.addEventListener(list[i], mclass.on_netEvent, mclass)
		else
			print_err(string.format('Context.registerModule %s.get_netEvents 第 %s 行命令为空',name,i))
		end
	end

	list = {mclass:get_localEvents()}
	for i = 1, #list do
		if list[i] then
			Dispatcher.addEventListener(list[i], mclass.on_localEvent, mclass)
		else
			print_err(string.format('Context.registerModule %s.get_localEvents 第 %s 行命令为空',name,i))
		end
	end
	
	
	for _, v in ipairs( {mclass:get_views()} ) do
		registerView(v,mclass)
	end

	
end

-- 卸载模块
local function unregisterModule(path)
	local name = getName(path)
	assert( allModules[name], '' )
	local mclass = allModules[name]
	
	local list = {mclass:get_netEvents()}
	for i = 1, #list do
		if list[i] then
			NetDispatcher.removeEventListener(list[i], mclass.on_netEvent)
		end
	end

	list = {mclass:get_localEvents()}
	for i = 1, #list do
		if list[i] then
			Dispatcher.removeEventListener(list[i], mclass.on_localEvent)
		end
	end
	
	for _, v in ipairs( {mclass:get_views()} ) do
		unregisterView(v)
	end

	mclass:on_unregister()
	allModules[name] = nil
end

-- 注册Command
local function registerCommand(path)
	local name = getName(path)
	assert( not allCommand[name], '' )
	local mclass = require( path )()

	local list = {mclass:get_netEvents()}
	for i = 1, #list do
		if list[i] then
			NetDispatcher.addEventListener(list[i], mclass.on_netEvent, mclass)
		else
			print_err(string.format('Context.registerCommand %s.get_netEvents 第 %s 行命令为空',name,i))
		end
	end

	list = {mclass:get_localEvents()}
	for i = 1, #list do
		if list[i] then
			Dispatcher.addEventListener(list[i], mclass.on_localEvent, mclass)
		else
			print_err(string.format('Context.registerCommand %s.get_localEvents 第 %s 行命令为空',name,i))
		end
	end
	

	allCommand[name] = mclass
	mclass:on_register()
end

-- 卸载Command
local function unregisterCommand(path)
	assert( allCommand[name], '' )
	local mclass = allCommand[name]

	local list = {mclass:get_netEvents()}
	for i = 1, #list do
		if list[i] then
			NetDispatcher.removeEventListener(v, mclass.on_netEvent)
		end
	end

	list = {mclass:get_localEvents()}
	for i = 1, #list do
		if list[i] then
			Dispatcher.removeEventListener(v, mclass.on_localEvent)
		end
	end
	
	allCommand[name]:on_unregister()
	allCommand[name] = nil
end

local function callAllModuleFunc(funcName,isReConnect)
	for _, v in pairs( allModules ) do if v[funcName] then v[funcName](v,isReConnect) end end
end


return
{
	registerModule 		 = registerModule,
	registerView		 = registerView,
	unregisterModule 	 = unregisterModule,
	registerCommand		 = registerCommand,
	unregisterCommand 	 = unregisterCommand,
	getModuleByView 	 = function(viewName)return viewToModule[viewName] end,
	callAllModuleFunc    = callAllModuleFunc,
	get_viewRequire 	 = get_viewRequire,
	__allView 			 = allViews,
	__allModule 		 = allModules
}


