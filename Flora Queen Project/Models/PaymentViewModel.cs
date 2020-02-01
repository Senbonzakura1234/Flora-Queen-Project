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

    public class CheckoutViewModel
    {
        [Display(Name = "Firstname *")]
        [Required(ErrorMessage = "Firstname is required")]
        public string Firstname { get; set; }

        [Display(Name = "Lastname *")]
        [Required(ErrorMessage = "Lastname is required")]
        public string Lastname { get; set; }

        [Display(Name = "Company Name *")]
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Display(Name = "Address *")]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Display(Name = "Postcode / ZIP *")]
        [Required(ErrorMessage = "Postcode / ZIP is required")]
        public string ZipCode { get; set; }

        [Display(Name = "Phone number *")]
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please type valid Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Email address *")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please type valid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Payment Method *")]
        [Required(ErrorMessage = "Payment Method is required")]
        public Order.OrderPaymentMethodEnum PaymentMethod { get; set; }
    }


    public class CartItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public double total { get; set; }

        public void Display()
        {
            Debug.WriteLine("id pro: " + id);
            Debug.WriteLine("name pro: " + name);
            Debug.WriteLine("imgUrl pro: " + imgUrl);
            Debug.WriteLine("count: " + count);
            Debug.WriteLine("price: " + price);
            Debug.WriteLine("discount: " + discount);
            Debug.WriteLine("total: " + total);
        }
    }
}