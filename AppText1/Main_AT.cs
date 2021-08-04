using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static AppText.Setting_AT;

namespace AppText
{
    //Static class for application text
    public class Text
    {
        //Collection with application text
        public static Dictionary<int, string> dct;
        public static string appName;
        public static string path;
        public static string pathFile;
        public static int dictServCount;
        public static int dictCount;
        public static bool IsLangLoad { get { return isLang; } }
        private static bool isLang;

        public static string GetText(int index)
        {
            if (!dct.TryGetValue(index, out string text))
                text = (dct[0] + index.ToString());
            if (text == null) text = dct[1];
            return text;
        }

        //Load lang

        public static void InitializeText(String appName, String pathLang)
        {
            //Load service text from setting
            dct = new Dictionary<int, string>();
            Text.appName = "@"+appName;
            path = pathLang;
            ServiceText();
            isLang = false;
        }

        public static void LoadLang(string lang)
        {
            isLang = false;
            pathFile = path + lang + ".txt";
            //Если не указан язык, то словари чистим
            if (lang == "")
            {
                dct.Clear();
                dictServCount = 0;
                dictCount = 0;
                isLang = true;
            }
            else
            {
                //Загружаем из файла
                try
                {
                    int CountBefore = dct.Count;
                    int l = splStr.Length;
                    string sw = File.ReadAllText(pathFile);
                    if (sw == null || sw.Length == 0)
                        throw new FileWrongException(dct[3], pathFile);
                    using (StringReader sr = new StringReader(sw))
                    {
                        string Head = sr.ReadLine();
                        if (Head != null && Head.StartsWith(appName))
                        {
                            string input = null;
                            while ((input = sr.ReadLine()) != null)
                            {
                                if (int.TryParse(input.Substring(0, keyTextLen), out int key) && IndexInRange(key) && (input[keyTextLen] == splStr[0]))
                                {
                                    if (!dct.ContainsKey(key))
                                        dct.Add(key, input.Substring(keyTextLen + l));
                                }
                            }
                        }
                        else
                            throw new FileWrongException(dct[4], pathFile);
                    }
                    //Confirm load lang
                    //if (dct.Count == CountBefore)
                    //{throw new FileWrongException(dct[5], pathFile); } Сделать другую ошибку
                    dictCount = CalcDictCount();
                    isLang = true;
                }
                catch (FileWrongException)
                    { throw; }//Заглушка
                catch (IOException e)
                    { throw new FileWrongException(e.Message, pathFile);} //rework later
            }
        }
        public static void WriteLang(string lang)
        {
            pathFile = path + lang + ".txt";
            try
            {
                using (StreamWriter writer = File.CreateText(pathFile))
                {
                    writer.WriteLine(appName + splStr + dct[6] + splStr + dictCount + splStr + dct[7] + splStr + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    foreach (KeyValuePair<int, string> item in dct)
                    {
                        if (IndexInRange(item.Key))
                            writer.WriteLine(item.Key.ToString() + splStr + item.Value);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
                { throw new FileWrongException(e.Message, path); } //rework later
            catch (IOException e)
                { throw new FileWrongException(e.Message, pathFile); } //rework later
        }
        //Check Range index of text
        public static bool IndexInRange(int index) => !(index < keyTextMin || index > keyTextMax);
        //rework Потом переписать нормально на свой класс диктионари
        public static int CalcDictCount()
        {
            int sum = 0;
            foreach (var item in dct)
                if (IndexInRange(item.Key)) sum++;
            return sum;
        }

        
    }
}

