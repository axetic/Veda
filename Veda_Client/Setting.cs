using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veda_Client
{
    //Static application settings
    public class Setting
    {
        //Support settings
        public const string SplStr = " ";
        internal const int KeyTextMin = 1000;
        internal const int KeyTextMax = 9999;
    }
    // Commom user setting
    public class Setting_Share
    {
        internal static string Lang = "ru";
        public static string PathLang = @".\lang\" + Lang +".txt";
    }
    //Personal user setting
    public class Setting_Personal
    {
        //internal static string Test = "Ru";
    }
}