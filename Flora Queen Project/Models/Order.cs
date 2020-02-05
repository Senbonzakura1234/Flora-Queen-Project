using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flora_Queen_Project.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public double Amount { get; set; }
        public string OrderDescription  { get; set; }

        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPhone { get; set; }
        public string ShipEmail { get; set; }

        public string BankCode { get; set; }
        // ReSharper disable InconsistentNaming
        public string vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }

        public OrderPaymentMethodEnum PaymentMethod { get; set; }
        public enum OrderPaymentMethodEnum
        {
            [Display(Name = "Online Banking")]
            OnlineBanking = 0,
            [Display(Name = "Cash on Delivery")]
            CashOnDelivery = 1,
            [Display(Name = "Direct Bank Transfer")]
            DirectBankTransfer = 2
        }


        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }


        public OrderStatusEnum OrderStatus { get; set; }
        public enum OrderStatusEnum
        {
            [Display(Name = "Pending")]
            Pending = 4,
            [Display(Name = "Shipping")]
            Shipping = 3,
            [Display(Name = "Paid")]
            Paid = 2,
            [Display(Name = "Cancel")]
            Cancel = 0,
            [Display(Name = "Done")]
            Done = 1,
            [Display(Name = "Deleted")]
            Deleted = -1,
        }

        public Order()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            OrderStatus = OrderStatusEnum.Pending;
        }
    }
}