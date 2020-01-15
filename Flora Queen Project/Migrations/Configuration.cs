using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //context.Database.ExecuteSqlCommand("dbcc checkident ('occasions', reseed, 0)");
            context.Database.ExecuteSqlCommand("delete from dbo.colors;");
            context.Database.ExecuteSqlCommand("delete from dbo.types;");
            context.Database.ExecuteSqlCommand("delete from dbo.products;");
            context.Database.ExecuteSqlCommand("delete from products where 1 = 1");

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
            //string file = HttpContext.Current.Server.MapPath(@"~\App_Data\Cloudinary.json");
            //string file = Path.Combine(HttpRuntime.AppDomainAppPath, "/App_Data/Cloudinary.json");
            //string file = HostingEnvironment.MapPath(@"~/App_Data/Cloudinary.json");

            Console.WriteLine("@@@111@@@@@@@@2");
            
            
            
            var content = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                , @"App_Data\Cloudinary.json"); ;
            Debug.WriteLine("@@@@@@@@@@@2");
            Debug.WriteLine(content);
            Debug.WriteLine("@@@");
            List<ProductItem> lsProductItems = JsonConvert.DeserializeObject<List<ProductItem>>(content);

            List<Product> listProducts = new List<Product>();
            foreach (var f in lsProductItems)
            {
                listProducts.Add(new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Description = f.imgUrl,
                    ColorId = context.Colors.FirstOrDefault(x => x.Name.Contains(f.Color))?.Id,
                    OccasionId = context.Occasions.FirstOrDefault(x => x.Name.Contains(f.Occasion))?.Id,
                    TypeId = context.Types.FirstOrDefault(x => x.Name.Contains(f.Occasion))?.Id,
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
        public string imgUrl { get; set; }
    }
}
