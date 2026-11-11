using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;


namespace MusicSchool.Views;

public partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; } = null!;
    private readonly Stack<UserControl> history = new();

    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
        // При запуске открываем страницу входа
        OpenPage(new LoginView(), clearHistory: true);
    }

    // Метод для переключения страниц
    public void OpenPage(UserControl page, bool clearHistory = false)
    {
        if (clearHistory) history.Clear();
        else if (PageHost.Content is UserControl current) history.Push(current);

        PageHost.Content = page;
        MessageText.Text = "";
        UserText.Text = Session.Caption;

        // Кнопку "Заявки" видит только администратор
        OrdersButton.IsVisible = Session.IsAdmin;
        TeacherGroupsButton.IsVisible = Session.RoleName == "Преподаватель" || Session.IsAdmin;
    }

    // Методы для кнопок (те самые, которые подчеркнуты на скриншоте)
    private void BackClick(object? sender, RoutedEventArgs e)
    {
        if (history.Count > 0) PageHost.Content = history.Pop();
    }

    private void CatalogClick(object? sender, RoutedEventArgs e)
    {
        OpenPage(new CatalogView(), clearHistory: true);
    }

    private void OrdersClick(object? sender, RoutedEventArgs e)
    {
        OpenPage(new OrdersView());
    }
    private void TeacherGroupsClick(object? sender, RoutedEventArgs e)
    {
        OpenPage(new TeacherGroupsView());
    }

    private void LogoutClick(object? sender, RoutedEventArgs e)
    {
        Session.Logout();
        OpenPage(new LoginView(), clearHistory: true);
    }

    // Показ ошибок и сообщений
    public void ShowMessage(string text)
    {
        MessageText.Foreground = Avalonia.Media.Brushes.LimeGreen;
        MessageText.Text = text;
    }

    public void ShowError(Exception ex)
    {
        MessageText.Foreground = Avalonia.Media.Brushes.Red;
        MessageText.Text = ex is AppException ? ex.Message : "Ошибка: " + ex.Message;
    }
}