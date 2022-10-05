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
                        Employee ep = new Employee(int.Parse(sdr[0].ToString()), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString(), Convert.ToDateTime((sdr[5].ToString())), Convert.ToDateTime(sdr[6].ToString()), sdr[7].ToString(), sdr[8].ToString(), sdr[9].ToString(), sdr[10].ToString(), sdr[11].ToString(), sdr[12].ToString(), sdr[13].ToString(), sdr[14].ToString(), sdr[15].ToString(), (sdr[16].ToString() == "") ? 0 : int.Parse(sdr[16].ToString()), sdr[17].ToString());

                        temp.Add(ep);
                    });
                }
            }
            return temp;
        }
        public async static Task ReturnEmployeesAsync(List<Employee> employees)
        {
            List<Task> tasks = new List<Task>();
            foreach (Employee emp in employees)
            {
                tasks.Add(Task.Run(() =>
                {
                    string cmdText = $"UPDATE Employees SET FirstName = {emp.FirstName}, LastName = {emp.LastName}, Title={emp.Title}, TitleOfCourtesy={emp.TitleOfCourtesy}, DateOfBirth={emp.DateOfBirth}, HireDate={emp.HireDate}, Address={emp.Address}, City={emp.City}, Region={emp.Region}, PostalCode={emp.PostalCode}, Country={emp.Country}, HomePhone={emp.HomePhone}, Extension={emp.Extension}, Photo={emp.Image}, Notes={emp.Text}, ReportsTo={emp.ReportsTo}, PhotoPath={emp.PhotoPath}";
                    ExecuteCommandAsync(cmdText);
                }));
            }
        }
    }
}
