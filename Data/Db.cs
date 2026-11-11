using Microsoft.EntityFrameworkCore;
using MusicSchool.Data;
using System;
using System.IO;
namespace MusicSchool;

public static class Db
{
    public static MusicSchoolContext Create()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "connection.txt");
        string connection = File.ReadAllText(path).Trim();

        var options = new DbContextOptionsBuilder<MusicSchoolContext>()
            .UseSqlServer(connection)
            .Options;

        return new MusicSchoolContext(options);
    }
}