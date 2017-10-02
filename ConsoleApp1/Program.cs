using Services;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var detector = new ArbitrageDetector();
            detector.Start();
            Console.WriteLine("CryptoBot started... (Press ENTER to stop)");
            Console.ReadLine();
            detector.Stop();
        }
    }
}
