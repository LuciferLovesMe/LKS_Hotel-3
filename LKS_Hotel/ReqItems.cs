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
    public partial class ReqItems : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public ReqItems()
        {
            InitializeComponent();
            loadroom();
            loaditems();

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void loadroom()
        {
            string com = "select reservationRoom.id, room.roomNumber from reservationRoom join room on reservationRoom.roomId = room.id";
            comboBox1.DataSource = Command.GetData(com);
            comboBox1.DisplayMember = "roomNumber";
            comboBox1.ValueMember = "id";
        }

        void loaditems()
        {
            string com = "select * from item";
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = Command.GetData(com);
        }

        void loaddetail()
        {
            command = new SqlCommand("select * from item where id = "+comboBox2.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            textBox1.Text = reader.GetInt32(2).ToString();
            textBox2.Text = (Convert.ToInt32(textBox1.Text) * numericUpDown1.Value).ToString();
            connection.Close();

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loaddetail();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(comboBox2.SelectedValue.ToString()) != 0)
            {
                loaddetail();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text.Length <1 || comboBox2.Text.Length < 1)
            {
                MessageBox.Show("Please select a room and an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numericUpDown1.Value < 1)
            {
                MessageBox.Show("Quantity values at least has 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = comboBox1.SelectedValue;
                dataGridView1.Rows[row].Cells[1].Value = comboBox1.Text;
                dataGridView1.Rows[row].Cells[2].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[row].Cells[3].Value = comboBox2.Text;
                dataGridView1.Rows[row].Cells[4].Value = textBox1.Text;
                dataGridView1.Rows[row].Cells[5].Value = numericUpDown1.Value;
                dataGridView1.Rows[row].Cells[6].Value = textBox2.Text;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string sql = "insert into reservationrequestitem values(" + dataGridView1.Rows[i].Cells[0].Value + ", " + dataGridView1.Rows[i].Cells[2].Value + ", " + dataGridView1.Rows[i].Cells[5].Value + ", " + dataGridView1.Rows[i].Cells[6].Value + ")";
                    try
                    {
                        Command.exec(sql);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        numericUpDown1.Value = 0;
                        loaddetail();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
