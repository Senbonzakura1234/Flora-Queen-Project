using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flora_Queen_Project.Models
{
    public class Type
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public TypeStatusEnum TypeStatus { get; set; }

        public enum TypeStatusEnum
        {
            [Display(Name = "Deleted")]
            Deleted = 0,
            [Display(Name = "Show")]
            Show = 1
        }

        public Type()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            TypeStatus = TypeStatusEnum.Show;
        }
    }

    public class Occasion
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public OccasionStatusEnum OccasionStatus { get; set; }

        public enum OccasionStatusEnum
        {
            [Display(Name = "Deleted")]
            Deleted = 0,
            [Display(Name = "Show")]
            Show = 1
        }

        public Occasion()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            OccasionStatus = OccasionStatusEnum.Show;
        }
    }

    public class Color
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public ColorStatusEnum ColorStatus { get; set; }

        public enum ColorStatusEnum
        {
            [Display(Name = "Deleted")]
            Deleted = 0,
            [Display(Name = "Show")]
            Show = 1
        }

        public Color()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            ColorStatus = ColorStatusEnum.Show;
        }
    }
}