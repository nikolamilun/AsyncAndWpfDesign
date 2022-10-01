using AsyncApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncApp.Core
{
    public class ProgressReportModel
    {
        public int PercentageComplete { get; set; } = 0;
        public List<Employee> EmpsAdded { get; set; } = new List<Employee>();
    }
}
