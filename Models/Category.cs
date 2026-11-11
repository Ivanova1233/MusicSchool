using System;
using System.Collections.Generic;

namespace MusicSchool.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<CourseProgram> Programs { get; set; } = new List<CourseProgram>();
}
