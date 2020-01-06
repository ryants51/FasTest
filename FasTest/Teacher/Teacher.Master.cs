using FasTest.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Teacher
{
    public partial class Teacher : System.Web.UI.MasterPage
    {
        int currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region TeacherValidation;
            // Gets the user's id from the session
            
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);

            // If no username, return to Login page
            if (!parseUser)
            {
                Response.Redirect("~/Default.aspx");
            }

            // Sends the user id to the Admin validator and redirects to the correct page if invalid user
            TeacherVal validStdnt = new TeacherVal(Convert.ToInt32(currentUser));
            string page = validStdnt.VerifyAccess();
            if (page.Length > 0)
            {
                Response.Redirect(page);
            }
            #endregion;

            HttpCookie TeacherId = new HttpCookie("TeacherId");
            TeacherId.Value = currentUser.ToString();
            Response.Cookies.Add(TeacherId);

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
                    LoginNames.InnerHtml = reader.GetString(0);
                    // 
                    con.Close();
                }
            }
            
        }
        protected void btnLogoutOut_Click(object sender, EventArgs e)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Default.aspx");
            }
    }
}