using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppText.Text;

namespace AppText
{
    public class Setting_AT
    {
        //Language file format settings
        internal const string splStr = " ";
        internal const int keyTextMin = 1000;
        internal const int keyTextMax = 9999;
        internal const int keyTextLen = 4;

        //Value set manual
        internal static void ServiceText()
        {   //Static application text
            dct.Add(0, "#");
            dct.Add(1, "#Undefine");
            dct.Add(2, "The language file incorrect: \nPath: ");
            dct.Add(3, "The file does not contain any chars");
            dct.Add(4, "Invalid header format");
            dct.Add(5, "Not any record was loaded");
            dct.Add(6, "Number of records:");
            dct.Add(7, "Time upload:");
            //Time domain for development

            dictServCount = dct.Count;
            dictCount = CalcDictCount();
        }
    }
    public class FileWrongException : ApplicationException
    {
        public string messageExc = "";
        public string pathFile = "";

        public FileWrongException() { }
        public FileWrongException(string message, string path)
        {
            messageExc = message;
            pathFile = path;
        }
        // Переопределить свойство Exception.Message .
        public override string Message => dct[2] + pathFile + "\n" + messageExc;
    }
}