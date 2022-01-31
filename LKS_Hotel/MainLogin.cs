using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Hotel
{
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MainLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string pass = Encrypt.enc(textBox2.Text);
                SqlCommand command = new SqlCommand("select * from employee where username = '"+textBox1.Text+"' and password = @pass", connection);
                connection.Open();
                command.Parameters.AddWithValue("@pass", pass);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Session.id = Convert.ToInt32(reader["id"]);
                    Session.jobId = Convert.ToInt32(reader["jobId"]);
                    Session.name = reader["name"].ToString();
                    Session.username = reader["username"].ToString();
                    Session.email = reader["email"].ToString();
                    Session.address = reader["address"].ToString();
                    connection.Close();
                    if(Session.id == 1)
                    {
                        MainAdmin ma = new MainAdmin();
                        this.Hide();
                        ma.ShowDialog();
                    }
                    else if(Session.id == 2)
                    {
                        MainFrontOffice ma = new MainFrontOffice();
                        this.Hide();
                        ma.ShowDialog();
                    }
                }
                connection.Close();
                MessageBox.Show("User Can't Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
