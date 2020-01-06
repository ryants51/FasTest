using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FasTest.Student.Testing
{
	public partial class TestSubmitted : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnReturnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Student/StudentHome.aspx", true);
        }
    }
}