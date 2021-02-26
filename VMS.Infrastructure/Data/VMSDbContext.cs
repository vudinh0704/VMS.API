using Microsoft.EntityFrameworkCore;
using VMS.Core.Entities;

namespace VMS.Infrastructure.Data
{
    public class VMSDbContext : DbContext
    {        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<TransactionAC> TransactionACs { get; set; }
        public DbSet<TransactionBA> TransactionBAs { get; set; }
        public DbSet<TransactionBC> TransactionBCs { get; set; }
        public DbSet<Ward> Wards { get; set; }

        public VMSDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().EnableSensitiveDataLogging().EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Account>().ToTable("Account").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Attendance>().ToTable("Attendance");
            modelBuilder.Entity<Campaign>().ToTable("Campaign").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Category>().ToTable("Category").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Comment>().ToTable("Comment").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Function>().ToTable("Function").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Group>().ToTable("Group").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Permission>().ToTable("Permission").HasKey(k => new { k.FunctionId, k.GroupId });
            modelBuilder.Entity<Post>().ToTable("Post").HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<TransactionAC>().ToTable("TransactionAC");
            modelBuilder.Entity<TransactionBA>().ToTable("TransactionBA");
            modelBuilder.Entity<TransactionBC>().ToTable("TransactionBC");
            modelBuilder.Entity<Ward>().ToTable("Ward");

            base.OnModelCreating(modelBuilder);
        }
    }
}