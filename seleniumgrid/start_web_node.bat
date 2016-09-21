@echo off
@echo =================================
@echo Author Kenny Wang at 2015.10.27
@echo Start selenium grid as web node on current machine
@echo =================================
set home=%cd%
set grid=selenium-server-standalone-2.53.1.jar
set config=%home%\web_node.json
set ie_driver=%home%\..\webdriver\x86\IEDriverServer.exe
set chrome_driver=%home%\..\webdriver\x86\chromedriver.exe
cd /d %home%
java -jar %grid% -role node -nodeConfig %config% -Dwebdriver.ie.driver=%ie_driver% -Dwebdriver.chrome.driver=%chrome_driver% -Dwebdriver.ie.driver.logfile=%TEMP%\ie_driver_server.log -Dwebdriver.ie.driver.loglevel=DEBUG