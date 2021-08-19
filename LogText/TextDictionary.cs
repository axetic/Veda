using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogText.LogService;
//Вынести в отдельный класс загрузку и сохранение

//Семейство объктов для работы со словарями
namespace LogText
{
    //Класс для словаря с текстами
    public class TextDictionary : Dictionary<int, string>
    {
        string _appName;
        public string AppName { get => _appName; }                          //Название приложения для которого формируется словарь
        string _lang;                                                       
        public string Lang { get => _lang; }                                //Идентификатор языка. Должен встречаться в название файла
        SettingTxt _setting;
        public SettingTxt Setting { get => _setting; }                      //Поле для хранение настроек
        Dictionary<string, string> _property;                               //Список свойств словаря. Можно в файле указывать свои дополнительные
        public Dictionary<string, string> Property { get => _property; set => _property = value; }
        LogSession _session;
        Action action;                                                      //Предоставляет возможность загружать и выгружать словарь

        //Конструкторы
        internal TextDictionary(LogSession session) : base()                //Конструктор для служебных сессий - убить потом
        {
            _setting = new SettingTxt();
            _appName = "AppText";
            _lang = "inner";
            _session = session;
            action = new Action(new BehavierServiceText(), this);
            action.Load.Execute(_lang);
        }
        public TextDictionary(string appName, string lang, LogSession session) : base() //Основной конструктор
        {
            _setting = new SettingTxt();
            if ((appName == null) || (appName == "")) throw new AppNameException(this, appName);
            _appName = appName;
            if (lang == "") throw new EmptyLangException(this);
            _lang = lang;
            _session = session;
            _property = new Dictionary<string, string>();
            action = new Action(new BehavierStandartText(), this);
            action.Load.Execute(lang);
            new NewDictionaryRecord(_session, _appName, lang);
        }
        //Индексаторы на основе строк
        public new string this[int index]                                   //Получение элемента по числовому индексу
        {
            get => base.TryGetValue(index, out string value) ? value : ("#" + index.ToString(_setting.KeyFormat));
            set => base[index] = value;
        }
        public string this[string index]                                    //Получение элемента по числовому индексу в виде строки
        {
            get => int.TryParse(index, out int key) ? this[key] : ("#" + index);
        }
        public string this[int index, params object[] objs]                 //С параметрами для подмены в тексте (кто сказал что так нельзя)
        {
            get => String.Format(this[index], objs);
            set => this[index] = value;
        }
        public string this[string index, params object[] objs]              //Аналогично, только через строковое представление индекса
        {
            get => String.Format(this[index], objs);
        }
        //Функции для работы со словарем. Если програмно захочется добавлять свои значения
        public new void Add(int key, string value)                     //Если есть значение то выдает ошибку
        {
            if (key < _setting.KeyMin || key > _setting.KeyMax) throw new IndexOutOfRangeException(this, key, _setting.KeyMin, _setting.KeyMax);
            if (value == null) throw new ValueIsNullException(this);
            base.Add(key, value);
            
        }
        public bool TryAdd(int key, string value)                       //Если есть значение ничего не меняет
        {
            if (base.ContainsKey(key)) return false;
            this.Add(key, value);
            return true;
        }
        public void ReplaceAdd(int key, string value)                   //Если есть значение переписывает новым значением
        {
            if (ContainsKey(key))
            {
                new ReplaceTextRecord(_session, _appName, key, this[key], value);
                this[key] = value;
            }else
                this.Add(key, value);
        }
        //Сохранить существующие тексты в файл
        public void SaveTxt(string lang)                                //Можно указать другой идентификатор языка, чтобы не перезатереть существующий файл
        {
            action.Save.Execute(lang);
            new SaveDictionaryRecord(_session, _appName, _lang, lang);
        }
        //Переопределение описание объекта
        public new string ToString() => (_setting.AppName + "." + _appName + "_" + _lang);
    }
    //Получение разных вариантов загрузки и записи в зависиомсти от типа словаря
    class Action
    {
        ITextAction _load;
        public ITextAction Load { get => _load;  }
        ITextAction _save;
        public ITextAction Save { get => _save;  }

        public Action(IBehavierText behavier, TextDictionary dictionary)
        {
            _load = behavier.GetLoad(dictionary);
            _save = behavier.GetWrite(dictionary);
        }
    }
    interface IBehavierText
    {
        ITextAction GetLoad(TextDictionary dictionary);
        ITextAction GetWrite(TextDictionary dictionary);
    }
    class BehavierStandartText : IBehavierText
    {
        public ITextAction GetLoad(TextDictionary dictionary)
        {
            return new LoadTxt(dictionary);
        }
        public ITextAction GetWrite(TextDictionary dictionary)
        {
            return new SaveTxt(dictionary);
        }
    }
    class BehavierServiceText : IBehavierText
    {
        public ITextAction GetLoad(TextDictionary dictionary)
        {
            return new LoadInitTxt(dictionary);
        }
        public ITextAction GetWrite(TextDictionary dictionary)
        {
            return new NothingTxt(dictionary);
        }
    }
}