// <copyright file="TestSuite.cs" company="[petsan]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace FinalProjectTeam3
{
    using System;
    using System.Data;
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

                Logger.Comment("start the Select Cut Word Test");
                SelectCutWordTestMethod();

                Logger.Comment("start the Delete Line Test");
                DeleteLineTestMethod();

                Logger.Comment("start the Insert Recently Deleted Test");
                InsertRecentlyDeletedTestMethod();

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
        /// The select cut word test method.
        /// </summary>
        private static void SelectCutWordTestMethod()
        {
            Logger.Comment("starting test case 1");
            IntPtr tedAppHandle;
            var tedApp = StartTedAppProcess(out tedAppHandle);

            Logger.Comment("get test paragraph");
            string testParagraph = GetTextForEditor(TextOption.UniCode);

            Logger.Comment("paste test paragraph");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            DialogHelper.SetTextboxText(
                tedAppHandle,
                (int)TEDnotepadHelper.TedApplication.MainTextEntryField,
                testParagraph);

            Logger.Comment("save file");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            SaveFile(tedAppHandle);

            // TODO: Get File name

            Logger.Comment("select Word");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            MenuHelper.ClickMenuItem(
                tedAppHandle,
                (int)TEDnotepadHelper.MenuItems.Edit,
                (int)TEDnotepadHelper.EditMenuItems.SelectWord);

            Logger.Comment("cut word");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            MenuHelper.ClickMenuItem(
                tedAppHandle,
                (int)TEDnotepadHelper.MenuItems.Edit,
                (int)TEDnotepadHelper.EditMenuItems.Cut);

            //TODO: retrieve the cut word into a string

            Logger.Comment("save new file");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

            string saveFileName = string.Empty;

            SaveFile(tedAppHandle);

            // TODO: Get File name

            Logger.Comment("Close the application");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            ProcessHelper.CloseApplication(tedApp);

            // TODO: ORACLE to see if the first word is cut
            // Oracle if the file contains the positive search word then the test passes
        }

        /// <summary>
        /// The delete line test method.
        /// </summary>
        private static void DeleteLineTestMethod()
        {
            Logger.Comment("starting test case 2");
            IntPtr tedAppHandle;
            var tedApp = StartTedAppProcess(out tedAppHandle);

            Logger.Comment("get test paragraph");
            string testParagraph = GetTextForEditor(TextOption.UniCode);

            Logger.Comment("paste test paragraph");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            DialogHelper.SetTextboxText(
                tedAppHandle,
                (int)TEDnotepadHelper.TedApplication.MainTextEntryField,
                testParagraph);

            Logger.Comment("save file");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            SaveFile(tedAppHandle);

            // TODO: Get File name

            // TODO: Get first line into a string

            Logger.Comment("delete line");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            MenuHelper.ClickMenuItem(
                tedAppHandle,
                (int)TEDnotepadHelper.MenuItems.Edit,
                (int)TEDnotepadHelper.EditMenuItems.DeleteLine);

            Logger.Comment("save new file");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            SaveFile(tedAppHandle);

            // TODO: Get File name

            Logger.Comment("Close the application");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            ProcessHelper.CloseApplication(tedApp);

            // TODO: ORACLE to compare files to see if string is in the new file
        }

        /// <summary>
        /// The insert recently deleted test method.
        /// </summary>
        private static void InsertRecentlyDeletedTestMethod()
        {
            Logger.Comment("starting test case 3");
            IntPtr tedAppHandle;
            var tedApp = StartTedAppProcess(out tedAppHandle);

            Logger.Comment("get test paragraph");
            string testParagraph = GetTextForEditor(TextOption.UniCode);

            Logger.Comment("paste test paragraph");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            DialogHelper.SetTextboxText(
                tedAppHandle,
                (int)TEDnotepadHelper.TedApplication.MainTextEntryField,
                testParagraph);

            Logger.Comment("save file");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            SaveFile(tedAppHandle);

            // TODO: Get File name


            // TODO: Copy all text into a string

            Logger.Comment("select All");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            MenuHelper.ClickMenuItem(
                tedAppHandle,
                (int)TEDnotepadHelper.MenuItems.Edit,
                (int)TEDnotepadHelper.EditMenuItems.SelectAll);

            Logger.Comment("delete all");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            MenuHelper.ClickMenuItem(
                tedAppHandle,
                (int)TEDnotepadHelper.MenuItems.Edit,
                (int)TEDnotepadHelper.EditMenuItems.Cut);

            Logger.Comment("insert recently deleted");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            SendKeys.SendWait("^Q");

            Logger.Comment("save new file");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            SaveFile(tedAppHandle);

            // TODO: Get File name

            Logger.Comment("Close the application");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            ProcessHelper.CloseApplication(tedApp);

            // TODO: ORACLE to see if the string exists in the new file
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
        /// Generic method to generate string of random text characters
        /// </summary>
        /// <param name="seed">Seed value</param>
        /// <returns>A string of unicode characters</returns>
        private static string GetTextString(int maxCharCount, out int seed)
        {
            Random prng = new Random();
            seed = prng.Next();
            StringGenerator sg = new StringGenerator();
            sg.Info.IsSendKeysSafe = true;
            sg.Info.Seed = seed;
            sg.Info.MaximumCharacterCount = maxCharCount;
            sg.Info.RandomizeCharacterCount = false;
            sg.Info.AllowControlCharacters = false;
            sg.Info.AllowFormattingCharacters = true;
            sg.Info.LanguageGroup = 2;
            sg.Info.UseCustomRange = true;
            sg.Info.CustomUnicodeStartRange = '0';
            sg.Info.CustomUnicodeEndRange = 'z';
            sg.Info.AllowNumberCharacters = true;
            return sg.Polyglot();
        }

        /// <summary>
        /// Get paragraph text to paste into the TED note pad file
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
            string returnText = string.Empty;

            // if option long, returns long paragraph, if options short, returns short paragraph
            if (option == TextOption.UniCode)
            {
                string word = string.Empty;
                string[] words = new string[2000];
                for (int runs = 0; runs < 2000; runs++)
                {
                    int seedValue = 15;
                    word = GetTextString(30, out seedValue);
                    words[runs] = word;
                }

                RandomText text = new RandomText(words);
                text.AddContentParagraphs(3, 3, 4, 5, 10);
                returnText = text.Content;
            }
            else if (option == TextOption.ASCII)
            {
                string[] words = 
                    {
                        "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed",
                        "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna",
                        "aliquam", "erat"
                    };
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
        /// <summary>
        /// The short.
        /// </summary>
        UniCode,
        ASCII
    }
}