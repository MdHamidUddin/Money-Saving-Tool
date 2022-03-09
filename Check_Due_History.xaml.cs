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
    /// Interaction logic for Check_Due_History.xaml
    /// </summary>
    public partial class Check_Due_History : Window
    {
        string EMAIL, Lend, Borrow;
        Boolean check = false;
        DatabaseConnection dc = new DatabaseConnection();

        public Check_Due_History(string Email)
        {

            InitializeComponent();
            EMAIL = Email;
            Total_Lend_Money();
            Total_Borrow_Money();
            Check_History();
        }

        void Total_Lend_Money()
        {
            string sql = "SELECT SUM(Lend_Money) as Total_Lend_Money  FROM Due_Balance WHERE Email like '" + EMAIL + "'  and Validity='" + "True" + "'";
            SqlCommand command = new SqlCommand(sql, dc.con);
            DataTable dt2 = new DataTable();

            dt2.Load(command.ExecuteReader());

            try
            {
                Lend = dt2.Rows[0]["Total_Lend_Money"].ToString();
                check = true;

            }

            catch (Exception)
            {

                Console.WriteLine("Couldnt find");
                check = false;
            }
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Due_Balance d = new Due_Balance(EMAIL);
            this.Hide();
            d.Show();
        }

        void Total_Borrow_Money()
        {
            string sql = "SELECT SUM(Borrow_Money) as Total_Borrow_Money  FROM Due_Balance WHERE Email like '" + EMAIL + "'  and Validity='" + "True" + "'";
            SqlCommand command = new SqlCommand(sql, dc.con);
            DataTable dt2 = new DataTable();

            dt2.Load(command.ExecuteReader());

            try
            {
                Borrow = dt2.Rows[0]["Total_Borrow_Money"].ToString();
                check = true;

            }

            catch (Exception)
            {

                Console.WriteLine("Couldnt find");
                check = false;
            }
        }

        void Check_History()
        {
            string Sql = "select Name,Lend_Money,Lend_Note,Borrow_Money,Borrow_Note,Date from [Due_Balance] where Email  like '" + EMAIL + "' and Validity='"+ "True" + "'";
            SqlDataAdapter sqlDA = new SqlDataAdapter(Sql, dc.con);
            DataTable dt = new DataTable();
            sqlDA.Fill(dt);
            dbgv.ItemsSource = dt.DefaultView;

            lend.Content = "Total Lend Money:" + Lend + " /-";
            borrow.Content = "Total Borrow Money" + Borrow + " /-";
        }
    }
}
