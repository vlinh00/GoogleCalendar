using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Share.Model
{
    public class ProgressStep
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int Step { get; set; }
        public string? ProgressName { get; set; }
        public bool IsCompleted { get; set; }

    }
}
