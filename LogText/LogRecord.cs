using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Семейство объектов для формирования объекта записи
namespace LogText
{
    //Интерфейс для объекта записи
    public interface ILogRecord
    {
        EVerbosity Verbosity { get;  }
        string ToString(LogSession session);
    }
    //Базовый вариант запрос для записи в журнал
    public class LogRecord : ILogRecord
    {
        internal DateTime time;
        public ESeverity severity;
        public EVerbosity _verbosity;
        public EVerbosity Verbosity { get => _verbosity; set => _verbosity = value; }    //Подробность журналирования
        public string code;
        public string message;
        public string source;
        public bool isData;
        public System.Collections.IDictionary data;
        //Конструкторы
        public LogRecord() 
            : this(ESeverity.Notice, EVerbosity.Normal, "##0000", "()", "()") { }
        public LogRecord(ESeverity severity, EVerbosity verbosity, string code, string message, string source, System.Collections.IDictionary data)
            : this(severity, verbosity, code, message, source) 
        {
            if (data != null) {isData = true; this.data = data;}
        }
        public LogRecord(ESeverity severity, EVerbosity verbosity, string code, string message, string source)
        {
            this.time = DateTime.Now;
            this.severity = severity;
            this._verbosity = verbosity;
            this.code = code;
            this.message = message;
            this.source = source;
            isData = false;
            this.data = new Dictionary<string, object>();
        }
        //Функция построения строки для записи в журнали
        public string ToString(LogSession session)
        {
            StringBuilder s = new StringBuilder("'");
            s.Append(time.ToString("HH:mm:ss.fff"));
            s.Append("|");
            s.Append(session.AppName);
            s.Append("|");
            s.Append(severity.ToString());
            s.Append("|");
            s.Append(code);
            s.Append("|");
            s.Append(message);
            s.Append("|");
            s.Append(source);
            if (isData)
            {
                s.Append("|");
                foreach (var item in data.Keys)
                {
                    s.Append(item); ;
                    s.Append(":");
                    s.Append(data[item].ToString());
                    s.Append(";");
                }
            }
            return s.ToString();
        }
    }
    //Второй вариант формата строки для записи в журнал - упрощенный
    public class LogSimpleRecord : ILogRecord
    {
        internal DateTime time;
        public EVerbosity _verbosity;
        public EVerbosity Verbosity { get => _verbosity; set => _verbosity = value; }
        public string _message;
        public object[] _data;
        //Конструкторы
        public LogSimpleRecord() : this("()") { }
        public LogSimpleRecord(string message) : this(message, null) { }
        public LogSimpleRecord(string message, params object[] data)
        {
            time = DateTime.Now;
            _verbosity = EVerbosity.Always;
            _message = message;
            _data = data;
        }
        //Функция построения строки для записи
        public string ToString(LogSession session)
        {
            StringBuilder s = new StringBuilder("'");
            s.Append(time.ToString("HH:mm:ss.fff"));
            s.Append("|");
            s.Append(session.AppName);
            s.Append("|");
            s.Append(_message);
            return s.ToString();
        }
    }
}