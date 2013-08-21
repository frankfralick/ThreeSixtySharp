using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeSixtySharp.Objects
{
    public class Document_Path
    {
        public int Count { get; set; }
        public int Dir_Size { get; set; }
        public string Path { get; set; }

        //This is not a response attribute of Document_Path but 
        //is useful/needed for implementing MVVM.
        public string Parent { get; set; }
    }
}
