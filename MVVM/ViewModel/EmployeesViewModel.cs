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
            

        }

        private async Task<Employee> LoadData()
        {
            List<Task<Employee>> list = new List<Task<Employee>>();
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-2571GNM\SQLEXPRESS;Integrated Security=True;"))
            {
                SqlCommand selectCmd = new SqlCommand("SELECT * FROM Employees", conn);
                SqlDataReader sdr = selectCmd.ExecuteReader();

                while (sdr.Read())
                {
                    list.Add(Task.Run(() => 
                        {
                            return new Employee((int)sdr[0], sdr[1].ToString(), sdr[2].ToString(), sdr[3].ToString(), sdr[4].ToString(), (DateTime)sdr[5], (DateTime)sdr[6], sdr[7].ToString(), sdr[8].ToString(), sdr[9].ToString(), sdr[10].ToString(), sdr[11].ToString(), sdr[12].ToString(), sdr[13].ToString(), (Image)sdr[14], sdr[15].ToString(), (int)sdr[16], sdr[17].ToString());
                        }
                    ));
                }
            }

            var results = await Task.WhenAll(list);

            return results;
        }
    }
}
