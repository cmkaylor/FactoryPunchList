namespace FactoryPunchList.App_Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyDatabaseEntities : DbContext
    {
        //hard coded the original db string placed in the web config file to replace quote& with a "
        private static string dbConnect = "";
        public MyDatabaseEntities()
            : base(dbConnect)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Coin> Coins { get; set; }
    }
}
