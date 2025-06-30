using System;
using System.Collections.Generic;

namespace AdForm_API.AdFormDB;

public partial class Discount
{
    public int DiscountId { get; set; }

    public int ProductId { get; set; }

    public float Percentage { get; set; }

    public int MinQuantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
