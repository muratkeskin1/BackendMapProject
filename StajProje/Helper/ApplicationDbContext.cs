using Microsoft.EntityFrameworkCore;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Helper
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

        }
        public DbSet<UserLogin> User { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<ATM> ATMs { get; set; }
        public DbSet<StdCapacity> StdCapacities {get;set;}
        public DbSet<AtmHistory> AtmHistories { get; set; }
        public DbSet<DeliveryHistory> DeliveryHistories { get; set; }
        public DbSet<AtmDeliveryHistory> AtmDeliveryHistories { get; set; }
    }
}
