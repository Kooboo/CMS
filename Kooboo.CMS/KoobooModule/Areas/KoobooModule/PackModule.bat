dir ..\..\lib\*.* /b >loglist.txt

xcopy ..\..\Bin\*.* Bin\*.* /S /E /Y /H
setlocal enabledelayedexpansion
for /f %%a in (loglist.txt) do (
	del /s /q Bin\%%a
)


del /s /q Bin\*.pdb
del /s /q Bin\*.xml

for %%* in (.) do set CurrDirName=%%~n*

..\..\7z\7z a -r %CurrDirName%.zip *.* -x@..\..\7z\ignores.txt

@pause
