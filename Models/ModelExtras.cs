using System;
using System.Collections.Generic;
using MusicSchool.Models;

namespace MusicSchool.Models;

public partial class User
{
    // ФИО одной строкой
    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    public override string ToString() => $"{Login} — {FullName}";
}

public partial class Order
{
    // Как заявка выглядит в списке
    public override string ToString() =>
        $"Заявка № {OrderId} · {OrderDate:dd.MM.yyyy} · {Student?.User?.FullName}";
}

public partial class OrderItem
{
    public override string ToString() =>
        $"Позиция № {OrderItemId} | Цена: {UnitPrice:N2} руб.";
}
public partial class CourseProgram
{
    public string ProgramName { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public int CategoryId { get; set; }
    public int ProgramId { get; set; }

    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public override string ToString() => $"{ProgramName} | Цена: {BasePrice} руб.";
}