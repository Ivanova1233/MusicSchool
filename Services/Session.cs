using MusicSchool.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MusicSchool;

public static class Session
{
    public static User? CurrentUser { get; private set; }
    public static string RoleName => CurrentUser?.Role?.RoleName ?? "";

    public static bool IsGuest => CurrentUser == null;
    public static bool IsAdmin => RoleName == "Администратор";
    public static bool IsStaff => RoleName == "Администратор" || RoleName == "Преподаватель";

    public static string Caption => CurrentUser == null ? "Гость" : $"{CurrentUser.FullName} · {RoleName}";

    public static async Task LoginAsync(int userId)
    {
        using var db = Db.Create();
        CurrentUser = await db.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .SingleAsync(u => u.UserId == userId);
    }

    public static void Logout()
    {
        CurrentUser = null;
    }

    public static void RequireAdmin()
    {
        if (!IsAdmin) throw new AppException("Недостаточно прав. Требуется роль Администратора.");
    }
}