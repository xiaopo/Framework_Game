local SplitSysfun = {}

--子系统
SplitSysfun.defines = {};


--=========注册
local context 				= require( 'game.framework.Context' )
function SplitSysfun.on_register(self)
	--self.contents = {};
	
	for k,list in pairs(self.defines) do
		for i,info in ipairs(list) do

			if(info.systs)then
				local systs = require(info.systs)();
				--self.contents[info.systemName] = systs;
				
				info.view    = systs.viewUIPath;
				info.prefab  = systs.ViewUIPrefab;
				
				for _, v in ipairs( {systs:add_netEvents()} ) do
					NetDispatcher.addEventListener(v, systs.call_netEvent, systs)
				end

				for _, v in ipairs( {systs:add_localEvents()} ) do
					Dispatcher.addEventListener(v, systs.call_localEvent, systs)
				end
				
				for _, v in ipairs( {systs:init_views()} ) do
					context.registerView(v);
				end
				

				systs:on_start();
			end
		end
	end
end

function SplitSysfun.on_registerSingle(self)
	for i,info in ipairs(self.defines) do

		if(info.systs)then
			local systs = require(info.systs)();
			--self.contents[info.systemName] = systs;
			
			info.view    = systs.viewUIPath;
			info.prefab  = systs.ViewUIPrefab;
			
			for _, v in ipairs( {systs:add_netEvents()} ) do
				NetDispatcher.addEventListener(v, systs.call_netEvent, systs)
			end

			for _, v in ipairs( {systs:add_localEvents()} ) do
				Dispatcher.addEventListener(v, systs.call_localEvent, systs)
			end
			
			for _, v in ipairs( {systs:init_views()} ) do
				context.registerView(v);
			end
			

			systs:on_start();
		end
	end
end

return SplitSysfun;