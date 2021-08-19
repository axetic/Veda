using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
//Реализовать пакетную запись в файл

//Семесйтво объектов потоков для различных вариантов журналирования
namespace LogText
{
    // Делегат для функций выполняющих запись в лог, которые будут вызываться из сессий
    public delegate void LogCall(string record);
    //Базовый класс с реализацией прерывателя
    public abstract class ALogStream
    {
        public abstract EStream Stream { get; }                     //Идентификатор потока
        bool _isActive;
        public bool IsActive { get => _isActive;                    //Дает возможность в целом отключить или включить поток
                               set => _isActive = value; }
        LogCall _logCall;
        public LogCall LogCall { get => _logCall; }                 //Возвращает ссылка на функуцию выполняющую запись в лог
        public ALogStream()
        {
            _logCall = new LogCall(InterruptReccord);               //Указывает на прерыватель
            _isActive = true;
        }
        internal void InterruptReccord(string record)                 //Прерватель
        {
            if (_isActive) Record(record);
        }
        protected abstract void Record(string record);                //Сбственна функция записи в лог
    }
    //Интерфейсы определяющие особенности работы LogStream    
    public interface IHasCommit                                      //Говорит что для данного потока релевантна функция Commit
    {
        void Commit();
    }
    interface IHasCancel
    {
        void Cancel();
        void Continue();
    }
    //Реализация вывода на консоль
    class LogToConsole : ALogStream
    {
        const EStream _stream = EStream.ToConsole;
        public override EStream Stream { get => _stream; }
        protected override void Record(string rec)                    //Функция записи
        {
            Console.WriteLine(Utility.StrReplace(rec));
        }
    }
    //Реализация класса для потоков, которые пишут в файл
    public abstract class ALogToFile : ALogStream
    {
        protected SettingLog _setting;
        protected FileInfo file;
        //Конструктор с проверками директории и файла
        public ALogToFile(string suffix) :base()
        {
            _setting = new SettingLog();
            if (!Directory.Exists(_setting.PathDirectory)) Directory.CreateDirectory(_setting.PathDirectory);
            file = new FileInfo(_setting.PathDirectory + "/" + _setting.AppName + suffix  + "_" + DateTime.Now.ToString("yyyy.MM.dd") + ".log");
            if (!file.Exists)
            {
                try
                {
                    using (StreamWriter writer = file.CreateText())  //Создание файла и запись зоголовка
                    {
                        writer.WriteLine("?" + _setting.FullName + suffix);
                    }
                }
                catch (IOException e) { throw new FileSaveLogException(this, file.FullName, e); }
            }
        }
    }
    //Реализация простой записи в файл
    class LogSimpleToFile : ALogToFile
    {
        const EStream _stream = EStream.SimpleToFile;
        public override EStream Stream { get => _stream; }
        //Конструктор
        public LogSimpleToFile() : base("") { }
        protected override void Record(string rec)                    //Функция записи
        {
            try
            {
                using (StreamWriter writer = file.AppendText())
                {
                    writer.WriteLine(rec);
                }
            }
            catch (IOException e) { throw new FileSaveLogException(this, file.FullName, e); }
        }
    }
    //Реализует пакетную запись в файл в отдельном потоке
    class LogAsyncPackageToFile : ALogToFile , IHasCommit, IHasCancel
    {
        const EStream _stream = EStream.AsyncPackageToFile;
        public override EStream Stream { get => _stream; }
        int sizePackage;
        Queue<LogPackage> packages;                             //Очередь пакетов
        LogPackage CurrentPackage;                              //Текущий пакет для записи
        EState stateWrite;                                      //Состояние потока
        //Конструктор
        public LogAsyncPackageToFile () : base("_Async")
        {
            sizePackage = _setting.PackageSize;
            packages = new Queue<LogPackage>();
            CurrentPackage = new LogPackage(sizePackage);
            stateWrite = EState.Wait;
        }
        protected override void Record(string rec)
        {
            if (!CurrentPackage.Add(rec))
            {
                packages.Enqueue(CurrentPackage);
                _ = WriteQueueAsync();                          
                CurrentPackage = new LogPackage(sizePackage);
            }
        }
        //Метод для ассинхронной вызова записи в файл по пакетам (сделанно без прерывания рабочих задач)
        public async Task WriteQueueAsync()
        {
            if (stateWrite == EState.Wait)
            {
                stateWrite = EState.Work;                                                
                while ((packages.Count > 0) && (stateWrite == EState.Work))     //Работает пока пакеты не заончаться
                {
                    await Task.Run(() => WritePackage(packages.Dequeue()));
                }
                if (stateWrite == EState.Work) stateWrite = EState.Wait;
            }
        }
        //Класс с пакетом для записи
        void WritePackage(LogPackage package)                    
        {
            try
            {
                using (StreamWriter writer = file.AppendText())
                {
                    for (int i = 0; i < package.index; i++)
                    {
                        writer.WriteLine(package.records[i]);
                    }
                }
            }
            catch (IOException e) { throw new FileSaveLogException(this, file.FullName, e); }
        }
        //Функции управления
        public void Cancel()
        {
            stateWrite = EState.Stop;                                                     
        }
        public void Continue()
        {
            stateWrite = EState.Wait;
        }
        public void Commit()
        {
            packages.Enqueue(CurrentPackage);
            _ = WriteQueueAsync();
            CurrentPackage = new LogPackage(sizePackage);
        }
    }
    //Класс пакета для хранение строкового массива
    class LogPackage
    {
        internal string[] records;
        internal int index;
        int _sizePackage;
        internal LogPackage(int sizePackage)
        {
            index = 0;
            _sizePackage = sizePackage;
            records = new string[sizePackage];
        }
        internal bool Add(string rec)
        {
            if (index == _sizePackage) return false;
            records[index] = rec;
            index++;
            return true;
        }
    }
}
