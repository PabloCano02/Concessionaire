﻿using Concessionaire.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Concessionaire.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<VehicleType> VehicleTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Brand>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique();
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<VehicleType>().HasIndex(vt => vt.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique();
        }
    }
}
