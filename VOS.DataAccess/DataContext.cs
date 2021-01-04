using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using VOS.Model;
using WalkingTec.Mvvm.Core;

namespace VOS.DataAccess
{
    public class DataContext : FrameworkContext
    {
        public DbSet<City> Citys { get; set; }
        public DbSet<Category> Categoryes { get; set; }
        public DbSet<VOS_PEmployee> VOS_PEmployees { get; set; }
        public DbSet<VOS_Customer> VOS_Customers { get; set; }
        public DbSet<VOS_Shop> VOS_Shops { get; set; }
        public DbSet<VOS_Plan> VOS_Plans { get; set; }
        public DbSet<VOS_Collection> VOS_Collections { get; set; }
        public DbSet<VOS_Task> VOS_Tasks { get; set; }
        public DbSet<VOS_Rule> VOS_Rules { get; set; }

        public DbSet<VOS_User> VOS_User { get; set; }

        public DbSet<VOS_Distribution> VOS_Distribution { get; set; }
        public DataContext(CS cs)
             : base(cs)
        {
        }

        public DataContext(string cs, DBTypeEnum dbtype, string version = null)
             : base(cs, dbtype, version)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VOS_Task>().HasOne(i => i.Executor).WithMany().HasForeignKey(t => t.ExecutorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<VOS_Task>().HasOne(i => i.Distributor).WithMany().HasForeignKey(t => t.DistributorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<VOS_Task>().HasOne(i => i.Completer).WithMany().HasForeignKey(t => t.CompleterId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<VOS_Task>().HasOne(i => i.Refunder).WithMany().HasForeignKey(t => t.RefunderId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<VOS_PEmployee>().HasIndex(x => x.WeChat);
            base.OnModelCreating(modelBuilder);
        }

    }

    /// <summary>
    /// DesignTimeFactory for EF Migration, use your full connection string,
    /// EF will find this class and use the connection defined here to run Add-Migration and Update-Database
    /// </summary>
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            return new DataContext("your full connection string", DBTypeEnum.SqlServer);
        }
    }

}
