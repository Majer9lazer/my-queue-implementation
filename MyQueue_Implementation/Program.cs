using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyQueue_Implementation.Core.Generator;
using MyQueue_Implementation.Core.MyCollections;
using MyQueue_Implementation.Core.MyGenericCollections;
using MyQueue_Implementation.Modeling.MyEntities;


namespace MyQueue_Implementation
{
    class Program
    {
        static void Main(string[] args)
        {
        tryStart:
            try
            {
            begin:
                Console.Clear();
                Console.WriteLine("Введите n (количество номеров которые вы хотите увидеть первыми)");
                bool b = int.TryParse(Console.ReadLine(), out int n);

                if (!b)
                {
                    ShowError();
                    goto begin;
                }
                else
                {
                whatToUse:
                    Console.WriteLine("1 - Использовать Generic Queue");
                    Console.WriteLine("2 - Использовать обычный Queue (завязанный только на Person)");
                    b = int.TryParse(Console.ReadLine(), out int choise);
                    if (!b)
                    {
                        ShowError();
                        goto whatToUse;
                    }
                    Console.WriteLine("Начинайте вводить номера! Чтобы закончить ввод нажмите escape");
                    if (choise == 1)
                    {

                        MyGenericQueue<Person> genericQueue = new MyGenericQueue<Person>();

                        int counter = 1;
                        do
                        {
                            Console.Write("+");
                            string phoneNumber = Console.ReadLine();
                            genericQueue.Enqueue(new Person(counter++, "+" + phoneNumber));
                            Console.WriteLine("Write enter to continue or ESC to exit");
                        }
                        while (Console.ReadKey().Key != ConsoleKey.Escape);

                        Person[] firstNPersons = genericQueue.GetNElements(n);

                        foreach (Person nPerson in firstNPersons)
                        {
                            Console.WriteLine("Unique Id = {0} , Id = {1} , PhoneNumber = {2}",
                                nPerson.UniqueId, nPerson.Id, nPerson.PhoneNumber);
                        }


                    }
                    else if (choise == 2)
                    {
                        MyQueue myQueue = new MyQueue();

                        int counter = 1;
                        do
                        {
                            Console.Write("+");
                            string phoneNumber = Console.ReadLine();
                            myQueue.Enqueue(new Person(counter++, "+" + phoneNumber));
                            Console.WriteLine("Write enter to continue or ESC to exit");
                        }
                        while (Console.ReadKey().Key != ConsoleKey.Escape);

                        Person[] firstNPersons = myQueue.GetNElements(n);

                        foreach (Person nPerson in firstNPersons)
                        {
                            Console.WriteLine("[{0}]PhoneNumber = {1},Unique Id = {2}.",
                                nPerson.Id, nPerson.PhoneNumber, nPerson.UniqueId);
                        }
                    }
                    else
                    {
                        ShowError();
                        goto begin;
                    }

                    Console.WriteLine("Всё прошло успешно");
                    Console.ReadLine();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(3500);
                goto tryStart;
            }

        }

        private static string InputError()
        {
            return "Вы что-то ввели не корректно попробуйте еще раз";
        }

        private static void ShowError()
        {
            Console.WriteLine(InputError());
            Thread.Sleep(1000);
        }
    }
}
