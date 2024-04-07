using System;

namespace ООАиД_ЛБ1
{
    class Program
    {
        private const int TIME_OF_SLEEP = 20;
        static void Main(string[] args)
        {
            string someMes = "Loremus muchachas. Here come a lot of churkas. We must run!!";
            Print(someMes);
            Thread.Sleep(1000);
            Delete();
            Print();
        }
        static async void Print(string input = "Hello, World!")
        {
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
                Thread.Sleep(TIME_OF_SLEEP);
            }
        }
        static void Delete()
        {
            while (Console.CursorLeft != 0)
            {
                Thread.Sleep(TIME_OF_SLEEP / 2);
                Console.Write("\b ");
                Console.CursorLeft = --Console.CursorLeft;
            }
        }
    }
}
