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
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using TestingMentor.TestAutomationFramework;
    using TestingMentor.TestTool.Babel;

    /// <summary>
    /// Test suite for Peter Sankiewicz 
    /// </summary>
    public class TestSuitePeter : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuitePeter"/> class. 
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
                IntPtr tedAppHandle;
                var tedApp = StartTedAppProcess(out tedAppHandle);

                // Get test Paragraphs into the Editor
                string testParagraph;
                Get_testParagraph(tedAppHandle, out testParagraph);

                // Save the file
                SaveFile(tedAppHandle);

                // 1. Edit-->Select word


                // 2. Edit-->Delete LIne

                // 3. Edit-->Insert-->Recently Deleted


                // Close the application
                // ProcessHelper.CloseApplication(tedApp);

                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }

        /// <summary>
        /// The get_test paragraph method.
        /// </summary>
        /// <param name="tedAppHandle">
        /// The ted app handle.
        /// </param>
        /// <param name="testParagraph">
        /// The test paragraph.
        /// </param>
        private static void Get_testParagraph(IntPtr tedAppHandle, out string testParagraph)
        {
            Logger.TestStep("Get test Paragraphs for the Editor");
            testParagraph = GetTextForEditor(TextOption.Short);

            Logger.TestStep("Write the Text into editor");
            DialogHelper.SetTextboxText(tedAppHandle, (int)TEDnotepadHelper.TedApplication.MainTextEntryField, testParagraph);
        }

        /// <summary>
        /// The start_ted app_process method.
        /// </summary>
        /// <param name="tedAppHandle">
        /// The ted app handle.
        /// </param>
        /// <returns>
        /// The <see cref="Process"/> tedApp .
        /// </returns>
        private static Process StartTedAppProcess(out IntPtr tedAppHandle)
        {
            Logger.TestStep("Lanch TED Notepad application"); 
            Process tedApp = ProcessHelper.LaunchApplication(TedEnvironmentInfo.TedApplicationExe);

            Logger.TestStep("Get the Application Handle");
            tedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();
            return tedApp;
        }

        /// <summary>
        /// The save file method.
        /// </summary>
        /// <param name="tedAppHandle">
        /// The ted app handle.
        /// </param>
        private static void SaveFile(IntPtr tedAppHandle)
        {
            // Save the file
            MenuHelper.ClickMenuItem(
                tedAppHandle,
                (int)TEDnotepadHelper.MenuItems.File,
                (int)TEDnotepadHelper.FileMenuItems.SaveAs);

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

            IntPtr saveAsDialog = DialogHelper.GetDialogHandle(tedAppHandle);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

            // get random file name and assign it to saveFilename
            string saveFileName = Path.GetRandomFileName();
            
            Logger.Comment(string.Format("File name = {0}", saveFileName));
            SendKeys.SendWait(saveFileName);
            DialogHelper.ClickButton(saveAsDialog, (int)TEDnotepadHelper.SaveAsDialog.Savebtn);

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Get paragraph text to save to the TED note pad file
        /// </summary>
        /// <param name="option">
        /// short | long
        /// </param>
        /// <returns>
        /// Random Text
        /// </returns>
        private static string GetTextForEditor(TextOption option)
        {
            // get seedValue
            // int seedValue;
            string returnText = string.Empty;

            // if option long, returns long paragraph, if options short, returns short paragraph
            if (option == TextOption.Long)
            {
                string[] words = { "anemone", "wagstaff", "man", "the", "for", "and", "a", "with", "bird", "fox" };
                RandomText text = new RandomText(words);
                text.AddContentParagraphs(5, 8, 12, 50, 100);
                returnText = text.Content;
            }
            else if (option == TextOption.Short)
            {
                string[] words = { "anemone", "wagstaff", "man", "the", "for", "and", "a", "with", "bird", "fox" };
                RandomText text = new RandomText(words);
                text.AddContentParagraphs(2, 1, 1, 2, 4);
                returnText = text.Content;
            }
            return returnText;
        }
    }

    /// <summary>
    /// The text option.
    /// </summary>
    public enum TextOption
    {
        Short = 1,
        Long = 2
    }
}