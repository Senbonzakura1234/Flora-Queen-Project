using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

// ReSharper disable InconsistentNaming

namespace Flora_Queen_Project.Models
{
    public class PaymentResultViewModel
    {
        public double? Amount { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }

        public enum PaymentStatusEnum
        {
            [Display(Name = "Fail")]
            Fail = 0,
            [Display(Name = "Success")]
            Success = 1
        }
    }

    public class CartItem
    {
        public string id { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public double discount { get; set; }

        public void Display()
        {
            Debug.WriteLine("id pro: " + id);
            Debug.WriteLine("count: " + count);
            Debug.WriteLine("price: " + price);
            Debug.WriteLine("discount: " + discount);
        }
    }
}