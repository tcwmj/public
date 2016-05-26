@echo off
@echo =================================
@echo Author Kenny Wang at 2015.10.27
@echo Start hub service on current machine
@echo =================================
set home=%cd%
set grid=selenium-server-standalone-2.53.0.jar
cd /d %home%
java -jar %grid% -role hub