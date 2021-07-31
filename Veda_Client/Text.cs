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
        public static Dictionary<int, string> Dct;
        internal static int DictServCount { get; }
        public static int DictCount { get; }
        public static bool IsLangLoad { get {return IsLang;} }
        private static bool IsLang;
        static AppText()
        {
            Dct = new Dictionary<int, string>();
            IsLang = false;
            //Если указан режим #Num, то словари не подключаем
            if (Setting_Share.Lang != String.Empty)
            {
                //Load service from func and main from file
                LoadServText();
                DictServCount = Dct.Count();
                LoadText();
                DictCount = Dct.Count() - DictServCount;
            }
            else
            {
                DictServCount = 0;
                DictCount = 0;
                IsLang = true;
            }
        }
        public static string GetText(int index)
        {
            if (!Dct.TryGetValue(index, out string text))
                text = (Dct[2] + index.ToString());
            if (text == null) text = Dct[1];
            return text;
        }

        //Check Range index of text
        public static Func<int, bool> IndexInRange = (index) => !(index < Setting.KeyTextMin || index > Setting.KeyTextMax);

        //Loader text is undercostruct. Temporary value set manual
        public static void LoadServText()
        {   //Static application text
            Dct.Add(0, "#");
            Dct.Add(1, "#Undefine");
            //Dct.Add(2, "#OutRange");
            Dct.Add(4, Setting.SplStr.ToString());
            Dct.Add(5, " Number of records: ");
            Dct.Add(6, " Time upload: ");
            //Dct.Add(7, "#Num");
            Dct.Add(8, "Путь: ");
            Dct.Add(9, "Файл пуст");
            Dct.Add(10, "Файл имеет не верный формат");
            Dct.Add(999, "@Veda");
            //Time domain for development
        }
        public static void LoadText()
        {
            try
            {
                string sw = File.ReadAllText(Setting_Share.PathLang);
                if (sw == null || sw.Length == 0)
                    throw new FileWrongException(Dct[9], Setting_Share.PathLang);
                using (StringReader sr = new StringReader(sw))
                {
                    string Head = sr.ReadLine();
                    if (Head != null && Head.StartsWith(Dct[999]))
                    {
                        string input = null;
                        while ((input = sr.ReadLine()) != null)
                        {
                            if (int.TryParse(input.Substring(0, Setting.KeyTextLen), out int Key) && IndexInRange(Key) && (input[Setting.KeyTextLen] == Setting.SplStr))
                            {
                                if (!Dct.ContainsKey(Key))
                                    Dct.Add(Key, input.Substring(Setting.KeyTextLen + 1));
                            }
                        }
                    }
                    else
                        throw new FileWrongException(Dct[10], Setting_Share.PathLang);
                }
                //Confirm load lang
                if (Dct.Count()> DictServCount) IsLang = true;
            }
            catch (FileWrongException e)
                { MessageBox.Show(e.Message); } //rework
            catch (IOException e)
                { MessageBox.Show(e.Message); } //rework
            }
        public static void WriteText()
        {
            try
            {
                using (StreamWriter writer = File.CreateText(Setting_Share.PathLang))
                {
                    writer.WriteLine(Dct[999] + Dct[5] + DictCount + Dct[6] + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    foreach (KeyValuePair<int, string> item in Dct)
                    {
                        if (AppText.IndexInRange(item.Key))
                            writer.WriteLine(item.Key.ToString() + Setting.SplStr + item.Value);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
                { MessageBox.Show(e.Message); }//rework
            catch (IOException e)
                { MessageBox.Show(e.Message); }//rework
        }
    }
}
