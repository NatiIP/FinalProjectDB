namespace RuppinZombiesDatabase.Models
{
    public class User
    {
        private string userID;
        private int xp;

        public string UserID { get => userID; set => userID = value; }
        public int Xp { get => xp; set => xp = value; }
    }
}
