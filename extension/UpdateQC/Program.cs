using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDAPIOLELib;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace UpdateQCCase
{
    class Program
    {
        private static string LogPath = "C:/Documents/testlog/";
        private static  string LogName = "auto_test.log";

        static void Main(string[] args)
        {

            WriteLog("Begin update case status in QC");
            string qc_serverURL = "http://172.20.20.30:8080/qcbin";
            string qc_password = "";
            string qc_domain = "LOMBARDRISK";
           
            string qc_user = args[0].Split('~')[0];
            string qc_proj = args[0].Split('~')[1];


            string testFolder = args[0].Split('~')[2];
            string testSetName = args[0].Split('~')[3];
            string testResultFile = args[0].Split('~')[4];

            testFolder = "Root\\FVT\\CiRRuS\\1.13.0\\1.13.0Sprint2Automation\\";
            WriteLog("User is: " + qc_user);
            WriteLog("QC project: " + qc_proj);

            WriteLog("Test folder is:" + testFolder);
            WriteLog("Testset is:" + testSetName);
            WriteLog("Test ResultFile is:" + testResultFile);

            TDConnection conn = new TDConnectionClass();
            try
            {
                conn.InitConnectionEx(qc_serverURL);
                conn.Login(qc_user, qc_password);
                conn.Connect(qc_domain, qc_proj);

                if (conn.Connected)
                {
                    WriteLog("Connetced" );
                    TestSetTreeManager tsTreeMgr = (TestSetTreeManager)conn.TestSetTreeManager;
                    TestSetFolder tsFolder = (TestSetFolder)tsTreeMgr.get_NodeByPath(testFolder);
                    List tsList = tsFolder.FindTestSets(testSetName, true);
                    foreach (TestSet testSet in tsList)
                    {
                        WriteLog("Search case");
                        String na = testSet.Name;
                        TSTestFactory tsTestFactory = (TSTestFactory)testSet.TSTestFactory;
                        List tsTestList = tsTestFactory.NewList("");

                        IWorkbook workbook = null;
                        string fileExt = Path.GetExtension(testResultFile);
                        using (var file = new FileStream(testResultFile, FileMode.Open, FileAccess.Read))
                        {
                            if (fileExt == ".xls")
                            {
                                workbook = new HSSFWorkbook(file);
                            }
                            else if (fileExt == ".xlsx")
                            {
                                workbook = new XSSFWorkbook(file);
                            }
                            else
                            {

                            }
                        }
                        ISheet sheet = workbook.GetSheetAt(0);

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            string testResult = null;
                            string caseID = null;

                            IRow row = sheet.GetRow(i);
                            if (row != null)
                            {
                                ICell cell = row.GetCell(0);
                                if (cell != null)
                                {
                                    caseID = cell.ToString();

                                    ICell cell2 = row.GetCell(1);
                                    if (cell2 != null)
                                    {
                                        testResult = cell2.ToString();

                                        if (testResult.Equals("Pass"))
                                            testResult = "Passed";
                                        else if (testResult.Equals("Fail"))
                                            testResult = "Failed";
                                        else if (testResult.Equals("Block"))
                                            testResult = "Blocked";
                                        else if (testResult.Equals("NA"))
                                            testResult = "N/A";
                                        else if (testResult.Equals("NR"))
                                            testResult = "No Run";
                                        else if (testResult.Equals("NC"))
                                            testResult = "Not Completed";

                                        WriteLog("Test result is:" + testResult);

                                        foreach (TSTest tsTest in tsTestList)
                                        {

                                            string name = tsTest.Name;
                                            string id = (string)tsTest.TestId;

                                            Run lastRun = (Run)tsTest.LastRun;
                                            string stas = (string)tsTest.Status;

                                            if (id == caseID)
                                            {
                                                WriteLog("Update case[" + caseID + "] status");
                                                RunFactory runFactory = (RunFactory)tsTest.RunFactory;
                                                String date = DateTime.Now.ToString("yyyyMMddhhmmss");
                                                Run run = (Run)runFactory.AddItem("Run" + date);

                                                run.Status = testResult;
                                                run.Post();
                                                Thread.Sleep(1000);
                                            }

                                        }

                                    }
                                }

                            }

                        }


                        

                    }
                    conn.DisconnectProject();
                    conn.Disconnect();
                }


            }
            catch (Exception e)
            {
                WriteLog(e.Message);
                if (conn.Connected)
                {
                    conn.DisconnectProject();
                    conn.Disconnect();
                }
            }
            finally
            {
                if (conn.Connected)
                {
                    conn.DisconnectProject();
                    conn.Disconnect();
                }
            }


        }
    
  
        public  static void WriteLog(string logTxt)  
        {  
            try  
            {  
            FileStream fs = new FileStream(LogPath + LogName, System.IO.FileMode.Append);  
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            DateTime dt = DateTime.Now;
            sw.WriteLine(string.Format("{0:s}", dt) + "+0800\t" + logTxt);
            sw.Flush();
            sw.Close();  
            fs.Close();  
            }  
            catch (Exception e)
            {
                
            }  
        }  
       
    }

}
