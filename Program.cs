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
            Console.WriteLine("\nУправление сервисом специализация которого продажа и установка шин.\n" +
                "Ваш слоган у нас есть все шины, а если нету то мы вам заплатим!");

            while (isWork)
            {
                Console.WriteLine($"Баланс сервиса {serves.Money}");
                Console.WriteLine(" 1 - Принять нового клиента. 2 - Закрыть программу.");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        serves.ServeClient();
                        break;

                    case "2":
                        isWork = false;
                        break;
                }
            }
        }
    }

    class Serves
    {
        private Stock _stock = new Stock();
        private int _money = 1000;
        private int _fine = 100;

        public int Money => _money;

        public void ServeClient()
        {
            bool isDone = false;
            string name = "";
            int numberTire = 0;
            int discDiameter = 0;
            int width = 0;
            int profile = 0;
            int numberValidInputs = 4;
            string userInput = "";

            while (isDone == false)
            {
                _stock.ShowInfo();
                int validInput = 0;

                Console.WriteLine("\nВнесите требуемые параметры клиента.");
                Console.WriteLine("Шины какой фирмы нужны клиенту");
                name = Console.ReadLine();
                Console.WriteLine("Какaя размерность нужна клиенту");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int discDiameterInput))
                {
                    discDiameter = discDiameterInput;
                    validInput++;
                }

                Console.WriteLine("Какая ширина нужна клиенту");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int widthInput))
                {
                    width = widthInput;
                    validInput++;
                }

                Console.WriteLine("Какая высота шины нужна клиенту");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int profileInput))
                {
                    profile = profileInput;
                    validInput++;
                }

                Console.WriteLine("Какое количество шин требуется.");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int number))
                {
                    numberTire = number;
                    validInput++;
                }


                if (validInput == numberValidInputs)
                {
                    isDone = true;
                }
                else
                {
                    Console.WriteLine("Данные введены некорректно.");
                }
            }

            Tire tire = new Tire(name, width, profile, discDiameter, 0);

            int indexTire = _stock.SearchTire(tire, numberTire);

            if (indexTire > -1)
            {
                CompleteDeal(indexTire, numberTire);
            }
            else
            {
                PayFine();
            }
        }

        private void CompleteDeal(int indexTire, int numberTire)
        {
            Console.WriteLine("Есть в наличии");
            CalculateProfit(indexTire, numberTire);
            _stock.DeleteTire(indexTire, numberTire);
        }

        private void CalculateProfit(int indexTire, int numberTire)
        {
            int price = _stock.GetPrice(indexTire);
            int priceAll = price * numberTire;
            Console.WriteLine($"Клиенту к оплате {priceAll}");
            _money += priceAll;
        }

        private void PayFine()
        {
            Console.WriteLine(" Такой шины нету в наличии и сервис выплачивает штраф");
            _money -= _fine;
        }
    }

    class Stock
    {
        private List<List<Tire>> _listTire = new List<List<Tire>>();

        public Stock()
        {
            AddTire();
        }

        public int SearchTire(Tire tireClient, int numberTire)
        {
            for (int i = 0; i < _listTire.Count; i++)
            {
                List<Tire> tires = _listTire[i];

                if (tires.Count != 0)
                {
                    Tire tireTemp = tires[0];

                    if (tireClient.Name == tireTemp.Name)
                    {
                        if (tireClient.DiscDiameter == tireTemp.DiscDiameter)
                        {
                            if (tireClient.Width == tireTemp.Width)
                            {
                                if (tireClient.Profile == tireTemp.Profile)
                                {
                                    if (numberTire <= tires.Count)
                                    {
                                        int indexTire = i;
                                        return indexTire;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public int GetPrice(int indexTire)
        {
            List<Tire> tires = _listTire[indexTire];
            Tire tireTemp = tires[0];
            return tireTemp.Price;
        }

        public void ShowInfo()
        {
            for (int i = 0; i < _listTire.Count; i++)
            {
                List<Tire> tires = _listTire[i];

                if (tires.Count != 0)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        tires[j].ShowInfo();
                        Console.Write($" Количество {tires.Count}");
                    }
                }              
            }
        }

        public void DeleteTire(int indexTire, int numberTire)
        {
            List<Tire> tires = _listTire[indexTire];
            tires.RemoveRange(0, numberTire);
        }

        private void AddTire()
        {
            List<Tire> michelinW195P70R14 = new List<Tire>();
            List<Tire> bridgestoneW185P65R17 = new List<Tire>();
            List<Tire> x = new List<Tire>();
            int number = 20;           

            for (int i = 0; i < number; i++)
            {
                michelinW195P70R14.Add(new Tire("Michelin", 195, 70, 14, 25));
            }
            
            for (int i = 0; i < number; i++)
            {
                bridgestoneW185P65R17.Add(new Tire("Bridgestone", 185, 65, 65, 30));
            }

            for (int i = 0; i < number; i++)
            {
                x.Add(new Tire("x", 185, 65, 65, 30));
            }

            _listTire.Add(michelinW195P70R14);
            _listTire.Add(bridgestoneW185P65R17);
            _listTire.Add(x);
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
            Console.Write($"\nШина {Name}: Размеры {Width}/{Profile}/{DiscDiameter} : Цена за штуку {Price}");
        }
    }
}
