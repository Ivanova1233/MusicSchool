using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSchool.Views;

public partial class OrdersView : UserControl
{
    public OrdersView()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync()
    {
        try
        {
            OrdersList.ItemsSource = null;

            using var db = Db.Create();

            // Загружаем заявки вместе с учениками и пользователями (чтобы получить ФИО)
            var orders = await db.Orders
                .Include(o => o.Student)
                .ThenInclude(s => s.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            OrdersList.ItemsSource = orders;
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }

    private async void ReloadClick(object? sender, RoutedEventArgs e)
    {
        await LoadOrdersAsync();
    }

    // --- УДАЛЕНИЕ ЗАЯВКИ ---
    private async void DeleteOrderClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Session.RequireAdmin();

            var button = sender as Button;
            var order = button?.CommandParameter as Order;
            if (order == null) return;

            using var db = Db.Create();

            var orderToDelete = await db.Orders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (orderToDelete != null)
            {
                db.Orders.Remove(orderToDelete);
                await db.SaveChangesAsync();

                MainWindow.Instance.ShowMessage($"Заявка №{order.OrderId} удалена.");
                await LoadOrdersAsync(); // Обновляем список после удаления
            }
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }
}