using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Daddy_Jobs
{
    public partial class Authorization_form : Form
    {

        private Class1 model;
        int BadPassword = 3;
        //static string connectionString = @"Data Source=HIMIKAT;Initial Catalog=DaddyJobs;Integrated Security=True";
        public Authorization_form()
        {
            this.model = new Class1();
            InitializeComponent();
            var source = new AutoCompleteStringCollection();
            using (SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand com = conn.CreateCommand())
                {
                    com.CommandText = string.Format("SELECT login FROM Users");
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            source.Add((string)r[0]);// Результаты запроса 
                        }
                    }
                }
                conn.Close();
                comboBox1.DataSource = source;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "daddyJobsDataSet2.Users". При необходимости она может быть перемещена или удалена.
            this.usersTableAdapter1.Fill(this.daddyJobsDataSet2.Users);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "daddyJobsDataSet11.Users". При необходимости она может быть перемещена или удалена.
            //this.usersTableAdapter.Fill(this.daddyJobsDataSet11.Users);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "daddyJobsDataSet.Сотрудники". При необходимости она может быть перемещена или удалена.
            // this.сотрудникиTableAdapter.Fill(this.daddyJobsDataSet.Сотрудники);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "daddyJobsDataSet.Работы". При необходимости она может быть перемещена или удалена.
            // this.работыTableAdapter.Fill(this.daddyJobsDataSet.Работы);


        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


public string GetPassword(string param)
        {
            string temp = "";
            using (SqlConnection conn = new SqlConnection("Data Source=TESTHDD\\SQLEXPRESS;Initial Catalog=DaddyJobs;Integrated Security=True"))
            {
                conn.Open();
                using (SqlCommand com = conn.CreateCommand())
                {
                    com.CommandText = string.Format("SELECT password FROM Users Where login = '{0}'", param);
                    using (SqlDataReader r = com.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            
                            temp += Regex.Replace((string)r[0], "[ ]+", " ");// Результаты запроса 
                        }

                    }
                    conn.Close();
                }
                return temp;
            }
        }

        // добавление пользователя
        /*        private static void AddClient(string fio, string number, string dok)
                {
                    // название процедуры
                    string sqlExpression = "ADDCLIENT";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter fioParam = new SqlParameter
                        {
                            ParameterName = "@fio",
                            Value = fio
                        };
                        command.Parameters.Add(fioParam);
                        SqlParameter dokParam = new SqlParameter
                        {
                            ParameterName = "@dok",
                            Value = dok
                        };
                        command.Parameters.Add(dokParam);
                        SqlParameter numberParam = new SqlParameter
                        {
                            ParameterName = "@number",
                            Value = number
                        };
                        command.Parameters.Add(numberParam);


                        var result = command.ExecuteScalar();

                        // если нам не надо возвращать id
                        //var result = command.ExecuteNonQuery();

                      //  Console.WriteLine("Id добавленного объекта: {0}", result);
                    }
                }


                private static void GetUsers()
                {
                    // название процедуры
                    string sqlExpression = "GetClient";
                    var source = new AutoCompleteStringCollection();
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        // указываем, что команда представляет хранимую процедуру
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                             string name0 = reader.GetName(0);
                            source.Add(name0);

                            while (reader.Read())
                            {
                                string name = reader.GetString(1);
                                source.Add(name);
                            }
                        }
                        reader.Close();

                    }
                }*/


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://xn--80aadlk2ccbx.xn--p1ai/");
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (MD5 md5Hash = MD5.Create())
            {

                if (VerifyMd5Hash(md5Hash, textBox1.Text, GetPassword(comboBox1.Text)))
                {
                    model.setLogin(comboBox1.Text);
                    model.show();
                    this.Hide();
                    textBox1.Text = "";                   
                }
                else
                {
                    if (BadPassword >= 1)
                    {
                        BadPassword--;
                        textBox1.Text = "";
                        MessageBox.Show("Пароль введен не верно!\nОсталось попыток " + BadPassword.ToString(),"Ошибка !");
                    }else
                    {
                        MessageBox.Show("Попытки ввода пароля исчерпаны!");
                        this.Close();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
