using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Veda_Client.AppText;

namespace Veda_Client
{
    public class FileWrongException : ApplicationException
    {
        private string messageExc = String.Empty;
        public string pathFile = String.Empty;

        public FileWrongException() { }
        public FileWrongException(string message, string path)
        {
            messageExc = message;
            pathFile = path;
        }
        // Переопределить свойство Exception.Message .
        public override string Message => messageExc + "\n" + Dct[8] + pathFile;
    }
}
