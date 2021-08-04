using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using AppText;

namespace Test
{
    public partial class Test
    {
        public static void Test_AppText()
        {
            TextDictionary txt = new TextDictionary("Veda-Client");
            TextDictionary.LoadAllLang("ru");
            Show(TextDictionary.txt);
            txt.Add(1011, "Program text: {0} is parameter");
            txt.Add(1012, "Program text: {0}, {1}");
            Show(txt);
            Console.WriteLine();
            txt.WriteLang("ru2");
            txt.LoadLang("ru2");
            Show(txt);
            txt[1010] = "New Text";
            txt[1015] = "New Record";
            Show(txt);
            Console.WriteLine(txt["1022ff"]);
            Console.WriteLine(txt["1010"]);
            Console.WriteLine(txt[1011, 105.6]);
            Console.WriteLine(txt[1011, txt]);
            Console.WriteLine(txt["1011", txt.ToString()]);
        }
        static void Show(TextDictionary txt)
        {
            Console.WriteLine("langAppName : {0}", txt.langAppName);
            Console.WriteLine("pathFileLoad : {0}", txt.pathFileLoad);
            Console.WriteLine("count : {0}", txt.Count);
            Console.WriteLine("\nProperty");
            foreach (var t in txt.property) Console.WriteLine("{0} : {1}", t.Key, t.Value);
            Console.WriteLine("\nValue");
            foreach (var t in txt) Console.WriteLine("{0} : {1}", t.Key.ToString("0000"), t.Value);
            Console.WriteLine();
        }

    }
}
