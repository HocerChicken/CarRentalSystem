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

namespace CarRentalSystem
{
    public partial class Rental : Form
    {

        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlDataReader reader;
        private SqlCommandBuilder commandBuilder;

        public Rental()
        {
            InitializeComponent();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resetTextBox()
        {
            tbRentId.Text = string.Empty;
            tbName.Text = string.Empty;
            cbCarReg.Text = string.Empty;
            cBCusId.Text = string.Empty;
            tbName.Text = string.Empty;
            tbRentFee.Text = string.Empty;
        }

        private void loadRental()
        {
            try
            {
                string query = "SELECT * FROM Rental";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    rentalDGV.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillCombo()
        {
            string query = "Select RegNum from Car Where Available = 'YES'";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("RegNum", typeof(string));   
                    dt.Load(reader);
                    cbCarReg.ValueMember = "RegNum";
                    cbCarReg.DataSource = dt;
                }
            }
        }

        private void fetchCustomer()
        {
            string query = "Select CusName From Customer where CusId = @CusId";
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
            string query = "Select CusId from Customer";
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
                string query = "Update Car SET Available = @Available Where RegNum = @RegNum";

                using (SqlConnection conn = new SqlConnection(@connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegNum", cbCarReg.Text);
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
            fillCombo();
            fillCustomer();
            loadRental();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbRentId.Text) || String.IsNullOrEmpty(cbCarReg.Text)
          || String.IsNullOrEmpty(cBCusId.Text)
          || String.IsNullOrEmpty(tbName.Text)
          || String.IsNullOrEmpty(tbRentFee.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Rental(RentId, CarReg, CusName, RentDate, ReturnDate, RentFee) VALUES (@RentId, @CarReg, @CusName, @RentDate, @ReturnDate, @RentFee)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@RentId", tbRentId.Text);
                            cmd.Parameters.AddWithValue("@CarReg", cbCarReg.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@CusName", tbName.Text);
                            cmd.Parameters.AddWithValue("@RentDate", dTPRentalDate.Value.ToString());
                            cmd.Parameters.AddWithValue("@ReturnDate", dTPReturnDate.Value.ToString());
                            cmd.Parameters.AddWithValue("@RentFee", tbRentFee.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Car successfully Rented!!!");
                                UpdateAvailable();
                                fillCombo();
                                loadRental();
                                resetTextBox();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Information!!!");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbRentId.Text) || String.IsNullOrEmpty(cbCarReg.Text)
        || String.IsNullOrEmpty(cBCusId.Text)
        || String.IsNullOrEmpty(tbName.Text)
        || String.IsNullOrEmpty(tbRentFee.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "Update Rental SET" +
                        " RentId = @RentId, CarReg = @CarReg, CusName = @CusName, RentDate = @RentDate, ReturnDate = @ReturnDate, RentFee = @RentFee" +
                        " Where RentId = @RentId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@RentId", tbRentId.Text);
                            cmd.Parameters.AddWithValue("@CarReg", cbCarReg.Text);
                            cmd.Parameters.AddWithValue("@CusName", tbName.Text);
                            cmd.Parameters.AddWithValue("@RentDate", dTPRentalDate.Value.ToString());
                            cmd.Parameters.AddWithValue("@ReturnDate", dTPReturnDate.Value.ToString());
                            cmd.Parameters.AddWithValue("@RentFee", tbRentFee.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                //UpdateAvailable();
                                loadRental();
                                resetTextBox();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Information!!!");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void rentalDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = rentalDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                tbRentId.Text = rentalDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                cbCarReg.Text = rentalDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbName.Text = rentalDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                tbRentFee.Text = rentalDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbRentId.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete Rental with ID: {tbRentId.Text}", "Delete Rental", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM RENTAL WHERE RentId = @Id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@Id", tbRentId.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete successfully!!!");
                                    UpdateAvailableOnDelete();
                                    loadRental();
                                    fillCombo();
                                    resetTextBox();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Information!!!");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else if (result == DialogResult.No) { }
            }
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
                        cmd.Parameters.AddWithValue("@RegNum", cbCarReg.Text);
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
    }
}
