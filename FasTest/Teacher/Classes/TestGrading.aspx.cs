using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Teacher.Classes
{
    public partial class TestGrading : System.Web.UI.Page
    {
        int currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
            if(!IsPostBack)
            fillClassesDropdown();

        }

        protected void ddlSelectAssignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectAssignment.SelectedIndex != 0)
                allTests.Visible = true;
            else
                allTests.Visible = false;
            StudentsTest.Visible = false;
            AllStudentsGridview.Visible = true;
            fillAllStudentsTestsGridview();
            fillAllTestVisible();

        }

        protected void ddlSelectclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            allTests.Visible = false;
            fillAssignmentDropdown();
            fillTestGridview();
        }

        private void fillClassesDropdown()
        {
            ListItem d = new ListItem();
            d.Value = "0";
            d.Text = "-- Please Select a Class --";

            ddlSelectclass.Items.Clear();
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
                        ddlSelectclass.DataSource = ds.Tables["Class"];
                        ddlSelectclass.DataBind();
                        ddlSelectclass.Items.Insert(0, d);
                        da.Dispose();
                        con.Close();
                    }
                }
            }
            fillAssignmentDropdown();

        }

        private void fillAssignmentDropdown()
        {
            ListItem d = new ListItem();
            d.Value = "0";
            d.Text = "-- Please Select a Test --";

            ddlSelectAssignment.Items.Clear();
            ddlSelectAssignment.Items.Add(d);

            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Existing_Class_Test_Names_For_Grading"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Existing_Class_Test_Names_For_Grading", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@ClassID", ddlSelectclass.SelectedValue);
                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Assignment");
                        ddlSelectAssignment.DataSource = ds.Tables["Assignment"];
                        ddlSelectAssignment.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
            fillAllStudentsTestsGridview();
            StudentsTest.Visible = false;
            AllStudentsGridview.Visible = true;
        }
        protected void fillGridview()
        {
                StudentsTest.Visible = true;
                fillTestGridview();
        }

        protected void fillAllStudentsTestsGridview()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_All_Students_Tests"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_All_Students_Tests", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@pTestID", ddlSelectAssignment.SelectedValue);

                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "StudentAssignment");
                        AllStudentsGridview.DataSource = ds.Tables["StudentAssignment"];


                        AllStudentsGridview.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
        }

        protected void fillTestGridview()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Student_Test_Answers"))
                    {
                        getcmd.Connection = con;
                        con.Open();
                        //Get Assignment Questions is the procedure
                        SqlDataAdapter da = new SqlDataAdapter("Get_Student_Test_Answers", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@pTestID", ddlSelectAssignment.SelectedValue);
                        da.SelectCommand.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(ViewState["SelectedStudentID"]));

                        DataSet ds = new DataSet();
                        // Question is Table name
                        da.Fill(ds, "Question");
                        grdStudentsView.DataSource = ds.Tables["Question"];


                        grdStudentsView.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
        }

        protected void visibleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
            int studentID = Int32.Parse(AllStudentsGridview.DataKeys[row.RowIndex].Values[0].ToString());
            CheckBox visibleCheckBox = (CheckBox)sender;
            bool TestVisiblity = visibleCheckBox.Checked;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Change_Test_Visiblity"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pVisible", TestVisiblity);
                    cmd.Parameters.AddWithValue("@pStudentID", studentID);
                    cmd.Parameters.AddWithValue("@pAssignmentID", ddlSelectAssignment.SelectedValue);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            AllStudentsGridview.DataBind();
            fillAllStudentsTestsGridview();
            fillAllTestVisible();
        }

        protected void CorrectCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
            int choiceID = Int32.Parse(grdStudentsView.DataKeys[row.RowIndex].Values[0].ToString());
            CheckBox CorrectCheckBox = (CheckBox)sender;
            bool QuestionCorrect = CorrectCheckBox.Checked;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Update_Question_Correct"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pCorrect", QuestionCorrect);
                    cmd.Parameters.AddWithValue("@pChoiceID", choiceID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            grdStudentsView.DataBind();
            fillAllStudentsTestsGridview();
            fillTestGridview();
        }

        protected void fillGradedCheckBox()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_test_Graded"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTestID", ddlSelectAssignment.SelectedValue);
                    cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(ViewState["SelectedStudentID"]));

                    cmd.Parameters.Add("@Graded", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    Gradedchk.Checked = Convert.ToBoolean(reader.GetBoolean(0));
                    con.Close();
                }
            }
        }

        protected void fillPledgeSignedCheckBox()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Test_Pledge_Signed"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTestID", ddlSelectAssignment.SelectedValue);
                    cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(ViewState["SelectedStudentID"]));

                    cmd.Parameters.Add("@isSigned", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    chkPledgeSigned.Checked = Convert.ToBoolean(reader.GetBoolean(0));
                    con.Close();
                }
            }
        }

        protected void Gradedchk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox GradedCheckbox = (CheckBox)sender;
            bool Graded = GradedCheckbox.Checked;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Mark_Test_Graded"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTestID", ddlSelectAssignment.SelectedValue);
                    cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(ViewState["SelectedStudentID"]));
                    cmd.Parameters.AddWithValue("@pGraded", Graded);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            AllStudentsGridview.DataBind();
            fillAllStudentsTestsGridview();
            fillAllTestVisible();
        }

        protected void btnChooseClass_Click(object sender, EventArgs e)
        {
            ddlSelectclass.SelectedIndex = 0;
            ddlSelectAssignment.Items.Clear();
        }


        protected void AllStudentsGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow selectedRow = AllStudentsGridview.Rows[currentRowIndex];
            ViewState["SelectedStudentID"] = Int32.Parse(AllStudentsGridview.DataKeys[selectedRow.RowIndex].Values[0].ToString());
            String testTaken = AllStudentsGridview.DataKeys[selectedRow.RowIndex].Values[1].ToString();
            String testName = AllStudentsGridview.DataKeys[selectedRow.RowIndex].Values[2].ToString();

            if (testTaken != "Not Started" && testTaken != "In Progress")
            {
                fillGridview();
                fillGradedCheckBox();
                fillPledgeSignedCheckBox();
                testInfo.InnerHtml = testName + " - " + ddlSelectAssignment.SelectedItem;
            }
            else
            {
                StudentsTest.Visible = false;
            }


        }

        protected void AllStudentsGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text == "Not Started")
                {
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[0].Enabled = false;
                    e.Row.Cells[4].Enabled = false;

                }
                else if (e.Row.Cells[3].Text == "Not Graded")
                {
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[0].Enabled = false;
                }
                else if (e.Row.Cells[3].Text == "In Progress")
                {
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Orange;
                    e.Row.Cells[0].Enabled = false;
                    e.Row.Cells[4].Enabled = false;
                }
                else if (e.Row.Cells[3].Text == "Time Expired")
                {
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.DarkOrange;
                    e.Row.Cells[0].Enabled = false;
                }
                else
                {
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void pledgeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;
            int studentID = Int32.Parse(AllStudentsGridview.DataKeys[row.RowIndex].Values[0].ToString());
            CheckBox PledgeSignedCheckBox = (CheckBox)sender;
            bool PledgeSigned = PledgeSignedCheckBox.Checked;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Change_Pledge_Signed_Grading"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pSigned", PledgeSigned);
                    cmd.Parameters.AddWithValue("@pStudentID", studentID);
                    cmd.Parameters.AddWithValue("@pAssignmentID", ddlSelectAssignment.SelectedValue);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            AllStudentsGridview.DataBind();
            fillAllStudentsTestsGridview();
            fillAllTestVisible();
        }

        protected void chkPledgeSigned_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox PledgeSignedCheckBox = (CheckBox)sender;
            bool PledgeSigned = PledgeSignedCheckBox.Checked;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Change_Pledge_Signed_Grading"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pSigned", PledgeSigned);
                    cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(ViewState["SelectedStudentID"]));
                    cmd.Parameters.AddWithValue("@pAssignmentID", ddlSelectAssignment.SelectedValue);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            AllStudentsGridview.DataBind();
            fillAllStudentsTestsGridview();
        }

        protected void fillAllTestVisible()
        {
            Boolean allGraded;
            Boolean allVisible = false;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Check_All_Graded"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", ddlSelectclass.SelectedValue);
                    cmd.Parameters.AddWithValue("@pAssignmentID", ddlSelectAssignment.SelectedValue);

                    cmd.Parameters.Add("@AllGraded", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add("@AllVisible", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    allGraded = Convert.ToBoolean(reader.GetBoolean(0));
                    if (!reader.IsDBNull(1))
                    allVisible = Convert.ToBoolean(reader.GetBoolean(1));
                    con.Close();
                }
            }
            if (allGraded)
                chkAllVisible.Enabled = true;

            if (!allGraded)
                chkAllVisible.Enabled = false;
            chkAllVisible.Checked = allVisible;
        }

        protected void chkAllVisible_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox allVisibleCheckbox = (CheckBox)sender;
            bool allVisible = allVisibleCheckbox.Checked;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Set_All_Visibility"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", ddlSelectclass.SelectedValue);
                    cmd.Parameters.AddWithValue("@pAssignmentID", ddlSelectAssignment.SelectedValue);
                    cmd.Parameters.AddWithValue("@pVisible", allVisible);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            AllStudentsGridview.DataBind();
            fillAllStudentsTestsGridview();
        }
    }
}