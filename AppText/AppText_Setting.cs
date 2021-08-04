using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppText.TextDictionary;

namespace AppText
{
    //Класс для словаря с текстами
    public partial class TextDictionary : Dictionary<int, string>
    {
        public const string appName = "AppText";
        //Настройки формата файла языка
        internal const string fileFormat = "AppTextFormat.1.0";
        internal const int keyMin = 1;
        internal const int keyMax = 9999;
        internal const string keyFormat = "0000";
        internal const string mErr = "#";
        internal static DirectoryInfo pathDir = new DirectoryInfo("./lang");
        public static TextDictionary txt; //inner text
        public static Dictionary<string, string> ValReplace;

        //Загрузка первоначальных текстов
        static void LoadInitDict()
        {
            txt.TryAdd(1, " out of range.");
            txt.TryAdd(2, "\nThe index must be in the range : ");
            txt.TryAdd(3, "Lang can't be empty.");
            txt.TryAdd(4, "The file is unreadable or empty.");
            txt.TryAdd(5, "\nFile : ");
            txt.TryAdd(6, "Wrong lang file format. \nCondition : ");
            txt.TryAdd(7, "The file is unwritable or blocked.");
            txt.TryAdd(10, "Time upload");
        }
        static void LoadStrReplace()
        {
            ValReplace = new Dictionary<string, string>()
            {
                {"\n", @"\n" },
                {"\t", @"\t" }
            };
        }
    }
    //Перечислитель, содержит кода служебных символов. Что-бы попробывать.
    public enum Mark : byte
    {
        Format = 63,   // = ?
        Property = 64, // = @
        Value = 39,    // = '
        Split = 58,    // = :
        Separator = 95 // = _
    }
}