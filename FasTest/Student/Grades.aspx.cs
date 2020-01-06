using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Student
{
    public partial class Grades : System.Web.UI.Page
    {
        int currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool parseUser = int.TryParse(HttpContext.Current.User.Identity.Name.ToString(), out currentUser);
                if (parseUser)
                {
                    getAllGrades.SelectParameters["pStudentID"].DefaultValue = currentUser.ToString();
                }
            }


        }

        protected void rptGrades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                    ((SqlDataSource)e.Item.FindControl("getClassGrades")).SelectParameters["pStudentID"].DefaultValue = currentUser.ToString();
            }

        }
    }
}