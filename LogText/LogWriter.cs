using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Класс объектов для формирование шаблонов записей в лог
namespace LogText
{
    //Абстрактный класс писателя в журнал
    public abstract class ALogWriter
    {
        public ALogWriter(LogSession logSession, ILogRecord logRecord)
        {
            logSession.Write(logRecord);
        }
    }
    //Журналирование для пользовательских сессий
    internal class UserRecordWrite : ALogWriter
    {
        public UserRecordWrite(LogSession session, ESeverity severity, EVerbosity verbosity, string code, string message, string source, System.Collections.IDictionary data)
                   : base(session, new LogRecord(severity, verbosity, code, message, source, data)) { }
        public UserRecordWrite(LogSession session, ESeverity severity, EVerbosity verbosity, string code, string message, string source)
           : base(session, new LogRecord(severity, verbosity, code, message, source)) { }
    }
    //Журналирование сообщений об ошибках
    internal class ErrorRecordWrite : ALogWriter
    {
        public ErrorRecordWrite(Exception e, string code)
                   : base(LogService.errorSession, new LogRecord(ESeverity.Error, EVerbosity.Quite, code, e.Message, e.Source, e.Data)) { }
    }
    //Журналирование внутренних сообщений
    internal class InnerRecordWrite : ALogWriter
    {
        public InnerRecordWrite(ESeverity severity, EVerbosity verbosity, string code, string message, string source, System.Collections.IDictionary data)
                   : base(LogService.innerSession, new LogRecord(severity, verbosity, code, message, source, data)) { }
        public InnerRecordWrite(ESeverity severity, EVerbosity verbosity, string code, string message, string source)
                   : base(LogService.innerSession, new LogRecord(severity, verbosity, code, message, source)) { }
    }
    //Конструкторы для быстрой записи в журнал через общую сессию
    public class Write : ALogWriter
    {
        public Write(ESeverity severity, EVerbosity verbosity, string code, string message, string source, params (object, object)[] data)
           : base(LogService.commonSession, new LogRecord(severity, verbosity, code, message, source,
               data.ToDictionary<(object, object), object, object>(p => p.Item1, p => p.Item2))) { }
        public Write(ESeverity severity, EVerbosity verbosity, string code, string message, string source, params object[] data)
           : base(LogService.commonSession, new LogRecord(severity, verbosity, code, message, source,
               data.ToDictionary<object, object, object>(p => p, p => ""))) { }
        public Write(ESeverity severity, EVerbosity verbosity, string code, string message, string source, System.Collections.IDictionary data)
           : base(LogService.commonSession, new LogRecord(severity, verbosity, code, message, source, data)) { }
        public Write(ESeverity severity, EVerbosity verbosity, string code, string message, string source)
            : base(LogService.commonSession, new LogRecord(severity, verbosity, code, message, source)) { }
        public Write(string message)
                   : base(LogService.commonSession, new LogSimpleRecord(message)) { }
        public Write(string message, params object[] data)
                   : base(LogService.commonSession, new LogSimpleRecord(message, data)) { }
    }
}
