using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogText
{

    interface ITextAction
    {
        void Execute(string lang);
    }
    //Загрузка тектсов из файла
    public class LoadTxt : ITextAction
    {
        TextDictionary _dictionary;
        public LoadTxt(TextDictionary dictionary)
        {
            _dictionary = dictionary;
            if (!Directory.Exists(_dictionary.Setting.PathDirectory)) Directory.CreateDirectory(_dictionary.Setting.PathDirectory);
        }
        public void Execute(string lang)
        {
            _dictionary.Clear();
            string pathFile = CreatePathFile(lang);
            if (!File.Exists(pathFile)) throw new FileLangNotFoundException(this, pathFile);
            try
            {
                //Сначала вызружаем файл в строку
                string sw = File.ReadAllText(pathFile);
                if (sw == null || sw.Length == 0) throw new FileLoadLangException(this, pathFile);

                using (StringReader sr = new StringReader(sw))
                {
                    int i = 0;
                    int key = 0;
                    string input = sr.ReadLine();
                    if ((input[0] != '?')                                                   //Проверка заголовка, для отсечения не таких файлов
                        || (input.Length <= _dictionary.Setting.FileFormat.Length)
                        || (input.Substring(1, _dictionary.Setting.FileFormat.Length) != _dictionary.Setting.FileFormat))
                        throw new FileFormatLangException(this, input.Substring(0, _dictionary.Setting.FileFormat.Length + 1) + " : " + _dictionary.Setting.FileFormat, pathFile);
                    while ((input = sr.ReadLine()) != null)
                    {
                        if (input.Length < 3) continue;
                        switch (input[0])
                        {
                            case '@':                                                       //Загрузка свойств
                                i = input.IndexOf(':');
                                if (i < 2) break;
                                _dictionary.Property.Add(input.Substring(1, i - 1).Trim(), Utility.StrReplace(input.Substring(i + 1), false));
                                break;
                            case (char)39:                                                  //Загрузка текстов. Код 39 соответствует "'"
                                i = input.IndexOf(':');
                                if (i < 2) break;
                                if (int.TryParse(input.Substring(1, i - 1).Trim(), out key))
                                    _dictionary.ReplaceAdd(key, Utility.StrReplace(input.Substring(i + 1), false));
                                break;
                        }
                    }
                }
            }
            catch (IOException e) { throw new FileLoadLangException(this, pathFile, e); }
        }
        //Формирует строку для пути файла с текстами
        string CreatePathFile(string lang) => (_dictionary.Setting.PathDirectory + "/" + _dictionary.AppName + "_" + lang + ".txt");
    }
    //Загрузка тектсов из файла
    public class SaveTxt : ITextAction
    {
        TextDictionary _dictionary;
        public SaveTxt(TextDictionary dictionary)
        {
            _dictionary = dictionary;
            if (!Directory.Exists(_dictionary.Setting.PathDirectory)) Directory.CreateDirectory(_dictionary.Setting.PathDirectory);
        }
        public void Execute(string lang)
        {
            string pathFile = CreatePathFile(lang);
            try
            {
                using (StreamWriter writer = File.CreateText(pathFile))
                {
                    writer.WriteLine('?' + _dictionary.Setting.FileFormat);
                    writer.WriteLine('@' + "AppName" + ':' + _dictionary.AppName);
                    writer.WriteLine('@' + "Source" + ':' + _dictionary.Setting.AppName);
                    writer.WriteLine('@' + "Lang" + ':' + lang);
                    writer.WriteLine('@' + "Time unload" + ':' + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    foreach (var item in _dictionary)
                    {
                        writer.WriteLine("'" + item.Key.ToString(_dictionary.Setting.KeyFormat) + ':' + Utility.StrReplace(item.Value, true));
                    }
                }
            }
            catch (UnauthorizedAccessException e) { throw new FileSaveLangException(this, pathFile, e); }
            catch (IOException e) { throw new FileSaveLangException(this, pathFile, e); }
        }
        //Формирует строку для пути файла с текстами
        string CreatePathFile(string lang) => (_dictionary.Setting.PathDirectory + "/" + _dictionary.AppName + "_" + lang + ".txt");
    }
    //Загрузка тектсов из настроек
    public class LoadInitTxt : ITextAction
    {
        TextDictionary _dictionary;
        public LoadInitTxt(TextDictionary dictionary)
        {
            _dictionary = dictionary;
        }
        public void Execute(string lang)
        {
            _dictionary.Clear();
            IDictionary<int, string> dct = new InitTxts();
            foreach (var item in dct)
            {
                _dictionary.Add(item.Key, item.Value);
            }
        }
    }
    //Класс заглушка
    public class NothingTxt : ITextAction
    {
        TextDictionary _dictionary;
        public NothingTxt(TextDictionary dictionary)
        {
            _dictionary = dictionary;
        }
        public void Execute(string lang)
        {
            new NothingWriteRecord(_dictionary.AppName);
        }
    }
}
