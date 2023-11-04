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
using System.Xml.Linq;

namespace CarRentalSystem
{
    public partial class SchedulesFr : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlDataReader reader;
        private SqlCommandBuilder commandBuilder;

        public SchedulesFr()
        {
            InitializeComponent();
        }

        private void loadScheduleById(int id)
        {
            try
            {
                string query = "SELECT B.fromDate AS StartDate, B.toDate AS EndDate, C.brand AS CarBrand, C.model AS CarModel, C.category AS Seats, S.fromPlace AS PickupLocation, S.toPlace AS ReturnLocation, B.totalCost AS TotalCarCost " +
                    "FROM Bookings AS B INNER JOIN Cars AS C ON B.carId = C.carId INNER JOIN Schedules AS S ON B.bookingId = S.bookingId " +
                    "WHERE B.cusId = @cusId";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@CusId", id);
                        adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        schedulesDGV.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillCustomer()
        {
            string query = "Select CusId from Customers";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CusId", typeof(int));
                    dt.Load(reader);
                    cBCusId2.ValueMember = "CusId";
                    cBCusId2.DataSource = dt;
                }
            }
            cBCusId2.SelectedValueChanged += cBCusId_SelectedValueChanged;
        }


        private void fetchCustomer()
        {
            string query = "Select CusName From Customers where CusId = @CusId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CusId", cBCusId2.Text);
                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        tbName.Text = row["CusName"].ToString();
                    }
                }
            }
            
        }

        private void bookingDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = schedulesDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                dtpReturnDate.Text = schedulesDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                DateTime d1 = dtpReturnDate.Value.Date;
                DateTime d2 = DateTime.Now;
                TimeSpan ts = d2 - d1;
                int delayDays = (int)ts.TotalDays;
                if (delayDays <= 0)
                {
                    tbDateDelay.Text = "No Delay";
                    tbFineCost.Text = "0";
                }
                else
                {
                    tbDateDelay.Text = delayDays.ToString();
                    tbFineCost.Text = "" + delayDays * 250;
                }
            }
        }

        private void lbExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SchedulesFr_Load(object sender, EventArgs e)
        {
            cBCusId2.SelectedValueChanged -= cBCusId_SelectedValueChanged;
            fillCustomer();
        }

        private void cBCusId_SelectedValueChanged(object sender, EventArgs e)
        {
            fetchCustomer();
            if (int.TryParse(cBCusId2.Text, out int id))
            {
                fetchCustomer();
                loadScheduleById(id);
            }
            else
            {
                MessageBox.Show("Invalid customer ID. Please enter a valid numeric value.");
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {

        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
    }
}
