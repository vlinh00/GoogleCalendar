using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Share.Model
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Progress { get; set; }
        public string? Link { get; set; }
        public string? EventId { get; set; }
    }
}
