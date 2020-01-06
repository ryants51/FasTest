using FasTest.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FasTest.Teacher
{
    public partial class TestCreation : System.Web.UI.Page
    {
        int currentUser;
        int classID;
        int questionID;


        protected void Page_Load(object sender, EventArgs e)
        {
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
            error.Visible = false;
        }

        //Set the Assignment ID Cookie value so that it can be kept through the autopostback
        protected void setAssignmentIDCookie(int assignmentID)
        {
            ViewState["AssignmentID"] = assignmentID;
        }

        protected void setClassIDcookie(int ClassID)
        {
            HttpCookie ClassIDCookie = new HttpCookie("ClassIDCookie");
            ClassIDCookie.Value = Convert.ToString(classID);
            Response.Cookies.Add(ClassIDCookie);
        }

        protected void setQuestionIDCookie(int QuestionID)
        {
            HttpCookie QuestionIDCookie = new HttpCookie("QuestionIDCookie");
            QuestionIDCookie.Value = Convert.ToString(QuestionID);
            Response.Cookies.Add(QuestionIDCookie);
        }

        protected int getQuestionIDCookie()
        {
            return Convert.ToInt32(Request.Cookies["QuestionIDCookie"].Value);
        }

        protected void btnChooseClass_Click(object sender, EventArgs e)
        {
            ddlSelectedClass.SelectedIndex = 0;
            collapse1.Attributes.Add("class", "panel-collapse collapse in");
            collapse2.Attributes.Add("class", "panel-collapse collapse");
            collapse3.Attributes.Add("class", "panel-collapse collapse");
            ddlInstructorTests.Items.Clear();
            inputTestName.Text = String.Empty;
            tabCreateEdit.Text = "Create a new test or choose to modify an existing one";
            tabSelectClass.Text = "Choose Class";
            btnChooseClass.Visible = false;
            tabSelectClass.Visible = true;
            btnChooseTest.Visible = false;
            editTestInfo.Visible = false;
            lblInfoSaved.Visible = false;
            tabCreateEdit.Visible = true;
            lblNoTestQuestions.Visible = false;
        }

        protected void ddlSelectedClass_SelectedIndexChanged1(object sender, EventArgs e)
        {
            collapse1.Attributes.Add("class", "panel-collapse collapse");
            collapse2.Attributes.Add("class", "panel-collapse collapse in");
            btnChooseClass.Text = "Class: " + ddlSelectedClass.SelectedItem.Text;
            getTeacherTests();
            btnChooseClass.Visible = true;
            tabSelectClass.Visible = false;
        }

        protected void btnChooseTest_Click(object sender, EventArgs e)
        {
            collapse1.Attributes.Add("class", "panel-collapse collapse");
            collapse2.Attributes.Add("class", "panel-collapse collapse in");
            collapse3.Attributes.Add("class", "panel-collapse collapse");
            getTeacherTests();
            inputTestName.Text = String.Empty;
            tabCreateEdit.Text = "Create a new test or choose to modify an existing one";
            tabCreateEdit.Visible = true;
            btnChooseTest.Visible = false;
            editTestInfo.Visible = false;
            lblInfoSaved.Visible = false;
            lblNoTestQuestions.Visible = false;

        }

        #region CreateTest_Click
        protected void btnCreateTest_Click(object sender, EventArgs e)
        {
            string testName = inputTestName.Text;
            InputValidation v = new InputValidation();


            if (testName == string.Empty)
            {
                errorTestName.Text = "The test name cannot be empty";
            }
            else if (v.VAlphaNumSpace(testName))
            {
                errorTestName.Text = "The test name contains restricted symbols";
            }
            else
            { 
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Create_Test"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(ddlSelectedClass.SelectedValue));
                        cmd.Parameters.AddWithValue("@pTestName", inputTestName.Text);

                        cmd.Connection = con;
                        con.Open();
                        object obj = cmd.ExecuteScalar();
                        if (obj == null)
                        {
                            // record not found, do something
                        }
                        else
                        {
                            // record found, do proper casting like:
                            ViewState["AssignmentID"] = (int)obj;
                        }
                        con.Close();
                        // Get the assignment ID(may not be needed)
                        GetAssignmentID();
                        // Call the method to fill the gridview

                    }
                    errorTestName.Text = "";
                }
                // Collapses the second panel and shows the 3rd
                collapse2.Attributes.Add("class", "panel-collapse collapse");
                collapse3.Attributes.Add("class", "panel-collapse collapse in");

                btnChooseTest.Text = "Creating " + inputTestName.Text;
                btnChooseTest.Visible = true;
                tabCreateEdit.Visible = false;
                GetAssignmentQuestions();
            }

            
        }
        #endregion

        // Fill in the Editable Test Dropdown
        public void getTeacherTests()
        {
            ListItem d = new ListItem();
            d.Value = "0";
            d.Text = "-- Please select a test to edit --";

            ddlInstructorTests.Items.Clear();
            ddlInstructorTests.Items.Add(d);

            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Class_Test_Names"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Class_Test_Names", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@ClassID", Convert.ToInt32(ddlSelectedClass.SelectedValue));
                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Assignment");
                        ddlInstructorTests.DataSource = ds.Tables["Assignment"];
                        ddlInstructorTests.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
        }

        protected void btnEditTest_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlInstructorTests.SelectedValue) != 0)
            {
                setAssignmentIDCookie(Convert.ToInt32(ddlInstructorTests.SelectedValue));
                GetAssignmentQuestions();
                showExistingQuestions();
                resetQuestionSelection();
                // Collapses the second panel and shows the 3rd
                collapse2.Attributes.Add("class", "panel-collapse collapse");
                collapse3.Attributes.Add("class", "panel-collapse collapse in");
                btnChooseTest.Text = "Editing " + ddlInstructorTests.SelectedItem.Text;
                btnChooseTest.Visible = true;
                tabCreateEdit.Visible = false;
                error.Visible = false;
            }
            else
                error.Visible = true;
        }
        protected void ShowThirdPanel()
        {
            GetAssignmentQuestions();
            showExistingQuestions();
            resetQuestionSelection();
            // Collapses the second panel and shows the 3rd
            collapse2.Attributes.Add("class", "panel-collapse collapse");
            collapse3.Attributes.Add("class", "panel-collapse collapse in");
            btnChooseTest.Text = "Editing " + ViewState["AssignmentName"];
            btnChooseTest.Visible = true;
            tabCreateEdit.Visible = false;
            error.Visible = false;
        }

        // Get the assignment questions and fill the gridview
        protected void GetAssignmentQuestions()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Assignment_Questions"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Assignment_Questions", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@AssignmentID", GetAssignmentID());
                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Question");
                        QuestionGridview.DataSource = ds.Tables["Question"];
                        QuestionGridview.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
        }

        //Get the assignmentID from the assginment name. may not be needed.
        protected int GetAssignmentID()
        {
            return Convert.ToInt32(ViewState["AssignmentID"]);
        }

        protected void sctQuestionChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Keeps the selected question type info on screen
            fixSelectedQuestion();
            // Always shows the questions in the test on type change
            showExistingQuestions();
        }

        public void fixSelectedQuestion()
        {
            String qType = sctQuestionChoice.SelectedValue.ToString();

            // Shows the specific criteria for each specific question
            if (qType == "1")
            {
                mcQuestion.Attributes.Clear();
                tfQuestion.Attributes.Add("hidden", "hidden");
                saQuestion.Attributes.Add("hidden", "hidden");
            }
            else if (qType == "3")
            {
                mcQuestion.Attributes.Add("hidden", "hidden");
                tfQuestion.Attributes.Clear();
                saQuestion.Attributes.Add("hidden", "hidden");
            }
            else
            {
                tfQuestion.Attributes.Add("hidden", "hidden");
                mcQuestion.Attributes.Add("hidden", "hidden");
                saQuestion.Attributes.Clear();
            }
        }

        //Create a Question on click
        protected void btnQuestionCreate_Click(object sender, EventArgs e)
        {
            // Multiple Choice
            if (Convert.ToInt32(sctQuestionChoice.SelectedValue) == 1)
            {
                lbNoSelectionMultChoice.Text = "";
                int assignmentID = GetAssignmentID();
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                string question = tbQuestion.Text,
                       question1 = tbMult1.Text,
                       question2 = tbMult2.Text,
                       question3 = tbMult3.Text,
                       question4 = tbMult4.Text;



                if (question.Length > 0 && question1.Length > 0 && question2.Length > 0 && question3.Length > 0)
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Add_MultipleChoice"))
                        {
                            // Pass the values entered to the database procedure
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pAssignmentID", GetAssignmentID());
                            cmd.Parameters.AddWithValue("@pQuestionDesc", question);
                            if (tbMult1.Text != null && tbMult2.Text != null)
                            {
                                cmd.Parameters.AddWithValue("@pChoice1", question1);
                                cmd.Parameters.AddWithValue("@pChoice2", question2);
                                cmd.Parameters.AddWithValue("@pChoice3", question3);
                                cmd.Parameters.AddWithValue("@pChoice4", question4);
                                cmd.Parameters.AddWithValue("@pPointValue", pointValueDropdwon.SelectedValue);
                                if (CheckBox1.Checked)
                                {
                                    cmd.Parameters.AddWithValue("@pAnswer", 1);

                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                else if (CheckBox2.Checked)
                                {
                                    cmd.Parameters.AddWithValue("@pAnswer", 2);

                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }

                                else if (CheckBox3.Checked)
                                {
                                    cmd.Parameters.AddWithValue("@pAnswer", 3);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                else if (CheckBox4.Checked)
                                {
                                    cmd.Parameters.AddWithValue("@pAnswer", 4);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                else
                                {
                                    lbNoSelectionMultChoice.Text = "Must select one option for multiple choice";
                                }
                            }
                            resetQuestionSelection();
                        }
                    }
                    endQuestionCreate();
                    errorQuestion.Text = string.Empty;
                }
                if(question.Length <= 0)
                    errorQuestion.Text = "Enter a Question";
                if(question1.Length <= 0 && question3.Length <= 0 && question2.Length <= 0)
                {
                    errorQuestion2.Text = "The first three answers must be entered";
                }
            }
            // True or False Question
            else if (Convert.ToInt32(sctQuestionChoice.SelectedValue) == 3)
            {
                if (rbTrueAndFalse.SelectedIndex != 0 && rbTrueAndFalse.SelectedIndex != 1)
                {
                    lblNoTFAnswer.Visible = true;
                    fixSelectedQuestion();
                }
                else
                {
                    lblNoTFAnswer.Visible = false;
                    string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Add_TrueFalse"))
                        {
                            // Pass the values entered to the database procedure
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pAssignmentID", GetAssignmentID());
                            cmd.Parameters.AddWithValue("@pQuestionDesc", tbQuestion.Text);

                            if (rbTrueAndFalse.SelectedIndex == 0)
                                cmd.Parameters.AddWithValue("@pAnswer", true);
                            else if (rbTrueAndFalse.SelectedIndex == 1)
                                cmd.Parameters.AddWithValue("@pAnswer", false);
                            cmd.Parameters.AddWithValue("@pPointValue", pointValueDropdwon.SelectedValue);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();

                            con.Close();
                        }
                    }

                    rbTrueAndFalse.SelectedItem.Value = "1";
                    tfQuestion.Attributes.Clear();
                    mcQuestion.Attributes.Add("hidden", "hidden");
                    saQuestion.Attributes.Add("hidden", "hidden");
                    endQuestionCreate();
                }
                
            }
            // Short Answer
            else if (Convert.ToInt32(sctQuestionChoice.SelectedValue) == 2)
            {
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_ShortAnswer"))
                    {
                        // Pass the values entered to the database procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pAssignmentID", GetAssignmentID());
                        cmd.Parameters.AddWithValue("@pQuestionDesc", tbQuestion.Text);
                        cmd.Parameters.AddWithValue("@pAnswer", tbShortAnswer.Text);
                        cmd.Parameters.AddWithValue("@pPointValue", pointValueDropdwon.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                tbShortAnswer.Text = String.Empty;
                tfQuestion.Attributes.Add("hidden", "hidden");
                mcQuestion.Attributes.Add("hidden", "hidden");
                saQuestion.Attributes.Clear();
                endQuestionCreate();
            }
            
        }
        protected void endQuestionCreate()
        {
            tbQuestion.Text = String.Empty;
            GetAssignmentID();
            GetAssignmentQuestions();

            showExistingQuestions();
        }
        
        #region checkBoxFunctions
        // Only allow for one selection on multiple choice
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox2.Checked = false;
            CheckBox3.Checked = false;
            CheckBox4.Checked = false;
            showExistingQuestions();
        }
        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox1.Checked = false;
            CheckBox3.Checked = false;
            CheckBox4.Checked = false;
            showExistingQuestions();
        }
        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            CheckBox4.Checked = false;
            showExistingQuestions();
        }
        protected void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            CheckBox3.Checked = false;
            showExistingQuestions();
        }
        #endregion

        protected void btnQuestionEdit_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                int questionType = Convert.ToInt32(sctQuestionChoice.SelectedValue);
                int mcQuestionAnswer = 0;
                questionID = getQuestionIDCookie();
                if (questionType == 1)
                {
                    using (SqlCommand MCcmd = new SqlCommand("Update_Multiple_Choice"))
                    {
                        MCcmd.CommandType = CommandType.StoredProcedure;
                        MCcmd.Parameters.AddWithValue("@pQuestionID", questionID);
                        MCcmd.Parameters.AddWithValue("@pQuestionDesc", tbQuestion.Text);
                        MCcmd.Parameters.AddWithValue("@pChoice1", tbMult1.Text);
                        MCcmd.Parameters.AddWithValue("@pChoice2", tbMult2.Text);
                        MCcmd.Parameters.AddWithValue("@pChoice3", tbMult3.Text);
                        MCcmd.Parameters.AddWithValue("@pChoice4", tbMult4.Text);
                        MCcmd.Parameters.AddWithValue("@pPointValue", pointValueDropdwon.SelectedValue);
                        if (CheckBox1.Checked)
                            mcQuestionAnswer = 1;
                        else if (CheckBox2.Checked)
                            mcQuestionAnswer = 2;
                        else if (CheckBox3.Checked)
                            mcQuestionAnswer = 3;
                        else if (CheckBox4.Checked)
                            mcQuestionAnswer = 4;

                        MCcmd.Parameters.AddWithValue("@pAnswer", mcQuestionAnswer);

                        MCcmd.Connection = con;
                        con.Open();
                        MCcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                // Short Answer
                else if (questionType == 2)
                {
                    using (SqlCommand SAcmd = new SqlCommand("Update_Short_Answer"))
                    {
                        SAcmd.CommandType = CommandType.StoredProcedure;
                        SAcmd.Parameters.AddWithValue("@pQuestionID", questionID);
                        SAcmd.Parameters.AddWithValue("@pQuestionDesc", tbQuestion.Text);
                        SAcmd.Parameters.AddWithValue("@pAnswer", tbShortAnswer.Text);
                        SAcmd.Parameters.AddWithValue("@pPointValue", pointValueDropdwon.SelectedValue);
                        SAcmd.Connection = con;
                        con.Open();
                        SAcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                // True False
                else if (questionType == 3)
                {
                    using (SqlCommand TFcmd = new SqlCommand("Update_True_False"))
                    {
                        TFcmd.CommandType = CommandType.StoredProcedure;
                        TFcmd.Parameters.AddWithValue("@pQuestionID", questionID);
                        TFcmd.Parameters.AddWithValue("@pQuestionDesc", tbQuestion.Text);
                        if (rbTrueAndFalse.SelectedIndex == 0)
                            TFcmd.Parameters.AddWithValue("@pAnswer", true);
                        else if (rbTrueAndFalse.SelectedIndex == 1)
                            TFcmd.Parameters.AddWithValue("@pAnswer", false);
                        TFcmd.Parameters.AddWithValue("@pPointValue", pointValueDropdwon.SelectedValue);
                        TFcmd.Connection = con;
                        con.Open();
                        TFcmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                GetAssignmentQuestions();
                resetQuestionSelection();
                
            }
        }

        protected void resetQuestionSelection()
        {
            tbQuestion.Text = String.Empty;
            tbMult1.Text = String.Empty;
            tbMult2.Text = String.Empty;
            tbMult3.Text = String.Empty;
            tbMult4.Text = String.Empty;
            CheckBox1.Checked = true;
            CheckBox2.Checked = false;
            CheckBox3.Checked = false;
            CheckBox4.Checked = false;
            tbShortAnswer.Text = String.Empty;
            sctQuestionChoice.Enabled = true;
            sctQuestionChoice.SelectedIndex = 0;
            pointValueDropdwon.SelectedIndex = 0;
            rbTrueAndFalse.ClearSelection();
            btnQuestionEdit.Visible = false;
            btnQuestionCreate.Visible = true;
            fixSelectedQuestion();
        }

        // View Questions in test
        protected void viewQuestionButton_Click(object sender, EventArgs e)
        {
            showExistingQuestions();


            fixSelectedQuestion();
        }

        // Fill in the questions from the test
        protected void getInstructorQuestions()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Assignment_Questions"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Assignment_Questions", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@AssignmentID", ddlAllTeacherTests.SelectedValue);
                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Question");
                        ExistingQuestionsGridview.DataSource = ds.Tables["Question"];


                        ExistingQuestionsGridview.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }

            fixSelectedQuestion();
        }

        // Delete Question or Fill in fields to edit a question
        #region testQuestionsCommands
        protected void QuestionGridview_RowCommand(object sender,
        System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow selectedRow = QuestionGridview.Rows[currentRowIndex];
            questionID = Int32.Parse(QuestionGridview.DataKeys[selectedRow.RowIndex].Values[0].ToString());
            setQuestionIDCookie(questionID);
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                if (e.CommandName == "deleteQuestion")
                {
                    using (SqlCommand cmd = new SqlCommand("Delete_Question_From_Test"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AssignmentID", GetAssignmentID());
                        cmd.Parameters.AddWithValue("@QuestionID", questionID);
                        try
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch
                        {

                        }
                    }
                    resetQuestionSelection();
                }
                else if (e.CommandName == "editQuestion")
                {

                    using (SqlCommand cmd = new SqlCommand("Get_Question_Information"))
                    {
                        btnQuestionCreate.Visible = false;
                        btnQuestionEdit.Visible = true;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@QuestionID", questionID);
                        cmd.Parameters.Add("@QuestionDescription", SqlDbType.VarChar, 200).Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add("@QuestionType", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add("@QuestionPointValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.Connection = con;
                        string questionDescription = string.Empty;
                        int questionType = 0;
                        int pointValue = 1;
                        try
                        {
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();
                            questionDescription = reader.GetString(0);
                            questionType = reader.GetInt32(1);
                            pointValue = reader.GetInt32(2);
                            sctQuestionChoice.SelectedValue = questionType.ToString();
                            sctQuestionChoice.Enabled = false;
                            pointValueDropdwon.SelectedValue = pointValue.ToString();
                            tbQuestion.Text = questionDescription;
                            con.Close();
                        }
                        catch
                        {

                        }
                        // Multiple Choice
                        if (questionType == 1)
                        {
                            using (SqlCommand MCcmd = new SqlCommand("Get_MC_Information"))
                            {
                                MCcmd.CommandType = CommandType.StoredProcedure;
                                MCcmd.Parameters.AddWithValue("@QuestionID", questionID);
                                MCcmd.Parameters.Add("@PossibleAnswer1", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;
                                MCcmd.Parameters.Add("@PossibleAnswer2", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;
                                MCcmd.Parameters.Add("@PossibleAnswer3", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;
                                MCcmd.Parameters.Add("@PossibleAnswer4", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;
                                MCcmd.Parameters.Add("@Answer", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                                MCcmd.Connection = con;
                                con.Open();
                                SqlDataReader MCreader = MCcmd.ExecuteReader();
                                MCreader.Read();
                                string PossibleAnswer1 = MCreader.GetString(0);
                                string PossibleAnswer2 = MCreader.GetString(1);
                                string PossibleAnswer3 = MCreader.GetString(2);
                                string PossibleAnswer4 = MCreader.GetString(3);
                                int QuestionAnswer = MCreader.GetInt32(4);
                                tbMult1.Text = PossibleAnswer1;
                                tbMult2.Text = PossibleAnswer2;
                                tbMult3.Text = PossibleAnswer3;
                                tbMult4.Text = PossibleAnswer4;
                                switch (QuestionAnswer)
                                {
                                    case 1:
                                        CheckBox1.Checked = true;
                                        CheckBox2.Checked = false;
                                        CheckBox3.Checked = false;
                                        CheckBox4.Checked = false;
                                        break;
                                    case 2:
                                        CheckBox2.Checked = true;
                                        CheckBox1.Checked = false;
                                        CheckBox3.Checked = false;
                                        CheckBox4.Checked = false;
                                        break;
                                    case 3:
                                        CheckBox3.Checked = true;
                                        CheckBox1.Checked = false;
                                        CheckBox2.Checked = false;
                                        CheckBox4.Checked = false;
                                        break;
                                    case 4:
                                        CheckBox4.Checked = true;
                                        CheckBox1.Checked = false;
                                        CheckBox2.Checked = false;
                                        CheckBox3.Checked = false;
                                        break;
                                    default:
                                        break;

                                }
                                con.Close();
                            }
                        }
                        // Short Answer
                        else if (questionType == 2)
                        {
                            using (SqlCommand SAcmd = new SqlCommand("Get_SA_Information"))
                            {
                                SAcmd.CommandType = CommandType.StoredProcedure;
                                SAcmd.Parameters.AddWithValue("@QuestionID", questionID);
                                SAcmd.Parameters.Add("@Answer", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;

                                SAcmd.Connection = con;
                                con.Open();
                                SqlDataReader SAreader = SAcmd.ExecuteReader();
                                SAreader.Read();
                                string QuestionAnswer = SAreader.GetString(0);
                                tbShortAnswer.Text = QuestionAnswer;
                                con.Close();
                            }
                        }
                        // True False
                        else if (questionType == 3)
                        {
                            using (SqlCommand TFcmd = new SqlCommand("Get_TF_Information"))
                            {
                                TFcmd.CommandType = CommandType.StoredProcedure;
                                TFcmd.Parameters.AddWithValue("@QuestionID", questionID);
                                TFcmd.Parameters.Add("@Answer", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue;

                                TFcmd.Connection = con;
                                con.Open();
                                SqlDataReader TFreader = TFcmd.ExecuteReader();
                                TFreader.Read();
                                try
                                {
                                    Boolean QuestionAnswer = TFreader.GetBoolean(0);
                                    if (QuestionAnswer)
                                    {
                                        rbTrueAndFalse.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        rbTrueAndFalse.SelectedIndex = 1;
                                    }
                                }
                                catch
                                {

                                }
                                con.Close();
                            }
                        }
                    }
                }
            }
            GetAssignmentQuestions();
            showExistingQuestions();
            fixSelectedQuestion();
        }
        #endregion
 
        // View questions from other tests
        protected void getOtherQuestionButton_Click(object sender, EventArgs e)
        {
            showQuestionsToBeAdded();
            getAllTeacherClasses();
            getAllTeacherTests();
            getInstructorQuestions();

        }

        private void getAllTeacherClasses()
        {
            ddlAllTeacherClasses.Items.Clear();
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Teacher_Class_Names"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Teacher_Class_Names", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@InstructorID", currentUser);
                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Class");
                        ddlAllTeacherClasses.DataSource = ds.Tables["Class"];
                        ddlAllTeacherClasses.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
            ddlAllTeacherClasses.SelectedValue = ddlSelectedClass.SelectedValue;
            getAllTeacherTests();
            
        }

        protected void AllTeacherClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlAllTeacherTests.Items.Clear();
            getAllTeacherTests();
            getInstructorQuestions();
            showQuestionsToBeAdded();
        }

        public void ddlAllTeacherTests_SelectedIndexChanged(object sender, EventArgs e)
        {
           // ddlAllTeacherTests.Items.Clear();
            getInstructorQuestions();
            showQuestionsToBeAdded();
        }

        private void getAllTeacherTests()
        {
            ListItem d = new ListItem();
            d.Value = "0";
            d.Text = "-- Please select a test to add questions from --";

            ddlAllTeacherTests.Items.Clear();
            ddlAllTeacherTests.Items.Add(d);

            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Existing_Class_Test_Names"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Existing_Class_Test_Names", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@ClassID", ddlAllTeacherClasses.SelectedValue);
                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Assignment");
                        ddlAllTeacherTests.DataSource = ds.Tables["Assignment"];
                        ddlAllTeacherTests.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
            if (ddlAllTeacherClasses.SelectedValue == ddlSelectedClass.SelectedValue)
            {
                ddlAllTeacherTests.Items.RemoveAt(ddlInstructorTests.SelectedIndex);
            }
        }

        // Automatically shows the questions added to the test
        public void showExistingQuestions()
        {
            viewQuestions.Attributes.Clear();
            viewQuestions.Attributes.Add("class", "col-lg-8");
            getOtherQuestions.Attributes.Clear();
            getOtherQuestions.Attributes.Add("hidden", "hidden");
        }

        // Automatically shows the questions to add from other tests
        public void showQuestionsToBeAdded()
        {
            getOtherQuestions.Attributes.Clear();
            getOtherQuestions.Attributes.Add("class", "col-lg-8");
            viewQuestions.Attributes.Clear();
            viewQuestions.Attributes.Add("hidden", "hidden");
        }
        
        // Add existing question to test
        protected void ExistingQuestionsGridview_RowCommand(object sender,
        System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow selectedRow = ExistingQuestionsGridview.Rows[currentRowIndex];
            int questionID = Int32.Parse(ExistingQuestionsGridview.DataKeys[selectedRow.RowIndex].Values[0].ToString());
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Copy_Question"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pAssignmentID", GetAssignmentID());
                    cmd.Parameters.AddWithValue("@pQuestionID", questionID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            GetAssignmentQuestions();

            showQuestionsToBeAdded();

            fixSelectedQuestion();
        }

        protected void btnUpdateTestName_Click(object sender, EventArgs e)
        {

        }

        protected void FillEditableTestInfo()
        {
            
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand testcmd = new SqlCommand("Get_Test_Name_And_Dates"))
                    {

                        //Get Assignment Questions is the procedure
                        testcmd.CommandType = CommandType.StoredProcedure;
                        testcmd.Parameters.AddWithValue("@pTestID", GetAssignmentID());
                        testcmd.Parameters.Add("@pTestName", SqlDbType.VarChar, 200).Direction = ParameterDirection.ReturnValue;
                        testcmd.Parameters.Add("@StartDate", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        testcmd.Parameters.Add("@Deadline", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        testcmd.Parameters.Add("@TestDuration", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                        testcmd.Connection = con;
                        con.Open();
                        SqlDataReader reader = testcmd.ExecuteReader();
                        reader.Read();
                           
                        txtTestName.Text = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                        {
                            testStartDate.Text = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                            testStartTime.Text = reader.GetDateTime(1).ToString("HH:mm");
                        }
                        else
                        {
                            testStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                            testStartTime.Text = DateTime.Now.ToString("HH:mm");
                        }

                        if (!reader.IsDBNull(2))
                        {
                            txtTestEndDate.Text = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                            txtTestEndTime.Text = reader.GetDateTime(2).ToString("HH:mm");
                        }
                        else
                        {
                            txtTestEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                            txtTestEndTime.Text = DateTime.Now.ToString("HH:mm");

                        }
                        txtTestDuration.Text = reader.GetValue(3).ToString();
                    }
                    
                }
            }
        }
        protected void btnSaveTestInfo_Click(object sender, EventArgs e)
        {
            lblInfoSaved.Visible = false;
            lblNoTestQuestions.Visible = false;
            int numberQuestions;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand testcmd = new SqlCommand("Get_Number_Questions"))
                    {

                        //Get Assignment Questions is the procedure
                        testcmd.CommandType = CommandType.StoredProcedure;
                        testcmd.Parameters.AddWithValue("@AssignmentID", ddlInstructorTests.SelectedValue);
                        testcmd.Parameters.Add("@pNumberQuestions", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                        testcmd.Connection = con;
                        con.Open();
                        SqlDataReader reader = testcmd.ExecuteReader();
                        reader.Read();

                        numberQuestions = reader.GetInt32(0);
                    }
                }
            }
            if (numberQuestions != 0)
            {
                try
                {
                    TimeSpan StartTime = TimeSpan.Parse(testStartTime.Text);
                    TimeSpan EndTime = TimeSpan.Parse(txtTestEndTime.Text);


                    if (txtTestDuration.Text.Length > 0 && txtTestDuration.Text.Length < 4 && testStartDate.Text.Length < 15)
                    {
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (con)
                            {
                                using (SqlCommand savecmd = new SqlCommand("Update_Test_Name_And_Dates"))
                                {
                                    try
                                    {
                                        //Get Assignment Questions is the procedure
                                        savecmd.CommandType = CommandType.StoredProcedure;
                                        savecmd.Parameters.AddWithValue("@pTestID", GetAssignmentID());
                                        savecmd.Parameters.AddWithValue("@pTestName", txtTestName.Text);
                                        savecmd.Parameters.AddWithValue("@pStartDate", testStartDate.Text + " " + StartTime);
                                        savecmd.Parameters.AddWithValue("@pDeadline", txtTestEndDate.Text + " " + EndTime);
                                        savecmd.Parameters.AddWithValue("@pTestDuration", txtTestDuration.Text);


                                        savecmd.Connection = con;
                                        con.Open();
                                        savecmd.ExecuteNonQuery();
                                        con.Close();
                                        getTeacherTests();
                                        ddlInstructorTests.SelectedValue = GetAssignmentID().ToString();
                                        publishingCatchAllError.Text = string.Empty;
                                    }
                                    catch
                                    {
                                        publishingCatchAllError.Text = "Please verify that your input is valid.";
                                    }
                                }
                            }
                        }
                        lblInfoSaved.Visible = true;
                        txtTestDurationerror.Text = string.Empty;
                    }
                    else
                    {
                        txtTestDurationerror.Text = "Time must fall between 1 - 999 minutes.";
                    }
                }
                catch
                {
                    publishingCatchAllError.Text = "Please enter valid Start and End Times";
                }
            }
            else
            {
                lblNoTestQuestions.Visible = true;
            }
        }

        protected void ddlInstructorTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            error.Visible = false;
            lblInfoSaved.Visible = false;
            lblNoTestQuestions.Visible = false;
            setAssignmentIDCookie(Convert.ToInt32(ddlInstructorTests.SelectedValue));
            if (ddlInstructorTests.SelectedIndex != 0)
            {
                FillEditableTestInfo();
                editTestInfo.Visible = true;
                
            }
            else
                editTestInfo.Visible = false;
        }

        protected void btnDeleteTest_Click(object sender, EventArgs e)
        {
            lblNoTestQuestions.Visible = false;
            lblInfoSaved.Visible = false;
            testNameDelete.InnerHtml = ddlInstructorTests.SelectedItem.ToString();
            mp1.Show();
        }
        protected void btnConfirmDeleteTest_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand testcmd = new SqlCommand("Delete_Test"))
                    {

                        //Get Assignment Questions is the procedure
                        testcmd.CommandType = CommandType.StoredProcedure;
                        testcmd.Parameters.AddWithValue("@pTestID", GetAssignmentID());
                        testcmd.Connection = con;
                        con.Open();
                        testcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            getTeacherTests();
            editTestInfo.Visible = false;
        }

        protected void btnSaveQuestions_Click(object sender, EventArgs e)
        {
            collapse1.Attributes.Add("class", "panel-collapse collapse");
            collapse2.Attributes.Add("class", "panel-collapse collapse in");
            collapse3.Attributes.Add("class", "panel-collapse collapse");
            getTeacherTests();
            
            FillEditableTestInfo();
            ddlInstructorTests.SelectedValue = GetAssignmentID().ToString();
            editTestInfo.Visible = true;
            tabCreateEdit.Text = "Choose Test";
            inputTestName.Text = "";
            btnChooseTest.Visible = false;
            tabCreateEdit.Visible = true;
            lblInfoSaved.Visible = false;
            lblNoTestQuestions.Visible = false;
        }

        
    }
}