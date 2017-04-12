using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Daddy_Jobs
{
    public partial class SMSform : Form
    {
        string GlobalConnection = "Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True";
        int[] idDevice = new int[1000];

        public SMSform()
        {
            InitializeComponent();
            checkedListBox1.Items.Clear();
            for (int i = 0; i == 1000; i++) { idDevice[i] = 0; }
            using (SqlConnection conn = new SqlConnection(GlobalConnection))
            {
                conn.Open();

                using (SqlCommand com = conn.CreateCommand())
                {
                    com.CommandText = string.Format("select Device.Device_code, Device.Model, SUM(Spare_part_cost) + SUM(Work_cost) " +
                    "from Device, Work "+
                    "where  Device.Device_code = Work.Device_code and Device.Status_code = 4 and Device.SMS_sending = 0 " +
                    "Group By Device.Device_code, Device.Model");
                    int i = 0;
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        
                        while (r.Read())
                        {
                            idDevice[i] = Convert.ToInt32(r[0]);
                            i++;
                            checkedListBox1.Items.Add("Заказ №A" + r[0].ToString().TrimEnd() + " (" + r[1].ToString().TrimEnd() + ") К оплате " + r[2].ToString().TrimEnd() + " руб");
                        }
                    }
                }
                conn.Close();
            }//

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConnection))
            {
                int addres = 0;
                conn.Open();
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                    using (SqlCommand com = conn.CreateCommand())
                    {
                        com.CommandText = string.Format("select Device.Device_code, Client.Phone_number, Device.Model, SUM(Spare_part_cost) + SUM(Work_cost)" +
                                                        " from Client, Device, Work" +
                                                        " where Device.Client_code = Client.Client_code and Device.Device_code = Work.Device_code" +
                                                        " and Device.Device_code = " + idDevice[indexChecked].ToString() +
                                                        " Group By Device.Device_code, Client.Phone_number, Device.Model");
                        using (SqlDataReader r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                addres++;
                                //SendingSMS(Convert.ToString(r[1]), Convert.ToInt32(r[0]), Convert.ToString(r[2]), Convert.ToInt32(r[3]));
                                var cmd = new SqlCommand("UPDATE Device SET SMS_sending = 1 WHERE Device_code = @device_code", conn);
                                cmd.Parameters.AddWithValue("@device_code", Convert.ToInt32(r[0]));
                               // cmd.ExecuteNonQuery();
                            }
                        }
                    }
            }
                conn.Close();
                string sklon = "";
                switch (addres) { case 1:  sklon = " уведомление."; break; case 2: case 3: case 4: sklon = " уведомления."; break; default:  sklon = " уведомлений."; break; }
                MessageBox.Show("Отправлено "+ addres.ToString() + sklon);
                SMSform.ActiveForm.Close();
            }//
        }
        private void SendingSMS(string phonenumber,  int order_code, string model, int payment )
        {
            Regex regex = new Regex(@"^9");
            if (regex.IsMatch(phonenumber) && phonenumber.TrimEnd().Length == 10)
            {
                string myApiKey = "3B17699B-0495-CFA4-F2AB-FFFC5CA6B9";
                SmsRu.SmsRu sms = new SmsRu.SmsRu(myApiKey);
                var response = sms.Send(phonenumber.TrimEnd(), "Ваш заказ №A" + order_code + " (" + model.TrimEnd() + ") Готов! К оплате " + payment + " руб");
            }
        }
    }
}
