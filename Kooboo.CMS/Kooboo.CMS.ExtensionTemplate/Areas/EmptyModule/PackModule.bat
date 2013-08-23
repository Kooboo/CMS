dir ..\..\lib\Kooboo_CMS\*.* /b >loglist.txt

xcopy ..\..\Bin\*.* Bin\*.* /S /E /Y /H
setlocal enabledelayedexpansion
for /f %%a in (loglist.txt) do (
	del /s /q Bin\%%a
)

del /q loglist.txt

copy "..\..\lib\EntityFramework\*.dll" "Bin\*.*"
copy "..\..\lib\EntityFramework\SQLCe\*.dll" "Bin\*.*"
copy "..\..\lib\SQLCe\*.dll" "Bin\*.*"
copy "..\..\lib\SQLCe\x86\*.dll" "Bin\*.*"

del /s /q Bin\*.pdb
del /s /q Bin\*.xml

for %%* in (.) do set CurrDirName=%%~n*

7z a -r %CurrDirName%.zip *.* -x@ignores.txt

@pause
