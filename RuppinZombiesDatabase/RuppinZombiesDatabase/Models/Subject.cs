using RuppinZombiesDatabase.Models.DAL;

namespace RuppinZombiesDatabase.Models
{
    public class Subject
    {
        private int subjectID;
        private string subjectName;

        public int SubjectID { get => subjectID; set => subjectID = value; }
        public string SubjectName { get => subjectName; set => subjectName = value; }
        public static List<string> GetAllSubjects()
        {
            DBservices db = new DBservices();
            return db.GetAllSubjects();
        }
    }
}
