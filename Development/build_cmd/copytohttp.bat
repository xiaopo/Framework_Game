@echo off

::set sourcePath=%cd%\Android
::set savePath=D:\nginx\html\assetBundles_tt\android

set sourcePath=%1
set savePath=%2

echo sourcePath %sourcePath%
echo savePath %savePath%

if exist %savePath% (
	del /s /q %savePath%\*.*
)

if not exist %savePath% (
	 md %savePath% 
)

xcopy %sourcePath% %savePath% /s /e /c /y /h /r

::echo up load finish !!!!!!!!!!!
::pause

::start explorer %savePath%
