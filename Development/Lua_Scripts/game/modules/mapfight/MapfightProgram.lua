local MapfightProgram = {}

local CSMapfightProgram = CS.Game.MScene.MapfightProgram.Instance


function MapfightProgram:Launch()
	
	CSMapfightProgram:AwakeMapFight()
	
end




local Time 			= Time;
local EffectTime	= EffectTime;--特效时间
function MF_Update(deltaTime, unscaledDeltaTime)
	
end


--10次/秒
local fixedframeCount = 0;
--物理update
function MF_FixedUpdate(fixedDeltaTime)
	Time:SetFixedDelta(fixedDeltaTime)
end



function MF_LateUpdate()	
	Time:SetFrameCount();	

end

_G.MapfightProgram = MapfightProgram;

return MapfightProgram;