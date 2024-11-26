using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Share.Model
{
    public class StepModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Assign { get; set; }
        public string DateTimeLine { get; set; }
        public string Description{ get; set; }
        public bool? IsCompleted { get; set; }
    }
}
