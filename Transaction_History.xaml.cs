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
    /// Interaction logic for Transaction_History.xaml
    /// </summary>
    public partial class Transaction_History : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        public static string EMAIL = "";

        public Transaction_History(string email)
        {
            EMAIL = email;
            InitializeComponent();
            try
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter("select Deposited,Note,Withdraw,Date from [User_Data] where Email  like '" + EMAIL + "'", db.con);
                sqlDA.Fill(dt);
                dbgv.ItemsSource = dt.DefaultView;
            }
            catch(Exception E)
            {
                MessageBox.Show("Transaction History Error : "+E.Message);
            }


            try
            {
                SqlDataAdapter sqlDA2 = new SqlDataAdapter("select Balance,Updated from [curr_balance] where Email  like '" + EMAIL + "'", db.con);
                sqlDA2.Fill(dt2);
                dbgv2.ItemsSource = dt2.DefaultView;
            }
            catch(Exception Ex)
            {
                MessageBox.Show("Current Balance Error : " + Ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Homepage h = new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }
    }
}
