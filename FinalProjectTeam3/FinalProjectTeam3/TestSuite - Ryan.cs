// <copyright file="TestSuite.cs" company="[YourName]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace FinalProjectTeam3
{
    using System;
    using System.IO;
    using System.Text;
    using TestingMentor.TestAutomationFramework;

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
        /// EXPECTED RESULT: The file has been saved
        /// </summary>
        public void SaveTedNotepadFile()
        {
            try
            {
                this.Initialize();

                //Open the Ted application 

                //Create some text to enter

                //Save the file
                

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