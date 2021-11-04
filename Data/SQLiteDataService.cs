using System.Collections.Generic;
using System;
using EFChessData;
using Microsoft.Extensions.Logging;

namespace OnlineChess.Data
{
    public class SQLiteDataService
    {
        private ILogger<SQLiteDataService> _logger;
        public SQLiteDataService(ILogger<SQLiteDataService> logger)
        {
            _logger = logger;
        }

        public void AddPlayer(string accountId, string userName, string accessToken, string refreshToken)
        {
            var db = new ChessDataContext();
            // _logger.LogInformation($"Database path: {db.DbPath}.");

            var user = db.Users.Find(accountId);

            if (user == null)
            {
                // create new entry
                _logger.LogInformation($"[SQLite] Inserting a new player {accountId}");
                db.Add(new Account { AccountId = accountId, AccessToken = accessToken,
                    RefreshToken = refreshToken, UserName = userName });
            }
            else
            {
                // Update entry
                _logger.LogInformation($"[SQLite] Updating player {accountId} data");
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