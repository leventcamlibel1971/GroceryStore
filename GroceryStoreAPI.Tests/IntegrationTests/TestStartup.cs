using System.Collections.Generic;
using FluentValidation.AspNetCore;
using GroceryStoreAPI.Entity;
using GroceryStoreAPI.Entity.Domain;
using GroceryStoreAPI.Filters;
using GroceryStoreAPI.Operation.Handlers;
using GroceryStoreAPI.ViewModel.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStoreAPI.Tests.IntegrationTests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)))
                .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetCustomerRequestValidator>())
                .AddApplicationPart(typeof(Startup).Assembly);


            services.AddDbContextFactory<GroceryStoreDbContext>(
                options =>
                    options.UseInMemoryDatabase("datasource=:memory:"));


            services.AddMediatR(typeof(GetCustomerHandler));

            services.AddScoped<ExceptionFilter>();
            services.AddScoped<ModelStateValidate>();


            //test data
            var serviceProvider = services.BuildServiceProvider();


            var customers = new List<Customer>
            {
                new() {Id = 1, Name = "Category1"},
                new() {Id = 2, Name = "Category2"},
                new() {Id = 3, Name = "Category3"},
                new() {Id = 100, Name = "Category100"},
                new() {Id = 101, Name = "Category200"},
                new() {Id = 102, Name = "Category300"}
            };

            var dbContextFactory = serviceProvider.GetService<IDbContextFactory<GroceryStoreDbContext>>();

            using var groceryStoreDbContext = dbContextFactory.CreateDbContext();
            groceryStoreDbContext.Database.EnsureDeleted();
            groceryStoreDbContext.Database.EnsureCreated();

            groceryStoreDbContext.Customers.RemoveRange(groceryStoreDbContext.Customers);
            groceryStoreDbContext.Set<Customer>().AddRange(customers);
            groceryStoreDbContext.SaveChanges();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}