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
using Microsoft.VisualBasic;

namespace LKS_Hotel
{
    public partial class SelectRoom : Form
    {
        int roomtypeid;
        int total = 0;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        public SelectRoom()
        {
            InitializeComponent();
            loadtype();
            getcode();

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
            lblcustomer.Text = "Customer's Name : " + Selected.name;
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
        private void label11_Click(object sender, EventArgs e)
        {
            ReportGuess report = new ReportGuess();
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

        void gettotal()
        {
            for(int i = 0; i < dgv_selected.RowCount; i++)
            {
                total += Convert.ToInt32(dgv_selected.Rows[i].Cells[3].Value);
            }

            lbltotal.Text = "Rp. " + total.ToString();
        }
        string getcode()
        {
            command = new SqlCommand("select top(1) * from reservation order by id desc", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            string code;
            if (reader.HasRows)
            {
                int id = reader.GetInt32(0);
                connection.Close();
                string s = "00000";
                code = "B" + s.Substring(0, s.Length - id.ToString().Length) + (id + 1).ToString();
            }
            else
                code = "B00001";

            labelcode.Text = code;
            connection.Close();
            return code;
        }

        bool val()
        {
            if(dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Check in must be less than check out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(dgv_selected.RowCount < 1)
            {
                MessageBox.Show("Please select a room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        void loadtype()
        {
            string sql = "select * from roomtype";
            comboBox1.DataSource = Command.GetData(sql);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        private void dgv_avail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv_avail.CurrentRow.Selected = true;
            textBox1.Text = dgv_avail.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dgv_avail.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dgv_avail.SelectedRows[0].Cells[3].Value.ToString();
            textBox4.Text = dgv_avail.SelectedRows[0].Cells[8].Value.ToString();
            textBox5.Text = lblstay.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dgv_avail.CurrentRow.Selected && Convert.ToInt32(lblstay.Text) > 0)
            {
                int rows = dgv_selected.Rows.Add();
                dgv_selected.Rows[rows].Cells[0].Value = Convert.ToInt32(textBox1.Text);
                dgv_selected.Rows[rows].Cells[1].Value = textBox2.Text;
                dgv_selected.Rows[rows].Cells[2].Value = textBox3.Text;
                dgv_selected.Rows[rows].Cells[4].Value = lblstay.Text;
                dgv_selected.Rows[rows].Cells[3].Value = (Convert.ToInt32(textBox4.Text) * Convert.ToInt32(lblstay.Text)).ToString();
                gettotal();
            }
        }

        int getstay()
        {
            TimeSpan ts = new TimeSpan(dateTimePicker2.Value.Ticks - dateTimePicker1.Value.Ticks);
            int stay = Convert.ToInt32(ts.Days);
            lblstay.Text = stay.ToString();
            gettotal();
            return stay;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getstay();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            getstay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "select * from avail_room where roomtypeId = " + comboBox1.SelectedValue;
            dgv_avail.DataSource = Command.GetData(sql);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dgv_selected.CurrentRow.Selected)
            {
                dgv_selected.Rows.Remove(dgv_selected.SelectedRows[0]);
            }
            gettotal();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(val())
            {
                string sql = "insert into reservation values(getdate(), " + Session.id + ", " + Selected.id + ", '" + labelcode.Text + "', '" + DateTime.Now.ToString("MMMM") + "', '" + DateTime.Now.ToString("yyyy") + "')";
                Command.exec(sql);
                command = new SqlCommand("select top(1) * from reservation order by id desc", connection);
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                int res_id = reader.GetInt32(0);
                connection.Close();

                for(int i = 0; i < dgv_selected.RowCount; i++)
                {
                    sql = "insert into reservationRoom values(" + res_id + ", " + dgv_selected.Rows[i].Cells[0].Value + ", getdate(), " + Convert.ToInt32(dgv_selected.Rows[i].Cells[3].Value) + ", " + dgv_selected.Rows[i].Cells[4].Value + ", '" + Convert.ToDateTime(dateTimePicker1.Value) + "', '" + Convert.ToDateTime(dateTimePicker2.Value) + "')";
                    Command.exec(sql);

                    sql = "update room set status = 'unavail' where id = " + dgv_selected.Rows[i].Cells[0].Value;
                    Command.exec(sql);
                }

                MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReqItems items = new ReqItems();
                this.Hide();
                items.ShowDialog();
            }
        }

        private void dgv_selected_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv_selected.CurrentRow.Selected = true;
        }
    }
}
