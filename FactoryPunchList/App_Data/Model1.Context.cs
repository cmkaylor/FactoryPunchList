namespace FactoryPunchList.App_Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class factorydbEntities : DbContext
    {
        //hard coded the original db string placed in the web config file to replace quote& with a "
        //Azure App Service does not like metadata formed from the ORM, Entity Models
        private static string dbConnect = "";
        public factorydbEntities()
            : base(dbConnect)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Punchlist> Punchlists { get; set; }
        public virtual DbSet<Supervisor> Supervisors { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<Update> Updates { get; set; }
    }
}
