using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeSixtySharp.Objects
{
    public class RealmPermission
    {
        //This needs work for changing permissions, fine for getting.
        public string Realm { get; set; }
        public string Permission { get; set; }
    }
}
