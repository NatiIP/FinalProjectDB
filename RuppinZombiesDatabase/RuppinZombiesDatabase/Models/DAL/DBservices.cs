using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace RuppinZombiesDatabase.Models.DAL
{

    public class DBservices
    {
        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }

            return cmd;
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand to get all of the questions
        //---------------------------------------------------------------------------------
        public List<Question> GetAllQuestions()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SP_GetAllQuestions", con, null);             // create the command

            List<Question> questions = new List<Question>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Question q = new Question();
                    q.Id = Convert.ToInt32(dataReader["QuestionID"]);
                    q.Content = dataReader["content"].ToString();
                    q.Answers = new List<string>();
                    q.Answers.Add(dataReader["answer1"].ToString());
                    q.Answers.Add(dataReader["answer2"].ToString());
                    q.Answers.Add(dataReader["answer3"].ToString());
                    q.Answers.Add(dataReader["answer4"].ToString());
                    q.LecturerID = (dataReader["LecturerID"].ToString());
                    q.Subject = (dataReader["Subject"].ToString());
                    q.CorrectAnswer = Convert.ToInt32(dataReader["correctAnswer"]);
                    q.Date_submitted = Convert.ToDateTime(dataReader["date_submitted"]);

                    questions.Add(q);
                }

                return questions;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        
        //---------------------------------------------------------------------------------
        // Create the SqlCommand to get all of the questions
        //---------------------------------------------------------------------------------
        public List<Question> GetUserQuestions(string id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@LecturerID", id);

            cmd = CreateCommandWithStoredProcedure("SP_GetLecturerQuestions", con, paramDic); 

            List<Question> questions = new List<Question>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Question q = new Question();
                    q.Id = Convert.ToInt32(dataReader["QuestionID"]);
                    q.Content = dataReader["content"].ToString();
                    q.Answers = new List<string>();
                    q.Answers.Add(dataReader["answer1"].ToString());
                    q.Answers.Add(dataReader["answer2"].ToString());
                    q.Answers.Add(dataReader["answer3"].ToString());
                    q.Answers.Add(dataReader["answer4"].ToString());
                    q.CorrectAnswer = Convert.ToInt32(dataReader["correctAnswer"]);
                    q.Date_submitted = Convert.ToDateTime(dataReader["date_submitted"]);

                    questions.Add(q);
                }

                return questions;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public PanelUser GetUserInfoById(string id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserID", id);

            cmd = CreateCommandWithStoredProcedure("GetUserById", con, paramDic);             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                PanelUser u = new();

                if (dataReader.Read())
                {
                    u.Email = dataReader["Email"].ToString();
                    u.Id = dataReader["Id"].ToString();
                    u.DisplayName = dataReader["DisplayName"].ToString();
                    u.Password = dataReader["Password"].ToString();
                    return u;
                }
                return null;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand to get question (by id) insights
        //---------------------------------------------------------------------------------
        public QuestionInsights GetQuestionInsights(int questionId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@QuestionID", questionId);

            cmd = CreateCommandWithStoredProcedure("SP_GetQuestionInsights", con, paramDic); // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                QuestionInsights insights = new QuestionInsights();

                if (dataReader.Read())
                {
                    insights.TotalAnswers = Convert.ToInt32(dataReader["TotalAnswers"]);
                    insights.CorrectAnswers = Convert.ToInt32(dataReader["CorrectAnswers"]);
                    insights.WrongAnswers = Convert.ToInt32(dataReader["WrongAnswers"]);
                    insights.PercentageCorrect = Convert.ToDouble(dataReader["PercentageCorrect"]);
                    insights.PercentageWrong = Convert.ToDouble(dataReader["PercentageWrong"]);
                    insights.MostCommonWrongAnswer = Convert.ToInt32(dataReader["MostCommonWrongAnswer"]);
                    insights.UniqueUsers = Convert.ToInt32(dataReader["UniqueUsers"]);
                }

                return insights;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand to get total question answers (by id) insights
        //---------------------------------------------------------------------------------
        public List<QuestionsAnsInsights> GetUserQuestionsAnsInsights(int lecturerId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@PanelUserID", lecturerId);

            cmd = CreateCommandWithStoredProcedure("SP_GetUserQuestionAnswersStats", con, paramDic); // create the command
            List<QuestionsAnsInsights> insightsList = new List<QuestionsAnsInsights>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    QuestionsAnsInsights insights = new QuestionsAnsInsights();
                    insights.TotalAnswers = Convert.ToInt32(dataReader["TotalAnswers"]);
                    insights.CorrectAnswers = Convert.ToInt32(dataReader["CorrectAnswers"]);
                    insights.WrongAnswers = Convert.ToInt32(dataReader["WrongAnswers"]);
                    insights.DateLabel = dataReader["DateLabel"].ToString(); 

                    insightsList.Add(insights);
                }

                return insightsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand to get all of the subjects
        //---------------------------------------------------------------------------------
        public List<string> GetAllSubjects()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SP_GetAllSubjects", con, null);             // create the command

            List<string> subject = new List<string>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    string s = dataReader["Subject"].ToString();
                    subject.Add(s);
                }

                return subject;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand to get all of user answers 
        //---------------------------------------------------------------------------------
        //public List<object> GetAllUserAnswers()
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect("myProjDB"); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }

        //    cmd = CreateCommandWithStoredProcedure("SP_GetAllSubjects", con, null);             // create the command

        //    List<Subject> subject = new List<Subject>();

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //        while (dataReader.Read())
        //        {
        //            Subject s = new Subject();
        //            s.SubjectID = Convert.ToInt32(dataReader["QuestionID"]);
        //            s.SubjectName = dataReader["content"].ToString();

        //            subject.Add(s);
        //        }

        //        return subject;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }

        //    finally
        //    {
        //        if (con != null)
        //        {
        //            // close the db connection
        //            con.Close();
        //        }
        //    }
        //}

        //--------------------------------------------------------------------------------------------------
        // This method Inserts a question to the database
        //--------------------------------------------------------------------------------------------------
        public int InsertQuestion(Question q)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            int questionID = 0;

            try
            {
                con = connect("myProjDB"); // create the connection

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@content", q.Content);
                paramDic.Add("@answer1", q.Answers[0]);
                paramDic.Add("@answer2", q.Answers[1]);
                paramDic.Add("@answer3", q.Answers[2]);
                paramDic.Add("@answer4", q.Answers[3]);
                paramDic.Add("@correctAnswer", q.CorrectAnswer);
                paramDic.Add("@LecturerID", q.LecturerID);
                paramDic.Add("@Subject", q.Subject);

                cmd = CreateCommandWithStoredProcedure("SP_InsertQuestion", con, paramDic);  // create the command

                SqlParameter outputParam = new SqlParameter("@QuestionID", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                int numEffected = cmd.ExecuteNonQuery(); // execute the command

                if (numEffected > 0)
                {
                    questionID = Convert.ToInt32(outputParam.Value);
                }

                return questionID;
            }
            catch (Exception ex)
            {
                // Write to log or handle exception appropriately
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // close the db connection
                }
            }
        }
        //--------------------------------------------------------------------------------------------------
        // This method Deletes a question to the database
        //--------------------------------------------------------------------------------------------------
        public int DeleteQuestion(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@QuestionID", id);

            cmd = CreateCommandWithStoredProcedure("DeleteQuestionById", con, paramDic);  // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                                                         //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return (numEffected);

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        } 
        
        //--------------------------------------------------------------------------------------------------
        // This method Deletes a question to the database
        //--------------------------------------------------------------------------------------------------
        public int DeleteQuestions(string ids)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@IdList", ids);

            cmd = CreateCommandWithStoredProcedure("DeleteQuestionsByIds", con, paramDic);  // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                                                         //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return (numEffected);

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Inserts a user to the database and if exists it returns the same user
        //--------------------------------------------------------------------------------------------------
        public object InsertUser(string UserID)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            int questionID = 0;

            try
            {
                con = connect("myProjDB"); // create the connection

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                paramDic.Add("@UserID", UserID);

                cmd = CreateCommandWithStoredProcedure("SP_InsertUser", con, paramDic);  // create the command

                object result = cmd.ExecuteScalar(); // execute the command and get the result

                return result;
            }
            catch (Exception ex)
            {
                // Write to log or handle exception appropriately
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // close the db connection
                }
            }
        }


        //--------------------------------------------------------------------------------------------------
        // This method Inserts a user's answer to a question
        //--------------------------------------------------------------------------------------------------
        public int InsertUserAnswer(string userID, int questionID, int userAnswer)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = connect("myProjDB"); // create the connection

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                paramDic.Add("@UserID", userID);
                paramDic.Add("@QuestionID", questionID);
                paramDic.Add("@UserAnswer", userAnswer);

                cmd = CreateCommandWithStoredProcedure("SP_InsertUserAnswer", con, paramDic);  // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                //int numEffected = Convert.ToInt32(cmd.ExecuteScalar());

                return numEffected;
            }
            catch (Exception ex)
            {
                // Write to log or handle exception appropriately
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // close the db connection
                }
            }
        }


        //--------------------------------------------------------------------------------------------------
        // \\\\\\\\\\\\\\\\\\\\-----------------------PANEL USER-----------------------------\\\\\\\\\\\\\\\
        //--------------------------------------------------------------------------------------------------
       
        //--------------------------------------------------------------------------------------------------
        // This method Inserts a panel user to the database and if exists it returns the same user
        //--------------------------------------------------------------------------------------------------
        public int RegisterPanelUser(PanelUser user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@Email", user.Email);
            paramDic.Add("@UserID", user.Id);
            paramDic.Add("@Password", user.Password);
            paramDic.Add("@DisplayName", user.DisplayName);

            cmd = CreateCommandWithStoredProcedure("SP_RegisterPanelUser", con, paramDic);  // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return (numEffected);
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        public string LogIn(string email, string password)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader reader;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Email", email);
                paramDic.Add("@Password", password);

                cmd = CreateCommandWithStoredProcedure("sp_LoginUser", con, paramDic);  // create the command
                reader = cmd.ExecuteReader(); // execute the command and get the reader

                if (reader.HasRows)
                {
                    reader.Read(); // read the first row

                    string userId = reader["UserId"].ToString(); // read the 'UserId' column
                    string status = reader["Status"].ToString(); // read the 'Status' column

                    if (status == "Pending")
                    {
                        return "Pending"; // Return "Pending" if user's status is "Pending"
                    }
                    else if (status == "Denied")
                    {
                        return "Denied"; // Return "Denied" if user's status is "Denied"
                    }
                    else
                    {
                        return userId; // return the UserId as string
                    }
                }
                else
                {
                    return "Invalid email or password"; // Return "Invalid email or password" if login fails
                }
            }
            catch (Exception ex)
            {
                // write to log or handle the exception
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }



        public int EditDisplayName(PanelUser userData)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@Id", userData.Id);
            paramDic.Add("@DisplayName", userData.DisplayName);

            cmd = CreateCommandWithStoredProcedure("USER_EditProfile", con, paramDic);  // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                                                         //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return (numEffected);

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
    }
}
