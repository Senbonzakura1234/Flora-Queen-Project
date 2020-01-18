using System.ComponentModel.DataAnnotations;

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
}