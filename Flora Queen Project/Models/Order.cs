﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Flora_Queen_Project.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public double Amount { get; set; }
        public string OrderDescription  { get; set; }
        public string BankCode { get; set; }
        // ReSharper disable InconsistentNaming
        public int vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }

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