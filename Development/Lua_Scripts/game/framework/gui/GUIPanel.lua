local UIDefine = require( 'game.defines.UIDefine' ) 
local base = require( 'game.framework.View' )

-- 视图下的子面板
--   view.prefab
--     |--panel1.prefab
--     |--panel2.prefab
--     |--panel3.prefab
local GUIPanel = class(base)
-- GUIPanel.name 	    = 'testpanel'    --面板名字			    (必选)*
-- GUIPanel.prefab 		= 'test.prefab'  --预置资源			    (必选)*
-- GUIPanel.node 		= 'panelNode'    --面板创建的父节点名	(必选)*


-- main  此面板所属视图
function GUIPanel:ctor(gameObject,mainView)
	self.main = mainView
	base.ctor(self,gameObject)
end


function GUIPanel:on_enabled()
	base.on_enabled(self)
	self:refreshView()
end

function GUIPanel:refreshView() if self.is_enabled then self:on_refreshView()end end

function GUIPanel:on_initView()end
function GUIPanel:on_refreshView()end


return GUIPanel
