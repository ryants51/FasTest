using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace FasTest.Teacher
{
    public partial class TeacherHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Teacher_Stats"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    int currentUser;
                    bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
                    cmd.Parameters.AddWithValue("@pTeacherID", currentUser);
                    cmd.Connection = con;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt32(0) == -99)
                                    assignedClasses.InnerHtml = "0";
                                else
                                    assignedClasses.InnerHtml = Convert.ToString(reader.GetInt32(0));

                                if (reader.GetString(1) == "-99")
                                    recentTest.InnerHtml = "0";
                                else
                                {
                                    recentTest.InnerHtml = Convert.ToString(reader.GetString(1) + "<br/> in <br/>" +
                                                                reader.GetString(2));
                                    
                                }


                                if (reader.GetInt32(3) == -99)
                                    timeSinceTestDays.InnerHtml = "0";
                                else
                                {
                                    timeSinceTestDays.InnerHtml =           Convert.ToString(reader.GetInt32(3) + " days ") +
                                                                  "<br/>" + Convert.ToString(reader.GetInt32(4) + " hours ") +
                                                                  "<br/>" + Convert.ToString(reader.GetInt32(5) + " minutes ");
                                }
                            }
                        }
                    }

                }
                con.Close();
            }
        }
    }
}