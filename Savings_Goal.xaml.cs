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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Money_Savings_Tool
{
    /// <summary>
    /// Interaction logic for Savings_Goal.xaml
    /// </summary>
    public partial class Savings_Goal : Window
    {
     
        DateTime date = DateTime.Now;
        string EMAIL;
        double Goal, Completed, Rest;
        DatabaseConnection db = new DatabaseConnection();
        public SeriesCollection SeriesCollection { get; set; }

       

       

        public Savings_Goal(string Email)
        {
            EMAIL = Email;
            Previous_Amount();
            InitializeComponent();
            Check_Goal_Status();
            Show_Progress_Bar();

            label_Show_Hidden();

        }

        void label_Show_Hidden()
        {
            double SugestedAMount = Sugesstion_Money() + (.05 * Sugesstion_Money());
            if (Completed < Goal)
            {
                PieControl();
                label_Goal.Content = "Current Savings Goal " + Goal + " Taka";
                labelCreated.Visibility = Visibility.Hidden;
                Set_Amount.Visibility = Visibility.Hidden;
                setAmount.Visibility = Visibility.Hidden;
                Create_Button.Visibility = Visibility.Hidden;
            }
            else if (Completed >= Goal)
            {
                Remaining_Time.Visibility = Visibility.Hidden;
                label_Days.Visibility = Visibility.Hidden;
                if (SugestedAMount > 0)
                {
                    label_Goal.Content = "Congratulations !! You have Successfully Completed Your Previous Goals.\nNow You Are 'Suggested' to Make " + SugestedAMount + " taka for an Another Goal ";
                }
                Change_Status_Savings_Goals();
                label_Goal.Foreground = Brushes.Red;
                label_Update.Visibility = Visibility.Hidden;
                label_Amount.Visibility = Visibility.Hidden;
                MyComboBox.Visibility = Visibility.Hidden;
                Update_Amount.Visibility = Visibility.Hidden;
                Update_Button.Visibility = Visibility.Hidden;
            }
        }


        void Show_Progress_Bar()
        {

            Remaining_Time.Value = Time_Control();
            label_Days.Content = "Remaining Days To Complete Current Goal " + Time_Control();

            if(Time_Control()<=5)
            {
                Remaining_Time.Foreground = Brushes.Red;
            }
        }



        void PieControl()
        {

            
            try
            {
                DataContext = new object();
                SeriesCollection = new SeriesCollection
            {

                new PieSeries
                {
                    
                    Title = "Complete Goal",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Convert.ToInt32(Completed)) },
                    DataLabels = true

                },
                 new PieSeries
                 {
                     Title = "Rest Goal",
                     Values = new ChartValues<ObservableValue> { new ObservableValue(Convert.ToInt32(Rest)) },
                     DataLabels = true

                 }

            };
                DataContext = this;
            }

            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        int Time_Control()
        {
            int Remaining_Day = 0;
            string sql = "SELECT DATEDIFF(day, Updated, GETDATE()) Remaining_Day from Savings_Goals where Email like'"+EMAIL+"' and Validity like '"+"True"+"'";
            try
            {
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt2 = new DataTable();
                dt2.Load(command.ExecuteReader());
                string Remaining_DAY = dt2.Rows[0]["Remaining_Day"].ToString();

                Remaining_Day = Convert.ToInt32(Remaining_DAY);

            }
            catch (Exception E)
            {
                // MessageBox.Show(E.Message);
            }
            Remaining_Day = 30 - Remaining_Day;
            return Remaining_Day;
           

        }

        void Change_Status_Savings_Goals()
        {
            string sql = "update Savings_Goals set Validity='"+"False"+"' where Email like '"+EMAIL+"'";
            try
            {
                SqlCommand command = new SqlCommand(sql,db.con);
                command.ExecuteNonQuery();
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        void Change_Status_Savings_Goals_History()
        {
            string sql = "update Savings_Goals_History set Validity='" + "False" + "' where Email like '" + EMAIL + "' and Validity like '"+"True"+"'";
            try
            {
                SqlCommand command = new SqlCommand(sql, db.con);
                command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
               // MessageBox.Show(E.Message);
            }
        }


        void Check_Goal_Status()
        {
            double Completed_Amount = 0,Created_Goal=0;
            string sql = "select * from Savings_Goals where Email like '" + EMAIL + "' and Validity='"+"True"+"'";
            try
            {
                SqlCommand command = new SqlCommand(sql, db.con);
                DataTable dt2 = new DataTable();
                dt2.Load(command.ExecuteReader());
                string Completed = dt2.Rows[0]["Completed"].ToString(); 
                string Goal = dt2.Rows[0]["Created_Goal"].ToString();
                Completed_Amount = Convert.ToDouble(Completed);
                Created_Goal = Convert.ToDouble(Goal);
            }
            catch(Exception E)
            {
               // MessageBox.Show(E.Message);
            }

            if(Completed_Amount>=Created_Goal)
            {
                Change_Status_Savings_Goals_History();
            }


        }



        void CreateGoal(string Email,double Amount)
        {
            try
            {
                string query = "insert into [Savings_Goals] (Email,Created_Goal,Completed,Rest,Updated,Validity) Values('"+Email+"','"+Amount+"',0,'"+ Amount + "','"+ date + "','"+"True"+"')";
                SqlCommand command = new SqlCommand(query,db.con);
                command.ExecuteScalar();
            
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

       



        private void Create_Goal_Click(object sender, RoutedEventArgs e)
        {
            string amount = setAmount.Text;
            if (amount.Equals(""))
            {
                label_amount_needed.Content = "Amount Needed !!";
            }
            else
            {
                label_amount_needed.Content = "";
            }
            if (amount!= "")
            {
                double Amount = Convert.ToDouble(amount);
                CreateGoal(EMAIL, Amount);
                MessageBox.Show("Savings Goal Created Successfully");
                Savings_Goals_History s = new Savings_Goals_History(EMAIL);
                this.Hide();
                s.Show();
            }
           

        }


        void Update_Goals(double Completed_Amount,double Rest_Amount)
        {
            
            try
            {
                string query = "Update [Savings_Goals] set Completed='"+Completed_Amount+"' , Rest='"+Rest_Amount+"' where serial=(select MAX(serial) from [Savings_Goals]) and Email like '" + EMAIL+"'";
                SqlCommand command = new SqlCommand(query,db.con);
                command.ExecuteNonQuery();
                MessageBox.Show("Updated Successfully !!");
            
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }

           
        }


        double Sugesstion_Money()
        {
            string goal;
            double SugestedAmount = 0;

            try
            {

                string Sql2 = "select * from [Savings_Goals] where serial=(select (MAX(serial)-1) from [Savings_Goals]) and Email like'" + EMAIL + "' ";
                // string Sql2 = "select *  from [Savings_Goals] where Email like '" + EMAIL + "'";
                SqlCommand command2 = new SqlCommand(Sql2, db.con);
                DataTable dt2 = new DataTable();
                dt2.Load(command2.ExecuteReader());
                goal = dt2.Rows[0]["Created_Goal"].ToString();


                SugestedAmount = Convert.ToDouble(goal);
            }
            catch (Exception E)
            {
                //MessageBox.Show(E.Message) ;
            }
            return SugestedAmount;
        }


        void Previous_Amount()
        {
            string goal,completed,rest;

            try
            {

                   string Sql2 = "select * from [Savings_Goals] where serial=(select MAX(serial) from [Savings_Goals]) and Email like'" + EMAIL + "' ";
               // string Sql2 = "select *  from [Savings_Goals] where Email like '" + EMAIL + "'";
                SqlCommand command2 = new SqlCommand(Sql2, db.con);
                DataTable dt2 = new DataTable();
                dt2.Load(command2.ExecuteReader());
                goal = dt2.Rows[0]["Created_Goal"].ToString();
                completed = dt2.Rows[0]["Completed"].ToString();
                rest = dt2.Rows[0]["Rest"].ToString();

                Goal= Convert.ToDouble(goal);
                Completed = Convert.ToDouble(completed);
                Rest = Convert.ToDouble(rest);
            }
            catch (Exception E)
            {
                //MessageBox.Show(E.Message) ;
            }

        }

        void Money_In(double Amount)
        {
            try
            {
                string query = "insert into [Savings_Goals_History] (Email,Money_In,Money_Out,Updated,Validity) Values ('" + EMAIL + "','"+Amount+"',0,'"+date+"','"+"True"+"')";
                SqlCommand command = new SqlCommand(query,db.con);
                command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        

        private void Check_History_Button_Click(object sender, RoutedEventArgs e)
        {
            Savings_Goals_History s = new Savings_Goals_History(EMAIL);
            this.Hide();
            s.Show();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Homepage h = new Homepage(EMAIL);
            this.Hide();
            h.Show();
        }

        void Money_Out(double Amount)
        {
            try
            {
                string query = "insert into [Savings_Goals_History] (Email,Money_In,Money_Out,Updated,Validity) Values ('" + EMAIL + "',0,'" + Amount + "','" + date + "','"+"True"+"')";
                SqlCommand command = new SqlCommand(query, db.con);
                command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }


        private void Update_Goal_Click(object sender, RoutedEventArgs e)
        {
            if (Time_Control()>=0)
            {
                string amount = Update_Amount.Text;
                double Amount = 0;
                Amount = Convert.ToDouble(amount);
                string option = MyComboBox.Text;
                if (amount.Equals(""))
                {
                    label_amount_needed.Content = "Amount Needed !!";
                }
                else
                {
                    label_amount_needed.Content = "";
                }
                if (amount != "")
                {


                    if (option.Equals("Out Money -"))
                    {
                        Money_Out(Amount);
                        Completed -= Amount;
                        Rest += Amount;
                    }
                    else if (option.Equals("In Money +"))
                    {
                        Money_In(Amount);
                        Completed += Amount;
                        Rest -= Amount;
                    }

                    Update_Goals(Completed, Rest);
                    Savings_Goals_History s = new Savings_Goals_History(EMAIL);
                    this.Hide();
                    s.Show();



                }
            }
        }
    }
}
