using Avalonia.Controls;
using MusicSchool.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSchool.Views;

public partial class TeacherGroupsView : UserControl
{
    public TeacherGroupsView()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadGroupsAsync();
    }

    private async Task LoadGroupsAsync()
    {
        try
        {
            if (Session.CurrentUser == null) return;

            using var db = Db.Create();

            // 1. Если зашел Администратор — он видит ВСЕ группы
            if (Session.IsAdmin)
            {
                var allGroups = await db.Groups.ToListAsync();
                GroupsList.ItemsSource = allGroups;
                return;
            }

            // 2. Если зашел Преподаватель — он видит только свои группы
            var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == Session.CurrentUser.UserId);

            if (teacher == null)
            {
                MainWindow.Instance.ShowMessage("У вас нет привязанных учебных групп.");
                GroupsList.ItemsSource = null;
                return;
            }

            var myGroups = await db.Groups
                .Where(g => g.TeacherId == teacher.TeacherId)
                .ToListAsync();

            GroupsList.ItemsSource = myGroups;
        }
        catch (Exception ex)
        {
            MainWindow.Instance.ShowError(ex);
        }
    }
}