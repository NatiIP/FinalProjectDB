using RuppinZombiesDatabase.Models.DAL;
namespace RuppinZombiesDatabase.Models
{
    public class QuestionInsights
    {
        private int totalAnswers;
        private int correctAnswers;
        private int wrongAnswers;
        private double percentageCorrect;
        private double percentageWrong;
        private int mostCommonWrongAnswer;
        private int uniqueUsers;

        public int TotalAnswers { get => totalAnswers; set => totalAnswers = value; }
        public int CorrectAnswers { get => correctAnswers; set => correctAnswers = value; }
        public int WrongAnswers { get => wrongAnswers; set => wrongAnswers = value; }
        public double PercentageCorrect { get => percentageCorrect; set => percentageCorrect = value; }
        public double PercentageWrong { get => percentageWrong; set => percentageWrong = value; }
        public int MostCommonWrongAnswer { get => mostCommonWrongAnswer; set => mostCommonWrongAnswer = value; }
        public int UniqueUsers { get => uniqueUsers; set => uniqueUsers = value; }

        public static QuestionInsights GetUserQuestionInsight(int questionId)
        {
            DBservices db = new DBservices();
            return db.GetQuestionInsights(questionId);
        }
    }
}
