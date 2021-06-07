local base = require( 'game.framework.View' )
local UIDefine = require( 'game.defines.UIDefine' )
local CommonLoadView = class(base)
local Color = CS.UnityEngine.Color
local Ease 	  = CS.DG.Tweening.Ease

--特殊视图，在界面打开或标签业切换时若是资源需要下载将会创建此视图
-- 视图名
CommonLoadView.name = 'CommonLoadView'
CommonLoadView.fullScreen = true

function CommonLoadView:on_open_refresh()
	--策划需求不需要显示关闭按钮
	if not self.inject.maskImg then return end
	
	-- self.inject.closeBtn:SetActive(false)
	if self.targetViewName and not string.isEmptyOrNull(self.targetViewName) then
		self.inject.closeBtn:SetActive(true)
	end
	
	self.inject.closeBtn:ClearClickEvent()
	self.inject.closeBtn:AddClickEvent(function()self:on_close_target()end)

	
	self.inject.maskImg:DOKill(false)
	self.inject.maskImg.color = Color(0,0,0,0)
	self.inject.maskImg:DOFade(0.3,0.3):SetEase(Ease.OutQuart)
	
	self:clear_timer()
	self:update_content()
	self:add_timer(function() self:update_content() end,0.1,-1)
	
	
end


function CommonLoadView:update_content()
	
	local contentTxt = ''
	if self.asyncLoader:IsDownloading() then
		local recSize,totalSize = self.asyncLoader:GetDownloadReceivedBytesSize(),self.asyncLoader:GetDownloadTotalBytesSize()
	
		local value1 = CS.XUtility.FormatBytes(recSize)
		local value2 = CS.XUtility.FormatBytes(totalSize)
		local value3 = self.asyncLoader:GetDownLoaderSizeInfo()
		if G_GUI_DOWNLOAD_DETAIL then
			--显示下载详情
			contentTxt = string.format('%s / %s \n%s',value1,value2,value3)
		else
			contentTxt = string.format('正在下载资源 %s / %s',value1,value2)
		end

	else
		
	end
	
	
	self.inject.content.text = contentTxt --string.format('%.2f\n%s',self.asyncLoader:GetProgress() * 100,self.asyncLoader:GetDownLoaderInfo())
end

function CommonLoadView:on_close_target()
	if not self.targetViewName or string.isEmptyOrNull(self.targetViewName) then
		return
	end
	
	GUIManager.closeView(self.targetViewName)
end


function CommonLoadView:on_dispose()
	if self.inject.maskImg then
		self.inject.maskImg:DOKill(false)
	end
	base.on_dispose(self)
end

return CommonLoadView