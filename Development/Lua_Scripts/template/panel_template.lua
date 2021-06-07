local base = require( 'game.framework.gui.GUIPanel' )
local UIDefine = require( 'game.defines.UIDefine' )
local ${NAME}Panel = class(base)
-- 面板名
${NAME}Panel.name = '${NAME}Panel' 
-- 面板预置件路径
${NAME}Panel.prefab = 'GUI_${NAME}_Panel.prefab' 
-- 父容器 必须挂载在 Inject 中
${NAME}Panel.node   = '${NAME}Panel_Parent_Node' 

function ${NAME}Panel:on_initView()
	base.on_initView(self)
end

function ${NAME}Panel:on_refreshView()
	base.on_refreshView(self)
end

function ${NAME}Panel:on_enabled()
	base.on_enabled(self)
end

function ${NAME}Panel:on_disable()
	base.on_disable(self)
end

function ${NAME}Panel:on_dispose()
	base.on_dispose(self)
end

return ${NAME}Panel