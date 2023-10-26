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
    public partial class Return : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlDataReader reader;
        private SqlCommandBuilder commandBuilder;

        public Return()
        {
            InitializeComponent();
        }

        private void Return_Load(object sender, EventArgs e)
        {
            tbFine.Enabled = false;
            loadReturns();
            loadRentals();
        }

        private void loadRentals()
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

        private void loadReturns()
        {
            try
            {
                string query = "SELECT * FROM ReturnTB";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    returnDGV.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                tbCarReg.Text = rentalDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbName.Text = rentalDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                dTPReturnDate.Text = rentalDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
                DateTime d1 = dTPReturnDate.Value.Date;
                DateTime d2 = DateTime.Now;
                TimeSpan ts = d2 - d1;
                int delayDays = (int) ts.TotalDays;
                if(delayDays <= 0)
                {
                    tbDelay.Text = "No Delay";
                    tbFine.Text = "0";
                } else
                {
                    tbDelay.Text = delayDays.ToString();
                    tbFine.Text = "" + delayDays * 250 ;
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(tbReturnId.Text) || String.IsNullOrEmpty(tbCarReg.Text)
           || String.IsNullOrEmpty(dTPReturnDate.Text)
           || String.IsNullOrEmpty(tbDelay.Text)
           || String.IsNullOrEmpty(tbFine.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO ReturnTb(ReturnId, CarReg, CusName, ReturnDate, Delay, Fine) VALUES (@ReturnId, @CarReg, @CusName, @ReturnDate, @Delay, @Fine)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ReturnId", tbReturnId.Text);
                            cmd.Parameters.AddWithValue("@CarReg", tbCarReg.Text);
                            cmd.Parameters.AddWithValue("@CusName", tbName.Text);
                            cmd.Parameters.AddWithValue("@ReturnDate", dTPReturnDate.Value.ToString());
                            cmd.Parameters.AddWithValue("@Delay", tbDelay.Text);
                            cmd.Parameters.AddWithValue("@Fine", tbFine.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Car returned!!!");
                                loadReturns();
                                deleteOnReturn();
                                //resetTextBox();
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

        private void deleteOnReturn()
        {
            if (String.IsNullOrEmpty(tbReturnId.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                string query = "DELETE FROM RENTAL WHERE RentId = @Id";
                try
                {
                    using (SqlConnection con = new SqlConnection(@connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(rentalDGV.SelectedCells[0].Value.ToString()));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToInt32(rentalDGV.SelectedRows[0].Cells[0].Value.ToString()).ToString());
        }

        private void returnDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = rentalDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            //if (e.RowIndex >= 0)
            //{
            //    tbCarReg.Text = rentalDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            //    tbName.Text = rentalDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            //    dTPReturnDate.Text = rentalDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
            //    DateTime d1 = dTPReturnDate.Value.Date;
            //    DateTime d2 = DateTime.Now;
            //    TimeSpan ts = d2 - d1;
            //    int delayDays = (int)ts.TotalDays;
            //    if (delayDays <= 0)
            //    {
            //        tbDelay.Text = "No Delay";
            //        tbFine.Text = "No Fine";
            //    }
            //    else
            //    {
            //        tbDelay.Text = delayDays.ToString();
            //        tbFine.Text = "" + delayDays * 250;
            //    }
            //}
        }
    }
}
