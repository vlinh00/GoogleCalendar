using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Share.Model
{
    public class StaffGroupByDepartment
    {
        public string DepartmentName { get; set; }
        public List<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
