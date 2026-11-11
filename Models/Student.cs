using System;
using System.Collections.Generic;

namespace MusicSchool.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public DateOnly? BirthDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
