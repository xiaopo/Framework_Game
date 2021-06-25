local guid = require( 'game.utilitys.GUID' )()
EventDefine = {}

-- ============================================================================

-- ===========================	请都使用大写统一规范	=======================

-- ============================================================================



-- 默认占空
EventDefine.DEFULT = guid:get()

EventDefine.INPUT_TOUCH_CLICK 				= guid:get()		--触摸点击
EventDefine.INPUT_TOUCH_DRAG 				= guid:get()		--触摸拖拽
EventDefine.INPUT_TOUCH_END_DRAG 			= guid:get()	    --触摸拖拽结束


EventDefine.GUIVIEW_DISABLE					= guid:get()		--视图关闭

--指引
EventDefine.VIEW_ONENABLED                  = guid:get()		--视图激活
EventDefine.VIEW_DISABLE                    = guid:get()		--视图关闭
EventDefine.VIEW_DISPOSE                    = guid:get()		--视图销毁

-- 网络
EventDefine.NETWORK_ONDISCONNECTED 			= guid:get()		--网络连接断开
EventDefine.NETWORK_CHANGE					= guid:get()		--网络切换

-- 登录

EventDefine.ETNER_LOGIN_SCENE				= guid:get()

--进入游戏
EventDefine.ENTER_GAME_MAP					= guid:get()

-- 地图切换
EventDefine.MAP_CUT_REQURE					= guid:get()--请求场景加载
EventDefine.MAP_CUT_BEGIN					= guid:get()--场景加载开始
EventDefine.MAP_CUT_DONE					= guid:get()--场景加载成功完毕


