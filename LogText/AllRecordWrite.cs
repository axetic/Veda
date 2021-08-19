using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogText.LogService;

//Шаблоны приложений
namespace LogText
{
    //Шаблоны сообщений приложения текстов
    internal class NewDictionaryRecord : UserRecordWrite
    {
        internal NewDictionaryRecord(LogSession session, string appName, string lang)
            : base(session, ESeverity.Notice, EVerbosity.Minimal, "AT2001", innerTxt[2001, appName, lang], "TextDictionary", null) { }
    }
    internal class NoNewTextRecord : UserRecordWrite
    {
        internal NoNewTextRecord(LogSession session, string appName, string lang)
            : base(session, ESeverity.Warning, EVerbosity.Normal, "AT2002", innerTxt[2002, appName, lang], "TextDictionary", null) { }
    }
    internal class SaveDictionaryRecord : UserRecordWrite
    {
        internal SaveDictionaryRecord(LogSession session, string appName, string oldLang, string newLang)
            : base(session, ESeverity.Notice, EVerbosity.Minimal, "AT2003", innerTxt[2003, appName, oldLang, newLang], "TextDictionary", null) { }
    }
    internal class ReplaceTextRecord : UserRecordWrite
    {
        internal ReplaceTextRecord(LogSession session, string appName, int key, string oldValue, string newValue)
            : base(session, ESeverity.Warning, EVerbosity.Minimal, "AT2004", innerTxt[2004, appName, key, oldValue, newValue], "TextDictionary", null) { }
    }

    //Шаблоны сообщений приложения журналирования
    internal class StartLogService : InnerRecordWrite
    {
        internal StartLogService(string lang, EStream[] streams)
            : base(ESeverity.Notice, EVerbosity.Minimal, "AТ2005", innerTxt[2005, lang], "LogService",
                  streams.ToDictionary<EStream, object, object>((p) => ((byte)p), p => p)) { }   //Не понимаю почему он принимает только статиче
    }
    internal class NewServiceSessionRecord : InnerRecordWrite
    {
        internal NewServiceSessionRecord(string appName)
            : base(ESeverity.Notice, EVerbosity.Normal, "AL2006", innerTxt[2006, appName], "LogService") { }
    }
    internal class NewSessionRecord : InnerRecordWrite
    {
        internal NewSessionRecord(string appName)
            : base(ESeverity.Notice, EVerbosity.Normal, "AL2007", innerTxt[2007, appName], "LogService") { }
    }
    internal class LogCommitRecord : InnerRecordWrite
    {
        internal LogCommitRecord()
            : base(ESeverity.Notice, EVerbosity.Detail, "AL2008", innerTxt[2008], "LogService") { }
    }
    internal class StreamToggleAllRecord : InnerRecordWrite
    {
        internal StreamToggleAllRecord(bool isActive)
            : base(ESeverity.Notice, EVerbosity.Detail, "AL2009", innerTxt[2009, isActive], "LogService") { }
    }
    internal class StreamToggleRecord : InnerRecordWrite
    {
        internal StreamToggleRecord(bool isActive, EStream[] streams)
            : base(ESeverity.Notice, EVerbosity.Detail, "AL2010", innerTxt[2010, isActive], "LogService", streams.ToDictionary<EStream, object, object>(p => (byte)p, p => p)) { }
    }
    internal class ChangeInnerVerbosityRecord : InnerRecordWrite
    {
        internal ChangeInnerVerbosityRecord(EVerbosity verbosity)
            : base(ESeverity.Notice, EVerbosity.Normal, "AL2011", innerTxt[2011, verbosity], "LogService") { }
    }
    internal class DisposeLogRecord : InnerRecordWrite
    {
        internal DisposeLogRecord()
            : base(ESeverity.Notice, EVerbosity.Minimal, "AL2012", innerTxt[2012], "LogService") { }
    }
    internal class ChangeStatusSessionRecord : InnerRecordWrite
    {
        internal ChangeStatusSessionRecord(string appName, EState status)
            : base(ESeverity.Notice, EVerbosity.Detail, "AL2013", innerTxt[2013, appName, status], "LogSession") { }
    }
    internal class NothingWriteRecord : InnerRecordWrite
    {
        internal NothingWriteRecord(string appName)
            : base(ESeverity.Notice, EVerbosity.Minimal, "AL2014", innerTxt[2014, appName], "LogSession") { }
    }
}