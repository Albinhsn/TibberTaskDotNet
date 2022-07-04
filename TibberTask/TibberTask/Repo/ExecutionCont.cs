using Microsoft.EntityFrameworkCore;
using TibberTask.Models;

namespace TibberTask.PG
{
    public class ExecutionCont : DbContext 
    {        
        public DbSet<execution> Executions { get; set; }
        
        public ExecutionCont(DbContextOptions<ExecutionCont> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<execution>()
                .Property(p => p.id)
                .ValueGeneratedOnAdd()
                .IsRequired();                            
        }
    }
}
