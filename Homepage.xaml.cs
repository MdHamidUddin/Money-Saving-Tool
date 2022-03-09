using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Data;
using Money_Savings_Tool.All_Menu;
using Microsoft.Win32;
using System.IO;


namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : Window
    {

        bool check = false;
        string Image_Serial;
        string EMAIL;
        static string GetBalance, GetName;
        int img_srl;
        DatabaseConnection db = new DatabaseConnection();
        DateTime date = DateTime.Now;
        string CreatePath ,AccessPath;
        // Window mainWindow=new Window();
        public Homepage(string Email)//
        {
            EMAIL = Email;
            InitializeComponent();
            Date_Time.Content = date;
            Get_Balance();
            Get_Name();
            SetName.Content = "Name : "+GetName;
            SetBalance.Content = "Current Balance : "+GetBalance;
            
            Get_Image_Serial();
            img_srl = Convert.ToInt32(Image_Serial);
            
            Settings s = new Settings(EMAIL);
            string loc = @"D:\Money_Savings_Tool\Media\" + EMAIL + "(" + img_srl + ")" + ".png";

            checkPath();
        

        }
        void checkPath()
        {
            CreatePath = @"D:\Money_Savings_Tool\Media\" + EMAIL + @"\";
            if (!Directory.Exists(CreatePath))
            {
                Directory.CreateDirectory(CreatePath);
            }

            AccessPath = @"D:\Money_Savings_Tool\Media\" + EMAIL + @"\" + "(" + img_srl + ")" + ".png";

            if (File.Exists(AccessPath))
            {
                Uri fileUri = new Uri(AccessPath);
                ImageBox.Source = new BitmapImage(fileUri);
                Upload_BTN.Visibility = Visibility.Hidden;
            }
            else
            {
                string loc2 = @"D:\Money_Savings_Tool\Media\Default pic.png";
                Uri fileUri = new Uri(loc2);
                ImageBox.Source = new BitmapImage(fileUri);

            }

        }

        void Get_Name()
        {
            string sql = "select * from Registration where Email like '" + EMAIL + "' ";
            db.command= new SqlCommand(sql, db.con);
            SqlDataReader reader = db.command.ExecuteReader();

            reader.Read();
            GetName = reader["Name"].ToString();
            reader.Close();
            Console.WriteLine("This name is " + GetName);
        }


        void Get_Balance()
        {
            string sql = "select * from curr_balance where Email like '" + EMAIL + "' ";
            SqlCommand command = new SqlCommand(sql, db.con);
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());

            try
            {
                GetBalance = dt.Rows[0]["Balance"].ToString();
                Console.WriteLine("Balance mane taka: " + GetBalance);

            }

            catch (Exception)
            {
                GetBalance = "0";
                Console.WriteLine("Couldnt find");
            }

        }

        void Get_Image_Serial()
        {
            string sql = "select * from Image_Control where Email like '" + EMAIL + "' ";
            SqlCommand command = new SqlCommand(sql, db.con);
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());

            try
            {
                Image_Serial = dt.Rows[0]["Image_Serial"].ToString();

            }

            catch (Exception)
            {
                check = true;
               // Image_Serial = "0";
            }
        }
        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Do you really want to close?";
            string caption = "Closing Application";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            // MessageBox.Show(messageBoxText, caption, button, icon);

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;

            }
                    
        }

        private void Expense_Button(object sender, RoutedEventArgs e)
        {
            Expense obj = new Expense(EMAIL);
            this.Hide();
            obj.Show();
        }

        private void Transaction_History_Button(object sender, RoutedEventArgs e)
        {
            Transaction_History obj = new Transaction_History(EMAIL);
            this.Hide();
            obj.Show();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Due_Balance d = new Due_Balance(EMAIL);
            this.Hide();
            d.Show();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help h = new Help(EMAIL);
            this.Hide();
            h.Show();
        }


        private void About_Click(object sender, RoutedEventArgs e)
        {
            About a = new About(EMAIL);
            this.Hide();
            a.Show();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.Hide();
            l.Show();
        }

        private void UserManual_Click(object sender, RoutedEventArgs e)
        {
            UserManual user = new UserManual(EMAIL);
            this.Hide();
            user.Show();
        }


        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings p = new Settings(EMAIL);
            this.Hide();
            p.Show();
 
        }

        void Insert_Image_Serial()
        {
            try
            {
                string sql = "insert into Image_Control (Email,Image_Serial) values('" + EMAIL + "','" + img_srl + "') ";
                SqlCommand command = new SqlCommand(sql, db.con);
                command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                MessageBox.Show("Here is error  "+E.Message);
            }
        }

        void Update_Image_Serial()
        {
            try
            {
                string sql = "Update Image_Control set Image_Serial='" + img_srl + "'";
                SqlCommand command = new SqlCommand(sql, db.con);
                command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }


        private void Upload_Picture(object sender, RoutedEventArgs e)
        {
           // img_srl++;

            try
            {


                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    ImageBox.Source = new BitmapImage(fileUri);
                    File.Copy(openFileDialog.FileName, @"D:\Money_Savings_Tool\Media\" + EMAIL + @"\" +"("+img_srl+")"+ ".png", true);
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }


            if (check == true)
            {
                Insert_Image_Serial();
            }
            else
            {
                Update_Image_Serial();
            }


        }

        private void Savings_Goal(object sender, RoutedEventArgs e)
        {
            Savings_Goal s = new Savings_Goal(EMAIL);
            this.Hide();
            s.Show();
        }

        private void Cash_In_Button(object sender, RoutedEventArgs e)
        {
            Cash_In obj = new Cash_In(EMAIL);
            this.Hide();
            obj.Show();
        }
    }
}
