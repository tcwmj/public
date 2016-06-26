@echo off
@echo =================================
@echo Author Kenny Wang at 2015.10.27
@echo Start node service on current machine
@echo =================================
set home=%cd%
set grid=selenium-server-standalone-2.53.0.jar
set config=%home%\node_config.json
set ie_driver=%home%\..\webdriver\x86\IEDriverServer.exe
set chrome_driver=%home%\..\webdriver\x86\chromedriver.exe
cd /d %home%
java -jar %grid% -role node -nodeConfig %config% -Dwebdriver.ie.driver=%ie_driver% -Dwebdriver.chrome.driver=%chrome_driver%