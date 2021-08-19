using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogText;
using static LogText.LogService;


namespace Test
{
    public partial class Test
    {
        static void Main()
        {
            //Должна быть директория lang c файлом AppLog_ru.txt и Text_ru.txt

            Console.Write("*****Start Test****\n");
            //Создание приложения для записи в лог. Выбирается внутренний язык и потоки которые предполагается использовать
            LogService app = new LogService("ru", EStream.AsyncPackageToFile, EStream.ToConsole);
            //Формирует сессию для обособленния записей. Определяется имя и степень детализации журналирования
            LogSession ses = app.GetSession("Test", EVerbosity.Detail);
            //В приложение загружаются словарь. Определяется сессия для внутреннего журналирования словаря
            TextDictionary dct = new TextDictionary("Test", "ru", ses);
            //Формируется запись для журналирования
            LogRecord log = new LogRecord(ESeverity.Notice, EVerbosity.Normal, "##0000", "Моя первая запись", "Test");
            //Пишем в журнал
            ses.Write(log);
            //Но можно написть через общую сессию
            new Write(ESeverity.Notice, EVerbosity.Normal, "##0000", "Моя вторая запись", "Test", ("Hi", 100), ("Next", 102));
            //можно даже вот так
            new Write("Моя третья запись");
            //Ошибки живут в отдельно своей сесии
            try
            {
                throw new ValueIsNullException("Main");
            }
            catch (Exception) { }
            //Для внутреннего логирования есть внутрення сессия
            //Можно прерывать работу сессий
            ses.Pause();
            //Можно отдельно останавливать и возобновлять работу для потоков
            app.WriterToggle(false, EStream.AsyncPackageToFile);

            
            Console.Write("\n*****Done****");
            Console.ReadLine();
        }
    }
}
