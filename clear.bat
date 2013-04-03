REM - performs a remove directory, with wildcard matching - example ; rd-wildcard 2007-*
dir BIN /b /s >loglist.txt
setlocal enabledelayedexpansion
for /f %%a in (./loglist.txt) do (
	rd /s /q %%a
)
del /q loglist.txt
endlocal

dir OBJ /b /s >loglist.txt
setlocal enabledelayedexpansion
for /f %%a in (./loglist.txt) do (
	rd /s /q %%a
)
del /q loglist.txt
endlocal