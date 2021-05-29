using System;
using System.Collections.Generic;
using GroceryStoreAPI.Entity;
using GroceryStoreAPI.Entity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStoreAPI.Tests.UnitTests
{
    public abstract class DatabaseFixture : IDisposable
    {
        private readonly DbContextOptions<GroceryStoreDbContext> _options;
        protected readonly IDbContextFactory<GroceryStoreDbContext> dbContextFactory;

        protected DatabaseFixture()
        {
            var Services = new ServiceCollection();

            Services.AddDbContextFactory<GroceryStoreDbContext>(
                options =>
                    options.UseInMemoryDatabase("datasource=:memory:"));

            ServiceProvider = Services.BuildServiceProvider();


            var customers = new List<Customer>
            {
                new() {Id = 1, Name = "Category1"},
                new() {Id = 2, Name = "Category2"},
                new() {Id = 3, Name = "Category3"},
                new() {Id = 100, Name = "Category100"},
                new() {Id = 101, Name = "Category200"},
                new() {Id = 102, Name = "Category300"}
            };

            dbContextFactory = ServiceProvider.GetService<IDbContextFactory<GroceryStoreDbContext>>();

            using var groceryStoreDbContext = dbContextFactory.CreateDbContext();
            groceryStoreDbContext.Database.EnsureDeleted();
            groceryStoreDbContext.Database.EnsureCreated();

            groceryStoreDbContext.Set<Customer>().AddRange(customers);
            groceryStoreDbContext.SaveChanges();
        }

        public ServiceProvider ServiceProvider { get; protected set; }

        public void Dispose()
        {
            using var groceryStoreDbContext = dbContextFactory.CreateDbContext();
            groceryStoreDbContext.Customers.RemoveRange(groceryStoreDbContext.Customers);
            groceryStoreDbContext.SaveChanges();
        }
    }
}