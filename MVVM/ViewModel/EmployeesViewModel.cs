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
            Task.Run(() => LoadDataAsync());
        }

        private async Task LoadDataAsync()
        {
            try
            {
                ProgressReportModel report = new ProgressReportModel();
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-0KT9RPJ;Initial Catalog=Northwind;Integrated Security=True"))
                {
                    conn.Open();

                    SqlCommand selectCmd = new SqlCommand("SELECT * FROM Employees", conn);
                    SqlDataReader sdr = selectCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        await Task.Run(() =>
                        {
                            Employee ep =  new Employee(int.Parse(sdr[0].ToString()), sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString(), Convert.ToDateTime((sdr[5].ToString())), Convert.ToDateTime(sdr[6].ToString()), sdr[7].ToString(), sdr[8].ToString(), sdr[9].ToString(), sdr[10].ToString(), sdr[11].ToString(), sdr[12].ToString(), sdr[13].ToString(), sdr[14].ToString(), sdr[15].ToString(), (sdr[16].ToString() == "") ? 0 : int.Parse(sdr[16].ToString()), sdr[17].ToString());

                            Employees.Add(ep);
                        });
                        
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
