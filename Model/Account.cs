namespace EFChessData
{
    public class Account
    {
        public string AccountId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public GameSession CurrentGameSession { get; set; }
    }
}