@echo off
set projectDir=D:\Projects\GeoSpace\GeoSpaceTrack\Sources
set exeDir=D:\Projects\GeoSpace\GeoSpaceTrack\Build
set exeName=track.exe
set currDir=%cd%

cd %projectDir%


gfortran -static %projectDir%\*.f95 -o %exeDir%\%exeName%


call :sleep 3

cd %currDir%
del %projectDir%\*.mod

%exeDir%\%exeName%


pause
goto :EOF




:sleep
    set /a ftime=100%time:~6,-3%%%100+%1
    if %ftime% GEQ 60 set /a ftime-=60
    :loop
    set ctime=%time:~6,-3%
    if /i %ftime% NEQ %ctime% goto :loop
exit /b 0


