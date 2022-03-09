using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Cash_Out.xaml
    /// </summary>
    public partial class Expense : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        string EMAIL;
        public static string GetBalance, Balance;
        double Previous_Balance = 0, New_Balance, Credit_Balance;
        bool check = false;

        public Expense(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
            Get_Balance();
            label_balance.Content = "Current Balance : " + Balance;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Homepage f1 = new Homepage(EMAIL);
            this.Hide();
            f1.Show();
        }

        void Get_Balance()
        {
            string sql = "select * from curr_balance where Email like '" + EMAIL + "' ";

            try
            {
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                Balance = dt.Rows[0]["Balance"].ToString();
                Console.WriteLine("Balance mane taka: " + Balance);

            }

            catch (Exception)
            {
                Balance = "0";
                Console.WriteLine("Couldnt find");
            }

        }

        int Expense_Money(params string[] inputs)
        {
            int result = 0;
            string sql = "INSERT INTO [User_Data] (Email,Deposited,Note,Withdraw,Date) VALUES('" + inputs[0] + "','" + inputs[1] + "','" + inputs[2] + "','" + inputs[3] + "','" + inputs[4] + "')";
            db.command = new SqlCommand(sql, db.con);
            result = Convert.ToInt32(db.command.ExecuteNonQuery());
            return result;
        }

        bool CheckTextBox()
        {
            bool check = false;
            if (amount.Text == "")
            {
                BlankAmount.Content = "Amount Required !!";
            }
            else
            {
                BlankAmount.Content = "";
            }
            if (note.Text == "")
            {
                BlankNote.Content = "Note Required !!";

            }
            else
            {
                BlankNote.Content = "";
            }

            if (amount.Text != "" && note.Text != "")
            {
                check = true;
            }

            return check;
        }


        private void Expense_Button(object sender, RoutedEventArgs e)
        {
            String date = System.DateTime.Now.ToString("dd.MM.yyyy");
            string Amount = amount.Text;
            string Note = note.Text;

            if (CheckTextBox() == true)
            {

                int i = Expense_Money(EMAIL, null, Note, Amount, date);
                if (i == 1)
                {


                    ///here we are trying to read previous balance from the database
                    ///then we will add this with current deposit balance
                    ///So this will be the current balance
                    ///

                    string sql = "select * from curr_balance where Email like '" + EMAIL + "' ";
                    SqlCommand command = new SqlCommand(sql, db.con);
                    DataTable dt = new DataTable();

                    dt.Load(command.ExecuteReader());

                    try
                    {
                        GetBalance = dt.Rows[0]["Balance"].ToString();
                        check = true;

                    }

                    catch (Exception)
                    {
                        Console.WriteLine("Couldnt find");
                    }


                    Console.WriteLine("Current Balance " + GetBalance);
                    Credit_Balance = Convert.ToDouble(Amount);//This is newly Credited balance
                    Previous_Balance = Convert.ToDouble(GetBalance);//this amount is read from database,which is actually previous balance
                    if (Previous_Balance >= Credit_Balance)
                    {

                        New_Balance = Previous_Balance - Credit_Balance;
                        try
                        {

                            if (check.Equals(true))
                            {
                                string sql2 = "update curr_balance set Balance = '" + New_Balance.ToString() + "', Updated = '" + date + "' where Email like '" + EMAIL + "' ";
                                command = new SqlCommand(sql2, db.con);
                                int result = command.ExecuteNonQuery();
                                MessageBox.Show("Amount Credited!!");
                            }

                            else if (check.Equals(false))
                            {
                                MessageBox.Show("Insufficient Amount ");
                            }
                        }
                        catch (Exception E)
                        {
                            MessageBox.Show(E.Message);
                        }

                        db.CloseConnection();
                        Homepage f1 = new Homepage(EMAIL);
                        this.Hide();
                        f1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Insufficient Balance ");
                    }


                }

            }
        }
    }
}
