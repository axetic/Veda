using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogText
{
    //Семейство объектов для комфорта написания. ОПП идет боком
    public static class Utility
    {
        //Заменяет специсимволы в строках для записи в файл и чтения
        public static string StrReplace(string str, bool Direction = true)
        {
            StringBuilder sb = new StringBuilder(str);
            foreach (var item in ValueToReplace)
            {
                if (Direction)
                    sb = sb.Replace(item.Key, item.Value);
                else
                    sb = sb.Replace(item.Value, item.Key);
            }
            return sb.ToString();
        }
        static Dictionary<string, string> ValueToReplace = new Dictionary<string, string>()  //Словарь подмен
        {
            {"\n", @"\n" },
            {"\t", @"\t" }
        };
    }
    //Перечислители
    public enum EVerbosity : byte           //Используется при определение подробности журналирования
    {
        Always = 0,
        Quite = 1,
        Minimal = 2,
        Normal = 3,
        Detail = 4,
    }
    public enum ESeverity : byte            //Используется для установки критичности сообщения в журнале
    {
        Error = 1,
        Warning = 2,
        Notice = 3
    }
    public enum EState : byte              //Определяет состояние сесии
    {
        Work = 0,
        Wait = 1,
        Stop = 2,
    }
    public enum EStream : byte              //Перечень реализованных потоков
    {
        ToConsole = 0,
        SimpleToFile = 1,
        AsyncPackageToFile = 2                     //Не реализован пока
    }
}