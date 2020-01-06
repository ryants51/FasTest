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

namespace FasTest.Classes
{
    public partial class ClassMainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Gets the user's id from the session
            int currentUser;
            bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);

            try
            {
                // Grab Class ID from gridview selection

                SqlDataSource2.SelectCommand += currentUser;
            }
            catch (Exception)
            {
                //Return user to Login Page with error not logged in
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            int classID = Convert.ToInt32(GridView1.SelectedValue.ToString());

            GetClassName.SelectCommand += classID;

            DataView dv = (DataView)GetClassName.Select(DataSourceSelectArguments.Empty);
            DataRowView drv = dv[0];
            
            ClassName.InnerHtml = drv["ClassTitle"].ToString();


            BindClassDetails(classID);





        }

        private void BindClassDetails(int classID)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (con)
                {
                    using (SqlCommand getcmd = new SqlCommand("Get_Class_Details", con))
                    {
                        
                        con.Open();
                        //Get details for the selected class
                        SqlDataAdapter da = new SqlDataAdapter(getcmd);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@pClassID", Convert.ToInt32(classID));
                        DataSet ds = new DataSet();
                        
                        da.Fill(ds, "Assignment");
                        ClassInformation.DataSource = ds.Tables["Assignment"];
                        ClassInformation.DataBind();
                        da.Dispose();
                        con.Close();
                    }
                }
            }
            ClassStudents.SelectParameters["pClassID"].DefaultValue = classID.ToString();
            ClassStudents.DataBind();
            StudentsEnrolledgrid.DataBind();
        }
    }
}