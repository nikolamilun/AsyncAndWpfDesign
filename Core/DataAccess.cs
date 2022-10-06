using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncApp.Model;
using System.Data.SqlClient;

namespace AsyncApp.Core
{
    public class DataAccess
    {
        private static string serverName = @".\SQLEXPRESS";
        private static string connString = @"Data Source=" + serverName + ";Initial Catalog=Northwind;Integrated Security=True";

        #region Database access

        private async static Task ExecuteCommandAsync(string cmdText)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.ExecuteNonQueryAsync();
            }
        }
        public async static Task<List<Employee>> GetEmployeesParallelAsync()
        {
            List<Employee> output = new List<Employee>();
            List<Task<Employee>> tasks = new List<Task<Employee>>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand selectCmd = new SqlCommand("SELECT * FROM Employees", conn);
                SqlDataReader sdr = await selectCmd.ExecuteReaderAsync();

                while (sdr.Read())
                {
                    int employeeID = sdr.GetInt32(0);
                    string lastName = sdr.GetString(1);
                    string firstName = sdr.GetString(2);
                    string title = sdr[3].ToString();
                    string titleOfCourtesy = sdr.GetString(4);
                    DateTime dateOfBirth = sdr.GetDateTime(5);
                    DateTime hireDate = sdr.GetDateTime(6);
                    string address = sdr.GetString(7);
                    string city = sdr.GetString(8);
                    string region = sdr.GetString(9);
                    string postalCode = sdr.GetString(10);
                    string country = sdr.GetString(11);
                    string homePhone = sdr.GetString(12);
                    string extension = sdr.GetString(13);
                    string text = sdr.GetString(15);
                    int reportsTo = sdr.GetInt32(16);
                    string photoPath = sdr.GetString(17);
                    tasks.Add(Task.Run(() =>
                    {
                        return new Employee(employeeID, lastName, firstName, title, titleOfCourtesy, dateOfBirth, hireDate, address, city, region, postalCode, country, homePhone, extension, text, reportsTo, photoPath);
                    }));
                }

                var results = await Task.WhenAll(tasks);

                foreach (Employee employee in results)
                {
                    output.Add(employee);
                }

                return output;
            }
        }
        public async static Task<List<Employee>> GetEmployeesAsync()
        {
            List<Employee> temp = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand selectCmd = new SqlCommand("SELECT * FROM Employees", conn);
                SqlDataReader sdr = selectCmd.ExecuteReader();

                while (sdr.Read())
                {
                    await Task.Run(() =>
                    {
                        Employee ep = new Employee(SafelyGetInt32(sdr, 0), SafelyGetString(sdr, 1), SafelyGetString(sdr, 2), SafelyGetString(sdr, 3), SafelyGetString(sdr, 4), SafelyGetDateTime(sdr, 5), SafelyGetDateTime(sdr, 6), SafelyGetString(sdr, 7), SafelyGetString(sdr, 8), SafelyGetString(sdr, 9), SafelyGetString(sdr, 10), SafelyGetString(sdr, 11), SafelyGetString(sdr, 12), SafelyGetString(sdr, 13), SafelyGetString(sdr, 15), SafelyGetInt32(sdr, 16), SafelyGetString(sdr, 17));

                        temp.Add(ep);
                    });
                }

                return temp;
            }
        }
        public async static Task ReturnEmployeesAsync(List<Employee> employees)
        {
            List<Task> tasks = new List<Task>();
            foreach (Employee emp in employees)
            {
                tasks.Add(Task.Run(async() =>
                {
                    string cmdText = $"UPDATE Employees SET FirstName = {emp.FirstName}, LastName = {emp.LastName}, Title={emp.Title}, TitleOfCourtesy={emp.TitleOfCourtesy}, DateOfBirth={emp.DateOfBirth}, HireDate={emp.HireDate}, Address={emp.Address}, City={emp.City}, Region={emp.Region}, PostalCode={emp.PostalCode}, Country={emp.Country}, HomePhone={emp.HomePhone}, Extension={emp.Extension}, Notes={emp.Text}, ReportsTo={emp.ReportsTo}, PhotoPath={emp.PhotoPath}";
                    await ExecuteCommandAsync(cmdText);
                }));
            }
        }

        #endregion

        #region Additional functions
        private static int SafelyGetInt32(SqlDataReader sdr, int i)
        {
            if (sdr.IsDBNull(i))
                return 0;
            return sdr.GetInt32(i);
        }
        private static string SafelyGetString(SqlDataReader sdr, int i)
        {
            if (sdr.IsDBNull(i))
                return "";
            return sdr.GetString(i);
        }
        private static DateTime SafelyGetDateTime(SqlDataReader sdr, int i)
        {
            if (sdr.IsDBNull(i))
                return new DateTime(0);
            return sdr.GetDateTime(i);
        }
        #endregion
    }
}
