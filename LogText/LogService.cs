using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//возможно можно уйти от статичности внутренней сессии

//Святая святых, управление журналирование
namespace LogText
{
    //Класс сервиса приложения собственной персоной
    public sealed class LogService : IDisposable
    {
        //Блок привязок
        public static TextDictionary innerTxt;                              //Словарь для внутренних текстов, общий  Временно открыт как Public
        public static LogSession innerSession;                              //Сессия для внутреннего логирования. Временно открыт как Public
        public static LogSession commonSession;                             //Сессия для сообщений без отедльных сессий
        public static LogSession errorSession;                              //Сессия для сообщений при ошибках
        //Переменные
        SettingLog _setting;                                                //Настройки для работы сервиса
        List<LogSession> _listSession;                                      //Хранит ссылки на созданные сервисом сессии
        Dictionary<EStream, ALogStream> _dctStreams;                        //Список используемых классом потоков. Определяются только при инициализации
        LogCall _logCalls;                                                  //Список функций вызовов для записей во все потоки
        //Конструкторы
        public LogService(string lang) : this(lang, EStream.ToConsole, EStream.SimpleToFile) { }      //Базовый запуск
        public LogService(string lang, params EStream[] streams)                            //Можно самому выбрать рабочие потоки
        {
            _setting = new SettingLog();
            _listSession = new List<LogSession>();
            _dctStreams = new Dictionary<EStream, ALogStream>();
            
            ALogStream _logStreams;
            for (int i = 0; i < streams.Length; i++)
            {
                switch (streams[i])                                         //Инициализация потоков
                {
                    case EStream.ToConsole:
                        _logStreams = new LogToConsole();
                        _logCalls += _logStreams.LogCall;
                        break;
                    case EStream.SimpleToFile:
                        _logStreams = new LogSimpleToFile();
                        _logCalls += _logStreams.LogCall;
                        break;
                    case EStream.AsyncPackageToFile:
                        _logStreams = new LogAsyncPackageToFile();             //Когда реализую, тогда сниму комменты
                        _logCalls += _logStreams.LogCall;
                        break;
                    default:
                        throw new StreamNotRealizedException(this, streams[i].ToString());
                }
                if (_dctStreams.ContainsKey(streams[i])) throw new DuplicateStreamException(this);
                _dctStreams.Add(streams[i], _logStreams);
            }
            //Инициализация внутренних сессий и словаря
            innerSession = innerSession ?? new LogSession(this, _setting.AppName, _setting.InnerVerbDefault, _logCalls, new StrategyInnerSession());
            errorSession = errorSession ?? new LogSession(this, "Error", EVerbosity.Detail, _logCalls, new StrategyErrorSession());
            innerTxt = innerTxt ?? new TextDictionary(innerSession);
            if (innerTxt[0] == "#") innerTxt = new TextDictionary(_setting.AppName, lang, innerSession);
            new NewServiceSessionRecord(_setting.AppName);
            new NewServiceSessionRecord("Error");
            //Инициализация общих сессий для пользовательских записей и ошибок
            commonSession = commonSession ?? new LogSession(this, "Сommon", EVerbosity.Detail, _logCalls, new StrategyCommonSession());
            new NewServiceSessionRecord("Сommon");
            new StartLogService(lang, streams);
        }
        //Предоставить сессию по запросу
        public LogSession GetSession(string appName, EVerbosity logVerbosity)
        {
            if ((appName == null) || (appName.Length < 1)) throw new AppNameException(this, appName);
            LogSession NewSession = new LogSession(this, appName, logVerbosity, _logCalls);
            _listSession.Add(NewSession);
            new NewSessionRecord(appName);
            return NewSession;
        }
        //Предоставить сессию по запросу только для выбранных потоков
        public LogSession GetSession(string appName, EVerbosity logVerbosity, params EStream[] streams)
        {
            if ((appName == null) || (appName.Length < 1)) throw new AppNameException(this, appName);
            LogCall logCalls = null;
            bool miss;
            for (int i = 0; i < streams.Length; i++)
            {
                miss = true;
                foreach (var s in _dctStreams)
                {
                    if (s.Key == streams[i])
                    {
                        logCalls += s.Value.LogCall;
                        miss = false;
                    }
                }
                if (miss) throw new  StreamNotRealizedException(this, streams[i].ToString());
            }
            LogSession NewSession = new LogSession(this, appName, logVerbosity, logCalls);
            _listSession.Add(NewSession);
            return NewSession;
        }
        //Фиксирует веременныйе данные
        public void Commit()
        {
            foreach (var s in _dctStreams)
            {
                if (s.Value is IHasCommit I) I.Commit();
            }
            new LogCommitRecord();
        }
        //Включает-выключает запись в лог для всех LogStream
        public void WriterToggle(bool isActive)
        {
            foreach (var s in _dctStreams)
            {
                s.Value.IsActive = isActive;
                if (s.Value is IHasCancel I) 
                {
                    if (isActive) I.Continue(); else I.Cancel(); 
                }
            }
            new StreamToggleAllRecord(isActive);
        }
        //Включает-выключает запись в лог для выбранного LogStream
        public void WriterToggle(bool isActive, params EStream[] streams)
        {
            bool miss;
            for (int i = 0; i < streams.Length; i++)
            {
                miss = true;
                foreach (var s in _dctStreams)
                {
                    if (s.Key == streams[i])
                    {
                        s.Value.IsActive = isActive;
                        if (s.Value is IHasCancel I)
                        {
                            if (isActive) I.Continue(); else I.Cancel();
                        }
                        miss = false;
                    }
                }
                if (miss) throw new StreamNotRealizedException(this, streams[i].ToString());
            }
            new StreamToggleRecord(isActive, streams);
        }
        //Предоставить список сессий
        public string[] GetListSession()
        {
            RemoveObsoleteSession();
            string[] list = new string[_listSession.Count];
            int i = 0;
            foreach (var item in _listSession)
            {
                list[i++] = item.ToString();
            }
            return list;
        }
        //Удаляет остановленные сессии
        void RemoveObsoleteSession()
        {
            foreach (LogSession item in _listSession)
                if (item._status == EState.Stop) _listSession.Remove(item);
        }
        public override string ToString()
        {
            return (_setting.FullName);
        }
        //Функция изменения детальности журналирования внутреннего лога
        public void ChangeInnerLogVerbosity(EVerbosity verbosity)
        {
            innerSession.Verbosity = verbosity;
            new ChangeInnerVerbosityRecord(verbosity);
        }
        //Почистить за собой класс
        public void Dispose()
        {
            new DisposeLogRecord();
            this.WriterToggle(false);                           //Выключить потоки
            foreach (var item in _listSession) item.Close();    //Закрыть сесии
            innerSession = null;                                //Освободить статические объекты
            commonSession = null;
            errorSession = null;
            innerTxt = null;
            GC.SuppressFinalize(this);
        }
    }
}
