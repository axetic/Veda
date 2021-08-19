using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Классы для конфигурации. Почему не статично, потому что это "ООП"
namespace LogText
{
    //Настройки для приложения работы с журналами
    public class SettingLog
    {
        string _appName = "AppLog";
        public string AppName { get => _appName; }                          //Имя приложения
        string _fullName = "AppLog 1.0";
        public string FullName { get => _fullName; }                        //Для отслеживание версий в файле    
        string _pathDirectory = "./log";
        public string PathDirectory { get => _pathDirectory; }              //Директория для сохранения логов
        EVerbosity _innerVerbDefault = EVerbosity.Minimal;
        public EVerbosity InnerVerbDefault { get => _innerVerbDefault; }    //Начальная детальность внутреннего логирования
        int _packageSize = 50;
        public int PackageSize { get => _packageSize; }    //Начальная детальность внутреннего логирования
    }
    //Настройки для приложения работы с текстами
    public class SettingTxt
    {
        string _appName = "AppText";
        public string AppName { get => _appName; }                          //Имя приложения
        string _fileFormat = "AppTextFormat:1.0";
        public string FileFormat { get => _fileFormat; }                    //Для отслеживание версий в файле  
        int _keyMin = 0;
        public int KeyMin { get => _keyMin; }                               //Миниальное значения для идентификатора текста
        int _keyMax = 9999;
        public int KeyMax { get => _keyMax; }                               //Миниальное значения для идентификатора текста
        string _keyFormat = "0000";
        public string KeyFormat { get => _keyFormat; }                      //Максимальное значения для идентификатора текста
        string _pathDirectory = "./lang";
        public string PathDirectory { get => _pathDirectory; }              //Директория с файлами текстов
    }
    //Список первоначальных текстов пока не подгрузились из файла
    public class InitTxts : Dictionary<int, string>
    {
        public InitTxts() : base()
        {
            this.Add(0, "#");                                               //Служебный символ для распознования текстов по умолчанию
            this.Add(1007, "Invalid inner text file");
            this.Add(1009, "The inner text file not found");
            this.Add(1010, "Error I/O to the inner text file");
            this.Add(1011, "The lang is not defined");
            this.Add(1012, "Wrong file head format");
            this.Add(1013, "Duplicate streams are specified");
            this.Add(2001, "Load inner texts");
            this.Add(2004, "Dublicate text {0} - {}:{}");
        }
    }
}