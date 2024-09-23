using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int? StatusId { get; set; }

    public int? DiscountId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual OrderStatus? Status { get; set; }

    public virtual User? User { get; set; }
}
