using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        DatabaseConnection db = new DatabaseConnection();
       // Image File = new Image();
        string imgLoc = "";
        //Color theme=Color.Red;
        string EMAIL;
        public Test()
        {
            InitializeComponent();
           
          
        }

    }
}
