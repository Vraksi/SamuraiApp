﻿using SamuraiAppDomain;
using System;
using SamuraiApp.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Transactions;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            //GetSamurais("before add");            
            //AddSamurai();
            //GetSamurais("after add");
            //InsertMultipleSamurais();
            //QueryFilters();
            //RetrieveAndUpdateSamurais();
            //RetrieveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //RetrieveAndDeleteSamurai();
            //GetSamurais("after add");
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(1);
            //AddQuoteToExistingSamuraiNotTracked_Easy(1);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();
            //ExplicitLoadQuotes();
            //LazyLoadQuotes();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();
            //JoinBattleAndSamurai();
            EnlistSamuraiIntoBattle();

            Console.Write("press key");
            Console.ReadLine();
        }

        private static void EnlistSamuraiIntoBattle()
        {
            var battle = _context.Battles.Find(1);
           
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 1 });

            _context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            // de skal eksitere på hvert vores tabel siden de begge 2 er 
            var sbjoin = new SamuraiBattle { SamuraiId = 1, BattleId = 1 };
            _context.Add(sbjoin);
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 1);
            var quote = samurai.Quotes[0];
            quote.Text = "Didd you hear that again?";
            using (var newcontext = new SamuraiContext())
            {
                //hvis update bliver kørt skulle den override dem alle men gør det ikke lige pt
                //newcontext.Quotes.Update(quote);
                newcontext.Entry(quote).State = EntityState.Modified;
                newcontext.SaveChanges();
            }

        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            //s.id for samurai og DEN MÅ IKKE VÆRE NULL
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 1);
            //Quotes er kun hans quotes og ikke mere.
            samurai.Quotes[1].Text = "Did you hear that";
            _context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("Happy")))
                .ToList();
        }

        private static void LazyLoadQuotes()
        {
            // FirstOrDefault tager den første uden en parameter ellers tager den det første der matcher kriterierne
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name.Contains("Peter-san-san"));

            var quoteCount = samurai.Quotes.Count();
            Console.WriteLine(quoteCount);
        }

        private static void ExplicitLoadQuotes()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name.Contains("Peter-san-san"));
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropertiesWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, s.Quotes})
            //    .ToList();    

            // Happy quotes tæller hvor mange gange ordet Happy opstår i den string
            //var somePropertiesWithQuotes = _context.Samurais
            //    .Select(s => new
            //    {
            //        s.Id,
            //        s.Name,
            //        happyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
            //    });          
            var somePropertiesWithHappyQuotes = _context.Samurais
                .Select(s => new
                {
                    Samurai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name}).ToList();
            var IdsAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }

        public struct IdAndName 
        {
            public int Id;
            public string Name;
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }


        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
        }

        private static void AddQuoteToExistingSamuraiNotTracked_Easy(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Now that i saved you twice",
                SamuraiId = samuraiId
            };
            using (var newcontext = new SamuraiContext())
            {
                newcontext.Quotes.Update(quote);
                newcontext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that i saved you"
            });
            // vi opretter en ny dbcontext og der først her vi begynder at tracke
            using ( var newcontext = new SamuraiContext())
            {
                newcontext.Samurais.Update(samurai);
                newcontext.SaveChanges();
            }
        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai()
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "i've come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai()
            {
                Name = "Kyozo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "i've come to save you"},
                    new Quote { Text = "I told you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "Happy to help"
            });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "kodawkopdaw" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
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
            //hvis vi lægger samurais foran kan vi kun tilføje samurais
            _context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4 );
            _context.SaveChanges();
        }

        private static void InsertMultipleTypes()
        {
            var samurai = new Samurai { Name = "Jin" };
            var clan = new Clan { ClanName = "Clan Sakai" };
            //hvis vi kun bruger add range kan vi tilføje alle de type vi har erklæret i vores metode
            _context.AddRange(samurai, clan);
            _context.SaveChanges();
        }

        private static void GetSamuraisSimpler()
        {
            //var samurais = context.Samurais.ToList();
            var query = _context.Samurais;
            //var samurais = query.ToList();
            // vi skal overveje om det er nødvendigt at bruge denne her metode at gøre det på fordi det kræver mere komputerkraft
            foreach (var samurai in query)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            //Du kan bruge en var til gemme informationen i vores SQL script så folk ikke kan se informationen
            var name = "jin";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            //var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "J%")).ToList();
            var samurais = _context.Samurais.Where(s => s.Name == name).FirstOrDefault();           
        }

        private static void RetrieveAndUpdateSamurais()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "-san";
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(3).ToList();
            samurais.ForEach(s => s.Name += "-San");
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "-san";
            _context.Samurais.Add(new Samurai { Name = "Kikuchiyo" });
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazema",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 01)
            });
            _context.SaveChanges();
        }
    }
}
