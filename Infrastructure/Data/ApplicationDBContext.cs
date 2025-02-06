using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<LogEntry> LogEntry { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<BusinessAccount> BusinessAccount { get; set; }

        public DbSet<Product> Product { get; set; }
        
        public DbSet<ProductCarat> ProductCarat { get; set; }

        public DbSet<ProductColor> ProductColor { get; set; }

        public DbSet<ProductCut> ProductCut { get; set; }

        public DbSet<ProductClarity> ProductClarity { get; set; }

        public DbSet<ProductShapes> ProductShape { get; set; }

        public DbSet<ProductImages> ProductImages { get; set; }

        public DbSet<FileManager> FileManager { get; set; }

        public DbSet<AddressMaster> AddressMaster { get; set; }

        public DbSet<VirtualAppointment> VirtualAppointment { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<SubCategory> SubCategory { get; set; }

    }
}
