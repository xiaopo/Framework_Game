local LoaderItemRenderer = require( 'game.components.renderers.LoaderItemRenderer' )
local base = require( 'game.framework.View' )
local Loader = class(base)

function Loader:ctor(gameObject)
	self.data 		  	= nil
	self.exData 		= nil
	self.item 		  	= nil
	self.xLoader	  	= nil
	self.itemRenderer 	= nil
	self.invalidClick	= false;
	base.ctor(self,gameObject)
end

function Loader:on_init()
	self.xLoader = self:get_cview()

	assert( self.xLoader, 'Loader:on_init.  xLoader is nil' )

	self.xLoader.onCreateRenderer:AddListener(function(citem)self:on_createItemRenderer(citem)end)
	self.xLoader.onUpdateRendererLua:AddListener(function()self:on_onUpdateRenderer()end)
	self.xLoader.onDestroy:AddListener(function()self:dispose(self)end)
end

function Loader:on_createItemRenderer(citem)

	local item = self.itemRenderer and self.itemRenderer(citem,self) or LoaderItemRenderer(citem,self)
	self.item = item
	item:on_create()
	local button = item:get_button()
	if button then
		button.onClick:AddListener(function()self:on_itemClick(item)end)
	end
end

function Loader:set_exData(exData)
	self.exData = exData
end

function Loader:on_onUpdateRenderer()

	self.item:on_data(self.exData)
end

function Loader:set_data(data)
	self.xLoader:StartLoad()
	
	self.data = data
	self.xLoader:ForceRefresh()
end

function Loader:set_ex_data(data,exData)
	self:set_exData(exData);
	self:set_data(data);
end

function Loader:set_templateAsset(templateAsset)
	if not self.xLoader then return end
	
	self.xLoader.templateAsset = templateAsset
end

function Loader:get_data()
	return self.data
end

function Loader:get_exdata()
	return self.exData
end

function Loader:on_itemClick(item)
	if self.invalidClick then return end;
	
	item:on_click(self.exData)
end

-- 设置数据项渲染器
function Loader:set_itemRenderer(renderer)
	if type(renderer) == 'string' then
		self.itemRenderer = require( renderer )
	else
		self.itemRenderer = renderer
	end
end

function Loader:removeEvents()
	if self.xLoader.onCreateRenderer then
		self.xLoader.onCreateRenderer:RemoveAllListeners()
	end
	if self.xLoader.onUpdateRendererLua then
		self.xLoader.onUpdateRendererLua:RemoveAllListeners()
	end
	if self.xLoader.onDestroy then
		self.xLoader.onDestroy:RemoveAllListeners()
	end
	if self.item then
		local button = self.item:get_button()
		if self.item and button then
			button.onClick:RemoveAllListeners()
		end
		self.item:dispose()
	end
end

function Loader:on_dispose()
	self:removeEvents()
	self.data 			= nil
	self.item 			= nil
	self.xLoader 		= nil
	self.itemRenderer 	= nil
	self.exData         = nil
	base.on_dispose(self)
end

return Loader