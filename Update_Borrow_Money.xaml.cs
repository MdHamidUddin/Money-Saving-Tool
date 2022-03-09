using System;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Update_Borrow_Money.xaml
    /// </summary>
    public partial class Update_Borrow_Money : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        String date = DateTime.Now.ToString("dd.MM.yyyy");
        String EMAIL, Phone2;
        string Priv_Balance;
        bool check = false;
        public Update_Borrow_Money(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
            ShowAllEvent();
        }
        void ShowAllEvent()
        {
            string sql = "select Name,Phone,Lend_Money,Borrow_Money from Due_Balance where Email like'" + EMAIL + "' and Validity like '" + "True" + "' ";
            SqlDataAdapter sqlDA = new SqlDataAdapter(sql, db.con);
            DataTable dt = new DataTable();
            sqlDA.Fill(dt);
            gridView.ItemsSource = dt.DefaultView;
        }
        Boolean Change_Borrow_Money_Take()
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

            //CUrrent balance change hobe
            Balance = Balance + Lend_Balance;
            Change_Current_Balance(Balance);
            check = true;



            return check;

        }

        Boolean Change_Borrow_Money_Give()
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

            //CUrrent balance change hobe
            Balance = Balance - Lend_Balance;
            Change_Current_Balance(Balance);
            check = true;



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

        double Current_Borrow_Money()
        {
            string phone = phone2.Text;
            double balance = 0;


            string query = "select * from Due_Balance where Validity like '" + "True" + "' and Phone like '" + phone + "'";
            SqlCommand command = new SqlCommand(query, db.con);
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());

            try
            {
                string lm = dt.Rows[0]["Borrow_Money"].ToString();
                balance = Convert.ToDouble(lm);

            }


            catch (Exception E)
            {
                MessageBox.Show(E.Message);
                // MessageBox.Show("Phone Number Not Found!!");
            }
            return balance;

        }

        double MyCurrent_Balance()
        {

            string Curr_Balance;
            double Balance = 0;
            string sql = "select * from curr_balance where Email like '" + EMAIL + "'";
            SqlCommand command = new SqlCommand(sql, db.con);
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());

            try
            {
                Curr_Balance = dt.Rows[0]["Balance"].ToString();
                Balance = Convert.ToDouble(Curr_Balance);

            }

            catch (Exception)
            {
                Curr_Balance = "0";

            }
            return Balance;
        }

        void Auto_Get_Back_Lend_Money()
        {
            double New_Curr_Balance = MyCurrent_Balance() - Current_Borrow_Money();
            Change_Current_Balance(New_Curr_Balance);

        }

        void Change_Current_Balance(double New_Balance)
        {
            try
            {
                string Sql = "update curr_balance set Balance='" + New_Balance.ToString() + "' where Email like '" + EMAIL + "' ";
                SqlCommand command = new SqlCommand(Sql, db.con);
                // DataTable dt = new DataTable();
                command.ExecuteNonQuery();


            }
            catch (Exception)
            {

            }
        }



        private void Delete_Event_Button(object sender, RoutedEventArgs e)
        {
            if (Check_Delete_TextBox() == true)
            {
                double PB = 0, BM = 0;
                string pn = "";
                Phone2 = phone2.Text;
                try
                {
                    string Sql2 = "select *  from Due_Balance where Email like '" + EMAIL + "' and Phone ='" + Phone2.ToString() + "'";
                    SqlCommand command2 = new SqlCommand(Sql2, db.con);
                    DataTable dt2 = new DataTable();
                    dt2.Load(command2.ExecuteReader());
                    pn = dt2.Rows[0]["Phone"].ToString();
                    string pb = dt2.Rows[0]["Lend_Money"].ToString();
                    string bm = dt2.Rows[0]["Borrow_Money"].ToString();
                    PB = Convert.ToDouble(pb);
                    BM = Convert.ToDouble(bm);//Borrow Money

                }
                catch (Exception)
                {

                }


                if (pn.Equals(Phone2) && PB == 0)
                {

                    string messageBoxText = "Do you really want to delete this event?";
                    string caption = "Delete Event";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    // MessageBox.Show(messageBoxText, caption, button, icon);

                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);


                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            // User pressed Yes button
                            try
                            {
                                Auto_Get_Back_Lend_Money();
                                // string Sql = "DELETE from Due_Balance where Email like '" + EMAIL + "' and Phone ='" + Phone2.ToString() + "'";
                                string Sql = "update Due_Balance set Validity='" + "False" + "' where Email like '" + EMAIL + "' and Phone='" + Phone2.ToString() + "'";
                                SqlCommand command = new SqlCommand(Sql, db.con);
                                DataTable dt = new DataTable();
                                dt.Load(command.ExecuteReader());
                                check = true;

                                MessageBox.Show("Event Deleted Successfully");

                                Due_Balance f = new Due_Balance(EMAIL);
                                this.Hide();
                                f.Show();
                                db.CloseConnection();
                            }
                            catch (Exception)
                            {

                            }
                            break;
                        case MessageBoxResult.No:
                            // User pressed No button
                            // ...
                            break;
                        case MessageBoxResult.Cancel:
                            // User pressed Cancel button
                            // ...
                            break;
                    }

                }

                else if (pn.Equals(Phone2) && PB > 0 && BM > 0)
                {
                    try
                    {


                        string sql2 = "update Due_Balance set Borrow_Money = '" + "0" + "', Borrow_Note = '" + null + "', Date = '" + date + "' where Phone like '" + Phone2 + "' and Email like '" + EMAIL + "'";
                        SqlCommand command = new SqlCommand(sql2, db.con);
                        int result = command.ExecuteNonQuery();
                        MessageBox.Show("Due Balance Updated !!");
                        Console.WriteLine("Due Balance Updated !!");

                        Due_Balance f = new Due_Balance(EMAIL);
                        this.Hide();
                        f.Show();
                        db.CloseConnection();
                    }
                    catch (Exception)
                    {

                    }
                }
                else if (BM == 0)//LM is lend Money ,if lend money is equal to zero ,then show a messsage ,phone number not found
                {
                    MessageBox.Show("Phone Number Not Found!!");
                }

            }
        }
        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Borrow_Money b = new Borrow_Money(EMAIL);
            this.Hide();
            b.Show();
        }

        private void Update_Event_Button(object sender, RoutedEventArgs e)
        {
            if (Check_Update_TextBox() == true)
            {
                string Amount1 = amount.Text;
                string Phone1 = phone1.Text;
                double Amount = Convert.ToDouble(Amount1);

                string Option = MyComboBox.Text;

                string sql = "select * from [Due_Balance] where Phone like '" + Phone1 + "' and Email like '" + EMAIL + "'";
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt = new DataTable();

                dt.Load(command.ExecuteReader());

                try
                {
                    Priv_Balance = dt.Rows[0]["Borrow_Money"].ToString();
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
                    double Curr_Balance = Amount;
                    double Privioue_Balance = Convert.ToDouble(Priv_Balance);
                    double New_Balance = 0;
                    string message = "";
                    if (Option.Equals(string.Empty))
                    {
                        MessageBox.Show("Please select an option !");
                    }

                    else if (Option.Equals("Give") && Privioue_Balance >= Curr_Balance)
                    {
                        Change_Borrow_Money_Give();
                        New_Balance = Privioue_Balance - Curr_Balance;
                        message = "Giving Money ";
                    }
                    else if (Option.Equals("Take"))
                    {
                        Change_Borrow_Money_Take();
                        New_Balance = Privioue_Balance + Curr_Balance;
                        message = "Receiving Money ";
                    }
                    else if (Option.Equals("Give") && Privioue_Balance < Curr_Balance)
                    {
                        Option = "";
                        MessageBox.Show("You are Giving More than Borrowing amount of money\nPlease check & change the amount");
                    }


                    if (Option != "")
                    {
                        try
                        {
                            string sql2 = "update [Due_Balance] set Borrow_Money = '" + New_Balance + "', Date = '" + date + "' where Phone like '" + Phone1 + "' and Email like '" + EMAIL + "'";
                            command = new SqlCommand(sql2, db.con);
                            int result = command.ExecuteNonQuery();
                            MessageBox.Show(message + Curr_Balance + " taka");
                            Console.WriteLine("Due Balance Updated !!");

                            Due_Balance f = new Due_Balance(EMAIL);
                            this.Hide();
                            f.Show();
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Phone Number Doesn't Match !!");
                }
            }
        }





        bool Check_Update_TextBox()
        {
            bool check = false;
            string Phone = phone1.Text;
            string Amount = amount.Text;

            if (Phone.Equals(string.Empty))
            {
                UpdatePhone.Content = "Phone Number Required !!";
            }

            else
            {
                UpdatePhone.Content = "";
            }
            if (Amount.Equals(string.Empty))
            {
                UpdateAmount.Content = "Amount Requred !";
            }
            else
            {
                UpdateAmount.Content = "";
            }

            if (Amount != "" && Phone != "")
            {
                check = true;
            }
            return check;
        }

        bool Check_Delete_TextBox()
        {
            bool check = false;
            string Phone = phone2.Text;
            if (Phone.Equals(string.Empty))
            {
                DeletePhone.Content = "Phone Number Required to Delete Event";
            }
            else
            {
                DeletePhone.Content = "";
            }
            if (Phone != "")
            {
                check = true;
            }
            return check;
        }
    }
}
