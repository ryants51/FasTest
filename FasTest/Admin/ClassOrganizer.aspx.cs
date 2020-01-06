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
    public partial class ClassOrganizer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnCreateClass_Click(object sender, EventArgs e)
        {
            string className = InputClassName.Text;
            string groupName = inputNewGroup.Text;
            InputValidation iv = new InputValidation();
            bool vClassName = iv.VAlphaNumSpace(className);
            bool vGroupName = iv.VAlpha(groupName);
            
            if (InputClassName.Text == string.Empty)
            {
                InvalidClassName.Text = "Please enter a valid class name";
            }
            else if (vClassName || vGroupName)
            {
                if (vGroupName)
                {
                    error1.Text = "a-Z only";
                }
                if (vClassName)
                {
                    InvalidClassName.Text = "a-Z, 0-9 and spaces only";
                }
               
            }
            else
            {
                string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Class"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pClassTitle", className);
                        if (groupName == String.Empty)
                        {
                            groupName = ddlGroupName.SelectedValue.ToString();
                        }
                        cmd.Parameters.AddWithValue("@pGroupName", groupName);
                        cmd.Parameters.AddWithValue("@pInstructorID", ddlInstructor.SelectedValue);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                Response.Redirect("ClassOrganizer.aspx");
            }
            
        }
        
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            HttpCookie SelectedClassID = new HttpCookie("SelectedClassID");
            HttpCookie SelectedClassTitle = new HttpCookie("SelectedClassTitle");
            SelectedClassID.Value = GridView2.SelectedRow.Cells[1].Text;
            SelectedClassTitle.Value = GridView2.SelectedRow.Cells[2].Text;
            Response.Cookies.Add(SelectedClassID);
            Response.Cookies.Add(SelectedClassTitle);
            Response.Redirect("ClassViewPage.aspx");
        }
    }
}