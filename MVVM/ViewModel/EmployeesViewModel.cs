using AsyncApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AsyncApp.Model;
using System.Windows.Controls;
using System.Threading;
using System.Drawing;
using System.IO;

namespace AsyncApp.ViewModel
{
    class EmployeesViewModel : ObservableObject
    {
        private List<Employee> _employees = new List<Employee>();

        public List<Employee> Employees
        {
            get { return _employees; }
            set
            {
                OnPropertyChanged();
                _employees = value; 
            }
        }

        public EmployeesViewModel()
        {
            Task.Run(() => LoadDataAsync().Wait());
        }

        private async Task LoadDataAsync()
        {
            try
            {
                List<Task<Employee>> tasks = new List<Task<Employee>>();
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-0KT9RPJ;Initial Catalog=Northwind;Integrated Security=True"))
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
                        Employees.Add(employee);
                    }
                }
            }
            catch (Exception)
            {
                Employees = null;
            }
        }
    }
}
