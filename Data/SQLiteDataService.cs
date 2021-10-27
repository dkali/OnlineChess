using System.Collections.Generic;
using System;
using EFChessData;

namespace OnlineChess.Data
{
    public class SQLiteDataService
    {
        // public SQLiteDataService(ChessDataContext db)
        // {
        //     _db = db;
        //     Console.WriteLine($"Database path: {_db.DbPath}.");
        // }

        public void AddPlayer(string accountId, string userName, string accessToken, string refreshToken)
        {
            var db = new ChessDataContext();
            Console.WriteLine($"Database path: {db.DbPath}.");

            var user = db.Users.Find(accountId);

            if (user == null)
            {
                // create new entry
                Console.WriteLine($"[SQLite] Inserting a new player {accountId}");
                db.Add(new Account { AccountId = accountId, AccessToken = accessToken,
                    RefreshToken = refreshToken, UserName = userName });
            }
            else
            {
                // Update entry
                Console.WriteLine($"[SQLite] Updating player {accountId} data");
                user.AccessToken = accessToken;
                if (!string.IsNullOrEmpty(refreshToken))
                    user.RefreshToken = refreshToken;
                user.UserName = userName;
            }
            db.SaveChanges();
        }

        public string GetPlayerName(string accountId)
        {
            var db = new ChessDataContext();

            var user = db.Users.Find(accountId);
            return (user == null ? "" : user.UserName);
        }
        
    }
}