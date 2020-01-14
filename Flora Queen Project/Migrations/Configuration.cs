using System.Collections.Generic;
using Flora_Queen_Project.Models;

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
                Name = "Red Rose",
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

            context.Types.AddRange(listTypes);
            context.Occasions.AddRange(listOccasions);
            context.Colors.AddRange(listColors);
            context.SaveChanges();

            //add product
            List<Product> lisrProducts = new List<Product>();
            lisrProducts.Add(new Product()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Description = "Flower",
                ColorId = listColors[0].Id,
                OccasionId = listOccasions[0].Id,
                TypeId = listTypes[0].Id,
                Name = "Flower 1",
                InStock = 100,
                Price = 100000,
                ProductStatus = Product.ProductStatusEnum.Published
            });
            lisrProducts.Add(new Product()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Description = "Flower",
                ColorId = listColors[1].Id,
                OccasionId = listOccasions[1].Id,
                TypeId = listTypes[1].Id,
                Name = "Flower 2",
                InStock = 100,
                Price = 100000,
                ProductStatus = Product.ProductStatusEnum.Published
            });
        }
    }
}
