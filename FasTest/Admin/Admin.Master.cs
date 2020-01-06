using FasTest.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            #region AdminValidation;
            // Gets the user's ID and Name from the session
            int currentUser;
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
            
            // If no username, return to Login page
            if (!parseUser)
            {
                Response.Redirect("~/Default.aspx");
            }
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
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
                    con.Close();
                }
            }
            // Sends the user id to the Admin validator and redirects to the correct page if invalid user
            AdminVal validAdmin = new AdminVal(currentUser);
            string page = validAdmin.VerifyAccess();
            if (page.Length > 0)
            {
                Response.Redirect(page);
            }
            #endregion;
        }

        protected void btnLogoutOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}