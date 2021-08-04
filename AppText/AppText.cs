using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppText.TextDictionary;

namespace AppText
{
    //Класс для словаря с текстами
    public partial class TextDictionary : Dictionary<int, string>
    {
        public static List<TextDictionary> appList;
        public string langAppName;
        public string appLang;
        public string pathFileLoad ="";
        public string pathFileSave = "";
        public Dictionary<string, string> property;

        //Свойства
        public new string ToString() => (appName+ "." +langAppName + ToStr(Mark.Separator) + appLang); 
        public static string AppName { get => appName; }
        public static string FileFormat { get => fileFormat; }
        public static string Keyformat { get => keyFormat; }
        public static int KeyMin { get => keyMin; }
        public static int KeyMax { get => keyMax; }
        public static DirectoryInfo PathDir { get => pathDir; set => pathDir = (value.Exists) ? value : pathDir; }

        //Конструкторы
        static TextDictionary()
        {
            appList = new List<TextDictionary>();
            txt = new TextDictionary(appName);
            appList.Add(txt);
            LoadStrReplace();
            LoadInitDict();
        }
        public TextDictionary() : this(mErr) { }
        public TextDictionary(string appName) : base()
        {
            appLang = mErr;
            if ((appName == null) || (appName.Length < 1))
                this.langAppName = mErr;
            else
            {
                this.langAppName = appName;
                appList.Add(this);
            }
            property = new Dictionary<string, string>();
        }
        
        //Статичные служебные функции
        public static bool IndexOutRange(int index) => (index < keyMin || index > keyMax); //Проверить что индекс в установленной области
        public static string ToStr(Mark m) => ((char)m).ToString(); //Для удобства перевести в строку
        public static bool IsEmpty(string s) => (s == null || s == "" || s == mErr);
        public string CreatePathFile(string lang) => pathDir.ToString() + "/" + langAppName + ToStr(Mark.Separator) + lang + ".txt"; //Сформировать строку к файлу
        public static string StrReplace(string str, bool Direction)
        {
            string s = str;
            foreach (var item in ValReplace)
            {   
                if (Direction)
                    s = s.Replace(item.Key, item.Value);
                else 
                    s = s.Replace(item.Value, item.Key);
            }
            return s;
        }

        //Индексаторы на основе строк
        public new string this[int index]
        {
            get
            {
                if (base.TryGetValue(index, out string value)) return value;
                return mErr + index.ToString(keyFormat);
            }
            set
            {
                if (base.ContainsKey(index))
                    base[index] = value;
                else 
                    this.Add(index, value);
            }
        }
        public string this[string index]
        {
            get
            {
                if (int.TryParse(index, out int key)) return this[key];
                if (index[0] == mErr[0]) return index;
                return mErr[0] + index;
            }
        }
        public string this[int index, params object[] objs]
        {
            get
            {
                string s = this[index];
                return Writer(s, objs);
            }
        }
        public string this[string index, params object[] objs]
        {
            get
            {
                string s = this[index];
                return Writer(s, objs);
            }
        }
        //Функция для индексаторов с параметрами
        public static string Writer(string s, params object[] objs)
        { 
            if (IsEmpty(s)) return mErr;
            if (s[0] == mErr[0] || objs.Length == 0 || objs.Length > 3) return s;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    switch (objs.Length)
                    {
                        case 1:
                            sw.Write(s, objs[0]);
                            break;
                        case 2:
                            sw.Write(s, objs[0], objs[1]);
                            break;
                        case 3:
                            sw.Write(s, objs[0], objs[1], objs[2]);
                            break;
                    }
                    return sw.ToString();
                }
            }
            catch (FormatException)
            { return mErr[0] + s; }
        }
        
        //Функции для работы со словарем
        public new void Add(int key, string value)
        {
            if (IndexOutRange(key)) throw new IndexOutOfRangeException(key);
            base.Add(key, value);
            
        }
        public bool TryAdd(int key, string value)
        {
            if (IndexOutRange(key)) return false;
            if (base.ContainsKey(key)) return false;
            base.Add(key, value);
            return true;
        }
        public void Replace(int key, string value)
        {
            if (IndexOutRange(key)) return ;
            if (ContainsKey(key))
                base[key] = value;
            else
                base.Add(key, value);
        }

        //Загрузка всех созданных языков
        public static void LoadAllLang(string lang)
        {
            if (IsEmpty(lang)) throw new EmptyLangException();
            foreach(var item in appList) item.LoadLang(lang);
        }
        //Загрузка языка из файла
        public void LoadLang(string lang)
        {
            if (!pathDir.Exists)
            {
                DirectoryNotFoundException e = new DirectoryNotFoundException();
                e.Data.Add("pathDir", pathDir);
                throw e;
            }
            property.Clear();
            try
            {
                if (IsEmpty(lang)) throw new EmptyLangException();
                appLang = lang;
                pathFileLoad = CreatePathFile(lang);
                int CountBefore = Count;
                //Сначала вызружаем файл в строку
                string sw = File.ReadAllText(pathFileLoad);
                if (sw == null || sw.Length == 0) throw new FileLoadLangException(pathFileLoad);
                using (StringReader sr = new StringReader(sw))
                {
                    int i = 0;
                    int key = 0;
                    string input = sr.ReadLine();
                    if ((input[0] != (char)Mark.Format) || (input.Length <= fileFormat.Length) || (input.Substring(1, fileFormat.Length) != fileFormat))
                        throw new FileFormatLangException(input.Substring(0, fileFormat.Length+1)+" : "+fileFormat, pathFileLoad);
                    while ((input = sr.ReadLine()) != null)
                    {
                        if (input.Length < 3) continue;
                        switch (input[0])
                        {
                            case (char)Mark.Property:
                                i = input.IndexOf((char)Mark.Split);
                                if (i < 2) break;
                                property.Add(input.Substring(1, i - 1).Trim(), StrReplace(input.Substring(i + 1),false));
                                break;
                            case (char)Mark.Value:
                                i = input.IndexOf((char)Mark.Split);
                                if (i < 2) break;
                                if (int.TryParse(input.Substring(1, i - 1).Trim(), out key))
                                    Replace(key, StrReplace(input.Substring(i + 1),false));
                                break;
                        }
                    }
                }
                if (Count == CountBefore) { }//Log;

            }
            catch (IOException e) { throw new FileLoadLangException(pathFileLoad, e); } 
        }

        //Сохранить существующие тексты в файл
        public void WriteLang(string lang)
        {
            if (!pathDir.Exists)
            {
                DirectoryNotFoundException e = new DirectoryNotFoundException();
                e.Data.Add("pathDir", pathDir);
                throw e;
            }
            try
            {
                string spl = ToStr(Mark.Split);
                if (IsEmpty(lang)) throw new EmptyLangException();
                pathFileSave = CreatePathFile(lang);
                using (StreamWriter writer = File.CreateText(pathFileSave))
                {
                    writer.WriteLine(ToStr(Mark.Format) + fileFormat);
                    writer.WriteLine(ToStr(Mark.Property) + "AppName" + spl + langAppName);
                    writer.WriteLine(ToStr(Mark.Property) + "Source" + spl + appName);
                    writer.WriteLine(ToStr(Mark.Property) + "Lang" + spl + lang);
                    writer.WriteLine(ToStr(Mark.Property) + "Time unload" + spl + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    
                    foreach (var item in this)
                    {
                        if (!IndexOutRange(item.Key))
                            writer.WriteLine(ToStr(Mark.Value) + item.Key.ToString(keyFormat) + spl + StrReplace(item.Value,true));
                    }
                }
            }
            catch (IOException e) { throw new FileSaveLangException(pathFileSave, e); }
        }
    }
}