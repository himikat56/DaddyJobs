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
    public partial class Edit_directory : Form
    {
        string GlobalConnection = "Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True";
        public Edit_directory()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Производители":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Manufacturer_code as 'Код',Namination as 'Наименование' from Manufacturer ", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Manufacturer");
                        dataGridView1.DataSource = ds.Tables["Manufacturer"];
                        conn.Close();
                    }
                    break;
                case "Статусы":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Status_code as 'Код',Namination as 'Наименование' from Status", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Status");
                        dataGridView1.DataSource = ds.Tables["Status"];
                        conn.Close();
                    }
                    break;
                case "Работы":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Code_name_of_work as 'Код',Name as 'Наименование',Cost as 'Стоимость' from Name_of_work ", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Name_of_work");
                        dataGridView1.DataSource = ds.Tables["Name_of_work"];
                        conn.Close();
                    }
                    break;
                case "Запчасти":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Spare_part_code as 'Код', Namination as 'Наименование', Cost as 'Стоимость' from Spare_part ", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Spare_part");
                        dataGridView1.DataSource = ds.Tables["Spare_part"];
                        conn.Close();
                    }
                    break;
                case "Работники":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Employee_code as 'Код', FIO as 'ФИО', Date_of_birth as 'Дата рождения', Addres as 'Адрес', Namination as 'Должность',Date_of_receipt as 'Дата приема', Date_of_dismissal as 'Дата увольнения' from  Position, Employee 
where Position.Position_code = Employee.Position_code", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Position, Employee");
                        dataGridView1.DataSource = ds.Tables["Position, Employee"];
                        conn.Close();
                    }
                    break;
                case "Должности":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Position_code as 'Код', Namination as 'Наименование' from Position", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Position");
                        dataGridView1.DataSource = ds.Tables["Position"];
                        conn.Close();
                    }
                    break;
                case "Типы ремонта":
                    using (SqlConnection conn = new SqlConnection(GlobalConnection))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(@"Select Code_type_of_repair as 'Код', Nomination as 'Наименование' from Type_of_repair", conn);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Type_of_repair");
                        dataGridView1.DataSource = ds.Tables["Type_of_repair"];
                        conn.Close();
                    }
                    break;

            }
        }
    }
}
