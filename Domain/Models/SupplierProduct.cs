using System;
using System.Collections.Generic;

namespace Domain.Models;
public partial class SupplierProduct
{
    public int SupplierProductId { get; set; }

    public int? SupplierId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
