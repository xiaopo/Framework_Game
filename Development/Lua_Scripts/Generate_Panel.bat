echo off
set currentPath=%cd%
set luaWorkPath=%cd%

echo %luaWorkPath%

set/p ModuleName=ModuleName:
set/p PanelName=PanelName:

echo %ModuleName%
echo %PanelName%

start /b %currentPath%\LuaTemplateGen %luaWorkPath% createPanel %ModuleName% %PanelName%
