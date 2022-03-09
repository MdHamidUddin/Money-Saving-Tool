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
    /// Interaction logic for Change_Password.xaml
    /// </summary>
    public partial class Change_Password : Window
    {
        string EMAIL, password;
        SqlCommand command;
        DatabaseConnection db = new DatabaseConnection();
        public Change_Password(string email)
        {
            EMAIL = email;
            InitializeComponent();
        }


        void GetPassword()
        {
            string sql = "select * from Registration where Email like '" + EMAIL + "' ";
            SqlCommand command = new SqlCommand(sql, db.con);
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());

            try
            {
                password = dt.Rows[0]["Password"].ToString();
                // check = true;
                Console.WriteLine("Balance mane taka: " + password);

            }

            catch (Exception)
            {
                Console.WriteLine("Old Password does not match!!!");
            }

        }

        Boolean CheckPassWordBox()
        {
            Boolean check = false;
            string Old_Password = old_password.Password;
            string New_Password = new_password.Password;
            string Confirm_New_Password = confirm_new_password.Password;

            if (Old_Password == "")
            {
                Blank_Old_Password.Content = "Old Password Required !!";
            }
            else
            {
                Blank_Old_Password.Content = "";
            }

            if (New_Password == "")
            {
                Blank_New_Password.Content = "New Password Required !!";
            }
            else
            {
                Blank_New_Password.Content = "";
            }
            if (Confirm_New_Password == "")
            {
                Blank_New_Confirm_Password.Content = "Confirm New Password Required !!";
            }
            else
            {
                Blank_New_Confirm_Password.Content = "";
            }
            if (Old_Password != "" && New_Password != "" && Confirm_New_Password!="")
            {
                if (New_Password .Equals(Confirm_New_Password))
                {
                    check = true;
                }
                else
                {
                    check = false;
                    MessageBox.Show("Password and Confirm password Doesn't match !! ");
                }

            }
            return check;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            All_Menu.Settings s = new All_Menu.Settings(EMAIL);
            this.Hide();
            s.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

              if (CheckPassWordBox()==true)
                {

                    try
                    {
                        string sql2 = "update [Registration] set Password  = '" + new_password.Password + "' where Email like '" + EMAIL + "'";
                        command = new SqlCommand(sql2, db.con);
                        int result = command.ExecuteNonQuery();

                    if((MyCheckBox.IsChecked== true))
                    {
                        Login l = new Login();
                        this.Hide();
                        l.Show();
                    }
                    else
                    {
                        Homepage h = new Homepage(EMAIL);
                        this.Hide();
                        h.Show();
                    }
           
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(e);
                    }

                }
            //else
            //{
            //    MessageBox.Show("Old Password Doesn't match !! ");
            //}

          
        }
    }
}
