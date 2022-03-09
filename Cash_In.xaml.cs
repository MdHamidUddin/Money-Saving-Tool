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
using System.Data.SqlClient;
using System.Data;

namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Cash_In.xaml
    /// </summary>
    public partial class Cash_In : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        bool check;
        string EMAIL;
        public static string GetBalance, Balance;
        double Previous_Balance = 0, New_Balance, Deposit_Balance;
        public Cash_In(string Email)
        {
            EMAIL = Email;
           // EMAIL = "hamiduddin280@gmail.com";
            InitializeComponent();
            Get_Balance();
            label_balance.Content = "Current Balance:" + Balance;
        }

        private void Cash_In_Button(object sender, RoutedEventArgs e)
        {
            String date = System.DateTime.Now.ToString("dd.MM.yyyy");
            string Amount = amount.Text;
            string Note = note.Text;
           if( CheckTextBox()==true)
            {

                    int i = Deposit(EMAIL, Amount, Note, null, date);
                    if (i == 1)
                    {
                        MessageBox.Show("Amount Deposited Successfully");

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
                            check = false;
                            Console.WriteLine("Couldnt find");
                        }


                        Console.WriteLine("Current Balance " + GetBalance);
                        Previous_Balance = Convert.ToDouble(GetBalance);//this amount is read from database,which is actually previous balance
                        Deposit_Balance = Convert.ToDouble(Amount);//This is newly deposited balance
                        New_Balance = Previous_Balance + Deposit_Balance;

                        try
                        {

                            if (check.Equals(true))
                            {
                                string sql2 = "update curr_balance set Balance = '" + New_Balance.ToString() + "', Updated = '" + date + "' where Email like '" + EMAIL + "' ";
                                command = new SqlCommand(sql2, db.con);
                                int result = command.ExecuteNonQuery();
                            }

                            else if (check.Equals(false))
                            {
                                string sql2 = "INSERT INTO curr_balance(Email,Balance,Updated) VALUES('" + EMAIL + "','" + Deposit_Balance.ToString() + "','" + date + "')";
                                // string sql2 = "update curr_balance set Balance = '" +Deposit_Balance.ToString() + "', Updated = '" + date + "' where Email like '" + EMAIL + "' ";
                                command = new SqlCommand(sql2, db.con);
                                int result = command.ExecuteNonQuery();
                                Console.WriteLine("New Balance Updated !!");
                            }
                        }
                        catch (Exception)
                        {

                        }

                        db.CloseConnection();
                        Homepage f1 = new Homepage(EMAIL);

                        this.Hide();
                        f1.Show();
                    }

             
            }


        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Homepage h = new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }

        void Get_Balance()
        {
            string sql = "select * from [curr_balance] where Email like '" + EMAIL + "' ";
            db.command = new SqlCommand(sql, db.con);
            DataTable dt = new DataTable();

            dt.Load(db.command.ExecuteReader());

            try
            {
                Balance = dt.Rows[0]["Balance"].ToString();
                check = true;
                Console.WriteLine("Balance mane taka: " + Balance);

            }

            catch (Exception)
            {
                Balance = "0";
                Console.WriteLine("Couldnt find");
            }

        }


        public int Deposit(params string[] inputs )
        {
            string sql = "INSERT INTO User_Data(Email,Deposited,Note,Withdraw,Date) VALUES('" + inputs[0] + "','" + inputs[1] + "','" + inputs[2] + "','" + inputs[3] + "','" + inputs[4] + "')";
            db.command = new SqlCommand(sql, db.con);
            int result = db.command.ExecuteNonQuery();
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

        
    }
}
