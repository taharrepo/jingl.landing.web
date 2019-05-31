using System;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace JinglBatch
{
    class Program
    {
        public static string connectionstring = "Server=dbsophieparis.cmztxkrk8l6n.ap-southeast-1.rds.amazonaws.com;Database=JINGLDB;Persist Security Info=True;User ID=jingluser;Password=jingluser_p4ss;MultipleActiveResultSets=True;";
        static void Main(string[] args)
        {
            EmailReminder();            
        }
        
        private static void EmailReminder()
        {
            DataTable datas = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionstring))
            {               
                SqlDataAdapter adp = new SqlDataAdapter("select [name], Email, emailreminder, reminderdate from LandingRegistration where FileURL is null or LEN(FileURL) = 0", con);
                adp.SelectCommand.CommandTimeout = 900;               
                adp.Fill(datas);
            }

            for (int i = 0; i < datas.Rows.Count; i++)
            {
                //check last reminder date & belum pernah dikirim email reminder
                if (datas.Rows[i]["reminderdate"].ToString().Length < 3)
                {
                    Helper.SendEmail(datas.Rows[i]["Email"].ToString(), datas.Rows[i]["name"].ToString(),"");
                    UpdateDB(datas.Rows[i]["Email"].ToString(), 1);
                    Console.WriteLine("Sending Email to "+ datas.Rows[i]["name"].ToString()+" done !!");
                } else
                {
                    DateTime dtemail = DateTime.Parse(datas.Rows[i]["reminderdate"].ToString());
                    var counter = int.Parse(datas.Rows[i]["emailreminder"].ToString())+1;
                    var diffday = (DateTime.Now - dtemail).Days;
                    if (diffday > 3)
                    {
                        Helper.SendEmail(datas.Rows[i]["Email"].ToString(), datas.Rows[i]["name"].ToString(), "");
                        UpdateDB(datas.Rows[i]["Email"].ToString(), counter);
                    }

                }
            }
        }

        private static void UpdateDB(string email, int emailnumber)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Update LandingRegistration set emailreminder = '"+emailnumber+"', reminderdate = GETDATE() where Email = '"+email+"' ", con);
                com.ExecuteNonQuery();
                con.Close();

            }
        }
    }
}
