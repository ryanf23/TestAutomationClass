// <copyright file="TestSuite.cs" company="[YourName]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace FinalProjectTeam3
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using TestingMentor.TestAutomationFramework;
    using TestingMentor.TestTool.Babel;

    /// <summary>
    /// Test suite for [project name] 
    /// </summary>
    public class TestSuiteSahana : TestBase
    {
        private readonly string path = "C:\\Sahana\\TestData\\";

        /// <summary>
        /// TEST OBJECTIVE: [Brief statement explaining purpose of test.]
        /// SETUP: [Information regarding machine state, test data, tools, etc for test to run.] 
        /// STEPS:
        /// 1. [Step 1]
        /// 2. [Step 2, etc.]
        /// EXPECTED RESULT: [Expected outcome of test]
        /// </summary>
        public void TestMethod()
        {
            try
            {
                this.Initialize();
                //Create a test file and save it to the test directory

                string TedpadApplication = path + "TedNPad.exe";
                Logger.TestStep("Launch TedPad");
                Process TedpadProcess = ProcessHelper.LaunchApplication(TedpadApplication);

                Logger.Comment("Get multiine text");
                //Get string for editor
                string text = GetMultiLineTextForEditor();

                Logger.TestStep("Send text to TedPad edit control");
                SendKeys.SendWait(text);

                // SendKeys is slow...so we need to sleep to allow text to get entered into edit window
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                Logger.Comment("Get Tednotepad window handle.");
                IntPtr TedpadHandle = ProcessHelper.GetMainForegroundWindowHandle();

                DialogHelper.SelectMenuItem(TedpadHandle, (int)TEDnotepadHelper.MenuItems.Search, (int)TEDnotepadHelper.SearchMenuItems.Find);
                //SendKeys.SendWait("^f");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));
                var findHandle = DialogHelper.GetDialogItemHandle(TedpadHandle,0);
               
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));
                DialogHelper.SetTextboxText(findHandle, (int)TEDnotepadHelper.Findblock.Findtextbox, "searchtext2");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));
                DialogHelper.ClickButton(
                   findHandle,
                   (int)TEDnotepadHelper.Findblock.Cancelbtn);
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));
                ////Logger.TestStep("Save file as Unicode encoding with random file name");
                ////string filename = Path.Combine(Environment.GetFolderPath(
                ////    Environment.SpecialFolder.Desktop),
                ////    Path.GetRandomFileName() + ".txt");

                ////Logger.TestStep("Save TedPad as text file");
                ////TEDnotepadHelper.SaveTedFile(filename);

                ////System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                ////Logger.TestStep("Close TedPad");
                ////ProcessHelper.CloseApplication(TedpadProcess);


                ////Logger.TestStep("Launch TedPad app");
                ////TedpadProcess = ProcessHelper.LaunchApplication(TedpadApplication);

                ////Logger.TestStep("Open test file");
                ////TEDnotepadHelper.OpenFile(filename);
               
                
                Logger.Comment("Close TedPad");
                ProcessHelper.CloseApplication(TedpadProcess);  
                //Test case operation
                ////Logger.TestStep("commad to convert to upper");
                ////SendKeys.SendWait("^+U");                

                ////Console.WriteLine(string.Format("Generated string Value: {0}", text));               
                ////Console.ReadLine();
                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }

        public static void SendMessageToTextBox(IntPtr myPhonebkHandle, int textboxId, string text)
        {
            IntPtr textBoxHandle = DialogHelper.GetDialogItemHandle(myPhonebkHandle, textboxId);
            NativeMethod.SendMessage(textBoxHandle, (int)0x000C, IntPtr.Zero, text);
        }

        private static string GetMultiLineTextForEditor()
        {
            int seedValue;
            string firstLine = GetTextString(15, out seedValue);
            string secondLine = GetTextString(20, out seedValue);
            string thirdLine = GetTextString(50, out seedValue);
            string FinalText = string.Concat(firstLine, " ", secondLine, secondLine, System.Environment.NewLine, thirdLine);
            return FinalText;
        }


        /// <summary>
        /// Generic method to generate string of random Unicode characters
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
        /// Generic method to generate string of random Unicode characters from a given seed value
        /// </summary>
        /// <param name="seed">Seed value</param>
        /// <returns>A string of unicode characters</returns>
        private static string GetUnicodeStringSeed(int seed)
        {
            Random prng = new Random();
            StringGenerator sg = new StringGenerator();
            sg.Info.IsSendKeysSafe = true;
            sg.Info.Seed = seed;
            sg.Info.MaximumCharacterCount = 10;
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
        /// A method to get all the text from a Ted edit note pad
        /// </summary>
        /// <returns>The string copied to the clipboard</returns>
        private static string GetUnicodeTextFromTEDEditControl()
        {
            Clipboard.Clear();
            SendKeys.SendWait("^a");
            System.Threading.Thread.Sleep(250);
            SendKeys.SendWait("^c");
            if (string.IsNullOrEmpty(Clipboard.GetText()))
            {
                throw new ArgumentOutOfRangeException("No data on clipboard.");
            }

            return Clipboard.GetText(TextDataFormat.UnicodeText);
        }
    }
}