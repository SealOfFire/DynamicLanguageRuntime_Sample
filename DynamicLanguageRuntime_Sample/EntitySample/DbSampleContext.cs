using EntitySample.Entity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace EntitySample
{
    public class DbSampleContext : DbContext
    {
        public DbSet<ScriptExpression> ScriptExpressions { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbSampleContext() : base("Name=DBConnection")
        {
            DbInterception.Add(new CommandInterceptor());
        }
    }
}