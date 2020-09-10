using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiAppDomain;
using System.Diagnostics;

namespace Test
{
    [TestClass]
    public class InMemory
    {
        [TestMethod]
        public void CanInsertSamuraiIntoDatabase()
        {
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
        }
    }
}
