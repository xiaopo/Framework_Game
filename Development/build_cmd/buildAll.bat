
@echo off


cd../

set currentPath=%cd%
set buildLogPath=%currentPath%\build_cmd\log.txt
set unityDir="C:\Program Files\Unity\Hub\Editor\2020.3.0f1c1\Editor\Unity.exe"
set httpPath=D:\nginx\html\assetBundles_tt
set buildPath=D:\assetBundles

::%unityDir% -batchmode -quit -nographics -executeMethod BuildUtil.buildCommonline -buildPath %buildPath%
%unityDir% -quit -batchmode -nographics -executeMethod BuildUtil.buildCommonline  -buildPath %buildPath% 

call build_cmd\copytohttp %buildPath% %httpPath% 

echo finish !!

pause