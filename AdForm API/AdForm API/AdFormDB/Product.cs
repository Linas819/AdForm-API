using System;
using System.Collections.Generic;

namespace AdForm_API.AdFormDB;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
