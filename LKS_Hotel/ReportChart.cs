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
    public partial class ReportChart : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;

        public ReportChart()
        {
            InitializeComponent();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
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

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            command = new SqlCommand("select month, count(*) as num from reservation where year = " + dateTimePicker1.Text + " group by month", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                chart1.Series["Total"].Points.AddXY(reader.GetString(0), reader.GetInt32(1));
            }
            connection.Close();
        }
    }
}
