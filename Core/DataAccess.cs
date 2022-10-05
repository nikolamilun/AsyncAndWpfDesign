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
                    int employeeID = int.Parse(sdr[0].ToString());
                    string lastName = sdr[1].ToString();
                    string firstName = sdr[2].ToString();
                    string title = sdr[3].ToString();
                    string titleOfCourtesy = sdr[4].ToString();
                    DateTime dateOfBirth = Convert.ToDateTime(sdr[5].ToString());
                    DateTime hireDate = Convert.ToDateTime(sdr[6].ToString());
                    string address = sdr[7].ToString();
                    string city = sdr[8].ToString();
                    string region = sdr[9].ToString();
                    string postalCode = sdr[10].ToString();
                    string country = sdr[11].ToString();
                    string homePhone = sdr[12].ToString();
                    string extension = sdr[13].ToString();
                    string image = sdr[14].ToString();
                    string text = sdr[15].ToString();
                    int reportsTo = (sdr[16].ToString() == "") ? 0 : int.Parse(sdr[16].ToString());
                    string photoPath = sdr[17].ToString();
                    tasks.Add(Task.Run(() =>
                    {
                        return new Employee(employeeID, lastName, firstName, title, titleOfCourtesy, dateOfBirth, hireDate, address, city, region, postalCode, country, homePhone, extension, image, text, reportsTo, photoPath);
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
    }
}
