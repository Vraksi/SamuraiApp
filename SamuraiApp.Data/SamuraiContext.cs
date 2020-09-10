﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiAppDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /* for kun at kunne gøre det igennem entity framework
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //kan vi bruge til at undgå tracking på vores queries siden det tager computer kraft at ændre/slette tracking
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory)
                .UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData6");
        }
        */
        //fortæller os at vi Samurai battle har en Key lavet ud af de 2 Id'er fra samurai og battle 
        //vi bruger ToTable til at fortælle hvad tabellen skal hedde vi smider dataen i
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
            modelBuilder.Entity<Horse>().ToTable("Horses");
            //HasNoKey Bliver ALDRIG tracked 
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
        }
        /* En console logger der fortæller hvad vores C# kode bliver lavet om til, Altså en sql query
        public static readonly ILoggerFactory ConsoleLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                //for at kunne bruge AddConsole skal ind i vores csproj og tilføje versionen vi gerne vil have fordi den ikke eksitere i den nuværende
                builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information)
                .AddConsole();
            });
        */
    }
}
