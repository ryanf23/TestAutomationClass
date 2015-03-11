// <copyright file="TestSuite.cs" company="[YourName]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace YourName.ProjectName
{
    using System;
    using System.IO;
    using System.Text;
    using TestingMentor.TestAutomationFramework;

    /// <summary>
    /// Test suite for [project name] 
    /// </summary>
    public class TestSuite : TestBase
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

                // TODO: add code here for test

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