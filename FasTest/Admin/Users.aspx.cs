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
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (errorDeleteUser.Visible == true)
                errorDeleteUser.Visible = false;
        }

        protected void btnAddNewUser_Click(object sender, EventArgs e)
        {

            string pass = tbPassword.Text;
            string fName = tbFirstName.Text;
            string lName = tbLastName.Text;
            InputValidation iv = new InputValidation();
            bool vPass = iv.VAlphaNum(pass);
            bool vfName = iv.VAlpha(fName);
            bool vlName = iv.VAlpha(lName);


            if (pass == String.Empty || fName == String.Empty || lName == String.Empty || pass.Length < 6)
            {
                if (pass == String.Empty || fName == String.Empty || lName == String.Empty)
                {
                    error1.Text = "All fields are required";
                }
                else if (pass.Length < 6)
                {
                    error3.Text = "Password must be at least 6 characters";
                }
            }
            else if (!vPass || vfName || vlName)
            {
                if (vPass)
                {
                    error3.Text = string.Empty;
                }
                else if (!vPass)
                {
                    error3.Text = "a-Z and 0-9 only";
                }
                if (vfName)
                {
                    error1.Text = "Names can only be alphabetic";
                }
                if (vlName)
                {
                    error1.Text = "Names can only be alphabetic";
                }

            }
            else if (vPass && !vlName && !vfName)
            {
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_User"))
                    {
                        // Pass the values entered to the database procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pPassword", CryptoService.ComputePasswordHash(pass));
                        cmd.Parameters.AddWithValue("@pSalt", CryptoService.GenerateSalt());
                        int credentialLevel = Convert.ToInt32(sctCredentialLevel.SelectedValue);
                        cmd.Parameters.AddWithValue("@pCredentialLevel", credentialLevel);
                        cmd.Parameters.AddWithValue("@pFirstName", fName);
                        cmd.Parameters.AddWithValue("@pLastName", lName);

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                error1.Text = String.Empty;
                Response.Redirect("Users.aspx");
            }
            else
            {
                error1.Text = "Alpha numeric characters only";
            }

        }

        protected void changePasswordButton_Click(object sender, EventArgs e)
        {
            string pass = enterNewPasswordTB.Text;
            // This is where the database password change would be made
            InputValidation iv = new InputValidation();
            bool vPass = iv.VAlphaNum(pass);

            if (pass != confirmNewPasswordTB.Text)
            {
                errorPassU1.Text = "The passwords do not match";
            }
            else if (pass == String.Empty)
            {
                errorPassU1.Text = "All fields are required";
            }
            else if (pass.Length < 6)
            {
                errorPassU1.Text = "Password must be at least 6 characters";
            }
            else if (!vPass)
            {
                errorPassU1.Text = "a-Z and 0-9 only";
            }
            else if (pass.Length > 6 && vPass && pass == confirmNewPasswordTB.Text)
            {
                ShowNewUser();
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_Password"))
                    {
                        // Pass the values entered to the database procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pUserID", ViewState["userID"]);
                        cmd.Parameters.AddWithValue("@pSalt", CryptoService.GenerateSalt());
                        cmd.Parameters.AddWithValue("@pNewPassword", CryptoService.ComputePasswordHash(pass));

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                errorPassU1.Text = string.Empty;
                ShowNewUser();
            }
            
            
        }

        protected void ShowUpdatePassword()
        {
            NewUserInput.Visible = false;
            changePasswordSection.Visible = true;
            confirmNewPassword.Text = "Confirm new password";
            tbFirstName.Text = tbLastName.Text = tbPassword.Text = String.Empty;
        }
        protected void ShowNewUser()
        {
            newUser.InnerHtml = "New User";
            enterNewPasswordTB.Text = confirmNewPassword.Text = tbFirstName.Text = tbLastName.Text = String.Empty;
            NewUserInput.Visible = btnAddNewUser.Visible = CredentialLevel.Visible = sctCredentialLevel.Visible = tbPassword.Visible = Password.Visible = true;
            changePasswordSection.Visible = btnCancelEdit.Visible = btnSaveUser.Visible = false;
        }
        protected void showEditUser()
        {
            newUser.InnerHtml = "Edit User";
            tbFirstName.Text = ViewState["FirstName"].ToString();
            tbLastName.Text = ViewState["LastName"].ToString();
            changePasswordSection.Visible = btnAddNewUser.Visible = CredentialLevel.Visible = sctCredentialLevel.Visible = tbPassword.Visible = Password.Visible = false;
            NewUserInput.Visible = btnCancelEdit.Visible = btnSaveUser.Visible = true;
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if(e.CommandName == "Sort")
            {

            }
            else
            {
                int currentRowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow selectedRow = GridView1.Rows[currentRowIndex];
                ViewState["FirstName"] = GridView1.DataKeys[selectedRow.RowIndex].Values[1].ToString();
                ViewState["LastName"] = GridView1.DataKeys[selectedRow.RowIndex].Values[2].ToString();
                ViewState["WholeName"] = ViewState["FirstName"].ToString() + " " + ViewState["LastName"].ToString();
                ViewState["userID"] = Int32.Parse(GridView1.DataKeys[selectedRow.RowIndex].Values[0].ToString());

                if (e.CommandName == "UpdatePassword")
                {
                    ChangePasswordName.InnerHtml = ViewState["WholeName"].ToString();
                    ShowUpdatePassword();
                }
                else if (e.CommandName == "DeleteUser")
                { 
                    ViewState["userID"] = Int32.Parse(GridView1.DataKeys[selectedRow.RowIndex].Values[0].ToString());
                    userName.InnerHtml = ViewState["WholeName"].ToString();
                    mp1.Show();
                    ShowNewUser();
                }
                else if (e.CommandName == "EditUser")
                {
                    showEditUser();              
                }

            }
        }

        protected void btnConfirmDelteUser_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Delete_User"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pUserID", Convert.ToInt32(ViewState["userID"]));

                    cmd.Connection = con;
                    con.Open();
                    try { 
                    cmd.ExecuteNonQuery();
                    con.Close();
                    } catch (SqlException)
                    {
                        errorDeleteUser.Visible = true;
                    }
                }
            }
            GridView1.DataBind();
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        { string pass = tbPassword.Text;
            string fName = tbFirstName.Text;
            string lName = tbLastName.Text;
            InputValidation iv = new InputValidation();
            bool vPass = iv.VAlphaNum(pass);
            bool vfName = iv.VAlpha(fName);
            bool vlName = iv.VAlpha(lName);


            if (fName == String.Empty || lName == String.Empty )
            {
                error1.Text = "All fields are required";                
            }
            else if (vfName || vlName)
            {     
                    error1.Text = "Names can only be alphabetic";
            }
            else if (!vlName && !vfName)
            {
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Update_Users_Name"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pUserID", Convert.ToInt32(ViewState["userID"]));
                        cmd.Parameters.AddWithValue("@pUserFirstName", tbFirstName.Text);
                        cmd.Parameters.AddWithValue("@pUserLastName", tbLastName.Text);

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                newUser.InnerHtml = "New User";
                tbFirstName.Text = String.Empty;
                tbLastName.Text = String.Empty;
                error1.Text = String.Empty;
                btnAddNewUser.Visible = CredentialLevel.Visible = sctCredentialLevel.Visible = tbPassword.Visible = Password.Visible = true;
                btnCancelEdit.Visible = btnSaveUser.Visible = false;
                ShowNewUser();
                GridView1.DataBind();
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            
            ShowNewUser();
            GridView1.DataBind();
        }

        protected void btnCancleUpdatePassword_Click(object sender, EventArgs e)
        {
            ShowNewUser();
        }
        
    }
}
