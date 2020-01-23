using System.ComponentModel.DataAnnotations;
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
        public int id { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double total { get; set; }
    }
}