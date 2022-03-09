using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Money_Savings_Tool
{
   public class DatabaseConnection
    {
       public SqlConnection con;
        public SqlCommand command=null;
       
       public DatabaseConnection()   
        {
            con = new SqlConnection(@"Data Source=LAPTOP-S5PRSF9U\SQLEXPRESS;Initial Catalog='Money Savings Tool';Integrated Security=True");
            con.Open(); 
        }

        public void CloseConnection()
        {
            con.Close();
        }


    }
}
