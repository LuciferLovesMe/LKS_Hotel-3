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
    public partial class CheckIn : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int id, reser_id;
        string gender;
        public CheckIn()
        {
            InitializeComponent();
            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void getroom()
        {
            command = new SqlCommand("select * from reservationRoom join reservation on reservation.id = reservationRoom.reservationID where checkIndateTime is null and bookingCode = '" + textBox7.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                string sql = "select * from reservationRoom join reservation on reservation.id = reservationRoom.reservationID where checkIndateTime is null and bookingCode = '" + textBox7.Text + "'";
                dataGridView1.DataSource = Command.GetData(sql);
            }

            connection.Close();
            MessageBox.Show("All Rooms are Checked In or Booking Code Can't Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        void getuser()
        {
            command = new SqlCommand("select * from customer where phoneNumber = '" + textBox1.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                id = reader.GetInt32(0);
                textBox2.Text = reader.GetString(1);
                textBox3.Text = reader.GetString(3);
                gender = reader.GetString(4);
                if(gender == "Male")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                if (gender == "Female")
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
                dateTimePicker1.Value = Convert.ToDateTime(reader["dateofBirth"]);
                textBox6.Text = reader.GetString(2);
                connection.Close();
            }
            else
            {
                id = 0;
                textBox2.Text = "";
                textBox3.Text = "";
                gender = "" ;
                textBox6.Text = "";
                dateTimePicker1.Value = DateTime.Now;
                radioButton2.Checked = false;
                radioButton1.Checked = false;
            }
            connection.Close();
        }

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || dateTimePicker1.Value == null || textBox6.TextLength < 1 || !radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("All Fields Must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from customer where nik = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("NIK already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || dateTimePicker1.Value == null || textBox6.TextLength < 1 || !radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("All Fields Must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from customer where nik = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && reader.GetInt32(0) != id)
            {
                connection.Close();
                MessageBox.Show("NIK already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }

        private void panelreser_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            this.Hide();
            reservation.ShowDialog();
        }

        private void panelci_Click(object sender, EventArgs e)
        {
            CheckIn ch = new CheckIn();
            this.Hide();
            ch.ShowDialog();
        }

        private void panelreq_Click(object sender, EventArgs e)
        {
            ReqItems req = new ReqItems();
            this.Hide();
            req.ShowDialog();
        }

        private void panelcheck_Click(object sender, EventArgs e)
        {
            CheckOut check = new CheckOut();
            this.Hide();
            check.ShowDialog();
        }

        private void panelchart_Click(object sender, EventArgs e)
        {
            ReportChart report = new ReportChart();
            this.Hide();
            report.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin master = new MainLogin();
                this.Hide();
                master.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            ReportGuess report = new ReportGuess();
            this.Hide();
            report.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            getuser();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getroom();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dateTimePicker1.Value.Ticks);
            int age = Convert.ToInt32(ts.Days) / 365;
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
            else if (radioButton2.Checked)
            {
                gender = "Female";
            }
            if (id != 0 && val_up())
            {
                string sql = "update customer set name = '" + textBox2.Text + "', nik = '" + textBox6.Text + "', email = '" + textBox3.Text + "', gender = '" + gender + "', age = " + age + ", dateofBirth = '" + Convert.ToDateTime(dateTimePicker1.Value) + "' where id = " + id;
                try
                {
                    Command.exec(sql);
                    MessageBox.Show("Successfully edited customer!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else if (id == 0)
            {
                SqlCommand command = new SqlCommand("insert into customer(nik, email, name, gender, phoneNumber, age, dateofbirth) values ('" + textBox6.Text + "', '" + textBox3.Text + "', '" + textBox2.Text + "', '" + gender + "', '" + textBox1.Text + "', " + age + ", '" + Convert.ToDateTime(dateTimePicker1.Value) + "')", connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully added new customer!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Reservation reservation = new Reservation();
                    reservation.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                string sql = "update reservationRoom set checkInDatetime = getdate()";
                Command.exec(sql);
                MessageBox.Show("Successfully checked in", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
            }
            else
            {
                MessageBox.Show("Please select a room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            reser_id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }
    }
}
