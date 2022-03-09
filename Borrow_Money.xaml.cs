using System;
using System.Data.SqlClient;
using System.Windows;
using System.Data;
namespace Money_Savings_Tool
{
    public partial class Borrow_Money : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        String date = System.DateTime.Now.ToString("dd.MM.yyyy");
        String EMAIL, Phone2;
        string GetPhone1, GetPhone2, Priv_Balance;
        bool check = false;
        string Validation;
        SqlCommand command;
        DateTime date2 = DateTime.Now;
        public Borrow_Money(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
            Date_Time.Content = date2;
        }


        private void Update_Event_Button(object sender, RoutedEventArgs e)
        {
            Update_Borrow_Money ubm = new Update_Borrow_Money(EMAIL);
            this.Hide();
            ubm.Show();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Due_Balance d = new Due_Balance(EMAIL);
            this.Hide();
            d.Show();
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
            Balance = Balance + Lend_Balance;
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
                string sql2 = "INSERT INTO Due_Balance(Email,Name,Phone,Lend_Money,Lend_Note,Borrow_Money,Borrow_Note,Date,Validity) VALUES('" + EMAIL + "','" + Name + "','" + Phone1 + "','" + "0" + "','" + null + "','" + Amount + "','" + Note + "','" + date + "','" + "True" + "')";
                command = new SqlCommand(sql2, db.con);
                int result = command.ExecuteNonQuery();
                Console.WriteLine("New Balance Created !!");
                MessageBox.Show("New Balance Created !!");
                Change_Borrow_Money_Take();
                Due_Balance f = new Due_Balance(EMAIL);
                this.Hide();
                f.Show();
            }
            catch (Exception)
            {

            }

        }

        private void Create_Event_Button(object sender, RoutedEventArgs e)
        {
            if (CheckTextBox() == true)
            {
             
                string Phone1 = phone1.Text;
                string sql = "select * from Due_Balance where Phone like '" + Phone1 + "' and Email like '" + EMAIL + "'";
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt = new DataTable();

                dt.Load(command.ExecuteReader());

                try
                {
                    Priv_Balance = dt.Rows[0]["Borrow_Money"].ToString();
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
                    //Here we will check ,when the phone number is present and the validity is false.
                    //then user can again create new amount under that phone nummber.

                    if (Validation=="False")
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
