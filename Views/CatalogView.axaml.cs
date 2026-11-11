using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSchool.Views;

public partial class CatalogView : UserControl
{
    public CatalogView()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadCoursesAsync();
    }

    private async Task LoadCoursesAsync()
    {
        try
        {
            using var db = Db.Create();
            var programs = await db.Programs.AsNoTracking().ToListAsync();
            this.FindControl<ListBox>("CoursesList")!.ItemsSource = programs;
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }

    private async void OrderClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (Session.CurrentUser == null)
                throw new AppException("Войдите в систему, чтобы оформить заявку.");

            var button = sender as Button;
            var program = button?.DataContext as CourseProgram;
            if (program == null) return;

            using var db = Db.Create();

            var student = await db.Students.FirstOrDefaultAsync(s => s.UserId == Session.CurrentUser.UserId);
            if (student == null)
                throw new AppException("Вы не являетесь учеником. Оформление заявки доступно только ученикам.");

            var group = await db.Groups.FirstOrDefaultAsync(g => g.ProgramId == program.ProgramId && g.AvailableSeats > 0);
            if (group == null)
                throw new AppException("Нет свободных мест на эту программу.");

            var studentOrders = await db.OrderItems
                .Where(oi => oi.Order.StudentId == student.StudentId)
                .ToListAsync();

            decimal totalSpent = studentOrders.Sum(oi => oi.UnitPrice);

            decimal basePrice = program.BasePrice;
            decimal finalPrice = basePrice;
            bool hasDiscount = false;

            // ПРАВИЛЬНЫЙ РАСЧЕТ СКИДКИ:
            // Если ученик уже потратил больше 4500, применяем скидку 20% на ТЕКУЩУЮ покупку
            if (totalSpent > 4500)
            {
                finalPrice = basePrice * 0.8m;
                hasDiscount = true;
            }

            var order = new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Today),
                StudentId = student.StudentId
            };
            db.Orders.Add(order);
            await db.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                GroupId = group.GroupId,
                UnitPrice = finalPrice
            };
            db.OrderItems.Add(orderItem);

            group.AvailableSeats -= 1;

            await db.SaveChangesAsync();

            // Формируем сообщение
            string message = $"Заявка успешно оформлена! Стоимость: {finalPrice:N2} руб.";
            if (hasDiscount)
                message += " (Применена скидка 20%)";

            // ВЫВОДИМ СООБЩЕНИЕ И ОСТАВЛЯЕМ ЕГО НА ЭКРАНЕ
            MainWindow.Instance.ShowMessage(message);

            // Обновляем каталог (чтобы уменьшилось количество мест), 
            // но сообщение при этом НЕ стирается, так как мы обновляем данные на этой же странице
            await LoadCoursesAsync();
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }
}