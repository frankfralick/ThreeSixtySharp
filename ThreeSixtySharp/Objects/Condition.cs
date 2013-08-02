using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeSixtySharp.Objects
{
    public class Condition
    {
        public string Identifier { get; set; }
        public bool Sort_Field { get; set; }
        public string Operation { get; set; }
        public string Sort_Direction { get; set; }
        public string Values { get; set; }
    }
}
