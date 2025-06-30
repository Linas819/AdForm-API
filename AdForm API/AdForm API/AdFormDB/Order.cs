using System;
using System.Collections.Generic;

namespace AdForm_API.AdFormDB;

public partial class Order
{
    public int Id { get; set; }

    public string? OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
