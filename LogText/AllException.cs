using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogText.LogService;

//Ошибки приложения
namespace LogText
{
    public class DuplicateStreamException : ApplicationException
    {
        public DuplicateStreamException() : this("()") { }
        public DuplicateStreamException(object source)
            : base("Duplicate streams are specified")
        {
            Source = source.ToString();
        }
    }
    public class CnahgeStatusSessionException : ApplicationException
    {
        public CnahgeStatusSessionException() : this("()", "()") { }
        public CnahgeStatusSessionException(object source, string name)
            : base(innerTxt[1001] + name)
        {
            Source = source.ToString();
            Data.Add("sessionName", name);
            new ErrorRecordWrite(this, "LT1001");
        }
    }
    public class StopSessionException : ApplicationException
    {
        public StopSessionException() : this("()", "()") { }
        public StopSessionException(object source, string appName)
            : base(innerTxt[1002, appName])
        {
            Source = source.ToString();
            new ErrorRecordWrite(this, "LT1002");
        }
    }
    public class FileSaveLogException : ApplicationException
    {
        public FileSaveLogException() : this("()", "()") { }
        public FileSaveLogException(object source, string pathFile)
            : base(innerTxt[1003, pathFile])
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1003");
        }
        public FileSaveLogException(object source, string pathFile, System.Exception inner)
            : base(innerTxt[1003, pathFile], inner)
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1003");

        }
    }
    public class FileSaveLangException : ApplicationException
    {
        public FileSaveLangException() : this("()", "()") { }
        public FileSaveLangException(object source, string pathFile)
            : base(innerTxt[1004, pathFile])
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1004");
        }
        public FileSaveLangException(object source, string pathFile, System.Exception inner)
            : base(innerTxt[1004, pathFile], inner)
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1004");
            new ErrorRecordWrite(inner, "##0000");
        }
    }
    public class StreamNotRealizedException : ApplicationException
    {
        public StreamNotRealizedException() : this("()", "()") { }
        public StreamNotRealizedException(object source, string name)
            : base(innerTxt[1005, name])
        {
            Source = source.ToString();
            Data.Add("sessionName", name);
            new ErrorRecordWrite(this, "LT1005");
        }
    }
    public class AppNameException : ApplicationException
    {
        public AppNameException() : this("()", "()") { }
        public AppNameException(object source, string name)
            : base(innerTxt[1006, name])
        {
            Source = source.ToString();
            Data.Add("sessionName", name);
            new ErrorRecordWrite(this, "LT1006");
        }
    }
    public class IndexOutOfRangeException : ApplicationException
    {
        public IndexOutOfRangeException() : this("()", 0, 0, 0) { }
        public IndexOutOfRangeException(object source, int index, int KeyMin, int KeyMax)
            : base(innerTxt[1007, index, KeyMin, KeyMax])
        {
            Source = source.ToString();
            Data.Add("index", index);
            new ErrorRecordWrite(this, "LT1007");
        }
    }
    public class ValueIsNullException : ApplicationException
    {
        public ValueIsNullException() : this("()") { }
        public ValueIsNullException(object source) : base(innerTxt[1008])
        {
            Source = source.ToString();
            new ErrorRecordWrite(this, "LT1008");
        }
    }
    public class FileLangNotFoundException : ApplicationException
    {
        public FileLangNotFoundException() : this("()", "()") { }
        public FileLangNotFoundException(object source, string pathFile)
            : base(innerTxt[1009, pathFile])
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1009");
        }
    }
    public class FileLoadLangException : ApplicationException
    {
        public FileLoadLangException() : this("()", "()") { }
        public FileLoadLangException (object source, string pathFile)
            : base(innerTxt[1010, pathFile])
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1010");
        }
        public FileLoadLangException(object source, string pathFile, System.Exception inner)
            : base(innerTxt[1010, pathFile], inner)
        {
            Source = source.ToString();
            Data.Add("pathFile", pathFile);
            new ErrorRecordWrite(this, "LT1010");
            new ErrorRecordWrite(inner, "##0000");
        }
    }
    public class EmptyLangException : ApplicationException
    {
        public EmptyLangException() : this("()") { }
        public EmptyLangException(object source) : base(innerTxt[1011]) 
        {
            Source = source.ToString();
            new ErrorRecordWrite(this, "LT1011");
        }
    }
    public class FileFormatLangException : ApplicationException
    {
        public FileFormatLangException() : this("()", "()", "()") { }
        public FileFormatLangException(object source, string condition, string fullFileName)
            : base(innerTxt[1012, condition, fullFileName])
        {
            Source = source.ToString();
            Data.Add("fullFileName", fullFileName);
            Data.Add("condition", condition);
            new ErrorRecordWrite(this, "LT1012");
        }
    }
}
