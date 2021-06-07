local base = require( 'game.components.renderers.ContentRenderer' )
local ${NAME}ContentRenderer = class(base)

function ${NAME}ContentRenderer:on_initView()
	base.on_initView(self)
end

function ${NAME}ContentRenderer:on_enabled()
	base.on_enabled(self)
end

function ${NAME}ContentRenderer:on_disable()
	base.on_disable(self)
end

function ${NAME}ContentRenderer:on_dispose()
	base.on_dispose(self)
end

return ${NAME}ContentRenderer