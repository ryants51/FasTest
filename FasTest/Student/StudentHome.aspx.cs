using FasTest.Validation;
using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace FasTest
{
    public partial class StudentHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Student_Stats"))
                {
                    // Pass the values entered to the database procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    int currentUser;
                    bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
                    cmd.Parameters.AddWithValue("@pStudentID", currentUser);
                    cmd.Connection = con;
                    con.Open();
                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                try
                                {


                                    while (reader.Read())
                                    {
                                        try
                                        {
                                            if (reader.GetInt32(0) == -99)
                                                highestScore.InnerHtml = "0";
                                            else
                                                highestScore.InnerHtml = Convert.ToString(reader.GetInt32(0) + "%");

                                            if (reader.GetInt32(1) == -99)
                                                overallAverage.InnerHtml = "0";
                                            else
                                                overallAverage.InnerHtml = Convert.ToString(reader.GetInt32(1) + "%");

                                            if (reader.GetInt32(2) == -99)
                                                availableTests.InnerHtml = "0";
                                            else
                                                availableTests.InnerHtml = Convert.ToString(reader.GetInt32(2));
                                        }
                                        catch
                                        {
                                            highestScore.InnerHtml = "0";
                                            overallAverage.InnerHtml = "0";
                                            availableTests.InnerHtml = "0";
                                        }
                                    }
                                }
                                catch (SqlException)
                                {

                                }
                            }
                        }
                    }
                    catch (SqlException)
                    { }

                }
                con.Close();
            }
        }
    }
}