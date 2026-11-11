using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Models;
using System;
using System.Threading.Tasks;

namespace MusicSchool.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            using var db = Db.Create();
            UsersBox.ItemsSource = await db.Users.Include(u => u.Role).ToListAsync();
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }

    private async void LoginClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (UsersBox.SelectedItem is not User user)
                throw new AppException("Выберите пользователя из списка.");

            await Session.LoginAsync(user.UserId);
            MainWindow.Instance.OpenPage(new CatalogView(), clearHistory: true);
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }

    private void GuestClick(object? sender, RoutedEventArgs e)
    {
        Session.Logout();
        MainWindow.Instance.OpenPage(new CatalogView(), clearHistory: true);
    }
}