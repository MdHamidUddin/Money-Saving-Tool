using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;


namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Forgot_Password.xaml
    /// </summary>
    public partial class Forgot_Password : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        int check = 0;
        string GetBalance="";
        public Forgot_Password()
        {
            InitializeComponent();
        }

        bool CheckTextBox()
        {
            bool check = false;

            string Name = name.Text.ToUpper();
            string Email = email.Text.ToUpper();
            string Last_Current_Balance = balance.Text.ToUpper();

            if(Name.Equals(string.Empty))
            {
                label_Name.Content = "Name Requred !!";
            }
            else
            {
                label_Name.Content = "";
            }

            if(Email.Equals(string.Empty))
            {
                label_Email.Content = "Email Requred !!";
            }
            else
            {
                label_Email.Content = "";
            }

            if(Last_Current_Balance.Equals(string.Empty))
            {
                label_Last_Balance.Content = "Last Current Balance required !";
            }
            else
            {
                label_Last_Balance.Content = "";
            }

            if(Name!="" && Email!="" && Last_Current_Balance!="")
            {
                check = true;
            }
            return check;
        }

        int Check_Name_Email()
        {
            int check = 0;
            string Email = email.Text.ToUpper();
            string Name = name.Text.ToUpper();
            try
            {
                string sql = "SELECT  Count(*) FROM [Registration] WHERE Email = '" + Email + "' and Name='" + Name + "' ";
                SqlCommand command = new SqlCommand(sql, db.con);
                check = Convert.ToInt32(command.ExecuteScalar());

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return check;
        }

        double Last_Balance()
        {
            string EMAIL = email.Text.ToUpper();
            double check = 0;
            string sql = "select * from curr_balance where Email like '" + EMAIL + "' ";
           

            try
            {
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                GetBalance = dt.Rows[0]["Balance"].ToString();
            }

            catch (Exception)
            {
                GetBalance = "0";
            }

            check = Convert.ToDouble(GetBalance);
            return check;
        }


        string Password()
        {
            string check = "";
            string EMAIL = email.Text.ToUpper();

            string sql = "select * from [Registration] where Email like '" + EMAIL + "' ";
            try
            {
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                check = dt.Rows[0]["Password"].ToString();
            }

            catch (Exception)
            {
                check = "0";
            }

            return check;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (CheckTextBox()==true)
            {
                string Name = name.Text.ToUpper();
                string Email = email.Text.ToUpper();
                string Last_Current_Balance = balance.Text;
                double last_balance = Convert.ToDouble(Last_Current_Balance);

                if (Check_Name_Email()==1)
                {
                    if(Last_Balance()==last_balance)
                    {
                        MessageBox.Show("You have successfully completed Security check\nThis is Your Password\n"+Password());
                        Login l = new Login();
                        this.Hide();
                        l.Show();
                    }
                    else
                    {
                        label_Last_Balance.Content = "Last Balance Doesn't match !";
                    }
                }
                else
                {
                    label_Email.Content = "Name or Email Doesn't match !!";
                    label_Name.Content = "Name or Email Doesn't match !!";
                }

                
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.Hide();
            l.Show();
        }
    }
}
