dir ..\..\lib\*.* /b >loglist.txt

xcopy ..\..\Bin\*.* Bin\*.* /S /E /Y /H
setlocal enabledelayedexpansion
for /f %%a in (loglist.txt) do (
	del /s /q Bin\%%a
)

del /q loglist.txt

for %%* in (.) do set CurrDirName=%%~n*

7z a -r %CurrDirName%.zip *.* -x@ignores.txt

@pause
