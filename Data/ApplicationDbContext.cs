using Microsoft.EntityFrameworkCore;
using Ticketing.Models;

namespace Ticketing.Data
{
      public class ApplicationDbContext : DbContext
      {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options){
                  
            }
            public DbSet<Ticket> Tickets{get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasData(
                  new Ticket{
                        Id=1,
                        Title="New ticket sample",
                        Description="An example of description",
                        Attachment="an example of an attachment.png"
                  }
            );
        }
    }
}

