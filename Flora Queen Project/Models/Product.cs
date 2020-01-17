using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flora_Queen_Project.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int InStock { get; set; }
        public double Price { get; set; }

        [ForeignKey("Type")]
        public string TypeId { get; set; }
        public virtual Type Type { get; set; }

        [ForeignKey("Occasion")]
        public string OccasionId { get; set; }
        public virtual Occasion Occasion { get; set; }

        [ForeignKey("Color")]
        public string ColorId { get; set; }
        public virtual Color Color { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public ProductStatusEnum ProductStatus { get; set; }


        public enum ProductStatusEnum
        {
            [Display(Name = "Published")]
            Published = 1,
            [Display(Name = "UnPublished")]
            UnPublished = 0,
            [Display(Name = "Removed")]
            Removed = -1
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            ProductStatus = ProductStatusEnum.Published;
        }


        public enum BouquetSize
        {
            [Display(Name = "Small")]
            Small = 5,
            [Display(Name = "Medium")]
            Medium = 15,
            [Display(Name = "Large")]
            Large = 30
        }
    }
}