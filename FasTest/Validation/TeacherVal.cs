﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FasTest.Validation
{
    public class TeacherVal
    {
        int UserId;

        public TeacherVal(int usrId)
        {
            UserId = usrId;
        }

        public string VerifyAccess()
        {
            int CredLvl;
            string constr = ConfigurationManager.ConnectionStrings["CS414_FasTestConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Credential_Level"))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", UserId);
                        cmd.Connection = con;
                        con.Open();
                        CredLvl = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                    finally
                    {
                        con.Close();
                    }
                }

                switch (CredLvl)
                {
                    case 1:
                        return "~/./Admin/AdminHome.aspx";
                    case 2:
                        return String.Empty;
                    case 3:
                        return "~/./Student/StudentHome.aspx";
                    default:
                        return "~/./Default.aspx";
                }
            }
        }
    }
}