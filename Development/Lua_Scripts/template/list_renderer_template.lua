local base = require( 'game.components.renderers.ListItemRenderer' )
local ${NAME}ItemRenderer = class(base)

function ${NAME}ItemRenderer:on_create()
	base.on_init(self)
end


function ${NAME}ItemRenderer:on_data()
	base.on_data(self)
end


function ${NAME}ItemRenderer:on_dispose()
	base.on_dispose(self)
end

return ${NAME}ItemRenderer