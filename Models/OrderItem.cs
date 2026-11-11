using System;
using System.Collections.Generic;

namespace MusicSchool.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int GroupId { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
