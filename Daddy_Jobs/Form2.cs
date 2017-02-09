using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Daddy_Jobs
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            var source = new AutoCompleteStringCollection();
            using (SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True"))
            {
                conn.Open();
                var cmd = new SqlCommand("Select Device.Device_code, Manufacturer.Namination, Device.Model,Device.IMEI, Client.FIO From Device, Client, Manufacturer Where Device.Client_code = Client.Client_code and Device.Manufacturer_code = Manufacturer.Manufacturer_code", conn);

                SqlDataAdapter sda = new SqlDataAdapter(); // адаптер
                sda.SelectCommand = cmd; // адаптеру даем команду

                sda.Fill(daddy_JobsDataSet1); // выполнение запроса и его результат помещаем в ds типа DataSet

                dataGridView1.DataSource = daddy_JobsDataSet1.Tables[0];
                using (SqlCommand com = conn.CreateCommand())
                {
                    com.CommandText = string.Format("SELECT FIO FROM Client");
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            source.Add((string)r[0]);// Результаты запроса 
                        }
                    }
                }
                conn.Close();
                textBox1.AutoCompleteCustomSource = source;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "daddyJobsDataSet21.Manufacturer". При необходимости она может быть перемещена или удалена.
            this.manufacturerTableAdapter.Fill(this.daddyJobsDataSet21.Manufacturer);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://xn--80aadlk2ccbx.xn--p1ai/");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox5.Text != "" && textBox3.Text != "" && textBox6.Text != "")
            {
                int IdClient = AddClient(textBox1.Text, textBox2.Text, dateTimePicker1.Text, textBox13.Text);
                int IdDevice = AddDevice(comboBox1.SelectedIndex + 1, textBox3.Text, textBox5.Text, textBox4.Text, textBox6.Text, Convert.ToBoolean(checkBox1.CheckState), IdClient);
                DateTime thisDay = DateTime.Today;
                AddWork(13, 1, IdDevice, 6, thisDay.ToString("d"), "");//Прием устройства
                label25.Text = "Код принятого устройства: " + IdDevice;
                label25.Visible = true;
                bool[] val = { false, false, false, false, false, false, false, false, false, false };
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    //MessageBox.Show("Index#: " + indexChecked.ToString() + ", is checked. Checked state is:" +
                     //               checkedListBox1.GetItemCheckState(indexChecked).ToString() + ".");
                    val[indexChecked] = true;
                }

                AddInspection(IdDevice, thisDay.ToString("d"),val[0], val[1], val[2], val[6], val[4], val[5], val[7], val[3], val[8], val[9],);
            }
            
        }
        public int AddClient(string fio,string phone, string date_of_birth, string document_number)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True");
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Client] ( [FIO],[Phone_number],[Date_of_birth],[Document_number]) VALUES (@fio,@phone,@date_of_birth,@document_number)", conn);
            cmd.Parameters.AddWithValue("@fio", fio);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@date_of_birth", date_of_birth);
            cmd.Parameters.AddWithValue("@document_number", document_number);
            cmd.ExecuteNonQuery();
           cmd.CommandText = "SELECT ident_current('Client') as Client_code";
           SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                id = Convert.ToInt16(reader[0]);
           }
            conn.Close();
            return id;
        }
        public void AddInspection(int device_code, string date_of_inspection, bool val1, bool val2, bool val3, bool val4, bool val5, bool val6, bool val7, bool val8, bool val9, bool val10)
        {
            SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True");
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Work] ([Device_code],[Date_of_inspection],[Display_module],[Microphone],[Hearing_speaker],[Charging_socket],[Main_camera],[Front_camera],[Headphone_jack],[Music_speaker],[Battery],[Button]) VALUES (@device_code,@date_of_inspection,@val1,@val2,@val3,@val4,@val5,@val6,@val7,@val8,@val9,@val10)", conn);
            cmd.Parameters.AddWithValue("@device_code", device_code);
            cmd.Parameters.AddWithValue("@date_of_inspection", date_of_inspection);
            cmd.Parameters.AddWithValue("@val1", val1);
            cmd.Parameters.AddWithValue("@val2", val2);
            cmd.Parameters.AddWithValue("@val3", val3);
            cmd.Parameters.AddWithValue("@val4", val4);
            cmd.Parameters.AddWithValue("@val5", val5);
            cmd.Parameters.AddWithValue("@val6", val6);
            cmd.Parameters.AddWithValue("@val7", val7);
            cmd.Parameters.AddWithValue("@val8", val8);
            cmd.Parameters.AddWithValue("@val9", val9);
            cmd.Parameters.AddWithValue("@val10", val10);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void AddWork(int spare_part_code, int employee_code, int device_code, int code_name_of_work, string date_of_completion, string comment)
        {
            SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True");
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Work] ([Spare_part_code],[Employee_code],[Device_code],[Code_name_of_work],[Date_of_completion],[Comment]) VALUES (@spare_part_code,@employee_code,@device_code,@code_name_of_work,@date_of_completion,@comment)", conn);
            cmd.Parameters.AddWithValue("@spare_part_code", spare_part_code);
            cmd.Parameters.AddWithValue("@employee_code", employee_code);
            cmd.Parameters.AddWithValue("@device_code", device_code);
            cmd.Parameters.AddWithValue("@code_name_of_work", code_name_of_work);
            cmd.Parameters.AddWithValue("@date_of_completion", date_of_completion);
            cmd.Parameters.AddWithValue("@comment", comment);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int AddDevice(int manufacturer, string model, string malfunction, string imei, string condition,bool urgent_repairs, int client_code)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True");
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Device] ( [Manufacturer_code],[Model],[Malfunction],[IMEI],[Condition],[Urgent_repairs],[Client_code]) VALUES (@manufacturer,@model,@malfunction,@imei,@condition,@urgent_repairs,@client_code)", conn);
            cmd.Parameters.AddWithValue("@manufacturer", manufacturer);
            cmd.Parameters.AddWithValue("@model", model);
            cmd.Parameters.AddWithValue("@malfunction", malfunction);
            cmd.Parameters.AddWithValue("@imei", imei);
            cmd.Parameters.AddWithValue("@condition", condition);
            cmd.Parameters.AddWithValue("@urgent_repairs", urgent_repairs);
            cmd.Parameters.AddWithValue("@client_code", client_code);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT ident_current('Device') as Device_code";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                id = Convert.ToInt16(reader[0]);
            }
            conn.Close();
            return id;
        }
        public void setLogin(String login)
        {
            this.label10.Text = login;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void релогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }

        private void приемУстройстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideGroupBox();
            groupBox1.Show();
        }

        private void работыСУстрйоствомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideGroupBox();
            groupBox2.Show();
        }

        private void HideGroupBox()
        {
            groupBox1.Hide();
            groupBox2.Hide();
            groupBox3.Hide();
            
        }


        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void выдачаУстрйостваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideGroupBox();
            groupBox3.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
