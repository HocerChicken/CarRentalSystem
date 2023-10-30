using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Utils
{
    internal class Utils
    {
        public bool IsCarBooked(int carId, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(@connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Bookings WHERE CarID = @CarId AND fromDate <= GETDATE() AND toDate >= GETDATE()";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarID", carId);
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}
