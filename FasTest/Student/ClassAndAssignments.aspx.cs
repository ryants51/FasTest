using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Student
{
    public partial class ClassViewPage : System.Web.UI.Page
    {

        int CurrentSelectedClass = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Gets the user's id from the session
            int currentUser;
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
            //int ClassId = Convert.ToInt32(ClassList.Rows[1].Cells[1].Text);

            if (!IsPostBack)
            {
                // Run the query and bind the resulting DataSet
                // to the GridView control.
                UpdateTestList();
            }

        }

        protected void ClassList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow selectedRow = ClassList.Rows[currentRowIndex];
            CurrentSelectedClass = Int32.Parse(ClassList.DataKeys[selectedRow.RowIndex].Values[0].ToString());
            UpdateTestList();
        }

        /*protected void ClassList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow selectedRow = AssignmentList.Rows[currentRowIndex];
            int assignmentID = Int32.Parse(AssignmentList.DataKeys[selectedRow.RowIndex].Values[0].ToString());
            CurrentSelectedClass = Convert.ToInt32(ClassList.SelectedRow.Cells[1].Text);
            UpdateTestList();
        }*/

        // Database procedure with parameters and fill a gridview
        private void UpdateTestList()
        {
            // Retrieve the connection string stored in the Web.config file.
            String connectionString = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            try
            {
                // Connect to the database and run the query.
                SqlConnection con = new SqlConnection(connectionString);
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Class_Tests", con))
                    {
                        //Assign the connection to the command
                        getcmd.Connection = con;
                        //Open a connection
                        try
                        {
                            con.Open();
                        }
                        catch (SqlException)
                        {
                            con.Close();
                        }
                        debug.InnerHtml = "";
                        // Assign the procedure and connection to an adapter
                        using (SqlDataAdapter da = new SqlDataAdapter("Get_Class_Tests", con))
                        {
                            int currentUser;
                            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
                            
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;
                            da.SelectCommand.Parameters.AddWithValue("@pClassID", CurrentSelectedClass);
                            da.SelectCommand.Parameters.AddWithValue("@pStudentID", currentUser);
                            DataSet ds = new DataSet();
                            da.Fill(ds, "Test");
                            AssignmentList.DataSource = ds.Tables["Test"];
                            AssignmentList.DataBind();
                            da.Dispose();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // The connection failed. Display an error message.
                debug.InnerHtml = "Unable to connect to the database.";
            }
        }

        protected void AssignmentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Gets the selected Assignment ID and then saves it to the session.
            //TODO: Add error checking to deny the user from taking it if the start or end date is out of scope.
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow selectedRow = AssignmentList.Rows[currentRowIndex];
            int assignmentID = Int32.Parse(AssignmentList.DataKeys[selectedRow.RowIndex].Values[0].ToString());
            HttpCookie questionAssignmentCookie = new HttpCookie("pAssignmentID");
            questionAssignmentCookie.Value = assignmentID.ToString();
            Response.Cookies.Add(questionAssignmentCookie);


            // Get Student ID and Assignment ID to start test
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Set_Test_Started"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTestID", assignmentID);
                    int currentUser;
                    int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
                    cmd.Parameters.AddWithValue("@pStudentID", currentUser);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }



            // Redirect to  test page
            Response.Redirect("~/Student/Testing/Test.aspx", true);
        }
    }
}
