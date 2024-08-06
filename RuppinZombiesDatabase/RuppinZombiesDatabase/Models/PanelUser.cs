using RuppinZombiesDatabase.Models.DAL;

namespace RuppinZombiesDatabase.Models
{
    public class PanelUser
    {
        private string id;
        private string displayName;
        private string email;
        private string password;

        public string Id { get => id; set => id = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        public bool RegisterUser()
        {
            DBservices dbs = new DBservices();
            int rowsAff = dbs.RegisterPanelUser(this);
            if (rowsAff > 0) return true;
            throw new Exception("We couldn't register an account with the email and password you provided. Please check your details and try again.");
        }

        public string LogIn(string email, string password)
        {
            DBservices dbs = new DBservices();
            string loginResult = dbs.LogIn(email, password);

            return loginResult;
        }

        public PanelUser GetUserInfoById(string id)
        {
            DBservices dbs = new DBservices();
            PanelUser userData = dbs.GetUserInfoById(id);
            if (userData == null) throw new Exception("Error getting user info");
            else return userData;
        }
         static public object GetPanelUsers()
        {
            DBservices dbs = new DBservices();
            object userData = dbs.GetPanelUsers();
            if (userData == null) throw new Exception("Error getting users info");
            else return userData;
        }

    }
}
