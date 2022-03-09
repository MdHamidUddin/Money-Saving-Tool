using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Data;

namespace Money_Savings_Tool.All_Menu
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private String date = DateTime.Now.ToString("dd.MM.yyyy");
        private string Image_Serial;
        private int img_srl;
        private bool check = false;
        private string EMAIL;
        public string Path;
        MediaPlayer m_mediaPlayer;
        DatabaseConnection db = new DatabaseConnection();
        public Settings(string Email)
        {
            EMAIL = Email;
            InitializeComponent();
            checkPath();
            //Music
            m_mediaPlayer = new MediaPlayer();
            string MusicPath = @"D:\Money_Savings_Tool\Media\Theme.wav";
            m_mediaPlayer.Open(new Uri(MusicPath));
            //Music End
            Get_Image_Serial();
            img_srl = Convert.ToInt32(Image_Serial);
            ShowPicture();


        }

        void checkPath()
        {
            Path = @"D:\Money_Savings_Tool\Media\" + EMAIL + @"\";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        void ShowPicture()
        {
            string loc = @"D:\Money_Savings_Tool\Media\" + EMAIL +@"\"+ "(" + img_srl + ")" + ".png";

            if (File.Exists(loc))
            {
                Uri fileUri = new Uri(loc);
                ImageBox.Source = new BitmapImage(fileUri);
            }
            else
            {
                string loc2 = @"D:\Money_Savings_Tool\Media\Default pic.png";
                Uri fileUri = new Uri(loc2);
                ImageBox.Source = new BitmapImage(fileUri);

            }

        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Homepage h = new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }

        void Play_Music()
        {
           
            m_mediaPlayer.Play();
            m_mediaPlayer.Volume = .03;
        }
        void Stop_Music()
        {
           m_mediaPlayer.Pause();
        }

        void Volume_Up()
        {
            m_mediaPlayer.Volume += .02;
        }

        void Volume_Down()
        {
            m_mediaPlayer.Volume -= .02;
        }

        private void Change_Password_Button(object sender, RoutedEventArgs e)
        {
            Change_Password c = new Change_Password(EMAIL);
            this.Hide();
            c.Show();
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
                Image_Serial = "0";
                //MessageBox.Show(Image_Serial);
            }
        }


        void Update_Image_Serial()
        {
           
            try
            {
                string sql = "Update Image_Control set Image_Serial='" + img_srl + "' where Email like '"+EMAIL+"'";
                SqlCommand command = new SqlCommand(sql, db.con);
                command.ExecuteNonQuery();
                Homepage h = new Homepage(EMAIL);
                this.Hide();
                h.Show();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }

        }

        private void Update_Picture(object sender, RoutedEventArgs e)
        {
            img_srl++;
            try
            {


                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    ImageBox.Source = new BitmapImage(fileUri);
                    File.Copy(openFileDialog.FileName, @"D:\Money_Savings_Tool\Media\" + EMAIL + @"\" + "(" + img_srl + ")" + ".png", true);
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Go to settings to change picture !!!");
            }

            Update_Image_Serial();
            

        }



        private void Media_Settings(object sender, RoutedEventArgs e)
        {
            Stop_Music();
        }

        private void PlayMusic(object sender, RoutedEventArgs e)
        {
            Play_Music();
        }

        private void Volume_Up_Click(object sender, RoutedEventArgs e)
        {
            Volume_Up();
        }

        private void Volume_Down_Click(object sender, RoutedEventArgs e)
        {
            Volume_Down();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update_User_Info up = new Update_User_Info(EMAIL);
            this.Hide();
            up.Show();
        }
    }
}
