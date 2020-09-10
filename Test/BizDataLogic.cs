using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiAppDomain;
using System.Diagnostics;
using SamuraiApp.Data;
using SamuraiAppDomain;
using ConsoleApp;
using System.Linq;

namespace Test
{
    [TestClass]
    public class BizDataLogic
    {
        [TestMethod]
        public void AddMultipleSamuraisReturnsCorrectNumberOfInsertedRows()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("AddMultipleSamurais");
            using (var context = new SamuraiContext(builder.Options))
            {
                var bizlogic = new BusinessDataLogic(context);
                var nameList = new string[] { "peter1", "peter2", "peter3", };
                var result = bizlogic.AddMultipleSamurais(nameList);
                Assert.AreEqual(nameList.Count(), result);
            }

            /*
            var builder = new DbContextOptionsBuilder();
            using (var context = new SamuraiContext(builder.Options))
            {                
                builder.UseInMemoryDatabase("CanInsertSamurai");
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                var samurai = new Samurai();
                context.Samurais.Add(samurai);
                Debug.WriteLine($"after save: {samurai.Id}");

                context.SaveChanges();
                Debug.WriteLine($"after save: {samurai.Id}");

                Assert.AreEqual(EntityState.Added, context.Entry(samurai).State);
            }
            */
        }
    }
}
