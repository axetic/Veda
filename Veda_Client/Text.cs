using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Text;
using System.Threading.Tasks;


namespace Veda_Client
{
    //Static class for application text
    public class AppText
    {
        //Collection with application text
        public static Dictionary<int,string> Dct;
        public static int DictCount {get;}
        internal static int DictServCount;
        static AppText()
        {
            Dct = new Dictionary<int, string>();
            LoadText();
            DictCount = Dct.Count()- DictServCount;
        }
        public static string GetText(int index)
        {
            if (!Dct.TryGetValue(index, out string text))
            {
                text = IndexInRange(index) ? (Dct[2] + index.ToString()): Dct[2];
            }
            if (text == null) text = Dct[1];
            return text;
        }

        public static bool IndexInRange(int index)
        {
            return !(index < Setting.KeyTextMin || index > Setting.KeyTextMax);
        }

        //Loader text is undercostruct. Temporary value set manual
        static void LoadText()
        {   //Static application text
            Dct.Add(0, "#");
            Dct.Add(1, "#Undefine");
            Dct.Add(2, "#OutRange");
            Dct.Add(4, Setting.SplStr);
            Dct.Add(5, " Number of records: ");
            Dct.Add(6, " Time upload: ");
            Dct.Add(999, "@Veda");
            DictServCount = Dct.Count();
            
            //Time domain for development
            Dct.Add(1000, "Test");
            Dct.Add(1001, "Hello");
        }
        public static void WriteText()
        {
            try
            {
                using (StreamWriter writer = File.CreateText(Setting_Share.PathLang))
                {
                    writer.WriteLine(Dct[999] + Dct[5] + DictCount + Dct[6] + DateTime.Now.ToString("dd.mm.yyyy HH:MM"));
                    foreach (KeyValuePair<int, string> item in Dct)
                    {
                        if (AppText.IndexInRange(item.Key))
                            writer.WriteLine(item.Key.ToString() + Setting.SplStr + item.Value);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                //rework
                MessageBox.Show(e.Message);
            }
            catch (IOException e)
            {
                //rework
                MessageBox.Show(e.Message);
            }
        }
    }
}
