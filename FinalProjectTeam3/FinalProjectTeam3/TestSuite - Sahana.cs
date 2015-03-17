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
    using System.Configuration;
    /// <summary>
    /// Test suite for [project name] 
    /// </summary>
    public class TestSuiteSahana : TestBase
    {

        private readonly string testPath;
        private readonly string tedNPadApp;
        private readonly string saveFileType;

        public TestSuiteSahana()
        {
            testPath = ConfigurationManager.AppSettings["SahanaTestPath"];
            tedNPadApp = ConfigurationManager.AppSettings["SahanaEditorApp"];
            saveFileType = ConfigurationManager.AppSettings["SahanaSaveFileType"];
        }

        /// <summary>
        /// Get Multiline text to save to the TedPadFile
        /// </summary>
        /// <returns></returns>
        private static string GetMultiLineTextForEditor()
        {
            int seedValue1;
            int seedValue2;
            int seedValue3;
            string firstLine = GetTextString(15, out seedValue1);
            string secondLine = GetTextString(20, out seedValue2);
            string thirdLine = GetTextString(50, out seedValue3);
            string FinalText = string.Concat(firstLine, System.Environment.NewLine, secondLine, System.Environment.NewLine, thirdLine);
            return FinalText;
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
        /// Create a test file and return test file name
        /// </summary>
        /// <returns></returns>
        private string CreateTestFile()
        {
            String TedApp = testPath + tedNPadApp;
            Logger.TestStep("Lauch TedApp ");
            Process TedProcess = ProcessHelper.LaunchApplication(TedApp);

            Logger.TestStep("Getting the Text for the Editor");
            string Text = GetMultiLineTextForEditor();

            Logger.TestStep("Write the Text into editor");
            SendKeys.SendWait(Text);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

            Logger.TestStep("Save the File");
            String fileName = testPath + Path.GetRandomFileName() + saveFileType;

            TEDnotepadHelper.SaveTedFile(fileName);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

            ProcessHelper.CloseApplication(TedProcess);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            return fileName;
        }

        private void ReportResult(string[] actual, string[] expected)
        {
            bool result = true;
            if(actual.Length == expected.Length)
            {
                for(int i=0;i<=actual.Length-1;i++)
                {
                    Logger.Comment(string.Format("Actual data array {0} is : {1}", i, actual[i]));
                    Logger.Comment(string.Format("Expected data array {0} is : {1}", i, expected[i]));
                    if (actual[i] != expected[i])
                    {
                        result = false;
                    }                   
                }
            }
            else
            {
                result = false;
                Logger.Comment("Acutal and expected array length did not match");
            }
            if (result == true)
            {
                Logger.Comment("Test Pass");
            }
            else
            {
                Logger.Comment("Test FAILED");
            }
        }

        /// <summary>
        /// TEST OBJECTIVE: To test convert to upper scenarios of TedNPad
        /// SETUP: TedNPad application, Testdata generator 
        /// STEPS:
        /// 1. Generate a new test file
        /// 2. Read the test data to string array
        /// 3. Convert first line of file to Upper and save it to result string array
        /// 4. Move to next line, select partial text and convert to upper
        /// 4. Convert all text to Upper and save it to result string array
        /// 5. Compare the log the results.
        /// EXPECTED RESULT: [Expected outcome of test]
        public void UpperCaseFirstLineTestMethod()
        {
            try
            {
                this.Initialize();
                Logger.TestStep("Start UpperCasteTestMethod");
                //Test Varialbes
                string fileName = string.Empty;
                string[] expectedData = { };
                string[] convertedData = { };

                Logger.TestStep("Create a test file");
                fileName = CreateTestFile();
                Logger.Comment(string.Format("Test file created with name:{0}", fileName));

                Logger.TestStep("Read saved data to a string array");
                expectedData = File.ReadAllLines(fileName);
                if (expectedData.Length > 0)
                {
                    expectedData[0] = expectedData[0].ToUpper();
                }

                Logger.TestStep("Launch TedNPadd application");
                Process TedAppProcess = ProcessHelper.LaunchApplication(testPath + tedNPadApp);
                IntPtr TedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();

                Logger.TestStep("Open the test file");
                TEDnotepadHelper.OpenFile(fileName);

                Logger.TestStep("Pass command control+shift+U to convert to upper case");
                SendKeys.SendWait("^+U");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2500));

                Logger.TestStep("Save and exit the modified file");
                SendKeys.SendWait("{F10}");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(5000));

                Logger.TestStep("Read the modified file data to a new array");
                convertedData = File.ReadAllLines(fileName);

                Logger.TestStep("Compare initial and converted data and report the results");
                ReportResult(convertedData, expectedData);

                Logger.TestStep("End UpperCasteTestMethod");
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
           
        }


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

                string TedpadApplication = testPath + tedNPadApp;
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