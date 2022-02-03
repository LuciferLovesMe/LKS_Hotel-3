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
    public partial class CheckOut : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        int reser_id, fdc;
        public CheckOut()
        {
            InitializeComponent();
            loadroom();
            loadstatus();

            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void loadroom()
        {
            string sql = "select room.roomNumber, reservationRoom.id from room join reservationRoom on room.id = reservationRoom.roomId where status = 'unavail'";
            comboBox1.DataSource = Command.GetData(sql);
            comboBox1.DisplayMember = "roomNumber";
            comboBox1.ValueMember = "id";
        }

        void loaditem()
        {
            string sql = "select item.id, item.name from reservationRequestitem join reservationRoom on reservationRequestItem.reservationRoomid = reservationRoom.id join item on reservationRequestItem.itemId = item.id where reservationRoom.id = " + reser_id;
            comboBox2.DataSource = Command.GetData(sql);
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "name";

            getsub();
        }

        void getsub()
        {
            if(comboBox2.Text.Length > 0)
            {
                command = new SqlCommand("select * from item where name = '" + comboBox2.Text + "'", connection);
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    tbcomp.Text = (Convert.ToInt32(reader["compensationFee"]) * numericUpDown1.Value).ToString();
                    txtsub.Text = (Convert.ToInt32(reader["requestPrice"]) * numericUpDown1.Value).ToString();
                }
                connection.Close();
            }
        }

        void loadstatus()
        {
            string sql = "select * from itemstatus";
            comboBox3.DataSource = Command.GetData(sql);
            comboBox3.ValueMember = "id";
            comboBox3.DisplayMember = "name";
        }

        void loadfd()
        {
            string sql = "select * from fdCheckout where reservationRoomId = " + reser_id;
            dataGridView2.DataSource = Command.GetData(sql);

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

        private void button4_Click(object sender, EventArgs e)
        {
            reser_id = Convert.ToInt32(comboBox1.SelectedValue);
            loaditem();
            loadfd();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(comboBox2.SelectedValue) != 0 && numericUpDown1.Value > 0)
            {
                int rows = dataGridView1.Rows.Add();
                int charge = 0;
                if(comboBox3.Text.ToLower() == "good")
                {
                    charge = 0;
                }
                else
                {
                    charge = Convert.ToInt32(tbcomp.Text);
                }
                dataGridView1.Rows[rows].Cells[0].Value = Convert.ToInt32(comboBox1.SelectedValue);
                dataGridView1.Rows[rows].Cells[1].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[rows].Cells[2].Value = comboBox2.Text;
                dataGridView1.Rows[rows].Cells[3].Value = comboBox3.SelectedValue;
                dataGridView1.Rows[rows].Cells[4].Value = comboBox3.Text;
                dataGridView1.Rows[rows].Cells[5].Value = numericUpDown1.Value;
                dataGridView1.Rows[rows].Cells[6].Value = txtsub.Text;
                dataGridView1.Rows[rows].Cells[7].Value = charge;
            }
            else
            {
                MessageBox.Show("Please select an item and Quantity must be more than 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            getsub();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            getsub();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(fdc == 0 || !dataGridView2.CurrentRow.Selected)
            {
                string sql = "delete from fdcheckout where id = " + fdc;
                DialogResult result = MessageBox.Show("Are you sure to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    Command.exec(sql);
                    loadfd();
                    fdc = 0;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(reser_id == 0)
            {
                MessageBox.Show("Please select a room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for(int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string sql = "insert into reservationCheckOut values("+reser_id+",)"

                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            fdc = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
        }
    }
}
