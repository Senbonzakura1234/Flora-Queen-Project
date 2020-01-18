using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.EnterpriseServices;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Hosting;
using Flora_Queen_Project.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flora_Queen_Project.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Flora_Queen_Project.Models.ApplicationDbContext>
    {
        private static string URL = "https://api.myjson.com/bins/c5pwu";
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Flora_Queen_Project.Models.ApplicationDbContext context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Database.ExecuteSqlCommand("delete from dbo.occasions;");
            context.Database.ExecuteSqlCommand("delete from dbo.colors;");
            context.Database.ExecuteSqlCommand("delete from dbo.types;");
            context.Database.ExecuteSqlCommand("delete from dbo.products;");
            //add Occasion
            List<Occasion> listOccasions = new List<Occasion>();
            listOccasions.Add(new Occasion()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Birthday",
                OccasionStatus = Occasion.OccasionStatusEnum.Show
            });
            listOccasions.Add(new Occasion()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Love and Romance",
                OccasionStatus = Occasion.OccasionStatusEnum.Show
            });
            listOccasions.Add(new Occasion()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "New Baby",
                OccasionStatus = Occasion.OccasionStatusEnum.Show
            });
            listOccasions.Add(new Occasion()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Sympathy",
                OccasionStatus = Occasion.OccasionStatusEnum.Show
            });

            //add Type
            List<Models.Type> listTypes = new List<Models.Type>();
            listTypes.Add(new Models.Type()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Rose",
                TypeStatus = Models.Type.TypeStatusEnum.Show
            });
            listTypes.Add(new Models.Type()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Lilies",
                TypeStatus = Models.Type.TypeStatusEnum.Show
            });
            listTypes.Add(new Models.Type()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Gerberas",
                TypeStatus = Models.Type.TypeStatusEnum.Show
            });

            //add Color
            List<Color> listColors = new List<Color>();
            listColors.Add(new Color()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Red Flower",
                ColorStatus = Color.ColorStatusEnum.Show
            });
            listColors.Add(new Color()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "Yellow Flower",
                ColorStatus = Color.ColorStatusEnum.Show
            });
            listColors.Add(new Color()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = "White Flower",
                ColorStatus = Color.ColorStatusEnum.Show
            });

            context.Types.AddRange(listTypes);
            context.Occasions.AddRange(listOccasions);
            context.Colors.AddRange(listColors);
            context.SaveChanges();

            //add product
            var client = new HttpClient();
            var responseContent = client.GetAsync(URL).Result.Content.ReadAsStringAsync().Result;
            List<ProductItem> lsProductItems = JsonConvert.DeserializeObject<List<ProductItem>>(responseContent);
            List<Product> listProducts = new List<Product>();
            foreach (var f in lsProductItems)
            {
                listProducts.Add(new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Description = f.name,
                    ImgUrl = f.imgUrl,
                    ColorId = context.Colors.FirstOrDefault(x => x.Name.Contains(f.Color))?.Id,
                    OccasionId = context.Occasions.FirstOrDefault(x => x.Name.Contains(f.Occasion))?.Id,
                    TypeId = context.Types.FirstOrDefault(x => x.Name.Contains(f.Type))?.Id,
                    Name = f.name,
                    InStock = 100,
                    Price = 100000,
                    ProductStatus = Product.ProductStatusEnum.Published
                });
            }

            context.Products.AddRange(listProducts);
            context.SaveChanges();
        }
    }

    public class ProductItem
    {
        public string name { get; set; }
        public string Occasion { get; set; }
        public string Color { get; set; }
        public string  Type { get; set; }
        public string imgUrl { get; set; }
    }
}
