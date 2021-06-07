local GUIConfig = {}
local Application 		= CS.UnityEngine.Application;
local RuntimePlatform	= CS.UnityEngine.RuntimePlatform

local destroyTimeTab = 
{
    {   
        memory = 1024,
        iosMemory = 2048,
        commonDestroyTime = 0,
        specailView = 
        {

        },
        --testStr = "第一种"
    },
    {
        memory = 2048,
        iosMemory = 3072,
        commonDestroyTime = 20,
        specailDestroyTime = 900,
        specailView = 
        {
            ["RoleView"] = 1,
            ["BagView"] = 1,
            ["FashionView"] = 1,
            ["AchievementView"] = 1,
            ["ForgingView"] = 1,
            ["RunesView"] = 1,
            ["MountPetsView"] = 1,
            ["GodTreasureView"] = 1,
            ["MarryView"] = 1,
            ["GuildView"] = 1,
            ["LotteryView"] = 1,
            ["RechargeView"] = 1,
            ["WelfareView"] = 1,
            ["DailyView"] = 1,
            ["BossView"] = 1,
            ["DungeonView"] = 1,
            ["MagicDemonView"] = 1,
        },
        --testStr = "第二种"
    },
    {
        memory = 3072,
        iosMemory = 4096,
        commonDestroyTime = 40,
        specailDestroyTime = 1800,
        specailView = 
        {
            ["RoleView"] = 1,
            ["BagView"] = 1,
            ["FashionView"] = 1,
            ["AchievementView"] = 1,
            ["ForgingView"] = 1,
            ["RunesView"] = 1,
            ["MountPetsView"] = 1,
            ["GodTreasureView"] = 1,
            ["MarryView"] = 1,
            ["GuildView"] = 1,
            ["LotteryView"] = 1,
            ["RechargeView"] = 1,
            ["WelfareView"] = 1,
            ["DailyView"] = 1,
            ["BossView"] = 1,
            ["DungeonView"] = 1,
            ["MagicDemonView"] = 1,
        },
        --testStr = "第三种"
    },
}

local SystemInfo = CS.UnityEngine.SystemInfo

local memoryConfig

local function getMemory()
    return SystemInfo.systemMemorySize
end

local function getMemoryConfig()
    if not memoryConfig then
        for i,v in ipairs(destroyTimeTab) do
            if Application.platform == RuntimePlatform.Android then 
                if getMemory() <= v.memory then
                    memoryConfig = v
                    break
                end
            elseif Application.platform == RuntimePlatform.IPhonePlayer then
                if getMemory() <= v.iosMemory then
                    memoryConfig = v
                    break
                end
            end
        end
        if not memoryConfig then
            memoryConfig = destroyTimeTab[#destroyTimeTab]
        end
    end
    return memoryConfig
end

function GUIConfig.DealDestroyTime(viewName)
	
	if CS.UnityEngine.Application.isEditor then return 300 end
	
    getMemoryConfig()
    --dump(memoryConfig.testStr,"memoryConfig")
    if memoryConfig then
        return memoryConfig.specailView[viewName] and memoryConfig.specailDestroyTime or memoryConfig.commonDestroyTime
    else
        return 20
    end
end

return GUIConfig