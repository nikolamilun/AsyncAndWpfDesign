using AsyncApp.Core;
using AsyncApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject 
    {
        public EmployeesViewModel EmployeesVM;
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                OnPropertyChanged();
                _currentView = value;
            }
        }

        public MainViewModel()
        {
            EmployeesVM = new EmployeesViewModel();
            CurrentView = EmployeesVM;
        }
    }
}
