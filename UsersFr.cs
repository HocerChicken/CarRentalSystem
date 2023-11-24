﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CarRentalSystem
{
    public partial class UsersFr : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;
        private MainFr mainFr;
        private int role;
        public UsersFr(MainFr mainFr, int role)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this.role = role;
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadUser()
        {
            try
            {
                string query = "SELECT * FROM Users";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    userDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetTextBox()
        {
            tbUsername.Text = String.Empty;
            tbPassword.Text = String.Empty;
            tbRole.Text = String.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUsername.Text) || String.IsNullOrEmpty(tbPassword.Text)
                || String.IsNullOrEmpty(tbRole.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Users(username, userpassword, role) VALUES (@username, @userpassword, @role)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", tbUsername.Text);
                            cmd.Parameters.AddWithValue("@userpassword", tbPassword.Text);
                            cmd.Parameters.AddWithValue("@role", tbRole.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("User successfully Added!!!");
                                loadUser();
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

        private void userDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = userDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                tbUserId.Text = userDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbUsername.Text = userDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbPassword.Text = userDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                tbRole.Text = userDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        } 

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUserId.Text) || String.IsNullOrEmpty(tbUsername.Text) || String.IsNullOrEmpty(tbPassword.Text)
                || String.IsNullOrEmpty(tbRole.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "Update Users SET Username = @Username, userpassword = @userpassword, role = @role Where userId = @userId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@userId", tbUserId.Text);
                            cmd.Parameters.AddWithValue("@Username", tbUsername.Text);
                            cmd.Parameters.AddWithValue("@Userpassword", tbPassword.Text);
                            cmd.Parameters.AddWithValue("@role", tbRole.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                loadUser();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUserId.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete user with ID: {tbUserId.Text}", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM USERS WHERE userId = @userId";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@userId", tbUserId.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete user successfully!!!");
                                    loadUser();
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            switch (role)
            {
                case 0:
                    this.Hide();
                    mainFr.Show();
                    mainFr.ShowAdminFeatures();
                    break;
                case 1:
                    this.Hide();
                    mainFr.Show();
                    mainFr.ShowEmployeeFeatures();
                    break;
            }
        }

        private void User_Load(object sender, EventArgs e)
        {
            tbUserId.Hide();
            loadUser();
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)userDGV.DataSource;
            Export(dt);
        }

        public void Export(DataTable tbl)
        {
            {
                try
                {
                    if (tbl == null || tbl.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    var excelApp = new Excel.Application();
                    var workbook = excelApp.Workbooks.Add();

                    Excel._Worksheet workSheet = excelApp.ActiveSheet;

                    for (var i = 0; i < tbl.Columns.Count; i++)
                    {
                        workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                    }

                    for (var i = 0; i < tbl.Rows.Count; i++)
                    {
                        for (var j = 0; j < tbl.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                        }
                    }

                    try
                    {
                        var saveFileDialog = new SaveFileDialog();
                        saveFileDialog.FileName = "CarData";
                        saveFileDialog.DefaultExt = ".xlsx";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        }
                        excelApp.Quit();
                        Console.WriteLine("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                        + ex.Message);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: \n" + ex.Message);
                }
            }
        }
    }
}
