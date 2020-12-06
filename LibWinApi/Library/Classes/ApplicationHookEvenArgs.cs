using System;
using LibWinApi.Library.Enums;


namespace LibWinApi.Library.Classes
{
    public class ApplicationHookEvenArgs:EventArgs
    {
        public WindowInfoObject WindowInfo { get; set; }
        public EnumApplicationEvents Events { get; set; }
    }
}