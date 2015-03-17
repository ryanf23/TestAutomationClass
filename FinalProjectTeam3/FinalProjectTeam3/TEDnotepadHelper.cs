
namespace FinalProjectTeam3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TestingMentor.TestAutomationFramework;
    using System.Windows.Forms;
    using System.IO;

    class TEDnotepadHelper
    {
        public enum OpenDialog
        {
            Textfilecombbx = 0x470,
            Openbtn = 0x1,
            Cancelbtn = 0x2,
            Filenametxt = 0x47C
        }

        public enum SaveAsDialog
        {
            Textfilecombbx = 0x0,
            Savebtn = 0x1,
            Cancelbtn = 0x2,
            Filenametxt = 0x3E9
        }

        public enum TedApplication
        {
            MainTextEntryField = 0x3FC,
            Statusbar = 0X406
        }

        public enum Findblock
        {
            Findtextbox = 0x83E,
            Nextbtn = 0x852,
            Previousbtn = 0x855,
            Cancelbtn = 0x2,
            Matchcasechbx = 0x83F,
            Wholewordchbx = 0x840,
            Escapeschbx = 0x841,
            RegExpschbx = 0x842,
            Wraparoundchbx = 0x843,
            Use2ndsearchchbx = 0x844
        }

        public enum Replaceblock
        {
            Replacetextbox = 0x845,
            Replacebtn = 0x85E,
            ReplaceAllbtn = 0x85F,
            MimicCasebtn = 0x846
        }

        public enum MenuItems
        {
            File = 0,
            Edit = 1,
            Search = 2,
            Tools = 3,
            Favourites = 4,
            Clips = 5,
            Options = 6,
            Help = 7
        }

        public enum FileMenuItems
        {
            New = 0,
            Open = 1,
            Revert = 2,
            Reopen = 3,
            Import = 4,
            Include = 5,
            Save = 7,
            SaveAs = 8,
            Export = 9,
            Exclude = 10,
            Encoding = 12,
            NewLines = 13,
            Properties = 14,
            PageSetup = 16,
            Print = 17,
            RecentFiles = 19,
            HideinTray = 21,
            NewWindow = 22,
            SavenExit = 24,
            Exit = 25,
        }

        public enum SearchMenuItems
        {
            Find = 0,
            FindNext = 1,
            FindPrevious = 2,
            FindSelected = 3,
            FindLater = 4,
            SelecttoNext = 5,
            SelecttoPrevious = 6,
            Replace = 8,
            LastTool = 9,
            Lookfor = 11,
            LookforNext = 12,
            LookforPrevious = 13,
            LookforSelected = 14,
            LookforLater = 15,
            SelecttoNext2 = 16,
            SelecttoPrevious2 = 17
        }

        public enum ToolMenuItems
        {
            LastTool=0,
            Case=1,
            Lines=2,
            Text=3,
            Reverse=4,
            Sort=5,
            Wrap=6,
            TextFilters=7
        }

        public enum CaseMenuItems
        {
            AllLowerCase=0,
            AllUpperCase=1,
            CaseInversion=2,
            WordCapitals=3,
            FirstCapital=4
        }

        public enum EditMenuItems
        {
            Undo = 0,
            Redo = 1,
            Cut = 3,
            Copy = 4,
            Paste = 5,
            Swape = 6,
            Insert = 8,
            GoTo = 10,
            SelectWord = 11,
            SelectLine = 12,
            SelectParagraph = 13,
            SelectAll = 14,
            DeleteLine = 16     
        }

        /// <summary>
        /// Method to save a file by manipulating menu items via accelerator key mnemonics
        /// </summary>
        /// <param name="filename">Filename to save</param>
        public static void SaveTedFile(string filename)
        {
            int pollCount = 0;
            int maxPollCount = 50;

            Logger.Comment("Open save dialog");
            SendKeys.SendWait("^s");

            // Sleep used to wait for dialog to open
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));

            Logger.Comment("set filename");
            SendKeys.SendWait("%n");
            SendKeys.SendWait(filename);

            // Sleep used for sendkeys delay
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));

            Logger.Comment("save as text file");
            SendKeys.SendWait("%t");
            SendKeys.SendWait("t");

            Logger.Comment("save file");
            SendKeys.SendWait("{ENTER}");
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(500));

            while (!File.Exists(filename + ".txt") && pollCount < maxPollCount)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));
                pollCount++;
            }

            if (!File.Exists(filename))
            {
                throw new ArgumentException("Test file not created.");
            }
        }

        public static void OpenFile(string filename)
        {
            Logger.Comment("Open file dialog");
            SendKeys.SendWait("^o");

            // Sleep used to wait for dialog to open
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));

            Logger.Comment("Set filename to textbox");
            SendKeys.SendWait("%n");
            SendKeys.SendWait(filename);

            // Sleep used for sendkeys
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(250));

            Logger.Comment("Open file");
            SendKeys.SendWait("%o");
        }
    }
}
