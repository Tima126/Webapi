using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string Code { get; set; } = null!;

    public decimal DiscountPercentage { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
