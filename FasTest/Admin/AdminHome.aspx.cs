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

namespace FasTest.Admin
{
    public partial class AdminHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Admin_Stats"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    

                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt32(0) == -99)
                                    totUsers.InnerHtml = "0";
                                else
                                    totUsers.InnerHtml = Convert.ToString(reader.GetInt32(0));
                                if (reader.GetInt32(1) == -99)
                                    totClasses.InnerHtml = "0";
                                else
                                    totClasses.InnerHtml = Convert.ToString(reader.GetInt32(1));
                                if (reader.GetInt32(2) == -99)
                                    totTests.InnerHtml = "0";
                                else
                                    totTests.InnerHtml = Convert.ToString(reader.GetInt32(2));

                            }
                        }
                    }

                }
                con.Close();
            }
        }
    }
}