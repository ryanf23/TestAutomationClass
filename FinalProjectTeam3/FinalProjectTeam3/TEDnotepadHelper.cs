using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinalProjectTeam3
{
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
    }
}
