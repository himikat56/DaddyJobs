﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Daddy_Jobs
{
    public partial class Main_Form : Form
    {
        public static string idDevice;
        string GlobalConnection = "Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True";
        public Main_Form()
        {
            InitializeComponent();
            groupBox4.Show();
            var source = new AutoCompleteStringCollection();
            var sourceDevice = new AutoCompleteStringCollection();
            using (SqlConnection conn = new SqlConnection(GlobalConnection))
            {
                conn.Open();
                var cmd = new SqlCommand("Select Device.Device_code, Manufacturer.Namination, Device.Model,Device.IMEI, Client.FIO From Device, Client, Manufacturer Where Device.Client_code = Client.Client_code and Device.Manufacturer_code = Manufacturer.Manufacturer_code", conn);

                SqlDataAdapter sda = new SqlDataAdapter(); // адаптер
                sda.SelectCommand = cmd; // адаптеру даем команду

                sda.Fill(original_DaddyJobs); // выполнение запроса и его результат помещаем в ds типа DataSet


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
                    textBox1.AutoCompleteCustomSource = source;

                    com.CommandText = string.Format("SELECT Device_code FROM Device");
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            sourceDevice.Add(r[0].ToString());
                        }
                    }
                    textBox12.AutoCompleteCustomSource = sourceDevice;
                }
                conn.Close();
            }
        }

        protected void Form2_Closed(object sender, EventArgs e) { Application.Exit(); } //Остановка приложения

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "original_DaddyJobs3.Type_of_repair". При необходимости она может быть перемещена или удалена.
            this.type_of_repairTableAdapter.Fill(this.original_DaddyJobs3.Type_of_repair);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "original_DaddyJobs.HomeTable". При необходимости она может быть перемещена или удалена.
            this.homeTableTableAdapter.Fill(this.original_DaddyJobs.HomeTable);
            ClearHomeTable();
            // TODO: данная строка кода позволяет загрузить данные в таблицу "original_DaddyJobs.Type_of_repair". При необходимости она может быть перемещена или удалена.
            this.type_of_repairTableAdapter.Fill(this.original_DaddyJobs.Type_of_repair);
            this.dataPaymentFormTableAdapter.Fill(this.original_DaddyJobs.DataPaymentForm);
            this.dataWorkTableAdapter.Fill(this.original_DaddyJobs2.DataWork);
            this.dataWork2TableAdapter.Fill(this.original_DaddyJobs2.DataWork2);
            this.workTableAdapter1.Fill(this.original_DaddyJobs.Work);
            this.dataWorkTableAdapter.Fill(this.original_DaddyJobs.DataWork);
            this.spare_partTableAdapter2.Fill(this.original_DaddyJobs1.Spare_part);
            this.name_of_workTableAdapter1.Fill(this.original_DaddyJobs.Name_of_work);
            this.spare_partTableAdapter1.Fill(this.daddyJobsDataSet21.Spare_part);
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
                int IdDevice = AddDevice(comboBox1.SelectedIndex + 1, textBox3.Text, textBox5.Text, textBox4.Text, textBox6.Text, Convert.ToBoolean(checkBox1.CheckState), IdClient,1);
                DateTime thisDay = DateTime.Today;
                AddWork(13, 0, 1, IdDevice, 5, 0, thisDay.ToString("d"), "");//Прием устройства
                label25.Text = "Код принятого устройства: " + IdDevice;
                label25.Visible = true;
                bool[] val = { false, false, false, false, false, false, false, false, false, false };
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    val[indexChecked] = true;
                }

                AddInspection(IdDevice, thisDay.ToString("d"), val[0], val[1], val[2], val[6], val[4], val[5], val[7], val[3], val[8], val[9]);
            }

        }
        public int AddClient(string fio, string phone, string date_of_birth, string document_number)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection(GlobalConnection);
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
            SqlConnection conn = new SqlConnection(GlobalConnection);
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Inspection] ([Device_code],[Date_of_inspection],[Display_module],[Microphone],[Hearing_speaker],[Charging_socket],[Main_camera],[Front_camera],[Headphone_jack],[Music_speaker],[Battery],[Button]) VALUES (@device_code,@date_of_inspection,@val1,@val2,@val3,@val4,@val5,@val6,@val7,@val8,@val9,@val10)", conn);
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
        public void AddWork(int spare_part_code, int spare_part_cost, int employee_code, int device_code, int code_name_of_work, int work_cost, string date_of_completion, string comment)
        {
            SqlConnection conn = new SqlConnection(GlobalConnection);
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Work] ([Spare_part_code],[Spare_part_cost],[Employee_code],[Device_code],[Code_name_of_work],[Work_cost],[Date_of_completion],[Comment]) VALUES (@spare_part_code,@spare_part_cost,@employee_code,@device_code,@code_name_of_work,@work_cost,@date_of_completion,@comment)", conn);
            cmd.Parameters.AddWithValue("@spare_part_code", spare_part_code);
            cmd.Parameters.AddWithValue("@spare_part_cost", spare_part_cost);
            cmd.Parameters.AddWithValue("@employee_code", employee_code);
            cmd.Parameters.AddWithValue("@device_code", device_code);
            cmd.Parameters.AddWithValue("@code_name_of_work", code_name_of_work);
            cmd.Parameters.AddWithValue("@work_cost", work_cost);
            cmd.Parameters.AddWithValue("@date_of_completion", date_of_completion);
            cmd.Parameters.AddWithValue("@comment", comment);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void AddPayment(int device_code, int payment, string comment, string date_time)
        {
            SqlConnection conn = new SqlConnection(GlobalConnection);
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Payment] ([Device_code],[Payment],[Comment],[Date_time]) VALUES (@device_code,@payment,@comment,@date_time)", conn);
            cmd.Parameters.AddWithValue("@device_code", device_code);
            cmd.Parameters.AddWithValue("@payment", payment);
            cmd.Parameters.AddWithValue("@comment", comment);
            cmd.Parameters.AddWithValue("@date_time", date_time);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int AddDevice(int manufacturer, string model, string malfunction, string imei, string condition, bool urgent_repairs, int client_code, int status_code)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection(GlobalConnection);
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO [Device] ( [Manufacturer_code],[Model],[Malfunction],[IMEI],[Condition],[Urgent_repairs],[Client_code],[Status_code]) VALUES (@manufacturer,@model,@malfunction,@imei,@condition,@urgent_repairs,@client_code,@status_code)", conn);
            cmd.Parameters.AddWithValue("@manufacturer", manufacturer);
            cmd.Parameters.AddWithValue("@model", model);
            cmd.Parameters.AddWithValue("@malfunction", malfunction);
            cmd.Parameters.AddWithValue("@imei", imei);
            cmd.Parameters.AddWithValue("@condition", condition);
            cmd.Parameters.AddWithValue("@urgent_repairs", urgent_repairs);
            cmd.Parameters.AddWithValue("@client_code", client_code);
            cmd.Parameters.AddWithValue("@status_code", status_code);
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
            this.label10.Text = "Пользователь:  " + login.TrimEnd();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void релогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Authorization_form form = new Authorization_form();
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
            groupBox4.Hide();

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

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(GlobalConnection))
                {
                    conn.Open();

                    using (SqlCommand com = conn.CreateCommand())
                    {
                        bool sourceDevice = false;
                        com.CommandText = string.Format("SELECT Device_code FROM Device");
                        using (SqlDataReader r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if(r[0].ToString() == textBox12.Text)
                                { sourceDevice = true; }
                            }
                        }
                        if (sourceDevice == true)
                        {
                            com.CommandText = string.Format("SELECT Model FROM Device WHERE Device_code = " + textBox12.Text);
                            using (SqlDataReader r = com.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    label16.Text = "Устройство: " + (string)r[0];// Результаты запроса 
                                }
                            }
                        }
                        else label16.Text = "Устройство: ";
                    }
                    conn.Close();
                }//
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime thisDay = DateTime.Today;
            AddWork(comboBox4.SelectedIndex + 1, Convert.ToInt32(textBox7.Text), 1, Convert.ToInt32(textBox12.Text), comboBox3.SelectedIndex + 1, Convert.ToInt32(textBox8.Text), thisDay.ToString("d"), textBox11.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(GlobalConnection))
                {
                    conn.Open();

                    using (SqlCommand com = conn.CreateCommand())
                    {
                        com.CommandText = string.Format("SELECT Cost FROM Name_of_work WHERE Name = '" + comboBox3.Text) + "'";
                        using (SqlDataReader r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                textBox8.Text = r[0].ToString();// Результаты запроса 
                            }
                        }
                    }
                    conn.Close();
                }//   
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(GlobalConnection))
                {
                    conn.Open();

                    using (SqlCommand com = conn.CreateCommand())
                    {
                        com.CommandText = string.Format("SELECT Cost FROM Spare_Part WHERE Namination = '" + comboBox4.Text + "'");
                        using (SqlDataReader r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                textBox7.Text = r[0].ToString();// Результаты запроса 
                            }
                        }
                    }
                    conn.Close();
                }//   
            }
        }
        private void ClearPayment ()
        {
            label13.Text = "Устройство: ";
            label11.Text = "К оплате: ";
            using (SqlConnection conn = new SqlConnection(GlobalConnection))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Work_code,Name_of_work.Name,Work_cost,Spare_part.Namination,Spare_part_cost,Comment FROM Work,Name_of_work,Spare_part WHERE Work.Spare_part_code = Spare_part.Spare_part_code and Work.Code_name_of_work = Name_of_work.Code_name_of_work", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Work,Name_of_work,Spare_part");
                dataGridView2.DataSource = ds.Tables["Work,Name_of_work,Spare_part"];
                conn.Close();
            }
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(GlobalConnection))
                {
                    conn.Open();

                    using (SqlCommand com = conn.CreateCommand())
                    {
                        bool sourceDevice = false;
                        com.CommandText = string.Format("SELECT Device_code FROM Device");
                        using (SqlDataReader r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if (r[0].ToString() == textBox10.Text)
                                { sourceDevice = true; }
                            }
                        }
                        if (sourceDevice == true)
                        {
                            SqlDataAdapter da = new SqlDataAdapter("SELECT Work_code,Name_of_work.Name,Work_cost,Spare_part.Namination,Spare_part_cost,Comment FROM Work,Name_of_work,Spare_part WHERE Work.Spare_part_code = Spare_part.Spare_part_code and Work.Code_name_of_work = Name_of_work.Code_name_of_work and  Device_code = " + textBox10.Text, conn);
                            SqlCommandBuilder cb = new SqlCommandBuilder(da);
                            DataSet ds = new DataSet();
                            da.Fill(ds, "Work,Name_of_work,Spare_part");
                            dataGridView2.DataSource = ds.Tables["Work,Name_of_work,Spare_part"];
                            com.CommandText = string.Format("SELECT Model FROM Device WHERE Device_code = " + textBox10.Text);
                            using (SqlDataReader r = com.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    label13.Text = "Устройство: " + (string)r[0];// Результаты запроса 
                                }
                            }
                            com.CommandText = string.Format("SELECT SUM(Spare_part_cost),SUM(Work_cost) FROM Work WHERE Device_code = " + textBox10.Text);
                            using (SqlDataReader r = com.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    int work = 0;
                                    int spare = 0;
                                    if (r[0] == DBNull.Value) { work = 0; } else { work = (int)r[0]; }
                                    if (r[1] == DBNull.Value) { spare = 0; } else { spare = (int)r[1]; }
                                    label11.Text = "К оплате: " + Convert.ToString(work + spare);// Результаты запроса 
                                }
                            }
                        }
                        else ClearPayment();
                    }
                    conn.Close();
                }//
            }
            else ClearPayment();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox10.Text != "" && textBox9.Text != "" && textBox14.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(GlobalConnection))
                {
                    conn.Open();

                    using (SqlCommand com = conn.CreateCommand())
                    {
                        bool sourceDevice = false;
                        com.CommandText = string.Format("SELECT Device_code FROM Device");
                        using (SqlDataReader r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if (r[0].ToString() == textBox10.Text)
                                {
                                    sourceDevice = true;
                                }
                            }
                        }
                        if (sourceDevice == true)
                        {
                            DateTime now = DateTime.Now;
                            AddPayment(Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox9.Text), textBox14.Text, now.ToString("d"));
                            AddWork(13, 0, 1, Convert.ToInt32(textBox10.Text), 6, 0, now.ToString("d"), "");//Выдача устройства
                        }
                    }
                }
            }
        }

        private void рассылкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form smsF = new SMSform();
            smsF.Show();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        public void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(dataGridView3.CurrentRow.Cells[0].Value.ToString());
            idDevice = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            Form editF = new Edit_form();
            editF.Show(this);
            


        }

        private void главнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideGroupBox();
            groupBox4.Show();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (textBox15.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(GlobalConnection))
                {
                    conn.Open();

                    using (SqlCommand com = conn.CreateCommand())
                    {
                            var regdev = Regex.Match(textBox15.Text, @"[0-9][0-9]+(?:\.[0-9]*)?");
                            int devid = 0;
                            if (regdev.Value.ToString() == "" || regdev.Value.ToString() == null) { devid = 0; } else devid = Convert.ToInt32(regdev.Value.ToString());
                            SqlDataAdapter da = new SqlDataAdapter(@"SELECT Device.Device_code, Status.Namination, Manufacturer.Namination AS Expr1, Device.Model, Device.IMEI, Device.Malfunction, Client.FIO, Client.Phone_number
FROM            Client INNER JOIN
                         Device ON Client.Client_code = Device.Client_code INNER JOIN
                         Status ON Device.Status_code = Status.Status_code INNER JOIN
                         Manufacturer ON Device.Manufacturer_code = Manufacturer.Manufacturer_code
WHERE Device.Device_code = " + devid.ToString() + " or Device.IMEI like '%" + textBox15.Text + "%' or Client.FIO like '%" + textBox15.Text + "%'", conn);
                            SqlCommandBuilder cb = new SqlCommandBuilder(da);
                            DataSet ds = new DataSet();
                            da.Fill(ds, "Client,Device,Status,Manufacturer");
                            dataGridView3.DataSource = ds.Tables["Client,Device,Status,Manufacturer"];
                    }
                    conn.Close();
                }//
            }
            else ClearHomeTable();
        }
        private void ClearHomeTable()
        {
            using (SqlConnection conn = new SqlConnection(GlobalConnection))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT Device.Device_code, Status.Namination, Manufacturer.Namination AS Expr1, Device.Model, Device.IMEI, Device.Malfunction, Client.FIO, Client.Phone_number
                         FROM Client INNER JOIN
                         Device ON Client.Client_code = Device.Client_code INNER JOIN
                         Status ON Device.Status_code = Status.Status_code INNER JOIN
                         Manufacturer ON Device.Manufacturer_code = Manufacturer.Manufacturer_code", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Client,Device,Status,Manufacturer");
                dataGridView3.DataSource = ds.Tables["Client,Device,Status,Manufacturer"];
                conn.Close();
            }
        }

        private void справочникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form ditF = new Edit_directory();
            ditF.Show();
        }
    }
}
