using System;
using System.Collections.Generic;

namespace WebApi_ModelsCons_.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string Code { get; set; } = null!;

    public decimal DiscountPercentage { get; set; }

    public DateTime ExpiryDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
