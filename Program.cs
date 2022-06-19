using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkAutoServes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Serves serves = new Serves();
            bool isWork = true;
            Console.WriteLine("Управление сервисом специализация которого продажа и установка шин.\n" +
                "Ваш слоган у нас есть все шины, а если нету то мы вам заплатим!");

            while (isWork)
            {
                Console.WriteLine($"Баланс сервиса {serves.Money}");
                Console.WriteLine(" 1 - Принять нового клиента. 2 - Показать все шины. 3 - Закрыть программу.");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        serves.CreateClient();
                        break;

                    case "2":
                        serves.ShowAllTire();
                        break;

                    case "3":
                        isWork = false;
                        break;
                }
            }
        }
    }

    class Serves
    {
        private int _money = 1000;
        private Dictionary<Tire, int> _tires = new Dictionary<Tire, int>();
        private int _fine = 100;
        private string _userInput = "";
        private int _discDiameter = 0;
        private int _width = 0;
        private int _profile = 0;
        private string _name = "";

        public int Money => _money;

        public Serves()
        {
            AddTire();
        }

        public void CreateClient()
        {
            bool isDone = false;
            
            while (isDone == false)
            {
                int validInput = 0;
                Console.WriteLine("Внесите требуемые параметры клиента.");
                Console.WriteLine("Шины какой фирмы нужны клиенту");
                _name = Console.ReadLine();
                Console.WriteLine("Какaя размерность нужна клиенту");
                _userInput = Console.ReadLine();

                if (int.TryParse(_userInput, out int discDiameter))
                {
                    _discDiameter = discDiameter;
                    validInput++;
                }

                Console.WriteLine("Какая ширина нужна клиенту");
                _userInput= Console.ReadLine();

                if (int.TryParse(_userInput, out int width))
                {
                    _width = width;
                    validInput++;
                }

                Console.WriteLine("Какая высота шины нужна клиенту");
                _userInput = Console.ReadLine();

                if (int.TryParse(_userInput, out int profile))
                {
                    _profile = profile;
                    validInput++;
                }

                if (validInput == 3)
                {
                    isDone = true;
                }
                else
                {
                    Console.WriteLine("Данные введены некорректно.");
                }
            } 
            
            Tire tire = new Tire(_name, _width, _profile, _discDiameter, 0);
            PerformWork(tire);
        }      
        
        public void ShowAllTire()
        {
            Console.WriteLine("Спиок шин на складе");

            foreach (var item in _tires)
            {
                Tire tireTemp = item.Key;
                tireTemp.ShowInfo();
                Console.WriteLine($": количество {item.Value}");
            }
        }

        private void PerformWork(Tire tire)
        {
            bool isWork = false;
            int numberTireServes = 0;
            string name = "";
            int width = 0;
            int profile = 0;
            int discDiameter = 0;
            int price = 0;
            int profit = 0;

            foreach (var item in _tires)
            {
                Tire tireTemp = item.Key;
                if (tireTemp.Name == tire.Name)
                {
                    if (tireTemp.Width == tire.Width)
                    {
                        if (tireTemp.Profile == tire.Profile)
                        {
                            if (tireTemp.DiscDiameter == tire.DiscDiameter)
                            {
                                numberTireServes = _tires[tireTemp];

                                if (numberTireServes > 0)
                                {
                                    tireTemp.ShowInfo();
                                    Console.WriteLine(" Есть в наличии.");
                                    Console.WriteLine(" Сколько шин требуется клиенту");
                                    int numberTireClient = Convert.ToInt32(Console.ReadLine());

                                    if (numberTireClient <= numberTireServes)
                                    {
                                        profit = numberTireClient * tireTemp.Price;
                                        isWork = true;
                                        numberTireServes -= numberTireClient;
                                        name = tireTemp.Name;
                                        width = tireTemp.Width;
                                        profile = tireTemp.Profile;
                                        discDiameter = tireTemp.DiscDiameter;
                                        price = tireTemp.Price;
                                        Console.WriteLine(" Клиент доволен вы получили деньги за работу!");
                                        _tires.Remove(tireTemp);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine(" Нету требуемого количества шин");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (isWork == false)
            {
                Console.WriteLine(" Такой шины нету в наличии и сервиси выплачивает штраф");
                _money -= _fine;
            }
            else
            {
                _money += profit;
                _tires.Add(new Tire(name, width, profile, discDiameter, price), numberTireServes);
            }
        }

        private void AddTire()
        {
            _tires.Add(new Tire("Michelin", 195, 70, 14, 25), 40);
            _tires.Add(new Tire("Bridgestone", 175, 65, 14, 25), 2);
            _tires.Add(new Tire("Bridgestone", 185, 65, 17, 30), 10);
            _tires.Add(new Tire("Goodyear", 185, 65, 17, 50), 4);
            _tires.Add(new Tire("Continental", 195, 70, 14, 20), 0);
        }
    }

    class Tire
    {
        public string Name { get; private set; }
        public int DiscDiameter { get; private set; }
        public int Width { get; private set; }
        public int Profile { get; private set; }
        public int Price { get; private set; }

        public Tire(string name, int width, int profile, int discDiameter, int price)
        {
            Name = name;
            Width = width;
            Profile = profile;
            DiscDiameter = discDiameter;
            Price = price;
        }

        public void ShowInfo()
        {
            Console.Write($"Шина {Name}: Размеры {Width}/{Profile}/{DiscDiameter} : Цена за штуку {Price}");
        }
    }
}
