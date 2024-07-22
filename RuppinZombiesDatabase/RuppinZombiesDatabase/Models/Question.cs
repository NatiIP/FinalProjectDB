using RuppinZombiesDatabase.Models.DAL;

namespace RuppinZombiesDatabase.Models
{
    public class Question
    {
        private int id;
        private string content;
        private List<string> answers;
        private int correctAnswer;
        private string lecturerID;
        private DateTime date_submitted;
        private string subject;

        public int Id { get => id; set => id = value; }
        public string Content { get => content; set => content = value; }
        public List<string> Answers { get => answers; set => answers = value; }
        public int CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }
        public string LecturerID { get => lecturerID; set => lecturerID = value; }
        public DateTime Date_submitted { get => date_submitted; set => date_submitted = value; }
        public string Subject { get => subject; set => subject = value; }

        public static List<Question> GetAllQuestions()
        {
            DBservices db = new DBservices();
            return db.GetAllQuestions();
        } 
        public static List<string> GetAllSubjects()
        {
            DBservices db = new DBservices();
            return db.GetAllSubjects();
        }
        public static List<Question> GetUserQuestions(string id)
        {
            DBservices db = new DBservices();
            return db.GetUserQuestions(id);
        }
        public bool Insert()
        {
            DBservices db = new DBservices();
            return db.InsertQuestion(this) > 0;
        }
        
        public static bool DeleteQuestion(int id)
        {
            DBservices db = new DBservices();
            return db.DeleteQuestion(id) > 0;
        } 
        public static bool DeleteQuestions(string ids)
        {
            DBservices db = new DBservices();
            return db.DeleteQuestions(ids) > 0;
        }
    }
}
