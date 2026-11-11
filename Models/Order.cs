using System;
using System.Collections.Generic;

namespace MusicSchool.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public int StudentId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Student Student { get; set; } = null!;
}
