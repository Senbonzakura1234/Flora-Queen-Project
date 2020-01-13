using System;
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
        public DateTime CreatedDate { get; set; }
        // ReSharper disable InconsistentNaming
        public int vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        public Order()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }
}