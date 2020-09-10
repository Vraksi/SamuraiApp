using SamuraiAppDomain;
using System;
using System.Collections.Generic;
using System.Text;
using SamuraiApp.Data;


namespace ConsoleApp
{
    public class BusinessDataLogic
    {
        private static SamuraiContext _context = new SamuraiContext();

        public BusinessDataLogic(SamuraiContext context)
        {
            _context = context;
        }

        public BusinessDataLogic()
        {
            _context = new SamuraiContext();
        }

        public int AddMultipleSamurais(string[] nameList)
        {
            var samuraiList = new List<Samurai>();
            foreach (var name in nameList)
            {
                samuraiList.Add(new Samurai { Name = name });
            }
            _context.AddRange(samuraiList);

            var dbresult = _context.SaveChanges();
            return dbresult;
        }

    }
}
