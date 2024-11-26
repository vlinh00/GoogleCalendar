using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Share.Model
{
    public class ProgressUpdate
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Assign { get; set; }
        public string Progress { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
