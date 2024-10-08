﻿using RuppinZombiesDatabase.Models.DAL;

namespace RuppinZombiesDatabase.Models
{
    public class User
    {
        private string userID;
        private int xp;

        public string UserID { get => userID; set => userID = value; }
        public int Xp { get => xp; set => xp = value; }
        public static object Insert(string UserID)
        {
            DBservices db = new DBservices();
            object res = db.InsertUser(UserID);
            return new { UserID = UserID, XP = res };
        }
        public static bool InsertUserAnswer(string userID, int questionID, int userAnswer)
        {
            DBservices db = new DBservices();
            return db.InsertUserAnswer(userID, questionID, userAnswer) > 0;
        }
    }
}
