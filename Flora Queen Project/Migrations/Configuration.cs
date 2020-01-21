using System.Collections.Generic;
using System.Net.Http;
using Flora_Queen_Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Flora_Queen_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    // ReSharper disable once UnusedMember.Global
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private const string Url = "https://api.myjson.com/bins/1g09l2";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Database.ExecuteSqlCommand("delete from dbo.occasions;");
            context.Database.ExecuteSqlCommand("delete from dbo.colors;");
            context.Database.ExecuteSqlCommand("delete from dbo.types;");
            context.Database.ExecuteSqlCommand("delete from dbo.products;");
            //add User Roles
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin"};

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Mod"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Mod" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "User" };

                manager.Create(role);
            }
            //add Occasion
            var listOccasions = new List<Occasion>
            {
                new Occasion()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Birthday",
                    OccasionStatus = Occasion.OccasionStatusEnum.Show
                },
                new Occasion()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Love and Romance",
                    OccasionStatus = Occasion.OccasionStatusEnum.Show
                },
                new Occasion()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "New Baby",
                    OccasionStatus = Occasion.OccasionStatusEnum.Show
                },
                new Occasion()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Sympathy",
                    OccasionStatus = Occasion.OccasionStatusEnum.Show
                }
            };

            //add Type
            var listTypes = new List<Models.Type>
            {
                new Models.Type()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Rose",
                    TypeStatus = Models.Type.TypeStatusEnum.Show
                },
                new Models.Type()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Lilies",
                    TypeStatus = Models.Type.TypeStatusEnum.Show
                },
                new Models.Type()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Gerberas",
                    TypeStatus = Models.Type.TypeStatusEnum.Show
                }
            };

            //add Color
            var listColors = new List<Color>
            {
                new Color()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Red Flower",
                    ColorStatus = Color.ColorStatusEnum.Show
                },
                new Color()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "Yellow Flower",
                    ColorStatus = Color.ColorStatusEnum.Show
                },
                new Color()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "White Flower",
                    ColorStatus = Color.ColorStatusEnum.Show
                }
            };

            context.Types.AddRange(listTypes);
            context.Occasions.AddRange(listOccasions);
            context.Colors.AddRange(listColors);
            context.SaveChanges();

            //add product
            var client = new HttpClient();
            var responseContent = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
            var lsProductItems = JsonConvert.DeserializeObject<List<ProductItem>>(responseContent);
            var listProducts = lsProductItems.Select(f => new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Description = f.Description,
                    ImgUrl = f.ImgUrl,
                    ColorId = context.Colors.FirstOrDefault(x => x.Name.Contains(f.Color))?.Id,
                    OccasionId = context.Occasions.FirstOrDefault(x => x.Name.Contains(f.Occasion))?.Id,
                    TypeId = context.Types.FirstOrDefault(x => x.Name.Contains(f.Type))?.Id,
                    Name = f.Name,
                    InStock = 100,
                    Price = Double.Parse(f.Price),
                    Discount = Math.Round(new Random().NextDouble(),2),
                    ProductStatus = Product.ProductStatusEnum.Published
                })
                .ToList();

            context.Products.AddRange(listProducts);
            context.SaveChanges();
        }
    }

    public class ProductItem
    {
        public string Name { get; set; }
        public string Occasion { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
    }
}
