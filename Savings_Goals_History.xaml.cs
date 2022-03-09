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
    /// Interaction logic for Savings_Goals_History.xaml
    /// </summary>
    public partial class Savings_Goals_History : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        string EMAIL;
        public Savings_Goals_History(String Email)
        {
            EMAIL = Email;
            InitializeComponent();

            try {
                SqlDataAdapter sqlDA = new SqlDataAdapter("select Email,Money_In,Money_Out,Updated from [Savings_Goals_History] where Email  like '" + EMAIL + "' and Validity like '"+"True"+"'", db.con);
                DataTable dt = new DataTable();
                sqlDA.Fill(dt);
                Total_History.ItemsSource = dt.DefaultView;
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }

            try
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter("select Email,Created_Goal,Completed,Rest,Updated  from [Savings_Goals] where Email  like '" + EMAIL + "' and Validity like '"+"True"+"'", db.con);
                DataTable dt2 = new DataTable();
                sqlDA.Fill(dt2);
                Goal.ItemsSource = dt2.DefaultView;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }



        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Savings_Goal s = new Savings_Goal(EMAIL);
            this.Hide();
            s.Show();

        }
    }
}
