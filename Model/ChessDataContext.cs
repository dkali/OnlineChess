using System;
using Microsoft.EntityFrameworkCore;

namespace EFChessData
{
    public class ChessDataContext : DbContext
    {
        public DbSet<Account> Users { get; set; }

        public string DbPath { get; private set; }

        public ChessDataContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}chessdata.db";
            // DbPath = "/Users/dkalinnikov/.local/share/chessdata.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}