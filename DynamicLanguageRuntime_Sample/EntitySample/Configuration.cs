using EntitySample.Entity;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace EntitySample
{
    public class Configuration : DbMigrationsConfiguration<DbSampleContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// 初始值
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(DbSampleContext context)
        {
            Producer producers1 = new Producer("intel");
            producers1.Products = new List<Product>();
            producers1.Products.Add(new Product("i3"));
            producers1.Products.Add(new Product("i5"));
            producers1.Products.Add(new Product("i7"));
            context.Producers.AddOrUpdate(producers1);

            Producer producers2 = new Producer("AMD");
            producers2.Products = new List<Product>();
            producers2.Products.Add(new Product("A4"));
            producers2.Products.Add(new Product("A6"));
            producers2.Products.Add(new Product("A8"));
            producers2.Products.Add(new Product("A10"));
            context.Producers.AddOrUpdate(producers2);

            context.SaveChanges();
        }
    }
}
