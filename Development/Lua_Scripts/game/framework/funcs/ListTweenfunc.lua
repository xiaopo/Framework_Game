local ListTweenfunc = {}

local Ease = CS.DG.Tweening.Ease
local RectTransform = CS.UnityEngine.RectTransform
local Tweenfunc = require( 'game.framework.funcs.Tweenfunc' )

--摇摆
function ListTweenfunc.ItemTween_Rock(startVect,endVect,duration,ease)
    return function (item)
        local effectRect = nil
        if item.inject then
            if item.inject.effectRect then
                effectRect = item.inject.effectRect
            end
        end
        if effectRect == nil then
            effectRect = item.transform:FindComponent(typeof(RectTransform),'effectRect')
        else
            effectRect:DOKill(false)
            effectRect.localRotation = Quaternion.Euler(startVect or Vector3(0,0,-20))
            effectRect:DOLocalRotate(endVect or Vector3.zero, duration or 2.5):SetEase(ease or Ease.OutElastic);
        end
        item.__groupTween = Tweenfunc.ui_tween_Fade(item,nil,true,0.5)
    end
end

--平移
function ListTweenfunc.ItemTween_Translate(delay,startVect,endVect,duration,ease,imposeIndex, fadetime)
    return function (item)
        local effectRect = nil
        if item.inject then
            if item.inject.effectRect then
                effectRect = item.inject.effectRect
            end
        end
        if effectRect == nil then
            effectRect = item.transform:FindComponent(typeof(RectTransform),'effectRect')
        end
        effectRect:DOKill(false)
        effectRect.anchoredPosition = startVect or Vector3(4.139999,60,0)
        local XNoDrawingView = effectRect.gameObject:GetComponent(typeof(CS.XGUI.XNoDrawingView))
        if XNoDrawingView then
            XNoDrawingView.enabled = false
        end
        local everyDelay = delay or 0
        if everyDelay then
            if item:get_data() then
                if item:get_data().Index then
                    everyDelay = item:get_data().Index*everyDelay
                elseif item.get_index then
                    everyDelay = ((item:get_index())+1)*everyDelay
                else
                    everyDelay = (item:get_data().___index or 1)*everyDelay
                end
            end
        end

        if imposeIndex then
            if item:get_data() and item:get_data().Index < imposeIndex then
                effectRect:DOAnchorPos(endVect or Vector3(4.139999, 15.06003, 0), duration or 0.6):SetEase(ease or Ease.OutQuad):SetDelay(everyDelay):OnComplete(function ()
                    if XNoDrawingView then
                        XNoDrawingView.enabled = true
                    end
                end)
                Tweenfunc.ui_tween_Fade(item, nil,true, fadetime or 0.8,everyDelay)
            else
                effectRect.anchoredPosition = endVect or Vector3(4.139999, 15.06003, 0)
                Tweenfunc.ui_atOnceSet_Alpha(item, 1)
            end

        else
            effectRect:DOAnchorPos(endVect or Vector3(4.139999, 15.06003, 0), duration or 0.6):SetEase(ease or Ease.OutQuad):SetDelay(everyDelay):OnComplete(function ()
                if XNoDrawingView then
                    XNoDrawingView.enabled = true
                end
            end)
           Tweenfunc.ui_tween_Fade(item, nil,true,fadetime or 0.8,everyDelay)
        end
    end
end   
--标签摇摆
--注意要手动调用 effectRect:DOKill(true)
function ListTweenfunc.ItemTween_SignInswing(self) 
    local pos_X = 160 + (self.listView.totalCount - self:get_index()) * 120
    self.inject.group:DOKill(false)
    self.inject.group:DOFade(1, 0.2):SetEase(Ease.InExpo)

    self.inject.effectRect:DOKill(true)
    local originPos = self.inject.effectRect.anchoredPosition
    self.inject.group.alpha = 0
   
	self.inject.effectRect.localEulerAngles = Vector3(0,0,11)
	self.inject.effectRect.anchoredPosition = Vector2(pos_X,self.inject.effectRect.anchoredPosition.y)
    self.inject.effectRect:DOAnchorPosX(originPos.x, pos_X/1500):OnComplete(function()
        self.inject.effectRect:DOLocalRotate(Vector3.zero, 3):SetEase(Ease.OutElastic)
    end)
end

function ListTweenfunc.ui_tween_Slide_y(self)
    self.inject.effectRect.transform:DOKill(true)
    local originPos = self.inject.effectRect.transform.localPosition
    self.inject.effectRect.transform.localPosition = Vector2(originPos.x,originPos.y - 400)
    self.inject.effectRect.transform:DOLocalMoveY(originPos.y,0.6):SetEase(Ease.OutCubic)
end

return ListTweenfunc