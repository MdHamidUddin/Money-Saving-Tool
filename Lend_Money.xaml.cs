

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
    /// Interaction logic for Lend_Money.xaml
    /// </summary>
    public partial class Lend_Money : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        String date = System.DateTime.Now.ToString("dd.MM.yyyy");
        String EMAIL, Phone2;
        string GetPhone1, GetPhone2, Priv_Balance;
        bool check = true;
        string Validation;
        SqlCommand command;

        public Lend_Money(string Email)//
        {
            EMAIL = Email;
            InitializeComponent();
            Date_Time.Content = date;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Due_Balance d = new Due_Balance(EMAIL);
            this.Hide();
            d.Show();
        }

        private void Update_Event_Button(object sender, RoutedEventArgs e)
        {
            Update_Lend_Money update = new Update_Lend_Money(EMAIL);
            this.Hide();
            update.Show();
        }


        bool CheckTextBox()
        {
            bool check = false;
            string Name = name.Text;
            string Amount = amount.Text;
            string Note = note.Text;
            string Phone1 = phone1.Text;

            if (Name.Equals(string.Empty))
            {
                BlankName.Content = "Name Required !! ";
            }
            else
            {
                BlankName.Content = "";
            }

            if (Amount.Equals(string.Empty))
            {
                BlankAmount.Content = "Amount Required !! ";
            }
            else
            {
                BlankAmount.Content = "";
            }
            if (Note.Equals(string.Empty))
            {
                BlankNote.Content = "Short Note Required !!";
            }
            else
            {
                BlankNote.Content = "";
            }
            if (Phone1.Equals(string.Empty))
            {
                BlankPhone.Content = "Phone Number Required !!";
            }
            else
            {
                BlankPhone.Content = "";
            }

            if (Name != "" && Amount != "" && Note != "" && Phone1 != "")
            {
                check = true;
            }
            return check;


        }
        void New_Event()
        {
            string Name, Amount1, Note, Phone1;
            Name = name.Text;
            Amount1 = amount.Text;
            Note = note.Text;
            Phone1 = phone1.Text;
            double Amount = Convert.ToDouble(Amount1);

            try
            {
                string sql2 = "INSERT INTO [Due_Balance] (Email,Name,Phone,Lend_Money,Lend_Note,Borrow_Money,Borrow_Note,Date,Validity) VALUES('" + EMAIL + "','" + Name + "','" + Phone1 + "','" + Amount + "','" + Note + "','" + "0" + "','" + null + "','" + date + "','" + "True" + "')";
                command = new SqlCommand(sql2, db.con);
                int result = command.ExecuteNonQuery();
                Console.WriteLine("New Balance Created !!");
                MessageBox.Show("New Balance Created !!");

                Due_Balance f = new Due_Balance(EMAIL);
                this.Hide();
                f.Show();
            }
            catch (Exception)
            {

            }

        }


        Boolean Available_Money()
        {
            //DatabaseConnection db = new DatabaseConnection();
            Boolean check = false;
            string Curr_Balance;
            string sql = "select * from curr_balance where Email like '" + EMAIL + "'";
            SqlCommand command = new SqlCommand(sql, db.con);
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());

            try
            {
                Curr_Balance = dt.Rows[0]["Balance"].ToString();

            }

            catch (Exception)
            {
                Curr_Balance = "0";

            }

            int Balance = Convert.ToInt32(Curr_Balance);
            string Amount1 = amount.Text;
            int Lend_Balance = Convert.ToInt32(Amount1);

            if (Lend_Balance <= Balance)
            {
                //CUrrent balance change hobe
                Balance = Balance - Lend_Balance;
                Change_Current_Balance(Balance);
                check = true;
            }

            else
            {
                MessageBox.Show("Insufficient Balance to Lend");
            }
            return check;

        }

        void Change_Current_Balance(int New_Balance)
        {
            try
            {
                string Sql = "update curr_balance set Balance='" + New_Balance.ToString() + "' where Email like '" + EMAIL + "' ";
                SqlCommand command = new SqlCommand(Sql, db.con);
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                check = true;

            }
            catch (Exception)
            {

            }
        }




        private void Create_Event_Button(object sender, RoutedEventArgs e)
        {
            DatabaseConnection db = new DatabaseConnection();
            if (CheckTextBox() == true)
            {
                //
                string Name, Amount1, Note, Phone1;
                Name = name.Text;
                Amount1 = amount.Text;
                Note = note.Text;
                Phone1 = phone1.Text;
                double Amount = Convert.ToDouble(Amount1);

                if (Available_Money() == true)
                {
                    string sql = "select * from Due_Balance where Phone like '" + Phone1 + "' and Email like '" + EMAIL + "'";
                    SqlCommand command = new SqlCommand(sql, db.con);
                    DataTable dt = new DataTable();

                    dt.Load(command.ExecuteReader());

                    try
                    {
                        Priv_Balance = dt.Rows[0]["Lend_Money"].ToString();
                        Validation = dt.Rows[0]["Validity"].ToString();
                        check = true;

                    }

                    catch (Exception)
                    {
                        Priv_Balance = "0";
                        Console.WriteLine("Couldnt find");
                        check = false;
                    }


                    if (check == true)
                    {

                        if (Validation == "False")
                        {
                            New_Event();
                        }
                        else
                        {
                            MessageBox.Show("Already have an CUrrent event with this phone number \n You can Update or Delete previous event");
                        }
                    }


                    else
                    {
                        New_Event();
                    }

                }
            }
        }
    }
}


