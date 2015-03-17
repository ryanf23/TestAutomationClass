// <copyright file="TestDriver.cs" company="[YourName]">
// Copyright © 2014. All rights reserved.
// </copyright>

namespace FinalProjectTeam3
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// This is our poor test suite driver
    /// </summary>
    class Program
    {
        /// <summary>
        /// Executes all defined test methods
        /// </summary>
        static void Main()
        {
            // Initializes the test suite
            TestSuiteSahana sahanaTest = new TestSuiteSahana();

            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Call the test methods
            sahanaTest.UpperCaseFirstLineTestMethod();
            sahanaTest.LowerCaseFirstLineTestMethod();
            sahanaTest.InvertCaseFirstLineTestMethod();
            timer.Stop();
            TimeSpan ts = timer.Elapsed;

            // Format TimeSpan value. 
            string elapsedTime = string.Format(
                CultureInfo.InvariantCulture,
                "{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours,
                ts.Minutes,
                ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("Total Test Suite Time: {0}", elapsedTime);
        }
    }
}
