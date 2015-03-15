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
               this.Initialize();

               string notepadApplication = "C:\\Sahana\\TestData\\TedNPad.exe";               

               Logger.TestStep("Launch Notepad");
               Process notepadProcess = ProcessHelper.LaunchApplication(notepadApplication);

               Logger.Comment("Get multiine text");
               //Get string for editor
               string text = GetMultiLineTextForEditor();               

               Logger.TestStep("Send text to Notepad edit control");
               SendKeys.SendWait(text);


                                             
               Console.WriteLine(string.Format("Generated string Value: {0}", text));               
               Console.ReadLine();
                this.Cleanup();
            }
            catch (Exception error)
            {
                // catch unexpected exceptions and log
                Logger.Comment("Test Aborted");
                Logger.Comment(error.ToString());
            }
        }

        private static string GetMultiLineTextForEditor()
        {
            int seedValue;            
            string firstLine = GetTextString(15,out seedValue);
            string secondLine = GetTextString(20,out seedValue);
            string thirdLine = GetTextString(50, out seedValue);
            string FinalText = string.Concat(firstLine, " ",secondLine, secondLine,System.Environment.NewLine,thirdLine);            
            return FinalText;
        }


        /// <summary>
        /// Generic method to generate string of random Unicode characters
        /// </summary>
        /// <param name="seed">Seed value</param>
        /// <returns>A string of unicode characters</returns>
        private static string GetTextString(int maxCharCount,out int seed)
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