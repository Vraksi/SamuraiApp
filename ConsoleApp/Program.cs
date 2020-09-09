using SamuraiAppDomain;
using System;
using SamuraiApp.Data;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext context = new SamuraiContext();

        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            //GetSamurais("before add");            
            //AddSamurai();
            GetSamurais("after add");
            InsertMultipleSamurais();
            Console.Write("press key");
            Console.ReadLine();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "kodawkopdaw" };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = context.Samurais.ToList();
            Console.WriteLine($"{text}: samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Peter" };
            var samurai2 = new Samurai { Name = "Shimura" }; 
            var samurai3 = new Samurai { Name = "Sakai" };
            var samurai4 = new Samurai { Name = "Sasuke" };
            context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4 );
            context.SaveChanges();
        }
    }
}
