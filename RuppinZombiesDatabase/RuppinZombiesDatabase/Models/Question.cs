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
    }
}
