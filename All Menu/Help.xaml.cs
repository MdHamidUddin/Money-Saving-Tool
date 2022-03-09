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

namespace Money_Savings_Tool.All_Menu
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        string EMAIL;
        public Help(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Homepage h = new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }
    }
}
