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
    public partial class ReviewQuestions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int currentUser;
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Users_Name"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pUserID", currentUser);
                    cmd.Parameters.Add("@UsersName", SqlDbType.VarChar, 50).Direction = ParameterDirection.ReturnValue;

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    pledge.InnerHtml = reader.GetString(0);
                    con.Close();
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Student/Testing/Test.aspx", true);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int pledgeNum = 99;
            if (pledge.InnerHtml == tbxPledge.Text) { pledgeNum = 1; } else { pledgeNum = 0; } 
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Grade_Student_Test"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    int studentAssignment = Convert.ToInt32(Request.Cookies["pStudentAssignment"].Value);

                    cmd.Parameters.AddWithValue("@pPledgeSigned", pledgeNum);
                    cmd.Parameters.AddWithValue("@pStudentTestID", studentAssignment);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            Response.Redirect("~/Student/Testing/TestSubmitted.aspx", true);
        }
    }
}