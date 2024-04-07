using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ООАиД_ЛБ1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Print("Hello, World!");
            Console.ReadLine();
        }
        static async void Print(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
                Thread.Sleep(60);
            }
        }
        static void Beep(char input)
        {
            int chas = Convert.ToInt32(input);
            Console.Beep(input, chas);
        }
    }
}
