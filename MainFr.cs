using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class MainFr : Form
    {

        private int userId;
        private int role;
        public MainFr(int userId, int role)
        {
            InitializeComponent();
            this.userId = userId;
            this.role = role;
            CustomizeFormBasedOnRole();
        }

        private void CustomizeFormBasedOnRole()
        {
            switch(role)
            {
                case 0:
                    ShowAdminFeatures();
                    break;
                case 1:
                    ShowEmployeeFeatures();
                    break;
            }
        }

        public void ShowEmployeeFeatures()
        {
            btnUser.Hide();
            btnDashBoard.Hide();
        }

        public void ShowAdminFeatures()
        {
            btnUser.Show();
            btnDashBoard.Show();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            this.Hide();
            CarsFr car = new CarsFr(this, role);
            car.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomersFr customer = new CustomersFr(this, role);
            customer.Show();
        }

        private void btnBookings_Click(object sender, EventArgs e)
        {
            this.Hide();
            BookingsFr bookingFr = new BookingsFr(this, role);
            bookingFr.Show();
        }

        private void btnSchedules_Click(object sender, EventArgs e)
        {
            this.Hide();
            SchedulesFr ccheduleFr = new SchedulesFr(this, role);
            ccheduleFr.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginFr loginFr = new LoginFr();
            loginFr.Show(); 
        }

        private void btnUser_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            UsersFr user = new UsersFr(this, role);
            user.Show();
        }

        private void btnDashBoard_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            DashBoardFr dashboardFr = new DashBoardFr(this, role);
            dashboardFr.Show();
        }
    }
}
