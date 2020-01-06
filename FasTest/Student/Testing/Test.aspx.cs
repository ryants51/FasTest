using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;



namespace FasTest.Student.Testing
{
    public partial class Test : System.Web.UI.Page
    {

        List<QuestionBank> questionBank;
        int questionIndex;
        int assignmentID = 0;
        int studentAssignment; //get_student_assignment
        public class QuestionBank
        {
            public int QuestionID { get; set; }
            public string Question { get; set; }
            public int QuestionType { get; set; }
            public string QuestionMultChoice1 { get; set; }
            public string QuestionMultChoice2 { get; set; }
            public string QuestionMultChoice3 { get; set; }
            public string QuestionMultChoice4 { get; set; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                questionIndex = 0;
                HttpCookie questionIndexCookie = new HttpCookie("questionIndex");
                questionIndexCookie.Value = questionIndex.ToString();
                Response.Cookies.Add(questionIndexCookie);
            }
            else
            {
                try
                {
                    //retrieve assignmentID id cookie, try catch is because it will autopostback before the cookie is set
                    //HttpCookie AssignmentIDCookie = new HttpCookie("AssignmentIDCookie");

                    questionIndex = Convert.ToInt32(Request.Cookies["questionIndex"].Value);
                }
                catch
                {

                }
            }
            questionBank = new List<QuestionBank>();
            assignmentID = Convert.ToInt32(Request.Cookies["pAssignmentID"].Value);

            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Questions_And_Choices"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTestID", assignmentID);

                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var list = new List<QuestionBank>();
                        while (reader.Read())
                        {

                            string iQuestionMultChoice1;
                            string iQuestionMultChoice2;
                            string iQuestionMultChoice3;
                            string iQuestionMultChoice4;

                            try
                            {
                                iQuestionMultChoice1 = reader.GetString(3);
                            }
                            catch
                            {
                                iQuestionMultChoice1 = String.Empty;
                            }

                            try
                            {
                                iQuestionMultChoice2 = reader.GetString(4);
                            }
                            catch
                            {
                                iQuestionMultChoice2 = String.Empty;
                            }

                            try
                            {
                                iQuestionMultChoice3 = reader.GetString(5);
                            }
                            catch
                            {
                                iQuestionMultChoice3 = String.Empty;
                            }

                            try
                            {
                                iQuestionMultChoice4 = reader.GetString(6);
                            }
                            catch
                            {
                                iQuestionMultChoice4 = String.Empty;
                            }

                            list.Add(new QuestionBank
                            {
                                QuestionID = reader.GetInt32(0),
                                Question = reader.GetString(1),
                                QuestionType = reader.GetInt32(2),
                                QuestionMultChoice1 = iQuestionMultChoice1,
                                QuestionMultChoice2 = iQuestionMultChoice2,
                                QuestionMultChoice3 = iQuestionMultChoice3,
                                QuestionMultChoice4 = iQuestionMultChoice4
                            });
                        }
                        questionBank = list;
                    }
                    con.Close();
                }

            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Student_Assignment"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    int currentUser;
                    bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
                    cmd.Parameters.AddWithValue("@pStudentID", currentUser);
                    cmd.Parameters.AddWithValue("@pTestID", assignmentID);

                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                studentAssignment = reader.GetInt32(0);
                            }
                        }
                    }

                }
                con.Close();
            }
            if(!IsPostBack)
            {
                Set_Question();
            }

            Validate_Time();

        }

        
        protected void Set_Question()
        {

            Set_Test_Name();
            lblQuestion.Text = "Question: " + questionBank[questionIndex].Question;
            Check_Btns();
            // True and False Question
            if (questionBank[questionIndex].QuestionType == 3)
            {
                rblTF.ClearSelection();
                divTF.Visible = true;
                divMult.Visible = false;
                divShortAns.Visible = false;
            }
            // Short Answer
            else if (questionBank[questionIndex].QuestionType == 2)
            {
                tbShortAnswer.Text = String.Empty;
                divTF.Visible = false;
                divMult.Visible = false;
                divShortAns.Visible = true;
            }

            // Multiple Choice
            else if (questionBank[questionIndex].QuestionType == 1)
            {
                string userSelection = String.Empty;
                divTF.Visible = false;
                divMult.Visible = true;
                divShortAns.Visible = false;
                rblMult.Items.Clear();
                rblMult.Items.Add(new ListItem(questionBank[questionIndex].QuestionMultChoice1, "1"));
                rblMult.Items.Add(new ListItem(questionBank[questionIndex].QuestionMultChoice2, "2"));
                rblMult.Items.Add(new ListItem(questionBank[questionIndex].QuestionMultChoice3, "3"));
                if (questionBank[questionIndex].QuestionMultChoice4 != "")
                {
                    rblMult.Items.Add(new ListItem(questionBank[questionIndex].QuestionMultChoice4, "4"));
                }
            }

            Get_Current_Answer(studentAssignment, questionBank[questionIndex].QuestionID);
        }

        protected void Check_Btns()
        {
            if (questionIndex == (questionBank.Count - 1))
            {
                btnNextQuestion.Visible = false;
                btnPrevQuestion.Visible = true;
                btnReviewTest.Visible = true;
            }
            else if (questionIndex == 0)
            {
                btnNextQuestion.Visible = true;
                btnPrevQuestion.Visible = false;
                btnReviewTest.Visible = false;
            }
            else
            {
                btnNextQuestion.Visible = true;
                btnPrevQuestion.Visible = true;
                btnReviewTest.Visible = false;
            }
        }

        protected void btnPrevQuestion_Click1(object sender, EventArgs e)
        {
            string selection = string.Empty;
            // T/F
            if (questionBank[questionIndex].QuestionType == 3)
            {
                // 1 = true, 2 = false
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, rblTF.SelectedValue);
            }
            // Short Ans
            else if (questionBank[questionIndex].QuestionType == 2)
            {
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, tbShortAnswer.Text);//tbShortAnswer.Text
            }
            // Mult choice
            else if (questionBank[questionIndex].QuestionType == 1)
            {
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, Convert.ToString(rblMult.SelectedIndex + 1));
            }
            questionIndex--;
            Set_Question();
            HttpCookie questionIndexCookie = new HttpCookie("questionIndex");
            questionIndexCookie.Value = questionIndex.ToString();
            Response.Cookies.Add(questionIndexCookie);
        }

        protected void btnNextQuestion_Click1(object sender, EventArgs e)
        {
            string selection = string.Empty;
            // T/F
            if (questionBank[questionIndex].QuestionType == 3)
            {
                // 1 = true, 2 = false
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, rblTF.SelectedValue);
            }
            // Short Ans
            else if (questionBank[questionIndex].QuestionType == 2)
            {
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, tbShortAnswer.Text);//tbShortAnswer.Text
            }
            // Mult choice
            else if (questionBank[questionIndex].QuestionType == 1)
            {
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, Convert.ToString(rblMult.SelectedIndex + 1));
            }

            questionIndex++;
            Set_Question();
            Console.WriteLine(selection);
            HttpCookie questionIndexCookie = new HttpCookie("questionIndex");
            questionIndexCookie.Value = questionIndex.ToString();
            Response.Cookies.Add(questionIndexCookie);
        }

        protected void Update_Current_Question_Choice(int ptestID, int pQuestionID, string pChoice)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Add_Student_Choice"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pStudentTestID", ptestID);
                    cmd.Parameters.AddWithValue("@pQuestionID", pQuestionID);
                    cmd.Parameters.AddWithValue("@pChoice", pChoice);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        protected void Get_Current_Answer(int ptestID, int pQuestionID)
        {
            string answer = String.Empty;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Question_Choice"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pStudentAssignmentID", ptestID);
                    cmd.Parameters.AddWithValue("@pQuestionID", pQuestionID);

                    cmd.Connection = con;
                    con.Open();   
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                answer = Convert.ToString(reader.GetString(0));
                            }
                        }
                    }

                }
                if (answer == "-1")
                {
                    // Display Error
                }
                else
                {
                    if (questionBank[questionIndex].QuestionType == 3)
                    {
                        // 1 = true, 2 = false
                        try
                        {
                            rblTF.SelectedValue = answer;
                        }
                        catch { }
                    }
                    // Short Ans
                    else if (questionBank[questionIndex].QuestionType == 2)
                    {
                        tbShortAnswer.Text = answer;
                    }
                    // Mult choice
                    else if (questionBank[questionIndex].QuestionType == 1)
                    {
                        // Update_Current_Question_Choice(assignmentID, questionBank[questionIndex].QuestionID, Convert.ToString(rblMult.SelectedIndex + 1));
                        try
                        {
                            rblMult.SelectedIndex = Convert.ToInt32(answer) - 1;
                        }
                        catch { }
                    }
                }

            }
        }

        protected void Validate_Time()
        {
            int isValid = 99;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Check_Test_Expiration"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pStudentTestID", studentAssignment);
                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            isValid = reader.GetInt32(0);
                        }
                    }
                    con.Close();
                }
            }

            // Time is valid
            if(isValid == 1)
            {
                
            }
            // Time not valid
            else if(isValid == 0)
            {
                Response.Redirect("~/Student/Testing/TimeExpired.aspx", true);
            }

            Set_Time();
        }

        protected void Set_Time()
        {
            
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Test_Time_Info"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pStudentTestID", studentAssignment);
                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lblStart.Text = "Start Time: " + Convert.ToString(reader.GetString(0));
                            lblDuration.Text = "Duration: " + Convert.ToString(reader.GetInt32(1)) + " minutes";
                            lblEnd.Text = "End Time: " + Convert.ToString(reader.GetString(2));
                        }
                    }
                    con.Close();
                }

            }
        }

        protected void Set_Test_Name()
        {
            // Set the Test Name
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Test_Name"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTestID", assignmentID);
                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testName.InnerHtml = reader.GetString(0);
                        }
                    }
                    con.Close();
                }
            }

            // Set the Question Number
            questionNumber.InnerHtml = "Question # " + Convert.ToString(questionIndex + 1) + " / " + Convert.ToString(questionBank.Count());
        }

        protected void btnReviewTest_Click(object sender, EventArgs e)
        {
            HttpCookie studentAssignmentCookie = new HttpCookie("pStudentAssignment");
            studentAssignmentCookie.Value = studentAssignment.ToString();
            Response.Cookies.Add(studentAssignmentCookie);

            string selection = string.Empty;
            // T/F
            if (questionBank[questionIndex].QuestionType == 3)
            {
                // 1 = true, 2 = false
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, rblTF.SelectedValue);
            }
            // Short Ans
            else if (questionBank[questionIndex].QuestionType == 2)
            {
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, tbShortAnswer.Text);//tbShortAnswer.Text
            }
            // Mult choice
            else if (questionBank[questionIndex].QuestionType == 1)
            {
                Update_Current_Question_Choice(studentAssignment, questionBank[questionIndex].QuestionID, Convert.ToString(rblMult.SelectedIndex + 1));
            }

            Response.Redirect("~/Student/Testing/ReviewQuestions.aspx", true);
        }
    }
}
