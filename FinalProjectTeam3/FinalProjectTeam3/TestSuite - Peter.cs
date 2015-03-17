// <copyright file="TestSuite.cs" company="[petsan]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace FinalProjectTeam3
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    using TestingMentor.TestAutomationFramework;
    using TestingMentor.TestTool.Babel;

    /// <summary>
    /// Test suite for [project name] 
    /// </summary>
    public class TestSuitePeter : TestBase
    {
        /// <summary>
        /// TEST OBJECTIVE: [Brief statement explaining purpose of test.]
        /// SETUP: [Information regarding machine state, test data, tools, etc for test to run.] 
        /// STEPS:
        /// 1. [Step 1]
        /// 2. [Step 2, etc.]
        /// EXPECTED RESULT: [Expected outcome of test]
        /// </summary>
        public TestSuitePeter()
        {
            try
            {
                this.Initialize();

                // Lanch TED Notepad application
                Process tedApp = ProcessHelper.LaunchApplication(TedEnvironmentInfo.TedApplicationExe);

                // Get the Application Handle
                IntPtr tedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();

                // Save the file
                MenuHelper.ClickMenuItem(tedAppHandle, (int)TEDnotepadHelper.MenuItems.File, (int)TEDnotepadHelper.FileMenuItems.SaveAs);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                IntPtr saveAsDialog = DialogHelper.GetDialogHandle(tedAppHandle);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                // get random file name and assign it to saveFilename
                string randomfile = Path.GetRandomFileName();
                string saveFileName = randomfile;
                Logger.Comment(string.Format("File name = {0}", saveFileName));

                SendKeys.SendWait(saveFileName);
                DialogHelper.ClickButton(saveAsDialog, (int)TEDnotepadHelper.SaveAsDialog.Savebtn);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                // Close the application
                ProcessHelper.CloseApplication(tedApp);

                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }
    }
}