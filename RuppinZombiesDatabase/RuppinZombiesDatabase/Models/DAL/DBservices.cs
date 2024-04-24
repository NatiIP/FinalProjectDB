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
                    q.CorrectAnswer = Convert.ToInt32(dataReader["correctAnswer"]);
                    q.SubjectID = Convert.ToInt32(dataReader["SubjectID"]);

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
        // Create the SqlCommand to get all of the subjects
        //---------------------------------------------------------------------------------
        public List<Subject> GetAllSubjects()
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

            List<Subject> subject = new List<Subject>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Subject s = new Subject();
                    s.SubjectID = Convert.ToInt32(dataReader["SubjectID"]);
                    s.SubjectName = dataReader["subjectName"].ToString();
  
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
                paramDic.Add("@SubjectID", q.SubjectID);

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

    }
}
