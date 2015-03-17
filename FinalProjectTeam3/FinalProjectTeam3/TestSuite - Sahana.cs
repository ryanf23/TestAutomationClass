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
    using System.Linq;
    /// <summary>
    /// Test suite for [project name] 
    /// </summary>
    public class TestSuiteSahana : TestBase
    {
        public enum TextOption
        {
            MultiLine = 1,
            MultiWord = 2            
        }

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
        private static string GetTextForEditor(TextOption option)
        {
            int seedValue;
            string firstLine = GetTextString(15, out seedValue);
            string secondLine = GetTextString(20, out seedValue);
            string thirdLine = GetTextString(50, out seedValue);
            string FinalText = string.Empty;
            if (option == TextOption.MultiLine)
            {
                FinalText = string.Concat(firstLine, "\n", secondLine, "\n", thirdLine);
            }
            else if(option == TextOption.MultiWord)
            {
                FinalText = string.Concat(firstLine, " ", secondLine, " ", thirdLine);
            }            
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
        public string CreateTestFile(TextOption option)
        {
            String TedApp = testPath + tedNPadApp;
            Logger.TestStep("Lauch TedApp ");
            Process TedProcess = ProcessHelper.LaunchApplication(TedApp);

            Logger.TestStep("Getting the Text for the Editor");
            string Text = GetTextForEditor(option);

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
        /// 4. Compare the log the results.
        /// EXPECTED RESULT: first line in file should be all upper case
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
                fileName = CreateTestFile(TextOption.MultiLine);
                Logger.Comment(string.Format("Test file created with name:{0}", fileName));

                Logger.TestStep("Read saved data to a string array");
                expectedData = File.ReadAllLines(fileName);
                if (expectedData.Length > 0)
                {
                    expectedData[0] = expectedData[0].ToUpper();
                }

                Logger.TestStep("Launch TedNPadd application");
                Process TedAppProcess = ProcessHelper.LaunchApplication(testPath + tedNPadApp);                

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
        /// TEST OBJECTIVE: To test convert to upper scenarios of TedNPad for  selected Word
        /// SETUP: TedNPad application, Testdata generator 
        /// STEPS:
        /// 1. Generate a new test file
        /// 2. Read the test data to string array
        /// 3. Convert selected word in file to Upper and save it to result string array
        /// 4. Compare and log the results.
        /// EXPECTED RESULT: selected Word in file should be all upper case
        public void UpperCaseSelectWordTestMethod()
        {
            try
            {
                this.Initialize();
                Logger.TestStep("Start UpperCaseSelectWordTestMethod");
                //Test Varialbes
                string fileName = string.Empty;
                string[] expectedData = { };
                string[] convertedData = { };

                Logger.TestStep("Create a test file");
                fileName = CreateTestFile(TextOption.MultiWord);
                Logger.Comment(string.Format("Test file created with name:{0}", fileName));

                Logger.TestStep("Read saved data to a string array");
                string expectedStringData = File.ReadAllText(fileName);
                expectedData = expectedStringData.Split(null);                
                if (expectedData.Length > 0)
                {
                    expectedData[0] = expectedData[0].ToUpper();
                }

                Logger.TestStep("Launch TedNPadd application");
                Process TedAppProcess = ProcessHelper.LaunchApplication(testPath + tedNPadApp);
                IntPtr TedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();
                Logger.Comment("TedHandle:" + TedAppHandle);
                Logger.TestStep("Open the test file");
                TEDnotepadHelper.OpenFile(fileName);


                Logger.TestStep("Select word ");
                MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.Edit, (int)TEDnotepadHelper.EditMenuItems.SelectWord);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                SendKeys.SendWait("^D");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                Logger.TestStep("Pass command control+shift+U to convert to upper case");
                SendKeys.SendWait("^+U");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2500));

                Logger.TestStep("Save and exit the modified file");
                SendKeys.SendWait("{F10}");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(5000));

                Logger.TestStep("Read the modified file data to a new array");
                string convertedStrData = File.ReadAllText(fileName);
                convertedData = convertedStrData.Split(null);
                
                Logger.TestStep("Compare initial and converted data and report the results");
                ReportResult(convertedData, expectedData);

                Logger.TestStep("End UpperCaseSelectWordTestMethod");
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }

        }

        /// <summary>
        /// TEST OBJECTIVE: To test convert to upper scenarios of TedNPad by selecting all text
        /// SETUP: TedNPad application, Testdata generator 
        /// STEPS:
        /// 1. Generate a new test file
        /// 2. Read the test data to string array
        /// 3. select all and Convert file to Upper and save it to result string array
        /// 4. Compare and log the results.
        /// EXPECTED RESULT: entire file should be converted to upper case
        public void UpperCaseSelectAllTestMethod()
        {
            try
            {
                this.Initialize();
                Logger.TestStep("Start UpperCaseSelectAllTestMethod");
                //Test Varialbes
                string fileName = string.Empty;
                string[] expectedData = { };
                string[] convertedData = { };

                Logger.TestStep("Create a test file");
                fileName = CreateTestFile(TextOption.MultiLine);
                Logger.Comment(string.Format("Test file created with name:{0}", fileName));

                Logger.TestStep("Read saved data to a string array");                
                expectedData = File.ReadAllLines(fileName);
                for (int i = 0; i <= expectedData.Length - 1; i++)
                {
                    expectedData[i] = expectedData[i].ToUpper();
                }

                Logger.TestStep("Launch TedNPadd application");
                Process TedAppProcess = ProcessHelper.LaunchApplication(testPath + tedNPadApp);
                IntPtr TedAppHandle = ProcessHelper.GetMainForegroundWindowHandle();
                Logger.Comment("TedHandle:" + TedAppHandle);
                Logger.TestStep("Open the test file");
                TEDnotepadHelper.OpenFile(fileName);


                Logger.TestStep("Select All word ");
                MenuHelper.ClickMenuItem(TedAppHandle, (int)TEDnotepadHelper.MenuItems.Edit, (int)TEDnotepadHelper.EditMenuItems.SelectAll);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                SendKeys.SendWait("^A");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                Logger.TestStep("Pass command control+shift+U to convert to upper case");
                SendKeys.SendWait("^+U");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2500));

                Logger.TestStep("Save and exit the modified file");
                SendKeys.SendWait("{F10}");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(5000));

                Logger.TestStep("Read the modified file data to a new array");
                
                convertedData = File.ReadAllLines(fileName);
                if (convertedData.Length > 0)
                {
                    convertedData[0] = convertedData[0].ToUpper();
                }
                Logger.TestStep("Compare initial and converted data and report the results");
                ReportResult(convertedData, expectedData);

                Logger.TestStep("End UpperCaseSelectAllTestMethod");
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }

        }

        /// <summary>
        /// TEST OBJECTIVE: To test convert to lower scenarios of TedNPad
        /// SETUP: TedNPad application, Testdata generator 
        /// STEPS:
        /// 1. Generate a new test file
        /// 2. Read the test data to string array
        /// 3. Convert first line of file to Lower and save it to result string array
        /// 4. Compare the log the results.
        /// EXPECTED RESULT: first line in file should be all lower case
        public void LowerCaseFirstLineTestMethod()
        {
            try
            {
                this.Initialize();
                Logger.TestStep("Start LowerCaseFirstLineTestMethod");
                //Test Varialbes
                string fileName = string.Empty;
                string[] expectedData = { };
                string[] convertedData = { };

                Logger.TestStep("Create a test file");
                fileName = CreateTestFile(TextOption.MultiLine);
                Logger.Comment(string.Format("Test file created with name:{0}", fileName));

                Logger.TestStep("Read saved data to a string array");
                expectedData = File.ReadAllLines(fileName);
                if (expectedData.Length > 0)
                {
                    expectedData[0] = expectedData[0].ToLower();
                }

                Logger.TestStep("Launch TedNPadd application");
                Process TedAppProcess = ProcessHelper.LaunchApplication(testPath + tedNPadApp);                

                Logger.TestStep("Open the test file");
                TEDnotepadHelper.OpenFile(fileName);

                Logger.TestStep("Pass command control+shift+L to convert to lower case");
                SendKeys.SendWait("^+L");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2500));

                Logger.TestStep("Save and exit the modified file");
                SendKeys.SendWait("{F10}");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(5000));

                Logger.TestStep("Read the modified file data to a new array");
                convertedData = File.ReadAllLines(fileName);

                Logger.TestStep("Compare initial and converted data and report the results");
                ReportResult(convertedData, expectedData);

                Logger.TestStep("End LowerCaseFirstLineTestMethod");
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }

        }

        /// <summary>
        /// TEST OBJECTIVE: To test Inversion case of TedNPad
        /// SETUP: TedNPad application, Testdata generator 
        /// STEPS:
        /// 1. Generate a new test file
        /// 2. Read the test data to string array
        /// 3. Invert case of first line and save it to result string array
        /// 4. Compare the log the results.
        /// EXPECTED RESULT: first line case should be inverted
        public void InvertCaseFirstLineTestMethod()
        {
            try
            {
                this.Initialize();
                Logger.TestStep("Start InvertCaseFirstLineTestMethod");
                //Test Varialbes
                string fileName = string.Empty;
                string[] expectedData = { };
                string[] convertedData = { };

                Logger.TestStep("Create a test file");
                fileName = CreateTestFile(TextOption.MultiLine);
                Logger.Comment(string.Format("Test file created with name:{0}", fileName));

                Logger.TestStep("Read saved data to a string array");
                expectedData = File.ReadAllLines(fileName);
                if (expectedData.Length > 0)
                {
                    expectedData[0] = new string(expectedData[0].Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                                          char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
                }

                Logger.TestStep("Launch TedNPadd application");
                Process TedAppProcess = ProcessHelper.LaunchApplication(testPath + tedNPadApp);                

                Logger.TestStep("Open the test file");
                TEDnotepadHelper.OpenFile(fileName);

                Logger.TestStep("Pass command control+shift+I to convert to Invert case");
                SendKeys.SendWait("^+I");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(2500));

                Logger.TestStep("Save and exit the modified file");
                SendKeys.SendWait("{F10}");
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(5000));

                Logger.TestStep("Read the modified file data to a new array");
                convertedData = File.ReadAllLines(fileName);

                Logger.TestStep("Compare initial and converted data and report the results");
                ReportResult(convertedData, expectedData);

                Logger.TestStep("End LowerCaseFirstLineTestMethod");
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }

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