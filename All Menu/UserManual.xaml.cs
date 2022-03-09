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
using Money_Savings_Tool;

namespace Money_Savings_Tool.All_Menu
{
    /// <summary>
    /// Interaction logic for UserManual.xaml
    /// </summary>
    public partial class UserManual : Window
    {
        string EMAIL;
        public UserManual(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Homepage h=new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }
    }
}
