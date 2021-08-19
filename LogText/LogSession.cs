using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Семейство объктов для формирования сессий
namespace LogText
{
    //Сессия пользователя
    public sealed class LogSession
    {
        internal LogService _appLogText;
        string _appName;
        public string AppName { get => _appName; }                                      //Приложение для которого созданна сессия
        internal EVerbosity _verbosity;
        public EVerbosity Verbosity { get => _verbosity; set => _verbosity = value; }   //Детальность журналирования
        internal EState _status;
        public EState Status { get => _status; }                                       //Статус активности сесии
        internal LogCall _logCall;
        ILogBehavier logStatus;
        //Конструктор
        public LogSession(LogService appLogText, string appName, EVerbosity logVerbosity, LogCall logCall)
            : this(appLogText, appName, logVerbosity, logCall, new StrategyUserSession()) { }
        public LogSession(LogService appLogText, string appName, EVerbosity logVerbosity, LogCall logCall, ILogBehavier logBehavier)
        {
            _appLogText = appLogText;
            _appName = appName;
            _verbosity = logVerbosity;
            _status = EState.Work;
            _logCall = logCall;
            logStatus = logBehavier;
        }
        //Функция записи в журнал
        public void Write(ILogRecord record)
        {
            if ((_status == EState.Work) && ((byte)record.Verbosity <= (byte)_verbosity))
                _logCall.Invoke(record.ToString(this));
            else
                if (_status == EState.Stop) throw new StopSessionException(this, _appName);
        }
        //Функци управления состоянием через соответсвующий класс поведения
        public void Pause()
        {
            logStatus.Pause.Set(this);
            new ChangeStatusSessionRecord(_appName, _status);
        }
        public void Continue()
        {
            logStatus.Continue.Set(this);
            new ChangeStatusSessionRecord(_appName, _status);
        }
        public void Close()
        {
            logStatus.Close.Set(this);
            new ChangeStatusSessionRecord(_appName, _status);
        }
        public bool IsActive { get => (_status == EState.Work); }                          //Индикатор активности
        public override string ToString() => ("Session@" + _appName);
    }

    //Класс для управления статегии поведения статусов  
    public interface ILogBehavier
    {
        ILogSesAction Pause { get; }
        ILogSesAction Continue { get; }
        ILogSesAction Close { get; }
    }
    // Реализвация стратегии поведения статусов
    public abstract class AStrategySession : ILogBehavier
    {
        protected ILogSesAction _pause;
        public ILogSesAction Pause { get => _pause; }
        protected ILogSesAction _continue;
        public ILogSesAction Continue { get => _continue; }
        protected ILogSesAction _close;
        public ILogSesAction Close { get => _close; }
    }
    public class StrategyUserSession : AStrategySession, ILogBehavier
    {
        public StrategyUserSession()
        {
            _pause = new Pause();
            _continue = new Continue();
            _close = new Close();
        }
    }
    //Реализация внутренней сессии
    public class StrategyInnerSession : AStrategySession, ILogBehavier
    {
        public StrategyInnerSession()
        {
            _pause = new Pause();
            _continue = new Continue();
            _close = new Except();
        }
    }
    //Реализация сессии для ошибок
    public class StrategyErrorSession : AStrategySession, ILogBehavier
    {
        public StrategyErrorSession()
        {
            _pause = new Nothing();
            _continue = new Nothing();
            _close = new Except();
        }
    }
    //Реализация общей сессии
    public class StrategyCommonSession : AStrategySession, ILogBehavier
    {
        public StrategyCommonSession()
        {
            _pause = new Nothing();
            _continue = new Nothing();
            _close = new Except();
        }
    }

    //Интерфейс для управления статусом сессий   
    public interface ILogSesAction
    {
        void Set(LogSession logSession); 
    }
    
    //Классы поведений для функци управления состоянием
    public class Pause : ILogSesAction
    {
        public void Set(LogSession logSession)
        {
            logSession._status = EState.Wait;
        }
    }
    public class Continue : ILogSesAction
    {
        public void Set(LogSession logSession)
        {
            if (logSession._status == EState.Wait) logSession._status = EState.Work;
        }
    }
    public class Close : ILogSesAction
    {
        public void Set(LogSession logSession)
        {
            logSession._appLogText.Commit();
            logSession._status = EState.Stop;
        }
    }
    public class Nothing : ILogSesAction
    {
        public void Set(LogSession logSession) { }
    }
    public class Except : ILogSesAction
    {
        public void Set(LogSession logSession)
        {
            throw new CnahgeStatusSessionException(this, logSession.AppName);
        }
    }
}