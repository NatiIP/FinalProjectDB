using RuppinZombiesDatabase.Models.DAL;

namespace RuppinZombiesDatabase.Models
{
    public class QuestionsAnsInsights
    {
        private string dateLabel;
        private int totalAnswers;
        private int correctAnswers;
        private int wrongAnswers;

        public int TotalAnswers { get => totalAnswers; set => totalAnswers = value; }
        public int CorrectAnswers { get => correctAnswers; set => correctAnswers = value; }
        public int WrongAnswers { get => wrongAnswers; set => wrongAnswers = value; }
        public string DateLabel { get => dateLabel; set => dateLabel = value; }

        public static List<QuestionsAnsInsights> GetUserQuestionsAnsInsights(int lecturerId)
        {
            DBservices db = new DBservices();
            return db.GetUserQuestionsAnsInsights(lecturerId);
        }
    }
}
