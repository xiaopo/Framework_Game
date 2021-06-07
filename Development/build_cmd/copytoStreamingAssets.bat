@echo off

::set sourcePath=D:\nginx\html\assetBundles_tt\android
::set savePath=%cd%\..\Assets\StreamingAssets\A_AssetBundles

set sourcePath=%1
set savePath=%2


if exist %savePath% (
	del /s /q %savePath%\*.*
)

if not exist %savePath% (
	md %savePath% 
)
 

xcopy %sourcePath% %savePath% /s /e /c /y /h /r

del /s /q %savePath%\*.manifest

echo copy finish !!!!!!!!!!!
::pause
::start explorer %savePath%
