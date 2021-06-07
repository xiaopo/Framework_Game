local Maskfunc = {}
local Object = CS.UnityEngine.Object
local Ease 	  = CS.DG.Tweening.Ease
local XImage = CS.XGUI.XImage
local Image = CS.UnityEngine.UI.Image
local RectTransform = CS.UnityEngine.RectTransform
local LayerMask = CS.UnityEngine.LayerMask
local Color = CS.UnityEngine.Color
local Adaptfunc = require("game.framework.funcs.Adaptfunc")

--local curColor = Color(31/255,31/255,39/255,179/255)
-- local curColor = Color(0,0,0,179/255)

function Maskfunc.AddMask(view,callBackFunc)
    local img = nil
    if not view._maskLayer then
        view._maskLayer = CS.UnityEngine.GameObject("Mask")
        view._maskLayer.layer = LayerMask.NameToLayer("UI")
        img = view._maskLayer:AddComponent(typeof(XImage))
        img.raycastTarget = true
        img.autoSetNativeSize = false
    else
        img = view._maskLayer:GetComponent(typeof(XImage))
        --img:DOKill(false)
    end
    if img and not img:IsNull() then
        if not string.isEmptyOrNull(view.maskImage) then 
            img.spriteAssetName = view.maskImage
            img.color = Color(1,1,1,1)
        else
            img.color = Color(0,0,0,view.alphaEnd or 179/255)
        end

        local rt = view._maskLayer:GetComponent(typeof(RectTransform))
        rt.anchorMin = Vector2.zero
        rt.anchorMax = Vector2.one 
        view._maskLayer.transform:SetParent(view.transform)
        rt.offsetMin = Vector2.zero
        rt.offsetMax = Vector2.zero

        view._maskLayer.transform:SetSiblingIndex(0)
        view._maskLayer.transform.localPosition = Vector3.zero
        view._maskLayer.transform.localScale = Vector3.one / Adaptfunc.getAdpTargetScale()

        if callBackFunc then
            callBackFunc()
        end
    end
end

function Maskfunc.RemoveMask(view,callBackFunc)
	if view._maskLayer then
        local img = view._maskLayer:GetComponent(typeof(Image))
        if img and not img:IsNull() then
            --img:DOKill(false)
            --local tween = img:DOFade(view.alphaBegin or 0,0.3)
            --if tween then
            --    tween:SetEase(Ease.InQuad)
            --    tween:OnComplete(function ()
            --        Object.DestroyImmediate(view._maskLayer)
            --        view._maskLayer = nil
            --        if callBackFunc then
            --            callBackFunc()
            --        end
            --    end)
            --end
            Object.DestroyImmediate(view._maskLayer)
            view._maskLayer = nil
            if callBackFunc then
                callBackFunc()
            end
        else
            Object.DestroyImmediate(view._maskLayer)
            view._maskLayer = nil
            if callBackFunc then
                callBackFunc()
            end
        end
	end
end


return Maskfunc