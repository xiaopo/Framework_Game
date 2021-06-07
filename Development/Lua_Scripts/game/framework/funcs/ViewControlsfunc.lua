
local ListView = nil
local function get_ListView()
    if not ListView then ListView = require("game.components.ListView") end
    return ListView
end

local TabView = nil
local function get_TabView()
    if not TabView then TabView = require("game.components.TabView") end
    return TabView
end

local UIAVatar = nil
local function get_UIAVatar()
    if not UIAVatar then UIAVatar = require( 'game.components.UIAvatar' ) end
    return UIAVatar
end


local UINodeAVatar = nil
local function get_UINodeAVatar()
    if not UINodeAVatar then UINodeAVatar = require( 'game.components.avatar.UINodeAvatar' ) end
    return UINodeAVatar
end


local ViewControlsfunc = {}
function ViewControlsfunc.createListView(self,...)
    if not self.__controls then self.__controls = {} end
    local listView = get_ListView()(...)
    table.insert( self.__controls, listView)
    return listView
end

function ViewControlsfunc.createTabView(self,...)
    if not self.__controls then self.__controls = {} end
    local tabView = get_TabView()(...)
    table.insert( self.__controls, tabView)
    return tabView
end

function ViewControlsfunc.createAvatar(self,...)
    if not self.__controls then self.__controls = {}end
    local args = {...}
    local avatar = #args > 0 and get_UINodeAVatar()(...)  or get_UIAVatar()()
    table.insert( self.__controls, avatar)
    return avatar
end


function ViewControlsfunc.on_enabled(self)
    if self.__controls then
        for _, v in ipairs( self.__controls ) do
            v:on_enabled()
        end
    end
end

function ViewControlsfunc.on_disable(self)
    if self.__controls then
        for _, v in ipairs( self.__controls ) do
            v:on_disable()
        end
    end
end

function ViewControlsfunc.on_dispose(self)
    if self.__controls then
        for _, v in ipairs( self.__controls ) do
            v:dispose()
        end
    end

    self.__controls = nil
end

function ViewControlsfunc.set_visible(self,bool)
    if self.__controls then
        for _, v in ipairs( self.__controls ) do
			if v.isAvatar then 
				v:set_visible(bool)
			end
        end
    end

end

return ViewControlsfunc







