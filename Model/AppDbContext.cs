namespace TasklistAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using TasklistAPI.Helper;
    using TasklistAPI.Model.Entity;

    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<TaskShare> TaskShares { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<TaskItem>();
            modelBuilder.Entity<UserAccount>();
            modelBuilder.Entity<TaskShare>();
            //modelBuilder.Entity<TaskItem>().HasData(
            //    new TaskItem { Id = 1, Title = "Judul Task 1", Description = "Desc Task 1", DueDate = new DateTime(2024,7,20), Priority = 1 },
            //    new TaskItem { Id = 2, Title = "Judul Task 2", Description = "Desc Task 2", DueDate = new DateTime(2024, 7, 5), Priority = 2 }
            //);

            //HashSalt hashSalt = HashSalt.GenerateSaltedHash(64, "123");
            //var Pwd = hashSalt.Hash;
            //var Salt = hashSalt.Salt;

            //modelBuilder.Entity<UserAccount>().HasData(
            //    new UserAccount { Id = 1, UserName = "user1", Password = Pwd, PasswordSalt = Salt },
            //    new UserAccount { Id = 2, UserName = "user2", Password = Pwd, PasswordSalt = Salt }
            //);

        }

        
    }

}
