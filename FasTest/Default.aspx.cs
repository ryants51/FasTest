using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using FasTest.Validation;

namespace FasTest
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ValidateUser(object sender, AuthenticateEventArgs e)
        {


            int userLvl = 0;
            string Password = Convert.ToString(Login1.Password);
            string HashedPassword = null;
            string Salt = null;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Validate_Password", con))
                //using (SqlCommand cmd = new SqlCommand("Validate_User"))
                {
                    try
                    {
                        try
                        {
                            int UserName = Convert.ToInt32(Login1.UserName);
                       

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pUserID", UserName);
                        cmd.Parameters.Add("@pPassword", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@pSalt", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CredentialLevel", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        HashedPassword = cmd.Parameters["@pPassword"].Value.ToString();
                        Salt = cmd.Parameters["@pSalt"].Value.ToString();
                        }
                        catch
                        {
                            Login1.FailureText = "Username and/or password is incorrect.";
                        }
                        try
                        {
                            userLvl = Convert.ToInt32(cmd.Parameters["@CredentialLevel"].Value);
                        }
                        catch
                        {
                            Login1.FailureText = "Username and/or password is incorrect.";
                        }
                    }
                    catch(SqlException ex)
                    {
                       Login1.FailureText = "Username and/or password is incorrect. " + ex.ToString();
                    }
                    finally
                    {
                        con.Close();
                    }

                    /*try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", Convert.ToInt32(Login1.UserName));
                        cmd.Parameters.AddWithValue("@Password", Convert.ToString(Login1.Password));
                        cmd.Connection = con;
                        con.Open();
                        userLvl = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch
                    {
                        Login1.FailureText = "Username and/or password is incorrect.";
                    }
                    finally
                    {
                       con.Close();
                    }*/

                }

                if (CryptoService.ValidateHash(HashedPassword, Password, Salt))
                {
                    switch (userLvl)
                    {
                        //case -1:
                        // Login1.FailureText = "Username and/or password is incorrect.";
                        // break;
                        case 1:
                            FormsAuthentication.SetAuthCookie(Login1.UserName, Login1.RememberMeSet);
                            Response.Redirect("~/Admin/AdminHome.aspx");
                            break;
                        case 2:
                            FormsAuthentication.SetAuthCookie(Login1.UserName, Login1.RememberMeSet);
                            Response.Redirect("~/Teacher/TeacherHome.aspx");
                            break;
                        case 3:
                            FormsAuthentication.SetAuthCookie(Login1.UserName, Login1.RememberMeSet);
                            Response.Redirect("~/Student/StudentHome.aspx");
                            break;
                    }
                }
                //else
                    //Login1.FailureText = "Username and/or password is incorrect.";
            }
        }
    }
}
