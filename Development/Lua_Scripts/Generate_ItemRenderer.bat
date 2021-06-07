echo off
set currentPath=%cd%
set luaWorkPath=%cd%

echo %luaWorkPath%

set/p ModuleName=ModuleName:
set/p ItemRendererName=ItemRendererName:

echo %ModuleName%
echo %ItemRendererName%

start /b %currentPath%\LuaTemplateGen %luaWorkPath% createItemRenderer %ModuleName% %ItemRendererName%
