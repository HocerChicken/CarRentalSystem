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
using System.Xml;

namespace CarRentalSystem
{
    public partial class BookingsFr : Form
    {

        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlDataReader reader;
        private SqlCommandBuilder commandBuilder;
        private int price = 0;

        public BookingsFr()
        {
            InitializeComponent();
        }

        private void resetTextBox()
        {
            tbBookingId.Text = string.Empty;
            tbName.Text = string.Empty;
            cbCarId.Text = string.Empty;
            cBCusId.Text = string.Empty;
            tbName.Text = string.Empty;
            //tbRentFee.Text = string.Empty;
        }

        private void loadRental()
        {
            try
            {
                string query = "SELECT * FROM Bookings";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    bookingDGV.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillComboCarId()
        {
            string query = "Select CarId from Cars Where available = 'YES'";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("carId", typeof(int));
                    dt.Load(reader);
                    cbCarId.ValueMember = "carId";
                    cbCarId.DataSource = dt;
                }
            }
        }

        private void fillTextBoxCarInfo()
        {
            string query2 = "Select * From Cars Where CarId = @CarId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    cmd.Parameters.AddWithValue("@CarId", cbCarId.Text);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string carBrand = reader["brand"].ToString();
                        string carModel = reader["model"].ToString();
                        //string carCategory = reader["category"].ToString();
                        tbModel.Text = carModel;
                        tbBrand.Text = carBrand;
                    }
                }
            }
        }

        private void fetchCustomer()
        {
            string query = "Select CusName From Customers where CusId = @CusId";
            using (SqlConnection conn = new SqlConnection(connectionString))                                       
            {
                conn.Open();
                using(SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CusId", cBCusId.Text);    
                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach(DataRow row in dt.Rows)
                    {
                        tbName.Text = row["CusName"].ToString();
                    }
                }
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
                    cBCusId.ValueMember = "CusId";
                    cBCusId.DataSource = dt;
                }
            }
        }

        private void UpdateAvailable()
        {
            try
            {
                string query = "Update Cars SET Available = @Available Where CarId = @CarId";

                using (SqlConnection conn = new SqlConnection(@connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarId", cBCusId.Text);
                        cmd.Parameters.AddWithValue("@Available", "NO");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Rental_Load(object sender, EventArgs e)
        {
            tbBookingId.Hide();
            fillComboCarId();
            fillCustomer();
            loadRental();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void rentalDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = bookingDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                tbBookingId.Text = bookingDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                cbCarId.Text = bookingDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbName.Text = bookingDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                //tbRentFee.Text = rentalDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void UpdateAvailableOnDelete()
        {
            try
            {
                string query = "Update Car SET Available = @Available Where RegNum = @RegNum";

                using (SqlConnection conn = new SqlConnection(@connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegNum", cbCarId.Text);
                        cmd.Parameters.AddWithValue("@Available", "YES");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void cBCusId_SelectedValueChanged(object sender, EventArgs e)
        {
            fetchCustomer();
        }

        private void cbCarId_SelectedValueChanged(object sender, EventArgs e)
        {
            fillTextBoxCarInfo();   
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
