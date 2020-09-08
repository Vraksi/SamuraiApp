using SamuraiAppDomain;
using System;
using SamuraiApp.Data;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext context = new SamuraiContext();

        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            GetSamurais("before add");            
            AddSamurai();
            GetSamurais("after add");
            Console.Write("press key");
            Console.ReadLine();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "kodawkopdaw" };
            context.Samurais.Add(samurai);
            //Savechanges ændrer laver en masse commits med sql kode og hvis af dem fejler, laver den en rollback på alt andet også.
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
    }
}
