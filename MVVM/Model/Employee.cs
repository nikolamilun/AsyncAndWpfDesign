using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AsyncApp.Model
{
    class Employee
    {
        public Employee(int employeeID, string lastName, string firstName, string title, string titleOfCourtesy, DateTime dateOfBirth, DateTime hireDate, string address, string city, string region, string postalCode, string country, string homePhone, string extension, Image myProperty, string text, int reportsTo, string photoPath)
        {
            EmployeeID = employeeID;
            LastName = lastName;
            FirstName = firstName;
            Title = title;
            TitleOfCourtesy = titleOfCourtesy;
            DateOfBirth = dateOfBirth;
            HireDate = hireDate;
            Address = address;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
            HomePhone = homePhone;
            Extension = extension;
            MyProperty = myProperty;
            Text = text;
            ReportsTo = reportsTo;
            PhotoPath = photoPath;
        }

        public int EmployeeID { get; private set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public Image MyProperty { get; set; }
        public string Text { get; set; }
        public int ReportsTo { get; set; }
        public string PhotoPath { get; set; }

        public override string ToString()
        {
            return $"Employee, ID:{EmployeeID}, Last Name: {LastName}, FirstName:{FirstName}";
        }
    }
}
