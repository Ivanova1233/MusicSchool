using System;
using System.Collections.Generic;

namespace MusicSchool.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public int ProgramId { get; set; }

    public int TeacherId { get; set; }

    public string GroupName { get; set; } = null!;

    public string? Schedule { get; set; }

    public int AvailableSeats { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual CourseProgram CourseProgram { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    // Этот метод определяет, как группа выглядит в списке
    public override string ToString() { return $"{GroupName} | Расписание: {Schedule}"; }
}

        

    