using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeSixtySharp.Objects
{
    public class Filter
    {
        public string Created_By { get; set; }
        public Condition Conditions {get;set;}
        public DateTime Updated_At { get; set; }
        public DateTime Created_At { get; set; }
        public string Filter_Id { get; set; }
        public string Container { get; set; }
        public string[] Roles { get; set; }  
        public string Update_By { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }

    }
}
