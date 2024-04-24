using RuppinZombiesDatabase.Models.DAL;

namespace RuppinZombiesDatabase.Models
{
    public class Question
    {
        private int id;
        private string content;
        private List<string> answers;
        private int correctAnswer;
        private int subjectID;

        public int Id { get => id; set => id = value; }
        public string Content { get => content; set => content = value; }
        public List<string> Answers { get => answers; set => answers = value; }
        public int CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }
        public int SubjectID { get => subjectID; set => subjectID = value; }
        public static List<Question> GetAllQuestions()
        {
            DBservices db = new DBservices();
            return db.GetAllQuestions();
        }
        public bool Insert()
        {
            DBservices db = new DBservices();
            return db.InsertQuestion(this) > 0;
        }
    }
}
