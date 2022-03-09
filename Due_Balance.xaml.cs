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

namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Due_Balance.xaml
    /// </summary>
    public partial class Due_Balance : Window
    {
        String EMAIL;
        public Due_Balance(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
        }

        private void Borrow_Money_Button(object sender, RoutedEventArgs e)
        {
            Borrow_Money l = new Borrow_Money(EMAIL);
            this.Hide();
            l.Show();
        }

        private void Lend_Money_Button(object sender, RoutedEventArgs e)
        {
            Lend_Money l = new Lend_Money(EMAIL);
            this.Hide();
            l.Show();
        }

        private void Check_History_Button(object sender, RoutedEventArgs e)
        {
            Check_Due_History l = new Check_Due_History(EMAIL);
            this.Hide();
            l.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Homepage h = new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }
    }
}
