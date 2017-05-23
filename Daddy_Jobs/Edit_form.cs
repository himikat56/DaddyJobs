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
    public partial class Edit_form : Form
    {
        string GlobalConnection = "Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True";
        public Edit_form()
        {
            InitializeComponent();
            
        }

        private void Edit_form_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "original_DaddyJobs.Manufacturer". При необходимости она может быть перемещена или удалена.
            this.manufacturerTableAdapter.Fill(this.original_DaddyJobs.Manufacturer);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "original_DaddyJobs.Status". При необходимости она может быть перемещена или удалена.
            this.statusTableAdapter.Fill(this.original_DaddyJobs.Status);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "original_DaddyJobs.Type_of_repair". При необходимости она может быть перемещена или удалена.
            this.type_of_repairTableAdapter.Fill(this.original_DaddyJobs.Type_of_repair);
            int Code_Man = 0, Code_Rep = 0, Code_Status = 0, Code_Client = 0;
            int IDDev = Convert.ToInt32(Main_Form.idDevice);
            label1.Text = "Заказ: " + Main_Form.idDevice;

            using (SqlConnection conn = new SqlConnection(GlobalConnection))
            {
                conn.Open();

                using (SqlCommand com = conn.CreateCommand())
                {
                    com.CommandText = string.Format("SELECT Manufacturer_code, Model, Malfunction, IMEI, Condition, Code_type_of_repair, Status_code, Client_code FROM Device Where Device_code = " + IDDev);
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Code_Man = Convert.ToInt32(r[0]);
                            textBox1.Text = r[1].ToString().TrimEnd();
                            textBox2.Text = r[3].ToString().TrimEnd();
                            textBox6.Text = r[4].ToString().TrimEnd();
                            textBox3.Text = r[2].ToString().TrimEnd();
                            Code_Rep = Convert.ToInt32(r[5]);
                            Code_Status = Convert.ToInt32(r[6]);
                            Code_Client = Convert.ToInt32(r[7]);
                        }
                    }
                }
                /*      using (SqlCommand com = conn.CreateCommand())
                      {
                          com.CommandText = string.Format("Select Namination From Manufacturer Where Manufacturer_code = " + Code_Man.ToString());

                          using (SqlDataReader r = com.ExecuteReader())
                          {
                              while (r.Read())
                              {
                                  int i = 0;
                                  while (i < 20)
                                  {
                                      MessageBox.Show(comboBox1.Item + " " + r[0].ToString().TrimEnd());
                                      if (comboBox1.Items[i].ToString().TrimEnd() == r[0].ToString().TrimEnd())
                                      {
                                          comboBox1.SelectedIndex = i;
                                          i = 20;
                                      }
                                      i++;
                                  }
                              }
                          }
                      }
                      using (SqlCommand com = conn.CreateCommand())
                      {
                          com.CommandText = string.Format("Select Nomination From Type_of_repair Where Code_type_of_repair = " + Code_Rep.ToString());
                          int i = 0;
                          using (SqlDataReader r = com.ExecuteReader())
                          {
                              while (r.Read())
                              {
                                  while (i < 100)
                                  {
                                      MessageBox.Show(comboBox3.Items[i].ToString().TrimEnd() + " " + r[0].ToString().TrimEnd());
                                      if (comboBox3.Items[i].ToString().TrimEnd() == r[0].ToString().TrimEnd())
                                      {
                                          comboBox3.SelectedIndex = i;
                                          i = 100;
                                      }
                                      i++;
                                  }
                              }
                          }
                      }
                      using (SqlCommand com = conn.CreateCommand())
                      {
                          com.CommandText = string.Format("Select Namination From Status Where Status_code = " + Code_Status.ToString());
                          int i = 0;
                          using (SqlDataReader r = com.ExecuteReader())
                          {
                              while (r.Read())
                              {
                                  while (i < 100)
                                  {
                                      if (comboBox2.Items[i].ToString().TrimEnd() == r[0].ToString().TrimEnd())
                                      {
                                          comboBox2.SelectedIndex = i;
                                          i = 100;
                                      }
                                      i++;
                                  }
                              }
                          }
                      }*/
                using (SqlCommand com = conn.CreateCommand())
                {
                    com.CommandText = string.Format("Select FIO, Phone_number From Client Where Client_code = " + Code_Client.ToString());
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            textBox4.Text = r[0].ToString().TrimEnd();
                            textBox5.Text = r[1].ToString().TrimEnd();
                        }
                    }
                }


                conn.Close();
            }
        }
    }
}
