local base = require( 'game.framework.gui.GUIView' )
local UIDefine = require( 'game.defines.UIDefine' )
local ${NAME}View = class(base)
-- 视图名
${NAME}View.name = '${NAME}View' 
-- 视图预置件路径
--${NAME}View.prefab = 'GUI_${NAME}_View.prefab' 
-- 视图所在层级
${NAME}View.layer = UIDefine.UILayer_2D.ViewLayer

function ${NAME}View:on_initView()
	base.on_initView(self)
end

function ${NAME}View:on_refreshView()
	base.on_refreshView(self)
end

function ${NAME}View:on_enabled()
	base.on_enabled(self)
end

function ${NAME}View:on_disable()
	base.on_disable(self)
end

function ${NAME}View:on_dispose()
	base.on_dispose(self)
end

return ${NAME}View