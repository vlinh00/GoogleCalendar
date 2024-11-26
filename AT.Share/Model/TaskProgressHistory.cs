using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Share.Model
{
    public class TaskProgressHistory
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Progress { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedBy { get; set; }
        public string? Comments { get; set; }

    }

}
