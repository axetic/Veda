/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Veda_Client;
using static Veda_Client.AppText;

namespace Test
{
    class CarShow
    {
        static void CarMain()
        {

            Car[] cars = new Car[4];
            cars[0] = new Car();
            cars[1] = new Car("Kia", 120, 160);
            cars[2] = new Car("Mers", 120, 180);
            cars[3] = new Car("Infiniti", 120, 200);

            Car.CarEngineHandler d = new Car.CarEngineHandler(Check);
            cars[1].Exploded += d;
            cars[1].AboutToBlow += d;
            cars[3].Exploded += d;
            cars[3].AboutToBlow += d;

            for (int j = 0; j < 11; j++)
            {
                foreach (Car i in cars)
                { i.Accelerate(10); }
                Console.WriteLine("");
            }
            Car.CarEngineHandler vv = new Car.CarEngineHandler(Check);
            Console.WriteLine("\nDone");
            Console.ReadLine();
        }

        static void Check(string info)
        {
            Console.WriteLine("GetEvent: {0}", info);
        }
    }
    //public delegate void CarEngineHandler(string Message);

    public class Car
    {
        public delegate void CarEngineHandler(string Message);
        public event CarEngineHandler Exploded;
        public event CarEngineHandler AboutToBlow;

        // Данные состояния,
        public int CurrentSpeed = 0;
        public int MaxSpeed = 0;
        public string PetName = "Unknown";
        private bool carIsDead = true;

        // Конструкторы класса,
        public Car() { }
        public Car(string name, int currSp, int maxSp)
        {
            CurrentSpeed = currSp;
            MaxSpeed = maxSp;
            PetName = name;
            carIsDead = false;
        }
        public void Accelerate(int delta)
        {
            // Если этот автомобиль сломан, то отправить сообщение об этом,
            if (carIsDead)
            {
                Exploded?.Invoke("Sorry, this car is dead...");
            }
            else
            {
                CurrentSpeed += delta;
                // Автомобиль почти сломан?
                if (11 > (MaxSpeed - CurrentSpeed) && AboutToBlow != null)
                    AboutToBlow("Careful buddy! Gonna blow!");

                if (CurrentSpeed >= MaxSpeed)
                    carIsDead = true;
                else
                    this.Show();
            }
        }
        public void Show() => Console.WriteLine("{0} drive {1}, (max {2})", PetName, CurrentSpeed, MaxSpeed);
    }
}
*/