// <copyright file="TestSuite.cs" company="[YourName]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace FinalProjectTeam3
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using TestingMentor.TestAutomationFramework;
    using System.Windows.Forms;

    /// <summary>
    /// Test suite for [project name] 
    /// </summary>
    public class TestSuiteRyan : TestBase
    {
        /// <summary>
        /// TEST OBJECTIVE: Open the Ted Notepad application, enter some text and save the file
        /// SETUP: Use a test data file for entering the data for use in other tests 
        /// STEPS:
        /// 1. Launch application 
        /// 2. Enter some data
        /// 3. Save the file 
        /// 4. Verify the file data
        /// EXPECTED RESULT: The file has been saved
        /// </summary>
        public void TednPadSaveTest()
        {
            try
            {
                this.Initialize();

                //Lanch TednPad application
                Process TedApp = ProcessHelper.LaunchApplication(TedEnvironmentInfo.TedApplicationExe);

                //Get the Application Handle
                IntPtr TedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();

                //initialize variables that will be used to complete the test
                string EnteredDataInfo = string.Empty;
                string SavedFileData = string.Empty;
                string saveFileName = string.Empty;
                string[] testData = TestDataReader.GetAllTestDataFromTextFile(Path.Combine(GetWorkingDirectory(), TedEnvironmentInfo.DataFolder, TedEnvironmentInfo.TednPadSaveTestData + TedEnvironmentInfo.FileType));

                //Loop through the test data provided - Currently only 1 line of data
                foreach (string s in testData)
                {
                    string[] testDataElements = TestDataReader.ParseTestCaseDataElements(s, TestDataReader.DelimiterCharacter.Comma);

                    DialogHelper.SetTextboxText(TedAppHandle, (int)TEDnotepadHelper.TedApplication.MainTextEntryField, testDataElements[0]);
                    EnteredDataInfo = testDataElements[0];
                }

                //Save the file
                MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.File, (int)TEDnotepadHelper.FileMenuItems.SaveAs);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                IntPtr SaveAsDialog = DialogHelper.GetDialogHandle(TedAppHandle);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                //get random file name and assign it to saveFilename
                string randomfile = Path.GetRandomFileName();
                saveFileName = randomfile;
                Logger.Comment(string.Format("File name = {0}", saveFileName));

                SendKeys.SendWait(saveFileName);
                DialogHelper.ClickButton(SaveAsDialog, (int)TEDnotepadHelper.SaveAsDialog.Savebtn);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                //Close the application
                ProcessHelper.CloseApplication(TedApp);

                //Read the saved file data
                SavedFileData = GetDatafromFile(SavedFileData, TedEnvironmentInfo.SaveLocation.ToString() + saveFileName, TedEnvironmentInfo.FileType);

                //Compare the entered data with the saved file
                Logger.ReportResult(SavedFileData, EnteredDataInfo);

                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }

        public void TednPadFindTest()
        {
            try
            {
                this.Initialize();

                //Lanch TednPad application
                Process TedApp = ProcessHelper.LaunchApplication(TedEnvironmentInfo.TedApplicationExe);

                //Get the Application Handle
                IntPtr TedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();

                //initialize variables that will be used to complete the test
                string EnteredDataInfo = string.Empty;
                string SavedFileData = string.Empty;
                string saveFileName = string.Empty;
                string positiveSearchWord = string.Empty;
                string negativeSearchword = string.Empty;
                string[] testData = TestDataReader.GetAllTestDataFromTextFile(Path.Combine(GetWorkingDirectory(), TedEnvironmentInfo.DataFolder, TedEnvironmentInfo.TednPadFindTestData + TedEnvironmentInfo.FileType));

                //Loop through the test data provided - Currently only 1 line of data
                foreach (string s in testData)
                {
                    string[] testDataElements = TestDataReader.ParseTestCaseDataElements(s, TestDataReader.DelimiterCharacter.Comma);

                    DialogHelper.SetTextboxText(TedAppHandle, (int)TEDnotepadHelper.TedApplication.MainTextEntryField, testDataElements[0]);
                    EnteredDataInfo = testDataElements[0];
                    saveFileName = testDataElements[1];
                    positiveSearchWord = testDataElements[2];
                    negativeSearchword = testDataElements[3];

                    bool doesFileExist = DoesFileExist(TedEnvironmentInfo.SaveLocation.ToString() + saveFileName + TedEnvironmentInfo.FileType.ToString());
                    if (doesFileExist == true)
                    {
                        DeleteFile(TedEnvironmentInfo.SaveLocation.ToString() + saveFileName + TedEnvironmentInfo.FileType.ToString());
                    }

                    //Search menu select Find
                    MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.Search, (int)TEDnotepadHelper.SearchMenuItems.Find);

                    //Enter negative search
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    IntPtr findHandle = DialogHelper.GetDialogItemHandle(TedAppHandle, 0);
                    DialogHelper.SetTextboxText(findHandle, (int)TEDnotepadHelper.Findblock.Findtextbox, negativeSearchword);

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    DialogHelper.ClickButton(findHandle, (int)TEDnotepadHelper.Findblock.Nextbtn);

                    //informational message displays
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    IntPtr NoneFound = DialogHelper.GetDialogHandle(TedAppHandle);
                    IntPtr InfoText = DialogHelper.GetDialogItemHandle(NoneFound, 0xFFFF);
                    string infoMessageText = DialogHelper.GetDialogItemText(NoneFound, InfoText);
                    Logger.Equals(infoMessageText, string.Format("Could not find{0}", negativeSearchword));
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    DialogHelper.ClickButton(NoneFound, 0x2);

                    //Enter Positive search
                    DialogHelper.SetTextboxText(findHandle, (int)TEDnotepadHelper.Findblock.Findtextbox, positiveSearchWord);
                    DialogHelper.SetCheckboxState(DialogHelper.GetDialogItemHandle(findHandle,(int)TEDnotepadHelper.Findblock.Wraparoundchbx), true);
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    DialogHelper.ClickButton(findHandle, (int)TEDnotepadHelper.Findblock.Nextbtn);

                    //Save the file
                    MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.File, (int)TEDnotepadHelper.FileMenuItems.SaveAs);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                    IntPtr SaveAsDialog = DialogHelper.GetDialogHandle(TedAppHandle);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    Logger.Comment(string.Format("File name = {0}", saveFileName));
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    SendKeys.SendWait(saveFileName);
                    DialogHelper.ClickButton(SaveAsDialog, (int)TEDnotepadHelper.SaveAsDialog.Savebtn);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                //Close the application
                ProcessHelper.CloseApplication(TedApp);

                //Oracle if the file contains the positive search word then the test passes
                SavedFileData = GetDatafromFile(SavedFileData, TedEnvironmentInfo.SaveLocation.ToString() + saveFileName, TedEnvironmentInfo.FileType);
                if (SavedFileData.Contains(positiveSearchWord))
                {
                    Logger.Comment("Test Passed");
                }
                else
                {
                    Logger.Comment("Test Failed");
                }

                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }

        public void TednPadReplaceTest()
        {
            try
            {
                this.Initialize();

                //Lanch TednPad application
                Process TedApp = ProcessHelper.LaunchApplication(TedEnvironmentInfo.TedApplicationExe);

                //Get the Application Handle
                IntPtr TedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();

                //initialize variables that will be used to complete the test
                string EnteredDataInfo = string.Empty;
                string SavedFileData = string.Empty;
                string saveFileName = string.Empty;
                string findWord = string.Empty;
                string replaceWord = string.Empty;
                string[] testData = TestDataReader.GetAllTestDataFromTextFile(Path.Combine(GetWorkingDirectory(), TedEnvironmentInfo.DataFolder, TedEnvironmentInfo.TednPadReplaceData + TedEnvironmentInfo.FileType));

                //Loop through the test data provided
                foreach (string s in testData)
                {
                    string[] testDataElements = TestDataReader.ParseTestCaseDataElements(s, TestDataReader.DelimiterCharacter.Comma);

                    DialogHelper.SetTextboxText(TedAppHandle, (int)TEDnotepadHelper.TedApplication.MainTextEntryField, testDataElements[0]);
                    EnteredDataInfo = testDataElements[0];
                    saveFileName = testDataElements[1];
                    findWord = testDataElements[2];
                    replaceWord = testDataElements[3];

                    bool doesFileExist = DoesFileExist(TedEnvironmentInfo.SaveLocation.ToString() + saveFileName + TedEnvironmentInfo.FileType.ToString());
                    if (doesFileExist == true)
                    {
                        DeleteFile(TedEnvironmentInfo.SaveLocation.ToString() + saveFileName + TedEnvironmentInfo.FileType.ToString());
                        Logger.Comment("The file was deleted");
                    }

                    //Search menu select Find
                    MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.Search, (int)TEDnotepadHelper.SearchMenuItems.Replace);

                    //Enter search text
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    IntPtr findHandle = DialogHelper.GetDialogItemHandle(TedAppHandle, 0);
                    DialogHelper.SetTextboxText(findHandle, (int)TEDnotepadHelper.Findblock.Findtextbox, findWord);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    DialogHelper.ClickButton(findHandle, (int)TEDnotepadHelper.Findblock.Nextbtn);

                    //Enter Replace text
                    DialogHelper.SetTextboxText(findHandle, (int)TEDnotepadHelper.Replaceblock.Replacetextbox, replaceWord);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    DialogHelper.ClickButton(findHandle, (int)TEDnotepadHelper.Replaceblock.Replacebtn);

                    //Save the file
                    MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.File, (int)TEDnotepadHelper.FileMenuItems.SaveAs);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                    IntPtr SaveAsDialog = DialogHelper.GetDialogHandle(TedAppHandle);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    Logger.Comment(string.Format("File name = {0}", saveFileName));
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    SendKeys.SendWait(saveFileName);
                    DialogHelper.ClickButton(SaveAsDialog, (int)TEDnotepadHelper.SaveAsDialog.Savebtn);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                //Close the application
                ProcessHelper.CloseApplication(TedApp);

                //Oracle if the file contains the replaced word the test has passed
                SavedFileData = GetDatafromFile(SavedFileData, TedEnvironmentInfo.SaveLocation.ToString() + saveFileName, TedEnvironmentInfo.FileType);
                if (SavedFileData.Contains(replaceWord))
                {
                    Logger.Comment("Test Passed");
                }
                else
                {
                    Logger.Comment("Test Failed");
                }
                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }
        private string GetDatafromFile(string SavedFileData, string saveFileName, string fileExtension)
        {
            byte[] buffer;
            FileStream fs = new FileStream(Path.Combine(TedEnvironmentInfo.SaveLocation, saveFileName + fileExtension), FileMode.Open, FileAccess.Read);

            try
            {
                int length = (int)fs.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;


                while ((count = fs.Read(buffer, sum, length - sum)) > 0)
                {
                    SavedFileData = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                    Logger.Comment(SavedFileData);
                    sum += count;
                }
            }
            finally
            {

                fs.Close();
            }
            return SavedFileData;
        }
        private bool DoesFileExist(string saveFileName)
        {
            bool result = File.Exists(saveFileName);
            return result;
        }
        private bool DeleteFile(string saveFileName)
        {
            bool result;
            try
            {
                File.Delete(saveFileName);
                result = true;
            }
            catch (IOException e)
            {
                result = false;
                Logger.Comment(e.ToString());

            }
            return result;
        }
        private string GetWorkingDirectory()
        {
            string releaseDirectory = "release";
            //Set the working directory to the project directory
            string workingDirectory = Environment.CurrentDirectory;
            int pathLength = workingDirectory.Length;

            if (workingDirectory.EndsWith(releaseDirectory, StringComparison.InvariantCultureIgnoreCase))
            {
                workingDirectory = workingDirectory.Remove(pathLength - releaseDirectory.Length);
            }
            else
            {
                workingDirectory = workingDirectory.Remove(pathLength - releaseDirectory.Length - 2);
            }
            return workingDirectory;
        }
    }
    public class TedEnvironmentInfo
    {
        public static string TedApplicationExe = @"C:\Users\Family\Desktop\TedNPad.exe";

        public static string FileType = ".txt";

        public static string SaveLocation = @"C:\Users\Family\Documents\";

        public static string TednPadSaveTestData = "SaveTestData";

        public static string TednPadFindTestData = "FindTestData";

        public static string TednPadReplaceData = "ReplaceTestData";

        public static string DataFolder = "Data";
    }

}