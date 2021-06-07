--M x N 树结构组件
--1- n 层，每层可 m 个节点
--层和节点，可以自由选择选中结构

--[[
	
	数据结构
	
	structure NodeInfo
	{
		title 				= "顶层",
		multiple 			= true, 		--子集节点 多选
		layerSele 			= false,		--子集同层只能选中一个对象
											-- layerSele 与 multiple 都为 false 时，子集节点 单选
											
		notclickCancel	    = false,		--点击选中子项，不取消选中
		List<NodeInfo> list = 
		{
			--list 里面可以无限套结构 NodeInfo
			--m - n
		}
	}

--]]

local NodeInfo = 
{
	title 				= "xx",
	multiple 			= false, 
	layerSele 			= false,
	notclickCancel		= false;
	list				= nil					
	
}

require("game.framework.funcs.CloneFunc")
local Json = require( 'xlua.json' )

local ListTreeItemRenderer = require( 'game.components.renderers.ListTreeItemRenderer' )
local base = require( 'game.framework.View' )
local ListTreeMxNView = class(base)

function ListTreeMxNView:ctor(gameObject)
	self.xLisTreeView = nil;
	
	self.treeData 		= nil;--传入C#的结构
	self.data     		= nil;--原装外部传入的结构
	
	self.keyMapData		= nil;--key :string   value:info
	self.seletLimit 	= -1;
	self.maxLayer       = 0;

	self.rednderClass   = {};
	self.totalItems		= {};
	self.loopItems		= {};
	
	self.selectFunction = nil -- 返回两个参数，第二个参数为，item在被再次激活的时候，可用此参数判断之前是否被选中
	self.clickFunction  = nil
	self.selectShrinkFunction = nil
	self.onGuideFunction = nil
	base.ctor(self,gameObject);
end

function ListTreeMxNView:on_init()
	self.xLisTreeView = self:get_cview()

	self.xLisTreeView.onItemClick = function (instanceId)
		--子项点击
		self:OnItemClick(instanceId);
	end

	self.xLisTreeView.onItemSelected = function (instanceId,value,oldSelected)
		--子项选中或取消
		--print(instanceId,value);
		self:OnItemSelected(instanceId,value,oldSelected);
	end
	
	self.xLisTreeView.onShowItem = function (key)
		--子项显示
		self:OnShowItem(key)
	end
	
	self.xLisTreeView.onCircleItem = function (instanceId)
		--回收一个子项
		self:OnResetItem(instanceId);
	end
	
	--播放tween
	self.xLisTreeView.onItemPlayTween = function ( )
		for k,v in pairs(self.totalItems) do
			v:play_Tween()
		end
		if self.onGuideFunction then
			self.onGuideFunction()
		end
	end

end


--/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理
--/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理
--/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理
--/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理/////////////////////////////Lua操作管理


--设置ItemRender,每一层可指定一个,否则使用上一层
function ListTreeMxNView:set_itemRenderClass(...)
	self.rednderClass = {...};
end

function ListTreeMxNView:get_itemRenderClass(layer)
	
	layer = math.max(1,math.min(#self.rednderClass,layer));
	
	return self.rednderClass[layer] or ListTreeItemRenderer;
end

--pool
function ListTreeMxNView:pop_item(layer,instaceId,cTreeItem)

	local item = self.loopItems[instaceId];

	if(item)then
		return item;
	else
		local CClass = self:get_itemRenderClass(layer);
		local instance = CClass(cTreeItem.gameObject,self);
		instance:on_create();
		return instance;
	end
	
end
--pool
function ListTreeMxNView:push_item(instanceId,item)
	self.loopItems[instanceId] =  item;
	item:on_reset();
end

--显示或数据刷新一个Item
function ListTreeMxNView:OnShowItem(key)
	
	local data = self.keyMapData[key];
	if not data then return end
	--TreeItem
	local cTreeItem = self.xLisTreeView:GetTreeItem(key);
	
	local instaceId = cTreeItem.gameObject:GetInstanceID();
	local item = self.totalItems[instaceId];
	if(not item)then
		item = self:pop_item(data.___layer,instaceId,cTreeItem);
		self.totalItems[instaceId] = item;
	end

	item:set_data(data,cTreeItem);
end


function ListTreeMxNView:OnResetItem(instanceId)
	
	local item = self.totalItems[instanceId];
	if(not item)then 
		--lua 端还没有此缓存
		return 
	end
	
	self.totalItems[instanceId] = nil;
	self:push_item(instanceId,item);
end


function ListTreeMxNView:OnItemClick(instanceId)
	
	local item = self.totalItems[instanceId];
	item:on_click();
	
	if(self.clickFunction ~= nil)then 
		self.clickFunction(item.data);
	end
	
	--print("点击：",item.data.key)
end

function ListTreeMxNView:OnItemSelected(instanceId,bool,oldSelected)
	
	local item = self.totalItems[instanceId];
	item:on_select(bool);
	
	if(bool)then
		if(self.selectFunction ~= nil)then 
			self.selectFunction(item.data,oldSelected);
		end
	else
		if self.selectShrinkFunction then
			self.selectShrinkFunction(item.data)
		end
	end
	--print("选择：",item.data.key,bool)
end


--////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据
--////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据
--////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据
--////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据////////////////////////////////数据

--[[
	listTree 有序树结构
	单选
	方案 A 
	listTree = 
	{
		{
			{
				{
				
				}
			}
			,
			{
			
			}
		}
		,
		{	
			{
			
			}
		}
	}
	
--]]

function ListTreeMxNView:set_DataA(listTree)
	self.seletLimit = - 1;
	self.data  = listTree;
	self:ConversToTreeData(listTree,1);
	
	self.invalidDraw = true;
	
	self:StartNextDraw();
end


--[[
方案 B（指定每个节点的选择模式）
	listTree = 
	{
		{
			title 				= "A-1层",
			multiple 			= true, 		
			layerSele 			= false,
			list = 
			{
				{
					title 				= "B-2层",
					multiple 			= true, 		
					layerSele 			= false,
					notclickCancel		= true,
					list 				=  	{}
				}
			}
		}
		，
		{
			title 				= "A-2层",
			multiple 			= true, 		
			layerSele 			= false,								
			list = 
			{
				{
					title 				= "B-2层",
					multiple 			= true, 		
					layerSele 			= false,
					notclickCancel		= true,
					list 				= {}
				}
			}
		}，
		
		
	}
	
	set_select_type  和  set_DataB 会相互覆盖选择规则。
--]]


function ListTreeMxNView:set_DataB(listTree)
	self.seletLimit = - 1;
	self.data  = listTree;
	self:ConversToTreeData(listTree,2);
	
	self.invalidDraw = true;
	
	self:StartNextDraw();
end



--prvNode   节点上一层
--info 		外部传入数据，尽量不要破坏其结构
--key       值和C#一样
function ListTreeMxNView:FormatTreeData(prvNode,info,key,index,layer,bType)
	
	local node ;
	local childList;
	if(bType == 1)then
		if(not prvNode.list)then prvNode.list = {} end;
		node = deepcopy(NodeInfo);
		node.title			= info.title or "";
		node.multiple		= info.multiple or false;
		node.layerSele		= info.layerSele or false;
		
		if(info[1])then
			childList		= info;
		end
		
		table.insert(prvNode.list,node);
	else
		node 		= info;
		childList   = info.list;
	end
	
	node.layer			 = layer;
	node.key 			 = key;
	
	self.keyMapData[key] = info;
	info.___key 		 = key;
	info.___layer		 = layer;
	info.___index		 = index
	info.___playTween	 = true
	
	if(childList)then
	
		layer = layer + 1;
		if(layer > self.maxLayer)then self.maxLayer = layer end;--记录最大层级
		
		for i,chdInfo in ipairs(childList) do
			local cIndex = i - 1;
			self:FormatTreeData(node,chdInfo,key .. "_" .. cIndex,cIndex,layer,bType);
		end
	end
end

function ListTreeMxNView:ConversToTreeData(list,bType)

	local topNode     		= deepcopy(NodeInfo);
	topNode.title 			= "顶层";
	topNode.multiple 		= false; 
	topNode.layerSele 		= false;
	topNode.key 			= "topTree";
	topNode.layer			= 0;
	if(bType == 1)then
		topNode.list			= {};				
	else
		topNode.list			= list;
	end
	

	self.keyMapData   = {};	--外部数据
	self.maxLayer     = 1;
	
	for i,info in ipairs(list) do
		local cIndex = i - 1;
		self:FormatTreeData(topNode,info,tostring(cIndex),cIndex,1,bType);
	end
	
	self.treeData = topNode;
	
end

function ListTreeMxNView:StartNextDraw()
	
	if(not self.drawInterId)then
		--父类中有移除
		 self.drawInterId = self:add_timer(function ()
				self.drawInterId = nil;
				self:NextFramedraw();
		end,0,1)
	end
	
end


--去除关键数据	
function ListTreeMxNView:TakeoutJsonData(jsonNode,node)
	-- body
	jsonNode.title 			= node.title
	jsonNode.multiple 		= node.multiple 
	jsonNode.layerSele 		= node.layerSele
	jsonNode.key 			= node.key
	jsonNode.layer			= node.layer
	jsonNode.notclickCancel = node.notclickCancel

	if node.list then
		jsonNode.list = {}
		for i,child in ipairs(node.list) do
			local josonCde = deepcopy(NodeInfo)
			self:TakeoutJsonData(josonCde,child)

			table.insert(jsonNode.list,josonCde)
		end
	end

end

function ListTreeMxNView:NextFramedraw()
	if(not self.treeData or self.xLisTreeView == nil)then return end ;
	
	if(self.drawInterId)then
		self:del_timer(self.drawInterId);
		self.drawInterId = nil;
	end
	
	if(self.invalidDraw)then

		local jsonData = deepcopy(NodeInfo)

		--取出关键数据
		self:TakeoutJsonData(jsonData,self.treeData);
		
		local dataStr = Json.encode(jsonData)
		self.xLisTreeView:SetData(dataStr);
		
		self.invalidDraw = false;
	end
	
	
end


--/////////////////////////操作/////////////////////////操作/////////////////////////操作/////////////////////////操作
--/////////////////////////操作/////////////////////////操作/////////////////////////操作/////////////////////////操作
--/////////////////////////操作/////////////////////////操作/////////////////////////操作/////////////////////////操作
--/////////////////////////操作/////////////////////////操作/////////////////////////操作/////////////////////////操作

--[[
	设置选择模式
	--全部默认最后一层，选中的目标不能点击取消
	
	1 全部单选
	2 全部多选
	3 单选 + 节点末端 层选（最后一层只能选一个）
	4 多选 + 节点末端 层选（最后一层只能选一个）
	5 多选 + 节点末端 单选
	6 单选 + 节点末端 多选
	
	
	set_DataB 和 set_select_type 会相互覆盖选择规则。
--]]
function ListTreeMxNView:set_select_type(sType)
	if(not sType)then  sType = 1 end;
	
	if(not self.treeData)then return end;
	
	self.seletLimit   = sType;

	self:on_select_set(self.treeData);
end


function ListTreeMxNView:on_select_set(node)
	local slType = self.seletLimit;
	if(slType == -1)then return end;
	
	if(slType == 1)then
		node.multiple		= false;
		node.layerSele		= false;
	elseif(slType == 2)then
		node.multiple		= true;
		node.layerSele		= false;
	else
		if(node.layer < self.maxLayer -1)then
			if(slType == 3 or slType == 6)then
				node.multiple		= false;
				node.layerSele		= false;
			elseif(slType == 4 or slType == 5)then
				node.multiple		= true;
				node.layerSele		= false;
			end
		end
	end
	
	
	if(node.layer == self.maxLayer -1)then
	
		--控制最后一层（倒数第二层）
		if(slType == 3 or slType == 4)then
			node.multiple		= false;
			node.layerSele		= true;
		elseif(slType == 5)then
			node.multiple		= false;
			node.layerSele		= false;
		elseif(slType == 5)then
			node.multiple		= true;
			node.layerSele		= false;
		end
		
		node.notclickCancel     = true;--最后一层点击不取消
	end
	
	local list = node.list or {};
	for k,cnode in ipairs(list) do
		self:on_select_set(cnode);
	end
end

--刷新一个数据
function ListTreeMxNView:UpdateInfo(info)
	local key  = info.___key;
	local cItem = self.xLisTreeView:GetTreeItem(key);
	if(cItem and cItem.visible and  cItem.gameObject)then
		local instanceId = cItem.gameObject:GetInstanceID();
		
		local lItem = self.totalItems[instanceId];
		
		lItem:set_data(info,cItem);
	end
	
	self.xLisTreeView:ForceCountLayer();
end


function ListTreeMxNView:CancelSelected()
	self.xLisTreeView:CancelSelected();
end


--控制数据同层的点击情况
function ListTreeMxNView:set_LayerNotClick(info,bool)
	local layer  = info.___layer;
	self.xLisTreeView:LayerNotClick(layer,bool);
end



--指定选中哪个数据
function ListTreeMxNView:set_SeletInfo(info)
	if(not info)then return end;
	local key  = info.___key;
	self.xLisTreeView:SetSeleIndex(key);
end

--滑动到指定数据
function ListTreeMxNView:set_scrollToInfo(info)
	if(not info)then return end;
	local key  = info.___key;
	
	self.xLisTreeView:OnScrollToItem(key);
end


function ListTreeMxNView:ScrollToTop(smoothTime)
	
	self.xLisTreeView:ScrollToTop(smoothTime);
end

function ListTreeMxNView:ScrollToBottom(smoothTime)
	
	self.xLisTreeView:ScrollToBottom(smoothTime);
end


function ListTreeMxNView:removeEvents()
	self.xLisTreeView:ClearEvent();
	
	self.clickFunction   = nil;
	self.selectFunction  = nil;
	self.selectShrinkFunction = nil;
end

function ListTreeMxNView:on_dispose()
	self.drawInterId    = nil;
	self.rednderClass   = nil;
	
	for k,item in pairs(self.totalItems) do
		item:on_dispose();
	end
	
	for k,item in pairs(self.loopItems) do
		item:on_dispose();
	end
	
	self.totalItems = nil;
	self.loopItems  = nil;
	self.data 		= nil;
	self:removeEvents()
	
	self.xLisTreeView:OnDestroy();
	self.xLisTreeView = nil;
	base.on_dispose(self);

end

return ListTreeMxNView;