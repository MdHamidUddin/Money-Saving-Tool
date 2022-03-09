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
using System.Data.SqlClient;
using System.IO;
using System.Speech.Synthesis;

namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Sign_Up.xaml
    /// </summary>
    public partial class Sign_Up : Window
    {
        DateTime date = DateTime.Now;
        DatabaseConnection db = new DatabaseConnection();
        SpeechSynthesizer speech = new SpeechSynthesizer();
        public Sign_Up()
        {
            InitializeComponent();
        }


        public int AddUser(params string[] inputs)
        {
            int check = 0;
            try
            {
                string sql = "INSERT INTO [Registration] (Name,Email,Phone,Password,Date) VALUES('" + inputs[0] + "','" + inputs[1] + "','" + inputs[2] + "','" + inputs[3] + "','" + date + "')";
                db.command = new SqlCommand(sql, db.con);
                check = db.command.ExecuteNonQuery();

            }
            catch (Exception E)
            {
                MessageBox.Show("Email has been already used.\nIf you forget your password, go to forget password");
            }
            return check;
        }

        void Null_Email()
        {

            string line = "Please Enter Email to Continue";
            try
            {

                speech.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                speech.SpeakAsync(line);


            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }

        }
        public bool CheckTextBox()
        {
            bool check = false;
            if (Name.Text == "")
            {
                BlankName.Content = "Name Required !!";
            }
            else
            {
                BlankName.Content = "";
            }

            if (Email.Text == "")
            {
                BlankEmail.Content = "Email Required !!";
            }
            else
            {
                BlankEmail.Content = "";
            }
            if (Phone.Text == "")
            {
                BlankPhone.Content = "Phone Required !!";
            }
            else
            {
                BlankPhone.Content = "";
            }
            if (Password.Password == "")
            {
                BlankPassword.Content = "Password Required !!";
            }
            else
            {
                BlankPassword.Content = "";
            }
            if (Confirm_Password.Password == "")
            {
                BlankConfirmPassword.Content = "Confirm Password Required !!";
            }
            else
            {
                BlankConfirmPassword.Content = "";
            }

            if (Password.Password != Confirm_Password.Password)
            {
                MessageBox.Show("Password and Confirm Password Doesn't Match !");
            }
            if (Password.Password.Length < 8)
            {
                BlankPassword.Content = "Password length should be 8 ";
                BlankConfirmPassword.Content = "Password length should be 8";
                BlankPassword.Foreground = Brushes.Red;
                BlankConfirmPassword.Foreground = Brushes.Red;
            }

            if (Name.Text != "" && Email.Text != "" && Phone.Text != "" && Password.Password != "" && Confirm_Password.Password != "" && Password.Password.Equals(Confirm_Password.Password) && Password.Password.Length >= 8)
            {
                if (MyCheckBox.IsChecked == true)
                {
                    check = true;
                }
                else
                {
                    MessageBox.Show("Click on agree to continue");
                }
            }


            return check;
        }



        private void Sign_Up_Button(object sender, RoutedEventArgs e)
        {
            // String date = System.DateTime.Now.ToString("dd.MM.yyyy");

            string name, email, phone, password, confirm_password;
            name = Name.Text.ToUpper();
            email = Email.Text.ToUpper();
            phone = Phone.Text;
            password = Password.Password;
            confirm_password = Confirm_Password.Password;


            if (CheckTextBox().Equals(true))
            {

                if (AddUser(name, email, phone, password) == 1)
                {
                    MessageBox.Show("User Added Successfully");
                    Login f1 = new Login();
                    this.Hide();
                    f1.Show();
                }
                db.CloseConnection();
            }
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Login obj = new Login();
            this.Hide();
            obj.Show();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
