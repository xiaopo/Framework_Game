echo off
set currentPath=%cd%
set luaWorkPath=%cd%

echo %luaWorkPath%

set/p ModuleName=ModuleName:
set/p ViewName=ViewName:

echo %ModuleName%
echo %ViewName%

start /b %currentPath%\LuaTemplateGen %luaWorkPath% createView %ModuleName% %ViewName%
