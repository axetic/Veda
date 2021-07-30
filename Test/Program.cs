using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Veda_Client;
using static Veda_Client.AppText;

namespace Test
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("*****Read****\n");
            string sf = File.ReadAllText(Setting_Share.PathLang);
            Console.WriteLine(sf);
            Console.ReadLine();
        }
    }
}
