// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome8222();
            welcome0227();
            Console.ReadKey();
        }
        static partial void welcome0227();
        private static void welcome8222()
        {
            Console.WriteLine("Enter your Name: ");
            string name;
            name = Console.ReadLine();
            Console.WriteLine("{0},welcome to my first console application",name);
        }

    }
}
