using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogText;

namespace Test
{
    public partial class Test
    {
        static void Test_AppLog()
        {
            LogService app = new LogService("ru");
            var ses = app.GetSession("Test", EVerbosity.Detail, EStream.ToConsole);
            int a = 5;

            new Write("Привет мир");
            new Write("Привет мир номер {0}", a);
            new Write(ESeverity.Warning, EVerbosity.Normal, "##0000", "Запись в формате", "Test");
            new Write(ESeverity.Warning, EVerbosity.Normal, "##0000", "Запись в формате", "Test", new Dictionary<string, object> { { "Dict", a } });
            new Write(ESeverity.Warning, EVerbosity.Normal, "##0000", "Запись в формате", "Test", ("Tuple ", a));
            new Write(ESeverity.Warning, EVerbosity.Normal, "##0000", "Запись в формате", "Test", a);

            ses.Write(new LogSimpleRecord("Только консоль1"));
            ses.Pause();
            ses.Write(new LogSimpleRecord("Не должно быть записи"));
            app.ChangeInnerLogVerbosity(EVerbosity.Detail);
            ses.Continue();
            ses.Write(new LogSimpleRecord("Должна быть запись"));

            app.WriterToggle(false, EStream.ToConsole);
            new Write("Только в файл");
            app.WriterToggle(true);
            new Write("Должна быть везде");

            ses.Close();
            //ses.Write(new LogSimpleRecord("Должна быть ошибка"));
        }
    }
}
