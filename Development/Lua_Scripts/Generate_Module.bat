echo off
set currentPath=%cd%
set luaWorkPath=%cd%

echo %luaWorkPath%

set/p ModuleName=ModuleName:

echo create Module: %ModuleName%

start /b %currentPath%\LuaTemplateGen %luaWorkPath% createModule %ModuleName%
