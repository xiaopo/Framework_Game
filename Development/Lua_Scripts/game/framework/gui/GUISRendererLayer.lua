local UIDefine = require( 'game.defines.UIDefine' )
local base = require( 'game.framework.gui.GUILayer' )
local GUISRendererLayer = class(base)

function GUISRendererLayer:on_init()
    self.rawImage = self.gameObject:AddComponent(typeof(CS.UnityEngine.UI.RawImage))
    self.rawImage.color = CS.UnityEngine.Color(1,1,1,0)
end



return GUISRendererLayer