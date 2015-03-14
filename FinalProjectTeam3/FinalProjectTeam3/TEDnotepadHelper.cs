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
        public enum SaveDialog
        {
            Textfilecombbx = 0x0,
            Savebtn = 0x1,
            Cancelbtn = 0x2,
            Filenametxt = 0x3E9
        }
        public enum TedApplication
        {
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
    }
}
