using FasTest.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Admin
{
    public partial class ClassViewPage : System.Web.UI.Page
    {
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            #region AdminValidation;
            // Gets the user's id from the session
            int currentUser;
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);

            // If no username, return to Login page
            if (!parseUser)
            {
                Response.Redirect("~/Default.aspx");
            }

            Update_Instructor();
            Refresh_Instructor();

            // Sends the user id to the Admin validator and redirects to the correct page if invalid user
            AdminVal validAdmin = new AdminVal(currentUser);
            string page = validAdmin.VerifyAccess();
            if (page.Length > 0)
            {
                Response.Redirect(page);
            }
            #endregion;
            if (!IsPostBack)

            {

                this.BindGrid();

            }
            
            try
            {
                // Grab Class ID from gridview selection
                currentUser = Int32.Parse(HttpContext.Current.User.Identity.Name.ToString());
                //UserText.InnerHtml = "Hello" + currentUser;
            }
            catch (Exception)
            {
                //Return user to Login Page with error not logged in
            }
        }

        private void BindGrid()

        {

            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))

            {
                HttpCookie SelectedClassID = new HttpCookie("SelectedClassID");
               // Convert.ToInt32(Request.Cookies["SelectedClassID"].Value)
                using (SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT ClassStudent.StudentID, CONCAT(FasTestUser.FirstName , ' ' , FasTestUser.LastName) AS NAME, ClassStudent.IsEnrolled
                                FROM ClassStudent right join FasTestUser on FasTestUser.IDNumber = ClassStudent.StudentID 
                                     where (FasTestUser.CredentialLevel = 3) and (ClassStudent.ClassID = " + Convert.ToInt32(Request.Cookies["SelectedClassID"].Value) + ")"))
                {

                    using (SqlDataAdapter sda = new SqlDataAdapter())

                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            StudentList.DataSource = dt;
                            StudentList.DataBind();
                        }

                    }

                }

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            HttpCookie SelectedClassID = new HttpCookie("SelectedClassID");

            for (int i = 0; i < StudentList.Rows.Count; i++)
            {
                GridViewRow row = StudentList.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("chkEnrolled")).Checked;

                if (isChecked)
                {
                    // Column 1 is the ID column

                    string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Change_Enrollment"))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                            cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(StudentList.Rows[i].Cells[0].Text));
                            cmd.Parameters.AddWithValue("@pEnrollCode", true);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

                else if (!isChecked)
                {
                    string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Change_Enrollment"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                            cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(StudentList.Rows[i].Cells[0].Text));
                            cmd.Parameters.AddWithValue("@pEnrollCode", false);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            Response.Redirect("ClassViewPage.aspx");
        }

        protected void btnEnrollNewUsers_Click(object sender, EventArgs e)
        {
            HttpCookie SelectedClassID = new HttpCookie("SelectedClassID");

            for (int i = 0; i < EnrollNewGridview.Rows.Count; i++)
            {
                GridViewRow row = EnrollNewGridview.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("chkNewEnrolled")).Checked;

                if (isChecked)
                {
                    // Column 1 is the ID column

                    string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Change_Enrollment"))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                            cmd.Parameters.AddWithValue("@pStudentID", Convert.ToInt32(EnrollNewGridview.Rows[i].Cells[0].Text));
                            cmd.Parameters.AddWithValue("@pEnrollCode", true);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                

                
            }
            Response.Redirect("ClassViewPage.aspx");
        }

        protected void btnSetTeacher_Click(object sender, EventArgs e)
        {
            int InstructorID = Convert.ToInt32(ddlInstructorName.SelectedValue);
            // Update the Instructor based on the dropdown
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Update_Teacher"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                    cmd.Parameters.AddWithValue("@pInstructorID", InstructorID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            Update_Instructor();

        }

        protected void ddlInstructorName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Update_Instructor()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Class_Instructor"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                    cmd.Parameters.Add("@pInstructorName", SqlDbType.VarChar, 25).Direction = ParameterDirection.ReturnValue;

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    CourseInstructor.InnerHtml = "Instructor: " + reader.GetString(0);
                    
                    con.Close();
                }
            }
        }

        protected void btnUpdateClassTitle_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Update_Class_Title"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                    cmd.Parameters.AddWithValue("@pClassTitle", txtClassTitle.Text);
                    
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            Refresh_Instructor();
        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StudentList.PageIndex = e.NewPageIndex;
            BindGrid();

        }
        protected void Refresh_Instructor()
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Class_Title"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                    cmd.Parameters.Add("@ClassTitle", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if(!reader.IsDBNull(0))
                    ClassTitle.InnerHtml = reader.GetString(0);
                    // 
                    con.Close();
                }
            }
        }

        protected void btnUpdateGroup_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Update_Class_Group"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pClassID", Convert.ToInt32(Request.Cookies["SelectedClassID"].Value));
                    cmd.Parameters.AddWithValue("@pGroupValue", ddlGroupName.SelectedValue);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}