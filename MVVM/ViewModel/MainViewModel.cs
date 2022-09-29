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
        public CustomersViewModel CustomersVM;
        public OrdersViewModel OrdersVM;
        public ProductsViewModel ProductsVM;
        public RegionsViewModel RegionsVM;
        public SearchViewModel SearchVM;

        public RelayCommand EmployeesViewCommand { get; set; }
        public RelayCommand CustomersViewCommand {get; set;}
        public RelayCommand OrdersViewCommand {get; set;}
        public RelayCommand ProductsViewCommand {get; set;}
        public RelayCommand RegionsViewCommand {get; set;}
        public RelayCommand SearchViewCommand {get; set;}

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
            CustomersVM = new CustomersViewModel();
            OrdersVM = new OrdersViewModel();
            ProductsVM = new ProductsViewModel();
            RegionsVM = new RegionsViewModel();
            SearchVM = new SearchViewModel();

            CurrentView = EmployeesVM;

            EmployeesViewCommand = new RelayCommand(o => { CurrentView = EmployeesVM; });
            CustomersViewCommand = new RelayCommand(o => { CurrentView = CustomersVM; });
            OrdersViewCommand = new RelayCommand(o => { CurrentView = OrdersVM; });
            ProductsViewCommand = new RelayCommand(o => { CurrentView = ProductsVM; });
            RegionsViewCommand = new RelayCommand(o => { CurrentView = RegionsVM; });
            SearchViewCommand = new RelayCommand(o => { CurrentView = SearchVM; });
        }
    }
}
