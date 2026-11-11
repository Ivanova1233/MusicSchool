using System;
using System.Collections.Generic;

namespace MusicSchool.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int UserId { get; set; }

    public string? Phone { get; set; }

    public string Specialization { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual User User { get; set; } = null!;
}
