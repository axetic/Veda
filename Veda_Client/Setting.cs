using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Veda_Client
{
    //Static application settings. Set by developer
    public partial class App : Application
    {
        public App()
        {
            this.Properties.Add("App_Name", "Veda_Client");
        }
    }


    public class Setting
    {
        //Support settings
        public static string appName = "Veda-Client";
        public static string pathLang = @".\lang\";
    }

    // Commom user setting. Can be change by interface
    public class Setting_Common
    {
        //Setting lang
        internal static string lang = "ru";
    }
    
    //Personal user setting
    public class Setting_Personal
    {
        //Almost empty ;)
    }

}