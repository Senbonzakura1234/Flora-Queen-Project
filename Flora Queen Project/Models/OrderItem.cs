using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flora_Queen_Project.Models
{
    public class OrderItem
    {
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }

        [Key, Column(Order = 0)]
        public string ProductId { get; set; }

        [Key, Column(Order = 1)]
        public string OrderId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}