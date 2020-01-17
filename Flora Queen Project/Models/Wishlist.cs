using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Flora_Queen_Project.Models
{
    public class Wishlist
    {
        [Key, Column(Order = 0)]
        public string ProductId { get; set; }

        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        public WishlistStatusEnum WishlistStatus { get; set; }
        public enum WishlistStatusEnum
        {
            [Display(Name = "Added")]
            Added = 1,
            [Display(Name = "Deleted")]
            Deleted = 0
        }

        public Wishlist()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            WishlistStatus = WishlistStatusEnum.Added;
        }
    }
}