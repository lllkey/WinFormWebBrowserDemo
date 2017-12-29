@echo off

::自动化编译
set NSIS_MAIN_PATH=C:\Program Files (x86)\NSIS
set CURRENT_MAIN_PATH=C:\worksapce\web_browser\test_my_webbrowser\WebBrowserDemo\testInstall


"%NSIS_MAIN_PATH%\makensis.exe" %CURRENT_MAIN_PATH%\testInstall.nsi


::编译完成
:EXIT
echo Client Build Complete!