local UILayer_2D =
{

    SCameraRenderer		= "SCameraRenderer",   	    --场景相机渲染
    SceneNameLayer      = "SceneNameLayer",         --地图名字层
	FightTextLayer		= "FightTextLayer",   	    --战斗飘字层
    TouchLayer		    = "TouchLayer",   	        --触摸层
    SceneNameTouchLayer = "SceneNameTouchLayer",    --地图名字层
    MainUILayer			= "MainUILayer", 		    --MainUI层
    PromptLayer			= "PromptLayer", 		    --右下角提示框层
    ViewLayer           = "ViewLayer",              --UI层
    AdaptLayer         = "AdaptLayer",              --自适应界面
    WindowLayer 		= "WindowLayer",   		    --窗口层
    AlertLayer 			= "AlertLayer",  		    --弹出框层
    GuideLayer 			= "GuideLayer",  		    --新手指引
    EffectLayer 		= "EffectLayer", 		    --全局特效层
    TopAlertLayer 		= "TopAlertLayer",    	    --顶层
    StoryLayer 			= "StoryLayer",  		    --剧情层
    StoryTopLayer 		= "StoryTopLayer", 		    --剧情夹层
    TopLayer 			= "TopLayer",    		    --顶层
    LoaderLayer 		= "LoaderLayer", 		    --地图切换加载层
    TouchEffLayer       = "TouchEffLayer", 		    --摸特效层最顶层
}




local UILayer_2D_Level =
{
	
    {name = UILayer_2D.SCameraRenderer, class = 'game.framework.gui.GUISRendererLayer'},    --场景相机渲染
    {name = UILayer_2D.SceneNameLayer,  class = 'game.framework.gui.GUIAdaptLayer'},        --地图名字层
	{name = UILayer_2D.FightTextLayer,  class = 'game.framework.gui.GUIAdaptLayer'},        --战斗飘字层
    {name = UILayer_2D.TouchLayer,      class = 'game.framework.gui.GUITouchLayer'},        --触摸层
    {name = UILayer_2D.SceneNameTouchLayer,class = 'game.framework.gui.GUIAdaptLayer'},        --地图名字触摸层
    {name = UILayer_2D.MainUILayer,     class = 'game.framework.gui.GUILayer'},             --MainUI层
    {name = UILayer_2D.PromptLayer,     class = 'game.framework.gui.GUIAdaptLayer'},        --右下角提示框层
    {name = UILayer_2D.ViewLayer,       class = 'game.framework.gui.GUIAdaptLayer'},        --UI层
    {name = UILayer_2D.AdaptLayer,      class = 'game.framework.gui.GUIAdaptLayer'},        --自适应界面
    {name = UILayer_2D.WindowLayer,     class = 'game.framework.gui.GUIAdaptLayer'},        --窗口层
    {name = UILayer_2D.AlertLayer,      class = 'game.framework.gui.GUIAdaptLayer'},        --弹出框层
    {name = UILayer_2D.GuideLayer,      class = 'game.framework.gui.GUIAdaptLayer'},        --新手指引
    {name = UILayer_2D.EffectLayer,     class = 'game.framework.gui.GUIAdaptLayer'},        --全局特效层
    {name = UILayer_2D.TopAlertLayer,   class = 'game.framework.gui.GUIAdaptLayer'},        --顶层
    {name = UILayer_2D.StoryLayer,      class = 'game.framework.gui.GUIAdaptLayer'},        --剧情层
    {name = UILayer_2D.StoryTopLayer,   class = 'game.framework.gui.GUIAdaptLayer'},        --剧情夹层
    {name = UILayer_2D.TopLayer,        class = 'game.framework.gui.GUIAdaptLayer'},        --顶层
    {name = UILayer_2D.LoaderLayer,     class = 'game.framework.gui.GUIAdaptLayer'},        --地图切换加载层
 
}



return
{
	UILayer_2D = UILayer_2D,
	UILayer_2D_Level = UILayer_2D_Level,
}