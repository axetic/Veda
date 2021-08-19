using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppText.TextDictionary;

namespace LogText
{
    public class IndexOutOfRangeException : ApplicationException
    {
        public IndexOutOfRangeException() : this(0) { }
        public IndexOutOfRangeException(int index) 
            : base(index.ToString() + txt[1] + txt[2] + "[" + TextDictionary.KeyMin +" , " + KeyMax + "]")
        {
            base.Data.Add("index", index);
        }
    }
    public class EmptyLangException : ApplicationException
    {
        public EmptyLangException() : base(txt[3]) { }
    }
    public class FileLoadLangException : ApplicationException
    {
        public FileLoadLangException() : this(mErr) { }
        public FileLoadLangException(string pathFile) 
            : base(txt[4] + txt[5] + pathFile)
        {
            base.Data.Add("pathFile", pathFile);
        }
        public FileLoadLangException(string pathFile, System.Exception inner)
            : base(txt[4] + txt[5] + pathFile, inner)
        {
            base.Data.Add("pathFile", pathFile);
        }
    }
    public class FileSaveLangException : ApplicationException
    {
        public FileSaveLangException() : this(mErr) { }
        public FileSaveLangException(string pathFile)
            : base(txt[6] + txt[5] + pathFile)
        {
            base.Data.Add("pathFile", pathFile);
        }
        public FileSaveLangException(string pathFile, System.Exception inner)
            : base(txt[6] + txt[5] + pathFile, inner)
        {
            base.Data.Add("pathFile", pathFile);
        }
    }
    public class FileFormatLangException : ApplicationException
    {
        public FileFormatLangException() : this(mErr, mErr) { }
        public FileFormatLangException(string condition, string fullFileName) 
            : base(txt[6] + condition + txt[5] + fullFileName)
        {
            base.Data.Add("fullFileName", fullFileName);
            base.Data.Add("condition", condition);
        }
    }
}