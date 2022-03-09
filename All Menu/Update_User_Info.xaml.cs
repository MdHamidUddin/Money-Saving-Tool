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

namespace Money_Savings_Tool.All_Menu
{
    /// <summary>
    /// Interaction logic for Update_User_Info.xaml
    /// </summary>
    public partial class Update_User_Info : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        string Name, Phone;
        string EMAIL;
        string Previous_Name, Previous_Phone;
        public Update_User_Info(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
        }

        void Read_Previous_Data()
        {
            try
            {
                string Sql2 = "select *  from Registration where Email like '" + EMAIL + "' ";
                SqlCommand command2 = new SqlCommand(Sql2, db.con);
                DataTable dt2 = new DataTable();
                dt2.Load(command2.ExecuteReader());
                Previous_Name = dt2.Rows[0]["Name"].ToString();
                Previous_Phone = dt2.Rows[0]["Phone"].ToString();

            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }



        void Update_Data(string Name,string Phone)
        {

            string caption = "Update Information";
            string messageBoxText = "Are you sure want to save changes?";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            // MessageBox.Show(messageBoxText, caption, button, icon);

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            if(result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    string sql = "update [Registration] set Name='" + Name + "' , Phone='" + Phone + "' Where Email like '" + EMAIL + "'";
                    SqlCommand command = new SqlCommand(sql, db.con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Information Updated Successfully !!");
                    Settings s = new Settings(EMAIL);
                    this.Hide();
                    s.Show();

                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }
           
            }

     
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Name = TextName.Text;
            Phone = TextPhone.Text;

            Read_Previous_Data();

            if (Name != "" || Phone != "")
            {
                if (Name == "")
                {
                    Name = Previous_Name;
                }

                if (Phone == "")
                {
                    Phone = Previous_Phone;
                }

                Update_Data(Name, Phone);
            }


           
          
            else
            {
                MessageBox.Show("Here is Nothing to change.. ");
            }

           

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Settings s = new Settings(EMAIL);
            this.Hide();
            s.Show();
        }
    }
}
