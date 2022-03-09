using System;
using System.Data.SqlClient;
using System.Windows;
using System.Speech.Synthesis;
namespace Money_Savings_Tool
{
    public partial class Login : Window
    {
        DatabaseConnection db = new DatabaseConnection();
        string EMAIL;
        static Boolean check = true; 
        SpeechSynthesizer speech = new SpeechSynthesizer();
        public Login()
        {
            DateTime date = DateTime.Now;
            InitializeComponent();
            Date_Time.Content = date;
            if (check == true)
            {
                Welcome_Message();
                check = false; 
            }
        }
        void Welcome_Message()
        {
            string line = "Welcome to money savings tools\n Please Sign in to Continue \n " +
                "if you do not have any account, please sign up";
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
        void Null_Email_Pass_Voice()
        {
            string line = "Please enter email and password to sign in";
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
        public int SignIn(string email, string pass)
        {
            int check = 0;
            try
            {
                string sql = "SELECT  Count(*) FROM [Registration] WHERE Email = '" + email + "' and Password='" + pass + "' ";
                SqlCommand command = new SqlCommand(sql, db.con);
                check = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
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
        void Null_Pass()
        {
            string line = "Please Enter Password to Continue";
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


        private void Sign_In_Button(object sender, RoutedEventArgs e)
        {
            EMAIL = Email.Text;
            string email = Email.Text.ToUpper();
            string pass = Pass.Password;

            if (email == "" && pass == "")
            {
                Null_Email_Pass_Voice();
            }
            if (email != "" && pass == "")
            {
                Null_Pass();
            }

            if (email == "" && pass != "")
            {
                Null_Email();
            }

            if (Email.Text == "")
            {
                EmptyEmail.Content = "Email Required";
            }
            else
            {
                EmptyEmail.Content = "";
            }

            if (Pass.Password == "")
            {
                EmptyPassword.Content = "Password Required";
            }
            else
            {
                EmptyPassword.Content = "";
            }

            if (Email.Text != "" && Pass.Password != "")
            {


                if (SignIn(email, pass) == 1)
                {
                    Homepage obj = new Homepage(EMAIL);
                    this.Hide();
                    obj.Show();
                    db.CloseConnection();
                }
                else
                {
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBox.Show("Input is Incorrect!!", "Login failed", button, icon);
                }

            }
        }

        private void Sign_Up_Button(object sender, RoutedEventArgs e)
        {
            Sign_Up obj = new Sign_Up();
            this.Hide();
            obj.Show();
        }

        private void Forgot_Password_Click(object sender, RoutedEventArgs e)
        {
            Forgot_Password f = new Forgot_Password();
            this.Hide();
            f.Show();

        }
    }
}
