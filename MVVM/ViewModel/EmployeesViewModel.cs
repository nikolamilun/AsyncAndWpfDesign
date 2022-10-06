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
using System.Windows.Input;
using System.Windows;

namespace AsyncApp.ViewModel
{
    class EmployeesViewModel : ObservableObject
    {
        private List<Employee> _employees = new List<Employee>();
        public RelayCommand UpdateCommand { get; set; }
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
            UpdateCommand = new RelayCommand(o => UpdateDatabase());
        }

        private async Task LoadDataAsync()
        {
            try
            {
                Employees = await DataAccess.GetEmployeesAsync();
            }
            catch (Exception)
            {
                Employees = null;
            }
        }

        private async Task UpdateDatabase()
        {
            MessageBox.Show("Greska pri upisu u bazu!");
            try
            {
                Task.Run(() => DataAccess.ReturnEmployeesAsync(Employees));
            }
            catch (Exception)
            {
                MessageBox.Show("Greska pri upisu u bazu!");
            }
        }
    }
}
