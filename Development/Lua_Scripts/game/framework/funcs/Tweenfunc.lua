local Tweenfunc = {}

local Ease 	  = CS.DG.Tweening.Ease
local Vector3 = CS.UnityEngine.Vector3
local RectTransform = CS.UnityEngine.RectTransform
--UI打开动画
function Tweenfunc.ui_tween_norm( view,func,isOpen )
	view.mainObject.transform:DOKill(false)
	if isOpen then
		view.mainObject.transform.localScale = Vector3(0.7,0.7,1)
		view.mainObject.transform:DOScale(Vector3(1,1,1),0.5):SetEase(Ease.OutBack):OnComplete(function()func(view)end)
	else
		view.mainObject.transform:DOScale(Vector3(0.1,0.1,0.1),0.3):SetEase(Ease.InBack):OnComplete(function()func(view)end)
	end
end

-- 3d旋转
function Tweenfunc.ui_tween_3d_rota( view,func,isOpen )
	view.mainTransform:DOKill(false)
	local group = view.mainObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))

	if not group or group:IsNull() then
		group = view.mainObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end

	if isOpen then
		group.alpha = 0
		local rand = math.random(1,3) > 2

		view.mainTransform:LocalPositionEx(rand and 50 or -50,-100,0)
		view.mainTransform:SetLocalRotation(-33,rand and -33 or 33,0)
		view.mainTransform:DOLocalRotate(Vector3(0,0,0),0.3):OnComplete(function()func(view)end)
		view.mainTransform:DOLocalMove(Vector3(0,0,0),0.3)
		group:DOKill(false)
		group:DOFade(1,0.5)
	else
		group:DOKill(false)
		group:DOFade(0,0.2):OnComplete(function()func(view)end)
	end
end

-- 场景加载界面
function Tweenfunc.ui_tween_scene_load( view,func,isOpen )
	view.mainTransform:DOKill(false)
	local group = view.mainObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group or group:IsNull() then
		group = view.mainObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	if isOpen then
		group.alpha = 0
		group:DOKill(false)
		group:DOFade(1,0.1):OnComplete(function()func(view)end)
	else
		group:DOKill(false)
		group:DOFade(0,0.5):OnComplete(function()func(view)end)
	end
end


-- 任务对话界面
function Tweenfunc.ui_tween_task_talk( view,func,isOpen )
	view.inject.content_tween:DOKill(false)
	view.inject.get_tween:DOKill(false)
	local group1 = view.inject.get_tween.gameObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group1 or group1:IsNull() then
		group1 = view.inject.get_tween.gameObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end

	local group2 = view.inject.content_tween.gameObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group2 or group2:IsNull() then
		group2 = view.inject.content_tween.gameObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	group2:DOKill(false)
	group1:DOKill(false)
	if isOpen then
		group1.alpha = 0
		group1:DOFade(1,0.1)

		group2.alpha = 0
		group2:DOFade(1,0.1)

		view.inject.get_tween.anchoredPosition = Vector2(270,26)
		view.inject.get_tween:DOAnchorPos(Vector2(-54,26.4),0.2)

		view.inject.content_tween.anchoredPosition = Vector2(0,-224)
		view.inject.content_tween:DOAnchorPosY(0,0.2):OnComplete(function()func(view)end)
	else
		view.inject.avatar:DOKill(true)
		local curPos = view.inject.avatar.anchoredPosition
		view.inject.avatar:DOAnchorPosX( curPos.x > 0 and  -500 or  500,0.2)
		group2:DOFade(0,0.2)
		group1:DOFade(0,0.2):OnComplete(function()func(view)end)
	end
end

--渐现/渐隐 动画
--isOpen == true 渐现
--isOpen == false 渐隐
function Tweenfunc.ui_tween_Fade(view,func,isOpen,time,delay,isAlpha)
	if not view or type(view) == "number" then return end
	local viewObject = nil
	if type(view) ~= "table" then
		viewObject = view
	else
		viewObject = view.mainObject and view.mainObject or view.gameObject
	end
	local group = viewObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))

	if not group or group:IsNull() then
		group = viewObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end

	local tween = nil
	if isOpen then
		group.alpha = 0
		group:DOKill(false)
		tween = group:DOFade(1,time or 0.2):SetDelay(delay or 0)
	else
		group.alpha = 1
		group:DOKill(false)
		tween = group:DOFade(0,time or 0.2):SetDelay(delay or 0)
	end

	if tween then
		tween:OnComplete(function ()
			if func then
				func(view)
			end
			if isAlpha then
				group.alpha = 1
			end
		end)
	end
	return group
end

--瞬间设置透明度
function Tweenfunc.ui_atOnceSet_Alpha(view,alpha)
	if not view or type(view) == "number" then return end
	local viewObject = view.mainObject and view.mainObject or view.gameObject
	local group = viewObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))

	if not group or group:IsNull() then
		group = viewObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	group.alpha = alpha
end

--渐隐渐现 单个物体之间过渡 动画
function Tweenfunc.ui_tween_Fade_Show(view,func,isOpen,time1,time2,delay1,delay2)
	if not view or type(view) == "number" then return end
	local viewObject = view.mainObject and view.mainObject or view.gameObject
	local group = viewObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))

	if not group or group:IsNull() then
		group = viewObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	local mySequence = CS.DG.Tweening.DOTween.Sequence()
	group:DOKill(false)
	local tween1 = nil
	local tween2 = nil
	if isOpen then
		group.alpha = 0
		tween1 = group:DOFade(1,time1 or 0.2):SetDelay(delay1 or 0)
		tween2 = group:DOFade(0,time2 or 0.2):SetDelay(delay2 or 0)
	else
		group.alpha = 1
		tween1 = group:DOFade(0,time1 or 0.2):SetDelay(delay1 or 0)
		tween2 = group:DOFade(1,time2 or 0.2):SetDelay(delay2 or 0)
	end
	if tween1 and tween2 and mySequence then
		mySequence:Append(tween1)
		mySequence:Append(tween2)
		mySequence:AppendCallback(function ()
			if func then
				func(view)
			end
		end)
	end
end

--渐隐渐现 2个物体之间过渡 动画
function Tweenfunc.ui_tween_Fade_Show2(view1,view2,func1,func2,time)
	if not view1 or not view2 or type(view1) == "number" or type(view2) == "number" then return end

	local viewObject1 = view1.mainObject and view1.mainObject or view1.gameObject
	local group1 = viewObject1:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group1 or group1:IsNull() then
		group1 = viewObject1:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end

	local viewObject2 = view2.mainObject and view2.mainObject or view2.gameObject
	local group2 = viewObject2:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group2 or group2:IsNull() then
		group2 = viewObject2:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end

	local mySequence = CS.DG.Tweening.DOTween.Sequence()
	--mySequence:Kill(false)
	group1:DOKill(false)
	group2:DOKill(false)
	local tween1 = group1:DOFade(0,time or 0.2)
	local tween2 = group2:DOFade(1,time or 0.2)
	if tween1 and tween2 and mySequence then
		group1.alpha = 1
		group2.alpha = 0
		mySequence:Append(tween1)
		mySequence:InsertCallback(time or 0.2,function()
			if func1 then
				func1(view1)
			end
		end)
		mySequence:Append(tween2)
		mySequence:AppendCallback(function()
			if func2 then
				func2(view2)
			end
		end)
	end
end

---缩小动画
function Tweenfunc.ui_tween_Scale(view,func)
	local group = view.mainObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group or group:IsNull() then
		group = view.mainObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	local rect = view.mainObject:GetComponent(typeof(RectTransform))

	local tween = rect:DOScale(Vector3(0,0,0), 0.3)
	tween:SetEase(Ease.InBack)
	tween:OnComplete(function ()
		group:DOKill(false)
		rect:DOKill(false)
		group.alpha = 0
		view.mainObject.transform.localScale = Vector3.one

		if func then func() end
	end)
end


--蒙黑动画
function Tweenfunc.ui_tween_Mask(view,func,isOpen)
	
	local Maskfunc = require("game.framework.funcs.Maskfunc")

	local group = view.mainObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group or group:IsNull() then
		group = view.mainObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	local rect = view.mainObject:GetComponent(typeof(RectTransform))

	group:DOKill(false)
	rect:DOKill(false)
	group.alpha = 1
	
	if isOpen then
		if not view.notball then
			view.mainObject.transform.localScale = Vector3.one*0.7
			local tween = rect:DOScale(Vector3(1,1,1),view.openTime or 0.5)
			if tween then
				tween:SetEase(Ease.OutBack)
				tween:OnComplete(function ()
					group:DOKill(false)
					rect:DOKill(false)
					group.alpha = 1
					if func then
						func(view)
					end
				end)
			end
		end
		Maskfunc.AddMask(view)
	else
		view.mainObject.transform.localScale = Vector3.one
		local tween2 = view.delayTime and group:DOFade(0.15,view.delayTime):SetEase(Ease.OutCirc) or group:DOFade(0,1):SetEase(Ease.OutCirc)
		if view.endTime then
			local tween1 = rect:DOScale(Vector3(1.05,1.05,0), view.endTime)
			local tween3 = rect:DOScale(Vector3(0.8,0.8,0), 0.3 - view.endTime):SetDelay(view.endTime)
			-- tween:SetEase(Ease.OutCirc)
			tween3:OnComplete(function ()
				group:DOKill(false)
				rect:DOKill(false)
				group.alpha = 0
				view.mainObject.transform.localScale = Vector3.one
				if func then
					func(view)
				end
			end)
		else
			local tween = rect:DOScale(Vector3(0,0,0), 0.3)
			tween:SetEase(Ease.InBack)
			tween:OnComplete(function ()
				group:DOKill(false)
				rect:DOKill(false)
				group.alpha = 0
				view.mainObject.transform.localScale = Vector3.one
				if func then
					func(view)
				end
			end)
		end
		Maskfunc.RemoveMask(view)
	end
end

--蒙黑动画(从左到右)
function Tweenfunc.ui_tween_Mask2(view,func,isOpen)
	
	local Maskfunc = require("game.framework.funcs.Maskfunc")

	local group = view.mainObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group or group:IsNull() then
		group = view.mainObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	local rect = view.mainObject:GetComponent(typeof(RectTransform))

	group:DOKill(false)
	rect:DOKill(false)
	group.alpha = 0
	
	if isOpen then
		-- view.mainObject.transform.localScale = Vector3.one*0.7
		-- local tween = rect:DOScale(Vector3(1,1,1),0.5)
		group:DOFade(1,0.5):SetEase(Ease.OutCirc)
		local originPos = rect.localPosition
		rect.localPosition = Vector2(originPos.x - 800,originPos.y)
		local tween = rect:DOLocalMoveX(originPos.x,0.5):SetEase(Ease.OutCubic)
		if tween then
			tween:OnComplete(function ()
				group:DOKill(false)
				rect:DOKill(false)
				group.alpha = 1
				if func then
					func(view)
				end
			end)
		end
		Maskfunc.AddMask(view)
	else
		view.mainObject.transform.localScale = Vector3.one
		group:DOFade(0,1):SetEase(Ease.OutCirc)
		local tween = rect:DOScale(Vector3(0,0,0),0.3)
		if tween then
			tween:SetEase(Ease.InBack)
			tween:OnComplete(function ()
				group:DOKill(false)
				rect:DOKill(false)
				group.alpha = 1
				view.mainObject.transform.localScale = Vector3.one
				if func then
					func(view)
				end
			end)
		end
		Maskfunc.RemoveMask(view)
	end
end

function Tweenfunc.ui_tween_Mask3(view,func,isOpen)

	local Maskfunc = require("game.framework.funcs.Maskfunc")

	local group = view.mainObject:GetComponent(typeof(CS.UnityEngine.CanvasGroup))
	if not group or group:IsNull() then
		group = view.mainObject:AddComponent(typeof(CS.UnityEngine.CanvasGroup))
	end
	local rect = view.mainObject:GetComponent(typeof(RectTransform))

	group:DOKill(false)
	rect:DOKill(false)
	group.alpha = 1

	if isOpen then
		view.mainObject.transform.localScale = Vector3.one*0.7
		view.mainObject.transform.localPosition = Vector3.one
		local tween = rect:DOScale(Vector3(1,1,1),view.openTime or 0.5)
		if tween then
			tween:SetEase(Ease.OutBack)
			tween:OnComplete(function ()
				group:DOKill(false)
				rect:DOKill(false)
				group.alpha = 1
				if func then
					func(view)
				end
			end)
		end
		Maskfunc.AddMask(view)
	else
		view.mainObject.transform.localScale = Vector3.one
		if view.viewData and view.viewData.btnTran then
			local goooItemPos = view.gameObject.transform:InverseTransformPoint(view.viewData.btnTran.position)
			rect:DOLocalMove(goooItemPos, 0.3):SetEase(Ease.InBack)
		end
		local tween = rect:DOScale(Vector3(0,0,0), 0.3)
		tween:SetEase(Ease.InBack)
		tween:OnComplete(function ()
			group:DOKill(false)
			rect:DOKill(false)
			group.alpha = 0
			view.mainObject.transform.localScale = Vector3.one
			if func then
				func(view)
			end
		end)
		Maskfunc.RemoveMask(view)
	end
end

function Tweenfunc.ui_tween_Pure_Mask(view,func,isOpen)
	local Maskfunc = require("game.framework.funcs.Maskfunc")
	if isOpen then
		Maskfunc.AddMask(view,function ()
			if(func)then func(view) end
		end)
	else
		Maskfunc.RemoveMask(view,function ()
			if(func)then func(view) end
		end)
	end
end

function Tweenfunc.ui_tween_rota_z(view,func,isOpen)
	local tween = nil
	view.transform:DOKill(false)
	if isOpen then
		tween = view.transform:DOLocalRotateZ(0,0.2)
	else
		tween = view.transform:DOLocalRotateZ(180,0.2)
	end
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end

--自由旋转/自由时间
function Tweenfunc.ui_tween_free_rota_z(view,func,angle,duration)
	local tween = nil
	view.transform:DOKill(false)
	duration = duration or 0.2
	tween = view.transform:DOLocalRotateZ(angle,duration)
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end


function Tweenfunc.ui_tween_rota1_z(view,func,angle,duration)
	local tween = nil
	view.transform:DOKill(false)
	duration = duration or 0.2
	tween = view.transform:DOLocalRotateZ(angle,duration):SetEase(Ease.OutQuad):SetLoops(50, CS.DG.Tweening.LoopType.Yoyo)
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end


function Tweenfunc.ui_tween_scale_x(view,func,isOpen)
	local tween = nil
	view.transform:DOKill(false)
	if isOpen then
		tween = view.transform:DOScaleX(1,0.2)
	else
		tween = view.transform:DOScaleX(0,0.2)
	end
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end

function Tweenfunc.ui_tween_listSlide_y(view,func,time,offsetY, delay, ease)
	local tween = nil
	view.transform:DOKill(true)
	local originPos = view.transform.localPosition
	view.transform.localPosition = Vector2(originPos.x,originPos.y - (offsetY or 400))
	tween = view.transform:DOLocalMoveY(originPos.y,time or 0.6):SetEase(ease or Ease.OutCubic):SetDelay(delay or 0)
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end

function Tweenfunc.ui_tween_frameMove_x(view,func,time,offsetX,isOut,ease, delay)
	local tween = nil
	view.transform:DOKill(true)
	local originPos = view.transform.localPosition
	if isOut then
		--view.transform.localPosition = Vector2(originPos.x + (offsetX or 300),originPos.y)
		tween = view.transform:DOLocalMoveX(originPos.x + (offsetX or 300),time or 0.5):SetEase(Ease.OutCubic)
	else
		view.transform.localPosition = Vector2(originPos.x + (offsetX or 300),originPos.y)
		tween = view.transform:DOLocalMoveX(originPos.x,time or 0.5):SetEase(ease or Ease.OutCubic):SetDelay(delay or 0)
	end
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end

function Tweenfunc.ui_tween_ManualMove_x(view,func,time,offsetX)
	local tween = nil
	view.transform:DOKill(true)
	local originPos = view.transform.localPosition
	tween = view.transform:DOLocalMoveX(originPos.x + (offsetX or 300),time or 0.5):SetEase(Ease.OutCubic)
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end

function Tweenfunc.ui_tween_LoopMove_Y(view,func)
	local tween = nil
	view.transform:DOKill(false)
	view.transform.localPosition = Vector2(0,60)
	tween = view.transform:DOLocalMoveY(20,2):SetEase(Ease.Linear):SetLoops(-1, CS.DG.Tweening.LoopType.Yoyo);
	tween:OnComplete(function ()
		if func then
			func()
		end
	end)
end


function Tweenfunc.ui_tween_hand_loopMove(view,startPos,endPos)
	local tween = nil
	view.transform:DOKill(false)
	view.transform.localPosition = startPos or Vector3(-65,-72.4,0)
	local vec = endPos or Vector3(-12,-122,0)
	tween = view.transform:DOLocalMove(vec,0.8):SetEase(Ease.OutQuad):SetLoops(-1, CS.DG.Tweening.LoopType.Yoyo);
end

function Tweenfunc.ui_tween_content_ScrollBottom(scrollRect,time,delay)
	if not scrollRect then
		return
	end
	local target = Vector2(0,0)
	scrollRect.content:DOKill(false)
	if scrollRect.vertical then
		target.x = 0
		target.y = scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.height
	elseif scrollRect.horizontal then
		target.x = scrollRect.content.sizeDelta.x - scrollRect.viewport.rect.width
		target.y = 0
	end
	--target.x = -(scrollRect.content.sizeDelta.x - scrollRect.viewport.rect.width)
	--target.y = scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.height
	scrollRect.content:DOAnchorPos(target,time):SetEase(Ease.InOutQuad):SetDelay(delay)
end

--结算界面数字滚动
function Tweenfunc.ui_tween_value_Anim(self,lab_text,value1,value2,time,str,flag, notNumber)
	if not str then
		str = "%s"
	end
	flag = flag or "_floatTween"
	if self[flag] then
		self[flag]:Kill()
	end



	self[flag] = CS.DG.Tweening.DOVirtual.Float(value1, value2, time,function (value)
		value = tonumber(string.format("%0.0f",value))
		if not lab_text then
			if self[flag] then
				self[flag]:Kill()
			end
			return
		end
		if flag == "Fight" then
			lab_text.text = "Z" ..  string.format( str,notNumber and value or string.formatNumber(value) ) 
		else
			lab_text.text = string.format( str,notNumber and value or string.formatNumber(value) ) 
		end
	end):SetDelay(0.15)
end



_G.Tweenfunc = Tweenfunc

return Tweenfunc