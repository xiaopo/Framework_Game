echo off
set currentPath=%cd%
set luaWorkPath=%cd%

echo %luaWorkPath%

start /b %currentPath%\LuaTemplateGen\LuaTemplateGen %luaWorkPath% updateDefine

