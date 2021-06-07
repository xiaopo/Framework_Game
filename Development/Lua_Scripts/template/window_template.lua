local base = require( 'game.framework.gui.GUIWindow' )
local UIDefine = require( 'game.defines.UIDefine' )

local ${NAME}Window = class(base)
-- 视图名
${NAME}Window.name = '${NAME}Window' 
-- Window样式
--${NAME}Window.prefab  = 'GUI_${NAME}_View.prefab' 
-- Window内容预置
--${NAME}Window.cprefab = 'GUI_${NAME}_View.prefab' 

function ${NAME}Window:on_initView()
	base.on_initView(self)
end

function ${NAME}Window:on_refreshView()
	base.on_refreshView(self)
end

function ${NAME}Window:on_enabled()
	base.on_enabled(self)
end

function ${NAME}Window:on_disable()
	base.on_disable(self)
end

function ${NAME}Window:on_dispose()
	base.on_dispose(self)
end

return ${NAME}Window