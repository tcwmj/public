@echo off

Set userName="Kent.Gu"
Set project="COLLINE"
Set testFolder="Root\FVT\14.1.0 IOSCO\14.1.0 FRT"
Set testSetName="FRT_Automation_Oracle"
Set testFilePath="D:\CollineAutomation\public\extension\UpdateQC\TestSuites.xls"

UpdateQCCase.exe %userName% %project% %testFolder% %testSetName% %testFilePath%

pause